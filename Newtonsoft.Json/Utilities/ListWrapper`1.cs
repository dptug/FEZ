// Type: Newtonsoft.Json.Utilities.ListWrapper`1
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
  internal class ListWrapper<T> : CollectionWrapper<T>, IList<T>, ICollection<T>, IEnumerable<T>, IWrappedList, IList, ICollection, IEnumerable
  {
    private readonly IList<T> _genericList;

    public T this[int index]
    {
      get
      {
        if (this._genericList != null)
          return this._genericList[index];
        else
          return (T) this[index];
      }
      set
      {
        if (this._genericList != null)
          this._genericList[index] = value;
        else
          this[index] = (object) value;
      }
    }

    public override int Count
    {
      get
      {
        if (this._genericList != null)
          return this._genericList.Count;
        else
          return base.Count;
      }
    }

    public override bool IsReadOnly
    {
      get
      {
        if (this._genericList != null)
          return this._genericList.IsReadOnly;
        else
          return base.IsReadOnly;
      }
    }

    public object UnderlyingList
    {
      get
      {
        if (this._genericList != null)
          return (object) this._genericList;
        else
          return this.UnderlyingCollection;
      }
    }

    public ListWrapper(IList list)
      : base(list)
    {
      ValidationUtils.ArgumentNotNull((object) list, "list");
      if (!(list is IList<T>))
        return;
      this._genericList = (IList<T>) list;
    }

    public ListWrapper(IList<T> list)
      : base((ICollection<T>) list)
    {
      ValidationUtils.ArgumentNotNull((object) list, "list");
      this._genericList = list;
    }

    public int IndexOf(T item)
    {
      if (this._genericList != null)
        return this._genericList.IndexOf(item);
      else
        return this.IndexOf((object) item);
    }

    public void Insert(int index, T item)
    {
      if (this._genericList != null)
        this._genericList.Insert(index, item);
      else
        this.Insert(index, (object) item);
    }

    public void RemoveAt(int index)
    {
      if (this._genericList != null)
        this._genericList.RemoveAt(index);
      else
        this.RemoveAt(index);
    }

    public override void Add(T item)
    {
      if (this._genericList != null)
        this._genericList.Add(item);
      else
        base.Add(item);
    }

    public override void Clear()
    {
      if (this._genericList != null)
        this._genericList.Clear();
      else
        base.Clear();
    }

    public override bool Contains(T item)
    {
      if (this._genericList != null)
        return this._genericList.Contains(item);
      else
        return base.Contains(item);
    }

    public override void CopyTo(T[] array, int arrayIndex)
    {
      if (this._genericList != null)
        this._genericList.CopyTo(array, arrayIndex);
      else
        base.CopyTo(array, arrayIndex);
    }

    public override bool Remove(T item)
    {
      if (this._genericList != null)
        return this._genericList.Remove(item);
      bool flag = base.Contains(item);
      if (flag)
        base.Remove(item);
      return flag;
    }

    public override IEnumerator<T> GetEnumerator()
    {
      if (this._genericList != null)
        return this._genericList.GetEnumerator();
      else
        return base.GetEnumerator();
    }
  }
}
