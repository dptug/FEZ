// Type: Newtonsoft.Json.Utilities.CollectionWrapper`1
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
  internal class CollectionWrapper<T> : ICollection<T>, IEnumerable<T>, IWrappedCollection, IList, ICollection, IEnumerable
  {
    private readonly IList _list;
    private readonly ICollection<T> _genericCollection;
    private object _syncRoot;

    public virtual int Count
    {
      get
      {
        if (this._genericCollection != null)
          return this._genericCollection.Count;
        else
          return this._list.Count;
      }
    }

    public virtual bool IsReadOnly
    {
      get
      {
        if (this._genericCollection != null)
          return this._genericCollection.IsReadOnly;
        else
          return this._list.IsReadOnly;
      }
    }

    bool IList.IsFixedSize
    {
      get
      {
        if (this._genericCollection != null)
          return this._genericCollection.IsReadOnly;
        else
          return this._list.IsFixedSize;
      }
    }

    bool ICollection.IsSynchronized
    {
      get
      {
        return false;
      }
    }

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    public object UnderlyingCollection
    {
      get
      {
        if (this._genericCollection != null)
          return (object) this._genericCollection;
        else
          return (object) this._list;
      }
    }

    public CollectionWrapper(IList list)
    {
      ValidationUtils.ArgumentNotNull((object) list, "list");
      if (list is ICollection<T>)
        this._genericCollection = (ICollection<T>) list;
      else
        this._list = list;
    }

    public CollectionWrapper(ICollection<T> list)
    {
      ValidationUtils.ArgumentNotNull((object) list, "list");
      this._genericCollection = list;
    }

    public virtual void Add(T item)
    {
      if (this._genericCollection != null)
        this._genericCollection.Add(item);
      else
        this._list.Add((object) item);
    }

    public virtual void Clear()
    {
      if (this._genericCollection != null)
        this._genericCollection.Clear();
      else
        this._list.Clear();
    }

    public virtual bool Contains(T item)
    {
      if (this._genericCollection != null)
        return this._genericCollection.Contains(item);
      else
        return this._list.Contains((object) item);
    }

    public virtual void CopyTo(T[] array, int arrayIndex)
    {
      if (this._genericCollection != null)
        this._genericCollection.CopyTo(array, arrayIndex);
      else
        this._list.CopyTo((Array) array, arrayIndex);
    }

    public virtual bool Remove(T item)
    {
      if (this._genericCollection != null)
        return this._genericCollection.Remove(item);
      bool flag = this._list.Contains((object) item);
      if (flag)
        this._list.Remove((object) item);
      return flag;
    }

    public virtual IEnumerator<T> GetEnumerator()
    {
      if (this._genericCollection != null)
        return this._genericCollection.GetEnumerator();
      else
        return Enumerable.Cast<T>((IEnumerable) this._list).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      if (this._genericCollection != null)
        return (IEnumerator) this._genericCollection.GetEnumerator();
      else
        return this._list.GetEnumerator();
    }

    int IList.Add(object value)
    {
      CollectionWrapper<T>.VerifyValueType(value);
      this.Add((T) value);
      return this.Count - 1;
    }

    bool IList.Contains(object value)
    {
      if (CollectionWrapper<T>.IsCompatibleObject(value))
        return this.Contains((T) value);
      else
        return false;
    }

    int IList.IndexOf(object value)
    {
      if (this._genericCollection != null)
        throw new InvalidOperationException("Wrapped ICollection<T> does not support IndexOf.");
      if (CollectionWrapper<T>.IsCompatibleObject(value))
        return this._list.IndexOf((object) (T) value);
      else
        return -1;
    }

    void IList.RemoveAt(int index)
    {
      if (this._genericCollection != null)
        throw new InvalidOperationException("Wrapped ICollection<T> does not support RemoveAt.");
      this._list.RemoveAt(index);
    }

    void IList.Insert(int index, object value)
    {
      if (this._genericCollection != null)
        throw new InvalidOperationException("Wrapped ICollection<T> does not support Insert.");
      CollectionWrapper<T>.VerifyValueType(value);
      this._list.Insert(index, (object) (T) value);
    }

    void IList.Remove(object value)
    {
      if (!CollectionWrapper<T>.IsCompatibleObject(value))
        return;
      this.Remove((T) value);
    }

    object IList.get_Item(int index)
    {
      if (this._genericCollection != null)
        throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
      else
        return this._list[index];
    }

    void IList.set_Item(int index, object value)
    {
      if (this._genericCollection != null)
        throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
      CollectionWrapper<T>.VerifyValueType(value);
      this._list[index] = (object) (T) value;
    }

    void ICollection.CopyTo(Array array, int arrayIndex)
    {
      this.CopyTo((T[]) array, arrayIndex);
    }

    private static void VerifyValueType(object value)
    {
      if (!CollectionWrapper<T>.IsCompatibleObject(value))
        throw new ArgumentException(StringUtils.FormatWith("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.", (IFormatProvider) CultureInfo.InvariantCulture, value, (object) typeof (T)), "value");
    }

    private static bool IsCompatibleObject(object value)
    {
      return value is T || value == null && (!TypeExtensions.IsValueType(typeof (T)) || ReflectionUtils.IsNullableType(typeof (T)));
    }
  }
}
