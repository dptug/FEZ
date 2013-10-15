// Type: OpenTK.Platform.MacOS.CarbonGLNative
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using OpenTK.Platform.MacOS.Carbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace OpenTK.Platform.MacOS
{
  internal class CarbonGLNative : INativeWindow, IDisposable
  {
    private string title = "OpenTK Window";
    private bool mExists = true;
    private WindowPositionMethod mPositionMethod = WindowPositionMethod.CenterOnMainScreen;
    private KeyPressEventArgs mKeyPressArgs = new KeyPressEventArgs(char.MinValue);
    private static MacOSKeyMap Keymap = new MacOSKeyMap();
    private static Dictionary<IntPtr, WeakReference> mWindows = new Dictionary<IntPtr, WeakReference>((IEqualityComparer<IntPtr>) new IntPtrEqualityComparer());
    private CarbonWindowInfo window;
    private CarbonInput mInputDriver;
    private IntPtr uppHandler;
    private Rectangle bounds;
    private Rectangle clientRectangle;
    private Rectangle windowedBounds;
    private bool mIsDisposed;
    private DisplayDevice mDisplayDevice;
    private WindowAttributes mWindowAttrib;
    private WindowClass mWindowClass;
    private int mTitlebarHeight;
    private WindowBorder windowBorder;
    private WindowState windowState;
    private bool mMouseIn;
    private bool mIsActive;
    private Icon mIcon;
    private float mouse_rel_x;
    private float mouse_rel_y;

    internal static Dictionary<IntPtr, WeakReference> WindowRefMap
    {
      get
      {
        return CarbonGLNative.mWindows;
      }
    }

    internal DisplayDevice TargetDisplayDevice
    {
      get
      {
        return this.mDisplayDevice;
      }
    }

    private bool IsDisposed
    {
      get
      {
        return this.mIsDisposed;
      }
    }

    private WindowPositionMethod WindowPositionMethod
    {
      get
      {
        return this.mPositionMethod;
      }
      set
      {
        this.mPositionMethod = value;
      }
    }

    public bool Exists
    {
      get
      {
        return this.mExists;
      }
    }

    public IWindowInfo WindowInfo
    {
      get
      {
        return (IWindowInfo) this.window;
      }
    }

    public bool IsIdle
    {
      get
      {
        return true;
      }
    }

    public IInputDriver InputDriver
    {
      get
      {
        return (IInputDriver) this.mInputDriver;
      }
    }

    public Icon Icon
    {
      get
      {
        return this.mIcon;
      }
      set
      {
        if (value == this.Icon)
          return;
        this.SetIcon(value);
        this.mIcon = value;
        this.IconChanged((object) this, EventArgs.Empty);
      }
    }

    public string Title
    {
      get
      {
        return this.title;
      }
      set
      {
        if (!(value != this.Title))
          return;
        API.SetWindowTitle(this.window.WindowRef, value);
        this.title = value;
        this.TitleChanged((object) this, EventArgs.Empty);
      }
    }

    public bool Visible
    {
      get
      {
        return API.IsWindowVisible(this.window.WindowRef);
      }
      set
      {
        if (value == this.Visible)
          return;
        if (value)
          this.Show();
        else
          this.Hide();
        this.VisibleChanged((object) this, EventArgs.Empty);
      }
    }

    public bool Focused
    {
      get
      {
        return this.mIsActive;
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
        this.Location = value.Location;
        this.Size = value.Size;
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
        this.SetLocation((short) value.X, (short) value.Y);
      }
    }

    public Size Size
    {
      get
      {
        return this.bounds.Size;
      }
      set
      {
        this.SetSize((short) value.Width, (short) value.Height);
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
        this.SetClientSize((short) value, (short) this.Height);
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
        this.SetClientSize((short) this.Width, (short) value);
      }
    }

    public int X
    {
      get
      {
        return this.ClientRectangle.X;
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
        return this.ClientRectangle.Y;
      }
      set
      {
        this.Location = new Point(this.X, value);
      }
    }

    public Rectangle ClientRectangle
    {
      get
      {
        return this.clientRectangle;
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
        return this.clientRectangle.Size;
      }
      set
      {
        API.SizeWindow(this.window.WindowRef, (short) value.Width, (short) value.Height, true);
        this.LoadSize();
        this.Resize((object) this, EventArgs.Empty);
      }
    }

    public bool CursorVisible
    {
      get
      {
        return CG.CursorIsVisible();
      }
      set
      {
        if (value)
        {
          int num1 = (int) CG.DisplayShowCursor(IntPtr.Zero);
          int num2 = (int) CG.AssociateMouseAndMouseCursorPosition(true);
        }
        else
        {
          int num1 = (int) CG.DisplayHideCursor(IntPtr.Zero);
          this.ResetMouseToWindowCenter();
          int num2 = (int) CG.AssociateMouseAndMouseCursorPosition(false);
        }
      }
    }

    public WindowState WindowState
    {
      get
      {
        if (this.windowState == WindowState.Fullscreen)
          return WindowState.Fullscreen;
        if (API.IsWindowCollapsed(this.window.WindowRef))
          return WindowState.Minimized;
        return API.IsWindowInStandardState(this.window.WindowRef) ? WindowState.Maximized : WindowState.Normal;
      }
      set
      {
        if (value == this.WindowState)
          return;
        WindowState windowState = this.WindowState;
        this.windowState = value;
        if (windowState == WindowState.Fullscreen)
        {
          this.window.GoWindowedHack = true;
        }
        else
        {
          if (windowState == WindowState.Minimized)
            API.CollapseWindow(this.window.WindowRef, false);
          this.SetCarbonWindowState();
        }
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
        if (this.windowBorder == value)
          return;
        this.windowBorder = value;
        if (this.windowBorder == WindowBorder.Resizable)
          API.ChangeWindowAttributes(this.window.WindowRef, WindowAttributes.FullZoom | WindowAttributes.Resizable, WindowAttributes.NoAttributes);
        else if (this.windowBorder == WindowBorder.Fixed)
          API.ChangeWindowAttributes(this.window.WindowRef, WindowAttributes.NoAttributes, WindowAttributes.FullZoom | WindowAttributes.Resizable);
        this.WindowBorderChanged((object) this, EventArgs.Empty);
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

    static CarbonGLNative()
    {
      Application.Initialize();
    }

    private CarbonGLNative()
      : this(WindowClass.Document, WindowAttributes.StandardDocument | WindowAttributes.StandardHandler | WindowAttributes.InWindowMenu | WindowAttributes.LiveResize)
    {
    }

    private CarbonGLNative(WindowClass @class, WindowAttributes attrib)
    {
      this.mWindowClass = @class;
      this.mWindowAttrib = attrib;
    }

    public CarbonGLNative(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
    {
      this.CreateNativeWindow(WindowClass.Document, WindowAttributes.StandardDocument | WindowAttributes.StandardHandler | WindowAttributes.InWindowMenu | WindowAttributes.LiveResize, new Rect((short) x, (short) y, (short) width, (short) height));
      this.mDisplayDevice = device;
    }

    ~CarbonGLNative()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.mIsDisposed)
        return;
      this.CursorVisible = true;
      API.DisposeWindow(this.window.WindowRef);
      this.mIsDisposed = true;
      this.mExists = false;
      int num = (int) CG.SetLocalEventsSuppressionInterval(0.25);
      if (disposing)
      {
        CarbonGLNative.mWindows.Remove(this.window.WindowRef);
        this.window.Dispose();
        this.window = (CarbonWindowInfo) null;
      }
      this.DisposeUPP();
      this.Disposed((object) this, EventArgs.Empty);
    }

    private void DisposeUPP()
    {
      int num = this.uppHandler != IntPtr.Zero ? 1 : 0;
      this.uppHandler = IntPtr.Zero;
    }

    private void CreateNativeWindow(WindowClass @class, WindowAttributes attrib, Rect r)
    {
      IntPtr newWindow = API.CreateNewWindow(@class, attrib, r);
      API.SetWindowTitle(newWindow, this.title);
      this.window = new CarbonWindowInfo(newWindow, true, false);
      this.SetLocation(r.X, r.Y);
      this.SetSize(r.Width, r.Height);
      CarbonGLNative.mWindows.Add(newWindow, new WeakReference((object) this));
      this.LoadSize();
      this.mTitlebarHeight = (int) API.GetWindowBounds(this.window.WindowRef, WindowRegionCode.TitleBarRegion).Height;
      this.ConnectEvents();
    }

    private void ConnectEvents()
    {
      this.mInputDriver = new CarbonInput();
      EventTypeSpec[] eventTypes = new EventTypeSpec[5]
      {
        new EventTypeSpec(EventClass.Window, WindowEventKind.WindowClose),
        new EventTypeSpec(EventClass.Window, WindowEventKind.WindowClosed),
        new EventTypeSpec(EventClass.Window, WindowEventKind.WindowBoundsChanged),
        new EventTypeSpec(EventClass.Window, WindowEventKind.WindowActivate),
        new EventTypeSpec(EventClass.Window, WindowEventKind.WindowDeactivate)
      };
      this.uppHandler = API.NewEventHandlerUPP(new MacOSEventHandler(CarbonGLNative.EventHandler));
      API.InstallWindowEventHandler(this.window.WindowRef, this.uppHandler, eventTypes, this.window.WindowRef, IntPtr.Zero);
      Application.WindowEventHandler = this;
    }

    private void Activate()
    {
      API.SelectWindow(this.window.WindowRef);
    }

    private void Show()
    {
      IntPtr parentWindow = IntPtr.Zero;
      API.ShowWindow(this.window.WindowRef);
      int num = (int) API.RepositionWindow(this.window.WindowRef, parentWindow, this.WindowPositionMethod);
      API.SelectWindow(this.window.WindowRef);
    }

    private void Hide()
    {
      API.HideWindow(this.window.WindowRef);
    }

    internal void SetFullscreen(AglContext context)
    {
      this.windowedBounds = this.bounds;
      int width;
      int height;
      context.SetFullScreen(this.window, out width, out height);
      this.clientRectangle.Size = new Size(width, height);
      this.bounds = this.mDisplayDevice.Bounds;
      this.windowState = WindowState.Fullscreen;
    }

    internal void UnsetFullscreen(AglContext context)
    {
      context.UnsetFullScreen(this.window);
      this.SetCarbonWindowState();
      this.SetSize((short) this.windowedBounds.Width, (short) this.windowedBounds.Height);
    }

    internal OSStatus DispatchEvent(IntPtr inCaller, IntPtr inEvent, EventInfo evt, IntPtr userData)
    {
      switch (evt.EventClass)
      {
        case EventClass.Keyboard:
          return this.ProcessKeyboardEvent(inCaller, inEvent, evt, userData);
        case EventClass.Mouse:
          return this.ProcessMouseEvent(inCaller, inEvent, evt, userData);
        case EventClass.Window:
          return this.ProcessWindowEvent(inCaller, inEvent, evt, userData);
        default:
          return OSStatus.EventNotHandled;
      }
    }

    protected static OSStatus EventHandler(IntPtr inCaller, IntPtr inEvent, IntPtr userData)
    {
      if (!CarbonGLNative.mWindows.ContainsKey(userData))
        return OSStatus.EventNotHandled;
      WeakReference weakReference = CarbonGLNative.mWindows[userData];
      if (!weakReference.IsAlive)
      {
        CarbonGLNative.mWindows.Remove(userData);
        return OSStatus.EventNotHandled;
      }
      else
      {
        CarbonGLNative carbonGlNative = (CarbonGLNative) weakReference.Target;
        if (carbonGlNative == null)
          return OSStatus.EventNotHandled;
        EventInfo evt = new EventInfo(inEvent);
        return carbonGlNative.DispatchEvent(inCaller, inEvent, evt, userData);
      }
    }

    private OSStatus ProcessKeyboardEvent(IntPtr inCaller, IntPtr inEvent, EventInfo evt, IntPtr userData)
    {
      MacOSKeyCode code = MacOSKeyCode.A;
      char charCode = char.MinValue;
      switch (evt.KeyboardEventKind)
      {
        case KeyboardEventKind.RawKeyDown:
        case KeyboardEventKind.RawKeyRepeat:
        case KeyboardEventKind.RawKeyUp:
          CarbonGLNative.GetCharCodes(inEvent, out code, out charCode);
          this.mKeyPressArgs.KeyChar = charCode;
          break;
      }
      switch (evt.KeyboardEventKind)
      {
        case KeyboardEventKind.RawKeyDown:
          Key index1;
          if (CarbonGLNative.Keymap.TryGetValue(code, out index1))
          {
            this.InputDriver.Keyboard[0][index1] = true;
            this.OnKeyPress(this.mKeyPressArgs);
          }
          return OSStatus.NoError;
        case KeyboardEventKind.RawKeyRepeat:
          if (!this.InputDriver.Keyboard[0].KeyRepeat)
            break;
          else
            goto case KeyboardEventKind.RawKeyDown;
        case KeyboardEventKind.RawKeyUp:
          Key index2;
          if (CarbonGLNative.Keymap.TryGetValue(code, out index2))
            this.InputDriver.Keyboard[0][index2] = false;
          return OSStatus.NoError;
        case KeyboardEventKind.RawKeyModifiersChanged:
          this.ProcessModifierKey(inEvent);
          return OSStatus.NoError;
      }
      return OSStatus.EventNotHandled;
    }

    private OSStatus ProcessWindowEvent(IntPtr inCaller, IntPtr inEvent, EventInfo evt, IntPtr userData)
    {
      switch (evt.WindowEventKind)
      {
        case WindowEventKind.WindowActivate:
          this.OnActivate();
          return OSStatus.EventNotHandled;
        case WindowEventKind.WindowDeactivate:
          this.OnDeactivate();
          return OSStatus.EventNotHandled;
        case WindowEventKind.WindowBoundsChanged:
          int width = this.Width;
          int height = this.Height;
          int x = this.X;
          int y = this.Y;
          this.LoadSize();
          if (x != this.X || y != this.Y)
            this.Move((object) this, EventArgs.Empty);
          if (width != this.Width || height != this.Height)
            this.Resize((object) this, EventArgs.Empty);
          return OSStatus.EventNotHandled;
        case WindowEventKind.WindowClose:
          CancelEventArgs e = new CancelEventArgs();
          this.OnClosing(e);
          return e.Cancel ? OSStatus.NoError : OSStatus.EventNotHandled;
        case WindowEventKind.WindowClosed:
          this.mExists = false;
          this.OnClosed();
          return OSStatus.NoError;
        default:
          return OSStatus.EventNotHandled;
      }
    }

    protected OSStatus ProcessMouseEvent(IntPtr inCaller, IntPtr inEvent, EventInfo evt, IntPtr userData)
    {
      HIPoint pt1 = new HIPoint();
      HIPoint pt2 = new HIPoint();
      IntPtr windowRef;
      int num = (int) API.GetEventWindowRef(inEvent, out windowRef);
      OSStatus errorCode = API.GetEventMouseLocation(inEvent, out pt2);
      if (this.windowState == WindowState.Fullscreen)
        pt1 = pt2;
      else if (this.CursorVisible)
      {
        errorCode = API.GetEventWindowMouseLocation(inEvent, out pt1);
        pt1.Y -= (float) this.mTitlebarHeight;
      }
      else
      {
        errorCode = API.GetEventMouseDelta(inEvent, out pt1);
        pt1.X += this.mouse_rel_x;
        pt1.Y += this.mouse_rel_y;
        pt1 = this.ConfineMouseToWindow(windowRef, pt1);
        this.ResetMouseToWindowCenter();
        this.mouse_rel_x = pt1.X;
        this.mouse_rel_y = pt1.Y;
      }
      if (errorCode != OSStatus.NoError && errorCode != OSStatus.EventParameterNotFound)
        throw new MacOSException(errorCode);
      Point pt3 = new Point((int) pt1.X, (int) pt1.Y);
      this.CheckEnterLeaveEvents(windowRef, pt3);
      switch (evt.MouseEventKind)
      {
        case MouseEventKind.MouseDown:
        case MouseEventKind.MouseUp:
          OpenTK.Platform.MacOS.Carbon.MouseButton eventMouseButton = API.GetEventMouseButton(inEvent);
          bool flag = evt.MouseEventKind == MouseEventKind.MouseDown;
          switch (eventMouseButton)
          {
            case OpenTK.Platform.MacOS.Carbon.MouseButton.Primary:
              this.InputDriver.Mouse[0][OpenTK.Input.MouseButton.Left] = flag;
              break;
            case OpenTK.Platform.MacOS.Carbon.MouseButton.Secondary:
              this.InputDriver.Mouse[0][OpenTK.Input.MouseButton.Right] = flag;
              break;
            case OpenTK.Platform.MacOS.Carbon.MouseButton.Tertiary:
              this.InputDriver.Mouse[0][OpenTK.Input.MouseButton.Middle] = flag;
              break;
          }
          return OSStatus.NoError;
        case MouseEventKind.MouseMoved:
        case MouseEventKind.MouseDragged:
          if (this.windowState == WindowState.Fullscreen)
          {
            if (pt3.X != this.InputDriver.Mouse[0].X || pt3.Y != this.InputDriver.Mouse[0].Y)
              this.InputDriver.Mouse[0].Position = pt3;
          }
          else
          {
            if ((double) pt1.Y < 0.0 || pt3.X == this.InputDriver.Mouse[0].X && pt3.Y == this.InputDriver.Mouse[0].Y)
              return OSStatus.EventNotHandled;
            this.InputDriver.Mouse[0].Position = pt3;
          }
          return OSStatus.EventNotHandled;
        case MouseEventKind.WheelMoved:
          this.InputDriver.Mouse[0].WheelPrecise += (float) API.GetEventMouseWheelDelta(inEvent);
          return OSStatus.NoError;
        default:
          return OSStatus.EventNotHandled;
      }
    }

    private void ResetMouseToWindowCenter()
    {
      Mouse.SetPosition((double) ((this.Bounds.Left + this.Bounds.Right) / 2), (double) ((this.Bounds.Top + this.Bounds.Bottom) / 2));
    }

    private void CheckEnterLeaveEvents(IntPtr eventWindowRef, Point pt)
    {
      if (this.window == null)
        return;
      bool flag = eventWindowRef == this.window.WindowRef;
      if (pt.Y < 0)
        flag = false;
      if (flag == this.mMouseIn)
        return;
      this.mMouseIn = flag;
      if (this.mMouseIn)
        this.OnMouseEnter();
      else
        this.OnMouseLeave();
    }

    private HIPoint ConfineMouseToWindow(IntPtr window, HIPoint client)
    {
      if ((double) client.X < 0.0)
        client.X = 0.0f;
      if ((double) client.X >= (double) this.Width)
        client.X = (float) (this.Width - 1);
      if ((double) client.Y < 0.0)
        client.Y = 0.0f;
      if ((double) client.Y >= (double) this.Height)
        client.Y = (float) (this.Height - 1);
      return client;
    }

    private static void GetCharCodes(IntPtr inEvent, out MacOSKeyCode code, out char charCode)
    {
      code = API.GetEventKeyboardKeyCode(inEvent);
      charCode = API.GetEventKeyboardChar(inEvent);
    }

    private void ProcessModifierKey(IntPtr inEvent)
    {
      MacOSKeyModifiers eventKeyModifiers = API.GetEventKeyModifiers(inEvent);
      bool flag1 = (eventKeyModifiers & MacOSKeyModifiers.CapsLock) != MacOSKeyModifiers.None;
      bool flag2 = (eventKeyModifiers & MacOSKeyModifiers.Control) != MacOSKeyModifiers.None;
      bool flag3 = (eventKeyModifiers & MacOSKeyModifiers.Command) != MacOSKeyModifiers.None;
      bool flag4 = (eventKeyModifiers & MacOSKeyModifiers.Option) != MacOSKeyModifiers.None;
      bool flag5 = (eventKeyModifiers & MacOSKeyModifiers.Shift) != MacOSKeyModifiers.None;
      KeyboardDevice keyboardDevice = this.InputDriver.Keyboard[0];
      if (keyboardDevice[Key.AltLeft] ^ flag4)
        keyboardDevice[Key.AltLeft] = flag4;
      if (keyboardDevice[Key.ShiftLeft] ^ flag5)
        keyboardDevice[Key.ShiftLeft] = flag5;
      if (keyboardDevice[Key.WinLeft] ^ flag3)
        keyboardDevice[Key.WinLeft] = flag3;
      if (keyboardDevice[Key.ControlLeft] ^ flag2)
        keyboardDevice[Key.ControlLeft] = flag2;
      if (!(keyboardDevice[Key.CapsLock] ^ flag1))
        return;
      keyboardDevice[Key.CapsLock] = flag1;
    }

    private Rect GetRegion()
    {
      return API.GetWindowBounds(this.window.WindowRef, WindowRegionCode.ContentRegion);
    }

    private void SetLocation(short x, short y)
    {
      if (this.windowState == WindowState.Fullscreen)
        return;
      API.MoveWindow(this.window.WindowRef, x, y, false);
    }

    private void SetSize(short width, short height)
    {
      if (this.WindowState == WindowState.Fullscreen)
        return;
      width -= (short) (this.bounds.Width - this.clientRectangle.Width);
      height -= (short) (this.bounds.Height - this.clientRectangle.Height);
      API.SizeWindow(this.window.WindowRef, width, height, true);
    }

    private void SetClientSize(short width, short height)
    {
      if (this.WindowState == WindowState.Fullscreen)
        return;
      API.SizeWindow(this.window.WindowRef, width, height, true);
    }

    private void LoadSize()
    {
      if (this.WindowState == WindowState.Fullscreen)
        return;
      Rect windowBounds1 = API.GetWindowBounds(this.window.WindowRef, WindowRegionCode.StructureRegion);
      this.bounds = new Rectangle((int) windowBounds1.X, (int) windowBounds1.Y, (int) windowBounds1.Width, (int) windowBounds1.Height);
      Rect windowBounds2 = API.GetWindowBounds(this.window.WindowRef, WindowRegionCode.GlobalPortRegion);
      this.clientRectangle = new Rectangle(0, 0, (int) windowBounds2.Width, (int) windowBounds2.Height);
    }

    public void ProcessEvents()
    {
      Application.ProcessEvents();
    }

    public Point PointToClient(Point point)
    {
      Rect windowBounds = API.GetWindowBounds(this.window.WindowRef, WindowRegionCode.ContentRegion);
      return new Point(point.X - (int) windowBounds.X, point.Y - (int) windowBounds.Y);
    }

    public Point PointToScreen(Point point)
    {
      Rect windowBounds = API.GetWindowBounds(this.window.WindowRef, WindowRegionCode.ContentRegion);
      return new Point(point.X + (int) windowBounds.X, point.Y + (int) windowBounds.Y);
    }

    private void SetIcon(Icon icon)
    {
      if (icon == null)
      {
        API.RestoreApplicationDockTileImage();
      }
      else
      {
        Bitmap bitmap = new Bitmap(128, 128);
        using (Graphics graphics = Graphics.FromImage((Image) bitmap))
          graphics.DrawImage((Image) icon.ToBitmap(), 0, 0, 128, 128);
        int num1 = 0;
        int length = bitmap.Width * bitmap.Height;
        IntPtr[] data = new IntPtr[length];
        for (int y = 0; y < bitmap.Height; ++y)
        {
          for (int x = 0; x < bitmap.Width; ++x)
          {
            int num2 = bitmap.GetPixel(x, y).ToArgb();
            if (BitConverter.IsLittleEndian)
            {
              byte num3 = (byte) (num2 >> 24 & (int) byte.MaxValue);
              byte num4 = (byte) (num2 >> 16 & (int) byte.MaxValue);
              byte num5 = (byte) (num2 >> 8 & (int) byte.MaxValue);
              byte num6 = (byte) (num2 & (int) byte.MaxValue);
              data[num1++] = (IntPtr) ((int) num3 + ((int) num4 << 8) + ((int) num5 << 16) + ((int) num6 << 24));
            }
            else
              data[num1++] = (IntPtr) num2;
          }
        }
        API.SetApplicationDockTileImage(API.CGImageCreate(128, 128, 8, 32, 512, API.CGColorSpaceCreateDeviceRGB(), 4U, API.CGDataProviderCreateWithData(IntPtr.Zero, data, length * 4, IntPtr.Zero), IntPtr.Zero, 0, 0));
      }
    }

    public void Close()
    {
      CancelEventArgs e = new CancelEventArgs();
      this.OnClosing(e);
      if (e.Cancel)
        return;
      this.OnClosed();
      this.Dispose();
    }

    private void SetCarbonWindowState()
    {
      switch (this.windowState)
      {
        case WindowState.Normal:
          if (this.WindowState == WindowState.Maximized)
          {
            CarbonPoint toIdealSize = new CarbonPoint();
            API.ZoomWindowIdeal(this.window.WindowRef, WindowPartCode.inZoomIn, ref toIdealSize);
            break;
          }
          else
            break;
        case WindowState.Minimized:
          API.CollapseWindow(this.window.WindowRef, true);
          break;
        case WindowState.Maximized:
          CarbonPoint toIdealSize1 = new CarbonPoint(9000, 9000);
          API.ZoomWindowIdeal(this.window.WindowRef, WindowPartCode.inZoomOut, ref toIdealSize1);
          break;
        case WindowState.Fullscreen:
          this.window.GoFullScreenHack = true;
          break;
      }
      this.WindowStateChanged((object) this, EventArgs.Empty);
      this.LoadSize();
      this.Resize((object) this, EventArgs.Empty);
    }

    private void OnKeyPress(KeyPressEventArgs keyPressArgs)
    {
      this.KeyPress((object) this, keyPressArgs);
    }

    private void OnWindowStateChanged()
    {
      this.WindowStateChanged((object) this, EventArgs.Empty);
    }

    protected virtual void OnClosing(CancelEventArgs e)
    {
      this.Closing((object) this, e);
    }

    protected virtual void OnClosed()
    {
      this.Closed((object) this, EventArgs.Empty);
    }

    private void OnMouseLeave()
    {
      this.MouseLeave((object) this, EventArgs.Empty);
    }

    private void OnMouseEnter()
    {
      this.MouseEnter((object) this, EventArgs.Empty);
    }

    private void OnActivate()
    {
      this.mIsActive = true;
      this.FocusedChanged((object) this, EventArgs.Empty);
    }

    private void OnDeactivate()
    {
      this.mIsActive = false;
      this.FocusedChanged((object) this, EventArgs.Empty);
    }
  }
}
