// Type: SharpDX.DrawingPointF
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;

namespace SharpDX
{
  [Serializable]
  public struct DrawingPointF : IEquatable<DrawingPointF>, IDataSerializable
  {
    public float X;
    public float Y;

    public DrawingPointF(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }

    public static implicit operator DrawingPointF(Vector2 input)
    {
      return new DrawingPointF(input.X, input.Y);
    }

    public static implicit operator Vector2(DrawingPointF input)
    {
      return new Vector2(input.X, input.Y);
    }

    public static bool operator ==(DrawingPointF left, DrawingPointF right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(DrawingPointF left, DrawingPointF right)
    {
      return !left.Equals(right);
    }

    public bool Equals(DrawingPointF other)
    {
      if ((double) other.X == (double) this.X)
        return (double) other.Y == (double) this.Y;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (DrawingPointF))
        return false;
      else
        return this.Equals((DrawingPointF) obj);
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() * 397 ^ this.Y.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("({0},{1})", (object) this.X, (object) this.Y);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.X);
        serializer.Writer.Write(this.Y);
      }
      else
      {
        this.X = serializer.Reader.ReadSingle();
        this.Y = serializer.Reader.ReadSingle();
      }
    }
  }
}
