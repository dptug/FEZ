// Type: FezEngine.Readers.IndexedUserPrimitivesReader`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Geometry;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Readers
{
  public class IndexedUserPrimitivesReader<T> : ContentTypeReader<IndexedUserPrimitives<T>> where T : struct, IVertexType
  {
    protected override IndexedUserPrimitives<T> Read(ContentReader input, IndexedUserPrimitives<T> existingInstance)
    {
      PrimitiveType type = input.ReadObject<PrimitiveType>();
      return new IndexedUserPrimitives<T>(input.ReadObject<T[]>(), input.ReadObject<int[]>(), type);
    }
  }
}
