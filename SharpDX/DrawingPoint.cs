// Type: SharpDX.DrawingPoint
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;

namespace SharpDX
{
  [Serializable]
  public struct DrawingPoint : IEquatable<DrawingPoint>, IDataSerializable
  {
    public int X;
    public int Y;

    public DrawingPoint(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    public static bool operator ==(DrawingPoint left, DrawingPoint right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(DrawingPoint left, DrawingPoint right)
    {
      return !left.Equals(right);
    }

    public bool Equals(DrawingPoint other)
    {
      if (other.X == this.X)
        return other.Y == this.Y;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (DrawingPoint))
        return false;
      else
        return this.Equals((DrawingPoint) obj);
    }

    public override int GetHashCode()
    {
      return this.X * 397 ^ this.Y;
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
        this.X = serializer.Reader.ReadInt32();
        this.Y = serializer.Reader.ReadInt32();
      }
    }
  }
}
