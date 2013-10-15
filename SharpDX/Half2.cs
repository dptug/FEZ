// Type: SharpDX.Half2
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Design;
using SharpDX.Serialization;
using System;
using System.ComponentModel;

namespace SharpDX
{
  [TypeConverter(typeof (Half2Converter))]
  [DynamicSerializer("TKH2")]
  [Serializable]
  public struct Half2 : IEquatable<Half2>, IDataSerializable
  {
    public Half X;
    public Half Y;

    public Half2(Half x, Half y)
    {
      this.X = x;
      this.Y = y;
    }

    public Half2(Half value)
    {
      this.X = value;
      this.Y = value;
    }

    public static implicit operator Half2(Vector2 value)
    {
      return new Half2((Half) value.X, (Half) value.Y);
    }

    public static implicit operator Vector2(Half2 value)
    {
      return new Vector2((float) value.X, (float) value.Y);
    }

    public static bool operator ==(Half2 left, Half2 right)
    {
      return Half2.Equals(ref left, ref right);
    }

    public static bool operator !=(Half2 left, Half2 right)
    {
      return !Half2.Equals(ref left, ref right);
    }

    public override int GetHashCode()
    {
      return this.Y.GetHashCode() + this.X.GetHashCode();
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.X.RawValue);
        serializer.Writer.Write(this.Y.RawValue);
      }
      else
      {
        this.X.RawValue = serializer.Reader.ReadUInt16();
        this.Y.RawValue = serializer.Reader.ReadUInt16();
      }
    }

    public static bool Equals(ref Half2 value1, ref Half2 value2)
    {
      if (value1.X == value2.X)
        return value1.Y == value2.Y;
      else
        return false;
    }

    public bool Equals(Half2 other)
    {
      if (this.X == other.X)
        return this.Y == other.Y;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !object.ReferenceEquals((object) obj.GetType(), (object) typeof (Half2)))
        return false;
      else
        return this.Equals((Half2) obj);
    }
  }
}
