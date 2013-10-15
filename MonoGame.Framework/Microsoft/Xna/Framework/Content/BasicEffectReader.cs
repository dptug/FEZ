// Type: Microsoft.Xna.Framework.Content.BasicEffectReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
  internal class BasicEffectReader : ContentTypeReader<BasicEffect>
  {
    protected internal override BasicEffect Read(ContentReader input, BasicEffect existingInstance)
    {
      BasicEffect basicEffect = new BasicEffect(input.GraphicsDevice);
      Texture2D texture2D = input.ReadExternalReference<Texture>() as Texture2D;
      if (texture2D != null)
      {
        basicEffect.Texture = texture2D;
        basicEffect.TextureEnabled = true;
      }
      basicEffect.DiffuseColor = input.ReadVector3();
      basicEffect.EmissiveColor = input.ReadVector3();
      basicEffect.SpecularColor = input.ReadVector3();
      basicEffect.SpecularPower = input.ReadSingle();
      basicEffect.Alpha = input.ReadSingle();
      basicEffect.VertexColorEnabled = input.ReadBoolean();
      return basicEffect;
    }
  }
}
