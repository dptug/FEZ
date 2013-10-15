// Type: FezEngine.Effects.CombineEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class CombineEffect : BaseEffect
  {
    private readonly SemanticMappedTexture rightTexture;
    private readonly SemanticMappedTexture leftTexture;
    private readonly SemanticMappedSingle redGamma;
    private readonly SemanticMappedMatrix rightFilter;
    private readonly SemanticMappedMatrix leftFilter;

    public Texture2D LeftTexture
    {
      set
      {
        this.leftTexture.Set((Texture) value);
      }
    }

    public Texture2D RightTexture
    {
      set
      {
        this.rightTexture.Set((Texture) value);
      }
    }

    public float RedGamma
    {
      set
      {
        this.redGamma.Set(value);
      }
    }

    public Matrix RightFilter
    {
      set
      {
        this.rightFilter.Set(value);
      }
    }

    public Matrix LeftFilter
    {
      set
      {
        this.leftFilter.Set(value);
      }
    }

    public CombineEffect()
      : base("CombineEffect")
    {
      this.rightTexture = new SemanticMappedTexture(this.effect.Parameters, "RightTexture");
      this.leftTexture = new SemanticMappedTexture(this.effect.Parameters, "LeftTexture");
      this.redGamma = new SemanticMappedSingle(this.effect.Parameters, "RedGamma");
      this.rightFilter = new SemanticMappedMatrix(this.effect.Parameters, "RightFilter");
      this.leftFilter = new SemanticMappedMatrix(this.effect.Parameters, "LeftFilter");
      this.LeftFilter = new Matrix(0.2125f, 0.7154f, 0.0721f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
      this.RightFilter = new Matrix(0.0f, 0.0f, 0.0f, 0.0f, 0.2125f, 0.7154f, 0.0721f, 0.0f, 0.2125f, 0.7154f, 0.0721f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    }
  }
}
