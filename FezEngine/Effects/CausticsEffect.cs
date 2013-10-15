// Type: FezEngine.Effects.CausticsEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Effects
{
  public class CausticsEffect : BaseEffect
  {
    private readonly SemanticMappedTexture animatedTexture;
    private readonly SemanticMappedMatrix nextFrameTextureMatrix;

    public CausticsEffect()
      : base("CausticsEffect")
    {
      this.animatedTexture = new SemanticMappedTexture(this.effect.Parameters, "AnimatedTexture");
      this.nextFrameTextureMatrix = new SemanticMappedMatrix(this.effect.Parameters, "NextFrameData");
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      if (mesh.CustomData == null)
        return;
      this.nextFrameTextureMatrix.Set((Matrix) mesh.CustomData);
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      if (this.IgnoreCache || !group.EffectOwner || group.InverseTransposeWorldMatrix.Dirty)
      {
        this.matrices.WorldInverseTranspose = (Matrix) group.InverseTransposeWorldMatrix;
        group.InverseTransposeWorldMatrix.Clean();
      }
      this.material.Diffuse = group.Material.Diffuse;
      this.animatedTexture.Set(group.Texture);
    }
  }
}
