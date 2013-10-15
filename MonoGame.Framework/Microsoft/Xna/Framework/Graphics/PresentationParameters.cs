// Type: Microsoft.Xna.Framework.Graphics.PresentationParameters
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class PresentationParameters : IDisposable
  {
    private int backBufferHeight = GraphicsDeviceManager.DefaultBackBufferHeight;
    private int backBufferWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
    public const int DefaultPresentRate = 60;
    private DepthFormat depthStencilFormat;
    private SurfaceFormat backBufferFormat;
    private IntPtr deviceWindowHandle;
    private bool isFullScreen;
    private int multiSampleCount;
    private bool disposed;

    public SurfaceFormat BackBufferFormat
    {
      get
      {
        return this.backBufferFormat;
      }
      set
      {
        this.backBufferFormat = value;
      }
    }

    public int BackBufferHeight
    {
      get
      {
        return this.backBufferHeight;
      }
      set
      {
        this.backBufferHeight = value;
      }
    }

    public int BackBufferWidth
    {
      get
      {
        return this.backBufferWidth;
      }
      set
      {
        this.backBufferWidth = value;
      }
    }

    public Rectangle Bounds
    {
      get
      {
        return new Rectangle(0, 0, this.backBufferWidth, this.backBufferHeight);
      }
    }

    public IntPtr DeviceWindowHandle
    {
      get
      {
        return this.deviceWindowHandle;
      }
      set
      {
        this.deviceWindowHandle = value;
      }
    }

    public DepthFormat DepthStencilFormat
    {
      get
      {
        return this.depthStencilFormat;
      }
      set
      {
        this.depthStencilFormat = value;
      }
    }

    public bool IsFullScreen
    {
      get
      {
        return this.isFullScreen;
      }
      set
      {
        this.isFullScreen = value;
      }
    }

    public int MultiSampleCount
    {
      get
      {
        return this.multiSampleCount;
      }
      set
      {
        this.multiSampleCount = value;
      }
    }

    public PresentInterval PresentationInterval { get; set; }

    public DisplayOrientation DisplayOrientation { get; set; }

    public RenderTargetUsage RenderTargetUsage { get; set; }

    public PresentationParameters()
    {
      this.Clear();
    }

    ~PresentationParameters()
    {
      this.Dispose(false);
    }

    public void Clear()
    {
      this.backBufferFormat = SurfaceFormat.Color;
      this.backBufferWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
      this.backBufferHeight = GraphicsDeviceManager.DefaultBackBufferHeight;
      this.deviceWindowHandle = IntPtr.Zero;
      this.depthStencilFormat = DepthFormat.None;
      this.multiSampleCount = 0;
      this.PresentationInterval = PresentInterval.Default;
      this.DisplayOrientation = DisplayOrientation.Default;
    }

    public PresentationParameters Clone()
    {
      return new PresentationParameters()
      {
        backBufferFormat = this.backBufferFormat,
        backBufferHeight = this.backBufferHeight,
        backBufferWidth = this.backBufferWidth,
        deviceWindowHandle = this.deviceWindowHandle,
        disposed = this.disposed,
        IsFullScreen = this.IsFullScreen,
        depthStencilFormat = this.depthStencilFormat,
        multiSampleCount = this.multiSampleCount,
        PresentationInterval = this.PresentationInterval,
        DisplayOrientation = this.DisplayOrientation
      };
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
      this.disposed = true;
      int num = disposing ? 1 : 0;
    }
  }
}
