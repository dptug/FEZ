// Type: FezEngine.Components.BurnInPostProcess
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezEngine.Components
{
  public class BurnInPostProcess : DrawableGameComponent
  {
    private Texture2D oldFrameBuffer;
    private RenderTargetHandle newFrameBuffer;
    private RenderTargetHandle ownedHandle;
    private BurnInPostEffect burnInEffect;

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderingManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    public BurnInPostProcess(Game game)
      : base(game)
    {
      this.DrawOrder = 200;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.Enabled = false;
      this.LevelManager.LevelChanged += new Action(this.TryCreateTargets);
      this.TryCreateTargets();
    }

    private void TryCreateTargets()
    {
      if (this.LevelManager.Name == null)
        return;
      if (this.LevelManager.BlinkingAlpha)
      {
        if (!this.Enabled)
        {
          this.ownedHandle = this.TargetRenderingManager.TakeTarget();
          this.newFrameBuffer = this.TargetRenderingManager.TakeTarget();
        }
        this.Enabled = true;
      }
      else
      {
        if (this.Enabled)
        {
          this.TargetRenderingManager.ReturnTarget(this.ownedHandle);
          this.ownedHandle = (RenderTargetHandle) null;
          this.TargetRenderingManager.ReturnTarget(this.newFrameBuffer);
          this.newFrameBuffer = (RenderTargetHandle) null;
        }
        this.Enabled = false;
      }
    }

    protected override void LoadContent()
    {
      this.burnInEffect = new BurnInPostEffect();
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.Enabled || this.EngineState.Loading || (this.EngineState.Paused || this.EngineState.InMap) || this.EngineState.InEditor)
      {
        if (this.newFrameBuffer == null || !this.TargetRenderingManager.IsHooked(this.newFrameBuffer.Target))
          return;
        this.TargetRenderingManager.Resolve(this.newFrameBuffer.Target, false);
        this.GraphicsDevice.Clear(Color.Black);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.TargetRenderingManager.DrawFullscreen((Texture) this.newFrameBuffer.Target);
      }
      else if (!this.TargetRenderingManager.IsHooked(this.newFrameBuffer.Target))
      {
        this.TargetRenderingManager.ScheduleHook(this.DrawOrder, this.newFrameBuffer.Target);
      }
      else
      {
        GraphicsDeviceExtensions.GetDssCombiner(this.GraphicsDevice).StencilEnable = false;
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Opaque);
        this.TargetRenderingManager.Resolve(this.newFrameBuffer.Target, true);
        this.burnInEffect.NewFrameBuffer = (Texture) this.newFrameBuffer.Target;
        this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        RenderTargetHandle handle = this.TargetRenderingManager.TakeTarget();
        this.GraphicsDevice.SetRenderTarget(handle.Target);
        this.TargetRenderingManager.DrawFullscreen((BaseEffect) this.burnInEffect);
        this.TargetRenderingManager.ReturnTarget(handle);
        this.GraphicsDevice.SetRenderTarget(this.ownedHandle.Target);
        this.GraphicsDevice.Clear(Color.Black);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.TargetRenderingManager.DrawFullscreen((Texture) handle.Target);
        this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
        this.oldFrameBuffer = (Texture2D) this.ownedHandle.Target;
        this.burnInEffect.OldFrameBuffer = (Texture) this.oldFrameBuffer;
        this.GraphicsDevice.Clear(Color.Black);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.TargetRenderingManager.DrawFullscreen((Texture) this.newFrameBuffer.Target);
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Maximum);
        this.TargetRenderingManager.DrawFullscreen((Texture) this.oldFrameBuffer);
        GraphicsDeviceExtensions.GetDssCombiner(this.GraphicsDevice).StencilEnable = true;
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      }
    }
  }
}
