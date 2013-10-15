// Type: FezEngine.Effects.VignetteEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;

namespace FezEngine.Effects
{
  public class VignetteEffect : BaseEffect
  {
    private readonly SemanticMappedSingle sinceStarted;

    public float SinceStarted
    {
      get
      {
        return this.sinceStarted.Get();
      }
      set
      {
        this.sinceStarted.Set(value);
      }
    }

    public VignetteEffect()
      : base("VignetteEffect")
    {
      this.sinceStarted = new SemanticMappedSingle(this.effect.Parameters, "SinceStarted");
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.material.Opacity = mesh.Material.Opacity;
    }
  }
}
