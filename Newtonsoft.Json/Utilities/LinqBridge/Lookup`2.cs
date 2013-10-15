// Type: Newtonsoft.Json.Utilities.LinqBridge.Lookup`2
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities.LinqBridge
{
  internal sealed class Lookup<TKey, TElement> : ILookup<TKey, TElement>, IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
  {
    private readonly Dictionary<TKey, IGrouping<TKey, TElement>> _map;

    public int Count
    {
      get
      {
        return this._map.Count;
      }
    }

    public IEnumerable<TElement> this[TKey key]
    {
      get
      {
        IGrouping<TKey, TElement> grouping;
        if (!this._map.TryGetValue(key, out grouping))
          return Enumerable.Empty<TElement>();
        else
          return (IEnumerable<TElement>) grouping;
      }
    }

    internal Lookup(IEqualityComparer<TKey> comparer)
    {
      this._map = new Dictionary<TKey, IGrouping<TKey, TElement>>(comparer);
    }

    internal void Add(IGrouping<TKey, TElement> item)
    {
      this._map.Add(item.Key, item);
    }

    internal IEnumerable<TElement> Find(TKey key)
    {
      IGrouping<TKey, TElement> grouping;
      if (!this._map.TryGetValue(key, out grouping))
        return (IEnumerable<TElement>) null;
      else
        return (IEnumerable<TElement>) grouping;
    }

    public bool Contains(TKey key)
    {
      return this._map.ContainsKey(key);
    }

    public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      foreach (KeyValuePair<TKey, IGrouping<TKey, TElement>> keyValuePair in this._map)
        yield return resultSelector(keyValuePair.Key, (IEnumerable<TElement>) keyValuePair.Value);
    }

    public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
    {
      return (IEnumerator<IGrouping<TKey, TElement>>) this._map.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
