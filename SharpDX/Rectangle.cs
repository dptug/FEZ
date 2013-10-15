// Type: SharpDX.Rectangle
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;

namespace SharpDX
{
  [Serializable]
  public struct Rectangle : IEquatable<Rectangle>, IDataSerializable
  {
    public static readonly Rectangle Empty = new Rectangle();
    private int _left;
    private int _top;
    private int _right;
    private int _bottom;

    public int Left
    {
      get
      {
        return this._left;
      }
      set
      {
        this._left = value;
      }
    }

    public int Top
    {
      get
      {
        return this._top;
      }
      set
      {
        this._top = value;
      }
    }

    public int Right
    {
      get
      {
        return this._right;
      }
      set
      {
        this._right = value;
      }
    }

    public int Bottom
    {
      get
      {
        return this._bottom;
      }
      set
      {
        this._bottom = value;
      }
    }

    public int X
    {
      get
      {
        return this._left;
      }
    }

    public int Y
    {
      get
      {
        return this._top;
      }
    }

    public int Width
    {
      get
      {
        return this.Right - this.Left;
      }
    }

    public int Height
    {
      get
      {
        return this.Bottom - this.Top;
      }
    }

    static Rectangle()
    {
    }

    public Rectangle(int left, int top, int right, int bottom)
    {
      this._left = left;
      this._top = top;
      this._right = right;
      this._bottom = bottom;
    }

    public static implicit operator Rectangle(DrawingRectangle input)
    {
      return new Rectangle(input.X, input.Y, input.X + input.Width, input.Y + input.Height);
    }

    public static implicit operator DrawingRectangle(Rectangle input)
    {
      return new DrawingRectangle(input.Left, input.Top, input.Right - input.Left, input.Bottom - input.Top);
    }

    public static bool operator ==(Rectangle left, Rectangle right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Rectangle left, Rectangle right)
    {
      return !(left == right);
    }

    public bool Contains(int x, int y)
    {
      return x >= this._left && x <= this._right && (y >= this._top && y <= this._bottom);
    }

    public bool Contains(float x, float y)
    {
      return (double) x >= (double) this._left && (double) x <= (double) this._right && ((double) y >= (double) this._top && (double) y <= (double) this._bottom);
    }

    public bool Contains(Vector2 vector2D)
    {
      return (double) vector2D.X >= (double) this._left && (double) vector2D.X <= (double) this._right && ((double) vector2D.Y >= (double) this._top && (double) vector2D.Y <= (double) this._bottom);
    }

    public bool Contains(DrawingPoint point)
    {
      return point.X >= this._left && point.X <= this._right && (point.Y >= this._top && point.Y <= this._bottom);
    }

    public bool Contains(DrawingPointF point)
    {
      return (double) point.X >= (double) this._left && (double) point.X <= (double) this._right && ((double) point.Y >= (double) this._top && (double) point.Y <= (double) this._bottom);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (Rectangle))
        return false;
      else
        return this.Equals((Rectangle) obj);
    }

    public bool Equals(Rectangle other)
    {
      if (other._left == this._left && other._top == this._top && other._right == this._right)
        return other._bottom == this._bottom;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return ((this._left * 397 ^ this._top) * 397 ^ this._right) * 397 ^ this._bottom;
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this._left);
        serializer.Writer.Write(this._top);
        serializer.Writer.Write(this._right);
        serializer.Writer.Write(this._bottom);
      }
      else
      {
        this._left = serializer.Reader.ReadInt32();
        this._top = serializer.Reader.ReadInt32();
        this._right = serializer.Reader.ReadInt32();
        this._bottom = serializer.Reader.ReadInt32();
      }
    }
  }
}
