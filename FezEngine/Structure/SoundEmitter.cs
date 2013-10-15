// Type: FezEngine.Structure.SoundEmitter
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezEngine.Structure
{
  public class SoundEmitter
  {
    public bool New = true;
    private readonly float VolumeLevel;
    private bool factorizeVolume;
    private bool pausedForViewTransition;
    private Vector3? position;
    private float pitch;
    private IWaiter deathWaiter;
    private IWaiter fadePauseWaiter;

    public SoundEffectInstance Cue { get; private set; }

    public bool FactorizeVolume
    {
      get
      {
        return this.factorizeVolume;
      }
      set
      {
        this.factorizeVolume = value;
      }
    }

    public bool PauseViewTransitions { get; set; }

    public float VolumeMaster { get; set; }

    public float VolumeFactor { get; set; }

    public float FadeDistance { get; set; }

    public float NonFactorizedVolume { get; set; }

    public bool Persistent { get; set; }

    public bool NoAttenuation { get; set; }

    public Vector3 AxisMask { get; set; }

    public bool OverrideMap { get; set; }

    public bool WasPlaying { get; set; }

    public Vector3 Position
    {
      get
      {
        if (!this.position.HasValue)
          return Vector3.Zero;
        else
          return this.position.Value;
      }
      set
      {
        this.position = new Vector3?(value);
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
        this.Cue.Pitch = this.pitch = value;
      }
    }

    public float Pan { get; set; }

    public bool Dead
    {
      get
      {
        if (this.Cue != null && !this.Cue.IsDisposed)
          return this.Cue.State == SoundState.Stopped;
        else
          return true;
      }
    }

    [ServiceDependency]
    public ISoundManager SM { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    internal SoundEmitter(SoundEffect sound, bool looped, float pitch, float volumeFactor, bool paused, Vector3? position)
    {
      this.SM = ServiceHelper.Get<ISoundManager>();
      this.EngineState = ServiceHelper.Get<IEngineStateManager>();
      this.CameraManager = ServiceHelper.Get<IDefaultCameraManager>();
      ILevelManager levelManager = ServiceHelper.Get<ILevelManager>();
      this.VolumeLevel = this.SM.GetVolumeLevelFor(sound.Name);
      this.VolumeMaster = this.EngineState.DotLoading ? 0.0f : 1f;
      this.FadeDistance = 10f;
      this.AxisMask = Vector3.One;
      this.VolumeFactor = volumeFactor;
      this.position = position;
      if (SoundManager.NoMoreSounds)
        return;
      try
      {
        this.Cue = sound.CreateInstance(levelManager.LowPass);
        this.Pitch = pitch;
        this.Cue.IsLooped = looped;
        if (!paused)
        {
          this.Cue.Volume = volumeFactor * this.VolumeLevel;
          this.Cue.Play();
          this.Update();
        }
        else
        {
          this.Cue.Volume = 0.0f;
          this.Cue.Play();
          this.Cue.Pause();
        }
      }
      catch (InstancePlayLimitException ex)
      {
        Logger.Log("SoundEmitter", LogSeverity.Warning, "Couldn't create sound instance (too many instances)");
      }
    }

    public void Update()
    {
      if (this.Cue == null)
        return;
      if (this.PauseViewTransitions)
      {
        if (this.Cue.State == SoundState.Paused && this.CameraManager.ViewTransitionReached && this.pausedForViewTransition)
        {
          this.pausedForViewTransition = false;
          this.Cue.Resume();
        }
        else if (this.Cue.State == SoundState.Playing && !this.CameraManager.ViewTransitionReached)
        {
          this.pausedForViewTransition = true;
          this.Cue.Pause();
        }
      }
      if (this.Cue.State == SoundState.Paused || this.EngineState.InMap && !this.OverrideMap)
        return;
      if (this.position.HasValue)
      {
        Vector3 right = this.CameraManager.InverseView.Right;
        Vector3 interpolatedCenter = this.CameraManager.InterpolatedCenter;
        Vector2 vector2 = new Vector2()
        {
          X = FezMath.Dot(this.Position - interpolatedCenter, right),
          Y = interpolatedCenter.Y - this.Position.Y
        };
        float num1 = 1f;
        if (!this.NoAttenuation)
        {
          float num2 = vector2.Length();
          num1 = (double) num2 > 10.0 ? (float) (0.600000023841858 / (((double) num2 - 10.0) / 5.0 + 1.0)) : (float) (1.0 - (double) Easing.EaseIn((double) num2 / 10.0, EasingType.Quadratic) * 0.400000005960464);
        }
        this.NonFactorizedVolume = num1;
        this.Cue.Volume = FezMath.Saturate(this.NonFactorizedVolume * this.VolumeFactor * this.VolumeLevel * this.VolumeMaster);
        this.Cue.Pan = MathHelper.Clamp((float) ((double) vector2.X / (double) this.SM.LimitDistance.X * 1.5), -1f, 1f);
      }
      else
      {
        this.Cue.Volume = FezMath.Saturate(this.VolumeFactor * this.VolumeLevel * this.VolumeMaster);
        this.Cue.Pan = this.Pan;
      }
    }

    public void FadeOutAndDie(float forSeconds, bool autoPause)
    {
      if ((double) forSeconds == 0.0)
      {
        if (this.Cue == null || this.Cue.IsDisposed || this.Cue.State == SoundState.Stopped)
          return;
        this.Cue.Stop(false);
      }
      else
      {
        if (this.Dead || this.deathWaiter != null)
          return;
        float volumeFactor = this.VolumeFactor * this.VolumeLevel * this.VolumeMaster;
        this.deathWaiter = Waiters.Interpolate((double) forSeconds, (Action<float>) (s => this.VolumeFactor = volumeFactor * (1f - s)), (Action) (() =>
        {
          if (this.Cue != null && !this.Cue.IsDisposed && this.Cue.State != SoundState.Stopped)
            this.Cue.Stop(false);
          this.deathWaiter = (IWaiter) null;
        }));
        this.deathWaiter.AutoPause = autoPause;
      }
    }

    public void FadeOutAndDie(float forSeconds)
    {
      this.FadeOutAndDie(forSeconds, true);
    }

    public void FadeOutAndPause(float forSeconds)
    {
      if ((double) forSeconds == 0.0)
      {
        if (this.Cue == null || this.Cue.IsDisposed || this.Cue.State == SoundState.Paused)
          return;
        this.Cue.Pause();
      }
      else
      {
        if (this.Dead || this.fadePauseWaiter != null)
          return;
        float volumeFactor = this.VolumeFactor * this.VolumeLevel * this.VolumeMaster;
        this.fadePauseWaiter = Waiters.Interpolate((double) forSeconds, (Action<float>) (s => this.VolumeFactor = volumeFactor * (1f - s)), (Action) (() =>
        {
          if (this.Cue != null && !this.Cue.IsDisposed && this.Cue.State != SoundState.Paused)
            this.Cue.Pause();
          this.fadePauseWaiter = (IWaiter) null;
        }));
        this.fadePauseWaiter.AutoPause = true;
      }
    }

    public void Dispose()
    {
      if (this.Cue != null)
        this.Cue.Dispose();
      this.Cue = (SoundEffectInstance) null;
    }
  }
}
