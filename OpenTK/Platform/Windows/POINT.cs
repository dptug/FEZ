// Type: OpenTK.Platform.Windows.POINT
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Drawing;

namespace OpenTK.Platform.Windows
{
  internal struct POINT
  {
    internal int X;
    internal int Y;

    internal POINT(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    internal Point ToPoint()
    {
      return new Point(this.X, this.Y);
    }

    public override string ToString()
    {
      return "Point {" + this.X.ToString() + ", " + this.Y.ToString() + ")";
    }
  }
}
