// Type: OpenTK.Vector4
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace OpenTK
{
  [Serializable]
  public struct Vector4 : IEquatable<Vector4>
  {
    public static Vector4 UnitX = new Vector4(1f, 0.0f, 0.0f, 0.0f);
    public static Vector4 UnitY = new Vector4(0.0f, 1f, 0.0f, 0.0f);
    public static Vector4 UnitZ = new Vector4(0.0f, 0.0f, 1f, 0.0f);
    public static Vector4 UnitW = new Vector4(0.0f, 0.0f, 0.0f, 1f);
    public static Vector4 Zero = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    public static readonly Vector4 One = new Vector4(1f, 1f, 1f, 1f);
    public static readonly int SizeInBytes = Marshal.SizeOf((object) new Vector4());
    public float X;
    public float Y;
    public float Z;
    public float W;

    public float Length
    {
      get
      {
        return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
      }
    }

    public float LengthFast
    {
      get
      {
        return 1f / MathHelper.InverseSqrtFast((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W));
      }
    }

    public float LengthSquared
    {
      get
      {
        return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
      }
    }

    [XmlIgnore]
    public Vector2 Xy
    {
      get
      {
        return new Vector2(this.X, this.Y);
      }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
      }
    }

    [XmlIgnore]
    public Vector3 Xyz
    {
      get
      {
        return new Vector3(this.X, this.Y, this.Z);
      }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
        this.Z = value.Z;
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

    public Vector4(Vector2 v)
    {
      this.X = v.X;
      this.Y = v.Y;
      this.Z = 0.0f;
      this.W = 0.0f;
    }

    public Vector4(Vector3 v)
    {
      this.X = v.X;
      this.Y = v.Y;
      this.Z = v.Z;
      this.W = 0.0f;
    }

    public Vector4(Vector3 v, float w)
    {
      this.X = v.X;
      this.Y = v.Y;
      this.Z = v.Z;
      this.W = w;
    }

    public Vector4(Vector4 v)
    {
      this.X = v.X;
      this.Y = v.Y;
      this.Z = v.Z;
      this.W = v.W;
    }

    [CLSCompliant(false)]
    public static unsafe explicit operator float*(Vector4 v)
    {
      return &v.X;
    }

    public static unsafe explicit operator IntPtr(Vector4 v)
    {
      return (IntPtr) ((void*) &v.X);
    }

    public static Vector4 operator +(Vector4 left, Vector4 right)
    {
      left.X += right.X;
      left.Y += right.Y;
      left.Z += right.Z;
      left.W += right.W;
      return left;
    }

    public static Vector4 operator -(Vector4 left, Vector4 right)
    {
      left.X -= right.X;
      left.Y -= right.Y;
      left.Z -= right.Z;
      left.W -= right.W;
      return left;
    }

    public static Vector4 operator -(Vector4 vec)
    {
      vec.X = -vec.X;
      vec.Y = -vec.Y;
      vec.Z = -vec.Z;
      vec.W = -vec.W;
      return vec;
    }

    public static Vector4 operator *(Vector4 vec, float scale)
    {
      vec.X *= scale;
      vec.Y *= scale;
      vec.Z *= scale;
      vec.W *= scale;
      return vec;
    }

    public static Vector4 operator *(float scale, Vector4 vec)
    {
      vec.X *= scale;
      vec.Y *= scale;
      vec.Z *= scale;
      vec.W *= scale;
      return vec;
    }

    public static Vector4 operator /(Vector4 vec, float scale)
    {
      float num = 1f / scale;
      vec.X *= num;
      vec.Y *= num;
      vec.Z *= num;
      vec.W *= num;
      return vec;
    }

    public static bool operator ==(Vector4 left, Vector4 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Vector4 left, Vector4 right)
    {
      return !left.Equals(right);
    }

    [Obsolete("Use static Add() method instead.")]
    public void Add(Vector4 right)
    {
      this.X += right.X;
      this.Y += right.Y;
      this.Z += right.Z;
      this.W += right.W;
    }

    [CLSCompliant(false)]
    [Obsolete("Use static Add() method instead.")]
    public void Add(ref Vector4 right)
    {
      this.X += right.X;
      this.Y += right.Y;
      this.Z += right.Z;
      this.W += right.W;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public void Sub(Vector4 right)
    {
      this.X -= right.X;
      this.Y -= right.Y;
      this.Z -= right.Z;
      this.W -= right.W;
    }

    [CLSCompliant(false)]
    [Obsolete("Use static Subtract() method instead.")]
    public void Sub(ref Vector4 right)
    {
      this.X -= right.X;
      this.Y -= right.Y;
      this.Z -= right.Z;
      this.W -= right.W;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Mult(float f)
    {
      this.X *= f;
      this.Y *= f;
      this.Z *= f;
      this.W *= f;
    }

    [Obsolete("Use static Divide() method instead.")]
    public void Div(float f)
    {
      float num = 1f / f;
      this.X *= num;
      this.Y *= num;
      this.Z *= num;
      this.W *= num;
    }

    public void Normalize()
    {
      float num = 1f / this.Length;
      this.X *= num;
      this.Y *= num;
      this.Z *= num;
      this.W *= num;
    }

    public void NormalizeFast()
    {
      float num = MathHelper.InverseSqrtFast((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W));
      this.X *= num;
      this.Y *= num;
      this.Z *= num;
      this.W *= num;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(float sx, float sy, float sz, float sw)
    {
      this.X = this.X * sx;
      this.Y = this.Y * sy;
      this.Z = this.Z * sz;
      this.W = this.W * sw;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(Vector4 scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
      this.Z *= scale.Z;
      this.W *= scale.W;
    }

    [CLSCompliant(false)]
    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(ref Vector4 scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
      this.Z *= scale.Z;
      this.W *= scale.W;
    }

    public static Vector4 Sub(Vector4 a, Vector4 b)
    {
      a.X -= b.X;
      a.Y -= b.Y;
      a.Z -= b.Z;
      a.W -= b.W;
      return a;
    }

    public static void Sub(ref Vector4 a, ref Vector4 b, out Vector4 result)
    {
      result.X = a.X - b.X;
      result.Y = a.Y - b.Y;
      result.Z = a.Z - b.Z;
      result.W = a.W - b.W;
    }

    public static Vector4 Mult(Vector4 a, float f)
    {
      a.X *= f;
      a.Y *= f;
      a.Z *= f;
      a.W *= f;
      return a;
    }

    public static void Mult(ref Vector4 a, float f, out Vector4 result)
    {
      result.X = a.X * f;
      result.Y = a.Y * f;
      result.Z = a.Z * f;
      result.W = a.W * f;
    }

    public static Vector4 Div(Vector4 a, float f)
    {
      float num = 1f / f;
      a.X *= num;
      a.Y *= num;
      a.Z *= num;
      a.W *= num;
      return a;
    }

    public static void Div(ref Vector4 a, float f, out Vector4 result)
    {
      float num = 1f / f;
      result.X = a.X * num;
      result.Y = a.Y * num;
      result.Z = a.Z * num;
      result.W = a.W * num;
    }

    public static Vector4 Add(Vector4 a, Vector4 b)
    {
      Vector4.Add(ref a, ref b, out a);
      return a;
    }

    public static void Add(ref Vector4 a, ref Vector4 b, out Vector4 result)
    {
      result = new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    }

    public static Vector4 Subtract(Vector4 a, Vector4 b)
    {
      Vector4.Subtract(ref a, ref b, out a);
      return a;
    }

    public static void Subtract(ref Vector4 a, ref Vector4 b, out Vector4 result)
    {
      result = new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    }

    public static Vector4 Multiply(Vector4 vector, float scale)
    {
      Vector4.Multiply(ref vector, scale, out vector);
      return vector;
    }

    public static void Multiply(ref Vector4 vector, float scale, out Vector4 result)
    {
      result = new Vector4(vector.X * scale, vector.Y * scale, vector.Z * scale, vector.W * scale);
    }

    public static Vector4 Multiply(Vector4 vector, Vector4 scale)
    {
      Vector4.Multiply(ref vector, ref scale, out vector);
      return vector;
    }

    public static void Multiply(ref Vector4 vector, ref Vector4 scale, out Vector4 result)
    {
      result = new Vector4(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z, vector.W * scale.W);
    }

    public static Vector4 Divide(Vector4 vector, float scale)
    {
      Vector4.Divide(ref vector, scale, out vector);
      return vector;
    }

    public static void Divide(ref Vector4 vector, float scale, out Vector4 result)
    {
      Vector4.Multiply(ref vector, 1f / scale, out result);
    }

    public static Vector4 Divide(Vector4 vector, Vector4 scale)
    {
      Vector4.Divide(ref vector, ref scale, out vector);
      return vector;
    }

    public static void Divide(ref Vector4 vector, ref Vector4 scale, out Vector4 result)
    {
      result = new Vector4(vector.X / scale.X, vector.Y / scale.Y, vector.Z / scale.Z, vector.W / scale.W);
    }

    public static Vector4 Min(Vector4 a, Vector4 b)
    {
      a.X = (double) a.X < (double) b.X ? a.X : b.X;
      a.Y = (double) a.Y < (double) b.Y ? a.Y : b.Y;
      a.Z = (double) a.Z < (double) b.Z ? a.Z : b.Z;
      a.W = (double) a.W < (double) b.W ? a.W : b.W;
      return a;
    }

    public static void Min(ref Vector4 a, ref Vector4 b, out Vector4 result)
    {
      result.X = (double) a.X < (double) b.X ? a.X : b.X;
      result.Y = (double) a.Y < (double) b.Y ? a.Y : b.Y;
      result.Z = (double) a.Z < (double) b.Z ? a.Z : b.Z;
      result.W = (double) a.W < (double) b.W ? a.W : b.W;
    }

    public static Vector4 Max(Vector4 a, Vector4 b)
    {
      a.X = (double) a.X > (double) b.X ? a.X : b.X;
      a.Y = (double) a.Y > (double) b.Y ? a.Y : b.Y;
      a.Z = (double) a.Z > (double) b.Z ? a.Z : b.Z;
      a.W = (double) a.W > (double) b.W ? a.W : b.W;
      return a;
    }

    public static void Max(ref Vector4 a, ref Vector4 b, out Vector4 result)
    {
      result.X = (double) a.X > (double) b.X ? a.X : b.X;
      result.Y = (double) a.Y > (double) b.Y ? a.Y : b.Y;
      result.Z = (double) a.Z > (double) b.Z ? a.Z : b.Z;
      result.W = (double) a.W > (double) b.W ? a.W : b.W;
    }

    public static Vector4 Clamp(Vector4 vec, Vector4 min, Vector4 max)
    {
      vec.X = (double) vec.X < (double) min.X ? min.X : ((double) vec.X > (double) max.X ? max.X : vec.X);
      vec.Y = (double) vec.Y < (double) min.Y ? min.Y : ((double) vec.Y > (double) max.Y ? max.Y : vec.Y);
      vec.Z = (double) vec.X < (double) min.Z ? min.Z : ((double) vec.Z > (double) max.Z ? max.Z : vec.Z);
      vec.W = (double) vec.Y < (double) min.W ? min.W : ((double) vec.W > (double) max.W ? max.W : vec.W);
      return vec;
    }

    public static void Clamp(ref Vector4 vec, ref Vector4 min, ref Vector4 max, out Vector4 result)
    {
      result.X = (double) vec.X < (double) min.X ? min.X : ((double) vec.X > (double) max.X ? max.X : vec.X);
      result.Y = (double) vec.Y < (double) min.Y ? min.Y : ((double) vec.Y > (double) max.Y ? max.Y : vec.Y);
      result.Z = (double) vec.X < (double) min.Z ? min.Z : ((double) vec.Z > (double) max.Z ? max.Z : vec.Z);
      result.W = (double) vec.Y < (double) min.W ? min.W : ((double) vec.W > (double) max.W ? max.W : vec.W);
    }

    public static Vector4 Normalize(Vector4 vec)
    {
      float num = 1f / vec.Length;
      vec.X *= num;
      vec.Y *= num;
      vec.Z *= num;
      vec.W *= num;
      return vec;
    }

    public static void Normalize(ref Vector4 vec, out Vector4 result)
    {
      float num = 1f / vec.Length;
      result.X = vec.X * num;
      result.Y = vec.Y * num;
      result.Z = vec.Z * num;
      result.W = vec.W * num;
    }

    public static Vector4 NormalizeFast(Vector4 vec)
    {
      float num = MathHelper.InverseSqrtFast((float) ((double) vec.X * (double) vec.X + (double) vec.Y * (double) vec.Y + (double) vec.Z * (double) vec.Z + (double) vec.W * (double) vec.W));
      vec.X *= num;
      vec.Y *= num;
      vec.Z *= num;
      vec.W *= num;
      return vec;
    }

    public static void NormalizeFast(ref Vector4 vec, out Vector4 result)
    {
      float num = MathHelper.InverseSqrtFast((float) ((double) vec.X * (double) vec.X + (double) vec.Y * (double) vec.Y + (double) vec.Z * (double) vec.Z + (double) vec.W * (double) vec.W));
      result.X = vec.X * num;
      result.Y = vec.Y * num;
      result.Z = vec.Z * num;
      result.W = vec.W * num;
    }

    public static float Dot(Vector4 left, Vector4 right)
    {
      return (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z + (double) left.W * (double) right.W);
    }

    public static void Dot(ref Vector4 left, ref Vector4 right, out float result)
    {
      result = (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z + (double) left.W * (double) right.W);
    }

    public static Vector4 Lerp(Vector4 a, Vector4 b, float blend)
    {
      a.X = blend * (b.X - a.X) + a.X;
      a.Y = blend * (b.Y - a.Y) + a.Y;
      a.Z = blend * (b.Z - a.Z) + a.Z;
      a.W = blend * (b.W - a.W) + a.W;
      return a;
    }

    public static void Lerp(ref Vector4 a, ref Vector4 b, float blend, out Vector4 result)
    {
      result.X = blend * (b.X - a.X) + a.X;
      result.Y = blend * (b.Y - a.Y) + a.Y;
      result.Z = blend * (b.Z - a.Z) + a.Z;
      result.W = blend * (b.W - a.W) + a.W;
    }

    public static Vector4 BaryCentric(Vector4 a, Vector4 b, Vector4 c, float u, float v)
    {
      return a + u * (b - a) + v * (c - a);
    }

    public static void BaryCentric(ref Vector4 a, ref Vector4 b, ref Vector4 c, float u, float v, out Vector4 result)
    {
      result = a;
      Vector4 result1 = b;
      Vector4.Subtract(ref result1, ref a, out result1);
      Vector4.Multiply(ref result1, u, out result1);
      Vector4.Add(ref result, ref result1, out result);
      Vector4 result2 = c;
      Vector4.Subtract(ref result2, ref a, out result2);
      Vector4.Multiply(ref result2, v, out result2);
      Vector4.Add(ref result, ref result2, out result);
    }

    public static Vector4 Transform(Vector4 vec, Matrix4 mat)
    {
      Vector4 result;
      Vector4.Transform(ref vec, ref mat, out result);
      return result;
    }

    public static void Transform(ref Vector4 vec, ref Matrix4 mat, out Vector4 result)
    {
      result = new Vector4((float) ((double) vec.X * (double) mat.Row0.X + (double) vec.Y * (double) mat.Row1.X + (double) vec.Z * (double) mat.Row2.X + (double) vec.W * (double) mat.Row3.X), (float) ((double) vec.X * (double) mat.Row0.Y + (double) vec.Y * (double) mat.Row1.Y + (double) vec.Z * (double) mat.Row2.Y + (double) vec.W * (double) mat.Row3.Y), (float) ((double) vec.X * (double) mat.Row0.Z + (double) vec.Y * (double) mat.Row1.Z + (double) vec.Z * (double) mat.Row2.Z + (double) vec.W * (double) mat.Row3.Z), (float) ((double) vec.X * (double) mat.Row0.W + (double) vec.Y * (double) mat.Row1.W + (double) vec.Z * (double) mat.Row2.W + (double) vec.W * (double) mat.Row3.W));
    }

    public static Vector4 Transform(Vector4 vec, Quaternion quat)
    {
      Vector4 result;
      Vector4.Transform(ref vec, ref quat, out result);
      return result;
    }

    public static void Transform(ref Vector4 vec, ref Quaternion quat, out Vector4 result)
    {
      Quaternion result1 = new Quaternion(vec.X, vec.Y, vec.Z, vec.W);
      Quaternion result2;
      Quaternion.Invert(ref quat, out result2);
      Quaternion result3;
      Quaternion.Multiply(ref quat, ref result1, out result3);
      Quaternion.Multiply(ref result3, ref result2, out result1);
      result = new Vector4(result1.X, result1.Y, result1.Z, result1.W);
    }

    public override string ToString()
    {
      return string.Format("({0}, {1}, {2}, {3})", (object) this.X, (object) this.Y, (object) this.Z, (object) this.W);
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode() ^ this.W.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Vector4))
        return false;
      else
        return this.Equals((Vector4) obj);
    }

    public bool Equals(Vector4 other)
    {
      if ((double) this.X == (double) other.X && (double) this.Y == (double) other.Y && (double) this.Z == (double) other.Z)
        return (double) this.W == (double) other.W;
      else
        return false;
    }
  }
}
