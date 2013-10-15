// Type: SharpDX.SingletonString
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;

namespace SharpDX
{
  public struct SingletonString : IEquatable<SingletonString>, IDataSerializable
  {
    private int hashCode;
    private string text;

    public SingletonString(string text)
    {
      this = new SingletonString();
      this.text = string.Intern(text);
      this.hashCode = text != null ? text.GetHashCode() : 0;
    }

    public static implicit operator string(SingletonString value)
    {
      return value.text;
    }

    public static explicit operator SingletonString(string value)
    {
      return new SingletonString(value);
    }

    public static bool operator ==(SingletonString left, SingletonString right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(SingletonString left, SingletonString right)
    {
      return !left.Equals(right);
    }

    public static bool operator ==(SingletonString left, string right)
    {
      return string.Equals(left.text, right);
    }

    public static bool operator !=(SingletonString left, string right)
    {
      return !string.Equals(left.text, right);
    }

    public bool Equals(SingletonString other)
    {
      if (this.hashCode == other.hashCode)
        return object.ReferenceEquals((object) this.text, (object) other.text);
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || !(obj is SingletonString))
        return false;
      else
        return this.Equals((SingletonString) obj);
    }

    public override int GetHashCode()
    {
      return this.hashCode;
    }

    public void Serialize(BinarySerializer serializer)
    {
      serializer.Serialize(ref this.text, SerializeFlags.Nullable);
      if (serializer.Mode != SerializerMode.Read)
        return;
      this.hashCode = this.text != null ? this.text.GetHashCode() : 0;
    }

    public override string ToString()
    {
      return this.text;
    }
  }
}
