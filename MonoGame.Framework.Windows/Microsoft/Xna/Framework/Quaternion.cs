// Type: Microsoft.Xna.Framework.Quaternion
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework
{
  [Serializable]
  public struct Quaternion : IEquatable<Quaternion>
  {
    private static Quaternion identity = new Quaternion(0.0f, 0.0f, 0.0f, 1f);
    public float X;
    public float Y;
    public float Z;
    public float W;

    public static Quaternion Identity
    {
      get
      {
        return Quaternion.identity;
      }
    }

    internal Vector3 Xyz
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

    static Quaternion()
    {
    }

    public Quaternion(float x, float y, float z, float w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public Quaternion(Vector3 vectorPart, float scalarPart)
    {
      this.X = vectorPart.X;
      this.Y = vectorPart.Y;
      this.Z = vectorPart.Z;
      this.W = scalarPart;
    }

    public static Quaternion operator +(Quaternion quaternion1, Quaternion quaternion2)
    {
      Quaternion quaternion;
      quaternion.X = quaternion1.X + quaternion2.X;
      quaternion.Y = quaternion1.Y + quaternion2.Y;
      quaternion.Z = quaternion1.Z + quaternion2.Z;
      quaternion.W = quaternion1.W + quaternion2.W;
      return quaternion;
    }

    public static Quaternion operator /(Quaternion quaternion1, Quaternion quaternion2)
    {
      float num1 = quaternion1.X;
      float num2 = quaternion1.Y;
      float num3 = quaternion1.Z;
      float num4 = quaternion1.W;
      float num5 = 1f / (float) ((double) quaternion2.X * (double) quaternion2.X + (double) quaternion2.Y * (double) quaternion2.Y + (double) quaternion2.Z * (double) quaternion2.Z + (double) quaternion2.W * (double) quaternion2.W);
      float num6 = -quaternion2.X * num5;
      float num7 = -quaternion2.Y * num5;
      float num8 = -quaternion2.Z * num5;
      float num9 = quaternion2.W * num5;
      float num10 = (float) ((double) num2 * (double) num8 - (double) num3 * (double) num7);
      float num11 = (float) ((double) num3 * (double) num6 - (double) num1 * (double) num8);
      float num12 = (float) ((double) num1 * (double) num7 - (double) num2 * (double) num6);
      float num13 = (float) ((double) num1 * (double) num6 + (double) num2 * (double) num7 + (double) num3 * (double) num8);
      Quaternion quaternion;
      quaternion.X = (float) ((double) num1 * (double) num9 + (double) num6 * (double) num4) + num10;
      quaternion.Y = (float) ((double) num2 * (double) num9 + (double) num7 * (double) num4) + num11;
      quaternion.Z = (float) ((double) num3 * (double) num9 + (double) num8 * (double) num4) + num12;
      quaternion.W = num4 * num9 - num13;
      return quaternion;
    }

    public static bool operator ==(Quaternion quaternion1, Quaternion quaternion2)
    {
      return (double) quaternion1.X == (double) quaternion2.X && (double) quaternion1.Y == (double) quaternion2.Y && (double) quaternion1.Z == (double) quaternion2.Z && (double) quaternion1.W == (double) quaternion2.W;
    }

    public static bool operator !=(Quaternion quaternion1, Quaternion quaternion2)
    {
      if ((double) quaternion1.X == (double) quaternion2.X && (double) quaternion1.Y == (double) quaternion2.Y && (double) quaternion1.Z == (double) quaternion2.Z)
        return (double) quaternion1.W != (double) quaternion2.W;
      else
        return true;
    }

    public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
    {
      float num1 = quaternion1.X;
      float num2 = quaternion1.Y;
      float num3 = quaternion1.Z;
      float num4 = quaternion1.W;
      float num5 = quaternion2.X;
      float num6 = quaternion2.Y;
      float num7 = quaternion2.Z;
      float num8 = quaternion2.W;
      float num9 = (float) ((double) num2 * (double) num7 - (double) num3 * (double) num6);
      float num10 = (float) ((double) num3 * (double) num5 - (double) num1 * (double) num7);
      float num11 = (float) ((double) num1 * (double) num6 - (double) num2 * (double) num5);
      float num12 = (float) ((double) num1 * (double) num5 + (double) num2 * (double) num6 + (double) num3 * (double) num7);
      Quaternion quaternion;
      quaternion.X = (float) ((double) num1 * (double) num8 + (double) num5 * (double) num4) + num9;
      quaternion.Y = (float) ((double) num2 * (double) num8 + (double) num6 * (double) num4) + num10;
      quaternion.Z = (float) ((double) num3 * (double) num8 + (double) num7 * (double) num4) + num11;
      quaternion.W = num4 * num8 - num12;
      return quaternion;
    }

    public static Quaternion operator *(Quaternion quaternion1, float scaleFactor)
    {
      Quaternion quaternion;
      quaternion.X = quaternion1.X * scaleFactor;
      quaternion.Y = quaternion1.Y * scaleFactor;
      quaternion.Z = quaternion1.Z * scaleFactor;
      quaternion.W = quaternion1.W * scaleFactor;
      return quaternion;
    }

    public static Quaternion operator -(Quaternion quaternion1, Quaternion quaternion2)
    {
      Quaternion quaternion;
      quaternion.X = quaternion1.X - quaternion2.X;
      quaternion.Y = quaternion1.Y - quaternion2.Y;
      quaternion.Z = quaternion1.Z - quaternion2.Z;
      quaternion.W = quaternion1.W - quaternion2.W;
      return quaternion;
    }

    public static Quaternion operator -(Quaternion quaternion)
    {
      Quaternion quaternion1;
      quaternion1.X = -quaternion.X;
      quaternion1.Y = -quaternion.Y;
      quaternion1.Z = -quaternion.Z;
      quaternion1.W = -quaternion.W;
      return quaternion1;
    }

    public static Quaternion Add(Quaternion quaternion1, Quaternion quaternion2)
    {
      Quaternion quaternion;
      quaternion.X = quaternion1.X + quaternion2.X;
      quaternion.Y = quaternion1.Y + quaternion2.Y;
      quaternion.Z = quaternion1.Z + quaternion2.Z;
      quaternion.W = quaternion1.W + quaternion2.W;
      return quaternion;
    }

    public static void Add(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
    {
      result.X = quaternion1.X + quaternion2.X;
      result.Y = quaternion1.Y + quaternion2.Y;
      result.Z = quaternion1.Z + quaternion2.Z;
      result.W = quaternion1.W + quaternion2.W;
    }

    public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
    {
      float num1 = value2.X;
      float num2 = value2.Y;
      float num3 = value2.Z;
      float num4 = value2.W;
      float num5 = value1.X;
      float num6 = value1.Y;
      float num7 = value1.Z;
      float num8 = value1.W;
      float num9 = (float) ((double) num2 * (double) num7 - (double) num3 * (double) num6);
      float num10 = (float) ((double) num3 * (double) num5 - (double) num1 * (double) num7);
      float num11 = (float) ((double) num1 * (double) num6 - (double) num2 * (double) num5);
      float num12 = (float) ((double) num1 * (double) num5 + (double) num2 * (double) num6 + (double) num3 * (double) num7);
      Quaternion quaternion;
      quaternion.X = (float) ((double) num1 * (double) num8 + (double) num5 * (double) num4) + num9;
      quaternion.Y = (float) ((double) num2 * (double) num8 + (double) num6 * (double) num4) + num10;
      quaternion.Z = (float) ((double) num3 * (double) num8 + (double) num7 * (double) num4) + num11;
      quaternion.W = num4 * num8 - num12;
      return quaternion;
    }

    public static void Concatenate(ref Quaternion value1, ref Quaternion value2, out Quaternion result)
    {
      float num1 = value2.X;
      float num2 = value2.Y;
      float num3 = value2.Z;
      float num4 = value2.W;
      float num5 = value1.X;
      float num6 = value1.Y;
      float num7 = value1.Z;
      float num8 = value1.W;
      float num9 = (float) ((double) num2 * (double) num7 - (double) num3 * (double) num6);
      float num10 = (float) ((double) num3 * (double) num5 - (double) num1 * (double) num7);
      float num11 = (float) ((double) num1 * (double) num6 - (double) num2 * (double) num5);
      float num12 = (float) ((double) num1 * (double) num5 + (double) num2 * (double) num6 + (double) num3 * (double) num7);
      result.X = (float) ((double) num1 * (double) num8 + (double) num5 * (double) num4) + num9;
      result.Y = (float) ((double) num2 * (double) num8 + (double) num6 * (double) num4) + num10;
      result.Z = (float) ((double) num3 * (double) num8 + (double) num7 * (double) num4) + num11;
      result.W = num4 * num8 - num12;
    }

    public void Conjugate()
    {
      this.X = -this.X;
      this.Y = -this.Y;
      this.Z = -this.Z;
    }

    public static Quaternion Conjugate(Quaternion value)
    {
      Quaternion quaternion;
      quaternion.X = -value.X;
      quaternion.Y = -value.Y;
      quaternion.Z = -value.Z;
      quaternion.W = value.W;
      return quaternion;
    }

    public static void Conjugate(ref Quaternion value, out Quaternion result)
    {
      result.X = -value.X;
      result.Y = -value.Y;
      result.Z = -value.Z;
      result.W = value.W;
    }

    public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
    {
      float num1 = angle * 0.5f;
      float num2 = (float) Math.Sin((double) num1);
      float num3 = (float) Math.Cos((double) num1);
      Quaternion quaternion;
      quaternion.X = axis.X * num2;
      quaternion.Y = axis.Y * num2;
      quaternion.Z = axis.Z * num2;
      quaternion.W = num3;
      return quaternion;
    }

    public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Quaternion result)
    {
      float num1 = angle * 0.5f;
      float num2 = (float) Math.Sin((double) num1);
      float num3 = (float) Math.Cos((double) num1);
      result.X = axis.X * num2;
      result.Y = axis.Y * num2;
      result.Z = axis.Z * num2;
      result.W = num3;
    }

    public static Quaternion CreateFromRotationMatrix(Matrix matrix)
    {
      float num1 = matrix.M11 + matrix.M22 + matrix.M33;
      Quaternion quaternion = new Quaternion();
      if ((double) num1 > 0.0)
      {
        float num2 = (float) Math.Sqrt((double) num1 + 1.0);
        quaternion.W = num2 * 0.5f;
        float num3 = 0.5f / num2;
        quaternion.X = (matrix.M23 - matrix.M32) * num3;
        quaternion.Y = (matrix.M31 - matrix.M13) * num3;
        quaternion.Z = (matrix.M12 - matrix.M21) * num3;
        return quaternion;
      }
      else if ((double) matrix.M11 >= (double) matrix.M22 && (double) matrix.M11 >= (double) matrix.M33)
      {
        float num2 = (float) Math.Sqrt(1.0 + (double) matrix.M11 - (double) matrix.M22 - (double) matrix.M33);
        float num3 = 0.5f / num2;
        quaternion.X = 0.5f * num2;
        quaternion.Y = (matrix.M12 + matrix.M21) * num3;
        quaternion.Z = (matrix.M13 + matrix.M31) * num3;
        quaternion.W = (matrix.M23 - matrix.M32) * num3;
        return quaternion;
      }
      else if ((double) matrix.M22 > (double) matrix.M33)
      {
        float num2 = (float) Math.Sqrt(1.0 + (double) matrix.M22 - (double) matrix.M11 - (double) matrix.M33);
        float num3 = 0.5f / num2;
        quaternion.X = (matrix.M21 + matrix.M12) * num3;
        quaternion.Y = 0.5f * num2;
        quaternion.Z = (matrix.M32 + matrix.M23) * num3;
        quaternion.W = (matrix.M31 - matrix.M13) * num3;
        return quaternion;
      }
      else
      {
        float num2 = (float) Math.Sqrt(1.0 + (double) matrix.M33 - (double) matrix.M11 - (double) matrix.M22);
        float num3 = 0.5f / num2;
        quaternion.X = (matrix.M31 + matrix.M13) * num3;
        quaternion.Y = (matrix.M32 + matrix.M23) * num3;
        quaternion.Z = 0.5f * num2;
        quaternion.W = (matrix.M12 - matrix.M21) * num3;
        return quaternion;
      }
    }

    public static void CreateFromRotationMatrix(ref Matrix matrix, out Quaternion result)
    {
      float num1 = matrix.M11 + matrix.M22 + matrix.M33;
      if ((double) num1 > 0.0)
      {
        float num2 = (float) Math.Sqrt((double) num1 + 1.0);
        result.W = num2 * 0.5f;
        float num3 = 0.5f / num2;
        result.X = (matrix.M23 - matrix.M32) * num3;
        result.Y = (matrix.M31 - matrix.M13) * num3;
        result.Z = (matrix.M12 - matrix.M21) * num3;
      }
      else if ((double) matrix.M11 >= (double) matrix.M22 && (double) matrix.M11 >= (double) matrix.M33)
      {
        float num2 = (float) Math.Sqrt(1.0 + (double) matrix.M11 - (double) matrix.M22 - (double) matrix.M33);
        float num3 = 0.5f / num2;
        result.X = 0.5f * num2;
        result.Y = (matrix.M12 + matrix.M21) * num3;
        result.Z = (matrix.M13 + matrix.M31) * num3;
        result.W = (matrix.M23 - matrix.M32) * num3;
      }
      else if ((double) matrix.M22 > (double) matrix.M33)
      {
        float num2 = (float) Math.Sqrt(1.0 + (double) matrix.M22 - (double) matrix.M11 - (double) matrix.M33);
        float num3 = 0.5f / num2;
        result.X = (matrix.M21 + matrix.M12) * num3;
        result.Y = 0.5f * num2;
        result.Z = (matrix.M32 + matrix.M23) * num3;
        result.W = (matrix.M31 - matrix.M13) * num3;
      }
      else
      {
        float num2 = (float) Math.Sqrt(1.0 + (double) matrix.M33 - (double) matrix.M11 - (double) matrix.M22);
        float num3 = 0.5f / num2;
        result.X = (matrix.M31 + matrix.M13) * num3;
        result.Y = (matrix.M32 + matrix.M23) * num3;
        result.Z = 0.5f * num2;
        result.W = (matrix.M12 - matrix.M21) * num3;
      }
    }

    public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
    {
      float num1 = roll * 0.5f;
      float num2 = (float) Math.Sin((double) num1);
      float num3 = (float) Math.Cos((double) num1);
      float num4 = pitch * 0.5f;
      float num5 = (float) Math.Sin((double) num4);
      float num6 = (float) Math.Cos((double) num4);
      float num7 = yaw * 0.5f;
      float num8 = (float) Math.Sin((double) num7);
      float num9 = (float) Math.Cos((double) num7);
      Quaternion quaternion;
      quaternion.X = (float) ((double) num9 * (double) num5 * (double) num3 + (double) num8 * (double) num6 * (double) num2);
      quaternion.Y = (float) ((double) num8 * (double) num6 * (double) num3 - (double) num9 * (double) num5 * (double) num2);
      quaternion.Z = (float) ((double) num9 * (double) num6 * (double) num2 - (double) num8 * (double) num5 * (double) num3);
      quaternion.W = (float) ((double) num9 * (double) num6 * (double) num3 + (double) num8 * (double) num5 * (double) num2);
      return quaternion;
    }

    public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Quaternion result)
    {
      float num1 = roll * 0.5f;
      float num2 = (float) Math.Sin((double) num1);
      float num3 = (float) Math.Cos((double) num1);
      float num4 = pitch * 0.5f;
      float num5 = (float) Math.Sin((double) num4);
      float num6 = (float) Math.Cos((double) num4);
      float num7 = yaw * 0.5f;
      float num8 = (float) Math.Sin((double) num7);
      float num9 = (float) Math.Cos((double) num7);
      result.X = (float) ((double) num9 * (double) num5 * (double) num3 + (double) num8 * (double) num6 * (double) num2);
      result.Y = (float) ((double) num8 * (double) num6 * (double) num3 - (double) num9 * (double) num5 * (double) num2);
      result.Z = (float) ((double) num9 * (double) num6 * (double) num2 - (double) num8 * (double) num5 * (double) num3);
      result.W = (float) ((double) num9 * (double) num6 * (double) num3 + (double) num8 * (double) num5 * (double) num2);
    }

    public static Quaternion Divide(Quaternion quaternion1, Quaternion quaternion2)
    {
      float num1 = quaternion1.X;
      float num2 = quaternion1.Y;
      float num3 = quaternion1.Z;
      float num4 = quaternion1.W;
      float num5 = 1f / (float) ((double) quaternion2.X * (double) quaternion2.X + (double) quaternion2.Y * (double) quaternion2.Y + (double) quaternion2.Z * (double) quaternion2.Z + (double) quaternion2.W * (double) quaternion2.W);
      float num6 = -quaternion2.X * num5;
      float num7 = -quaternion2.Y * num5;
      float num8 = -quaternion2.Z * num5;
      float num9 = quaternion2.W * num5;
      float num10 = (float) ((double) num2 * (double) num8 - (double) num3 * (double) num7);
      float num11 = (float) ((double) num3 * (double) num6 - (double) num1 * (double) num8);
      float num12 = (float) ((double) num1 * (double) num7 - (double) num2 * (double) num6);
      float num13 = (float) ((double) num1 * (double) num6 + (double) num2 * (double) num7 + (double) num3 * (double) num8);
      Quaternion quaternion;
      quaternion.X = (float) ((double) num1 * (double) num9 + (double) num6 * (double) num4) + num10;
      quaternion.Y = (float) ((double) num2 * (double) num9 + (double) num7 * (double) num4) + num11;
      quaternion.Z = (float) ((double) num3 * (double) num9 + (double) num8 * (double) num4) + num12;
      quaternion.W = num4 * num9 - num13;
      return quaternion;
    }

    public static void Divide(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
    {
      float num1 = quaternion1.X;
      float num2 = quaternion1.Y;
      float num3 = quaternion1.Z;
      float num4 = quaternion1.W;
      float num5 = 1f / (float) ((double) quaternion2.X * (double) quaternion2.X + (double) quaternion2.Y * (double) quaternion2.Y + (double) quaternion2.Z * (double) quaternion2.Z + (double) quaternion2.W * (double) quaternion2.W);
      float num6 = -quaternion2.X * num5;
      float num7 = -quaternion2.Y * num5;
      float num8 = -quaternion2.Z * num5;
      float num9 = quaternion2.W * num5;
      float num10 = (float) ((double) num2 * (double) num8 - (double) num3 * (double) num7);
      float num11 = (float) ((double) num3 * (double) num6 - (double) num1 * (double) num8);
      float num12 = (float) ((double) num1 * (double) num7 - (double) num2 * (double) num6);
      float num13 = (float) ((double) num1 * (double) num6 + (double) num2 * (double) num7 + (double) num3 * (double) num8);
      result.X = (float) ((double) num1 * (double) num9 + (double) num6 * (double) num4) + num10;
      result.Y = (float) ((double) num2 * (double) num9 + (double) num7 * (double) num4) + num11;
      result.Z = (float) ((double) num3 * (double) num9 + (double) num8 * (double) num4) + num12;
      result.W = num4 * num9 - num13;
    }

    public static float Dot(Quaternion quaternion1, Quaternion quaternion2)
    {
      return (float) ((double) quaternion1.X * (double) quaternion2.X + (double) quaternion1.Y * (double) quaternion2.Y + (double) quaternion1.Z * (double) quaternion2.Z + (double) quaternion1.W * (double) quaternion2.W);
    }

    public static void Dot(ref Quaternion quaternion1, ref Quaternion quaternion2, out float result)
    {
      result = (float) ((double) quaternion1.X * (double) quaternion2.X + (double) quaternion1.Y * (double) quaternion2.Y + (double) quaternion1.Z * (double) quaternion2.Z + (double) quaternion1.W * (double) quaternion2.W);
    }

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Quaternion)
        flag = this.Equals((Quaternion) obj);
      return flag;
    }

    public bool Equals(Quaternion other)
    {
      return (double) this.X == (double) other.X && (double) this.Y == (double) other.Y && (double) this.Z == (double) other.Z && (double) this.W == (double) other.W;
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode() + this.W.GetHashCode();
    }

    public static Quaternion Inverse(Quaternion quaternion)
    {
      float num = 1f / (float) ((double) quaternion.X * (double) quaternion.X + (double) quaternion.Y * (double) quaternion.Y + (double) quaternion.Z * (double) quaternion.Z + (double) quaternion.W * (double) quaternion.W);
      Quaternion quaternion1;
      quaternion1.X = -quaternion.X * num;
      quaternion1.Y = -quaternion.Y * num;
      quaternion1.Z = -quaternion.Z * num;
      quaternion1.W = quaternion.W * num;
      return quaternion1;
    }

    public static void Inverse(ref Quaternion quaternion, out Quaternion result)
    {
      float num = 1f / (float) ((double) quaternion.X * (double) quaternion.X + (double) quaternion.Y * (double) quaternion.Y + (double) quaternion.Z * (double) quaternion.Z + (double) quaternion.W * (double) quaternion.W);
      result.X = -quaternion.X * num;
      result.Y = -quaternion.Y * num;
      result.Z = -quaternion.Z * num;
      result.W = quaternion.W * num;
    }

    public float Length()
    {
      return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
    }

    public float LengthSquared()
    {
      return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
    }

    public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
    {
      float num1 = amount;
      float num2 = 1f - num1;
      Quaternion quaternion = new Quaternion();
      if ((double) quaternion1.X * (double) quaternion2.X + (double) quaternion1.Y * (double) quaternion2.Y + (double) quaternion1.Z * (double) quaternion2.Z + (double) quaternion1.W * (double) quaternion2.W >= 0.0)
      {
        quaternion.X = (float) ((double) num2 * (double) quaternion1.X + (double) num1 * (double) quaternion2.X);
        quaternion.Y = (float) ((double) num2 * (double) quaternion1.Y + (double) num1 * (double) quaternion2.Y);
        quaternion.Z = (float) ((double) num2 * (double) quaternion1.Z + (double) num1 * (double) quaternion2.Z);
        quaternion.W = (float) ((double) num2 * (double) quaternion1.W + (double) num1 * (double) quaternion2.W);
      }
      else
      {
        quaternion.X = (float) ((double) num2 * (double) quaternion1.X - (double) num1 * (double) quaternion2.X);
        quaternion.Y = (float) ((double) num2 * (double) quaternion1.Y - (double) num1 * (double) quaternion2.Y);
        quaternion.Z = (float) ((double) num2 * (double) quaternion1.Z - (double) num1 * (double) quaternion2.Z);
        quaternion.W = (float) ((double) num2 * (double) quaternion1.W - (double) num1 * (double) quaternion2.W);
      }
      float num3 = 1f / (float) Math.Sqrt((double) quaternion.X * (double) quaternion.X + (double) quaternion.Y * (double) quaternion.Y + (double) quaternion.Z * (double) quaternion.Z + (double) quaternion.W * (double) quaternion.W);
      quaternion.X *= num3;
      quaternion.Y *= num3;
      quaternion.Z *= num3;
      quaternion.W *= num3;
      return quaternion;
    }

    public static void Lerp(ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result)
    {
      float num1 = amount;
      float num2 = 1f - num1;
      if ((double) quaternion1.X * (double) quaternion2.X + (double) quaternion1.Y * (double) quaternion2.Y + (double) quaternion1.Z * (double) quaternion2.Z + (double) quaternion1.W * (double) quaternion2.W >= 0.0)
      {
        result.X = (float) ((double) num2 * (double) quaternion1.X + (double) num1 * (double) quaternion2.X);
        result.Y = (float) ((double) num2 * (double) quaternion1.Y + (double) num1 * (double) quaternion2.Y);
        result.Z = (float) ((double) num2 * (double) quaternion1.Z + (double) num1 * (double) quaternion2.Z);
        result.W = (float) ((double) num2 * (double) quaternion1.W + (double) num1 * (double) quaternion2.W);
      }
      else
      {
        result.X = (float) ((double) num2 * (double) quaternion1.X - (double) num1 * (double) quaternion2.X);
        result.Y = (float) ((double) num2 * (double) quaternion1.Y - (double) num1 * (double) quaternion2.Y);
        result.Z = (float) ((double) num2 * (double) quaternion1.Z - (double) num1 * (double) quaternion2.Z);
        result.W = (float) ((double) num2 * (double) quaternion1.W - (double) num1 * (double) quaternion2.W);
      }
      float num3 = 1f / (float) Math.Sqrt((double) result.X * (double) result.X + (double) result.Y * (double) result.Y + (double) result.Z * (double) result.Z + (double) result.W * (double) result.W);
      result.X *= num3;
      result.Y *= num3;
      result.Z *= num3;
      result.W *= num3;
    }

    public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
    {
      float num1 = amount;
      float num2 = (float) ((double) quaternion1.X * (double) quaternion2.X + (double) quaternion1.Y * (double) quaternion2.Y + (double) quaternion1.Z * (double) quaternion2.Z + (double) quaternion1.W * (double) quaternion2.W);
      bool flag = false;
      if ((double) num2 < 0.0)
      {
        flag = true;
        num2 = -num2;
      }
      float num3;
      float num4;
      if ((double) num2 > 0.999998986721039)
      {
        num3 = 1f - num1;
        num4 = flag ? -num1 : num1;
      }
      else
      {
        float num5 = (float) Math.Acos((double) num2);
        float num6 = (float) (1.0 / Math.Sin((double) num5));
        num3 = (float) Math.Sin((1.0 - (double) num1) * (double) num5) * num6;
        num4 = flag ? (float) -Math.Sin((double) num1 * (double) num5) * num6 : (float) Math.Sin((double) num1 * (double) num5) * num6;
      }
      Quaternion quaternion;
      quaternion.X = (float) ((double) num3 * (double) quaternion1.X + (double) num4 * (double) quaternion2.X);
      quaternion.Y = (float) ((double) num3 * (double) quaternion1.Y + (double) num4 * (double) quaternion2.Y);
      quaternion.Z = (float) ((double) num3 * (double) quaternion1.Z + (double) num4 * (double) quaternion2.Z);
      quaternion.W = (float) ((double) num3 * (double) quaternion1.W + (double) num4 * (double) quaternion2.W);
      return quaternion;
    }

    public static void Slerp(ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result)
    {
      float num1 = amount;
      float num2 = (float) ((double) quaternion1.X * (double) quaternion2.X + (double) quaternion1.Y * (double) quaternion2.Y + (double) quaternion1.Z * (double) quaternion2.Z + (double) quaternion1.W * (double) quaternion2.W);
      bool flag = false;
      if ((double) num2 < 0.0)
      {
        flag = true;
        num2 = -num2;
      }
      float num3;
      float num4;
      if ((double) num2 > 0.999998986721039)
      {
        num3 = 1f - num1;
        num4 = flag ? -num1 : num1;
      }
      else
      {
        float num5 = (float) Math.Acos((double) num2);
        float num6 = (float) (1.0 / Math.Sin((double) num5));
        num3 = (float) Math.Sin((1.0 - (double) num1) * (double) num5) * num6;
        num4 = flag ? (float) -Math.Sin((double) num1 * (double) num5) * num6 : (float) Math.Sin((double) num1 * (double) num5) * num6;
      }
      result.X = (float) ((double) num3 * (double) quaternion1.X + (double) num4 * (double) quaternion2.X);
      result.Y = (float) ((double) num3 * (double) quaternion1.Y + (double) num4 * (double) quaternion2.Y);
      result.Z = (float) ((double) num3 * (double) quaternion1.Z + (double) num4 * (double) quaternion2.Z);
      result.W = (float) ((double) num3 * (double) quaternion1.W + (double) num4 * (double) quaternion2.W);
    }

    public static Quaternion Subtract(Quaternion quaternion1, Quaternion quaternion2)
    {
      Quaternion quaternion;
      quaternion.X = quaternion1.X - quaternion2.X;
      quaternion.Y = quaternion1.Y - quaternion2.Y;
      quaternion.Z = quaternion1.Z - quaternion2.Z;
      quaternion.W = quaternion1.W - quaternion2.W;
      return quaternion;
    }

    public static void Subtract(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
    {
      result.X = quaternion1.X - quaternion2.X;
      result.Y = quaternion1.Y - quaternion2.Y;
      result.Z = quaternion1.Z - quaternion2.Z;
      result.W = quaternion1.W - quaternion2.W;
    }

    public static Quaternion Multiply(Quaternion quaternion1, Quaternion quaternion2)
    {
      float num1 = quaternion1.X;
      float num2 = quaternion1.Y;
      float num3 = quaternion1.Z;
      float num4 = quaternion1.W;
      float num5 = quaternion2.X;
      float num6 = quaternion2.Y;
      float num7 = quaternion2.Z;
      float num8 = quaternion2.W;
      float num9 = (float) ((double) num2 * (double) num7 - (double) num3 * (double) num6);
      float num10 = (float) ((double) num3 * (double) num5 - (double) num1 * (double) num7);
      float num11 = (float) ((double) num1 * (double) num6 - (double) num2 * (double) num5);
      float num12 = (float) ((double) num1 * (double) num5 + (double) num2 * (double) num6 + (double) num3 * (double) num7);
      Quaternion quaternion;
      quaternion.X = (float) ((double) num1 * (double) num8 + (double) num5 * (double) num4) + num9;
      quaternion.Y = (float) ((double) num2 * (double) num8 + (double) num6 * (double) num4) + num10;
      quaternion.Z = (float) ((double) num3 * (double) num8 + (double) num7 * (double) num4) + num11;
      quaternion.W = num4 * num8 - num12;
      return quaternion;
    }

    public static Quaternion Multiply(Quaternion quaternion1, float scaleFactor)
    {
      Quaternion quaternion;
      quaternion.X = quaternion1.X * scaleFactor;
      quaternion.Y = quaternion1.Y * scaleFactor;
      quaternion.Z = quaternion1.Z * scaleFactor;
      quaternion.W = quaternion1.W * scaleFactor;
      return quaternion;
    }

    public static void Multiply(ref Quaternion quaternion1, float scaleFactor, out Quaternion result)
    {
      result.X = quaternion1.X * scaleFactor;
      result.Y = quaternion1.Y * scaleFactor;
      result.Z = quaternion1.Z * scaleFactor;
      result.W = quaternion1.W * scaleFactor;
    }

    public static void Multiply(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
    {
      float num1 = quaternion1.X;
      float num2 = quaternion1.Y;
      float num3 = quaternion1.Z;
      float num4 = quaternion1.W;
      float num5 = quaternion2.X;
      float num6 = quaternion2.Y;
      float num7 = quaternion2.Z;
      float num8 = quaternion2.W;
      float num9 = (float) ((double) num2 * (double) num7 - (double) num3 * (double) num6);
      float num10 = (float) ((double) num3 * (double) num5 - (double) num1 * (double) num7);
      float num11 = (float) ((double) num1 * (double) num6 - (double) num2 * (double) num5);
      float num12 = (float) ((double) num1 * (double) num5 + (double) num2 * (double) num6 + (double) num3 * (double) num7);
      result.X = (float) ((double) num1 * (double) num8 + (double) num5 * (double) num4) + num9;
      result.Y = (float) ((double) num2 * (double) num8 + (double) num6 * (double) num4) + num10;
      result.Z = (float) ((double) num3 * (double) num8 + (double) num7 * (double) num4) + num11;
      result.W = num4 * num8 - num12;
    }

    public static Quaternion Negate(Quaternion quaternion)
    {
      Quaternion quaternion1;
      quaternion1.X = -quaternion.X;
      quaternion1.Y = -quaternion.Y;
      quaternion1.Z = -quaternion.Z;
      quaternion1.W = -quaternion.W;
      return quaternion1;
    }

    public static void Negate(ref Quaternion quaternion, out Quaternion result)
    {
      result.X = -quaternion.X;
      result.Y = -quaternion.Y;
      result.Z = -quaternion.Z;
      result.W = -quaternion.W;
    }

    public void Normalize()
    {
      float num = 1f / (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
      this.X *= num;
      this.Y *= num;
      this.Z *= num;
      this.W *= num;
    }

    public static Quaternion Normalize(Quaternion quaternion)
    {
      float num = 1f / (float) Math.Sqrt((double) quaternion.X * (double) quaternion.X + (double) quaternion.Y * (double) quaternion.Y + (double) quaternion.Z * (double) quaternion.Z + (double) quaternion.W * (double) quaternion.W);
      Quaternion quaternion1;
      quaternion1.X = quaternion.X * num;
      quaternion1.Y = quaternion.Y * num;
      quaternion1.Z = quaternion.Z * num;
      quaternion1.W = quaternion.W * num;
      return quaternion1;
    }

    public static void Normalize(ref Quaternion quaternion, out Quaternion result)
    {
      float num = 1f / (float) Math.Sqrt((double) quaternion.X * (double) quaternion.X + (double) quaternion.Y * (double) quaternion.Y + (double) quaternion.Z * (double) quaternion.Z + (double) quaternion.W * (double) quaternion.W);
      result.X = quaternion.X * num;
      result.Y = quaternion.Y * num;
      result.Z = quaternion.Z * num;
      result.W = quaternion.W * num;
    }

    public override string ToString()
    {
      throw new NotImplementedException();
    }

    internal Matrix ToMatrix()
    {
      Matrix matrix = Matrix.Identity;
      this.ToMatrix(out matrix);
      return matrix;
    }

    internal void ToMatrix(out Matrix matrix)
    {
      Quaternion.ToMatrix(this, out matrix);
    }

    internal static void ToMatrix(Quaternion quaternion, out Matrix matrix)
    {
      float num1 = quaternion.X * quaternion.X;
      float num2 = quaternion.Y * quaternion.Y;
      float num3 = quaternion.Z * quaternion.Z;
      float num4 = quaternion.X * quaternion.Y;
      float num5 = quaternion.X * quaternion.Z;
      float num6 = quaternion.Y * quaternion.Z;
      float num7 = quaternion.W * quaternion.X;
      float num8 = quaternion.W * quaternion.Y;
      float num9 = quaternion.W * quaternion.Z;
      matrix.M11 = (float) (1.0 - 2.0 * ((double) num2 + (double) num3));
      matrix.M12 = (float) (2.0 * ((double) num4 - (double) num9));
      matrix.M13 = (float) (2.0 * ((double) num5 + (double) num8));
      matrix.M14 = 0.0f;
      matrix.M21 = (float) (2.0 * ((double) num4 + (double) num9));
      matrix.M22 = (float) (1.0 - 2.0 * ((double) num1 + (double) num3));
      matrix.M23 = (float) (2.0 * ((double) num6 - (double) num7));
      matrix.M24 = 0.0f;
      matrix.M31 = (float) (2.0 * ((double) num5 - (double) num8));
      matrix.M32 = (float) (2.0 * ((double) num6 + (double) num7));
      matrix.M33 = (float) (1.0 - 2.0 * ((double) num1 + (double) num2));
      matrix.M34 = 0.0f;
      matrix.M41 = (float) (2.0 * ((double) num5 - (double) num8));
      matrix.M42 = (float) (2.0 * ((double) num6 + (double) num7));
      matrix.M43 = (float) (1.0 - 2.0 * ((double) num1 + (double) num2));
      matrix.M44 = 0.0f;
    }
  }
}
