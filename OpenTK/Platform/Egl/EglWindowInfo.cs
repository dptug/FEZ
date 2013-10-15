// Type: OpenTK.Platform.Egl.EglWindowInfo
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using OpenTK.Platform;
using System;

namespace OpenTK.Platform.Egl
{
  internal class EglWindowInfo : IWindowInfo, IDisposable
  {
    private IntPtr handle;
    private IntPtr display;
    private IntPtr surface;
    private bool disposed;

    public IntPtr Handle
    {
      get
      {
        return this.handle;
      }
      private set
      {
        this.handle = value;
      }
    }

    public IntPtr Display
    {
      get
      {
        return this.display;
      }
      private set
      {
        this.display = value;
      }
    }

    public IntPtr Surface
    {
      get
      {
        return this.surface;
      }
      private set
      {
        this.surface = value;
      }
    }

    public EglWindowInfo(IntPtr handle, IntPtr display)
    {
      this.Handle = handle;
      this.Display = display;
    }

    public EglWindowInfo(IntPtr handle, IntPtr display, IntPtr surface)
    {
      this.Handle = handle;
      this.Display = display;
      this.Surface = surface;
    }

    ~EglWindowInfo()
    {
      this.Dispose(false);
    }

    public void CreateWindowSurface(IntPtr config)
    {
      this.Surface = Egl.CreateWindowSurface(this.Display, config, this.Handle, (int[]) null);
      if (this.Surface == IntPtr.Zero)
        throw new GraphicsContextException(string.Format("[Error] Failed to create EGL window surface, error {0}.", (object) Egl.GetError()));
    }

    public void DestroySurface()
    {
      if (!(this.Surface != IntPtr.Zero))
        return;
      Egl.DestroySurface(this.Display, this.Surface);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool manual)
    {
      if (this.disposed || !manual)
        return;
      this.DestroySurface();
      this.disposed = true;
    }
  }
}
