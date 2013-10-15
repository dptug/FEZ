// Type: SharpDX.Half3
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Design;
using SharpDX.Serialization;
using System;
using System.ComponentModel;

namespace SharpDX
{
  [DynamicSerializer("TKH3")]
  [TypeConverter(typeof (Half3Converter))]
  [Serializable]
  public struct Half3 : IEquatable<Half3>, IDataSerializable
  {
    public Half X;
    public Half Y;
    public Half Z;

    public Half3(Half x, Half y, Half z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Half3(Half value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
    }

    public static implicit operator Half3(Vector3 value)
    {
      return new Half3((Half) value.X, (Half) value.Y, (Half) value.Z);
    }

    public static implicit operator Vector3(Half3 value)
    {
      return new Vector3((float) value.X, (float) value.Y, (float) value.Z);
    }

    public static explicit operator Half3(Vector2 value)
    {
      return new Half3((Half) value.X, (Half) value.Y, (Half) 0.0f);
    }

    public static explicit operator Vector2(Half3 value)
    {
      return new Vector2((float) value.X, (float) value.Y);
    }

    public static bool operator ==(Half3 left, Half3 right)
    {
      return Half3.Equals(ref left, ref right);
    }

    public static bool operator !=(Half3 left, Half3 right)
    {
      return !Half3.Equals(ref left, ref right);
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() + (this.Z.GetHashCode() + this.Y.GetHashCode());
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.X.RawValue);
        serializer.Writer.Write(this.Y.RawValue);
        serializer.Writer.Write(this.Z.RawValue);
      }
      else
      {
        this.X.RawValue = serializer.Reader.ReadUInt16();
        this.Y.RawValue = serializer.Reader.ReadUInt16();
        this.Z.RawValue = serializer.Reader.ReadUInt16();
      }
    }

    public static bool Equals(ref Half3 value1, ref Half3 value2)
    {
      if (value1.X == value2.X && value1.Y == value2.Y)
        return value1.Z == value2.Z;
      else
        return false;
    }

    public bool Equals(Half3 other)
    {
      if (this.X == other.X && this.Y == other.Y)
        return this.Z == other.Z;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !object.ReferenceEquals((object) obj.GetType(), (object) typeof (Half3)))
        return false;
      else
        return this.Equals((Half3) obj);
    }
  }
}
