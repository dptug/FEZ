// Type: Newtonsoft.Json.Serialization.JsonTypeReflector
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Security.Permissions;

namespace Newtonsoft.Json.Serialization
{
  internal static class JsonTypeReflector
  {
    private static readonly ThreadSafeStore<ICustomAttributeProvider, Type> JsonConverterTypeCache = new ThreadSafeStore<ICustomAttributeProvider, Type>(new Func<ICustomAttributeProvider, Type>(JsonTypeReflector.GetJsonConverterTypeFromAttribute));
    public const string IdPropertyName = "$id";
    public const string RefPropertyName = "$ref";
    public const string TypePropertyName = "$type";
    public const string ValuePropertyName = "$value";
    public const string ArrayValuesPropertyName = "$values";
    public const string ShouldSerializePrefix = "ShouldSerialize";
    public const string SpecifiedPostfix = "Specified";
    private static bool? _dynamicCodeGeneration;
    private static bool? _fullyTrusted;

    public static bool DynamicCodeGeneration
    {
      get
      {
        if (!JsonTypeReflector._dynamicCodeGeneration.HasValue)
        {
          try
          {
            new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
            new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess).Demand();
            new SecurityPermission(SecurityPermissionFlag.SkipVerification).Demand();
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
            new SecurityPermission(PermissionState.Unrestricted).Demand();
            JsonTypeReflector._dynamicCodeGeneration = new bool?(true);
          }
          catch (Exception ex)
          {
            JsonTypeReflector._dynamicCodeGeneration = new bool?(false);
          }
        }
        return JsonTypeReflector._dynamicCodeGeneration.Value;
      }
    }

    public static bool FullyTrusted
    {
      get
      {
        if (!JsonTypeReflector._fullyTrusted.HasValue)
        {
          try
          {
            new SecurityPermission(PermissionState.Unrestricted).Demand();
            JsonTypeReflector._fullyTrusted = new bool?(true);
          }
          catch (Exception ex)
          {
            JsonTypeReflector._fullyTrusted = new bool?(false);
          }
        }
        return JsonTypeReflector._fullyTrusted.Value;
      }
    }

    public static ReflectionDelegateFactory ReflectionDelegateFactory
    {
      get
      {
        if (JsonTypeReflector.DynamicCodeGeneration)
          return (ReflectionDelegateFactory) DynamicReflectionDelegateFactory.Instance;
        else
          return LateBoundReflectionDelegateFactory.Instance;
      }
    }

    static JsonTypeReflector()
    {
    }

    public static JsonContainerAttribute GetJsonContainerAttribute(Type type)
    {
      return CachedAttributeGetter<JsonContainerAttribute>.GetAttribute(ReflectionUtils.GetCustomAttributeProvider((object) type));
    }

    public static JsonObjectAttribute GetJsonObjectAttribute(Type type)
    {
      return JsonTypeReflector.GetJsonContainerAttribute(type) as JsonObjectAttribute;
    }

    public static JsonArrayAttribute GetJsonArrayAttribute(Type type)
    {
      return JsonTypeReflector.GetJsonContainerAttribute(type) as JsonArrayAttribute;
    }

    public static JsonDictionaryAttribute GetJsonDictionaryAttribute(Type type)
    {
      return JsonTypeReflector.GetJsonContainerAttribute(type) as JsonDictionaryAttribute;
    }

    public static SerializableAttribute GetSerializableAttribute(Type type)
    {
      return CachedAttributeGetter<SerializableAttribute>.GetAttribute(ReflectionUtils.GetCustomAttributeProvider((object) type));
    }

    public static MemberSerialization GetObjectMemberSerialization(Type objectType, bool ignoreSerializableAttribute)
    {
      JsonObjectAttribute jsonObjectAttribute = JsonTypeReflector.GetJsonObjectAttribute(objectType);
      if (jsonObjectAttribute != null)
        return jsonObjectAttribute.MemberSerialization;
      return !ignoreSerializableAttribute && JsonTypeReflector.GetSerializableAttribute(objectType) != null ? MemberSerialization.Fields : MemberSerialization.OptOut;
    }

    private static Type GetJsonConverterType(ICustomAttributeProvider attributeProvider)
    {
      return JsonTypeReflector.JsonConverterTypeCache.Get(attributeProvider);
    }

    private static Type GetJsonConverterTypeFromAttribute(ICustomAttributeProvider attributeProvider)
    {
      JsonConverterAttribute attribute = JsonTypeReflector.GetAttribute<JsonConverterAttribute>(attributeProvider);
      if (attribute == null)
        return (Type) null;
      else
        return attribute.ConverterType;
    }

    public static JsonConverter GetJsonConverter(ICustomAttributeProvider attributeProvider, Type targetConvertedType)
    {
      Type jsonConverterType = JsonTypeReflector.GetJsonConverterType(attributeProvider);
      if (jsonConverterType != null)
        return JsonConverterAttribute.CreateJsonConverterInstance(jsonConverterType);
      else
        return (JsonConverter) null;
    }

    public static TypeConverter GetTypeConverter(Type type)
    {
      return TypeDescriptor.GetConverter(type);
    }

    private static T GetAttribute<T>(Type type) where T : Attribute
    {
      T attribute1 = ReflectionUtils.GetAttribute<T>(ReflectionUtils.GetCustomAttributeProvider((object) type), true);
      if ((object) attribute1 != null)
        return attribute1;
      foreach (object o in type.GetInterfaces())
      {
        T attribute2 = ReflectionUtils.GetAttribute<T>(ReflectionUtils.GetCustomAttributeProvider(o), true);
        if ((object) attribute2 != null)
          return attribute2;
      }
      return default (T);
    }

    private static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
    {
      T attribute1 = ReflectionUtils.GetAttribute<T>(ReflectionUtils.GetCustomAttributeProvider((object) memberInfo), true);
      if ((object) attribute1 != null)
        return attribute1;
      if (memberInfo.DeclaringType != null)
      {
        foreach (Type targetType in memberInfo.DeclaringType.GetInterfaces())
        {
          MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(targetType, memberInfo);
          if (memberInfoFromType != null)
          {
            T attribute2 = ReflectionUtils.GetAttribute<T>(ReflectionUtils.GetCustomAttributeProvider((object) memberInfoFromType), true);
            if ((object) attribute2 != null)
              return attribute2;
          }
        }
      }
      return default (T);
    }

    public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
    {
      object obj = (object) attributeProvider;
      Type type = obj as Type;
      if (type != null)
        return JsonTypeReflector.GetAttribute<T>(type);
      MemberInfo memberInfo = obj as MemberInfo;
      if (memberInfo != null)
        return JsonTypeReflector.GetAttribute<T>(memberInfo);
      else
        return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
    }
  }
}
