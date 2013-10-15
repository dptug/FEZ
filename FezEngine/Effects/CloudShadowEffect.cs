// Type: FezEngine.Effects.CloudShadowEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class CloudShadowEffect : BaseEffect
  {
    private readonly SemanticMappedTexture texture;

    public CloudShadowPasses Pass
    {
      set
      {
        this.currentPass = this.currentTechnique.Passes[value == CloudShadowPasses.Canopy ? 1 : 0];
      }
    }

    public CloudShadowEffect()
      : base("CloudShadowEffect")
    {
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.texture.Set((Texture) mesh.Texture);
    }
  }
}
