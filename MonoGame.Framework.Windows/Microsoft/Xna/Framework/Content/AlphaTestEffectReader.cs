// Type: Microsoft.Xna.Framework.Content.AlphaTestEffectReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  internal class AlphaTestEffectReader : ContentTypeReader<AlphaTestEffect>
  {
    protected internal override AlphaTestEffect Read(ContentReader input, AlphaTestEffect existingInstance)
    {
      return new AlphaTestEffect(input.GraphicsDevice)
      {
        Texture = input.ReadExternalReference<Texture>() as Texture2D,
        AlphaFunction = (CompareFunction) input.ReadInt32(),
        ReferenceAlpha = (int) input.ReadUInt32(),
        DiffuseColor = input.ReadVector3(),
        Alpha = input.ReadSingle(),
        VertexColorEnabled = input.ReadBoolean()
      };
    }
  }
}
