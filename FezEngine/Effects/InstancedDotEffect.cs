// Type: FezEngine.Effects.InstancedDotEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class InstancedDotEffect : BaseEffect, IShaderInstantiatableEffect<Vector4>
  {
    private readonly SemanticMappedSingle theta;
    private readonly SemanticMappedSingle eightShapeStep;
    private readonly SemanticMappedSingle distanceFactor;
    private readonly SemanticMappedSingle immobilityFactor;
    private readonly SemanticMappedVectorArray instanceData;

    public float Theta
    {
      set
      {
        this.theta.Set(value);
      }
    }

    public float EightShapeStep
    {
      set
      {
        this.eightShapeStep.Set(value);
      }
    }

    public float DistanceFactor
    {
      set
      {
        this.distanceFactor.Set(value);
      }
    }

    public float ImmobilityFactor
    {
      set
      {
        this.immobilityFactor.Set(value);
      }
    }

    public InstancedDotEffect()
      : base("InstancedDotEffect")
    {
      this.theta = new SemanticMappedSingle(this.effect.Parameters, "Theta");
      this.eightShapeStep = new SemanticMappedSingle(this.effect.Parameters, "EightShapeStep");
      this.distanceFactor = new SemanticMappedSingle(this.effect.Parameters, "DistanceFactor");
      this.immobilityFactor = new SemanticMappedSingle(this.effect.Parameters, "ImmobilityFactor");
      this.instanceData = new SemanticMappedVectorArray(this.effect.Parameters, "InstanceData");
      this.SimpleGroupPrepare = true;
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.material.Diffuse = mesh.Material.Diffuse;
      this.material.Opacity = mesh.Material.Opacity;
    }

    public void SetInstanceData(Vector4[] instances)
    {
      this.instanceData.Set(instances);
    }
  }
}
