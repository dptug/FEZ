// Type: FezEngine.Effects.CubemappedEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class CubemappedEffect : BaseEffect
  {
    private readonly SemanticMappedTexture cubemap;
    private readonly SemanticMappedBoolean forceShading;

    public LightingEffectPass Pass
    {
      set
      {
        this.currentPass = this.currentTechnique.Passes[value == LightingEffectPass.Pre ? 0 : 1];
      }
    }

    public bool ForceShading
    {
      set
      {
        this.forceShading.Set(value);
      }
    }

    public CubemappedEffect()
      : base("CubemappedEffect")
    {
      this.cubemap = new SemanticMappedTexture(this.effect.Parameters, "CubemapTexture");
      this.forceShading = new SemanticMappedBoolean(this.effect.Parameters, "ForceShading");
      this.Pass = LightingEffectPass.Main;
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      if (this.IgnoreCache || !group.EffectOwner || group.InverseTransposeWorldMatrix.Dirty)
      {
        this.matrices.WorldInverseTranspose = (Matrix) group.InverseTransposeWorldMatrix;
        group.InverseTransposeWorldMatrix.Clean();
      }
      this.cubemap.Set(group.Texture);
      this.material.Diffuse = group.Material != null ? group.Material.Diffuse : group.Mesh.Material.Diffuse;
    }
  }
}
