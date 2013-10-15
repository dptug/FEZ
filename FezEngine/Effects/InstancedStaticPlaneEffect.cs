// Type: FezEngine.Effects.InstancedStaticPlaneEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class InstancedStaticPlaneEffect : BaseEffect, IShaderInstantiatableEffect<Matrix>
  {
    private readonly SemanticMappedTexture baseTexture;
    private readonly SemanticMappedBoolean ignoreFog;
    private readonly SemanticMappedBoolean sewerHax;
    private readonly SemanticMappedMatrixArray instanceData;

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

    public InstancedStaticPlaneEffect()
      : base("InstancedStaticPlaneEffect")
    {
      this.baseTexture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.ignoreFog = new SemanticMappedBoolean(this.effect.Parameters, "IgnoreFog");
      this.sewerHax = new SemanticMappedBoolean(this.effect.Parameters, "SewerHax");
      this.instanceData = new SemanticMappedMatrixArray(this.effect.Parameters, "InstanceData");
      this.Pass = LightingEffectPass.Main;
    }

    public override void Prepare(Mesh mesh)
    {
      this.sewerHax.Set(this.LevelManager.WaterType == LiquidType.Sewer);
      this.matrices.WorldViewProjection = this.viewProjection;
      base.Prepare(mesh);
    }

    public override void Prepare(Group group)
    {
      this.baseTexture.Set(group.Texture);
    }

    public void SetInstanceData(Matrix[] instances)
    {
      this.instanceData.Set(instances);
    }
  }
}
