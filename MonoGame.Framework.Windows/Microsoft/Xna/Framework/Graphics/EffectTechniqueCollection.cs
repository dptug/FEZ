// Type: Microsoft.Xna.Framework.Graphics.EffectTechniqueCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  public class EffectTechniqueCollection : IEnumerable<EffectTechnique>, IEnumerable
  {
    private List<EffectTechnique> _techniques = new List<EffectTechnique>();

    public int Count
    {
      get
      {
        return this._techniques.Count;
      }
    }

    public EffectTechnique this[int index]
    {
      get
      {
        return this._techniques[index];
      }
    }

    public EffectTechnique this[string name]
    {
      get
      {
        foreach (EffectTechnique effectTechnique in this._techniques)
        {
          if (effectTechnique.Name == name)
            return effectTechnique;
        }
        return (EffectTechnique) null;
      }
    }

    internal EffectTechniqueCollection()
    {
    }

    internal EffectTechniqueCollection(Effect effect, EffectTechniqueCollection cloneSource)
    {
      foreach (EffectTechnique cloneSource1 in cloneSource)
        this.Add(new EffectTechnique(effect, cloneSource1));
    }

    public IEnumerator<EffectTechnique> GetEnumerator()
    {
      return (IEnumerator<EffectTechnique>) this._techniques.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this._techniques.GetEnumerator();
    }

    internal void Add(EffectTechnique technique)
    {
      this._techniques.Add(technique);
    }
  }
}
