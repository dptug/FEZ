// Type: OpenTK.Platform.X11.X11GLContext
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  internal sealed class X11GLContext : DesktopGraphicsContext
  {
    private int swap_interval = 1;
    private IntPtr display;
    private X11WindowInfo currentWindow;
    private bool vsync_supported;
    private bool glx_loaded;

    private IntPtr Display
    {
      get
      {
        return this.display;
      }
      set
      {
        if (value == IntPtr.Zero)
          throw new ArgumentOutOfRangeException();
        if (this.display != IntPtr.Zero)
          throw new InvalidOperationException("The display connection may not be changed after being set.");
        this.display = value;
      }
    }

    public override bool IsCurrent
    {
      get
      {
        using (new XLock(this.Display))
          return Glx.GetCurrentContext() == this.Handle.Handle;
      }
    }

    public override int SwapInterval
    {
      get
      {
        if (this.vsync_supported)
          return this.swap_interval;
        else
          return 0;
      }
      set
      {
        if (!this.vsync_supported)
          return;
        ErrorCode errorCode = ErrorCode.NO_ERROR;
        using (new XLock(this.Display))
          errorCode = Glx.Sgi.SwapInterval(value);
        if (errorCode != ErrorCode.NO_ERROR)
          return;
        this.swap_interval = value;
      }
    }

    public unsafe X11GLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shared, bool direct, int major, int minor, GraphicsContextFlags flags)
    {
      if (mode == null)
        throw new ArgumentNullException("mode");
      if (window == null)
        throw new ArgumentNullException("window");
      this.Mode = mode;
      this.Display = ((X11WindowInfo) window).Display;
      this.currentWindow = (X11WindowInfo) window;
      this.currentWindow.VisualInfo = this.SelectVisual(mode, this.currentWindow);
      ContextHandle contextHandle = shared != null ? (shared as IGraphicsContextInternal).Context : (ContextHandle) IntPtr.Zero;
      if (!this.glx_loaded)
      {
        XVisualInfo visualInfo = this.currentWindow.VisualInfo;
        IntPtr num = IntPtr.Zero;
        using (new XLock(this.Display))
        {
          num = Glx.CreateContext(this.Display, ref visualInfo, IntPtr.Zero, true);
          if (num == IntPtr.Zero)
            num = Glx.CreateContext(this.Display, ref visualInfo, IntPtr.Zero, false);
        }
        if (num != IntPtr.Zero)
        {
          new Glx().LoadEntryPoints();
          using (new XLock(this.Display))
            Glx.MakeCurrent(this.Display, IntPtr.Zero, IntPtr.Zero);
          this.glx_loaded = true;
        }
      }
      if (major * 10 + minor >= 30 && Glx.Delegates.glXCreateContextAttribsARB != null)
      {
        int fbount;
        IntPtr* numPtr = Glx.ChooseFBConfig(this.Display, this.currentWindow.Screen, new int[3]
        {
          32779,
          (int) mode.Index.Value,
          0
        }, out fbount);
        if (fbount > 0)
        {
          List<int> list = new List<int>();
          list.Add(8337);
          list.Add(major);
          list.Add(8338);
          list.Add(minor);
          if (flags != GraphicsContextFlags.Default)
          {
            list.Add(8340);
            list.Add((int) flags);
          }
          list.Add(0);
          list.Add(0);
          using (new XLock(this.Display))
          {
            this.Handle = new ContextHandle(Glx.Arb.CreateContextAttribs(this.Display, *numPtr, contextHandle.Handle, direct, list.ToArray()));
            if (this.Handle == ContextHandle.Zero)
              this.Handle = new ContextHandle(Glx.Arb.CreateContextAttribs(this.Display, *numPtr, contextHandle.Handle, !direct, list.ToArray()));
          }
          int num = this.Handle == ContextHandle.Zero ? 1 : 0;
          using (new XLock(this.Display))
            Functions.XFree((IntPtr) ((void*) numPtr));
        }
      }
      if (this.Handle == ContextHandle.Zero)
      {
        XVisualInfo visualInfo = this.currentWindow.VisualInfo;
        using (new XLock(this.Display))
        {
          this.Handle = new ContextHandle(Glx.CreateContext(this.Display, ref visualInfo, contextHandle.Handle, direct));
          if (this.Handle == ContextHandle.Zero)
            this.Handle = new ContextHandle(Glx.CreateContext(this.Display, ref visualInfo, IntPtr.Zero, !direct));
        }
      }
      if (!(this.Handle != ContextHandle.Zero))
        throw new GraphicsContextException("Failed to create OpenGL context. Glx.CreateContext call returned 0.");
      using (new XLock(this.Display))
        Glx.IsDirect(this.Display, this.Handle.Handle);
    }

    public X11GLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shared, bool direct, int major, int minor, GraphicsContextFlags flags)
    {
      if (handle == ContextHandle.Zero)
        throw new ArgumentException("handle");
      if (window == null)
        throw new ArgumentNullException("window");
      this.Handle = handle;
      this.currentWindow = (X11WindowInfo) window;
      this.Display = this.currentWindow.Display;
    }

    ~X11GLContext()
    {
      this.Dispose(false);
    }

    private XVisualInfo SelectVisual(GraphicsMode mode, X11WindowInfo currentWindow)
    {
      XVisualInfo template = new XVisualInfo();
      template.VisualID = mode.Index.Value;
      template.Screen = currentWindow.Screen;
      lock (API.Lock)
      {
        int local_1;
        IntPtr local_2 = Functions.XGetVisualInfo(this.Display, XVisualInfoMask.ID | XVisualInfoMask.Screen, ref template, out local_1);
        if (local_1 == 0)
          throw new GraphicsModeException(string.Format("Invalid GraphicsMode specified ({0}).", (object) mode));
        template = (XVisualInfo) Marshal.PtrToStructure(local_2, typeof (XVisualInfo));
        Functions.XFree(local_2);
      }
      return template;
    }

    private bool SupportsExtension(X11WindowInfo window, string e)
    {
      if (window == null)
        throw new ArgumentNullException("window");
      if (e == null)
        throw new ArgumentNullException("e");
      if (window.Display != this.Display)
        throw new InvalidOperationException();
      string str = (string) null;
      using (new XLock(this.Display))
        str = Glx.QueryExtensionsString(this.Display, window.Screen);
      if (!string.IsNullOrEmpty(str))
        return str.Contains(e);
      else
        return false;
    }

    public override void SwapBuffers()
    {
      if (this.Display == IntPtr.Zero || this.currentWindow.WindowHandle == IntPtr.Zero)
        throw new InvalidOperationException(string.Format("Window is invalid. Display ({0}), Handle ({1}).", (object) this.Display, (object) this.currentWindow.WindowHandle));
      using (new XLock(this.Display))
        Glx.SwapBuffers(this.Display, this.currentWindow.WindowHandle);
    }

    public override void MakeCurrent(IWindowInfo window)
    {
      if (window == this.currentWindow && this.IsCurrent)
        return;
      if (window != null && ((X11WindowInfo) window).Display != this.Display)
        throw new InvalidOperationException("MakeCurrent() may only be called on windows originating from the same display that spawned this GL context.");
      if (window == null)
      {
        using (new XLock(this.Display))
        {
          if (Glx.MakeCurrent(this.Display, IntPtr.Zero, IntPtr.Zero))
            this.currentWindow = (X11WindowInfo) null;
        }
      }
      else
      {
        X11WindowInfo x11WindowInfo = (X11WindowInfo) window;
        if (this.Display == IntPtr.Zero || x11WindowInfo.WindowHandle == IntPtr.Zero || this.Handle == ContextHandle.Zero)
          throw new InvalidOperationException("Invalid display, window or context.");
        bool flag;
        using (new XLock(this.Display))
        {
          flag = Glx.MakeCurrent(this.Display, x11WindowInfo.WindowHandle, this.Handle);
          if (flag)
            this.currentWindow = x11WindowInfo;
        }
        if (!flag)
          throw new GraphicsContextException("Failed to make context current.");
      }
      this.currentWindow = (X11WindowInfo) window;
    }

    public override IntPtr GetAddress(string function)
    {
      using (new XLock(this.Display))
        return Glx.GetProcAddress(function);
    }

    public override void LoadAll()
    {
      new Glx().LoadEntryPoints();
      this.vsync_supported = this.GetAddress("glXSwapIntervalSGI") != IntPtr.Zero;
      base.LoadAll();
    }

    public override void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool manuallyCalled)
    {
      if (!this.IsDisposed && manuallyCalled)
      {
        IntPtr display = this.Display;
        if (this.IsCurrent)
        {
          using (new XLock(display))
            Glx.MakeCurrent(display, IntPtr.Zero, IntPtr.Zero);
        }
        using (new XLock(display))
          Glx.DestroyContext(display, this.Handle);
      }
      this.IsDisposed = true;
    }
  }
}
