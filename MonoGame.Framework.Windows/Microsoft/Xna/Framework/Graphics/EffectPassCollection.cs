// Type: Microsoft.Xna.Framework.Graphics.EffectPassCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  public class EffectPassCollection : IEnumerable<EffectPass>, IEnumerable
  {
    private List<EffectPass> _passes = new List<EffectPass>();

    public EffectPass this[int index]
    {
      get
      {
        return this._passes[index];
      }
    }

    public EffectPass this[string name]
    {
      get
      {
        foreach (EffectPass effectPass in this._passes)
        {
          if (effectPass.Name == name)
            return effectPass;
        }
        return (EffectPass) null;
      }
    }

    public int Count
    {
      get
      {
        return this._passes.Count;
      }
    }

    internal EffectPassCollection()
    {
    }

    internal EffectPassCollection(Effect effect, EffectPassCollection cloneSource)
    {
      foreach (EffectPass cloneSource1 in cloneSource)
        this.Add(new EffectPass(effect, cloneSource1));
    }

    public List<EffectPass>.Enumerator GetEnumerator()
    {
      return this._passes.GetEnumerator();
    }

    IEnumerator<EffectPass> IEnumerable<EffectPass>.GetEnumerator()
    {
      return (IEnumerator<EffectPass>) this._passes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this._passes.GetEnumerator();
    }

    internal void Add(EffectPass pass)
    {
      this._passes.Add(pass);
    }
  }
}
