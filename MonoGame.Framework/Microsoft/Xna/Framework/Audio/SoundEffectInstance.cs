// Type: Microsoft.Xna.Framework.Audio.SoundEffectInstance
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK.Audio.OpenAL;
using System;

namespace Microsoft.Xna.Framework.Audio
{
  public class SoundEffectInstance : IDisposable
  {
    private readonly int sourceId = -1;
    private float volume = 1f;
    private SoundState soundState = SoundState.Stopped;
    private readonly SoundEffect soundEffect;
    private bool looped;
    private float pan;
    private float pitch;
    private bool isDisposed;
    private bool lowPass;

    public SoundEffect SoundEffect
    {
      get
      {
        return this.soundEffect;
      }
    }

    public bool IsDisposed
    {
      get
      {
        return this.isDisposed;
      }
    }

    public bool IsLooped
    {
      get
      {
        return this.looped;
      }
      set
      {
        if (this.isDisposed || this.sourceId == -1)
          return;
        this.looped = value;
        AL.Source(this.sourceId, ALSourceb.Looping, this.looped);
      }
    }

    public float Pan
    {
      get
      {
        return this.pan;
      }
      set
      {
        if (this.isDisposed || this.sourceId == -1)
          return;
        this.pan = value;
        AL.Source(this.sourceId, ALSource3f.Position, this.pan / 5f, 0.0f, 0.1f);
      }
    }

    public float Pitch
    {
      get
      {
        return this.pitch;
      }
      set
      {
        if (this.isDisposed || this.sourceId == -1)
          return;
        this.pitch = value;
        AL.Source(this.sourceId, ALSourcef.Pitch, this.XnaPitchToAlPitch(this.pitch));
      }
    }

    public bool LowPass
    {
      get
      {
        return this.lowPass;
      }
      set
      {
        if (this.sourceId == -1)
          return;
        if (this.lowPass != value)
          OpenALSoundController.Instance.SetSourceFiltered(this.sourceId, value);
        this.lowPass = value;
      }
    }

    public SoundState State
    {
      get
      {
        return this.soundState;
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
        if (this.isDisposed || this.sourceId == -1)
          return;
        this.volume = value;
        AL.Source(this.sourceId, ALSourcef.Gain, this.volume * SoundEffect.MasterVolume);
      }
    }

    protected SoundEffectInstance()
    {
    }

    internal SoundEffectInstance(SoundEffect soundEffect, bool forceNoFilter = false)
      : this()
    {
      this.soundEffect = soundEffect;
      if (OpenALSoundController.Instance == null || soundEffect.Size <= 0)
        return;
      this.sourceId = OpenALSoundController.Instance.RegisterSfxInstance(this, forceNoFilter);
    }

    ~SoundEffectInstance()
    {
      this.Dispose();
    }

    public void Dispose()
    {
      if (this.isDisposed)
        return;
      if (this.soundState != SoundState.Stopped)
        this.Stop(false);
      this.isDisposed = true;
      if (OpenALSoundController.Instance == null || this.sourceId == -1)
        return;
      OpenALSoundController.Instance.ReturnSourceFor(this.soundEffect, this.sourceId);
    }

    public void Pause()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
      if (this.sourceId == -1 || this.soundState != SoundState.Playing)
        return;
      AL.SourcePause(this.sourceId);
      this.soundState = SoundState.Paused;
    }

    public void Play()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
      if (this.sourceId == -1 || this.soundState == SoundState.Playing)
        return;
      AL.SourcePlay(this.sourceId);
      this.soundState = SoundState.Playing;
    }

    public void Resume()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
      if (this.sourceId != -1 && this.soundState != SoundState.Paused)
        return;
      this.Play();
    }

    public void Stop(bool immediate = false)
    {
      if (this.isDisposed || this.sourceId == -1 || this.soundState == SoundState.Stopped)
        return;
      AL.SourceStop(this.sourceId);
      this.soundState = SoundState.Stopped;
    }

    internal bool RefreshState()
    {
      if (this.soundState != SoundState.Playing || AL.GetSourceState(this.sourceId) != ALSourceState.Stopped)
        return false;
      this.soundState = SoundState.Stopped;
      return true;
    }

    private float XnaPitchToAlPitch(float pitch)
    {
      return (float) Math.Exp(0.69314718 * (double) pitch);
    }
  }
}
