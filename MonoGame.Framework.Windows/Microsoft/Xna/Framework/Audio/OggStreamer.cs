// Type: Microsoft.Xna.Framework.Audio.OggStreamer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Microsoft.Xna.Framework.Audio
{
  public class OggStreamer : IDisposable
  {
    private static readonly object singletonMutex = new object();
    private readonly object iterationMutex = new object();
    private readonly object readMutex = new object();
    private readonly HashSet<OggStream> streams = new HashSet<OggStream>();
    private readonly List<OggStream> threadLocalStreams = new List<OggStream>();
    private float musicVolume = 1f;
    private float ambienceVolume = 1f;
    private const float DefaultUpdateRate = 10f;
    private const int DefaultBufferSize = 44100;
    private readonly float[] readSampleBuffer;
    private readonly short[] castBuffer;
    private readonly Thread underlyingThread;
    private volatile bool cancelled;
    private static OggStreamer instance;

    public float UpdateRate { get; private set; }

    public int BufferSize { get; private set; }

    public static OggStreamer Instance
    {
      get
      {
        lock (OggStreamer.singletonMutex)
        {
          if (OggStreamer.instance == null)
            throw new InvalidOperationException("No instance running");
          else
            return OggStreamer.instance;
        }
      }
      private set
      {
        lock (OggStreamer.singletonMutex)
          OggStreamer.instance = value;
      }
    }

    public static bool HasInstance
    {
      get
      {
        lock (OggStreamer.singletonMutex)
          return OggStreamer.instance != null;
      }
    }

    public float LowPassHFGain
    {
      set
      {
        OpenALSoundController.Instance.LowPassHFGain = value;
      }
    }

    public float MusicVolume
    {
      get
      {
        return this.musicVolume;
      }
      set
      {
        this.musicVolume = value;
        foreach (OggStream oggStream in this.streams)
        {
          if (oggStream.Category == "Music")
            oggStream.GlobalVolume = value;
        }
      }
    }

    public float AmbienceVolume
    {
      get
      {
        return this.ambienceVolume;
      }
      set
      {
        this.ambienceVolume = value;
        foreach (OggStream oggStream in this.streams)
        {
          if (oggStream.Category == "Ambience")
            oggStream.GlobalVolume = value;
        }
      }
    }

    static OggStreamer()
    {
    }

    public OggStreamer(int bufferSize = 44100, float updateRate = 10f)
    {
      lock (OggStreamer.singletonMutex)
      {
        if (OggStreamer.instance != null)
          throw new InvalidOperationException("Already running");
        OggStreamer.Instance = this;
        this.underlyingThread = new Thread(new ThreadStart(this.EnsureBuffersFilled))
        {
          Priority = ThreadPriority.Lowest,
          Name = "Ogg Streamer"
        };
        this.underlyingThread.Start();
      }
      this.UpdateRate = updateRate;
      this.BufferSize = bufferSize;
      this.readSampleBuffer = new float[bufferSize];
      this.castBuffer = new short[bufferSize];
    }

    public void Dispose()
    {
      lock (OggStreamer.singletonMutex)
      {
        this.cancelled = true;
        lock (this.iterationMutex)
          this.streams.Clear();
        OggStreamer.Instance = (OggStreamer) null;
      }
    }

    internal bool AddStream(OggStream stream)
    {
      lock (this.iterationMutex)
        return this.streams.Add(stream);
    }

    internal bool RemoveStream(OggStream stream)
    {
      lock (this.iterationMutex)
        return this.streams.Remove(stream);
    }

    public bool FillBuffer(OggStream stream, int bufferId)
    {
      int length;
      lock (this.readMutex)
      {
        length = stream.Reader.ReadSamples(this.readSampleBuffer, 0, this.BufferSize);
        OggStreamer.CastBuffer(this.readSampleBuffer, this.castBuffer, length);
      }
      AL.BufferData<short>(bufferId, stream.Reader.Channels == 1 ? ALFormat.Mono16 : ALFormat.Stereo16, this.castBuffer, length * 2, stream.Reader.SampleRate);
      ALHelper.Check();
      return length != this.BufferSize;
    }

    private static void CastBuffer(float[] inBuffer, short[] outBuffer, int length)
    {
      for (int index = 0; index < length; ++index)
      {
        int num = (int) ((double) short.MaxValue * (double) inBuffer[index]);
        if (num > (int) short.MaxValue)
          num = (int) short.MaxValue;
        else if (num < (int) short.MinValue)
          num = (int) short.MinValue;
        outBuffer[index] = (short) num;
      }
    }

    private void EnsureBuffersFilled()
    {
      while (!this.cancelled)
      {
        Thread.Sleep((int) (1000.0 / (double) this.UpdateRate));
        if (this.cancelled)
          break;
        this.threadLocalStreams.Clear();
        lock (this.iterationMutex)
          this.threadLocalStreams.AddRange((IEnumerable<OggStream>) this.streams);
        if (this.threadLocalStreams.Count != 0)
        {
          foreach (OggStream stream in this.threadLocalStreams)
          {
            lock (stream.prepareMutex)
            {
              lock (this.iterationMutex)
              {
                if (!this.streams.Contains(stream))
                  continue;
              }
              bool local_1 = false;
              int local_2;
              AL.GetSource(stream.alSourceId, ALGetSourcei.BuffersQueued, out local_2);
              ALHelper.Check();
              int local_3;
              AL.GetSource(stream.alSourceId, ALGetSourcei.BuffersProcessed, out local_3);
              ALHelper.Check();
              if (local_3 != 0 || local_2 != stream.BufferCount)
              {
                int[] local_4 = local_3 <= 0 ? Enumerable.ToArray<int>(Enumerable.Skip<int>((IEnumerable<int>) stream.alBufferIds, local_2)) : AL.SourceUnqueueBuffers(stream.alSourceId, local_3);
                for (int local_5 = 0; local_5 < local_4.Length; ++local_5)
                {
                  local_1 = local_1 | this.FillBuffer(stream, local_4[local_5]);
                  if (local_1)
                  {
                    if (stream.IsLooped)
                    {
                      stream.Reader.DecodedTime = TimeSpan.Zero;
                    }
                    else
                    {
                      this.streams.Remove(stream);
                      local_5 = local_4.Length;
                    }
                  }
                }
                AL.SourceQueueBuffers(stream.alSourceId, local_4.Length, local_4);
                ALHelper.Check();
                if (local_1 && !stream.IsLooped)
                  continue;
              }
              else
                continue;
            }
            lock (stream.stopMutex)
            {
              if (!stream.Precaching)
              {
                lock (this.iterationMutex)
                {
                  if (!this.streams.Contains(stream))
                    continue;
                }
                if (AL.GetSourceState(stream.alSourceId) == ALSourceState.Stopped)
                {
                  Trace.WriteLine("[OpenAL] Buffer underrun on " + stream.Name);
                  AL.SourcePlay(stream.alSourceId);
                  ALHelper.Check();
                }
              }
            }
          }
        }
      }
    }
  }
}
