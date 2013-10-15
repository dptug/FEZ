// Type: OpenTK.Platform.Egl.EglContext
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using System;

namespace OpenTK.Platform.Egl
{
  internal class EglContext : EmbeddedGraphicsContext
  {
    private int swap_interval = 1;
    private EglWindowInfo WindowInfo;

    private IntPtr HandleAsEGLContext
    {
      get
      {
        return this.Handle.Handle;
      }
      set
      {
        this.Handle = new ContextHandle(value);
      }
    }

    public override bool IsCurrent
    {
      get
      {
        return Egl.GetCurrentContext() == this.HandleAsEGLContext;
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
        if (!Egl.SwapInterval(this.WindowInfo.Display, value))
          return;
        this.swap_interval = value;
      }
    }

    public EglContext(GraphicsMode mode, EglWindowInfo window, IGraphicsContext sharedContext, int major, int minor, GraphicsContextFlags flags)
    {
      if (mode == null)
        throw new ArgumentNullException("mode");
      if (window == null)
        throw new ArgumentNullException("window");
      EglContext eglContext = (EglContext) sharedContext;
      int major1;
      int minor1;
      if (!Egl.Initialize(window.Display, out major1, out minor1))
        throw new GraphicsContextException(string.Format("Failed to initialize EGL, error {0}.", (object) Egl.GetError()));
      this.WindowInfo = window;
      this.Mode = new EglGraphicsMode().SelectGraphicsMode(mode.ColorFormat, mode.Depth, mode.Stencil, mode.Samples, mode.AccumulatorFormat, mode.Buffers, mode.Stereo, major > 1 ? RenderableFlags.ES2 : RenderableFlags.ES);
      if (!this.Mode.Index.HasValue)
        throw new GraphicsModeException("Invalid or unsupported GraphicsMode.");
      IntPtr config = this.Mode.Index.Value;
      if (window.Surface == IntPtr.Zero)
        window.CreateWindowSurface(config);
      int[] attrib_list = new int[3]
      {
        12440,
        major,
        12344
      };
      this.HandleAsEGLContext = Egl.CreateContext(window.Display, config, eglContext != null ? eglContext.HandleAsEGLContext : IntPtr.Zero, attrib_list);
      this.MakeCurrent((IWindowInfo) window);
    }

    public EglContext(ContextHandle handle, EglWindowInfo window, IGraphicsContext sharedContext, int major, int minor, GraphicsContextFlags flags)
    {
      if (handle == ContextHandle.Zero)
        throw new ArgumentException("handle");
      if (window == null)
        throw new ArgumentNullException("window");
      this.Handle = handle;
    }

    ~EglContext()
    {
      this.Dispose(false);
    }

    public override void SwapBuffers()
    {
      Egl.SwapBuffers(this.WindowInfo.Display, this.WindowInfo.Surface);
    }

    public override void MakeCurrent(IWindowInfo window)
    {
      if (window is EglWindowInfo)
        this.WindowInfo = (EglWindowInfo) window;
      Egl.MakeCurrent(this.WindowInfo.Display, this.WindowInfo.Surface, this.WindowInfo.Surface, this.HandleAsEGLContext);
    }

    public override IntPtr GetAddress(string function)
    {
      return Egl.GetProcAddress(function);
    }

    public override void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool manual)
    {
      if (this.IsDisposed)
        return;
      if (manual)
      {
        Egl.MakeCurrent(this.WindowInfo.Display, this.WindowInfo.Surface, this.WindowInfo.Surface, IntPtr.Zero);
        Egl.DestroyContext(this.WindowInfo.Display, this.HandleAsEGLContext);
      }
      this.IsDisposed = true;
    }
  }
}
