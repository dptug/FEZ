// Type: SharpDX.Color4
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Design;
using SharpDX.Serialization;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [TypeConverter(typeof (Color4Converter))]
  [DynamicSerializer("TKC4")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Color4 : IEquatable<Color4>, IFormattable, IDataSerializable
  {
    public static readonly Color4 Black = new Color4(0.0f, 0.0f, 0.0f, 1f);
    public static readonly Color4 White = new Color4(1f, 1f, 1f, 1f);
    public float Red;
    public float Green;
    public float Blue;
    public float Alpha;

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
          case 3:
            return this.Alpha;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Color4 run from 0 to 3, inclusive.");
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
          case 3:
            this.Alpha = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Color4 run from 0 to 3, inclusive.");
        }
      }
    }

    static Color4()
    {
    }

    public Color4(float value)
    {
      this.Alpha = this.Red = this.Green = this.Blue = value;
    }

    public Color4(float red, float green, float blue, float alpha)
    {
      this.Red = red;
      this.Green = green;
      this.Blue = blue;
      this.Alpha = alpha;
    }

    public Color4(Vector4 value)
    {
      this.Red = value.X;
      this.Green = value.Y;
      this.Blue = value.Z;
      this.Alpha = value.W;
    }

    public Color4(Vector3 value, float alpha)
    {
      this.Red = value.X;
      this.Green = value.Y;
      this.Blue = value.Z;
      this.Alpha = alpha;
    }

    public Color4(uint rgba)
    {
      this.Alpha = (float) (rgba >> 24 & (uint) byte.MaxValue) / (float) byte.MaxValue;
      this.Blue = (float) (rgba >> 16 & (uint) byte.MaxValue) / (float) byte.MaxValue;
      this.Green = (float) (rgba >> 8 & (uint) byte.MaxValue) / (float) byte.MaxValue;
      this.Red = (float) (rgba & (uint) byte.MaxValue) / (float) byte.MaxValue;
    }

    public Color4(int rgba)
    {
      this.Alpha = (float) (rgba >> 24 & (int) byte.MaxValue) / (float) byte.MaxValue;
      this.Blue = (float) (rgba >> 16 & (int) byte.MaxValue) / (float) byte.MaxValue;
      this.Green = (float) (rgba >> 8 & (int) byte.MaxValue) / (float) byte.MaxValue;
      this.Red = (float) (rgba & (int) byte.MaxValue) / (float) byte.MaxValue;
    }

    public Color4(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Color4.");
      this.Red = values[0];
      this.Green = values[1];
      this.Blue = values[2];
      this.Alpha = values[3];
    }

    public Color4(Color3 color)
    {
      this.Red = color.Red;
      this.Green = color.Green;
      this.Blue = color.Blue;
      this.Alpha = 1f;
    }

    public Color4(Color3 color, float alpha)
    {
      this.Red = color.Red;
      this.Green = color.Green;
      this.Blue = color.Blue;
      this.Alpha = alpha;
    }

    public static explicit operator Color3(Color4 value)
    {
      return new Color3(value.Red, value.Green, value.Blue);
    }

    public static explicit operator Vector3(Color4 value)
    {
      return new Vector3(value.Red, value.Green, value.Blue);
    }

    public static explicit operator Vector4(Color4 value)
    {
      return new Vector4(value.Red, value.Green, value.Blue, value.Alpha);
    }

    public static explicit operator Color4(Vector3 value)
    {
      return new Color4(value.X, value.Y, value.Z, 1f);
    }

    public static explicit operator Color4(Vector4 value)
    {
      return new Color4(value.X, value.Y, value.Z, value.W);
    }

    public static explicit operator Color4(ColorBGRA value)
    {
      return new Color4((float) value.R, (float) value.G, (float) value.B, (float) value.A);
    }

    public static explicit operator ColorBGRA(Color4 value)
    {
      return new ColorBGRA(value.Red, value.Green, value.Blue, value.Alpha);
    }

    public static explicit operator int(Color4 value)
    {
      return value.ToRgba();
    }

    public static explicit operator Color4(int value)
    {
      return new Color4(value);
    }

    public static Color4 operator +(Color4 left, Color4 right)
    {
      return new Color4(left.Red + right.Red, left.Green + right.Green, left.Blue + right.Blue, left.Alpha + right.Alpha);
    }

    public static Color4 operator +(Color4 value)
    {
      return value;
    }

    public static Color4 operator -(Color4 left, Color4 right)
    {
      return new Color4(left.Red - right.Red, left.Green - right.Green, left.Blue - right.Blue, left.Alpha - right.Alpha);
    }

    public static Color4 operator -(Color4 value)
    {
      return new Color4(-value.Red, -value.Green, -value.Blue, -value.Alpha);
    }

    public static Color4 operator *(float scale, Color4 value)
    {
      return new Color4(value.Red * scale, value.Green * scale, value.Blue * scale, value.Alpha * scale);
    }

    public static Color4 operator *(Color4 value, float scale)
    {
      return new Color4(value.Red * scale, value.Green * scale, value.Blue * scale, value.Alpha * scale);
    }

    public static Color4 operator *(Color4 left, Color4 right)
    {
      return new Color4(left.Red * right.Red, left.Green * right.Green, left.Blue * right.Blue, left.Alpha * right.Alpha);
    }

    public static bool operator ==(Color4 left, Color4 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Color4 left, Color4 right)
    {
      return !left.Equals(right);
    }

    public int ToBgra()
    {
      return (int) ((uint) ((double) this.Blue * (double) byte.MaxValue) & (uint) byte.MaxValue | ((uint) ((double) this.Green * (double) byte.MaxValue) & (uint) byte.MaxValue) << 8 | ((uint) ((double) this.Red * (double) byte.MaxValue) & (uint) byte.MaxValue) << 16 | ((uint) ((double) this.Alpha * (double) byte.MaxValue) & (uint) byte.MaxValue) << 24);
    }

    public void ToBgra(out byte r, out byte g, out byte b, out byte a)
    {
      a = (byte) ((double) this.Alpha * (double) byte.MaxValue);
      r = (byte) ((double) this.Red * (double) byte.MaxValue);
      g = (byte) ((double) this.Green * (double) byte.MaxValue);
      b = (byte) ((double) this.Blue * (double) byte.MaxValue);
    }

    public int ToRgba()
    {
      return (int) ((uint) ((double) this.Red * (double) byte.MaxValue) & (uint) byte.MaxValue | ((uint) ((double) this.Green * (double) byte.MaxValue) & (uint) byte.MaxValue) << 8 | ((uint) ((double) this.Blue * (double) byte.MaxValue) & (uint) byte.MaxValue) << 16 | ((uint) ((double) this.Alpha * (double) byte.MaxValue) & (uint) byte.MaxValue) << 24);
    }

    public Vector3 ToVector3()
    {
      return new Vector3(this.Red, this.Green, this.Blue);
    }

    public Vector4 ToVector4()
    {
      return new Vector4(this.Red, this.Green, this.Blue, this.Alpha);
    }

    public float[] ToArray()
    {
      return new float[4]
      {
        this.Red,
        this.Green,
        this.Blue,
        this.Alpha
      };
    }

    public static void Add(ref Color4 left, ref Color4 right, out Color4 result)
    {
      result.Alpha = left.Alpha + right.Alpha;
      result.Red = left.Red + right.Red;
      result.Green = left.Green + right.Green;
      result.Blue = left.Blue + right.Blue;
    }

    public static Color4 Add(Color4 left, Color4 right)
    {
      return new Color4(left.Red + right.Red, left.Green + right.Green, left.Blue + right.Blue, left.Alpha + right.Alpha);
    }

    public static void Subtract(ref Color4 left, ref Color4 right, out Color4 result)
    {
      result.Alpha = left.Alpha - right.Alpha;
      result.Red = left.Red - right.Red;
      result.Green = left.Green - right.Green;
      result.Blue = left.Blue - right.Blue;
    }

    public static Color4 Subtract(Color4 left, Color4 right)
    {
      return new Color4(left.Red - right.Red, left.Green - right.Green, left.Blue - right.Blue, left.Alpha - right.Alpha);
    }

    public static void Modulate(ref Color4 left, ref Color4 right, out Color4 result)
    {
      result.Alpha = left.Alpha * right.Alpha;
      result.Red = left.Red * right.Red;
      result.Green = left.Green * right.Green;
      result.Blue = left.Blue * right.Blue;
    }

    public static Color4 Modulate(Color4 left, Color4 right)
    {
      return new Color4(left.Red * right.Red, left.Green * right.Green, left.Blue * right.Blue, left.Alpha * right.Alpha);
    }

    public static void Scale(ref Color4 value, float scale, out Color4 result)
    {
      result.Alpha = value.Alpha * scale;
      result.Red = value.Red * scale;
      result.Green = value.Green * scale;
      result.Blue = value.Blue * scale;
    }

    public static Color4 Scale(Color4 value, float scale)
    {
      return new Color4(value.Red * scale, value.Green * scale, value.Blue * scale, value.Alpha * scale);
    }

    public static void Negate(ref Color4 value, out Color4 result)
    {
      result.Alpha = 1f - value.Alpha;
      result.Red = 1f - value.Red;
      result.Green = 1f - value.Green;
      result.Blue = 1f - value.Blue;
    }

    public static Color4 Negate(Color4 value)
    {
      return new Color4(1f - value.Red, 1f - value.Green, 1f - value.Blue, 1f - value.Alpha);
    }

    public static void Clamp(ref Color4 value, ref Color4 min, ref Color4 max, out Color4 result)
    {
      float num1 = value.Alpha;
      float num2 = (double) num1 > (double) max.Alpha ? max.Alpha : num1;
      float alpha = (double) num2 < (double) min.Alpha ? min.Alpha : num2;
      float num3 = value.Red;
      float num4 = (double) num3 > (double) max.Red ? max.Red : num3;
      float red = (double) num4 < (double) min.Red ? min.Red : num4;
      float num5 = value.Green;
      float num6 = (double) num5 > (double) max.Green ? max.Green : num5;
      float green = (double) num6 < (double) min.Green ? min.Green : num6;
      float num7 = value.Blue;
      float num8 = (double) num7 > (double) max.Blue ? max.Blue : num7;
      float blue = (double) num8 < (double) min.Blue ? min.Blue : num8;
      result = new Color4(red, green, blue, alpha);
    }

    public static Color4 Clamp(Color4 value, Color4 min, Color4 max)
    {
      Color4 result;
      Color4.Clamp(ref value, ref min, ref max, out result);
      return result;
    }

    public static void Lerp(ref Color4 start, ref Color4 end, float amount, out Color4 result)
    {
      result.Alpha = start.Alpha + amount * (end.Alpha - start.Alpha);
      result.Red = start.Red + amount * (end.Red - start.Red);
      result.Green = start.Green + amount * (end.Green - start.Green);
      result.Blue = start.Blue + amount * (end.Blue - start.Blue);
    }

    public static Color4 Lerp(Color4 start, Color4 end, float amount)
    {
      return new Color4(start.Red + amount * (end.Red - start.Red), start.Green + amount * (end.Green - start.Green), start.Blue + amount * (end.Blue - start.Blue), start.Alpha + amount * (end.Alpha - start.Alpha));
    }

    public static void SmoothStep(ref Color4 start, ref Color4 end, float amount, out Color4 result)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      result.Alpha = start.Alpha + (end.Alpha - start.Alpha) * amount;
      result.Red = start.Red + (end.Red - start.Red) * amount;
      result.Green = start.Green + (end.Green - start.Green) * amount;
      result.Blue = start.Blue + (end.Blue - start.Blue) * amount;
    }

    public static Color4 SmoothStep(Color4 start, Color4 end, float amount)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      return new Color4(start.Red + (end.Red - start.Red) * amount, start.Green + (end.Green - start.Green) * amount, start.Blue + (end.Blue - start.Blue) * amount, start.Alpha + (end.Alpha - start.Alpha) * amount);
    }

    public static void Max(ref Color4 left, ref Color4 right, out Color4 result)
    {
      result.Alpha = (double) left.Alpha > (double) right.Alpha ? left.Alpha : right.Alpha;
      result.Red = (double) left.Red > (double) right.Red ? left.Red : right.Red;
      result.Green = (double) left.Green > (double) right.Green ? left.Green : right.Green;
      result.Blue = (double) left.Blue > (double) right.Blue ? left.Blue : right.Blue;
    }

    public static Color4 Max(Color4 left, Color4 right)
    {
      Color4 result;
      Color4.Max(ref left, ref right, out result);
      return result;
    }

    public static void Min(ref Color4 left, ref Color4 right, out Color4 result)
    {
      result.Alpha = (double) left.Alpha < (double) right.Alpha ? left.Alpha : right.Alpha;
      result.Red = (double) left.Red < (double) right.Red ? left.Red : right.Red;
      result.Green = (double) left.Green < (double) right.Green ? left.Green : right.Green;
      result.Blue = (double) left.Blue < (double) right.Blue ? left.Blue : right.Blue;
    }

    public static Color4 Min(Color4 left, Color4 right)
    {
      Color4 result;
      Color4.Min(ref left, ref right, out result);
      return result;
    }

    public static void AdjustContrast(ref Color4 value, float contrast, out Color4 result)
    {
      result.Alpha = value.Alpha;
      result.Red = (float) (0.5 + (double) contrast * ((double) value.Red - 0.5));
      result.Green = (float) (0.5 + (double) contrast * ((double) value.Green - 0.5));
      result.Blue = (float) (0.5 + (double) contrast * ((double) value.Blue - 0.5));
    }

    public static Color4 AdjustContrast(Color4 value, float contrast)
    {
      return new Color4((float) (0.5 + (double) contrast * ((double) value.Red - 0.5)), (float) (0.5 + (double) contrast * ((double) value.Green - 0.5)), (float) (0.5 + (double) contrast * ((double) value.Blue - 0.5)), value.Alpha);
    }

    public static void AdjustSaturation(ref Color4 value, float saturation, out Color4 result)
    {
      float num = (float) ((double) value.Red * 0.212500005960464 + (double) value.Green * 0.715399980545044 + (double) value.Blue * 0.0720999985933304);
      result.Alpha = value.Alpha;
      result.Red = num + saturation * (value.Red - num);
      result.Green = num + saturation * (value.Green - num);
      result.Blue = num + saturation * (value.Blue - num);
    }

    public static Color4 AdjustSaturation(Color4 value, float saturation)
    {
      float num = (float) ((double) value.Red * 0.212500005960464 + (double) value.Green * 0.715399980545044 + (double) value.Blue * 0.0720999985933304);
      return new Color4(num + saturation * (value.Red - num), num + saturation * (value.Green - num), num + saturation * (value.Blue - num), value.Alpha);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Alpha:{0} Red:{1} Green:{2} Blue:{3}", (object) this.Alpha, (object) this.Red, (object) this.Green, (object) this.Blue);
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Alpha:{0} Red:{1} Green:{2} Blue:{3}", (object) this.Alpha.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Red.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Green.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.Blue.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture));
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "Alpha:{0} Red:{1} Green:{2} Blue:{3}", (object) this.Alpha, (object) this.Red, (object) this.Green, (object) this.Blue);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        return this.ToString(formatProvider);
      return string.Format(formatProvider, "Alpha:{0} Red:{1} Green:{2} Blue:{3}", (object) this.Alpha.ToString(format, formatProvider), (object) this.Red.ToString(format, formatProvider), (object) this.Green.ToString(format, formatProvider), (object) this.Blue.ToString(format, formatProvider));
    }

    public override int GetHashCode()
    {
      return this.Alpha.GetHashCode() + this.Red.GetHashCode() + this.Green.GetHashCode() + this.Blue.GetHashCode();
    }

    public bool Equals(Color4 other)
    {
      if ((double) this.Alpha == (double) other.Alpha && (double) this.Red == (double) other.Red && (double) this.Green == (double) other.Green)
        return (double) this.Blue == (double) other.Blue;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Color4)))
        return false;
      else
        return this.Equals((Color4) value);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.Red);
        serializer.Writer.Write(this.Green);
        serializer.Writer.Write(this.Blue);
        serializer.Writer.Write(this.Alpha);
      }
      else
      {
        this.Red = serializer.Reader.ReadSingle();
        this.Green = serializer.Reader.ReadSingle();
        this.Blue = serializer.Reader.ReadSingle();
        this.Alpha = serializer.Reader.ReadSingle();
      }
    }
  }
}
