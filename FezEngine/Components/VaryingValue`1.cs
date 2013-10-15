// Type: FezEngine.Components.VaryingValue`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Components
{
  public abstract class VaryingValue<T>
  {
    public T Base;
    public T Variation;
    public Func<T, T, T> Function;

    protected abstract Func<T, T, T> DefaultFunction { get; }

    public T Evaluate()
    {
      if (this.Function != null)
        return this.Function(this.Base, this.Variation);
      else
        return this.DefaultFunction(this.Base, this.Variation);
    }
  }
}
