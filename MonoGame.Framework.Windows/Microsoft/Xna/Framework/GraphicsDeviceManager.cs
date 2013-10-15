// Type: Microsoft.Xna.Framework.GraphicsDeviceManager
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace Microsoft.Xna.Framework
{
  public class GraphicsDeviceManager : IGraphicsDeviceService, IDisposable, IGraphicsDeviceManager
  {
    public static readonly int DefaultBackBufferHeight = 480;
    public static readonly int DefaultBackBufferWidth = 800;
    private bool _synchronizedWithVerticalRetrace = true;
    private Game _game;
    private GraphicsDevice _graphicsDevice;
    private int _preferredBackBufferHeight;
    private int _preferredBackBufferWidth;
    private SurfaceFormat _preferredBackBufferFormat;
    private DepthFormat _preferredDepthStencilFormat;
    private bool _preferMultiSampling;
    private DisplayOrientation _supportedOrientations;
    private bool disposed;

    public GraphicsProfile GraphicsProfile { get; set; }

    public GraphicsDevice GraphicsDevice
    {
      get
      {
        return this._graphicsDevice;
      }
    }

    public bool IsFullScreen
    {
      get
      {
        return this._graphicsDevice.PresentationParameters.IsFullScreen;
      }
      set
      {
        this._graphicsDevice.PresentationParameters.IsFullScreen = value;
      }
    }

    public bool PreferMultiSampling
    {
      get
      {
        return this._preferMultiSampling;
      }
      set
      {
        this._preferMultiSampling = value;
      }
    }

    public SurfaceFormat PreferredBackBufferFormat
    {
      get
      {
        return this._preferredBackBufferFormat;
      }
      set
      {
        this._preferredBackBufferFormat = value;
      }
    }

    public int PreferredBackBufferHeight
    {
      get
      {
        return this._preferredBackBufferHeight;
      }
      set
      {
        this._preferredBackBufferHeight = value;
      }
    }

    public int PreferredBackBufferWidth
    {
      get
      {
        return this._preferredBackBufferWidth;
      }
      set
      {
        this._preferredBackBufferWidth = value;
      }
    }

    public DepthFormat PreferredDepthStencilFormat
    {
      get
      {
        return this._preferredDepthStencilFormat;
      }
      set
      {
        this._preferredDepthStencilFormat = value;
      }
    }

    public bool SynchronizeWithVerticalRetrace
    {
      get
      {
        return this._synchronizedWithVerticalRetrace;
      }
      set
      {
        this._synchronizedWithVerticalRetrace = value;
      }
    }

    public DisplayOrientation SupportedOrientations
    {
      get
      {
        return this._supportedOrientations;
      }
      set
      {
        this._supportedOrientations = value;
        this._game.Window.SetSupportedOrientations(this._supportedOrientations);
      }
    }

    public event EventHandler<EventArgs> DeviceCreated;

    public event EventHandler<EventArgs> DeviceDisposing;

    public event EventHandler<EventArgs> DeviceReset;

    public event EventHandler<EventArgs> DeviceResetting;

    public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

    static GraphicsDeviceManager()
    {
    }

    public GraphicsDeviceManager(Game game)
    {
      if (game == null)
        throw new ArgumentNullException("The game cannot be null!");
      this._game = game;
      this._supportedOrientations = DisplayOrientation.Default;
      this._preferredBackBufferHeight = GraphicsDeviceManager.DefaultBackBufferHeight;
      this._preferredBackBufferWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
      this._preferredBackBufferFormat = SurfaceFormat.Color;
      this._preferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
      this._synchronizedWithVerticalRetrace = true;
      if (game.Services.GetService(typeof (IGraphicsDeviceManager)) != null)
        throw new ArgumentException("Graphics Device Manager Already Present");
      game.Services.AddService(typeof (IGraphicsDeviceManager), (object) this);
      game.Services.AddService(typeof (IGraphicsDeviceService), (object) this);
      this.CreateDevice();
    }

    ~GraphicsDeviceManager()
    {
      this.Dispose(false);
    }

    public void CreateDevice()
    {
      this._graphicsDevice = new GraphicsDevice();
      this.Initialize();
      this.OnDeviceCreated(EventArgs.Empty);
    }

    public bool BeginDraw()
    {
      throw new NotImplementedException();
    }

    public void EndDraw()
    {
      throw new NotImplementedException();
    }

    internal void OnDeviceDisposing(EventArgs e)
    {
      this.Raise<EventArgs>(this.DeviceDisposing, e);
    }

    internal void OnDeviceResetting(EventArgs e)
    {
      this.Raise<EventArgs>(this.DeviceResetting, e);
    }

    public void OnDeviceReset(EventArgs e)
    {
      this.Raise<EventArgs>(this.DeviceReset, e);
      this.GraphicsDevice.OnDeviceReset();
    }

    internal void OnDeviceCreated(EventArgs e)
    {
      this.Raise<EventArgs>(this.DeviceCreated, e);
    }

    private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e) where TEventArgs : EventArgs
    {
      if (handler == null)
        return;
      handler((object) this, e);
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
      if (disposing && this._graphicsDevice != null)
      {
        this._graphicsDevice.Dispose();
        this._graphicsDevice = (GraphicsDevice) null;
      }
      this.disposed = true;
    }

    public void ApplyChanges()
    {
      if (this._graphicsDevice == null)
        return;
      this._game.ResizeWindow(false);
      TouchPanel.DisplayWidth = this._graphicsDevice.PresentationParameters.BackBufferWidth;
      TouchPanel.DisplayHeight = this._graphicsDevice.PresentationParameters.BackBufferHeight;
    }

    private void Initialize()
    {
      this._game.Window.SetSupportedOrientations(this._supportedOrientations);
      this._graphicsDevice.PresentationParameters.BackBufferFormat = this._preferredBackBufferFormat;
      this._graphicsDevice.PresentationParameters.BackBufferWidth = this._preferredBackBufferWidth;
      this._graphicsDevice.PresentationParameters.BackBufferHeight = this._preferredBackBufferHeight;
      this._graphicsDevice.PresentationParameters.DepthStencilFormat = this._preferredDepthStencilFormat;
      this._graphicsDevice.PresentationParameters.IsFullScreen = false;
      this._graphicsDevice.GraphicsProfile = this.GraphicsProfile;
      this._graphicsDevice.PresentationParameters.DeviceWindowHandle = this._game.Window.Handle;
      this._graphicsDevice.Initialize();
      TouchPanel.DisplayWidth = this._graphicsDevice.PresentationParameters.BackBufferWidth;
      TouchPanel.DisplayHeight = this._graphicsDevice.PresentationParameters.BackBufferHeight;
    }

    public void ToggleFullScreen()
    {
      this.IsFullScreen = !this.IsFullScreen;
    }

    internal void ResetClientBounds()
    {
    }
  }
}
