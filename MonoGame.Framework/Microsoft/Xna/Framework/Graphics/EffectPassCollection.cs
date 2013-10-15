// Type: Microsoft.Xna.Framework.Graphics.EffectPassCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
