// Type: OpenTK.Platform.MacOS.Agl
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS
{
  internal static class Agl
  {
    private const string agl = "/System/Library/Frameworks/AGL.framework/Versions/Current/AGL";
    private const int AGL_VERSION_2_0 = 1;

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static IntPtr aglChoosePixelFormat(ref IntPtr gdevs, int ndev, int[] attribs);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static IntPtr aglChoosePixelFormat(IntPtr gdevs, int ndev, int[] attribs);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static void aglDestroyPixelFormat(IntPtr pix);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static IntPtr aglNextPixelFormat(IntPtr pix);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static bool aglDescribePixelFormat(IntPtr pix, Agl.PixelFormatAttribute attrib, out int value);

    [Obsolete("Use aglDisplaysOfPixelFormat instead.")]
    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static IntPtr* aglDevicesOfPixelFormat(IntPtr pix, int* ndevs);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static IntPtr aglQueryRendererInfo(IntPtr[] gdevs, int ndev);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static void aglDestroyRendererInfo(IntPtr rend);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static IntPtr aglNextRendererInfo(IntPtr rend);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglDescribeRenderer(IntPtr rend, int prop, out int value);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static IntPtr aglCreateContext(IntPtr pix, IntPtr share);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglDestroyContext")]
    private static byte _aglDestroyContext(IntPtr ctx);

    internal static bool aglDestroyContext(IntPtr context)
    {
      return (int) Agl._aglDestroyContext(context) != 0;
    }

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglCopyContext(IntPtr src, IntPtr dst, uint mask);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static byte aglUpdateContext(IntPtr ctx);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglSetCurrentContext")]
    private static byte _aglSetCurrentContext(IntPtr ctx);

    internal static bool aglSetCurrentContext(IntPtr context)
    {
      return (int) Agl._aglSetCurrentContext(context) != 0;
    }

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static IntPtr aglGetCurrentContext();

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglSetDrawable")]
    private static byte _aglSetDrawable(IntPtr ctx, IntPtr draw);

    internal static void aglSetDrawable(IntPtr ctx, IntPtr draw)
    {
      if ((int) Agl._aglSetDrawable(ctx, draw) != 0)
        return;
      Agl.AglError error = Agl.GetError();
      throw new MacOSException(error, Agl.ErrorString(error));
    }

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglSetOffScreen(IntPtr ctx, int width, int height, int rowbytes, IntPtr baseaddr);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static IntPtr aglGetDrawable(IntPtr ctx);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglSetFullScreen")]
    private static byte _aglSetFullScreen(IntPtr ctx, int width, int height, int freq, int device);

    internal static void aglSetFullScreen(IntPtr ctx, int width, int height, int freq, int device)
    {
      if ((int) Agl._aglSetFullScreen(ctx, width, height, freq, device) != 0)
        return;
      Agl.AglError error = Agl.GetError();
      throw new MacOSException(error, Agl.ErrorString(error));
    }

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglSetVirtualScreen(IntPtr ctx, int screen);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static int aglGetVirtualScreen(IntPtr ctx);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static void aglGetVersion(int* major, int* minor);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglConfigure(uint pname, uint param);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static void aglSwapBuffers(IntPtr ctx);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static bool aglEnable(IntPtr ctx, Agl.ParameterNames pname);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static bool aglDisable(IntPtr ctx, Agl.ParameterNames pname);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static bool aglIsEnabled(IntPtr ctx, uint pname);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static bool aglSetInteger(IntPtr ctx, Agl.ParameterNames pname, ref int param);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static bool aglSetInteger(IntPtr ctx, Agl.ParameterNames pname, int[] @params);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    internal static bool aglGetInteger(IntPtr ctx, Agl.ParameterNames pname, out int param);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglUseFont(IntPtr ctx, int fontID, int face, int size, int first, int count, int @base);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglGetError")]
    internal static Agl.AglError GetError();

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglErrorString")]
    private static IntPtr _aglErrorString(Agl.AglError code);

    internal static string ErrorString(Agl.AglError code)
    {
      return Marshal.PtrToStringAnsi(Agl._aglErrorString(code));
    }

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static void aglResetLibrary();

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static void aglSurfaceTexture(IntPtr context, uint target, uint internalformat, IntPtr surfacecontext);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglCreatePBuffer(int width, int height, uint target, uint internalFormat, long max_level, IntPtr* pbuffer);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglDestroyPBuffer(IntPtr pbuffer);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglDescribePBuffer(IntPtr pbuffer, int* width, int* height, uint* target, uint* internalFormat, int* max_level);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglTexImagePBuffer(IntPtr ctx, IntPtr pbuffer, int source);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglSetPBuffer(IntPtr ctx, IntPtr pbuffer, int face, int level, int screen);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglGetPBuffer(IntPtr ctx, IntPtr* pbuffer, int* face, int* level, int* screen);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglGetCGLContext(IntPtr ctx, void** cgl_ctx);

    [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
    private static byte aglGetCGLPixelFormat(IntPtr pix, void** cgl_pix);

    internal enum PixelFormatAttribute
    {
      AGL_NONE = 0,
      AGL_ALL_RENDERERS = 1,
      AGL_BUFFER_SIZE = 2,
      AGL_LEVEL = 3,
      AGL_RGBA = 4,
      AGL_DOUBLEBUFFER = 5,
      AGL_STEREO = 6,
      AGL_AUX_BUFFERS = 7,
      AGL_RED_SIZE = 8,
      AGL_GREEN_SIZE = 9,
      AGL_BLUE_SIZE = 10,
      AGL_ALPHA_SIZE = 11,
      AGL_DEPTH_SIZE = 12,
      AGL_STENCIL_SIZE = 13,
      AGL_ACCUM_RED_SIZE = 14,
      AGL_ACCUM_GREEN_SIZE = 15,
      AGL_ACCUM_BLUE_SIZE = 16,
      AGL_ACCUM_ALPHA_SIZE = 17,
      AGL_PIXEL_SIZE = 50,
      AGL_MINIMUM_POLICY = 51,
      AGL_MAXIMUM_POLICY = 52,
      AGL_OFFSCREEN = 53,
      AGL_FULLSCREEN = 54,
      AGL_SAMPLE_BUFFERS_ARB = 55,
      AGL_SAMPLES_ARB = 56,
      AGL_AUX_DEPTH_STENCIL = 57,
      AGL_COLOR_FLOAT = 58,
      AGL_MULTISAMPLE = 59,
      AGL_SUPERSAMPLE = 60,
      AGL_SAMPLE_ALPHA = 61,
    }

    internal enum ExtendedAttribute
    {
      AGL_PIXEL_SIZE = 50,
      AGL_MINIMUM_POLICY = 51,
      AGL_MAXIMUM_POLICY = 52,
      AGL_OFFSCREEN = 53,
      AGL_FULLSCREEN = 54,
      AGL_SAMPLE_BUFFERS_ARB = 55,
      AGL_SAMPLES_ARB = 56,
      AGL_AUX_DEPTH_STENCIL = 57,
      AGL_COLOR_FLOAT = 58,
      AGL_MULTISAMPLE = 59,
      AGL_SUPERSAMPLE = 60,
      AGL_SAMPLE_ALPHA = 61,
    }

    internal enum RendererManagement
    {
      AGL_RENDERER_ID = 70,
      AGL_SINGLE_RENDERER = 71,
      AGL_NO_RECOVERY = 72,
      AGL_ACCELERATED = 73,
      AGL_CLOSEST_POLICY = 74,
      AGL_ROBUST = 75,
      AGL_BACKING_STORE = 76,
      AGL_MP_SAFE = 78,
      AGL_WINDOW = 80,
      AGL_MULTISCREEN = 81,
      AGL_VIRTUAL_SCREEN = 82,
      AGL_COMPLIANT = 83,
      AGL_PBUFFER = 90,
      AGL_REMOTE_PBUFFER = 91,
    }

    internal enum RendererProperties
    {
      AGL_BUFFER_MODES = 100,
      AGL_MIN_LEVEL = 101,
      AGL_MAX_LEVEL = 102,
      AGL_COLOR_MODES = 103,
      AGL_ACCUM_MODES = 104,
      AGL_DEPTH_MODES = 105,
      AGL_STENCIL_MODES = 106,
      AGL_MAX_AUX_BUFFERS = 107,
      AGL_VIDEO_MEMORY = 120,
      AGL_TEXTURE_MEMORY = 121,
      AGL_RENDERER_COUNT = 128,
    }

    internal enum ParameterNames
    {
      AGL_SWAP_RECT = 200,
      AGL_BUFFER_RECT = 202,
      AGL_SWAP_LIMIT = 203,
      AGL_COLORMAP_TRACKING = 210,
      AGL_COLORMAP_ENTRY = 212,
      AGL_RASTERIZATION = 220,
      AGL_SWAP_INTERVAL = 222,
      AGL_STATE_VALIDATION = 230,
      AGL_BUFFER_NAME = 231,
      AGL_ORDER_CONTEXT_TO_FRONT = 232,
      AGL_CONTEXT_SURFACE_ID = 233,
      AGL_CONTEXT_DISPLAY_ID = 234,
      AGL_SURFACE_ORDER = 235,
      AGL_SURFACE_OPACITY = 236,
      AGL_CLIP_REGION = 254,
      AGL_FS_CAPTURE_SINGLE = 255,
      AGL_SURFACE_BACKING_SIZE = 304,
      AGL_ENABLE_SURFACE_BACKING_SIZE = 305,
      AGL_SURFACE_VOLATILE = 306,
    }

    internal enum OptionName
    {
      AGL_FORMAT_CACHE_SIZE = 501,
      AGL_CLEAR_FORMAT_CACHE = 502,
      AGL_RETAIN_RENDERERS = 503,
    }

    internal enum BufferModes
    {
      AGL_MONOSCOPIC_BIT = 1,
      AGL_STEREOSCOPIC_BIT = 2,
      AGL_SINGLEBUFFER_BIT = 4,
      AGL_DOUBLEBUFFER_BIT = 8,
    }

    internal enum BitDepths
    {
      AGL_0_BIT = 1,
      AGL_1_BIT = 2,
      AGL_2_BIT = 4,
      AGL_3_BIT = 8,
      AGL_4_BIT = 16,
      AGL_5_BIT = 32,
      AGL_6_BIT = 64,
      AGL_8_BIT = 128,
      AGL_10_BIT = 256,
      AGL_12_BIT = 512,
      AGL_16_BIT = 1024,
      AGL_24_BIT = 2048,
      AGL_32_BIT = 4096,
      AGL_48_BIT = 8192,
      AGL_64_BIT = 16384,
      AGL_96_BIT = 32768,
      AGL_128_BIT = 65536,
    }

    internal enum ColorModes
    {
      AGL_RGB8_BIT = 1,
      AGL_RGB8_A8_BIT = 2,
      AGL_BGR233_BIT = 4,
      AGL_BGR233_A8_BIT = 8,
      AGL_RGB332_BIT = 16,
      AGL_RGB332_A8_BIT = 32,
      AGL_RGB444_BIT = 64,
      AGL_ARGB4444_BIT = 128,
      AGL_RGB444_A8_BIT = 256,
      AGL_RGB555_BIT = 512,
      AGL_ARGB1555_BIT = 1024,
      AGL_RGB555_A8_BIT = 2048,
      AGL_RGB565_BIT = 4096,
      AGL_RGB565_A8_BIT = 8192,
      AGL_RGB888_BIT = 16384,
      AGL_ARGB8888_BIT = 32768,
      AGL_RGB888_A8_BIT = 65536,
      AGL_RGB101010_BIT = 131072,
      AGL_ARGB2101010_BIT = 262144,
      AGL_RGB101010_A8_BIT = 524288,
      AGL_RGB121212_BIT = 1048576,
      AGL_ARGB12121212_BIT = 2097152,
      AGL_RGB161616_BIT = 4194304,
      AGL_ARGB16161616_BIT = 8388608,
      AGL_RGBFLOAT64_BIT = 16777216,
      AGL_RGBAFLOAT64_BIT = 33554432,
      AGL_RGBFLOAT128_BIT = 67108864,
      AGL_RGBAFLOAT128_BIT = 134217728,
      AGL_RGBFLOAT256_BIT = 268435456,
      AGL_INDEX8_BIT = 536870912,
      AGL_RGBAFLOAT256_BIT = 536870912,
      AGL_INDEX16_BIT = 1073741824,
    }

    internal enum AglError
    {
      NoError = 0,
      BadAttribute = 10000,
      BadProperty = 10001,
      BadPixelFormat = 10002,
      BadRendererInfo = 10003,
      BadContext = 10004,
      BadDrawable = 10005,
      BadGraphicsDevice = 10006,
      BadState = 10007,
      BadValue = 10008,
      BadMatch = 10009,
      BadEnum = 10010,
      BadOffscreen = 10011,
      BadFullscreen = 10012,
      BadWindow = 10013,
      BadPointer = 10014,
      BadModule = 10015,
      BadAlloc = 10016,
      BadConnection = 10017,
    }
  }
}
