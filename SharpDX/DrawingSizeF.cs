// Type: SharpDX.DrawingSizeF
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;

namespace SharpDX
{
  [Serializable]
  public struct DrawingSizeF : IEquatable<DrawingSizeF>, IDataSerializable
  {
    public float Width;
    public float Height;

    public DrawingSizeF(float width, float height)
    {
      this.Width = width;
      this.Height = height;
    }

    public static bool operator ==(DrawingSizeF left, DrawingSizeF right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(DrawingSizeF left, DrawingSizeF right)
    {
      return !left.Equals(right);
    }

    public bool Equals(DrawingSizeF other)
    {
      if ((double) other.Width == (double) this.Width)
        return (double) other.Height == (double) this.Height;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (DrawingSizeF))
        return false;
      else
        return this.Equals((DrawingSizeF) obj);
    }

    public override int GetHashCode()
    {
      return this.Width.GetHashCode() * 397 ^ this.Height.GetHashCode();
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
        this.Width = serializer.Reader.ReadSingle();
        this.Height = serializer.Reader.ReadSingle();
      }
    }
  }
}
