// Type: OpenTK.Graphics.GraphicsContextBase
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Platform;
using System;

namespace OpenTK.Graphics
{
  internal abstract class GraphicsContextBase : IGraphicsContext, IDisposable, IGraphicsContextInternal
  {
    private bool disposed;
    protected ContextHandle Handle;
    protected GraphicsMode Mode;

    public abstract bool IsCurrent { get; }

    public bool IsDisposed
    {
      get
      {
        return this.disposed;
      }
      protected set
      {
        this.disposed = value;
      }
    }

    public bool VSync
    {
      get
      {
        return this.SwapInterval > 0;
      }
      set
      {
        if (value && this.SwapInterval <= 0)
        {
          this.SwapInterval = 1;
        }
        else
        {
          if (value || this.SwapInterval <= 0)
            return;
          this.SwapInterval = 0;
        }
      }
    }

    public abstract int SwapInterval { get; set; }

    public GraphicsMode GraphicsMode
    {
      get
      {
        return this.Mode;
      }
    }

    public bool ErrorChecking
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public IGraphicsContext Implementation
    {
      get
      {
        return (IGraphicsContext) this;
      }
    }

    public ContextHandle Context
    {
      get
      {
        return this.Handle;
      }
    }

    public abstract void SwapBuffers();

    public abstract void MakeCurrent(IWindowInfo window);

    public virtual void Update(IWindowInfo window)
    {
    }

    public abstract void LoadAll();

    public abstract IntPtr GetAddress(string function);

    public abstract void Dispose();
  }
}
