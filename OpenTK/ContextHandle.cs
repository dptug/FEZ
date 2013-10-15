// Type: OpenTK.ContextHandle
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  public struct ContextHandle : IComparable<ContextHandle>, IEquatable<ContextHandle>
  {
    public static readonly ContextHandle Zero = new ContextHandle(IntPtr.Zero);
    private IntPtr handle;

    public IntPtr Handle
    {
      get
      {
        return this.handle;
      }
    }

    static ContextHandle()
    {
    }

    public ContextHandle(IntPtr h)
    {
      this.handle = h;
    }

    public static explicit operator IntPtr(ContextHandle c)
    {
      if (!(c != ContextHandle.Zero))
        return IntPtr.Zero;
      else
        return c.handle;
    }

    public static explicit operator ContextHandle(IntPtr p)
    {
      return new ContextHandle(p);
    }

    public static bool operator ==(ContextHandle left, ContextHandle right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(ContextHandle left, ContextHandle right)
    {
      return !left.Equals(right);
    }

    public override string ToString()
    {
      return this.Handle.ToString();
    }

    public override bool Equals(object obj)
    {
      if (obj is ContextHandle)
        return this.Equals((ContextHandle) obj);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Handle.GetHashCode();
    }

    public unsafe int CompareTo(ContextHandle other)
    {
      return (int) ((int*) other.handle.ToPointer() - (int*) this.handle.ToPointer());
    }

    public bool Equals(ContextHandle other)
    {
      return this.Handle == other.Handle;
    }
  }
}
