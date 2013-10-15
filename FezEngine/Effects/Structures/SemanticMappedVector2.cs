// Type: FezEngine.Effects.Structures.SemanticMappedVector2
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects.Structures
{
  public class SemanticMappedVector2 : SemanticMappedParameter<Vector2>
  {
    public SemanticMappedVector2(EffectParameterCollection parent, string semanticName)
      : base(parent, semanticName)
    {
    }

    protected override void DoSet(Vector2 value)
    {
      this.parameter.SetValue(value);
    }
  }
}
