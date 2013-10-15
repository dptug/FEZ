// Type: Newtonsoft.Json.Utilities.LinqBridge.OrderedEnumerable`2
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities.LinqBridge
{
  internal sealed class OrderedEnumerable<T, K> : IOrderedEnumerable<T>, IEnumerable<T>, IEnumerable
  {
    private readonly IEnumerable<T> _source;
    private readonly List<Comparison<T>> _comparisons;

    public OrderedEnumerable(IEnumerable<T> source, Func<T, K> keySelector, IComparer<K> comparer, bool descending)
      : this(source, (List<Comparison<T>>) null, keySelector, comparer, descending)
    {
    }

    private OrderedEnumerable(IEnumerable<T> source, List<Comparison<T>> comparisons, Func<T, K> keySelector, IComparer<K> comparer, bool descending)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      this._source = source;
      comparer = comparer ?? (IComparer<K>) Comparer<K>.Default;
      if (comparisons == null)
        comparisons = new List<Comparison<T>>(4);
      comparisons.Add((Comparison<T>) ((x, y) => (descending ? -1 : 1) * comparer.Compare(keySelector(x), keySelector(y))));
      this._comparisons = comparisons;
    }

    public IOrderedEnumerable<T> CreateOrderedEnumerable<KK>(Func<T, KK> keySelector, IComparer<KK> comparer, bool descending)
    {
      return (IOrderedEnumerable<T>) new OrderedEnumerable<T, KK>(this._source, this._comparisons, keySelector, comparer, descending);
    }

    public IEnumerator<T> GetEnumerator()
    {
      List<Tuple<T, int>> list = Enumerable.ToList<Tuple<T, int>>(Enumerable.Select<T, Tuple<T, int>>(this._source, new Func<T, int, Tuple<T, int>>(OrderedEnumerable<T, K>.TagPosition)));
      list.Sort((Comparison<Tuple<T, int>>) ((x, y) =>
      {
        List<Comparison<T>> local_0 = this._comparisons;
        for (int local_1 = 0; local_1 < local_0.Count; ++local_1)
        {
          int local_2 = local_0[local_1](x.First, y.First);
          if (local_2 != 0)
            return local_2;
        }
        return x.Second.CompareTo(y.Second);
      }));
      return Enumerable.Select<Tuple<T, int>, T>((IEnumerable<Tuple<T, int>>) list, new Func<Tuple<T, int>, T>(OrderedEnumerable<T, K>.GetFirst)).GetEnumerator();
    }

    private static Tuple<T, int> TagPosition(T e, int i)
    {
      return new Tuple<T, int>(e, i);
    }

    private static T GetFirst(Tuple<T, int> pv)
    {
      return pv.First;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
