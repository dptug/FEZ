// Type: Microsoft.Xna.Framework.Graphics.EffectAnnotationCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  public class EffectAnnotationCollection : IEnumerable<EffectAnnotation>, IEnumerable
  {
    private List<EffectAnnotation> _annotations = new List<EffectAnnotation>();

    public int Count
    {
      get
      {
        return this._annotations.Count;
      }
    }

    public EffectAnnotation this[int index]
    {
      get
      {
        return this._annotations[index];
      }
    }

    public EffectAnnotation this[string name]
    {
      get
      {
        foreach (EffectAnnotation effectAnnotation in this._annotations)
        {
          if (effectAnnotation.Name == name)
            return effectAnnotation;
        }
        return (EffectAnnotation) null;
      }
    }

    public IEnumerator<EffectAnnotation> GetEnumerator()
    {
      return (IEnumerator<EffectAnnotation>) this._annotations.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this._annotations.GetEnumerator();
    }

    internal void Add(EffectAnnotation annotation)
    {
      this._annotations.Add(annotation);
    }
  }
}
