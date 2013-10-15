// Type: FezEngine.Effects.Structures.SemanticMappedInt32
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects.Structures
{
  public class SemanticMappedInt32 : SemanticMappedParameter<int>
  {
    public SemanticMappedInt32(EffectParameterCollection parent, string semanticName)
      : base(parent, semanticName)
    {
    }

    protected override void DoSet(int value)
    {
      this.parameter.SetValue((float) value);
      this.currentValue = value;
    }
  }
}
