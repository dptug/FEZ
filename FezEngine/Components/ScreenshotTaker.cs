// Type: FezEngine.Components.ScreenshotTaker
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace FezEngine.Components
{
  public class ScreenshotTaker : DrawableGameComponent
  {
    private int counter;
    private bool screenshotScheduled;
    private RenderTargetHandle rt;

    [ServiceDependency]
    public IKeyboardStateManager KeyboardProvider { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TRM { private get; set; }

    public ScreenshotTaker(Game game)
      : base(game)
    {
      this.DrawOrder = (int) short.MaxValue;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.KeyboardProvider.RegisterKey(Keys.F2);
    }

    public override void Update(GameTime gameTime)
    {
      ScreenshotTaker screenshotTaker = this;
      int num = screenshotTaker.screenshotScheduled | this.KeyboardProvider.GetKeyState(Keys.F2) == FezButtonState.Pressed ? 1 : 0;
      screenshotTaker.screenshotScheduled = num != 0;
      if (!this.screenshotScheduled)
        return;
      this.rt = this.TRM.TakeTarget();
      this.TRM.ScheduleHook(this.DrawOrder, this.rt.Target);
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.screenshotScheduled || this.rt == null || !this.TRM.IsHooked(this.rt.Target))
        return;
      this.TRM.Resolve(this.rt.Target, false);
      using (FileStream fileStream = new FileStream(string.Format("C:\\Screenshot_{0:000}.png", (object) this.counter++), FileMode.Create))
        this.rt.Target.SaveAsPng((Stream) fileStream, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
      this.TRM.ReturnTarget(this.rt);
      this.rt = (RenderTargetHandle) null;
      this.screenshotScheduled = false;
    }
  }
}
