// Type: Microsoft.Xna.Framework.Content.SkinnedEffectReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
