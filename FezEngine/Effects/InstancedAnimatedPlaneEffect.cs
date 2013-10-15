// Type: FezEngine.Effects.InstancedAnimatedPlaneEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class InstancedAnimatedPlaneEffect : BaseEffect, IShaderInstantiatableEffect<Matrix>
  {
    private readonly SemanticMappedTexture animatedTexture;
    private readonly SemanticMappedVector2 frameScale;
    private readonly SemanticMappedBoolean ignoreFog;
    private readonly SemanticMappedBoolean sewerHax;
    private readonly SemanticMappedBoolean ignoreShading;
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

    public bool IgnoreShading
    {
      set
      {
        this.ignoreShading.Set(value);
      }
    }

    public InstancedAnimatedPlaneEffect()
      : base("InstancedAnimatedPlaneEffect")
    {
      this.animatedTexture = new SemanticMappedTexture(this.effect.Parameters, "AnimatedTexture");
      this.ignoreFog = new SemanticMappedBoolean(this.effect.Parameters, "IgnoreFog");
      this.sewerHax = new SemanticMappedBoolean(this.effect.Parameters, "SewerHax");
      this.ignoreShading = new SemanticMappedBoolean(this.effect.Parameters, "IgnoreShading");
      this.instanceData = new SemanticMappedMatrixArray(this.effect.Parameters, "InstanceData");
      this.frameScale = new SemanticMappedVector2(this.effect.Parameters, "FrameScale");
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
      this.animatedTexture.Set(group.Texture);
      this.frameScale.Set((Vector2) group.CustomData);
    }

    public void SetInstanceData(Matrix[] instances)
    {
      this.instanceData.Set(instances);
    }
  }
}
