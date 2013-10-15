// Type: Microsoft.Xna.Framework.Content.TextureReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  public class TextureReader : ContentTypeReader<Texture>
  {
    protected internal override Texture Read(ContentReader reader, Texture existingInstance)
    {
      return existingInstance;
    }
  }
}
