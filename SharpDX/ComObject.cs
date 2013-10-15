// Type: SharpDX.ComObject
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Diagnostics;
using System;
using System.Runtime.InteropServices;

namespace SharpDX
{
  public class ComObject : CppObject, IUnknown
  {
    public ComObject(IntPtr pointer)
      : base(pointer)
    {
    }

    public ComObject(object iunknowObject)
    {
      this.NativePointer = Marshal.GetIUnknownForObject(iunknowObject);
    }

    protected ComObject()
    {
    }

    public static explicit operator ComObject(IntPtr nativePointer)
    {
      if (!(nativePointer == IntPtr.Zero))
        return new ComObject(nativePointer);
      else
        return (ComObject) null;
    }

    public virtual void QueryInterface(Guid guid, out IntPtr outPtr)
    {
      this.QueryInterface(ref guid, out outPtr).CheckError();
    }

    public virtual IntPtr QueryInterfaceOrNull(Guid guid)
    {
      IntPtr comObject = IntPtr.Zero;
      this.QueryInterface(ref guid, out comObject);
      return comObject;
    }

    public virtual T QueryInterface<T>() where T : ComObject
    {
      IntPtr outPtr;
      this.QueryInterface(Utilities.GetGuidFromType(typeof (T)), out outPtr);
      return CppObject.FromPointer<T>(outPtr);
    }

    internal virtual T QueryInterfaceUnsafe<T>()
    {
      IntPtr outPtr;
      this.QueryInterface(Utilities.GetGuidFromType(typeof (T)), out outPtr);
      return CppObject.FromPointerUnsafe<T>(outPtr);
    }

    public static T As<T>(object comObject) where T : ComObject
    {
      using (ComObject comObject1 = new ComObject(Marshal.GetIUnknownForObject(comObject)))
        return comObject1.QueryInterface<T>();
    }

    public static T As<T>(IntPtr iunknownPtr) where T : ComObject
    {
      using (ComObject comObject = new ComObject(iunknownPtr))
        return comObject.QueryInterface<T>();
    }

    internal static T AsUnsafe<T>(IntPtr iunknownPtr)
    {
      using (ComObject comObject = new ComObject(iunknownPtr))
        return comObject.QueryInterfaceUnsafe<T>();
    }

    public static void Dispose<T>(ref T comObject) where T : class, IDisposable
    {
      if ((object) comObject == null)
        return;
      comObject.Dispose();
      comObject = default (T);
    }

    public static T QueryInterface<T>(object comObject) where T : ComObject
    {
      using (ComObject comObject1 = new ComObject(Marshal.GetIUnknownForObject(comObject)))
        return comObject1.QueryInterface<T>();
    }

    public virtual T QueryInterfaceOrNull<T>() where T : ComObject
    {
      return CppObject.FromPointer<T>(this.QueryInterfaceOrNull(Utilities.GetGuidFromType(typeof (T))));
    }

    protected void QueryInterfaceFrom<T>(T fromObject) where T : ComObject
    {
      IntPtr outPtr;
      fromObject.QueryInterface(Utilities.GetGuidFromType(this.GetType()), out outPtr);
      this.NativePointer = outPtr;
    }

    Result IUnknown.QueryInterface(ref Guid guid, out IntPtr comObject)
    {
      return (Result) Marshal.QueryInterface(this.NativePointer, ref guid, out comObject);
    }

    int IUnknown.AddReference()
    {
      if (this.NativePointer == IntPtr.Zero)
        throw new InvalidOperationException("COM Object pointer is null");
      else
        return Marshal.AddRef(this.NativePointer);
    }

    int IUnknown.Release()
    {
      if (this.NativePointer == IntPtr.Zero)
        throw new InvalidOperationException("COM Object pointer is null");
      else
        return Marshal.Release(this.NativePointer);
    }

    protected override unsafe void Dispose(bool disposing)
    {
      if (this.NativePointer != IntPtr.Zero)
      {
        if (disposing || Configuration.EnableReleaseOnFinalizer)
          this.Release();
        if (Configuration.EnableObjectTracking)
          ObjectTracker.UnTrack(this);
        this._nativePointer = (void*) null;
      }
      base.Dispose(disposing);
    }

    protected override void NativePointerUpdating()
    {
      if (!Configuration.EnableObjectTracking)
        return;
      ObjectTracker.UnTrack(this);
    }

    protected override void NativePointerUpdated(IntPtr oldNativePointer)
    {
      if (!Configuration.EnableObjectTracking)
        return;
      ObjectTracker.Track(this);
    }
  }
}
