// Type: SharpDX.Int3
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [DynamicSerializer("TKI3")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Int3 : IEquatable<Int3>, IFormattable, IDataSerializable
  {
    public static readonly int SizeInBytes = Marshal.SizeOf(typeof (Int3));
    public static readonly Int3 Zero = new Int3();
    public static readonly Int3 UnitX = new Int3(1, 0, 0);
    public static readonly Int3 UnitY = new Int3(0, 1, 0);
    public static readonly Int3 UnitZ = new Int3(0, 0, 1);
    public static readonly Int3 One = new Int3(1, 1, 1);
    public int X;
    public int Y;
    public int Z;

    public int this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.X;
          case 1:
            return this.Y;
          case 2:
            return this.Z;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Int3 run from 0 to 2, inclusive.");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.X = value;
            break;
          case 1:
            this.Y = value;
            break;
          case 2:
            this.Z = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Int3 run from 0 to 2, inclusive.");
        }
      }
    }

    static Int3()
    {
    }

    public Int3(int value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
    }

    public Int3(int x, int y, int z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Int3(int[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 3)
        throw new ArgumentOutOfRangeException("values", "There must be three and only three input values for Int3.");
      this.X = values[0];
      this.Y = values[1];
      this.Z = values[2];
    }

    public static explicit operator Vector2(Int3 value)
    {
      return new Vector2((float) value.X, (float) value.Y);
    }

    public static explicit operator Vector3(Int3 value)
    {
      return new Vector3((float) value.X, (float) value.Y, (float) value.Z);
    }

    public static implicit operator Int3(int[] input)
    {
      return new Int3(input);
    }

    public static implicit operator int[](Int3 input)
    {
      return input.ToArray();
    }

    public static Int3 operator +(Int3 left, Int3 right)
    {
      return new Int3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Int3 operator +(Int3 value)
    {
      return value;
    }

    public static Int3 operator -(Int3 left, Int3 right)
    {
      return new Int3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static Int3 operator -(Int3 value)
    {
      return new Int3(-value.X, -value.Y, -value.Z);
    }

    public static Int3 operator *(int scale, Int3 value)
    {
      return new Int3(value.X * scale, value.Y * scale, value.Z * scale);
    }

    public static Int3 operator *(Int3 value, int scale)
    {
      return new Int3(value.X * scale, value.Y * scale, value.Z * scale);
    }

    public static Int3 operator /(Int3 value, int scale)
    {
      return new Int3(value.X / scale, value.Y / scale, value.Z / scale);
    }

    public static bool operator ==(Int3 left, Int3 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Int3 left, Int3 right)
    {
      return !left.Equals(right);
    }

    public int[] ToArray()
    {
      return new int[3]
      {
        this.X,
        this.Y,
        this.Z
      };
    }

    public static void Add(ref Int3 left, ref Int3 right, out Int3 result)
    {
      result = new Int3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Int3 Add(Int3 left, Int3 right)
    {
      return new Int3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static void Subtract(ref Int3 left, ref Int3 right, out Int3 result)
    {
      result = new Int3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static Int3 Subtract(Int3 left, Int3 right)
    {
      return new Int3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static void Multiply(ref Int3 value, int scale, out Int3 result)
    {
      result = new Int3(value.X * scale, value.Y * scale, value.Z * scale);
    }

    public static Int3 Multiply(Int3 value, int scale)
    {
      return new Int3(value.X * scale, value.Y * scale, value.Z * scale);
    }

    public static void Modulate(ref Int3 left, ref Int3 right, out Int3 result)
    {
      result = new Int3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static Int3 Modulate(Int3 left, Int3 right)
    {
      return new Int3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static void Divide(ref Int3 value, int scale, out Int3 result)
    {
      result = new Int3(value.X / scale, value.Y / scale, value.Z / scale);
    }

    public static Int3 Divide(Int3 value, int scale)
    {
      return new Int3(value.X / scale, value.Y / scale, value.Z / scale);
    }

    public static void Negate(ref Int3 value, out Int3 result)
    {
      result = new Int3(-value.X, -value.Y, -value.Z);
    }

    public static Int3 Negate(Int3 value)
    {
      return new Int3(-value.X, -value.Y, -value.Z);
    }

    public static void Clamp(ref Int3 value, ref Int3 min, ref Int3 max, out Int3 result)
    {
      int num1 = value.X;
      int num2 = num1 > max.X ? max.X : num1;
      int x = num2 < min.X ? min.X : num2;
      int num3 = value.Y;
      int num4 = num3 > max.Y ? max.Y : num3;
      int y = num4 < min.Y ? min.Y : num4;
      int num5 = value.Z;
      int num6 = num5 > max.Z ? max.Z : num5;
      int z = num6 < min.Z ? min.Z : num6;
      result = new Int3(x, y, z);
    }

    public static Int3 Clamp(Int3 value, Int3 min, Int3 max)
    {
      Int3 result;
      Int3.Clamp(ref value, ref min, ref max, out result);
      return result;
    }

    public static void Max(ref Int3 left, ref Int3 right, out Int3 result)
    {
      result.X = left.X > right.X ? left.X : right.X;
      result.Y = left.Y > right.Y ? left.Y : right.Y;
      result.Z = left.Z > right.Z ? left.Z : right.Z;
    }

    public static Int3 Max(Int3 left, Int3 right)
    {
      Int3 result;
      Int3.Max(ref left, ref right, out result);
      return result;
    }

    public static void Min(ref Int3 left, ref Int3 right, out Int3 result)
    {
      result.X = left.X < right.X ? left.X : right.X;
      result.Y = left.Y < right.Y ? left.Y : right.Y;
      result.Z = left.Z < right.Z ? left.Z : right.Z;
    }

    public static Int3 Min(Int3 left, Int3 right)
    {
      Int3 result;
      Int3.Min(ref left, ref right, out result);
      return result;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", (object) this.X, (object) this.Y, (object) this.Z);
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", (object) this.X.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Y.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Z.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture));
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", (object) this.X, (object) this.Y, (object) this.Z);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        this.ToString(formatProvider);
      return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", (object) this.X.ToString(format, formatProvider), (object) this.Y.ToString(format, formatProvider), (object) this.Z.ToString(format, formatProvider));
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode();
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.X);
        serializer.Writer.Write(this.Y);
        serializer.Writer.Write(this.Z);
      }
      else
      {
        this.X = serializer.Reader.ReadInt32();
        this.Y = serializer.Reader.ReadInt32();
        this.Z = serializer.Reader.ReadInt32();
      }
    }

    public bool Equals(Int3 other)
    {
      if (other.X == this.X && other.Y == this.Y)
        return other.Z == this.Z;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Int3)))
        return false;
      else
        return this.Equals((Int3) value);
    }
  }
}
