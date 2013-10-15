// Type: FezGame.Components.EndCutscene32.Pixelizer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components.EndCutscene32
{
  internal class Pixelizer : DrawableGameComponent
  {
    private const float ZoomStepDuration = 3f;
    private readonly EndCutscene32Host Host;
    private RenderTarget2D LowResRT;
    private Mesh GoMesh;
    private Group GomezGroup;
    private Group FezGroup;
    private float TotalTime;
    private float StepTime;
    private Pixelizer.State ActiveState;
    private int ZoomStep;
    private float InitialRadius;
    private float OldSfxVol;

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    public Pixelizer(Game game, EndCutscene32Host host)
      : base(game)
    {
      this.Host = host;
      this.DrawOrder = 1000;
      this.UpdateOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      Pixelizer pixelizer = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
      vertexColored1.Fullbright = true;
      DefaultEffect.VertexColored vertexColored2 = vertexColored1;
      mesh2.Effect = (BaseEffect) vertexColored2;
      mesh1.AlwaysOnTop = true;
      mesh1.DepthWrites = false;
      Mesh mesh3 = mesh1;
      pixelizer.GoMesh = mesh3;
      this.GomezGroup = this.GoMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, Color.White, true, false, false);
      this.FezGroup = this.GoMesh.AddFace(Vector3.One / 2f, Vector3.Zero, FaceOrientation.Front, Color.Red, true, false, false);
      this.GoMesh.Effect.ForcedViewMatrix = new Matrix?(new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, -249.95f, 1f));
      this.GoMesh.Effect.ForcedProjectionMatrix = new Matrix?(new Matrix(0.2f, 0.0f, 0.0f, 0.0f, 0.0f, 0.3555556f, 0.0f, 0.0f, 0.0f, 0.0f, -0.0020004f, 0.0f, 0.0f, 0.0f, -0.00020004f, 1f));
      this.LevelManager.ActualAmbient = new Color(0.25f, 0.25f, 0.25f);
      this.LevelManager.ActualDiffuse = Color.White;
      this.OldSfxVol = this.SoundManager.SoundEffectVolume;
      this.Reset();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.LowResRT.Dispose();
      this.LowResRT = (RenderTarget2D) null;
      if (this.GoMesh != null)
        this.GoMesh.Dispose();
      this.GoMesh = (Mesh) null;
    }

    private void Reset()
    {
      this.CameraManager.PixelsPerTrixel = 2f;
      this.CameraManager.SnapInterpolation();
      this.InitialRadius = this.CameraManager.Radius;
      this.GomezGroup.Position = Vector3.Zero;
      this.FezGroup.Position = new Vector3(-0.25f, 0.75f, 0.0f);
      this.GoMesh.Scale = Vector3.One;
      this.FezGroup.Rotation = this.GomezGroup.Rotation = Quaternion.Identity;
      this.CameraManager.Center = Vector3.Zero;
      this.CameraManager.Direction = Vector3.UnitZ;
      this.CameraManager.Radius = 10f;
      this.ZoomStep = 1;
      this.TotalTime = 0.0f;
      this.StepTime = 0.0f;
      this.RescaleRT();
    }

    private void RescaleRT()
    {
      if (this.LowResRT != null)
      {
        this.TargetRenderer.UnscheduleHook(this.LowResRT);
        this.LowResRT.Dispose();
      }
      this.LowResRT = new RenderTarget2D(this.GraphicsDevice, 1280 / this.ZoomStep, 720 / this.ZoomStep, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
      if (this.ActiveState != Pixelizer.State.Zooming)
        return;
      this.TargetRenderer.ScheduleHook(this.DrawOrder, this.LowResRT);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      float num = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.StepTime += num;
      if (this.ActiveState == Pixelizer.State.Wait)
      {
        this.CameraManager.Center = this.PlayerManager.Center + new Vector3(0.0f, 2f, 0.0f);
        if ((double) this.StepTime <= 5.0)
          return;
        this.ChangeState();
      }
      else
      {
        if (this.ActiveState != Pixelizer.State.Zooming)
          return;
        this.TotalTime += num;
        this.PlayerManager.FullBright = true;
        if ((double) this.StepTime > 3.0 / Math.Max(Math.Pow((double) this.ZoomStep / 10.0, 2.0), 1.0))
        {
          this.RescaleRT();
          ++this.ZoomStep;
          this.StepTime = 0.0f;
        }
        this.CameraManager.Radius = MathHelper.Lerp(this.InitialRadius, 6f * SettingsManager.GetViewScale(this.GraphicsDevice), Easing.EaseIn((double) Easing.EaseOut((double) FezMath.Saturate(this.TotalTime / 57f), EasingType.Sine), EasingType.Quadratic));
        this.CameraManager.Center = Vector3.Lerp(this.PlayerManager.Center + new Vector3(0.0f, 2f, 0.0f), this.PlayerManager.Center, Easing.EaseOut((double) FezMath.Saturate(this.TotalTime / 57f), EasingType.Sine));
        if ((double) this.TotalTime <= 57.0)
          return;
        this.ChangeState();
      }
    }

    private void ChangeState()
    {
      if (this.ActiveState == Pixelizer.State.Wait)
        this.TargetRenderer.ScheduleHook(this.DrawOrder, this.LowResRT);
      if (this.ActiveState == Pixelizer.State.Zooming)
      {
        this.TargetRenderer.UnscheduleHook(this.LowResRT);
        this.SoundManager.KillSounds();
        this.SoundManager.SoundEffectVolume = this.OldSfxVol;
        this.Host.Cycle();
      }
      else
      {
        this.StepTime = 0.0f;
        ++this.ActiveState;
        this.Update(new GameTime());
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.ActiveState == Pixelizer.State.Wait || this.GameState.Loading)
        return;
      if (this.GameState.Paused)
      {
        if (!this.TargetRenderer.IsHooked(this.LowResRT))
          return;
        this.TargetRenderer.Resolve(this.LowResRT, true);
        this.GraphicsDevice.Clear(Color.Black);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Opaque);
        this.TargetRenderer.DrawFullscreen((Texture) this.LowResRT);
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      }
      else
      {
        GraphicsDevice graphicsDevice = this.GraphicsDevice;
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.NotEqual, StencilMask.Gomez);
        Vector3 vector3 = EndCutscene32Host.PurpleBlack.ToVector3();
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Alphablending);
        this.TargetRenderer.DrawFullscreen(new Color(vector3.X, vector3.Y, vector3.Z, Easing.EaseIn((double) Easing.EaseOut((double) FezMath.Saturate(this.TotalTime / 57f), EasingType.Sine), EasingType.Quartic)));
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Always, StencilMask.None);
        this.SoundManager.SoundEffectVolume = 1f - Easing.EaseIn((double) FezMath.Saturate(this.TotalTime / 57f), EasingType.Quadratic);
        if ((double) this.TotalTime > 54.0 && (double) this.TotalTime < 57.0)
        {
          this.PlayerManager.Hidden = true;
          this.GoMesh.Draw();
        }
        if (this.TargetRenderer.IsHooked(this.LowResRT))
        {
          this.TargetRenderer.Resolve(this.LowResRT, true);
          this.GraphicsDevice.Clear(Color.Black);
          SettingsManager.SetupViewport(this.GraphicsDevice, false);
          this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
          GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Opaque);
          this.TargetRenderer.DrawFullscreen((Texture) this.LowResRT);
          GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Alphablending);
        }
        if ((double) this.TotalTime <= 57.0)
          return;
        this.PlayerManager.Hidden = true;
        this.GoMesh.Draw();
      }
    }

    private enum State
    {
      Wait,
      Zooming,
    }
  }
}
