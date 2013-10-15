// Type: FezEngine.Effects.SplitCollectorEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;

namespace FezEngine.Effects
{
  public class SplitCollectorEffect : BaseEffect
  {
    private readonly SemanticMappedSingle varyingOpacity;
    private readonly SemanticMappedSingle offset;

    public float VaryingOpacity
    {
      set
      {
        this.varyingOpacity.Set(value);
      }
    }

    public float Offset
    {
      set
      {
        this.offset.Set(value);
      }
    }

    public SplitCollectorEffect()
      : base("SplitCollectorEffect")
    {
      this.varyingOpacity = new SemanticMappedSingle(this.effect.Parameters, "VaryingOpacity");
      this.offset = new SemanticMappedSingle(this.effect.Parameters, "Offset");
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.matrices.World = mesh.WorldMatrix;
      this.material.Diffuse = mesh.Material.Diffuse;
    }
  }
}
