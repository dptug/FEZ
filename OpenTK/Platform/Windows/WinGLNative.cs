// Type: OpenTK.Platform.Windows.WinGLNative
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenTK.Platform.Windows
{
  internal sealed class WinGLNative : INativeWindow, IInputDriver, IKeyboardDriver, IMouseDriver, IJoystickDriver, IDisposable
  {
    private static readonly WinKeyMap KeyMap = new WinKeyMap();
    private static readonly uint ShiftRightScanCode = Functions.MapVirtualKey(VirtualKeys.RSHIFT, MapVirtualKeyType.VirtualKeyToScanCode);
    private static readonly object SyncRoot = new object();
    private readonly IntPtr Instance = Marshal.GetHINSTANCE(typeof (WinGLNative).Module);
    private readonly IntPtr ClassName = Marshal.StringToHGlobalAuto(Guid.NewGuid().ToString());
    private readonly uint ModalLoopTimerPeriod = 1U;
    private bool mouse_outside_window = true;
    private Rectangle bounds = new Rectangle();
    private Rectangle client_rectangle = new Rectangle();
    private Rectangle previous_bounds = new Rectangle();
    private WinMMJoystick joystick_driver = new WinMMJoystick();
    private KeyboardDevice keyboard = new KeyboardDevice();
    private MouseDevice mouse = new MouseDevice();
    private IList<KeyboardDevice> keyboards = (IList<KeyboardDevice>) new List<KeyboardDevice>(1);
    private IList<MouseDevice> mice = (IList<MouseDevice>) new List<MouseDevice>(1);
    private KeyPressEventArgs key_press = new KeyPressEventArgs(char.MinValue);
    private StringBuilder sb_title = new StringBuilder(256);
    private const ExtendedWindowStyle ParentStyleEx = ExtendedWindowStyle.WindowEdge | ExtendedWindowStyle.ApplicationWindow;
    private const ExtendedWindowStyle ChildStyleEx = ExtendedWindowStyle.Left;
    private const ClassStyle DefaultClassStyle = ClassStyle.OwnDC;
    private const long ExtendedBit = 16777216L;
    private readonly WindowProcedure WindowProcedureDelegate;
    private UIntPtr timer_handle;
    private readonly Functions.TimerProc ModalLoopCallback;
    private bool class_registered;
    private bool disposed;
    private bool exists;
    private WinWindowInfo window;
    private WinWindowInfo child_window;
    private WindowBorder windowBorder;
    private WindowBorder? previous_window_border;
    private WindowBorder? deferred_window_border;
    private WindowState windowState;
    private bool borderless_maximized_window_state;
    private bool focused;
    private bool invisible_since_creation;
    private int suppress_resize;
    private Icon icon;
    private int cursor_visible_count;
    private int ret;
    private MSG msg;

    private bool IsIdle
    {
      get
      {
        MSG msg = new MSG();
        return !Functions.PeekMessage(ref msg, this.window.WindowHandle, 0, 0, 0);
      }
    }

    public Rectangle Bounds
    {
      get
      {
        return this.bounds;
      }
      set
      {
        Functions.SetWindowPos(this.window.WindowHandle, IntPtr.Zero, value.X, value.Y, value.Width, value.Height, (SetWindowPosFlags) 0);
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
        Functions.SetWindowPos(this.window.WindowHandle, IntPtr.Zero, value.X, value.Y, 0, 0, SetWindowPosFlags.NOSIZE);
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
        Functions.SetWindowPos(this.window.WindowHandle, IntPtr.Zero, 0, 0, value.Width, value.Height, SetWindowPosFlags.NOMOVE);
      }
    }

    public Rectangle ClientRectangle
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
        this.ClientSize = value.Size;
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
        WindowStyle dwStyle = (WindowStyle) (uint) Functions.GetWindowLong(this.window.WindowHandle, GetWindowLongOffsets.STYLE);
        Win32Rectangle lpRect = Win32Rectangle.From(value);
        Functions.AdjustWindowRect(out lpRect, dwStyle, false);
        this.Size = new Size(lpRect.Width, lpRect.Height);
      }
    }

    public int Width
    {
      get
      {
        return this.ClientRectangle.Width;
      }
      set
      {
        this.ClientRectangle = new Rectangle(0, 0, value, this.Height);
      }
    }

    public int Height
    {
      get
      {
        return this.ClientRectangle.Height;
      }
      set
      {
        this.ClientRectangle = new Rectangle(0, 0, this.Width, value);
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
        this.icon = value;
        if (this.window.WindowHandle != IntPtr.Zero)
        {
          Functions.SendMessage(this.window.WindowHandle, WindowMessage.SETICON, (IntPtr) 0, this.icon == null ? IntPtr.Zero : value.Handle);
          Functions.SendMessage(this.window.WindowHandle, WindowMessage.SETICON, (IntPtr) 1, this.icon == null ? IntPtr.Zero : value.Handle);
        }
        this.IconChanged((object) this, EventArgs.Empty);
      }
    }

    public bool Focused
    {
      get
      {
        return this.focused;
      }
    }

    public string Title
    {
      get
      {
        this.sb_title.Remove(0, this.sb_title.Length);
        Functions.GetWindowText(this.window.WindowHandle, this.sb_title, this.sb_title.Capacity);
        return ((object) this.sb_title).ToString();
      }
      set
      {
        if (!(this.Title != value))
          return;
        Functions.SetWindowText(this.window.WindowHandle, value);
        this.TitleChanged((object) this, EventArgs.Empty);
      }
    }

    public bool Visible
    {
      get
      {
        return Functions.IsWindowVisible(this.window.WindowHandle);
      }
      set
      {
        if (value == this.Visible)
          return;
        if (value)
        {
          Functions.ShowWindow(this.window.WindowHandle, ShowWindowCommand.SHOW);
          if (this.invisible_since_creation)
          {
            Functions.BringWindowToTop(this.window.WindowHandle);
            Functions.SetForegroundWindow(this.window.WindowHandle);
          }
        }
        else if (!value)
          Functions.ShowWindow(this.window.WindowHandle, ShowWindowCommand.HIDE);
        this.VisibleChanged((object) this, EventArgs.Empty);
      }
    }

    public bool Exists
    {
      get
      {
        return this.exists;
      }
    }

    public bool CursorVisible
    {
      get
      {
        return this.cursor_visible_count >= 0;
      }
      set
      {
        if (value && this.cursor_visible_count < 0)
        {
          do
          {
            this.cursor_visible_count = Functions.ShowCursor(true);
          }
          while (this.cursor_visible_count < 0);
          this.UngrabCursor();
        }
        else
        {
          if (value || this.cursor_visible_count < 0)
            return;
          do
          {
            this.cursor_visible_count = Functions.ShowCursor(false);
          }
          while (this.cursor_visible_count >= 0);
          this.GrabCursor();
        }
      }
    }

    public WindowState WindowState
    {
      get
      {
        return this.windowState;
      }
      set
      {
        if (this.WindowState == value)
          return;
        ShowWindowCommand nCmdShow = ShowWindowCommand.HIDE;
        bool flag = false;
        this.borderless_maximized_window_state = false;
        switch (value)
        {
          case WindowState.Normal:
            nCmdShow = ShowWindowCommand.RESTORE;
            if (this.WindowState == WindowState.Fullscreen)
            {
              flag = true;
              break;
            }
            else
              break;
          case WindowState.Minimized:
            nCmdShow = ShowWindowCommand.MINIMIZE;
            break;
          case WindowState.Maximized:
            this.ResetWindowState();
            if (this.WindowBorder == WindowBorder.Hidden)
            {
              IntPtr hMonitor = Functions.MonitorFromWindow(this.window.WindowHandle, MonitorFrom.Nearest);
              MonitorInfo lpmi = new MonitorInfo();
              lpmi.Size = MonitorInfo.SizeInBytes;
              Functions.GetMonitorInfo(hMonitor, ref lpmi);
              this.previous_bounds = this.Bounds;
              this.borderless_maximized_window_state = true;
              this.Bounds = lpmi.Work.ToRectangle();
              break;
            }
            else
            {
              nCmdShow = ShowWindowCommand.SHOWMAXIMIZED;
              break;
            }
          case WindowState.Fullscreen:
            this.ResetWindowState();
            this.previous_bounds = this.Bounds;
            this.previous_window_border = new WindowBorder?(this.WindowBorder);
            this.HideBorder();
            nCmdShow = ShowWindowCommand.SHOWMAXIMIZED;
            Functions.SetForegroundWindow(this.window.WindowHandle);
            break;
        }
        if (nCmdShow != ShowWindowCommand.HIDE)
          Functions.ShowWindow(this.window.WindowHandle, nCmdShow);
        if (flag)
          this.RestoreBorder();
        if (nCmdShow != ShowWindowCommand.RESTORE || !(this.previous_bounds != Rectangle.Empty))
          return;
        this.Bounds = this.previous_bounds;
        this.previous_bounds = Rectangle.Empty;
      }
    }

    public WindowBorder WindowBorder
    {
      get
      {
        return this.windowBorder;
      }
      set
      {
        if (this.WindowState == WindowState.Fullscreen)
        {
          this.deferred_window_border = new WindowBorder?(value);
        }
        else
        {
          if (this.windowBorder == value)
            return;
          bool visible = this.Visible;
          WindowState windowState = this.WindowState;
          this.ResetWindowState();
          WindowStyle dwStyle = WindowStyle.ClipSiblings | WindowStyle.ClipChildren;
          switch (value)
          {
            case WindowBorder.Resizable:
              dwStyle |= WindowStyle.TiledWindow;
              break;
            case WindowBorder.Fixed:
              dwStyle |= WindowStyle.Caption | WindowStyle.SystemMenu | WindowStyle.Group;
              break;
            case WindowBorder.Hidden:
              dwStyle |= WindowStyle.Popup;
              break;
          }
          Win32Rectangle lpRect = Win32Rectangle.From(this.ClientSize);
          Functions.AdjustWindowRectEx(ref lpRect, dwStyle, false, ExtendedWindowStyle.WindowEdge | ExtendedWindowStyle.ApplicationWindow);
          if (visible)
            this.Visible = false;
          Functions.SetWindowLong(this.window.WindowHandle, GetWindowLongOffsets.STYLE, (IntPtr) ((int) dwStyle));
          Functions.SetWindowPos(this.window.WindowHandle, IntPtr.Zero, 0, 0, lpRect.Width, lpRect.Height, SetWindowPosFlags.NOMOVE | SetWindowPosFlags.NOZORDER | SetWindowPosFlags.FRAMECHANGED);
          if (visible)
            this.Visible = true;
          this.WindowState = windowState;
          this.WindowBorderChanged((object) this, EventArgs.Empty);
        }
      }
    }

    public IInputDriver InputDriver
    {
      get
      {
        return (IInputDriver) this;
      }
    }

    public IWindowInfo WindowInfo
    {
      get
      {
        return (IWindowInfo) this.child_window;
      }
    }

    public IList<KeyboardDevice> Keyboard
    {
      get
      {
        return this.keyboards;
      }
    }

    public IList<MouseDevice> Mouse
    {
      get
      {
        return this.mice;
      }
    }

    public IList<JoystickDevice> Joysticks
    {
      get
      {
        return this.joystick_driver.Joysticks;
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

    static WinGLNative()
    {
    }

    public WinGLNative(int x, int y, int width, int height, string title, GameWindowFlags options, DisplayDevice device)
    {
      lock (WinGLNative.SyncRoot)
      {
        this.WindowProcedureDelegate = new WindowProcedure(this.WindowProcedure);
        this.window = new WinWindowInfo(this.CreateWindow(x, y, width, height, title, options, device, IntPtr.Zero), (WinWindowInfo) null);
        this.child_window = new WinWindowInfo(this.CreateWindow(0, 0, this.ClientSize.Width, this.ClientSize.Height, title, options, device, this.window.WindowHandle), this.window);
        this.exists = true;
        this.keyboard.Description = "Standard Windows keyboard";
        this.keyboard.NumberOfFunctionKeys = 12;
        this.keyboard.NumberOfKeys = 101;
        this.keyboard.NumberOfLeds = 3;
        this.mouse.Description = "Standard Windows mouse";
        this.mouse.NumberOfButtons = 3;
        this.mouse.NumberOfWheels = 1;
        this.keyboards.Add(this.keyboard);
        this.mice.Add(this.mouse);
      }
    }

    ~WinGLNative()
    {
      this.Dispose(false);
    }

    private unsafe IntPtr WindowProcedure(IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam)
    {
      switch (message)
      {
        case WindowMessage.MOUSEMOVE:
          this.mouse.Position = new Point((int) (short) (lParam.ToInt32() & (int) ushort.MaxValue), (int) (short) ((uint) (lParam.ToInt32() & -65536) >> 16));
          if (this.mouse_outside_window)
          {
            this.mouse_outside_window = false;
            this.EnableMouseTracking();
            this.MouseEnter((object) this, EventArgs.Empty);
            break;
          }
          else
            break;
        case WindowMessage.LBUTTONDOWN:
          Functions.SetCapture(this.window.WindowHandle);
          this.mouse[MouseButton.Left] = true;
          break;
        case WindowMessage.LBUTTONUP:
          Functions.ReleaseCapture();
          this.mouse[MouseButton.Left] = false;
          break;
        case WindowMessage.RBUTTONDOWN:
          Functions.SetCapture(this.window.WindowHandle);
          this.mouse[MouseButton.Right] = true;
          break;
        case WindowMessage.RBUTTONUP:
          Functions.ReleaseCapture();
          this.mouse[MouseButton.Right] = false;
          break;
        case WindowMessage.MBUTTONDOWN:
          Functions.SetCapture(this.window.WindowHandle);
          this.mouse[MouseButton.Middle] = true;
          break;
        case WindowMessage.MBUTTONUP:
          Functions.ReleaseCapture();
          this.mouse[MouseButton.Middle] = false;
          break;
        case WindowMessage.MOUSEWHEEL:
          this.mouse.WheelPrecise += (float) ((long) wParam << 32 >> 48) / 120f;
          break;
        case WindowMessage.XBUTTONDOWN:
          Functions.SetCapture(this.window.WindowHandle);
          this.mouse[((long) wParam.ToInt32() & 4294901760L) >> 16 != 32L ? MouseButton.Button1 : MouseButton.Button2] = true;
          break;
        case WindowMessage.XBUTTONUP:
          Functions.ReleaseCapture();
          this.mouse[((long) wParam.ToInt32() & 4294901760L) >> 16 != 32L ? MouseButton.Button1 : MouseButton.Button2] = false;
          break;
        case WindowMessage.ENTERMENULOOP:
        case WindowMessage.ENTERSIZEMOVE:
          this.StartTimer(handle);
          break;
        case WindowMessage.EXITMENULOOP:
        case WindowMessage.EXITSIZEMOVE:
          this.StopTimer(handle);
          break;
        case WindowMessage.MOUSELEAVE:
          this.mouse_outside_window = true;
          this.MouseLeave((object) this, EventArgs.Empty);
          break;
        case WindowMessage.STYLECHANGED:
          if (wParam.ToInt64() == -16L)
          {
            WindowStyle windowStyle = ((StyleStruct*) (void*) lParam)->New;
            if ((windowStyle & WindowStyle.Popup) != WindowStyle.Overlapped)
              this.windowBorder = WindowBorder.Hidden;
            else if ((windowStyle & WindowStyle.ThickFrame) != WindowStyle.Overlapped)
              this.windowBorder = WindowBorder.Resizable;
            else if ((windowStyle & ~(WindowStyle.ThickFrame | WindowStyle.TabStop)) != WindowStyle.Overlapped)
              this.windowBorder = WindowBorder.Fixed;
          }
          if (!this.CursorVisible)
          {
            this.GrabCursor();
            break;
          }
          else
            break;
        case WindowMessage.KEYDOWN:
        case WindowMessage.KEYUP:
        case WindowMessage.SYSKEYDOWN:
        case WindowMessage.SYSKEYUP:
          bool flag1 = message == WindowMessage.KEYDOWN || message == WindowMessage.SYSKEYDOWN;
          bool flag2 = (lParam.ToInt64() & 16777216L) != 0L;
          switch ((short) (int) wParam)
          {
            case (short) 13:
              if (flag2)
                this.keyboard[Key.KeypadEnter] = flag1;
              else
                this.keyboard[Key.Enter] = flag1;
              return IntPtr.Zero;
            case (short) 16:
              if ((int) WinGLNative.ShiftRightScanCode != 0 && flag1)
              {
                if ((lParam.ToInt64() >> 16 & (long) byte.MaxValue) == (long) WinGLNative.ShiftRightScanCode)
                  this.keyboard[Key.ShiftRight] = flag1;
                else
                  this.keyboard[Key.ShiftLeft] = flag1;
              }
              else
                this.keyboard[Key.ShiftLeft] = this.keyboard[Key.ShiftRight] = flag1;
              return IntPtr.Zero;
            case (short) 17:
              if (flag2)
                this.keyboard[Key.ControlRight] = flag1;
              else
                this.keyboard[Key.ControlLeft] = flag1;
              return IntPtr.Zero;
            case (short) 18:
              if (flag2)
                this.keyboard[Key.AltRight] = flag1;
              else
                this.keyboard[Key.AltLeft] = flag1;
              return IntPtr.Zero;
            default:
              if (WinGLNative.KeyMap.ContainsKey((VirtualKeys) (int) wParam))
              {
                this.keyboard[WinGLNative.KeyMap[(VirtualKeys) (int) wParam]] = flag1;
                return IntPtr.Zero;
              }
              else
                break;
          }
        case WindowMessage.CHAR:
          this.key_press.KeyChar = IntPtr.Size != 4 ? (char) wParam.ToInt64() : (char) wParam.ToInt32();
          this.KeyPress((object) this, this.key_press);
          break;
        case WindowMessage.SYSCHAR:
          return IntPtr.Zero;
        case WindowMessage.ERASEBKGND:
          return new IntPtr(1);
        case WindowMessage.WINDOWPOSCHANGED:
          WindowPosition* windowPositionPtr = (WindowPosition*) (void*) lParam;
          if (this.window != null && windowPositionPtr->hwnd == this.window.WindowHandle)
          {
            Point point = new Point(windowPositionPtr->x, windowPositionPtr->y);
            if (this.Location != point)
            {
              this.bounds.Location = point;
              this.Move((object) this, EventArgs.Empty);
            }
            if (this.Size != new Size(windowPositionPtr->cx, windowPositionPtr->cy))
            {
              this.bounds.Width = windowPositionPtr->cx;
              this.bounds.Height = windowPositionPtr->cy;
              Win32Rectangle clientRectangle;
              Functions.GetClientRect(handle, out clientRectangle);
              this.client_rectangle = clientRectangle.ToRectangle();
              Functions.SetWindowPos(this.child_window.WindowHandle, IntPtr.Zero, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, SetWindowPosFlags.NOZORDER | SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.NOOWNERZORDER | SetWindowPosFlags.NOSENDCHANGING);
              if (this.suppress_resize <= 0)
                this.Resize((object) this, EventArgs.Empty);
            }
            if (!this.CursorVisible)
            {
              this.GrabCursor();
              break;
            }
            else
              break;
          }
          else
            break;
        case WindowMessage.CREATE:
          CreateStruct createStruct = (CreateStruct) Marshal.PtrToStructure(lParam, typeof (CreateStruct));
          if (createStruct.hwndParent == IntPtr.Zero)
          {
            this.bounds.X = createStruct.x;
            this.bounds.Y = createStruct.y;
            this.bounds.Width = createStruct.cx;
            this.bounds.Height = createStruct.cy;
            Win32Rectangle clientRectangle;
            Functions.GetClientRect(handle, out clientRectangle);
            this.client_rectangle = clientRectangle.ToRectangle();
            this.invisible_since_creation = true;
            break;
          }
          else
            break;
        case WindowMessage.DESTROY:
          this.exists = false;
          int num = (int) Functions.UnregisterClass(this.ClassName, this.Instance);
          this.window.Dispose();
          this.child_window.Dispose();
          this.Closed((object) this, EventArgs.Empty);
          break;
        case WindowMessage.SIZE:
          SizeMessage sizeMessage = (SizeMessage) wParam.ToInt64();
          WindowState windowState = this.windowState;
          switch (sizeMessage)
          {
            case SizeMessage.RESTORED:
              windowState = this.borderless_maximized_window_state ? WindowState.Maximized : WindowState.Normal;
              break;
            case SizeMessage.MINIMIZED:
              windowState = WindowState.Minimized;
              break;
            case SizeMessage.MAXIMIZED:
              windowState = this.WindowBorder == WindowBorder.Hidden ? WindowState.Fullscreen : WindowState.Maximized;
              break;
          }
          if (windowState != this.windowState)
          {
            this.windowState = windowState;
            this.WindowStateChanged((object) this, EventArgs.Empty);
          }
          if (!this.CursorVisible)
          {
            this.GrabCursor();
            break;
          }
          else
            break;
        case WindowMessage.ACTIVATE:
          bool focused = this.Focused;
          this.focused = IntPtr.Size != 4 ? (wParam.ToInt64() & (long) ushort.MaxValue) != 0L : (wParam.ToInt32() & (int) ushort.MaxValue) != 0;
          if (focused != this.Focused)
          {
            this.FocusedChanged((object) this, EventArgs.Empty);
            break;
          }
          else
            break;
        case WindowMessage.KILLFOCUS:
          this.keyboard.ClearKeys();
          break;
        case WindowMessage.CLOSE:
          CancelEventArgs e = new CancelEventArgs();
          this.Closing((object) this, e);
          if (e.Cancel)
            return IntPtr.Zero;
          this.DestroyWindow();
          break;
      }
      return Functions.DefWindowProc(handle, message, wParam, lParam);
    }

    private void EnableMouseTracking()
    {
      Functions.TrackMouseEvent(ref new TrackMouseEventStructure()
      {
        Size = TrackMouseEventStructure.SizeInBytes,
        TrackWindowHandle = this.child_window.WindowHandle,
        Flags = TrackMouseEventFlags.LEAVE
      });
    }

    private void StartTimer(IntPtr handle)
    {
      if (!(this.timer_handle == UIntPtr.Zero))
        return;
      this.timer_handle = Functions.SetTimer(handle, new UIntPtr(1U), this.ModalLoopTimerPeriod, this.ModalLoopCallback);
      int num = this.timer_handle == UIntPtr.Zero ? 1 : 0;
    }

    private void StopTimer(IntPtr handle)
    {
      if (!(this.timer_handle != UIntPtr.Zero))
        return;
      Functions.KillTimer(handle, this.timer_handle);
      this.timer_handle = UIntPtr.Zero;
    }

    private IntPtr CreateWindow(int x, int y, int width, int height, string title, GameWindowFlags options, DisplayDevice device, IntPtr parentHandle)
    {
      WindowStyle windowStyle1 = WindowStyle.Overlapped;
      WindowStyle windowStyle2;
      ExtendedWindowStyle extendedWindowStyle;
      if (parentHandle == IntPtr.Zero)
      {
        windowStyle2 = windowStyle1 | WindowStyle.TiledWindow | WindowStyle.ClipChildren;
        extendedWindowStyle = ExtendedWindowStyle.WindowEdge | ExtendedWindowStyle.ApplicationWindow;
      }
      else
      {
        windowStyle2 = windowStyle1 | WindowStyle.Child | WindowStyle.Visible | WindowStyle.ClipSiblings;
        extendedWindowStyle = ExtendedWindowStyle.Left;
      }
      Win32Rectangle lpRect = new Win32Rectangle();
      lpRect.left = x;
      lpRect.top = y;
      lpRect.right = x + width;
      lpRect.bottom = y + height;
      Functions.AdjustWindowRectEx(ref lpRect, windowStyle2, false, extendedWindowStyle);
      if (!this.class_registered)
      {
        if ((int) Functions.RegisterClassEx(ref new ExtendedWindowClass()
        {
          Size = ExtendedWindowClass.SizeInBytes,
          Style = ClassStyle.OwnDC,
          Instance = this.Instance,
          WndProc = this.WindowProcedureDelegate,
          ClassName = this.ClassName,
          Icon = this.Icon != null ? this.Icon.Handle : IntPtr.Zero,
          IconSm = this.Icon != null ? new Icon(this.Icon, 16, 16).Handle : IntPtr.Zero,
          Cursor = Functions.LoadCursor(CursorName.Arrow)
        }) == 0)
          throw new PlatformException(string.Format("Failed to register window class. Error: {0}", (object) Marshal.GetLastWin32Error()));
        this.class_registered = true;
      }
      IntPtr WindowName = Marshal.StringToHGlobalAuto(title);
      IntPtr windowEx = Functions.CreateWindowEx(extendedWindowStyle, this.ClassName, WindowName, windowStyle2, lpRect.left, lpRect.top, lpRect.Width, lpRect.Height, parentHandle, IntPtr.Zero, this.Instance, IntPtr.Zero);
      if (windowEx == IntPtr.Zero)
        throw new PlatformException(string.Format("Failed to create window. Error: {0}", (object) Marshal.GetLastWin32Error()));
      else
        return windowEx;
    }

    private void DestroyWindow()
    {
      if (!this.Exists)
        return;
      Functions.DestroyWindow(this.window.WindowHandle);
      this.exists = false;
    }

    private void HideBorder()
    {
      ++this.suppress_resize;
      this.WindowBorder = WindowBorder.Hidden;
      this.ProcessEvents();
      --this.suppress_resize;
    }

    private void RestoreBorder()
    {
      ++this.suppress_resize;
      this.WindowBorder = this.deferred_window_border.HasValue ? this.deferred_window_border.Value : (this.previous_window_border.HasValue ? this.previous_window_border.Value : this.WindowBorder);
      this.ProcessEvents();
      --this.suppress_resize;
      WinGLNative winGlNative1 = this;
      WinGLNative winGlNative2 = this;
      WinGLNative winGlNative3 = this;
      WindowBorder? nullable1 = new WindowBorder?();
      WindowBorder? nullable2 = nullable1;
      winGlNative3.previous_window_border = nullable2;
      WindowBorder? nullable3;
      WindowBorder? nullable4 = nullable3 = nullable1;
      winGlNative2.previous_window_border = nullable3;
      WindowBorder? nullable5 = nullable4;
      winGlNative1.deferred_window_border = nullable5;
    }

    private void ResetWindowState()
    {
      ++this.suppress_resize;
      this.WindowState = WindowState.Normal;
      this.ProcessEvents();
      --this.suppress_resize;
    }

    private void GrabCursor()
    {
      Point point = this.PointToScreen(new Point(this.ClientRectangle.X, this.ClientRectangle.Y));
      Functions.ClipCursor(ref new Win32Rectangle()
      {
        left = point.X,
        right = point.X + this.ClientRectangle.Width,
        top = point.Y,
        bottom = point.Y + this.ClientRectangle.Height
      });
    }

    private void UngrabCursor()
    {
      Functions.ClipCursor(IntPtr.Zero);
    }

    public void Close()
    {
      Functions.PostMessage(this.window.WindowHandle, WindowMessage.CLOSE, IntPtr.Zero, IntPtr.Zero);
    }

    public Point PointToClient(Point point)
    {
      if (!Functions.ScreenToClient(this.window.WindowHandle, ref point))
        throw new InvalidOperationException(string.Format("Could not convert point {0} from screen to client coordinates. Windows error: {1}", (object) point.ToString(), (object) Marshal.GetLastWin32Error()));
      else
        return point;
    }

    public Point PointToScreen(Point point)
    {
      if (!Functions.ClientToScreen(this.window.WindowHandle, ref point))
        throw new InvalidOperationException(string.Format("Could not convert point {0} from screen to client coordinates. Windows error: {1}", (object) point.ToString(), (object) Marshal.GetLastWin32Error()));
      else
        return point;
    }

    public void ProcessEvents()
    {
      while (!this.IsIdle)
      {
        this.ret = Functions.GetMessage(ref this.msg, this.window.WindowHandle, 0, 0);
        if (this.ret == -1)
          throw new PlatformException(string.Format("An error happened while processing the message queue. Windows error: {0}", (object) Marshal.GetLastWin32Error()));
        Functions.TranslateMessage(ref this.msg);
        Functions.DispatchMessage(ref this.msg);
      }
    }

    public void Poll()
    {
      this.joystick_driver.Poll();
    }

    public KeyboardState GetState()
    {
      throw new NotImplementedException();
    }

    public KeyboardState GetState(int index)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool calledManually)
    {
      if (this.disposed)
        return;
      if (calledManually)
      {
        this.DestroyWindow();
        if (this.Icon != null)
          this.Icon.Dispose();
      }
      this.Disposed((object) this, EventArgs.Empty);
      this.disposed = true;
    }
  }
}
