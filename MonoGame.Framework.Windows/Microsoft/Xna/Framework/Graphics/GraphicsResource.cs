// Type: Microsoft.Xna.Framework.Graphics.GraphicsResource
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  public abstract class GraphicsResource : IDisposable
  {
    private static object resourcesLock = new object();
    private static List<WeakReference> resources = new List<WeakReference>();
    private bool disposed;
    private WeakReference graphicsDevice;

    public GraphicsDevice GraphicsDevice
    {
      get
      {
        return this.graphicsDevice == null || !this.graphicsDevice.IsAlive ? (GraphicsDevice) null : this.graphicsDevice.Target as GraphicsDevice;
      }
      internal set
      {
        this.graphicsDevice = new WeakReference((object) value);
      }
    }

    public bool IsDisposed
    {
      get
      {
        return this.disposed;
      }
    }

    public string Name { get; set; }

    public object Tag { get; set; }

    public event EventHandler<EventArgs> Disposing;

    static GraphicsResource()
    {
    }

    internal GraphicsResource()
    {
      lock (GraphicsResource.resourcesLock)
        GraphicsResource.resources.Add(new WeakReference((object) this));
    }

    ~GraphicsResource()
    {
      this.Dispose(false);
    }

    protected internal virtual void GraphicsDeviceResetting()
    {
    }

    internal static void DoGraphicsDeviceResetting()
    {
      lock (GraphicsResource.resourcesLock)
      {
        foreach (WeakReference item_0 in GraphicsResource.resources)
        {
          if (item_0.IsAlive)
            (item_0.Target as GraphicsResource).GraphicsDeviceResetting();
        }
        GraphicsResource.resources.Clear();
      }
    }

    internal static void DisposeAll()
    {
      lock (GraphicsResource.resourcesLock)
      {
        foreach (WeakReference item_0 in GraphicsResource.resources)
        {
          if (item_0.IsAlive)
            (item_0.Target as IDisposable).Dispose();
        }
        GraphicsResource.resources.Clear();
      }
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
      if (!disposing)
        ;
      if (disposing && this.Disposing != null)
        this.Disposing((object) this, EventArgs.Empty);
      lock (GraphicsResource.resourcesLock)
        GraphicsResource.resources.Remove(new WeakReference((object) this));
      this.disposed = true;
    }
  }
}
