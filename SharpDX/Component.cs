// Type: SharpDX.Component
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public abstract class Component : ComponentBase, IDisposable
  {
    protected DisposeCollector DisposeCollector { get; set; }

    internal bool IsAttached { get; set; }

    protected internal bool IsDisposed { get; private set; }

    protected internal bool IsDisposing { get; private set; }

    public event EventHandler<EventArgs> Disposing;

    protected internal Component()
    {
    }

    protected Component(string name)
      : base(name)
    {
    }

    public void Dispose()
    {
      if (this.IsDisposed)
        return;
      this.IsDisposing = true;
      EventHandler<EventArgs> eventHandler = this.Disposing;
      if (eventHandler != null)
        eventHandler((object) this, EventArgs.Empty);
      this.Dispose(true);
      this.IsDisposed = true;
    }

    protected virtual void Dispose(bool disposeManagedResources)
    {
      if (!disposeManagedResources)
        return;
      if (this.DisposeCollector != null)
        this.DisposeCollector.Dispose();
      this.DisposeCollector = (DisposeCollector) null;
    }

    protected internal T ToDispose<T>(T toDisposeArg)
    {
      if (object.ReferenceEquals((object) toDisposeArg, (object) null))
        return default (T);
      if (this.DisposeCollector == null)
        this.DisposeCollector = new DisposeCollector();
      return this.DisposeCollector.Collect<T>(toDisposeArg);
    }

    protected internal void RemoveAndDispose<T>(ref T objectToDispose)
    {
      if (object.ReferenceEquals((object) objectToDispose, (object) null) || this.DisposeCollector == null)
        return;
      this.DisposeCollector.RemoveAndDispose<T>(ref objectToDispose);
    }

    protected internal void RemoveToDispose<T>(T toDisposeArg)
    {
      if (object.ReferenceEquals((object) toDisposeArg, (object) null) || this.DisposeCollector == null)
        return;
      this.DisposeCollector.Remove<T>(toDisposeArg);
    }
  }
}
