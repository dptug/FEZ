// Type: Microsoft.Xna.Framework.Graphics.EffectParameterCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
