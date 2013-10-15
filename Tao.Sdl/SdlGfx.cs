// Type: Tao.Sdl.SdlGfx
// Assembly: Tao.Sdl, Version=1.2.13.0, Culture=neutral, PublicKeyToken=9c7a200e36c0094e
// MVID: 45EBE10E-1CB9-425D-83FF-C3B8997BBF28
// Assembly location: F:\Program Files (x86)\FEZ\Tao.Sdl.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Tao.Sdl
{
  [SuppressUnmanagedCodeSecurity]
  public static class SdlGfx
  {
    private const string SDL_GFX_NATIVE_LIBRARY = "SDL_gfx.dll";
    private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
    public const int SDL_GFXPRIMITIVES_MAJOR = 2;
    public const int SDL_GFXPRIMITIVES_MINOR = 0;
    public const int SDL_GFXPRIMITIVES_MICRO = 16;
    public const int SMOOTHING_OFF = 0;
    public const int SMOOTHING_ON = 1;
    public const int FPS_UPPER_LIMIT = 200;
    public const int FPS_LOWER_LIMIT = 1;
    public const int FPS_DEFAULT = 30;

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int pixelColor(IntPtr dst, short x, short y, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int pixelRGBA(IntPtr dst, short x, short y, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int hlineColor(IntPtr dst, short x1, short x2, short y, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int hlineRGBA(IntPtr dst, short x1, short x2, short y, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int vlineColor(IntPtr dst, short x, short y1, short y2, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int vlineRGBA(IntPtr dst, short x, short y1, short y2, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int rectangleColor(IntPtr dst, short x1, short y1, short x2, short y2, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int rectangleRGBA(IntPtr dst, short x1, short y1, short x2, short y2, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int boxColor(IntPtr dst, short x1, short y1, short x2, short y2, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int boxRGBA(IntPtr dst, short x1, short y1, short x2, short y2, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int lineColor(IntPtr dst, short x1, short y1, short x2, short y2, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int lineRGBA(IntPtr dst, short x1, short y1, short x2, short y2, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aalineColor(IntPtr dst, short x1, short y1, short x2, short y2, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aalineRGBA(IntPtr dst, short x1, short y1, short x2, short y2, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int circleColor(IntPtr dst, short x, short y, short r, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int circleRGBA(IntPtr dst, short x, short y, short rad, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledCircleColor(IntPtr dst, short x, short y, short r, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledCircleRGBA(IntPtr dst, short x, short y, short rad, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aacircleColor(IntPtr dst, short x, short y, short r, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aacircleRGBA(IntPtr dst, short x, short y, short rad, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int ellipseColor(IntPtr dst, short x, short y, short rx, short ry, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int ellipseRGBA(IntPtr dst, short x, short y, short rx, short ry, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aaellipseColor(IntPtr dst, short xc, short yc, short rx, short ry, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aaellipseRGBA(IntPtr dst, short x, short y, short rx, short ry, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledEllipseColor(IntPtr dst, short x, short y, short rx, short ry, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledEllipseRGBA(IntPtr dst, short x, short y, short rx, short ry, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int pieColor(IntPtr dst, short x, short y, short rad, short start, short end, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int pieRGBA(IntPtr dst, short x, short y, short rad, short start, short end, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledPieColor(IntPtr dst, short x, short y, short rad, short start, short end, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledPieRGBA(IntPtr dst, short x, short y, short rad, short start, short end, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int trigonColor(IntPtr dst, short x1, short y1, short x2, short y2, short x3, short y3, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int trigonRGBA(IntPtr dst, short x1, short y1, short x2, short y2, short x3, short y3, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aatrigonColor(IntPtr dst, short x1, short y1, short x2, short y2, short x3, short y3, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aatrigonRGBA(IntPtr dst, short x1, short y1, short x2, short y2, short x3, short y3, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledTrigonColor(IntPtr dst, short x1, short y1, short x2, short y2, short x3, short y3, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledTrigonRGBA(IntPtr dst, short x1, short y1, short x2, short y2, short x3, short y3, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int polygonColor(IntPtr dst, short[] vx, short[] vy, int n, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int polygonRGBA(IntPtr dst, short[] vx, short[] vy, int n, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aapolygonColor(IntPtr dst, short[] vx, short[] vy, int n, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int aapolygonRGBA(IntPtr dst, short[] vx, short[] vy, int n, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledPolygonColor(IntPtr dst, short[] vx, short[] vy, int n, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int filledPolygonRGBA(IntPtr dst, short[] vx, short[] vy, int n, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int texturedPolygon(IntPtr dst, short[] vx, short[] vy, int n, IntPtr texture, int texture_dx, int texture_dy);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int bezierColor(IntPtr dst, short[] vx, short[] vy, int n, int s, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int bezierRGBA(IntPtr dst, short[] vx, short[] vy, int n, int s, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int characterColor(IntPtr dst, short x, short y, char c, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int characterRGBA(IntPtr dst, short x, short y, char c, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int stringColor(IntPtr dst, short x, short y, string c, int color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int stringRGBA(IntPtr dst, short x, short y, string c, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void gfxPrimitivesSetFont(object fontdata, int cw, int ch);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr rotozoomSurface(IntPtr src, double angle, double zoom, int smooth);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr rotozoomSurfaceXY(IntPtr src, double angle, double zoomx, double zoomy, int smooth);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void rotozoomSurfaceSize(int width, int height, double angle, double zoom, out int dstwidth, out int dstheight);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void rotozoomSurfaceSizeXY(int width, int height, double angle, double zoomx, double zoomy, out int dstwidth, out int dstheight);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr zoomSurface(IntPtr src, double zoomx, double zoomy, int smooth);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void zoomSurfaceSize(int width, int height, double zoomx, double zoomy, out int dstwidth, out int dstheight);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr shrinkSurface(IntPtr src, int factorx, int factory);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_initFramerate(IntPtr manager);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_setFramerate(IntPtr manager, int rate);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_getFramerate(IntPtr manager);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_framerateDelay(IntPtr manager);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterMMXdetect();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_imageFilterMMXoff();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_imageFilterMMXon();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterAdd(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterMean(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterSub(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterAbsDiff(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterMult(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterMultNor(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterMultDivby2(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterMultDivby4(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterBitAnd(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterBitOr(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterDiv(byte[] Src1, byte[] Src2, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterBitNegation(byte[] Src1, byte[] Dest, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterAddByte(byte[] Src1, byte[] Dest, int length, byte C);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterAddUint(byte[] Src1, byte[] Dest, int length, int C);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterAddByteToHalf(byte[] Src1, byte[] Dest, int length, byte C);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterSubByte(byte[] Src1, byte[] Dest, int length, byte C);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterSubUint(byte[] Src1, byte[] Dest, int length, int C);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterShiftRight(byte[] Src1, byte[] Dest, int length, byte N);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterShiftRightUint(byte[] Src1, byte[] Dest, int length, byte N);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterMultByByte(byte[] Src1, byte[] Dest, int length, byte C);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterShiftRightAndMultByByte(byte[] Src1, byte[] Dest, int length, byte N, byte C);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterShiftLeftByte(byte[] Src1, byte[] Dest, int length, byte N);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterShiftLeft(byte[] Src1, byte[] Dest, int length, byte N);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterShiftLeftUint(byte[] Src1, byte[] Dest, int length, byte N);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterBinarizeUsingThreshold(byte[] Src1, byte[] Dest, int length, byte T);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterClipToRange(byte[] Src1, byte[] Dest, int length, byte Tmin, byte Tmax);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterNormalizeLinear(byte[] Src1, byte[] Dest, int length, int Cmin, int Cmax, int Nmin, int Nmax);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterConvolveKernel3x3Divide(byte[] Src1, byte[] Dest, int rows, int columns, short[] Kernel, byte Divisor);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterConvolveKernel5x5Divide(byte[] Src1, byte[] Dest, int rows, int columns, short[] Kernel, byte Divisor);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterConvolveKernel7x7Divide(byte[] Src1, byte[] Dest, int rows, int columns, short[] Kernel, byte Divisor);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterConvolveKernel9x9Divide(byte[] Src1, byte[] Dest, int rows, int columns, short[] Kernel, byte Divisor);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterConvolveKernel3x3ShiftRight(byte[] Src1, byte[] Dest, int rows, int columns, short[] Kernel, byte NRightShift);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterConvolveKernel5x5ShiftRight(byte[] Src1, byte[] Dest, int rows, int columns, short[] Kernel, byte NRightShift);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterConvolveKernel7x7ShiftRight(byte[] Src1, byte[] Dest, int rows, int columns, short[] Kernel, byte NRightShift);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterConvolveKernel9x9ShiftRight(byte[] Src1, byte[] Dest, int rows, int columns, short[] Kernel, byte NRightShift);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterSobelX(byte[] Src1, byte[] Dest, int rows, int columns);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_imageFilterSobelXShiftRight(byte[] Src1, byte[] Dest, int rows, int columns, byte NRightShift);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_imageFilterAlignStack();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_imageFilterRestoreStack();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_gfxBlitRGBA(IntPtr src, ref Sdl.SDL_Rect srcrect, IntPtr dst, Sdl.SDL_Rect dstrect);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_gfx.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_gfxSetAlpha(IntPtr src, byte a);

    public struct tColorRGBA
    {
      public byte r;
      public byte g;
      public byte b;
      public byte a;
    }

    public struct tColorY
    {
      public byte y;
    }

    public struct FPSmanager
    {
      public int framecount;
      public float rateticks;
      public int lastticks;
      public int rate;
    }
  }
}
