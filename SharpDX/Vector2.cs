// Type: SharpDX.Vector2
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
  [DynamicSerializer("TKV2")]
  [TypeConverter(typeof (Vector2Converter))]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Vector2 : IEquatable<Vector2>, IFormattable, IDataSerializable
  {
    public static readonly int SizeInBytes = Marshal.SizeOf(typeof (Vector2));
    public static readonly Vector2 Zero = new Vector2();
    public static readonly Vector2 UnitX = new Vector2(1f, 0.0f);
    public static readonly Vector2 UnitY = new Vector2(0.0f, 1f);
    public static readonly Vector2 One = new Vector2(1f, 1f);
    public float X;
    public float Y;

    public bool IsNormalized
    {
      get
      {
        return (double) Math.Abs((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y - 1.0)) < 9.99999997475243E-07;
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
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
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
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
        }
      }
    }

    static Vector2()
    {
    }

    public Vector2(float value)
    {
      this.X = value;
      this.Y = value;
    }

    public Vector2(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }

    public Vector2(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 2)
        throw new ArgumentOutOfRangeException("values", "There must be two and only two input values for Vector2.");
      this.X = values[0];
      this.Y = values[1];
    }

    public static explicit operator Vector3(Vector2 value)
    {
      return new Vector3(value, 0.0f);
    }

    public static explicit operator Vector4(Vector2 value)
    {
      return new Vector4(value, 0.0f, 0.0f);
    }

    public static Vector2 operator +(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X + right.X, left.Y + right.Y);
    }

    public static Vector2 operator *(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X * right.X, left.Y * right.Y);
    }

    public static Vector2 operator +(Vector2 value)
    {
      return value;
    }

    public static Vector2 operator -(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X - right.X, left.Y - right.Y);
    }

    public static Vector2 operator -(Vector2 value)
    {
      return new Vector2(-value.X, -value.Y);
    }

    public static Vector2 operator *(float scale, Vector2 value)
    {
      return new Vector2(value.X * scale, value.Y * scale);
    }

    public static Vector2 operator *(Vector2 value, float scale)
    {
      return new Vector2(value.X * scale, value.Y * scale);
    }

    public static Vector2 operator /(Vector2 value, float scale)
    {
      return new Vector2(value.X / scale, value.Y / scale);
    }

    public static bool operator ==(Vector2 left, Vector2 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Vector2 left, Vector2 right)
    {
      return !left.Equals(right);
    }

    public float Length()
    {
      return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
    }

    public float LengthSquared()
    {
      return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
    }

    public void Normalize()
    {
      float num1 = this.Length();
      if ((double) num1 <= 9.99999997475243E-07)
        return;
      float num2 = 1f / num1;
      this.X *= num2;
      this.Y *= num2;
    }

    public float[] ToArray()
    {
      return new float[2]
      {
        this.X,
        this.Y
      };
    }

    public static void Add(ref Vector2 left, ref Vector2 right, out Vector2 result)
    {
      result = new Vector2(left.X + right.X, left.Y + right.Y);
    }

    public static Vector2 Add(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X + right.X, left.Y + right.Y);
    }

    public static void Subtract(ref Vector2 left, ref Vector2 right, out Vector2 result)
    {
      result = new Vector2(left.X - right.X, left.Y - right.Y);
    }

    public static Vector2 Subtract(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X - right.X, left.Y - right.Y);
    }

    public static void Multiply(ref Vector2 value, float scale, out Vector2 result)
    {
      result = new Vector2(value.X * scale, value.Y * scale);
    }

    public static Vector2 Multiply(Vector2 value, float scale)
    {
      return new Vector2(value.X * scale, value.Y * scale);
    }

    public static void Modulate(ref Vector2 left, ref Vector2 right, out Vector2 result)
    {
      result = new Vector2(left.X * right.X, left.Y * right.Y);
    }

    public static Vector2 Modulate(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X * right.X, left.Y * right.Y);
    }

    public static void Divide(ref Vector2 value, float scale, out Vector2 result)
    {
      result = new Vector2(value.X / scale, value.Y / scale);
    }

    public static Vector2 Divide(Vector2 value, float scale)
    {
      return new Vector2(value.X / scale, value.Y / scale);
    }

    public static void Negate(ref Vector2 value, out Vector2 result)
    {
      result = new Vector2(-value.X, -value.Y);
    }

    public static Vector2 Negate(Vector2 value)
    {
      return new Vector2(-value.X, -value.Y);
    }

    public static void Barycentric(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result)
    {
      result = new Vector2((float) ((double) value1.X + (double) amount1 * ((double) value2.X - (double) value1.X) + (double) amount2 * ((double) value3.X - (double) value1.X)), (float) ((double) value1.Y + (double) amount1 * ((double) value2.Y - (double) value1.Y) + (double) amount2 * ((double) value3.Y - (double) value1.Y)));
    }

    public static Vector2 Barycentric(Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2)
    {
      Vector2 result;
      Vector2.Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out result);
      return result;
    }

    public static void Clamp(ref Vector2 value, ref Vector2 min, ref Vector2 max, out Vector2 result)
    {
      float num1 = value.X;
      float num2 = (double) num1 > (double) max.X ? max.X : num1;
      float x = (double) num2 < (double) min.X ? min.X : num2;
      float num3 = value.Y;
      float num4 = (double) num3 > (double) max.Y ? max.Y : num3;
      float y = (double) num4 < (double) min.Y ? min.Y : num4;
      result = new Vector2(x, y);
    }

    public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
    {
      Vector2 result;
      Vector2.Clamp(ref value, ref min, ref max, out result);
      return result;
    }

    public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      result = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static float Distance(Vector2 value1, Vector2 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      result = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static float DistanceSquared(Vector2 value1, Vector2 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static void Dot(ref Vector2 left, ref Vector2 right, out float result)
    {
      result = (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y);
    }

    public static float Dot(Vector2 left, Vector2 right)
    {
      return (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y);
    }

    public static void Normalize(ref Vector2 value, out Vector2 result)
    {
      result = value;
      result.Normalize();
    }

    public static Vector2 Normalize(Vector2 value)
    {
      value.Normalize();
      return value;
    }

    public static void Lerp(ref Vector2 start, ref Vector2 end, float amount, out Vector2 result)
    {
      result.X = start.X + (end.X - start.X) * amount;
      result.Y = start.Y + (end.Y - start.Y) * amount;
    }

    public static Vector2 Lerp(Vector2 start, Vector2 end, float amount)
    {
      Vector2 result;
      Vector2.Lerp(ref start, ref end, amount, out result);
      return result;
    }

    public static void SmoothStep(ref Vector2 start, ref Vector2 end, float amount, out Vector2 result)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      result.X = start.X + (end.X - start.X) * amount;
      result.Y = start.Y + (end.Y - start.Y) * amount;
    }

    public static Vector2 SmoothStep(Vector2 start, Vector2 end, float amount)
    {
      Vector2 result;
      Vector2.SmoothStep(ref start, ref end, amount, out result);
      return result;
    }

    public static void Hermite(ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      float num3 = (float) (2.0 * (double) num2 - 3.0 * (double) num1 + 1.0);
      float num4 = (float) (-2.0 * (double) num2 + 3.0 * (double) num1);
      float num5 = num2 - 2f * num1 + amount;
      float num6 = num2 - num1;
      result.X = (float) ((double) value1.X * (double) num3 + (double) value2.X * (double) num4 + (double) tangent1.X * (double) num5 + (double) tangent2.X * (double) num6);
      result.Y = (float) ((double) value1.Y * (double) num3 + (double) value2.Y * (double) num4 + (double) tangent1.Y * (double) num5 + (double) tangent2.Y * (double) num6);
    }

    public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
    {
      Vector2 result;
      Vector2.Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
      return result;
    }

    public static void CatmullRom(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      result.X = (float) (0.5 * (2.0 * (double) value2.X + (-(double) value1.X + (double) value3.X) * (double) amount + (2.0 * (double) value1.X - 5.0 * (double) value2.X + 4.0 * (double) value3.X - (double) value4.X) * (double) num1 + (-(double) value1.X + 3.0 * (double) value2.X - 3.0 * (double) value3.X + (double) value4.X) * (double) num2));
      result.Y = (float) (0.5 * (2.0 * (double) value2.Y + (-(double) value1.Y + (double) value3.Y) * (double) amount + (2.0 * (double) value1.Y - 5.0 * (double) value2.Y + 4.0 * (double) value3.Y - (double) value4.Y) * (double) num1 + (-(double) value1.Y + 3.0 * (double) value2.Y - 3.0 * (double) value3.Y + (double) value4.Y) * (double) num2));
    }

    public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
    {
      Vector2 result;
      Vector2.CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out result);
      return result;
    }

    public static void Max(ref Vector2 left, ref Vector2 right, out Vector2 result)
    {
      result.X = (double) left.X > (double) right.X ? left.X : right.X;
      result.Y = (double) left.Y > (double) right.Y ? left.Y : right.Y;
    }

    public static Vector2 Max(Vector2 left, Vector2 right)
    {
      Vector2 result;
      Vector2.Max(ref left, ref right, out result);
      return result;
    }

    public static void Min(ref Vector2 left, ref Vector2 right, out Vector2 result)
    {
      result.X = (double) left.X < (double) right.X ? left.X : right.X;
      result.Y = (double) left.Y < (double) right.Y ? left.Y : right.Y;
    }

    public static Vector2 Min(Vector2 left, Vector2 right)
    {
      Vector2 result;
      Vector2.Min(ref left, ref right, out result);
      return result;
    }

    public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
    {
      float num = (float) ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y);
      result.X = vector.X - 2f * num * normal.X;
      result.Y = vector.Y - 2f * num * normal.Y;
    }

    public static Vector2 Reflect(Vector2 vector, Vector2 normal)
    {
      Vector2 result;
      Vector2.Reflect(ref vector, ref normal, out result);
      return result;
    }

    public static void Orthogonalize(Vector2[] destination, params Vector2[] source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        Vector2 right = source[index1];
        for (int index2 = 0; index2 < index1; ++index2)
          right -= Vector2.Dot(destination[index2], right) / Vector2.Dot(destination[index2], destination[index2]) * destination[index2];
        destination[index1] = right;
      }
    }

    public static void Orthonormalize(Vector2[] destination, params Vector2[] source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        Vector2 right = source[index1];
        for (int index2 = 0; index2 < index1; ++index2)
          right -= Vector2.Dot(destination[index2], right) * destination[index2];
        right.Normalize();
        destination[index1] = right;
      }
    }

    public static void Transform(ref Vector2 vector, ref Quaternion rotation, out Vector2 result)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num3;
      float num5 = rotation.X * num1;
      float num6 = rotation.X * num2;
      float num7 = rotation.Y * num2;
      float num8 = rotation.Z * num3;
      result = new Vector2((float) ((double) vector.X * (1.0 - (double) num7 - (double) num8) + (double) vector.Y * ((double) num6 - (double) num4)), (float) ((double) vector.X * ((double) num6 + (double) num4) + (double) vector.Y * (1.0 - (double) num5 - (double) num8)));
    }

    public static Vector2 Transform(Vector2 vector, Quaternion rotation)
    {
      Vector2 result;
      Vector2.Transform(ref vector, ref rotation, out result);
      return result;
    }

    public static void Transform(Vector2[] source, ref Quaternion rotation, Vector2[] destination)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num3;
      float num5 = rotation.X * num1;
      float num6 = rotation.X * num2;
      float num7 = rotation.Y * num2;
      float num8 = rotation.Z * num3;
      float num9 = 1f - num7 - num8;
      float num10 = num6 - num4;
      float num11 = num6 + num4;
      float num12 = 1f - num5 - num8;
      for (int index = 0; index < source.Length; ++index)
        destination[index] = new Vector2((float) ((double) source[index].X * (double) num9 + (double) source[index].Y * (double) num10), (float) ((double) source[index].X * (double) num11 + (double) source[index].Y * (double) num12));
    }

    public static void Transform(ref Vector2 vector, ref Matrix transform, out Vector4 result)
    {
      result = new Vector4((float) ((double) vector.X * (double) transform.M11 + (double) vector.Y * (double) transform.M21) + transform.M41, (float) ((double) vector.X * (double) transform.M12 + (double) vector.Y * (double) transform.M22) + transform.M42, (float) ((double) vector.X * (double) transform.M13 + (double) vector.Y * (double) transform.M23) + transform.M43, (float) ((double) vector.X * (double) transform.M14 + (double) vector.Y * (double) transform.M24) + transform.M44);
    }

    public static Vector4 Transform(Vector2 vector, Matrix transform)
    {
      Vector4 result;
      Vector2.Transform(ref vector, ref transform, out result);
      return result;
    }

    public static void Transform(Vector2[] source, ref Matrix transform, Vector4[] destination)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index = 0; index < source.Length; ++index)
        Vector2.Transform(ref source[index], ref transform, out destination[index]);
    }

    public static void TransformCoordinate(ref Vector2 coordinate, ref Matrix transform, out Vector2 result)
    {
      Vector4 vector4 = new Vector4();
      vector4.X = (float) ((double) coordinate.X * (double) transform.M11 + (double) coordinate.Y * (double) transform.M21) + transform.M41;
      vector4.Y = (float) ((double) coordinate.X * (double) transform.M12 + (double) coordinate.Y * (double) transform.M22) + transform.M42;
      vector4.Z = (float) ((double) coordinate.X * (double) transform.M13 + (double) coordinate.Y * (double) transform.M23) + transform.M43;
      vector4.W = (float) (1.0 / ((double) coordinate.X * (double) transform.M14 + (double) coordinate.Y * (double) transform.M24 + (double) transform.M44));
      result = new Vector2(vector4.X * vector4.W, vector4.Y * vector4.W);
    }

    public static Vector2 TransformCoordinate(Vector2 coordinate, Matrix transform)
    {
      Vector2 result;
      Vector2.TransformCoordinate(ref coordinate, ref transform, out result);
      return result;
    }

    public static void TransformCoordinate(Vector2[] source, ref Matrix transform, Vector2[] destination)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index = 0; index < source.Length; ++index)
        Vector2.TransformCoordinate(ref source[index], ref transform, out destination[index]);
    }

    public static void TransformNormal(ref Vector2 normal, ref Matrix transform, out Vector2 result)
    {
      result = new Vector2((float) ((double) normal.X * (double) transform.M11 + (double) normal.Y * (double) transform.M21), (float) ((double) normal.X * (double) transform.M12 + (double) normal.Y * (double) transform.M22));
    }

    public static Vector2 TransformNormal(Vector2 normal, Matrix transform)
    {
      Vector2 result;
      Vector2.TransformNormal(ref normal, ref transform, out result);
      return result;
    }

    public static void TransformNormal(Vector2[] source, ref Matrix transform, Vector2[] destination)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index = 0; index < source.Length; ++index)
        Vector2.TransformNormal(ref source[index], ref transform, out destination[index]);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "X:{0} Y:{1}", new object[2]
      {
        (object) this.X,
        (object) this.Y
      });
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "X:{0} Y:{1}", new object[2]
      {
        (object) this.X.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture),
        (object) this.Y.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture)
      });
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "X:{0} Y:{1}", new object[2]
      {
        (object) this.X,
        (object) this.Y
      });
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        this.ToString(formatProvider);
      return string.Format(formatProvider, "X:{0} Y:{1}", new object[2]
      {
        (object) this.X.ToString(format, formatProvider),
        (object) this.Y.ToString(format, formatProvider)
      });
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() + this.Y.GetHashCode();
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.X);
        serializer.Writer.Write(this.Y);
      }
      else
      {
        this.X = serializer.Reader.ReadSingle();
        this.Y = serializer.Reader.ReadSingle();
      }
    }

    public bool Equals(Vector2 other)
    {
      if ((double) Math.Abs(other.X - this.X) < 9.99999997475243E-07)
        return (double) Math.Abs(other.Y - this.Y) < 9.99999997475243E-07;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Vector2)))
        return false;
      else
        return this.Equals((Vector2) value);
    }
  }
}
