// Type: SharpDX.Vector4
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
  [TypeConverter(typeof (Vector4Converter))]
  [DynamicSerializer("TKV4")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Vector4 : IEquatable<Vector4>, IFormattable, IDataSerializable
  {
    public static readonly int SizeInBytes = Marshal.SizeOf(typeof (Vector4));
    public static readonly Vector4 Zero = new Vector4();
    public static readonly Vector4 UnitX = new Vector4(1f, 0.0f, 0.0f, 0.0f);
    public static readonly Vector4 UnitY = new Vector4(0.0f, 1f, 0.0f, 0.0f);
    public static readonly Vector4 UnitZ = new Vector4(0.0f, 0.0f, 1f, 0.0f);
    public static readonly Vector4 UnitW = new Vector4(0.0f, 0.0f, 0.0f, 1f);
    public static readonly Vector4 One = new Vector4(1f, 1f, 1f, 1f);
    public float X;
    public float Y;
    public float Z;
    public float W;

    public bool IsNormalized
    {
      get
      {
        return (double) Math.Abs((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W - 1.0)) < 9.99999997475243E-07;
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
            throw new ArgumentOutOfRangeException("index", "Indices for Vector4 run from 0 to 3, inclusive.");
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
            throw new ArgumentOutOfRangeException("index", "Indices for Vector4 run from 0 to 3, inclusive.");
        }
      }
    }

    static Vector4()
    {
    }

    public Vector4(float value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
      this.W = value;
    }

    public Vector4(float x, float y, float z, float w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public Vector4(Vector3 value, float w)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = value.Z;
      this.W = w;
    }

    public Vector4(Vector2 value, float z, float w)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = z;
      this.W = w;
    }

    public Vector4(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Vector4.");
      this.X = values[0];
      this.Y = values[1];
      this.Z = values[2];
      this.W = values[3];
    }

    public static explicit operator Vector2(Vector4 value)
    {
      return new Vector2(value.X, value.Y);
    }

    public static explicit operator Vector3(Vector4 value)
    {
      return new Vector3(value.X, value.Y, value.Z);
    }

    public static Vector4 operator +(Vector4 left, Vector4 right)
    {
      return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    public static Vector4 operator *(Vector4 left, Vector4 right)
    {
      return new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }

    public static Vector4 operator +(Vector4 value)
    {
      return value;
    }

    public static Vector4 operator -(Vector4 left, Vector4 right)
    {
      return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }

    public static Vector4 operator -(Vector4 value)
    {
      return new Vector4(-value.X, -value.Y, -value.Z, -value.W);
    }

    public static Vector4 operator *(float scale, Vector4 value)
    {
      return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
    }

    public static Vector4 operator *(Vector4 value, float scale)
    {
      return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
    }

    public static Vector4 operator /(Vector4 value, float scale)
    {
      return new Vector4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
    }

    public static bool operator ==(Vector4 left, Vector4 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Vector4 left, Vector4 right)
    {
      return !left.Equals(right);
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

    public static void Add(ref Vector4 left, ref Vector4 right, out Vector4 result)
    {
      result = new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    public static Vector4 Add(Vector4 left, Vector4 right)
    {
      return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    public static void Subtract(ref Vector4 left, ref Vector4 right, out Vector4 result)
    {
      result = new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }

    public static Vector4 Subtract(Vector4 left, Vector4 right)
    {
      return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }

    public static void Multiply(ref Vector4 value, float scale, out Vector4 result)
    {
      result = new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
    }

    public static Vector4 Multiply(Vector4 value, float scale)
    {
      return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
    }

    public static void Modulate(ref Vector4 left, ref Vector4 right, out Vector4 result)
    {
      result = new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }

    public static Vector4 Modulate(Vector4 left, Vector4 right)
    {
      return new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }

    public static void Divide(ref Vector4 value, float scale, out Vector4 result)
    {
      result = new Vector4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
    }

    public static Vector4 Divide(Vector4 value, float scale)
    {
      return new Vector4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
    }

    public static void Negate(ref Vector4 value, out Vector4 result)
    {
      result = new Vector4(-value.X, -value.Y, -value.Z, -value.W);
    }

    public static Vector4 Negate(Vector4 value)
    {
      return new Vector4(-value.X, -value.Y, -value.Z, -value.W);
    }

    public static void Barycentric(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, float amount1, float amount2, out Vector4 result)
    {
      result = new Vector4((float) ((double) value1.X + (double) amount1 * ((double) value2.X - (double) value1.X) + (double) amount2 * ((double) value3.X - (double) value1.X)), (float) ((double) value1.Y + (double) amount1 * ((double) value2.Y - (double) value1.Y) + (double) amount2 * ((double) value3.Y - (double) value1.Y)), (float) ((double) value1.Z + (double) amount1 * ((double) value2.Z - (double) value1.Z) + (double) amount2 * ((double) value3.Z - (double) value1.Z)), (float) ((double) value1.W + (double) amount1 * ((double) value2.W - (double) value1.W) + (double) amount2 * ((double) value3.W - (double) value1.W)));
    }

    public static Vector4 Barycentric(Vector4 value1, Vector4 value2, Vector4 value3, float amount1, float amount2)
    {
      Vector4 result;
      Vector4.Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out result);
      return result;
    }

    public static void Clamp(ref Vector4 value, ref Vector4 min, ref Vector4 max, out Vector4 result)
    {
      float num1 = value.X;
      float num2 = (double) num1 > (double) max.X ? max.X : num1;
      float x = (double) num2 < (double) min.X ? min.X : num2;
      float num3 = value.Y;
      float num4 = (double) num3 > (double) max.Y ? max.Y : num3;
      float y = (double) num4 < (double) min.Y ? min.Y : num4;
      float num5 = value.Z;
      float num6 = (double) num5 > (double) max.Z ? max.Z : num5;
      float z = (double) num6 < (double) min.Z ? min.Z : num6;
      float num7 = value.W;
      float num8 = (double) num7 > (double) max.W ? max.W : num7;
      float w = (double) num8 < (double) min.W ? min.W : num8;
      result = new Vector4(x, y, z, w);
    }

    public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max)
    {
      Vector4 result;
      Vector4.Clamp(ref value, ref min, ref max, out result);
      return result;
    }

    public static void Distance(ref Vector4 value1, ref Vector4 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      float num3 = value1.Z - value2.Z;
      float num4 = value1.W - value2.W;
      result = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 + (double) num4 * (double) num4);
    }

    public static float Distance(Vector4 value1, Vector4 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      float num3 = value1.Z - value2.Z;
      float num4 = value1.W - value2.W;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 + (double) num4 * (double) num4);
    }

    public static void DistanceSquared(ref Vector4 value1, ref Vector4 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      float num3 = value1.Z - value2.Z;
      float num4 = value1.W - value2.W;
      result = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 + (double) num4 * (double) num4);
    }

    public static float DistanceSquared(Vector4 value1, Vector4 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      float num3 = value1.Z - value2.Z;
      float num4 = value1.W - value2.W;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 + (double) num4 * (double) num4);
    }

    public static void Dot(ref Vector4 left, ref Vector4 right, out float result)
    {
      result = (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z + (double) left.W * (double) right.W);
    }

    public static float Dot(Vector4 left, Vector4 right)
    {
      return (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z + (double) left.W * (double) right.W);
    }

    public static void Normalize(ref Vector4 value, out Vector4 result)
    {
      Vector4 vector4 = value;
      result = vector4;
      result.Normalize();
    }

    public static Vector4 Normalize(Vector4 value)
    {
      value.Normalize();
      return value;
    }

    public static void Lerp(ref Vector4 start, ref Vector4 end, float amount, out Vector4 result)
    {
      result.X = start.X + (end.X - start.X) * amount;
      result.Y = start.Y + (end.Y - start.Y) * amount;
      result.Z = start.Z + (end.Z - start.Z) * amount;
      result.W = start.W + (end.W - start.W) * amount;
    }

    public static Vector4 Lerp(Vector4 start, Vector4 end, float amount)
    {
      Vector4 result;
      Vector4.Lerp(ref start, ref end, amount, out result);
      return result;
    }

    public static void SmoothStep(ref Vector4 start, ref Vector4 end, float amount, out Vector4 result)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      result.X = start.X + (end.X - start.X) * amount;
      result.Y = start.Y + (end.Y - start.Y) * amount;
      result.Z = start.Z + (end.Z - start.Z) * amount;
      result.W = start.W + (end.W - start.W) * amount;
    }

    public static Vector4 SmoothStep(Vector4 start, Vector4 end, float amount)
    {
      Vector4 result;
      Vector4.SmoothStep(ref start, ref end, amount, out result);
      return result;
    }

    public static void Hermite(ref Vector4 value1, ref Vector4 tangent1, ref Vector4 value2, ref Vector4 tangent2, float amount, out Vector4 result)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      float num3 = (float) (2.0 * (double) num2 - 3.0 * (double) num1 + 1.0);
      float num4 = (float) (-2.0 * (double) num2 + 3.0 * (double) num1);
      float num5 = num2 - 2f * num1 + amount;
      float num6 = num2 - num1;
      result = new Vector4((float) ((double) value1.X * (double) num3 + (double) value2.X * (double) num4 + (double) tangent1.X * (double) num5 + (double) tangent2.X * (double) num6), (float) ((double) value1.Y * (double) num3 + (double) value2.Y * (double) num4 + (double) tangent1.Y * (double) num5 + (double) tangent2.Y * (double) num6), (float) ((double) value1.Z * (double) num3 + (double) value2.Z * (double) num4 + (double) tangent1.Z * (double) num5 + (double) tangent2.Z * (double) num6), (float) ((double) value1.W * (double) num3 + (double) value2.W * (double) num4 + (double) tangent1.W * (double) num5 + (double) tangent2.W * (double) num6));
    }

    public static Vector4 Hermite(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
    {
      Vector4 result;
      Vector4.Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
      return result;
    }

    public static void CatmullRom(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, ref Vector4 value4, float amount, out Vector4 result)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      result.X = (float) (0.5 * (2.0 * (double) value2.X + (-(double) value1.X + (double) value3.X) * (double) amount + (2.0 * (double) value1.X - 5.0 * (double) value2.X + 4.0 * (double) value3.X - (double) value4.X) * (double) num1 + (-(double) value1.X + 3.0 * (double) value2.X - 3.0 * (double) value3.X + (double) value4.X) * (double) num2));
      result.Y = (float) (0.5 * (2.0 * (double) value2.Y + (-(double) value1.Y + (double) value3.Y) * (double) amount + (2.0 * (double) value1.Y - 5.0 * (double) value2.Y + 4.0 * (double) value3.Y - (double) value4.Y) * (double) num1 + (-(double) value1.Y + 3.0 * (double) value2.Y - 3.0 * (double) value3.Y + (double) value4.Y) * (double) num2));
      result.Z = (float) (0.5 * (2.0 * (double) value2.Z + (-(double) value1.Z + (double) value3.Z) * (double) amount + (2.0 * (double) value1.Z - 5.0 * (double) value2.Z + 4.0 * (double) value3.Z - (double) value4.Z) * (double) num1 + (-(double) value1.Z + 3.0 * (double) value2.Z - 3.0 * (double) value3.Z + (double) value4.Z) * (double) num2));
      result.W = (float) (0.5 * (2.0 * (double) value2.W + (-(double) value1.W + (double) value3.W) * (double) amount + (2.0 * (double) value1.W - 5.0 * (double) value2.W + 4.0 * (double) value3.W - (double) value4.W) * (double) num1 + (-(double) value1.W + 3.0 * (double) value2.W - 3.0 * (double) value3.W + (double) value4.W) * (double) num2));
    }

    public static Vector4 CatmullRom(Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount)
    {
      Vector4 result;
      Vector4.CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out result);
      return result;
    }

    public static void Max(ref Vector4 left, ref Vector4 right, out Vector4 result)
    {
      result.X = (double) left.X > (double) right.X ? left.X : right.X;
      result.Y = (double) left.Y > (double) right.Y ? left.Y : right.Y;
      result.Z = (double) left.Z > (double) right.Z ? left.Z : right.Z;
      result.W = (double) left.W > (double) right.W ? left.W : right.W;
    }

    public static Vector4 Max(Vector4 left, Vector4 right)
    {
      Vector4 result;
      Vector4.Max(ref left, ref right, out result);
      return result;
    }

    public static void Min(ref Vector4 left, ref Vector4 right, out Vector4 result)
    {
      result.X = (double) left.X < (double) right.X ? left.X : right.X;
      result.Y = (double) left.Y < (double) right.Y ? left.Y : right.Y;
      result.Z = (double) left.Z < (double) right.Z ? left.Z : right.Z;
      result.W = (double) left.W < (double) right.W ? left.W : right.W;
    }

    public static Vector4 Min(Vector4 left, Vector4 right)
    {
      Vector4 result;
      Vector4.Min(ref left, ref right, out result);
      return result;
    }

    public static void Orthogonalize(Vector4[] destination, params Vector4[] source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        Vector4 right = source[index1];
        for (int index2 = 0; index2 < index1; ++index2)
          right -= Vector4.Dot(destination[index2], right) / Vector4.Dot(destination[index2], destination[index2]) * destination[index2];
        destination[index1] = right;
      }
    }

    public static void Orthonormalize(Vector4[] destination, params Vector4[] source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        Vector4 right = source[index1];
        for (int index2 = 0; index2 < index1; ++index2)
          right -= Vector4.Dot(destination[index2], right) * destination[index2];
        right.Normalize();
        destination[index1] = right;
      }
    }

    public static void Transform(ref Vector4 vector, ref Quaternion rotation, out Vector4 result)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num1;
      float num5 = rotation.W * num2;
      float num6 = rotation.W * num3;
      float num7 = rotation.X * num1;
      float num8 = rotation.X * num2;
      float num9 = rotation.X * num3;
      float num10 = rotation.Y * num2;
      float num11 = rotation.Y * num3;
      float num12 = rotation.Z * num3;
      result = new Vector4((float) ((double) vector.X * (1.0 - (double) num10 - (double) num12) + (double) vector.Y * ((double) num8 - (double) num6) + (double) vector.Z * ((double) num9 + (double) num5)), (float) ((double) vector.X * ((double) num8 + (double) num6) + (double) vector.Y * (1.0 - (double) num7 - (double) num12) + (double) vector.Z * ((double) num11 - (double) num4)), (float) ((double) vector.X * ((double) num9 - (double) num5) + (double) vector.Y * ((double) num11 + (double) num4) + (double) vector.Z * (1.0 - (double) num7 - (double) num10)), vector.W);
    }

    public static Vector4 Transform(Vector4 vector, Quaternion rotation)
    {
      Vector4 result;
      Vector4.Transform(ref vector, ref rotation, out result);
      return result;
    }

    public static void Transform(Vector4[] source, ref Quaternion rotation, Vector4[] destination)
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
      float num4 = rotation.W * num1;
      float num5 = rotation.W * num2;
      float num6 = rotation.W * num3;
      float num7 = rotation.X * num1;
      float num8 = rotation.X * num2;
      float num9 = rotation.X * num3;
      float num10 = rotation.Y * num2;
      float num11 = rotation.Y * num3;
      float num12 = rotation.Z * num3;
      float num13 = 1f - num10 - num12;
      float num14 = num8 - num6;
      float num15 = num9 + num5;
      float num16 = num8 + num6;
      float num17 = 1f - num7 - num12;
      float num18 = num11 - num4;
      float num19 = num9 - num5;
      float num20 = num11 + num4;
      float num21 = 1f - num7 - num10;
      for (int index = 0; index < source.Length; ++index)
        destination[index] = new Vector4((float) ((double) source[index].X * (double) num13 + (double) source[index].Y * (double) num14 + (double) source[index].Z * (double) num15), (float) ((double) source[index].X * (double) num16 + (double) source[index].Y * (double) num17 + (double) source[index].Z * (double) num18), (float) ((double) source[index].X * (double) num19 + (double) source[index].Y * (double) num20 + (double) source[index].Z * (double) num21), source[index].W);
    }

    public static void Transform(ref Vector4 vector, ref Matrix transform, out Vector4 result)
    {
      result = new Vector4((float) ((double) vector.X * (double) transform.M11 + (double) vector.Y * (double) transform.M21 + (double) vector.Z * (double) transform.M31 + (double) vector.W * (double) transform.M41), (float) ((double) vector.X * (double) transform.M12 + (double) vector.Y * (double) transform.M22 + (double) vector.Z * (double) transform.M32 + (double) vector.W * (double) transform.M42), (float) ((double) vector.X * (double) transform.M13 + (double) vector.Y * (double) transform.M23 + (double) vector.Z * (double) transform.M33 + (double) vector.W * (double) transform.M43), (float) ((double) vector.X * (double) transform.M14 + (double) vector.Y * (double) transform.M24 + (double) vector.Z * (double) transform.M34 + (double) vector.W * (double) transform.M44));
    }

    public static Vector4 Transform(Vector4 vector, Matrix transform)
    {
      Vector4 result;
      Vector4.Transform(ref vector, ref transform, out result);
      return result;
    }

    public static void Transform(ref Vector4 vector, ref Matrix5x4 transform, out Vector4 result)
    {
      result = new Vector4((float) ((double) vector.X * (double) transform.M11 + (double) vector.Y * (double) transform.M21 + (double) vector.Z * (double) transform.M31 + (double) vector.W * (double) transform.M41) + transform.M51, (float) ((double) vector.X * (double) transform.M12 + (double) vector.Y * (double) transform.M22 + (double) vector.Z * (double) transform.M32 + (double) vector.W * (double) transform.M42) + transform.M52, (float) ((double) vector.X * (double) transform.M13 + (double) vector.Y * (double) transform.M23 + (double) vector.Z * (double) transform.M33 + (double) vector.W * (double) transform.M43) + transform.M53, (float) ((double) vector.X * (double) transform.M14 + (double) vector.Y * (double) transform.M24 + (double) vector.Z * (double) transform.M34 + (double) vector.W * (double) transform.M44) + transform.M54);
    }

    public static Vector4 Transform(Vector4 vector, Matrix5x4 transform)
    {
      Vector4 result;
      Vector4.Transform(ref vector, ref transform, out result);
      return result;
    }

    public static void Transform(Vector4[] source, ref Matrix transform, Vector4[] destination)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index = 0; index < source.Length; ++index)
        Vector4.Transform(ref source[index], ref transform, out destination[index]);
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
        this.X = serializer.Reader.ReadSingle();
        this.Y = serializer.Reader.ReadSingle();
        this.Z = serializer.Reader.ReadSingle();
        this.W = serializer.Reader.ReadSingle();
      }
    }

    public bool Equals(Vector4 other)
    {
      if ((double) Math.Abs(other.X - this.X) < 9.99999997475243E-07 && (double) Math.Abs(other.Y - this.Y) < 9.99999997475243E-07 && (double) Math.Abs(other.Z - this.Z) < 9.99999997475243E-07)
        return (double) Math.Abs(other.W - this.W) < 9.99999997475243E-07;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Vector4)))
        return false;
      else
        return this.Equals((Vector4) value);
    }
  }
}
