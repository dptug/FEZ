// Type: Microsoft.Xna.Framework.Rectangle
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework
{
  public struct Rectangle : IEquatable<Rectangle>
  {
    private static Rectangle emptyRectangle = new Rectangle();
    public int X;
    public int Y;
    public int Width;
    public int Height;

    public static Rectangle Empty
    {
      get
      {
        return Rectangle.emptyRectangle;
      }
    }

    public int Left
    {
      get
      {
        return this.X;
      }
    }

    public int Right
    {
      get
      {
        return this.X + this.Width;
      }
    }

    public int Top
    {
      get
      {
        return this.Y;
      }
    }

    public int Bottom
    {
      get
      {
        return this.Y + this.Height;
      }
    }

    public Point Location
    {
      get
      {
        return new Point(this.X, this.Y);
      }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
      }
    }

    public Point Center
    {
      get
      {
        return new Point(this.X + this.Width / 2, this.Y + this.Height / 2);
      }
    }

    public bool IsEmpty
    {
      get
      {
        if (this.Width == 0 && this.Height == 0 && this.X == 0)
          return this.Y == 0;
        else
          return false;
      }
    }

    static Rectangle()
    {
    }

    public Rectangle(int x, int y, int width, int height)
    {
      this.X = x;
      this.Y = y;
      this.Width = width;
      this.Height = height;
    }

    public static bool operator ==(Rectangle a, Rectangle b)
    {
      if (a.X == b.X && a.Y == b.Y && a.Width == b.Width)
        return a.Height == b.Height;
      else
        return false;
    }

    public static bool operator !=(Rectangle a, Rectangle b)
    {
      return !(a == b);
    }

    public bool Contains(int x, int y)
    {
      if (this.X <= x && x < this.X + this.Width && this.Y <= y)
        return y < this.Y + this.Height;
      else
        return false;
    }

    public bool Contains(Point value)
    {
      if (this.X <= value.X && value.X < this.X + this.Width && this.Y <= value.Y)
        return value.Y < this.Y + this.Height;
      else
        return false;
    }

    public bool Contains(Rectangle value)
    {
      if (this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y)
        return value.Y + value.Height <= this.Y + this.Height;
      else
        return false;
    }

    public void Offset(Point offset)
    {
      this.X += offset.X;
      this.Y += offset.Y;
    }

    public void Offset(int offsetX, int offsetY)
    {
      this.X += offsetX;
      this.Y += offsetY;
    }

    public void Inflate(int horizontalValue, int verticalValue)
    {
      this.X -= horizontalValue;
      this.Y -= verticalValue;
      this.Width += horizontalValue * 2;
      this.Height += verticalValue * 2;
    }

    public bool Equals(Rectangle other)
    {
      return this == other;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Rectangle))
        return false;
      else
        return this == (Rectangle) obj;
    }

    public override string ToString()
    {
      return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", (object) this.X, (object) this.Y, (object) this.Width, (object) this.Height);
    }

    public override int GetHashCode()
    {
      return this.X ^ this.Y ^ this.Width ^ this.Height;
    }

    public bool Intersects(Rectangle value)
    {
      if (value.Left < this.Right && this.Left < value.Right && value.Top < this.Bottom)
        return this.Top < value.Bottom;
      else
        return false;
    }

    public void Intersects(ref Rectangle value, out bool result)
    {
      result = value.Left < this.Right && this.Left < value.Right && value.Top < this.Bottom && this.Top < value.Bottom;
    }

    public static Rectangle Intersect(Rectangle value1, Rectangle value2)
    {
      Rectangle result;
      Rectangle.Intersect(ref value1, ref value2, out result);
      return result;
    }

    public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
    {
      if (value1.Intersects(value2))
      {
        int num1 = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
        int x = Math.Max(value1.X, value2.X);
        int y = Math.Max(value1.Y, value2.Y);
        int num2 = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
        result = new Rectangle(x, y, num1 - x, num2 - y);
      }
      else
        result = new Rectangle(0, 0, 0, 0);
    }

    public static Rectangle Union(Rectangle value1, Rectangle value2)
    {
      int x = Math.Min(value1.X, value2.X);
      int y = Math.Min(value1.Y, value2.Y);
      return new Rectangle(x, y, Math.Max(value1.Right, value2.Right) - x, Math.Max(value1.Bottom, value2.Bottom) - y);
    }

    public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
    {
      result.X = Math.Min(value1.X, value2.X);
      result.Y = Math.Min(value1.Y, value2.Y);
      result.Width = Math.Max(value1.Right, value2.Right) - result.X;
      result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
    }
  }
}
