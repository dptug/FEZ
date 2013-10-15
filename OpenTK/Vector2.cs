// Type: OpenTK.Vector2
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK
{
  [Serializable]
  public struct Vector2 : IEquatable<Vector2>
  {
    public static readonly Vector2 UnitX = new Vector2(1f, 0.0f);
    public static readonly Vector2 UnitY = new Vector2(0.0f, 1f);
    public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);
    public static readonly Vector2 One = new Vector2(1f, 1f);
    public static readonly int SizeInBytes = Marshal.SizeOf((object) new Vector2());
    public float X;
    public float Y;

    public float Length
    {
      get
      {
        return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
      }
    }

    public float LengthFast
    {
      get
      {
        return 1f / MathHelper.InverseSqrtFast((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y));
      }
    }

    public float LengthSquared
    {
      get
      {
        return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
      }
    }

    public Vector2 PerpendicularRight
    {
      get
      {
        return new Vector2(this.Y, -this.X);
      }
    }

    public Vector2 PerpendicularLeft
    {
      get
      {
        return new Vector2(-this.Y, this.X);
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

    [Obsolete]
    public Vector2(Vector2 v)
    {
      this.X = v.X;
      this.Y = v.Y;
    }

    [Obsolete]
    public Vector2(Vector3 v)
    {
      this.X = v.X;
      this.Y = v.Y;
    }

    [Obsolete]
    public Vector2(Vector4 v)
    {
      this.X = v.X;
      this.Y = v.Y;
    }

    public static Vector2 operator +(Vector2 left, Vector2 right)
    {
      left.X += right.X;
      left.Y += right.Y;
      return left;
    }

    public static Vector2 operator -(Vector2 left, Vector2 right)
    {
      left.X -= right.X;
      left.Y -= right.Y;
      return left;
    }

    public static Vector2 operator -(Vector2 vec)
    {
      vec.X = -vec.X;
      vec.Y = -vec.Y;
      return vec;
    }

    public static Vector2 operator *(Vector2 vec, float scale)
    {
      vec.X *= scale;
      vec.Y *= scale;
      return vec;
    }

    public static Vector2 operator *(float scale, Vector2 vec)
    {
      vec.X *= scale;
      vec.Y *= scale;
      return vec;
    }

    public static Vector2 operator /(Vector2 vec, float scale)
    {
      float num = 1f / scale;
      vec.X *= num;
      vec.Y *= num;
      return vec;
    }

    public static bool operator ==(Vector2 left, Vector2 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Vector2 left, Vector2 right)
    {
      return !left.Equals(right);
    }

    [Obsolete("Use static Add() method instead.")]
    public void Add(Vector2 right)
    {
      this.X += right.X;
      this.Y += right.Y;
    }

    [CLSCompliant(false)]
    [Obsolete("Use static Add() method instead.")]
    public void Add(ref Vector2 right)
    {
      this.X += right.X;
      this.Y += right.Y;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public void Sub(Vector2 right)
    {
      this.X -= right.X;
      this.Y -= right.Y;
    }

    [CLSCompliant(false)]
    [Obsolete("Use static Subtract() method instead.")]
    public void Sub(ref Vector2 right)
    {
      this.X -= right.X;
      this.Y -= right.Y;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Mult(float f)
    {
      this.X *= f;
      this.Y *= f;
    }

    [Obsolete("Use static Divide() method instead.")]
    public void Div(float f)
    {
      float num = 1f / f;
      this.X *= num;
      this.Y *= num;
    }

    public void Normalize()
    {
      float num = 1f / this.Length;
      this.X *= num;
      this.Y *= num;
    }

    public void NormalizeFast()
    {
      float num = MathHelper.InverseSqrtFast((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y));
      this.X *= num;
      this.Y *= num;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(float sx, float sy)
    {
      this.X = this.X * sx;
      this.Y = this.Y * sy;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(Vector2 scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
    }

    [Obsolete("Use static Multiply() method instead.")]
    [CLSCompliant(false)]
    public void Scale(ref Vector2 scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public static Vector2 Sub(Vector2 a, Vector2 b)
    {
      a.X -= b.X;
      a.Y -= b.Y;
      return a;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public static void Sub(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
      result.X = a.X - b.X;
      result.Y = a.Y - b.Y;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public static Vector2 Mult(Vector2 a, float f)
    {
      a.X *= f;
      a.Y *= f;
      return a;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public static void Mult(ref Vector2 a, float f, out Vector2 result)
    {
      result.X = a.X * f;
      result.Y = a.Y * f;
    }

    [Obsolete("Use static Divide() method instead.")]
    public static Vector2 Div(Vector2 a, float f)
    {
      float num = 1f / f;
      a.X *= num;
      a.Y *= num;
      return a;
    }

    [Obsolete("Use static Divide() method instead.")]
    public static void Div(ref Vector2 a, float f, out Vector2 result)
    {
      float num = 1f / f;
      result.X = a.X * num;
      result.Y = a.Y * num;
    }

    public static Vector2 Add(Vector2 a, Vector2 b)
    {
      Vector2.Add(ref a, ref b, out a);
      return a;
    }

    public static void Add(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
      result = new Vector2(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 Subtract(Vector2 a, Vector2 b)
    {
      Vector2.Subtract(ref a, ref b, out a);
      return a;
    }

    public static void Subtract(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
      result = new Vector2(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 Multiply(Vector2 vector, float scale)
    {
      Vector2.Multiply(ref vector, scale, out vector);
      return vector;
    }

    public static void Multiply(ref Vector2 vector, float scale, out Vector2 result)
    {
      result = new Vector2(vector.X * scale, vector.Y * scale);
    }

    public static Vector2 Multiply(Vector2 vector, Vector2 scale)
    {
      Vector2.Multiply(ref vector, ref scale, out vector);
      return vector;
    }

    public static void Multiply(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
    {
      result = new Vector2(vector.X * scale.X, vector.Y * scale.Y);
    }

    public static Vector2 Divide(Vector2 vector, float scale)
    {
      Vector2.Divide(ref vector, scale, out vector);
      return vector;
    }

    public static void Divide(ref Vector2 vector, float scale, out Vector2 result)
    {
      Vector2.Multiply(ref vector, 1f / scale, out result);
    }

    public static Vector2 Divide(Vector2 vector, Vector2 scale)
    {
      Vector2.Divide(ref vector, ref scale, out vector);
      return vector;
    }

    public static void Divide(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
    {
      result = new Vector2(vector.X / scale.X, vector.Y / scale.Y);
    }

    public static Vector2 ComponentMin(Vector2 a, Vector2 b)
    {
      a.X = (double) a.X < (double) b.X ? a.X : b.X;
      a.Y = (double) a.Y < (double) b.Y ? a.Y : b.Y;
      return a;
    }

    public static void ComponentMin(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
      result.X = (double) a.X < (double) b.X ? a.X : b.X;
      result.Y = (double) a.Y < (double) b.Y ? a.Y : b.Y;
    }

    public static Vector2 ComponentMax(Vector2 a, Vector2 b)
    {
      a.X = (double) a.X > (double) b.X ? a.X : b.X;
      a.Y = (double) a.Y > (double) b.Y ? a.Y : b.Y;
      return a;
    }

    public static void ComponentMax(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
      result.X = (double) a.X > (double) b.X ? a.X : b.X;
      result.Y = (double) a.Y > (double) b.Y ? a.Y : b.Y;
    }

    public static Vector2 Min(Vector2 left, Vector2 right)
    {
      if ((double) left.LengthSquared >= (double) right.LengthSquared)
        return right;
      else
        return left;
    }

    public static Vector2 Max(Vector2 left, Vector2 right)
    {
      if ((double) left.LengthSquared < (double) right.LengthSquared)
        return right;
      else
        return left;
    }

    public static Vector2 Clamp(Vector2 vec, Vector2 min, Vector2 max)
    {
      vec.X = (double) vec.X < (double) min.X ? min.X : ((double) vec.X > (double) max.X ? max.X : vec.X);
      vec.Y = (double) vec.Y < (double) min.Y ? min.Y : ((double) vec.Y > (double) max.Y ? max.Y : vec.Y);
      return vec;
    }

    public static void Clamp(ref Vector2 vec, ref Vector2 min, ref Vector2 max, out Vector2 result)
    {
      result.X = (double) vec.X < (double) min.X ? min.X : ((double) vec.X > (double) max.X ? max.X : vec.X);
      result.Y = (double) vec.Y < (double) min.Y ? min.Y : ((double) vec.Y > (double) max.Y ? max.Y : vec.Y);
    }

    public static Vector2 Normalize(Vector2 vec)
    {
      float num = 1f / vec.Length;
      vec.X *= num;
      vec.Y *= num;
      return vec;
    }

    public static void Normalize(ref Vector2 vec, out Vector2 result)
    {
      float num = 1f / vec.Length;
      result.X = vec.X * num;
      result.Y = vec.Y * num;
    }

    public static Vector2 NormalizeFast(Vector2 vec)
    {
      float num = MathHelper.InverseSqrtFast((float) ((double) vec.X * (double) vec.X + (double) vec.Y * (double) vec.Y));
      vec.X *= num;
      vec.Y *= num;
      return vec;
    }

    public static void NormalizeFast(ref Vector2 vec, out Vector2 result)
    {
      float num = MathHelper.InverseSqrtFast((float) ((double) vec.X * (double) vec.X + (double) vec.Y * (double) vec.Y));
      result.X = vec.X * num;
      result.Y = vec.Y * num;
    }

    public static float Dot(Vector2 left, Vector2 right)
    {
      return (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y);
    }

    public static void Dot(ref Vector2 left, ref Vector2 right, out float result)
    {
      result = (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y);
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float blend)
    {
      a.X = blend * (b.X - a.X) + a.X;
      a.Y = blend * (b.Y - a.Y) + a.Y;
      return a;
    }

    public static void Lerp(ref Vector2 a, ref Vector2 b, float blend, out Vector2 result)
    {
      result.X = blend * (b.X - a.X) + a.X;
      result.Y = blend * (b.Y - a.Y) + a.Y;
    }

    public static Vector2 BaryCentric(Vector2 a, Vector2 b, Vector2 c, float u, float v)
    {
      return a + u * (b - a) + v * (c - a);
    }

    public static void BaryCentric(ref Vector2 a, ref Vector2 b, ref Vector2 c, float u, float v, out Vector2 result)
    {
      result = a;
      Vector2 result1 = b;
      Vector2.Subtract(ref result1, ref a, out result1);
      Vector2.Multiply(ref result1, u, out result1);
      Vector2.Add(ref result, ref result1, out result);
      Vector2 result2 = c;
      Vector2.Subtract(ref result2, ref a, out result2);
      Vector2.Multiply(ref result2, v, out result2);
      Vector2.Add(ref result, ref result2, out result);
    }

    public static Vector2 Transform(Vector2 vec, Quaternion quat)
    {
      Vector2 result;
      Vector2.Transform(ref vec, ref quat, out result);
      return result;
    }

    public static void Transform(ref Vector2 vec, ref Quaternion quat, out Vector2 result)
    {
      Quaternion result1 = new Quaternion(vec.X, vec.Y, 0.0f, 0.0f);
      Quaternion result2;
      Quaternion.Invert(ref quat, out result2);
      Quaternion result3;
      Quaternion.Multiply(ref quat, ref result1, out result3);
      Quaternion.Multiply(ref result3, ref result2, out result1);
      result = new Vector2(result1.X, result1.Y);
    }

    public override string ToString()
    {
      return string.Format("({0}, {1})", (object) this.X, (object) this.Y);
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() ^ this.Y.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Vector2))
        return false;
      else
        return this.Equals((Vector2) obj);
    }

    public bool Equals(Vector2 other)
    {
      if ((double) this.X == (double) other.X)
        return (double) this.Y == (double) other.Y;
      else
        return false;
    }
  }
}
