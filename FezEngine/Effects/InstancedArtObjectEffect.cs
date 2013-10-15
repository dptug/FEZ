// Type: FezEngine.Effects.InstancedArtObjectEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class InstancedArtObjectEffect : BaseEffect, IShaderInstantiatableEffect<Matrix>
  {
    private readonly SemanticMappedTexture texture;
    private readonly SemanticMappedMatrixArray instanceData;

    public LightingEffectPass Pass
    {
      set
      {
        this.currentPass = this.currentTechnique.Passes[value == LightingEffectPass.Pre ? 0 : 1];
      }
    }

    public InstancedArtObjectEffect()
      : base("InstancedArtObjectEffect")
    {
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "CubemapTexture");
      this.instanceData = new SemanticMappedMatrixArray(this.effect.Parameters, "InstanceData");
      this.Pass = LightingEffectPass.Main;
      this.SimpleMeshPrepare = this.SimpleGroupPrepare = true;
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      this.texture.Set(group.Texture);
    }

    public void SetInstanceData(Matrix[] instances)
    {
      this.instanceData.Set(instances);
    }
  }
}
