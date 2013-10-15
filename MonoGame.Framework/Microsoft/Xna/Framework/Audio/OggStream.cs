// Type: Microsoft.Xna.Framework.Audio.OggStream
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using NVorbis;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Microsoft.Xna.Framework.Audio
{
  public class OggStream : IDisposable
  {
    internal readonly ReaderWriterLockSlim PreparationLock = new ReaderWriterLockSlim();
    internal readonly ReaderWriterLockSlim StoppingLock = new ReaderWriterLockSlim();
    private float globalVolume = 1f;
    private const int DefaultBufferCount = 6;
    internal int alSourceId;
    internal readonly int[] alBufferIds;
    internal readonly Stack<int> bufferStack;
    private Stream underlyingStream;
    private bool lowPass;
    private float volume;
    private string category;

    internal VorbisReader Reader { get; private set; }

    internal bool Precaching { get; private set; }

    internal bool FirstBufferPrecached { get; set; }

    internal int QueuedBuffers { get; set; }

    internal int ProcessedBuffers { get; set; }

    public int BufferCount { get; private set; }

    public string Name { get; private set; }

    public string RealName { get; set; }

    public bool LowPass
    {
      get
      {
        return this.lowPass;
      }
      set
      {
        if (this.lowPass != value)
          OpenALSoundController.Instance.SetSourceFiltered(this.alSourceId, value);
        this.lowPass = value;
      }
    }

    public float Volume
    {
      get
      {
        return this.volume;
      }
      set
      {
        AL.Source(this.alSourceId, ALSourcef.Gain, MathHelper.Clamp((this.volume = value) * this.globalVolume, 0.0f, 1f));
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

    public OggStream(string filename, int bufferCount = 6)
      : this((Stream) File.OpenRead(filename), bufferCount)
    {
      this.Name = filename;
    }

    public OggStream(Stream stream, int bufferCount = 6)
    {
      this.BufferCount = bufferCount;
      this.alBufferIds = OpenALSoundController.Instance.TakeBuffers(this.BufferCount);
      this.bufferStack = new Stack<int>((IEnumerable<int>) this.alBufferIds);
      this.alSourceId = OpenALSoundController.Instance.TakeSource();
      if (ALHelper.XRam.IsInitialized)
        ALHelper.XRam.SetBufferMode(this.BufferCount, ref this.alBufferIds[0], XRamExtension.XRamStorage.Hardware);
      this.Volume = 1f;
      this.lowPass = true;
      this.underlyingStream = stream;
    }

    public void Prepare(bool asynchronous = false)
    {
      ALSourceState sourceState = AL.GetSourceState(this.alSourceId);
      this.StoppingLock.EnterReadLock();
      switch (sourceState)
      {
        case ALSourceState.Playing:
        case ALSourceState.Paused:
          this.StoppingLock.ExitReadLock();
          break;
        default:
          this.PreparationLock.EnterWriteLock();
          if (this.Reader == null)
            this.Open();
          if (!this.Precaching)
          {
            this.Precaching = true;
            this.Precache(asynchronous);
          }
          this.PreparationLock.ExitWriteLock();
          this.StoppingLock.ExitReadLock();
          break;
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
          if (this.bufferStack.Count == this.BufferCount)
            this.Prepare(false);
          else if (!this.FirstBufferPrecached)
            ALHelper.Log(string.Concat(new object[4]
            {
              (object) "Buffers lost for ",
              (object) this.RealName,
              (object) " with source ",
              (object) this.alSourceId
            }), "OpenAL");
          AL.SourcePlay(this.alSourceId);
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
    }

    public void Resume()
    {
      if (AL.GetSourceState(this.alSourceId) != ALSourceState.Paused)
        return;
      OggStreamer.Instance.AddStream(this);
      AL.SourcePlay(this.alSourceId);
    }

    public void Stop()
    {
      switch (AL.GetSourceState(this.alSourceId))
      {
        case ALSourceState.Playing:
        case ALSourceState.Paused:
          this.StopPlayback();
          break;
      }
      this.StoppingLock.EnterWriteLock();
      if (OggStreamer.HasInstance)
        OggStreamer.Instance.RemoveStream(this);
      this.StoppingLock.ExitWriteLock();
    }

    public void Dispose()
    {
      if (this.IsDisposed)
        return;
      this.IsDisposed = true;
      this.PreparationLock.EnterWriteLock();
      if (OggStreamer.HasInstance)
        OggStreamer.Instance.RemoveStream(this);
      this.StopPlayback();
      this.Empty();
      this.Close();
      if (OpenALSoundController.Instance != null)
      {
        OpenALSoundController.Instance.ReturnSource(this.alSourceId);
        OpenALSoundController.Instance.ReturnBuffers(this.alBufferIds);
      }
      this.PreparationLock.ExitWriteLock();
    }

    private void StopPlayback()
    {
      AL.SourceStop(this.alSourceId);
    }

    private void Empty()
    {
      int numEntries;
      AL.GetSource(this.alSourceId, ALGetSourcei.BuffersQueued, out numEntries);
      if (numEntries <= 0)
        return;
      AL.SourceUnqueueBuffers(this.alSourceId, numEntries);
      if (ALHelper.TryCheck())
        return;
      AL.DeleteSource(this.alSourceId);
      this.alSourceId = AL.GenSource();
    }

    private void Open()
    {
      this.underlyingStream.Seek(0L, SeekOrigin.Begin);
      OggStreamer.decodeLock.EnterWriteLock();
      this.Reader = new VorbisReader(this.underlyingStream, true);
      OggStreamer.decodeLock.ExitWriteLock();
    }

    private void Precache(bool asynchronous = false)
    {
      if (!asynchronous)
      {
        int num = this.bufferStack.Pop();
        OggStreamer.Instance.FillBuffer(this, num);
        AL.SourceQueueBuffer(this.alSourceId, num);
        this.FirstBufferPrecached = true;
      }
      else
        Interlocked.Increment(ref OggStreamer.Instance.PendingPrecaches);
      OggStreamer.Instance.AddStream(this);
    }

    private void Close()
    {
      if (this.Reader == null)
        return;
      this.Reader.Dispose();
      this.Reader = (VorbisReader) null;
      this.underlyingStream.Close();
      this.underlyingStream.Dispose();
      this.underlyingStream = (Stream) null;
    }
  }
}
