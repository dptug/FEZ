// Type: OpenTK.Vector3
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace OpenTK
{
  [Serializable]
  public struct Vector3 : IEquatable<Vector3>
  {
    public static readonly Vector3 UnitX = new Vector3(1f, 0.0f, 0.0f);
    public static readonly Vector3 UnitY = new Vector3(0.0f, 1f, 0.0f);
    public static readonly Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1f);
    public static readonly Vector3 Zero = new Vector3(0.0f, 0.0f, 0.0f);
    public static readonly Vector3 One = new Vector3(1f, 1f, 1f);
    public static readonly int SizeInBytes = Marshal.SizeOf((object) new Vector3());
    public float X;
    public float Y;
    public float Z;

    public float Length
    {
      get
      {
        return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);
      }
    }

    public float LengthFast
    {
      get
      {
        return 1f / MathHelper.InverseSqrtFast((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z));
      }
    }

    public float LengthSquared
    {
      get
      {
        return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);
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

    public Vector3(Vector2 v)
    {
      this.X = v.X;
      this.Y = v.Y;
      this.Z = 0.0f;
    }

    public Vector3(Vector3 v)
    {
      this.X = v.X;
      this.Y = v.Y;
      this.Z = v.Z;
    }

    public Vector3(Vector4 v)
    {
      this.X = v.X;
      this.Y = v.Y;
      this.Z = v.Z;
    }

    public static Vector3 operator +(Vector3 left, Vector3 right)
    {
      left.X += right.X;
      left.Y += right.Y;
      left.Z += right.Z;
      return left;
    }

    public static Vector3 operator -(Vector3 left, Vector3 right)
    {
      left.X -= right.X;
      left.Y -= right.Y;
      left.Z -= right.Z;
      return left;
    }

    public static Vector3 operator -(Vector3 vec)
    {
      vec.X = -vec.X;
      vec.Y = -vec.Y;
      vec.Z = -vec.Z;
      return vec;
    }

    public static Vector3 operator *(Vector3 vec, float scale)
    {
      vec.X *= scale;
      vec.Y *= scale;
      vec.Z *= scale;
      return vec;
    }

    public static Vector3 operator *(float scale, Vector3 vec)
    {
      vec.X *= scale;
      vec.Y *= scale;
      vec.Z *= scale;
      return vec;
    }

    public static Vector3 operator /(Vector3 vec, float scale)
    {
      float num = 1f / scale;
      vec.X *= num;
      vec.Y *= num;
      vec.Z *= num;
      return vec;
    }

    public static bool operator ==(Vector3 left, Vector3 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Vector3 left, Vector3 right)
    {
      return !left.Equals(right);
    }

    [Obsolete("Use static Add() method instead.")]
    public void Add(Vector3 right)
    {
      this.X += right.X;
      this.Y += right.Y;
      this.Z += right.Z;
    }

    [Obsolete("Use static Add() method instead.")]
    [CLSCompliant(false)]
    public void Add(ref Vector3 right)
    {
      this.X += right.X;
      this.Y += right.Y;
      this.Z += right.Z;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public void Sub(Vector3 right)
    {
      this.X -= right.X;
      this.Y -= right.Y;
      this.Z -= right.Z;
    }

    [Obsolete("Use static Subtract() method instead.")]
    [CLSCompliant(false)]
    public void Sub(ref Vector3 right)
    {
      this.X -= right.X;
      this.Y -= right.Y;
      this.Z -= right.Z;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Mult(float f)
    {
      this.X *= f;
      this.Y *= f;
      this.Z *= f;
    }

    [Obsolete("Use static Divide() method instead.")]
    public void Div(float f)
    {
      float num = 1f / f;
      this.X *= num;
      this.Y *= num;
      this.Z *= num;
    }

    public void Normalize()
    {
      float num = 1f / this.Length;
      this.X *= num;
      this.Y *= num;
      this.Z *= num;
    }

    public void NormalizeFast()
    {
      float num = MathHelper.InverseSqrtFast((float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z));
      this.X *= num;
      this.Y *= num;
      this.Z *= num;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(float sx, float sy, float sz)
    {
      this.X = this.X * sx;
      this.Y = this.Y * sy;
      this.Z = this.Z * sz;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public void Scale(Vector3 scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
      this.Z *= scale.Z;
    }

    [Obsolete("Use static Multiply() method instead.")]
    [CLSCompliant(false)]
    public void Scale(ref Vector3 scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
      this.Z *= scale.Z;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public static Vector3 Sub(Vector3 a, Vector3 b)
    {
      a.X -= b.X;
      a.Y -= b.Y;
      a.Z -= b.Z;
      return a;
    }

    [Obsolete("Use static Subtract() method instead.")]
    public static void Sub(ref Vector3 a, ref Vector3 b, out Vector3 result)
    {
      result.X = a.X - b.X;
      result.Y = a.Y - b.Y;
      result.Z = a.Z - b.Z;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public static Vector3 Mult(Vector3 a, float f)
    {
      a.X *= f;
      a.Y *= f;
      a.Z *= f;
      return a;
    }

    [Obsolete("Use static Multiply() method instead.")]
    public static void Mult(ref Vector3 a, float f, out Vector3 result)
    {
      result.X = a.X * f;
      result.Y = a.Y * f;
      result.Z = a.Z * f;
    }

    [Obsolete("Use static Divide() method instead.")]
    public static Vector3 Div(Vector3 a, float f)
    {
      float num = 1f / f;
      a.X *= num;
      a.Y *= num;
      a.Z *= num;
      return a;
    }

    [Obsolete("Use static Divide() method instead.")]
    public static void Div(ref Vector3 a, float f, out Vector3 result)
    {
      float num = 1f / f;
      result.X = a.X * num;
      result.Y = a.Y * num;
      result.Z = a.Z * num;
    }

    public static Vector3 Add(Vector3 a, Vector3 b)
    {
      Vector3.Add(ref a, ref b, out a);
      return a;
    }

    public static void Add(ref Vector3 a, ref Vector3 b, out Vector3 result)
    {
      result = new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3 Subtract(Vector3 a, Vector3 b)
    {
      Vector3.Subtract(ref a, ref b, out a);
      return a;
    }

    public static void Subtract(ref Vector3 a, ref Vector3 b, out Vector3 result)
    {
      result = new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector3 Multiply(Vector3 vector, float scale)
    {
      Vector3.Multiply(ref vector, scale, out vector);
      return vector;
    }

    public static void Multiply(ref Vector3 vector, float scale, out Vector3 result)
    {
      result = new Vector3(vector.X * scale, vector.Y * scale, vector.Z * scale);
    }

    public static Vector3 Multiply(Vector3 vector, Vector3 scale)
    {
      Vector3.Multiply(ref vector, ref scale, out vector);
      return vector;
    }

    public static void Multiply(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
    {
      result = new Vector3(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
    }

    public static Vector3 Divide(Vector3 vector, float scale)
    {
      Vector3.Divide(ref vector, scale, out vector);
      return vector;
    }

    public static void Divide(ref Vector3 vector, float scale, out Vector3 result)
    {
      Vector3.Multiply(ref vector, 1f / scale, out result);
    }

    public static Vector3 Divide(Vector3 vector, Vector3 scale)
    {
      Vector3.Divide(ref vector, ref scale, out vector);
      return vector;
    }

    public static void Divide(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
    {
      result = new Vector3(vector.X / scale.X, vector.Y / scale.Y, vector.Z / scale.Z);
    }

    public static Vector3 ComponentMin(Vector3 a, Vector3 b)
    {
      a.X = (double) a.X < (double) b.X ? a.X : b.X;
      a.Y = (double) a.Y < (double) b.Y ? a.Y : b.Y;
      a.Z = (double) a.Z < (double) b.Z ? a.Z : b.Z;
      return a;
    }

    public static void ComponentMin(ref Vector3 a, ref Vector3 b, out Vector3 result)
    {
      result.X = (double) a.X < (double) b.X ? a.X : b.X;
      result.Y = (double) a.Y < (double) b.Y ? a.Y : b.Y;
      result.Z = (double) a.Z < (double) b.Z ? a.Z : b.Z;
    }

    public static Vector3 ComponentMax(Vector3 a, Vector3 b)
    {
      a.X = (double) a.X > (double) b.X ? a.X : b.X;
      a.Y = (double) a.Y > (double) b.Y ? a.Y : b.Y;
      a.Z = (double) a.Z > (double) b.Z ? a.Z : b.Z;
      return a;
    }

    public static void ComponentMax(ref Vector3 a, ref Vector3 b, out Vector3 result)
    {
      result.X = (double) a.X > (double) b.X ? a.X : b.X;
      result.Y = (double) a.Y > (double) b.Y ? a.Y : b.Y;
      result.Z = (double) a.Z > (double) b.Z ? a.Z : b.Z;
    }

    public static Vector3 Min(Vector3 left, Vector3 right)
    {
      if ((double) left.LengthSquared >= (double) right.LengthSquared)
        return right;
      else
        return left;
    }

    public static Vector3 Max(Vector3 left, Vector3 right)
    {
      if ((double) left.LengthSquared < (double) right.LengthSquared)
        return right;
      else
        return left;
    }

    public static Vector3 Clamp(Vector3 vec, Vector3 min, Vector3 max)
    {
      vec.X = (double) vec.X < (double) min.X ? min.X : ((double) vec.X > (double) max.X ? max.X : vec.X);
      vec.Y = (double) vec.Y < (double) min.Y ? min.Y : ((double) vec.Y > (double) max.Y ? max.Y : vec.Y);
      vec.Z = (double) vec.Z < (double) min.Z ? min.Z : ((double) vec.Z > (double) max.Z ? max.Z : vec.Z);
      return vec;
    }

    public static void Clamp(ref Vector3 vec, ref Vector3 min, ref Vector3 max, out Vector3 result)
    {
      result.X = (double) vec.X < (double) min.X ? min.X : ((double) vec.X > (double) max.X ? max.X : vec.X);
      result.Y = (double) vec.Y < (double) min.Y ? min.Y : ((double) vec.Y > (double) max.Y ? max.Y : vec.Y);
      result.Z = (double) vec.Z < (double) min.Z ? min.Z : ((double) vec.Z > (double) max.Z ? max.Z : vec.Z);
    }

    public static Vector3 Normalize(Vector3 vec)
    {
      float num = 1f / vec.Length;
      vec.X *= num;
      vec.Y *= num;
      vec.Z *= num;
      return vec;
    }

    public static void Normalize(ref Vector3 vec, out Vector3 result)
    {
      float num = 1f / vec.Length;
      result.X = vec.X * num;
      result.Y = vec.Y * num;
      result.Z = vec.Z * num;
    }

    public static Vector3 NormalizeFast(Vector3 vec)
    {
      float num = MathHelper.InverseSqrtFast((float) ((double) vec.X * (double) vec.X + (double) vec.Y * (double) vec.Y + (double) vec.Z * (double) vec.Z));
      vec.X *= num;
      vec.Y *= num;
      vec.Z *= num;
      return vec;
    }

    public static void NormalizeFast(ref Vector3 vec, out Vector3 result)
    {
      float num = MathHelper.InverseSqrtFast((float) ((double) vec.X * (double) vec.X + (double) vec.Y * (double) vec.Y + (double) vec.Z * (double) vec.Z));
      result.X = vec.X * num;
      result.Y = vec.Y * num;
      result.Z = vec.Z * num;
    }

    public static float Dot(Vector3 left, Vector3 right)
    {
      return (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z);
    }

    public static void Dot(ref Vector3 left, ref Vector3 right, out float result)
    {
      result = (float) ((double) left.X * (double) right.X + (double) left.Y * (double) right.Y + (double) left.Z * (double) right.Z);
    }

    public static Vector3 Cross(Vector3 left, Vector3 right)
    {
      Vector3 result;
      Vector3.Cross(ref left, ref right, out result);
      return result;
    }

    public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
    {
      result = new Vector3((float) ((double) left.Y * (double) right.Z - (double) left.Z * (double) right.Y), (float) ((double) left.Z * (double) right.X - (double) left.X * (double) right.Z), (float) ((double) left.X * (double) right.Y - (double) left.Y * (double) right.X));
    }

    public static Vector3 Lerp(Vector3 a, Vector3 b, float blend)
    {
      a.X = blend * (b.X - a.X) + a.X;
      a.Y = blend * (b.Y - a.Y) + a.Y;
      a.Z = blend * (b.Z - a.Z) + a.Z;
      return a;
    }

    public static void Lerp(ref Vector3 a, ref Vector3 b, float blend, out Vector3 result)
    {
      result.X = blend * (b.X - a.X) + a.X;
      result.Y = blend * (b.Y - a.Y) + a.Y;
      result.Z = blend * (b.Z - a.Z) + a.Z;
    }

    public static Vector3 BaryCentric(Vector3 a, Vector3 b, Vector3 c, float u, float v)
    {
      return a + u * (b - a) + v * (c - a);
    }

    public static void BaryCentric(ref Vector3 a, ref Vector3 b, ref Vector3 c, float u, float v, out Vector3 result)
    {
      result = a;
      Vector3 result1 = b;
      Vector3.Subtract(ref result1, ref a, out result1);
      Vector3.Multiply(ref result1, u, out result1);
      Vector3.Add(ref result, ref result1, out result);
      Vector3 result2 = c;
      Vector3.Subtract(ref result2, ref a, out result2);
      Vector3.Multiply(ref result2, v, out result2);
      Vector3.Add(ref result, ref result2, out result);
    }

    public static Vector3 TransformVector(Vector3 vec, Matrix4 mat)
    {
      Vector3 vector3;
      vector3.X = Vector3.Dot(vec, new Vector3(mat.Column0));
      vector3.Y = Vector3.Dot(vec, new Vector3(mat.Column1));
      vector3.Z = Vector3.Dot(vec, new Vector3(mat.Column2));
      return vector3;
    }

    public static void TransformVector(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
    {
      result.X = (float) ((double) vec.X * (double) mat.Row0.X + (double) vec.Y * (double) mat.Row1.X + (double) vec.Z * (double) mat.Row2.X);
      result.Y = (float) ((double) vec.X * (double) mat.Row0.Y + (double) vec.Y * (double) mat.Row1.Y + (double) vec.Z * (double) mat.Row2.Y);
      result.Z = (float) ((double) vec.X * (double) mat.Row0.Z + (double) vec.Y * (double) mat.Row1.Z + (double) vec.Z * (double) mat.Row2.Z);
    }

    public static Vector3 TransformNormal(Vector3 norm, Matrix4 mat)
    {
      mat.Invert();
      return Vector3.TransformNormalInverse(norm, mat);
    }

    public static void TransformNormal(ref Vector3 norm, ref Matrix4 mat, out Vector3 result)
    {
      Matrix4 invMat = Matrix4.Invert(mat);
      Vector3.TransformNormalInverse(ref norm, ref invMat, out result);
    }

    public static Vector3 TransformNormalInverse(Vector3 norm, Matrix4 invMat)
    {
      Vector3 vector3;
      vector3.X = Vector3.Dot(norm, new Vector3(invMat.Row0));
      vector3.Y = Vector3.Dot(norm, new Vector3(invMat.Row1));
      vector3.Z = Vector3.Dot(norm, new Vector3(invMat.Row2));
      return vector3;
    }

    public static void TransformNormalInverse(ref Vector3 norm, ref Matrix4 invMat, out Vector3 result)
    {
      result.X = (float) ((double) norm.X * (double) invMat.Row0.X + (double) norm.Y * (double) invMat.Row0.Y + (double) norm.Z * (double) invMat.Row0.Z);
      result.Y = (float) ((double) norm.X * (double) invMat.Row1.X + (double) norm.Y * (double) invMat.Row1.Y + (double) norm.Z * (double) invMat.Row1.Z);
      result.Z = (float) ((double) norm.X * (double) invMat.Row2.X + (double) norm.Y * (double) invMat.Row2.Y + (double) norm.Z * (double) invMat.Row2.Z);
    }

    public static Vector3 TransformPosition(Vector3 pos, Matrix4 mat)
    {
      Vector3 vector3;
      vector3.X = Vector3.Dot(pos, new Vector3(mat.Column0)) + mat.Row3.X;
      vector3.Y = Vector3.Dot(pos, new Vector3(mat.Column1)) + mat.Row3.Y;
      vector3.Z = Vector3.Dot(pos, new Vector3(mat.Column2)) + mat.Row3.Z;
      return vector3;
    }

    public static void TransformPosition(ref Vector3 pos, ref Matrix4 mat, out Vector3 result)
    {
      result.X = (float) ((double) pos.X * (double) mat.Row0.X + (double) pos.Y * (double) mat.Row1.X + (double) pos.Z * (double) mat.Row2.X) + mat.Row3.X;
      result.Y = (float) ((double) pos.X * (double) mat.Row0.Y + (double) pos.Y * (double) mat.Row1.Y + (double) pos.Z * (double) mat.Row2.Y) + mat.Row3.Y;
      result.Z = (float) ((double) pos.X * (double) mat.Row0.Z + (double) pos.Y * (double) mat.Row1.Z + (double) pos.Z * (double) mat.Row2.Z) + mat.Row3.Z;
    }

    public static Vector3 Transform(Vector3 vec, Matrix4 mat)
    {
      Vector3 result;
      Vector3.Transform(ref vec, ref mat, out result);
      return result;
    }

    public static void Transform(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
    {
      Vector4 result1 = new Vector4(vec.X, vec.Y, vec.Z, 1f);
      Vector4.Transform(ref result1, ref mat, out result1);
      result = result1.Xyz;
    }

    public static Vector3 Transform(Vector3 vec, Quaternion quat)
    {
      Vector3 result;
      Vector3.Transform(ref vec, ref quat, out result);
      return result;
    }

    public static void Transform(ref Vector3 vec, ref Quaternion quat, out Vector3 result)
    {
      Vector3 xyz = quat.Xyz;
      Vector3 result1;
      Vector3.Cross(ref xyz, ref vec, out result1);
      Vector3 result2;
      Vector3.Multiply(ref vec, quat.W, out result2);
      Vector3.Add(ref result1, ref result2, out result1);
      Vector3.Cross(ref xyz, ref result1, out result1);
      Vector3.Multiply(ref result1, 2f, out result1);
      Vector3.Add(ref vec, ref result1, out result);
    }

    public static Vector3 TransformPerspective(Vector3 vec, Matrix4 mat)
    {
      Vector3 result;
      Vector3.TransformPerspective(ref vec, ref mat, out result);
      return result;
    }

    public static void TransformPerspective(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
    {
      Vector4 result1 = new Vector4(vec, 1f);
      Vector4.Transform(ref result1, ref mat, out result1);
      result.X = result1.X / result1.W;
      result.Y = result1.Y / result1.W;
      result.Z = result1.Z / result1.W;
    }

    public static float CalculateAngle(Vector3 first, Vector3 second)
    {
      return (float) Math.Acos((double) Vector3.Dot(first, second) / ((double) first.Length * (double) second.Length));
    }

    public static void CalculateAngle(ref Vector3 first, ref Vector3 second, out float result)
    {
      float result1;
      Vector3.Dot(ref first, ref second, out result1);
      result = (float) Math.Acos((double) result1 / ((double) first.Length * (double) second.Length));
    }

    public override string ToString()
    {
      return string.Format("({0}, {1}, {2})", (object) this.X, (object) this.Y, (object) this.Z);
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Vector3))
        return false;
      else
        return this.Equals((Vector3) obj);
    }

    public bool Equals(Vector3 other)
    {
      if ((double) this.X == (double) other.X && (double) this.Y == (double) other.Y)
        return (double) this.Z == (double) other.Z;
      else
        return false;
    }
  }
}
