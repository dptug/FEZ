// Type: Microsoft.Xna.Framework.Content.Texture2DReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
      int width = reader.ReadInt32();
      int height = reader.ReadInt32();
      int num1 = reader.ReadInt32();
      SurfaceFormat format = surfaceFormat;
      if (surfaceFormat == SurfaceFormat.NormalizedByte4)
        format = SurfaceFormat.Color;
      Texture2D texture2D = existingInstance != null ? existingInstance : new Texture2D(reader.GraphicsDevice, width, height, num1 > 1, format);
      for (int level = 0; level < num1; ++level)
      {
        int count = reader.ReadInt32();
        byte[] data = reader.ReadBytes(count);
        int num2 = width >> level;
        int num3 = height >> level;
        switch (surfaceFormat)
        {
          case SurfaceFormat.Bgra4444:
            int startIndex = 0;
            for (int index1 = 0; index1 < num3; ++index1)
            {
              for (int index2 = 0; index2 < num2; ++index2)
              {
                ushort num4 = BitConverter.ToUInt16(data, startIndex);
                ushort num5 = (ushort) (((int) num4 & 4095) << 4 | ((int) num4 & 61440) >> 12);
                data[startIndex] = (byte) num5;
                data[startIndex + 1] = (byte) ((uint) num5 >> 8);
                startIndex += 2;
              }
            }
            break;
          case SurfaceFormat.NormalizedByte4:
            int num6 = GraphicsExtensions.Size(surfaceFormat);
            int num7 = num2 * num6;
            for (int index1 = 0; index1 < num3; ++index1)
            {
              for (int index2 = 0; index2 < num2; ++index2)
              {
                int num4 = BitConverter.ToInt32(data, index1 * num7 + index2 * num6);
                data[index1 * num7 + index2 * 4] = (byte) (num4 >> 16 & (int) byte.MaxValue);
                data[index1 * num7 + index2 * 4 + 1] = (byte) (num4 >> 8 & (int) byte.MaxValue);
                data[index1 * num7 + index2 * 4 + 2] = (byte) (num4 & (int) byte.MaxValue);
                data[index1 * num7 + index2 * 4 + 3] = (byte) (num4 >> 24 & (int) byte.MaxValue);
              }
            }
            break;
        }
        texture2D.SetData<byte>(level, new Rectangle?(), data, 0, data.Length);
      }
      return texture2D;
    }
  }
}
