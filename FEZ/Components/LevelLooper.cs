// Type: FezGame.Components.LevelLooper
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
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
  internal class LevelLooper : DrawableGameComponent
  {
    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { private get; set; }

    public LevelLooper(Game game)
      : base(game)
    {
      this.UpdateOrder = -1;
      this.DrawOrder = 6;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += (Action) (() => this.CameraManager.ViewOffset = Vector3.Zero);
      this.LightingPostProcess.DrawGeometryLights += new Action(this.Draw);
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.LevelManager.Loops || this.PlayerManager.Action == ActionType.FreeFalling || (this.GameState.Loading || this.GameState.InMap) || this.GameState.Paused)
        return;
      while ((double) this.PlayerManager.Position.Y < 0.0)
      {
        this.PlayerManager.Position += this.LevelManager.Size * Vector3.UnitY;
        IGameCameraManager cameraManager1 = this.CameraManager;
        Vector3 vector3_1 = cameraManager1.Center + this.LevelManager.Size * Vector3.UnitY;
        cameraManager1.Center = vector3_1;
        IGameCameraManager cameraManager2 = this.CameraManager;
        Vector3 vector3_2 = cameraManager2.ViewOffset + this.LevelManager.Size * Vector3.UnitY;
        cameraManager2.ViewOffset = vector3_2;
      }
      while ((double) this.PlayerManager.Position.Y > (double) this.LevelManager.Size.Y)
      {
        this.PlayerManager.Position -= this.LevelManager.Size * Vector3.UnitY;
        IGameCameraManager cameraManager1 = this.CameraManager;
        Vector3 vector3_1 = cameraManager1.Center - this.LevelManager.Size * Vector3.UnitY;
        cameraManager1.Center = vector3_1;
        IGameCameraManager cameraManager2 = this.CameraManager;
        Vector3 vector3_2 = cameraManager2.ViewOffset - this.LevelManager.Size * Vector3.UnitY;
        cameraManager2.ViewOffset = vector3_2;
        this.PlayerManager.IgnoreFreefall = true;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      this.Draw();
    }

    private void Draw()
    {
      if (!this.LevelManager.Loops || this.GameState.Loading)
        return;
      float num = this.LevelManager.Size.Y * ((double) this.PlayerManager.Position.Y < (double) this.LevelManager.Size.Y / 2.0 ? 1f : -1f);
      this.GameState.LoopRender = true;
      if (this.LoopVisible())
      {
        IGameCameraManager cameraManager1 = this.CameraManager;
        Vector3 vector3_1 = cameraManager1.ViewOffset + num * Vector3.UnitY;
        cameraManager1.ViewOffset = vector3_1;
        this.DrawLoop();
        IGameCameraManager cameraManager2 = this.CameraManager;
        Vector3 vector3_2 = cameraManager2.ViewOffset - num * Vector3.UnitY;
        cameraManager2.ViewOffset = vector3_2;
      }
      this.GameState.LoopRender = false;
    }

    private bool LoopVisible()
    {
      if (!FezMath.IsOrthographic(this.CameraManager.Viewpoint))
        return true;
      Vector3 vector3_1 = FezMath.Abs(FezMath.RightVector(this.CameraManager.Viewpoint));
      BoundingFrustum frustum = this.CameraManager.Frustum;
      BoundingBox boundingBox = new BoundingBox()
      {
        Min = {
          X = -frustum.Left.D * frustum.Left.DotNormal(vector3_1),
          Y = -frustum.Bottom.D * frustum.Bottom.Normal.Y
        },
        Max = {
          X = -frustum.Right.D * frustum.Right.DotNormal(vector3_1),
          Y = -frustum.Top.D * frustum.Top.Normal.Y
        }
      };
      Vector3 vector3_2 = FezMath.Min(boundingBox.Min, boundingBox.Max);
      Vector3 vector3_3 = FezMath.Max(boundingBox.Min, boundingBox.Max);
      Rectangle rectangle = new Rectangle()
      {
        X = (int) Math.Floor((double) vector3_2.X),
        Y = (int) Math.Floor((double) vector3_2.Y),
        Width = (int) Math.Ceiling((double) vector3_3.X - (double) vector3_2.X),
        Height = (int) Math.Ceiling((double) vector3_3.Y - (double) vector3_2.Y)
      };
      if (rectangle.Y >= 0)
        return (double) (rectangle.Y + rectangle.Height) >= (double) this.LevelManager.Size.Y;
      else
        return true;
    }

    private void DrawLoop()
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      bool flag = this.LevelMaterializer.RenderPass == RenderPass.LightInAlphaEmitters;
      if (!flag)
        this.LevelMaterializer.RenderPass = RenderPass.Normal;
      if (!flag)
        GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
      this.LevelMaterializer.TrilesMesh.Draw();
      this.LevelMaterializer.ArtObjectsMesh.Draw();
      if (flag)
        GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).DepthBias = -0.0001f;
      this.LevelMaterializer.StaticPlanesMesh.Draw();
      this.LevelMaterializer.AnimatedPlanesMesh.Draw();
      if (flag)
        GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).DepthBias = 0.0f;
      if (!flag)
        GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.NoSilhouette));
      this.LevelMaterializer.NpcMesh.Draw();
      if (flag)
        return;
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }
  }
}
