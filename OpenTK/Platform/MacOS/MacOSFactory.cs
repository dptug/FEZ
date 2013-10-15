// Type: OpenTK.Platform.MacOS.MacOSFactory
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;

namespace OpenTK.Platform.MacOS
{
  internal class MacOSFactory : IPlatformFactory
  {
    private readonly IInputDriver2 InputDriver = (IInputDriver2) new HIDInput();

    public virtual INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
    {
      return (INativeWindow) new CarbonGLNative(x, y, width, height, title, mode, options, device);
    }

    public virtual IDisplayDeviceDriver CreateDisplayDeviceDriver()
    {
      return (IDisplayDeviceDriver) new QuartzDisplayDeviceDriver();
    }

    public virtual IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      return (IGraphicsContext) new AglContext(mode, window, shareContext);
    }

    public virtual IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      return (IGraphicsContext) new AglContext(handle, window, shareContext);
    }

    public virtual GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
    {
      return (GraphicsContext.GetCurrentContextDelegate) (() => new ContextHandle(Agl.aglGetCurrentContext()));
    }

    public virtual IGraphicsMode CreateGraphicsMode()
    {
      return (IGraphicsMode) new MacOSGraphicsMode();
    }

    public virtual IKeyboardDriver2 CreateKeyboardDriver()
    {
      return this.InputDriver.KeyboardDriver;
    }

    public virtual IMouseDriver2 CreateMouseDriver()
    {
      return this.InputDriver.MouseDriver;
    }
  }
}
