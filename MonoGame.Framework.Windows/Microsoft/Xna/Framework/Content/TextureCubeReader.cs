// Type: Microsoft.Xna.Framework.Content.TextureCubeReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
