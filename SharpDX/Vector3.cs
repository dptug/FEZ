// Type: SharpDX.Vector3
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
  [TypeConverter(typeof (Vector3Converter))]
  [DynamicSerializer("TKV3")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Vector3 : IEquatable<Vector3>, IFormattable, IDataSerializable
  {
    public static readonly int SizeInBytes = Marshal.SizeOf(typeof (Vector3));
    public static readonly Vector3 Zero = new Vector3();
    public static readonly Vector3 UnitX = new Vector3(1f, 0.0f, 0.0f);
    public static readonly Vector3 UnitY = new Vector3(0.0f, 1f, 0.0f);
    public static readonly Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1f);
    public static readonly Vector3 One = new Vector3(1f, 1f, 1f);
    public float X;
    public float Y;
    public float Z;

    public bool IsNormalized
    {
      get
      {
        return (double) Math.Abs((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z - 1.0)) < 9.99999997475243E-07;
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
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
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
            throw new ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
        }
      }
    }

    static Vector3()
    {
    }

    public Vector3(float value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
    }

    public Vector3(float x, float y, float z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Vector3(Vector2 value, float z)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = z;
    }

    public Vector3(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 3)
        throw new ArgumentOutOfRangeException("values", "There must be three and only three input values for Vector3.");
      this.X = values[0];
      this.Y = values[1];
      this.Z = values[2];
    }

    public static explicit operator Vector2(Vector3 value)
    {
      return new Vector2(value.X, value.Y);
    }

    public static explicit operator Vector4(Vector3 value)
    {
      return new Vector4(value, 0.0f);
    }

    public static Vector3 operator +(Vector3 left, Vector3 right)
    {
      return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vector3 operator *(Vector3 left, Vector3 right)
    {
      return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static Vector3 operator +(Vector3 value)
    {
      return value;
    }

    public static Vector3 operator -(Vector3 left, Vector3 right)
    {
      return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static Vector3 operator -(Vector3 value)
    {
      return new Vector3(-value.X, -value.Y, -value.Z);
    }

    public static Vector3 operator *(float scale, Vector3 value)
    {
      return new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
    }

    public static Vector3 operator *(Vector3 value, float scale)
    {
      return new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
    }

    public static Vector3 operator /(Vector3 value, float scale)
    {
      return new Vector3(value.X / scale, value.Y / scale, value.Z / scale);
    }

    public static bool operator ==(Vector3 left, Vector3 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Vector3 left, Vector3 right)
    {
      return !left.Equals(right);
    }

    public float Length()
    {
      return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);
    }

    public float LengthSquared()
    {
      return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);
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
    }

    public float[] ToArray()
    {
      return new float[3]
      {
        this.X,
        this.Y,
        this.Z
      };
    }

    public static void Add(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
      result = new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vector3 Add(Vector3 left, Vector3 right)
    {
      return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static void Subtract(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
      result = new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static Vector3 Subtract(Vector3 left, Vector3 right)
    {
      return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static void Multiply(ref Vector3 value, float scale, out Vector3 result)
    {
      result = new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
    }

    public static Vector3 Multiply(Vector3 value, float scale)
    {
      return new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
    }

    public static void Modulate(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
      result = new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static Vector3 Modulate(Vector3 left, Vector3 right)
    {
      return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static void Divide(ref Vector3 value, float scale, out Vector3 result)
    {
      result = new Vector3(value.X / scale, value.Y / scale, value.Z / scale);
    }

    public static Vector3 Divide(Vector3 value, float scale)
    {
      return new Vector3(value.X / scale, value.Y / scale, value.Z / scale);
    }

    public static void Negate(ref Vector3 value, out Vector3 result)
    {
      result = new Vector3(-value.X, -value.Y, -value.Z);
    }

    public static Vector3 Negate(Vector3 value)
    {
      return new Vector3(-value.X, -value.Y, -value.Z);
    }

    public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result)
    {
      result = new Vector3((float) ((double) value1.X + (double) amount1 * ((double) value2.X - (double) value1.X) + (double) amount2 * ((double) value3.X - (double) value1.X)), (float) ((double) value1.Y + (double) amount1 * ((double) value2.Y - (double) value1.Y) + (double) amount2 * ((double) value3.Y - (double) value1.Y)), (float) ((double) value1.Z + (double) amount1 * ((double) value2.Z - (double) value1.Z) + (double) amount2 * ((double) value3.Z - (double) value1.Z)));
    }

    public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
    {
      Vector3 result;
      Vector3.Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out result);
      return result;
    }

    public static void Clamp(ref Vector3 value, ref Vector3 min, ref Vector3 max, out Vector3 result)
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
      result = new Vector3(x, y, z);
    }

    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
    {
      Vector3 result;
      Vector3.Clamp(ref value, ref min, ref max, out result);
      return result;
    }

    public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
      result = new Vector3((float) ((double) left.Y * (double) right.Z - (double) left.Z * (double) right.Y), (float) ((double) left.Z * (double) right.X - (double) left.X * (double) right.Z), (float) ((double) left.X * (double) right.Y - (double) left.Y * (double) right.X));
    }

    public static Vector3 Cross(Vector3 left, Vector3 right)
    {
      Vector3 result;
      Vector3.Cross(ref left, ref right, out result);
      return result;
    }

    public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      float num3 = value1.Z - value2.Z;
      result = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
    }

    public static float Distance(Vector3 value1, Vector3 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      float num3 = value1.Z - value2.Z;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
    }

    public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      float num3 = value1.Z - value2.Z;
      result = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
    }

    public static float DistanceSquared(Vector3 value1, Vector3 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      float num3 = value1.Z - value2.Z;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
    }

    public static void Dot(ref Vector3 left, ref Vector3 right, out float result)
    {
      result = (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z);
    }

    public static float Dot(Vector3 left, Vector3 right)
    {
      return (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z);
    }

    public static void Normalize(ref Vector3 value, out Vector3 result)
    {
      result = value;
      result.Normalize();
    }

    public static Vector3 Normalize(Vector3 value)
    {
      value.Normalize();
      return value;
    }

    public static void Lerp(ref Vector3 start, ref Vector3 end, float amount, out Vector3 result)
    {
      result.X = start.X + (end.X - start.X) * amount;
      result.Y = start.Y + (end.Y - start.Y) * amount;
      result.Z = start.Z + (end.Z - start.Z) * amount;
    }

    public static Vector3 Lerp(Vector3 start, Vector3 end, float amount)
    {
      Vector3 result;
      Vector3.Lerp(ref start, ref end, amount, out result);
      return result;
    }

    public static void SmoothStep(ref Vector3 start, ref Vector3 end, float amount, out Vector3 result)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      result.X = start.X + (end.X - start.X) * amount;
      result.Y = start.Y + (end.Y - start.Y) * amount;
      result.Z = start.Z + (end.Z - start.Z) * amount;
    }

    public static Vector3 SmoothStep(Vector3 start, Vector3 end, float amount)
    {
      Vector3 result;
      Vector3.SmoothStep(ref start, ref end, amount, out result);
      return result;
    }

    public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      float num3 = (float) (2.0 * (double) num2 - 3.0 * (double) num1 + 1.0);
      float num4 = (float) (-2.0 * (double) num2 + 3.0 * (double) num1);
      float num5 = num2 - 2f * num1 + amount;
      float num6 = num2 - num1;
      result.X = (float) ((double) value1.X * (double) num3 + (double) value2.X * (double) num4 + (double) tangent1.X * (double) num5 + (double) tangent2.X * (double) num6);
      result.Y = (float) ((double) value1.Y * (double) num3 + (double) value2.Y * (double) num4 + (double) tangent1.Y * (double) num5 + (double) tangent2.Y * (double) num6);
      result.Z = (float) ((double) value1.Z * (double) num3 + (double) value2.Z * (double) num4 + (double) tangent1.Z * (double) num5 + (double) tangent2.Z * (double) num6);
    }

    public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
    {
      Vector3 result;
      Vector3.Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
      return result;
    }

    public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      result.X = (float) (0.5 * (2.0 * (double) value2.X + (-(double) value1.X + (double) value3.X) * (double) amount + (2.0 * (double) value1.X - 5.0 * (double) value2.X + 4.0 * (double) value3.X - (double) value4.X) * (double) num1 + (-(double) value1.X + 3.0 * (double) value2.X - 3.0 * (double) value3.X + (double) value4.X) * (double) num2));
      result.Y = (float) (0.5 * (2.0 * (double) value2.Y + (-(double) value1.Y + (double) value3.Y) * (double) amount + (2.0 * (double) value1.Y - 5.0 * (double) value2.Y + 4.0 * (double) value3.Y - (double) value4.Y) * (double) num1 + (-(double) value1.Y + 3.0 * (double) value2.Y - 3.0 * (double) value3.Y + (double) value4.Y) * (double) num2));
      result.Z = (float) (0.5 * (2.0 * (double) value2.Z + (-(double) value1.Z + (double) value3.Z) * (double) amount + (2.0 * (double) value1.Z - 5.0 * (double) value2.Z + 4.0 * (double) value3.Z - (double) value4.Z) * (double) num1 + (-(double) value1.Z + 3.0 * (double) value2.Z - 3.0 * (double) value3.Z + (double) value4.Z) * (double) num2));
    }

    public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
    {
      Vector3 result;
      Vector3.CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out result);
      return result;
    }

    public static void Max(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
      result.X = (double) left.X > (double) right.X ? left.X : right.X;
      result.Y = (double) left.Y > (double) right.Y ? left.Y : right.Y;
      result.Z = (double) left.Z > (double) right.Z ? left.Z : right.Z;
    }

    public static Vector3 Max(Vector3 left, Vector3 right)
    {
      Vector3 result;
      Vector3.Max(ref left, ref right, out result);
      return result;
    }

    public static void Min(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
      result.X = (double) left.X < (double) right.X ? left.X : right.X;
      result.Y = (double) left.Y < (double) right.Y ? left.Y : right.Y;
      result.Z = (double) left.Z < (double) right.Z ? left.Z : right.Z;
    }

    public static Vector3 Min(Vector3 left, Vector3 right)
    {
      Vector3 result;
      Vector3.Min(ref left, ref right, out result);
      return result;
    }

    public static void Project(ref Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, ref Matrix worldViewProjection, out Vector3 result)
    {
      Vector3 result1 = new Vector3();
      Vector3.TransformCoordinate(ref vector, ref worldViewProjection, out result1);
      result = new Vector3((float) ((1.0 + (double) result1.X) * 0.5) * width + x, (float) ((1.0 - (double) result1.Y) * 0.5) * height + y, result1.Z * (maxZ - minZ) + minZ);
    }

    public static Vector3 Project(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix worldViewProjection)
    {
      Vector3 result;
      Vector3.Project(ref vector, x, y, width, height, minZ, maxZ, ref worldViewProjection, out result);
      return result;
    }

    public static void Unproject(ref Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, ref Matrix worldViewProjection, out Vector3 result)
    {
      Vector3 coordinate = new Vector3();
      Matrix result1 = new Matrix();
      Matrix.Invert(ref worldViewProjection, out result1);
      coordinate.X = (float) (((double) vector.X - (double) x) / (double) width * 2.0 - 1.0);
      coordinate.Y = (float) -(((double) vector.Y - (double) y) / (double) height * 2.0 - 1.0);
      coordinate.Z = (float) (((double) vector.Z - (double) minZ) / ((double) maxZ - (double) minZ));
      Vector3.TransformCoordinate(ref coordinate, ref result1, out result);
    }

    public static Vector3 Unproject(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix worldViewProjection)
    {
      Vector3 result;
      Vector3.Unproject(ref vector, x, y, width, height, minZ, maxZ, ref worldViewProjection, out result);
      return result;
    }

    public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
    {
      float num = (float) ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y + (double) vector.Z * (double) normal.Z);
      result.X = vector.X - 2f * num * normal.X;
      result.Y = vector.Y - 2f * num * normal.Y;
      result.Z = vector.Z - 2f * num * normal.Z;
    }

    public static Vector3 Reflect(Vector3 vector, Vector3 normal)
    {
      Vector3 result;
      Vector3.Reflect(ref vector, ref normal, out result);
      return result;
    }

    public static void Orthogonalize(Vector3[] destination, params Vector3[] source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        Vector3 right = source[index1];
        for (int index2 = 0; index2 < index1; ++index2)
          right -= Vector3.Dot(destination[index2], right) / Vector3.Dot(destination[index2], destination[index2]) * destination[index2];
        destination[index1] = right;
      }
    }

    public static void Orthonormalize(Vector3[] destination, params Vector3[] source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        Vector3 right = source[index1];
        for (int index2 = 0; index2 < index1; ++index2)
          right -= Vector3.Dot(destination[index2], right) * destination[index2];
        right.Normalize();
        destination[index1] = right;
      }
    }

    public static void Transform(ref Vector3 vector, ref Quaternion rotation, out Vector3 result)
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
      result = new Vector3((float) ((double) vector.X * (1.0 - (double) num10 - (double) num12) + (double) vector.Y * ((double) num8 - (double) num6) + (double) vector.Z * ((double) num9 + (double) num5)), (float) ((double) vector.X * ((double) num8 + (double) num6) + (double) vector.Y * (1.0 - (double) num7 - (double) num12) + (double) vector.Z * ((double) num11 - (double) num4)), (float) ((double) vector.X * ((double) num9 - (double) num5) + (double) vector.Y * ((double) num11 + (double) num4) + (double) vector.Z * (1.0 - (double) num7 - (double) num10)));
    }

    public static Vector3 Transform(Vector3 vector, Quaternion rotation)
    {
      Vector3 result;
      Vector3.Transform(ref vector, ref rotation, out result);
      return result;
    }

    public static void Transform(Vector3[] source, ref Quaternion rotation, Vector3[] destination)
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
        destination[index] = new Vector3((float) ((double) source[index].X * (double) num13 + (double) source[index].Y * (double) num14 + (double) source[index].Z * (double) num15), (float) ((double) source[index].X * (double) num16 + (double) source[index].Y * (double) num17 + (double) source[index].Z * (double) num18), (float) ((double) source[index].X * (double) num19 + (double) source[index].Y * (double) num20 + (double) source[index].Z * (double) num21));
    }

    public static void Transform(ref Vector3 vector, ref Matrix transform, out Vector4 result)
    {
      result = new Vector4((float) ((double) vector.X * (double) transform.M11 + (double) vector.Y * (double) transform.M21 + (double) vector.Z * (double) transform.M31) + transform.M41, (float) ((double) vector.X * (double) transform.M12 + (double) vector.Y * (double) transform.M22 + (double) vector.Z * (double) transform.M32) + transform.M42, (float) ((double) vector.X * (double) transform.M13 + (double) vector.Y * (double) transform.M23 + (double) vector.Z * (double) transform.M33) + transform.M43, (float) ((double) vector.X * (double) transform.M14 + (double) vector.Y * (double) transform.M24 + (double) vector.Z * (double) transform.M34) + transform.M44);
    }

    public static Vector4 Transform(Vector3 vector, Matrix transform)
    {
      Vector4 result;
      Vector3.Transform(ref vector, ref transform, out result);
      return result;
    }

    public static void Transform(Vector3[] source, ref Matrix transform, Vector4[] destination)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index = 0; index < source.Length; ++index)
        Vector3.Transform(ref source[index], ref transform, out destination[index]);
    }

    public static void TransformCoordinate(ref Vector3 coordinate, ref Matrix transform, out Vector3 result)
    {
      Vector4 vector4 = new Vector4();
      vector4.X = (float) ((double) coordinate.X * (double) transform.M11 + (double) coordinate.Y * (double) transform.M21 + (double) coordinate.Z * (double) transform.M31) + transform.M41;
      vector4.Y = (float) ((double) coordinate.X * (double) transform.M12 + (double) coordinate.Y * (double) transform.M22 + (double) coordinate.Z * (double) transform.M32) + transform.M42;
      vector4.Z = (float) ((double) coordinate.X * (double) transform.M13 + (double) coordinate.Y * (double) transform.M23 + (double) coordinate.Z * (double) transform.M33) + transform.M43;
      vector4.W = (float) (1.0 / ((double) coordinate.X * (double) transform.M14 + (double) coordinate.Y * (double) transform.M24 + (double) coordinate.Z * (double) transform.M34 + (double) transform.M44));
      result = new Vector3(vector4.X * vector4.W, vector4.Y * vector4.W, vector4.Z * vector4.W);
    }

    public static Vector3 TransformCoordinate(Vector3 coordinate, Matrix transform)
    {
      Vector3 result;
      Vector3.TransformCoordinate(ref coordinate, ref transform, out result);
      return result;
    }

    public static void TransformCoordinate(Vector3[] source, ref Matrix transform, Vector3[] destination)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index = 0; index < source.Length; ++index)
        Vector3.TransformCoordinate(ref source[index], ref transform, out destination[index]);
    }

    public static void TransformNormal(ref Vector3 normal, ref Matrix transform, out Vector3 result)
    {
      result = new Vector3((float) ((double) normal.X * (double) transform.M11 + (double) normal.Y * (double) transform.M21 + (double) normal.Z * (double) transform.M31), (float) ((double) normal.X * (double) transform.M12 + (double) normal.Y * (double) transform.M22 + (double) normal.Z * (double) transform.M32), (float) ((double) normal.X * (double) transform.M13 + (double) normal.Y * (double) transform.M23 + (double) normal.Z * (double) transform.M33));
    }

    public static Vector3 TransformNormal(Vector3 normal, Matrix transform)
    {
      Vector3 result;
      Vector3.TransformNormal(ref normal, ref transform, out result);
      return result;
    }

    public static void TransformNormal(Vector3[] source, ref Matrix transform, Vector3[] destination)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (destination.Length < source.Length)
        throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
      for (int index = 0; index < source.Length; ++index)
        Vector3.TransformNormal(ref source[index], ref transform, out destination[index]);
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
        return this.ToString(formatProvider);
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
        this.X = serializer.Reader.ReadSingle();
        this.Y = serializer.Reader.ReadSingle();
        this.Z = serializer.Reader.ReadSingle();
      }
    }

    public bool Equals(Vector3 other)
    {
      if ((double) Math.Abs(other.X - this.X) < 9.99999997475243E-07 && (double) Math.Abs(other.Y - this.Y) < 9.99999997475243E-07)
        return (double) Math.Abs(other.Z - this.Z) < 9.99999997475243E-07;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Vector3)))
        return false;
      else
        return this.Equals((Vector3) value);
    }
  }
}
