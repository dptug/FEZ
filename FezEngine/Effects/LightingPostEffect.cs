// Type: FezEngine.Effects.LightingPostEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Effects.Structures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FezEngine.Effects
{
  public class LightingPostEffect : BaseEffect
  {
    private readonly SemanticMappedSingle dawnContribution;
    private readonly SemanticMappedSingle duskContribution;
    private readonly SemanticMappedSingle nightContribution;
    private readonly Dictionary<LightingPostEffect.Passes, EffectPass> passes;

    public float DawnContribution
    {
      get
      {
        return this.dawnContribution.Get();
      }
      set
      {
        this.dawnContribution.Set(value);
      }
    }

    public float DuskContribution
    {
      get
      {
        return this.duskContribution.Get();
      }
      set
      {
        this.duskContribution.Set(value);
      }
    }

    public float NightContribution
    {
      get
      {
        return this.nightContribution.Get();
      }
      set
      {
        this.nightContribution.Set(value);
      }
    }

    public LightingPostEffect.Passes Pass
    {
      set
      {
        this.currentPass = this.passes[value];
      }
    }

    public LightingPostEffect()
      : base("LightingPostEffect")
    {
      this.dawnContribution = new SemanticMappedSingle(this.effect.Parameters, "DawnContribution");
      this.duskContribution = new SemanticMappedSingle(this.effect.Parameters, "DuskContribution");
      this.nightContribution = new SemanticMappedSingle(this.effect.Parameters, "NightContribution");
      this.passes = new Dictionary<LightingPostEffect.Passes, EffectPass>((IEqualityComparer<LightingPostEffect.Passes>) LightingPostEffect.PassesComparer.Default);
      foreach (LightingPostEffect.Passes key in Util.GetValues<LightingPostEffect.Passes>())
        this.passes.Add(key, this.currentTechnique.Passes[((object) key).ToString()]);
    }

    public enum Passes
    {
      Dawn,
      Dusk_Multiply,
      Dusk_Screen,
      Night,
    }

    private class PassesComparer : IEqualityComparer<LightingPostEffect.Passes>
    {
      public static readonly LightingPostEffect.PassesComparer Default = new LightingPostEffect.PassesComparer();

      static PassesComparer()
      {
      }

      public bool Equals(LightingPostEffect.Passes x, LightingPostEffect.Passes y)
      {
        return x == y;
      }

      public int GetHashCode(LightingPostEffect.Passes obj)
      {
        return (int) obj;
      }
    }
  }
}
