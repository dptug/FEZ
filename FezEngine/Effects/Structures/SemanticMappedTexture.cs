// Type: FezEngine.Effects.Structures.SemanticMappedTexture
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FezEngine.Effects.Structures
{
  public class SemanticMappedTexture : SemanticMappedParameter<Texture>
  {
    public SemanticMappedTexture(EffectParameterCollection parent, string semanticName)
      : base(parent, semanticName.Replace("Texture", "Sampler"))
    {
    }

    protected override void DoSet(Texture value)
    {
      if (value == null)
        this.parameter.SetValue(value);
      else if (value.IsDisposed)
      {
        this.parameter.SetValue((Texture) null);
      }
      else
      {
        HashSet<SemanticMappedTexture> hashSet;
        if (value.Tag == null)
        {
          hashSet = new HashSet<SemanticMappedTexture>();
          value.Tag = (object) hashSet;
        }
        else
          hashSet = value.Tag as HashSet<SemanticMappedTexture>;
        if (hashSet != null)
          hashSet.Add(this);
        this.parameter.SetValue(value);
      }
    }
  }
}
