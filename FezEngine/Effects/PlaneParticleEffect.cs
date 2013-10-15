// Type: FezEngine.Effects.PlaneParticleEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class PlaneParticleEffect : BaseEffect, IShaderInstantiatableEffect<Matrix>
  {
    private readonly SemanticMappedTexture texture;
    private readonly SemanticMappedBoolean additive;
    private readonly SemanticMappedBoolean fullbright;
    private readonly SemanticMappedMatrixArray instanceData;

    public Matrix? ForcedViewProjection { get; set; }

    public LightingEffectPass Pass
    {
      set
      {
        this.currentPass = this.currentTechnique.Passes[value == LightingEffectPass.Pre ? 0 : 1];
      }
    }

    public bool Additive
    {
      set
      {
        this.additive.Set(value);
      }
    }

    public bool Fullbright
    {
      set
      {
        this.fullbright.Set(value);
      }
    }

    public PlaneParticleEffect()
      : base("PlaneParticleEffect")
    {
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.additive = new SemanticMappedBoolean(this.effect.Parameters, "Additive");
      this.fullbright = new SemanticMappedBoolean(this.effect.Parameters, "Fullbright");
      this.instanceData = new SemanticMappedMatrixArray(this.effect.Parameters, "InstanceData");
      this.Pass = LightingEffectPass.Main;
      this.SimpleMeshPrepare = this.SimpleGroupPrepare = true;
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.texture.Set((Texture) mesh.Texture);
      if (!this.ForcedViewProjection.HasValue)
        return;
      this.matrices.WorldViewProjection = this.ForcedViewProjection.Value;
    }

    public void SetInstanceData(Matrix[] instances)
    {
      this.instanceData.Set(instances);
    }
  }
}
