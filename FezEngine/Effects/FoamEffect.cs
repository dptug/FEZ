// Type: FezEngine.Effects.FoamEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;

namespace FezEngine.Effects
{
  public class FoamEffect : BaseEffect
  {
    private readonly SemanticMappedSingle timeAccumulator;
    private readonly SemanticMappedSingle shoreTotalWidth;
    private readonly SemanticMappedSingle screenCenterSide;
    private readonly SemanticMappedBoolean isEmerged;
    private readonly SemanticMappedBoolean isWobbling;

    public float TimeAccumulator
    {
      set
      {
        this.timeAccumulator.Set(value);
      }
    }

    public float ShoreTotalWidth
    {
      set
      {
        this.shoreTotalWidth.Set(value);
      }
    }

    public float ScreenCenterSide
    {
      set
      {
        this.screenCenterSide.Set(value);
      }
    }

    public bool IsWobbling
    {
      set
      {
        this.isWobbling.Set(value);
      }
    }

    public FoamEffect()
      : base("FoamEffect")
    {
      this.timeAccumulator = new SemanticMappedSingle(this.effect.Parameters, "TimeAccumulator");
      this.shoreTotalWidth = new SemanticMappedSingle(this.effect.Parameters, "ShoreTotalWidth");
      this.screenCenterSide = new SemanticMappedSingle(this.effect.Parameters, "ScreenCenterSide");
      this.isEmerged = new SemanticMappedBoolean(this.effect.Parameters, "IsEmerged");
      this.isWobbling = new SemanticMappedBoolean(this.effect.Parameters, "IsWobbling");
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      this.isEmerged.Set((bool) group.CustomData);
      this.material.Diffuse = group.Material.Diffuse;
      this.material.Opacity = group.Material.Opacity;
    }
  }
}
