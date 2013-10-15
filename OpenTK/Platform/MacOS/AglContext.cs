// Type: OpenTK.Platform.MacOS.AglContext
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.MacOS.Carbon;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS
{
  internal class AglContext : DesktopGraphicsContext
  {
    private const string Library = "libdl.dylib";
    private GraphicsMode graphics_mode;
    private CarbonWindowInfo carbonWindow;
    private IntPtr shareContextRef;
    private DisplayDevice device;
    private bool mIsFullscreen;
    private bool firstFullScreen;
    private bool firstSwap;

    public override bool IsCurrent
    {
      get
      {
        return this.Handle.Handle == Agl.aglGetCurrentContext();
      }
    }

    public override int SwapInterval
    {
      get
      {
        int num = 0;
        if (Agl.aglGetInteger(this.Handle.Handle, Agl.ParameterNames.AGL_SWAP_INTERVAL, out num))
          return num;
        this.MyAGLReportError("aglGetInteger");
        return 0;
      }
      set
      {
        if (Agl.aglSetInteger(this.Handle.Handle, Agl.ParameterNames.AGL_SWAP_INTERVAL, ref value))
          return;
        this.MyAGLReportError("aglSetInteger");
      }
    }

    public AglContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext)
    {
      this.graphics_mode = mode;
      this.carbonWindow = (CarbonWindowInfo) window;
      if (shareContext is AglContext)
        this.shareContextRef = ((GraphicsContextBase) shareContext).Handle.Handle;
      if (shareContext is GraphicsContext)
        this.shareContextRef = (shareContext != null ? (shareContext as IGraphicsContextInternal).Context : (ContextHandle) IntPtr.Zero).Handle;
      int num = this.shareContextRef == IntPtr.Zero ? 1 : 0;
      this.CreateContext(mode, this.carbonWindow, this.shareContextRef, true);
    }

    public AglContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext)
    {
      if (handle == ContextHandle.Zero)
        throw new ArgumentException("handle");
      if (window == null)
        throw new ArgumentNullException("window");
      this.Handle = handle;
      this.carbonWindow = (CarbonWindowInfo) window;
    }

    ~AglContext()
    {
      this.Dispose(false);
    }

    private void AddPixelAttrib(List<int> aglAttributes, Agl.PixelFormatAttribute pixelFormatAttribute)
    {
      aglAttributes.Add((int) pixelFormatAttribute);
    }

    private void AddPixelAttrib(List<int> aglAttributes, Agl.PixelFormatAttribute pixelFormatAttribute, int value)
    {
      aglAttributes.Add((int) pixelFormatAttribute);
      aglAttributes.Add(value);
    }

    private void CreateContext(GraphicsMode mode, CarbonWindowInfo carbonWindow, IntPtr shareContextRef, bool fullscreen)
    {
      List<int> aglAttributes = new List<int>();
      this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_RGBA);
      this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_DOUBLEBUFFER);
      this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_RED_SIZE, mode.ColorFormat.Red);
      this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_GREEN_SIZE, mode.ColorFormat.Green);
      this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_BLUE_SIZE, mode.ColorFormat.Blue);
      this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_ALPHA_SIZE, mode.ColorFormat.Alpha);
      if (mode.Depth > 0)
        this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_DEPTH_SIZE, mode.Depth);
      if (mode.Stencil > 0)
        this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_STENCIL_SIZE, mode.Stencil);
      if (mode.AccumulatorFormat.BitsPerPixel > 0)
      {
        this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_ACCUM_RED_SIZE, mode.AccumulatorFormat.Red);
        this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_ACCUM_GREEN_SIZE, mode.AccumulatorFormat.Green);
        this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_ACCUM_BLUE_SIZE, mode.AccumulatorFormat.Blue);
        this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_ACCUM_ALPHA_SIZE, mode.AccumulatorFormat.Alpha);
      }
      if (mode.Samples > 1)
      {
        this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_SAMPLE_BUFFERS_ARB, 1);
        this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_SAMPLES_ARB, mode.Samples);
      }
      if (fullscreen)
        this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_FULLSCREEN);
      this.AddPixelAttrib(aglAttributes, Agl.PixelFormatAttribute.AGL_NONE);
      int num = 0;
      while (num < aglAttributes.Count)
        ++num;
      IntPtr pix;
      if (fullscreen)
      {
        IntPtr displayID = this.GetQuartzDevice(carbonWindow);
        if (displayID == IntPtr.Zero)
          displayID = (IntPtr) DisplayDevice.Default.Id;
        IntPtr displayDevice;
        OSStatus gdeviceByDisplayId = API.DMGetGDeviceByDisplayID(displayID, out displayDevice, false);
        if (gdeviceByDisplayId != OSStatus.NoError)
          throw new MacOSException(gdeviceByDisplayId, "DMGetGDeviceByDisplayID failed.");
        pix = Agl.aglChoosePixelFormat(ref displayDevice, 1, aglAttributes.ToArray());
        if (Agl.GetError() == Agl.AglError.BadPixelFormat)
        {
          this.CreateContext(mode, carbonWindow, shareContextRef, false);
          return;
        }
      }
      else
      {
        pix = Agl.aglChoosePixelFormat(IntPtr.Zero, 0, aglAttributes.ToArray());
        this.MyAGLReportError("aglChoosePixelFormat");
      }
      this.Handle = new ContextHandle(Agl.aglCreateContext(pix, shareContextRef));
      this.MyAGLReportError("aglCreateContext");
      Agl.aglDestroyPixelFormat(pix);
      this.MyAGLReportError("aglDestroyPixelFormat");
      this.SetDrawable(carbonWindow);
      this.SetBufferRect(carbonWindow);
      this.Update((IWindowInfo) carbonWindow);
      this.MakeCurrent((IWindowInfo) carbonWindow);
    }

    private IntPtr GetQuartzDevice(CarbonWindowInfo carbonWindow)
    {
      IntPtr windowRef = carbonWindow.WindowRef;
      if (!CarbonGLNative.WindowRefMap.ContainsKey(windowRef))
        return IntPtr.Zero;
      WeakReference weakReference = CarbonGLNative.WindowRefMap[windowRef];
      if (!weakReference.IsAlive)
        return IntPtr.Zero;
      CarbonGLNative carbonGlNative = weakReference.Target as CarbonGLNative;
      if (carbonGlNative == null)
        return IntPtr.Zero;
      else
        return QuartzDisplayDeviceDriver.HandleTo(carbonGlNative.TargetDisplayDevice);
    }

    private void SetBufferRect(CarbonWindowInfo carbonWindow)
    {
      if (carbonWindow.IsControl)
        throw new NotImplementedException();
    }

    private void SetDrawable(CarbonWindowInfo carbonWindow)
    {
      Agl.aglSetDrawable(this.Handle.Handle, AglContext.GetWindowPortForWindowInfo(carbonWindow));
      this.MyAGLReportError("aglSetDrawable");
    }

    private static IntPtr GetWindowPortForWindowInfo(CarbonWindowInfo carbonWindow)
    {
      return !carbonWindow.IsControl ? API.GetWindowPort(carbonWindow.WindowRef) : API.GetWindowPort(API.GetControlOwner(carbonWindow.WindowRef));
    }

    public override void Update(IWindowInfo window)
    {
      CarbonWindowInfo carbonWindow1 = (CarbonWindowInfo) window;
      if (carbonWindow1.GoFullScreenHack)
      {
        carbonWindow1.GoFullScreenHack = false;
        CarbonGLNative carbonWindow2 = this.GetCarbonWindow(carbonWindow1);
        if (carbonWindow2 == null)
          return;
        carbonWindow2.SetFullscreen(this);
      }
      else
      {
        if (carbonWindow1.GoWindowedHack)
        {
          carbonWindow1.GoWindowedHack = false;
          CarbonGLNative carbonWindow2 = this.GetCarbonWindow(carbonWindow1);
          if (carbonWindow2 != null)
            carbonWindow2.UnsetFullscreen(this);
        }
        if (this.mIsFullscreen)
          return;
        this.SetDrawable(carbonWindow1);
        this.SetBufferRect(carbonWindow1);
        int num = (int) Agl.aglUpdateContext(this.Handle.Handle);
      }
    }

    private CarbonGLNative GetCarbonWindow(CarbonWindowInfo carbonWindow)
    {
      WeakReference weakReference = CarbonGLNative.WindowRefMap[carbonWindow.WindowRef];
      if (weakReference.IsAlive)
        return (CarbonGLNative) weakReference.Target;
      else
        return (CarbonGLNative) null;
    }

    private void MyAGLReportError(string function)
    {
      Agl.AglError error = Agl.GetError();
      if (error != Agl.AglError.NoError)
        throw new MacOSException((OSStatus) error, string.Format("AGL Error from function {0}: {1}  {2}", (object) function, (object) error, (object) Agl.ErrorString(error)));
    }

    internal void SetFullScreen(CarbonWindowInfo info, out int width, out int height)
    {
      CarbonGLNative carbonWindow = this.GetCarbonWindow(info);
      int num = (int) CG.DisplayCapture(this.GetQuartzDevice(info));
      Agl.aglSetFullScreen(this.Handle.Handle, carbonWindow.TargetDisplayDevice.Width, carbonWindow.TargetDisplayDevice.Height, 0, 0);
      this.MakeCurrent((IWindowInfo) info);
      width = carbonWindow.TargetDisplayDevice.Width;
      height = carbonWindow.TargetDisplayDevice.Height;
      if (!this.firstFullScreen)
      {
        this.firstFullScreen = true;
        this.UnsetFullScreen(info);
        this.SetFullScreen(info, out width, out height);
      }
      this.mIsFullscreen = true;
    }

    internal void UnsetFullScreen(CarbonWindowInfo windowInfo)
    {
      Agl.aglSetDrawable(this.Handle.Handle, IntPtr.Zero);
      int num1 = (int) Agl.aglUpdateContext(this.Handle.Handle);
      int num2 = (int) CG.DisplayRelease(this.GetQuartzDevice(windowInfo));
      this.SetDrawable(windowInfo);
      this.mIsFullscreen = false;
    }

    public override void SwapBuffers()
    {
      if (!this.firstSwap && this.carbonWindow.IsControl)
      {
        this.firstSwap = true;
        this.SetDrawable(this.carbonWindow);
        this.Update((IWindowInfo) this.carbonWindow);
      }
      Agl.aglSwapBuffers(this.Handle.Handle);
      this.MyAGLReportError("aglSwapBuffers");
    }

    public override void MakeCurrent(IWindowInfo window)
    {
      if (Agl.aglSetCurrentContext(this.Handle.Handle))
        return;
      this.MyAGLReportError("aglSetCurrentContext");
    }

    public override void Dispose()
    {
      this.Dispose(true);
    }

    private void Dispose(bool disposing)
    {
      if (this.IsDisposed || this.Handle.Handle == IntPtr.Zero)
        return;
      Agl.aglSetCurrentContext(IntPtr.Zero);
      if (Agl.aglDestroyContext(this.Handle.Handle))
      {
        this.Handle = ContextHandle.Zero;
      }
      else
      {
        if (disposing)
          throw new MacOSException((OSStatus) Agl.GetError(), Agl.ErrorString(Agl.GetError()));
        this.IsDisposed = true;
      }
    }

    [DllImport("libdl.dylib")]
    private static bool NSIsSymbolNameDefined(string s);

    [DllImport("libdl.dylib")]
    private static IntPtr NSLookupAndBindSymbol(string s);

    [DllImport("libdl.dylib")]
    private static IntPtr NSAddressOfSymbol(IntPtr symbol);

    public override IntPtr GetAddress(string function)
    {
      string s = "_" + function;
      if (!AglContext.NSIsSymbolNameDefined(s))
        return IntPtr.Zero;
      IntPtr symbol = AglContext.NSLookupAndBindSymbol(s);
      if (symbol != IntPtr.Zero)
        symbol = AglContext.NSAddressOfSymbol(symbol);
      return symbol;
    }
  }
}
