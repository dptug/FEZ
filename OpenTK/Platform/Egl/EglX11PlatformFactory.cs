// Type: OpenTK.Platform.Egl.EglX11PlatformFactory
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.X11;

namespace OpenTK.Platform.Egl
{
  internal class EglX11PlatformFactory : X11Factory
  {
    public override IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      X11WindowInfo x11WindowInfo = (X11WindowInfo) window;
      EglWindowInfo window1 = new EglWindowInfo(x11WindowInfo.WindowHandle, Egl.GetDisplay(x11WindowInfo.Display));
      return (IGraphicsContext) new EglContext(mode, window1, shareContext, major, minor, flags);
    }

    public override IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      X11WindowInfo x11WindowInfo = (X11WindowInfo) window;
      EglWindowInfo window1 = new EglWindowInfo(x11WindowInfo.WindowHandle, Egl.GetDisplay(x11WindowInfo.Display));
      return (IGraphicsContext) new EglContext(handle, window1, shareContext, major, minor, flags);
    }

    public override GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
    {
      return (GraphicsContext.GetCurrentContextDelegate) (() => new ContextHandle(Egl.GetCurrentContext()));
    }
  }
}
