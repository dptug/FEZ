// Type: FezEngine.Effects.HorizontalTrailsEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;

namespace FezEngine.Effects
{
  public class HorizontalTrailsEffect : BaseEffect
  {
    private SemanticMappedSingle timing;
    private SemanticMappedVector3 right;

    public float Timing
    {
      get
      {
        return this.timing.Get();
      }
      set
      {
        this.timing.Set(value);
      }
    }

    public HorizontalTrailsEffect()
      : base("HorizontalTrailsEffect")
    {
      this.timing = new SemanticMappedSingle(this.effect.Parameters, "Timing");
      this.right = new SemanticMappedVector3(this.effect.Parameters, "Right");
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.right.Set(this.CameraProvider.InverseView.Right);
    }
  }
}
