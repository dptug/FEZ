// Type: OpenTK.Platform.MacOS.CarbonWindowInfo
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform;
using OpenTK.Platform.MacOS.Carbon;
using System;

namespace OpenTK.Platform.MacOS
{
  internal sealed class CarbonWindowInfo : IWindowInfo, IDisposable
  {
    private IntPtr windowRef;
    private bool ownHandle;
    private bool disposed;
    private bool isControl;
    private bool goFullScreenHack;
    private bool goWindowedHack;

    internal IntPtr WindowRef
    {
      get
      {
        return this.windowRef;
      }
    }

    internal bool GoFullScreenHack
    {
      get
      {
        return this.goFullScreenHack;
      }
      set
      {
        this.goFullScreenHack = value;
      }
    }

    internal bool GoWindowedHack
    {
      get
      {
        return this.goWindowedHack;
      }
      set
      {
        this.goWindowedHack = value;
      }
    }

    public bool IsControl
    {
      get
      {
        return this.isControl;
      }
    }

    public CarbonWindowInfo(IntPtr windowRef, bool ownHandle, bool isControl)
    {
      this.windowRef = windowRef;
      this.ownHandle = ownHandle;
      this.isControl = isControl;
    }

    ~CarbonWindowInfo()
    {
      this.Dispose(false);
    }

    public override string ToString()
    {
      return string.Format("MacOS.CarbonWindowInfo: Handle {0}", (object) this.WindowRef);
    }

    public void Dispose()
    {
      this.Dispose(true);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      int num = disposing ? 1 : 0;
      if (this.ownHandle)
      {
        API.DisposeWindow(this.windowRef);
        this.windowRef = IntPtr.Zero;
      }
      this.disposed = true;
    }
  }
}
