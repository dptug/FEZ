// Type: Microsoft.Xna.Framework.CurveKeyCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
  public class CurveKeyCollection : ICollection<CurveKey>, IEnumerable<CurveKey>, IEnumerable
  {
    private bool isReadOnly;
    private List<CurveKey> innerlist;

    public int Count
    {
      get
      {
        return this.innerlist.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return this.isReadOnly;
      }
    }

    public CurveKey this[int index]
    {
      get
      {
        return this.innerlist[index];
      }
      set
      {
        if (value == (CurveKey) null)
          throw new ArgumentNullException();
        if (index >= this.innerlist.Count)
          throw new IndexOutOfRangeException();
        if ((double) this.innerlist[index].Position == (double) value.Position)
        {
          this.innerlist[index] = value;
        }
        else
        {
          this.innerlist.RemoveAt(index);
          this.innerlist.Add(value);
        }
      }
    }

    public CurveKeyCollection()
    {
      this.innerlist = new List<CurveKey>();
    }

    public void Add(CurveKey item)
    {
      if (item == (CurveKey) null)
        throw new ArgumentNullException();
      if (this.innerlist.Count == 0)
      {
        this.innerlist.Add(item);
      }
      else
      {
        for (int index = 0; index < this.innerlist.Count; ++index)
        {
          if ((double) item.Position < (double) this.innerlist[index].Position)
          {
            this.innerlist.Insert(index, item);
            return;
          }
        }
        this.innerlist.Add(item);
      }
    }

    public void Clear()
    {
      this.innerlist.Clear();
    }

    public CurveKeyCollection Clone()
    {
      CurveKeyCollection curveKeyCollection = new CurveKeyCollection();
      foreach (CurveKey curveKey in this.innerlist)
        curveKeyCollection.Add(curveKey);
      return curveKeyCollection;
    }

    public bool Contains(CurveKey item)
    {
      return this.innerlist.Contains(item);
    }

    public void CopyTo(CurveKey[] array, int arrayIndex)
    {
      this.innerlist.CopyTo(array, arrayIndex);
    }

    public IEnumerator<CurveKey> GetEnumerator()
    {
      return (IEnumerator<CurveKey>) this.innerlist.GetEnumerator();
    }

    public int IndexOf(CurveKey item)
    {
      return this.innerlist.IndexOf(item);
    }

    public bool Remove(CurveKey item)
    {
      return this.innerlist.Remove(item);
    }

    public void RemoveAt(int index)
    {
      this.innerlist.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.innerlist.GetEnumerator();
    }
  }
}
