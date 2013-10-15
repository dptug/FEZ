// Type: Newtonsoft.Json.Utilities.CollectionUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class CollectionUtils
  {
    public static IEnumerable<T> CastValid<T>(this IEnumerable enumerable)
    {
      ValidationUtils.ArgumentNotNull((object) enumerable, "enumerable");
      return Enumerable.Cast<T>((IEnumerable) Enumerable.Where<object>(Enumerable.Cast<object>(enumerable), (Func<object, bool>) (o => o is T)));
    }

    public static bool IsNullOrEmpty<T>(ICollection<T> collection)
    {
      if (collection != null)
        return collection.Count == 0;
      else
        return true;
    }

    public static void AddRange<T>(this IList<T> initial, IEnumerable<T> collection)
    {
      if (initial == null)
        throw new ArgumentNullException("initial");
      if (collection == null)
        return;
      foreach (T obj in collection)
        initial.Add(obj);
    }

    public static void AddRange(this IList initial, IEnumerable collection)
    {
      ValidationUtils.ArgumentNotNull((object) initial, "initial");
      CollectionUtils.AddRange<object>((IList<object>) new ListWrapper<object>(initial), Enumerable.Cast<object>(collection));
    }

    public static IList CreateGenericList(Type listType)
    {
      ValidationUtils.ArgumentNotNull((object) listType, "listType");
      return (IList) ReflectionUtils.CreateGeneric(typeof (List<>), listType, new object[0]);
    }

    public static bool IsDictionaryType(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, "type");
      return typeof (IDictionary).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof (IDictionary<,>));
    }

    public static IWrappedCollection CreateCollectionWrapper(object list)
    {
      ValidationUtils.ArgumentNotNull(list, "list");
      Type collectionDefinition;
      if (ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof (ICollection<>), out collectionDefinition))
        return (IWrappedCollection) ReflectionUtils.CreateGeneric(typeof (CollectionWrapper<>), (IList<Type>) new Type[1]
        {
          ReflectionUtils.GetCollectionItemType(collectionDefinition)
        }, (Func<Type, IList<object>, object>) ((t, a) => t.GetConstructor(new Type[1]
        {
          collectionDefinition
        }).Invoke(new object[1]
        {
          list
        })), new object[1]
        {
          list
        });
      else if (list is IList)
        return (IWrappedCollection) new CollectionWrapper<object>((IList) list);
      else
        throw new ArgumentException(StringUtils.FormatWith("Can not create ListWrapper for type {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) list.GetType()), "list");
    }

    public static IWrappedDictionary CreateDictionaryWrapper(object dictionary)
    {
      ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
      Type dictionaryDefinition;
      if (ReflectionUtils.ImplementsGenericDefinition(dictionary.GetType(), typeof (IDictionary<,>), out dictionaryDefinition))
        return (IWrappedDictionary) ReflectionUtils.CreateGeneric(typeof (DictionaryWrapper<,>), (IList<Type>) new Type[2]
        {
          ReflectionUtils.GetDictionaryKeyType(dictionaryDefinition),
          ReflectionUtils.GetDictionaryValueType(dictionaryDefinition)
        }, (Func<Type, IList<object>, object>) ((t, a) => t.GetConstructor(new Type[1]
        {
          dictionaryDefinition
        }).Invoke(new object[1]
        {
          dictionary
        })), new object[1]
        {
          dictionary
        });
      else if (dictionary is IDictionary)
        return (IWrappedDictionary) new DictionaryWrapper<object, object>((IDictionary) dictionary);
      else
        throw new ArgumentException(StringUtils.FormatWith("Can not create DictionaryWrapper for type {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) dictionary.GetType()), "dictionary");
    }

    public static IList CreateList(Type listType, out bool isReadOnlyOrFixedSize)
    {
      ValidationUtils.ArgumentNotNull((object) listType, "listType");
      isReadOnlyOrFixedSize = false;
      IList list1;
      if (listType.IsArray)
      {
        list1 = (IList) new List<object>();
        isReadOnlyOrFixedSize = true;
      }
      else
      {
        Type implementingType;
        if (ReflectionUtils.InheritsGenericDefinition(listType, typeof (ReadOnlyCollection<>), out implementingType))
        {
          Type listType1 = implementingType.GetGenericArguments()[0];
          Type type = ReflectionUtils.MakeGenericType(typeof (IEnumerable<>), new Type[1]
          {
            listType1
          });
          bool flag = false;
          foreach (MethodBase methodBase in listType.GetConstructors())
          {
            IList<ParameterInfo> list2 = (IList<ParameterInfo>) methodBase.GetParameters();
            if (list2.Count == 1 && type.IsAssignableFrom(list2[0].ParameterType))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            throw new Exception(StringUtils.FormatWith("Read-only type {0} does not have a public constructor that takes a type that implements {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) listType, (object) type));
          list1 = CollectionUtils.CreateGenericList(listType1);
          isReadOnlyOrFixedSize = true;
        }
        else
          list1 = !typeof (IList).IsAssignableFrom(listType) ? (!ReflectionUtils.ImplementsGenericDefinition(listType, typeof (ICollection<>)) ? (IList) null : (!ReflectionUtils.IsInstantiatableType(listType) ? (IList) null : (IList) CollectionUtils.CreateCollectionWrapper(Activator.CreateInstance(listType)))) : (!ReflectionUtils.IsInstantiatableType(listType) ? (listType != typeof (IList) ? (IList) null : (IList) new List<object>()) : (IList) Activator.CreateInstance(listType));
      }
      if (list1 == null)
        throw new InvalidOperationException(StringUtils.FormatWith("Cannot create and populate list type {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) listType));
      else
        return list1;
    }

    public static Array ToArray(Array initial, Type type)
    {
      if (type == null)
        throw new ArgumentNullException("type");
      Array instance = Array.CreateInstance(type, initial.Length);
      Array.Copy(initial, 0, instance, 0, initial.Length);
      return instance;
    }

    public static bool AddDistinct<T>(this IList<T> list, T value)
    {
      return CollectionUtils.AddDistinct<T>(list, value, (IEqualityComparer<T>) EqualityComparer<T>.Default);
    }

    public static bool AddDistinct<T>(this IList<T> list, T value, IEqualityComparer<T> comparer)
    {
      if (CollectionUtils.ContainsValue<T>((IEnumerable<T>) list, value, comparer))
        return false;
      list.Add(value);
      return true;
    }

    public static bool ContainsValue<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
    {
      if (comparer == null)
        comparer = (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default;
      if (source == null)
        throw new ArgumentNullException("source");
      foreach (TSource x in source)
      {
        if (comparer.Equals(x, value))
          return true;
      }
      return false;
    }

    public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values, IEqualityComparer<T> comparer)
    {
      bool flag = true;
      foreach (T obj in values)
      {
        if (!CollectionUtils.AddDistinct<T>(list, obj, comparer))
          flag = false;
      }
      return flag;
    }

    public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
    {
      int num = 0;
      foreach (T a in collection)
      {
        if (predicate(a))
          return num;
        ++num;
      }
      return -1;
    }

    public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
    {
      int num = 0;
      foreach (TSource x in list)
      {
        if (comparer.Equals(x, value))
          return num;
        ++num;
      }
      return -1;
    }

    private static IList<int> GetDimensions(IList values)
    {
      IList<int> list1 = (IList<int>) new List<int>();
      IList list2 = values;
      while (true)
      {
        list1.Add(list2.Count);
        if (list2.Count != 0)
        {
          object obj = list2[0];
          if (obj is IList)
            list2 = (IList) obj;
          else
            break;
        }
        else
          break;
      }
      return list1;
    }

    private static void CopyFromJaggedToMultidimensionalArray(IList values, Array multidimensionalArray, int[] indices)
    {
      int length1 = indices.Length;
      if (length1 == multidimensionalArray.Rank)
      {
        multidimensionalArray.SetValue(CollectionUtils.JaggedArrayGetValue(values, indices), indices);
      }
      else
      {
        int length2 = multidimensionalArray.GetLength(length1);
        if (((ICollection) CollectionUtils.JaggedArrayGetValue(values, indices)).Count != length2)
          throw new Exception("Cannot deserialize non-cubical array as multidimensional array.");
        int[] indices1 = new int[length1 + 1];
        for (int index = 0; index < length1; ++index)
          indices1[index] = indices[index];
        for (int index = 0; index < multidimensionalArray.GetLength(length1); ++index)
        {
          indices1[length1] = index;
          CollectionUtils.CopyFromJaggedToMultidimensionalArray(values, multidimensionalArray, indices1);
        }
      }
    }

    private static object JaggedArrayGetValue(IList values, int[] indices)
    {
      IList list = values;
      for (int index1 = 0; index1 < indices.Length; ++index1)
      {
        int index2 = indices[index1];
        if (index1 == indices.Length - 1)
          return list[index2];
        list = (IList) list[index2];
      }
      return (object) list;
    }

    public static Array ToMultidimensionalArray(IList values, Type type, int rank)
    {
      IList<int> dimensions = CollectionUtils.GetDimensions(values);
      while (dimensions.Count < rank)
        dimensions.Add(0);
      Array instance = Array.CreateInstance(type, Enumerable.ToArray<int>((IEnumerable<int>) dimensions));
      CollectionUtils.CopyFromJaggedToMultidimensionalArray(values, instance, new int[0]);
      return instance;
    }
  }
}
