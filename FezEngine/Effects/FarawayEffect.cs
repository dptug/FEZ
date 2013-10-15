// Type: FezEngine.Effects.FarawayEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class FarawayEffect : BaseEffect
  {
    private readonly SemanticMappedTexture texture;
    private readonly SemanticMappedSingle actualOpacity;

    public float ActualOpacity
    {
      set
      {
        this.actualOpacity.Set(value);
      }
    }

    public FarawayEffect()
      : base("FarawayEffect")
    {
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.actualOpacity = new SemanticMappedSingle(this.effect.Parameters, "ActualOpacity");
      this.ActualOpacity = 1f;
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.material.Diffuse = mesh.Material.Diffuse;
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      this.material.Opacity = group.Material.Opacity;
      this.texture.Set(group.Texture);
    }

    public void CleanUp()
    {
      this.texture.Set((Texture) null);
    }
  }
}
