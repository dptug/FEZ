// Type: FezEngine.Effects.VibratingEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class VibratingEffect : BaseEffect
  {
    private readonly SemanticMappedSingle intensity;
    private readonly SemanticMappedSingle timeStep;
    private readonly SemanticMappedSingle fogDensity;
    private Vector3 lastDiffuse;

    public float Intensity
    {
      get
      {
        return this.intensity.Get();
      }
      set
      {
        this.intensity.Set(value);
      }
    }

    public float TimeStep
    {
      get
      {
        return this.timeStep.Get();
      }
      set
      {
        this.timeStep.Set(value);
      }
    }

    public float FogDensity
    {
      get
      {
        return this.fogDensity.Get();
      }
      set
      {
        this.fogDensity.Set(value);
      }
    }

    public VibratingEffect()
      : base("VibratingEffect")
    {
      this.intensity = new SemanticMappedSingle(this.effect.Parameters, "Intensity");
      this.timeStep = new SemanticMappedSingle(this.effect.Parameters, "TimeStep");
      this.fogDensity = new SemanticMappedSingle(this.effect.Parameters, "FogDensity");
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
