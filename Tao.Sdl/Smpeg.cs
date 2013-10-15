// Type: Tao.Sdl.Smpeg
// Assembly: Tao.Sdl, Version=1.2.13.0, Culture=neutral, PublicKeyToken=9c7a200e36c0094e
// MVID: 45EBE10E-1CB9-425D-83FF-C3B8997BBF28
// Assembly location: F:\Program Files (x86)\FEZ\Tao.Sdl.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Tao.Sdl
{
  [SuppressUnmanagedCodeSecurity]
  public static class Smpeg
  {
    private const string SMPEG_NATIVE_LIBRARY = "smpeg.dll";
    private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
    public const int SMPEG_FILTER_INFO_MB_ERROR = 1;
    public const int SMPEG_FILTER_INFO_PIXEL_ERROR = 2;
    public const int SMPEG_MAJOR_VERSION = 0;
    public const int SMPEG_MINOR_VERSION = 4;
    public const int SMPEG_PATCHLEVEL = 5;
    [CLSCompliant(false)]
    public const int SMPEG_ERROR = -1;
    public const int SMPEG_STOPPED = 0;
    public const int SMPEG_PLAYING = 1;

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SMPEGfilter_null();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SMPEGfilter_bilinear();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SMPEGfilter_deblocking();

    [CLSCompliant(false)]
    public static Smpeg.SMPEG_version SMPEG_VERSION()
    {
      return new Smpeg.SMPEG_version()
      {
        major = (byte) 0,
        minor = (byte) 4,
        patch = (byte) 5
      };
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SMPEG_new(string file, out Smpeg.SMPEG_Info info, int sdl_audio);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SMPEG_new_descr(int file, out Smpeg.SMPEG_Info info, int sdl_audio);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SMPEG_new_data(object data, int size, out Smpeg.SMPEG_Info info, int sdl_audio);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SMPEG_new_rwops(IntPtr src, out Smpeg.SMPEG_Info info, int sdl_audio);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_getinfo(IntPtr mpeg, out Smpeg.SMPEG_Info info);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_enableaudio(IntPtr mpeg, int enable);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_enablevideo(IntPtr mpeg, int enable);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_delete(IntPtr mpeg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SMPEG_status(IntPtr mpeg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_setvolume(IntPtr mpeg, int volume);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_setdisplay(IntPtr mpeg, IntPtr dst, IntPtr surfLock, Smpeg.SMPEG_DisplayCallback callback);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_loop(IntPtr mpeg, int repeat);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_scaleXY(IntPtr mpeg, int width, int height);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_scale(IntPtr mpeg, int scale);

    public static void SMPEG_double(IntPtr mpeg, int on)
    {
      if (on != 0)
        Smpeg.SMPEG_scale(mpeg, 2);
      else
        Smpeg.SMPEG_scale(mpeg, 1);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_move(IntPtr mpeg, int x, int y);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_setdisplayregion(IntPtr mpeg, int x, int y, int w, int h);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_play(IntPtr mpeg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_pause(IntPtr mpeg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_stop(IntPtr mpeg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_rewind(IntPtr mpeg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_seek(IntPtr mpeg, int bytes);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_skip(IntPtr mpeg, float seconds);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_renderFrame(IntPtr mpeg, int framenum);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_renderFinal(IntPtr mpeg, IntPtr dst, int x, int y);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SMPEG_filter(IntPtr mpeg, IntPtr filter);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static string SMPEG_error(IntPtr mpeg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SMPEG_playAudio(IntPtr mpeg, byte[] stream, int len);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_playAudioSDL(IntPtr mpeg, byte[] stream, int len);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SMPEG_wantedSpec(IntPtr mpeg, IntPtr wanted);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("smpeg.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SMPEG_actualSpec(IntPtr mpeg, ref Sdl.SDL_AudioSpec spec);

    public struct SMPEG_FilterInfo
    {
      public IntPtr yuv_mb_square_error;
      public IntPtr yuv_pixel_square_error;
    }

    [CLSCompliant(false)]
    public struct SMPEG_Filter
    {
      public int flags;
      public object data;
      public Smpeg.SMPEG_FilterCallback callback;
      public Smpeg.SMPEG_FilterDestroy destroy;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SMPEG_version
    {
      public byte major;
      public byte minor;
      public byte patch;

      public override string ToString()
      {
        object[] objArray = new object[5];
        int index1 = 0;
        // ISSUE: variable of a boxed type
        __Boxed<byte> local1 = (ValueType) this.major;
        objArray[index1] = (object) local1;
        int index2 = 1;
        string str1 = ".";
        objArray[index2] = (object) str1;
        int index3 = 2;
        // ISSUE: variable of a boxed type
        __Boxed<byte> local2 = (ValueType) this.minor;
        objArray[index3] = (object) local2;
        int index4 = 3;
        string str2 = ".";
        objArray[index4] = (object) str2;
        int index5 = 4;
        // ISSUE: variable of a boxed type
        __Boxed<byte> local3 = (ValueType) this.patch;
        objArray[index5] = (object) local3;
        return string.Concat(objArray);
      }
    }

    public struct SMPEG_Info
    {
      public int has_audio;
      public int has_video;
      public int width;
      public int height;
      public int current_frame;
      public double current_fps;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
      public string audio_string;
      public int audio_current_frame;
      public int current_offset;
      public int total_size;
      public double current_time;
      public double total_time;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SMPEG_FilterCallback(IntPtr dest, IntPtr source, out Sdl.SDL_Rect region, IntPtr filter_info, object data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SMPEG_FilterDestroy(IntPtr filter);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SMPEG_DisplayCallback(IntPtr dst, int x, int y, int w, int h);
  }
}
