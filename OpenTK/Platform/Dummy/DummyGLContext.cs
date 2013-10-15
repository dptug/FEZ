// Type: OpenTK.Platform.Dummy.DummyGLContext
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Threading;

namespace OpenTK.Platform.Dummy
{
  internal sealed class DummyGLContext : DesktopGraphicsContext
  {
    private bool vsync;
    private int swap_interval;
    private static int handle_count;
    private Thread current_thread;

    public override bool IsCurrent
    {
      get
      {
        if (this.current_thread != null)
          return this.current_thread == Thread.CurrentThread;
        else
          return false;
      }
    }

    public override int SwapInterval
    {
      get
      {
        return this.swap_interval;
      }
      set
      {
        this.swap_interval = value;
      }
    }

    public DummyGLContext()
      : this(new ContextHandle(new IntPtr(++DummyGLContext.handle_count)))
    {
    }

    public DummyGLContext(ContextHandle handle)
    {
      this.Handle = handle;
      this.Mode = new GraphicsMode(new IntPtr?(new IntPtr(2)), (ColorFormat) 32, 16, 0, 0, (ColorFormat) 0, 2, false);
    }

    public void CreateContext(bool direct, IGraphicsContext source)
    {
      if (!(this.Handle == ContextHandle.Zero))
        return;
      ++DummyGLContext.handle_count;
      this.Handle = new ContextHandle((IntPtr) DummyGLContext.handle_count);
    }

    public override void SwapBuffers()
    {
    }

    public override void MakeCurrent(IWindowInfo info)
    {
      if (this.current_thread != null && Thread.CurrentThread != this.current_thread)
        throw new GraphicsContextException("Cannot make context current on two threads at the same time");
      if (info != null)
        this.current_thread = Thread.CurrentThread;
      else
        this.current_thread = (Thread) null;
    }

    public override IntPtr GetAddress(string function)
    {
      return IntPtr.Zero;
    }

    public override void Update(IWindowInfo window)
    {
    }

    public override void LoadAll()
    {
    }

    public override void Dispose()
    {
      this.IsDisposed = true;
    }
  }
}
