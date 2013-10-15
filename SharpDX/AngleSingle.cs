// Type: SharpDX.AngleSingle
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [Serializable]
  [StructLayout(LayoutKind.Explicit)]
  public struct AngleSingle : IComparable, IComparable<AngleSingle>, IEquatable<AngleSingle>, IFormattable, IDataSerializable
  {
    public const float Degree = 0.002777778f;
    public const float Minute = 4.62963E-05f;
    public const float Second = 7.71605E-07f;
    public const float Radian = 0.1591549f;
    public const float Milliradian = 0.0001591549f;
    public const float Gradian = 0.0025f;
    [FieldOffset(0)]
    private float radians;
    [FieldOffset(0)]
    private int radiansInt;

    public float Revolutions
    {
      get
      {
        return MathUtil.RadiansToRevolutions(this.radians);
      }
      set
      {
        this.radians = MathUtil.RevolutionsToRadians(value);
      }
    }

    public float Degrees
    {
      get
      {
        return MathUtil.RadiansToDegrees(this.radians);
      }
      set
      {
        this.radians = MathUtil.DegreesToRadians(value);
      }
    }

    public float Minutes
    {
      get
      {
        float num1 = MathUtil.RadiansToDegrees(this.radians);
        if ((double) num1 < 0.0)
        {
          float num2 = (float) Math.Ceiling((double) num1);
          return (float) (((double) num1 - (double) num2) * 60.0);
        }
        else
        {
          float num2 = (float) Math.Floor((double) num1);
          return (float) (((double) num1 - (double) num2) * 60.0);
        }
      }
      set
      {
        this.radians = MathUtil.DegreesToRadians((float) Math.Floor((double) MathUtil.RadiansToDegrees(this.radians)) + value / 60f);
      }
    }

    public float Seconds
    {
      get
      {
        float num1 = MathUtil.RadiansToDegrees(this.radians);
        if ((double) num1 < 0.0)
        {
          float num2 = (float) Math.Ceiling((double) num1);
          float num3 = (float) (((double) num1 - (double) num2) * 60.0);
          float num4 = (float) Math.Ceiling((double) num3);
          return (float) (((double) num3 - (double) num4) * 60.0);
        }
        else
        {
          float num2 = (float) Math.Floor((double) num1);
          float num3 = (float) (((double) num1 - (double) num2) * 60.0);
          float num4 = (float) Math.Floor((double) num3);
          return (float) (((double) num3 - (double) num4) * 60.0);
        }
      }
      set
      {
        float num1 = MathUtil.RadiansToDegrees(this.radians);
        float num2 = (float) Math.Floor((double) num1);
        float num3 = (float) Math.Floor(((double) num1 - (double) num2) * 60.0) + value / 60f;
        this.radians = MathUtil.DegreesToRadians(num2 + num3 / 60f);
      }
    }

    public float Radians
    {
      get
      {
        return this.radians;
      }
      set
      {
        this.radians = value;
      }
    }

    public float Milliradians
    {
      get
      {
        // ISSUE: unable to decompile the method.
      }
      set
      {
        this.radians = value * (1.0 / 1000.0);
      }
    }

    public float Gradians
    {
      get
      {
        return MathUtil.RadiansToGradians(this.radians);
      }
      set
      {
        this.radians = MathUtil.RadiansToGradians(value);
      }
    }

    public bool IsRight
    {
      get
      {
        return (double) this.radians == 1.57079637050629;
      }
    }

    public bool IsStraight
    {
      get
      {
        return (double) this.radians == 3.14159274101257;
      }
    }

    public bool IsFullRotation
    {
      get
      {
        return (double) this.radians == 6.28318548202515;
      }
    }

    public bool IsOblique
    {
      get
      {
        return (double) AngleSingle.WrapPositive(this).radians != 1.57079637050629;
      }
    }

    public bool IsAcute
    {
      get
      {
        if ((double) this.radians > 0.0)
          return (double) this.radians < 1.57079637050629;
        else
          return false;
      }
    }

    public bool IsObtuse
    {
      get
      {
        if ((double) this.radians > 1.57079637050629)
          return (double) this.radians < 3.14159274101257;
        else
          return false;
      }
    }

    public bool IsReflex
    {
      get
      {
        if ((double) this.radians > 3.14159274101257)
          return (double) this.radians < 6.28318548202515;
        else
          return false;
      }
    }

    public AngleSingle Complement
    {
      get
      {
        return new AngleSingle(1.570796f - this.radians, AngleType.Radian);
      }
    }

    public AngleSingle Supplement
    {
      get
      {
        return new AngleSingle(3.141593f - this.radians, AngleType.Radian);
      }
    }

    public static AngleSingle ZeroAngle
    {
      get
      {
        return new AngleSingle(0.0f, AngleType.Radian);
      }
    }

    public static AngleSingle RightAngle
    {
      get
      {
        return new AngleSingle(1.570796f, AngleType.Radian);
      }
    }

    public static AngleSingle StraightAngle
    {
      get
      {
        return new AngleSingle(3.141593f, AngleType.Radian);
      }
    }

    public static AngleSingle FullRotationAngle
    {
      get
      {
        return new AngleSingle(6.283185f, AngleType.Radian);
      }
    }

    public AngleSingle(float angle, AngleType type)
    {
      this.radiansInt = 0;
      switch (type)
      {
        case AngleType.Revolution:
          this.radians = MathUtil.RevolutionsToRadians(angle);
          break;
        case AngleType.Degree:
          this.radians = MathUtil.DegreesToRadians(angle);
          break;
        case AngleType.Radian:
          this.radians = angle;
          break;
        case AngleType.Gradian:
          this.radians = MathUtil.GradiansToRadians(angle);
          break;
        default:
          this.radians = 0.0f;
          break;
      }
    }

    public AngleSingle(float arcLength, float radius)
    {
      this.radiansInt = 0;
      this.radians = arcLength / radius;
    }

    public static bool operator ==(AngleSingle left, AngleSingle right)
    {
      return (double) left.radians == (double) right.radians;
    }

    public static bool operator !=(AngleSingle left, AngleSingle right)
    {
      return (double) left.radians != (double) right.radians;
    }

    public static bool operator <(AngleSingle left, AngleSingle right)
    {
      return (double) left.radians < (double) right.radians;
    }

    public static bool operator >(AngleSingle left, AngleSingle right)
    {
      return (double) left.radians > (double) right.radians;
    }

    public static bool operator <=(AngleSingle left, AngleSingle right)
    {
      return (double) left.radians <= (double) right.radians;
    }

    public static bool operator >=(AngleSingle left, AngleSingle right)
    {
      return (double) left.radians >= (double) right.radians;
    }

    public static AngleSingle operator +(AngleSingle value)
    {
      return value;
    }

    public static AngleSingle operator -(AngleSingle value)
    {
      return new AngleSingle(-value.radians, AngleType.Radian);
    }

    public static AngleSingle operator +(AngleSingle left, AngleSingle right)
    {
      return new AngleSingle(left.radians + right.radians, AngleType.Radian);
    }

    public static AngleSingle operator -(AngleSingle left, AngleSingle right)
    {
      return new AngleSingle(left.radians - right.radians, AngleType.Radian);
    }

    public static AngleSingle operator *(AngleSingle left, AngleSingle right)
    {
      return new AngleSingle(left.radians * right.radians, AngleType.Radian);
    }

    public static AngleSingle operator /(AngleSingle left, AngleSingle right)
    {
      return new AngleSingle(left.radians / right.radians, AngleType.Radian);
    }

    public void Wrap()
    {
      float num = (float) Math.IEEERemainder((double) this.radians, 6.28318548202515);
      if ((double) num <= -3.14159274101257)
        num += 6.283185f;
      else if ((double) num > 3.14159274101257)
        num -= 6.283185f;
      this.radians = num;
    }

    public void WrapPositive()
    {
      float num = this.radians % 6.283185f;
      if ((double) num < 0.0)
        num += 6.283185f;
      this.radians = num;
    }

    public static AngleSingle Wrap(AngleSingle value)
    {
      value.Wrap();
      return value;
    }

    public static AngleSingle WrapPositive(AngleSingle value)
    {
      value.WrapPositive();
      return value;
    }

    public static AngleSingle Min(AngleSingle left, AngleSingle right)
    {
      if ((double) left.radians < (double) right.radians)
        return left;
      else
        return right;
    }

    public static AngleSingle Max(AngleSingle left, AngleSingle right)
    {
      if ((double) left.radians > (double) right.radians)
        return left;
      else
        return right;
    }

    public static AngleSingle Add(AngleSingle left, AngleSingle right)
    {
      return new AngleSingle(left.radians + right.radians, AngleType.Radian);
    }

    public static AngleSingle Subtract(AngleSingle left, AngleSingle right)
    {
      return new AngleSingle(left.radians - right.radians, AngleType.Radian);
    }

    public static AngleSingle Multiply(AngleSingle left, AngleSingle right)
    {
      return new AngleSingle(left.radians * right.radians, AngleType.Radian);
    }

    public static AngleSingle Divide(AngleSingle left, AngleSingle right)
    {
      return new AngleSingle(left.radians / right.radians, AngleType.Radian);
    }

    public int CompareTo(object other)
    {
      if (other == null)
        return 1;
      if (!(other is AngleSingle))
        throw new ArgumentException("Argument must be of type Angle.", "other");
      float num = ((AngleSingle) other).radians;
      if ((double) this.radians > (double) num)
        return 1;
      return (double) this.radians < (double) num ? -1 : 0;
    }

    public int CompareTo(AngleSingle other)
    {
      if ((double) this.radians > (double) other.radians)
        return 1;
      return (double) this.radians < (double) other.radians ? -1 : 0;
    }

    public bool Equals(AngleSingle other)
    {
      return this == other;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, MathUtil.RadiansToDegrees(this.radians).ToString("0.##°"), new object[0]);
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}°", new object[1]
      {
        (object) MathUtil.RadiansToDegrees(this.radians).ToString(format, (IFormatProvider) CultureInfo.CurrentCulture)
      });
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, MathUtil.RadiansToDegrees(this.radians).ToString("0.##°"), new object[0]);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        return this.ToString(formatProvider);
      return string.Format(formatProvider, "{0}°", new object[1]
      {
        (object) MathUtil.RadiansToDegrees(this.radians).ToString(format, (IFormatProvider) CultureInfo.CurrentCulture)
      });
    }

    public override int GetHashCode()
    {
      return this.radiansInt;
    }

    public override bool Equals(object obj)
    {
      if (obj is AngleSingle)
        return this == (AngleSingle) obj;
      else
        return false;
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
        serializer.Writer.Write(this.radiansInt);
      else
        this.radiansInt = serializer.Reader.ReadInt32();
    }
  }
}
