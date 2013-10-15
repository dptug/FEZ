// Type: SharpDX.Multimedia.FourCC
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX.Multimedia
{
  [StructLayout(LayoutKind.Sequential, Size = 4)]
  public struct FourCC : IEquatable<FourCC>, IDataSerializable
  {
    public static readonly FourCC Empty = new FourCC(0);
    private uint value;

    static FourCC()
    {
    }

    public FourCC(string fourCC)
    {
      if (fourCC.Length != 4)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Invalid length for FourCC(\"{0}\". Must be be 4 characters long ", new object[1]
        {
          (object) fourCC
        }), "fourCC");
      else
        this.value = (uint) ((int) fourCC[3] << 24 | (int) fourCC[2] << 16 | (int) fourCC[1] << 8) | (uint) fourCC[0];
    }

    public FourCC(char byte1, char byte2, char byte3, char byte4)
    {
      this.value = (uint) ((int) byte4 << 24 | (int) byte3 << 16 | (int) byte2 << 8) | (uint) byte1;
    }

    public FourCC(uint fourCC)
    {
      this.value = fourCC;
    }

    public FourCC(int fourCC)
    {
      this.value = (uint) fourCC;
    }

    public static implicit operator uint(FourCC d)
    {
      return d.value;
    }

    public static implicit operator int(FourCC d)
    {
      return (int) d.value;
    }

    public static implicit operator FourCC(uint d)
    {
      return new FourCC(d);
    }

    public static implicit operator FourCC(int d)
    {
      return new FourCC(d);
    }

    public static implicit operator string(FourCC d)
    {
      return d.ToString();
    }

    public static implicit operator FourCC(string d)
    {
      return new FourCC(d);
    }

    public static bool operator ==(FourCC left, FourCC right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(FourCC left, FourCC right)
    {
      return !left.Equals(right);
    }

    public override string ToString()
    {
      return string.Format("{0}", (object) new string(new char[4]
      {
        (char) (this.value & (uint) byte.MaxValue),
        (char) (this.value >> 8 & (uint) byte.MaxValue),
        (char) (this.value >> 16 & (uint) byte.MaxValue),
        (char) (this.value >> 24 & (uint) byte.MaxValue)
      }));
    }

    public bool Equals(FourCC other)
    {
      return (int) this.value == (int) other.value;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || !(obj is FourCC))
        return false;
      else
        return this.Equals((FourCC) obj);
    }

    public override int GetHashCode()
    {
      return (int) this.value;
    }

    public void Serialize(BinarySerializer serializer)
    {
      serializer.Serialize(ref this.value);
    }
  }
}
