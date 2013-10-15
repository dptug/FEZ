// Type: FezEngine.Effects.BurnInPostEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class BurnInPostEffect : BaseEffect
  {
    private readonly SemanticMappedVector3 acceptColor;
    private readonly SemanticMappedTexture newFrameBuffer;
    private readonly SemanticMappedTexture oldFrameBuffer;

    public Texture OldFrameBuffer
    {
      set
      {
        this.oldFrameBuffer.Set(value);
      }
    }

    public Texture NewFrameBuffer
    {
      set
      {
        this.newFrameBuffer.Set(value);
      }
    }

    public BurnInPostEffect()
      : base("BurnInPostEffect")
    {
      this.acceptColor = new SemanticMappedVector3(this.effect.Parameters, "AcceptColor");
      this.acceptColor.Set(new Vector3(0.9333333f, 0.0f, 0.5529412f));
      this.oldFrameBuffer = new SemanticMappedTexture(this.effect.Parameters, "OldFrameTexture");
      this.newFrameBuffer = new SemanticMappedTexture(this.effect.Parameters, "NewFrameTexture");
    }
  }
}
