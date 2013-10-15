// Type: OpenTK.Platform.X11.Glx
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenTK.Platform.X11
{
  internal class Glx : BindingsBase
  {
    private static readonly object sync_root = new object();
    private const string Library = "libGL.so.1";

    protected override object SyncRoot
    {
      get
      {
        return Glx.sync_root;
      }
    }

    static Glx()
    {
    }

    protected override IntPtr GetAddress(string funcname)
    {
      return Glx.GetProcAddress(funcname);
    }

    [DllImport("libGL.so.1", EntryPoint = "glXIsDirect")]
    public static bool IsDirect(IntPtr dpy, IntPtr context);

    [DllImport("libGL.so.1", EntryPoint = "glXQueryExtension")]
    public static bool QueryExtension(IntPtr dpy, ref int errorBase, ref int eventBase);

    [DllImport("libGL.so.1", EntryPoint = "glXQueryExtensionsString")]
    private static IntPtr QueryExtensionsStringInternal(IntPtr dpy, int screen);

    public static string QueryExtensionsString(IntPtr dpy, int screen)
    {
      return Marshal.PtrToStringAnsi(Glx.QueryExtensionsStringInternal(dpy, screen));
    }

    [DllImport("libGL.so.1", EntryPoint = "glXCreateContext")]
    public static IntPtr CreateContext(IntPtr dpy, IntPtr vis, IntPtr shareList, bool direct);

    [DllImport("libGL.so.1", EntryPoint = "glXCreateContext")]
    public static IntPtr CreateContext(IntPtr dpy, ref XVisualInfo vis, IntPtr shareList, bool direct);

    [DllImport("libGL.so.1", EntryPoint = "glXDestroyContext")]
    public static void DestroyContext(IntPtr dpy, IntPtr context);

    public static void DestroyContext(IntPtr dpy, ContextHandle context)
    {
      Glx.DestroyContext(dpy, context.Handle);
    }

    [DllImport("libGL.so.1", EntryPoint = "glXGetCurrentContext")]
    public static IntPtr GetCurrentContext();

    [DllImport("libGL.so.1", EntryPoint = "glXMakeCurrent")]
    public static bool MakeCurrent(IntPtr display, IntPtr drawable, IntPtr context);

    public static bool MakeCurrent(IntPtr display, IntPtr drawable, ContextHandle context)
    {
      return Glx.MakeCurrent(display, drawable, context.Handle);
    }

    [DllImport("libGL.so.1", EntryPoint = "glXSwapBuffers")]
    public static void SwapBuffers(IntPtr display, IntPtr drawable);

    [DllImport("libGL.so.1", EntryPoint = "glXGetProcAddress")]
    public static IntPtr GetProcAddress([MarshalAs(UnmanagedType.LPTStr)] string procName);

    [DllImport("libGL.so.1", EntryPoint = "glXGetConfig")]
    public static int GetConfig(IntPtr dpy, ref XVisualInfo vis, GLXAttribute attrib, out int value);

    [DllImport("libGL.so.1", EntryPoint = "glXChooseVisual")]
    public static IntPtr ChooseVisual(IntPtr dpy, int screen, IntPtr attriblist);

    [DllImport("libGL.so.1", EntryPoint = "glXChooseVisual")]
    public static IntPtr ChooseVisual(IntPtr dpy, int screen, ref int attriblist);

    public static unsafe IntPtr ChooseVisual(IntPtr dpy, int screen, int[] attriblist)
    {
      fixed (int* numPtr = attriblist)
        return Glx.ChooseVisual(dpy, screen, (IntPtr) ((void*) numPtr));
    }

    [DllImport("libGL.so.1", EntryPoint = "glXChooseFBConfig")]
    public static IntPtr* ChooseFBConfig(IntPtr dpy, int screen, int[] attriblist, out int fbount);

    [DllImport("libGL.so.1", EntryPoint = "glXGetVisualFromFBConfig")]
    public static IntPtr GetVisualFromFBConfig(IntPtr dpy, IntPtr fbconfig);

    public class Sgi
    {
      public static ErrorCode SwapInterval(int interval)
      {
        return (ErrorCode) Glx.Delegates.glXSwapIntervalSGI(interval);
      }
    }

    public class Arb
    {
      public static unsafe IntPtr CreateContextAttribs(IntPtr display, IntPtr fbconfig, IntPtr share_context, bool direct, int* attribs)
      {
        return Glx.Delegates.glXCreateContextAttribsARB(display, fbconfig, share_context, direct, attribs);
      }

      public static unsafe IntPtr CreateContextAttribs(IntPtr display, IntPtr fbconfig, IntPtr share_context, bool direct, int[] attribs)
      {
        fixed (int* attribs1 = attribs)
          return Glx.Delegates.glXCreateContextAttribsARB(display, fbconfig, share_context, direct, attribs1);
      }
    }

    internal static class Delegates
    {
      public static Glx.Delegates.SwapIntervalSGI glXSwapIntervalSGI;
      public static Glx.Delegates.CreateContextAttribsARB glXCreateContextAttribsARB;

      static Delegates()
      {
      }

      [SuppressUnmanagedCodeSecurity]
      public delegate int SwapIntervalSGI(int interval);

      [SuppressUnmanagedCodeSecurity]
      public delegate IntPtr CreateContextAttribsARB(IntPtr display, IntPtr fbconfig, IntPtr share_context, bool direct, int* attribs);
    }
  }
}
