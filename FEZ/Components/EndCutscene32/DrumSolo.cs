// Type: FezGame.Components.EndCutscene32.DrumSolo
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components.EndCutscene32
{
  internal class DrumSolo : DrawableGameComponent
  {
    private const float ZoomOutDuration = 6f;
    private const float DrumSoloDuration = 15f;
    private const float JumpUpDuration = 4.5f;
    private const float JumpDistance = 8f;
    private float DestinationRadius;
    private readonly EndCutscene32Host Host;
    private float Time;
    private DrumSolo.State ActiveState;
    private float StarRotationTimeKeeper;
    private Mesh StarMesh;
    private Mesh Flare;
    private FezLogo FezLogo;
    private SoundEffect sTitleBassHit;

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public DrumSolo(Game game, EndCutscene32Host host)
      : base(game)
    {
      this.Host = host;
      this.DrawOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.DestinationRadius = 20f * SettingsManager.GetViewScale(this.GraphicsDevice);
      this.Flare = new Mesh()
      {
        Blending = new BlendingMode?(BlendingMode.Additive),
        SamplerState = SamplerState.LinearClamp,
        DepthWrites = false
      };
      this.Flare.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, true, true);
      this.StarMesh = new Mesh()
      {
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        DepthWrites = false
      };
      Color color = new Color(1f, 1f, 0.3f, 0.0f);
      for (int index = 0; index < 8; ++index)
      {
        float num1 = (float) ((double) index * 6.28318548202515 / 8.0);
        float num2 = (float) (((double) index + 0.5) * 6.28318548202515 / 8.0);
        this.StarMesh.AddColoredTriangle(Vector3.Zero, new Vector3((float) Math.Sin((double) num1), (float) Math.Cos((double) num1), 0.0f), new Vector3((float) Math.Sin((double) num2), (float) Math.Cos((double) num2), 0.0f), new Color(1f, 1f, 0.5f, 0.7f), color, color);
      }
      this.Flare.Effect = (BaseEffect) new DefaultEffect.Textured();
      this.StarMesh.Effect = (BaseEffect) new DefaultEffect.VertexColored();
      this.Flare.Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Background Planes/flare"));
      ServiceHelper.AddComponent((IGameComponent) (this.FezLogo = new FezLogo(this.Game)));
      this.FezLogo.Enabled = false;
      this.sTitleBassHit = this.CMProvider.Global.Load<SoundEffect>("Sounds/Intro/LogoZoom");
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.StarMesh != null)
        this.StarMesh.Dispose();
      if (this.Flare != null)
        this.Flare.Dispose();
      this.Flare = this.StarMesh = (Mesh) null;
      Waiters.Wait(2.0, (Action) (() => this.CMProvider.Dispose(CM.EndCutscene)));
    }

    private void Reset()
    {
      this.GameState.SkipRendering = false;
      this.GameState.InCutscene = false;
      this.GameState.SkyOpacity = 1f;
      this.LevelManager.ChangeLevel("DRUM");
      this.CameraManager.ChangeViewpoint(Viewpoint.Back, 0.0f);
      this.CameraManager.Center = new Vector3(15f, 13f, 18f);
      this.CameraManager.SnapInterpolation();
      this.PlayerManager.Action = ActionType.DrumsIdle;
      this.LevelManager.ActualAmbient = Color.White;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      float num1 = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.Time += num1;
      switch (this.ActiveState)
      {
        case DrumSolo.State.ZoomOut:
          if ((double) this.Time == 0.0)
            this.Reset();
          float num2 = FezMath.Saturate(this.Time / 6f);
          this.PlayerManager.Position = new Vector3(483.0 / 32.0, 197.0 / 16.0, 18.33866f);
          this.CameraManager.Center = new Vector3(15.25f, 405.0 / 32.0, 18.33866f);
          this.CameraManager.Radius = MathHelper.Lerp(0.0f, this.DestinationRadius, Easing.EaseIn((double) Easing.EaseOut((double) num2, EasingType.Sine), EasingType.Cubic));
          this.CameraManager.SnapInterpolation();
          if ((double) this.Time <= 6.0)
            break;
          this.ChangeState();
          this.PlayerManager.Action = (ActionType) (106 + RandomHelper.Random.Next(1, 7));
          this.SoundManager.PlayNewSong("gomez_drums", 0.0f);
          break;
        case DrumSolo.State.DrumSolo:
          float num3 = FezMath.Saturate(this.Time / 15f);
          this.PlayerManager.Position = new Vector3(483.0 / 32.0, 197.0 / 16.0, 18.33866f);
          this.CameraManager.Center = new Vector3(15.25f, 405.0 / 32.0, 18.33866f);
          this.PlayerManager.LeaveGroundPosition = this.PlayerManager.Position;
          this.CameraManager.Radius = this.DestinationRadius;
          this.CameraManager.SnapInterpolation();
          float num4 = Easing.EaseOut((double) FezMath.Saturate(num3 * 25f), EasingType.Cubic);
          this.StarMesh.Material.Opacity = num4;
          this.Flare.Material.Diffuse = new Vector3(num4 / 3f);
          this.Flare.Position = this.StarMesh.Position = this.PlayerManager.Position + Vector3.Normalize(-this.CameraManager.Direction) * 10f;
          this.Flare.Scale = this.StarMesh.Scale = MathHelper.Lerp(this.CameraManager.Radius * 0.8f, this.CameraManager.Radius * 0.6f, (float) (Math.Sin((double) this.Time * 5.0) / 2.0 + 0.5)) * Vector3.One * num4;
          this.StarRotationTimeKeeper += (float) ((double) num1 * Math.Sin((double) this.Time) * 0.75 + 1.0);
          this.StarMesh.Rotation = this.CameraManager.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, this.StarRotationTimeKeeper * (float) (0.025000000372529 + 0.025000000372529 * (double) num3));
          if (this.PlayerManager.Animation.Timing.Ended)
            this.PlayerManager.Action = (ActionType) (106 + RandomHelper.Random.Next(1, 7));
          this.PlayerManager.Animation.Timing.Update(TimeSpan.FromSeconds((double) num1), (float) (1.0 + (double) num3 * 3.0));
          if ((double) this.Time <= 15.0)
            break;
          this.ChangeState();
          break;
        case DrumSolo.State.JumpUp:
          float num5 = FezMath.Saturate(this.Time / 4.5f);
          float num6 = 1f - Easing.EaseIn((double) FezMath.Saturate(num5 * 25f), EasingType.Cubic);
          this.StarMesh.Material.Opacity = num6;
          this.Flare.Material.Diffuse = new Vector3(num6 / 3f);
          if ((double) num6 > 0.0)
          {
            this.Flare.Position = this.StarMesh.Position = this.PlayerManager.Position + Vector3.Normalize(-this.CameraManager.Direction) * 10f;
            this.Flare.Scale = this.StarMesh.Scale = MathHelper.Lerp(this.CameraManager.Radius * 0.6f, this.CameraManager.Radius * 0.4f, (float) (Math.Sin((double) this.Time * 5.0) / 2.0 + 0.5)) * Vector3.One * num6;
            this.StarRotationTimeKeeper += (float) ((double) num1 * Math.Sin((double) this.Time) * 0.75 + 1.0);
            this.StarMesh.Rotation = this.CameraManager.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, this.StarRotationTimeKeeper * 0.05f);
          }
          this.PlayerManager.Action = ActionType.VictoryForever;
          float num7 = Easing.EaseOut((double) num5, EasingType.Quadratic);
          this.PlayerManager.Position = new Vector3(483.0 / 32.0, (float) (197.0 / 16.0 + (double) num7 * 8.0), 18.33866f);
          this.CameraManager.Center = new Vector3(15.25f, (float) (405.0 / 32.0 + (double) num7 * 8.0), 18.33866f);
          this.CameraManager.Radius = this.DestinationRadius;
          if ((double) this.Time <= 4.5)
            break;
          this.ChangeState();
          break;
        case DrumSolo.State.TetraSplode:
          this.ChangeState();
          this.FezLogo.Visible = true;
          this.FezLogo.Enabled = true;
          this.FezLogo.TransitionStarted = true;
          this.FezLogo.SinceStarted = 1f;
          this.FezLogo.Opacity = 1f;
          this.FezLogo.HalfSpeed = true;
          SoundEffectExtensions.Emit(this.sTitleBassHit).Persistent = true;
          break;
        case DrumSolo.State.FezSplash:
          this.PlayerManager.Position = new Vector3(483.0 / 32.0, 325.0 / 16.0, 18.33866f);
          this.CameraManager.Center = new Vector3(15.25f, 661.0 / 32.0, 18.33866f);
          this.CameraManager.Radius = this.DestinationRadius;
          this.CameraManager.SnapInterpolation();
          if (!this.FezLogo.IsFullscreen)
            break;
          this.Host.Cycle();
          PauseMenu.Starfield = this.FezLogo.Starfield;
          ServiceHelper.RemoveComponent<FezLogo>(this.FezLogo);
          this.GameState.Pause(true);
          break;
      }
    }

    private void ChangeState()
    {
      this.Time = 0.0f;
      ++this.ActiveState;
      this.Update(new GameTime());
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      switch (this.ActiveState)
      {
        case DrumSolo.State.ZoomOut:
          float num = FezMath.Saturate(this.Time / 6f);
          Vector3 vector3 = EndCutscene32Host.PurpleBlack.ToVector3();
          this.TargetRenderer.DrawFullscreen(new Color(vector3.X, vector3.Y, vector3.Z, 1f - FezMath.Saturate(num * 10f)));
          break;
        case DrumSolo.State.DrumSolo:
        case DrumSolo.State.JumpUp:
          this.StarMesh.Draw();
          this.Flare.Draw();
          break;
        case DrumSolo.State.FezSplash:
          this.GraphicsDevice.Clear(Color.White);
          break;
      }
    }

    private enum State
    {
      ZoomOut,
      DrumSolo,
      JumpUp,
      TetraSplode,
      FezSplash,
    }
  }
}
