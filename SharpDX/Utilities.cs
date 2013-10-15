// Type: SharpDX.Utilities
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Direct3D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace SharpDX
{
  public static class Utilities
  {
    public static unsafe void CopyMemory(IntPtr dest, IntPtr src, int sizeInBytesToCopy)
    {
      Interop.memcpy((void*) dest, (void*) src, sizeInBytesToCopy);
    }

    public static unsafe bool CompareMemory(IntPtr from, IntPtr against, int sizeToCompare)
    {
      byte* numPtr1 = (byte*) (void*) from;
      byte* numPtr2 = (byte*) (void*) against;
      for (int index = sizeToCompare >> 3; index > 0; --index)
      {
        if (*(long*) numPtr1 != *(long*) numPtr2)
          return false;
        numPtr1 += 8;
        numPtr2 += 8;
      }
      for (int index = sizeToCompare & 7; index > 0; --index)
      {
        if ((int) *numPtr1 != (int) *numPtr2)
          return false;
        ++numPtr1;
        ++numPtr2;
      }
      return true;
    }

    public static unsafe void ClearMemory(IntPtr dest, byte value, int sizeInBytesToClear)
    {
      Interop.memset((void*) dest, value, sizeInBytesToClear);
    }

    public static int SizeOf<T>() where T : struct
    {
      return sizeof (T);
    }

    public static int SizeOf<T>(T[] array) where T : struct
    {
      if (array != null)
        return array.Length * sizeof (T);
      else
        return 0;
    }

    public static unsafe void Pin<T>(ref T source, Action<IntPtr> pinAction) where T : struct
    {
      Action<IntPtr> action = pinAction;
      fixed (T* objPtr = &source)
      {
        IntPtr num = (IntPtr) ((void*) objPtr);
        action(num);
      }
    }

    public static unsafe void Pin<T>(T[] source, Action<IntPtr> pinAction) where T : struct
    {
      Action<IntPtr> action = pinAction;
      IntPtr num;
      if (source != null)
      {
        fixed (T* objPtr = &source[0])
          num = (IntPtr) ((void*) objPtr);
      }
      else
        num = IntPtr.Zero;
      action(num);
    }

    public static unsafe byte[] ToByteArray<T>(T[] source) where T : struct
    {
      if (source == null)
        return (byte[]) null;
      byte[] numArray = new byte[Utilities.SizeOf<T>() * source.Length];
      if (source.Length == 0)
        return numArray;
      fixed (byte* numPtr = numArray)
        Interop.Write<T>((void*) numPtr, source, 0, source.Length);
      return numArray;
    }

    public static unsafe T Read<T>(IntPtr source) where T : struct
    {
      return *(T*) (void*) source;
    }

    public static unsafe void Read<T>(IntPtr source, ref T data) where T : struct
    {
      // ISSUE: explicit reference operation
      *(T*) @data = *(T*) (void*) source;
    }

    public static unsafe void ReadOut<T>(IntPtr source, out T data) where T : struct
    {
      // ISSUE: explicit reference operation
      *(T*) @data = *(T*) (void*) source;
    }

    public static unsafe IntPtr ReadAndPosition<T>(IntPtr source, ref T data) where T : struct
    {
      return (IntPtr) Interop.Read<T>((void*) source, ref data);
    }

    public static unsafe IntPtr Read<T>(IntPtr source, T[] data, int offset, int count) where T : struct
    {
      return (IntPtr) Interop.Read<T>((void*) source, data, offset, count);
    }

    public static unsafe void Write<T>(IntPtr destination, ref T data) where T : struct
    {
      // ISSUE: explicit reference operation
      *(T*) (void*) destination = *(T*) @data;
    }

    public static unsafe IntPtr WriteAndPosition<T>(IntPtr destination, ref T data) where T : struct
    {
      return (IntPtr) Interop.Write<T>((void*) destination, ref data);
    }

    public static unsafe IntPtr Write<T>(IntPtr destination, T[] data, int offset, int count) where T : struct
    {
      return (IntPtr) Interop.Write<T>((void*) destination, data, offset, count);
    }

    public static unsafe void ConvertToIntArray(bool[] array, int* dest)
    {
      for (int index = 0; index < array.Length; ++index)
        dest[index] = array[index] ? 1 : 0;
    }

    public static unsafe bool[] ConvertToBoolArray(int* array, int length)
    {
      bool[] flagArray = new bool[length];
      for (int index = 0; index < flagArray.Length; ++index)
        flagArray[index] = array[index] != 0;
      return flagArray;
    }

    public static Bool[] ConvertToIntArray(bool[] array)
    {
      Bool[] boolArray = new Bool[array.Length];
      for (int index = 0; index < boolArray.Length; ++index)
        boolArray[index] = (Bool) array[index];
      return boolArray;
    }

    public static bool[] ConvertToBoolArray(Bool[] array)
    {
      bool[] flagArray = new bool[array.Length];
      for (int index = 0; index < flagArray.Length; ++index)
        flagArray[index] = (bool) array[index];
      return flagArray;
    }

    public static Guid GetGuidFromType(Type type)
    {
      return type.GUID;
    }

    public static unsafe IntPtr AllocateMemory(int sizeInBytes, int align = 16)
    {
      int num1 = align - 1;
      IntPtr num2 = Marshal.AllocHGlobal(sizeInBytes + num1 + IntPtr.Size);
      long num3 = (long) (ulong) ((UIntPtr) ((void**) (void*) num2 + 1) + (UIntPtr) num1) & (long) ~num1;
      *(IntPtr*) ((IntPtr) (ulong) num3 + IntPtr(-1) * sizeof (IntPtr)) = num2;
      return new IntPtr(num3);
    }

    public static IntPtr AllocateClearedMemory(int sizeInBytes, byte clearValue = (byte) 0, int align = 16)
    {
      IntPtr dest = Utilities.AllocateMemory(sizeInBytes, align);
      Utilities.ClearMemory(dest, clearValue, sizeInBytes);
      return dest;
    }

    public static bool IsMemoryAligned(IntPtr memoryPtr, int align = 16)
    {
      return (memoryPtr.ToInt64() & (long) (align - 1)) == 0L;
    }

    public static unsafe void FreeMemory(IntPtr alignedBuffer)
    {
      Marshal.FreeHGlobal(((IntPtr*) (void*) alignedBuffer)[-1]);
    }

    public static unsafe string PtrToStringAnsi(IntPtr pointer, int maxLength)
    {
      byte* numPtr = (byte*) (void*) pointer;
      for (int index = 0; index < maxLength; ++index)
      {
        if ((int) *numPtr++ == 0)
          return new string((sbyte*) (void*) pointer);
      }
      return new string((sbyte*) (void*) pointer, 0, maxLength);
    }

    public static unsafe string PtrToStringUni(IntPtr pointer, int maxLength)
    {
      char* chPtr = (char*) (void*) pointer;
      for (int index = 0; index < maxLength; ++index)
      {
        if ((int) *chPtr++ == 0)
          return new string((char*) (void*) pointer);
      }
      return new string((char*) (void*) pointer, 0, maxLength);
    }

    public static IntPtr StringToHGlobalAnsi(string s)
    {
      return Marshal.StringToHGlobalAnsi(s);
    }

    public static IntPtr StringToHGlobalUni(string s)
    {
      return Marshal.StringToHGlobalUni(s);
    }

    public static IntPtr GetIUnknownForObject(object obj)
    {
      return obj == null ? IntPtr.Zero : Marshal.GetIUnknownForObject(obj);
    }

    public static object GetObjectForIUnknown(IntPtr iunknownPtr)
    {
      if (!(iunknownPtr == IntPtr.Zero))
        return Marshal.GetObjectForIUnknown(iunknownPtr);
      else
        return (object) null;
    }

    public static string Join<T>(string separator, T[] array)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (array != null)
      {
        for (int index = 0; index < array.Length; ++index)
        {
          if (index > 0)
            stringBuilder.Append(separator);
          stringBuilder.Append((object) array[index]);
        }
      }
      return ((object) stringBuilder).ToString();
    }

    public static string Join(string separator, IEnumerable elements)
    {
      List<string> list = new List<string>();
      foreach (object obj in elements)
        list.Add(obj.ToString());
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < list.Count; ++index)
      {
        string str = list[index];
        if (index > 0)
          stringBuilder.Append(separator);
        stringBuilder.Append(str);
      }
      return ((object) stringBuilder).ToString();
    }

    public static string Join(string separator, IEnumerator elements)
    {
      List<string> list = new List<string>();
      while (elements.MoveNext())
        list.Add(elements.Current.ToString());
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < list.Count; ++index)
      {
        string str = list[index];
        if (index > 0)
          stringBuilder.Append(separator);
        stringBuilder.Append(str);
      }
      return ((object) stringBuilder).ToString();
    }

    public static unsafe string BlobToString(Blob blob)
    {
      if (blob == null)
        return (string) null;
      string str = new string((sbyte*) (void*) blob.BufferPointer);
      blob.Dispose();
      return str;
    }

    public static unsafe IntPtr IntPtrAdd(IntPtr ptr, int offset)
    {
      return new IntPtr((void*) ((IntPtr) (void*) ptr + offset));
    }

    public static byte[] ReadStream(Stream stream)
    {
      int readLength = 0;
      return Utilities.ReadStream(stream, ref readLength);
    }

    public static byte[] ReadStream(Stream stream, ref int readLength)
    {
      if (readLength == 0)
        readLength = (int) (stream.Length - stream.Position);
      int length = readLength;
      if (length == 0)
        return new byte[0];
      byte[] buffer = new byte[length];
      int offset = 0;
      if (length > 0)
      {
        do
        {
          offset += stream.Read(buffer, offset, readLength - offset);
        }
        while (offset < readLength);
      }
      return buffer;
    }

    public static bool Compare(IEnumerable left, IEnumerable right)
    {
      if (object.ReferenceEquals((object) left, (object) right))
        return true;
      if (object.ReferenceEquals((object) left, (object) null) || object.ReferenceEquals((object) right, (object) null))
        return false;
      else
        return Utilities.Compare(left.GetEnumerator(), right.GetEnumerator());
    }

    public static bool Compare(IEnumerator leftIt, IEnumerator rightIt)
    {
      if (object.ReferenceEquals((object) leftIt, (object) rightIt))
        return true;
      if (object.ReferenceEquals((object) leftIt, (object) null) || object.ReferenceEquals((object) rightIt, (object) null))
        return false;
      bool flag1;
      bool flag2;
      do
      {
        flag1 = leftIt.MoveNext();
        flag2 = rightIt.MoveNext();
        if (!flag1 || !flag2)
          goto label_7;
      }
      while (object.Equals(leftIt.Current, rightIt.Current));
      return false;
label_7:
      return flag1 == flag2;
    }

    public static bool Compare(ICollection left, ICollection right)
    {
      if (object.ReferenceEquals((object) left, (object) right))
        return true;
      if (object.ReferenceEquals((object) left, (object) null) || object.ReferenceEquals((object) right, (object) null) || left.Count != right.Count)
        return false;
      int num = 0;
      IEnumerator enumerator1 = left.GetEnumerator();
      IEnumerator enumerator2 = right.GetEnumerator();
      while (enumerator1.MoveNext() && enumerator2.MoveNext())
      {
        if (!object.Equals(enumerator1.Current, enumerator2.Current))
          return false;
        ++num;
      }
      return num == left.Count;
    }

    public static T GetCustomAttribute<T>(MemberInfo memberInfo, bool inherited = false) where T : Attribute
    {
      object[] customAttributes = memberInfo.GetCustomAttributes(typeof (T), inherited);
      if (customAttributes.Length == 0)
        return default (T);
      else
        return (T) customAttributes[0];
    }

    public static IEnumerable<T> GetCustomAttributes<T>(MemberInfo memberInfo, bool inherited = false) where T : Attribute
    {
      object[] customAttributes = memberInfo.GetCustomAttributes(typeof (T), inherited);
      if (customAttributes.Length == 0)
        return (IEnumerable<T>) new T[0];
      T[] objArray = new T[customAttributes.Length];
      Array.Copy((Array) customAttributes, (Array) objArray, customAttributes.Length);
      return (IEnumerable<T>) objArray;
    }

    public static bool IsAssignableFrom(Type toType, Type fromType)
    {
      return toType.IsAssignableFrom(fromType);
    }

    public static bool IsEnum(Type typeToTest)
    {
      return typeToTest.IsEnum;
    }

    public static bool IsValueType(Type typeToTest)
    {
      return typeToTest.IsValueType;
    }

    private static MethodInfo GetMethod(Type type, string name, Type[] typeArgs)
    {
      return type.GetMethod(name, typeArgs);
    }

    public static GetValueFastDelegate<T> BuildPropertyGetter<T>(Type customEffectType, PropertyInfo propertyInfo)
    {
      Type targetType = typeof (T);
      Type propertyType = propertyInfo.PropertyType;
      DynamicMethod dynamicMethod = new DynamicMethod("GetValueDelegate", typeof (void), new Type[2]
      {
        typeof (object),
        targetType.MakeByRefType()
      });
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Castclass, customEffectType);
      ilGenerator.EmitCall(OpCodes.Callvirt, propertyInfo.GetGetMethod(), (Type[]) null);
      if (targetType == typeof (byte) || targetType == typeof (sbyte))
        ilGenerator.Emit(OpCodes.Stind_I1);
      else if (targetType == typeof (short) || targetType == typeof (ushort))
        ilGenerator.Emit(OpCodes.Stind_I2);
      else if (targetType == typeof (int) || targetType == typeof (uint))
      {
        if (propertyType == typeof (bool))
          ilGenerator.EmitCall(OpCodes.Call, Utilities.GetMethod(typeof (Convert), "ToInt32", new Type[1]
          {
            typeof (bool)
          }), (Type[]) null);
        ilGenerator.Emit(OpCodes.Stind_I4);
      }
      else if (targetType == typeof (long) || targetType == typeof (ulong))
        ilGenerator.Emit(OpCodes.Stind_I8);
      else if (targetType == typeof (float))
        ilGenerator.Emit(OpCodes.Stind_R4);
      else if (targetType == typeof (double))
      {
        ilGenerator.Emit(OpCodes.Stind_R8);
      }
      else
      {
        MethodInfo explicitConverstion = Utilities.FindExplicitConverstion(propertyType, targetType);
        if (explicitConverstion != (MethodInfo) null)
          ilGenerator.EmitCall(OpCodes.Call, explicitConverstion, (Type[]) null);
        ilGenerator.Emit(OpCodes.Stobj, typeof (T));
      }
      ilGenerator.Emit(OpCodes.Ret);
      return (GetValueFastDelegate<T>) dynamicMethod.CreateDelegate(typeof (GetValueFastDelegate<T>));
    }

    public static SetValueFastDelegate<T> BuildPropertySetter<T>(Type customEffectType, PropertyInfo propertyInfo)
    {
      Type sourceType = typeof (T);
      Type propertyType = propertyInfo.PropertyType;
      DynamicMethod dynamicMethod = new DynamicMethod("SetValueDelegate", typeof (void), new Type[2]
      {
        typeof (object),
        sourceType.MakeByRefType()
      });
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Castclass, customEffectType);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      if (sourceType == typeof (byte) || sourceType == typeof (sbyte))
        ilGenerator.Emit(OpCodes.Ldind_I1);
      else if (sourceType == typeof (short) || sourceType == typeof (ushort))
        ilGenerator.Emit(OpCodes.Ldind_I2);
      else if (sourceType == typeof (int) || sourceType == typeof (uint))
      {
        ilGenerator.Emit(OpCodes.Ldind_I4);
        if (propertyType == typeof (bool))
          ilGenerator.EmitCall(OpCodes.Call, Utilities.GetMethod(typeof (Convert), "ToBoolean", new Type[1]
          {
            sourceType
          }), (Type[]) null);
      }
      else if (sourceType == typeof (long) || sourceType == typeof (ulong))
        ilGenerator.Emit(OpCodes.Ldind_I8);
      else if (sourceType == typeof (float))
        ilGenerator.Emit(OpCodes.Ldind_R4);
      else if (sourceType == typeof (double))
      {
        ilGenerator.Emit(OpCodes.Ldind_R8);
      }
      else
      {
        ilGenerator.Emit(OpCodes.Ldobj, typeof (T));
        MethodInfo explicitConverstion = Utilities.FindExplicitConverstion(sourceType, propertyType);
        if (explicitConverstion != (MethodInfo) null)
          ilGenerator.EmitCall(OpCodes.Call, explicitConverstion, (Type[]) null);
      }
      ilGenerator.EmitCall(OpCodes.Callvirt, propertyInfo.GetSetMethod(), (Type[]) null);
      ilGenerator.Emit(OpCodes.Ret);
      return (SetValueFastDelegate<T>) dynamicMethod.CreateDelegate(typeof (SetValueFastDelegate<T>));
    }

    public static void Sleep(TimeSpan sleepTimeInMillis)
    {
      Thread.Sleep(sleepTimeInMillis);
    }

    private static MethodInfo FindExplicitConverstion(Type sourceType, Type targetType)
    {
      if (sourceType == targetType)
        return (MethodInfo) null;
      List<MethodInfo> list = new List<MethodInfo>();
      for (Type type = sourceType; type != (Type) null; type = type.BaseType)
        list.AddRange((IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.Static | BindingFlags.Public));
      for (Type type = targetType; type != (Type) null; type = type.BaseType)
        list.AddRange((IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.Static | BindingFlags.Public));
      foreach (MethodInfo methodInfo in list)
      {
        if (methodInfo.Name == "op_Explicit" && methodInfo.ReturnType == targetType && Utilities.IsAssignableFrom(methodInfo.GetParameters()[0].ParameterType, sourceType))
          return methodInfo;
      }
      return (MethodInfo) null;
    }

    [DllImport("ole32.dll")]
    private static Result CoCreateInstance([MarshalAs(UnmanagedType.LPStruct), In] Guid rclsid, IntPtr pUnkOuter, Utilities.CLSCTX dwClsContext, [MarshalAs(UnmanagedType.LPStruct), In] Guid riid, out IntPtr comObject);

    internal static void CreateComInstance(Guid clsid, Utilities.CLSCTX clsctx, Guid riid, ComObject comObject)
    {
      IntPtr comObject1;
      Utilities.CoCreateInstance(clsid, IntPtr.Zero, clsctx, riid, out comObject1).CheckError();
      comObject.NativePointer = comObject1;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static bool CloseHandle(IntPtr handle);

    public static IntPtr LoadLibrary(string dllName)
    {
      IntPtr num = Utilities.LoadLibrary_(dllName);
      if (num == IntPtr.Zero)
        throw new DllNotFoundException(string.Format("Unable to find [{0}] in the PATH", (object) dllName));
      else
        return num;
    }

    [DllImport("kernel32", EntryPoint = "LoadLibrary", CharSet = CharSet.Ansi, SetLastError = true)]
    private static IntPtr LoadLibrary_(string lpFileName);

    public static IntPtr GetProcAddress(IntPtr handle, string dllFunctionToImport)
    {
      IntPtr procAddress = Utilities.GetProcAddress_(handle, dllFunctionToImport);
      if (procAddress == IntPtr.Zero)
        throw new SharpDXException(dllFunctionToImport, new object[0]);
      else
        return procAddress;
    }

    [DllImport("kernel32", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, SetLastError = true)]
    private static IntPtr GetProcAddress_(IntPtr hModule, string procName);

    public static int ComputeHashFNVModified(byte[] data)
    {
      uint num1 = 2166136261U;
      foreach (byte num2 in data)
        num1 = (uint) (((int) num1 ^ (int) num2) * 16777619);
      uint num3 = num1 + (num1 << 13);
      uint num4 = num3 ^ num3 >> 7;
      uint num5 = num4 + (num4 << 3);
      uint num6 = num5 ^ num5 >> 17;
      return (int) (num6 + (num6 << 5));
    }

    [System.Flags]
    public enum CLSCTX : uint
    {
      ClsctxInprocServer = 1U,
      ClsctxInprocHandler = 2U,
      ClsctxLocalServer = 4U,
      ClsctxInprocServer16 = 8U,
      ClsctxRemoteServer = 16U,
      ClsctxInprocHandler16 = 32U,
      ClsctxReserved1 = 64U,
      ClsctxReserved2 = 128U,
      ClsctxReserved3 = 256U,
      ClsctxReserved4 = 512U,
      ClsctxNoCodeDownload = 1024U,
      ClsctxReserved5 = 2048U,
      ClsctxNoCustomMarshal = 4096U,
      ClsctxEnableCodeDownload = 8192U,
      ClsctxNoFailureLog = 16384U,
      ClsctxDisableAaa = 32768U,
      ClsctxEnableAaa = 65536U,
      ClsctxFromDefaultContext = 131072U,
      ClsctxInproc = ClsctxInprocHandler | ClsctxInprocServer,
      ClsctxServer = ClsctxRemoteServer | ClsctxLocalServer | ClsctxInprocServer,
      ClsctxAll = ClsctxServer | ClsctxInprocHandler,
    }

    public enum CoInit
    {
      MultiThreaded = 0,
      ApartmentThreaded = 2,
      DisableOle1Dde = 4,
      SpeedOverMemory = 8,
    }
  }
}
