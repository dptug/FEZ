// Type: Microsoft.Xna.Framework.Audio.SoundEffectInstance
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Audio.OpenAL;
using System;

namespace Microsoft.Xna.Framework.Audio
{
  public class SoundEffectInstance : IDisposable
  {
    private float volume = 1f;
    private SoundState soundState = SoundState.Stopped;
    private readonly SoundEffect soundEffect;
    private readonly int sourceId;
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
        if (this.isDisposed)
          throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
        this.looped = value;
        AL.Source(this.sourceId, ALSourceb.Looping, this.looped);
        ALHelper.Check();
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
        if (this.isDisposed)
          throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
        this.pan = value;
        AL.Source(this.sourceId, ALSource3f.Position, this.pan / 1.25f, 0.0f, 0.1f);
        ALHelper.Check();
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
        if (this.isDisposed)
          throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
        this.pitch = value;
        AL.Source(this.sourceId, ALSourcef.Pitch, this.XnaPitchToAlPitch(this.pitch));
        ALHelper.Check();
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
        if (this.isDisposed)
          throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
        this.volume = value;
        AL.Source(this.sourceId, ALSourcef.Gain, this.volume * SoundEffect.MasterVolume);
        ALHelper.Check();
      }
    }

    protected SoundEffectInstance()
    {
    }

    internal SoundEffectInstance(SoundEffect soundEffect, bool forceNoFilter = false)
      : this()
    {
      this.soundEffect = soundEffect;
      if (OpenALSoundController.Instance == null)
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
      if (OpenALSoundController.Instance == null)
        return;
      OpenALSoundController.Instance.ReturnSourceFor(this.soundEffect, this.sourceId);
    }

    public void Pause()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
      if (this.soundState != SoundState.Playing)
        return;
      AL.SourcePause(this.sourceId);
      ALHelper.Check();
      this.soundState = SoundState.Paused;
    }

    public void Play()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
      if (this.soundState == SoundState.Playing)
        return;
      AL.SourcePlay(this.sourceId);
      ALHelper.Check();
      this.soundState = SoundState.Playing;
    }

    public void Resume()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
      if (this.soundState != SoundState.Paused)
        return;
      this.Play();
    }

    public void Stop(bool immediate = false)
    {
      if (this.isDisposed)
        throw new ObjectDisposedException("SoundEffectInstance (" + this.soundEffect.Name + ")");
      if (this.soundState == SoundState.Stopped)
        return;
      AL.SourceStop(this.sourceId);
      ALHelper.Check();
      this.soundState = SoundState.Stopped;
    }

    internal bool RefreshState()
    {
      if (this.soundState != SoundState.Playing || AL.GetSourceState(this.sourceId) != ALSourceState.Stopped)
        return false;
      ALHelper.Check();
      this.soundState = SoundState.Stopped;
      return true;
    }

    private float XnaPitchToAlPitch(float pitch)
    {
      return (float) Math.Exp(0.69314718 * (double) pitch);
    }
  }
}
