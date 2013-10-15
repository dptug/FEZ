// Type: OpenTK.Platform.INativeGLWindow
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Drawing;

namespace OpenTK.Platform
{
  [Obsolete]
  internal interface INativeGLWindow : IDisposable
  {
    bool Exists { get; }

    IWindowInfo WindowInfo { get; }

    string Title { get; set; }

    bool Visible { get; set; }

    bool IsIdle { get; }

    IInputDriver InputDriver { get; }

    WindowState WindowState { get; set; }

    WindowBorder WindowBorder { get; set; }

    event CreateEvent Create;

    event DestroyEvent Destroy;

    void CreateWindow(int width, int height, GraphicsMode mode, int major, int minor, GraphicsContextFlags flags, out IGraphicsContext context);

    void DestroyWindow();

    void ProcessEvents();

    Point PointToClient(Point point);

    Point PointToScreen(Point point);
  }
}
