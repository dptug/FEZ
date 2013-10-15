// Type: OpenTK.Platform.Windows.WinWindowInfo
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform;
using System;

namespace OpenTK.Platform.Windows
{
  internal sealed class WinWindowInfo : IWindowInfo, IDisposable
  {
    private IntPtr handle;
    private IntPtr dc;
    private WinWindowInfo parent;
    private bool disposed;

    public IntPtr WindowHandle
    {
      get
      {
        return this.handle;
      }
      set
      {
        this.handle = value;
      }
    }

    public WinWindowInfo Parent
    {
      get
      {
        return this.parent;
      }
      set
      {
        this.parent = value;
      }
    }

    public IntPtr DeviceContext
    {
      get
      {
        if (this.dc == IntPtr.Zero)
          this.dc = Functions.GetDC(this.WindowHandle);
        return this.dc;
      }
    }

    public WinWindowInfo()
    {
    }

    public WinWindowInfo(IntPtr handle, WinWindowInfo parent)
    {
      this.handle = handle;
      this.parent = parent;
    }

    ~WinWindowInfo()
    {
      this.Dispose(false);
    }

    public override string ToString()
    {
      return string.Format("Windows.WindowInfo: Handle {0}, Parent ({1})", (object) this.WindowHandle, this.Parent != null ? (object) this.Parent.ToString() : (object) "null");
    }

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      WinWindowInfo winWindowInfo = (WinWindowInfo) obj;
      if (winWindowInfo == null)
        return false;
      else
        return this.handle.Equals((object) winWindowInfo.handle);
    }

    public override int GetHashCode()
    {
      return this.handle.GetHashCode();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool manual)
    {
      if (this.disposed)
        return;
      if (this.dc != IntPtr.Zero)
        Functions.ReleaseDC(this.handle, this.dc);
      if (manual && this.parent != null)
        this.parent.Dispose();
      this.disposed = true;
    }
  }
}
