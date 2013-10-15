// Type: SharpDX.Direct3D.Blob
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D
{
  [Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
  public class Blob : ComObject
  {
    public IntPtr BufferPointer
    {
      get
      {
        return this.GetBufferPointer();
      }
    }

    public PointerSize BufferSize
    {
      get
      {
        return this.GetBufferSize();
      }
    }

    public Blob(IntPtr nativePtr)
      : base(nativePtr)
    {
    }

    public static explicit operator Blob(IntPtr nativePointer)
    {
      if (!(nativePointer == IntPtr.Zero))
        return new Blob(nativePointer);
      else
        return (Blob) null;
    }

    internal unsafe IntPtr GetBufferPointer()
    {
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      return __calli((__FnPtr<IntPtr (void*)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(3) * sizeof (void*)))(this._nativePointer);
    }

    internal unsafe PointerSize GetBufferSize()
    {
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      return (PointerSize) (__calli((__FnPtr<void* (void*)>) *(IntPtr*) (*(IntPtr*) this._nativePointer + IntPtr(4) * sizeof (void*)))(this._nativePointer));
    }
  }
}
