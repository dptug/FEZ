// Type: Newtonsoft.Json.Serialization.JsonDictionaryContract
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  public class JsonDictionaryContract : JsonContainerContract
  {
    private readonly bool _isDictionaryValueTypeNullableType;
    private readonly Type _genericCollectionDefinitionType;
    private Type _genericWrapperType;
    private MethodCall<object, object> _genericWrapperCreator;

    public Func<string, string> PropertyNameResolver { get; set; }

    public Type DictionaryKeyType { get; private set; }

    public Type DictionaryValueType { get; private set; }

    internal JsonContract KeyContract { get; set; }

    public JsonDictionaryContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Dictionary;
      Type keyType;
      Type valueType;
      if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (IDictionary<,>), out this._genericCollectionDefinitionType))
      {
        keyType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
        valueType = this._genericCollectionDefinitionType.GetGenericArguments()[1];
      }
      else
        ReflectionUtils.GetDictionaryKeyValueTypes(this.UnderlyingType, out keyType, out valueType);
      this.DictionaryKeyType = keyType;
      this.DictionaryValueType = valueType;
      if (this.DictionaryValueType != null)
        this._isDictionaryValueTypeNullableType = ReflectionUtils.IsNullableType(this.DictionaryValueType);
      if (this.IsTypeGenericDictionaryInterface(this.UnderlyingType))
      {
        this.CreatedType = ReflectionUtils.MakeGenericType(typeof (Dictionary<,>), keyType, valueType);
      }
      else
      {
        if (this.UnderlyingType != typeof (IDictionary))
          return;
        this.CreatedType = typeof (Dictionary<object, object>);
      }
    }

    internal IWrappedDictionary CreateWrapper(object dictionary)
    {
      if (dictionary is IDictionary && (this.DictionaryValueType == null || !this._isDictionaryValueTypeNullableType))
        return (IWrappedDictionary) new DictionaryWrapper<object, object>((IDictionary) dictionary);
      if (this._genericWrapperCreator == null)
      {
        this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof (DictionaryWrapper<,>), this.DictionaryKeyType, this.DictionaryValueType);
        this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) this._genericWrapperType.GetConstructor(new Type[1]
        {
          this._genericCollectionDefinitionType
        }));
      }
      return (IWrappedDictionary) this._genericWrapperCreator((object) null, new object[1]
      {
        dictionary
      });
    }

    private bool IsTypeGenericDictionaryInterface(Type type)
    {
      if (!TypeExtensions.IsGenericType(type))
        return false;
      else
        return type.GetGenericTypeDefinition() == typeof (IDictionary<,>);
    }
  }
}
