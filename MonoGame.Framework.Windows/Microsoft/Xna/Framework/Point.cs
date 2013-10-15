// Type: Microsoft.Xna.Framework.Point
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
      return this.X == other.X && this.Y == other.Y;
    }

    public override bool Equals(object obj)
    {
      return obj is Point && this.Equals((Point) obj);
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
