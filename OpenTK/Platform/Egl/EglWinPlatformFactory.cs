// Type: OpenTK.Platform.Egl.EglWinPlatformFactory
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.Windows;
using System;

namespace OpenTK.Platform.Egl
{
  internal class EglWinPlatformFactory : WinFactory
  {
    public override IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      WinWindowInfo winWindowInfo = (WinWindowInfo) window;
      IntPtr display = this.GetDisplay(winWindowInfo.DeviceContext);
      EglWindowInfo window1 = new EglWindowInfo(winWindowInfo.WindowHandle, display);
      return (IGraphicsContext) new EglContext(mode, window1, shareContext, major, minor, flags);
    }

    public override IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      WinWindowInfo winWindowInfo = (WinWindowInfo) window;
      IntPtr display = this.GetDisplay(winWindowInfo.DeviceContext);
      EglWindowInfo window1 = new EglWindowInfo(winWindowInfo.WindowHandle, display);
      return (IGraphicsContext) new EglContext(handle, window1, shareContext, major, minor, flags);
    }

    public override GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
    {
      return (GraphicsContext.GetCurrentContextDelegate) (() => new ContextHandle(Egl.GetCurrentContext()));
    }

    public override IGraphicsMode CreateGraphicsMode()
    {
      return (IGraphicsMode) new EglGraphicsMode();
    }

    private IntPtr GetDisplay(IntPtr dc)
    {
      IntPtr display = Egl.GetDisplay(dc);
      if (display == IntPtr.Zero)
        display = Egl.GetDisplay(IntPtr.Zero);
      return display;
    }
  }
}
