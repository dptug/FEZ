// Type: OpenTK.Platform.MacOS.Carbon.Rect
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Drawing;

namespace OpenTK.Platform.MacOS.Carbon
{
  internal struct Rect
  {
    private short top;
    private short left;
    private short bottom;
    private short right;

    internal short X
    {
      get
      {
        return this.left;
      }
      set
      {
        short width = this.Width;
        this.left = value;
        this.right = (short) ((int) this.left + (int) width);
      }
    }

    internal short Y
    {
      get
      {
        return this.top;
      }
      set
      {
        short height = this.Height;
        this.top = value;
        this.bottom = (short) ((int) this.top + (int) height);
      }
    }

    internal short Width
    {
      get
      {
        return (short) ((int) this.right - (int) this.left);
      }
      set
      {
        this.right = (short) ((int) this.left + (int) value);
      }
    }

    internal short Height
    {
      get
      {
        return (short) ((int) this.bottom - (int) this.top);
      }
      set
      {
        this.bottom = (short) ((int) this.top + (int) value);
      }
    }

    internal Rect(short _left, short _top, short _width, short _height)
    {
      this.top = _top;
      this.left = _left;
      this.bottom = (short) ((int) _top + (int) _height);
      this.right = (short) ((int) _left + (int) _width);
    }

    public override string ToString()
    {
      return string.Format("Rect: [{0}, {1}, {2}, {3}]", (object) this.X, (object) this.Y, (object) this.Width, (object) this.Height);
    }

    public Rectangle ToRectangle()
    {
      return new Rectangle((int) this.X, (int) this.Y, (int) this.Width, (int) this.Height);
    }
  }
}
