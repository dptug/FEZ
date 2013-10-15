// Type: Newtonsoft.Json.Utilities.ReflectionUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
  internal static class ReflectionUtils
  {
    public static readonly Type[] EmptyTypes = Type.EmptyTypes;

    static ReflectionUtils()
    {
    }

    public static ICustomAttributeProvider GetCustomAttributeProvider(this object o)
    {
      return (ICustomAttributeProvider) o;
    }

    public static bool IsVirtual(this PropertyInfo propertyInfo)
    {
      ValidationUtils.ArgumentNotNull((object) propertyInfo, "propertyInfo");
      MethodInfo getMethod = propertyInfo.GetGetMethod();
      if (getMethod != null && getMethod.IsVirtual)
        return true;
      MethodInfo setMethod = propertyInfo.GetSetMethod();
      return setMethod != null && setMethod.IsVirtual;
    }

    public static Type GetObjectType(object v)
    {
      if (v == null)
        return (Type) null;
      else
        return v.GetType();
    }

    public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat)
    {
      return ReflectionUtils.GetTypeName(t, assemblyFormat, (SerializationBinder) null);
    }

    public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat, SerializationBinder binder)
    {
      string assemblyQualifiedName = t.AssemblyQualifiedName;
      switch (assemblyFormat)
      {
        case FormatterAssemblyStyle.Simple:
          return ReflectionUtils.RemoveAssemblyDetails(assemblyQualifiedName);
        case FormatterAssemblyStyle.Full:
          return assemblyQualifiedName;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < fullyQualifiedTypeName.Length; ++index)
      {
        char ch = fullyQualifiedTypeName[index];
        switch (ch)
        {
          case ',':
            if (!flag1)
            {
              flag1 = true;
              stringBuilder.Append(ch);
              break;
            }
            else
            {
              flag2 = true;
              break;
            }
          case '[':
            flag1 = false;
            flag2 = false;
            stringBuilder.Append(ch);
            break;
          case ']':
            flag1 = false;
            flag2 = false;
            stringBuilder.Append(ch);
            break;
          default:
            if (!flag2)
            {
              stringBuilder.Append(ch);
              break;
            }
            else
              break;
        }
      }
      return ((object) stringBuilder).ToString();
    }

    public static bool IsInstantiatableType(Type t)
    {
      ValidationUtils.ArgumentNotNull((object) t, "t");
      return !TypeExtensions.IsAbstract(t) && !TypeExtensions.IsInterface(t) && (!t.IsArray && !TypeExtensions.IsGenericTypeDefinition(t)) && (t != typeof (void) && ReflectionUtils.HasDefaultConstructor(t));
    }

    public static bool HasDefaultConstructor(Type t)
    {
      return ReflectionUtils.HasDefaultConstructor(t, false);
    }

    public static bool HasDefaultConstructor(Type t, bool nonPublic)
    {
      ValidationUtils.ArgumentNotNull((object) t, "t");
      if (TypeExtensions.IsValueType(t))
        return true;
      else
        return ReflectionUtils.GetDefaultConstructor(t, nonPublic) != null;
    }

    public static ConstructorInfo GetDefaultConstructor(Type t)
    {
      return ReflectionUtils.GetDefaultConstructor(t, false);
    }

    public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
    {
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public;
      if (nonPublic)
        bindingAttr |= BindingFlags.NonPublic;
      return Enumerable.SingleOrDefault<ConstructorInfo>((IEnumerable<ConstructorInfo>) t.GetConstructors(bindingAttr), (Func<ConstructorInfo, bool>) (c => !Enumerable.Any<ParameterInfo>((IEnumerable<ParameterInfo>) c.GetParameters())));
    }

    public static bool IsNullable(Type t)
    {
      ValidationUtils.ArgumentNotNull((object) t, "t");
      if (TypeExtensions.IsValueType(t))
        return ReflectionUtils.IsNullableType(t);
      else
        return true;
    }

    public static bool IsNullableType(Type t)
    {
      ValidationUtils.ArgumentNotNull((object) t, "t");
      if (TypeExtensions.IsGenericType(t))
        return t.GetGenericTypeDefinition() == typeof (Nullable<>);
      else
        return false;
    }

    public static Type EnsureNotNullableType(Type t)
    {
      if (!ReflectionUtils.IsNullableType(t))
        return t;
      else
        return Nullable.GetUnderlyingType(t);
    }

    public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
    {
      Type implementingType;
      return ReflectionUtils.ImplementsGenericDefinition(type, genericInterfaceDefinition, out implementingType);
    }

    public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
    {
      ValidationUtils.ArgumentNotNull((object) type, "type");
      ValidationUtils.ArgumentNotNull((object) genericInterfaceDefinition, "genericInterfaceDefinition");
      if (!TypeExtensions.IsInterface(genericInterfaceDefinition) || !TypeExtensions.IsGenericTypeDefinition(genericInterfaceDefinition))
        throw new ArgumentNullException(StringUtils.FormatWith("'{0}' is not a generic interface definition.", (IFormatProvider) CultureInfo.InvariantCulture, (object) genericInterfaceDefinition));
      if (TypeExtensions.IsInterface(type) && TypeExtensions.IsGenericType(type))
      {
        Type genericTypeDefinition = type.GetGenericTypeDefinition();
        if (genericInterfaceDefinition == genericTypeDefinition)
        {
          implementingType = type;
          return true;
        }
      }
      foreach (Type type1 in type.GetInterfaces())
      {
        if (TypeExtensions.IsGenericType(type1))
        {
          Type genericTypeDefinition = type1.GetGenericTypeDefinition();
          if (genericInterfaceDefinition == genericTypeDefinition)
          {
            implementingType = type1;
            return true;
          }
        }
      }
      implementingType = (Type) null;
      return false;
    }

    public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
    {
      Type implementingType;
      return ReflectionUtils.InheritsGenericDefinition(type, genericClassDefinition, out implementingType);
    }

    public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
    {
      ValidationUtils.ArgumentNotNull((object) type, "type");
      ValidationUtils.ArgumentNotNull((object) genericClassDefinition, "genericClassDefinition");
      if (!TypeExtensions.IsClass(genericClassDefinition) || !TypeExtensions.IsGenericTypeDefinition(genericClassDefinition))
        throw new ArgumentNullException(StringUtils.FormatWith("'{0}' is not a generic class definition.", (IFormatProvider) CultureInfo.InvariantCulture, (object) genericClassDefinition));
      else
        return ReflectionUtils.InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
    }

    private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, out Type implementingType)
    {
      if (TypeExtensions.IsGenericType(currentType))
      {
        Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
        if (genericClassDefinition == genericTypeDefinition)
        {
          implementingType = currentType;
          return true;
        }
      }
      if (TypeExtensions.BaseType(currentType) != null)
        return ReflectionUtils.InheritsGenericDefinitionInternal(TypeExtensions.BaseType(currentType), genericClassDefinition, out implementingType);
      implementingType = (Type) null;
      return false;
    }

    public static Type GetCollectionItemType(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, "type");
      if (type.IsArray)
        return type.GetElementType();
      Type implementingType;
      if (ReflectionUtils.ImplementsGenericDefinition(type, typeof (IEnumerable<>), out implementingType))
      {
        if (TypeExtensions.IsGenericTypeDefinition(implementingType))
          throw new Exception(StringUtils.FormatWith("Type {0} is not a collection.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type));
        else
          return implementingType.GetGenericArguments()[0];
      }
      else if (typeof (IEnumerable).IsAssignableFrom(type))
        return (Type) null;
      else
        throw new Exception(StringUtils.FormatWith("Type {0} is not a collection.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type));
    }

    public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
    {
      ValidationUtils.ArgumentNotNull((object) dictionaryType, "type");
      Type implementingType;
      if (ReflectionUtils.ImplementsGenericDefinition(dictionaryType, typeof (IDictionary<,>), out implementingType))
      {
        if (TypeExtensions.IsGenericTypeDefinition(implementingType))
          throw new Exception(StringUtils.FormatWith("Type {0} is not a dictionary.", (IFormatProvider) CultureInfo.InvariantCulture, (object) dictionaryType));
        Type[] genericArguments = implementingType.GetGenericArguments();
        keyType = genericArguments[0];
        valueType = genericArguments[1];
      }
      else
      {
        if (!typeof (IDictionary).IsAssignableFrom(dictionaryType))
          throw new Exception(StringUtils.FormatWith("Type {0} is not a dictionary.", (IFormatProvider) CultureInfo.InvariantCulture, (object) dictionaryType));
        keyType = (Type) null;
        valueType = (Type) null;
      }
    }

    public static Type GetDictionaryValueType(Type dictionaryType)
    {
      Type keyType;
      Type valueType;
      ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);
      return valueType;
    }

    public static Type GetDictionaryKeyType(Type dictionaryType)
    {
      Type keyType;
      Type valueType;
      ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);
      return keyType;
    }

    public static Type GetMemberUnderlyingType(MemberInfo member)
    {
      ValidationUtils.ArgumentNotNull((object) member, "member");
      switch (TypeExtensions.MemberType(member))
      {
        case MemberTypes.Event:
          return ((EventInfo) member).EventHandlerType;
        case MemberTypes.Field:
          return ((FieldInfo) member).FieldType;
        case MemberTypes.Property:
          return ((PropertyInfo) member).PropertyType;
        default:
          throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or EventInfo", "member");
      }
    }

    public static bool IsIndexedProperty(MemberInfo member)
    {
      ValidationUtils.ArgumentNotNull((object) member, "member");
      PropertyInfo property = member as PropertyInfo;
      if (property != null)
        return ReflectionUtils.IsIndexedProperty(property);
      else
        return false;
    }

    public static bool IsIndexedProperty(PropertyInfo property)
    {
      ValidationUtils.ArgumentNotNull((object) property, "property");
      return property.GetIndexParameters().Length > 0;
    }

    public static object GetMemberValue(MemberInfo member, object target)
    {
      ValidationUtils.ArgumentNotNull((object) member, "member");
      ValidationUtils.ArgumentNotNull(target, "target");
      switch (TypeExtensions.MemberType(member))
      {
        case MemberTypes.Field:
          return ((FieldInfo) member).GetValue(target);
        case MemberTypes.Property:
          try
          {
            return ((PropertyInfo) member).GetValue(target, (object[]) null);
          }
          catch (TargetParameterCountException ex)
          {
            throw new ArgumentException(StringUtils.FormatWith("MemberInfo '{0}' has index parameters", (IFormatProvider) CultureInfo.InvariantCulture, (object) member.Name), (Exception) ex);
          }
        default:
          throw new ArgumentException(StringUtils.FormatWith("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo", (IFormatProvider) CultureInfo.InvariantCulture, (object) CultureInfo.InvariantCulture, (object) member.Name), "member");
      }
    }

    public static void SetMemberValue(MemberInfo member, object target, object value)
    {
      ValidationUtils.ArgumentNotNull((object) member, "member");
      ValidationUtils.ArgumentNotNull(target, "target");
      switch (TypeExtensions.MemberType(member))
      {
        case MemberTypes.Field:
          ((FieldInfo) member).SetValue(target, value);
          break;
        case MemberTypes.Property:
          ((PropertyInfo) member).SetValue(target, value, (object[]) null);
          break;
        default:
          throw new ArgumentException(StringUtils.FormatWith("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo", (IFormatProvider) CultureInfo.InvariantCulture, (object) member.Name), "member");
      }
    }

    public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
    {
      switch (TypeExtensions.MemberType(member))
      {
        case MemberTypes.Field:
          FieldInfo fieldInfo = (FieldInfo) member;
          return nonPublic || fieldInfo.IsPublic;
        case MemberTypes.Property:
          PropertyInfo propertyInfo = (PropertyInfo) member;
          if (!propertyInfo.CanRead)
            return false;
          if (nonPublic)
            return true;
          else
            return propertyInfo.GetGetMethod(nonPublic) != null;
        default:
          return false;
      }
    }

    public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
    {
      switch (TypeExtensions.MemberType(member))
      {
        case MemberTypes.Field:
          FieldInfo fieldInfo = (FieldInfo) member;
          return (!fieldInfo.IsInitOnly || canSetReadOnly) && (nonPublic || fieldInfo.IsPublic);
        case MemberTypes.Property:
          PropertyInfo propertyInfo = (PropertyInfo) member;
          if (!propertyInfo.CanWrite)
            return false;
          if (nonPublic)
            return true;
          else
            return propertyInfo.GetSetMethod(nonPublic) != null;
        default:
          return false;
      }
    }

    public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
    {
      List<MemberInfo> list1 = new List<MemberInfo>();
      CollectionUtils.AddRange((IList) list1, (IEnumerable) ReflectionUtils.GetFields(type, bindingAttr));
      CollectionUtils.AddRange((IList) list1, (IEnumerable) ReflectionUtils.GetProperties(type, bindingAttr));
      List<MemberInfo> list2 = new List<MemberInfo>(list1.Count);
      foreach (IGrouping<string, MemberInfo> grouping in Enumerable.GroupBy<MemberInfo, string>((IEnumerable<MemberInfo>) list1, (Func<MemberInfo, string>) (m => m.Name)))
      {
        int num = Enumerable.Count<MemberInfo>((IEnumerable<MemberInfo>) grouping);
        IList<MemberInfo> list3 = (IList<MemberInfo>) Enumerable.ToList<MemberInfo>((IEnumerable<MemberInfo>) grouping);
        if (num == 1)
        {
          list2.Add(Enumerable.First<MemberInfo>((IEnumerable<MemberInfo>) list3));
        }
        else
        {
          IEnumerable<MemberInfo> collection = Enumerable.Where<MemberInfo>((IEnumerable<MemberInfo>) list3, (Func<MemberInfo, bool>) (m =>
          {
            if (ReflectionUtils.IsOverridenGenericMember(m, bindingAttr))
              return m.Name == "Item";
            else
              return true;
          }));
          list2.AddRange(collection);
        }
      }
      return list2;
    }

    private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
    {
      switch (TypeExtensions.MemberType(memberInfo))
      {
        case MemberTypes.Field:
        case MemberTypes.Property:
          Type declaringType = memberInfo.DeclaringType;
          if (!TypeExtensions.IsGenericType(declaringType))
            return false;
          Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
          if (genericTypeDefinition == null)
            return false;
          MemberInfo[] member = genericTypeDefinition.GetMember(memberInfo.Name, bindingAttr);
          return member.Length != 0 && ReflectionUtils.GetMemberUnderlyingType(member[0]).IsGenericParameter;
        default:
          throw new ArgumentException("Member must be a field or property.");
      }
    }

    public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
    {
      return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
    }

    public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
    {
      return Enumerable.SingleOrDefault<T>((IEnumerable<T>) ReflectionUtils.GetAttributes<T>(attributeProvider, inherit));
    }

    public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
    {
      ValidationUtils.ArgumentNotNull((object) attributeProvider, "attributeProvider");
      object obj = (object) attributeProvider;
      if (obj is Type)
        return (T[]) ((Type) obj).GetCustomAttributes(typeof (T), inherit);
      if (obj is Assembly)
        return (T[]) Attribute.GetCustomAttributes((Assembly) obj, typeof (T));
      if (obj is MemberInfo)
        return (T[]) Attribute.GetCustomAttributes((MemberInfo) obj, typeof (T), inherit);
      if (obj is Module)
        return (T[]) Attribute.GetCustomAttributes((Module) obj, typeof (T), inherit);
      if (obj is ParameterInfo)
        return (T[]) Attribute.GetCustomAttributes((ParameterInfo) obj, typeof (T), inherit);
      else
        return (T[]) attributeProvider.GetCustomAttributes(typeof (T), inherit);
    }

    public static Type MakeGenericType(Type genericTypeDefinition, params Type[] innerTypes)
    {
      ValidationUtils.ArgumentNotNull((object) genericTypeDefinition, "genericTypeDefinition");
      ValidationUtils.ArgumentNotNullOrEmpty<Type>((ICollection<Type>) innerTypes, "innerTypes");
      ValidationUtils.ArgumentConditionTrue(TypeExtensions.IsGenericTypeDefinition(genericTypeDefinition), "genericTypeDefinition", StringUtils.FormatWith("Type {0} is not a generic type definition.", (IFormatProvider) CultureInfo.InvariantCulture, (object) genericTypeDefinition));
      return genericTypeDefinition.MakeGenericType(innerTypes);
    }

    public static object CreateGeneric(Type genericTypeDefinition, Type innerType, params object[] args)
    {
      return ReflectionUtils.CreateGeneric(genericTypeDefinition, (IList<Type>) new Type[1]
      {
        innerType
      }, args);
    }

    public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, params object[] args)
    {
      return ReflectionUtils.CreateGeneric(genericTypeDefinition, innerTypes, (Func<Type, IList<object>, object>) ((t, a) => ReflectionUtils.CreateInstance(t, Enumerable.ToArray<object>((IEnumerable<object>) a))), args);
    }

    public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, Func<Type, IList<object>, object> instanceCreator, params object[] args)
    {
      ValidationUtils.ArgumentNotNull((object) genericTypeDefinition, "genericTypeDefinition");
      ValidationUtils.ArgumentNotNullOrEmpty<Type>((ICollection<Type>) innerTypes, "innerTypes");
      ValidationUtils.ArgumentNotNull((object) instanceCreator, "createInstance");
      Type type = ReflectionUtils.MakeGenericType(genericTypeDefinition, Enumerable.ToArray<Type>((IEnumerable<Type>) innerTypes));
      return instanceCreator(type, (IList<object>) args);
    }

    public static object CreateInstance(Type type, params object[] args)
    {
      ValidationUtils.ArgumentNotNull((object) type, "type");
      return Activator.CreateInstance(type, args);
    }

    public static void SplitFullyQualifiedTypeName(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
    {
      int? assemblyDelimiterIndex = ReflectionUtils.GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
      if (assemblyDelimiterIndex.HasValue)
      {
        typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
        assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value - 1).Trim();
      }
      else
      {
        typeName = fullyQualifiedTypeName;
        assemblyName = (string) null;
      }
    }

    private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
    {
      int num = 0;
      for (int index = 0; index < fullyQualifiedTypeName.Length; ++index)
      {
        switch (fullyQualifiedTypeName[index])
        {
          case ',':
            if (num == 0)
              return new int?(index);
            else
              break;
          case '[':
            ++num;
            break;
          case ']':
            --num;
            break;
        }
      }
      return new int?();
    }

    public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
    {
      if (TypeExtensions.MemberType(memberInfo) != MemberTypes.Property)
        return Enumerable.SingleOrDefault<MemberInfo>((IEnumerable<MemberInfo>) targetType.GetMember(memberInfo.Name, TypeExtensions.MemberType(memberInfo), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
      PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
      Type[] types = Enumerable.ToArray<Type>(Enumerable.Select<ParameterInfo, Type>((IEnumerable<ParameterInfo>) propertyInfo.GetIndexParameters(), (Func<ParameterInfo, Type>) (p => p.ParameterType)));
      return (MemberInfo) targetType.GetProperty(propertyInfo.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, propertyInfo.PropertyType, types, (ParameterModifier[]) null);
    }

    public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
    {
      ValidationUtils.ArgumentNotNull((object) targetType, "targetType");
      List<MemberInfo> list = new List<MemberInfo>((IEnumerable<MemberInfo>) targetType.GetFields(bindingAttr));
      ReflectionUtils.GetChildPrivateFields((IList<MemberInfo>) list, targetType, bindingAttr);
      return Enumerable.Cast<FieldInfo>((IEnumerable) list);
    }

    private static void GetChildPrivateFields(IList<MemberInfo> initialFields, Type targetType, BindingFlags bindingAttr)
    {
      if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
        return;
      BindingFlags bindingAttr1 = ReflectionUtils.RemoveFlag(bindingAttr, BindingFlags.Public);
      while ((targetType = TypeExtensions.BaseType(targetType)) != null)
      {
        IEnumerable<MemberInfo> collection = Enumerable.Cast<MemberInfo>((IEnumerable) Enumerable.Where<FieldInfo>((IEnumerable<FieldInfo>) targetType.GetFields(bindingAttr1), (Func<FieldInfo, bool>) (f => f.IsPrivate)));
        CollectionUtils.AddRange<MemberInfo>(initialFields, collection);
      }
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
    {
      ValidationUtils.ArgumentNotNull((object) targetType, "targetType");
      List<PropertyInfo> list = new List<PropertyInfo>((IEnumerable<PropertyInfo>) targetType.GetProperties(bindingAttr));
      ReflectionUtils.GetChildPrivateProperties((IList<PropertyInfo>) list, targetType, bindingAttr);
      for (int index = 0; index < list.Count; ++index)
      {
        PropertyInfo propertyInfo1 = list[index];
        if (propertyInfo1.DeclaringType != targetType)
        {
          PropertyInfo propertyInfo2 = (PropertyInfo) ReflectionUtils.GetMemberInfoFromType(propertyInfo1.DeclaringType, (MemberInfo) propertyInfo1);
          list[index] = propertyInfo2;
        }
      }
      return (IEnumerable<PropertyInfo>) list;
    }

    public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
    {
      if ((bindingAttr & flag) != flag)
        return bindingAttr;
      else
        return bindingAttr ^ flag;
    }

    private static void GetChildPrivateProperties(IList<PropertyInfo> initialProperties, Type targetType, BindingFlags bindingAttr)
    {
      if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
        return;
      BindingFlags bindingAttr1 = ReflectionUtils.RemoveFlag(bindingAttr, BindingFlags.Public);
      while ((targetType = TypeExtensions.BaseType(targetType)) != null)
      {
        foreach (PropertyInfo propertyInfo in targetType.GetProperties(bindingAttr1))
        {
          PropertyInfo nonPublicProperty = propertyInfo;
          int index = CollectionUtils.IndexOf<PropertyInfo>((IEnumerable<PropertyInfo>) initialProperties, (Func<PropertyInfo, bool>) (p => p.Name == nonPublicProperty.Name));
          if (index == -1)
            initialProperties.Add(nonPublicProperty);
          else
            initialProperties[index] = nonPublicProperty;
        }
      }
    }

    public static bool IsMethodOverridden(Type currentType, Type methodDeclaringType, string method)
    {
      return Enumerable.Any<MethodInfo>((IEnumerable<MethodInfo>) currentType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (Func<MethodInfo, bool>) (info =>
      {
        if (info.Name == method && info.DeclaringType != methodDeclaringType)
          return info.GetBaseDefinition().DeclaringType == methodDeclaringType;
        else
          return false;
      }));
    }

    public static object GetDefaultValue(Type type)
    {
      if (!TypeExtensions.IsValueType(type))
        return (object) null;
      switch (ConvertUtils.GetTypeCode(type))
      {
        case TypeCode.Boolean:
          return (object) false;
        case TypeCode.Char:
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
          return (object) 0;
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return (object) 0;
        case TypeCode.Single:
          return (object) 0.0f;
        case TypeCode.Double:
          return (object) 0.0;
        case TypeCode.Decimal:
          return (object) new Decimal(0);
        case TypeCode.DateTime:
          return (object) new DateTime();
        default:
          if (type == typeof (Guid))
            return (object) new Guid();
          if (type == typeof (DateTimeOffset))
            return (object) new DateTimeOffset();
          if (ReflectionUtils.IsNullable(type))
            return (object) null;
          else
            return Activator.CreateInstance(type);
      }
    }
  }
}
