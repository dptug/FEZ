// Type: OpenTK.DisplayResolution
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenTK
{
  public class DisplayResolution
  {
    private Rectangle bounds;
    private int bits_per_pixel;
    private float refresh_rate;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This property will return invalid results if a monitor changes resolution. Use DisplayDevice.Bounds instead.")]
    public Rectangle Bounds
    {
      get
      {
        return this.bounds;
      }
    }

    public int Width
    {
      get
      {
        return this.bounds.Width;
      }
      internal set
      {
        this.bounds.Width = value;
      }
    }

    public int Height
    {
      get
      {
        return this.bounds.Height;
      }
      internal set
      {
        this.bounds.Height = value;
      }
    }

    public int BitsPerPixel
    {
      get
      {
        return this.bits_per_pixel;
      }
      internal set
      {
        this.bits_per_pixel = value;
      }
    }

    public float RefreshRate
    {
      get
      {
        return this.refresh_rate;
      }
      internal set
      {
        this.refresh_rate = value;
      }
    }

    internal DisplayResolution()
    {
    }

    internal DisplayResolution(int x, int y, int width, int height, int bitsPerPixel, float refreshRate)
    {
      if (width <= 0)
        throw new ArgumentOutOfRangeException("width", "Must be greater than zero.");
      if (height <= 0)
        throw new ArgumentOutOfRangeException("height", "Must be greater than zero.");
      if (bitsPerPixel <= 0)
        throw new ArgumentOutOfRangeException("bitsPerPixel", "Must be greater than zero.");
      if ((double) refreshRate < 0.0)
        throw new ArgumentOutOfRangeException("refreshRate", "Must be greater than, or equal to zero.");
      this.bounds = new Rectangle(x, y, width, height);
      this.bits_per_pixel = bitsPerPixel;
      this.refresh_rate = refreshRate;
    }

    public static bool operator ==(DisplayResolution left, DisplayResolution right)
    {
      if (left == null && right == null)
        return true;
      if (left == null && right != null || left != null && right == null)
        return false;
      else
        return left.Equals((object) right);
    }

    public static bool operator !=(DisplayResolution left, DisplayResolution right)
    {
      return !(left == right);
    }

    public override string ToString()
    {
      return string.Format("{0}x{1}@{2}Hz", (object) this.Bounds, (object) this.bits_per_pixel, (object) this.refresh_rate);
    }

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      DisplayResolution displayResolution = (DisplayResolution) obj;
      if (this.Width == displayResolution.Width && this.Height == displayResolution.Height && this.BitsPerPixel == displayResolution.BitsPerPixel)
        return (double) this.RefreshRate == (double) displayResolution.RefreshRate;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.Bounds.GetHashCode() ^ this.bits_per_pixel ^ this.refresh_rate.GetHashCode();
    }
  }
}
