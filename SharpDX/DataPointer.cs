// Type: SharpDX.DataPointer
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public struct DataPointer : IEquatable<DataPointer>
  {
    public static readonly DataPointer Zero = new DataPointer(IntPtr.Zero, 0);
    public IntPtr Pointer;
    public int Size;

    static DataPointer()
    {
    }

    public DataPointer(IntPtr pointer, int size)
    {
      this.Pointer = pointer;
      this.Size = size;
    }

    public unsafe DataPointer(void* pointer, int size)
    {
      this.Pointer = (IntPtr) pointer;
      this.Size = size;
    }

    public static bool operator ==(DataPointer left, DataPointer right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(DataPointer left, DataPointer right)
    {
      return !left.Equals(right);
    }

    public bool Equals(DataPointer other)
    {
      if (this.Pointer.Equals((object) other.Pointer))
        return this.Size == other.Size;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || !(obj is DataPointer))
        return false;
      else
        return this.Equals((DataPointer) obj);
    }

    public override int GetHashCode()
    {
      return this.Pointer.GetHashCode() * 397 ^ this.Size;
    }
  }
}
