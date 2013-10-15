// Type: SharpDX.Color3
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [DynamicSerializer("TKC3")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Color3 : IEquatable<Color3>, IFormattable, IDataSerializable
  {
    public static readonly Color3 Black = new Color3(0.0f, 0.0f, 0.0f);
    public static readonly Color3 White = new Color3(1f, 1f, 1f);
    public float Red;
    public float Green;
    public float Blue;

    public float this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.Red;
          case 1:
            return this.Green;
          case 2:
            return this.Blue;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Color3 run from 0 to 2, inclusive.");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.Red = value;
            break;
          case 1:
            this.Green = value;
            break;
          case 2:
            this.Blue = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Color3 run from 0 to 2, inclusive.");
        }
      }
    }

    static Color3()
    {
    }

    public Color3(float value)
    {
      this.Red = this.Green = this.Blue = value;
    }

    public Color3(float red, float green, float blue)
    {
      this.Red = red;
      this.Green = green;
      this.Blue = blue;
    }

    public Color3(Vector3 value)
    {
      this.Red = value.X;
      this.Green = value.Y;
      this.Blue = value.Z;
    }

    public Color3(int rgb)
    {
      this.Blue = (float) (rgb >> 16 & (int) byte.MaxValue) / (float) byte.MaxValue;
      this.Green = (float) (rgb >> 8 & (int) byte.MaxValue) / (float) byte.MaxValue;
      this.Red = (float) (rgb & (int) byte.MaxValue) / (float) byte.MaxValue;
    }

    public Color3(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 3)
        throw new ArgumentOutOfRangeException("values", "There must be three and only three input values for Color3.");
      this.Red = values[0];
      this.Green = values[1];
      this.Blue = values[2];
    }

    public static explicit operator Color4(Color3 value)
    {
      return new Color4(value.Red, value.Green, value.Blue, 1f);
    }

    public static explicit operator Vector3(Color3 value)
    {
      return new Vector3(value.Red, value.Green, value.Blue);
    }

    public static explicit operator Color3(Vector3 value)
    {
      return new Color3(value.X, value.Y, value.Z);
    }

    public static explicit operator Color3(int value)
    {
      return new Color3(value);
    }

    public static Color3 operator +(Color3 left, Color3 right)
    {
      return new Color3(left.Red + right.Red, left.Green + right.Green, left.Blue + right.Blue);
    }

    public static Color3 operator +(Color3 value)
    {
      return value;
    }

    public static Color3 operator -(Color3 left, Color3 right)
    {
      return new Color3(left.Red - right.Red, left.Green - right.Green, left.Blue - right.Blue);
    }

    public static Color3 operator -(Color3 value)
    {
      return new Color3(-value.Red, -value.Green, -value.Blue);
    }

    public static Color3 operator *(float scale, Color3 value)
    {
      return new Color3(value.Red * scale, value.Green * scale, value.Blue * scale);
    }

    public static Color3 operator *(Color3 value, float scale)
    {
      return new Color3(value.Red * scale, value.Green * scale, value.Blue * scale);
    }

    public static Color3 operator *(Color3 left, Color3 right)
    {
      return new Color3(left.Red * right.Red, left.Green * right.Green, left.Blue * right.Blue);
    }

    public static bool operator ==(Color3 left, Color3 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Color3 left, Color3 right)
    {
      return !left.Equals(right);
    }

    public int ToRgba()
    {
      return (int) ((uint) ((double) this.Red * (double) byte.MaxValue) & (uint) byte.MaxValue | ((uint) ((double) this.Green * (double) byte.MaxValue) & (uint) byte.MaxValue) << 8 | ((uint) ((double) this.Blue * (double) byte.MaxValue) & (uint) byte.MaxValue) << 16 | (uint) ((int) byte.MaxValue << 24));
    }

    public int ToBgra()
    {
      return (int) ((uint) ((double) this.Blue * (double) byte.MaxValue) & (uint) byte.MaxValue | ((uint) ((double) this.Green * (double) byte.MaxValue) & (uint) byte.MaxValue) << 8 | ((uint) ((double) this.Red * (double) byte.MaxValue) & (uint) byte.MaxValue) << 16 | (uint) ((int) byte.MaxValue << 24));
    }

    public Vector3 ToVector3()
    {
      return new Vector3(this.Red, this.Green, this.Blue);
    }

    public float[] ToArray()
    {
      return new float[3]
      {
        this.Red,
        this.Green,
        this.Blue
      };
    }

    public static void Add(ref Color3 left, ref Color3 right, out Color3 result)
    {
      result.Red = left.Red + right.Red;
      result.Green = left.Green + right.Green;
      result.Blue = left.Blue + right.Blue;
    }

    public static Color3 Add(Color3 left, Color3 right)
    {
      return new Color3(left.Red + right.Red, left.Green + right.Green, left.Blue + right.Blue);
    }

    public static void Subtract(ref Color3 left, ref Color3 right, out Color3 result)
    {
      result.Red = left.Red - right.Red;
      result.Green = left.Green - right.Green;
      result.Blue = left.Blue - right.Blue;
    }

    public static Color3 Subtract(Color3 left, Color3 right)
    {
      return new Color3(left.Red - right.Red, left.Green - right.Green, left.Blue - right.Blue);
    }

    public static void Modulate(ref Color3 left, ref Color3 right, out Color3 result)
    {
      result.Red = left.Red * right.Red;
      result.Green = left.Green * right.Green;
      result.Blue = left.Blue * right.Blue;
    }

    public static Color3 Modulate(Color3 left, Color3 right)
    {
      return new Color3(left.Red * right.Red, left.Green * right.Green, left.Blue * right.Blue);
    }

    public static void Scale(ref Color3 value, float scale, out Color3 result)
    {
      result.Red = value.Red * scale;
      result.Green = value.Green * scale;
      result.Blue = value.Blue * scale;
    }

    public static Color3 Scale(Color3 value, float scale)
    {
      return new Color3(value.Red * scale, value.Green * scale, value.Blue * scale);
    }

    public static void Negate(ref Color3 value, out Color3 result)
    {
      result.Red = 1f - value.Red;
      result.Green = 1f - value.Green;
      result.Blue = 1f - value.Blue;
    }

    public static Color3 Negate(Color3 value)
    {
      return new Color3(1f - value.Red, 1f - value.Green, 1f - value.Blue);
    }

    public static void Clamp(ref Color3 value, ref Color3 min, ref Color3 max, out Color3 result)
    {
      float num1 = value.Red;
      float num2 = (double) num1 > (double) max.Red ? max.Red : num1;
      float red = (double) num2 < (double) min.Red ? min.Red : num2;
      float num3 = value.Green;
      float num4 = (double) num3 > (double) max.Green ? max.Green : num3;
      float green = (double) num4 < (double) min.Green ? min.Green : num4;
      float num5 = value.Blue;
      float num6 = (double) num5 > (double) max.Blue ? max.Blue : num5;
      float blue = (double) num6 < (double) min.Blue ? min.Blue : num6;
      result = new Color3(red, green, blue);
    }

    public static Color3 Clamp(Color3 value, Color3 min, Color3 max)
    {
      Color3 result;
      Color3.Clamp(ref value, ref min, ref max, out result);
      return result;
    }

    public static void Lerp(ref Color3 start, ref Color3 end, float amount, out Color3 result)
    {
      result.Red = start.Red + amount * (end.Red - start.Red);
      result.Green = start.Green + amount * (end.Green - start.Green);
      result.Blue = start.Blue + amount * (end.Blue - start.Blue);
    }

    public static Color3 Lerp(Color3 start, Color3 end, float amount)
    {
      return new Color3(start.Red + amount * (end.Red - start.Red), start.Green + amount * (end.Green - start.Green), start.Blue + amount * (end.Blue - start.Blue));
    }

    public static void SmoothStep(ref Color3 start, ref Color3 end, float amount, out Color3 result)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      result.Red = start.Red + (end.Red - start.Red) * amount;
      result.Green = start.Green + (end.Green - start.Green) * amount;
      result.Blue = start.Blue + (end.Blue - start.Blue) * amount;
    }

    public static Color3 SmoothStep(Color3 start, Color3 end, float amount)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      return new Color3(start.Red + (end.Red - start.Red) * amount, start.Green + (end.Green - start.Green) * amount, start.Blue + (end.Blue - start.Blue) * amount);
    }

    public static void Max(ref Color3 left, ref Color3 right, out Color3 result)
    {
      result.Red = (double) left.Red > (double) right.Red ? left.Red : right.Red;
      result.Green = (double) left.Green > (double) right.Green ? left.Green : right.Green;
      result.Blue = (double) left.Blue > (double) right.Blue ? left.Blue : right.Blue;
    }

    public static Color3 Max(Color3 left, Color3 right)
    {
      Color3 result;
      Color3.Max(ref left, ref right, out result);
      return result;
    }

    public static void Min(ref Color3 left, ref Color3 right, out Color3 result)
    {
      result.Red = (double) left.Red < (double) right.Red ? left.Red : right.Red;
      result.Green = (double) left.Green < (double) right.Green ? left.Green : right.Green;
      result.Blue = (double) left.Blue < (double) right.Blue ? left.Blue : right.Blue;
    }

    public static Color3 Min(Color3 left, Color3 right)
    {
      Color3 result;
      Color3.Min(ref left, ref right, out result);
      return result;
    }

    public static void AdjustContrast(ref Color3 value, float contrast, out Color3 result)
    {
      result.Red = (float) (0.5 + (double) contrast * ((double) value.Red - 0.5));
      result.Green = (float) (0.5 + (double) contrast * ((double) value.Green - 0.5));
      result.Blue = (float) (0.5 + (double) contrast * ((double) value.Blue - 0.5));
    }

    public static Color3 AdjustContrast(Color3 value, float contrast)
    {
      return new Color3((float) (0.5 + (double) contrast * ((double) value.Red - 0.5)), (float) (0.5 + (double) contrast * ((double) value.Green - 0.5)), (float) (0.5 + (double) contrast * ((double) value.Blue - 0.5)));
    }

    public static void AdjustSaturation(ref Color3 value, float saturation, out Color3 result)
    {
      float num = (float) ((double) value.Red * 0.212500005960464 + (double) value.Green * 0.715399980545044 + (double) value.Blue * 0.0720999985933304);
      result.Red = num + saturation * (value.Red - num);
      result.Green = num + saturation * (value.Green - num);
      result.Blue = num + saturation * (value.Blue - num);
    }

    public static Color3 AdjustSaturation(Color3 value, float saturation)
    {
      float num = (float) ((double) value.Red * 0.212500005960464 + (double) value.Green * 0.715399980545044 + (double) value.Blue * 0.0720999985933304);
      return new Color3(num + saturation * (value.Red - num), num + saturation * (value.Green - num), num + saturation * (value.Blue - num));
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Red:{1} Green:{2} Blue:{3}", (object) this.Red, (object) this.Green, (object) this.Blue);
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Red:{1} Green:{2} Blue:{3}", (object) this.Red.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Green.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Blue.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture));
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "Red:{1} Green:{2} Blue:{3}", (object) this.Red, (object) this.Green, (object) this.Blue);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        return this.ToString(formatProvider);
      return string.Format(formatProvider, "Red:{0} Green:{1} Blue:{2}", (object) this.Red.ToString(format, formatProvider), (object) this.Green.ToString(format, formatProvider), (object) this.Blue.ToString(format, formatProvider));
    }

    public override int GetHashCode()
    {
      return this.Red.GetHashCode() + this.Green.GetHashCode() + this.Blue.GetHashCode();
    }

    public bool Equals(Color3 other)
    {
      if ((double) this.Red == (double) other.Red && (double) this.Green == (double) other.Green)
        return (double) this.Blue == (double) other.Blue;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Color3)))
        return false;
      else
        return this.Equals((Color3) value);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.Red);
        serializer.Writer.Write(this.Green);
        serializer.Writer.Write(this.Blue);
      }
      else
      {
        this.Red = serializer.Reader.ReadSingle();
        this.Green = serializer.Reader.ReadSingle();
        this.Blue = serializer.Reader.ReadSingle();
      }
    }
  }
}
