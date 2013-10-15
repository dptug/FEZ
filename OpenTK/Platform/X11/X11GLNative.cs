// Type: OpenTK.Platform.X11.X11GLNative
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenTK.Platform.X11
{
  internal sealed class X11GLNative : INativeWindow, IDisposable
  {
    private static readonly IntPtr _atom_remove = (IntPtr) 0;
    private static readonly IntPtr _atom_add = (IntPtr) 1;
    private static readonly IntPtr _atom_toggle = (IntPtr) 2;
    public static bool MouseWarpActive = false;
    private X11WindowInfo window = new X11WindowInfo();
    private readonly IntPtr _atom_xa_cardinal = new IntPtr(6);
    private XEvent e = new XEvent();
    private bool cursor_visible = true;
    private readonly byte[] ascii = new byte[16];
    private readonly char[] chars = new char[16];
    private readonly KeyPressEventArgs KPEventArgs = new KeyPressEventArgs(char.MinValue);
    private const int _min_width = 30;
    private const int _min_height = 30;
    private const string MOTIF_WM_ATOM = "_MOTIF_WM_HINTS";
    private const string KDE_WM_ATOM = "KWM_WIN_DECORATION";
    private const string KDE_NET_WM_ATOM = "_KDE_NET_WM_WINDOW_TYPE";
    private const string ICCM_WM_ATOM = "_NET_WM_WINDOW_TYPE";
    private const string ICON_NET_ATOM = "_NET_WM_ICON";
    private X11Input driver;
    private MouseDevice mouse;
    private IntPtr _atom_wm_destroy;
    private IntPtr _atom_net_wm_state;
    private IntPtr _atom_net_wm_state_minimized;
    private IntPtr _atom_net_wm_state_fullscreen;
    private IntPtr _atom_net_wm_state_maximized_horizontal;
    private IntPtr _atom_net_wm_state_maximized_vertical;
    private IntPtr _atom_net_wm_allowed_actions;
    private IntPtr _atom_net_wm_action_resize;
    private IntPtr _atom_net_wm_action_maximize_horizontally;
    private IntPtr _atom_net_wm_action_maximize_vertically;
    private IntPtr _atom_net_wm_icon;
    private IntPtr _atom_net_frame_extents;
    private System.Drawing.Rectangle bounds;
    private System.Drawing.Rectangle client_rectangle;
    private int border_left;
    private int border_right;
    private int border_top;
    private int border_bottom;
    private Icon icon;
    private bool has_focus;
    private bool visible;
    private bool disposed;
    private bool exists;
    private bool isExiting;
    private bool _decorations_hidden;
    private int mouse_rel_x;
    private int mouse_rel_y;
    private readonly IntPtr EmptyCursor;

    private bool IsWindowBorderResizable
    {
      get
      {
        IntPtr prop = IntPtr.Zero;
        using (new XLock(this.window.Display))
        {
          IntPtr actual_type;
          int actual_format;
          IntPtr nitems;
          IntPtr bytes_after;
          Functions.XGetWindowProperty(this.window.Display, this.window.WindowHandle, this._atom_net_wm_allowed_actions, IntPtr.Zero, new IntPtr(256), false, IntPtr.Zero, out actual_type, out actual_format, out nitems, out bytes_after, ref prop);
          if ((long) nitems > 0L)
          {
            if (prop != IntPtr.Zero)
            {
              for (int index = 0; (long) index < (long) nitems; ++index)
              {
                if (Marshal.ReadIntPtr(prop, index * IntPtr.Size) == this._atom_net_wm_action_resize)
                  return true;
              }
              Functions.XFree(prop);
            }
          }
        }
        return false;
      }
    }

    private bool IsWindowBorderHidden
    {
      get
      {
        IntPtr num = IntPtr.Zero;
        using (new XLock(this.window.Display))
        {
          if (Functions.XInternAtom(this.window.Display, "_MOTIF_WM_HINTS", true) != IntPtr.Zero && this._decorations_hidden)
            return true;
          IntPtr prop_window_return;
          Functions.XGetTransientForHint(this.window.Display, this.window.WindowHandle, out prop_window_return);
          return prop_window_return != IntPtr.Zero;
        }
      }
    }

    public System.Drawing.Rectangle Bounds
    {
      get
      {
        return this.bounds;
      }
      set
      {
        using (new XLock(this.window.Display))
          Functions.XMoveResizeWindow(this.window.Display, this.window.WindowHandle, value.X, value.Y, value.Width - this.border_left - this.border_right, value.Height - this.border_top - this.border_bottom);
        this.ProcessEvents();
      }
    }

    public Point Location
    {
      get
      {
        return this.Bounds.Location;
      }
      set
      {
        using (new XLock(this.window.Display))
          Functions.XMoveWindow(this.window.Display, this.window.WindowHandle, value.X, value.Y);
        this.ProcessEvents();
      }
    }

    public Size Size
    {
      get
      {
        return this.Bounds.Size;
      }
      set
      {
        int num1 = value.Width - this.border_left - this.border_right;
        int num2 = value.Height - this.border_top - this.border_bottom;
        int width = num1 <= 0 ? 1 : num1;
        int height = num2 <= 0 ? 1 : num2;
        using (new XLock(this.window.Display))
          Functions.XResizeWindow(this.window.Display, this.window.WindowHandle, width, height);
        this.ProcessEvents();
      }
    }

    public System.Drawing.Rectangle ClientRectangle
    {
      get
      {
        if (this.client_rectangle.Width == 0)
          this.client_rectangle.Width = 1;
        if (this.client_rectangle.Height == 0)
          this.client_rectangle.Height = 1;
        return this.client_rectangle;
      }
      set
      {
        using (new XLock(this.window.Display))
          Functions.XResizeWindow(this.window.Display, this.window.WindowHandle, value.Width, value.Height);
        this.ProcessEvents();
      }
    }

    public Size ClientSize
    {
      get
      {
        return this.ClientRectangle.Size;
      }
      set
      {
        this.ClientRectangle = new System.Drawing.Rectangle(Point.Empty, value);
      }
    }

    public int Width
    {
      get
      {
        return this.ClientSize.Width;
      }
      set
      {
        this.ClientSize = new Size(value, this.Height);
      }
    }

    public int Height
    {
      get
      {
        return this.ClientSize.Height;
      }
      set
      {
        this.ClientSize = new Size(this.Width, value);
      }
    }

    public int X
    {
      get
      {
        return this.Location.X;
      }
      set
      {
        this.Location = new Point(value, this.Y);
      }
    }

    public int Y
    {
      get
      {
        return this.Location.Y;
      }
      set
      {
        this.Location = new Point(this.X, value);
      }
    }

    public Icon Icon
    {
      get
      {
        return this.icon;
      }
      set
      {
        if (value == this.icon)
          return;
        if (value == null)
        {
          using (new XLock(this.window.Display))
          {
            Functions.XDeleteProperty(this.window.Display, this.window.WindowHandle, this._atom_net_wm_icon);
            X11GLNative.DeleteIconPixmaps(this.window.Display, this.window.WindowHandle);
          }
        }
        else
        {
          Bitmap image = value.ToBitmap();
          int nelements = image.Width * image.Height + 2;
          IntPtr[] data = new IntPtr[nelements];
          int num1 = 0;
          IntPtr[] numArray1 = data;
          int index1 = num1;
          int num2 = 1;
          int num3 = index1 + num2;
          numArray1[index1] = (IntPtr) image.Width;
          IntPtr[] numArray2 = data;
          int index2 = num3;
          int num4 = 1;
          int num5 = index2 + num4;
          numArray2[index2] = (IntPtr) image.Height;
          for (int y = 0; y < image.Height; ++y)
          {
            for (int x = 0; x < image.Width; ++x)
              data[num5++] = (IntPtr) image.GetPixel(x, y).ToArgb();
          }
          using (new XLock(this.window.Display))
            Functions.XChangeProperty(this.window.Display, this.window.WindowHandle, this._atom_net_wm_icon, this._atom_xa_cardinal, 32, PropertyMode.Replace, data, nelements);
          X11GLNative.DeleteIconPixmaps(this.window.Display, this.window.WindowHandle);
          using (new XLock(this.window.Display))
          {
            IntPtr num6 = Functions.XGetWMHints(this.window.Display, this.window.WindowHandle);
            if (num6 == IntPtr.Zero)
              num6 = Functions.XAllocWMHints();
            XWMHints wmhints = (XWMHints) Marshal.PtrToStructure(num6, typeof (XWMHints));
            wmhints.flags = new IntPtr(wmhints.flags.ToInt32() | 36);
            wmhints.icon_pixmap = Functions.CreatePixmapFromImage(this.window.Display, image);
            wmhints.icon_mask = Functions.CreateMaskFromImage(this.window.Display, image);
            Functions.XSetWMHints(this.window.Display, this.window.WindowHandle, ref wmhints);
            Functions.XFree(num6);
            Functions.XSync(this.window.Display, false);
          }
        }
        this.icon = value;
        this.IconChanged((object) this, EventArgs.Empty);
      }
    }

    public bool Focused
    {
      get
      {
        return this.has_focus;
      }
    }

    public OpenTK.WindowState WindowState
    {
      get
      {
        IntPtr prop = IntPtr.Zero;
        bool flag1 = false;
        int num1 = 0;
        bool flag2 = false;
        IntPtr nitems;
        using (new XLock(this.window.Display))
        {
          IntPtr actual_type;
          int actual_format;
          IntPtr bytes_after;
          Functions.XGetWindowProperty(this.window.Display, this.window.WindowHandle, this._atom_net_wm_state, IntPtr.Zero, new IntPtr(256), false, new IntPtr(4), out actual_type, out actual_format, out nitems, out bytes_after, ref prop);
        }
        if ((long) nitems > 0L && prop != IntPtr.Zero)
        {
          for (int index = 0; (long) index < (long) nitems; ++index)
          {
            IntPtr num2 = Marshal.ReadIntPtr(prop, index * IntPtr.Size);
            if (num2 == this._atom_net_wm_state_maximized_horizontal || num2 == this._atom_net_wm_state_maximized_vertical)
              ++num1;
            else if (num2 == this._atom_net_wm_state_minimized)
              flag2 = true;
            else if (num2 == this._atom_net_wm_state_fullscreen)
              flag1 = true;
          }
          using (new XLock(this.window.Display))
            Functions.XFree(prop);
        }
        if (flag2)
          return OpenTK.WindowState.Minimized;
        if (num1 == 2)
          return OpenTK.WindowState.Maximized;
        return flag1 ? OpenTK.WindowState.Fullscreen : OpenTK.WindowState.Normal;
      }
      set
      {
        OpenTK.WindowState windowState = this.WindowState;
        if (windowState == value)
          return;
        using (new XLock(this.window.Display))
        {
          if (windowState == OpenTK.WindowState.Minimized)
            Functions.XMapWindow(this.window.Display, this.window.WindowHandle);
          else if (windowState == OpenTK.WindowState.Fullscreen)
            Functions.SendNetWMMessage(this.window, this._atom_net_wm_state, X11GLNative._atom_remove, this._atom_net_wm_state_fullscreen, IntPtr.Zero);
          else if (windowState == OpenTK.WindowState.Maximized)
            Functions.SendNetWMMessage(this.window, this._atom_net_wm_state, X11GLNative._atom_toggle, this._atom_net_wm_state_maximized_horizontal, this._atom_net_wm_state_maximized_vertical);
          Functions.XSync(this.window.Display, false);
        }
        bool flag = false;
        WindowBorder windowBorder = this.WindowBorder;
        if (this.WindowBorder != WindowBorder.Resizable)
        {
          flag = true;
          this.WindowBorder = WindowBorder.Resizable;
        }
        using (new XLock(this.window.Display))
        {
          switch (value)
          {
            case OpenTK.WindowState.Normal:
              Functions.XRaiseWindow(this.window.Display, this.window.WindowHandle);
              break;
            case OpenTK.WindowState.Minimized:
              Functions.XIconifyWindow(this.window.Display, this.window.WindowHandle, this.window.Screen);
              break;
            case OpenTK.WindowState.Maximized:
              Functions.SendNetWMMessage(this.window, this._atom_net_wm_state, X11GLNative._atom_add, this._atom_net_wm_state_maximized_horizontal, this._atom_net_wm_state_maximized_vertical);
              Functions.XRaiseWindow(this.window.Display, this.window.WindowHandle);
              break;
            case OpenTK.WindowState.Fullscreen:
              Functions.SendNetWMMessage(this.window, this._atom_net_wm_state, X11GLNative._atom_add, this._atom_net_wm_state_fullscreen, IntPtr.Zero);
              Functions.XRaiseWindow(this.window.Display, this.window.WindowHandle);
              break;
          }
        }
        if (flag)
          this.WindowBorder = windowBorder;
        this.ProcessEvents();
      }
    }

    public WindowBorder WindowBorder
    {
      get
      {
        if (this.IsWindowBorderHidden)
          return WindowBorder.Hidden;
        return this.IsWindowBorderResizable ? WindowBorder.Resizable : WindowBorder.Fixed;
      }
      set
      {
        if (this.WindowBorder == value)
          return;
        if (this.WindowBorder == WindowBorder.Hidden)
          this.EnableWindowDecorations();
        switch (value)
        {
          case WindowBorder.Resizable:
            this.SetWindowMinMax((short) 30, (short) 30, (short) -1, (short) -1);
            break;
          case WindowBorder.Fixed:
            this.SetWindowMinMax((short) this.Width, (short) this.Height, (short) this.Width, (short) this.Height);
            break;
          case WindowBorder.Hidden:
            this.DisableWindowDecorations();
            break;
        }
        this.WindowBorderChanged((object) this, EventArgs.Empty);
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
        if (value)
        {
          using (new XLock(this.window.Display))
          {
            Functions.XUndefineCursor(this.window.Display, this.window.WindowHandle);
            this.cursor_visible = true;
          }
        }
        else
        {
          using (new XLock(this.window.Display))
          {
            Functions.XDefineCursor(this.window.Display, this.window.WindowHandle, this.EmptyCursor);
            this.cursor_visible = false;
          }
        }
      }
    }

    public IInputDriver InputDriver
    {
      get
      {
        return (IInputDriver) this.driver;
      }
    }

    public bool Exists
    {
      get
      {
        return this.exists;
      }
    }

    public bool IsIdle
    {
      get
      {
        throw new Exception("The method or operation is not implemented.");
      }
    }

    public IntPtr Handle
    {
      get
      {
        return this.window.WindowHandle;
      }
    }

    public string Title
    {
      get
      {
        IntPtr window_name = IntPtr.Zero;
        using (new XLock(this.window.Display))
          Functions.XFetchName(this.window.Display, this.window.WindowHandle, ref window_name);
        if (window_name != IntPtr.Zero)
          return Marshal.PtrToStringAnsi(window_name);
        else
          return string.Empty;
      }
      set
      {
        if (value != null && value != this.Title)
        {
          using (new XLock(this.window.Display))
            Functions.XStoreName(this.window.Display, this.window.WindowHandle, value);
        }
        this.TitleChanged((object) this, EventArgs.Empty);
      }
    }

    public bool Visible
    {
      get
      {
        return this.visible;
      }
      set
      {
        if (value && !this.visible)
        {
          using (new XLock(this.window.Display))
            Functions.XMapWindow(this.window.Display, this.window.WindowHandle);
        }
        else
        {
          if (value || !this.visible)
            return;
          using (new XLock(this.window.Display))
            Functions.XUnmapWindow(this.window.Display, this.window.WindowHandle);
        }
      }
    }

    public IWindowInfo WindowInfo
    {
      get
      {
        return (IWindowInfo) this.window;
      }
    }

    public event EventHandler<EventArgs> Move = delegate {};

    public event EventHandler<EventArgs> Resize = delegate {};

    public event EventHandler<CancelEventArgs> Closing = delegate {};

    public event EventHandler<EventArgs> Closed = delegate {};

    public event EventHandler<EventArgs> Disposed = delegate {};

    public event EventHandler<EventArgs> IconChanged = delegate {};

    public event EventHandler<EventArgs> TitleChanged = delegate {};

    public event EventHandler<EventArgs> VisibleChanged = delegate {};

    public event EventHandler<EventArgs> FocusedChanged = delegate {};

    public event EventHandler<EventArgs> WindowBorderChanged = delegate {};

    public event EventHandler<EventArgs> WindowStateChanged = delegate {};

    public event EventHandler<KeyboardKeyEventArgs> KeyDown = delegate {};

    public event EventHandler<KeyPressEventArgs> KeyPress = delegate {};

    public event EventHandler<KeyboardKeyEventArgs> KeyUp = delegate {};

    public event EventHandler<EventArgs> MouseEnter = delegate {};

    public event EventHandler<EventArgs> MouseLeave = delegate {};

    static X11GLNative()
    {
    }

    public X11GLNative(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
      : this()
    {
      if (width <= 0)
        throw new ArgumentOutOfRangeException("width", "Must be higher than zero.");
      if (height <= 0)
        throw new ArgumentOutOfRangeException("height", "Must be higher than zero.");
      XVisualInfo template = new XVisualInfo();
      using (new XLock(this.window.Display))
      {
        if (!mode.Index.HasValue)
          throw new GraphicsModeException("Invalid or unsupported GraphicsMode.");
        template.VisualID = mode.Index.Value;
        int nitems;
        this.window.VisualInfo = (XVisualInfo) Marshal.PtrToStructure(Functions.XGetVisualInfo(this.window.Display, XVisualInfoMask.ID, ref template, out nitems), typeof (XVisualInfo));
        XSetWindowAttributes attributes = new XSetWindowAttributes();
        attributes.background_pixel = IntPtr.Zero;
        attributes.border_pixel = IntPtr.Zero;
        attributes.colormap = Functions.XCreateColormap(this.window.Display, this.window.RootWindow, this.window.VisualInfo.Visual, 0);
        this.window.EventMask = EventMask.KeyPressMask | EventMask.KeyReleaseMask | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | EventMask.EnterWindowMask | EventMask.LeaveWindowMask | EventMask.PointerMotionMask | EventMask.KeymapStateMask | EventMask.ExposureMask | EventMask.StructureNotifyMask | EventMask.FocusChangeMask | EventMask.PropertyChangeMask;
        attributes.event_mask = (IntPtr) ((long) this.window.EventMask);
        uint num = 10250U;
        this.window.WindowHandle = Functions.XCreateWindow(this.window.Display, this.window.RootWindow, x, y, width, height, 0, this.window.VisualInfo.Depth, 1, this.window.VisualInfo.Visual, (UIntPtr) num, ref attributes);
        if (this.window.WindowHandle == IntPtr.Zero)
          throw new ApplicationException("XCreateWindow call failed (returned 0).");
        if (title != null)
          Functions.XStoreName(this.window.Display, this.window.WindowHandle, title);
      }
      this.SetWindowMinMax((short) 30, (short) 30, (short) -1, (short) -1);
      XSizeHints hints = new XSizeHints();
      hints.base_width = width;
      hints.base_height = height;
      hints.flags = (IntPtr) 12L;
      using (new XLock(this.window.Display))
      {
        Functions.XSetWMNormalHints(this.window.Display, this.window.WindowHandle, ref hints);
        Functions.XSetWMProtocols(this.window.Display, this.window.WindowHandle, new IntPtr[1]
        {
          this._atom_wm_destroy
        }, 1);
      }
      this.RefreshWindowBounds(ref new XEvent()
      {
        ConfigureEvent = {
          x = x,
          y = y,
          width = width,
          height = height
        }
      });
      this.driver = new X11Input((IWindowInfo) this.window);
      this.mouse = this.driver.Mouse[0];
      this.EmptyCursor = X11GLNative.CreateEmptyCursor(this.window);
      this.exists = true;
    }

    public X11GLNative()
    {
      this.window.Display = Functions.XOpenDisplay(IntPtr.Zero);
      if (this.window.Display == IntPtr.Zero)
        throw new Exception("Could not open connection to X");
      using (new XLock(this.window.Display))
      {
        this.window.Screen = Functions.XDefaultScreen(this.window.Display);
        this.window.RootWindow = Functions.XRootWindow(this.window.Display, this.window.Screen);
      }
      this.RegisterAtoms(this.window);
    }

    ~X11GLNative()
    {
      this.Dispose(false);
    }

    private void RegisterAtoms(X11WindowInfo window)
    {
      using (new XLock(window.Display))
      {
        this._atom_wm_destroy = Functions.XInternAtom(window.Display, "WM_DELETE_WINDOW", true);
        this._atom_net_wm_state = Functions.XInternAtom(window.Display, "_NET_WM_STATE", false);
        this._atom_net_wm_state_minimized = Functions.XInternAtom(window.Display, "_NET_WM_STATE_MINIMIZED", false);
        this._atom_net_wm_state_fullscreen = Functions.XInternAtom(window.Display, "_NET_WM_STATE_FULLSCREEN", false);
        this._atom_net_wm_state_maximized_horizontal = Functions.XInternAtom(window.Display, "_NET_WM_STATE_MAXIMIZED_HORZ", false);
        this._atom_net_wm_state_maximized_vertical = Functions.XInternAtom(window.Display, "_NET_WM_STATE_MAXIMIZED_VERT", false);
        this._atom_net_wm_allowed_actions = Functions.XInternAtom(window.Display, "_NET_WM_ALLOWED_ACTIONS", false);
        this._atom_net_wm_action_resize = Functions.XInternAtom(window.Display, "_NET_WM_ACTION_RESIZE", false);
        this._atom_net_wm_action_maximize_horizontally = Functions.XInternAtom(window.Display, "_NET_WM_ACTION_MAXIMIZE_HORZ", false);
        this._atom_net_wm_action_maximize_vertically = Functions.XInternAtom(window.Display, "_NET_WM_ACTION_MAXIMIZE_VERT", false);
        this._atom_net_wm_icon = Functions.XInternAtom(window.Display, "_NEW_WM_ICON", false);
        this._atom_net_frame_extents = Functions.XInternAtom(window.Display, "_NET_FRAME_EXTENTS", false);
      }
    }

    private void SetWindowMinMax(short min_width, short min_height, short max_width, short max_height)
    {
      XSizeHints hints = new XSizeHints();
      using (new XLock(this.window.Display))
      {
        IntPtr supplied_return;
        Functions.XGetWMNormalHints(this.window.Display, this.window.WindowHandle, ref hints, out supplied_return);
      }
      if ((int) min_width > 0 || (int) min_height > 0)
      {
        hints.flags = (IntPtr) ((int) hints.flags | 16);
        hints.min_width = (int) min_width;
        hints.min_height = (int) min_height;
      }
      else
        hints.flags = (IntPtr) ((int) hints.flags & -17);
      if ((int) max_width > 0 || (int) max_height > 0)
      {
        hints.flags = (IntPtr) ((int) hints.flags | 32);
        hints.max_width = (int) max_width;
        hints.max_height = (int) max_height;
      }
      else
        hints.flags = (IntPtr) ((int) hints.flags & -33);
      if (!(hints.flags != IntPtr.Zero))
        return;
      using (new XLock(this.window.Display))
        Functions.XSetWMNormalHints(this.window.Display, this.window.WindowHandle, ref hints);
    }

    private void DisableWindowDecorations()
    {
      if (this.DisableMotifDecorations())
        this._decorations_hidden = true;
      using (new XLock(this.window.Display))
      {
        Functions.XSetTransientForHint(this.window.Display, this.Handle, this.window.RootWindow);
        if (!this._decorations_hidden)
          return;
        Functions.XUnmapWindow(this.window.Display, this.Handle);
        Functions.XMapWindow(this.window.Display, this.Handle);
      }
    }

    private bool DisableMotifDecorations()
    {
      using (new XLock(this.window.Display))
      {
        IntPtr num = Functions.XInternAtom(this.window.Display, "_MOTIF_WM_HINTS", true);
        if (!(num != IntPtr.Zero))
          return false;
        MotifWmHints data = new MotifWmHints();
        data.flags = (IntPtr) 2L;
        Functions.XChangeProperty(this.window.Display, this.Handle, num, num, 32, PropertyMode.Replace, ref data, Marshal.SizeOf((object) data) / IntPtr.Size);
        return true;
      }
    }

    private bool DisableGnomeDecorations()
    {
      using (new XLock(this.window.Display))
      {
        IntPtr num1 = Functions.XInternAtom(this.window.Display, "_WIN_HINTS", true);
        if (!(num1 != IntPtr.Zero))
          return false;
        IntPtr num2 = IntPtr.Zero;
        Functions.XChangeProperty(this.window.Display, this.Handle, num1, num1, 32, PropertyMode.Replace, ref num2, Marshal.SizeOf((object) num2) / IntPtr.Size);
        return true;
      }
    }

    private void EnableWindowDecorations()
    {
      if (this.EnableMotifDecorations())
        this._decorations_hidden = false;
      using (new XLock(this.window.Display))
      {
        Functions.XSetTransientForHint(this.window.Display, this.Handle, IntPtr.Zero);
        if (this._decorations_hidden)
          return;
        Functions.XUnmapWindow(this.window.Display, this.Handle);
        Functions.XMapWindow(this.window.Display, this.Handle);
      }
    }

    private bool EnableMotifDecorations()
    {
      using (new XLock(this.window.Display))
      {
        IntPtr num = Functions.XInternAtom(this.window.Display, "_MOTIF_WM_HINTS", true);
        if (!(num != IntPtr.Zero))
          return false;
        MotifWmHints data = new MotifWmHints();
        data.flags = (IntPtr) 2L;
        data.decorations = (IntPtr) 1L;
        Functions.XChangeProperty(this.window.Display, this.Handle, num, num, 32, PropertyMode.Replace, ref data, Marshal.SizeOf((object) data) / IntPtr.Size);
        return true;
      }
    }

    private bool EnableGnomeDecorations()
    {
      using (new XLock(this.window.Display))
      {
        IntPtr property = Functions.XInternAtom(this.window.Display, "_WIN_HINTS", true);
        if (!(property != IntPtr.Zero))
          return false;
        Functions.XDeleteProperty(this.window.Display, this.Handle, property);
        return true;
      }
    }

    private static void DeleteIconPixmaps(IntPtr display, IntPtr window)
    {
      using (new XLock(display))
      {
        IntPtr num = Functions.XGetWMHints(display, window);
        if (!(num != IntPtr.Zero))
          return;
        XWMHints wmhints = (XWMHints) Marshal.PtrToStructure(num, typeof (XWMHints));
        XWMHintsFlags xwmHintsFlags = (XWMHintsFlags) wmhints.flags.ToInt32();
        if ((xwmHintsFlags & XWMHintsFlags.IconPixmapHint) != (XWMHintsFlags) 0)
        {
          wmhints.flags = new IntPtr((int) (xwmHintsFlags & ~XWMHintsFlags.IconPixmapHint));
          Functions.XFreePixmap(display, wmhints.icon_pixmap);
        }
        if ((xwmHintsFlags & XWMHintsFlags.IconMaskHint) != (XWMHintsFlags) 0)
        {
          wmhints.flags = new IntPtr((int) (xwmHintsFlags & ~XWMHintsFlags.IconMaskHint));
          Functions.XFreePixmap(display, wmhints.icon_mask);
        }
        Functions.XSetWMHints(display, window, ref wmhints);
        Functions.XFree(num);
      }
    }

    private bool RefreshWindowBorders()
    {
      IntPtr prop = IntPtr.Zero;
      bool flag = false;
      IntPtr nitems;
      using (new XLock(this.window.Display))
      {
        IntPtr actual_type;
        int actual_format;
        IntPtr bytes_after;
        Functions.XGetWindowProperty(this.window.Display, this.window.WindowHandle, this._atom_net_frame_extents, IntPtr.Zero, new IntPtr(16), false, (IntPtr) 6L, out actual_type, out actual_format, out nitems, out bytes_after, ref prop);
      }
      if (prop != IntPtr.Zero)
      {
        if ((long) nitems == 4L)
        {
          int num1 = Marshal.ReadIntPtr(prop, 0).ToInt32();
          int num2 = Marshal.ReadIntPtr(prop, IntPtr.Size).ToInt32();
          int num3 = Marshal.ReadIntPtr(prop, IntPtr.Size * 2).ToInt32();
          int num4 = Marshal.ReadIntPtr(prop, IntPtr.Size * 3).ToInt32();
          flag = num1 != this.border_left || num2 != this.border_right || num3 != this.border_top || num4 != this.border_bottom;
          this.border_left = num1;
          this.border_right = num2;
          this.border_top = num3;
          this.border_bottom = num4;
        }
        using (new XLock(this.window.Display))
          Functions.XFree(prop);
      }
      return flag;
    }

    private void RefreshWindowBounds(ref XEvent e)
    {
      this.RefreshWindowBorders();
      Point point = new Point(e.ConfigureEvent.x - this.border_left, e.ConfigureEvent.y - this.border_top);
      if (this.Location != point)
      {
        this.bounds.Location = point;
        this.Move((object) this, EventArgs.Empty);
      }
      Size size = new Size(e.ConfigureEvent.width + this.border_left + this.border_right, e.ConfigureEvent.height + this.border_top + this.border_bottom);
      if (!(this.Bounds.Size != size))
        return;
      this.bounds.Size = size;
      this.client_rectangle.Size = new Size(e.ConfigureEvent.width, e.ConfigureEvent.height);
      this.Resize((object) this, EventArgs.Empty);
    }

    private static IntPtr CreateEmptyCursor(X11WindowInfo window)
    {
      IntPtr num1 = IntPtr.Zero;
      using (new XLock(window.Display))
      {
        IntPtr colormap = Functions.XDefaultColormap(window.Display, window.Screen);
        XColor screen_def_return;
        XColor exact_def_return;
        Functions.XAllocNamedColor(window.Display, colormap, "black", out screen_def_return, out exact_def_return);
        IntPtr num2 = Functions.XCreateBitmapFromData(window.Display, window.WindowHandle, new byte[1, 1]);
        return Functions.XCreatePixmapCursor(window.Display, num2, num2, ref screen_def_return, ref screen_def_return, 0, 0);
      }
    }

    private static void SetMouseClamped(MouseDevice mouse, int x, int y, int left, int top, int width, int height)
    {
      x = Math.Max(x, left);
      x = Math.Min(x, width);
      y = Math.Max(y, top);
      y = Math.Min(y, height);
      mouse.Position = new Point(x, y);
    }

    public void ProcessEvents()
    {
      while (this.Exists && this.window != null)
      {
        using (new XLock(this.window.Display))
        {
          if (!Functions.XCheckWindowEvent(this.window.Display, this.window.WindowHandle, this.window.EventMask, ref this.e))
          {
            if (!Functions.XCheckTypedWindowEvent(this.window.Display, this.window.WindowHandle, XEventName.ClientMessage, ref this.e))
              break;
          }
        }
        switch (this.e.type)
        {
          case XEventName.KeyPress:
            this.driver.ProcessEvent(ref this.e);
            int byteCount = Functions.XLookupString(ref this.e.KeyEvent, this.ascii, this.ascii.Length, (IntPtr[]) null, IntPtr.Zero);
            Encoding.Default.GetChars(this.ascii, 0, byteCount, this.chars, 0);
            EventHandler<KeyPressEventArgs> eventHandler = this.KeyPress;
            if (eventHandler != null)
            {
              for (int index = 0; index < byteCount; ++index)
              {
                this.KPEventArgs.KeyChar = this.chars[index];
                eventHandler((object) this, this.KPEventArgs);
              }
              continue;
            }
            else
              continue;
          case XEventName.KeyRelease:
            this.driver.ProcessEvent(ref this.e);
            continue;
          case XEventName.ButtonPress:
          case XEventName.ButtonRelease:
            this.driver.ProcessEvent(ref this.e);
            continue;
          case XEventName.MotionNotify:
            int x = this.e.MotionEvent.x;
            int y = this.e.MotionEvent.y;
            int num1 = (this.Bounds.Left + this.Bounds.Right) / 2;
            int num2 = (this.Bounds.Top + this.Bounds.Bottom) / 2;
            Point point = this.PointToScreen(new Point(x, y));
            if (!this.CursorVisible && X11GLNative.MouseWarpActive && (point.X == num1 && point.Y == num2))
            {
              X11GLNative.MouseWarpActive = false;
              this.mouse_rel_x = x;
              this.mouse_rel_y = y;
              continue;
            }
            else if (!this.CursorVisible)
            {
              X11GLNative.SetMouseClamped(this.mouse, this.mouse.X + x - this.mouse_rel_x, this.mouse.Y + y - this.mouse_rel_y, 0, 0, this.Width, this.Height);
              this.mouse_rel_x = x;
              this.mouse_rel_y = y;
              X11GLNative.MouseWarpActive = true;
              Mouse.SetPosition((double) num1, (double) num2);
              continue;
            }
            else
            {
              X11GLNative.SetMouseClamped(this.mouse, x, y, 0, 0, this.Width, this.Height);
              this.mouse_rel_x = x;
              this.mouse_rel_y = y;
              continue;
            }
          case XEventName.EnterNotify:
            this.MouseEnter((object) this, EventArgs.Empty);
            continue;
          case XEventName.LeaveNotify:
            if (this.CursorVisible)
            {
              this.MouseLeave((object) this, EventArgs.Empty);
              continue;
            }
            else
              continue;
          case XEventName.FocusIn:
            bool flag1 = this.has_focus;
            this.has_focus = true;
            if (this.has_focus != flag1)
            {
              this.FocusedChanged((object) this, EventArgs.Empty);
              continue;
            }
            else
              continue;
          case XEventName.FocusOut:
            bool flag2 = this.has_focus;
            this.has_focus = false;
            if (this.has_focus != flag2)
            {
              this.FocusedChanged((object) this, EventArgs.Empty);
              continue;
            }
            else
              continue;
          case XEventName.DestroyNotify:
            this.exists = false;
            this.Closed((object) this, EventArgs.Empty);
            return;
          case XEventName.UnmapNotify:
            bool flag3 = this.visible;
            this.visible = false;
            if (this.visible != flag3)
            {
              this.VisibleChanged((object) this, EventArgs.Empty);
              continue;
            }
            else
              continue;
          case XEventName.MapNotify:
            bool flag4 = this.visible;
            this.visible = true;
            if (this.visible == flag4)
              return;
            this.VisibleChanged((object) this, EventArgs.Empty);
            return;
          case XEventName.ConfigureNotify:
            this.RefreshWindowBounds(ref this.e);
            continue;
          case XEventName.PropertyNotify:
            if (this.e.PropertyEvent.atom == this._atom_net_wm_state)
            {
              this.WindowStateChanged((object) this, EventArgs.Empty);
              continue;
            }
            else
              continue;
          case XEventName.ClientMessage:
            if (!this.isExiting && this.e.ClientMessageEvent.ptr1 == this._atom_wm_destroy)
            {
              CancelEventArgs e = new CancelEventArgs();
              this.Closing((object) this, e);
              if (!e.Cancel)
              {
                this.isExiting = true;
                using (new XLock(this.window.Display))
                {
                  Functions.XDestroyWindow(this.window.Display, this.window.WindowHandle);
                  continue;
                }
              }
              else
                continue;
            }
            else
              continue;
          case XEventName.MappingNotify:
            if (this.e.MappingEvent.request == 0 || this.e.MappingEvent.request == 1)
            {
              Functions.XRefreshKeyboardMapping(ref this.e.MappingEvent);
              continue;
            }
            else
              continue;
          default:
            continue;
        }
      }
    }

    public void Close()
    {
      this.Exit();
    }

    public void Exit()
    {
      XEvent send_event = new XEvent();
      send_event.type = XEventName.ClientMessage;
      send_event.ClientMessageEvent.format = 32;
      send_event.ClientMessageEvent.display = this.window.Display;
      send_event.ClientMessageEvent.window = this.window.WindowHandle;
      send_event.ClientMessageEvent.ptr1 = this._atom_wm_destroy;
      using (new XLock(this.window.Display))
      {
        Functions.XSendEvent(this.window.Display, this.window.WindowHandle, false, EventMask.NoEventMask, ref send_event);
        Functions.XFlush(this.window.Display);
      }
    }

    public void DestroyWindow()
    {
      using (new XLock(this.window.Display))
        Functions.XDestroyWindow(this.window.Display, this.window.WindowHandle);
    }

    public Point PointToClient(Point point)
    {
      int intdest_x_return;
      int dest_y_return;
      using (new XLock(this.window.Display))
      {
        IntPtr child_return;
        Functions.XTranslateCoordinates(this.window.Display, this.window.RootWindow, this.window.WindowHandle, point.X, point.Y, out intdest_x_return, out dest_y_return, out child_return);
      }
      point.X = intdest_x_return;
      point.Y = dest_y_return;
      return point;
    }

    public Point PointToScreen(Point point)
    {
      int intdest_x_return;
      int dest_y_return;
      using (new XLock(this.window.Display))
      {
        IntPtr child_return;
        Functions.XTranslateCoordinates(this.window.Display, this.window.WindowHandle, this.window.RootWindow, point.X, point.Y, out intdest_x_return, out dest_y_return, out child_return);
      }
      point.X = intdest_x_return;
      point.Y = dest_y_return;
      return point;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool manuallyCalled)
    {
      if (this.disposed)
        return;
      if (manuallyCalled && this.window != null && this.window.WindowHandle != IntPtr.Zero)
      {
        if (this.Exists)
        {
          using (new XLock(this.window.Display))
          {
            Functions.XFreeCursor(this.window.Display, this.EmptyCursor);
            Functions.XDestroyWindow(this.window.Display, this.window.WindowHandle);
          }
          while (this.Exists)
            this.ProcessEvents();
        }
        this.window.Dispose();
        this.window = (X11WindowInfo) null;
      }
      this.disposed = true;
    }
  }
}
