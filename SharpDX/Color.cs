// Type: SharpDX.Color
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
  [DynamicSerializer("TKC1")]
  [TypeConverter(typeof (ColorConverter))]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Size = 4)]
  public struct Color : IEquatable<Color>, IFormattable, IDataSerializable
  {
    public static readonly Color Zero = Color.FromBgra(0);
    public static readonly Color Transparent = Color.FromBgra(16777215);
    public static readonly Color AliceBlue = Color.FromBgra(4293982463U);
    public static readonly Color AntiqueWhite = Color.FromBgra(4294634455U);
    public static readonly Color Aqua = Color.FromBgra(4278255615U);
    public static readonly Color Aquamarine = Color.FromBgra(4286578644U);
    public static readonly Color Azure = Color.FromBgra(4293984255U);
    public static readonly Color Beige = Color.FromBgra(4294309340U);
    public static readonly Color Bisque = Color.FromBgra(4294960324U);
    public static readonly Color Black = Color.FromBgra(4278190080U);
    public static readonly Color BlanchedAlmond = Color.FromBgra(4294962125U);
    public static readonly Color Blue = Color.FromBgra(4278190335U);
    public static readonly Color BlueViolet = Color.FromBgra(4287245282U);
    public static readonly Color Brown = Color.FromBgra(4289014314U);
    public static readonly Color BurlyWood = Color.FromBgra(4292786311U);
    public static readonly Color CadetBlue = Color.FromBgra(4284456608U);
    public static readonly Color Chartreuse = Color.FromBgra(4286578432U);
    public static readonly Color Chocolate = Color.FromBgra(4291979550U);
    public static readonly Color Coral = Color.FromBgra(4294934352U);
    public static readonly Color CornflowerBlue = Color.FromBgra(4284782061U);
    public static readonly Color Cornsilk = Color.FromBgra(4294965468U);
    public static readonly Color Crimson = Color.FromBgra(4292613180U);
    public static readonly Color Cyan = Color.FromBgra(4278255615U);
    public static readonly Color DarkBlue = Color.FromBgra(4278190219U);
    public static readonly Color DarkCyan = Color.FromBgra(4278225803U);
    public static readonly Color DarkGoldenrod = Color.FromBgra(4290283019U);
    public static readonly Color DarkGray = Color.FromBgra(4289309097U);
    public static readonly Color DarkGreen = Color.FromBgra(4278215680U);
    public static readonly Color DarkKhaki = Color.FromBgra(4290623339U);
    public static readonly Color DarkMagenta = Color.FromBgra(4287299723U);
    public static readonly Color DarkOliveGreen = Color.FromBgra(4283788079U);
    public static readonly Color DarkOrange = Color.FromBgra(4294937600U);
    public static readonly Color DarkOrchid = Color.FromBgra(4288230092U);
    public static readonly Color DarkRed = Color.FromBgra(4287299584U);
    public static readonly Color DarkSalmon = Color.FromBgra(4293498490U);
    public static readonly Color DarkSeaGreen = Color.FromBgra(4287609995U);
    public static readonly Color DarkSlateBlue = Color.FromBgra(4282924427U);
    public static readonly Color DarkSlateGray = Color.FromBgra(4281290575U);
    public static readonly Color DarkTurquoise = Color.FromBgra(4278243025U);
    public static readonly Color DarkViolet = Color.FromBgra(4287889619U);
    public static readonly Color DeepPink = Color.FromBgra(4294907027U);
    public static readonly Color DeepSkyBlue = Color.FromBgra(4278239231U);
    public static readonly Color DimGray = Color.FromBgra(4285098345U);
    public static readonly Color DodgerBlue = Color.FromBgra(4280193279U);
    public static readonly Color Firebrick = Color.FromBgra(4289864226U);
    public static readonly Color FloralWhite = Color.FromBgra(4294966000U);
    public static readonly Color ForestGreen = Color.FromBgra(4280453922U);
    public static readonly Color Fuchsia = Color.FromBgra(4294902015U);
    public static readonly Color Gainsboro = Color.FromBgra(4292664540U);
    public static readonly Color GhostWhite = Color.FromBgra(4294506751U);
    public static readonly Color Gold = Color.FromBgra(4294956800U);
    public static readonly Color Goldenrod = Color.FromBgra(4292519200U);
    public static readonly Color Gray = Color.FromBgra(4286611584U);
    public static readonly Color Green = Color.FromBgra(4278222848U);
    public static readonly Color GreenYellow = Color.FromBgra(4289593135U);
    public static readonly Color Honeydew = Color.FromBgra(4293984240U);
    public static readonly Color HotPink = Color.FromBgra(4294928820U);
    public static readonly Color IndianRed = Color.FromBgra(4291648604U);
    public static readonly Color Indigo = Color.FromBgra(4283105410U);
    public static readonly Color Ivory = Color.FromBgra(4294967280U);
    public static readonly Color Khaki = Color.FromBgra(4293977740U);
    public static readonly Color Lavender = Color.FromBgra(4293322490U);
    public static readonly Color LavenderBlush = Color.FromBgra(4294963445U);
    public static readonly Color LawnGreen = Color.FromBgra(4286381056U);
    public static readonly Color LemonChiffon = Color.FromBgra(4294965965U);
    public static readonly Color LightBlue = Color.FromBgra(4289583334U);
    public static readonly Color LightCoral = Color.FromBgra(4293951616U);
    public static readonly Color LightCyan = Color.FromBgra(4292935679U);
    public static readonly Color LightGoldenrodYellow = Color.FromBgra(4294638290U);
    public static readonly Color LightGray = Color.FromBgra(4292072403U);
    public static readonly Color LightGreen = Color.FromBgra(4287688336U);
    public static readonly Color LightPink = Color.FromBgra(4294948545U);
    public static readonly Color LightSalmon = Color.FromBgra(4294942842U);
    public static readonly Color LightSeaGreen = Color.FromBgra(4280332970U);
    public static readonly Color LightSkyBlue = Color.FromBgra(4287090426U);
    public static readonly Color LightSlateGray = Color.FromBgra(4286023833U);
    public static readonly Color LightSteelBlue = Color.FromBgra(4289774814U);
    public static readonly Color LightYellow = Color.FromBgra(4294967264U);
    public static readonly Color Lime = Color.FromBgra(4278255360U);
    public static readonly Color LimeGreen = Color.FromBgra(4281519410U);
    public static readonly Color Linen = Color.FromBgra(4294635750U);
    public static readonly Color Magenta = Color.FromBgra(4294902015U);
    public static readonly Color Maroon = Color.FromBgra(4286578688U);
    public static readonly Color MediumAquamarine = Color.FromBgra(4284927402U);
    public static readonly Color MediumBlue = Color.FromBgra(4278190285U);
    public static readonly Color MediumOrchid = Color.FromBgra(4290401747U);
    public static readonly Color MediumPurple = Color.FromBgra(4287852763U);
    public static readonly Color MediumSeaGreen = Color.FromBgra(4282168177U);
    public static readonly Color MediumSlateBlue = Color.FromBgra(4286277870U);
    public static readonly Color MediumSpringGreen = Color.FromBgra(4278254234U);
    public static readonly Color MediumTurquoise = Color.FromBgra(4282962380U);
    public static readonly Color MediumVioletRed = Color.FromBgra(4291237253U);
    public static readonly Color MidnightBlue = Color.FromBgra(4279834992U);
    public static readonly Color MintCream = Color.FromBgra(4294311930U);
    public static readonly Color MistyRose = Color.FromBgra(4294960353U);
    public static readonly Color Moccasin = Color.FromBgra(4294960309U);
    public static readonly Color NavajoWhite = Color.FromBgra(4294958765U);
    public static readonly Color Navy = Color.FromBgra(4278190208U);
    public static readonly Color OldLace = Color.FromBgra(4294833638U);
    public static readonly Color Olive = Color.FromBgra(4286611456U);
    public static readonly Color OliveDrab = Color.FromBgra(4285238819U);
    public static readonly Color Orange = Color.FromBgra(4294944000U);
    public static readonly Color OrangeRed = Color.FromBgra(4294919424U);
    public static readonly Color Orchid = Color.FromBgra(4292505814U);
    public static readonly Color PaleGoldenrod = Color.FromBgra(4293847210U);
    public static readonly Color PaleGreen = Color.FromBgra(4288215960U);
    public static readonly Color PaleTurquoise = Color.FromBgra(4289720046U);
    public static readonly Color PaleVioletRed = Color.FromBgra(4292571283U);
    public static readonly Color PapayaWhip = Color.FromBgra(4294963157U);
    public static readonly Color PeachPuff = Color.FromBgra(4294957753U);
    public static readonly Color Peru = Color.FromBgra(4291659071U);
    public static readonly Color Pink = Color.FromBgra(4294951115U);
    public static readonly Color Plum = Color.FromBgra(4292714717U);
    public static readonly Color PowderBlue = Color.FromBgra(4289781990U);
    public static readonly Color Purple = Color.FromBgra(4286578816U);
    public static readonly Color Red = Color.FromBgra(4294901760U);
    public static readonly Color RosyBrown = Color.FromBgra(4290547599U);
    public static readonly Color RoyalBlue = Color.FromBgra(4282477025U);
    public static readonly Color SaddleBrown = Color.FromBgra(4287317267U);
    public static readonly Color Salmon = Color.FromBgra(4294606962U);
    public static readonly Color SandyBrown = Color.FromBgra(4294222944U);
    public static readonly Color SeaGreen = Color.FromBgra(4281240407U);
    public static readonly Color SeaShell = Color.FromBgra(4294964718U);
    public static readonly Color Sienna = Color.FromBgra(4288696877U);
    public static readonly Color Silver = Color.FromBgra(4290822336U);
    public static readonly Color SkyBlue = Color.FromBgra(4287090411U);
    public static readonly Color SlateBlue = Color.FromBgra(4285160141U);
    public static readonly Color SlateGray = Color.FromBgra(4285563024U);
    public static readonly Color Snow = Color.FromBgra(4294966010U);
    public static readonly Color SpringGreen = Color.FromBgra(4278255487U);
    public static readonly Color SteelBlue = Color.FromBgra(4282811060U);
    public static readonly Color Tan = Color.FromBgra(4291998860U);
    public static readonly Color Teal = Color.FromBgra(4278222976U);
    public static readonly Color Thistle = Color.FromBgra(4292394968U);
    public static readonly Color Tomato = Color.FromBgra(4294927175U);
    public static readonly Color Turquoise = Color.FromBgra(4282441936U);
    public static readonly Color Violet = Color.FromBgra(4293821166U);
    public static readonly Color Wheat = Color.FromBgra(4294303411U);
    public static readonly Color White = Color.FromBgra(uint.MaxValue);
    public static readonly Color WhiteSmoke = Color.FromBgra(4294309365U);
    public static readonly Color Yellow = Color.FromBgra(4294967040U);
    public static readonly Color YellowGreen = Color.FromBgra(4288335154U);
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public byte this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.R;
          case 1:
            return this.G;
          case 2:
            return this.B;
          case 3:
            return this.A;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Color run from 0 to 3, inclusive.");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.R = value;
            break;
          case 1:
            this.G = value;
            break;
          case 2:
            this.B = value;
            break;
          case 3:
            this.A = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Color run from 0 to 3, inclusive.");
        }
      }
    }

    static Color()
    {
    }

    public Color(byte value)
    {
      this.A = this.R = this.G = this.B = value;
    }

    public Color(float value)
    {
      this.A = this.R = this.G = this.B = Color.ToByte(value);
    }

    public Color(byte red, byte green, byte blue, byte alpha)
    {
      this.R = red;
      this.G = green;
      this.B = blue;
      this.A = alpha;
    }

    public Color(float red, float green, float blue, float alpha)
    {
      this.R = Color.ToByte(red);
      this.G = Color.ToByte(green);
      this.B = Color.ToByte(blue);
      this.A = Color.ToByte(alpha);
    }

    public Color(Vector4 value)
    {
      this.R = Color.ToByte(value.X);
      this.G = Color.ToByte(value.Y);
      this.B = Color.ToByte(value.Z);
      this.A = Color.ToByte(value.W);
    }

    public Color(Vector3 value, float alpha)
    {
      this.R = Color.ToByte(value.X);
      this.G = Color.ToByte(value.Y);
      this.B = Color.ToByte(value.Z);
      this.A = Color.ToByte(alpha);
    }

    public Color(uint rgba)
    {
      this.A = (byte) (rgba >> 24 & (uint) byte.MaxValue);
      this.B = (byte) (rgba >> 16 & (uint) byte.MaxValue);
      this.G = (byte) (rgba >> 8 & (uint) byte.MaxValue);
      this.R = (byte) (rgba & (uint) byte.MaxValue);
    }

    public Color(int rgba)
    {
      this.A = (byte) (rgba >> 24 & (int) byte.MaxValue);
      this.B = (byte) (rgba >> 16 & (int) byte.MaxValue);
      this.G = (byte) (rgba >> 8 & (int) byte.MaxValue);
      this.R = (byte) (rgba & (int) byte.MaxValue);
    }

    public Color(float[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Color.");
      this.R = Color.ToByte(values[0]);
      this.G = Color.ToByte(values[1]);
      this.B = Color.ToByte(values[2]);
      this.A = Color.ToByte(values[3]);
    }

    public Color(byte[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Color.");
      this.R = values[0];
      this.G = values[1];
      this.B = values[2];
      this.A = values[3];
    }

    public static explicit operator Color3(Color value)
    {
      return new Color3((float) value.R, (float) value.G, (float) value.B);
    }

    public static explicit operator Vector3(Color value)
    {
      return new Vector3((float) value.R / (float) byte.MaxValue, (float) value.G / (float) byte.MaxValue, (float) value.B / (float) byte.MaxValue);
    }

    public static explicit operator Vector4(Color value)
    {
      return new Vector4((float) value.R / (float) byte.MaxValue, (float) value.G / (float) byte.MaxValue, (float) value.B / (float) byte.MaxValue, (float) value.A / (float) byte.MaxValue);
    }

    public static implicit operator Color4(Color value)
    {
      return new Color4((float) value.R / (float) byte.MaxValue, (float) value.G / (float) byte.MaxValue, (float) value.B / (float) byte.MaxValue, (float) value.A / (float) byte.MaxValue);
    }

    public static explicit operator Color(Vector3 value)
    {
      return new Color(value.X, value.Y, value.Z, 1f);
    }

    public static explicit operator Color(Color3 value)
    {
      return new Color(value.Red, value.Green, value.Blue, 1f);
    }

    public static explicit operator Color(Vector4 value)
    {
      return new Color(value.X, value.Y, value.Z, value.W);
    }

    public static explicit operator Color(Color4 value)
    {
      return new Color(value.Red, value.Green, value.Blue, value.Alpha);
    }

    public static explicit operator int(Color value)
    {
      return value.ToRgba();
    }

    public static explicit operator Color(int value)
    {
      return new Color(value);
    }

    public static Color operator +(Color left, Color right)
    {
      return new Color((float) ((int) left.R + (int) right.R), (float) ((int) left.G + (int) right.G), (float) ((int) left.B + (int) right.B), (float) ((int) left.A + (int) right.A));
    }

    public static Color operator +(Color value)
    {
      return value;
    }

    public static Color operator -(Color left, Color right)
    {
      return new Color((float) ((int) left.R - (int) right.R), (float) ((int) left.G - (int) right.G), (float) ((int) left.B - (int) right.B), (float) ((int) left.A - (int) right.A));
    }

    public static Color operator -(Color value)
    {
      return new Color((float) -value.R, (float) -value.G, (float) -value.B, (float) -value.A);
    }

    public static Color operator *(float scale, Color value)
    {
      return new Color((byte) ((double) value.R * (double) scale), (byte) ((double) value.G * (double) scale), (byte) ((double) value.B * (double) scale), (byte) ((double) value.A * (double) scale));
    }

    public static Color operator *(Color value, float scale)
    {
      return new Color((byte) ((double) value.R * (double) scale), (byte) ((double) value.G * (double) scale), (byte) ((double) value.B * (double) scale), (byte) ((double) value.A * (double) scale));
    }

    public static Color operator *(Color left, Color right)
    {
      return new Color((byte) ((double) ((int) left.R * (int) right.R) / (double) byte.MaxValue), (byte) ((double) ((int) left.G * (int) right.G) / (double) byte.MaxValue), (byte) ((double) ((int) left.B * (int) right.B) / (double) byte.MaxValue), (byte) ((double) ((int) left.A * (int) right.A) / (double) byte.MaxValue));
    }

    public static bool operator ==(Color left, Color right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Color left, Color right)
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
        this.R,
        this.G,
        this.B,
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

    public static void Add(ref Color left, ref Color right, out Color result)
    {
      result.A = (byte) ((uint) left.A + (uint) right.A);
      result.R = (byte) ((uint) left.R + (uint) right.R);
      result.G = (byte) ((uint) left.G + (uint) right.G);
      result.B = (byte) ((uint) left.B + (uint) right.B);
    }

    public static Color Add(Color left, Color right)
    {
      return new Color((float) ((int) left.R + (int) right.R), (float) ((int) left.G + (int) right.G), (float) ((int) left.B + (int) right.B), (float) ((int) left.A + (int) right.A));
    }

    public static void Subtract(ref Color left, ref Color right, out Color result)
    {
      result.A = (byte) ((uint) left.A - (uint) right.A);
      result.R = (byte) ((uint) left.R - (uint) right.R);
      result.G = (byte) ((uint) left.G - (uint) right.G);
      result.B = (byte) ((uint) left.B - (uint) right.B);
    }

    public static Color Subtract(Color left, Color right)
    {
      return new Color((float) ((int) left.R - (int) right.R), (float) ((int) left.G - (int) right.G), (float) ((int) left.B - (int) right.B), (float) ((int) left.A - (int) right.A));
    }

    public static void Modulate(ref Color left, ref Color right, out Color result)
    {
      result.A = (byte) ((double) ((int) left.A * (int) right.A) / (double) byte.MaxValue);
      result.R = (byte) ((double) ((int) left.R * (int) right.R) / (double) byte.MaxValue);
      result.G = (byte) ((double) ((int) left.G * (int) right.G) / (double) byte.MaxValue);
      result.B = (byte) ((double) ((int) left.B * (int) right.B) / (double) byte.MaxValue);
    }

    public static Color Modulate(Color left, Color right)
    {
      return new Color((float) ((int) left.R * (int) right.R), (float) ((int) left.G * (int) right.G), (float) ((int) left.B * (int) right.B), (float) ((int) left.A * (int) right.A));
    }

    public static void Scale(ref Color value, float scale, out Color result)
    {
      result.A = (byte) ((double) value.A * (double) scale);
      result.R = (byte) ((double) value.R * (double) scale);
      result.G = (byte) ((double) value.G * (double) scale);
      result.B = (byte) ((double) value.B * (double) scale);
    }

    public static Color Scale(Color value, float scale)
    {
      return new Color((byte) ((double) value.R * (double) scale), (byte) ((double) value.G * (double) scale), (byte) ((double) value.B * (double) scale), (byte) ((double) value.A * (double) scale));
    }

    public static void Negate(ref Color value, out Color result)
    {
      result.A = (byte) ((uint) byte.MaxValue - (uint) value.A);
      result.R = (byte) ((uint) byte.MaxValue - (uint) value.R);
      result.G = (byte) ((uint) byte.MaxValue - (uint) value.G);
      result.B = (byte) ((uint) byte.MaxValue - (uint) value.B);
    }

    public static Color Negate(Color value)
    {
      return new Color((float) ((int) byte.MaxValue - (int) value.R), (float) ((int) byte.MaxValue - (int) value.G), (float) ((int) byte.MaxValue - (int) value.B), (float) ((int) byte.MaxValue - (int) value.A));
    }

    public static void Clamp(ref Color value, ref Color min, ref Color max, out Color result)
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
      result = new Color(red, green, blue, alpha);
    }

    public static Color FromBgra(int color)
    {
      return new Color((byte) (color >> 16 & (int) byte.MaxValue), (byte) (color >> 8 & (int) byte.MaxValue), (byte) (color & (int) byte.MaxValue), (byte) (color >> 24 & (int) byte.MaxValue));
    }

    public static Color FromBgra(uint color)
    {
      return Color.FromBgra((int) color);
    }

    public static Color FromRgba(int color)
    {
      return new Color(color);
    }

    public static Color FromRgba(uint color)
    {
      return new Color(color);
    }

    public static Color Clamp(Color value, Color min, Color max)
    {
      Color result;
      Color.Clamp(ref value, ref min, ref max, out result);
      return result;
    }

    public static void Lerp(ref Color start, ref Color end, float amount, out Color result)
    {
      result.A = (byte) ((double) start.A + (double) amount * (double) ((int) end.A - (int) start.A));
      result.R = (byte) ((double) start.R + (double) amount * (double) ((int) end.R - (int) start.R));
      result.G = (byte) ((double) start.G + (double) amount * (double) ((int) end.G - (int) start.G));
      result.B = (byte) ((double) start.B + (double) amount * (double) ((int) end.B - (int) start.B));
    }

    public static Color Lerp(Color start, Color end, float amount)
    {
      return new Color((byte) ((double) start.R + (double) amount * (double) ((int) end.R - (int) start.R)), (byte) ((double) start.G + (double) amount * (double) ((int) end.G - (int) start.G)), (byte) ((double) start.B + (double) amount * (double) ((int) end.B - (int) start.B)), (byte) ((double) start.A + (double) amount * (double) ((int) end.A - (int) start.A)));
    }

    public static void SmoothStep(ref Color start, ref Color end, float amount, out Color result)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      result.A = (byte) ((double) start.A + (double) ((int) end.A - (int) start.A) * (double) amount);
      result.R = (byte) ((double) start.R + (double) ((int) end.R - (int) start.R) * (double) amount);
      result.G = (byte) ((double) start.G + (double) ((int) end.G - (int) start.G) * (double) amount);
      result.B = (byte) ((double) start.B + (double) ((int) end.B - (int) start.B) * (double) amount);
    }

    public static Color SmoothStep(Color start, Color end, float amount)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      return new Color((byte) ((double) start.R + (double) ((int) end.R - (int) start.R) * (double) amount), (byte) ((double) start.G + (double) ((int) end.G - (int) start.G) * (double) amount), (byte) ((double) start.B + (double) ((int) end.B - (int) start.B) * (double) amount), (byte) ((double) start.A + (double) ((int) end.A - (int) start.A) * (double) amount));
    }

    public static void Max(ref Color left, ref Color right, out Color result)
    {
      result.A = (int) left.A > (int) right.A ? left.A : right.A;
      result.R = (int) left.R > (int) right.R ? left.R : right.R;
      result.G = (int) left.G > (int) right.G ? left.G : right.G;
      result.B = (int) left.B > (int) right.B ? left.B : right.B;
    }

    public static Color Max(Color left, Color right)
    {
      Color result;
      Color.Max(ref left, ref right, out result);
      return result;
    }

    public static void Min(ref Color left, ref Color right, out Color result)
    {
      result.A = (int) left.A < (int) right.A ? left.A : right.A;
      result.R = (int) left.R < (int) right.R ? left.R : right.R;
      result.G = (int) left.G < (int) right.G ? left.G : right.G;
      result.B = (int) left.B < (int) right.B ? left.B : right.B;
    }

    public static Color Min(Color left, Color right)
    {
      Color result;
      Color.Min(ref left, ref right, out result);
      return result;
    }

    public static void AdjustContrast(ref Color value, float contrast, out Color result)
    {
      result.A = value.A;
      result.R = Color.ToByte((float) (0.5 + (double) contrast * ((double) value.R / (double) byte.MaxValue - 0.5)));
      result.G = Color.ToByte((float) (0.5 + (double) contrast * ((double) value.G / (double) byte.MaxValue - 0.5)));
      result.B = Color.ToByte((float) (0.5 + (double) contrast * ((double) value.B / (double) byte.MaxValue - 0.5)));
    }

    public static Color AdjustContrast(Color value, float contrast)
    {
      return new Color(Color.ToByte((float) (0.5 + (double) contrast * ((double) value.R / (double) byte.MaxValue - 0.5))), Color.ToByte((float) (0.5 + (double) contrast * ((double) value.G / (double) byte.MaxValue - 0.5))), Color.ToByte((float) (0.5 + (double) contrast * ((double) value.B / (double) byte.MaxValue - 0.5))), value.A);
    }

    public static void AdjustSaturation(ref Color value, float saturation, out Color result)
    {
      float num = (float) ((double) value.R / (double) byte.MaxValue * 0.212500005960464 + (double) value.G / (double) byte.MaxValue * 0.715399980545044 + (double) value.B / (double) byte.MaxValue * 0.0720999985933304);
      result.A = value.A;
      result.R = Color.ToByte(num + saturation * ((float) value.R / (float) byte.MaxValue - num));
      result.G = Color.ToByte(num + saturation * ((float) value.G / (float) byte.MaxValue - num));
      result.B = Color.ToByte(num + saturation * ((float) value.B / (float) byte.MaxValue - num));
    }

    public static Color AdjustSaturation(Color value, float saturation)
    {
      float num = (float) ((double) value.R / (double) byte.MaxValue * 0.212500005960464 + (double) value.G / (double) byte.MaxValue * 0.715399980545044 + (double) value.B / (double) byte.MaxValue * 0.0720999985933304);
      return new Color(Color.ToByte(num + saturation * ((float) value.R / (float) byte.MaxValue - num)), Color.ToByte(num + saturation * ((float) value.G / (float) byte.MaxValue - num)), Color.ToByte(num + saturation * ((float) value.B / (float) byte.MaxValue - num)), value.A);
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

    public bool Equals(Color other)
    {
      if ((int) this.R == (int) other.R && (int) this.G == (int) other.G && (int) this.B == (int) other.B)
        return (int) this.A == (int) other.A;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Color)))
        return false;
      else
        return this.Equals((Color) value);
    }

    private static byte ToByte(float component)
    {
      int num = (int) ((double) component * (double) byte.MaxValue);
      return num < 0 ? (byte) 0 : (num > (int) byte.MaxValue ? byte.MaxValue : (byte) num);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
        serializer.Writer.Write(this.ToRgba());
      else
        this = Color.FromRgba(serializer.Reader.ReadInt32());
    }
  }
}
