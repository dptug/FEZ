// Type: SharpDX.RectangleF
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;

namespace SharpDX
{
  [Serializable]
  public struct RectangleF : IEquatable<RectangleF>, IDataSerializable
  {
    public static readonly RectangleF Empty = new RectangleF();
    private float _left;
    private float _top;
    private float _right;
    private float _bottom;

    public float Left
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

    public float Top
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

    public float Right
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

    public float Bottom
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

    public float X
    {
      get
      {
        return this._left;
      }
    }

    public float Y
    {
      get
      {
        return this._top;
      }
    }

    public float Width
    {
      get
      {
        return this.Right - this.Left;
      }
    }

    public float Height
    {
      get
      {
        return this.Bottom - this.Top;
      }
    }

    static RectangleF()
    {
    }

    public RectangleF(float left, float top, float right, float bottom)
    {
      this._left = left;
      this._top = top;
      this._right = right;
      this._bottom = bottom;
    }

    public static implicit operator RectangleF(DrawingRectangleF input)
    {
      return new RectangleF(input.X, input.Y, input.X + input.Width, input.Y + input.Height);
    }

    public static implicit operator DrawingRectangleF(RectangleF input)
    {
      return new DrawingRectangleF(input.Left, input.Top, input.Right - input.Left, input.Bottom - input.Top);
    }

    public static bool operator ==(RectangleF left, RectangleF right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(RectangleF left, RectangleF right)
    {
      return !(left == right);
    }

    public bool Contains(int x, int y)
    {
      return (double) x >= (double) this._left && (double) x <= (double) this._right && ((double) y >= (double) this._top && (double) y <= (double) this._bottom);
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
      return (double) point.X >= (double) this._left && (double) point.X <= (double) this._right && ((double) point.Y >= (double) this._top && (double) point.Y <= (double) this._bottom);
    }

    public bool Contains(DrawingPointF point)
    {
      return (double) point.X >= (double) this._left && (double) point.X <= (double) this._right && ((double) point.Y >= (double) this._top && (double) point.Y <= (double) this._bottom);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (RectangleF))
        return false;
      else
        return this.Equals((RectangleF) obj);
    }

    public bool Equals(RectangleF other)
    {
      if ((double) Math.Abs(other.Left - this.Left) < 9.99999997475243E-07 && (double) Math.Abs(other.Right - this.Right) < 9.99999997475243E-07 && (double) Math.Abs(other.Top - this.Top) < 9.99999997475243E-07)
        return (double) Math.Abs(other.Bottom - this.Bottom) < 9.99999997475243E-07;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return ((this._left.GetHashCode() * 397 ^ this._top.GetHashCode()) * 397 ^ this._right.GetHashCode()) * 397 ^ this._bottom.GetHashCode();
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
        this._left = serializer.Reader.ReadSingle();
        this._top = serializer.Reader.ReadSingle();
        this._right = serializer.Reader.ReadSingle();
        this._bottom = serializer.Reader.ReadSingle();
      }
    }
  }
}
