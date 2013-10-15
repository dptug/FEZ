// Type: Tao.Sdl.SdlTtf
// Assembly: Tao.Sdl, Version=1.2.13.0, Culture=neutral, PublicKeyToken=9c7a200e36c0094e
// MVID: 45EBE10E-1CB9-425D-83FF-C3B8997BBF28
// Assembly location: F:\Program Files (x86)\FEZ\Tao.Sdl.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Tao.Sdl
{
  [SuppressUnmanagedCodeSecurity]
  public static class SdlTtf
  {
    private const string SDL_TTF_NATIVE_LIBRARY = "SDL_ttf.dll";
    private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
    public const int TTF_MAJOR_VERSION = 2;
    public const int TTF_MINOR_VERSION = 0;
    public const int TTF_PATCHLEVEL = 8;
    public const byte TTF_STYLE_NORMAL = (byte) 0;
    public const byte TTF_STYLE_BOLD = (byte) 1;
    public const byte TTF_STYLE_ITALIC = (byte) 2;
    public const byte TTF_STYLE_UNDERLINE = (byte) 4;
    public const int UNICODE_BOM_NATIVE = 65279;
    public const int UNICODE_BOM_SWAPPED = 65534;

    public static Sdl.SDL_version TTF_VERSION()
    {
      return new Sdl.SDL_version()
      {
        major = (byte) 2,
        minor = (byte) 0,
        patch = (byte) 8
      };
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", EntryPoint = "TTF_Linked_Version", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr TTF_Linked_VersionInternal();

    public static Sdl.SDL_version TTF_Linked_Version()
    {
      return (Sdl.SDL_version) Marshal.PtrToStructure(SdlTtf.TTF_Linked_VersionInternal(), typeof (Sdl.SDL_version));
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void TTF_ByteSwappedUNICODE(int swapped);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_Init();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_OpenFont(string file, int ptsize);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_OpenFontIndex(string file, int ptsize, long index);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_OpenFontRW(IntPtr src, int freesrc, int ptsize);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_OpenFontIndexRW(IntPtr src, int freesrc, int ptsize, long index);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void TTF_SetFontStyle(IntPtr font, int style);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_GetFontStyle(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_FontHeight(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_FontAscent(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_FontDescent(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_FontLineSkip(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static long TTF_FontFaces(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_FontFaceIsFixedWidth(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static string TTF_FontFaceFamilyName(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static string TTF_FontFaceStyleName(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_GlyphMetrics(IntPtr font, short ch, out int minx, out int maxx, out int miny, out int maxy, out int advance);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_SizeText(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, out int w, out int h);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_SizeUTF8(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, out int w, out int h);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_SizeUNICODE(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, out int w, out int h);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderText_Solid(IntPtr font, string text, Sdl.SDL_Color fg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderUTF8_Solid(IntPtr font, string text, Sdl.SDL_Color fg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderUNICODE_Solid(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, Sdl.SDL_Color fg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderGlyph_Solid(IntPtr font, short ch, Sdl.SDL_Color fg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderText_Shaded(IntPtr font, string text, Sdl.SDL_Color fg, Sdl.SDL_Color bg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderUTF8_Shaded(IntPtr font, string text, Sdl.SDL_Color fg, Sdl.SDL_Color bg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderUNICODE_Shaded(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, Sdl.SDL_Color fg, Sdl.SDL_Color bg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderGlyph_Shaded(IntPtr font, short ch, Sdl.SDL_Color fg, Sdl.SDL_Color bg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderText_Blended(IntPtr font, string text, Sdl.SDL_Color fg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderUTF8_Blended(IntPtr font, string text, Sdl.SDL_Color fg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderUNICODE_Blended(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, Sdl.SDL_Color fg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr TTF_RenderGlyph_Blended(IntPtr font, short ch, Sdl.SDL_Color fg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void TTF_CloseFont(IntPtr font);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void TTF_Quit();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int TTF_WasInit();

    public static void TTF_SetError(string message)
    {
      Sdl.SDL_SetError(message);
    }

    public static string TTF_GetError()
    {
      return Sdl.SDL_GetError();
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct TTF_Font
    {
    }
  }
}
