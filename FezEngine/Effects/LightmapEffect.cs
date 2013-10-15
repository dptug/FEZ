// Type: FezEngine.Effects.LightmapEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class LightmapEffect : BaseEffect
  {
    private readonly SemanticMappedTexture texture;
    private readonly SemanticMappedBoolean shadowPass;

    public bool ShadowPass
    {
      get
      {
        return this.shadowPass.Get();
      }
      set
      {
        this.shadowPass.Set(value);
      }
    }

    public LightmapEffect()
      : base("LightmapEffect")
    {
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.shadowPass = new SemanticMappedBoolean(this.effect.Parameters, "ShadowPass");
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.texture.Set((Texture) mesh.Texture);
    }
  }
}
