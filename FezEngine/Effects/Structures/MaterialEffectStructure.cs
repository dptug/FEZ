// Type: FezEngine.Effects.Structures.MaterialEffectStructure
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects.Structures
{
  internal class MaterialEffectStructure
  {
    private readonly SemanticMappedVector3 diffuse;
    private readonly SemanticMappedSingle opacity;

    public Vector3 Diffuse
    {
      set
      {
        this.diffuse.Set(value);
      }
    }

    public float Opacity
    {
      set
      {
        this.opacity.Set(value);
      }
    }

    public MaterialEffectStructure(EffectParameterCollection parameters)
    {
      this.diffuse = new SemanticMappedVector3(parameters, "Material_Diffuse");
      this.opacity = new SemanticMappedSingle(parameters, "Material_Opacity");
    }
  }
}
