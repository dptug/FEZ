// Type: OpenTK.Platform.Egl.Egl
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.Egl
{
  internal static class Egl
  {
    public const int VERSION_1_0 = 1;
    public const int VERSION_1_1 = 1;
    public const int VERSION_1_2 = 1;
    public const int VERSION_1_3 = 1;
    public const int VERSION_1_4 = 1;
    public const int FALSE = 0;
    public const int TRUE = 1;
    public const int DONT_CARE = -1;
    public const int SUCCESS = 12288;
    public const int NOT_INITIALIZED = 12289;
    public const int BAD_ACCESS = 12290;
    public const int BAD_ALLOC = 12291;
    public const int BAD_ATTRIBUTE = 12292;
    public const int BAD_CONFIG = 12293;
    public const int BAD_CONTEXT = 12294;
    public const int BAD_CURRENT_SURFACE = 12295;
    public const int BAD_DISPLAY = 12296;
    public const int BAD_MATCH = 12297;
    public const int BAD_NATIVE_PIXMAP = 12298;
    public const int BAD_NATIVE_WINDOW = 12299;
    public const int BAD_PARAMETER = 12300;
    public const int BAD_SURFACE = 12301;
    public const int CONTEXT_LOST = 12302;
    public const int BUFFER_SIZE = 12320;
    public const int ALPHA_SIZE = 12321;
    public const int BLUE_SIZE = 12322;
    public const int GREEN_SIZE = 12323;
    public const int RED_SIZE = 12324;
    public const int DEPTH_SIZE = 12325;
    public const int STENCIL_SIZE = 12326;
    public const int CONFIG_CAVEAT = 12327;
    public const int CONFIG_ID = 12328;
    public const int LEVEL = 12329;
    public const int MAX_PBUFFER_HEIGHT = 12330;
    public const int MAX_PBUFFER_PIXELS = 12331;
    public const int MAX_PBUFFER_WIDTH = 12332;
    public const int NATIVE_RENDERABLE = 12333;
    public const int NATIVE_VISUAL_ID = 12334;
    public const int NATIVE_VISUAL_TYPE = 12335;
    public const int PRESERVED_RESOURCES = 12336;
    public const int SAMPLES = 12337;
    public const int SAMPLE_BUFFERS = 12338;
    public const int SURFACE_TYPE = 12339;
    public const int TRANSPARENT_TYPE = 12340;
    public const int TRANSPARENT_BLUE_VALUE = 12341;
    public const int TRANSPARENT_GREEN_VALUE = 12342;
    public const int TRANSPARENT_RED_VALUE = 12343;
    public const int NONE = 12344;
    public const int BIND_TO_TEXTURE_RGB = 12345;
    public const int BIND_TO_TEXTURE_RGBA = 12346;
    public const int MIN_SWAP_INTERVAL = 12347;
    public const int MAX_SWAP_INTERVAL = 12348;
    public const int LUMINANCE_SIZE = 12349;
    public const int ALPHA_MASK_SIZE = 12350;
    public const int COLOR_BUFFER_TYPE = 12351;
    public const int RENDERABLE_TYPE = 12352;
    public const int MATCH_NATIVE_PIXMAP = 12353;
    public const int CONFORMANT = 12354;
    public const int SLOW_CONFIG = 12368;
    public const int NON_CONFORMANT_CONFIG = 12369;
    public const int TRANSPARENT_RGB = 12370;
    public const int RGB_BUFFER = 12430;
    public const int LUMINANCE_BUFFER = 12431;
    public const int NO_TEXTURE = 12380;
    public const int TEXTURE_RGB = 12381;
    public const int TEXTURE_RGBA = 12382;
    public const int TEXTURE_2D = 12383;
    public const int PBUFFER_BIT = 1;
    public const int PIXMAP_BIT = 2;
    public const int WINDOW_BIT = 4;
    public const int VG_COLORSPACE_LINEAR_BIT = 32;
    public const int VG_ALPHA_FORMAT_PRE_BIT = 64;
    public const int MULTISAMPLE_RESOLVE_BOX_BIT = 512;
    public const int SWAP_BEHAVIOR_PRESERVED_BIT = 1024;
    public const int OPENGL_ES_BIT = 1;
    public const int OPENVG_BIT = 2;
    public const int OPENGL_ES2_BIT = 4;
    public const int OPENGL_BIT = 8;
    public const int VENDOR = 12371;
    public const int VERSION = 12372;
    public const int EXTENSIONS = 12373;
    public const int CLIENT_APIS = 12429;
    public const int HEIGHT = 12374;
    public const int WIDTH = 12375;
    public const int LARGEST_PBUFFER = 12376;
    public const int TEXTURE_FORMAT = 12416;
    public const int TEXTURE_TARGET = 12417;
    public const int MIPMAP_TEXTURE = 12418;
    public const int MIPMAP_LEVEL = 12419;
    public const int RENDER_BUFFER = 12422;
    public const int VG_COLORSPACE = 12423;
    public const int VG_ALPHA_FORMAT = 12424;
    public const int HORIZONTAL_RESOLUTION = 12432;
    public const int VERTICAL_RESOLUTION = 12433;
    public const int PIXEL_ASPECT_RATIO = 12434;
    public const int SWAP_BEHAVIOR = 12435;
    public const int MULTISAMPLE_RESOLVE = 12441;
    public const int BACK_BUFFER = 12420;
    public const int SINGLE_BUFFER = 12421;
    public const int VG_COLORSPACE_sRGB = 12425;
    public const int VG_COLORSPACE_LINEAR = 12426;
    public const int VG_ALPHA_FORMAT_NONPRE = 12427;
    public const int VG_ALPHA_FORMAT_PRE = 12428;
    public const int DISPLAY_SCALING = 10000;
    public const int UNKNOWN = -1;
    public const int BUFFER_PRESERVED = 12436;
    public const int BUFFER_DESTROYED = 12437;
    public const int OPENVG_IMAGE = 12438;
    public const int CONTEXT_CLIENT_TYPE = 12439;
    public const int CONTEXT_CLIENT_VERSION = 12440;
    public const int MULTISAMPLE_RESOLVE_DEFAULT = 12442;
    public const int MULTISAMPLE_RESOLVE_BOX = 12443;
    public const int OPENGL_ES_API = 12448;
    public const int OPENVG_API = 12449;
    public const int OPENGL_API = 12450;
    public const int DRAW = 12377;
    public const int READ = 12378;
    public const int CORE_NATIVE_ENGINE = 12379;
    public const int COLORSPACE = 12423;
    public const int ALPHA_FORMAT = 12424;
    public const int COLORSPACE_sRGB = 12425;
    public const int COLORSPACE_LINEAR = 12426;
    public const int ALPHA_FORMAT_NONPRE = 12427;
    public const int ALPHA_FORMAT_PRE = 12428;

    public static bool IsSupported
    {
      get
      {
        try
        {
          Egl.GetCurrentContext();
        }
        catch (Exception ex)
        {
          return false;
        }
        return true;
      }
    }

    [DllImport("libEGL.dll", EntryPoint = "eglGetError")]
    public static int GetError();

    [DllImport("libEGL.dll", EntryPoint = "eglGetDisplay")]
    public static IntPtr GetDisplay(IntPtr display_id);

    [DllImport("libEGL.dll", EntryPoint = "eglInitialize")]
    public static bool Initialize(IntPtr dpy, out int major, out int minor);

    [DllImport("libEGL.dll", EntryPoint = "eglTerminate")]
    public static bool Terminate(IntPtr dpy);

    [DllImport("libEGL.dll", EntryPoint = "eglQueryString")]
    public static IntPtr QueryString(IntPtr dpy, int name);

    [DllImport("libEGL.dll", EntryPoint = "eglGetConfigs")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool GetConfigs(IntPtr dpy, IntPtr[] configs, int config_size, out int num_config);

    [DllImport("libEGL.dll", EntryPoint = "eglChooseConfig")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool ChooseConfig(IntPtr dpy, int[] attrib_list, [In, Out] IntPtr[] configs, int config_size, out int num_config);

    [DllImport("libEGL.dll", EntryPoint = "eglGetConfigAttrib")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool GetConfigAttrib(IntPtr dpy, IntPtr config, int attribute, out int value);

    [DllImport("libEGL.dll", EntryPoint = "eglCreateWindowSurface")]
    public static IntPtr CreateWindowSurface(IntPtr dpy, IntPtr config, IntPtr win, int[] attrib_list);

    [DllImport("libEGL.dll", EntryPoint = "eglCreatePbufferSurface")]
    public static IntPtr CreatePbufferSurface(IntPtr dpy, IntPtr config, int[] attrib_list);

    [DllImport("libEGL.dll", EntryPoint = "eglCreatePixmapSurface")]
    public static IntPtr CreatePixmapSurface(IntPtr dpy, IntPtr config, IntPtr pixmap, int[] attrib_list);

    [DllImport("libEGL.dll", EntryPoint = "eglDestroySurface")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool DestroySurface(IntPtr dpy, IntPtr surface);

    [DllImport("libEGL.dll", EntryPoint = "eglQuerySurface")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool QuerySurface(IntPtr dpy, IntPtr surface, int attribute, out int value);

    [DllImport("libEGL.dll", EntryPoint = "eglBindAPI")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool BindAPI(int api);

    [DllImport("libEGL.dll", EntryPoint = "eglQueryAPI")]
    public static int QueryAPI();

    [DllImport("libEGL.dll", EntryPoint = "eglWaitClient")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool WaitClient();

    [DllImport("libEGL.dll", EntryPoint = "eglReleaseThread")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool ReleaseThread();

    [DllImport("libEGL.dll", EntryPoint = "eglCreatePbufferFromClientBuffer")]
    public static IntPtr CreatePbufferFromClientBuffer(IntPtr dpy, int buftype, IntPtr buffer, IntPtr config, int[] attrib_list);

    [DllImport("libEGL.dll", EntryPoint = "eglSurfaceAttrib")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool SurfaceAttrib(IntPtr dpy, IntPtr surface, int attribute, int value);

    [DllImport("libEGL.dll", EntryPoint = "eglBindTexImage")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool BindTexImage(IntPtr dpy, IntPtr surface, int buffer);

    [DllImport("libEGL.dll", EntryPoint = "eglReleaseTexImage")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool ReleaseTexImage(IntPtr dpy, IntPtr surface, int buffer);

    [DllImport("libEGL.dll", EntryPoint = "eglSwapInterval")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool SwapInterval(IntPtr dpy, int interval);

    [DllImport("libEGL.dll")]
    private static IntPtr eglCreateContext(IntPtr dpy, IntPtr config, IntPtr share_context, int[] attrib_list);

    public static IntPtr CreateContext(IntPtr dpy, IntPtr config, IntPtr share_context, int[] attrib_list)
    {
      IntPtr context = Egl.eglCreateContext(dpy, config, share_context, attrib_list);
      if (context == IntPtr.Zero)
        throw new GraphicsContextException(string.Format("Failed to create EGL context, error: {0}.", (object) Egl.GetError()));
      else
        return context;
    }

    [DllImport("libEGL.dll", EntryPoint = "eglDestroyContext")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool DestroyContext(IntPtr dpy, IntPtr ctx);

    [DllImport("libEGL.dll", EntryPoint = "eglMakeCurrent")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool MakeCurrent(IntPtr dpy, IntPtr draw, IntPtr read, IntPtr ctx);

    [DllImport("libEGL.dll", EntryPoint = "eglGetCurrentContext")]
    public static IntPtr GetCurrentContext();

    [DllImport("libEGL.dll", EntryPoint = "eglGetCurrentSurface")]
    public static IntPtr GetCurrentSurface(int readdraw);

    [DllImport("libEGL.dll", EntryPoint = "eglGetCurrentDisplay")]
    public static IntPtr GetCurrentDisplay();

    [DllImport("libEGL.dll", EntryPoint = "eglQueryContext")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool QueryContext(IntPtr dpy, IntPtr ctx, int attribute, out int value);

    [DllImport("libEGL.dll", EntryPoint = "eglWaitGL")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool WaitGL();

    [DllImport("libEGL.dll", EntryPoint = "eglWaitNative")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool WaitNative(int engine);

    [DllImport("libEGL.dll", EntryPoint = "eglSwapBuffers")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool SwapBuffers(IntPtr dpy, IntPtr surface);

    [DllImport("libEGL.dll", EntryPoint = "eglCopyBuffers")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static bool CopyBuffers(IntPtr dpy, IntPtr surface, IntPtr target);

    [DllImport("libEGL.dll", EntryPoint = "eglGetProcAddress")]
    public static IntPtr GetProcAddress(string funcname);
  }
}
