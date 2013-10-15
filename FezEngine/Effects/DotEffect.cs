// Type: FezEngine.Effects.DotEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class DotEffect : BaseEffect
  {
    private readonly SemanticMappedSingle HueOffset;
    private float hueOffset;
    private Vector3 lastDiffuse;

    public float ShiftSpeed { get; set; }

    public float AdditionalOffset { get; set; }

    public DotEffect()
      : base("DotEffect")
    {
      this.HueOffset = new SemanticMappedSingle(this.effect.Parameters, "HueOffset");
      this.ShiftSpeed = 1f;
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.hueOffset += 0.0003472222f * this.ShiftSpeed;
      float num = this.hueOffset + this.AdditionalOffset * 360f;
      while ((double) num >= 360.0)
        num -= 360f;
      while ((double) num < 0.0)
        num += 360f;
      this.HueOffset.Set(num);
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      if (group.Material != null)
      {
        if (!(this.lastDiffuse != group.Material.Diffuse))
          return;
        this.material.Diffuse = group.Material.Diffuse;
        this.lastDiffuse = group.Material.Diffuse;
      }
      else
        this.material.Diffuse = group.Mesh.Material.Diffuse;
    }
  }
}
