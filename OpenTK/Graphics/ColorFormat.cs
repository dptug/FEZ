// Type: OpenTK.Graphics.ColorFormat
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Graphics
{
  public struct ColorFormat : IComparable<ColorFormat>
  {
    public static readonly ColorFormat Empty = new ColorFormat(0);
    private byte red;
    private byte green;
    private byte blue;
    private byte alpha;
    private bool isIndexed;
    private int bitsPerPixel;

    public int Red
    {
      get
      {
        return (int) this.red;
      }
      private set
      {
        this.red = (byte) value;
      }
    }

    public int Green
    {
      get
      {
        return (int) this.green;
      }
      private set
      {
        this.green = (byte) value;
      }
    }

    public int Blue
    {
      get
      {
        return (int) this.blue;
      }
      private set
      {
        this.blue = (byte) value;
      }
    }

    public int Alpha
    {
      get
      {
        return (int) this.alpha;
      }
      private set
      {
        this.alpha = (byte) value;
      }
    }

    public bool IsIndexed
    {
      get
      {
        return this.isIndexed;
      }
      private set
      {
        this.isIndexed = value;
      }
    }

    public int BitsPerPixel
    {
      get
      {
        return this.bitsPerPixel;
      }
      private set
      {
        this.bitsPerPixel = value;
      }
    }

    static ColorFormat()
    {
    }

    public ColorFormat(int bpp)
    {
      if (bpp < 0)
        throw new ArgumentOutOfRangeException("bpp", "Must be greater or equal to zero.");
      this.red = this.green = this.blue = this.alpha = (byte) 0;
      this.bitsPerPixel = bpp;
      this.isIndexed = false;
      switch (bpp)
      {
        case 15:
          this.Red = this.Green = this.Blue = 5;
          break;
        case 16:
          this.Red = this.Blue = 5;
          this.Green = 6;
          break;
        case 24:
          this.Red = this.Green = this.Blue = 8;
          break;
        case 32:
          this.Red = this.Green = this.Blue = this.Alpha = 8;
          break;
        case 1:
          this.IsIndexed = true;
          break;
        case 4:
          this.Red = this.Green = 2;
          this.Blue = 1;
          this.IsIndexed = true;
          break;
        case 8:
          this.Red = this.Green = 3;
          this.Blue = 2;
          this.IsIndexed = true;
          break;
        default:
          this.Red = this.Blue = this.Alpha = (int) (byte) (bpp / 4);
          this.Green = (int) (byte) (bpp / 4 + bpp % 4);
          break;
      }
    }

    public ColorFormat(int red, int green, int blue, int alpha)
    {
      if (red < 0 || green < 0 || (blue < 0 || alpha < 0))
        throw new ArgumentOutOfRangeException("Arguments must be greater or equal to zero.");
      this.red = (byte) red;
      this.green = (byte) green;
      this.blue = (byte) blue;
      this.alpha = (byte) alpha;
      this.bitsPerPixel = red + green + blue + alpha;
      this.isIndexed = false;
      if (this.bitsPerPixel >= 15 || this.bitsPerPixel == 0)
        return;
      this.isIndexed = true;
    }

    public static implicit operator ColorFormat(int bpp)
    {
      return new ColorFormat(bpp);
    }

    public static bool operator ==(ColorFormat left, ColorFormat right)
    {
      if ((ValueType) left == null && (ValueType) right != null || (ValueType) left != null && (ValueType) right == null)
        return false;
      if ((ValueType) left == null && (ValueType) right == null)
        return true;
      if (left.Red == right.Red && left.Green == right.Green && left.Blue == right.Blue)
        return left.Alpha == right.Alpha;
      else
        return false;
    }

    public static bool operator !=(ColorFormat left, ColorFormat right)
    {
      return !(left == right);
    }

    public static bool operator >(ColorFormat left, ColorFormat right)
    {
      return left.CompareTo(right) > 0;
    }

    public static bool operator >=(ColorFormat left, ColorFormat right)
    {
      return left.CompareTo(right) >= 0;
    }

    public static bool operator <(ColorFormat left, ColorFormat right)
    {
      return left.CompareTo(right) < 0;
    }

    public static bool operator <=(ColorFormat left, ColorFormat right)
    {
      return left.CompareTo(right) <= 0;
    }

    public int CompareTo(ColorFormat other)
    {
      int num1 = this.BitsPerPixel.CompareTo(other.BitsPerPixel);
      if (num1 != 0)
        return num1;
      int num2 = this.IsIndexed.CompareTo(other.IsIndexed);
      if (num2 != 0)
        return num2;
      else
        return this.Alpha.CompareTo(other.Alpha);
    }

    public override bool Equals(object obj)
    {
      if (!(obj is ColorFormat))
        return false;
      else
        return this == (ColorFormat) obj;
    }

    public override int GetHashCode()
    {
      return this.Red ^ this.Green ^ this.Blue ^ this.Alpha;
    }

    public override string ToString()
    {
      return string.Format("{0} ({1})", (object) this.BitsPerPixel, this.IsIndexed ? (object) " indexed" : (object) (this.Red.ToString() + this.Green.ToString() + this.Blue.ToString() + this.Alpha.ToString()));
    }
  }
}
