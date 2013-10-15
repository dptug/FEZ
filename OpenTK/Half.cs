// Type: OpenTK.Half
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

namespace OpenTK
{
  [Serializable]
  public struct Half : ISerializable, IComparable<Half>, IFormattable, IEquatable<Half>
  {
    public static readonly int SizeInBytes = 2;
    public static readonly float MinValue = 5.960464E-08f;
    public static readonly float MinNormalizedValue = 6.103516E-05f;
    public static readonly float MaxValue = 65504f;
    public static readonly float Epsilon = 0.00097656f;
    private const int maxUlps = 1;
    private ushort bits;

    public bool IsZero
    {
      get
      {
        if ((int) this.bits != 0)
          return (int) this.bits == 32768;
        else
          return true;
      }
    }

    public bool IsNaN
    {
      get
      {
        if (((int) this.bits & 31744) == 31744)
          return ((int) this.bits & 1023) != 0;
        else
          return false;
      }
    }

    public bool IsPositiveInfinity
    {
      get
      {
        return (int) this.bits == 31744;
      }
    }

    public bool IsNegativeInfinity
    {
      get
      {
        return (int) this.bits == 64512;
      }
    }

    static Half()
    {
    }

    public unsafe Half(float f)
    {
      this = new Half();
      this.bits = this.SingleToHalf(*(int*) &f);
    }

    public Half(float f, bool throwOnError)
    {
      this = new Half(f);
      if (!throwOnError)
        return;
      if ((double) f > (double) Half.MaxValue)
        throw new ArithmeticException("Half: Positive maximum value exceeded.");
      if ((double) f < -(double) Half.MaxValue)
        throw new ArithmeticException("Half: Negative minimum value exceeded.");
      if (float.IsNaN(f))
        throw new ArithmeticException("Half: Input is not a number (NaN).");
      if (float.IsPositiveInfinity(f))
        throw new ArithmeticException("Half: Input is positive infinity.");
      if (float.IsNegativeInfinity(f))
        throw new ArithmeticException("Half: Input is negative infinity.");
    }

    public Half(double d)
    {
      this = new Half((float) d);
    }

    public Half(double d, bool throwOnError)
    {
      this = new Half((float) d, throwOnError);
    }

    public Half(SerializationInfo info, StreamingContext context)
    {
      this.bits = (ushort) info.GetValue("bits", typeof (ushort));
    }

    public static explicit operator Half(float f)
    {
      return new Half(f);
    }

    public static explicit operator Half(double d)
    {
      return new Half(d);
    }

    public static implicit operator float(Half h)
    {
      return h.ToSingle();
    }

    public static implicit operator double(Half h)
    {
      return (double) h.ToSingle();
    }

    private ushort SingleToHalf(int si32)
    {
      int num1 = si32 >> 16 & 32768;
      int num2 = (si32 >> 23 & (int) byte.MaxValue) - 112;
      int num3 = si32 & 8388607;
      if (num2 <= 0)
      {
        if (num2 < -10)
          return (ushort) num1;
        int num4 = num3 | 8388608;
        int num5 = 14 - num2;
        int num6 = (1 << num5 - 1) - 1;
        int num7 = num4 >> num5 & 1;
        int num8 = num4 + num6 + num7 >> num5;
        return (ushort) (num1 | num8);
      }
      else if (num2 == 143)
      {
        if (num3 == 0)
          return (ushort) (num1 | 31744);
        int num4 = num3 >> 13;
        return (ushort) (num1 | 31744 | num4 | (num4 == 0 ? 1 : 0));
      }
      else
      {
        int num4 = num3 + 4095 + (num3 >> 13 & 1);
        if ((num4 & 8388608) == 1)
        {
          num4 = 0;
          ++num2;
        }
        if (num2 > 30)
          throw new ArithmeticException("Half: Hardware floating-point overflow.");
        else
          return (ushort) (num1 | num2 << 10 | num4 >> 13);
      }
    }

    public unsafe float ToSingle()
    {
      return *(float*) &this.HalfToFloat(this.bits);
    }

    private int HalfToFloat(ushort ui16)
    {
      int num1 = (int) ui16 >> 15 & 1;
      int num2 = (int) ui16 >> 10 & 31;
      int num3 = (int) ui16 & 1023;
      if (num2 == 0)
      {
        if (num3 == 0)
          return num1 << 31;
        while ((num3 & 1024) == 0)
        {
          num3 <<= 1;
          --num2;
        }
        ++num2;
        num3 &= -1025;
      }
      else if (num2 == 31)
      {
        if (num3 == 0)
          return num1 << 31 | 2139095040;
        else
          return num1 << 31 | 2139095040 | num3 << 13;
      }
      int num4 = num2 + 112;
      int num5 = num3 << 13;
      return num1 << 31 | num4 << 23 | num5;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("bits", this.bits);
    }

    public void FromBinaryStream(BinaryReader bin)
    {
      this.bits = bin.ReadUInt16();
    }

    public void ToBinaryStream(BinaryWriter bin)
    {
      bin.Write(this.bits);
    }

    public bool Equals(Half other)
    {
      short num1 = (short) other.bits;
      short num2 = (short) this.bits;
      if ((int) num1 < 0)
        num1 = (short) (32768 - (int) num1);
      if ((int) num2 < 0)
        num2 = (short) (32768 - (int) num2);
      return (int) Math.Abs((short) ((int) num1 - (int) num2)) <= 1;
    }

    public int CompareTo(Half other)
    {
      return (float) this.CompareTo((float) other);
    }

    public override string ToString()
    {
      return this.ToSingle().ToString();
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      return this.ToSingle().ToString(format, formatProvider);
    }

    public static Half Parse(string s)
    {
      return (Half) float.Parse(s);
    }

    public static Half Parse(string s, NumberStyles style, IFormatProvider provider)
    {
      return (Half) float.Parse(s, style, provider);
    }

    public static bool TryParse(string s, out Half result)
    {
      float result1;
      bool flag = float.TryParse(s, out result1);
      result = (Half) result1;
      return flag;
    }

    public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Half result)
    {
      float result1;
      bool flag = float.TryParse(s, style, provider, out result1);
      result = (Half) result1;
      return flag;
    }

    public static byte[] GetBytes(Half h)
    {
      return BitConverter.GetBytes(h.bits);
    }

    public static Half FromBytes(byte[] value, int startIndex)
    {
      Half half;
      half.bits = BitConverter.ToUInt16(value, startIndex);
      return half;
    }
  }
}
