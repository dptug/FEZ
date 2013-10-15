// Type: SharpDX.CppObjectShadow
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Runtime.InteropServices;

namespace SharpDX
{
  internal abstract class CppObjectShadow : CppObject
  {
    public ICallbackable Callback { get; private set; }

    protected abstract CppObjectVtbl GetVtbl { get; }

    public virtual unsafe void Initialize(ICallbackable callbackInstance)
    {
      this.Callback = callbackInstance;
      this.NativePointer = Marshal.AllocHGlobal(IntPtr.Size * 2);
      GCHandle gcHandle = GCHandle.Alloc((object) this);
      Marshal.WriteIntPtr(this.NativePointer, this.GetVtbl.Pointer);
      ((IntPtr*) (void*) this.NativePointer)[1] = GCHandle.ToIntPtr(gcHandle);
    }

    public static IntPtr ToIntPtr<TCallback>(ICallbackable callback) where TCallback : ICallbackable
    {
      if (callback == null)
        return IntPtr.Zero;
      if (callback is CppObject)
        return ((CppObject) callback).NativePointer;
      ShadowContainer shadowContainer = callback.Shadow as ShadowContainer;
      if (shadowContainer == null)
      {
        shadowContainer = new ShadowContainer();
        shadowContainer.Initialize(callback);
      }
      return shadowContainer.Find(typeof (TCallback));
    }

    protected override unsafe void Dispose(bool disposing)
    {
      if (this.NativePointer != IntPtr.Zero)
      {
        GCHandle.FromIntPtr(((IntPtr*) (void*) this.NativePointer)[1]).Free();
        Marshal.FreeHGlobal(this.NativePointer);
        this.NativePointer = IntPtr.Zero;
      }
      this.Callback = (ICallbackable) null;
      base.Dispose(disposing);
    }

    internal static unsafe T ToShadow<T>(IntPtr thisPtr) where T : CppObjectShadow
    {
      return (T) GCHandle.FromIntPtr(((IntPtr*) (void*) thisPtr)[1]).Target;
    }
  }
}
