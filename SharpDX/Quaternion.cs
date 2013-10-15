// Type: SharpDX.Quaternion
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Design;
using SharpDX.Serialization;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [TypeConverter(typeof (QuaternionConverter))]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Quaternion : IEquatable<Quaternion>, IFormattable, IDataSerializable
  {
    public static readonly int SizeInBytes = Marshal.SizeOf(typeof (Quaternion));
    public static readonly Quaternion Zero = new Quaternion();
    public static readonly Quaternion One = new Quaternion(1f, 1f, 1f, 1f);
    public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1f);
    public float X;
    public float Y;
    public float Z;
    public float W;

    public bool IsIdentity
    {
      get
      {
        return this.Equals(Quaternion.Identity);
      }
    }

    public bool IsNormalized
    {
      get
      {
        return (double) Math.Abs((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W - 1.0)) < 9.99999997475243E-07;
      }
    }

    public float Angle
    {
      get
      {
        if ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z < 9.99999997475243E-07)
          return 0.0f;
        else
          return (float) (2.0 * Math.Acos((double) this.W));
      }
    }

    public Vector3 Axis
    {
      get
      {
        float num1 = (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);
        if ((double) num1 < 9.99999997475243E-07)
          return Vector3.UnitX;
        float num2 = 1f / num1;
        return new Vector3(this.X * num2, this.Y * num2, this.Z * num2);
      }
    }

    public float this[int index]
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
            throw new ArgumentOutOfRangeException("index", "Indices for Quaternion run from 0 to 3, inclusive.");
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
            throw new ArgumentOutOfRangeException("index", "Indices for Quaternion run from 0 to 3, inclusive.");
        }
      }
    }

    static Quaternion()
    {
    }

    public Quaternion(float value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
      this.W = value;
    }

    public Quaternion(Vector4 value)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = value.Z;
      this.W = value.W;
    }

    public Quaternion(Vector3 value, float w)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = value.Z;
      this.W = w;
    }

    public Quaternion(Vector2 value, float z, float w)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = z;
      this.W = w;
    }

    public Quaternion(float x, float y, float z, float w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public Quaternion(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Quaternion.");
      this.X = values[0];
      this.Y = values[1];
      this.Z = values[2];
      this.W = values[3];
    }

    public static Quaternion operator +(Quaternion left, Quaternion right)
    {
      Quaternion result;
      Quaternion.Add(ref left, ref right, out result);
      return result;
    }

    public static Quaternion operator -(Quaternion left, Quaternion right)
    {
      Quaternion result;
      Quaternion.Subtract(ref left, ref right, out result);
      return result;
    }

    public static Quaternion operator -(Quaternion value)
    {
      Quaternion result;
      Quaternion.Negate(ref value, out result);
      return result;
    }

    public static Quaternion operator *(float scale, Quaternion value)
    {
      Quaternion result;
      Quaternion.Multiply(ref value, scale, out result);
      return result;
    }

    public static Quaternion operator *(Quaternion value, float scale)
    {
      Quaternion result;
      Quaternion.Multiply(ref value, scale, out result);
      return result;
    }

    public static Quaternion operator *(Quaternion left, Quaternion right)
    {
      Quaternion result;
      Quaternion.Multiply(ref left, ref right, out result);
      return result;
    }

    public static bool operator ==(Quaternion left, Quaternion right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Quaternion left, Quaternion right)
    {
      return !left.Equals(right);
    }

    public void Conjugate()
    {
      this.X = -this.X;
      this.Y = -this.Y;
      this.Z = -this.Z;
    }

    public void Invert()
    {
      float num1 = this.LengthSquared();
      if ((double) num1 <= 9.99999997475243E-07)
        return;
      float num2 = 1f / num1;
      this.X = -this.X * num2;
      this.Y = -this.Y * num2;
      this.Z = -this.Z * num2;
      this.W = this.W * num2;
    }

    public float Length()
    {
      return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
    }

    public float LengthSquared()
    {
      return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
    }

    public void Normalize()
    {
      float num1 = this.Length();
      if ((double) num1 <= 9.99999997475243E-07)
        return;
      float num2 = 1f / num1;
      this.X *= num2;
      this.Y *= num2;
      this.Z *= num2;
      this.W *= num2;
    }

    public float[] ToArray()
    {
      return new float[4]
      {
        this.X,
        this.Y,
        this.Z,
        this.W
      };
    }

    public static void Add(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
      result.X = left.X + right.X;
      result.Y = left.Y + right.Y;
      result.Z = left.Z + right.Z;
      result.W = left.W + right.W;
    }

    public static Quaternion Add(Quaternion left, Quaternion right)
    {
      Quaternion result;
      Quaternion.Add(ref left, ref right, out result);
      return result;
    }

    public static void Subtract(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
      result.X = left.X - right.X;
      result.Y = left.Y - right.Y;
      result.Z = left.Z - right.Z;
      result.W = left.W - right.W;
    }

    public static Quaternion Subtract(Quaternion left, Quaternion right)
    {
      Quaternion result;
      Quaternion.Subtract(ref left, ref right, out result);
      return result;
    }

    public static void Multiply(ref Quaternion value, float scale, out Quaternion result)
    {
      result.X = value.X * scale;
      result.Y = value.Y * scale;
      result.Z = value.Z * scale;
      result.W = value.W * scale;
    }

    public static Quaternion Multiply(Quaternion value, float scale)
    {
      Quaternion result;
      Quaternion.Multiply(ref value, scale, out result);
      return result;
    }

    public static void Multiply(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
      float num1 = left.X;
      float num2 = left.Y;
      float num3 = left.Z;
      float num4 = left.W;
      float num5 = right.X;
      float num6 = right.Y;
      float num7 = right.Z;
      float num8 = right.W;
      result.X = (float) ((double) num5 * (double) num4 + (double) num1 * (double) num8 + (double) num6 * (double) num3 - (double) num7 * (double) num2);
      result.Y = (float) ((double) num6 * (double) num4 + (double) num2 * (double) num8 + (double) num7 * (double) num1 - (double) num5 * (double) num3);
      result.Z = (float) ((double) num7 * (double) num4 + (double) num3 * (double) num8 + (double) num5 * (double) num2 - (double) num6 * (double) num1);
      result.W = (float) ((double) num8 * (double) num4 - ((double) num5 * (double) num1 + (double) num6 * (double) num2 + (double) num7 * (double) num3));
    }

    public static Quaternion Multiply(Quaternion left, Quaternion right)
    {
      Quaternion result;
      Quaternion.Multiply(ref left, ref right, out result);
      return result;
    }

    public static void Negate(ref Quaternion value, out Quaternion result)
    {
      result.X = -value.X;
      result.Y = -value.Y;
      result.Z = -value.Z;
      result.W = -value.W;
    }

    public static Quaternion Negate(Quaternion value)
    {
      Quaternion result;
      Quaternion.Negate(ref value, out result);
      return result;
    }

    public static void Barycentric(ref Quaternion value1, ref Quaternion value2, ref Quaternion value3, float amount1, float amount2, out Quaternion result)
    {
      Quaternion result1;
      Quaternion.Slerp(ref value1, ref value2, amount1 + amount2, out result1);
      Quaternion result2;
      Quaternion.Slerp(ref value1, ref value3, amount1 + amount2, out result2);
      Quaternion.Slerp(ref result1, ref result2, amount2 / (amount1 + amount2), out result);
    }

    public static Quaternion Barycentric(Quaternion value1, Quaternion value2, Quaternion value3, float amount1, float amount2)
    {
      Quaternion result;
      Quaternion.Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out result);
      return result;
    }

    public static void Conjugate(ref Quaternion value, out Quaternion result)
    {
      result.X = -value.X;
      result.Y = -value.Y;
      result.Z = -value.Z;
      result.W = value.W;
    }

    public static Quaternion Conjugate(Quaternion value)
    {
      Quaternion result;
      Quaternion.Conjugate(ref value, out result);
      return result;
    }

    public static void Dot(ref Quaternion left, ref Quaternion right, out float result)
    {
      result = (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z + (double) left.W * (double) right.W);
    }

    public static float Dot(Quaternion left, Quaternion right)
    {
      return (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z + (double) left.W * (double) right.W);
    }

    public static void Exponential(ref Quaternion value, out Quaternion result)
    {
      float num1 = (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y + (double) value.Z * (double) value.Z);
      float num2 = (float) Math.Sin((double) num1);
      if ((double) Math.Abs(num2) >= 9.99999997475243E-07)
      {
        float num3 = num2 / num1;
        result.X = num3 * value.X;
        result.Y = num3 * value.Y;
        result.Z = num3 * value.Z;
      }
      else
        result = value;
      result.W = (float) Math.Cos((double) num1);
    }

    public static Quaternion Exponential(Quaternion value)
    {
      Quaternion result;
      Quaternion.Exponential(ref value, out result);
      return result;
    }

    public static void Invert(ref Quaternion value, out Quaternion result)
    {
      result = value;
      result.Invert();
    }

    public static Quaternion Invert(Quaternion value)
    {
      Quaternion result;
      Quaternion.Invert(ref value, out result);
      return result;
    }

    public static void Lerp(ref Quaternion start, ref Quaternion end, float amount, out Quaternion result)
    {
      float num = 1f - amount;
      if ((double) Quaternion.Dot(start, end) >= 0.0)
      {
        result.X = (float) ((double) num * (double) start.X + (double) amount * (double) end.X);
        result.Y = (float) ((double) num * (double) start.Y + (double) amount * (double) end.Y);
        result.Z = (float) ((double) num * (double) start.Z + (double) amount * (double) end.Z);
        result.W = (float) ((double) num * (double) start.W + (double) amount * (double) end.W);
      }
      else
      {
        result.X = (float) ((double) num * (double) start.X - (double) amount * (double) end.X);
        result.Y = (float) ((double) num * (double) start.Y - (double) amount * (double) end.Y);
        result.Z = (float) ((double) num * (double) start.Z - (double) amount * (double) end.Z);
        result.W = (float) ((double) num * (double) start.W - (double) amount * (double) end.W);
      }
      result.Normalize();
    }

    public static Quaternion Lerp(Quaternion start, Quaternion end, float amount)
    {
      Quaternion result;
      Quaternion.Lerp(ref start, ref end, amount, out result);
      return result;
    }

    public static void Logarithm(ref Quaternion value, out Quaternion result)
    {
      if ((double) Math.Abs(value.W) < 1.0)
      {
        float num1 = (float) Math.Acos((double) value.W);
        float num2 = (float) Math.Sin((double) num1);
        if ((double) Math.Abs(num2) >= 9.99999997475243E-07)
        {
          float num3 = num1 / num2;
          result.X = value.X * num3;
          result.Y = value.Y * num3;
          result.Z = value.Z * num3;
        }
        else
          result = value;
      }
      else
        result = value;
      result.W = 0.0f;
    }

    public static Quaternion Logarithm(Quaternion value)
    {
      Quaternion result;
      Quaternion.Logarithm(ref value, out result);
      return result;
    }

    public static void Normalize(ref Quaternion value, out Quaternion result)
    {
      Quaternion quaternion = value;
      result = quaternion;
      result.Normalize();
    }

    public static Quaternion Normalize(Quaternion value)
    {
      value.Normalize();
      return value;
    }

    public static void RotationAxis(ref Vector3 axis, float angle, out Quaternion result)
    {
      Vector3 result1;
      Vector3.Normalize(ref axis, out result1);
      float num1 = angle * 0.5f;
      float num2 = (float) Math.Sin((double) num1);
      float num3 = (float) Math.Cos((double) num1);
      result.X = result1.X * num2;
      result.Y = result1.Y * num2;
      result.Z = result1.Z * num2;
      result.W = num3;
    }

    public static Quaternion RotationAxis(Vector3 axis, float angle)
    {
      Quaternion result;
      Quaternion.RotationAxis(ref axis, angle, out result);
      return result;
    }

    public static void RotationMatrix(ref Matrix matrix, out Quaternion result)
    {
      float num1 = matrix.M11 + matrix.M22 + matrix.M33;
      if ((double) num1 > 0.0)
      {
        float num2 = (float) Math.Sqrt((double) num1 + 1.0);
        result.W = num2 * 0.5f;
        float num3 = 0.5f / num2;
        result.X = (matrix.M23 - matrix.M32) * num3;
        result.Y = (matrix.M31 - matrix.M13) * num3;
        result.Z = (matrix.M12 - matrix.M21) * num3;
      }
      else if ((double) matrix.M11 >= (double) matrix.M22 && (double) matrix.M11 >= (double) matrix.M33)
      {
        float num2 = (float) Math.Sqrt(1.0 + (double) matrix.M11 - (double) matrix.M22 - (double) matrix.M33);
        float num3 = 0.5f / num2;
        result.X = 0.5f * num2;
        result.Y = (matrix.M12 + matrix.M21) * num3;
        result.Z = (matrix.M13 + matrix.M31) * num3;
        result.W = (matrix.M23 - matrix.M32) * num3;
      }
      else if ((double) matrix.M22 > (double) matrix.M33)
      {
        float num2 = (float) Math.Sqrt(1.0 + (double) matrix.M22 - (double) matrix.M11 - (double) matrix.M33);
        float num3 = 0.5f / num2;
        result.X = (matrix.M21 + matrix.M12) * num3;
        result.Y = 0.5f * num2;
        result.Z = (matrix.M32 + matrix.M23) * num3;
        result.W = (matrix.M31 - matrix.M13) * num3;
      }
      else
      {
        float num2 = (float) Math.Sqrt(1.0 + (double) matrix.M33 - (double) matrix.M11 - (double) matrix.M22);
        float num3 = 0.5f / num2;
        result.X = (matrix.M31 + matrix.M13) * num3;
        result.Y = (matrix.M32 + matrix.M23) * num3;
        result.Z = 0.5f * num2;
        result.W = (matrix.M12 - matrix.M21) * num3;
      }
    }

    public static Quaternion RotationMatrix(Matrix matrix)
    {
      Quaternion result;
      Quaternion.RotationMatrix(ref matrix, out result);
      return result;
    }

    public static void RotationYawPitchRoll(float yaw, float pitch, float roll, out Quaternion result)
    {
      float num1 = roll * 0.5f;
      float num2 = pitch * 0.5f;
      float num3 = yaw * 0.5f;
      float num4 = (float) Math.Sin((double) num1);
      float num5 = (float) Math.Cos((double) num1);
      float num6 = (float) Math.Sin((double) num2);
      float num7 = (float) Math.Cos((double) num2);
      float num8 = (float) Math.Sin((double) num3);
      float num9 = (float) Math.Cos((double) num3);
      result.X = (float) ((double) num9 * (double) num6 * (double) num5 + (double) num8 * (double) num7 * (double) num4);
      result.Y = (float) ((double) num8 * (double) num7 * (double) num5 - (double) num9 * (double) num6 * (double) num4);
      result.Z = (float) ((double) num9 * (double) num7 * (double) num4 - (double) num8 * (double) num6 * (double) num5);
      result.W = (float) ((double) num9 * (double) num7 * (double) num5 + (double) num8 * (double) num6 * (double) num4);
    }

    public static Quaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
    {
      Quaternion result;
      Quaternion.RotationYawPitchRoll(yaw, pitch, roll, out result);
      return result;
    }

    public static void Slerp(ref Quaternion start, ref Quaternion end, float amount, out Quaternion result)
    {
      float num1 = Quaternion.Dot(start, end);
      float num2;
      float num3;
      if ((double) Math.Abs(num1) > 0.999998986721039)
      {
        num2 = 1f - amount;
        num3 = amount * (float) Math.Sign(num1);
      }
      else
      {
        float num4 = (float) Math.Acos((double) Math.Abs(num1));
        float num5 = (float) (1.0 / Math.Sin((double) num4));
        num2 = (float) Math.Sin((1.0 - (double) amount) * (double) num4) * num5;
        num3 = (float) Math.Sin((double) amount * (double) num4) * num5 * (float) Math.Sign(num1);
      }
      result.X = (float) ((double) num2 * (double) start.X + (double) num3 * (double) end.X);
      result.Y = (float) ((double) num2 * (double) start.Y + (double) num3 * (double) end.Y);
      result.Z = (float) ((double) num2 * (double) start.Z + (double) num3 * (double) end.Z);
      result.W = (float) ((double) num2 * (double) start.W + (double) num3 * (double) end.W);
    }

    public static Quaternion Slerp(Quaternion start, Quaternion end, float amount)
    {
      Quaternion result;
      Quaternion.Slerp(ref start, ref end, amount, out result);
      return result;
    }

    public static void Squad(ref Quaternion value1, ref Quaternion value2, ref Quaternion value3, ref Quaternion value4, float amount, out Quaternion result)
    {
      Quaternion result1;
      Quaternion.Slerp(ref value1, ref value4, amount, out result1);
      Quaternion result2;
      Quaternion.Slerp(ref value2, ref value3, amount, out result2);
      Quaternion.Slerp(ref result1, ref result2, (float) (2.0 * (double) amount * (1.0 - (double) amount)), out result);
    }

    public static Quaternion Squad(Quaternion value1, Quaternion value2, Quaternion value3, Quaternion value4, float amount)
    {
      Quaternion result;
      Quaternion.Squad(ref value1, ref value2, ref value3, ref value4, amount, out result);
      return result;
    }

    public static Quaternion[] SquadSetup(Quaternion value1, Quaternion value2, Quaternion value3, Quaternion value4)
    {
      Quaternion quaternion1 = (double) (value1 + value2).LengthSquared() < (double) (value1 - value2).LengthSquared() ? -value1 : value1;
      Quaternion quaternion2 = (double) (value2 + value3).LengthSquared() < (double) (value2 - value3).LengthSquared() ? -value3 : value3;
      Quaternion quaternion3 = (double) (value3 + value4).LengthSquared() < (double) (value3 - value4).LengthSquared() ? -value4 : value4;
      Quaternion quaternion4 = value2;
      Quaternion result1;
      Quaternion.Exponential(ref quaternion4, out result1);
      Quaternion result2;
      Quaternion.Exponential(ref quaternion2, out result2);
      return new Quaternion[3]
      {
        quaternion4 * Quaternion.Exponential(-0.25f * (Quaternion.Logarithm(result1 * quaternion2) + Quaternion.Logarithm(result1 * quaternion1))),
        quaternion2 * Quaternion.Exponential(-0.25f * (Quaternion.Logarithm(result2 * quaternion3) + Quaternion.Logarithm(result2 * quaternion4))),
        quaternion2
      };
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
        return this.ToString(formatProvider);
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
        this.X = serializer.Reader.ReadSingle();
        this.Y = serializer.Reader.ReadSingle();
        this.Z = serializer.Reader.ReadSingle();
        this.W = serializer.Reader.ReadSingle();
      }
    }

    public bool Equals(Quaternion other)
    {
      if ((double) Math.Abs(other.X - this.X) < 9.99999997475243E-07 && (double) Math.Abs(other.Y - this.Y) < 9.99999997475243E-07 && (double) Math.Abs(other.Z - this.Z) < 9.99999997475243E-07)
        return (double) Math.Abs(other.W - this.W) < 9.99999997475243E-07;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Quaternion)))
        return false;
      else
        return this.Equals((Quaternion) value);
    }
  }
}
