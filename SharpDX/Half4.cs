// Type: SharpDX.Half4
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Design;
using SharpDX.Serialization;
using System;
using System.ComponentModel;

namespace SharpDX
{
  [TypeConverter(typeof (Half4Converter))]
  [DynamicSerializer("TKH4")]
  [Serializable]
  public struct Half4 : IEquatable<Half4>, IDataSerializable
  {
    public Half X;
    public Half Y;
    public Half Z;
    public Half W;

    public Half4(Half x, Half y, Half z, Half w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public Half4(Half value)
    {
      this.X = value;
      this.Y = value;
      this.Z = value;
      this.W = value;
    }

    public static implicit operator Half4(Vector4 value)
    {
      return new Half4((Half) value.X, (Half) value.Y, (Half) value.Z, (Half) value.W);
    }

    public static implicit operator Vector4(Half4 value)
    {
      return new Vector4((float) value.X, (float) value.Y, (float) value.Z, (float) value.W);
    }

    public static explicit operator Half4(Vector3 value)
    {
      return new Half4((Half) value.X, (Half) value.Y, (Half) value.Z, (Half) 0.0f);
    }

    public static explicit operator Vector3(Half4 value)
    {
      return new Vector3((float) value.X, (float) value.Y, (float) value.Z);
    }

    public static explicit operator Half4(Vector2 value)
    {
      return new Half4((Half) value.X, (Half) value.Y, (Half) 0.0f, (Half) 0.0f);
    }

    public static explicit operator Vector2(Half4 value)
    {
      return new Vector2((float) value.X, (float) value.Y);
    }

    public static bool operator ==(Half4 left, Half4 right)
    {
      return Half4.Equals(ref left, ref right);
    }

    public static bool operator !=(Half4 left, Half4 right)
    {
      return !Half4.Equals(ref left, ref right);
    }

    public override int GetHashCode()
    {
      return this.X.GetHashCode() + (this.Y.GetHashCode() + (this.W.GetHashCode() + this.Z.GetHashCode()));
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.X.RawValue);
        serializer.Writer.Write(this.Y.RawValue);
        serializer.Writer.Write(this.Z.RawValue);
        serializer.Writer.Write(this.W.RawValue);
      }
      else
      {
        this.X.RawValue = serializer.Reader.ReadUInt16();
        this.Y.RawValue = serializer.Reader.ReadUInt16();
        this.Z.RawValue = serializer.Reader.ReadUInt16();
        this.W.RawValue = serializer.Reader.ReadUInt16();
      }
    }

    public static bool Equals(ref Half4 value1, ref Half4 value2)
    {
      if (value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z)
        return value1.W == value2.W;
      else
        return false;
    }

    public bool Equals(Half4 other)
    {
      if (this.X == other.X && this.Y == other.Y && this.Z == other.Z)
        return this.W == other.W;
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !object.ReferenceEquals((object) obj.GetType(), (object) typeof (Half4)))
        return false;
      else
        return this.Equals((Half4) obj);
    }
  }
}
