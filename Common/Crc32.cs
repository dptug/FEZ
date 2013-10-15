// Type: Common.Crc32
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9992B00D-7E50-4755-8BAA-4E3BBC8F3470
// Assembly location: F:\Program Files (x86)\FEZ\Common.dll

using System;

namespace Common
{
  public class Crc32
  {
    private static readonly uint[] table = new uint[256];

    static Crc32()
    {
      for (uint index1 = 0U; (long) index1 < (long) Crc32.table.Length; ++index1)
      {
        uint num = index1;
        for (int index2 = 8; index2 > 0; --index2)
        {
          if (((int) num & 1) == 1)
            num = num >> 1 ^ 3988292384U;
          else
            num >>= 1;
        }
        Crc32.table[(IntPtr) index1] = num;
      }
    }

    public static uint ComputeChecksum(byte[] bytes)
    {
      uint num1 = uint.MaxValue;
      for (int index = 0; index < bytes.Length; ++index)
      {
        byte num2 = (byte) (num1 & (uint) byte.MaxValue ^ (uint) bytes[index]);
        num1 = num1 >> 8 ^ Crc32.table[(int) num2];
      }
      return ~num1;
    }
  }
}
