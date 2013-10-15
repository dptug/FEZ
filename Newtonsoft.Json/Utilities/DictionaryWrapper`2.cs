// Type: Newtonsoft.Json.Utilities.DictionaryWrapper`2
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
  internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IWrappedDictionary, IDictionary, ICollection, IEnumerable
  {
    private readonly IDictionary _dictionary;
    private readonly IDictionary<TKey, TValue> _genericDictionary;
    private object _syncRoot;

    public ICollection<TKey> Keys
    {
      get
      {
        if (this._dictionary != null)
          return (ICollection<TKey>) Enumerable.ToList<TKey>(Enumerable.Cast<TKey>((IEnumerable) this._dictionary.Keys));
        else
          return this._genericDictionary.Keys;
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        if (this._dictionary != null)
          return (ICollection<TValue>) Enumerable.ToList<TValue>(Enumerable.Cast<TValue>((IEnumerable) this._dictionary.Values));
        else
          return this._genericDictionary.Values;
      }
    }

    public TValue this[TKey key]
    {
      get
      {
        if (this._dictionary != null)
          return (TValue) this._dictionary[(object) key];
        else
          return this._genericDictionary[key];
      }
      set
      {
        if (this._dictionary != null)
          this._dictionary[(object) key] = (object) value;
        else
          this._genericDictionary[key] = value;
      }
    }

    public int Count
    {
      get
      {
        if (this._dictionary != null)
          return this._dictionary.Count;
        else
          return this._genericDictionary.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        if (this._dictionary != null)
          return this._dictionary.IsReadOnly;
        else
          return this._genericDictionary.IsReadOnly;
      }
    }

    bool IDictionary.IsFixedSize
    {
      get
      {
        if (this._genericDictionary != null)
          return false;
        else
          return this._dictionary.IsFixedSize;
      }
    }

    ICollection IDictionary.Keys
    {
      get
      {
        if (this._genericDictionary != null)
          return (ICollection) Enumerable.ToList<TKey>((IEnumerable<TKey>) this._genericDictionary.Keys);
        else
          return this._dictionary.Keys;
      }
    }

    ICollection IDictionary.Values
    {
      get
      {
        if (this._genericDictionary != null)
          return (ICollection) Enumerable.ToList<TValue>((IEnumerable<TValue>) this._genericDictionary.Values);
        else
          return this._dictionary.Values;
      }
    }

    bool ICollection.IsSynchronized
    {
      get
      {
        if (this._dictionary != null)
          return this._dictionary.IsSynchronized;
        else
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

    public object UnderlyingDictionary
    {
      get
      {
        if (this._dictionary != null)
          return (object) this._dictionary;
        else
          return (object) this._genericDictionary;
      }
    }

    public DictionaryWrapper(IDictionary dictionary)
    {
      ValidationUtils.ArgumentNotNull((object) dictionary, "dictionary");
      this._dictionary = dictionary;
    }

    public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
    {
      ValidationUtils.ArgumentNotNull((object) dictionary, "dictionary");
      this._genericDictionary = dictionary;
    }

    public void Add(TKey key, TValue value)
    {
      if (this._dictionary != null)
        this._dictionary.Add((object) key, (object) value);
      else
        this._genericDictionary.Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
      if (this._dictionary != null)
        return this._dictionary.Contains((object) key);
      else
        return this._genericDictionary.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
      if (this._dictionary == null)
        return this._genericDictionary.Remove(key);
      if (!this._dictionary.Contains((object) key))
        return false;
      this._dictionary.Remove((object) key);
      return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      if (this._dictionary == null)
        return this._genericDictionary.TryGetValue(key, out value);
      if (!this._dictionary.Contains((object) key))
      {
        value = default (TValue);
        return false;
      }
      else
      {
        value = (TValue) this._dictionary[(object) key];
        return true;
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      if (this._dictionary != null)
        ((IList) this._dictionary).Add((object) item);
      else
        this._genericDictionary.Add(item);
    }

    public void Clear()
    {
      if (this._dictionary != null)
        this._dictionary.Clear();
      else
        this._genericDictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      if (this._dictionary != null)
        return ((IList) this._dictionary).Contains((object) item);
      else
        return this._genericDictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      if (this._dictionary != null)
      {
        foreach (DictionaryEntry dictionaryEntry in this._dictionary)
          array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey) dictionaryEntry.Key, (TValue) dictionaryEntry.Value);
      }
      else
        this._genericDictionary.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      if (this._dictionary == null)
        return this._genericDictionary.Remove(item);
      if (!this._dictionary.Contains((object) item.Key))
        return true;
      if (!object.Equals(this._dictionary[(object) item.Key], (object) item.Value))
        return false;
      this._dictionary.Remove((object) item.Key);
      return true;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      if (this._dictionary != null)
        return Enumerable.Select<DictionaryEntry, KeyValuePair<TKey, TValue>>(Enumerable.Cast<DictionaryEntry>((IEnumerable) this._dictionary), (Func<DictionaryEntry, KeyValuePair<TKey, TValue>>) (de => new KeyValuePair<TKey, TValue>((TKey) de.Key, (TValue) de.Value))).GetEnumerator();
      else
        return this._genericDictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    void IDictionary.Add(object key, object value)
    {
      if (this._dictionary != null)
        this._dictionary.Add(key, value);
      else
        this._genericDictionary.Add((TKey) key, (TValue) value);
    }

    object IDictionary.get_Item(object key)
    {
      if (this._dictionary != null)
        return this._dictionary[key];
      else
        return (object) this._genericDictionary[(TKey) key];
    }

    void IDictionary.set_Item(object key, object value)
    {
      if (this._dictionary != null)
        this._dictionary[key] = value;
      else
        this._genericDictionary[(TKey) key] = (TValue) value;
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
      if (this._dictionary != null)
        return this._dictionary.GetEnumerator();
      else
        return (IDictionaryEnumerator) new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator());
    }

    bool IDictionary.Contains(object key)
    {
      if (this._genericDictionary != null)
        return this._genericDictionary.ContainsKey((TKey) key);
      else
        return this._dictionary.Contains(key);
    }

    public void Remove(object key)
    {
      if (this._dictionary != null)
        this._dictionary.Remove(key);
      else
        this._genericDictionary.Remove((TKey) key);
    }

    void ICollection.CopyTo(Array array, int index)
    {
      if (this._dictionary != null)
        this._dictionary.CopyTo(array, index);
      else
        this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[]) array, index);
    }

    private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
    {
      private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

      public DictionaryEntry Entry
      {
        get
        {
          return (DictionaryEntry) this.Current;
        }
      }

      public object Key
      {
        get
        {
          return this.Entry.Key;
        }
      }

      public object Value
      {
        get
        {
          return this.Entry.Value;
        }
      }

      public object Current
      {
        get
        {
          return (object) new DictionaryEntry((object) this._e.Current.Key, (object) this._e.Current.Value);
        }
      }

      public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
      {
        ValidationUtils.ArgumentNotNull((object) e, "e");
        this._e = e;
      }

      public bool MoveNext()
      {
        return this._e.MoveNext();
      }

      public void Reset()
      {
        this._e.Reset();
      }
    }
  }
}
