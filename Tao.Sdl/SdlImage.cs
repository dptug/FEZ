// Type: Tao.Sdl.SdlImage
// Assembly: Tao.Sdl, Version=1.2.13.0, Culture=neutral, PublicKeyToken=9c7a200e36c0094e
// MVID: 45EBE10E-1CB9-425D-83FF-C3B8997BBF28
// Assembly location: F:\Program Files (x86)\FEZ\Tao.Sdl.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Tao.Sdl
{
  [SuppressUnmanagedCodeSecurity]
  public static class SdlImage
  {
    private const string SDL_IMAGE_NATIVE_LIBRARY = "SDL_image.dll";
    private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
    public const int SDL_IMAGE_MAJOR_VERSION = 1;
    public const int SDL_IMAGE_MINOR_VERSION = 2;
    public const int SDL_IMAGE_PATCHLEVEL = 5;

    public static Sdl.SDL_version SDL_IMAGE_VERSION()
    {
      return new Sdl.SDL_version()
      {
        major = (byte) 1,
        minor = (byte) 2,
        patch = (byte) 5
      };
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", EntryPoint = "IMG_Linked_Version", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr IMG_Linked_VersionInternal();

    public static Sdl.SDL_version IMG_Linked_Version()
    {
      return (Sdl.SDL_version) Marshal.PtrToStructure(SdlImage.IMG_Linked_VersionInternal(), typeof (Sdl.SDL_version));
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadTyped_RW(IntPtr src, int freesrc, string type);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_Load(string file);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_Load_RW(IntPtr src, int freesrc);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isBMP(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isPNM(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isXPM(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isXV(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isXCF(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isPCX(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isGIF(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isJPG(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isTIF(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isPNG(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_isLBM(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadBMP_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadPNM_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadXPM_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadXCF_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadXV_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadPCX_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadGIF_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadJPG_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadTIF_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadPNG_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadTGA_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_LoadLBM_RW(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_image.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr IMG_ReadXPMFromArray(string[] src);

    public static void IMG_SetError(string message)
    {
      Sdl.SDL_SetError(message);
    }

    public static string IMG_GetError()
    {
      return Sdl.SDL_GetError();
    }
  }
}
