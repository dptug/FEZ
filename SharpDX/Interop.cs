// Type: SharpDX.Interop
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  internal class Interop
  {
    public static void* Fixed<T>(ref T data)
    {
      throw new NotImplementedException();
    }

    public static void* Fixed<T>(T[] data)
    {
      throw new NotImplementedException();
    }

    public static unsafe void* Cast<T>(ref T data) where T : struct
    {
      // ISSUE: explicit reference operation
      return (void*) @data;
    }

    public static unsafe void* CastOut<T>(out T data) where T : struct
    {
      // ISSUE: explicit reference operation
      return (void*) @data;
    }

    public static TCAST[] CastArray<TCAST, T>(T[] arrayData) where TCAST : struct where T : struct
    {
      return (TCAST[]) arrayData;
    }

    public static unsafe void memcpy(void* pDest, void* pSrc, int count)
    {
      // ISSUE: cpblk instruction
      __memcpy((IntPtr) pDest, (IntPtr) pSrc, count);
    }

    public static unsafe void memset(void* pDest, byte value, int count)
    {
      // ISSUE: initblk instruction
      __memset((IntPtr) pDest, (int) value, count);
    }

    public static unsafe void* Read<T>(void* pSrc, ref T data) where T : struct
    {
      fixed (T* objPtr = &data)
      {
        void* voidPtr = pSrc;
        int num1 = sizeof (T);
        int num2 = num1;
        // ISSUE: cast to a reference type
        // ISSUE: cpblk instruction
        __memcpy((T&) objPtr, (IntPtr) voidPtr, num2);
        return (void*) ((IntPtr) num1 + (IntPtr) pSrc);
      }
    }

    public static unsafe T ReadInline<T>(void* pSrc) where T : struct
    {
      throw new NotImplementedException();
    }

    public static unsafe void WriteInline<T>(void* pDest, ref T data) where T : struct
    {
      throw new NotImplementedException();
    }

    public static unsafe void CopyInline<T>(ref T data, void* pSrc) where T : struct
    {
      throw new NotImplementedException();
    }

    public static unsafe void CopyInline<T>(void* pDest, ref T srcData) where T : struct
    {
      throw new NotImplementedException();
    }

    public static unsafe void CopyInlineOut<T>(out T data, void* pSrc) where T : struct
    {
      throw new NotImplementedException();
    }

    public static unsafe void* ReadOut<T>(void* pSrc, out T data) where T : struct
    {
      fixed (T* objPtr = &data)
      {
        void* voidPtr = pSrc;
        int num1 = sizeof (T);
        int num2 = num1;
        // ISSUE: cast to a reference type
        // ISSUE: cpblk instruction
        __memcpy((T&) objPtr, (IntPtr) voidPtr, num2);
        return (void*) ((IntPtr) num1 + (IntPtr) pSrc);
      }
    }

    public static unsafe void* Read<T>(void* pSrc, T[] data, int offset, int count) where T : struct
    {
      fixed (T* objPtr = &data[offset])
      {
        void* voidPtr = pSrc;
        int num1 = sizeof (T) * count;
        int num2 = num1;
        // ISSUE: cast to a reference type
        // ISSUE: cpblk instruction
        __memcpy((T&) objPtr, (IntPtr) voidPtr, num2);
        return (void*) ((IntPtr) num1 + (IntPtr) pSrc);
      }
    }

    public static unsafe void* Read2D<T>(void* pSrc, T[,] data, int offset, int count) where T : struct
    {
      fixed (T* objPtr = &data[offset])
      {
        void* voidPtr = pSrc;
        int num1 = sizeof (T) * count;
        int num2 = num1;
        // ISSUE: cast to a reference type
        // ISSUE: cpblk instruction
        __memcpy((T&) objPtr, (IntPtr) voidPtr, num2);
        return (void*) ((IntPtr) num1 + (IntPtr) pSrc);
      }
    }

    public static int SizeOf<T>()
    {
      throw new NotImplementedException();
    }

    public static unsafe void* Write<T>(void* pDest, ref T data) where T : struct
    {
      void* voidPtr = pDest;
      fixed (T* objPtr = &data)
      {
        int num1 = sizeof (T);
        int num2 = num1;
        // ISSUE: cast to a reference type
        // ISSUE: cpblk instruction
        __memcpy((IntPtr) voidPtr, (T&) objPtr, num2);
        return (void*) ((IntPtr) num1 + (IntPtr) pDest);
      }
    }

    public static unsafe void* Write<T>(void* pDest, T[] data, int offset, int count) where T : struct
    {
      void* voidPtr = pDest;
      fixed (T* objPtr = &data[offset])
      {
        int num1 = sizeof (T) * count;
        int num2 = num1;
        // ISSUE: cast to a reference type
        // ISSUE: cpblk instruction
        __memcpy((IntPtr) voidPtr, (T&) objPtr, num2);
        return (void*) ((IntPtr) num1 + (IntPtr) pDest);
      }
    }

    public static unsafe void* Write2D<T>(void* pDest, T[,] data, int offset, int count) where T : struct
    {
      void* voidPtr = pDest;
      fixed (T* objPtr = &data[offset])
      {
        int num1 = sizeof (T) * count;
        int num2 = num1;
        // ISSUE: cast to a reference type
        // ISSUE: cpblk instruction
        __memcpy((IntPtr) voidPtr, (T&) objPtr, num2);
        return (void*) ((IntPtr) num1 + (IntPtr) pDest);
      }
    }

    [Tag("SharpDX.ModuleInit")]
    public static void ModuleInit()
    {
    }
  }
}
