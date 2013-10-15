// Type: FezGame.Components.IntroPanDown
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components
{
  internal class IntroPanDown : GameComponent
  {
    public float SinceStarted;
    private Vector3 Origin;
    private Vector3 Destination;
    private float Distance;
    public bool DoPanDown;
    public bool IsDisposed;
    private bool FirstConstraint;
    private SoundEmitter ePanDown;
    private Vector3 lastPosition;

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IDotManager Dot { get; set; }

    public IntroPanDown(Game game)
      : base(game)
    {
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.ePanDown == null || this.ePanDown.Dead)
        return;
      this.ePanDown.FadeOutAndDie(0.1f, false);
    }

    public override void Initialize()
    {
      base.Initialize();
      this.GameState.InCutscene = false;
      this.GameState.ForceTimePaused = true;
      this.PlayerManager.CanControl = false;
      this.Destination = this.CameraManager.Center;
      this.RecalculateOrigin();
      this.CameraManager.Center = this.Origin;
      if (this.CameraManager.Constrained)
        this.FirstConstraint = true;
      this.SoundManager.PlayNewAmbience();
      this.ePanDown = SoundEffectExtensions.Emit(this.CMProvider.Get(CM.Intro).Load<SoundEffect>("Sounds/Intro/PanDown"), true, true);
    }

    private void RecalculateOrigin()
    {
      bool flag = this.LevelManager.Sky != null && (this.LevelManager.Sky.Name == "GRAVE" || this.LevelManager.Sky.Name == "MINE");
      float num = (float) ((double) this.CameraManager.Radius / (double) this.CameraManager.AspectRatio * 0.5);
      this.Origin = this.Destination * FezMath.XZMask;
      this.Origin.Y = flag ? -num : this.LevelManager.Size.Y + num;
      this.Distance = Math.Abs(this.Origin.Y - this.Destination.Y);
      if ((double) this.Distance > 0.0)
        return;
      this.Distance = 1f;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      if (this.LevelManager.WaterType != LiquidType.None)
      {
        LiquidHost.Instance.ForcedUpdate = true;
        LiquidHost.Instance.Update(gameTime);
        LiquidHost.Instance.ForcedUpdate = false;
      }
      if (this.CameraManager.Constrained && !this.FirstConstraint)
      {
        this.FirstConstraint = true;
        this.Destination = this.CameraManager.Center;
        this.RecalculateOrigin();
      }
      if (!this.DoPanDown)
        this.CameraManager.Center = this.Origin;
      else if (this.ePanDown == null || this.ePanDown.Dead)
      {
        ServiceHelper.RemoveComponent<IntroPanDown>(this);
      }
      else
      {
        if (this.ePanDown.Cue.State != SoundState.Playing)
          this.ePanDown.Cue.Resume();
        if (!this.GameState.Paused)
          this.SinceStarted += (float) (gameTime.ElapsedGameTime.TotalSeconds * 8.0);
        this.CameraManager.Center = Vector3.Lerp(this.Origin, this.Destination, Easing.EaseInOut((double) this.SinceStarted / (double) this.Distance, EasingType.Quadratic, EasingType.Sine));
        if (this.lastPosition != Vector3.Zero)
        {
          this.ePanDown.VolumeFactor = FezMath.Saturate((float) (((double) this.lastPosition.Y - (double) this.CameraManager.Center.Y) * 2.0));
          this.ePanDown.Pitch = (float) ((double) FezMath.Saturate((float) (((double) this.lastPosition.Y - (double) this.CameraManager.Center.Y) * 2.0)) * 2.0 - 1.0);
        }
        this.lastPosition = this.CameraManager.Center;
        if ((double) this.SinceStarted < (double) this.Distance)
          return;
        this.GameState.ForceTimePaused = false;
        if (this.Dot.Behaviour == DotHost.BehaviourType.SpiralAroundWithCamera)
          this.PlayerManager.CanControl = true;
        else if (this.LevelManager.Name == "ELDERS")
        {
          while (!this.PlayerManager.CanControl)
            this.PlayerManager.CanControl = true;
          this.PlayerManager.CanControl = false;
        }
        else
        {
          while (!this.PlayerManager.CanControl)
            this.PlayerManager.CanControl = true;
        }
        if (this.LevelManager.Name == "STARGATE" && !this.GameState.SaveData.ThisLevel.InactiveArtObjects.Contains(0))
          this.SoundManager.PlayNewSong("Swell");
        else
          this.SoundManager.PlayNewSong();
        this.IsDisposed = true;
        ServiceHelper.RemoveComponent<IntroPanDown>(this);
      }
    }
  }
}
