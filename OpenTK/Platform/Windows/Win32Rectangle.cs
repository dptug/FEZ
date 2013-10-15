// Type: OpenTK.Platform.Windows.Win32Rectangle
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Drawing;

namespace OpenTK.Platform.Windows
{
  internal struct Win32Rectangle
  {
    internal int left;
    internal int top;
    internal int right;
    internal int bottom;

    internal int Width
    {
      get
      {
        return this.right - this.left;
      }
    }

    internal int Height
    {
      get
      {
        return this.bottom - this.top;
      }
    }

    public override string ToString()
    {
      return string.Format("({0},{1})-({2},{3})", (object) this.left, (object) this.top, (object) this.right, (object) this.bottom);
    }

    internal Rectangle ToRectangle()
    {
      return Rectangle.FromLTRB(this.left, this.top, this.right, this.bottom);
    }

    internal static Win32Rectangle From(Rectangle value)
    {
      return new Win32Rectangle()
      {
        left = value.Left,
        right = value.Right,
        top = value.Top,
        bottom = value.Bottom
      };
    }

    internal static Win32Rectangle From(Size value)
    {
      return new Win32Rectangle()
      {
        left = 0,
        right = value.Width,
        top = 0,
        bottom = value.Height
      };
    }
  }
}
