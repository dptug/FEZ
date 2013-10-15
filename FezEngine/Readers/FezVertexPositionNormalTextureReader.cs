// Type: FezEngine.Readers.FezVertexPositionNormalTextureReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Geometry;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class FezVertexPositionNormalTextureReader : ContentTypeReader<FezVertexPositionNormalTexture>
  {
    protected override FezVertexPositionNormalTexture Read(ContentReader input, FezVertexPositionNormalTexture existingInstance)
    {
      return new FezVertexPositionNormalTexture(input.ReadVector3(), input.ReadVector3())
      {
        TextureCoordinate = input.ReadVector2()
      };
    }
  }
}
