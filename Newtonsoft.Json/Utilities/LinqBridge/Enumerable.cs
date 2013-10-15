// Type: Newtonsoft.Json.Utilities.LinqBridge.Enumerable
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Newtonsoft.Json.Utilities.LinqBridge
{
  internal static class Enumerable
  {
    public static IEnumerable<TSource> AsEnumerable<TSource>(IEnumerable<TSource> source)
    {
      return source;
    }

    public static IEnumerable<TResult> Empty<TResult>()
    {
      return Enumerable.Sequence<TResult>.Empty;
    }

    public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
    {
      Enumerable.CheckNotNull<IEnumerable>(source, "source");
      return Enumerable.CastYield<TResult>(source);
    }

    private static IEnumerable<TResult> CastYield<TResult>(IEnumerable source)
    {
      foreach (object obj in source)
        yield return (TResult) obj;
    }

    public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
    {
      Enumerable.CheckNotNull<IEnumerable>(source, "source");
      return Enumerable.OfTypeYield<TResult>(source);
    }

    private static IEnumerable<TResult> OfTypeYield<TResult>(IEnumerable source)
    {
      foreach (object obj in source)
      {
        if (obj is TResult)
          yield return (TResult) obj;
      }
    }

    public static IEnumerable<int> Range(int start, int count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", (object) count, (string) null);
      long end = (long) start + (long) count;
      if (end - 1L >= (long) int.MaxValue)
        throw new ArgumentOutOfRangeException("count", (object) count, (string) null);
      else
        return Enumerable.RangeYield(start, end);
    }

    private static IEnumerable<int> RangeYield(int start, long end)
    {
      for (int i = start; (long) i < end; ++i)
        yield return i;
    }

    public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", (object) count, (string) null);
      else
        return Enumerable.RepeatYield<TResult>(element, count);
    }

    private static IEnumerable<TResult> RepeatYield<TResult>(TResult element, int count)
    {
      for (int i = 0; i < count; ++i)
        yield return element;
    }

    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      Enumerable.CheckNotNull<Func<TSource, bool>>(predicate, "predicate");
      return Enumerable.Where<TSource>(source, (Func<TSource, int, bool>) ((item, i) => predicate(item)));
    }

    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, int, bool>>(predicate, "predicate");
      return Enumerable.WhereYield<TSource>(source, predicate);
    }

    private static IEnumerable<TSource> WhereYield<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
      int i = 0;
      foreach (TSource source1 in source)
      {
        if (predicate(source1, i++))
          yield return source1;
      }
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
      Enumerable.CheckNotNull<Func<TSource, TResult>>(selector, "selector");
      return Enumerable.Select<TSource, TResult>(source, (Func<TSource, int, TResult>) ((item, i) => selector(item)));
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, int, TResult>>(selector, "selector");
      return Enumerable.SelectYield<TSource, TResult>(source, selector);
    }

    private static IEnumerable<TResult> SelectYield<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
    {
      int i = 0;
      foreach (TSource source1 in source)
        yield return selector(source1, i++);
    }

    public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
    {
      Enumerable.CheckNotNull<Func<TSource, IEnumerable<TResult>>>(selector, "selector");
      return Enumerable.SelectMany<TSource, TResult>(source, (Func<TSource, int, IEnumerable<TResult>>) ((item, i) => selector(item)));
    }

    public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
    {
      Enumerable.CheckNotNull<Func<TSource, int, IEnumerable<TResult>>>(selector, "selector");
      return Enumerable.SelectMany<TSource, TResult, TResult>(source, selector, (Func<TSource, TResult, TResult>) ((item, subitem) => subitem));
    }

    public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
    {
      Enumerable.CheckNotNull<Func<TSource, IEnumerable<TCollection>>>(collectionSelector, "collectionSelector");
      return Enumerable.SelectMany<TSource, TCollection, TResult>(source, (Func<TSource, int, IEnumerable<TCollection>>) ((item, i) => collectionSelector(item)), resultSelector);
    }

    public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, int, IEnumerable<TCollection>>>(collectionSelector, "collectionSelector");
      Enumerable.CheckNotNull<Func<TSource, TCollection, TResult>>(resultSelector, "resultSelector");
      return Enumerable.SelectManyYield<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    private static IEnumerable<TResult> SelectManyYield<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
    {
      int i = 0;
      foreach (TSource source1 in source)
      {
        foreach (TCollection collection in collectionSelector(source1, i++))
          yield return resultSelector(source1, collection);
      }
    }

    public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      Enumerable.CheckNotNull<Func<TSource, bool>>(predicate, "predicate");
      return Enumerable.TakeWhile<TSource>(source, (Func<TSource, int, bool>) ((item, i) => predicate(item)));
    }

    public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, int, bool>>(predicate, "predicate");
      return Enumerable.TakeWhileYield<TSource>(source, predicate);
    }

    private static IEnumerable<TSource> TakeWhileYield<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
      int i = 0;
      foreach (TSource source1 in source)
      {
        if (predicate(source1, i++))
          yield return source1;
        else
          break;
      }
    }

    private static TSource FirstImpl<TSource>(this IEnumerable<TSource> source, Func<TSource> empty)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      IList<TSource> list = source as IList<TSource>;
      if (list != null)
      {
        if (list.Count <= 0)
          return empty();
        else
          return list[0];
      }
      else
      {
        using (IEnumerator<TSource> enumerator = source.GetEnumerator())
          return enumerator.MoveNext() ? enumerator.Current : empty();
      }
    }

    public static TSource First<TSource>(this IEnumerable<TSource> source)
    {
      return Enumerable.FirstImpl<TSource>(source, Enumerable.Futures<TSource>.Undefined);
    }

    public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return Enumerable.First<TSource>(Enumerable.Where<TSource>(source, predicate));
    }

    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
    {
      return Enumerable.FirstImpl<TSource>(source, Enumerable.Futures<TSource>.Default);
    }

    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return Enumerable.FirstOrDefault<TSource>(Enumerable.Where<TSource>(source, predicate));
    }

    private static TSource LastImpl<TSource>(this IEnumerable<TSource> source, Func<TSource> empty)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      IList<TSource> list = source as IList<TSource>;
      if (list != null)
      {
        if (list.Count <= 0)
          return empty();
        else
          return list[list.Count - 1];
      }
      else
      {
        using (IEnumerator<TSource> enumerator = source.GetEnumerator())
        {
          if (!enumerator.MoveNext())
            return empty();
          TSource current = enumerator.Current;
          while (enumerator.MoveNext())
            current = enumerator.Current;
          return current;
        }
      }
    }

    public static TSource Last<TSource>(this IEnumerable<TSource> source)
    {
      return Enumerable.LastImpl<TSource>(source, Enumerable.Futures<TSource>.Undefined);
    }

    public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return Enumerable.Last<TSource>(Enumerable.Where<TSource>(source, predicate));
    }

    public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source)
    {
      return Enumerable.LastImpl<TSource>(source, Enumerable.Futures<TSource>.Default);
    }

    public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return Enumerable.LastOrDefault<TSource>(Enumerable.Where<TSource>(source, predicate));
    }

    private static TSource SingleImpl<TSource>(this IEnumerable<TSource> source, Func<TSource> empty)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      using (IEnumerator<TSource> enumerator = source.GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return empty();
        TSource current = enumerator.Current;
        if (!enumerator.MoveNext())
          return current;
        else
          throw new InvalidOperationException();
      }
    }

    public static TSource Single<TSource>(this IEnumerable<TSource> source)
    {
      return Enumerable.SingleImpl<TSource>(source, Enumerable.Futures<TSource>.Undefined);
    }

    public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return Enumerable.Single<TSource>(Enumerable.Where<TSource>(source, predicate));
    }

    public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source)
    {
      return Enumerable.SingleImpl<TSource>(source, Enumerable.Futures<TSource>.Default);
    }

    public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return Enumerable.SingleOrDefault<TSource>(Enumerable.Where<TSource>(source, predicate));
    }

    public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      if (index < 0)
        throw new ArgumentOutOfRangeException("index", (object) index, (string) null);
      IList<TSource> list = source as IList<TSource>;
      if (list != null)
        return list[index];
      try
      {
        return Enumerable.First<TSource>(Enumerable.SkipWhile<TSource>(source, (Func<TSource, int, bool>) ((item, i) => i < index)));
      }
      catch (InvalidOperationException ex)
      {
        throw new ArgumentOutOfRangeException("index", (object) index, (string) null);
      }
    }

    public static TSource ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      if (index < 0)
        return default (TSource);
      IList<TSource> list = source as IList<TSource>;
      if (list == null)
        return Enumerable.FirstOrDefault<TSource>(Enumerable.SkipWhile<TSource>(source, (Func<TSource, int, bool>) ((item, i) => i < index)));
      if (index >= list.Count)
        return default (TSource);
      else
        return list[index];
    }

    public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      return Enumerable.ReverseYield<TSource>(source);
    }

    private static IEnumerable<TSource> ReverseYield<TSource>(IEnumerable<TSource> source)
    {
      Stack<TSource> stack = new Stack<TSource>();
      foreach (TSource source1 in source)
        stack.Push(source1);
      foreach (TSource source1 in stack)
        yield return source1;
    }

    public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
    {
      return Enumerable.Where<TSource>(source, (Func<TSource, int, bool>) ((item, i) => i < count));
    }

    public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
    {
      return Enumerable.Where<TSource>(source, (Func<TSource, int, bool>) ((item, i) => i >= count));
    }

    public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      Enumerable.CheckNotNull<Func<TSource, bool>>(predicate, "predicate");
      return Enumerable.SkipWhile<TSource>(source, (Func<TSource, int, bool>) ((item, i) => predicate(item)));
    }

    public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, int, bool>>(predicate, "predicate");
      return Enumerable.SkipWhileYield<TSource>(source, predicate);
    }

    private static IEnumerable<TSource> SkipWhileYield<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
      using (IEnumerator<TSource> enumerator = source.GetEnumerator())
      {
        int num = 0;
        while (true)
        {
          if (enumerator.MoveNext())
          {
            if (predicate(enumerator.Current, num))
              ++num;
            else
              goto label_5;
          }
          else
            break;
        }
        yield break;
label_5:
        do
        {
          yield return enumerator.Current;
        }
        while (enumerator.MoveNext());
      }
    }

    public static int Count<TSource>(this IEnumerable<TSource> source)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      ICollection collection = source as ICollection;
      if (collection == null)
        return Enumerable.Aggregate<TSource, int>(source, 0, (Func<int, TSource, int>) ((count, item) => {checked {count + 1;}}));
      else
        return collection.Count;
    }

    public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return Enumerable.Count<TSource>(Enumerable.Where<TSource>(source, predicate));
    }

    public static long LongCount<TSource>(this IEnumerable<TSource> source)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Array array = source as Array;
      if (array == null)
        return Enumerable.Aggregate<TSource, long>(source, 0L, (Func<long, TSource, long>) ((count, item) => count + 1L));
      else
        return array.LongLength;
    }

    public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return Enumerable.LongCount<TSource>(Enumerable.Where<TSource>(source, predicate));
    }

    public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(first, "first");
      Enumerable.CheckNotNull<IEnumerable<TSource>>(second, "second");
      return Enumerable.ConcatYield<TSource>(first, second);
    }

    private static IEnumerable<TSource> ConcatYield<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
      foreach (TSource source in first)
        yield return source;
      foreach (TSource source in second)
        yield return source;
    }

    public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      return new List<TSource>(source);
    }

    public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
    {
      return Enumerable.ToList<TSource>(source).ToArray();
    }

    public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
    {
      return Enumerable.Distinct<TSource>(source, (IEqualityComparer<TSource>) null);
    }

    public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      return Enumerable.DistinctYield<TSource>(source, comparer);
    }

    private static IEnumerable<TSource> DistinctYield<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
    {
      Dictionary<TSource, object> set = new Dictionary<TSource, object>(comparer);
      bool gotNull = false;
      foreach (TSource key in source)
      {
        if ((object) (TSource) key == null)
        {
          if (!gotNull)
            gotNull = true;
          else
            continue;
        }
        else if (!set.ContainsKey(key))
          set.Add(key, (object) null);
        else
          continue;
        yield return key;
      }
    }

    public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      return Enumerable.ToLookup<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (e => e), (IEqualityComparer<TKey>) null);
    }

    public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
      return Enumerable.ToLookup<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (e => e), comparer);
    }

    public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
      return Enumerable.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) null);
    }

    public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, TKey>>(keySelector, "keySelector");
      Enumerable.CheckNotNull<Func<TSource, TElement>>(elementSelector, "elementSelector");
      Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
      foreach (TSource a in source)
      {
        TKey key = keySelector(a);
        Enumerable.Grouping<TKey, TElement> grouping = (Enumerable.Grouping<TKey, TElement>) lookup.Find(key);
        if (grouping == null)
        {
          grouping = new Enumerable.Grouping<TKey, TElement>(key);
          lookup.Add((IGrouping<TKey, TElement>) grouping);
        }
        grouping.Add(elementSelector(a));
      }
      return (ILookup<TKey, TElement>) lookup;
    }

    public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      return Enumerable.GroupBy<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) null);
    }

    public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
      return Enumerable.GroupBy<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (e => e), comparer);
    }

    public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
      return Enumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) null);
    }

    public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, TKey>>(keySelector, "keySelector");
      Enumerable.CheckNotNull<Func<TSource, TElement>>(elementSelector, "elementSelector");
      return (IEnumerable<IGrouping<TKey, TElement>>) Enumerable.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
    {
      return Enumerable.GroupBy<TSource, TKey, TResult>(source, keySelector, resultSelector, (IEqualityComparer<TKey>) null);
    }

    public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, TKey>>(keySelector, "keySelector");
      Enumerable.CheckNotNull<Func<TKey, IEnumerable<TSource>, TResult>>(resultSelector, "resultSelector");
      return Enumerable.Select<IGrouping<TKey, TSource>, TResult>((IEnumerable<IGrouping<TKey, TSource>>) Enumerable.ToLookup<TSource, TKey>(source, keySelector, comparer), (Func<IGrouping<TKey, TSource>, TResult>) (g => resultSelector(g.Key, (IEnumerable<TSource>) g)));
    }

    public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
      return Enumerable.GroupBy<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, (IEqualityComparer<TKey>) null);
    }

    public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, TKey>>(keySelector, "keySelector");
      Enumerable.CheckNotNull<Func<TSource, TElement>>(elementSelector, "elementSelector");
      Enumerable.CheckNotNull<Func<TKey, IEnumerable<TElement>, TResult>>(resultSelector, "resultSelector");
      return Enumerable.Select<IGrouping<TKey, TElement>, TResult>((IEnumerable<IGrouping<TKey, TElement>>) Enumerable.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer), (Func<IGrouping<TKey, TElement>, TResult>) (g => resultSelector(g.Key, (IEnumerable<TElement>) g)));
    }

    public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, TSource, TSource>>(func, "func");
      using (IEnumerator<TSource> enumerator = source.GetEnumerator())
      {
        if (!enumerator.MoveNext())
          throw new InvalidOperationException();
        else
          return Enumerable.Aggregate<TSource, TSource>(Enumerable.Skip<TSource>(Enumerable.Renumerable<TSource>(enumerator), 1), enumerator.Current, func);
      }
    }

    public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
    {
      return Enumerable.Aggregate<TSource, TAccumulate, TAccumulate>(source, seed, func, (Func<TAccumulate, TAccumulate>) (r => r));
    }

    public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TAccumulate, TSource, TAccumulate>>(func, "func");
      Enumerable.CheckNotNull<Func<TAccumulate, TResult>>(resultSelector, "resultSelector");
      TAccumulate a = seed;
      foreach (TSource source1 in source)
        a = func(a, source1);
      return resultSelector(a);
    }

    public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
      return Enumerable.Union<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
      return Enumerable.Distinct<TSource>(Enumerable.Concat<TSource>(first, second), comparer);
    }

    public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
    {
      return Enumerable.DefaultIfEmpty<TSource>(source, default (TSource));
    }

    public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      return Enumerable.DefaultIfEmptyYield<TSource>(source, defaultValue);
    }

    private static IEnumerable<TSource> DefaultIfEmptyYield<TSource>(IEnumerable<TSource> source, TSource defaultValue)
    {
      using (IEnumerator<TSource> enumerator = source.GetEnumerator())
      {
        if (!enumerator.MoveNext())
        {
          yield return defaultValue;
        }
        else
        {
          do
          {
            yield return enumerator.Current;
          }
          while (enumerator.MoveNext());
        }
      }
    }

    public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, bool>>(predicate, "predicate");
      foreach (TSource a in source)
      {
        if (!predicate(a))
          return false;
      }
      return true;
    }

    public static bool Any<TSource>(this IEnumerable<TSource> source)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      using (IEnumerator<TSource> enumerator = source.GetEnumerator())
        return enumerator.MoveNext();
    }

    public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return Enumerable.Any<TSource>(Enumerable.Where<TSource>(source, predicate));
    }

    public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
    {
      return Enumerable.Contains<TSource>(source, value, (IEqualityComparer<TSource>) null);
    }

    public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      if (comparer == null)
      {
        ICollection<TSource> collection = source as ICollection<TSource>;
        if (collection != null)
          return collection.Contains(value);
      }
      comparer = comparer ?? (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default;
      return Enumerable.Any<TSource>(source, (Func<TSource, bool>) (item => comparer.Equals(item, value)));
    }

    public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
      return Enumerable.SequenceEqual<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(first, "frist");
      Enumerable.CheckNotNull<IEnumerable<TSource>>(second, "second");
      comparer = comparer ?? (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default;
      using (IEnumerator<TSource> enumerator1 = first.GetEnumerator())
      {
        using (IEnumerator<TSource> enumerator2 = second.GetEnumerator())
        {
          while (enumerator1.MoveNext())
          {
            if (!enumerator2.MoveNext() || !comparer.Equals(enumerator1.Current, enumerator2.Current))
              return false;
          }
          return !enumerator2.MoveNext();
        }
      }
    }

    private static TSource MinMaxImpl<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> lesser)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      return Enumerable.Aggregate<TSource>(source, (Func<TSource, TSource, TSource>) ((a, item) =>
      {
        if (!lesser(a, item))
          return item;
        else
          return a;
      }));
    }

    private static TSource? MinMaxImpl<TSource>(this IEnumerable<TSource?> source, TSource? seed, Func<TSource?, TSource?, bool> lesser) where TSource : struct
    {
      Enumerable.CheckNotNull<IEnumerable<TSource?>>(source, "source");
      return Enumerable.Aggregate<TSource?, TSource?>(source, seed, (Func<TSource?, TSource?, TSource?>) ((a, item) =>
      {
        if (!lesser(a, item))
          return item;
        else
          return a;
      }));
    }

    public static TSource Min<TSource>(this IEnumerable<TSource> source)
    {
      Comparer<TSource> comparer = Comparer<TSource>.Default;
      return Enumerable.MinMaxImpl<TSource>(source, (Func<TSource, TSource, bool>) ((x, y) => comparer.Compare(x, y) < 0));
    }

    public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
      return Enumerable.Min<TResult>(Enumerable.Select<TSource, TResult>(source, selector));
    }

    public static TSource Max<TSource>(this IEnumerable<TSource> source)
    {
      Comparer<TSource> comparer = Comparer<TSource>.Default;
      return Enumerable.MinMaxImpl<TSource>(source, (Func<TSource, TSource, bool>) ((x, y) => comparer.Compare(x, y) > 0));
    }

    public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
      return Enumerable.Max<TResult>(Enumerable.Select<TSource, TResult>(source, selector));
    }

    private static IEnumerable<T> Renumerable<T>(this IEnumerator<T> e)
    {
      do
      {
        yield return e.Current;
      }
      while (e.MoveNext());
    }

    public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      return Enumerable.OrderBy<TSource, TKey>(source, keySelector, (IComparer<TKey>) null);
    }

    public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, TKey>>(keySelector, "keySelector");
      return (IOrderedEnumerable<TSource>) new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, false);
    }

    public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      return Enumerable.OrderByDescending<TSource, TKey>(source, keySelector, (IComparer<TKey>) null);
    }

    public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "keySelector");
      return (IOrderedEnumerable<TSource>) new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, true);
    }

    public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      return Enumerable.ThenBy<TSource, TKey>(source, keySelector, (IComparer<TKey>) null);
    }

    public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IOrderedEnumerable<TSource>>(source, "source");
      return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
    }

    public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      return Enumerable.ThenByDescending<TSource, TKey>(source, keySelector, (IComparer<TKey>) null);
    }

    public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IOrderedEnumerable<TSource>>(source, "source");
      return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
    }

    private static IEnumerable<TSource> IntersectExceptImpl<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer, bool flag)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(first, "first");
      Enumerable.CheckNotNull<IEnumerable<TSource>>(second, "second");
      List<TSource> list = new List<TSource>();
      Dictionary<TSource, bool> flags = new Dictionary<TSource, bool>(comparer);
      foreach (TSource key in Enumerable.Where<TSource>(first, (Func<TSource, bool>) (k => !flags.ContainsKey(k))))
      {
        flags.Add(key, !flag);
        list.Add(key);
      }
      foreach (TSource index in Enumerable.Where<TSource>(second, new Func<TSource, bool>(flags.ContainsKey)))
        flags[index] = flag;
      return Enumerable.Where<TSource>((IEnumerable<TSource>) list, (Func<TSource, bool>) (item => flags[item]));
    }

    public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
      return Enumerable.Intersect<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
      return Enumerable.IntersectExceptImpl<TSource>(first, second, comparer, true);
    }

    public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
      return Enumerable.Except<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
      return Enumerable.IntersectExceptImpl<TSource>(first, second, comparer, false);
    }

    public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      return Enumerable.ToDictionary<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) null);
    }

    public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
      return Enumerable.ToDictionary<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (e => e));
    }

    public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
      return Enumerable.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) null);
    }

    public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TSource>>(source, "source");
      Enumerable.CheckNotNull<Func<TSource, TKey>>(keySelector, "keySelector");
      Enumerable.CheckNotNull<Func<TSource, TElement>>(elementSelector, "elementSelector");
      Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(comparer);
      foreach (TSource a in source)
        dictionary.Add(keySelector(a), elementSelector(a));
      return dictionary;
    }

    public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
    {
      return Enumerable.Join<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, (IEqualityComparer<TKey>) null);
    }

    public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TOuter>>(outer, "outer");
      Enumerable.CheckNotNull<IEnumerable<TInner>>(inner, "inner");
      Enumerable.CheckNotNull<Func<TOuter, TKey>>(outerKeySelector, "outerKeySelector");
      Enumerable.CheckNotNull<Func<TInner, TKey>>(innerKeySelector, "innerKeySelector");
      Enumerable.CheckNotNull<Func<TOuter, TInner, TResult>>(resultSelector, "resultSelector");
      ILookup<TKey, TInner> lookup = Enumerable.ToLookup<TInner, TKey>(inner, innerKeySelector, comparer);
      return Enumerable.SelectMany<TOuter, TInner, TResult>(outer, (Func<TOuter, IEnumerable<TInner>>) (o => lookup[outerKeySelector(o)]), (Func<TOuter, TInner, TResult>) ((o, i) => resultSelector(o, i)));
    }

    public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
    {
      return Enumerable.GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, (IEqualityComparer<TKey>) null);
    }

    public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      Enumerable.CheckNotNull<IEnumerable<TOuter>>(outer, "outer");
      Enumerable.CheckNotNull<IEnumerable<TInner>>(inner, "inner");
      Enumerable.CheckNotNull<Func<TOuter, TKey>>(outerKeySelector, "outerKeySelector");
      Enumerable.CheckNotNull<Func<TInner, TKey>>(innerKeySelector, "innerKeySelector");
      Enumerable.CheckNotNull<Func<TOuter, IEnumerable<TInner>, TResult>>(resultSelector, "resultSelector");
      ILookup<TKey, TInner> lookup = Enumerable.ToLookup<TInner, TKey>(inner, innerKeySelector, comparer);
      return Enumerable.Select<TOuter, TResult>(outer, (Func<TOuter, TResult>) (o => resultSelector(o, lookup[outerKeySelector(o)])));
    }

    [DebuggerStepThrough]
    private static void CheckNotNull<T>(T value, string name) where T : class
    {
      if ((object) value == null)
        throw new ArgumentNullException(name);
    }

    public static int Sum(this IEnumerable<int> source)
    {
      Enumerable.CheckNotNull<IEnumerable<int>>(source, "source");
      int num1 = 0;
      foreach (int num2 in source)
        checked { num1 += num2; }
      return num1;
    }

    public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, int>(source, selector));
    }

    public static double Average(this IEnumerable<int> source)
    {
      Enumerable.CheckNotNull<IEnumerable<int>>(source, "source");
      long num1 = 0L;
      long num2 = 0L;
      foreach (int num3 in source)
      {
        checked { num1 += (long) num3; }
        checked { ++num2; }
      }
      if (num2 == 0L)
        throw new InvalidOperationException();
      else
        return (double) num1 / (double) num2;
    }

    public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, int>(source, selector));
    }

    public static int? Sum(this IEnumerable<int?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<int?>>(source, "source");
      int num = 0;
      foreach (int? nullable in source)
        checked { num += nullable ?? 0; }
      return new int?(num);
    }

    public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, int?>(source, selector));
    }

    public static double? Average(this IEnumerable<int?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<int?>>(source, "source");
      long num1 = 0L;
      long num2 = 0L;
      foreach (int? nullable in Enumerable.Where<int?>(source, (Func<int?, bool>) (n => n.HasValue)))
      {
        checked { num1 += (long) nullable.Value; }
        checked { ++num2; }
      }
      if (num2 == 0L)
        return new double?();
      double? nullable1 = new double?((double) num1);
      double num3 = (double) num2;
      if (!nullable1.HasValue)
        return new double?();
      else
        return new double?(nullable1.GetValueOrDefault() / num3);
    }

    public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, int?>(source, selector));
    }

    public static int? Min(this IEnumerable<int?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<int?>>(source, "source");
      return Enumerable.MinMaxImpl<int>(Enumerable.Where<int?>(source, (Func<int?, bool>) (x => x.HasValue)), new int?(), (Func<int?, int?, bool>) ((min, x) =>
      {
        int? local_0 = min;
        int? local_1 = x;
        if (local_0.GetValueOrDefault() < local_1.GetValueOrDefault())
          return local_0.HasValue & local_1.HasValue;
        else
          return false;
      }));
    }

    public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
      return Enumerable.Min(Enumerable.Select<TSource, int?>(source, selector));
    }

    public static int? Max(this IEnumerable<int?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<int?>>(source, "source");
      return Enumerable.MinMaxImpl<int>(Enumerable.Where<int?>(source, (Func<int?, bool>) (x => x.HasValue)), new int?(), (Func<int?, int?, bool>) ((max, x) =>
      {
        if (!x.HasValue)
          return true;
        if (max.HasValue)
          return x.Value < max.Value;
        else
          return false;
      }));
    }

    public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
      return Enumerable.Max(Enumerable.Select<TSource, int?>(source, selector));
    }

    public static long Sum(this IEnumerable<long> source)
    {
      Enumerable.CheckNotNull<IEnumerable<long>>(source, "source");
      long num1 = 0L;
      foreach (long num2 in source)
        checked { num1 += num2; }
      return num1;
    }

    public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, long>(source, selector));
    }

    public static double Average(this IEnumerable<long> source)
    {
      Enumerable.CheckNotNull<IEnumerable<long>>(source, "source");
      long num1 = 0L;
      long num2 = 0L;
      foreach (long num3 in source)
      {
        checked { num1 += num3; }
        checked { ++num2; }
      }
      if (num2 == 0L)
        throw new InvalidOperationException();
      else
        return (double) num1 / (double) num2;
    }

    public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, long>(source, selector));
    }

    public static long? Sum(this IEnumerable<long?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<long?>>(source, "source");
      long num = 0L;
      foreach (long? nullable in source)
        checked { num += nullable ?? 0L; }
      return new long?(num);
    }

    public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, long?>(source, selector));
    }

    public static double? Average(this IEnumerable<long?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<long?>>(source, "source");
      long num1 = 0L;
      long num2 = 0L;
      foreach (long? nullable in Enumerable.Where<long?>(source, (Func<long?, bool>) (n => n.HasValue)))
      {
        checked { num1 += nullable.Value; }
        checked { ++num2; }
      }
      if (num2 == 0L)
        return new double?();
      double? nullable1 = new double?((double) num1);
      double num3 = (double) num2;
      if (!nullable1.HasValue)
        return new double?();
      else
        return new double?(nullable1.GetValueOrDefault() / num3);
    }

    public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, long?>(source, selector));
    }

    public static long? Min(this IEnumerable<long?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<long?>>(source, "source");
      return Enumerable.MinMaxImpl<long>(Enumerable.Where<long?>(source, (Func<long?, bool>) (x => x.HasValue)), new long?(), (Func<long?, long?, bool>) ((min, x) =>
      {
        long? local_0 = min;
        long? local_1 = x;
        if (local_0.GetValueOrDefault() < local_1.GetValueOrDefault())
          return local_0.HasValue & local_1.HasValue;
        else
          return false;
      }));
    }

    public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
      return Enumerable.Min(Enumerable.Select<TSource, long?>(source, selector));
    }

    public static long? Max(this IEnumerable<long?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<long?>>(source, "source");
      return Enumerable.MinMaxImpl<long>(Enumerable.Where<long?>(source, (Func<long?, bool>) (x => x.HasValue)), new long?(), (Func<long?, long?, bool>) ((max, x) =>
      {
        if (!x.HasValue)
          return true;
        if (max.HasValue)
          return x.Value < max.Value;
        else
          return false;
      }));
    }

    public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
      return Enumerable.Max(Enumerable.Select<TSource, long?>(source, selector));
    }

    public static float Sum(this IEnumerable<float> source)
    {
      Enumerable.CheckNotNull<IEnumerable<float>>(source, "source");
      float num1 = 0.0f;
      foreach (float num2 in source)
        num1 += num2;
      return num1;
    }

    public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, float>(source, selector));
    }

    public static float Average(this IEnumerable<float> source)
    {
      Enumerable.CheckNotNull<IEnumerable<float>>(source, "source");
      float num1 = 0.0f;
      long num2 = 0L;
      foreach (float num3 in source)
      {
        num1 += num3;
        checked { ++num2; }
      }
      if (num2 == 0L)
        throw new InvalidOperationException();
      else
        return num1 / (float) num2;
    }

    public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, float>(source, selector));
    }

    public static float? Sum(this IEnumerable<float?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<float?>>(source, "source");
      float num1 = 0.0f;
      foreach (float? nullable1 in source)
      {
        double num2 = (double) num1;
        float? nullable2 = nullable1;
        double num3 = nullable2.HasValue ? (double) nullable2.GetValueOrDefault() : 0.0;
        num1 = (float) (num2 + num3);
      }
      return new float?(num1);
    }

    public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, float?>(source, selector));
    }

    public static float? Average(this IEnumerable<float?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<float?>>(source, "source");
      float num1 = 0.0f;
      long num2 = 0L;
      foreach (float? nullable in Enumerable.Where<float?>(source, (Func<float?, bool>) (n => n.HasValue)))
      {
        num1 += nullable.Value;
        checked { ++num2; }
      }
      if (num2 == 0L)
        return new float?();
      float? nullable1 = new float?(num1);
      float num3 = (float) num2;
      if (!nullable1.HasValue)
        return new float?();
      else
        return new float?(nullable1.GetValueOrDefault() / num3);
    }

    public static float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, float?>(source, selector));
    }

    public static float? Min(this IEnumerable<float?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<float?>>(source, "source");
      return Enumerable.MinMaxImpl<float>(Enumerable.Where<float?>(source, (Func<float?, bool>) (x => x.HasValue)), new float?(), (Func<float?, float?, bool>) ((min, x) =>
      {
        float? local_0 = min;
        float? local_1 = x;
        if ((double) local_0.GetValueOrDefault() < (double) local_1.GetValueOrDefault())
          return local_0.HasValue & local_1.HasValue;
        else
          return false;
      }));
    }

    public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
      return Enumerable.Min(Enumerable.Select<TSource, float?>(source, selector));
    }

    public static float? Max(this IEnumerable<float?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<float?>>(source, "source");
      return Enumerable.MinMaxImpl<float>(Enumerable.Where<float?>(source, (Func<float?, bool>) (x => x.HasValue)), new float?(), (Func<float?, float?, bool>) ((max, x) =>
      {
        if (!x.HasValue)
          return true;
        if (max.HasValue)
          return (double) x.Value < (double) max.Value;
        else
          return false;
      }));
    }

    public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
      return Enumerable.Max(Enumerable.Select<TSource, float?>(source, selector));
    }

    public static double Sum(this IEnumerable<double> source)
    {
      Enumerable.CheckNotNull<IEnumerable<double>>(source, "source");
      double num1 = 0.0;
      foreach (double num2 in source)
        num1 += num2;
      return num1;
    }

    public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, double>(source, selector));
    }

    public static double Average(this IEnumerable<double> source)
    {
      Enumerable.CheckNotNull<IEnumerable<double>>(source, "source");
      double num1 = 0.0;
      long num2 = 0L;
      foreach (double num3 in source)
      {
        num1 += num3;
        checked { ++num2; }
      }
      if (num2 == 0L)
        throw new InvalidOperationException();
      else
        return num1 / (double) num2;
    }

    public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, double>(source, selector));
    }

    public static double? Sum(this IEnumerable<double?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<double?>>(source, "source");
      double num = 0.0;
      foreach (double? nullable in source)
        num += nullable ?? 0.0;
      return new double?(num);
    }

    public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, double?>(source, selector));
    }

    public static double? Average(this IEnumerable<double?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<double?>>(source, "source");
      double num1 = 0.0;
      long num2 = 0L;
      foreach (double? nullable in Enumerable.Where<double?>(source, (Func<double?, bool>) (n => n.HasValue)))
      {
        num1 += nullable.Value;
        checked { ++num2; }
      }
      if (num2 == 0L)
        return new double?();
      double? nullable1 = new double?(num1);
      double num3 = (double) num2;
      if (!nullable1.HasValue)
        return new double?();
      else
        return new double?(nullable1.GetValueOrDefault() / num3);
    }

    public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, double?>(source, selector));
    }

    public static double? Min(this IEnumerable<double?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<double?>>(source, "source");
      return Enumerable.MinMaxImpl<double>(Enumerable.Where<double?>(source, (Func<double?, bool>) (x => x.HasValue)), new double?(), (Func<double?, double?, bool>) ((min, x) =>
      {
        double? local_0 = min;
        double? local_1 = x;
        if (local_0.GetValueOrDefault() < local_1.GetValueOrDefault())
          return local_0.HasValue & local_1.HasValue;
        else
          return false;
      }));
    }

    public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
      return Enumerable.Min(Enumerable.Select<TSource, double?>(source, selector));
    }

    public static double? Max(this IEnumerable<double?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<double?>>(source, "source");
      return Enumerable.MinMaxImpl<double>(Enumerable.Where<double?>(source, (Func<double?, bool>) (x => x.HasValue)), new double?(), (Func<double?, double?, bool>) ((max, x) =>
      {
        if (!x.HasValue)
          return true;
        if (max.HasValue)
          return x.Value < max.Value;
        else
          return false;
      }));
    }

    public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
      return Enumerable.Max(Enumerable.Select<TSource, double?>(source, selector));
    }

    public static Decimal Sum(this IEnumerable<Decimal> source)
    {
      Enumerable.CheckNotNull<IEnumerable<Decimal>>(source, "source");
      Decimal num1 = new Decimal(0);
      foreach (Decimal num2 in source)
        num1 += num2;
      return num1;
    }

    public static Decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Decimal> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, Decimal>(source, selector));
    }

    public static Decimal Average(this IEnumerable<Decimal> source)
    {
      Enumerable.CheckNotNull<IEnumerable<Decimal>>(source, "source");
      Decimal num1 = new Decimal(0);
      long num2 = 0L;
      foreach (Decimal num3 in source)
      {
        num1 += num3;
        checked { ++num2; }
      }
      if (num2 == 0L)
        throw new InvalidOperationException();
      else
        return num1 / (Decimal) num2;
    }

    public static Decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Decimal> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, Decimal>(source, selector));
    }

    public static Decimal? Sum(this IEnumerable<Decimal?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<Decimal?>>(source, "source");
      Decimal num = new Decimal(0);
      foreach (Decimal? nullable in source)
        num += nullable ?? new Decimal(0);
      return new Decimal?(num);
    }

    public static Decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Decimal?> selector)
    {
      return Enumerable.Sum(Enumerable.Select<TSource, Decimal?>(source, selector));
    }

    public static Decimal? Average(this IEnumerable<Decimal?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<Decimal?>>(source, "source");
      Decimal num1 = new Decimal(0);
      long num2 = 0L;
      foreach (Decimal? nullable in Enumerable.Where<Decimal?>(source, (Func<Decimal?, bool>) (n => n.HasValue)))
      {
        num1 += nullable.Value;
        checked { ++num2; }
      }
      if (num2 == 0L)
        return new Decimal?();
      Decimal? nullable1 = new Decimal?(num1);
      Decimal num3 = (Decimal) num2;
      if (!nullable1.HasValue)
        return new Decimal?();
      else
        return new Decimal?(nullable1.GetValueOrDefault() / num3);
    }

    public static Decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Decimal?> selector)
    {
      return Enumerable.Average(Enumerable.Select<TSource, Decimal?>(source, selector));
    }

    public static Decimal? Min(this IEnumerable<Decimal?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<Decimal?>>(source, "source");
      return Enumerable.MinMaxImpl<Decimal>(Enumerable.Where<Decimal?>(source, (Func<Decimal?, bool>) (x => x.HasValue)), new Decimal?(), (Func<Decimal?, Decimal?, bool>) ((min, x) =>
      {
        Decimal? local_0 = min;
        Decimal? local_1 = x;
        if (local_0.GetValueOrDefault() < local_1.GetValueOrDefault())
          return local_0.HasValue & local_1.HasValue;
        else
          return false;
      }));
    }

    public static Decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Decimal?> selector)
    {
      return Enumerable.Min(Enumerable.Select<TSource, Decimal?>(source, selector));
    }

    public static Decimal? Max(this IEnumerable<Decimal?> source)
    {
      Enumerable.CheckNotNull<IEnumerable<Decimal?>>(source, "source");
      return Enumerable.MinMaxImpl<Decimal>(Enumerable.Where<Decimal?>(source, (Func<Decimal?, bool>) (x => x.HasValue)), new Decimal?(), (Func<Decimal?, Decimal?, bool>) ((max, x) =>
      {
        if (!x.HasValue)
          return true;
        if (max.HasValue)
          return x.Value < max.Value;
        else
          return false;
      }));
    }

    public static Decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Decimal?> selector)
    {
      return Enumerable.Max(Enumerable.Select<TSource, Decimal?>(source, selector));
    }

    private static class Futures<T>
    {
      public static readonly Func<T> Default = (Func<T>) (() => default (T));
      public static readonly Func<T> Undefined = (Func<T>) (() =>
      {
        throw new InvalidOperationException();
      });

      static Futures()
      {
      }
    }

    private static class Sequence<T>
    {
      public static readonly IEnumerable<T> Empty = (IEnumerable<T>) new T[0];

      static Sequence()
      {
      }
    }

    private sealed class Grouping<K, V> : List<V>, IGrouping<K, V>, IEnumerable<V>, IEnumerable
    {
      public K Key { get; private set; }

      internal Grouping(K key)
      {
        this.Key = key;
      }
    }
  }
}
