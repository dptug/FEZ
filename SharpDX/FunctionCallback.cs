// Type: SharpDX.FunctionCallback
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [StructLayout(LayoutKind.Sequential)]
  internal class FunctionCallback
  {
    public IntPtr Pointer;

    public FunctionCallback(IntPtr pointer)
    {
      this.Pointer = pointer;
    }

    public unsafe FunctionCallback(void* pointer)
    {
      this.Pointer = new IntPtr(pointer);
    }

    public static explicit operator IntPtr(FunctionCallback value)
    {
      return value.Pointer;
    }

    public static implicit operator FunctionCallback(IntPtr value)
    {
      return new FunctionCallback(value);
    }

    public static unsafe implicit operator void*(FunctionCallback value)
    {
      return (void*) value.Pointer;
    }

    public static unsafe explicit operator FunctionCallback(void* value)
    {
      return new FunctionCallback(value);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}", new object[1]
      {
        (object) this.Pointer
      });
    }

    public string ToString(string format)
    {
      if (format == null)
        return base.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}", new object[1]
      {
        (object) this.Pointer.ToString(format)
      });
    }

    public override int GetHashCode()
    {
      return this.Pointer.ToInt32();
    }

    public bool Equals(FunctionCallback other)
    {
      return this.Pointer == other.Pointer;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (FunctionCallback)))
        return false;
      else
        return this.Equals((FunctionCallback) value);
    }
  }
}
