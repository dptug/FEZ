// Type: FezGame.Components.SewerLightHacks
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  internal class SewerLightHacks : DrawableGameComponent
  {
    private SewerHaxEffect Effect;

    [ServiceDependency]
    public IGameStateManager GameState { get; private set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; private set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; private set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { get; private set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { get; private set; }

    public SewerLightHacks(Game game)
      : base(game)
    {
      this.DrawOrder = 49;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.Effect = new SewerHaxEffect();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
    }

    private void TryInitialize()
    {
      this.Visible = this.LevelManager.WaterType == LiquidType.Sewer;
      if (!this.Visible)
        return;
      this.LevelManager.BaseAmbient = 1f;
      this.LevelManager.BaseDiffuse = 0.0f;
      this.LevelManager.HaloFiltering = false;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.StereoMode)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
      GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.None);
      this.LevelMaterializer.StaticPlanesMesh.AlwaysOnTop = true;
      this.LevelMaterializer.StaticPlanesMesh.DepthWrites = false;
      foreach (BackgroundPlane backgroundPlane in this.LevelMaterializer.LevelPlanes)
        backgroundPlane.Group.Enabled = backgroundPlane.Id < 0;
      this.LevelMaterializer.StaticPlanesMesh.Draw();
      this.LevelMaterializer.StaticPlanesMesh.AlwaysOnTop = false;
      this.LevelMaterializer.StaticPlanesMesh.DepthWrites = true;
      foreach (BackgroundPlane backgroundPlane in this.LevelMaterializer.LevelPlanes)
        backgroundPlane.Group.Enabled = true;
      GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.All);
      GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.LessEqual, StencilMask.Sky);
      this.TargetRenderer.DrawFullscreen((BaseEffect) this.Effect, (Texture) this.LightingPostProcess.LightmapTexture);
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }
  }
}
