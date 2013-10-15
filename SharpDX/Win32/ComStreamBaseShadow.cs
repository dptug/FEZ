// Type: SharpDX.Win32.ComStreamBaseShadow
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
  internal class ComStreamBaseShadow : ComObjectShadow
  {
    private static readonly ComStreamBaseShadow.ComStreamBaseVtbl Vtbl = new ComStreamBaseShadow.ComStreamBaseVtbl(0);

    protected override CppObjectVtbl GetVtbl
    {
      get
      {
        return (CppObjectVtbl) ComStreamBaseShadow.Vtbl;
      }
    }

    static ComStreamBaseShadow()
    {
    }

    internal class ComStreamBaseVtbl : ComObjectShadow.ComObjectVtbl
    {
      public ComStreamBaseVtbl(int numberOfMethods)
        : base(numberOfMethods + 2)
      {
        this.AddMethod((Delegate) new ComStreamBaseShadow.ComStreamBaseVtbl.ReadDelegate(ComStreamBaseShadow.ComStreamBaseVtbl.ReadImpl));
        this.AddMethod((Delegate) new ComStreamBaseShadow.ComStreamBaseVtbl.WriteDelegate(ComStreamBaseShadow.ComStreamBaseVtbl.WriteImpl));
      }

      private static int ReadImpl(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesRead)
      {
        bytesRead = 0;
        try
        {
          IStream stream = (IStream) CppObjectShadow.ToShadow<ComStreamBaseShadow>(thisPtr).Callback;
          bytesRead = stream.Read(buffer, sizeOfBytes);
        }
        catch (Exception ex)
        {
          return (int) Result.GetResultFromException(ex);
        }
        return Result.Ok.Code;
      }

      private static int WriteImpl(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesWrite)
      {
        bytesWrite = 0;
        try
        {
          IStream stream = (IStream) CppObjectShadow.ToShadow<ComStreamBaseShadow>(thisPtr).Callback;
          bytesWrite = stream.Write(buffer, sizeOfBytes);
        }
        catch (Exception ex)
        {
          return (int) Result.GetResultFromException(ex);
        }
        return Result.Ok.Code;
      }

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate int ReadDelegate(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesRead);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      private delegate int WriteDelegate(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesWrite);
    }
  }
}
