// Type: Microsoft.Xna.Framework.Graphics.DxtUtil
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.IO;

namespace Microsoft.Xna.Framework.Graphics
{
  internal static class DxtUtil
  {
    internal static byte[] DecompressDxt1(byte[] imageData, int width, int height)
    {
      using (MemoryStream memoryStream = new MemoryStream(imageData))
        return DxtUtil.DecompressDxt1((Stream) memoryStream, width, height);
    }

    internal static byte[] DecompressDxt1(Stream imageStream, int width, int height)
    {
      byte[] imageData = new byte[width * height * 4];
      using (BinaryReader imageReader = new BinaryReader(imageStream))
      {
        int blockCountX = (width + 3) / 4;
        int num = (height + 3) / 4;
        for (int y = 0; y < num; ++y)
        {
          for (int x = 0; x < blockCountX; ++x)
            DxtUtil.DecompressDxt1Block(imageReader, x, y, blockCountX, width, height, imageData);
        }
      }
      return imageData;
    }

    private static void DecompressDxt1Block(BinaryReader imageReader, int x, int y, int blockCountX, int width, int height, byte[] imageData)
    {
      ushort color1 = imageReader.ReadUInt16();
      ushort color2 = imageReader.ReadUInt16();
      byte r1;
      byte g1;
      byte b1;
      DxtUtil.ConvertRgb565ToRgb888(color1, out r1, out g1, out b1);
      byte r2;
      byte g2;
      byte b2;
      DxtUtil.ConvertRgb565ToRgb888(color2, out r2, out g2, out b2);
      uint num1 = imageReader.ReadUInt32();
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          byte num2 = (byte) 0;
          byte num3 = (byte) 0;
          byte num4 = (byte) 0;
          byte num5 = byte.MaxValue;
          uint num6 = num1 >> 2 * (4 * index1 + index2) & 3U;
          if ((int) color1 > (int) color2)
          {
            switch (num6)
            {
              case 0U:
                num2 = r1;
                num3 = g1;
                num4 = b1;
                break;
              case 1U:
                num2 = r2;
                num3 = g2;
                num4 = b2;
                break;
              case 2U:
                num2 = (byte) ((2 * (int) r1 + (int) r2) / 3);
                num3 = (byte) ((2 * (int) g1 + (int) g2) / 3);
                num4 = (byte) ((2 * (int) b1 + (int) b2) / 3);
                break;
              case 3U:
                num2 = (byte) (((int) r1 + 2 * (int) r2) / 3);
                num3 = (byte) (((int) g1 + 2 * (int) g2) / 3);
                num4 = (byte) (((int) b1 + 2 * (int) b2) / 3);
                break;
            }
          }
          else
          {
            switch (num6)
            {
              case 0U:
                num2 = r1;
                num3 = g1;
                num4 = b1;
                break;
              case 1U:
                num2 = r2;
                num3 = g2;
                num4 = b2;
                break;
              case 2U:
                num2 = (byte) (((int) r1 + (int) r2) / 2);
                num3 = (byte) (((int) g1 + (int) g2) / 2);
                num4 = (byte) (((int) b1 + (int) b2) / 2);
                break;
              case 3U:
                num2 = (byte) 0;
                num3 = (byte) 0;
                num4 = (byte) 0;
                num5 = (byte) 0;
                break;
            }
          }
          int num7 = (x << 2) + index2;
          int num8 = (y << 2) + index1;
          if (num7 < width && num8 < height)
          {
            int index3 = num8 * width + num7 << 2;
            imageData[index3] = num2;
            imageData[index3 + 1] = num3;
            imageData[index3 + 2] = num4;
            imageData[index3 + 3] = num5;
          }
        }
      }
    }

    internal static byte[] DecompressDxt3(byte[] imageData, int width, int height)
    {
      using (MemoryStream memoryStream = new MemoryStream(imageData))
        return DxtUtil.DecompressDxt3((Stream) memoryStream, width, height);
    }

    internal static byte[] DecompressDxt3(Stream imageStream, int width, int height)
    {
      byte[] imageData = new byte[width * height * 4];
      using (BinaryReader imageReader = new BinaryReader(imageStream))
      {
        int blockCountX = (width + 3) / 4;
        int num = (height + 3) / 4;
        for (int y = 0; y < num; ++y)
        {
          for (int x = 0; x < blockCountX; ++x)
            DxtUtil.DecompressDxt3Block(imageReader, x, y, blockCountX, width, height, imageData);
        }
      }
      return imageData;
    }

    private static void DecompressDxt3Block(BinaryReader imageReader, int x, int y, int blockCountX, int width, int height, byte[] imageData)
    {
      byte num1 = imageReader.ReadByte();
      byte num2 = imageReader.ReadByte();
      byte num3 = imageReader.ReadByte();
      byte num4 = imageReader.ReadByte();
      byte num5 = imageReader.ReadByte();
      byte num6 = imageReader.ReadByte();
      byte num7 = imageReader.ReadByte();
      byte num8 = imageReader.ReadByte();
      ushort color1 = imageReader.ReadUInt16();
      ushort color2 = imageReader.ReadUInt16();
      byte r1;
      byte g1;
      byte b1;
      DxtUtil.ConvertRgb565ToRgb888(color1, out r1, out g1, out b1);
      byte r2;
      byte g2;
      byte b2;
      DxtUtil.ConvertRgb565ToRgb888(color2, out r2, out g2, out b2);
      uint num9 = imageReader.ReadUInt32();
      int num10 = 0;
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          byte num11 = (byte) 0;
          byte num12 = (byte) 0;
          byte num13 = (byte) 0;
          byte num14 = (byte) 0;
          uint num15 = num9 >> 2 * (4 * index1 + index2) & 3U;
          switch (num10)
          {
            case 0:
              num14 = (byte) ((int) num1 & 15 | ((int) num1 & 15) << 4);
              break;
            case 1:
              num14 = (byte) ((int) num1 & 240 | ((int) num1 & 240) >> 4);
              break;
            case 2:
              num14 = (byte) ((int) num2 & 15 | ((int) num2 & 15) << 4);
              break;
            case 3:
              num14 = (byte) ((int) num2 & 240 | ((int) num2 & 240) >> 4);
              break;
            case 4:
              num14 = (byte) ((int) num3 & 15 | ((int) num3 & 15) << 4);
              break;
            case 5:
              num14 = (byte) ((int) num3 & 240 | ((int) num3 & 240) >> 4);
              break;
            case 6:
              num14 = (byte) ((int) num4 & 15 | ((int) num4 & 15) << 4);
              break;
            case 7:
              num14 = (byte) ((int) num4 & 240 | ((int) num4 & 240) >> 4);
              break;
            case 8:
              num14 = (byte) ((int) num5 & 15 | ((int) num5 & 15) << 4);
              break;
            case 9:
              num14 = (byte) ((int) num5 & 240 | ((int) num5 & 240) >> 4);
              break;
            case 10:
              num14 = (byte) ((int) num6 & 15 | ((int) num6 & 15) << 4);
              break;
            case 11:
              num14 = (byte) ((int) num6 & 240 | ((int) num6 & 240) >> 4);
              break;
            case 12:
              num14 = (byte) ((int) num7 & 15 | ((int) num7 & 15) << 4);
              break;
            case 13:
              num14 = (byte) ((int) num7 & 240 | ((int) num7 & 240) >> 4);
              break;
            case 14:
              num14 = (byte) ((int) num8 & 15 | ((int) num8 & 15) << 4);
              break;
            case 15:
              num14 = (byte) ((int) num8 & 240 | ((int) num8 & 240) >> 4);
              break;
          }
          ++num10;
          switch (num15)
          {
            case 0U:
              num11 = r1;
              num12 = g1;
              num13 = b1;
              break;
            case 1U:
              num11 = r2;
              num12 = g2;
              num13 = b2;
              break;
            case 2U:
              num11 = (byte) ((2 * (int) r1 + (int) r2) / 3);
              num12 = (byte) ((2 * (int) g1 + (int) g2) / 3);
              num13 = (byte) ((2 * (int) b1 + (int) b2) / 3);
              break;
            case 3U:
              num11 = (byte) (((int) r1 + 2 * (int) r2) / 3);
              num12 = (byte) (((int) g1 + 2 * (int) g2) / 3);
              num13 = (byte) (((int) b1 + 2 * (int) b2) / 3);
              break;
          }
          int num16 = (x << 2) + index2;
          int num17 = (y << 2) + index1;
          if (num16 < width && num17 < height)
          {
            int index3 = num17 * width + num16 << 2;
            imageData[index3] = num11;
            imageData[index3 + 1] = num12;
            imageData[index3 + 2] = num13;
            imageData[index3 + 3] = num14;
          }
        }
      }
    }

    internal static byte[] DecompressDxt5(byte[] imageData, int width, int height)
    {
      using (MemoryStream memoryStream = new MemoryStream(imageData))
        return DxtUtil.DecompressDxt5((Stream) memoryStream, width, height);
    }

    internal static byte[] DecompressDxt5(Stream imageStream, int width, int height)
    {
      byte[] imageData = new byte[width * height * 4];
      using (BinaryReader imageReader = new BinaryReader(imageStream))
      {
        int blockCountX = (width + 3) / 4;
        int num = (height + 3) / 4;
        for (int y = 0; y < num; ++y)
        {
          for (int x = 0; x < blockCountX; ++x)
            DxtUtil.DecompressDxt5Block(imageReader, x, y, blockCountX, width, height, imageData);
        }
      }
      return imageData;
    }

    private static void DecompressDxt5Block(BinaryReader imageReader, int x, int y, int blockCountX, int width, int height, byte[] imageData)
    {
      byte num1 = imageReader.ReadByte();
      byte num2 = imageReader.ReadByte();
      ulong num3 = ((ulong) imageReader.ReadUInt32() << 16) + (ulong) imageReader.ReadUInt16();
      ushort color1 = imageReader.ReadUInt16();
      ushort color2 = imageReader.ReadUInt16();
      byte r1;
      byte g1;
      byte b1;
      DxtUtil.ConvertRgb565ToRgb888(color1, out r1, out g1, out b1);
      byte r2;
      byte g2;
      byte b2;
      DxtUtil.ConvertRgb565ToRgb888(color2, out r2, out g2, out b2);
      uint num4 = imageReader.ReadUInt32();
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          byte num5 = (byte) 0;
          byte num6 = (byte) 0;
          byte num7 = (byte) 0;
          uint num8 = num4 >> 2 * (4 * index1 + index2) & 3U;
          uint num9 = (uint) (num3 >> 3 * (4 * index1 + index2) & 7UL);
          byte num10;
          switch (num9)
          {
            case 0U:
              num10 = num1;
              break;
            case 1U:
              num10 = num2;
              break;
            default:
              num10 = (int) num1 <= (int) num2 ? ((int) num9 != 6 ? ((int) num9 != 7 ? (byte) ((uint) ((6 - (int) num9) * (int) num1 + ((int) num9 - 1) * (int) num2) / 5U) : byte.MaxValue) : (byte) 0) : (byte) ((uint) ((8 - (int) num9) * (int) num1 + ((int) num9 - 1) * (int) num2) / 7U);
              break;
          }
          switch (num8)
          {
            case 0U:
              num5 = r1;
              num6 = g1;
              num7 = b1;
              break;
            case 1U:
              num5 = r2;
              num6 = g2;
              num7 = b2;
              break;
            case 2U:
              num5 = (byte) ((2 * (int) r1 + (int) r2) / 3);
              num6 = (byte) ((2 * (int) g1 + (int) g2) / 3);
              num7 = (byte) ((2 * (int) b1 + (int) b2) / 3);
              break;
            case 3U:
              num5 = (byte) (((int) r1 + 2 * (int) r2) / 3);
              num6 = (byte) (((int) g1 + 2 * (int) g2) / 3);
              num7 = (byte) (((int) b1 + 2 * (int) b2) / 3);
              break;
          }
          int num11 = (x << 2) + index2;
          int num12 = (y << 2) + index1;
          if (num11 < width && num12 < height)
          {
            int index3 = num12 * width + num11 << 2;
            imageData[index3] = num5;
            imageData[index3 + 1] = num6;
            imageData[index3 + 2] = num7;
            imageData[index3 + 3] = num10;
          }
        }
      }
    }

    private static void ConvertRgb565ToRgb888(ushort color, out byte r, out byte g, out byte b)
    {
      int num1 = ((int) color >> 11) * (int) byte.MaxValue + 16;
      r = (byte) ((num1 / 32 + num1) / 32);
      int num2 = (((int) color & 2016) >> 5) * (int) byte.MaxValue + 32;
      g = (byte) ((num2 / 64 + num2) / 64);
      int num3 = ((int) color & 31) * (int) byte.MaxValue + 16;
      b = (byte) ((num3 / 32 + num3) / 32);
    }
  }
}
