// Type: Microsoft.Xna.Framework.Content.Texture3DReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;
using System;

namespace Microsoft.Xna.Framework.Content
{
  internal class Texture3DReader : ContentTypeReader<Texture3D>
  {
    protected internal override Texture3D Read(ContentReader reader, Texture3D existingInstance)
    {
      SurfaceFormat format = (SurfaceFormat) reader.ReadInt32();
      int num1 = reader.ReadInt32();
      int num2 = reader.ReadInt32();
      int num3 = reader.ReadInt32();
      int num4 = reader.ReadInt32();
      Texture3D texture3D = new Texture3D(reader.GraphicsDevice, num1, num2, num3, num4 > 1, format);
      for (int level = 0; level < num4; ++level)
      {
        int num5 = reader.ReadInt32();
        byte[] data = reader.ReadBytes(num5);
        texture3D.SetData<byte>(level, 0, 0, num1, num2, 0, num3, data, 0, num5);
        num1 = Math.Max(num1 >> 1, 1);
        num2 = Math.Max(num2 >> 1, 1);
        num3 = Math.Max(num3 >> 1, 1);
      }
      return texture3D;
    }
  }
}
