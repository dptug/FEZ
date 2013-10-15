// Type: Microsoft.Xna.Framework.Content.SkinnedEffectReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  internal class SkinnedEffectReader : ContentTypeReader<SkinnedEffect>
  {
    protected internal override SkinnedEffect Read(ContentReader input, SkinnedEffect existingInstance)
    {
      return new SkinnedEffect(input.GraphicsDevice)
      {
        Texture = input.ReadExternalReference<Texture>() as Texture2D,
        WeightsPerVertex = input.ReadInt32(),
        DiffuseColor = input.ReadVector3(),
        EmissiveColor = input.ReadVector3(),
        SpecularColor = input.ReadVector3(),
        SpecularPower = input.ReadSingle(),
        Alpha = input.ReadSingle()
      };
    }
  }
}
