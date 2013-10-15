// Type: Newtonsoft.Json.Serialization.JsonArrayContract
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  public class JsonArrayContract : JsonContainerContract
  {
    private readonly bool _isCollectionItemTypeNullableType;
    private readonly Type _genericCollectionDefinitionType;
    private Type _genericWrapperType;
    private MethodCall<object, object> _genericWrapperCreator;

    public Type CollectionItemType { get; private set; }

    public bool IsMultidimensionalArray { get; private set; }

    public JsonArrayContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Array;
      if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (ICollection<>), out this._genericCollectionDefinitionType))
        this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
      else if (TypeExtensions.IsGenericType(underlyingType) && underlyingType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
      {
        this._genericCollectionDefinitionType = typeof (IEnumerable<>);
        this.CollectionItemType = underlyingType.GetGenericArguments()[0];
      }
      else
        this.CollectionItemType = ReflectionUtils.GetCollectionItemType(this.UnderlyingType);
      if (this.CollectionItemType != null)
        this._isCollectionItemTypeNullableType = ReflectionUtils.IsNullableType(this.CollectionItemType);
      if (this.IsTypeGenericCollectionInterface(this.UnderlyingType))
        this.CreatedType = ReflectionUtils.MakeGenericType(typeof (List<>), new Type[1]
        {
          this.CollectionItemType
        });
      this.IsMultidimensionalArray = this.UnderlyingType.IsArray && this.UnderlyingType.GetArrayRank() > 1;
    }

    internal IWrappedCollection CreateWrapper(object list)
    {
      if (list is IList && (this.CollectionItemType == null || !this._isCollectionItemTypeNullableType) || this.UnderlyingType.IsArray)
        return (IWrappedCollection) new CollectionWrapper<object>((IList) list);
      if (this._genericCollectionDefinitionType != null)
      {
        this.EnsureGenericWrapperCreator();
        return (IWrappedCollection) this._genericWrapperCreator((object) null, new object[1]
        {
          list
        });
      }
      else
      {
        IList list1 = (IList) Enumerable.ToList<object>(Enumerable.Cast<object>((IEnumerable) list));
        if (this.CollectionItemType != null)
        {
          Array instance = Array.CreateInstance(this.CollectionItemType, list1.Count);
          for (int index = 0; index < list1.Count; ++index)
            instance.SetValue(list1[index], index);
          list1 = (IList) instance;
        }
        return (IWrappedCollection) new CollectionWrapper<object>(list1);
      }
    }

    private void EnsureGenericWrapperCreator()
    {
      if (this._genericWrapperCreator != null)
        return;
      this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof (CollectionWrapper<>), new Type[1]
      {
        this.CollectionItemType
      });
      Type type;
      if (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof (List<>)) || this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
        type = ReflectionUtils.MakeGenericType(typeof (ICollection<>), new Type[1]
        {
          this.CollectionItemType
        });
      else
        type = this._genericCollectionDefinitionType;
      this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) this._genericWrapperType.GetConstructor(new Type[1]
      {
        type
      }));
    }

    private bool IsTypeGenericCollectionInterface(Type type)
    {
      if (!TypeExtensions.IsGenericType(type))
        return false;
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      if (genericTypeDefinition != typeof (IList<>) && genericTypeDefinition != typeof (ICollection<>))
        return genericTypeDefinition == typeof (IEnumerable<>);
      else
        return true;
    }
  }
}
