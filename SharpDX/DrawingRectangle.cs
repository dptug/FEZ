// Type: SharpDX.DrawingRectangle
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;

namespace SharpDX
{
  [Serializable]
  public struct DrawingRectangle : IEquatable<DrawingRectangle>, IDataSerializable
  {
    public static readonly DrawingRectangle Empty = new DrawingRectangle();
    public int X;
    public int Y;
    public int Width;
    public int Height;

    static DrawingRectangle()
    {
    }

    public DrawingRectangle(int x, int y, int width, int height)
    {
      this.X = x;
      this.Y = y;
      this.Width = width;
      this.Height = height;
    }

    public static bool operator ==(DrawingRectangle left, DrawingRectangle right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(DrawingRectangle left, DrawingRectangle right)
    {
      return !left.Equals(right);
    }

    public bool Contains(int x, int y)
    {
      return x >= this.X && x <= this.X + this.Width && (y >= this.Y && y <= this.Y + this.Width);
    }

    public bool Contains(float x, float y)
    {
      return (double) x >= (double) this.X && (double) x <= (double) (this.X + this.Width) && ((double) y >= (double) this.Y && (double) y <= (double) (this.Y + this.Width));
    }

    public bool Contains(Vector2 vector2D)
    {
      return (double) vector2D.X >= (double) this.X && (double) vector2D.X <= (double) (this.X + this.Width) && ((double) vector2D.Y >= (double) this.Y && (double) vector2D.Y <= (double) (this.Y + this.Width));
    }

    public bool Contains(DrawingPoint point)
    {
      return point.X >= this.X && point.X <= this.X + this.Width && (point.Y >= this.Y && point.Y <= this.Y + this.Width);
    }

    public bool Contains(DrawingPointF point)
    {
      return (double) point.X >= (double) this.X && (double) point.X <= (double) (this.X + this.Width) && ((double) point.Y >= (double) this.Y && (double) point.Y <= (double) (this.Y + this.Width));
    }

    public bool Equals(DrawingRectangle other)
    {
      if (other.X == this.X && other.Y == this.Y && other.Width == this.Width)
        return other.Height == this.Height;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (DrawingRectangle))
        return false;
      else
        return this.Equals((DrawingRectangle) obj);
    }

    public override int GetHashCode()
    {
      return ((this.X * 397 ^ this.Y) * 397 ^ this.Width) * 397 ^ this.Height;
    }

    public override string ToString()
    {
      return string.Format("(X: {0} Y: {1} W: {2} H: {3})", (object) this.X, (object) this.Y, (object) this.Width, (object) this.Height);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.X);
        serializer.Writer.Write(this.Y);
        serializer.Writer.Write(this.Width);
        serializer.Writer.Write(this.Height);
      }
      else
      {
        this.X = serializer.Reader.ReadInt32();
        this.Y = serializer.Reader.ReadInt32();
        this.Width = serializer.Reader.ReadInt32();
        this.Height = serializer.Reader.ReadInt32();
      }
    }
  }
}
