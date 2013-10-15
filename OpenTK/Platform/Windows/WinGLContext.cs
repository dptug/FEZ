// Type: OpenTK.Platform.Windows.WinGLContext
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  internal sealed class WinGLContext : DesktopGraphicsContext
  {
    private static readonly object LoadLock = new object();
    private static readonly object SyncRoot = new object();
    private const string opengl32Name = "OPENGL32.DLL";
    private static IntPtr opengl32Handle;
    private static bool wgl_loaded;
    private bool vsync_supported;

    public override bool IsCurrent
    {
      get
      {
        return Wgl.Imports.GetCurrentContext() == this.Handle.Handle;
      }
    }

    public override int SwapInterval
    {
      get
      {
        lock (WinGLContext.LoadLock)
        {
          if (this.vsync_supported)
            return Wgl.Ext.GetSwapInterval();
          else
            return 0;
        }
      }
      set
      {
        lock (WinGLContext.LoadLock)
        {
          if (!this.vsync_supported)
            return;
          Wgl.Ext.SwapInterval(value);
        }
      }
    }

    internal IntPtr DeviceContext
    {
      get
      {
        return Wgl.Imports.GetCurrentDC();
      }
    }

    static WinGLContext()
    {
      WinGLContext.Init();
    }

    public WinGLContext(GraphicsMode format, WinWindowInfo window, IGraphicsContext sharedContext, int major, int minor, GraphicsContextFlags flags)
    {
      lock (WinGLContext.SyncRoot)
      {
        if (window == null)
          throw new ArgumentNullException("window", "Must point to a valid window.");
        if (window.WindowHandle == IntPtr.Zero)
          throw new ArgumentException("window", "Must be a valid window.");
        this.Mode = format;
        this.SetGraphicsModePFD(format, window);
        lock (WinGLContext.LoadLock)
        {
          if (!WinGLContext.wgl_loaded)
          {
            ContextHandle local_0 = new ContextHandle(Wgl.Imports.CreateContext(window.DeviceContext));
            Wgl.Imports.MakeCurrent(window.DeviceContext, local_0.Handle);
            Wgl.LoadAll();
            Wgl.Imports.MakeCurrent(IntPtr.Zero, IntPtr.Zero);
            Wgl.Imports.DeleteContext(local_0.Handle);
            WinGLContext.wgl_loaded = true;
          }
          if (Wgl.Delegates.wglCreateContextAttribsARB != null)
          {
            try
            {
              List<int> local_1 = new List<int>();
              local_1.Add(8337);
              local_1.Add(major);
              local_1.Add(8338);
              local_1.Add(minor);
              if (flags != GraphicsContextFlags.Default)
              {
                local_1.Add(8340);
                local_1.Add((int) flags);
              }
              local_1.Add(0);
              local_1.Add(0);
              this.Handle = new ContextHandle(Wgl.Arb.CreateContextAttribs(window.DeviceContext, sharedContext != null ? (sharedContext as IGraphicsContextInternal).Context.Handle : IntPtr.Zero, local_1.ToArray()));
              int temp_84 = this.Handle == ContextHandle.Zero ? 1 : 0;
            }
            catch (EntryPointNotFoundException exception_0)
            {
            }
            catch (NullReferenceException exception_1)
            {
            }
          }
        }
        if (this.Handle == ContextHandle.Zero)
        {
          this.Handle = new ContextHandle(Wgl.Imports.CreateContext(window.DeviceContext));
          if (this.Handle == ContextHandle.Zero)
            this.Handle = new ContextHandle(Wgl.Imports.CreateContext(window.DeviceContext));
          if (this.Handle == ContextHandle.Zero)
            throw new GraphicsContextException(string.Format("Context creation failed. Wgl.CreateContext() error: {0}.", (object) Marshal.GetLastWin32Error()));
        }
        if (sharedContext == null)
          return;
        Marshal.GetLastWin32Error();
        Wgl.Imports.ShareLists((sharedContext as IGraphicsContextInternal).Context.Handle, this.Handle.Handle);
      }
    }

    public WinGLContext(ContextHandle handle, WinWindowInfo window, IGraphicsContext sharedContext, int major, int minor, GraphicsContextFlags flags)
    {
      if (handle == ContextHandle.Zero)
        throw new ArgumentException("handle");
      if (window == null)
        throw new ArgumentNullException("window");
      this.Handle = handle;
    }

    ~WinGLContext()
    {
      this.Dispose(false);
    }

    public override void SwapBuffers()
    {
      if (!Functions.SwapBuffers(this.DeviceContext))
        throw new GraphicsContextException(string.Format("Failed to swap buffers for context {0} current. Error: {1}", (object) this, (object) Marshal.GetLastWin32Error()));
    }

    public override void MakeCurrent(IWindowInfo window)
    {
      lock (WinGLContext.SyncRoot)
      {
        lock (WinGLContext.LoadLock)
        {
          bool local_0;
          if (window != null)
          {
            if (((WinWindowInfo) window).WindowHandle == IntPtr.Zero)
              throw new ArgumentException("window", "Must point to a valid window.");
            local_0 = Wgl.Imports.MakeCurrent(((WinWindowInfo) window).DeviceContext, this.Handle.Handle);
          }
          else
            local_0 = Wgl.Imports.MakeCurrent(IntPtr.Zero, IntPtr.Zero);
          if (!local_0)
            throw new GraphicsContextException(string.Format("Failed to make context {0} current. Error: {1}", (object) this, (object) Marshal.GetLastWin32Error()));
        }
      }
    }

    public override void LoadAll()
    {
      lock (WinGLContext.LoadLock)
      {
        Wgl.LoadAll();
        this.vsync_supported = Wgl.Arb.SupportsExtension(this, "WGL_EXT_swap_control") && Wgl.Load("wglGetSwapIntervalEXT") && Wgl.Load("wglSwapIntervalEXT");
      }
      base.LoadAll();
    }

    public override IntPtr GetAddress(string function_string)
    {
      return Wgl.Imports.GetProcAddress(function_string);
    }

    private void SetGraphicsModePFD(GraphicsMode mode, WinWindowInfo window)
    {
      if (!mode.Index.HasValue)
        throw new GraphicsModeException("Invalid or unsupported GraphicsMode.");
      if (window == null)
        throw new ArgumentNullException("window", "Must point to a valid window.");
      PixelFormatDescriptor formatDescriptor = new PixelFormatDescriptor();
      Functions.DescribePixelFormat(window.DeviceContext, (int) mode.Index.Value, (int) API.PixelFormatDescriptorSize, ref formatDescriptor);
      if (!Functions.SetPixelFormat(window.DeviceContext, (int) mode.Index.Value, ref formatDescriptor))
        throw new GraphicsContextException(string.Format("Requested GraphicsMode not available. SetPixelFormat error: {0}", (object) Marshal.GetLastWin32Error()));
    }

    internal static void Init()
    {
      lock (WinGLContext.SyncRoot)
      {
        if (!(WinGLContext.opengl32Handle == IntPtr.Zero))
          return;
        WinGLContext.opengl32Handle = Functions.LoadLibrary("OPENGL32.DLL");
        if (WinGLContext.opengl32Handle == IntPtr.Zero)
          throw new ApplicationException(string.Format("LoadLibrary(\"{0}\") call failed with code {1}", (object) "OPENGL32.DLL", (object) Marshal.GetLastWin32Error()));
      }
    }

    public override string ToString()
    {
      return this.Context.ToString();
    }

    public override void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool calledManually)
    {
      if (this.IsDisposed)
        return;
      if (calledManually)
        this.DestroyContext();
      this.IsDisposed = true;
    }

    private void DestroyContext()
    {
      if (!(this.Handle != ContextHandle.Zero))
        return;
      try
      {
        Wgl.Imports.DeleteContext(this.Handle.Handle);
      }
      catch (AccessViolationException ex)
      {
      }
      this.Handle = ContextHandle.Zero;
    }
  }
}
