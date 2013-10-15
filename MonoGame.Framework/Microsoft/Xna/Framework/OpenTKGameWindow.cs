// Type: Microsoft.Xna.Framework.OpenTKGameWindow
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Microsoft.Xna.Framework
{
  public class OpenTKGameWindow : GameWindow, IDisposable
  {
    private IntPtr _windowHandle = IntPtr.Zero;
    private bool _isResizable;
    private bool _isBorderless;
    private bool _isMouseHidden;
    private bool _isMouseInBounds;
    private DisplayOrientation _currentOrientation;
    private OpenTK.GameWindow window;
    protected Game game;
    private List<Microsoft.Xna.Framework.Input.Keys> keys;
    private OpenTK.Graphics.GraphicsContext backgroundContext;
    private WindowState windowState;
    private Rectangle clientBounds;
    private Rectangle targetBounds;
    private bool updateClientBounds;
    private bool disposed;

    internal Game Game
    {
      get
      {
        return this.game;
      }
      set
      {
        if (this.game == value)
          return;
        this.game = value;
      }
    }

    internal OpenTK.GameWindow Window
    {
      get
      {
        return this.window;
      }
    }

    public override IntPtr Handle
    {
      get
      {
        return this._windowHandle;
      }
    }

    public override string ScreenDeviceName
    {
      get
      {
        return this.window.Title;
      }
    }

    public override Rectangle ClientBounds
    {
      get
      {
        return this.clientBounds;
      }
    }

    public override bool AllowUserResizing
    {
      get
      {
        return this._isResizable;
      }
      set
      {
        if (this._isResizable == value)
          return;
        this._isResizable = value;
        if (this._isBorderless)
          return;
        this.window.WindowBorder = this._isResizable ? WindowBorder.Resizable : WindowBorder.Fixed;
      }
    }

    public override DisplayOrientation CurrentOrientation
    {
      get
      {
        return DisplayOrientation.LandscapeLeft;
      }
    }

    public override bool IsBorderless
    {
      get
      {
        return this._isBorderless;
      }
      set
      {
        if (this._isBorderless == value)
          return;
        this._isBorderless = value;
        if (this._isBorderless)
          this.window.WindowBorder = WindowBorder.Hidden;
        else
          this.window.WindowBorder = this._isResizable ? WindowBorder.Resizable : WindowBorder.Fixed;
      }
    }

    public OpenTKGameWindow()
    {
      this.Initialize();
    }

    ~OpenTKGameWindow()
    {
      this.Dispose(false);
    }

    protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
    {
    }

    private void OpenTkGameWindow_Closing(object sender, CancelEventArgs e)
    {
      this.Game.Exit();
    }

    private void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
    {
      Microsoft.Xna.Framework.Input.Keys keys = KeyboardUtil.ToXna(e.Key);
      if (!this.keys.Contains(keys))
        return;
      this.keys.Remove(keys);
    }

    private void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
    {
      if (e.Key == Key.F4 && this.keys.Contains(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
      {
        this.window.Close();
      }
      else
      {
        Microsoft.Xna.Framework.Input.Keys keys = KeyboardUtil.ToXna(e.Key);
        if (this.keys.Contains(keys))
          return;
        this.keys.Add(keys);
      }
    }

    private void OnResize(object sender, EventArgs e)
    {
      int width = this.window.ClientRectangle.Width;
      int height = this.window.ClientRectangle.Height;
      Rectangle rectangle = new Rectangle(0, 0, width, height);
      if (width <= 1 || height <= 1 || this.updateClientBounds)
        return;
      this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth = width;
      this.Game.GraphicsDevice.PresentationParameters.BackBufferHeight = height;
      this.Game.GraphicsDevice.Viewport = new Viewport(0, 0, width, height);
      this.clientBounds = rectangle;
      this.OnClientSizeChanged();
    }

    private void OnRenderFrame(object sender, FrameEventArgs e)
    {
      if (OpenTK.Graphics.GraphicsContext.CurrentContext == null || OpenTK.Graphics.GraphicsContext.CurrentContext.IsDisposed)
        return;
      if (!OpenTK.Graphics.GraphicsContext.CurrentContext.IsCurrent)
        this.window.MakeCurrent();
      this.UpdateWindowState();
    }

    private void UpdateWindowState()
    {
      if (!this.updateClientBounds)
        return;
      this.updateClientBounds = false;
      if (this.windowState == WindowState.Normal && this.window.WindowState == WindowState.Maximized || this.windowState == WindowState.Maximized && this.window.WindowState == WindowState.Normal)
        this.windowState = this.window.WindowState;
      else
        this.window.WindowState = this.windowState;
      WindowBorder windowBorder = !this._isBorderless ? (this._isResizable ? WindowBorder.Resizable : WindowBorder.Fixed) : WindowBorder.Hidden;
      if (windowBorder != this.window.WindowBorder && this.window.WindowState != WindowState.Fullscreen)
        this.window.WindowBorder = windowBorder;
      this.window.ClientRectangle = new System.Drawing.Rectangle(this.targetBounds.X, this.targetBounds.Y, this.targetBounds.Width, this.targetBounds.Height);
      DisplayDevice display = DisplayDevice.GetDisplay(DisplayIndex.Primary);
      this.window.X = (display.Width - this.window.Width) / 2;
      this.window.Y = (display.Height - this.window.Height) / 2;
    }

    private void OnUpdateFrame(object sender, FrameEventArgs e)
    {
      this.UpdateWindowState();
      if (this.Game == null)
        return;
      this.HandleInput();
      this.Game.Tick();
    }

    private void HandleInput()
    {
      Microsoft.Xna.Framework.Input.Keyboard.SetKeys(this.keys);
    }

    private void OnMouseEnter(object sender, EventArgs e)
    {
      this._isMouseInBounds = true;
      if (this.game.IsMouseVisible || this._isMouseHidden)
        return;
      this._isMouseHidden = true;
      Cursor.Hide();
    }

    private void OnMouseLeave(object sender, EventArgs e)
    {
      if (Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Released)
        return;
      this._isMouseInBounds = false;
      if (!this._isMouseHidden)
        return;
      this._isMouseHidden = false;
      Cursor.Show();
    }

    private void OnKeyPress(object sender, OpenTK.KeyPressEventArgs e)
    {
      this.OnTextInput(sender, new TextInputEventArgs(e.KeyChar));
    }

    private void Initialize()
    {
      OpenTK.Graphics.GraphicsContext.ShareContexts = true;
      GraphicsMode mode = new GraphicsMode((ColorFormat) DisplayDevice.Default.BitsPerPixel, 24, 8);
      this.window = new OpenTK.GameWindow(640, 480, mode);
      this.window.RenderFrame += new EventHandler<FrameEventArgs>(this.OnRenderFrame);
      this.window.UpdateFrame += new EventHandler<FrameEventArgs>(this.OnUpdateFrame);
      this.window.Closing += new EventHandler<CancelEventArgs>(this.OpenTkGameWindow_Closing);
      this.window.Resize += new EventHandler<EventArgs>(this.OnResize);
      this.window.Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(this.Keyboard_KeyDown);
      this.window.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(this.Keyboard_KeyUp);
      this.window.MouseEnter += new EventHandler<EventArgs>(this.OnMouseEnter);
      this.window.MouseLeave += new EventHandler<EventArgs>(this.OnMouseLeave);
      this.window.KeyPress += new EventHandler<OpenTK.KeyPressEventArgs>(this.OnKeyPress);
      this.window.Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
      this.updateClientBounds = false;
      this.clientBounds = new Rectangle(this.window.ClientRectangle.X, this.window.ClientRectangle.Y, this.window.ClientRectangle.Width, this.window.ClientRectangle.Height);
      this.windowState = this.window.WindowState;
      this._windowHandle = (IntPtr) this.window.WindowInfo.GetType().GetProperty("WindowHandle").GetValue((object) this.window.WindowInfo, (object[]) null);
      Threading.BackgroundContext = (IGraphicsContext) new OpenTK.Graphics.GraphicsContext(mode, this.window.WindowInfo);
      Threading.WindowInfo = this.window.WindowInfo;
      this.keys = new List<Microsoft.Xna.Framework.Input.Keys>();
      if (OpenTK.Graphics.GraphicsContext.CurrentContext == null || !OpenTK.Graphics.GraphicsContext.CurrentContext.IsCurrent)
        this.window.MakeCurrent();
      Microsoft.Xna.Framework.Input.Mouse.setWindows(this.window);
      this.AllowUserResizing = false;
    }

    protected override void SetTitle(string title)
    {
      this.window.Title = title;
    }

    internal void Run(double updateRate)
    {
      this.window.Run(updateRate);
    }

    internal void ToggleFullScreen()
    {
      this.windowState = this.windowState != WindowState.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;
      this.updateClientBounds = true;
    }

    internal void ChangeClientBounds(Rectangle clientBounds)
    {
      if (!(this.clientBounds != clientBounds))
        return;
      this.updateClientBounds = true;
      this.targetBounds = clientBounds;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
        ((OpenTK.NativeWindow) this.window).Dispose();
      if (Threading.BackgroundContext != null)
      {
        Threading.BackgroundContext.Dispose();
        Threading.BackgroundContext = (IGraphicsContext) null;
        Threading.WindowInfo = (IWindowInfo) null;
      }
      this.disposed = true;
    }

    public override void BeginScreenDeviceChange(bool willBeFullScreen)
    {
    }

    public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
    {
    }

    public void MouseVisibleToggled()
    {
      if (this.game.IsMouseVisible)
      {
        if (!this._isMouseHidden)
          return;
        Cursor.Show();
        this._isMouseHidden = false;
      }
      else
      {
        if (this._isMouseHidden || !this._isMouseInBounds)
          return;
        Cursor.Hide();
        this._isMouseHidden = true;
      }
    }
  }
}
