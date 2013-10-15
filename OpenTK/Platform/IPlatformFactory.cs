// Type: OpenTK.Platform.IPlatformFactory
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace OpenTK.Platform
{
  internal interface IPlatformFactory
  {
    INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device);

    IDisplayDeviceDriver CreateDisplayDeviceDriver();

    IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags);

    IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags);

    GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext();

    IGraphicsMode CreateGraphicsMode();

    IKeyboardDriver2 CreateKeyboardDriver();

    IMouseDriver2 CreateMouseDriver();
  }
}
