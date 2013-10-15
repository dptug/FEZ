// Type: FezEngine.Effects.Structures.FogEffectStructure
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects.Structures
{
  internal class FogEffectStructure
  {
    private readonly SemanticMappedInt32 fogType;
    private readonly SemanticMappedVector3 fogColor;
    private readonly SemanticMappedSingle fogDensity;

    public FogType FogType
    {
      set
      {
        this.fogType.Set((int) value);
      }
    }

    public Color FogColor
    {
      set
      {
        this.fogColor.Set(value.ToVector3());
      }
    }

    public float FogDensity
    {
      set
      {
        this.fogDensity.Set(value);
      }
    }

    public FogEffectStructure(EffectParameterCollection parameters)
    {
      this.fogType = new SemanticMappedInt32(parameters, "Fog_Type");
      this.fogColor = new SemanticMappedVector3(parameters, "Fog_Color");
      this.fogDensity = new SemanticMappedSingle(parameters, "Fog_Density");
    }
  }
}
