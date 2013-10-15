// Type: OpenTK.Platform.Windows.WinFactory
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using System;

namespace OpenTK.Platform.Windows
{
  internal class WinFactory : IPlatformFactory
  {
    private readonly object SyncRoot = new object();
    private IInputDriver2 inputDriver;

    private IInputDriver2 InputDriver
    {
      get
      {
        lock (this.SyncRoot)
        {
          if (this.inputDriver == null)
            this.inputDriver = Environment.OSVersion.Version.Major > 5 || Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor > 0 ? (IInputDriver2) new WinRawInput() : (IInputDriver2) new WMInput();
          return this.inputDriver;
        }
      }
    }

    public virtual INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
    {
      return (INativeWindow) new WinGLNative(x, y, width, height, title, options, device);
    }

    public virtual IDisplayDeviceDriver CreateDisplayDeviceDriver()
    {
      return (IDisplayDeviceDriver) new WinDisplayDeviceDriver();
    }

    public virtual IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      return (IGraphicsContext) new WinGLContext(mode, (WinWindowInfo) window, shareContext, major, minor, flags);
    }

    public virtual IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      return (IGraphicsContext) new WinGLContext(handle, (WinWindowInfo) window, shareContext, major, minor, flags);
    }

    public virtual GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
    {
      return (GraphicsContext.GetCurrentContextDelegate) (() => new ContextHandle(Wgl.GetCurrentContext()));
    }

    public virtual IGraphicsMode CreateGraphicsMode()
    {
      return (IGraphicsMode) new WinGraphicsMode();
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
