// Type: OpenTK.Quaternion
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace OpenTK
{
  [Serializable]
  public struct Quaternion : IEquatable<Quaternion>
  {
    public static Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1f);
    private Vector3 xyz;
    private float w;

    [XmlIgnore]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [CLSCompliant(false)]
    [Obsolete("Use Xyz property instead.")]
    public Vector3 XYZ
    {
      get
      {
        return this.Xyz;
      }
      set
      {
        this.Xyz = value;
      }
    }

    public Vector3 Xyz
    {
      get
      {
        return this.xyz;
      }
      set
      {
        this.xyz = value;
      }
    }

    [XmlIgnore]
    public float X
    {
      get
      {
        return this.xyz.X;
      }
      set
      {
        this.xyz.X = value;
      }
    }

    [XmlIgnore]
    public float Y
    {
      get
      {
        return this.xyz.Y;
      }
      set
      {
        this.xyz.Y = value;
      }
    }

    [XmlIgnore]
    public float Z
    {
      get
      {
        return this.xyz.Z;
      }
      set
      {
        this.xyz.Z = value;
      }
    }

    public float W
    {
      get
      {
        return this.w;
      }
      set
      {
        this.w = value;
      }
    }

    public float Length
    {
      get
      {
        return (float) Math.Sqrt((double) this.W * (double) this.W + (double) this.Xyz.LengthSquared);
      }
    }

    public float LengthSquared
    {
      get
      {
        return this.W * this.W + this.Xyz.LengthSquared;
      }
    }

    static Quaternion()
    {
    }

    public Quaternion(Vector3 v, float w)
    {
      this.xyz = v;
      this.w = w;
    }

    public Quaternion(float x, float y, float z, float w)
    {
      this = new Quaternion(new Vector3(x, y, z), w);
    }

    public static Quaternion operator +(Quaternion left, Quaternion right)
    {
      left.Xyz += right.Xyz;
      left.W += right.W;
      return left;
    }

    public static Quaternion operator -(Quaternion left, Quaternion right)
    {
      left.Xyz -= right.Xyz;
      left.W -= right.W;
      return left;
    }

    public static Quaternion operator *(Quaternion left, Quaternion right)
    {
      Quaternion.Multiply(ref left, ref right, out left);
      return left;
    }

    public static Quaternion operator *(Quaternion quaternion, float scale)
    {
      Quaternion.Multiply(ref quaternion, scale, out quaternion);
      return quaternion;
    }

    public static Quaternion operator *(float scale, Quaternion quaternion)
    {
      return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
    }

    public static bool operator ==(Quaternion left, Quaternion right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Quaternion left, Quaternion right)
    {
      return !left.Equals(right);
    }

    public void ToAxisAngle(out Vector3 axis, out float angle)
    {
      Vector4 vector4 = this.ToAxisAngle();
      axis = vector4.Xyz;
      angle = vector4.W;
    }

    public Vector4 ToAxisAngle()
    {
      Quaternion quaternion = this;
      if ((double) Math.Abs(quaternion.W) > 1.0)
        quaternion.Normalize();
      Vector4 vector4 = new Vector4();
      vector4.W = 2f * (float) Math.Acos((double) quaternion.W);
      float num = (float) Math.Sqrt(1.0 - (double) quaternion.W * (double) quaternion.W);
      vector4.Xyz = (double) num <= 9.99999974737875E-05 ? Vector3.UnitX : quaternion.Xyz / num;
      return vector4;
    }

    public void Normalize()
    {
      float num = 1f / this.Length;
      this.Xyz *= num;
      this.W *= num;
    }

    public void Conjugate()
    {
      this.Xyz = -this.Xyz;
    }

    public static Quaternion Add(Quaternion left, Quaternion right)
    {
      return new Quaternion(left.Xyz + right.Xyz, left.W + right.W);
    }

    public static void Add(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
      result = new Quaternion(left.Xyz + right.Xyz, left.W + right.W);
    }

    public static Quaternion Sub(Quaternion left, Quaternion right)
    {
      return new Quaternion(left.Xyz - right.Xyz, left.W - right.W);
    }

    public static void Sub(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
      result = new Quaternion(left.Xyz - right.Xyz, left.W - right.W);
    }

    [Obsolete("Use Multiply instead.")]
    public static Quaternion Mult(Quaternion left, Quaternion right)
    {
      return new Quaternion(right.W * left.Xyz + left.W * right.Xyz + Vector3.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3.Dot(left.Xyz, right.Xyz));
    }

    [Obsolete("Use Multiply instead.")]
    public static void Mult(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
      result = new Quaternion(right.W * left.Xyz + left.W * right.Xyz + Vector3.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3.Dot(left.Xyz, right.Xyz));
    }

    public static Quaternion Multiply(Quaternion left, Quaternion right)
    {
      Quaternion result;
      Quaternion.Multiply(ref left, ref right, out result);
      return result;
    }

    public static void Multiply(ref Quaternion left, ref Quaternion right, out Quaternion result)
    {
      result = new Quaternion(right.W * left.Xyz + left.W * right.Xyz + Vector3.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3.Dot(left.Xyz, right.Xyz));
    }

    public static void Multiply(ref Quaternion quaternion, float scale, out Quaternion result)
    {
      result = new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
    }

    public static Quaternion Multiply(Quaternion quaternion, float scale)
    {
      return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
    }

    public static Quaternion Conjugate(Quaternion q)
    {
      return new Quaternion(-q.Xyz, q.W);
    }

    public static void Conjugate(ref Quaternion q, out Quaternion result)
    {
      result = new Quaternion(-q.Xyz, q.W);
    }

    public static Quaternion Invert(Quaternion q)
    {
      Quaternion result;
      Quaternion.Invert(ref q, out result);
      return result;
    }

    public static void Invert(ref Quaternion q, out Quaternion result)
    {
      float lengthSquared = q.LengthSquared;
      if ((double) lengthSquared != 0.0)
      {
        float num = 1f / lengthSquared;
        result = new Quaternion(q.Xyz * -num, q.W * num);
      }
      else
        result = q;
    }

    public static Quaternion Normalize(Quaternion q)
    {
      Quaternion result;
      Quaternion.Normalize(ref q, out result);
      return result;
    }

    public static void Normalize(ref Quaternion q, out Quaternion result)
    {
      float num = 1f / q.Length;
      result = new Quaternion(q.Xyz * num, q.W * num);
    }

    public static Quaternion FromAxisAngle(Vector3 axis, float angle)
    {
      if ((double) axis.LengthSquared == 0.0)
        return Quaternion.Identity;
      Quaternion q = Quaternion.Identity;
      angle *= 0.5f;
      axis.Normalize();
      q.Xyz = axis * (float) Math.Sin((double) angle);
      q.W = (float) Math.Cos((double) angle);
      return Quaternion.Normalize(q);
    }

    public static Quaternion Slerp(Quaternion q1, Quaternion q2, float blend)
    {
      if ((double) q1.LengthSquared == 0.0)
      {
        if ((double) q2.LengthSquared == 0.0)
          return Quaternion.Identity;
        else
          return q2;
      }
      else
      {
        if ((double) q2.LengthSquared == 0.0)
          return q1;
        float num1 = q1.W * q2.W + Vector3.Dot(q1.Xyz, q2.Xyz);
        if ((double) num1 >= 1.0 || (double) num1 <= -1.0)
          return q1;
        if ((double) num1 < 0.0)
        {
          q2.Xyz = -q2.Xyz;
          q2.W = -q2.W;
          num1 = -num1;
        }
        float num2;
        float num3;
        if ((double) num1 < 0.990000009536743)
        {
          float num4 = (float) Math.Acos((double) num1);
          float num5 = 1f / (float) Math.Sin((double) num4);
          num2 = (float) Math.Sin((double) num4 * (1.0 - (double) blend)) * num5;
          num3 = (float) Math.Sin((double) num4 * (double) blend) * num5;
        }
        else
        {
          num2 = 1f - blend;
          num3 = blend;
        }
        Quaternion q = new Quaternion(num2 * q1.Xyz + num3 * q2.Xyz, (float) ((double) num2 * (double) q1.W + (double) num3 * (double) q2.W));
        if ((double) q.LengthSquared > 0.0)
          return Quaternion.Normalize(q);
        else
          return Quaternion.Identity;
      }
    }

    public override string ToString()
    {
      return string.Format("V: {0}, W: {1}", (object) this.Xyz, (object) this.W);
    }

    public override bool Equals(object other)
    {
      if (!(other is Quaternion))
        return false;
      else
        return this == (Quaternion) other;
    }

    public override int GetHashCode()
    {
      return this.Xyz.GetHashCode() ^ this.W.GetHashCode();
    }

    public bool Equals(Quaternion other)
    {
      if (this.Xyz == other.Xyz)
        return (double) this.W == (double) other.W;
      else
        return false;
    }
  }
}
