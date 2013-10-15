// Type: Microsoft.Xna.Framework.Vector2
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Globalization;

namespace Microsoft.Xna.Framework
{
  [Serializable]
  public struct Vector2 : IEquatable<Vector2>
  {
    private static Vector2 zeroVector = new Vector2(0.0f, 0.0f);
    private static Vector2 unitVector = new Vector2(1f, 1f);
    private static Vector2 unitXVector = new Vector2(1f, 0.0f);
    private static Vector2 unitYVector = new Vector2(0.0f, 1f);
    public float X;
    public float Y;

    public static Vector2 Zero
    {
      get
      {
        return Vector2.zeroVector;
      }
    }

    public static Vector2 One
    {
      get
      {
        return Vector2.unitVector;
      }
    }

    public static Vector2 UnitX
    {
      get
      {
        return Vector2.unitXVector;
      }
    }

    public static Vector2 UnitY
    {
      get
      {
        return Vector2.unitYVector;
      }
    }

    static Vector2()
    {
    }

    public Vector2(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }

    public Vector2(float value)
    {
      this.X = value;
      this.Y = value;
    }

    public static Vector2 operator -(Vector2 value)
    {
      value.X = -value.X;
      value.Y = -value.Y;
      return value;
    }

    public static bool operator ==(Vector2 value1, Vector2 value2)
    {
      return (double) value1.X == (double) value2.X && (double) value1.Y == (double) value2.Y;
    }

    public static bool operator !=(Vector2 value1, Vector2 value2)
    {
      return (double) value1.X != (double) value2.X || (double) value1.Y != (double) value2.Y;
    }

    public static Vector2 operator +(Vector2 value1, Vector2 value2)
    {
      value1.X += value2.X;
      value1.Y += value2.Y;
      return value1;
    }

    public static Vector2 operator -(Vector2 value1, Vector2 value2)
    {
      value1.X -= value2.X;
      value1.Y -= value2.Y;
      return value1;
    }

    public static Vector2 operator *(Vector2 value1, Vector2 value2)
    {
      value1.X *= value2.X;
      value1.Y *= value2.Y;
      return value1;
    }

    public static Vector2 operator *(Vector2 value, float scaleFactor)
    {
      value.X *= scaleFactor;
      value.Y *= scaleFactor;
      return value;
    }

    public static Vector2 operator *(float scaleFactor, Vector2 value)
    {
      value.X *= scaleFactor;
      value.Y *= scaleFactor;
      return value;
    }

    public static Vector2 operator /(Vector2 value1, Vector2 value2)
    {
      value1.X /= value2.X;
      value1.Y /= value2.Y;
      return value1;
    }

    public static Vector2 operator /(Vector2 value1, float divider)
    {
      float num = 1f / divider;
      value1.X *= num;
      value1.Y *= num;
      return value1;
    }

    public static Vector2 Add(Vector2 value1, Vector2 value2)
    {
      value1.X += value2.X;
      value1.Y += value2.Y;
      return value1;
    }

    public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = value1.X + value2.X;
      result.Y = value1.Y + value2.Y;
    }

    public static Vector2 Barycentric(Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2)
    {
      return new Vector2(MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2), MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
    }

    public static void Barycentric(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result)
    {
      result = new Vector2(MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2), MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
    }

    public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
    {
      return new Vector2(MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount), MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
    }

    public static void CatmullRom(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result)
    {
      result = new Vector2(MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount), MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
    }

    public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
    {
      return new Vector2(MathHelper.Clamp(value1.X, min.X, max.X), MathHelper.Clamp(value1.Y, min.Y, max.Y));
    }

    public static void Clamp(ref Vector2 value1, ref Vector2 min, ref Vector2 max, out Vector2 result)
    {
      result = new Vector2(MathHelper.Clamp(value1.X, min.X, max.X), MathHelper.Clamp(value1.Y, min.Y, max.Y));
    }

    public static float Distance(Vector2 value1, Vector2 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      result = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static float DistanceSquared(Vector2 value1, Vector2 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      result = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static Vector2 Divide(Vector2 value1, Vector2 value2)
    {
      value1.X /= value2.X;
      value1.Y /= value2.Y;
      return value1;
    }

    public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = value1.X / value2.X;
      result.Y = value1.Y / value2.Y;
    }

    public static Vector2 Divide(Vector2 value1, float divider)
    {
      float num = 1f / divider;
      value1.X *= num;
      value1.Y *= num;
      return value1;
    }

    public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
    {
      float num = 1f / divider;
      result.X = value1.X * num;
      result.Y = value1.Y * num;
    }

    public static float Dot(Vector2 value1, Vector2 value2)
    {
      return (float) ((double) value1.X * (double) value2.X + (double) value1.Y * (double) value2.Y);
    }

    public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result)
    {
      result = (float) ((double) value1.X * (double) value2.X + (double) value1.Y * (double) value2.Y);
    }

    public override bool Equals(object obj)
    {
      if (obj is Vector2)
        return this.Equals(this);
      else
        return false;
    }

    public bool Equals(Vector2 other)
    {
      return (double) this.X == (double) other.X && (double) this.Y == (double) other.Y;
    }

    public static Vector2 Reflect(Vector2 vector, Vector2 normal)
    {
      float num = (float) (2.0 * ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y));
      Vector2 vector2;
      vector2.X = vector.X - normal.X * num;
      vector2.Y = vector.Y - normal.Y * num;
      return vector2;
    }

    public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
    {
      float num = (float) (2.0 * ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y));
      result.X = vector.X - normal.X * num;
      result.Y = vector.Y - normal.Y * num;
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() + this.Y.GetHashCode();
    }

    public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
    {
      Vector2 result = new Vector2();
      Vector2.Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
      return result;
    }

    public static void Hermite(ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result)
    {
      result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
      result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
    }

    public float Length()
    {
      return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
    }

    public float LengthSquared()
    {
      return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
    }

    public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
    {
      return new Vector2(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount));
    }

    public static void Lerp(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
    {
      result = new Vector2(MathHelper.Lerp(value1.X, value2.X, amount), MathHelper.Lerp(value1.Y, value2.Y, amount));
    }

    public static Vector2 Max(Vector2 value1, Vector2 value2)
    {
      return new Vector2((double) value1.X > (double) value2.X ? value1.X : value2.X, (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y);
    }

    public static void Max(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = (double) value1.X > (double) value2.X ? value1.X : value2.X;
      result.Y = (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y;
    }

    public static Vector2 Min(Vector2 value1, Vector2 value2)
    {
      return new Vector2((double) value1.X < (double) value2.X ? value1.X : value2.X, (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y);
    }

    public static void Min(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = (double) value1.X < (double) value2.X ? value1.X : value2.X;
      result.Y = (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y;
    }

    public static Vector2 Multiply(Vector2 value1, Vector2 value2)
    {
      value1.X *= value2.X;
      value1.Y *= value2.Y;
      return value1;
    }

    public static Vector2 Multiply(Vector2 value1, float scaleFactor)
    {
      value1.X *= scaleFactor;
      value1.Y *= scaleFactor;
      return value1;
    }

    public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
    {
      result.X = value1.X * scaleFactor;
      result.Y = value1.Y * scaleFactor;
    }

    public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = value1.X * value2.X;
      result.Y = value1.Y * value2.Y;
    }

    public static Vector2 Negate(Vector2 value)
    {
      value.X = -value.X;
      value.Y = -value.Y;
      return value;
    }

    public static void Negate(ref Vector2 value, out Vector2 result)
    {
      result.X = -value.X;
      result.Y = -value.Y;
    }

    public void Normalize()
    {
      float num = 1f / (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
      this.X *= num;
      this.Y *= num;
    }

    public static Vector2 Normalize(Vector2 value)
    {
      float num = 1f / (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y);
      value.X *= num;
      value.Y *= num;
      return value;
    }

    public static void Normalize(ref Vector2 value, out Vector2 result)
    {
      float num = 1f / (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y);
      result.X = value.X * num;
      result.Y = value.Y * num;
    }

    public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
    {
      return new Vector2(MathHelper.SmoothStep(value1.X, value2.X, amount), MathHelper.SmoothStep(value1.Y, value2.Y, amount));
    }

    public static void SmoothStep(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
    {
      result = new Vector2(MathHelper.SmoothStep(value1.X, value2.X, amount), MathHelper.SmoothStep(value1.Y, value2.Y, amount));
    }

    public static Vector2 Subtract(Vector2 value1, Vector2 value2)
    {
      value1.X -= value2.X;
      value1.Y -= value2.Y;
      return value1;
    }

    public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = value1.X - value2.X;
      result.Y = value1.Y - value2.Y;
    }

    public static Vector2 Transform(Vector2 position, Matrix matrix)
    {
      Vector2.Transform(ref position, ref matrix, out position);
      return position;
    }

    public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector2 result)
    {
      result = new Vector2((float) ((double) position.X * (double) matrix.M11 + (double) position.Y * (double) matrix.M21) + matrix.M41, (float) ((double) position.X * (double) matrix.M12 + (double) position.Y * (double) matrix.M22) + matrix.M42);
    }

    public static Vector2 Transform(Vector2 position, Quaternion quat)
    {
      Vector2.Transform(ref position, ref quat, out position);
      return position;
    }

    public static void Transform(ref Vector2 position, ref Quaternion quat, out Vector2 result)
    {
      Quaternion result1 = new Quaternion(position.X, position.Y, 0.0f, 0.0f);
      Quaternion result2;
      Quaternion.Inverse(ref quat, out result2);
      Quaternion result3;
      Quaternion.Multiply(ref quat, ref result1, out result3);
      Quaternion.Multiply(ref result3, ref result2, out result1);
      result = new Vector2(result1.X, result1.Y);
    }

    public static void Transform(Vector2[] sourceArray, ref Matrix matrix, Vector2[] destinationArray)
    {
      Vector2.Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
    }

    public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2[] destinationArray, int destinationIndex, int length)
    {
      for (int index = 0; index < length; ++index)
      {
        Vector2 vector2_1 = sourceArray[sourceIndex + index];
        Vector2 vector2_2 = destinationArray[destinationIndex + index];
        vector2_2.X = (float) ((double) vector2_1.X * (double) matrix.M11 + (double) vector2_1.Y * (double) matrix.M21) + matrix.M41;
        vector2_2.Y = (float) ((double) vector2_1.X * (double) matrix.M12 + (double) vector2_1.Y * (double) matrix.M22) + matrix.M42;
        destinationArray[destinationIndex + index] = vector2_2;
      }
    }

    public static Vector2 TransformNormal(Vector2 normal, Matrix matrix)
    {
      Vector2.TransformNormal(ref normal, ref matrix, out normal);
      return normal;
    }

    public static void TransformNormal(ref Vector2 normal, ref Matrix matrix, out Vector2 result)
    {
      result = new Vector2((float) ((double) normal.X * (double) matrix.M11 + (double) normal.Y * (double) matrix.M21), (float) ((double) normal.X * (double) matrix.M12 + (double) normal.Y * (double) matrix.M22));
    }

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return string.Format((IFormatProvider) currentCulture, "{{X:{0} Y:{1}}}", new object[2]
      {
        (object) this.X.ToString((IFormatProvider) currentCulture),
        (object) this.Y.ToString((IFormatProvider) currentCulture)
      });
    }
  }
}
