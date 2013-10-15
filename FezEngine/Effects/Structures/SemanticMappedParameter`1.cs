// Type: FezEngine.Effects.Structures.SemanticMappedParameter`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects.Structures
{
  public abstract class SemanticMappedParameter<T>
  {
    protected bool firstSet = true;
    protected readonly EffectParameter parameter;
    private readonly bool missingParameter;
    protected T currentValue;

    protected SemanticMappedParameter(EffectParameterCollection parent, string semanticName)
    {
      this.parameter = parent[semanticName] ?? parent[semanticName.Replace("Sampler", "Texture")];
      this.missingParameter = this.parameter == null;
    }

    public void Set(T value)
    {
      if (this.missingParameter)
        return;
      this.DoSet(value);
    }

    protected abstract void DoSet(T value);

    public T Get()
    {
      return this.currentValue;
    }
  }
}
