// Type: SharpDX.Bool
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [DynamicSerializer("TKB1")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Size = 4)]
  public struct Bool : IEquatable<Bool>, IDataSerializable
  {
    private int boolValue;

    public Bool(bool boolValue)
    {
      this.boolValue = boolValue ? 1 : 0;
    }

    public static implicit operator bool(Bool booleanValue)
    {
      return booleanValue.boolValue != 0;
    }

    public static implicit operator Bool(bool boolValue)
    {
      return new Bool(boolValue);
    }

    public static bool operator ==(Bool left, Bool right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Bool left, Bool right)
    {
      return !left.Equals(right);
    }

    public bool Equals(Bool other)
    {
      return this.boolValue == other.boolValue;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || !(obj is Bool))
        return false;
      else
        return this.Equals((Bool) obj);
    }

    public override int GetHashCode()
    {
      return this.boolValue;
    }

    public override string ToString()
    {
      return string.Format("{0}", (object) (bool) (this.boolValue != 0 ? 1 : 0));
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
        serializer.Writer.Write(this.boolValue);
      else
        this.boolValue = serializer.Reader.ReadInt32();
    }
  }
}
