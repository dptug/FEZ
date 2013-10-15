// Type: FezEngine.Effects.GomezEffect
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
  public class GomezEffect : BaseEffect
  {
    private readonly SemanticMappedTexture animatedTexture;
    private readonly SemanticMappedBoolean silhouette;
    private readonly SemanticMappedSingle background;
    private readonly SemanticMappedBoolean colorSwap;
    private readonly SemanticMappedBoolean noMoreFez;
    private readonly SemanticMappedVector3 redSwap;
    private readonly SemanticMappedVector3 blackSwap;
    private readonly SemanticMappedVector3 whiteSwap;
    private readonly SemanticMappedVector3 yellowSwap;
    private readonly SemanticMappedVector3 graySwap;
    private ColorSwapMode colorSwapMode;

    public Texture Animation
    {
      set
      {
        this.animatedTexture.Set(value);
      }
    }

    public bool Silhouette
    {
      set
      {
        this.silhouette.Set(value);
      }
    }

    public float Background
    {
      set
      {
        this.background.Set(value);
      }
    }

    public ColorSwapMode ColorSwapMode
    {
      get
      {
        return this.colorSwapMode;
      }
      set
      {
        this.colorSwapMode = value;
        switch (this.colorSwapMode)
        {
          case ColorSwapMode.None:
            this.colorSwap.Set(false);
            break;
          case ColorSwapMode.VirtualBoy:
            this.colorSwap.Set(true);
            this.redSwap.Set(new Vector3(0.6196079f, 0.0f, 0.01960784f));
            this.blackSwap.Set(new Vector3(0.0f, 0.0f, 0.0f));
            this.whiteSwap.Set(new Vector3(0.9960784f, 0.003921569f, 0.0f));
            this.yellowSwap.Set(new Vector3(0.8156863f, 0.003921569f, 0.0f));
            this.graySwap.Set(new Vector3(0.3960784f, 0.003921569f, 0.0f));
            break;
          case ColorSwapMode.Gameboy:
            this.colorSwap.Set(true);
            this.redSwap.Set(new Vector3(0.3215686f, 0.4980392f, 0.2235294f));
            this.blackSwap.Set(new Vector3(0.1254902f, 0.2745098f, 0.1921569f));
            this.whiteSwap.Set(new Vector3(0.8431373f, 0.9098039f, 0.5803922f));
            this.yellowSwap.Set(new Vector3(0.682353f, 0.7686275f, 0.2509804f));
            this.graySwap.Set(new Vector3(0.3215686f, 0.4980392f, 0.2235294f));
            break;
          case ColorSwapMode.Cmyk:
            this.colorSwap.Set(true);
            this.redSwap.Set(new Vector3(0.9333333f, 0.0f, 0.5529412f));
            this.blackSwap.Set(new Vector3(0.0f, 0.0f, 0.0f));
            this.whiteSwap.Set(new Vector3(1f, 1f, 1f));
            this.yellowSwap.Set(new Vector3(1f, 1f, 0.0f));
            this.graySwap.Set(new Vector3(1f, 1f, 1f));
            break;
        }
      }
    }

    public bool NoMoreFez
    {
      set
      {
        this.noMoreFez.Set(value);
      }
    }

    public LightingEffectPass Pass
    {
      set
      {
        this.currentPass = this.currentTechnique.Passes[value == LightingEffectPass.Pre ? 0 : 1];
      }
    }

    public GomezEffect()
      : base("GomezEffect")
    {
      this.animatedTexture = new SemanticMappedTexture(this.effect.Parameters, "AnimatedTexture");
      this.silhouette = new SemanticMappedBoolean(this.effect.Parameters, "Silhouette");
      this.background = new SemanticMappedSingle(this.effect.Parameters, "Background");
      this.colorSwap = new SemanticMappedBoolean(this.effect.Parameters, "ColorSwap");
      this.redSwap = new SemanticMappedVector3(this.effect.Parameters, "RedSwap");
      this.blackSwap = new SemanticMappedVector3(this.effect.Parameters, "BlackSwap");
      this.whiteSwap = new SemanticMappedVector3(this.effect.Parameters, "WhiteSwap");
      this.yellowSwap = new SemanticMappedVector3(this.effect.Parameters, "YellowSwap");
      this.graySwap = new SemanticMappedVector3(this.effect.Parameters, "GraySwap");
      this.noMoreFez = new SemanticMappedBoolean(this.effect.Parameters, "NoMoreFez");
      this.Pass = LightingEffectPass.Main;
    }

    public override BaseEffect Clone()
    {
      return (BaseEffect) new GomezEffect()
      {
        Animation = this.animatedTexture.Get(),
        Silhouette = this.silhouette.Get(),
        Background = this.background.Get(),
        ColorSwapMode = this.colorSwapMode
      };
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      if (this.IgnoreCache || !group.EffectOwner || group.WorldMatrix.Dirty)
      {
        this.matrices.World = (Matrix) group.WorldMatrix;
        group.WorldMatrix.Clean();
      }
      if (group.TextureMatrix.Value.HasValue)
      {
        this.matrices.TextureMatrix = group.TextureMatrix.Value.Value;
        this.textureMatrixDirty = true;
      }
      else
      {
        if (!this.textureMatrixDirty)
          return;
        this.matrices.TextureMatrix = Matrix.Identity;
        this.textureMatrixDirty = false;
      }
    }
  }
}
