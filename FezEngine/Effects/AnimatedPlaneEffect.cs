// Type: FezEngine.Effects.AnimatedPlaneEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class AnimatedPlaneEffect : BaseEffect
  {
    private readonly SemanticMappedTexture animatedTexture;
    private readonly SemanticMappedBoolean ignoreFog;
    private readonly SemanticMappedBoolean fullbright;
    private readonly SemanticMappedBoolean alphaIsEmissive;
    private readonly SemanticMappedBoolean ignoreShading;
    private readonly SemanticMappedBoolean sewerHax;

    public LightingEffectPass Pass
    {
      set
      {
        this.currentPass = this.currentTechnique.Passes[value == LightingEffectPass.Pre ? 0 : 1];
      }
    }

    public bool IgnoreFog
    {
      set
      {
        this.ignoreFog.Set(value);
      }
    }

    public bool IgnoreShading
    {
      set
      {
        this.ignoreShading.Set(value);
      }
    }

    public AnimatedPlaneEffect()
      : base("AnimatedPlaneEffect")
    {
      this.animatedTexture = new SemanticMappedTexture(this.effect.Parameters, "AnimatedTexture");
      this.ignoreFog = new SemanticMappedBoolean(this.effect.Parameters, "IgnoreFog");
      this.fullbright = new SemanticMappedBoolean(this.effect.Parameters, "Fullbright");
      this.alphaIsEmissive = new SemanticMappedBoolean(this.effect.Parameters, "AlphaIsEmissive");
      this.ignoreShading = new SemanticMappedBoolean(this.effect.Parameters, "IgnoreShading");
      this.sewerHax = new SemanticMappedBoolean(this.effect.Parameters, "SewerHax");
      this.Pass = LightingEffectPass.Main;
    }

    public override void Prepare(Mesh mesh)
    {
      this.sewerHax.Set(this.LevelManager.WaterType == LiquidType.Sewer);
      base.Prepare(mesh);
      if (!this.ForcedViewMatrix.HasValue || !this.ForcedProjectionMatrix.HasValue)
        return;
      this.matrices.ViewProjection = this.ForcedViewMatrix.Value * this.ForcedProjectionMatrix.Value;
    }

    public override void Prepare(Group group)
    {
      if (this.IgnoreCache || !group.EffectOwner || group.InverseTransposeWorldMatrix.Dirty)
      {
        this.matrices.WorldInverseTranspose = (Matrix) group.InverseTransposeWorldMatrix;
        group.InverseTransposeWorldMatrix.Clean();
      }
      this.matrices.World = (Matrix) group.WorldMatrix;
      if (group.TextureMatrix.Value.HasValue)
      {
        this.matrices.TextureMatrix = group.TextureMatrix.Value.Value;
        this.textureMatrixDirty = true;
      }
      else if (this.textureMatrixDirty)
      {
        this.matrices.TextureMatrix = Matrix.Identity;
        this.textureMatrixDirty = false;
      }
      this.animatedTexture.Set(group.Texture);
      this.material.Diffuse = group.Material.Diffuse;
      this.material.Opacity = group.Material.Opacity;
      PlaneCustomData planeCustomData = group.CustomData as PlaneCustomData;
      if (planeCustomData != null)
      {
        this.fullbright.Set(planeCustomData.Fullbright);
        this.alphaIsEmissive.Set(planeCustomData.AlphaIsEmissive);
      }
      else
      {
        this.fullbright.Set(false);
        this.alphaIsEmissive.Set(false);
      }
    }
  }
}
