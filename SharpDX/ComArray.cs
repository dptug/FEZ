// Type: SharpDX.ComArray
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Collections;

namespace SharpDX
{
  public class ComArray : DisposeBase, IEnumerable
  {
    protected ComObject[] values;
    private IntPtr nativeBuffer;

    public IntPtr NativePointer
    {
      get
      {
        return this.nativeBuffer;
      }
    }

    public int Length
    {
      get
      {
        if (this.values != null)
          return this.values.Length;
        else
          return 0;
      }
    }

    public ComArray(params ComObject[] array)
    {
      this.values = array;
      this.nativeBuffer = IntPtr.Zero;
      if (this.values == null)
        return;
      int length = array.Length;
      this.values = new ComObject[length];
      this.nativeBuffer = Utilities.AllocateMemory(length * Utilities.SizeOf<IntPtr>(), 16);
      for (int index = 0; index < length; ++index)
        this.Set(index, array[index]);
    }

    public ComArray(int size)
    {
      this.values = new ComObject[size];
      this.nativeBuffer = Utilities.AllocateMemory(size * Utilities.SizeOf<IntPtr>(), 16);
    }

    public ComObject Get(int index)
    {
      return this.values[index];
    }

    internal unsafe void SetFromNative(int index, ComObject value)
    {
      this.values[index] = value;
      value.NativePointer = ((IntPtr*) (void*) this.nativeBuffer)[index];
    }

    public unsafe void Set(int index, ComObject value)
    {
      this.values[index] = value;
      ((IntPtr*) (void*) this.nativeBuffer)[index] = value.NativePointer;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.values = (ComObject[]) null;
      Utilities.FreeMemory(this.nativeBuffer);
      this.nativeBuffer = IntPtr.Zero;
    }

    public IEnumerator GetEnumerator()
    {
      return this.values.GetEnumerator();
    }
  }
}
