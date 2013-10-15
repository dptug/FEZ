// Type: Microsoft.Xna.Framework.Graphics.ModelEffectCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class ModelEffectCollection : ReadOnlyCollection<Effect>
  {
    public ModelEffectCollection(IList<Effect> list)
      : base(list)
    {
    }

    internal ModelEffectCollection()
      : base((IList<Effect>) new List<Effect>())
    {
    }

    internal void Add(Effect item)
    {
      this.Items.Add(item);
    }

    internal void Remove(Effect item)
    {
      this.Items.Remove(item);
    }

    public ModelEffectCollection.Enumerator GetEnumerator()
    {
      return new ModelEffectCollection.Enumerator((List<Effect>) this.Items);
    }

    public struct Enumerator : IEnumerator<Effect>, IDisposable, IEnumerator
    {
      private List<Effect>.Enumerator enumerator;
      private bool disposed;

      public Effect Current
      {
        get
        {
          return this.enumerator.Current;
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.Current;
        }
      }

      internal Enumerator(List<Effect> list)
      {
        this.enumerator = list.GetEnumerator();
        this.disposed = false;
      }

      public void Dispose()
      {
        if (this.disposed)
          return;
        this.enumerator.Dispose();
        this.disposed = true;
      }

      public bool MoveNext()
      {
        return this.enumerator.MoveNext();
      }

      void IEnumerator.Reset()
      {
        IEnumerator enumerator = (IEnumerator) this.enumerator;
        enumerator.Reset();
        this.enumerator = (List<Effect>.Enumerator) enumerator;
      }
    }
  }
}
