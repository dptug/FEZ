// Type: Microsoft.Xna.Framework.Content.Texture2DReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Microsoft.Xna.Framework.Content
{
  internal class Texture2DReader : ContentTypeReader<Texture2D>
  {
    private static string[] supportedExtensions = new string[7]
    {
      ".jpg",
      ".bmp",
      ".jpeg",
      ".png",
      ".gif",
      ".pict",
      ".tga"
    };

    static Texture2DReader()
    {
    }

    internal Texture2DReader()
    {
    }

    internal static string Normalize(string fileName)
    {
      return ContentTypeReader.Normalize(fileName, Texture2DReader.supportedExtensions);
    }

    protected internal override Texture2D Read(ContentReader reader, Texture2D existingInstance)
    {
      SurfaceFormat surfaceFormat;
      if (reader.version < 5)
      {
        switch ((SurfaceFormat_Legacy) reader.ReadInt32())
        {
          case SurfaceFormat_Legacy.Color:
            surfaceFormat = SurfaceFormat.Color;
            break;
          case SurfaceFormat_Legacy.Dxt1:
            surfaceFormat = SurfaceFormat.Dxt1;
            break;
          case SurfaceFormat_Legacy.Dxt3:
            surfaceFormat = SurfaceFormat.Dxt3;
            break;
          case SurfaceFormat_Legacy.Dxt5:
            surfaceFormat = SurfaceFormat.Dxt5;
            break;
          default:
            throw new NotImplementedException();
        }
      }
      else
        surfaceFormat = (SurfaceFormat) reader.ReadInt32();
      int width1 = reader.ReadInt32();
      int height1 = reader.ReadInt32();
      int num1 = reader.ReadInt32();
      int num2 = num1;
      if (num1 > 1 && !GraphicsCapabilities.NonPowerOfTwo && (!MathHelper.IsPowerOfTwo(width1) || !MathHelper.IsPowerOfTwo(height1)))
        num2 = 1;
      SurfaceFormat format = surfaceFormat;
      if (surfaceFormat == SurfaceFormat.NormalizedByte4)
        format = SurfaceFormat.Color;
      if (!GraphicsExtensions.UseDxtCompression)
      {
        switch (surfaceFormat)
        {
          case SurfaceFormat.Dxt1:
          case SurfaceFormat.Dxt3:
          case SurfaceFormat.Dxt5:
            format = SurfaceFormat.Color;
            break;
        }
      }
      Texture2D texture2D = existingInstance != null ? existingInstance : new Texture2D(reader.GraphicsDevice, width1, height1, num2 > 1, format);
      for (int level = 0; level < num1; ++level)
      {
        int count = reader.ReadInt32();
        byte[] numArray = reader.ReadBytes(count);
        int width2 = width1 >> level;
        int height2 = height1 >> level;
        if (level < num2)
        {
          if (!GraphicsExtensions.UseDxtCompression)
          {
            switch (surfaceFormat)
            {
              case SurfaceFormat.Dxt1:
                numArray = DxtUtil.DecompressDxt1(numArray, width2, height2);
                break;
              case SurfaceFormat.Dxt3:
                numArray = DxtUtil.DecompressDxt3(numArray, width2, height2);
                break;
              case SurfaceFormat.Dxt5:
                numArray = DxtUtil.DecompressDxt5(numArray, width2, height2);
                break;
            }
          }
          switch (surfaceFormat)
          {
            case SurfaceFormat.Bgra5551:
              int startIndex1 = 0;
              for (int index1 = 0; index1 < height2; ++index1)
              {
                for (int index2 = 0; index2 < width2; ++index2)
                {
                  ushort num3 = BitConverter.ToUInt16(numArray, startIndex1);
                  ushort num4 = (ushort) (((int) num3 & (int) short.MaxValue) << 1 | ((int) num3 & 32768) >> 15);
                  numArray[startIndex1] = (byte) num4;
                  numArray[startIndex1 + 1] = (byte) ((uint) num4 >> 8);
                  startIndex1 += 2;
                }
              }
              break;
            case SurfaceFormat.Bgra4444:
              int startIndex2 = 0;
              for (int index1 = 0; index1 < height2; ++index1)
              {
                for (int index2 = 0; index2 < width2; ++index2)
                {
                  ushort num3 = BitConverter.ToUInt16(numArray, startIndex2);
                  ushort num4 = (ushort) (((int) num3 & 4095) << 4 | ((int) num3 & 61440) >> 12);
                  numArray[startIndex2] = (byte) num4;
                  numArray[startIndex2 + 1] = (byte) ((uint) num4 >> 8);
                  startIndex2 += 2;
                }
              }
              break;
            case SurfaceFormat.NormalizedByte4:
              int num5 = GraphicsExtensions.Size(surfaceFormat);
              int num6 = width2 * num5;
              for (int index1 = 0; index1 < height2; ++index1)
              {
                for (int index2 = 0; index2 < width2; ++index2)
                {
                  int num3 = BitConverter.ToInt32(numArray, index1 * num6 + index2 * num5);
                  numArray[index1 * num6 + index2 * 4] = (byte) (num3 >> 16 & (int) byte.MaxValue);
                  numArray[index1 * num6 + index2 * 4 + 1] = (byte) (num3 >> 8 & (int) byte.MaxValue);
                  numArray[index1 * num6 + index2 * 4 + 2] = (byte) (num3 & (int) byte.MaxValue);
                  numArray[index1 * num6 + index2 * 4 + 3] = (byte) (num3 >> 24 & (int) byte.MaxValue);
                }
              }
              break;
          }
          texture2D.SetData<byte>(level, new Rectangle?(), numArray, 0, numArray.Length);
        }
      }
      return texture2D;
    }
  }
}
