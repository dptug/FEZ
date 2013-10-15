// Type: SharpDX.DrawingSize
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;

namespace SharpDX
{
  [Serializable]
  public struct DrawingSize : IEquatable<DrawingSize>, IDataSerializable
  {
    public int Width;
    public int Height;

    public DrawingSize(int width, int height)
    {
      this.Width = width;
      this.Height = height;
    }

    public static bool operator ==(DrawingSize left, DrawingSize right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(DrawingSize left, DrawingSize right)
    {
      return !left.Equals(right);
    }

    public bool Equals(DrawingSize other)
    {
      if (other.Width == this.Width)
        return other.Height == this.Height;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (DrawingSize))
        return false;
      else
        return this.Equals((DrawingSize) obj);
    }

    public override int GetHashCode()
    {
      return this.Width * 397 ^ this.Height;
    }

    public override string ToString()
    {
      return string.Format("({0},{1})", (object) this.Width, (object) this.Height);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.Width);
        serializer.Writer.Write(this.Height);
      }
      else
      {
        this.Width = serializer.Reader.ReadInt32();
        this.Height = serializer.Reader.ReadInt32();
      }
    }
  }
}
