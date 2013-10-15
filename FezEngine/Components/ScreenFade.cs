// Type: FezEngine.Components.ScreenFade
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezEngine.Components
{
  public class ScreenFade : DrawableGameComponent
  {
    public Action Faded;
    public Action ScreenCaptured;
    private Texture capturedScreen;
    private RenderTargetHandle RtHandle;
    private TimeSpan Elapsed;

    public Color FromColor { get; set; }

    public Color ToColor { get; set; }

    public float Duration { get; set; }

    public bool EaseOut { get; set; }

    public EasingType EasingType { get; set; }

    public bool IsDisposed { get; set; }

    public Func<bool> WaitUntil { private get; set; }

    public bool CaptureScreen { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    public ScreenFade(Game game)
      : base(game)
    {
      this.DrawOrder = 1000;
      this.EasingType = EasingType.Cubic;
    }

    public override void Initialize()
    {
      base.Initialize();
      if (!this.CaptureScreen)
        return;
      this.RtHandle = this.TargetRenderer.TakeTarget();
      this.TargetRenderer.ScheduleHook(this.DrawOrder, this.RtHandle.Target);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.RtHandle != null)
      {
        this.TargetRenderer.ReturnTarget(this.RtHandle);
        this.RtHandle = (RenderTargetHandle) null;
      }
      this.Faded = (Action) null;
      this.ScreenCaptured = (Action) null;
      this.WaitUntil = (Func<bool>) null;
      this.capturedScreen = (Texture) null;
      this.IsDisposed = true;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.RtHandle != null && this.TargetRenderer.IsHooked(this.RtHandle.Target))
      {
        this.TargetRenderer.Resolve(this.RtHandle.Target, false);
        this.capturedScreen = (Texture) this.RtHandle.Target;
        if (this.ScreenCaptured != null)
          this.ScreenCaptured();
      }
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      if (this.capturedScreen != null)
      {
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.TargetRenderer.DrawFullscreen(this.capturedScreen);
      }
      this.Elapsed += gameTime.ElapsedGameTime;
      float num = (float) this.Elapsed.TotalSeconds / this.Duration;
      float amount = FezMath.Saturate(this.EaseOut ? Easing.EaseOut((double) num, this.EasingType) : Easing.EaseIn((double) num, this.EasingType));
      if ((double) amount == 1.0 && (this.WaitUntil == null || this.WaitUntil()))
      {
        if (this.Faded != null)
        {
          this.Faded();
          this.Faded = (Action) null;
        }
        this.WaitUntil = (Func<bool>) null;
        ServiceHelper.RemoveComponent<ScreenFade>(this);
      }
      this.TargetRenderer.DrawFullscreen(Color.Lerp(this.FromColor, this.ToColor, amount));
    }
  }
}
