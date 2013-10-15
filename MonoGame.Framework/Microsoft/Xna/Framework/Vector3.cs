// Type: Microsoft.Xna.Framework.Vector3
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Text;

namespace Microsoft.Xna.Framework
{
  [Serializable]
  public struct Vector3 : IEquatable<Vector3>
  {
    private static Vector3 zero = new Vector3(0.0f, 0.0f, 0.0f);
    private static Vector3 one = new Vector3(1f, 1f, 1f);
    private static Vector3 unitX = new Vector3(1f, 0.0f, 0.0f);
    private static Vector3 unitY = new Vector3(0.0f, 1f, 0.0f);
    private static Vector3 unitZ = new Vector3(0.0f, 0.0f, 1f);
    private static Vector3 up = new Vector3(0.0f, 1f, 0.0f);
    private static Vector3 down = new Vector3(0.0f, -1f, 0.0f);
    private static Vector3 right = new Vector3(1f, 0.0f, 0.0f);
    private static Vector3 left = new Vector3(-1f, 0.0f, 0.0f);
    private static Vector3 forward = new Vector3(0.0f, 0.0f, -1f);
    private static Vector3 backward = new Vector3(0.0f, 0.0f, 1f);
    public float X;
    public float Y;
    public float Z;

    public static Vector3 Zero
    {
      get
      {
        return Vector3.zero;
      }
    }

    public static Vector3 One
    {
      get
      {
        return Vector3.one;
      }
    }

    public static Vector3 UnitX
    {
      get
      {
        return Vector3.unitX;
      }
    }

    public static Vector3 UnitY
    {
      get
      {
        return Vector3.unitY;
      }
    }

    public static Vector3 UnitZ
    {
      get
      {
        return Vector3.unitZ;
      }
    }

    public static Vector3 Up
    {
      get
      {
        return Vector3.up;
      }
    }

    public static Vector3 Down
    {
      get
      {
        return Vector3.down;
      }
    }

    public static Vector3 Right
    {
      get
      {
        return Vector3.right;
      }
    }

    public static Vector3 Left
    {
      get
      {
        return Vector3.left;
      }
    }

    public static Vector3 Forward
    {
      get
      {
        return Vector3.forward;
      }
    }

    public static Vector3 Backward
    {
      get
      {
        return Vector3.backward;
      }
    }

    static Vector3()
    {
    }

    public Vector3(float x, float y, float z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Vector3(float value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
    }

    public Vector3(Vector2 value, float z)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = z;
    }

    public static bool operator ==(Vector3 value1, Vector3 value2)
    {
      if ((double) value1.X == (double) value2.X && (double) value1.Y == (double) value2.Y)
        return (double) value1.Z == (double) value2.Z;
      else
        return false;
    }

    public static bool operator !=(Vector3 value1, Vector3 value2)
    {
      return !(value1 == value2);
    }

    public static Vector3 operator +(Vector3 value1, Vector3 value2)
    {
      value1.X += value2.X;
      value1.Y += value2.Y;
      value1.Z += value2.Z;
      return value1;
    }

    public static Vector3 operator -(Vector3 value)
    {
      value = new Vector3(-value.X, -value.Y, -value.Z);
      return value;
    }

    public static Vector3 operator -(Vector3 value1, Vector3 value2)
    {
      value1.X -= value2.X;
      value1.Y -= value2.Y;
      value1.Z -= value2.Z;
      return value1;
    }

    public static Vector3 operator *(Vector3 value1, Vector3 value2)
    {
      value1.X *= value2.X;
      value1.Y *= value2.Y;
      value1.Z *= value2.Z;
      return value1;
    }

    public static Vector3 operator *(Vector3 value, float scaleFactor)
    {
      value.X *= scaleFactor;
      value.Y *= scaleFactor;
      value.Z *= scaleFactor;
      return value;
    }

    public static Vector3 operator *(float scaleFactor, Vector3 value)
    {
      value.X *= scaleFactor;
      value.Y *= scaleFactor;
      value.Z *= scaleFactor;
      return value;
    }

    public static Vector3 operator /(Vector3 value1, Vector3 value2)
    {
      value1.X /= value2.X;
      value1.Y /= value2.Y;
      value1.Z /= value2.Z;
      return value1;
    }

    public static Vector3 operator /(Vector3 value, float divider)
    {
      float num = 1f / divider;
      value.X *= num;
      value.Y *= num;
      value.Z *= num;
      return value;
    }

    public static Vector3 Add(Vector3 value1, Vector3 value2)
    {
      value1.X += value2.X;
      value1.Y += value2.Y;
      value1.Z += value2.Z;
      return value1;
    }

    public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
      result.X = value1.X + value2.X;
      result.Y = value1.Y + value2.Y;
      result.Z = value1.Z + value2.Z;
    }

    public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
    {
      return new Vector3(MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2), MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2), MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2));
    }

    public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result)
    {
      result = new Vector3(MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2), MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2), MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2));
    }

    public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
    {
      return new Vector3(MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount), MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount), MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount));
    }

    public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
    {
      result = new Vector3(MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount), MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount), MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount));
    }

    public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
    {
      return new Vector3(MathHelper.Clamp(value1.X, min.X, max.X), MathHelper.Clamp(value1.Y, min.Y, max.Y), MathHelper.Clamp(value1.Z, min.Z, max.Z));
    }

    public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
    {
      result = new Vector3(MathHelper.Clamp(value1.X, min.X, max.X), MathHelper.Clamp(value1.Y, min.Y, max.Y), MathHelper.Clamp(value1.Z, min.Z, max.Z));
    }

    public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
    {
      Vector3.Cross(ref vector1, ref vector2, out vector1);
      return vector1;
    }

    public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
    {
      result = new Vector3((float) ((double) vector1.Y * (double) vector2.Z - (double) vector2.Y * (double) vector1.Z), (float) -((double) vector1.X * (double) vector2.Z - (double) vector2.X * (double) vector1.Z), (float) ((double) vector1.X * (double) vector2.Y - (double) vector2.X * (double) vector1.Y));
    }

    public static float Distance(Vector3 vector1, Vector3 vector2)
    {
      float result;
      Vector3.DistanceSquared(ref vector1, ref vector2, out result);
      return (float) Math.Sqrt((double) result);
    }

    public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
    {
      Vector3.DistanceSquared(ref value1, ref value2, out result);
      result = (float) Math.Sqrt((double) result);
    }

    public static float DistanceSquared(Vector3 value1, Vector3 value2)
    {
      float result;
      Vector3.DistanceSquared(ref value1, ref value2, out result);
      return result;
    }

    public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
    {
      result = (float) (((double) value1.X - (double) value2.X) * ((double) value1.X - (double) value2.X) + ((double) value1.Y - (double) value2.Y) * ((double) value1.Y - (double) value2.Y) + ((double) value1.Z - (double) value2.Z) * ((double) value1.Z - (double) value2.Z));
    }

    public static Vector3 Divide(Vector3 value1, Vector3 value2)
    {
      value1.X /= value2.X;
      value1.Y /= value2.Y;
      value1.Z /= value2.Z;
      return value1;
    }

    public static Vector3 Divide(Vector3 value1, float value2)
    {
      float num = 1f / value2;
      value1.X *= num;
      value1.Y *= num;
      value1.Z *= num;
      return value1;
    }

    public static void Divide(ref Vector3 value1, float divisor, out Vector3 result)
    {
      float num = 1f / divisor;
      result.X = value1.X * num;
      result.Y = value1.Y * num;
      result.Z = value1.Z * num;
    }

    public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
      result.X = value1.X / value2.X;
      result.Y = value1.Y / value2.Y;
      result.Z = value1.Z / value2.Z;
    }

    public static float Dot(Vector3 vector1, Vector3 vector2)
    {
      return (float) ((double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y + (double) vector1.Z * (double) vector2.Z);
    }

    public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out float result)
    {
      result = (float) ((double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y + (double) vector1.Z * (double) vector2.Z);
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Vector3))
        return false;
      else
        return this == (Vector3) obj;
    }

    public bool Equals(Vector3 other)
    {
      return this == other;
    }

    public override int GetHashCode()
    {
      return (int) ((double) this.X + (double) this.Y + (double) this.Z);
    }

    public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
    {
      Vector3 result = new Vector3();
      Vector3.Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
      return result;
    }

    public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
    {
      result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
      result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
      result.Z = MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
    }

    public float Length()
    {
      float result;
      Vector3.DistanceSquared(ref this, ref Vector3.zero, out result);
      return (float) Math.Sqrt((double) result);
    }

    public float LengthSquared()
    {
      float result;
      Vector3.DistanceSquared(ref this, ref Vector3.zero, out result);
      return result;
    }

    public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
    {
      return new Vector3(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount), MathHelper.Lerp(value1.Z, value2.Z, amount));
    }

    public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
    {
      result = new Vector3(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount), MathHelper.Lerp(value1.Z, value2.Z, amount));
    }

    public static Vector3 Max(Vector3 value1, Vector3 value2)
    {
      return new Vector3(MathHelper.Max(value1.X, value2.X), MathHelper.Max(value1.Y, value2.Y), MathHelper.Max(value1.Z, value2.Z));
    }

    public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
      result = new Vector3(MathHelper.Max(value1.X, value2.X), MathHelper.Max(value1.Y, value2.Y), MathHelper.Max(value1.Z, value2.Z));
    }

    public static Vector3 Min(Vector3 value1, Vector3 value2)
    {
      return new Vector3(MathHelper.Min(value1.X, value2.X), MathHelper.Min(value1.Y, value2.Y), MathHelper.Min(value1.Z, value2.Z));
    }

    public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
      result = new Vector3(MathHelper.Min(value1.X, value2.X), MathHelper.Min(value1.Y, value2.Y), MathHelper.Min(value1.Z, value2.Z));
    }

    public static Vector3 Multiply(Vector3 value1, Vector3 value2)
    {
      value1.X *= value2.X;
      value1.Y *= value2.Y;
      value1.Z *= value2.Z;
      return value1;
    }

    public static Vector3 Multiply(Vector3 value1, float scaleFactor)
    {
      value1.X *= scaleFactor;
      value1.Y *= scaleFactor;
      value1.Z *= scaleFactor;
      return value1;
    }

    public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
    {
      result.X = value1.X * scaleFactor;
      result.Y = value1.Y * scaleFactor;
      result.Z = value1.Z * scaleFactor;
    }

    public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
      result.X = value1.X * value2.X;
      result.Y = value1.Y * value2.Y;
      result.Z = value1.Z * value2.Z;
    }

    public static Vector3 Negate(Vector3 value)
    {
      value = new Vector3(-value.X, -value.Y, -value.Z);
      return value;
    }

    public static void Negate(ref Vector3 value, out Vector3 result)
    {
      result = new Vector3(-value.X, -value.Y, -value.Z);
    }

    public void Normalize()
    {
      Vector3.Normalize(ref this, out this);
    }

    public static Vector3 Normalize(Vector3 vector)
    {
      Vector3.Normalize(ref vector, out vector);
      return vector;
    }

    public static void Normalize(ref Vector3 value, out Vector3 result)
    {
      float result1;
      Vector3.Distance(ref value, ref Vector3.zero, out result1);
      float num = 1f / result1;
      result.X = value.X * num;
      result.Y = value.Y * num;
      result.Z = value.Z * num;
    }

    public static Vector3 Reflect(Vector3 vector, Vector3 normal)
    {
      float num = (float) ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y + (double) vector.Z * (double) normal.Z);
      Vector3 vector3;
      vector3.X = vector.X - 2f * normal.X * num;
      vector3.Y = vector.Y - 2f * normal.Y * num;
      vector3.Z = vector.Z - 2f * normal.Z * num;
      return vector3;
    }

    public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
    {
      float num = (float) ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y + (double) vector.Z * (double) normal.Z);
      result.X = vector.X - 2f * normal.X * num;
      result.Y = vector.Y - 2f * normal.Y * num;
      result.Z = vector.Z - 2f * normal.Z * num;
    }

    public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
    {
      return new Vector3(MathHelper.SmoothStep(value1.X, value2.X, amount), MathHelper.SmoothStep(value1.Y, value2.Y, amount), MathHelper.SmoothStep(value1.Z, value2.Z, amount));
    }

    public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
    {
      result = new Vector3(MathHelper.SmoothStep(value1.X, value2.X, amount), MathHelper.SmoothStep(value1.Y, value2.Y, amount), MathHelper.SmoothStep(value1.Z, value2.Z, amount));
    }

    public static Vector3 Subtract(Vector3 value1, Vector3 value2)
    {
      value1.X -= value2.X;
      value1.Y -= value2.Y;
      value1.Z -= value2.Z;
      return value1;
    }

    public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
      result.X = value1.X - value2.X;
      result.Y = value1.Y - value2.Y;
      result.Z = value1.Z - value2.Z;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(32);
      stringBuilder.Append("{X:");
      stringBuilder.Append(this.X);
      stringBuilder.Append(" Y:");
      stringBuilder.Append(this.Y);
      stringBuilder.Append(" Z:");
      stringBuilder.Append(this.Z);
      stringBuilder.Append("}");
      return ((object) stringBuilder).ToString();
    }

    public static Vector3 Transform(Vector3 position, Matrix matrix)
    {
      Vector3.Transform(ref position, ref matrix, out position);
      return position;
    }

    public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector3 result)
    {
      result = new Vector3((float) ((double) position.X * (double) matrix.M11 + (double) position.Y * (double) matrix.M21 + (double) position.Z * (double) matrix.M31) + matrix.M41, (float) ((double) position.X * (double) matrix.M12 + (double) position.Y * (double) matrix.M22 + (double) position.Z * (double) matrix.M32) + matrix.M42, (float) ((double) position.X * (double) matrix.M13 + (double) position.Y * (double) matrix.M23 + (double) position.Z * (double) matrix.M33) + matrix.M43);
    }

    public static void Transform(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        Vector3 vector3 = sourceArray[index];
        destinationArray[index] = new Vector3((float) ((double) vector3.X * (double) matrix.M11 + (double) vector3.Y * (double) matrix.M21 + (double) vector3.Z * (double) matrix.M31) + matrix.M41, (float) ((double) vector3.X * (double) matrix.M12 + (double) vector3.Y * (double) matrix.M22 + (double) vector3.Z * (double) matrix.M32) + matrix.M42, (float) ((double) vector3.X * (double) matrix.M13 + (double) vector3.Y * (double) matrix.M23 + (double) vector3.Z * (double) matrix.M33) + matrix.M43);
      }
    }

    public static Vector3 Transform(Vector3 vec, Quaternion quat)
    {
      Vector3 result;
      Vector3.Transform(ref vec, ref quat, out result);
      return result;
    }

    public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector3 result)
    {
      float num1 = (float) (2.0 * ((double) rotation.Y * (double) value.Z - (double) rotation.Z * (double) value.Y));
      float num2 = (float) (2.0 * ((double) rotation.Z * (double) value.X - (double) rotation.X * (double) value.Z));
      float num3 = (float) (2.0 * ((double) rotation.X * (double) value.Y - (double) rotation.Y * (double) value.X));
      result.X = (float) ((double) value.X + (double) num1 * (double) rotation.W + ((double) rotation.Y * (double) num3 - (double) rotation.Z * (double) num2));
      result.Y = (float) ((double) value.Y + (double) num2 * (double) rotation.W + ((double) rotation.Z * (double) num1 - (double) rotation.X * (double) num3));
      result.Z = (float) ((double) value.Z + (double) num3 * (double) rotation.W + ((double) rotation.X * (double) num2 - (double) rotation.Y * (double) num1));
    }

    public static Vector3 TransformNormal(Vector3 normal, Matrix matrix)
    {
      Vector3.TransformNormal(ref normal, ref matrix, out normal);
      return normal;
    }

    public static void TransformNormal(ref Vector3 normal, ref Matrix matrix, out Vector3 result)
    {
      result = new Vector3((float) ((double) normal.X * (double) matrix.M11 + (double) normal.Y * (double) matrix.M21 + (double) normal.Z * (double) matrix.M31), (float) ((double) normal.X * (double) matrix.M12 + (double) normal.Y * (double) matrix.M22 + (double) normal.Z * (double) matrix.M32), (float) ((double) normal.X * (double) matrix.M13 + (double) normal.Y * (double) matrix.M23 + (double) normal.Z * (double) matrix.M33));
    }
  }
}
