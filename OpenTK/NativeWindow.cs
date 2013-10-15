// Type: OpenTK.NativeWindow
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenTK
{
  public class NativeWindow : INativeWindow, IDisposable
  {
    private bool cursor_visible = true;
    private bool previous_cursor_visible = true;
    private readonly GameWindowFlags options;
    private readonly DisplayDevice device;
    private readonly INativeWindow implementation;
    private bool disposed;
    private bool events;

    public Rectangle Bounds
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Bounds;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.Bounds = value;
      }
    }

    public Rectangle ClientRectangle
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.ClientRectangle;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.ClientRectangle = value;
      }
    }

    public Size ClientSize
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.ClientSize;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.ClientSize = value;
      }
    }

    public bool Exists
    {
      get
      {
        if (!this.IsDisposed)
          return this.implementation.Exists;
        else
          return false;
      }
    }

    public bool Focused
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Focused;
      }
    }

    public int Height
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Height;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.Height = value;
      }
    }

    public Icon Icon
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Icon;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.Icon = value;
      }
    }

    [Obsolete]
    public IInputDriver InputDriver
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.InputDriver;
      }
    }

    public Point Location
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Location;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.Location = value;
      }
    }

    public Size Size
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Size;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.Size = value;
      }
    }

    public string Title
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Title;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.Title = value;
      }
    }

    public bool Visible
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Visible;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.Visible = value;
      }
    }

    public int Width
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Width;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.Width = value;
      }
    }

    public WindowBorder WindowBorder
    {
      get
      {
        return this.implementation.WindowBorder;
      }
      set
      {
        this.implementation.WindowBorder = value;
      }
    }

    public IWindowInfo WindowInfo
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.WindowInfo;
      }
    }

    public virtual WindowState WindowState
    {
      get
      {
        return this.implementation.WindowState;
      }
      set
      {
        this.implementation.WindowState = value;
      }
    }

    public int X
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.X;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.X = value;
      }
    }

    public int Y
    {
      get
      {
        this.EnsureUndisposed();
        return this.implementation.Y;
      }
      set
      {
        this.EnsureUndisposed();
        this.implementation.Y = value;
      }
    }

    public bool CursorVisible
    {
      get
      {
        return this.cursor_visible;
      }
      set
      {
        this.cursor_visible = value;
        this.implementation.CursorVisible = value;
      }
    }

    protected bool IsDisposed
    {
      get
      {
        return this.disposed;
      }
      set
      {
        this.disposed = value;
      }
    }

    private bool Events
    {
      set
      {
        if (value)
        {
          if (this.events)
            throw new InvalidOperationException("Event propagation is already enabled.");
          this.implementation.Closed += new EventHandler<EventArgs>(this.OnClosedInternal);
          this.implementation.Closing += new EventHandler<CancelEventArgs>(this.OnClosingInternal);
          this.implementation.Disposed += new EventHandler<EventArgs>(this.OnDisposedInternal);
          this.implementation.FocusedChanged += new EventHandler<EventArgs>(this.OnFocusedChangedInternal);
          this.implementation.IconChanged += new EventHandler<EventArgs>(this.OnIconChangedInternal);
          this.implementation.KeyPress += new EventHandler<KeyPressEventArgs>(this.OnKeyPressInternal);
          this.implementation.MouseEnter += new EventHandler<EventArgs>(this.OnMouseEnterInternal);
          this.implementation.MouseLeave += new EventHandler<EventArgs>(this.OnMouseLeaveInternal);
          this.implementation.Move += new EventHandler<EventArgs>(this.OnMoveInternal);
          this.implementation.Resize += new EventHandler<EventArgs>(this.OnResizeInternal);
          this.implementation.TitleChanged += new EventHandler<EventArgs>(this.OnTitleChangedInternal);
          this.implementation.VisibleChanged += new EventHandler<EventArgs>(this.OnVisibleChangedInternal);
          this.implementation.WindowBorderChanged += new EventHandler<EventArgs>(this.OnWindowBorderChangedInternal);
          this.implementation.WindowStateChanged += new EventHandler<EventArgs>(this.OnWindowStateChangedInternal);
          this.events = true;
        }
        else
        {
          if (!this.events)
            throw new InvalidOperationException("Event propagation is already disabled.");
          this.implementation.Closed -= new EventHandler<EventArgs>(this.OnClosedInternal);
          this.implementation.Closing -= new EventHandler<CancelEventArgs>(this.OnClosingInternal);
          this.implementation.Disposed -= new EventHandler<EventArgs>(this.OnDisposedInternal);
          this.implementation.FocusedChanged -= new EventHandler<EventArgs>(this.OnFocusedChangedInternal);
          this.implementation.IconChanged -= new EventHandler<EventArgs>(this.OnIconChangedInternal);
          this.implementation.KeyPress -= new EventHandler<KeyPressEventArgs>(this.OnKeyPressInternal);
          this.implementation.MouseEnter -= new EventHandler<EventArgs>(this.OnMouseEnterInternal);
          this.implementation.MouseLeave -= new EventHandler<EventArgs>(this.OnMouseLeaveInternal);
          this.implementation.Move -= new EventHandler<EventArgs>(this.OnMoveInternal);
          this.implementation.Resize -= new EventHandler<EventArgs>(this.OnResizeInternal);
          this.implementation.TitleChanged -= new EventHandler<EventArgs>(this.OnTitleChangedInternal);
          this.implementation.VisibleChanged -= new EventHandler<EventArgs>(this.OnVisibleChangedInternal);
          this.implementation.WindowBorderChanged -= new EventHandler<EventArgs>(this.OnWindowBorderChangedInternal);
          this.implementation.WindowStateChanged -= new EventHandler<EventArgs>(this.OnWindowStateChangedInternal);
          this.events = false;
        }
      }
    }

    public event EventHandler<EventArgs> Closed = delegate {};

    public event EventHandler<CancelEventArgs> Closing = delegate {};

    public event EventHandler<EventArgs> Disposed = delegate {};

    public event EventHandler<EventArgs> FocusedChanged = delegate {};

    public event EventHandler<EventArgs> IconChanged = delegate {};

    public event EventHandler<KeyboardKeyEventArgs> KeyDown = delegate {};

    public event EventHandler<KeyPressEventArgs> KeyPress = delegate {};

    public event EventHandler<KeyboardKeyEventArgs> KeyUp = delegate {};

    public event EventHandler<EventArgs> Move = delegate {};

    public event EventHandler<EventArgs> MouseEnter = delegate {};

    public event EventHandler<EventArgs> MouseLeave = delegate {};

    public event EventHandler<EventArgs> Resize = delegate {};

    public event EventHandler<EventArgs> TitleChanged = delegate {};

    public event EventHandler<EventArgs> VisibleChanged = delegate {};

    public event EventHandler<EventArgs> WindowBorderChanged = delegate {};

    public event EventHandler<EventArgs> WindowStateChanged = delegate {};

    public NativeWindow()
      : this(640, 480, "OpenTK Native Window", GameWindowFlags.Default, GraphicsMode.Default, DisplayDevice.Default)
    {
    }

    public NativeWindow(int width, int height, string title, GameWindowFlags options, GraphicsMode mode, DisplayDevice device)
      : this(device.Bounds.Left + (device.Bounds.Width - width) / 2, device.Bounds.Top + (device.Bounds.Height - height) / 2, width, height, title, options, mode, device)
    {
    }

    public NativeWindow(int x, int y, int width, int height, string title, GameWindowFlags options, GraphicsMode mode, DisplayDevice device)
    {
      if (width < 1)
        throw new ArgumentOutOfRangeException("width", "Must be greater than zero.");
      if (height < 1)
        throw new ArgumentOutOfRangeException("height", "Must be greater than zero.");
      if (mode == null)
        throw new ArgumentNullException("mode");
      if (device == null)
        throw new ArgumentNullException("device");
      this.options = options;
      this.device = device;
      this.implementation = Factory.Default.CreateNativeWindow(x, y, width, height, title, mode, options, this.device);
      if ((options & GameWindowFlags.Fullscreen) == GameWindowFlags.Default)
        return;
      this.device.ChangeResolution(width, height, mode.ColorFormat.BitsPerPixel, 0.0f);
      this.WindowState = WindowState.Fullscreen;
    }

    public void Close()
    {
      this.EnsureUndisposed();
      this.implementation.Close();
    }

    public Point PointToClient(Point point)
    {
      return this.implementation.PointToClient(point);
    }

    public Point PointToScreen(Point point)
    {
      Point point1 = this.PointToClient(Point.Empty);
      point.X -= point1.X;
      point.Y -= point1.Y;
      return point;
    }

    public void ProcessEvents()
    {
      this.ProcessEvents(false);
    }

    public virtual void Dispose()
    {
      if (this.IsDisposed)
        return;
      if ((this.options & GameWindowFlags.Fullscreen) != GameWindowFlags.Default)
        this.device.RestoreResolution();
      this.implementation.Dispose();
      GC.SuppressFinalize((object) this);
      this.IsDisposed = true;
    }

    protected void EnsureUndisposed()
    {
      if (this.IsDisposed)
        throw new ObjectDisposedException(this.GetType().Name);
    }

    protected virtual void OnClosed(EventArgs e)
    {
      this.Closed((object) this, e);
    }

    protected virtual void OnClosing(CancelEventArgs e)
    {
      this.Closing((object) this, e);
    }

    protected virtual void OnDisposed(EventArgs e)
    {
      this.Disposed((object) this, e);
    }

    protected virtual void OnFocusedChanged(EventArgs e)
    {
      if (!this.Focused)
      {
        this.previous_cursor_visible = this.CursorVisible;
        this.CursorVisible = true;
      }
      else if (!this.previous_cursor_visible)
      {
        this.previous_cursor_visible = true;
        this.CursorVisible = false;
      }
      this.FocusedChanged((object) this, e);
    }

    protected virtual void OnIconChanged(EventArgs e)
    {
      this.IconChanged((object) this, e);
    }

    protected virtual void OnKeyDown(KeyboardKeyEventArgs e)
    {
      this.KeyDown((object) this, e);
    }

    protected virtual void OnKeyPress(KeyPressEventArgs e)
    {
      this.KeyPress((object) this, e);
    }

    protected virtual void OnKeyUp(KeyboardKeyEventArgs e)
    {
      this.KeyUp((object) this, e);
    }

    protected virtual void OnMove(EventArgs e)
    {
      this.Move((object) this, e);
    }

    protected virtual void OnMouseEnter(EventArgs e)
    {
      this.MouseEnter((object) this, e);
    }

    protected virtual void OnMouseLeave(EventArgs e)
    {
      this.MouseLeave((object) this, e);
    }

    protected virtual void OnResize(EventArgs e)
    {
      this.Resize((object) this, e);
    }

    protected virtual void OnTitleChanged(EventArgs e)
    {
      this.TitleChanged((object) this, e);
    }

    protected virtual void OnVisibleChanged(EventArgs e)
    {
      this.VisibleChanged((object) this, e);
    }

    protected virtual void OnWindowBorderChanged(EventArgs e)
    {
      this.WindowBorderChanged((object) this, e);
    }

    protected virtual void OnWindowStateChanged(EventArgs e)
    {
      this.WindowStateChanged((object) this, e);
    }

    protected void ProcessEvents(bool retainEvents)
    {
      this.EnsureUndisposed();
      if (!retainEvents && !this.events)
        this.Events = true;
      this.implementation.ProcessEvents();
    }

    private void OnClosedInternal(object sender, EventArgs e)
    {
      this.OnClosed(e);
      this.Events = false;
    }

    private void OnClosingInternal(object sender, CancelEventArgs e)
    {
      this.OnClosing(e);
    }

    private void OnDisposedInternal(object sender, EventArgs e)
    {
      this.OnDisposed(e);
    }

    private void OnFocusedChangedInternal(object sender, EventArgs e)
    {
      this.OnFocusedChanged(e);
    }

    private void OnIconChangedInternal(object sender, EventArgs e)
    {
      this.OnIconChanged(e);
    }

    private void OnKeyPressInternal(object sender, KeyPressEventArgs e)
    {
      this.OnKeyPress(e);
    }

    private void OnMouseEnterInternal(object sender, EventArgs e)
    {
      this.OnMouseEnter(e);
    }

    private void OnMouseLeaveInternal(object sender, EventArgs e)
    {
      this.OnMouseLeave(e);
    }

    private void OnMoveInternal(object sender, EventArgs e)
    {
      this.OnMove(e);
    }

    private void OnResizeInternal(object sender, EventArgs e)
    {
      this.OnResize(e);
    }

    private void OnTitleChangedInternal(object sender, EventArgs e)
    {
      this.OnTitleChanged(e);
    }

    private void OnVisibleChangedInternal(object sender, EventArgs e)
    {
      this.OnVisibleChanged(e);
    }

    private void OnWindowBorderChangedInternal(object sender, EventArgs e)
    {
      this.OnWindowBorderChanged(e);
    }

    private void OnWindowStateChangedInternal(object sender, EventArgs e)
    {
      this.OnWindowStateChanged(e);
    }
  }
}
