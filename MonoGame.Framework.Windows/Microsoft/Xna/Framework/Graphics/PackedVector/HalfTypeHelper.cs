// Type: Microsoft.Xna.Framework.Graphics.PackedVector.HalfTypeHelper
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
  internal class HalfTypeHelper
  {
    internal static ushort convert(float f)
    {
      return HalfTypeHelper.convert(new HalfTypeHelper.uif()
      {
        f = f
      }.i);
    }

    internal static ushort convert(int i)
    {
      int num1 = i >> 16 & 32768;
      int num2 = (i >> 23 & (int) byte.MaxValue) - 112;
      int num3 = i & 8388607;
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
        if ((num4 & 8388608) != 0)
        {
          num4 = 0;
          ++num2;
        }
        if (num2 > 30)
          return (ushort) (num1 | 31744);
        else
          return (ushort) (num1 | num2 << 10 | num4 >> 13);
      }
    }

    internal static unsafe float convert(ushort value)
    {
      uint num1 = (uint) value & 1023U;
      uint num2 = 4294967282U;
      uint num3;
      if (((int) value & -33792) == 0)
      {
        if ((int) num1 != 0)
        {
          while (((int) num1 & 1024) == 0)
          {
            --num2;
            num1 <<= 1;
          }
          uint num4 = num1 & 4294966271U;
          num3 = (uint) ((ulong) (((int) value & 32768) << 16) | (ulong) (uint) ((int) num2 + (int) sbyte.MaxValue << 23)) | num4 << 13;
        }
        else
          num3 = (uint) (((int) value & 32768) << 16);
      }
      else
        num3 = (uint) ((ulong) (((int) value & 32768) << 16 | ((int) value >> 10 & 31) - 15 + (int) sbyte.MaxValue << 23) | (ulong) (num1 << 13));
      return *(float*) &num3;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct uif
    {
      [FieldOffset(0)]
      public float f;
      [FieldOffset(0)]
      public int i;
    }
  }
}
