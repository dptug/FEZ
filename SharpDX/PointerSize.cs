// Type: SharpDX.PointerSize
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Globalization;

namespace SharpDX
{
  public struct PointerSize
  {
    private IntPtr _size;

    public PointerSize(IntPtr size)
    {
      this._size = size;
    }

    private unsafe PointerSize(void* size)
    {
      this._size = new IntPtr(size);
    }

    public PointerSize(int size)
    {
      this._size = new IntPtr(size);
    }

    public PointerSize(long size)
    {
      this._size = new IntPtr(size);
    }

    public static implicit operator int(PointerSize value)
    {
      return value._size.ToInt32();
    }

    public static implicit operator long(PointerSize value)
    {
      return value._size.ToInt64();
    }

    public static implicit operator PointerSize(int value)
    {
      return new PointerSize(value);
    }

    public static implicit operator PointerSize(long value)
    {
      return new PointerSize(value);
    }

    public static implicit operator PointerSize(IntPtr value)
    {
      return new PointerSize(value);
    }

    public static implicit operator IntPtr(PointerSize value)
    {
      return value._size;
    }

    public static unsafe implicit operator PointerSize(void* value)
    {
      return new PointerSize(value);
    }

    public static unsafe implicit operator void*(PointerSize value)
    {
      return (void*) value._size;
    }

    public static PointerSize operator +(PointerSize left, PointerSize right)
    {
      return new PointerSize(left._size.ToInt64() + right._size.ToInt64());
    }

    public static PointerSize operator +(PointerSize value)
    {
      return value;
    }

    public static PointerSize operator -(PointerSize left, PointerSize right)
    {
      return new PointerSize(left._size.ToInt64() - right._size.ToInt64());
    }

    public static PointerSize operator -(PointerSize value)
    {
      return new PointerSize(-value._size.ToInt64());
    }

    public static PointerSize operator *(int scale, PointerSize value)
    {
      return new PointerSize((long) scale * value._size.ToInt64());
    }

    public static PointerSize operator *(PointerSize value, int scale)
    {
      return new PointerSize((long) scale * value._size.ToInt64());
    }

    public static PointerSize operator /(PointerSize value, int scale)
    {
      return new PointerSize(value._size.ToInt64() / (long) scale);
    }

    public static bool operator ==(PointerSize left, PointerSize right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(PointerSize left, PointerSize right)
    {
      return !left.Equals(right);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}", new object[1]
      {
        (object) this._size
      });
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}", new object[1]
      {
        (object) this._size.ToString(format)
      });
    }

    public override int GetHashCode()
    {
      return this._size.ToInt32();
    }

    public bool Equals(PointerSize other)
    {
      return this._size == other._size;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (PointerSize)))
        return false;
      else
        return this.Equals((PointerSize) value);
    }
  }
}
