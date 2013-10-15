// Type: Microsoft.Xna.Framework.Graphics.EffectParameterCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  public class EffectParameterCollection : IEnumerable<EffectParameter>, IEnumerable
  {
    private List<EffectParameter> _parameters = new List<EffectParameter>();

    public int Count
    {
      get
      {
        return this._parameters.Count;
      }
    }

    public EffectParameter this[int index]
    {
      get
      {
        return this._parameters[index];
      }
    }

    public EffectParameter this[string name]
    {
      get
      {
        foreach (EffectParameter effectParameter in this._parameters)
        {
          if (effectParameter != null && effectParameter.Name == name)
            return effectParameter;
        }
        return (EffectParameter) null;
      }
    }

    internal EffectParameterCollection()
    {
    }

    internal EffectParameterCollection(EffectParameterCollection cloneSource)
    {
      foreach (EffectParameter cloneSource1 in cloneSource)
        this.Add(new EffectParameter(cloneSource1));
    }

    public IEnumerator<EffectParameter> GetEnumerator()
    {
      return (IEnumerator<EffectParameter>) this._parameters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this._parameters.GetEnumerator();
    }

    internal void Add(EffectParameter param)
    {
      this._parameters.Add(param);
    }
  }
}
