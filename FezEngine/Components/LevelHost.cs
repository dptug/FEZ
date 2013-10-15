// Type: FezEngine.Components.LevelHost
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Components
{
  public abstract class LevelHost : DrawableGameComponent
  {
    [ServiceDependency]
    public IDebuggingBag DebuggingBag { protected get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { protected get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { protected get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { protected get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardProvider { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    protected LevelHost(Game game)
      : base(game)
    {
      this.DrawOrder = 5;
    }

    protected override void LoadContent()
    {
      this.LevelMaterializer.TrilesMesh.Effect = (BaseEffect) new TrileEffect();
      this.LevelMaterializer.ArtObjectsMesh.Effect = (BaseEffect) new InstancedArtObjectEffect();
      this.LevelMaterializer.StaticPlanesMesh.Effect = (BaseEffect) new InstancedStaticPlaneEffect();
      this.LevelMaterializer.AnimatedPlanesMesh.Effect = (BaseEffect) new InstancedAnimatedPlaneEffect();
      this.LevelMaterializer.NpcMesh.Effect = (BaseEffect) new AnimatedPlaneEffect()
      {
        IgnoreShading = true
      };
      this.CameraManager.PixelsPerTrixel = 3f;
    }

    public override void Draw(GameTime gameTime)
    {
      this.DoDraw();
    }

    protected void DoDraw()
    {
      if (this.LevelManager.Sky != null && this.LevelManager.Sky.Name == "GRAVE")
      {
        this.LevelMaterializer.RenderPass = RenderPass.Ghosts;
        this.LevelMaterializer.NpcMesh.DepthWrites = false;
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.Ghosts));
        this.LevelMaterializer.NpcMesh.Draw();
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
        this.LevelMaterializer.NpcMesh.DepthWrites = true;
      }
      this.LevelMaterializer.RenderPass = RenderPass.Normal;
      GraphicsDeviceExtensions.GetDssCombiner(this.GraphicsDevice).DepthBufferEnable = true;
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.Level));
      this.LevelMaterializer.TrilesMesh.Draw();
      this.LevelMaterializer.ArtObjectsMesh.Draw();
      GraphicsDeviceExtensions.GetRasterCombiner(this.GraphicsDevice).SlopeScaleDepthBias = -0.1f;
      GraphicsDeviceExtensions.GetRasterCombiner(this.GraphicsDevice).DepthBias = FezMath.IsOrthographic(this.CameraManager.Viewpoint) ? -1E-07f : (float) (-0.100000001490116 / ((double) this.CameraManager.FarPlane - (double) this.CameraManager.NearPlane));
      this.LevelMaterializer.StaticPlanesMesh.Draw();
      this.LevelMaterializer.AnimatedPlanesMesh.Draw();
      GraphicsDeviceExtensions.GetRasterCombiner(this.GraphicsDevice).DepthBias = 0.0f;
      GraphicsDeviceExtensions.GetRasterCombiner(this.GraphicsDevice).SlopeScaleDepthBias = 0.0f;
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.NoSilhouette));
      this.LevelMaterializer.NpcMesh.Draw();
      this.LevelMaterializer.RenderPass = RenderPass.Normal;
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.None));
    }
  }
}
