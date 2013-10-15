// Type: SharpDX.DrawingRectangleF
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;

namespace SharpDX
{
  [Serializable]
  public struct DrawingRectangleF : IEquatable<DrawingRectangleF>, IDataSerializable
  {
    public Vector2 Position;
    public Vector2 Size;

    public float X
    {
      get
      {
        return this.Position.X;
      }
      set
      {
        this.Position.X = value;
      }
    }

    public float Y
    {
      get
      {
        return this.Position.Y;
      }
      set
      {
        this.Position.Y = value;
      }
    }

    public float Width
    {
      get
      {
        return this.Size.X;
      }
      set
      {
        this.Size.X = value;
      }
    }

    public float Height
    {
      get
      {
        return this.Size.Y;
      }
      set
      {
        this.Size.Y = value;
      }
    }

    public DrawingRectangleF(Vector2 position, Vector2 size)
    {
      this.Position = position;
      this.Size = size;
    }

    public DrawingRectangleF(float x, float y, float width, float height)
    {
      this.Position = new Vector2(x, y);
      this.Size = new Vector2(width, height);
    }

    public static bool operator ==(DrawingRectangleF left, DrawingRectangleF right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(DrawingRectangleF left, DrawingRectangleF right)
    {
      return !left.Equals(right);
    }

    public bool Contains(int x, int y)
    {
      return (double) x >= (double) this.X && (double) x <= (double) this.X + (double) this.Width && ((double) y >= (double) this.Y && (double) y <= (double) this.Y + (double) this.Width);
    }

    public bool Contains(float x, float y)
    {
      return (double) x >= (double) this.X && (double) x <= (double) this.X + (double) this.Width && ((double) y >= (double) this.Y && (double) y <= (double) this.Y + (double) this.Width);
    }

    public bool Contains(Vector2 vector2D)
    {
      return (double) vector2D.X >= (double) this.X && (double) vector2D.X <= (double) this.X + (double) this.Width && ((double) vector2D.Y >= (double) this.Y && (double) vector2D.Y <= (double) this.Y + (double) this.Width);
    }

    public bool Contains(DrawingPoint point)
    {
      return (double) point.X >= (double) this.X && (double) point.X <= (double) this.X + (double) this.Width && ((double) point.Y >= (double) this.Y && (double) point.Y <= (double) this.Y + (double) this.Width);
    }

    public bool Contains(DrawingPointF point)
    {
      return (double) point.X >= (double) this.X && (double) point.X <= (double) this.X + (double) this.Width && ((double) point.Y >= (double) this.Y && (double) point.Y <= (double) this.Y + (double) this.Width);
    }

    public bool Equals(DrawingRectangleF other)
    {
      if (other.X.Equals(this.X) && other.Y.Equals(this.Y) && other.Width.Equals(this.Width))
        return other.Height.Equals(this.Height);
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (DrawingRectangleF))
        return false;
      else
        return this.Equals((DrawingRectangleF) obj);
    }

    public override int GetHashCode()
    {
      return ((this.X.GetHashCode() * 397 ^ this.Y.GetHashCode()) * 397 ^ this.Width.GetHashCode()) * 397 ^ this.Height.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("(X: {0} Y: {1} W: {2} H: {3})", (object) this.X, (object) this.Y, (object) this.Width, (object) this.Height);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.Position.X);
        serializer.Writer.Write(this.Position.Y);
        serializer.Writer.Write(this.Size.X);
        serializer.Writer.Write(this.Size.Y);
      }
      else
      {
        this.X = serializer.Reader.ReadSingle();
        this.Y = serializer.Reader.ReadSingle();
        this.Width = serializer.Reader.ReadSingle();
        this.Height = serializer.Reader.ReadSingle();
      }
    }
  }
}
