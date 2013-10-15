// Type: Microsoft.Xna.Framework.Content.TextureCubeReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  internal class TextureCubeReader : ContentTypeReader<TextureCube>
  {
    protected internal override TextureCube Read(ContentReader reader, TextureCube existingInstance)
    {
      SurfaceFormat format = (SurfaceFormat) reader.ReadInt32();
      int size = reader.ReadInt32();
      int num1 = reader.ReadInt32();
      TextureCube textureCube = new TextureCube(reader.GraphicsDevice, size, num1 > 1, format);
      for (int index = 0; index < 6; ++index)
      {
        for (int level = 0; level < num1; ++level)
        {
          int num2 = reader.ReadInt32();
          byte[] data = reader.ReadBytes(num2);
          textureCube.SetData<byte>((CubeMapFace) index, level, new Rectangle?(), data, 0, num2);
        }
      }
      return textureCube;
    }
  }
}
