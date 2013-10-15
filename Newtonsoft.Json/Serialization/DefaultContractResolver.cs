// Type: Newtonsoft.Json.Serialization.DefaultContractResolver
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  public class DefaultContractResolver : IContractResolver
  {
    private static readonly IContractResolver _instance = (IContractResolver) new DefaultContractResolver(true);
    private static readonly IList<JsonConverter> BuiltInConverters = (IList<JsonConverter>) new List<JsonConverter>()
    {
      (JsonConverter) new XmlNodeConverter(),
      (JsonConverter) new BinaryConverter(),
      (JsonConverter) new DataSetConverter(),
      (JsonConverter) new DataTableConverter(),
      (JsonConverter) new KeyValuePairConverter(),
      (JsonConverter) new BsonObjectIdConverter()
    };
    private static readonly object _typeContractCacheLock = new object();
    private static Dictionary<ResolverContractKey, JsonContract> _sharedContractCache;
    private Dictionary<ResolverContractKey, JsonContract> _instanceContractCache;
    private readonly bool _sharedCache;

    internal static IContractResolver Instance
    {
      get
      {
        return DefaultContractResolver._instance;
      }
    }

    public bool DynamicCodeGeneration
    {
      get
      {
        return JsonTypeReflector.DynamicCodeGeneration;
      }
    }

    public BindingFlags DefaultMembersSearchFlags { get; set; }

    public bool SerializeCompilerGeneratedMembers { get; set; }

    public bool IgnoreSerializableInterface { get; set; }

    public bool IgnoreSerializableAttribute { get; set; }

    static DefaultContractResolver()
    {
    }

    public DefaultContractResolver()
      : this(false)
    {
    }

    public DefaultContractResolver(bool shareCache)
    {
      this.DefaultMembersSearchFlags = BindingFlags.Instance | BindingFlags.Public;
      this.IgnoreSerializableAttribute = true;
      this._sharedCache = shareCache;
    }

    private Dictionary<ResolverContractKey, JsonContract> GetCache()
    {
      if (this._sharedCache)
        return DefaultContractResolver._sharedContractCache;
      else
        return this._instanceContractCache;
    }

    private void UpdateCache(Dictionary<ResolverContractKey, JsonContract> cache)
    {
      if (this._sharedCache)
        DefaultContractResolver._sharedContractCache = cache;
      else
        this._instanceContractCache = cache;
    }

    public virtual JsonContract ResolveContract(Type type)
    {
      if (type == null)
        throw new ArgumentNullException("type");
      ResolverContractKey key = new ResolverContractKey(this.GetType(), type);
      Dictionary<ResolverContractKey, JsonContract> cache = this.GetCache();
      JsonContract contract;
      if (cache == null || !cache.TryGetValue(key, out contract))
      {
        contract = this.CreateContract(type);
        lock (DefaultContractResolver._typeContractCacheLock)
        {
          Dictionary<ResolverContractKey, JsonContract> local_2_1 = this.GetCache();
          Dictionary<ResolverContractKey, JsonContract> local_3 = local_2_1 != null ? new Dictionary<ResolverContractKey, JsonContract>((IDictionary<ResolverContractKey, JsonContract>) local_2_1) : new Dictionary<ResolverContractKey, JsonContract>();
          local_3[key] = contract;
          this.UpdateCache(local_3);
        }
      }
      return contract;
    }

    protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
    {
      bool serializableAttribute = this.IgnoreSerializableAttribute;
      MemberSerialization memberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType, serializableAttribute);
      List<MemberInfo> list1 = Enumerable.ToList<MemberInfo>(Enumerable.Where<MemberInfo>((IEnumerable<MemberInfo>) ReflectionUtils.GetFieldsAndProperties(objectType, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), (Func<MemberInfo, bool>) (m => !ReflectionUtils.IsIndexedProperty(m))));
      List<MemberInfo> list2 = new List<MemberInfo>();
      if (memberSerialization != MemberSerialization.Fields)
      {
        List<MemberInfo> list3 = Enumerable.ToList<MemberInfo>(Enumerable.Where<MemberInfo>((IEnumerable<MemberInfo>) ReflectionUtils.GetFieldsAndProperties(objectType, this.DefaultMembersSearchFlags), (Func<MemberInfo, bool>) (m => !ReflectionUtils.IsIndexedProperty(m))));
        foreach (MemberInfo memberInfo in list1)
        {
          if (this.SerializeCompilerGeneratedMembers || !memberInfo.IsDefined(typeof (CompilerGeneratedAttribute), true))
          {
            if (list3.Contains(memberInfo))
              list2.Add(memberInfo);
            else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(ReflectionUtils.GetCustomAttributeProvider((object) memberInfo)) != null)
              list2.Add(memberInfo);
            else if (memberSerialization == MemberSerialization.Fields && TypeExtensions.MemberType(memberInfo) == MemberTypes.Field)
              list2.Add(memberInfo);
          }
        }
      }
      else
      {
        foreach (MemberInfo memberInfo in list1)
        {
          if (TypeExtensions.MemberType(memberInfo) == MemberTypes.Field)
            list2.Add(memberInfo);
        }
      }
      return list2;
    }

    protected virtual JsonObjectContract CreateObjectContract(Type objectType)
    {
      JsonObjectContract jsonObjectContract = new JsonObjectContract(objectType);
      this.InitializeContract((JsonContract) jsonObjectContract);
      bool serializableAttribute = this.IgnoreSerializableAttribute;
      jsonObjectContract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(jsonObjectContract.NonNullableUnderlyingType, serializableAttribute);
      CollectionUtils.AddRange<JsonProperty>((IList<JsonProperty>) jsonObjectContract.Properties, (IEnumerable<JsonProperty>) this.CreateProperties(jsonObjectContract.NonNullableUnderlyingType, jsonObjectContract.MemberSerialization));
      JsonObjectAttribute jsonObjectAttribute = JsonTypeReflector.GetJsonObjectAttribute(jsonObjectContract.NonNullableUnderlyingType);
      if (jsonObjectAttribute != null)
        jsonObjectContract.ItemRequired = jsonObjectAttribute._itemRequired;
      if (Enumerable.Any<ConstructorInfo>((IEnumerable<ConstructorInfo>) jsonObjectContract.NonNullableUnderlyingType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (Func<ConstructorInfo, bool>) (c => c.IsDefined(typeof (JsonConstructorAttribute), true))))
      {
        ConstructorInfo attributeConstructor = this.GetAttributeConstructor(jsonObjectContract.NonNullableUnderlyingType);
        if (attributeConstructor != null)
        {
          jsonObjectContract.OverrideConstructor = attributeConstructor;
          CollectionUtils.AddRange<JsonProperty>((IList<JsonProperty>) jsonObjectContract.ConstructorParameters, (IEnumerable<JsonProperty>) this.CreateConstructorParameters(attributeConstructor, jsonObjectContract.Properties));
        }
      }
      else if (jsonObjectContract.MemberSerialization == MemberSerialization.Fields)
      {
        if (JsonTypeReflector.FullyTrusted)
          jsonObjectContract.DefaultCreator = new Func<object>(jsonObjectContract.GetUninitializedObject);
      }
      else if (jsonObjectContract.DefaultCreator == null || jsonObjectContract.DefaultCreatorNonPublic)
      {
        ConstructorInfo parametrizedConstructor = this.GetParametrizedConstructor(jsonObjectContract.NonNullableUnderlyingType);
        if (parametrizedConstructor != null)
        {
          jsonObjectContract.ParametrizedConstructor = parametrizedConstructor;
          CollectionUtils.AddRange<JsonProperty>((IList<JsonProperty>) jsonObjectContract.ConstructorParameters, (IEnumerable<JsonProperty>) this.CreateConstructorParameters(parametrizedConstructor, jsonObjectContract.Properties));
        }
      }
      return jsonObjectContract;
    }

    private ConstructorInfo GetAttributeConstructor(Type objectType)
    {
      IList<ConstructorInfo> list = (IList<ConstructorInfo>) Enumerable.ToList<ConstructorInfo>(Enumerable.Where<ConstructorInfo>((IEnumerable<ConstructorInfo>) objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (Func<ConstructorInfo, bool>) (c => c.IsDefined(typeof (JsonConstructorAttribute), true))));
      if (list.Count > 1)
        throw new JsonException("Multiple constructors with the JsonConstructorAttribute.");
      if (list.Count == 1)
        return list[0];
      else
        return (ConstructorInfo) null;
    }

    private ConstructorInfo GetParametrizedConstructor(Type objectType)
    {
      IList<ConstructorInfo> list = (IList<ConstructorInfo>) Enumerable.ToList<ConstructorInfo>((IEnumerable<ConstructorInfo>) objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public));
      if (list.Count == 1)
        return list[0];
      else
        return (ConstructorInfo) null;
    }

    protected virtual IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
    {
      ParameterInfo[] parameters = constructor.GetParameters();
      JsonPropertyCollection propertyCollection = new JsonPropertyCollection(constructor.DeclaringType);
      foreach (ParameterInfo parameterInfo in parameters)
      {
        JsonProperty matchingMemberProperty = parameterInfo.Name != null ? memberProperties.GetClosestMatchProperty(parameterInfo.Name) : (JsonProperty) null;
        if (matchingMemberProperty != null && matchingMemberProperty.PropertyType != parameterInfo.ParameterType)
          matchingMemberProperty = (JsonProperty) null;
        JsonProperty constructorParameter = this.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);
        if (constructorParameter != null)
          propertyCollection.AddProperty(constructorParameter);
      }
      return (IList<JsonProperty>) propertyCollection;
    }

    protected virtual JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
    {
      JsonProperty property = new JsonProperty();
      property.PropertyType = parameterInfo.ParameterType;
      bool allowNonPublicAccess;
      this.SetPropertySettingsFromAttributes(property, ReflectionUtils.GetCustomAttributeProvider((object) parameterInfo), parameterInfo.Name, parameterInfo.Member.DeclaringType, MemberSerialization.OptOut, out allowNonPublicAccess);
      property.Readable = false;
      property.Writable = true;
      if (matchingMemberProperty != null)
      {
        property.PropertyName = property.PropertyName != parameterInfo.Name ? property.PropertyName : matchingMemberProperty.PropertyName;
        property.Converter = property.Converter ?? matchingMemberProperty.Converter;
        property.MemberConverter = property.MemberConverter ?? matchingMemberProperty.MemberConverter;
        if (!property._hasExplicitDefaultValue && matchingMemberProperty._hasExplicitDefaultValue)
          property.DefaultValue = matchingMemberProperty.DefaultValue;
        JsonProperty jsonProperty1 = property;
        Required? nullable1 = property._required;
        Required? nullable2 = nullable1.HasValue ? new Required?(nullable1.GetValueOrDefault()) : matchingMemberProperty._required;
        jsonProperty1._required = nullable2;
        JsonProperty jsonProperty2 = property;
        bool? isReference = property.IsReference;
        bool? nullable3 = isReference.HasValue ? new bool?(isReference.GetValueOrDefault()) : matchingMemberProperty.IsReference;
        jsonProperty2.IsReference = nullable3;
        JsonProperty jsonProperty3 = property;
        NullValueHandling? nullValueHandling = property.NullValueHandling;
        NullValueHandling? nullable4 = nullValueHandling.HasValue ? new NullValueHandling?(nullValueHandling.GetValueOrDefault()) : matchingMemberProperty.NullValueHandling;
        jsonProperty3.NullValueHandling = nullable4;
        JsonProperty jsonProperty4 = property;
        DefaultValueHandling? defaultValueHandling = property.DefaultValueHandling;
        DefaultValueHandling? nullable5 = defaultValueHandling.HasValue ? new DefaultValueHandling?(defaultValueHandling.GetValueOrDefault()) : matchingMemberProperty.DefaultValueHandling;
        jsonProperty4.DefaultValueHandling = nullable5;
        JsonProperty jsonProperty5 = property;
        ReferenceLoopHandling? referenceLoopHandling = property.ReferenceLoopHandling;
        ReferenceLoopHandling? nullable6 = referenceLoopHandling.HasValue ? new ReferenceLoopHandling?(referenceLoopHandling.GetValueOrDefault()) : matchingMemberProperty.ReferenceLoopHandling;
        jsonProperty5.ReferenceLoopHandling = nullable6;
        JsonProperty jsonProperty6 = property;
        ObjectCreationHandling? creationHandling = property.ObjectCreationHandling;
        ObjectCreationHandling? nullable7 = creationHandling.HasValue ? new ObjectCreationHandling?(creationHandling.GetValueOrDefault()) : matchingMemberProperty.ObjectCreationHandling;
        jsonProperty6.ObjectCreationHandling = nullable7;
        JsonProperty jsonProperty7 = property;
        TypeNameHandling? typeNameHandling = property.TypeNameHandling;
        TypeNameHandling? nullable8 = typeNameHandling.HasValue ? new TypeNameHandling?(typeNameHandling.GetValueOrDefault()) : matchingMemberProperty.TypeNameHandling;
        jsonProperty7.TypeNameHandling = nullable8;
      }
      return property;
    }

    protected virtual JsonConverter ResolveContractConverter(Type objectType)
    {
      return JsonTypeReflector.GetJsonConverter(ReflectionUtils.GetCustomAttributeProvider((object) objectType), objectType);
    }

    private Func<object> GetDefaultCreator(Type createdType)
    {
      return JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);
    }

    private void InitializeContract(JsonContract contract)
    {
      JsonContainerAttribute containerAttribute = JsonTypeReflector.GetJsonContainerAttribute(contract.NonNullableUnderlyingType);
      if (containerAttribute != null)
        contract.IsReference = containerAttribute._isReference;
      contract.Converter = this.ResolveContractConverter(contract.NonNullableUnderlyingType);
      contract.InternalConverter = JsonSerializer.GetMatchingConverter(DefaultContractResolver.BuiltInConverters, contract.NonNullableUnderlyingType);
      if (ReflectionUtils.HasDefaultConstructor(contract.CreatedType, true) || TypeExtensions.IsValueType(contract.CreatedType))
      {
        contract.DefaultCreator = this.GetDefaultCreator(contract.CreatedType);
        contract.DefaultCreatorNonPublic = !TypeExtensions.IsValueType(contract.CreatedType) && ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null;
      }
      this.ResolveCallbackMethods(contract, contract.NonNullableUnderlyingType);
    }

    private void ResolveCallbackMethods(JsonContract contract, Type t)
    {
      if (TypeExtensions.BaseType(t) != null)
        this.ResolveCallbackMethods(contract, TypeExtensions.BaseType(t));
      MethodInfo onSerializing;
      MethodInfo onSerialized;
      MethodInfo onDeserializing;
      MethodInfo onDeserialized;
      MethodInfo onError;
      this.GetCallbackMethodsForType(t, out onSerializing, out onSerialized, out onDeserializing, out onDeserialized, out onError);
      if (onSerializing != null)
        contract.OnSerializing = onSerializing;
      if (onSerialized != null)
        contract.OnSerialized = onSerialized;
      if (onDeserializing != null)
        contract.OnDeserializing = onDeserializing;
      if (onDeserialized != null)
        contract.OnDeserialized = onDeserialized;
      if (onError == null)
        return;
      contract.OnError = onError;
    }

    private void GetCallbackMethodsForType(Type type, out MethodInfo onSerializing, out MethodInfo onSerialized, out MethodInfo onDeserializing, out MethodInfo onDeserialized, out MethodInfo onError)
    {
      onSerializing = (MethodInfo) null;
      onSerialized = (MethodInfo) null;
      onDeserializing = (MethodInfo) null;
      onDeserialized = (MethodInfo) null;
      onError = (MethodInfo) null;
      foreach (MethodInfo method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!method.ContainsGenericParameters)
        {
          Type prevAttributeType = (Type) null;
          ParameterInfo[] parameters = method.GetParameters();
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnSerializingAttribute), onSerializing, ref prevAttributeType))
            onSerializing = method;
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnSerializedAttribute), onSerialized, ref prevAttributeType))
            onSerialized = method;
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnDeserializingAttribute), onDeserializing, ref prevAttributeType))
            onDeserializing = method;
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnDeserializedAttribute), onDeserialized, ref prevAttributeType))
            onDeserialized = method;
          if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnErrorAttribute), onError, ref prevAttributeType))
            onError = method;
        }
      }
    }

    protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
    {
      JsonDictionaryContract dictionaryContract = new JsonDictionaryContract(objectType);
      this.InitializeContract((JsonContract) dictionaryContract);
      dictionaryContract.PropertyNameResolver = new Func<string, string>(this.ResolvePropertyName);
      return dictionaryContract;
    }

    protected virtual JsonArrayContract CreateArrayContract(Type objectType)
    {
      JsonArrayContract jsonArrayContract = new JsonArrayContract(objectType);
      this.InitializeContract((JsonContract) jsonArrayContract);
      return jsonArrayContract;
    }

    protected virtual JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
    {
      JsonPrimitiveContract primitiveContract = new JsonPrimitiveContract(objectType);
      this.InitializeContract((JsonContract) primitiveContract);
      return primitiveContract;
    }

    protected virtual JsonLinqContract CreateLinqContract(Type objectType)
    {
      JsonLinqContract jsonLinqContract = new JsonLinqContract(objectType);
      this.InitializeContract((JsonContract) jsonLinqContract);
      return jsonLinqContract;
    }

    protected virtual JsonISerializableContract CreateISerializableContract(Type objectType)
    {
      JsonISerializableContract iserializableContract = new JsonISerializableContract(objectType);
      this.InitializeContract((JsonContract) iserializableContract);
      ConstructorInfo constructor = iserializableContract.NonNullableUnderlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
      {
        typeof (SerializationInfo),
        typeof (StreamingContext)
      }, (ParameterModifier[]) null);
      if (constructor != null)
      {
        MethodCall<object, object> methodCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) constructor);
        iserializableContract.ISerializableCreator = (ObjectConstructor<object>) (args => methodCall((object) null, args));
      }
      return iserializableContract;
    }

    protected virtual JsonStringContract CreateStringContract(Type objectType)
    {
      JsonStringContract jsonStringContract = new JsonStringContract(objectType);
      this.InitializeContract((JsonContract) jsonStringContract);
      return jsonStringContract;
    }

    protected virtual JsonContract CreateContract(Type objectType)
    {
      Type type = ReflectionUtils.EnsureNotNullableType(objectType);
      if (JsonConvert.IsJsonPrimitiveType(type))
        return (JsonContract) this.CreatePrimitiveContract(objectType);
      if (JsonTypeReflector.GetJsonObjectAttribute(type) != null)
        return (JsonContract) this.CreateObjectContract(objectType);
      if (JsonTypeReflector.GetJsonArrayAttribute(type) != null)
        return (JsonContract) this.CreateArrayContract(objectType);
      if (JsonTypeReflector.GetJsonDictionaryAttribute(type) != null)
        return (JsonContract) this.CreateDictionaryContract(objectType);
      if (type == typeof (JToken) || type.IsSubclassOf(typeof (JToken)))
        return (JsonContract) this.CreateLinqContract(objectType);
      if (CollectionUtils.IsDictionaryType(type))
        return (JsonContract) this.CreateDictionaryContract(objectType);
      if (typeof (IEnumerable).IsAssignableFrom(type))
        return (JsonContract) this.CreateArrayContract(objectType);
      if (DefaultContractResolver.CanConvertToString(type))
        return (JsonContract) this.CreateStringContract(objectType);
      if (!this.IgnoreSerializableInterface && typeof (ISerializable).IsAssignableFrom(type))
        return (JsonContract) this.CreateISerializableContract(objectType);
      else
        return (JsonContract) this.CreateObjectContract(objectType);
    }

    internal static bool CanConvertToString(Type type)
    {
      TypeConverter converter = ConvertUtils.GetConverter(type);
      return converter != null && !(converter is ComponentConverter) && (!(converter is ReferenceConverter) && converter.GetType() != typeof (TypeConverter)) && converter.CanConvertTo(typeof (string)) || (type == typeof (Type) || type.IsSubclassOf(typeof (Type)));
    }

    private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
    {
      if (!method.IsDefined(attributeType, false))
        return false;
      if (currentCallback != null)
        throw new JsonException(StringUtils.FormatWith("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) method, (object) currentCallback, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) attributeType));
      else if (prevAttributeType != null)
      {
        throw new JsonException(StringUtils.FormatWith("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) prevAttributeType, (object) attributeType, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method));
      }
      else
      {
        if (method.IsVirtual)
          throw new JsonException(StringUtils.FormatWith("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.", (IFormatProvider) CultureInfo.InvariantCulture, (object) method, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) attributeType));
        if (method.ReturnType != typeof (void))
          throw new JsonException(StringUtils.FormatWith("Serialization Callback '{1}' in type '{0}' must return void.", (IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method));
        if (attributeType == typeof (OnErrorAttribute))
        {
          if (parameters == null || parameters.Length != 2 || (parameters[0].ParameterType != typeof (StreamingContext) || parameters[1].ParameterType != typeof (ErrorContext)))
            throw new JsonException(StringUtils.FormatWith("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method, (object) typeof (StreamingContext), (object) typeof (ErrorContext)));
        }
        else if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != typeof (StreamingContext))
          throw new JsonException(StringUtils.FormatWith("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method, (object) typeof (StreamingContext)));
        prevAttributeType = attributeType;
        return true;
      }
    }

    internal static string GetClrTypeFullName(Type type)
    {
      if (TypeExtensions.IsGenericTypeDefinition(type) || !TypeExtensions.ContainsGenericParameters(type))
        return type.FullName;
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}.{1}", new object[2]
      {
        (object) type.Namespace,
        (object) type.Name
      });
    }

    protected virtual IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
      List<MemberInfo> serializableMembers = this.GetSerializableMembers(type);
      if (serializableMembers == null)
        throw new JsonSerializationException("Null collection of seralizable members returned.");
      JsonPropertyCollection propertyCollection = new JsonPropertyCollection(type);
      foreach (MemberInfo member in serializableMembers)
      {
        JsonProperty property = this.CreateProperty(member, memberSerialization);
        if (property != null)
          propertyCollection.AddProperty(property);
      }
      return (IList<JsonProperty>) Enumerable.ToList<JsonProperty>((IEnumerable<JsonProperty>) Enumerable.OrderBy<JsonProperty, int>((IEnumerable<JsonProperty>) propertyCollection, (Func<JsonProperty, int>) (p => p.Order ?? -1)));
    }

    protected virtual IValueProvider CreateMemberValueProvider(MemberInfo member)
    {
      return !this.DynamicCodeGeneration ? (IValueProvider) new ReflectionValueProvider(member) : (IValueProvider) new DynamicValueProvider(member);
    }

    protected virtual JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      JsonProperty property = new JsonProperty();
      property.PropertyType = ReflectionUtils.GetMemberUnderlyingType(member);
      property.DeclaringType = member.DeclaringType;
      property.ValueProvider = this.CreateMemberValueProvider(member);
      bool allowNonPublicAccess;
      this.SetPropertySettingsFromAttributes(property, ReflectionUtils.GetCustomAttributeProvider((object) member), member.Name, member.DeclaringType, memberSerialization, out allowNonPublicAccess);
      if (memberSerialization != MemberSerialization.Fields)
      {
        property.Readable = ReflectionUtils.CanReadMemberValue(member, allowNonPublicAccess);
        property.Writable = ReflectionUtils.CanSetMemberValue(member, allowNonPublicAccess, property.HasMemberAttribute);
      }
      else
      {
        property.Readable = true;
        property.Writable = true;
      }
      property.ShouldSerialize = this.CreateShouldSerializeTest(member);
      this.SetIsSpecifiedActions(property, member, allowNonPublicAccess);
      return property;
    }

    private void SetPropertySettingsFromAttributes(JsonProperty property, ICustomAttributeProvider attributeProvider, string name, Type declaringType, MemberSerialization memberSerialization, out bool allowNonPublicAccess)
    {
      JsonPropertyAttribute attribute1 = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(attributeProvider);
      if (attribute1 != null)
        property.HasMemberAttribute = true;
      string propertyName = attribute1 == null || attribute1.PropertyName == null ? name : attribute1.PropertyName;
      property.PropertyName = this.ResolvePropertyName(propertyName);
      property.UnderlyingName = name;
      bool flag1 = false;
      if (attribute1 != null)
      {
        property._required = attribute1._required;
        property.Order = attribute1._order;
        property.DefaultValueHandling = attribute1._defaultValueHandling;
        flag1 = true;
      }
      bool flag2 = JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(attributeProvider) != null || JsonTypeReflector.GetAttribute<NonSerializedAttribute>(attributeProvider) != null;
      if (memberSerialization != MemberSerialization.OptIn)
      {
        bool flag3 = false;
        property.Ignored = flag2 || flag3;
      }
      else
        property.Ignored = flag2 || !flag1;
      property.Converter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
      property.MemberConverter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
      DefaultValueAttribute attribute2 = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(attributeProvider);
      if (attribute2 != null)
        property.DefaultValue = attribute2.Value;
      property.NullValueHandling = attribute1 != null ? attribute1._nullValueHandling : new NullValueHandling?();
      property.ReferenceLoopHandling = attribute1 != null ? attribute1._referenceLoopHandling : new ReferenceLoopHandling?();
      property.ObjectCreationHandling = attribute1 != null ? attribute1._objectCreationHandling : new ObjectCreationHandling?();
      property.TypeNameHandling = attribute1 != null ? attribute1._typeNameHandling : new TypeNameHandling?();
      property.IsReference = attribute1 != null ? attribute1._isReference : new bool?();
      property.ItemIsReference = attribute1 != null ? attribute1._itemIsReference : new bool?();
      property.ItemConverter = attribute1 == null || attribute1.ItemConverterType == null ? (JsonConverter) null : JsonConverterAttribute.CreateJsonConverterInstance(attribute1.ItemConverterType);
      property.ItemReferenceLoopHandling = attribute1 != null ? attribute1._itemReferenceLoopHandling : new ReferenceLoopHandling?();
      property.ItemTypeNameHandling = attribute1 != null ? attribute1._itemTypeNameHandling : new TypeNameHandling?();
      allowNonPublicAccess = false;
      if ((this.DefaultMembersSearchFlags & BindingFlags.NonPublic) == BindingFlags.NonPublic)
        allowNonPublicAccess = true;
      if (attribute1 != null)
        allowNonPublicAccess = true;
      if (memberSerialization != MemberSerialization.Fields)
        return;
      allowNonPublicAccess = true;
    }

    private Predicate<object> CreateShouldSerializeTest(MemberInfo member)
    {
      MethodInfo method = member.DeclaringType.GetMethod("ShouldSerialize" + member.Name, ReflectionUtils.EmptyTypes);
      if (method == null || method.ReturnType != typeof (bool))
        return (Predicate<object>) null;
      MethodCall<object, object> shouldSerializeCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
      return (Predicate<object>) (o => (bool) shouldSerializeCall(o, new object[0]));
    }

    private void SetIsSpecifiedActions(JsonProperty property, MemberInfo member, bool allowNonPublicAccess)
    {
      MemberInfo memberInfo = (MemberInfo) member.DeclaringType.GetProperty(member.Name + "Specified") ?? (MemberInfo) member.DeclaringType.GetField(member.Name + "Specified");
      if (memberInfo == null || ReflectionUtils.GetMemberUnderlyingType(memberInfo) != typeof (bool))
        return;
      Func<object, object> specifiedPropertyGet = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(memberInfo);
      property.GetIsSpecified = (Predicate<object>) (o => (bool) specifiedPropertyGet(o));
      if (!ReflectionUtils.CanSetMemberValue(memberInfo, allowNonPublicAccess, false))
        return;
      property.SetIsSpecified = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(memberInfo);
    }

    protected internal virtual string ResolvePropertyName(string propertyName)
    {
      return propertyName;
    }

    public string GetResolvedPropertyName(string propertyName)
    {
      return this.ResolvePropertyName(propertyName);
    }
  }
}
