// Type: SharpDX.Bool4
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [DynamicSerializer("TKB4")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Bool4 : IEquatable<Bool4>, IFormattable, IDataSerializable
  {
    public static readonly int SizeInBytes = Marshal.SizeOf(typeof (Bool4));
    public static readonly Bool4 False = new Bool4();
    public static readonly Bool4 UnitX = new Bool4(true, false, false, false);
    public static readonly Bool4 UnitY = new Bool4(false, true, false, false);
    public static readonly Bool4 UnitZ = new Bool4(false, false, true, false);
    public static readonly Bool4 UnitW = new Bool4(false, false, false, true);
    public static readonly Bool4 One = new Bool4(true, true, true, true);
    private int iX;
    private int iY;
    private int iZ;
    private int iW;

    public bool X
    {
      get
      {
        return this.iX != 0;
      }
      set
      {
        this.iX = value ? 1 : 0;
      }
    }

    public bool Y
    {
      get
      {
        return this.iY != 0;
      }
      set
      {
        this.iY = value ? 1 : 0;
      }
    }

    public bool Z
    {
      get
      {
        return this.iZ != 0;
      }
      set
      {
        this.iZ = value ? 1 : 0;
      }
    }

    public bool W
    {
      get
      {
        return this.iW != 0;
      }
      set
      {
        this.iW = value ? 1 : 0;
      }
    }

    public bool this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.X;
          case 1:
            return this.Y;
          case 2:
            return this.Z;
          case 3:
            return this.W;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Bool4 run from 0 to 3, inclusive.");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.X = value;
            break;
          case 1:
            this.Y = value;
            break;
          case 2:
            this.Z = value;
            break;
          case 3:
            this.W = value;
            break;
          default:
            throw new ArgumentOutOfRangeException("index", "Indices for Bool4 run from 0 to 3, inclusive.");
        }
      }
    }

    static Bool4()
    {
    }

    public Bool4(bool value)
    {
      this.iX = value ? 1 : 0;
      this.iY = value ? 1 : 0;
      this.iZ = value ? 1 : 0;
      this.iW = value ? 1 : 0;
    }

    public Bool4(bool x, bool y, bool z, bool w)
    {
      this.iX = x ? 1 : 0;
      this.iY = y ? 1 : 0;
      this.iZ = z ? 1 : 0;
      this.iW = w ? 1 : 0;
    }

    public Bool4(bool[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length != 4)
        throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Bool4.");
      this.iX = values[0] ? 1 : 0;
      this.iY = values[1] ? 1 : 0;
      this.iZ = values[2] ? 1 : 0;
      this.iW = values[3] ? 1 : 0;
    }

    public static implicit operator Bool4(bool[] input)
    {
      return new Bool4(input);
    }

    public static implicit operator bool[](Bool4 input)
    {
      return input.ToArray();
    }

    public static bool operator ==(Bool4 left, Bool4 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Bool4 left, Bool4 right)
    {
      return !left.Equals(right);
    }

    public bool[] ToArray()
    {
      return new bool[4]
      {
        this.X,
        this.Y,
        this.Z,
        this.W
      };
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", (object) (bool) (this.X ? 1 : 0), (object) (bool) (this.Y ? 1 : 0), (object) (bool) (this.Z ? 1 : 0), (object) (bool) (this.W ? 1 : 0));
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      return string.Format(formatProvider, format, (object) (bool) (this.X ? 1 : 0), (object) (bool) (this.Y ? 1 : 0), (object) (bool) (this.Z ? 1 : 0), (object) (bool) (this.W ? 1 : 0));
    }

    public override int GetHashCode()
    {
      return this.iX.GetHashCode() + this.iY.GetHashCode() + this.iZ.GetHashCode() + this.iW.GetHashCode();
    }

    public bool Equals(Bool4 other)
    {
      if (other.X == this.X && other.Y == this.Y && other.Z == this.Z)
        return other.W == this.W;
      else
        return false;
    }

    public override bool Equals(object value)
    {
      if (value == null || !object.ReferenceEquals((object) value.GetType(), (object) typeof (Bool4)))
        return false;
      else
        return this.Equals((Bool4) value);
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
      {
        serializer.Writer.Write(this.iX);
        serializer.Writer.Write(this.iY);
        serializer.Writer.Write(this.iZ);
        serializer.Writer.Write(this.iW);
      }
      else
      {
        this.iX = serializer.Reader.ReadInt32();
        this.iY = serializer.Reader.ReadInt32();
        this.iZ = serializer.Reader.ReadInt32();
        this.iW = serializer.Reader.ReadInt32();
      }
    }
  }
}
