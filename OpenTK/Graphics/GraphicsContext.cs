// Type: OpenTK.Graphics.GraphicsContext
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Platform;
using OpenTK.Platform.Dummy;
using System;
using System.Collections.Generic;

namespace OpenTK.Graphics
{
  public sealed class GraphicsContext : IGraphicsContext, IDisposable, IGraphicsContextInternal
  {
    private static bool share_contexts = true;
    private static bool direct_rendering = true;
    private static readonly object SyncRoot = new object();
    private static readonly Dictionary<ContextHandle, WeakReference> available_contexts = new Dictionary<ContextHandle, WeakReference>();
    internal static GraphicsContext.GetCurrentContextDelegate GetCurrentContext = Factory.Default.CreateGetCurrentGraphicsContext();
    private bool check_errors = true;
    private IGraphicsContext implementation;
    private bool disposed;
    private readonly bool IsExternal;

    public static IGraphicsContext CurrentContext
    {
      get
      {
        lock (GraphicsContext.SyncRoot)
        {
          if (GraphicsContext.available_contexts.Count > 0)
          {
            ContextHandle local_0 = GraphicsContext.GetCurrentContext();
            if (local_0.Handle != IntPtr.Zero)
              return (IGraphicsContext) GraphicsContext.available_contexts[local_0].Target;
          }
          return (IGraphicsContext) null;
        }
      }
    }

    public static bool ShareContexts
    {
      get
      {
        return GraphicsContext.share_contexts;
      }
      set
      {
        GraphicsContext.share_contexts = value;
      }
    }

    public static bool DirectRendering
    {
      get
      {
        return GraphicsContext.direct_rendering;
      }
      set
      {
        GraphicsContext.direct_rendering = value;
      }
    }

    public bool ErrorChecking
    {
      get
      {
        return this.check_errors;
      }
      set
      {
        this.check_errors = value;
      }
    }

    public bool IsCurrent
    {
      get
      {
        return this.implementation.IsCurrent;
      }
    }

    public bool IsDisposed
    {
      get
      {
        if (this.disposed)
          return this.implementation.IsDisposed;
        else
          return false;
      }
      private set
      {
        this.disposed = value;
      }
    }

    [Obsolete("Use SwapInterval property instead.")]
    public bool VSync
    {
      get
      {
        return this.implementation.VSync;
      }
      set
      {
        this.implementation.VSync = value;
      }
    }

    public int SwapInterval
    {
      get
      {
        return this.implementation.SwapInterval;
      }
      set
      {
        this.implementation.SwapInterval = value;
      }
    }

    IGraphicsContext IGraphicsContextInternal.Implementation
    {
      get
      {
        return this.implementation;
      }
    }

    ContextHandle IGraphicsContextInternal.Context
    {
      get
      {
        return ((IGraphicsContextInternal) this.implementation).Context;
      }
    }

    public GraphicsMode GraphicsMode
    {
      get
      {
        return this.implementation.GraphicsMode;
      }
    }

    static GraphicsContext()
    {
    }

    private GraphicsContext(ContextHandle handle)
    {
      this.implementation = (IGraphicsContext) new DummyGLContext(handle);
      lock (GraphicsContext.SyncRoot)
        GraphicsContext.available_contexts.Add((this.implementation as IGraphicsContextInternal).Context, new WeakReference((object) this));
    }

    public GraphicsContext(GraphicsMode mode, IWindowInfo window)
      : this(mode, window, 1, 0, GraphicsContextFlags.Default)
    {
    }

    public GraphicsContext(GraphicsMode mode, IWindowInfo window, int major, int minor, GraphicsContextFlags flags)
    {
      lock (GraphicsContext.SyncRoot)
      {
        bool local_0 = false;
        if (mode == null && window == null)
        {
          local_0 = true;
        }
        else
        {
          if (mode == null)
            throw new ArgumentNullException("mode", "Must be a valid GraphicsMode.");
          if (window == null)
            throw new ArgumentNullException("window", "Must point to a valid window.");
        }
        if (major <= 0)
          major = 1;
        if (minor < 0)
          minor = 0;
        IGraphicsContext local_1_1 = GraphicsContext.FindSharedContext();
        if (local_0)
        {
          this.implementation = (IGraphicsContext) new DummyGLContext();
        }
        else
        {
          IPlatformFactory local_2 = (IPlatformFactory) null;
          switch ((flags & GraphicsContextFlags.Embedded) == GraphicsContextFlags.Embedded)
          {
            case false:
              local_2 = Factory.Default;
              break;
            case true:
              local_2 = Factory.Embedded;
              break;
          }
          this.implementation = local_2.CreateGLContext(mode, window, local_1_1, GraphicsContext.direct_rendering, major, minor, flags);
          if (GraphicsContext.GetCurrentContext == null)
          {
            GraphicsContext.GetCurrentContextDelegate local_3 = local_2.CreateGetCurrentGraphicsContext();
            if (local_3 != null)
              GraphicsContext.GetCurrentContext = local_3;
          }
        }
        GraphicsContext.available_contexts.Add(this.Context, new WeakReference((object) this));
      }
    }

    public GraphicsContext(ContextHandle handle, IWindowInfo window)
      : this(handle, window, (IGraphicsContext) null, 1, 0, GraphicsContextFlags.Default)
    {
    }

    public GraphicsContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, int major, int minor, GraphicsContextFlags flags)
    {
      lock (GraphicsContext.SyncRoot)
      {
        this.IsExternal = true;
        if (handle == ContextHandle.Zero)
        {
          this.implementation = (IGraphicsContext) new DummyGLContext(handle);
        }
        else
        {
          if (GraphicsContext.available_contexts.ContainsKey(handle))
            throw new GraphicsContextException("Context already exists.");
          switch ((flags & GraphicsContextFlags.Embedded) == GraphicsContextFlags.Embedded)
          {
            case false:
              this.implementation = Factory.Default.CreateGLContext(handle, window, shareContext, GraphicsContext.direct_rendering, major, minor, flags);
              break;
            case true:
              this.implementation = Factory.Embedded.CreateGLContext(handle, window, shareContext, GraphicsContext.direct_rendering, major, minor, flags);
              break;
          }
        }
        GraphicsContext.available_contexts.Add((this.implementation as IGraphicsContextInternal).Context, new WeakReference((object) this));
        this.LoadAll();
      }
    }

    public override string ToString()
    {
      return this.Context.ToString();
    }

    public override int GetHashCode()
    {
      return this.Context.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is GraphicsContext)
        return this.Context == (obj as IGraphicsContextInternal).Context;
      else
        return false;
    }

    private static IGraphicsContext FindSharedContext()
    {
      if (GraphicsContext.ShareContexts)
      {
        foreach (WeakReference weakReference in GraphicsContext.available_contexts.Values)
        {
          IGraphicsContext graphicsContext = weakReference.Target as IGraphicsContext;
          if (graphicsContext != null)
            return graphicsContext;
        }
      }
      return (IGraphicsContext) null;
    }

    public static GraphicsContext CreateDummyContext()
    {
      ContextHandle handle = GraphicsContext.GetCurrentContext();
      if (handle == ContextHandle.Zero)
        throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");
      else
        return GraphicsContext.CreateDummyContext(handle);
    }

    public static GraphicsContext CreateDummyContext(ContextHandle handle)
    {
      if (handle == ContextHandle.Zero)
        throw new ArgumentOutOfRangeException("handle");
      else
        return new GraphicsContext(handle);
    }

    public static void Assert()
    {
      if (GraphicsContext.CurrentContext == null)
        throw new GraphicsContextMissingException();
    }

    private void CreateContext(bool direct, IGraphicsContext source)
    {
      lock (GraphicsContext.SyncRoot)
        GraphicsContext.available_contexts.Add(this.Context, new WeakReference((object) this));
    }

    public void SwapBuffers()
    {
      this.implementation.SwapBuffers();
    }

    public void MakeCurrent(IWindowInfo window)
    {
      this.implementation.MakeCurrent(window);
    }

    public void Update(IWindowInfo window)
    {
      this.implementation.Update(window);
    }

    public void LoadAll()
    {
      if (GraphicsContext.CurrentContext != this)
        throw new GraphicsContextException();
      this.implementation.LoadAll();
    }

    IntPtr IGraphicsContextInternal.GetAddress(string function)
    {
      return (this.implementation as IGraphicsContextInternal).GetAddress(function);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool manual)
    {
      if (this.IsDisposed)
        return;
      lock (GraphicsContext.SyncRoot)
        GraphicsContext.available_contexts.Remove(this.Context);
      if (manual && !this.IsExternal && this.implementation != null)
        this.implementation.Dispose();
      this.IsDisposed = true;
    }

    internal delegate ContextHandle GetCurrentContextDelegate();
  }
}
