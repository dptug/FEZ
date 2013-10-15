// Type: Microsoft.Xna.Framework.OpenTKGameWindow
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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

namespace Microsoft.Xna.Framework
{
  public class OpenTKGameWindow : GameWindow, IDisposable
  {
    private IntPtr _windowHandle = IntPtr.Zero;
    private bool _allowUserResizing;
    private DisplayOrientation _currentOrientation;
    private OpenTK.GameWindow window;
    protected Game game;
    private List<Keys> keys;
    private OpenTK.Graphics.GraphicsContext backgroundContext;
    private WindowState windowState;
    private Rectangle clientBounds;
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
        return this._allowUserResizing;
      }
      set
      {
        this._allowUserResizing = value;
        if (this._allowUserResizing)
          this.window.WindowBorder = WindowBorder.Resizable;
        else
          this.window.WindowBorder = WindowBorder.Fixed;
      }
    }

    public override DisplayOrientation CurrentOrientation
    {
      get
      {
        return DisplayOrientation.LandscapeLeft;
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
      Keys keys = KeyboardUtil.ToXna(e.Key);
      if (!this.keys.Contains(keys))
        return;
      this.keys.Remove(keys);
    }

    private void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
    {
      if (e.Key == Key.F4 && this.keys.Contains(Keys.LeftAlt))
      {
        this.window.Close();
      }
      else
      {
        Keys keys = KeyboardUtil.ToXna(e.Key);
        if (this.keys.Contains(keys))
          return;
        this.keys.Add(keys);
      }
    }

    private void OnResize(object sender, EventArgs e)
    {
      int width = this.window.ClientRectangle.Width;
      int height = this.window.ClientRectangle.Height;
      Rectangle clientBounds = new Rectangle(0, 0, width, height);
      if (width == 0 || height == 0 || this.updateClientBounds)
        return;
      this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth = width;
      this.Game.GraphicsDevice.PresentationParameters.BackBufferHeight = height;
      this.Game.GraphicsDevice.Viewport = new Viewport(0, 0, width, height);
      this.ChangeClientBounds(clientBounds);
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
      this.window.ClientRectangle = new System.Drawing.Rectangle(this.clientBounds.X, this.clientBounds.Y, this.clientBounds.Width, this.clientBounds.Height);
      this.updateClientBounds = false;
      DisplayDevice display = DisplayDevice.GetDisplay(DisplayIndex.Primary);
      this.window.X = (display.Width - this.window.Width) / 2;
      this.window.Y = (display.Height - this.window.Height) / 2;
      if (this.windowState == WindowState.Normal && this.window.WindowState == WindowState.Maximized || this.windowState == WindowState.Maximized && this.window.WindowState == WindowState.Normal)
        this.windowState = this.window.WindowState;
      else
        this.window.WindowState = this.windowState;
      WindowBorder windowBorder = this.AllowUserResizing ? WindowBorder.Resizable : WindowBorder.Fixed;
      if (windowBorder != this.window.WindowBorder && this.window.WindowState != WindowState.Fullscreen)
        this.window.WindowBorder = windowBorder;
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
      Microsoft.Xna.Framework.Input.Keyboard.State = new Microsoft.Xna.Framework.Input.KeyboardState(this.keys.ToArray());
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
      this.window.Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
      this.updateClientBounds = false;
      OpenTKGameWindow openTkGameWindow = this;
      System.Drawing.Rectangle clientRectangle = this.window.ClientRectangle;
      int x = clientRectangle.X;
      clientRectangle = this.window.ClientRectangle;
      int y = clientRectangle.Y;
      clientRectangle = this.window.ClientRectangle;
      int width = clientRectangle.Width;
      clientRectangle = this.window.ClientRectangle;
      int height = clientRectangle.Height;
      Rectangle rectangle = new Rectangle(x, y, width, height);
      openTkGameWindow.clientBounds = rectangle;
      this.windowState = this.window.WindowState;
      this._windowHandle = (IntPtr) this.window.WindowInfo.GetType().GetProperty("WindowHandle").GetValue((object) this.window.WindowInfo, (object[]) null);
      Threading.BackgroundContext = (IGraphicsContext) new OpenTK.Graphics.GraphicsContext(mode, this.window.WindowInfo);
      Threading.WindowInfo = this.window.WindowInfo;
      this.keys = new List<Keys>();
      if (!OpenTK.Graphics.GraphicsContext.CurrentContext.IsCurrent)
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
      if (this.windowState == WindowState.Fullscreen)
        this.windowState = WindowState.Normal;
      else
        this.windowState = WindowState.Fullscreen;
    }

    internal void ChangeClientBounds(Rectangle clientBounds)
    {
      if (this.updateClientBounds)
        return;
      this.updateClientBounds = true;
      this.clientBounds = clientBounds;
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
        ((NativeWindow) this.window).Dispose();
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
  }
}
