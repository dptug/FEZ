// Type: FezEngine.Effects.ShimmeringPointsEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class ShimmeringPointsEffect : BaseEffect
  {
    private readonly SemanticMappedVector3 randomSeed;
    private readonly SemanticMappedSingle saturation;

    public float Saturation
    {
      set
      {
        this.saturation.Set(value);
      }
    }

    public ShimmeringPointsEffect()
      : base("ShimmeringPointsEffect")
    {
      this.randomSeed = new SemanticMappedVector3(this.effect.Parameters, "RandomSeed");
      this.saturation = new SemanticMappedSingle(this.effect.Parameters, "Saturation");
      this.saturation.Set(1f);
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.randomSeed.Set(new Vector3(RandomHelper.Unit(), RandomHelper.Unit(), RandomHelper.Unit()));
      this.material.Diffuse = mesh.Material.Diffuse;
      this.material.Opacity = mesh.Material.Opacity;
    }
  }
}
