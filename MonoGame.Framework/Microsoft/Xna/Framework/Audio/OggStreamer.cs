// Type: Microsoft.Xna.Framework.Audio.OggStreamer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Xna.Framework.Audio
{
  public class OggStreamer : IDisposable
  {
    private static readonly ReaderWriterLockSlim singletonLock = new ReaderWriterLockSlim();
    private static readonly ReaderWriterLockSlim iterationLock = new ReaderWriterLockSlim();
    public static readonly ReaderWriterLockSlim decodeLock = new ReaderWriterLockSlim();
    private readonly HashSet<OggStream> streams = new HashSet<OggStream>();
    private readonly List<OggStream> threadLocalStreams = new List<OggStream>();
    private float musicVolume = 1f;
    private float ambienceVolume = 1f;
    private const float DefaultUpdateRate = 60f;
    private const int DefaultBufferSize = 22050;
    private readonly float[] readSampleBuffer;
    private readonly short[] castBuffer;
    private readonly Thread underlyingThread;
    private volatile bool cancelled;
    public int PendingPrecaches;
    private static OggStreamer instance;

    public float UpdateRate { get; private set; }

    public int BufferSize { get; private set; }

    public static OggStreamer Instance
    {
      get
      {
        OggStreamer.singletonLock.EnterReadLock();
        if (OggStreamer.instance == null)
          throw new InvalidOperationException("No instance running");
        OggStreamer oggStreamer = OggStreamer.instance;
        OggStreamer.singletonLock.ExitReadLock();
        return oggStreamer;
      }
      private set
      {
        OggStreamer.instance = value;
      }
    }

    public static bool HasInstance
    {
      get
      {
        OggStreamer.singletonLock.EnterReadLock();
        bool flag = OggStreamer.instance != null;
        OggStreamer.singletonLock.ExitReadLock();
        return flag;
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
        OggStreamer.iterationLock.EnterReadLock();
        foreach (OggStream oggStream in this.streams)
        {
          if (oggStream.Category == "Music")
            oggStream.GlobalVolume = value;
        }
        OggStreamer.iterationLock.ExitReadLock();
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
        OggStreamer.iterationLock.EnterReadLock();
        foreach (OggStream oggStream in this.streams)
        {
          if (oggStream.Category == "Ambience")
            oggStream.GlobalVolume = value;
        }
        OggStreamer.iterationLock.ExitReadLock();
      }
    }

    static OggStreamer()
    {
    }

    public OggStreamer(int bufferSize = 22050, float updateRate = 60f)
    {
      try
      {
        OggStreamer.singletonLock.EnterUpgradeableReadLock();
        if (OggStreamer.instance != null)
          throw new InvalidOperationException("Already running");
        OggStreamer.singletonLock.EnterWriteLock();
        OggStreamer.Instance = this;
        OggStreamer.singletonLock.ExitWriteLock();
      }
      finally
      {
        OggStreamer.singletonLock.ExitUpgradeableReadLock();
      }
      this.underlyingThread = new Thread(new ThreadStart(this.EnsureBuffersFilled))
      {
        Priority = ThreadPriority.Normal,
        Name = "Ogg Streamer"
      };
      this.underlyingThread.Start();
      this.UpdateRate = updateRate;
      this.BufferSize = bufferSize;
      this.readSampleBuffer = new float[bufferSize];
      this.castBuffer = new short[bufferSize];
    }

    public void Dispose()
    {
      OggStreamer.singletonLock.EnterWriteLock();
      this.cancelled = true;
      OggStreamer.iterationLock.EnterWriteLock();
      this.streams.Clear();
      OggStreamer.iterationLock.ExitWriteLock();
      OggStreamer.Instance = (OggStreamer) null;
      OggStreamer.singletonLock.ExitWriteLock();
    }

    internal bool AddStream(OggStream stream)
    {
      OggStreamer.iterationLock.EnterWriteLock();
      bool flag = this.streams.Add(stream);
      OggStreamer.iterationLock.ExitWriteLock();
      return flag;
    }

    internal bool RemoveStream(OggStream stream)
    {
      OggStreamer.iterationLock.EnterWriteLock();
      bool flag = this.streams.Remove(stream);
      OggStreamer.iterationLock.ExitWriteLock();
      if (flag && !stream.FirstBufferPrecached)
        Interlocked.Decrement(ref this.PendingPrecaches);
      return flag;
    }

    public bool FillBuffer(OggStream stream, int bufferId)
    {
      OggStreamer.decodeLock.EnterWriteLock();
      if (stream.IsDisposed)
      {
        OggStreamer.decodeLock.ExitWriteLock();
        return true;
      }
      else
      {
        int num = stream.Reader.ReadSamples(this.readSampleBuffer, 0, this.BufferSize);
        for (int index = 0; index < num; ++index)
          this.castBuffer[index] = (short) ((double) short.MaxValue * (double) this.readSampleBuffer[index]);
        AL.BufferData<short>(bufferId, stream.Reader.Channels == 1 ? ALFormat.Mono16 : ALFormat.Stereo16, this.castBuffer, num * 2, stream.Reader.SampleRate);
        OggStreamer.decodeLock.ExitWriteLock();
        return num != this.BufferSize;
      }
    }

    private void EnsureBuffersFilled()
    {
      while (!this.cancelled && !this.cancelled)
      {
        this.threadLocalStreams.Clear();
        OggStreamer.iterationLock.EnterReadLock();
        this.threadLocalStreams.AddRange((IEnumerable<OggStream>) this.streams);
        OggStreamer.iterationLock.ExitReadLock();
        if (this.threadLocalStreams.Count != 0)
        {
          int val1 = int.MaxValue;
          for (int index = this.threadLocalStreams.Count - 1; index >= 0; --index)
          {
            OggStream oggStream = this.threadLocalStreams[index];
            OggStreamer.iterationLock.EnterReadLock();
            bool flag = !this.streams.Contains(oggStream);
            OggStreamer.iterationLock.ExitReadLock();
            if (flag)
            {
              this.threadLocalStreams.RemoveAt(index);
            }
            else
            {
              int num1;
              AL.GetSource(oggStream.alSourceId, ALGetSourcei.BuffersQueued, out num1);
              oggStream.QueuedBuffers = num1;
              int num2;
              AL.GetSource(oggStream.alSourceId, ALGetSourcei.BuffersProcessed, out num2);
              oggStream.ProcessedBuffers = num2;
              if (!oggStream.Precaching)
                val1 = Math.Min(val1, num1 - num2);
            }
          }
          foreach (OggStream stream in this.threadLocalStreams)
          {
            stream.PreparationLock.EnterReadLock();
            OggStreamer.iterationLock.EnterReadLock();
            bool flag1 = !this.streams.Contains(stream);
            OggStreamer.iterationLock.ExitReadLock();
            if (flag1)
              stream.PreparationLock.ExitReadLock();
            else if (stream.ProcessedBuffers == 0 && stream.bufferStack.Count == 0)
              stream.PreparationLock.ExitReadLock();
            else if (stream.QueuedBuffers - stream.ProcessedBuffers > val1)
              stream.PreparationLock.ExitReadLock();
            else if (stream.Precaching && val1 <= stream.BufferCount * 2 / 3)
            {
              stream.PreparationLock.ExitReadLock();
            }
            else
            {
              int num = stream.ProcessedBuffers <= 0 ? stream.bufferStack.Pop() : AL.SourceUnqueueBuffer(stream.alSourceId);
              bool flag2 = this.FillBuffer(stream, num);
              if (flag2)
              {
                if (stream.IsLooped)
                {
                  stream.Reader.DecodedTime = TimeSpan.Zero;
                }
                else
                {
                  OggStreamer.iterationLock.EnterWriteLock();
                  this.streams.Remove(stream);
                  OggStreamer.iterationLock.ExitWriteLock();
                }
              }
              AL.SourceQueueBuffer(stream.alSourceId, num);
              if (!stream.FirstBufferPrecached)
              {
                Interlocked.Decrement(ref this.PendingPrecaches);
                stream.FirstBufferPrecached = true;
              }
              if (flag2 && !stream.IsLooped)
              {
                stream.PreparationLock.ExitReadLock();
              }
              else
              {
                stream.PreparationLock.ExitReadLock();
                stream.StoppingLock.EnterReadLock();
                if (stream.Precaching)
                {
                  stream.StoppingLock.ExitReadLock();
                }
                else
                {
                  OggStreamer.iterationLock.EnterReadLock();
                  bool flag3 = !this.streams.Contains(stream);
                  OggStreamer.iterationLock.ExitReadLock();
                  if (flag3)
                  {
                    stream.StoppingLock.ExitReadLock();
                  }
                  else
                  {
                    if (AL.GetSourceState(stream.alSourceId) == ALSourceState.Stopped)
                    {
                      ALHelper.Log(string.Concat(new object[4]
                      {
                        (object) "Buffer underrun on ",
                        (object) stream.RealName,
                        (object) " with source ",
                        (object) stream.alSourceId
                      }), "OpenAL");
                      AL.SourcePlay(stream.alSourceId);
                    }
                    stream.StoppingLock.ExitReadLock();
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
