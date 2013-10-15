// Type: OpenTK.INativeWindow
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenTK
{
  public interface INativeWindow : IDisposable
  {
    Icon Icon { get; set; }

    string Title { get; set; }

    bool Focused { get; }

    bool Visible { get; set; }

    bool Exists { get; }

    IWindowInfo WindowInfo { get; }

    WindowState WindowState { get; set; }

    WindowBorder WindowBorder { get; set; }

    Rectangle Bounds { get; set; }

    Point Location { get; set; }

    Size Size { get; set; }

    int X { get; set; }

    int Y { get; set; }

    int Width { get; set; }

    int Height { get; set; }

    Rectangle ClientRectangle { get; set; }

    Size ClientSize { get; set; }

    [Obsolete]
    IInputDriver InputDriver { get; }

    bool CursorVisible { get; set; }

    event EventHandler<EventArgs> Move;

    event EventHandler<EventArgs> Resize;

    event EventHandler<CancelEventArgs> Closing;

    event EventHandler<EventArgs> Closed;

    event EventHandler<EventArgs> Disposed;

    event EventHandler<EventArgs> IconChanged;

    event EventHandler<EventArgs> TitleChanged;

    event EventHandler<EventArgs> VisibleChanged;

    event EventHandler<EventArgs> FocusedChanged;

    event EventHandler<EventArgs> WindowBorderChanged;

    event EventHandler<EventArgs> WindowStateChanged;

    event EventHandler<KeyboardKeyEventArgs> KeyDown;

    event EventHandler<KeyPressEventArgs> KeyPress;

    event EventHandler<KeyboardKeyEventArgs> KeyUp;

    event EventHandler<EventArgs> MouseLeave;

    event EventHandler<EventArgs> MouseEnter;

    void Close();

    void ProcessEvents();

    Point PointToClient(Point point);

    Point PointToScreen(Point point);
  }
}
