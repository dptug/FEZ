// Type: FezEngine.Effects.TrileEffect
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
  public class TrileEffect : BaseEffect, IShaderInstantiatableEffect<Vector4>
  {
    private static readonly TrileCustomData DefaultCustom = new TrileCustomData();
    private readonly SemanticMappedTexture textureAtlas;
    private readonly SemanticMappedBoolean blink;
    private readonly SemanticMappedBoolean unstable;
    private readonly SemanticMappedBoolean tiltTwoAxis;
    private readonly SemanticMappedBoolean shiny;
    private readonly SemanticMappedVectorArray instanceData;
    private readonly bool InEditor;
    private bool lastWasCustom;

    public LightingEffectPass Pass
    {
      set
      {
        this.currentPass = this.currentTechnique.Passes[value == LightingEffectPass.Pre ? 0 : 1];
      }
    }

    public bool Blink
    {
      set
      {
        this.blink.Set(value);
      }
    }

    static TrileEffect()
    {
    }

    public TrileEffect()
      : base("TrileEffect")
    {
      this.textureAtlas = new SemanticMappedTexture(this.effect.Parameters, "AtlasTexture");
      this.blink = new SemanticMappedBoolean(this.effect.Parameters, "Blink");
      this.unstable = new SemanticMappedBoolean(this.effect.Parameters, "Unstable");
      this.tiltTwoAxis = new SemanticMappedBoolean(this.effect.Parameters, "TiltTwoAxis");
      this.shiny = new SemanticMappedBoolean(this.effect.Parameters, "Shiny");
      this.instanceData = new SemanticMappedVectorArray(this.effect.Parameters, "InstanceData");
      this.InEditor = this.EngineState.InEditor;
      this.Pass = LightingEffectPass.Main;
      this.SimpleGroupPrepare = true;
      this.material.Opacity = 1f;
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.textureAtlas.Set((Texture) mesh.Texture);
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      if (this.InEditor)
        this.textureAtlas.Set(group.Texture);
      TrileCustomData trileCustomData = group.CustomData as TrileCustomData ?? TrileEffect.DefaultCustom;
      bool flag = trileCustomData.IsCustom;
      if (!this.lastWasCustom && !flag)
        return;
      this.unstable.Set(trileCustomData.Unstable);
      this.shiny.Set(trileCustomData.Shiny);
      this.tiltTwoAxis.Set(trileCustomData.TiltTwoAxis);
      this.lastWasCustom = flag;
    }

    public void SetInstanceData(Vector4[] instances)
    {
      this.instanceData.Set(instances);
    }
  }
}
