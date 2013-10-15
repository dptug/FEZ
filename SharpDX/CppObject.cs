// Type: SharpDX.CppObject
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public class CppObject : DisposeBase
  {
    protected internal unsafe void* _nativePointer;

    public unsafe IntPtr NativePointer
    {
      get
      {
        return (IntPtr) this._nativePointer;
      }
      set
      {
        void* voidPtr1 = (void*) value;
        if (this._nativePointer == voidPtr1)
          return;
        this.NativePointerUpdating();
        void* voidPtr2 = this._nativePointer;
        this._nativePointer = voidPtr1;
        this.NativePointerUpdated((IntPtr) voidPtr2);
      }
    }

    public CppObject(IntPtr pointer)
    {
      this.NativePointer = pointer;
    }

    protected CppObject()
    {
    }

    public static explicit operator IntPtr(CppObject cppObject)
    {
      if (cppObject != null)
        return cppObject.NativePointer;
      else
        return IntPtr.Zero;
    }

    protected void FromTemp(CppObject temp)
    {
      this.NativePointer = temp.NativePointer;
      temp.NativePointer = IntPtr.Zero;
    }

    protected void FromTemp(IntPtr temp)
    {
      this.NativePointer = temp;
    }

    protected virtual void NativePointerUpdating()
    {
    }

    protected virtual void NativePointerUpdated(IntPtr oldNativePointer)
    {
    }

    protected override void Dispose(bool disposing)
    {
    }

    public static T FromPointer<T>(IntPtr comObjectPtr) where T : CppObject
    {
      if (comObjectPtr == IntPtr.Zero)
        return default (T);
      return (T) Activator.CreateInstance(typeof (T), new object[1]
      {
        (object) comObjectPtr
      });
    }

    internal static T FromPointerUnsafe<T>(IntPtr comObjectPtr)
    {
      if (comObjectPtr == IntPtr.Zero)
        return (T) null;
      return (T) Activator.CreateInstance(typeof (T), new object[1]
      {
        (object) comObjectPtr
      });
    }
  }
}
