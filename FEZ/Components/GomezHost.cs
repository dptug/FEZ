// Type: FezGame.Components.GomezHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  public class GomezHost : DrawableGameComponent
  {
    private const float InvincibilityBlinkSpeed = 5f;
    private GomezEffect effect;
    private readonly Mesh playerMesh;
    public static GomezHost Instance;
    private TimeSpan sinceBackgroundChanged;
    private bool lastBackground;
    private bool lastHideFez;
    private AnimatedTexture lastAnimation;

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    public GomezHost(Game game)
      : base(game)
    {
      this.playerMesh = new Mesh()
      {
        SamplerState = SamplerState.PointClamp
      };
      this.UpdateOrder = 11;
      this.DrawOrder = 9;
      GomezHost.Instance = this;
    }

    public override void Initialize()
    {
      this.playerMesh.AddFace(new Vector3(1f), new Vector3(0.0f, 0.25f, 0.0f), FaceOrientation.Front, true, true);
      this.PlayerManager.Mesh = this.playerMesh;
      this.LevelManager.LevelChanged += (Action) (() => this.effect.ColorSwapMode = this.LevelManager.WaterType == LiquidType.Sewer ? ColorSwapMode.Gameboy : (this.LevelManager.WaterType == LiquidType.Lava ? ColorSwapMode.VirtualBoy : (this.LevelManager.BlinkingAlpha ? ColorSwapMode.Cmyk : ColorSwapMode.None)));
      this.LightingPostProcess.DrawGeometryLights += new Action(this.PreDraw);
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.playerMesh.Effect = (BaseEffect) (this.effect = new GomezEffect());
    }

    public override void Update(GameTime gameTime)
    {
      if (this.PlayerManager.Animation == null)
        return;
      this.playerMesh.Position = this.PlayerManager.Position + (float) ((1.0 - ((double) this.PlayerManager.Size.Y + (this.PlayerManager.CarriedInstance != null || this.PlayerManager.Action == ActionType.ThrowingHeavy ? -2.0 : 0.0))) / 2.0) * Vector3.UnitY;
      if (this.lastAnimation != this.PlayerManager.Animation)
      {
        this.effect.Animation = (Texture) this.PlayerManager.Animation.Texture;
        this.lastAnimation = this.PlayerManager.Animation;
      }
      int width = this.lastAnimation.Texture.Width;
      int height = this.lastAnimation.Texture.Height;
      Rectangle rectangle = this.lastAnimation.Offsets[this.lastAnimation.Timing.Frame];
      this.playerMesh.FirstGroup.TextureMatrix.Set(new Matrix?(new Matrix((float) rectangle.Width / (float) width, 0.0f, 0.0f, 0.0f, 0.0f, (float) rectangle.Height / (float) height, 0.0f, 0.0f, (float) rectangle.X / (float) width, (float) rectangle.Y / (float) height, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f)));
      if (this.lastBackground != this.PlayerManager.Background && !ActionTypeExtensions.NoBackgroundDarkening(this.PlayerManager.Action))
      {
        this.sinceBackgroundChanged = TimeSpan.Zero;
        this.lastBackground = this.PlayerManager.Background;
        if (!this.LevelManager.LowPass && EndCutscene32Host.Instance == null && EndCutscene64Host.Instance == null)
          this.SoundManager.FadeFrequencies(this.PlayerManager.Background);
      }
      if (this.sinceBackgroundChanged.TotalSeconds < 1.0)
        this.sinceBackgroundChanged += gameTime.ElapsedGameTime;
      this.effect.Background = ActionTypeExtensions.NoBackgroundDarkening(this.PlayerManager.Action) ? 0.0f : FezMath.Saturate(this.PlayerManager.Background ? (float) (this.sinceBackgroundChanged.TotalSeconds * 2.0) : (float) (1.0 - this.sinceBackgroundChanged.TotalSeconds * 2.0));
      bool flag1 = (double) this.CollisionManager.GravityFactor < 0.0;
      Viewpoint view = FezMath.IsOrthographic(this.CameraManager.Viewpoint) || !this.CameraManager.ActionRunning ? this.CameraManager.Viewpoint : this.CameraManager.LastViewpoint;
      Vector2 vector2 = ActionTypeExtensions.GetOffset(this.PlayerManager.Action) / 16f;
      vector2.Y -= this.lastAnimation.PotOffset.Y / 64f;
      this.playerMesh.Scale = new Vector3((float) this.PlayerManager.Animation.FrameWidth / 16f, (float) this.PlayerManager.Animation.FrameHeight / 16f * (float) Math.Sign(this.CollisionManager.GravityFactor), 1f);
      this.playerMesh.Position += vector2.X * FezMath.RightVector(view) * (float) FezMath.Sign(this.PlayerManager.LookingDirection) + (flag1 ? (float) (-(double) vector2.Y - 1.0 / 16.0) : vector2.Y) * Vector3.UnitY;
      bool flag2 = this.PlayerManager.HideFez && !this.GameState.SaveData.IsNewGamePlus && !this.PlayerManager.Animation.NoHat && !ActionTypeExtensions.IsCarry(this.PlayerManager.Action);
      if (this.lastHideFez == flag2)
        return;
      this.lastHideFez = flag2;
      this.effect.NoMoreFez = this.lastHideFez;
    }

    private void PreDraw()
    {
      if (this.GameState.Loading || this.PlayerManager.Hidden || this.GameState.InFpsMode)
        return;
      this.effect.Pass = LightingEffectPass.Pre;
      if (!this.PlayerManager.FullBright)
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.Level));
      else
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.None));
      this.playerMesh.Draw();
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.None));
      this.effect.Pass = LightingEffectPass.Main;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.StereoMode && !this.GameState.FarawaySettings.InTransition)
        return;
      this.DoDraw();
    }

    public void DoDraw()
    {
      if (this.GameState.Loading || this.PlayerManager.Hidden || (this.GameState.InMap || FezMath.AlmostEqual(this.PlayerManager.GomezOpacity, 0.0f)))
        return;
      if (this.GameState.StereoMode || this.LevelManager.Quantum)
      {
        this.PlayerManager.Mesh.Rotation = FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.CameraManager.LastViewpoint == Viewpoint.None ? this.CameraManager.Rotation : Quaternion.CreateFromAxisAngle(Vector3.UnitY, FezMath.ToPhi(this.CameraManager.LastViewpoint));
        if (this.PlayerManager.LookingDirection == HorizontalDirection.Left)
          this.PlayerManager.Mesh.Rotation *= FezMath.QuaternionFromPhi(3.141593f);
      }
      this.PlayerManager.Mesh.Material.Opacity = this.PlayerManager.Action == ActionType.Suffering || this.PlayerManager.Action == ActionType.Sinking ? (float) FezMath.Saturate((Math.Sin((double) this.PlayerManager.BlinkSpeed * 6.28318548202515 * 5.0) + 0.5 - (double) this.PlayerManager.BlinkSpeed * 1.25) * 2.0) : this.PlayerManager.GomezOpacity;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      if (!ActionTypeExtensions.SkipSilhouette(this.PlayerManager.Action))
      {
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Greater, StencilMask.NoSilhouette);
        this.playerMesh.DepthWrites = false;
        this.playerMesh.AlwaysOnTop = true;
        this.effect.Silhouette = true;
        this.playerMesh.Draw();
      }
      if (!this.PlayerManager.Background)
      {
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Equal, StencilMask.Hole);
        this.playerMesh.AlwaysOnTop = true;
        this.playerMesh.DepthWrites = false;
        this.effect.Silhouette = false;
        this.playerMesh.Draw();
      }
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Gomez));
      this.playerMesh.AlwaysOnTop = ActionTypeExtensions.NeedsAlwaysOnTop(this.PlayerManager.Action);
      this.playerMesh.DepthWrites = true;
      this.effect.Silhouette = false;
      this.playerMesh.Draw();
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }
  }
}
