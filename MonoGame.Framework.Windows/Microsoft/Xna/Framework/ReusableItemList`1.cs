// Type: Microsoft.Xna.Framework.ReusableItemList`1
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
  internal class ReusableItemList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IEnumerator<T>, IDisposable, IEnumerator
  {
    private readonly List<T> _list = new List<T>();
    private int _listTop = 0;
    private int _iteratorIndex;

    public T this[int index]
    {
      get
      {
        if (index >= this._listTop)
          throw new IndexOutOfRangeException();
        else
          return this._list[index];
      }
      set
      {
        if (index >= this._listTop)
          throw new IndexOutOfRangeException();
        this._list[index] = value;
      }
    }

    public int Count
    {
      get
      {
        return this._listTop;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public T Current
    {
      get
      {
        return this._list[this._iteratorIndex];
      }
    }

    object IEnumerator.Current
    {
      get
      {
        return (object) this._list[this._iteratorIndex];
      }
    }

    public void Add(T item)
    {
      if (this._list.Count > this._listTop)
        this._list[this._listTop] = item;
      else
        this._list.Add(item);
      ++this._listTop;
    }

    public void Sort(IComparer<T> comparison)
    {
      this._list.Sort(comparison);
    }

    public T GetNewItem()
    {
      if (this._listTop < this._list.Count)
        return this._list[this._listTop++];
      else
        return default (T);
    }

    public void Clear()
    {
      this._listTop = 0;
    }

    public void Reset()
    {
      this.Clear();
      this._list.Clear();
    }

    public bool Contains(T item)
    {
      return this._list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      this._list.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
      this._iteratorIndex = -1;
      return (IEnumerator<T>) this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      this._iteratorIndex = -1;
      return (IEnumerator) this;
    }

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
      ++this._iteratorIndex;
      return this._iteratorIndex < this._listTop;
    }
  }
}
