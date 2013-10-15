// Type: Microsoft.Xna.Framework.Vector4
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Text;

namespace Microsoft.Xna.Framework
{
  [Serializable]
  public struct Vector4 : IEquatable<Vector4>
  {
    private static Vector4 zeroVector = new Vector4();
    private static Vector4 unitVector = new Vector4(1f, 1f, 1f, 1f);
    private static Vector4 unitXVector = new Vector4(1f, 0.0f, 0.0f, 0.0f);
    private static Vector4 unitYVector = new Vector4(0.0f, 1f, 0.0f, 0.0f);
    private static Vector4 unitZVector = new Vector4(0.0f, 0.0f, 1f, 0.0f);
    private static Vector4 unitWVector = new Vector4(0.0f, 0.0f, 0.0f, 1f);
    public float X;
    public float Y;
    public float Z;
    public float W;

    public static Vector4 Zero
    {
      get
      {
        return Vector4.zeroVector;
      }
    }

    public static Vector4 One
    {
      get
      {
        return Vector4.unitVector;
      }
    }

    public static Vector4 UnitX
    {
      get
      {
        return Vector4.unitXVector;
      }
    }

    public static Vector4 UnitY
    {
      get
      {
        return Vector4.unitYVector;
      }
    }

    public static Vector4 UnitZ
    {
      get
      {
        return Vector4.unitZVector;
      }
    }

    public static Vector4 UnitW
    {
      get
      {
        return Vector4.unitWVector;
      }
    }

    static Vector4()
    {
    }

    public Vector4(float x, float y, float z, float w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public Vector4(Vector2 value, float z, float w)
    {
      this.X = value.X;
      this.Y = value.Y;
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

    public Vector4(float value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
      this.W = value;
    }

    public static Vector4 operator -(Vector4 value)
    {
      return new Vector4(-value.X, -value.Y, -value.Z, -value.W);
    }

    public static bool operator ==(Vector4 value1, Vector4 value2)
    {
      return (double) value1.W == (double) value2.W && (double) value1.X == (double) value2.X && (double) value1.Y == (double) value2.Y && (double) value1.Z == (double) value2.Z;
    }

    public static bool operator !=(Vector4 value1, Vector4 value2)
    {
      return !(value1 == value2);
    }

    public static Vector4 operator +(Vector4 value1, Vector4 value2)
    {
      value1.W += value2.W;
      value1.X += value2.X;
      value1.Y += value2.Y;
      value1.Z += value2.Z;
      return value1;
    }

    public static Vector4 operator -(Vector4 value1, Vector4 value2)
    {
      value1.W -= value2.W;
      value1.X -= value2.X;
      value1.Y -= value2.Y;
      value1.Z -= value2.Z;
      return value1;
    }

    public static Vector4 operator *(Vector4 value1, Vector4 value2)
    {
      value1.W *= value2.W;
      value1.X *= value2.X;
      value1.Y *= value2.Y;
      value1.Z *= value2.Z;
      return value1;
    }

    public static Vector4 operator *(Vector4 value1, float scaleFactor)
    {
      value1.W *= scaleFactor;
      value1.X *= scaleFactor;
      value1.Y *= scaleFactor;
      value1.Z *= scaleFactor;
      return value1;
    }

    public static Vector4 operator *(float scaleFactor, Vector4 value1)
    {
      value1.W *= scaleFactor;
      value1.X *= scaleFactor;
      value1.Y *= scaleFactor;
      value1.Z *= scaleFactor;
      return value1;
    }

    public static Vector4 operator /(Vector4 value1, Vector4 value2)
    {
      value1.W /= value2.W;
      value1.X /= value2.X;
      value1.Y /= value2.Y;
      value1.Z /= value2.Z;
      return value1;
    }

    public static Vector4 operator /(Vector4 value1, float divider)
    {
      float num = 1f / divider;
      value1.W *= num;
      value1.X *= num;
      value1.Y *= num;
      value1.Z *= num;
      return value1;
    }

    public static Vector4 Add(Vector4 value1, Vector4 value2)
    {
      value1.W += value2.W;
      value1.X += value2.X;
      value1.Y += value2.Y;
      value1.Z += value2.Z;
      return value1;
    }

    public static void Add(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
    {
      result.W = value1.W + value2.W;
      result.X = value1.X + value2.X;
      result.Y = value1.Y + value2.Y;
      result.Z = value1.Z + value2.Z;
    }

    public static Vector4 Barycentric(Vector4 value1, Vector4 value2, Vector4 value3, float amount1, float amount2)
    {
      return new Vector4(MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2), MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2), MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2), MathHelper.Barycentric(value1.W, value2.W, value3.W, amount1, amount2));
    }

    public static void Barycentric(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, float amount1, float amount2, out Vector4 result)
    {
      result = new Vector4(MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2), MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2), MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2), MathHelper.Barycentric(value1.W, value2.W, value3.W, amount1, amount2));
    }

    public static Vector4 CatmullRom(Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount)
    {
      return new Vector4(MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount), MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount), MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount), MathHelper.CatmullRom(value1.W, value2.W, value3.W, value4.W, amount));
    }

    public static void CatmullRom(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, ref Vector4 value4, float amount, out Vector4 result)
    {
      result = new Vector4(MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount), MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount), MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount), MathHelper.CatmullRom(value1.W, value2.W, value3.W, value4.W, amount));
    }

    public static Vector4 Clamp(Vector4 value1, Vector4 min, Vector4 max)
    {
      return new Vector4(MathHelper.Clamp(value1.X, min.X, max.X), MathHelper.Clamp(value1.Y, min.Y, max.Y), MathHelper.Clamp(value1.Z, min.Z, max.Z), MathHelper.Clamp(value1.W, min.W, max.W));
    }

    public static void Clamp(ref Vector4 value1, ref Vector4 min, ref Vector4 max, out Vector4 result)
    {
      result = new Vector4(MathHelper.Clamp(value1.X, min.X, max.X), MathHelper.Clamp(value1.Y, min.Y, max.Y), MathHelper.Clamp(value1.Z, min.Z, max.Z), MathHelper.Clamp(value1.W, min.W, max.W));
    }

    public static float Distance(Vector4 value1, Vector4 value2)
    {
      return (float) Math.Sqrt((double) Vector4.DistanceSquared(value1, value2));
    }

    public static void Distance(ref Vector4 value1, ref Vector4 value2, out float result)
    {
      result = (float) Math.Sqrt((double) Vector4.DistanceSquared(value1, value2));
    }

    public static float DistanceSquared(Vector4 value1, Vector4 value2)
    {
      float result;
      Vector4.DistanceSquared(ref value1, ref value2, out result);
      return result;
    }

    public static void DistanceSquared(ref Vector4 value1, ref Vector4 value2, out float result)
    {
      result = (float) (((double) value1.W - (double) value2.W) * ((double) value1.W - (double) value2.W) + ((double) value1.X - (double) value2.X) * ((double) value1.X - (double) value2.X) + ((double) value1.Y - (double) value2.Y) * ((double) value1.Y - (double) value2.Y) + ((double) value1.Z - (double) value2.Z) * ((double) value1.Z - (double) value2.Z));
    }

    public static Vector4 Divide(Vector4 value1, Vector4 value2)
    {
      value1.W /= value2.W;
      value1.X /= value2.X;
      value1.Y /= value2.Y;
      value1.Z /= value2.Z;
      return value1;
    }

    public static Vector4 Divide(Vector4 value1, float divider)
    {
      float num = 1f / divider;
      value1.W *= num;
      value1.X *= num;
      value1.Y *= num;
      value1.Z *= num;
      return value1;
    }

    public static void Divide(ref Vector4 value1, float divider, out Vector4 result)
    {
      float num = 1f / divider;
      result.W = value1.W * num;
      result.X = value1.X * num;
      result.Y = value1.Y * num;
      result.Z = value1.Z * num;
    }

    public static void Divide(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
    {
      result.W = value1.W / value2.W;
      result.X = value1.X / value2.X;
      result.Y = value1.Y / value2.Y;
      result.Z = value1.Z / value2.Z;
    }

    public static float Dot(Vector4 vector1, Vector4 vector2)
    {
      return (float) ((double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y + (double) vector1.Z * (double) vector2.Z + (double) vector1.W * (double) vector2.W);
    }

    public static void Dot(ref Vector4 vector1, ref Vector4 vector2, out float result)
    {
      result = (float) ((double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y + (double) vector1.Z * (double) vector2.Z + (double) vector1.W * (double) vector2.W);
    }

    public override bool Equals(object obj)
    {
      return obj is Vector4 && this == (Vector4) obj;
    }

    public bool Equals(Vector4 other)
    {
      return (double) this.W == (double) other.W && (double) this.X == (double) other.X && (double) this.Y == (double) other.Y && (double) this.Z == (double) other.Z;
    }

    public override int GetHashCode()
    {
      return (int) ((double) this.W + (double) this.X + (double) this.Y + (double) this.Y);
    }

    public static Vector4 Hermite(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
    {
      Vector4 result = new Vector4();
      Vector4.Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
      return result;
    }

    public static void Hermite(ref Vector4 value1, ref Vector4 tangent1, ref Vector4 value2, ref Vector4 tangent2, float amount, out Vector4 result)
    {
      result.W = MathHelper.Hermite(value1.W, tangent1.W, value2.W, tangent2.W, amount);
      result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
      result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
      result.Z = MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
    }

    public float Length()
    {
      float result;
      Vector4.DistanceSquared(ref this, ref Vector4.zeroVector, out result);
      return (float) Math.Sqrt((double) result);
    }

    public float LengthSquared()
    {
      float result;
      Vector4.DistanceSquared(ref this, ref Vector4.zeroVector, out result);
      return result;
    }

    public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount)
    {
      return new Vector4(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount), MathHelper.Lerp(value1.Z, value2.Z, amount), MathHelper.Lerp(value1.W, value2.W, amount));
    }

    public static void Lerp(ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result)
    {
      result = new Vector4(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount), MathHelper.Lerp(value1.Z, value2.Z, amount), MathHelper.Lerp(value1.W, value2.W, amount));
    }

    public static Vector4 Max(Vector4 value1, Vector4 value2)
    {
      return new Vector4(MathHelper.Max(value1.X, value2.X), MathHelper.Max(value1.Y, value2.Y), MathHelper.Max(value1.Z, value2.Z), MathHelper.Max(value1.W, value2.W));
    }

    public static void Max(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
    {
      result = new Vector4(MathHelper.Max(value1.X, value2.X), MathHelper.Max(value1.Y, value2.Y), MathHelper.Max(value1.Z, value2.Z), MathHelper.Max(value1.W, value2.W));
    }

    public static Vector4 Min(Vector4 value1, Vector4 value2)
    {
      return new Vector4(MathHelper.Min(value1.X, value2.X), MathHelper.Min(value1.Y, value2.Y), MathHelper.Min(value1.Z, value2.Z), MathHelper.Min(value1.W, value2.W));
    }

    public static void Min(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
    {
      result = new Vector4(MathHelper.Min(value1.X, value2.X), MathHelper.Min(value1.Y, value2.Y), MathHelper.Min(value1.Z, value2.Z), MathHelper.Min(value1.W, value2.W));
    }

    public static Vector4 Multiply(Vector4 value1, Vector4 value2)
    {
      value1.W *= value2.W;
      value1.X *= value2.X;
      value1.Y *= value2.Y;
      value1.Z *= value2.Z;
      return value1;
    }

    public static Vector4 Multiply(Vector4 value1, float scaleFactor)
    {
      value1.W *= scaleFactor;
      value1.X *= scaleFactor;
      value1.Y *= scaleFactor;
      value1.Z *= scaleFactor;
      return value1;
    }

    public static void Multiply(ref Vector4 value1, float scaleFactor, out Vector4 result)
    {
      result.W = value1.W * scaleFactor;
      result.X = value1.X * scaleFactor;
      result.Y = value1.Y * scaleFactor;
      result.Z = value1.Z * scaleFactor;
    }

    public static void Multiply(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
    {
      result.W = value1.W * value2.W;
      result.X = value1.X * value2.X;
      result.Y = value1.Y * value2.Y;
      result.Z = value1.Z * value2.Z;
    }

    public static Vector4 Negate(Vector4 value)
    {
      value = new Vector4(-value.X, -value.Y, -value.Z, -value.W);
      return value;
    }

    public static void Negate(ref Vector4 value, out Vector4 result)
    {
      result = new Vector4(-value.X, -value.Y, -value.Z, -value.W);
    }

    public void Normalize()
    {
      Vector4.Normalize(ref this, out this);
    }

    public static Vector4 Normalize(Vector4 vector)
    {
      Vector4.Normalize(ref vector, out vector);
      return vector;
    }

    public static void Normalize(ref Vector4 vector, out Vector4 result)
    {
      float result1;
      Vector4.DistanceSquared(ref vector, ref Vector4.zeroVector, out result1);
      float num = 1f / (float) Math.Sqrt((double) result1);
      result.W = vector.W * num;
      result.X = vector.X * num;
      result.Y = vector.Y * num;
      result.Z = vector.Z * num;
    }

    public static Vector4 SmoothStep(Vector4 value1, Vector4 value2, float amount)
    {
      return new Vector4(MathHelper.SmoothStep(value1.X, value2.X, amount), MathHelper.SmoothStep(value1.Y, value2.Y, amount), MathHelper.SmoothStep(value1.Z, value2.Z, amount), MathHelper.SmoothStep(value1.W, value2.W, amount));
    }

    public static void SmoothStep(ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result)
    {
      result = new Vector4(MathHelper.SmoothStep(value1.X, value2.X, amount), MathHelper.SmoothStep(value1.Y, value2.Y, amount), MathHelper.SmoothStep(value1.Z, value2.Z, amount), MathHelper.SmoothStep(value1.W, value2.W, amount));
    }

    public static Vector4 Subtract(Vector4 value1, Vector4 value2)
    {
      value1.W -= value2.W;
      value1.X -= value2.X;
      value1.Y -= value2.Y;
      value1.Z -= value2.Z;
      return value1;
    }

    public static void Subtract(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
    {
      result.W = value1.W - value2.W;
      result.X = value1.X - value2.X;
      result.Y = value1.Y - value2.Y;
      result.Z = value1.Z - value2.Z;
    }

    public static Vector4 Transform(Vector2 position, Matrix matrix)
    {
      Vector4 result;
      Vector4.Transform(ref position, ref matrix, out result);
      return result;
    }

    public static Vector4 Transform(Vector3 position, Matrix matrix)
    {
      Vector4 result;
      Vector4.Transform(ref position, ref matrix, out result);
      return result;
    }

    public static Vector4 Transform(Vector4 vector, Matrix matrix)
    {
      Vector4.Transform(ref vector, ref matrix, out vector);
      return vector;
    }

    public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector4 result)
    {
      result = new Vector4((float) ((double) position.X * (double) matrix.M11 + (double) position.Y * (double) matrix.M21) + matrix.M41, (float) ((double) position.X * (double) matrix.M12 + (double) position.Y * (double) matrix.M22) + matrix.M42, (float) ((double) position.X * (double) matrix.M13 + (double) position.Y * (double) matrix.M23) + matrix.M43, (float) ((double) position.X * (double) matrix.M14 + (double) position.Y * (double) matrix.M24) + matrix.M44);
    }

    public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector4 result)
    {
      result = new Vector4((float) ((double) position.X * (double) matrix.M11 + (double) position.Y * (double) matrix.M21 + (double) position.Z * (double) matrix.M31) + matrix.M41, (float) ((double) position.X * (double) matrix.M12 + (double) position.Y * (double) matrix.M22 + (double) position.Z * (double) matrix.M32) + matrix.M42, (float) ((double) position.X * (double) matrix.M13 + (double) position.Y * (double) matrix.M23 + (double) position.Z * (double) matrix.M33) + matrix.M43, (float) ((double) position.X * (double) matrix.M14 + (double) position.Y * (double) matrix.M24 + (double) position.Z * (double) matrix.M34) + matrix.M44);
    }

    public static void Transform(ref Vector4 vector, ref Matrix matrix, out Vector4 result)
    {
      result = new Vector4((float) ((double) vector.X * (double) matrix.M11 + (double) vector.Y * (double) matrix.M21 + (double) vector.Z * (double) matrix.M31 + (double) vector.W * (double) matrix.M41), (float) ((double) vector.X * (double) matrix.M12 + (double) vector.Y * (double) matrix.M22 + (double) vector.Z * (double) matrix.M32 + (double) vector.W * (double) matrix.M42), (float) ((double) vector.X * (double) matrix.M13 + (double) vector.Y * (double) matrix.M23 + (double) vector.Z * (double) matrix.M33 + (double) vector.W * (double) matrix.M43), (float) ((double) vector.X * (double) matrix.M14 + (double) vector.Y * (double) matrix.M24 + (double) vector.Z * (double) matrix.M34 + (double) vector.W * (double) matrix.M44));
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
      stringBuilder.Append(" W:");
      stringBuilder.Append(this.W);
      stringBuilder.Append("}");
      return ((object) stringBuilder).ToString();
    }
  }
}
