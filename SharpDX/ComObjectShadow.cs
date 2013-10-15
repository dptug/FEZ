// Type: SharpDX.ComObjectShadow
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SharpDX
{
  internal abstract class ComObjectShadow : CppObjectShadow
  {
    public static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");
    private int count = 1;

    static ComObjectShadow()
    {
    }

    protected int QueryInterfaceImpl(IntPtr thisObject, ref Guid guid, out IntPtr output)
    {
      ComObjectShadow comObjectShadow = (ComObjectShadow) ((ShadowContainer) this.Callback.Shadow).FindShadow(guid);
      if (comObjectShadow != null)
      {
        comObjectShadow.AddRefImpl(thisObject);
        output = comObjectShadow.NativePointer;
        return Result.Ok.Code;
      }
      else
      {
        output = IntPtr.Zero;
        return Result.NoInterface.Code;
      }
    }

    protected virtual int AddRefImpl(IntPtr thisObject)
    {
      return Interlocked.Increment(ref this.count);
    }

    protected virtual int ReleaseImpl(IntPtr thisObject)
    {
      return Interlocked.Decrement(ref this.count);
    }

    internal class ComObjectVtbl : CppObjectVtbl
    {
      public ComObjectVtbl(int numberOfCallbackMethods)
        : base(numberOfCallbackMethods + 3)
      {
        this.AddMethod((Delegate) new ComObjectShadow.ComObjectVtbl.QueryInterfaceDelegate(ComObjectShadow.ComObjectVtbl.QueryInterfaceImpl));
        this.AddMethod((Delegate) new ComObjectShadow.ComObjectVtbl.AddRefDelegate(ComObjectShadow.ComObjectVtbl.AddRefImpl));
        this.AddMethod((Delegate) new ComObjectShadow.ComObjectVtbl.ReleaseDelegate(ComObjectShadow.ComObjectVtbl.ReleaseImpl));
      }

      protected static unsafe int QueryInterfaceImpl(IntPtr thisObject, IntPtr guid, out IntPtr output)
      {
        ComObjectShadow comObjectShadow = CppObjectShadow.ToShadow<ComObjectShadow>(thisObject);
        if (comObjectShadow != null)
          return comObjectShadow.QueryInterfaceImpl(thisObject, ref *(Guid*) (void*) guid, out output);
        output = IntPtr.Zero;
        return Result.NoInterface.Code;
      }

      protected static int AddRefImpl(IntPtr thisObject)
      {
        ComObjectShadow comObjectShadow = CppObjectShadow.ToShadow<ComObjectShadow>(thisObject);
        if (comObjectShadow == null)
          return 0;
        else
          return comObjectShadow.AddRefImpl(thisObject);
      }

      protected static int ReleaseImpl(IntPtr thisObject)
      {
        ComObjectShadow comObjectShadow = CppObjectShadow.ToShadow<ComObjectShadow>(thisObject);
        if (comObjectShadow == null)
          return 0;
        else
          return comObjectShadow.ReleaseImpl(thisObject);
      }

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      public delegate int QueryInterfaceDelegate(IntPtr thisObject, IntPtr guid, out IntPtr output);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      public delegate int AddRefDelegate(IntPtr thisObject);

      [UnmanagedFunctionPointer(CallingConvention.StdCall)]
      public delegate int ReleaseDelegate(IntPtr thisObject);
    }
  }
}
