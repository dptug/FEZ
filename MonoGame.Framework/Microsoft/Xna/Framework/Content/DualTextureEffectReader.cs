// Type: Microsoft.Xna.Framework.Content.DualTextureEffectReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  internal class DualTextureEffectReader : ContentTypeReader<DualTextureEffect>
  {
    protected internal override DualTextureEffect Read(ContentReader input, DualTextureEffect existingInstance)
    {
      return new DualTextureEffect(input.GraphicsDevice)
      {
        Texture = input.ReadExternalReference<Texture>() as Texture2D,
        Texture2 = input.ReadExternalReference<Texture>() as Texture2D,
        DiffuseColor = input.ReadVector3(),
        Alpha = input.ReadSingle(),
        VertexColorEnabled = input.ReadBoolean()
      };
    }
  }
}
