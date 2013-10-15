// Type: SharpDX.Half
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Design;
using SharpDX.Serialization;
using System;
using System.ComponentModel;
using System.Globalization;

namespace SharpDX
{
  [DynamicSerializer("TKH1")]
  [TypeConverter(typeof (HalfConverter))]
  [Serializable]
  public struct Half : IDataSerializable
  {
    public static readonly float Epsilon = 0.0004887581f;
    public static readonly float MaxValue = 65504f;
    public static readonly float MinValue = 6.103516E-05f;
    public const int PrecisionDigits = 3;
    public const int MantissaBits = 11;
    public const int MaximumDecimalExponent = 4;
    public const int MaximumBinaryExponent = 15;
    public const int MinimumDecimalExponent = -4;
    public const int MinimumBinaryExponent = -14;
    public const int ExponentRadix = 2;
    public const int AdditionRounding = 1;
    private ushort value;

    public ushort RawValue
    {
      get
      {
        return this.value;
      }
      set
      {
        this.value = value;
      }
    }

    static Half()
    {
    }

    public Half(float value)
    {
      this.value = HalfUtils.Pack(value);
    }

    public static implicit operator Half(float value)
    {
      return new Half(value);
    }

    public static implicit operator float(Half value)
    {
      return HalfUtils.Unpack(value.value);
    }

    public static bool operator ==(Half left, Half right)
    {
      return (int) left.value == (int) right.value;
    }

    public static bool operator !=(Half left, Half right)
    {
      return (int) left.value != (int) right.value;
    }

    public static float[] ConvertToFloat(Half[] values)
    {
      float[] numArray = new float[values.Length];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = HalfUtils.Unpack(values[index].RawValue);
      return numArray;
    }

    public static Half[] ConvertToHalf(float[] values)
    {
      Half[] halfArray = new Half[values.Length];
      for (int index = 0; index < halfArray.Length; ++index)
        halfArray[index] = new Half(values[index]);
      return halfArray;
    }

    public override string ToString()
    {
      return (float) this.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    public override int GetHashCode()
    {
      ushort num = this.value;
      return (int) num * 3 / 2 ^ (int) num;
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      if (serializer.Mode == SerializerMode.Write)
        serializer.Writer.Write(this.value);
      else
        this.value = serializer.Reader.ReadUInt16();
    }

    public static bool Equals(ref Half value1, ref Half value2)
    {
      return (int) value1.value == (int) value2.value;
    }

    public bool Equals(Half other)
    {
      return (int) other.value == (int) this.value;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !object.ReferenceEquals((object) obj.GetType(), (object) typeof (Half)))
        return false;
      else
        return (int) ((Half) obj).value == (int) this.value;
    }
  }
}
