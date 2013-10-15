// Type: Microsoft.Xna.Framework.Graphics.GraphicsResource
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  public abstract class GraphicsResource : IDisposable
  {
    private static object resourcesLock = new object();
    private static List<WeakReference> resources = new List<WeakReference>();
    private bool disposed;
    private GraphicsDevice graphicsDevice;

    public GraphicsDevice GraphicsDevice
    {
      get
      {
        return this.graphicsDevice;
      }
      internal set
      {
        this.graphicsDevice = value;
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
          object local_1 = item_0.Target;
          if (local_1 != null)
            (local_1 as GraphicsResource).GraphicsDeviceResetting();
        }
        GraphicsResource.resources.RemoveAll((Predicate<WeakReference>) (wr => !wr.IsAlive));
      }
    }

    internal static void DisposeAll()
    {
      lock (GraphicsResource.resourcesLock)
      {
        foreach (WeakReference item_0 in GraphicsResource.resources)
        {
          object local_1 = item_0.Target;
          if (local_1 != null)
            (local_1 as IDisposable).Dispose();
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
      int num = disposing ? 1 : 0;
      if (disposing && this.Disposing != null)
        this.Disposing((object) this, EventArgs.Empty);
      lock (GraphicsResource.resourcesLock)
        GraphicsResource.resources.Remove(new WeakReference((object) this));
      this.graphicsDevice = (GraphicsDevice) null;
      this.disposed = true;
    }
  }
}
