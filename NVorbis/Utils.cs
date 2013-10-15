// Type: NVorbis.Utils
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NVorbis
{
  internal static class Utils
  {
    internal static int ilog(int x)
    {
      int num = 0;
      while (x > 0)
      {
        ++num;
        x >>= 1;
      }
      return num;
    }

    internal static uint BitReverse(uint n)
    {
      return Utils.BitReverse(n, 32);
    }

    internal static uint BitReverse(uint n, int bits)
    {
      n = (n & 2863311530U) >> 1 | (uint) (((int) n & 1431655765) << 1);
      n = (n & 3435973836U) >> 2 | (uint) (((int) n & 858993459) << 2);
      n = (n & 4042322160U) >> 4 | (uint) (((int) n & 252645135) << 4);
      n = (n & 4278255360U) >> 8 | (uint) (((int) n & 16711935) << 8);
      return (n >> 16 | n << 16) >> 32 - bits;
    }

    internal static float ClipValue(float value, ref bool clipped)
    {
      Utils.FloatBits floatBits;
      floatBits.Bits = 0U;
      floatBits.Float = value;
      if ((floatBits.Bits & (uint) int.MaxValue) > 1065353216U)
      {
        clipped = true;
        floatBits.Bits = (uint) (1065353216 | (int) floatBits.Bits & int.MinValue);
      }
      return floatBits.Float;
    }

    internal static float ConvertFromVorbisFloat32(uint bits)
    {
      int num = (int) bits >> 31;
      double y = (double) ((int) ((bits & 2145386496U) >> 21) - 788);
      return (float) (((long) (bits & 2097151U) ^ (long) num) + (long) (num & 1)) * (float) Math.Pow(2.0, y);
    }

    internal static int Sum(Queue<int> queue)
    {
      int num1 = 0;
      for (int index = 0; index < queue.Count; ++index)
      {
        int num2 = queue.Dequeue();
        num1 += num2;
        queue.Enqueue(num2);
      }
      return num1;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct FloatBits
    {
      [FieldOffset(0)]
      public float Float;
      [FieldOffset(0)]
      public uint Bits;
    }
  }
}
