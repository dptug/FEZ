// Type: Microsoft.Xna.Framework.Graphics.DisplayMode
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.Globalization;

namespace Microsoft.Xna.Framework.Graphics
{
  [Serializable]
  public class DisplayMode
  {
    private SurfaceFormat format;
    private int height;
    private int refreshRate;
    private int width;

    public float AspectRatio
    {
      get
      {
        return (float) (this.width / this.height);
      }
    }

    public SurfaceFormat Format
    {
      get
      {
        return this.format;
      }
    }

    public int Height
    {
      get
      {
        return this.height;
      }
    }

    public int RefreshRate
    {
      get
      {
        return this.refreshRate;
      }
    }

    public int Width
    {
      get
      {
        return this.width;
      }
    }

    public Rectangle TitleSafeArea
    {
      get
      {
        return new Rectangle(0, 0, this.Width, this.Height);
      }
    }

    internal DisplayMode(int width, int height, int refreshRate, SurfaceFormat format)
    {
      this.width = width;
      this.height = height;
      this.refreshRate = refreshRate;
      this.format = format;
    }

    public static bool operator !=(DisplayMode left, DisplayMode right)
    {
      return !(left == right);
    }

    public static bool operator ==(DisplayMode left, DisplayMode right)
    {
      if (left.format == right.format && left.height == right.height && left.refreshRate == right.refreshRate)
        return left.width == right.width;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (obj is DisplayMode)
        return this == (DisplayMode) obj;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.width.GetHashCode() ^ this.height.GetHashCode() ^ this.refreshRate.GetHashCode() ^ this.format.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{Width:{0} Height:{1} Format:{2} RefreshRate{3}}}", (object) this.width, (object) this.height, (object) this.Format, (object) this.refreshRate);
    }
  }
}
