// Type: SharpDX.ColorBGRA
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [DynamicSerializer("TKC0")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Size = 4)]
  public struct ColorBGRA : IEquatable<ColorBGRA>, IFormattable, IDataSerializable
  {
    public byte B;
    public byte G;
    public byte R;
    public byte A;

    public byte this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.B;
          case 1:
            return this.G;
          case 2:
            return this.R;
          case 3:
            return this.A;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for ColorBGRA run from 0 to 3, inclusive.");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.B = value;
            break;
          case 1:
            this.G = value;
            break;
          case 2:
            this.R = value;
            break;
          case 3:
            this.A = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for ColorBGRA run from 0 to 3, inclusive.");
        }
      }
    }

    public ColorBGRA(byte value)
    {
      this.A = this.R = this.G = this.B = value;
    }

    public ColorBGRA(float value)
    {
      this.A = this.R = this.G = this.B = ColorBGRA.ToByte(value);
    }

    public ColorBGRA(byte red, byte green, byte blue, byte alpha)
    {
      this.R = red;
      this.G = green;
      this.B = blue;
      this.A = alpha;
    }

    public ColorBGRA(float red, float green, float blue, float alpha)
    {
      this.R = ColorBGRA.ToByte(red);
      this.G = ColorBGRA.ToByte(green);
      this.B = ColorBGRA.ToByte(blue);
      this.A = ColorBGRA.ToByte(alpha);
    }

    public ColorBGRA(Vector4 value)
    {
      this.R = ColorBGRA.ToByte(value.X);
      this.G = ColorBGRA.ToByte(value.Y);
      this.B = ColorBGRA.ToByte(value.Z);
      this.A = ColorBGRA.ToByte(value.W);
    }

    public ColorBGRA(Vector3 value, float alpha)
    {
      this.R = ColorBGRA.ToByte(value.X);
      this.G = ColorBGRA.ToByte(value.Y);
      this.B = ColorBGRA.ToByte(value.Z);
      this.A = ColorBGRA.ToByte(alpha);
    }

    public ColorBGRA(uint bgra)
    {
      this.A = (byte) (bgra >> 24 & (uint) byte.MaxValue);
      this.R = (byte) (bgra >> 16 & (uint) byte.MaxValue);
      this.G = (byte) (bgra >> 8 & (uint) byte.MaxValue);
      this.B = (byte) (bgra & (uint) byte.MaxValue);
    }

    public ColorBGRA(int bgra)
    {
      this.A = (byte) (bgra >> 24 & (int) byte.MaxValue);
      this.R = (byte) (bgra >> 16 & (int) byte.MaxValue);
      this.G = (byte) (bgra >> 8 & (int) byte.MaxValue);
      this.B = (byte) (bgra & (int) byte.MaxValue);
    }

    public ColorBGRA(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for ColorBGRA.");
      this.B = ColorBGRA.ToByte(values[0]);
      this.G = ColorBGRA.ToByte(values[1]);
      this.R = ColorBGRA.ToByte(values[2]);
      this.A = ColorBGRA.ToByte(values[3]);
    }

    public ColorBGRA(byte[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for ColorBGRA.");
      this.B = values[0];
      this.G = values[1];
      this.R = values[2];
      this.A = values[3];
    }

    public static explicit operator Color3(ColorBGRA value)
    {
      return new Color3((float) value.R, (float) value.G, (float) value.B);
    }

    public static explicit operator Vector3(ColorBGRA value)
    {
      return new Vector3((float) value.R / (float) byte.MaxValue, (float) value.G / (float) byte.MaxValue, (float) value.B / (float) byte.MaxValue);
    }

    public static explicit operator Vector4(ColorBGRA value)
    {
      return new Vector4((float) value.R / (float) byte.MaxValue, (float) value.G / (float) byte.MaxValue, (float) value.B / (float) byte.MaxValue, (float) value.A / (float) byte.MaxValue);
    }

    public static explicit operator Color4(ColorBGRA value)
    {
      return new Color4((float) value.R / (float) byte.MaxValue, (float) value.G / (float) byte.MaxValue, (float) value.B / (float) byte.MaxValue, (float) value.A / (float) byte.MaxValue);
    }

    public static explicit operator ColorBGRA(Vector3 value)
    {
      return new ColorBGRA(value.X / (float) byte.MaxValue, value.Y / (float) byte.MaxValue, value.Z / (float) byte.MaxValue, 1f);
    }

    public static explicit operator ColorBGRA(Color3 value)
    {
      return new ColorBGRA(value.Red, value.Green, value.Blue, 1f);
    }

    public static explicit operator ColorBGRA(Vector4 value)
    {
      return new ColorBGRA(value.X, value.Y, value.Z, value.W);
    }

    public static explicit operator ColorBGRA(Color4 value)
    {
      return new ColorBGRA(value.Red, value.Green, value.Blue, value.Alpha);
    }

    public static implicit operator ColorBGRA(Color value)
    {
      return new ColorBGRA(value.R, value.G, value.B, value.A);
    }

    public static implicit operator Color(ColorBGRA value)
    {
      return new Color(value.R, value.G, value.B, value.A);
    }

    public static explicit operator int(ColorBGRA value)
    {
      return value.ToBgra();
    }

    public static explicit operator ColorBGRA(int value)
    {
      return new ColorBGRA(value);
    }

    public static ColorBGRA operator +(ColorBGRA left, ColorBGRA right)
    {
      return new ColorBGRA((float) ((int) left.R + (int) right.R), (float) ((int) left.G + (int) right.G), (float) ((int) left.B + (int) right.B), (float) ((int) left.A + (int) right.A));
    }

    public static ColorBGRA operator +(ColorBGRA value)
    {
      return value;
    }

    public static ColorBGRA operator -(ColorBGRA left, ColorBGRA right)
    {
      return new ColorBGRA((float) ((int) left.R - (int) right.R), (float) ((int) left.G - (int) right.G), (float) ((int) left.B - (int) right.B), (float) ((int) left.A - (int) right.A));
    }

    public static ColorBGRA operator -(ColorBGRA value)
    {
      return new ColorBGRA((float) -value.R, (float) -value.G, (float) -value.B, (float) -value.A);
    }

    public static ColorBGRA operator *(float scale, ColorBGRA value)
    {
      return new ColorBGRA((byte) ((double) value.R * (double) scale), (byte) ((double) value.G * (double) scale), (byte) ((double) value.B * (double) scale), (byte) ((double) value.A * (double) scale));
    }

    public static ColorBGRA operator *(ColorBGRA value, float scale)
    {
      return new ColorBGRA((byte) ((double) value.R * (double) scale), (byte) ((double) value.G * (double) scale), (byte) ((double) value.B * (double) scale), (byte) ((double) value.A * (double) scale));
    }

    public static ColorBGRA operator *(ColorBGRA left, ColorBGRA right)
    {
      return new ColorBGRA((byte) ((double) ((int) left.R * (int) right.R) / (double) byte.MaxValue), (byte) ((double) ((int) left.G * (int) right.G) / (double) byte.MaxValue), (byte) ((double) ((int) left.B * (int) right.B) / (double) byte.MaxValue), (byte) ((double) ((int) left.A * (int) right.A) / (double) byte.MaxValue));
    }

    public static bool operator ==(ColorBGRA left, ColorBGRA right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(ColorBGRA left, ColorBGRA right)
    {
      return !left.Equals(right);
    }

    public int ToBgra()
    {
      return (int) this.B | (int) this.G << 8 | (int) this.R << 16 | (int) this.A << 24;
    }

    public int ToRgba()
    {
      return (int) this.R | (int) this.G << 8 | (int) this.B << 16 | (int) this.A << 24;
    }

    public Vector3 ToVector3()
    {
      return new Vector3((float) this.R / (float) byte.MaxValue, (float) this.G / (float) byte.MaxValue, (float) this.B / (float) byte.MaxValue);
    }

    public Color3 ToColor3()
    {
      return new Color3((float) this.R / (float) byte.MaxValue, (float) this.G / (float) byte.MaxValue, (float) this.B / (float) byte.MaxValue);
    }

    public Vector4 ToVector4()
    {
      return new Vector4((float) this.R / (float) byte.MaxValue, (float) this.G / (float) byte.MaxValue, (float) this.B / (float) byte.MaxValue, (float) this.A / (float) byte.MaxValue);
    }

    public byte[] ToArray()
    {
      return new byte[4]
      {
        this.B,
        this.G,
        this.R,
        this.A
      };
    }

    public float GetBrightness()
    {
      float num1 = (float) this.R / (float) byte.MaxValue;
      float num2 = (float) this.G / (float) byte.MaxValue;
      float num3 = (float) this.B / (float) byte.MaxValue;
      float num4 = num1;
      float num5 = num1;
      if ((double) num2 > (double) num4)
        num4 = num2;
      if ((double) num3 > (double) num4)
        num4 = num3;
      if ((double) num2 < (double) num5)
        num5 = num2;
      if ((double) num3 < (double) num5)
        num5 = num3;
      return (float) (((double) num4 + (double) num5) / 2.0);
    }

    public float GetHue()
    {
      if ((int) this.R == (int) this.G && (int) this.G == (int) this.B)
        return 0.0f;
      float num1 = (float) this.R / (float) byte.MaxValue;
      float num2 = (float) this.G / (float) byte.MaxValue;
      float num3 = (float) this.B / (float) byte.MaxValue;
      float num4 = 0.0f;
      float num5 = num1;
      float num6 = num1;
      if ((double) num2 > (double) num5)
        num5 = num2;
      if ((double) num3 > (double) num5)
        num5 = num3;
      if ((double) num2 < (double) num6)
        num6 = num2;
      if ((double) num3 < (double) num6)
        num6 = num3;
      float num7 = num5 - num6;
      if ((double) num1 == (double) num5)
        num4 = (num2 - num3) / num7;
      else if ((double) num2 == (double) num5)
        num4 = (float) (2.0 + ((double) num3 - (double) num1) / (double) num7);
      else if ((double) num3 == (double) num5)
        num4 = (float) (4.0 + ((double) num1 - (double) num2) / (double) num7);
      float num8 = num4 * 60f;
      if ((double) num8 < 0.0)
        num8 += 360f;
      return num8;
    }

    public float GetSaturation()
    {
      float num1 = (float) this.R / (float) byte.MaxValue;
      float num2 = (float) this.G / (float) byte.MaxValue;
      float num3 = (float) this.B / (float) byte.MaxValue;
      float num4 = 0.0f;
      float num5 = num1;
      float num6 = num1;
      if ((double) num2 > (double) num5)
        num5 = num2;
      if ((double) num3 > (double) num5)
        num5 = num3;
      if ((double) num2 < (double) num6)
        num6 = num2;
      if ((double) num3 < (double) num6)
        num6 = num3;
      if ((double) num5 != (double) num6)
        num4 = ((double) num5 + (double) num6) / 2.0 > 0.5 ? (float) (((double) num5 - (double) num6) / (2.0 - (double) num5 - (double) num6)) : (float) (((double) num5 - (double) num6) / ((double) num5 + (double) num6));
      return num4;
    }

    public static ColorBGRA FromBgra(int color)
    {
      return new ColorBGRA(color);
    }

    public static ColorBGRA FromBgra(uint color)
    {
      return new ColorBGRA(color);
    }

    public static ColorBGRA FromRgba(int color)
    {
      return new ColorBGRA((byte) (color & (int) byte.MaxValue), (byte) (color >> 8 & (int) byte.MaxValue), (byte) (color >> 16 & (int) byte.MaxValue), (byte) (color >> 24 & (int) byte.MaxValue));
    }

    public static ColorBGRA FromRgba(uint color)
    {
      return ColorBGRA.FromRgba((int) color);
    }

    public static void Add(ref ColorBGRA left, ref ColorBGRA right, out ColorBGRA result)
    {
      result.A = (byte) ((uint) left.A + (uint) right.A);
      result.R = (byte) ((uint) left.R + (uint) right.R);
      result.G = (byte) ((uint) left.G + (uint) right.G);
      result.B = (byte) ((uint) left.B + (uint) right.B);
    }

    public static ColorBGRA Add(ColorBGRA left, ColorBGRA right)
    {
      return new ColorBGRA((float) ((int) left.R + (int) right.R), (float) ((int) left.G + (int) right.G), (float) ((int) left.B + (int) right.B), (float) ((int) left.A + (int) right.A));
    }

    public static void Subtract(ref ColorBGRA left, ref ColorBGRA right, out ColorBGRA result)
    {
      result.A = (byte) ((uint) left.A - (uint) right.A);
      result.R = (byte) ((uint) left.R - (uint) right.R);
      result.G = (byte) ((uint) left.G - (uint) right.G);
      result.B = (byte) ((uint) left.B - (uint) right.B);
    }

    public static ColorBGRA Subtract(ColorBGRA left, ColorBGRA right)
    {
      return new ColorBGRA((float) ((int) left.R - (int) right.R), (float) ((int) left.G - (int) right.G), (float) ((int) left.B - (int) right.B), (float) ((int) left.A - (int) right.A));
    }

    public static void Modulate(ref ColorBGRA left, ref ColorBGRA right, out ColorBGRA result)
    {
      result.A = (byte) ((double) ((int) left.A * (int) right.A) / (double) byte.MaxValue);
      result.R = (byte) ((double) ((int) left.R * (int) right.R) / (double) byte.MaxValue);
      result.G = (byte) ((double) ((int) left.G * (int) right.G) / (double) byte.MaxValue);
      result.B = (byte) ((double) ((int) left.B * (int) right.B) / (double) byte.MaxValue);
    }

    public static ColorBGRA Modulate(ColorBGRA left, ColorBGRA right)
    {
      return new ColorBGRA((float) ((int) left.R * (int) right.R >> 8), (float) ((int) left.G * (int) right.G >> 8), (float) ((int) left.B * (int) right.B >> 8), (float) ((int) left.A * (int) right.A >> 8));
    }

    public static void Scale(ref ColorBGRA value, float scale, out ColorBGRA result)
    {
      result.A = (byte) ((double) value.A * (double) scale);
      result.R = (byte) ((double) value.R * (double) scale);
      result.G = (byte) ((double) value.G * (double) scale);
      result.B = (byte) ((double) value.B * (double) scale);
    }

    public static ColorBGRA Scale(ColorBGRA value, float scale)
    {
      return new ColorBGRA((byte) ((double) value.R * (double) scale), (byte) ((double) value.G * (double) scale), (byte) ((double) value.B * (double) scale), (byte) ((double) value.A * (double) scale));
    }

    public static void Negate(ref ColorBGRA value, out ColorBGRA result)
    {
      result.A = (byte) ((uint) byte.MaxValue - (uint) value.A);
      result.R = (byte) ((uint) byte.MaxValue - (uint) value.R);
      result.G = (byte) ((uint) byte.MaxValue - (uint) value.G);
      result.B = (byte) ((uint) byte.MaxValue - (uint) value.B);
    }

    public static ColorBGRA Negate(ColorBGRA value)
    {
      return new ColorBGRA((float) ((int) byte.MaxValue - (int) value.R), (float) ((int) byte.MaxValue - (int) value.G), (float) ((int) byte.MaxValue - (int) value.B), (float) ((int) byte.MaxValue - (int) value.A));
    }

    public static void Clamp(ref ColorBGRA value, ref ColorBGRA min, ref ColorBGRA max, out ColorBGRA result)
    {
      byte num1 = value.A;
      byte num2 = (int) num1 > (int) max.A ? max.A : num1;
      byte alpha = (int) num2 < (int) min.A ? min.A : num2;
      byte num3 = value.R;
      byte num4 = (int) num3 > (int) max.R ? max.R : num3;
      byte red = (int) num4 < (int) min.R ? min.R : num4;
      byte num5 = value.G;
      byte num6 = (int) num5 > (int) max.G ? max.G : num5;
      byte green = (int) num6 < (int) min.G ? min.G : num6;
      byte num7 = value.B;
      byte num8 = (int) num7 > (int) max.B ? max.B : num7;
      byte blue = (int) num8 < (int) min.B ? min.B : num8;
      result = new ColorBGRA(red, green, blue, alpha);
    }

    public static ColorBGRA Clamp(ColorBGRA value, ColorBGRA min, ColorBGRA max)
    {
      ColorBGRA result;
      ColorBGRA.Clamp(ref value, ref min, ref max, out result);
      return result;
    }

    public static void Lerp(ref ColorBGRA start, ref ColorBGRA end, float amount, out ColorBGRA result)
    {
      result.A = (byte) ((double) start.A + (double) amount * (double) ((int) end.A - (int) start.A));
      result.R = (byte) ((double) start.R + (double) amount * (double) ((int) end.R - (int) start.R));
      result.G = (byte) ((double) start.G + (double) amount * (double) ((int) end.G - (int) start.G));
      result.B = (byte) ((double) start.B + (double) amount * (double) ((int) end.B - (int) start.B));
    }

    public static ColorBGRA Lerp(ColorBGRA start, ColorBGRA end, float amount)
    {
      return new ColorBGRA((byte) ((double) start.R + (double) amount * (double) ((int) end.R - (int) start.R)), (byte) ((double) start.G + (double) amount * (double) ((int) end.G - (int) start.G)), (byte) ((double) start.B + (double) amount * (double) ((int) end.B - (int) start.B)), (byte) ((double) start.A + (double) amount * (double) ((int) end.A - (int) start.A)));
    }

    public static void SmoothStep(ref ColorBGRA start, ref ColorBGRA end, float amount, out ColorBGRA result)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      result.A = (byte) ((double) start.A + (double) ((int) end.A - (int) start.A) * (double) amount);
      result.R = (byte) ((double) start.R + (double) ((int) end.R - (int) start.R) * (double) amount);
      result.G = (byte) ((double) start.G + (double) ((int) end.G - (int) start.G) * (double) amount);
      result.B = (byte) ((double) start.B + (double) ((int) end.B - (int) start.B) * (double) amount);
    }

    public static ColorBGRA SmoothStep(ColorBGRA start, ColorBGRA end, float amount)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      return new ColorBGRA((byte) ((double) start.R + (double) ((int) end.R - (int) start.R) * (double) amount), (byte) ((double) start.G + (double) ((int) end.G - (int) start.G) * (double) amount), (byte) ((double) start.B + (double) ((int) end.B - (int) start.B) * (double) amount), (byte) ((double) start.A + (double) ((int) end.A - (int) start.A) * (double) amount));
    }

    public static void Max(ref ColorBGRA left, ref ColorBGRA right, out ColorBGRA result)
    {
      result.A = (int) left.A > (int) right.A ? left.A : right.A;
      result.R = (int) left.R > (int) right.R ? left.R : right.R;
      result.G = (int) left.G > (int) right.G ? left.G : right.G;
      result.B = (int) left.B > (int) right.B ? left.B : right.B;
    }

    public static ColorBGRA Max(ColorBGRA left, ColorBGRA right)
    {
      ColorBGRA result;
      ColorBGRA.Max(ref left, ref right, out result);
      return result;
    }

    public static void Min(ref ColorBGRA left, ref ColorBGRA right, out ColorBGRA result)
    {
      result.A = (int) left.A < (int) right.A ? left.A : right.A;
      result.R = (int) left.R < (int) right.R ? left.R : right.R;
      result.G = (int) left.G < (int) right.G ? left.G : right.G;
      result.B = (int) left.B < (int) right.B ? left.B : right.B;
    }

    public static ColorBGRA Min(ColorBGRA left, ColorBGRA right)
    {
      ColorBGRA result;
      ColorBGRA.Min(ref left, ref right, out result);
      return result;
    }

    public static void AdjustContrast(ref ColorBGRA value, float contrast, out ColorBGRA result)
    {
      result.A = value.A;
      result.R = ColorBGRA.ToByte((float) (0.5 + (double) contrast * ((double) value.R / (double) byte.MaxValue - 0.5)));
      result.G = ColorBGRA.ToByte((float) (0.5 + (double) contrast * ((double) value.G / (double) byte.MaxValue - 0.5)));
      result.B = ColorBGRA.ToByte((float) (0.5 + (double) contrast * ((double) value.B / (double) byte.MaxValue - 0.5)));
    }

    public static ColorBGRA AdjustContrast(ColorBGRA value, float contrast)
    {
      return new ColorBGRA(ColorBGRA.ToByte((float) (0.5 + (double) contrast * ((double) value.R / (double) byte.MaxValue - 0.5))), ColorBGRA.ToByte((float) (0.5 + (double) contrast * ((double) value.G / (double) byte.MaxValue - 0.5))), ColorBGRA.ToByte((float) (0.5 + (double) contrast * ((double) value.B / (double) byte.MaxValue - 0.5))), value.A);
    }

    public static void AdjustSaturation(ref ColorBGRA value, float saturation, out ColorBGRA result)
    {
      float num = (float) ((double) value.R / (double) byte.MaxValue * 0.212500005960464 + (double) value.G / (double) byte.MaxValue * 0.715399980545044 + (double) value.B / (double) byte.MaxValue * 0.0720999985933304);
      result.A = value.A;
      result.R = ColorBGRA.ToByte(num + saturation * ((float) value.R / (float) byte.MaxValue - num));
      result.G = ColorBGRA.ToByte(num + saturation * ((float) value.G / (float) byte.MaxValue - num));
      result.B = ColorBGRA.ToByte(num + saturation * ((float) value.B / (float) byte.MaxValue - num));
    }

    public static ColorBGRA AdjustSaturation(ColorBGRA value, float saturation)
    {
      float num = (float) ((double) value.R / (double) byte.MaxValue * 0.212500005960464 + (double) value.G / (double) byte.MaxValue * 0.715399980545044 + (double) value.B / (double) byte.MaxValue * 0.0720999985933304);
      return new ColorBGRA(ColorBGRA.ToByte(num + saturation * ((float) value.R / (float) byte.MaxValue - num)), ColorBGRA.ToByte(num + saturation * ((float) value.G / (float) byte.MaxValue - num)), ColorBGRA.ToByte(num + saturation * ((float) value.B / (float) byte.MaxValue - num)), value.A);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "A:{0} R:{1} G:{2} B:{3}", (object) this.A, (object) this.R, (object) this.G, (object) this.B);
    }

    public string ToString(string format)
    {
      if (format == null)
        return this.ToString();
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "A:{0} R:{1} G:{2} B:{3}", (object) this.A.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.R.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.G.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture), (object) this.B.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture));
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, "A:{0} R:{1} G:{2} B:{3}", (object) this.A, (object) this.R, (object) this.G, (object) this.B);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (format == null)
        return this.ToString(formatProvider);
      return string.Format(formatProvider, "A:{0} R:{1} G:{2} B:{3}", (object) this.A.ToString(format, formatProvider), (object) this.R.ToString(format, formatProvider), (object) this.G.ToString(format, formatProvider), (object) this.B.ToString(format, formatProvider));
    }

    public override int GetHashCode()
    {
      return this.A.GetHashCode() + this.R.GetHashCode() + this.G.GetHashCode() + this.B.GetHashCode();
    }

    public bool Equals(ColorBGRA other)
    {
      if ((int) this.R == (int) other.R && (int) this.G == (int) other.G && (int) this.B == (int) other.B)
        return (int) this.A == (int) other.A;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (ColorBGRA)))
        return false;
      else
        return this.Equals((ColorBGRA) value);
    }

    private static byte ToByte(float component)
    {
      int num = (int) ((double) component * (double) byte.MaxValue);
      return num < 0 ? (byte) 0 : (num > (int) byte.MaxValue ? byte.MaxValue : (byte) num);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
        serializer.Writer.Write(this.ToBgra());
      else
        this = ColorBGRA.FromBgra(serializer.Reader.ReadInt32());
    }
  }
}
