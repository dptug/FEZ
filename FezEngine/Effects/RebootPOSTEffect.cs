// Type: FezEngine.Effects.RebootPOSTEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class RebootPOSTEffect : BaseEffect
  {
    private readonly SemanticMappedTexture texture;
    private readonly SemanticMappedMatrix pseudoWorldMatrix;

    public Matrix PseudoWorld
    {
      set
      {
        this.pseudoWorldMatrix.Set(value);
      }
    }

    public RebootPOSTEffect()
      : base("RebootPOSTEffect")
    {
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.pseudoWorldMatrix = new SemanticMappedMatrix(this.effect.Parameters, "PseudoWorldMatrix");
      this.PseudoWorld = Matrix.Identity;
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.material.Diffuse = mesh.Material.Diffuse;
      this.material.Opacity = mesh.Material.Opacity;
      this.texture.Set((Texture) mesh.Texture);
    }
  }
}
