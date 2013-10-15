// Type: Microsoft.Xna.Framework.Content.EnvironmentMapEffectReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  internal class EnvironmentMapEffectReader : ContentTypeReader<EnvironmentMapEffect>
  {
    protected internal override EnvironmentMapEffect Read(ContentReader input, EnvironmentMapEffect existingInstance)
    {
      return new EnvironmentMapEffect(input.GraphicsDevice)
      {
        Texture = input.ReadExternalReference<Texture>() as Texture2D,
        EnvironmentMap = input.ReadExternalReference<TextureCube>(),
        EnvironmentMapAmount = input.ReadSingle(),
        EnvironmentMapSpecular = input.ReadVector3(),
        FresnelFactor = input.ReadSingle(),
        DiffuseColor = input.ReadVector3(),
        EmissiveColor = input.ReadVector3(),
        Alpha = input.ReadSingle()
      };
    }
  }
}
