// Type: SharpDX.Win32.ComStreamBase
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
  [Guid("0c733a30-2a1c-11ce-ade5-00aa0044773d")]
  public class ComStreamBase : ComObjectCallback, IStreamBase, ICallbackable, IDisposable
  {
    public ComStreamBase(IntPtr nativePtr)
      : base(nativePtr)
    {
    }

    public static explicit operator ComStreamBase(IntPtr nativePointer)
    {
      if (!(nativePointer == IntPtr.Zero))
        return new ComStreamBase(nativePointer);
      else
        return (ComStreamBase) null;
    }

    public unsafe int Read(IntPtr vRef, int cb)
    {
      int num;
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      (Result) (__calli((__FnPtr<int (void*, void*, int, void*)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(3) * sizeof (void*)))(this._nativePointer, (int) (void*) vRef, (void*) cb, (void*) &num)).CheckError();
      return num;
    }

    public unsafe int Write(IntPtr vRef, int cb)
    {
      int num;
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      (Result) (__calli((__FnPtr<int (void*, void*, int, void*)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(4) * sizeof (void*)))(this._nativePointer, (int) (void*) vRef, (void*) cb, (void*) &num)).CheckError();
      return num;
    }
  }
}
