// Type: FezEngine.Effects.FastBlurEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class FastBlurEffect : BaseEffect
  {
    private readonly SemanticMappedVector2 texelSize;
    private readonly SemanticMappedVector2 direction;
    private readonly SemanticMappedTexture texture;
    private readonly SemanticMappedSingle blurWidth;

    public BlurPass Pass
    {
      set
      {
        if (value == BlurPass.Horizontal)
          this.direction.Set(Vector2.UnitX);
        if (value != BlurPass.Vertical)
          return;
        this.direction.Set(Vector2.UnitY);
      }
    }

    public float BlurWidth
    {
      set
      {
        this.blurWidth.Set(value);
      }
    }

    public FastBlurEffect()
      : base("FastBlurEffect")
    {
      this.texelSize = new SemanticMappedVector2(this.effect.Parameters, "TexelSize");
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.blurWidth = new SemanticMappedSingle(this.effect.Parameters, "BlurWidth");
      this.direction = new SemanticMappedVector2(this.effect.Parameters, "Direction");
      this.effect.Parameters["Weights"].SetValue(new float[5]
      {
        0.08812122f,
        0.1675553f,
        0.1369112f,
        0.09517907f,
        0.05629372f
      });
      this.effect.Parameters["Offsets"].SetValue(new float[5]
      {
        0.0f,
        -0.01529978f,
        -0.03565004f,
        -0.05588228f,
        -0.07593089f
      });
      this.BlurWidth = 1f;
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.texture.Set((Texture) mesh.Texture);
      this.texelSize.Set(new Vector2(1f / (float) mesh.TextureMap.Width, 1f / (float) mesh.TextureMap.Height));
    }
  }
}
