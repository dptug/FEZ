// Type: OpenTK.Platform.Factory
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform.Egl;
using OpenTK.Platform.MacOS;
using OpenTK.Platform.Windows;
using OpenTK.Platform.X11;
using System;

namespace OpenTK.Platform
{
  internal sealed class Factory : IPlatformFactory
  {
    private static IPlatformFactory default_implementation;
    private static IPlatformFactory embedded_implementation;

    public static IPlatformFactory Default
    {
      get
      {
        return Factory.default_implementation;
      }
      private set
      {
        Factory.default_implementation = value;
      }
    }

    public static IPlatformFactory Embedded
    {
      get
      {
        return Factory.embedded_implementation;
      }
      private set
      {
        Factory.embedded_implementation = value;
      }
    }

    static Factory()
    {
      Factory.Default = !Configuration.RunningOnWindows ? (!Configuration.RunningOnMacOS ? (!Configuration.RunningOnX11 ? (IPlatformFactory) new Factory.UnsupportedPlatform() : (IPlatformFactory) new X11Factory()) : (IPlatformFactory) new MacOSFactory()) : (IPlatformFactory) new WinFactory();
      Factory.Embedded = !Egl.IsSupported ? (IPlatformFactory) new Factory.UnsupportedPlatform() : (!Configuration.RunningOnWindows ? (!Configuration.RunningOnMacOS ? (!Configuration.RunningOnX11 ? (IPlatformFactory) new Factory.UnsupportedPlatform() : (IPlatformFactory) new EglX11PlatformFactory()) : (IPlatformFactory) new EglMacPlatformFactory()) : (IPlatformFactory) new EglWinPlatformFactory());
      if (!(Factory.Default is Factory.UnsupportedPlatform) || Factory.Embedded is Factory.UnsupportedPlatform)
        return;
      Factory.Default = Factory.Embedded;
    }

    public INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
    {
      return Factory.default_implementation.CreateNativeWindow(x, y, width, height, title, mode, options, device);
    }

    public IDisplayDeviceDriver CreateDisplayDeviceDriver()
    {
      return Factory.default_implementation.CreateDisplayDeviceDriver();
    }

    public IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      return Factory.default_implementation.CreateGLContext(mode, window, shareContext, directRendering, major, minor, flags);
    }

    public IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
    {
      return Factory.default_implementation.CreateGLContext(handle, window, shareContext, directRendering, major, minor, flags);
    }

    public GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
    {
      return Factory.default_implementation.CreateGetCurrentGraphicsContext();
    }

    public IGraphicsMode CreateGraphicsMode()
    {
      return Factory.default_implementation.CreateGraphicsMode();
    }

    public IKeyboardDriver2 CreateKeyboardDriver()
    {
      return Factory.default_implementation.CreateKeyboardDriver();
    }

    public IMouseDriver2 CreateMouseDriver()
    {
      return Factory.default_implementation.CreateMouseDriver();
    }

    private class UnsupportedPlatform : IPlatformFactory
    {
      private static readonly string error_string = "Please, refer to http://www.opentk.com for more information.";

      static UnsupportedPlatform()
      {
      }

      public INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
      {
        throw new PlatformNotSupportedException(Factory.UnsupportedPlatform.error_string);
      }

      public IDisplayDeviceDriver CreateDisplayDeviceDriver()
      {
        throw new PlatformNotSupportedException(Factory.UnsupportedPlatform.error_string);
      }

      public IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
      {
        throw new PlatformNotSupportedException(Factory.UnsupportedPlatform.error_string);
      }

      public IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
      {
        throw new PlatformNotSupportedException(Factory.UnsupportedPlatform.error_string);
      }

      public IGraphicsContext CreateESContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, int major, int minor, GraphicsContextFlags flags)
      {
        throw new PlatformNotSupportedException(Factory.UnsupportedPlatform.error_string);
      }

      public GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
      {
        throw new PlatformNotSupportedException(Factory.UnsupportedPlatform.error_string);
      }

      public IGraphicsMode CreateGraphicsMode()
      {
        throw new PlatformNotSupportedException(Factory.UnsupportedPlatform.error_string);
      }

      public IKeyboardDriver2 CreateKeyboardDriver()
      {
        throw new PlatformNotSupportedException(Factory.UnsupportedPlatform.error_string);
      }

      public IMouseDriver2 CreateMouseDriver()
      {
        throw new PlatformNotSupportedException(Factory.UnsupportedPlatform.error_string);
      }
    }
  }
}
