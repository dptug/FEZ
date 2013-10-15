// Type: SharpDX.DisposeBase
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public abstract class DisposeBase : IDisposable
  {
    public bool IsDisposed { get; private set; }

    public event EventHandler<EventArgs> Disposing;

    public event EventHandler<EventArgs> Disposed;

    ~DisposeBase()
    {
      this.CheckAndDispose(false);
    }

    public void Dispose()
    {
      this.CheckAndDispose(true);
    }

    private void CheckAndDispose(bool disposing)
    {
      if (this.IsDisposed)
        return;
      if (this.Disposing != null)
        this.Disposing((object) this, EventArgs.Empty);
      this.Dispose(disposing);
      if (disposing)
        GC.SuppressFinalize((object) this);
      this.IsDisposed = true;
      if (this.Disposed == null)
        return;
      this.Disposed((object) this, EventArgs.Empty);
    }

    protected abstract void Dispose(bool disposing);
  }
}
