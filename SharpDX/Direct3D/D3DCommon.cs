// Type: SharpDX.Direct3D.D3DCommon
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SharpDX.Direct3D
{
  internal static class D3DCommon
  {
    public static unsafe void CreateBlob(PointerSize size, Blob blobOut)
    {
      IntPtr num = IntPtr.Zero;
      Result result = (Result) D3DCommon.D3DCreateBlob_((void*) size, (void*) &num);
      blobOut.NativePointer = num;
      result.CheckError();
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("d3dcompiler_43.dll", EntryPoint = "D3DCreateBlob", CallingConvention = CallingConvention.StdCall)]
    private static int D3DCreateBlob_(void* arg0, void* arg1);
  }
}
