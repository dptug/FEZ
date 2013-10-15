// Type: FezEngine.Components.LightingPostProcess
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezEngine.Components
{
  public class LightingPostProcess : DrawableGameComponent, ILightingPostProcess
  {
    private RenderTargetHandle lightMapsRth;
    private LightingPostEffect lightingPostEffect;
    private bool hadRt;
    private int i;

    public Texture2D LightmapTexture
    {
      get
      {
        return (Texture2D) this.lightMapsRth.Target;
      }
    }

    [ServiceDependency]
    public IEngineStateManager EngineState { protected get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { protected get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { protected get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { protected get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { protected get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderingManager { protected get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { protected get; set; }

    [ServiceDependency]
    public IFogManager FogManager { private get; set; }

    public event Action DrawGeometryLights = new Action(Util.NullAction);

    public event Action DrawOnTopLights = new Action(Util.NullAction);

    public LightingPostProcess(Game game)
      : base(game)
    {
      this.DrawOrder = 100;
      ServiceHelper.AddService((object) this);
    }

    protected override void LoadContent()
    {
      this.lightingPostEffect = new LightingPostEffect();
      this.lightMapsRth = this.TargetRenderingManager.TakeTarget();
      this.TargetRenderingManager.PreDraw += new Action<GameTime>(this.PreDraw);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.TargetRenderingManager.ReturnTarget(this.lightMapsRth);
      this.lightMapsRth = (RenderTargetHandle) null;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.LevelManager.SkipPostProcess)
        return;
      this.UpdateLightFilter();
    }

    private void UpdateLightFilter()
    {
      this.lightingPostEffect.DawnContribution = this.TimeManager.DawnContribution;
      this.lightingPostEffect.DuskContribution = this.TimeManager.DuskContribution;
      this.lightingPostEffect.NightContribution = this.TimeManager.NightContribution;
    }

    private void PreDraw(GameTime gameTime)
    {
      this.LevelMaterializer.RegisterSatellites();
      if (this.EngineState.StereoMode || this.LevelManager.Quantum)
        return;
      this.LevelManager.ActualDiffuse = new Color(this.LevelManager.BaseDiffuse, this.LevelManager.BaseDiffuse, this.LevelManager.BaseDiffuse);
      this.LevelManager.ActualAmbient = new Color(this.LevelManager.BaseAmbient, this.LevelManager.BaseAmbient, this.LevelManager.BaseAmbient);
      this.hadRt = this.TargetRenderingManager.HasRtInQueue || this.LevelManager.WaterType == LiquidType.Sewer || this.LevelManager.WaterType == LiquidType.Lava;
      this.GraphicsDevice.SetRenderTarget(this.lightMapsRth.Target);
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.PrepareDraw(graphicsDevice);
      SettingsManager.SetupViewport(this.GraphicsDevice, false);
      if (!this.LevelManager.SkipPostProcess && (double) this.TimeManager.NightContribution != 0.0)
      {
        this.LevelManager.ActualDiffuse = Color.Lerp(this.LevelManager.ActualDiffuse, this.FogManager.Color, this.TimeManager.NightContribution * 0.4f);
        this.LevelManager.ActualAmbient = this.LevelManager.Sky == null || !this.LevelManager.Sky.FoliageShadows ? Color.Lerp(this.LevelManager.ActualAmbient, Color.White, this.TimeManager.NightContribution * 0.5f) : Color.Lerp(this.LevelManager.ActualAmbient, Color.Lerp(this.FogManager.Color, Color.White, 0.5f), this.TimeManager.NightContribution * 0.5f);
      }
      if (!this.LevelManager.SkipPostProcess)
        this.LevelManager.ActualAmbient = Color.Lerp(this.LevelManager.ActualAmbient, this.FogManager.Color, 0.14375f);
      if (this.LevelManager.WaterType == LiquidType.Sewer)
        this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, Color.Black, 1f, 0);
      else
        this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, Color.Gray, 1f, 0);
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
      this.LevelMaterializer.RenderPass = RenderPass.Occluders;
      GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.None);
      this.DrawLightOccluders();
      GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue);
      this.LevelMaterializer.RenderPass = RenderPass.LightInAlphaEmitters;
      this.LevelMaterializer.TrilesMesh.Draw();
      this.LevelMaterializer.ArtObjectsMesh.Draw();
      GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).SlopeScaleDepthBias = -0.1f;
      GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).DepthBias = FezMath.IsOrthographic(this.CameraManager.Viewpoint) ? -1E-07f : (float) (-9.99999974737875E-05 / ((double) this.CameraManager.FarPlane - (double) this.CameraManager.NearPlane));
      this.LevelMaterializer.AnimatedPlanesMesh.Draw();
      this.LevelMaterializer.StaticPlanesMesh.Draw();
      this.LevelMaterializer.NpcMesh.Draw();
      this.DrawGeometryLights();
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
      this.DrawOnTopLights();
      GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      this.LevelMaterializer.RenderPass = RenderPass.WorldspaceLightmaps;
      this.LevelMaterializer.StaticPlanesMesh.Draw();
      this.LevelMaterializer.AnimatedPlanesMesh.Draw();
      this.LevelMaterializer.RenderPass = RenderPass.ScreenspaceLightmaps;
      this.LevelMaterializer.StaticPlanesMesh.Draw();
      this.LevelMaterializer.AnimatedPlanesMesh.Draw();
      GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).DepthBias = 0.0f;
      GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).SlopeScaleDepthBias = 0.0f;
      this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.All);
    }

    protected virtual void DrawLightOccluders()
    {
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.EngineState.StereoMode || this.LevelManager.Quantum || (this.EngineState.Loading || this.LevelManager.WaterType == LiquidType.Sewer) || this.EngineState.SkipRendering)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferEnable = false;
      GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue);
      if (!this.LevelManager.SkipPostProcess)
      {
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.LessEqual, StencilMask.Level);
        this.DrawLightFilter();
      }
      GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Always, StencilMask.None);
      GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Multiply2X);
      this.TargetRenderingManager.DrawFullscreen((Texture) this.lightMapsRth.Target);
      GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Alphablending);
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
      GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.All);
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferEnable = true;
    }

    private void DrawLightFilter()
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      if (!FezMath.AlmostEqual(this.lightingPostEffect.DawnContribution, 0.0f))
      {
        this.lightingPostEffect.Pass = LightingPostEffect.Passes.Dawn;
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Screen);
        this.TargetRenderingManager.DrawFullscreen((BaseEffect) this.lightingPostEffect);
      }
      if (!FezMath.AlmostEqual(this.lightingPostEffect.DuskContribution, 0.0f))
      {
        this.lightingPostEffect.Pass = LightingPostEffect.Passes.Dusk_Multiply;
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Multiply);
        this.TargetRenderingManager.DrawFullscreen((BaseEffect) this.lightingPostEffect);
        this.lightingPostEffect.Pass = LightingPostEffect.Passes.Dusk_Screen;
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Screen);
        this.TargetRenderingManager.DrawFullscreen((BaseEffect) this.lightingPostEffect);
      }
      if (FezMath.AlmostEqual(this.lightingPostEffect.NightContribution, 0.0f))
        return;
      this.lightingPostEffect.Pass = LightingPostEffect.Passes.Night;
      GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Multiply);
      this.TargetRenderingManager.DrawFullscreen((BaseEffect) this.lightingPostEffect);
    }
  }
}
