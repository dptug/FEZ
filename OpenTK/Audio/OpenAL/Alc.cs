// Type: OpenTK.Audio.OpenAL.Alc
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenTK.Audio.OpenAL
{
  public static class Alc
  {
    private const string Lib = "soft_oal.dll";
    private const CallingConvention Style = CallingConvention.Cdecl;

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcCreateContext", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr sys_CreateContext([In] IntPtr device, [In] int* attrlist);

    [CLSCompliant(false)]
    public static unsafe ContextHandle CreateContext([In] IntPtr device, [In] int* attrlist)
    {
      return new ContextHandle(Alc.sys_CreateContext(device, attrlist));
    }

    public static unsafe ContextHandle CreateContext(IntPtr device, int[] attriblist)
    {
      fixed (int* attrlist = attriblist)
        return Alc.CreateContext(device, attrlist);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcMakeContextCurrent", CallingConvention = CallingConvention.Cdecl)]
    private static bool MakeContextCurrent(IntPtr context);

    public static bool MakeContextCurrent(ContextHandle context)
    {
      return Alc.MakeContextCurrent(context.Handle);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcProcessContext", CallingConvention = CallingConvention.Cdecl)]
    private static void ProcessContext(IntPtr context);

    public static void ProcessContext(ContextHandle context)
    {
      Alc.ProcessContext(context.Handle);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcSuspendContext", CallingConvention = CallingConvention.Cdecl)]
    private static void SuspendContext(IntPtr context);

    public static void SuspendContext(ContextHandle context)
    {
      Alc.SuspendContext(context.Handle);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcDestroyContext", CallingConvention = CallingConvention.Cdecl)]
    private static void DestroyContext(IntPtr context);

    public static void DestroyContext(ContextHandle context)
    {
      Alc.DestroyContext(context.Handle);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcGetCurrentContext", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr sys_GetCurrentContext();

    public static ContextHandle GetCurrentContext()
    {
      return new ContextHandle(Alc.sys_GetCurrentContext());
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcGetContextsDevice", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr GetContextsDevice(IntPtr context);

    public static IntPtr GetContextsDevice(ContextHandle context)
    {
      return Alc.GetContextsDevice(context.Handle);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcOpenDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr OpenDevice([In] string devicename);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcCloseDevice", CallingConvention = CallingConvention.Cdecl)]
    public static bool CloseDevice([In] IntPtr device);

    public static AlcError GetError(IntPtr device)
    {
      return AlcError.NoError;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcIsExtensionPresent", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static bool IsExtensionPresent([In] IntPtr device, [In] string extname);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcGetProcAddress", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr GetProcAddress([In] IntPtr device, [In] string funcname);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcGetEnumValue", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static int GetEnumValue([In] IntPtr device, [In] string enumname);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcGetString", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr GetStringPrivate([In] IntPtr device, AlcGetString param);

    public static string GetString(IntPtr device, AlcGetString param)
    {
      return Marshal.PtrToStringAnsi(Alc.GetStringPrivate(device, param));
    }

    public static unsafe IList<string> GetString(IntPtr device, AlcGetStringList param)
    {
      List<string> list = new List<string>();
      byte* numPtr = (byte*) Alc.GetStringPrivate(device, (AlcGetString) param).ToPointer();
      for (string str = Marshal.PtrToStringAnsi(new IntPtr((void*) numPtr)); !string.IsNullOrEmpty(str); str = Marshal.PtrToStringAnsi(new IntPtr((void*) numPtr)))
      {
        list.Add(str);
        numPtr += str.Length + 1;
      }
      return (IList<string>) list;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcGetIntegerv", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private static void GetInteger(IntPtr device, AlcGetInteger param, int size, int* data);

    public static unsafe void GetInteger(IntPtr device, AlcGetInteger param, int size, out int data)
    {
      fixed (int* data1 = &data)
        Alc.GetInteger(device, param, size, data1);
    }

    public static unsafe void GetInteger(IntPtr device, AlcGetInteger param, int size, int[] data)
    {
      fixed (int* data1 = data)
        Alc.GetInteger(device, param, size, data1);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcCaptureOpenDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr CaptureOpenDevice(string devicename, uint frequency, ALFormat format, int buffersize);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcCaptureOpenDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr CaptureOpenDevice(string devicename, int frequency, ALFormat format, int buffersize);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcCaptureCloseDevice", CallingConvention = CallingConvention.Cdecl)]
    public static bool CaptureCloseDevice([In] IntPtr device);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcCaptureStart", CallingConvention = CallingConvention.Cdecl)]
    public static void CaptureStart([In] IntPtr device);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcCaptureStop", CallingConvention = CallingConvention.Cdecl)]
    public static void CaptureStop([In] IntPtr device);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alcCaptureSamples", CallingConvention = CallingConvention.Cdecl)]
    public static void CaptureSamples(IntPtr device, IntPtr buffer, int samples);

    public static void CaptureSamples<T>(IntPtr device, ref T buffer, int samples) where T : struct
    {
      GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
      try
      {
        Alc.CaptureSamples(device, gcHandle.AddrOfPinnedObject(), samples);
      }
      finally
      {
        gcHandle.Free();
      }
    }

    public static void CaptureSamples<T>(IntPtr device, T[] buffer, int samples) where T : struct
    {
      Alc.CaptureSamples<T>(device, ref buffer[0], samples);
    }

    public static void CaptureSamples<T>(IntPtr device, T[,] buffer, int samples) where T : struct
    {
      Alc.CaptureSamples<T>(device, buffer.Address(0, 0), samples);
    }

    public static void CaptureSamples<T>(IntPtr device, T[,,] buffer, int samples) where T : struct
    {
      Alc.CaptureSamples<T>(device, buffer.Address(0, 0, 0), samples);
    }
  }
}
