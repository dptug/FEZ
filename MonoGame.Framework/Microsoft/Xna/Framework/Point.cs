// Type: Microsoft.Xna.Framework.Point
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework
{
  public struct Point : IEquatable<Point>
  {
    private static Point zeroPoint = new Point();
    public int X;
    public int Y;

    public static Point Zero
    {
      get
      {
        return Point.zeroPoint;
      }
    }

    static Point()
    {
    }

    public Point(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    public static bool operator ==(Point a, Point b)
    {
      return a.Equals(b);
    }

    public static bool operator !=(Point a, Point b)
    {
      return !a.Equals(b);
    }

    public bool Equals(Point other)
    {
      if (this.X == other.X)
        return this.Y == other.Y;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Point))
        return false;
      else
        return this.Equals((Point) obj);
    }

    public override int GetHashCode()
    {
      return this.X ^ this.Y;
    }

    public override string ToString()
    {
      return string.Format("{{X:{0} Y:{1}}}", (object) this.X, (object) this.Y);
    }
  }
}
