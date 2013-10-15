// Type: FezGame.Components.GameLevelHost
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
  public class GameLevelHost : LevelHost
  {
    private CombineEffect combineEffect;
    private bool needsReset;
    private RenderTargetHandle backgroundRth;
    private RenderTargetHandle rightRT;
    private RenderTargetHandle leftRT;
    public static GameLevelHost Instance;

    [ServiceDependency]
    public IPlaneParticleSystems PlanePS { private get; set; }

    [ServiceDependency]
    public ITrixelParticleSystems TrixelPS { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    public GameLevelHost(Game game)
      : base(game)
    {
      GameLevelHost.Instance = this;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.combineEffect = new CombineEffect()
      {
        RedGamma = 1f
      };
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        if (this.LevelManager.WaterType == LiquidType.Lava)
        {
          this.combineEffect.LeftFilter = new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
          this.combineEffect.RightFilter = new Matrix(0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
        }
        else
        {
          this.combineEffect.LeftFilter = new Matrix(0.2125f, 0.7154f, 0.0721f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
          this.combineEffect.RightFilter = new Matrix(0.0f, 0.0f, 0.0f, 0.0f, 0.2125f, 0.7154f, 0.0721f, 0.0f, 0.2125f, 0.7154f, 0.0721f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
        }
      });
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.SkipRendering)
      {
        if (this.backgroundRth == null || !this.TargetRenderer.IsHooked(this.backgroundRth.Target))
          return;
        this.TargetRenderer.Resolve(this.backgroundRth.Target, true);
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Opaque);
        this.combineEffect.LeftTexture = this.combineEffect.RightTexture = (Texture2D) this.backgroundRth.Target;
        this.TargetRenderer.DrawFullscreen((BaseEffect) this.combineEffect);
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      }
      else if (!this.GameState.StereoMode)
      {
        if (this.rightRT != null || this.leftRT != null)
        {
          this.TargetRenderer.ReturnTarget(this.leftRT);
          this.TargetRenderer.ReturnTarget(this.rightRT);
          this.leftRT = this.rightRT = (RenderTargetHandle) null;
        }
        if (this.backgroundRth != null)
        {
          this.TargetRenderer.Resolve(this.backgroundRth.Target, false);
          GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Opaque);
          this.TargetRenderer.DrawFullscreen((Texture) this.backgroundRth.Target, Matrix.Identity);
          this.TargetRenderer.ReturnTarget(this.backgroundRth);
          GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
        }
        this.backgroundRth = (RenderTargetHandle) null;
        if (this.needsReset)
          BaseEffect.EyeSign = Vector3.Zero;
        this.DoDraw();
      }
      else
      {
        if (this.rightRT == null || this.leftRT == null)
        {
          this.rightRT = this.TargetRenderer.TakeTarget();
          this.leftRT = this.TargetRenderer.TakeTarget();
        }
        if (this.backgroundRth == null)
        {
          this.backgroundRth = this.TargetRenderer.TakeTarget();
          this.TargetRenderer.ScheduleHook(this.DrawOrder, this.backgroundRth.Target);
        }
        else
        {
          this.needsReset = true;
          this.DoStereo(this.LevelManager.Size / 2f, this.LevelManager.Size, new Action(this.DoFullDraw), (Texture) null);
        }
      }
    }

    private void DoFullDraw()
    {
      this.DoDraw();
      GomezHost.Instance.DoDraw();
      this.PlanePS.ForceDraw();
      this.TrixelPS.ForceDraw();
      if (LiquidHost.Instance.Visible)
        LiquidHost.Instance.DoDraw(false);
      BlackHolesHost.Instance.DoDraw();
    }

    public void DoStereo(Vector3 center, Vector3 size, Action drawMethod, Texture backgroundTexture = null)
    {
      if (backgroundTexture == null)
      {
        this.TargetRenderer.Resolve(this.backgroundRth.Target, true);
        backgroundTexture = (Texture) this.backgroundRth.Target;
      }
      if (this.GameState.FarawaySettings.InTransition && this.GameState.FarawaySettings.SkyRt != null && this.GraphicsDevice.GetRenderTargets().Length > 0)
      {
        this.DoStereoTransition(center, size, drawMethod);
      }
      else
      {
        float num = (size * this.CameraManager.InverseView.Forward).Length() * 1.5f;
        BaseEffect.LevelCenter = center;
        RenderTargetBinding[] renderTargets = this.GraphicsDevice.GetRenderTargets();
        RenderTarget2D renderTarget = renderTargets.Length == 0 ? (RenderTarget2D) null : renderTargets[0].RenderTarget as RenderTarget2D;
        this.GraphicsDevice.SetRenderTarget(this.leftRT.Target);
        this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, Color.Black, 1f, 0);
        GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
        BaseEffect.EyeSign = -1f * this.CameraManager.InverseView.Right / num;
        this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.Sky));
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Opaque);
        this.TargetRenderer.DrawFullscreen(backgroundTexture, new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.005f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f));
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
        drawMethod();
        this.GraphicsDevice.SetRenderTarget(this.rightRT.Target);
        this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, Color.Black, 1f, 0);
        GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
        BaseEffect.EyeSign = this.CameraManager.InverseView.Right / num;
        this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.Sky));
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Opaque);
        this.TargetRenderer.DrawFullscreen(backgroundTexture, new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, -0.005f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f));
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
        drawMethod();
        this.GraphicsDevice.SetRenderTarget(renderTarget);
        GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Opaque);
        this.combineEffect.LeftTexture = (Texture2D) this.leftRT.Target;
        this.combineEffect.RightTexture = (Texture2D) this.rightRT.Target;
        this.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1f, 0);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        this.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
        this.TargetRenderer.DrawFullscreen((BaseEffect) this.combineEffect);
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      }
    }

    private void DoStereoTransition(Vector3 center, Vector3 size, Action drawMethod)
    {
      float num = (size * this.CameraManager.InverseView.Forward).Length() * 1.5f;
      BaseEffect.LevelCenter = center;
      RenderTarget2D renderTarget = this.GraphicsDevice.GetRenderTargets()[0].RenderTarget as RenderTarget2D;
      this.GraphicsDevice.SetRenderTarget(this.leftRT.Target);
      this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, ColorEx.TransparentBlack, 1f, 0);
      GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
      GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      drawMethod();
      this.GraphicsDevice.SetRenderTarget(this.rightRT.Target);
      this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, ColorEx.TransparentBlack, 1f, 0);
      GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
      BaseEffect.EyeSign = this.CameraManager.InverseView.Right / num;
      GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      drawMethod();
      this.GraphicsDevice.SetRenderTarget(renderTarget);
      this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, ColorEx.TransparentBlack, 1f, 0);
      this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
      this.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
      GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      this.combineEffect.LeftTexture = (Texture2D) this.leftRT.Target;
      this.combineEffect.RightTexture = (Texture2D) this.rightRT.Target;
      this.TargetRenderer.DrawFullscreen((BaseEffect) this.combineEffect);
    }
  }
}
