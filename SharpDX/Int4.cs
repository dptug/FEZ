// Type: SharpDX.Int4
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [DynamicSerializer("TKI4")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Int4 : IEquatable<Int4>, IFormattable, IDataSerializable
  {
    public static readonly int SizeInBytes = Marshal.SizeOf(typeof (Int4));
    public static readonly Int4 Zero = new Int4();
    public static readonly Int4 UnitX = new Int4(1, 0, 0, 0);
    public static readonly Int4 UnitY = new Int4(0, 1, 0, 0);
    public static readonly Int4 UnitZ = new Int4(0, 0, 1, 0);
    public static readonly Int4 UnitW = new Int4(0, 0, 0, 1);
    public static readonly Int4 One = new Int4(1, 1, 1, 1);
    public int X;
    public int Y;
    public int Z;
    public int W;

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
          case 3:
            return this.W;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Int4 run from 0 to 3, inclusive.");
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
          case 3:
            this.W = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Int4 run from 0 to 3, inclusive.");
        }
      }
    }

    static Int4()
    {
    }

    public Int4(int value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
      this.W = value;
    }

    public Int4(int x, int y, int z, int w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public Int4(int[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Int4.");
      this.X = values[0];
      this.Y = values[1];
      this.Z = values[2];
      this.W = values[3];
    }

    public static explicit operator Vector2(Int4 value)
    {
      return new Vector2((float) value.X, (float) value.Y);
    }

    public static explicit operator Vector3(Int4 value)
    {
      return new Vector3((float) value.X, (float) value.Y, (float) value.Z);
    }

    public static explicit operator Vector4(Int4 value)
    {
      return new Vector4((float) value.X, (float) value.Y, (float) value.Z, (float) value.W);
    }

    public static implicit operator Int4(int[] input)
    {
      return new Int4(input);
    }

    public static implicit operator int[](Int4 input)
    {
      return input.ToArray();
    }

    public static Int4 operator +(Int4 left, Int4 right)
    {
      return new Int4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    public static Int4 operator +(Int4 value)
    {
      return value;
    }

    public static Int4 operator -(Int4 left, Int4 right)
    {
      return new Int4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }

    public static Int4 operator -(Int4 value)
    {
      return new Int4(-value.X, -value.Y, -value.Z, -value.W);
    }

    public static Int4 operator *(int scale, Int4 value)
    {
      return new Int4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
    }

    public static Int4 operator *(Int4 value, int scale)
    {
      return new Int4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
    }

    public static Int4 operator /(Int4 value, int scale)
    {
      return new Int4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
    }

    public static bool operator ==(Int4 left, Int4 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Int4 left, Int4 right)
    {
      return !left.Equals(right);
    }

    public int[] ToArray()
    {
      return new int[4]
      {
        this.X,
        this.Y,
        this.Z,
        this.W
      };
    }

    public static void Add(ref Int4 left, ref Int4 right, out Int4 result)
    {
      result = new Int4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    public static Int4 Add(Int4 left, Int4 right)
    {
      return new Int4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    public static void Subtract(ref Int4 left, ref Int4 right, out Int4 result)
    {
      result = new Int4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }

    public static Int4 Subtract(Int4 left, Int4 right)
    {
      return new Int4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }

    public static void Multiply(ref Int4 value, int scale, out Int4 result)
    {
      result = new Int4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
    }

    public static Int4 Multiply(Int4 value, int scale)
    {
      return new Int4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
    }

    public static void Modulate(ref Int4 left, ref Int4 right, out Int4 result)
    {
      result = new Int4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }

    public static Int4 Modulate(Int4 left, Int4 right)
    {
      return new Int4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }

    public static void Divide(ref Int4 value, int scale, out Int4 result)
    {
      result = new Int4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
    }

    public static Int4 Divide(Int4 value, int scale)
    {
      return new Int4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
    }

    public static void Negate(ref Int4 value, out Int4 result)
    {
      result = new Int4(-value.X, -value.Y, -value.Z, -value.W);
    }

    public static Int4 Negate(Int4 value)
    {
      return new Int4(-value.X, -value.Y, -value.Z, -value.W);
    }

    public static void Clamp(ref Int4 value, ref Int4 min, ref Int4 max, out Int4 result)
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
      int num7 = value.W;
      int num8 = num7 > max.W ? max.W : num7;
      int w = num8 < min.W ? min.W : num8;
      result = new Int4(x, y, z, w);
    }

    public static Int4 Clamp(Int4 value, Int4 min, Int4 max)
    {
      Int4 result;
      Int4.Clamp(ref value, ref min, ref max, out result);
      return result;
    }

    public static void Max(ref Int4 left, ref Int4 right, out Int4 result)
    {
      result.X = left.X > right.X ? left.X : right.X;
      result.Y = left.Y > right.Y ? left.Y : right.Y;
      result.Z = left.Z > right.Z ? left.Z : right.Z;
      result.W = left.W > right.W ? left.W : right.W;
    }

    public static Int4 Max(Int4 left, Int4 right)
    {
      Int4 result;
      Int4.Max(ref left, ref right, out result);
      return result;
    }

    public static void Min(ref Int4 left, ref Int4 right, out Int4 result)
    {
      result.X = left.X < right.X ? left.X : right.X;
      result.Y = left.Y < right.Y ? left.Y : right.Y;
      result.Z = left.Z < right.Z ? left.Z : right.Z;
      result.W = left.W < right.W ? left.W : right.W;
    }

    public static Int4 Min(Int4 left, Int4 right)
    {
      Int4 result;
      Int4.Min(ref left, ref right, out result);
      return result;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", (object) this.X, (object) this.Y, (object) this.Z, (object) this.W);
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", (object) this.X.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Y.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Z.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.W.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture));
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", (object) this.X, (object) this.Y, (object) this.Z, (object) this.W);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        this.ToString(formatProvider);
      return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", (object) this.X.ToString(format, formatProvider), (object) this.Y.ToString(format, formatProvider), (object) this.Z.ToString(format, formatProvider), (object) this.W.ToString(format, formatProvider));
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode() + this.W.GetHashCode();
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.X);
        serializer.Writer.Write(this.Y);
        serializer.Writer.Write(this.Z);
        serializer.Writer.Write(this.W);
      }
      else
      {
        this.X = serializer.Reader.ReadInt32();
        this.Y = serializer.Reader.ReadInt32();
        this.Z = serializer.Reader.ReadInt32();
        this.W = serializer.Reader.ReadInt32();
      }
    }

    public bool Equals(Int4 other)
    {
      if (other.X == this.X && other.Y == this.Y && other.Z == this.Z)
        return other.W == this.W;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Int4)))
        return false;
      else
        return this.Equals((Int4) value);
    }
  }
}
