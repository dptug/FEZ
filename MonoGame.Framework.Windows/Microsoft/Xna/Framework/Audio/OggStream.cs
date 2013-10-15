// Type: Microsoft.Xna.Framework.Audio.OggStream
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using NVorbis;
using OpenTK.Audio.OpenAL;
using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  public class OggStream : IDisposable
  {
    internal readonly object stopMutex = new object();
    internal readonly object prepareMutex = new object();
    private float globalVolume = 1f;
    private const int DefaultBufferCount = 3;
    internal readonly int alSourceId;
    internal readonly int[] alBufferIds;
    private readonly Stream underlyingStream;
    private float volume;
    private string category;

    internal VorbisReader Reader { get; private set; }

    internal bool Precaching { get; private set; }

    public int BufferCount { get; private set; }

    public string Name { get; private set; }

    public float Volume
    {
      get
      {
        return this.volume;
      }
      set
      {
        AL.Source(this.alSourceId, ALSourcef.Gain, MathHelper.Clamp((this.volume = value) * this.globalVolume, 0.0f, 1f));
        ALHelper.Check();
      }
    }

    public float GlobalVolume
    {
      set
      {
        this.globalVolume = value;
        this.Volume = this.volume;
      }
    }

    public string Category
    {
      get
      {
        return this.category;
      }
      set
      {
        this.category = value;
        if (!OggStreamer.HasInstance)
          return;
        this.GlobalVolume = this.category == "Ambience" ? OggStreamer.Instance.AmbienceVolume : OggStreamer.Instance.MusicVolume;
      }
    }

    public bool IsLooped { get; set; }

    public bool IsStopped
    {
      get
      {
        return AL.GetSourceState(this.alSourceId) == ALSourceState.Stopped;
      }
    }

    public bool IsPlaying
    {
      get
      {
        return AL.GetSourceState(this.alSourceId) == ALSourceState.Playing;
      }
    }

    public bool IsDisposed { get; private set; }

    public OggStream(string filename, int bufferCount = 3)
      : this((Stream) File.OpenRead(filename), bufferCount)
    {
      this.Name = filename;
    }

    public OggStream(Stream stream, int bufferCount = 3)
    {
      ALHelper.Check();
      this.BufferCount = bufferCount;
      this.alBufferIds = OpenALSoundController.Instance.TakeBuffers(this.BufferCount);
      this.alSourceId = OpenALSoundController.Instance.TakeSource();
      if (ALHelper.XRam.IsInitialized)
      {
        ALHelper.XRam.SetBufferMode(this.BufferCount, ref this.alBufferIds[0], XRamExtension.XRamStorage.Hardware);
        ALHelper.Check();
      }
      this.Volume = 1f;
      this.underlyingStream = stream;
    }

    public void Prepare(bool asynchronous = false)
    {
      ALSourceState sourceState = AL.GetSourceState(this.alSourceId);
      lock (this.stopMutex)
      {
        switch (sourceState)
        {
          case ALSourceState.Playing:
            break;
          case ALSourceState.Paused:
            break;
          default:
            lock (this.prepareMutex)
            {
              if (this.Reader == null)
                this.Open();
              if (!this.Precaching)
              {
                this.Precaching = true;
                this.Precache(asynchronous);
                break;
              }
              else
                break;
            }
        }
      }
    }

    public void Play()
    {
      switch (AL.GetSourceState(this.alSourceId))
      {
        case ALSourceState.Playing:
          break;
        case ALSourceState.Paused:
          this.Resume();
          break;
        default:
          this.Prepare(false);
          AL.SourcePlay(this.alSourceId);
          ALHelper.Check();
          this.Precaching = false;
          OggStreamer.Instance.AddStream(this);
          break;
      }
    }

    public void Pause()
    {
      if (AL.GetSourceState(this.alSourceId) != ALSourceState.Playing)
        return;
      OggStreamer.Instance.RemoveStream(this);
      AL.SourcePause(this.alSourceId);
      ALHelper.Check();
    }

    public void Resume()
    {
      if (AL.GetSourceState(this.alSourceId) != ALSourceState.Paused)
        return;
      OggStreamer.Instance.AddStream(this);
      AL.SourcePlay(this.alSourceId);
      ALHelper.Check();
    }

    public void Stop()
    {
      ALSourceState sourceState = AL.GetSourceState(this.alSourceId);
      if (sourceState == ALSourceState.Playing || sourceState == ALSourceState.Paused)
        this.StopPlayback();
      lock (this.stopMutex)
      {
        if (!OggStreamer.HasInstance)
          return;
        OggStreamer.Instance.RemoveStream(this);
      }
    }

    public void Dispose()
    {
      if (this.IsDisposed)
        return;
      this.IsDisposed = true;
      ALSourceState sourceState = AL.GetSourceState(this.alSourceId);
      if (sourceState == ALSourceState.Playing || sourceState == ALSourceState.Paused)
        this.StopPlayback();
      lock (this.prepareMutex)
      {
        if (OggStreamer.HasInstance)
          OggStreamer.Instance.RemoveStream(this);
        if (sourceState != ALSourceState.Initial)
          this.Empty(false);
        this.Close();
        this.underlyingStream.Dispose();
      }
      if (OpenALSoundController.Instance != null)
      {
        OpenALSoundController.Instance.ReturnSource(this.alSourceId);
        OpenALSoundController.Instance.ReturnBuffers(this.alBufferIds);
      }
      ALHelper.Check();
    }

    private void StopPlayback()
    {
      AL.SourceStop(this.alSourceId);
      ALHelper.Check();
    }

    private void Empty(bool giveUp = false)
    {
      int numEntries1;
      AL.GetSource(this.alSourceId, ALGetSourcei.BuffersQueued, out numEntries1);
      if (numEntries1 <= 0)
        return;
      AL.SourceUnqueueBuffers(this.alSourceId, numEntries1);
      if (!ALHelper.TryCheck() && !giveUp)
      {
        int numEntries2;
        AL.GetSource(this.alSourceId, ALGetSourcei.BuffersProcessed, out numEntries2);
        int[] bids = new int[numEntries2];
        if (numEntries2 > 0)
        {
          AL.SourceUnqueueBuffers(this.alSourceId, numEntries2, bids);
          ALHelper.Check();
        }
        AL.SourceStop(this.alSourceId);
        ALHelper.Check();
        this.Empty(true);
      }
    }

    internal void Open()
    {
      this.underlyingStream.Seek(0L, SeekOrigin.Begin);
      this.Reader = new VorbisReader(this.underlyingStream, false);
    }

    internal void Precache(bool asynchronous = false)
    {
      if (!asynchronous)
      {
        OggStreamer.Instance.FillBuffer(this, this.alBufferIds[0]);
        AL.SourceQueueBuffer(this.alSourceId, this.alBufferIds[0]);
        ALHelper.Check();
      }
      OggStreamer.Instance.AddStream(this);
    }

    internal void Close()
    {
      if (this.Reader == null)
        return;
      this.Reader.Dispose();
      this.Reader = (VorbisReader) null;
    }
  }
}
