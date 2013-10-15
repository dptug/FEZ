// Type: FezEngine.Readers.ArtObjectReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Readers
{
  public class ArtObjectReader : ContentTypeReader<ArtObject>
  {
    protected override ArtObject Read(ContentReader input, ArtObject existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new ArtObject();
      existingInstance.Name = input.ReadString();
      existingInstance.Cubemap = input.ReadObject<Texture2D>();
      existingInstance.Size = input.ReadVector3();
      existingInstance.Geometry = input.ReadObject<ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix>>(existingInstance.Geometry);
      existingInstance.ActorType = input.ReadObject<ActorType>();
      existingInstance.NoSihouette = input.ReadBoolean();
      return existingInstance;
    }
  }
}
