// Type: Microsoft.Xna.Framework.Content.DualTextureEffectReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
