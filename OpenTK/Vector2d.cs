// Type: OpenTK.Vector2d
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK
{
  [Serializable]
  public struct Vector2d : IEquatable<Vector2d>
  {
    public static Vector2d UnitX = new Vector2d(1.0, 0.0);
    public static Vector2d UnitY = new Vector2d(0.0, 1.0);
    public static Vector2d Zero = new Vector2d(0.0, 0.0);
    public static readonly Vector2d One = new Vector2d(1.0, 1.0);
    public static readonly int SizeInBytes = Marshal.SizeOf((object) new Vector2d());
    public double X;
    public double Y;

    public double Length
    {
      get
      {
        return Math.Sqrt(this.X * this.X + this.Y * this.Y);
      }
    }

    public double LengthSquared
    {
      get
      {
        return this.X * this.X + this.Y * this.Y;
      }
    }

    public Vector2d PerpendicularRight
    {
      get
      {
        return new Vector2d(this.Y, -this.X);
      }
    }

    public Vector2d PerpendicularLeft
    {
      get
      {
        return new Vector2d(-this.Y, this.X);
      }
    }

    static Vector2d()
    {
    }

    public Vector2d(double value)
    {
      this.X = value;
      this.Y = value;
    }

    public Vector2d(double x, double y)
    {
      this.X = x;
      this.Y = y;
    }

    public static explicit operator Vector2d(Vector2 v2)
    {
      return new Vector2d((double) v2.X, (double) v2.Y);
    }

    public static explicit operator Vector2(Vector2d v2d)
    {
      return new Vector2((float) v2d.X, (float) v2d.Y);
    }

    public static Vector2d operator +(Vector2d left, Vector2d right)
    {
      left.X += right.X;
      left.Y += right.Y;
      return left;
    }

    public static Vector2d operator -(Vector2d left, Vector2d right)
    {
      left.X -= right.X;
      left.Y -= right.Y;
      return left;
    }

    public static Vector2d operator -(Vector2d vec)
    {
      vec.X = -vec.X;
      vec.Y = -vec.Y;
      return vec;
    }

    public static Vector2d operator *(Vector2d vec, double f)
    {
      vec.X *= f;
      vec.Y *= f;
      return vec;
    }

    public static Vector2d operator *(double f, Vector2d vec)
    {
      vec.X *= f;
      vec.Y *= f;
      return vec;
    }

    public static Vector2d operator /(Vector2d vec, double f)
    {
      double num = 1.0 / f;
      vec.X *= num;
      vec.Y *= num;
      return vec;
    }

    public static bool operator ==(Vector2d left, Vector2d right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Vector2d left, Vector2d right)
    {
      return !left.Equals(right);
    }

    [Obsolete("Use static Add() method instead.")]
    public void Add(Vector2d right)
    {
      this.X += right.X;
      this.Y += right.Y;
    }

    [CLSCompliant(false)]
    [Obsolete("Use static Add() method instead.")]
    public void Add(ref Vector2d right)
    {
      this.X += right.X;
      this.Y += right.Y;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public void Sub(Vector2d right)
    {
      this.X -= right.X;
      this.Y -= right.Y;
    }

    [CLSCompliant(false)]
    [Obsolete("Use static Subtract() method instead.")]
    public void Sub(ref Vector2d right)
    {
      this.X -= right.X;
      this.Y -= right.Y;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Mult(double f)
    {
      this.X *= f;
      this.Y *= f;
    }

    [Obsolete("Use static Divide() method instead.")]
    public void Div(double f)
    {
      double num = 1.0 / f;
      this.X *= num;
      this.Y *= num;
    }

    public void Normalize()
    {
      double num = 1.0 / this.Length;
      this.X *= num;
      this.Y *= num;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(double sx, double sy)
    {
      this.X *= sx;
      this.Y *= sy;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(Vector2d scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
    }

    [CLSCompliant(false)]
    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(ref Vector2d scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public static Vector2d Sub(Vector2d a, Vector2d b)
    {
      a.X -= b.X;
      a.Y -= b.Y;
      return a;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public static void Sub(ref Vector2d a, ref Vector2d b, out Vector2d result)
    {
      result.X = a.X - b.X;
      result.Y = a.Y - b.Y;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public static Vector2d Mult(Vector2d a, double d)
    {
      a.X *= d;
      a.Y *= d;
      return a;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public static void Mult(ref Vector2d a, double d, out Vector2d result)
    {
      result.X = a.X * d;
      result.Y = a.Y * d;
    }

    [Obsolete("Use static Divide() method instead.")]
    public static Vector2d Div(Vector2d a, double d)
    {
      double num = 1.0 / d;
      a.X *= num;
      a.Y *= num;
      return a;
    }

    [Obsolete("Use static Divide() method instead.")]
    public static void Div(ref Vector2d a, double d, out Vector2d result)
    {
      double num = 1.0 / d;
      result.X = a.X * num;
      result.Y = a.Y * num;
    }

    public static Vector2d Add(Vector2d a, Vector2d b)
    {
      Vector2d.Add(ref a, ref b, out a);
      return a;
    }

    public static void Add(ref Vector2d a, ref Vector2d b, out Vector2d result)
    {
      result = new Vector2d(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2d Subtract(Vector2d a, Vector2d b)
    {
      Vector2d.Subtract(ref a, ref b, out a);
      return a;
    }

    public static void Subtract(ref Vector2d a, ref Vector2d b, out Vector2d result)
    {
      result = new Vector2d(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2d Multiply(Vector2d vector, double scale)
    {
      Vector2d.Multiply(ref vector, scale, out vector);
      return vector;
    }

    public static void Multiply(ref Vector2d vector, double scale, out Vector2d result)
    {
      result = new Vector2d(vector.X * scale, vector.Y * scale);
    }

    public static Vector2d Multiply(Vector2d vector, Vector2d scale)
    {
      Vector2d.Multiply(ref vector, ref scale, out vector);
      return vector;
    }

    public static void Multiply(ref Vector2d vector, ref Vector2d scale, out Vector2d result)
    {
      result = new Vector2d(vector.X * scale.X, vector.Y * scale.Y);
    }

    public static Vector2d Divide(Vector2d vector, double scale)
    {
      Vector2d.Divide(ref vector, scale, out vector);
      return vector;
    }

    public static void Divide(ref Vector2d vector, double scale, out Vector2d result)
    {
      Vector2d.Multiply(ref vector, 1.0 / scale, out result);
    }

    public static Vector2d Divide(Vector2d vector, Vector2d scale)
    {
      Vector2d.Divide(ref vector, ref scale, out vector);
      return vector;
    }

    public static void Divide(ref Vector2d vector, ref Vector2d scale, out Vector2d result)
    {
      result = new Vector2d(vector.X / scale.X, vector.Y / scale.Y);
    }

    public static Vector2d Min(Vector2d a, Vector2d b)
    {
      a.X = a.X < b.X ? a.X : b.X;
      a.Y = a.Y < b.Y ? a.Y : b.Y;
      return a;
    }

    public static void Min(ref Vector2d a, ref Vector2d b, out Vector2d result)
    {
      result.X = a.X < b.X ? a.X : b.X;
      result.Y = a.Y < b.Y ? a.Y : b.Y;
    }

    public static Vector2d Max(Vector2d a, Vector2d b)
    {
      a.X = a.X > b.X ? a.X : b.X;
      a.Y = a.Y > b.Y ? a.Y : b.Y;
      return a;
    }

    public static void Max(ref Vector2d a, ref Vector2d b, out Vector2d result)
    {
      result.X = a.X > b.X ? a.X : b.X;
      result.Y = a.Y > b.Y ? a.Y : b.Y;
    }

    public static Vector2d Clamp(Vector2d vec, Vector2d min, Vector2d max)
    {
      vec.X = vec.X < min.X ? min.X : (vec.X > max.X ? max.X : vec.X);
      vec.Y = vec.Y < min.Y ? min.Y : (vec.Y > max.Y ? max.Y : vec.Y);
      return vec;
    }

    public static void Clamp(ref Vector2d vec, ref Vector2d min, ref Vector2d max, out Vector2d result)
    {
      result.X = vec.X < min.X ? min.X : (vec.X > max.X ? max.X : vec.X);
      result.Y = vec.Y < min.Y ? min.Y : (vec.Y > max.Y ? max.Y : vec.Y);
    }

    public static Vector2d Normalize(Vector2d vec)
    {
      double num = 1.0 / vec.Length;
      vec.X *= num;
      vec.Y *= num;
      return vec;
    }

    public static void Normalize(ref Vector2d vec, out Vector2d result)
    {
      double num = 1.0 / vec.Length;
      result.X = vec.X * num;
      result.Y = vec.Y * num;
    }

    public static Vector2d NormalizeFast(Vector2d vec)
    {
      double num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
      vec.X *= num;
      vec.Y *= num;
      return vec;
    }

    public static void NormalizeFast(ref Vector2d vec, out Vector2d result)
    {
      double num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
      result.X = vec.X * num;
      result.Y = vec.Y * num;
    }

    public static double Dot(Vector2d left, Vector2d right)
    {
      return left.X * right.X + left.Y * right.Y;
    }

    public static void Dot(ref Vector2d left, ref Vector2d right, out double result)
    {
      result = left.X * right.X + left.Y * right.Y;
    }

    public static Vector2d Lerp(Vector2d a, Vector2d b, double blend)
    {
      a.X = blend * (b.X - a.X) + a.X;
      a.Y = blend * (b.Y - a.Y) + a.Y;
      return a;
    }

    public static void Lerp(ref Vector2d a, ref Vector2d b, double blend, out Vector2d result)
    {
      result.X = blend * (b.X - a.X) + a.X;
      result.Y = blend * (b.Y - a.Y) + a.Y;
    }

    public static Vector2d BaryCentric(Vector2d a, Vector2d b, Vector2d c, double u, double v)
    {
      return a + u * (b - a) + v * (c - a);
    }

    public static void BaryCentric(ref Vector2d a, ref Vector2d b, ref Vector2d c, double u, double v, out Vector2d result)
    {
      result = a;
      Vector2d result1 = b;
      Vector2d.Subtract(ref result1, ref a, out result1);
      Vector2d.Multiply(ref result1, u, out result1);
      Vector2d.Add(ref result, ref result1, out result);
      Vector2d result2 = c;
      Vector2d.Subtract(ref result2, ref a, out result2);
      Vector2d.Multiply(ref result2, v, out result2);
      Vector2d.Add(ref result, ref result2, out result);
    }

    public static Vector2d Transform(Vector2d vec, Quaterniond quat)
    {
      Vector2d result;
      Vector2d.Transform(ref vec, ref quat, out result);
      return result;
    }

    public static void Transform(ref Vector2d vec, ref Quaterniond quat, out Vector2d result)
    {
      Quaterniond result1 = new Quaterniond(vec.X, vec.Y, 0.0, 0.0);
      Quaterniond result2;
      Quaterniond.Invert(ref quat, out result2);
      Quaterniond result3;
      Quaterniond.Multiply(ref quat, ref result1, out result3);
      Quaterniond.Multiply(ref result3, ref result2, out result1);
      result = new Vector2d(result1.X, result1.Y);
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
      if (!(obj is Vector2d))
        return false;
      else
        return this.Equals((Vector2d) obj);
    }

    public bool Equals(Vector2d other)
    {
      if (this.X == other.X)
        return this.Y == other.Y;
      else
        return false;
    }
  }
}
