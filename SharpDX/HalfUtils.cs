// Type: SharpDX.HalfUtils
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Runtime.InteropServices;

namespace SharpDX
{
  internal class HalfUtils
  {
    private static readonly uint[] HalfToFloatMantissaTable = new uint[2048];
    private static readonly uint[] HalfToFloatExponentTable = new uint[64];
    private static readonly uint[] HalfToFloatOffsetTable = new uint[64];
    private static readonly ushort[] FloatToHalfBaseTable = new ushort[512];
    private static readonly byte[] FloatToHalfShiftTable = new byte[512];

    static HalfUtils()
    {
      HalfUtils.HalfToFloatMantissaTable[0] = 0U;
      for (int index = 1; index < 1024; ++index)
      {
        uint num1 = (uint) (index << 13);
        uint num2 = 0U;
        while (((int) num1 & 8388608) == 0)
        {
          num2 -= 8388608U;
          num1 <<= 1;
        }
        uint num3 = num1 & 4286578687U;
        uint num4 = num2 + 947912704U;
        HalfUtils.HalfToFloatMantissaTable[index] = num3 | num4;
      }
      for (int index = 1024; index < 2048; ++index)
        HalfUtils.HalfToFloatMantissaTable[index] = (uint) (939524096 + (index - 1024 << 13));
      HalfUtils.HalfToFloatExponentTable[0] = 0U;
      for (int index = 1; index < 63; ++index)
        HalfUtils.HalfToFloatExponentTable[index] = index >= 31 ? (uint) (int.MinValue + (index - 32 << 23)) : (uint) (index << 23);
      HalfUtils.HalfToFloatExponentTable[31] = 1199570944U;
      HalfUtils.HalfToFloatExponentTable[32] = (uint) int.MinValue;
      HalfUtils.HalfToFloatExponentTable[63] = 3347054592U;
      HalfUtils.HalfToFloatOffsetTable[0] = 0U;
      for (int index = 1; index < 64; ++index)
        HalfUtils.HalfToFloatOffsetTable[index] = 1024U;
      HalfUtils.HalfToFloatOffsetTable[32] = 0U;
      for (int index = 0; index < 256; ++index)
      {
        int num = index - (int) sbyte.MaxValue;
        if (num < -24)
        {
          HalfUtils.FloatToHalfBaseTable[index] = (ushort) 0;
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) short.MinValue;
          HalfUtils.FloatToHalfShiftTable[index] = (byte) 24;
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) 24;
        }
        else if (num < -14)
        {
          HalfUtils.FloatToHalfBaseTable[index] = (ushort) (1024 >> -num - 14);
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) (1024 >> -num - 14 | 32768);
          HalfUtils.FloatToHalfShiftTable[index] = (byte) (-num - 1);
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) (-num - 1);
        }
        else if (num <= 15)
        {
          HalfUtils.FloatToHalfBaseTable[index] = (ushort) (num + 15 << 10);
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) (num + 15 << 10 | 32768);
          HalfUtils.FloatToHalfShiftTable[index] = (byte) 13;
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) 13;
        }
        else if (num < 128)
        {
          HalfUtils.FloatToHalfBaseTable[index] = (ushort) 31744;
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) 64512;
          HalfUtils.FloatToHalfShiftTable[index] = (byte) 24;
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) 24;
        }
        else
        {
          HalfUtils.FloatToHalfBaseTable[index] = (ushort) 31744;
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) 64512;
          HalfUtils.FloatToHalfShiftTable[index] = (byte) 13;
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) 13;
        }
      }
    }

    public static float Unpack(ushort h)
    {
      return new HalfUtils.FloatToUint()
      {
        uintValue = (HalfUtils.HalfToFloatMantissaTable[(IntPtr) (HalfUtils.HalfToFloatOffsetTable[(int) h >> 10] + ((uint) h & 1023U))] + HalfUtils.HalfToFloatExponentTable[(int) h >> 10])
      }.floatValue;
    }

    public static ushort Pack(float f)
    {
      HalfUtils.FloatToUint floatToUint = new HalfUtils.FloatToUint();
      floatToUint.floatValue = f;
      return (ushort) ((uint) HalfUtils.FloatToHalfBaseTable[(IntPtr) (floatToUint.uintValue >> 23 & 511U)] + ((floatToUint.uintValue & 8388607U) >> (int) HalfUtils.FloatToHalfShiftTable[(IntPtr) (floatToUint.uintValue >> 23 & 511U)]));
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct FloatToUint
    {
      [FieldOffset(0)]
      public uint uintValue;
      [FieldOffset(0)]
      public float floatValue;
    }
  }
}
