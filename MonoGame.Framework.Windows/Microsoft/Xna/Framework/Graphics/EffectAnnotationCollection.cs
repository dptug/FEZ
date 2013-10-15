// Type: Microsoft.Xna.Framework.Graphics.EffectAnnotationCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
