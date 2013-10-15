// Type: FezEngine.Readers.VertexPositionNormalTextureInstanceReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Geometry;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class VertexPositionNormalTextureInstanceReader : ContentTypeReader<VertexPositionNormalTextureInstance>
  {
    protected override VertexPositionNormalTextureInstance Read(ContentReader input, VertexPositionNormalTextureInstance existingInstance)
    {
      return new VertexPositionNormalTextureInstance(input.ReadVector3(), input.ReadByte(), input.ReadVector2());
    }
  }
}
