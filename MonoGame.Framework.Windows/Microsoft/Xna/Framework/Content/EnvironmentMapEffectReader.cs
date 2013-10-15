// Type: Microsoft.Xna.Framework.Content.EnvironmentMapEffectReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
