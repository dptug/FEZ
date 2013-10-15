// Type: OpenTK.Platform.X11.X11Factory
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using System;

namespace OpenTK.Platform.X11
{
  internal class X11Factory : IPlatformFactory
  {
    public X11Factory()
    {
      Functions.XInitThreads();
    }

    public virtual INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
    {
      return (INativeWindow) new X11GLNative(x, y, width, height, title, mode, options, device);
    }

    public virtual IDisplayDeviceDriver CreateDisplayDeviceDriver()
    {
      return (IDisplayDeviceDriver) new X11DisplayDevice();
    }

    public virtual IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      return (IGraphicsContext) new X11GLContext(mode, window, shareContext, directRendering, major, minor, flags);
    }

    public virtual IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      return (IGraphicsContext) new X11GLContext(handle, window, shareContext, directRendering, major, minor, flags);
    }

    public virtual GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
    {
      return (GraphicsContext.GetCurrentContextDelegate) (() => new ContextHandle(Glx.GetCurrentContext()));
    }

    public virtual IGraphicsMode CreateGraphicsMode()
    {
      return (IGraphicsMode) new X11GraphicsMode();
    }

    public virtual IKeyboardDriver2 CreateKeyboardDriver()
    {
      return (IKeyboardDriver2) new X11Keyboard();
    }

    public virtual IMouseDriver2 CreateMouseDriver()
    {
      if (XI2Mouse.IsSupported(IntPtr.Zero))
        return (IMouseDriver2) new XI2Mouse();
      else
        return (IMouseDriver2) new X11Mouse();
    }
  }
}
