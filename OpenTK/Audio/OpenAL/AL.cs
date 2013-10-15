// Type: OpenTK.Audio.OpenAL.AL
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenTK.Audio.OpenAL
{
  public static class AL
  {
    internal const string Lib = "soft_oal.dll";
    private const CallingConvention Style = CallingConvention.Cdecl;

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alEnable", CallingConvention = CallingConvention.Cdecl)]
    public static void Enable(ALCapability capability);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alDisable", CallingConvention = CallingConvention.Cdecl)]
    public static void Disable(ALCapability capability);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alIsEnabled", CallingConvention = CallingConvention.Cdecl)]
    public static bool IsEnabled(ALCapability capability);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetString", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr GetStringPrivate(ALGetString param);

    public static string Get(ALGetString param)
    {
      return Marshal.PtrToStringAnsi(AL.GetStringPrivate(param));
    }

    public static string GetErrorString(ALError param)
    {
      return Marshal.PtrToStringAnsi(AL.GetStringPrivate((ALGetString) param));
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetInteger", CallingConvention = CallingConvention.Cdecl)]
    public static int Get(ALGetInteger param);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetFloat", CallingConvention = CallingConvention.Cdecl)]
    public static float Get(ALGetFloat param);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetError", CallingConvention = CallingConvention.Cdecl)]
    public static ALError GetError();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alIsExtensionPresent", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static bool IsExtensionPresent([In] string extname);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetProcAddress", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr GetProcAddress([In] string fname);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetEnumValue", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static int GetEnumValue([In] string ename);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alListenerf", CallingConvention = CallingConvention.Cdecl)]
    public static void Listener(ALListenerf param, float value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alListener3f", CallingConvention = CallingConvention.Cdecl)]
    public static void Listener(ALListener3f param, float value1, float value2, float value3);

    public static void Listener(ALListener3f param, ref Vector3 values)
    {
      AL.Listener(param, values.X, values.Y, values.Z);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alListenerfv", CallingConvention = CallingConvention.Cdecl)]
    private static void ListenerPrivate(ALListenerfv param, float* values);

    public static unsafe void Listener(ALListenerfv param, ref float[] values)
    {
      fixed (float* values1 = &values[0])
        AL.ListenerPrivate(param, values1);
    }

    public static unsafe void Listener(ALListenerfv param, ref Vector3 at, ref Vector3 up)
    {
      fixed (float* values = &new float[6]
      {
        at.X,
        at.Y,
        at.Z,
        up.X,
        up.Y,
        up.Z
      }[0])
        AL.ListenerPrivate(param, values);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetListenerf", CallingConvention = CallingConvention.Cdecl)]
    public static void GetListener(ALListenerf param, out float value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetListener3f", CallingConvention = CallingConvention.Cdecl)]
    public static void GetListener(ALListener3f param, out float value1, out float value2, out float value3);

    public static void GetListener(ALListener3f param, out Vector3 values)
    {
      AL.GetListener(param, out values.X, out values.Y, out values.Z);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetListenerfv", CallingConvention = CallingConvention.Cdecl)]
    public static void GetListener(ALListenerfv param, float* values);

    public static unsafe void GetListener(ALListenerfv param, out Vector3 at, out Vector3 up)
    {
      float[] numArray = new float[6];
      fixed (float* values = &numArray[0])
      {
        AL.GetListener(param, values);
        at.X = numArray[0];
        at.Y = numArray[1];
        at.Z = numArray[2];
        up.X = numArray[3];
        up.Y = numArray[4];
        up.Z = numArray[5];
      }
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGenSources", CallingConvention = CallingConvention.Cdecl)]
    private static void GenSourcesPrivate(int n, [Out] uint* sources);

    [CLSCompliant(false)]
    public static unsafe void GenSources(int n, out uint sources)
    {
      fixed (uint* sources1 = &sources)
        AL.GenSourcesPrivate(n, sources1);
    }

    public static unsafe void GenSources(int n, out int sources)
    {
      fixed (int* numPtr = &sources)
        AL.GenSourcesPrivate(n, (uint*) numPtr);
    }

    public static void GenSources(int[] sources)
    {
      uint[] numArray = new uint[sources.Length];
      AL.GenSources(numArray.Length, out numArray[0]);
      for (int index = 0; index < numArray.Length; ++index)
        sources[index] = (int) numArray[index];
    }

    public static int[] GenSources(int n)
    {
      uint[] numArray1 = new uint[n];
      AL.GenSources(numArray1.Length, out numArray1[0]);
      int[] numArray2 = new int[n];
      for (int index = 0; index < numArray1.Length; ++index)
        numArray2[index] = (int) numArray1[index];
      return numArray2;
    }

    public static int GenSource()
    {
      int sources;
      AL.GenSources(1, out sources);
      return sources;
    }

    [CLSCompliant(false)]
    public static void GenSource(out uint source)
    {
      AL.GenSources(1, out source);
    }

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alDeleteSources", CallingConvention = CallingConvention.Cdecl)]
    public static void DeleteSources(int n, [In] uint* sources);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alDeleteSources", CallingConvention = CallingConvention.Cdecl)]
    public static void DeleteSources(int n, ref uint sources);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alDeleteSources", CallingConvention = CallingConvention.Cdecl)]
    public static void DeleteSources(int n, ref int sources);

    [CLSCompliant(false)]
    public static void DeleteSources(uint[] sources)
    {
      if (sources == null)
        throw new ArgumentNullException();
      if (sources.Length == 0)
        throw new ArgumentOutOfRangeException();
      AL.DeleteBuffers(sources.Length, ref sources[0]);
    }

    public static void DeleteSources(int[] sources)
    {
      if (sources == null)
        throw new ArgumentNullException();
      if (sources.Length == 0)
        throw new ArgumentOutOfRangeException();
      AL.DeleteBuffers(sources.Length, ref sources[0]);
    }

    [CLSCompliant(false)]
    public static void DeleteSource(ref uint source)
    {
      AL.DeleteSources(1, ref source);
    }

    public static void DeleteSource(int source)
    {
      AL.DeleteSources(1, ref source);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alIsSource", CallingConvention = CallingConvention.Cdecl)]
    public static bool IsSource(uint sid);

    public static bool IsSource(int sid)
    {
      return AL.IsSource((uint) sid);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourcef", CallingConvention = CallingConvention.Cdecl)]
    public static void Source(uint sid, ALSourcef param, float value);

    public static void Source(int sid, ALSourcef param, float value)
    {
      AL.Source((uint) sid, param, value);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSource3f", CallingConvention = CallingConvention.Cdecl)]
    public static void Source(uint sid, ALSource3f param, float value1, float value2, float value3);

    public static void Source(int sid, ALSource3f param, float value1, float value2, float value3)
    {
      AL.Source((uint) sid, param, value1, value2, value3);
    }

    [CLSCompliant(false)]
    public static void Source(uint sid, ALSource3f param, ref Vector3 values)
    {
      AL.Source(sid, param, values.X, values.Y, values.Z);
    }

    public static void Source(int sid, ALSource3f param, ref Vector3 values)
    {
      AL.Source((uint) sid, param, values.X, values.Y, values.Z);
    }

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alSourcei", CallingConvention = CallingConvention.Cdecl)]
    public static void Source(uint sid, ALSourcei param, int value);

    public static void Source(int sid, ALSourcei param, int value)
    {
      AL.Source((uint) sid, param, value);
    }

    [CLSCompliant(false)]
    public static void Source(uint sid, ALSourceb param, bool value)
    {
      AL.Source(sid, (ALSourcei) param, value ? 1 : 0);
    }

    public static void Source(int sid, ALSourceb param, bool value)
    {
      AL.Source((uint) sid, (ALSourcei) param, value ? 1 : 0);
    }

    [CLSCompliant(false)]
    public static void BindBufferToSource(uint source, uint buffer)
    {
      AL.Source(source, ALSourcei.Buffer, (int) buffer);
    }

    public static void BindBufferToSource(int source, int buffer)
    {
      AL.Source((uint) source, ALSourcei.Buffer, buffer);
    }

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alSource3i", CallingConvention = CallingConvention.Cdecl)]
    public static void Source(uint sid, ALSource3i param, int value1, int value2, int value3);

    public static void Source(int sid, ALSource3i param, int value1, int value2, int value3)
    {
      AL.Source((uint) sid, param, value1, value2, value3);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetSourcef", CallingConvention = CallingConvention.Cdecl)]
    public static void GetSource(uint sid, ALSourcef param, out float value);

    public static void GetSource(int sid, ALSourcef param, out float value)
    {
      AL.GetSource((uint) sid, param, out value);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetSource3f", CallingConvention = CallingConvention.Cdecl)]
    public static void GetSource(uint sid, ALSource3f param, out float value1, out float value2, out float value3);

    public static void GetSource(int sid, ALSource3f param, out float value1, out float value2, out float value3)
    {
      AL.GetSource((uint) sid, param, out value1, out value2, out value3);
    }

    [CLSCompliant(false)]
    public static void GetSource(uint sid, ALSource3f param, out Vector3 values)
    {
      AL.GetSource(sid, param, out values.X, out values.Y, out values.Z);
    }

    public static void GetSource(int sid, ALSource3f param, out Vector3 values)
    {
      AL.GetSource((uint) sid, param, out values.X, out values.Y, out values.Z);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetSourcei", CallingConvention = CallingConvention.Cdecl)]
    public static void GetSource(uint sid, ALGetSourcei param, out int value);

    public static void GetSource(int sid, ALGetSourcei param, out int value)
    {
      AL.GetSource((uint) sid, param, out value);
    }

    [CLSCompliant(false)]
    public static void GetSource(uint sid, ALSourceb param, out bool value)
    {
      int num;
      AL.GetSource(sid, (ALGetSourcei) param, out num);
      value = num != 0;
    }

    public static void GetSource(int sid, ALSourceb param, out bool value)
    {
      int num;
      AL.GetSource((uint) sid, (ALGetSourcei) param, out num);
      value = num != 0;
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourcePlayv")]
    public static void SourcePlay(int ns, [In] uint* sids);

    [CLSCompliant(false)]
    public static unsafe void SourcePlay(int ns, uint[] sids)
    {
      fixed (uint* sids1 = sids)
        AL.SourcePlay(ns, sids1);
    }

    public static void SourcePlay(int ns, int[] sids)
    {
      uint[] sids1 = new uint[ns];
      for (int index = 0; index < ns; ++index)
        sids1[index] = (uint) sids[index];
      AL.SourcePlay(ns, sids1);
    }

    [CLSCompliant(false)]
    public static unsafe void SourcePlay(int ns, ref uint sids)
    {
      fixed (uint* sids1 = &sids)
        AL.SourcePlay(ns, sids1);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceStopv")]
    public static void SourceStop(int ns, [In] uint* sids);

    [CLSCompliant(false)]
    public static unsafe void SourceStop(int ns, uint[] sids)
    {
      fixed (uint* sids1 = sids)
        AL.SourceStop(ns, sids1);
    }

    public static void SourceStop(int ns, int[] sids)
    {
      uint[] sids1 = new uint[ns];
      for (int index = 0; index < ns; ++index)
        sids1[index] = (uint) sids[index];
      AL.SourceStop(ns, sids1);
    }

    [CLSCompliant(false)]
    public static unsafe void SourceStop(int ns, ref uint sids)
    {
      fixed (uint* sids1 = &sids)
        AL.SourceStop(ns, sids1);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceRewindv")]
    public static void SourceRewind(int ns, [In] uint* sids);

    [CLSCompliant(false)]
    public static unsafe void SourceRewind(int ns, uint[] sids)
    {
      fixed (uint* sids1 = sids)
        AL.SourceRewind(ns, sids1);
    }

    public static void SourceRewind(int ns, int[] sids)
    {
      uint[] sids1 = new uint[ns];
      for (int index = 0; index < ns; ++index)
        sids1[index] = (uint) sids[index];
      AL.SourceRewind(ns, sids1);
    }

    [CLSCompliant(false)]
    public static unsafe void SourceRewind(int ns, ref uint sids)
    {
      fixed (uint* sids1 = &sids)
        AL.SourceRewind(ns, sids1);
    }

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alSourcePausev")]
    public static void SourcePause(int ns, [In] uint* sids);

    [CLSCompliant(false)]
    public static unsafe void SourcePause(int ns, uint[] sids)
    {
      fixed (uint* sids1 = sids)
        AL.SourcePause(ns, sids1);
    }

    public static void SourcePause(int ns, int[] sids)
    {
      uint[] sids1 = new uint[ns];
      for (int index = 0; index < ns; ++index)
        sids1[index] = (uint) sids[index];
      AL.SourcePause(ns, sids1);
    }

    [CLSCompliant(false)]
    public static unsafe void SourcePause(int ns, ref uint sids)
    {
      fixed (uint* sids1 = &sids)
        AL.SourcePause(ns, sids1);
    }

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alSourcePlay", CallingConvention = CallingConvention.Cdecl)]
    public static void SourcePlay(uint sid);

    public static void SourcePlay(int sid)
    {
      AL.SourcePlay((uint) sid);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceStop", CallingConvention = CallingConvention.Cdecl)]
    public static void SourceStop(uint sid);

    public static void SourceStop(int sid)
    {
      AL.SourceStop((uint) sid);
    }

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceRewind", CallingConvention = CallingConvention.Cdecl)]
    public static void SourceRewind(uint sid);

    public static void SourceRewind(int sid)
    {
      AL.SourceRewind((uint) sid);
    }

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alSourcePause", CallingConvention = CallingConvention.Cdecl)]
    public static void SourcePause(uint sid);

    public static void SourcePause(int sid)
    {
      AL.SourcePause((uint) sid);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceQueueBuffers")]
    public static void SourceQueueBuffers(uint sid, int numEntries, [In] uint* bids);

    [CLSCompliant(false)]
    public static unsafe void SourceQueueBuffers(uint sid, int numEntries, uint[] bids)
    {
      fixed (uint* bids1 = bids)
        AL.SourceQueueBuffers(sid, numEntries, bids1);
    }

    public static void SourceQueueBuffers(int sid, int numEntries, int[] bids)
    {
      uint[] bids1 = new uint[numEntries];
      for (int index = 0; index < numEntries; ++index)
        bids1[index] = (uint) bids[index];
      AL.SourceQueueBuffers((uint) sid, numEntries, bids1);
    }

    [CLSCompliant(false)]
    public static unsafe void SourceQueueBuffers(uint sid, int numEntries, ref uint bids)
    {
      fixed (uint* bids1 = &bids)
        AL.SourceQueueBuffers(sid, numEntries, bids1);
    }

    public static unsafe void SourceQueueBuffer(int source, int buffer)
    {
      AL.SourceQueueBuffers((uint) source, 1, (uint*) &buffer);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceUnqueueBuffers")]
    public static void SourceUnqueueBuffers(uint sid, int numEntries, [In] uint* bids);

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceUnqueueBuffers")]
    public static void SourceUnqueueBuffers(uint sid, int numEntries, [Out] uint[] bids);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceUnqueueBuffers")]
    public static void SourceUnqueueBuffers(int sid, int numEntries, [Out] int[] bids);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceUnqueueBuffers")]
    public static void SourceUnqueueBuffers(uint sid, int numEntries, ref uint bids);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSourceUnqueueBuffers")]
    public static void SourceUnqueueBuffers(int sid, int numEntries, ref int bids);

    public static unsafe int SourceUnqueueBuffer(int sid)
    {
      uint num;
      AL.SourceUnqueueBuffers((uint) sid, 1, &num);
      return (int) num;
    }

    public static int[] SourceUnqueueBuffers(int sid, int numEntries)
    {
      if (numEntries <= 0)
        throw new ArgumentOutOfRangeException("numEntries", "Must be greater than zero.");
      int[] bids = new int[numEntries];
      AL.SourceUnqueueBuffers(sid, numEntries, bids);
      return bids;
    }

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alGenBuffers", CallingConvention = CallingConvention.Cdecl)]
    public static void GenBuffers(int n, [Out] uint* buffers);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alGenBuffers", CallingConvention = CallingConvention.Cdecl)]
    public static void GenBuffers(int n, [Out] int* buffers);

    [CLSCompliant(false)]
    public static unsafe void GenBuffers(int n, out uint buffers)
    {
      fixed (uint* buffers1 = &buffers)
        AL.GenBuffers(n, buffers1);
    }

    public static unsafe void GenBuffers(int n, out int buffers)
    {
      fixed (int* buffers1 = &buffers)
        AL.GenBuffers(n, buffers1);
    }

    public static int[] GenBuffers(int n)
    {
      int[] numArray = new int[n];
      AL.GenBuffers(numArray.Length, out numArray[0]);
      return numArray;
    }

    public static int GenBuffer()
    {
      int buffers;
      AL.GenBuffers(1, out buffers);
      return buffers;
    }

    [CLSCompliant(false)]
    public static void GenBuffer(out uint buffer)
    {
      AL.GenBuffers(1, out buffer);
    }

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alDeleteBuffers", CallingConvention = CallingConvention.Cdecl)]
    public static void DeleteBuffers(int n, [In] uint* buffers);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("soft_oal.dll", EntryPoint = "alDeleteBuffers", CallingConvention = CallingConvention.Cdecl)]
    public static void DeleteBuffers(int n, [In] int* buffers);

    [CLSCompliant(false)]
    public static unsafe void DeleteBuffers(int n, [In] ref uint buffers)
    {
      fixed (uint* buffers1 = &buffers)
        AL.DeleteBuffers(n, buffers1);
    }

    public static unsafe void DeleteBuffers(int n, [In] ref int buffers)
    {
      fixed (int* buffers1 = &buffers)
        AL.DeleteBuffers(n, buffers1);
    }

    [CLSCompliant(false)]
    public static void DeleteBuffers(uint[] buffers)
    {
      if (buffers == null)
        throw new ArgumentNullException();
      if (buffers.Length == 0)
        throw new ArgumentOutOfRangeException();
      AL.DeleteBuffers(buffers.Length, ref buffers[0]);
    }

    public static void DeleteBuffers(int[] buffers)
    {
      if (buffers == null)
        throw new ArgumentNullException();
      if (buffers.Length == 0)
        throw new ArgumentOutOfRangeException();
      AL.DeleteBuffers(buffers.Length, ref buffers[0]);
    }

    [CLSCompliant(false)]
    public static void DeleteBuffer(ref uint buffer)
    {
      AL.DeleteBuffers(1, ref buffer);
    }

    public static void DeleteBuffer(int buffer)
    {
      AL.DeleteBuffers(1, ref buffer);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alIsBuffer", CallingConvention = CallingConvention.Cdecl)]
    public static bool IsBuffer(uint bid);

    public static bool IsBuffer(int bid)
    {
      return AL.IsBuffer((uint) bid);
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alBufferData", CallingConvention = CallingConvention.Cdecl)]
    public static void BufferData(uint bid, ALFormat format, IntPtr buffer, int size, int freq);

    public static void BufferData(int bid, ALFormat format, IntPtr buffer, int size, int freq)
    {
      AL.BufferData((uint) bid, format, buffer, size, freq);
    }

    public static void BufferData<TBuffer>(int bid, ALFormat format, TBuffer[] buffer, int size, int freq) where TBuffer : struct
    {
      if (!BlittableValueType.Check<TBuffer>(buffer))
        throw new ArgumentException("buffer");
      GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
      try
      {
        AL.BufferData(bid, format, gcHandle.AddrOfPinnedObject(), size, freq);
      }
      finally
      {
        gcHandle.Free();
      }
    }

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alGetBufferi", CallingConvention = CallingConvention.Cdecl)]
    public static void GetBuffer(uint bid, ALGetBufferi param, out int value);

    public static void GetBuffer(int bid, ALGetBufferi param, out int value)
    {
      AL.GetBuffer((uint) bid, param, out value);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alDopplerFactor", CallingConvention = CallingConvention.Cdecl)]
    public static void DopplerFactor(float value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alDopplerVelocity", CallingConvention = CallingConvention.Cdecl)]
    public static void DopplerVelocity(float value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alSpeedOfSound", CallingConvention = CallingConvention.Cdecl)]
    public static void SpeedOfSound(float value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("soft_oal.dll", EntryPoint = "alDistanceModel", CallingConvention = CallingConvention.Cdecl)]
    public static void DistanceModel(ALDistanceModel distancemodel);

    [CLSCompliant(false)]
    public static ALSourceState GetSourceState(uint sid)
    {
      int num;
      AL.GetSource(sid, ALGetSourcei.SourceState, out num);
      return (ALSourceState) num;
    }

    public static ALSourceState GetSourceState(int sid)
    {
      int num;
      AL.GetSource(sid, ALGetSourcei.SourceState, out num);
      return (ALSourceState) num;
    }

    [CLSCompliant(false)]
    public static ALSourceType GetSourceType(uint sid)
    {
      int num;
      AL.GetSource(sid, ALGetSourcei.SourceType, out num);
      return (ALSourceType) num;
    }

    public static ALSourceType GetSourceType(int sid)
    {
      int num;
      AL.GetSource(sid, ALGetSourcei.SourceType, out num);
      return (ALSourceType) num;
    }

    public static ALDistanceModel GetDistanceModel()
    {
      return (ALDistanceModel) AL.Get(ALGetInteger.DistanceModel);
    }
  }
}
