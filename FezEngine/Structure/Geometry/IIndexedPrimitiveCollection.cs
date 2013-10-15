// Type: FezEngine.Structure.Geometry.IIndexedPrimitiveCollection
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects;

namespace FezEngine.Structure.Geometry
{
  public interface IIndexedPrimitiveCollection
  {
    bool Empty { get; }

    int VertexCount { get; }

    void Draw(BaseEffect effect);

    IIndexedPrimitiveCollection Clone();
  }
}
