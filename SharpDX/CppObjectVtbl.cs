// Type: SharpDX.CppObjectVtbl
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX
{
  internal class CppObjectVtbl
  {
    private readonly List<Delegate> methods;
    private readonly IntPtr vtbl;

    public IntPtr Pointer
    {
      get
      {
        return this.vtbl;
      }
    }

    public CppObjectVtbl(int numberOfCallbackMethods)
    {
      this.vtbl = Marshal.AllocHGlobal(IntPtr.Size * numberOfCallbackMethods);
      this.methods = new List<Delegate>();
    }

    public unsafe void AddMethod(Delegate method)
    {
      int count = this.methods.Count;
      this.methods.Add(method);
      ((IntPtr*) (void*) this.vtbl)[count] = Marshal.GetFunctionPointerForDelegate(method);
    }
  }
}
