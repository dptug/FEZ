// Type: FezEngine.Effects.GlitchyPostEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class GlitchyPostEffect : BaseEffect, IShaderInstantiatableEffect<Matrix>
  {
    private readonly SemanticMappedTexture glitchTexture;
    private readonly SemanticMappedMatrixArray instanceData;

    public GlitchyPostEffect()
      : base("GlitchyPostEffect")
    {
      this.glitchTexture = new SemanticMappedTexture(this.effect.Parameters, "GlitchTexture");
      this.instanceData = new SemanticMappedMatrixArray(this.effect.Parameters, "InstanceData");
      this.SimpleGroupPrepare = true;
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.glitchTexture.Set((Texture) mesh.Texture);
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      this.Apply();
    }

    public void SetInstanceData(Matrix[] instances)
    {
      this.instanceData.Set(instances);
    }
  }
}
