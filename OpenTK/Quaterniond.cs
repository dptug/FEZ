// Type: OpenTK.Quaterniond
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace OpenTK
{
  [Serializable]
  public struct Quaterniond : IEquatable<Quaterniond>
  {
    public static readonly Quaterniond Identity = new Quaterniond(0.0, 0.0, 0.0, 1.0);
    private Vector3d xyz;
    private double w;

    [XmlIgnore]
    [Obsolete("Use Xyz property instead.")]
    [CLSCompliant(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Vector3d XYZ
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

    public Vector3d Xyz
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
    public double X
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
    public double Y
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
    public double Z
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

    public double W
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

    public double Length
    {
      get
      {
        return Math.Sqrt(this.W * this.W + this.Xyz.LengthSquared);
      }
    }

    public double LengthSquared
    {
      get
      {
        return this.W * this.W + this.Xyz.LengthSquared;
      }
    }

    static Quaterniond()
    {
    }

    public Quaterniond(Vector3d v, double w)
    {
      this.xyz = v;
      this.w = w;
    }

    public Quaterniond(double x, double y, double z, double w)
    {
      this = new Quaterniond(new Vector3d(x, y, z), w);
    }

    public static Quaterniond operator +(Quaterniond left, Quaterniond right)
    {
      left.Xyz += right.Xyz;
      left.W += right.W;
      return left;
    }

    public static Quaterniond operator -(Quaterniond left, Quaterniond right)
    {
      left.Xyz -= right.Xyz;
      left.W -= right.W;
      return left;
    }

    public static Quaterniond operator *(Quaterniond left, Quaterniond right)
    {
      Quaterniond.Multiply(ref left, ref right, out left);
      return left;
    }

    public static Quaterniond operator *(Quaterniond quaternion, double scale)
    {
      Quaterniond.Multiply(ref quaternion, scale, out quaternion);
      return quaternion;
    }

    public static Quaterniond operator *(double scale, Quaterniond quaternion)
    {
      return new Quaterniond(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
    }

    public static bool operator ==(Quaterniond left, Quaterniond right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Quaterniond left, Quaterniond right)
    {
      return !left.Equals(right);
    }

    public void ToAxisAngle(out Vector3d axis, out double angle)
    {
      Vector4d vector4d = this.ToAxisAngle();
      axis = vector4d.Xyz;
      angle = vector4d.W;
    }

    public Vector4d ToAxisAngle()
    {
      Quaterniond quaterniond = this;
      if (Math.Abs(quaterniond.W) > 1.0)
        quaterniond.Normalize();
      Vector4d vector4d = new Vector4d();
      vector4d.W = 2.0 * Math.Acos(quaterniond.W);
      float num = (float) Math.Sqrt(1.0 - quaterniond.W * quaterniond.W);
      vector4d.Xyz = (double) num <= 9.99999974737875E-05 ? Vector3d.UnitX : quaterniond.Xyz / (double) num;
      return vector4d;
    }

    public void Normalize()
    {
      double num = 1.0 / this.Length;
      this.Xyz *= num;
      this.W *= num;
    }

    public void Conjugate()
    {
      this.Xyz = -this.Xyz;
    }

    public static Quaterniond Add(Quaterniond left, Quaterniond right)
    {
      return new Quaterniond(left.Xyz + right.Xyz, left.W + right.W);
    }

    public static void Add(ref Quaterniond left, ref Quaterniond right, out Quaterniond result)
    {
      result = new Quaterniond(left.Xyz + right.Xyz, left.W + right.W);
    }

    public static Quaterniond Sub(Quaterniond left, Quaterniond right)
    {
      return new Quaterniond(left.Xyz - right.Xyz, left.W - right.W);
    }

    public static void Sub(ref Quaterniond left, ref Quaterniond right, out Quaterniond result)
    {
      result = new Quaterniond(left.Xyz - right.Xyz, left.W - right.W);
    }

    [Obsolete("Use Multiply instead.")]
    public static Quaterniond Mult(Quaterniond left, Quaterniond right)
    {
      return new Quaterniond(right.W * left.Xyz + left.W * right.Xyz + Vector3d.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3d.Dot(left.Xyz, right.Xyz));
    }

    [Obsolete("Use Multiply instead.")]
    public static void Mult(ref Quaterniond left, ref Quaterniond right, out Quaterniond result)
    {
      result = new Quaterniond(right.W * left.Xyz + left.W * right.Xyz + Vector3d.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3d.Dot(left.Xyz, right.Xyz));
    }

    public static Quaterniond Multiply(Quaterniond left, Quaterniond right)
    {
      Quaterniond result;
      Quaterniond.Multiply(ref left, ref right, out result);
      return result;
    }

    public static void Multiply(ref Quaterniond left, ref Quaterniond right, out Quaterniond result)
    {
      result = new Quaterniond(right.W * left.Xyz + left.W * right.Xyz + Vector3d.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3d.Dot(left.Xyz, right.Xyz));
    }

    public static void Multiply(ref Quaterniond quaternion, double scale, out Quaterniond result)
    {
      result = new Quaterniond(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
    }

    public static Quaterniond Multiply(Quaterniond quaternion, double scale)
    {
      return new Quaterniond(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
    }

    public static Quaterniond Conjugate(Quaterniond q)
    {
      return new Quaterniond(-q.Xyz, q.W);
    }

    public static void Conjugate(ref Quaterniond q, out Quaterniond result)
    {
      result = new Quaterniond(-q.Xyz, q.W);
    }

    public static Quaterniond Invert(Quaterniond q)
    {
      Quaterniond result;
      Quaterniond.Invert(ref q, out result);
      return result;
    }

    public static void Invert(ref Quaterniond q, out Quaterniond result)
    {
      double lengthSquared = q.LengthSquared;
      if (lengthSquared != 0.0)
      {
        double num = 1.0 / lengthSquared;
        result = new Quaterniond(q.Xyz * -num, q.W * num);
      }
      else
        result = q;
    }

    public static Quaterniond Normalize(Quaterniond q)
    {
      Quaterniond result;
      Quaterniond.Normalize(ref q, out result);
      return result;
    }

    public static void Normalize(ref Quaterniond q, out Quaterniond result)
    {
      double num = 1.0 / q.Length;
      result = new Quaterniond(q.Xyz * num, q.W * num);
    }

    public static Quaterniond FromAxisAngle(Vector3d axis, double angle)
    {
      if (axis.LengthSquared == 0.0)
        return Quaterniond.Identity;
      Quaterniond q = Quaterniond.Identity;
      angle *= 0.5;
      axis.Normalize();
      q.Xyz = axis * Math.Sin(angle);
      q.W = Math.Cos(angle);
      return Quaterniond.Normalize(q);
    }

    public static Quaterniond Slerp(Quaterniond q1, Quaterniond q2, double blend)
    {
      if (q1.LengthSquared == 0.0)
      {
        if (q2.LengthSquared == 0.0)
          return Quaterniond.Identity;
        else
          return q2;
      }
      else
      {
        if (q2.LengthSquared == 0.0)
          return q1;
        double d = q1.W * q2.W + Vector3d.Dot(q1.Xyz, q2.Xyz);
        if (d >= 1.0 || d <= -1.0)
          return q1;
        if (d < 0.0)
        {
          q2.Xyz = -q2.Xyz;
          q2.W = -q2.W;
          d = -d;
        }
        double num1;
        double num2;
        if (d < 0.990000009536743)
        {
          double a = Math.Acos(d);
          double num3 = 1.0 / Math.Sin(a);
          num1 = Math.Sin(a * (1.0 - blend)) * num3;
          num2 = Math.Sin(a * blend) * num3;
        }
        else
        {
          num1 = 1.0 - blend;
          num2 = blend;
        }
        Quaterniond q = new Quaterniond(num1 * q1.Xyz + num2 * q2.Xyz, num1 * q1.W + num2 * q2.W);
        if (q.LengthSquared > 0.0)
          return Quaterniond.Normalize(q);
        else
          return Quaterniond.Identity;
      }
    }

    public override string ToString()
    {
      return string.Format("V: {0}, W: {1}", (object) this.Xyz, (object) this.W);
    }

    public override bool Equals(object other)
    {
      if (!(other is Quaterniond))
        return false;
      else
        return this == (Quaterniond) other;
    }

    public override int GetHashCode()
    {
      return this.Xyz.GetHashCode() ^ this.W.GetHashCode();
    }

    public bool Equals(Quaterniond other)
    {
      if (this.Xyz == other.Xyz)
        return this.W == other.W;
      else
        return false;
    }
  }
}
