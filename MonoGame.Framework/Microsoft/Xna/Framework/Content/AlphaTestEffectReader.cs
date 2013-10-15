// Type: Microsoft.Xna.Framework.Content.AlphaTestEffectReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
