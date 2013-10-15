// Type: Newtonsoft.Json.Utilities.TypeExtensions
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class TypeExtensions
  {
    public static MemberTypes MemberType(this MemberInfo memberInfo)
    {
      return memberInfo.MemberType;
    }

    public static bool ContainsGenericParameters(this Type type)
    {
      return type.ContainsGenericParameters;
    }

    public static bool IsInterface(this Type type)
    {
      return type.IsInterface;
    }

    public static bool IsGenericType(this Type type)
    {
      return type.IsGenericType;
    }

    public static bool IsGenericTypeDefinition(this Type type)
    {
      return type.IsGenericTypeDefinition;
    }

    public static Type BaseType(this Type type)
    {
      return type.BaseType;
    }

    public static bool IsEnum(this Type type)
    {
      return type.IsEnum;
    }

    public static bool IsClass(this Type type)
    {
      return type.IsClass;
    }

    public static bool IsSealed(this Type type)
    {
      return type.IsSealed;
    }

    public static bool IsAbstract(this Type type)
    {
      return type.IsAbstract;
    }

    public static bool IsVisible(this Type type)
    {
      return type.IsVisible;
    }

    public static bool IsValueType(this Type type)
    {
      return type.IsValueType;
    }

    public static bool AssignableToTypeName(this Type type, string fullTypeName, out Type match)
    {
      for (Type type1 = type; type1 != null; type1 = TypeExtensions.BaseType(type1))
      {
        if (string.Equals(type1.FullName, fullTypeName, StringComparison.Ordinal))
        {
          match = type1;
          return true;
        }
      }
      foreach (MemberInfo memberInfo in type.GetInterfaces())
      {
        if (string.Equals(memberInfo.Name, fullTypeName, StringComparison.Ordinal))
        {
          match = type;
          return true;
        }
      }
      match = (Type) null;
      return false;
    }

    public static bool AssignableToTypeName(this Type type, string fullTypeName)
    {
      Type match;
      return TypeExtensions.AssignableToTypeName(type, fullTypeName, out match);
    }

    public static MethodInfo GetGenericMethod(this Type type, string name, params Type[] parameterTypes)
    {
      foreach (MethodInfo method in Enumerable.Where<MethodInfo>((IEnumerable<MethodInfo>) type.GetMethods(), (Func<MethodInfo, bool>) (method => method.Name == name)))
      {
        if (TypeExtensions.HasParameters(method, parameterTypes))
          return method;
      }
      return (MethodInfo) null;
    }

    public static bool HasParameters(this MethodInfo method, params Type[] parameterTypes)
    {
      Type[] typeArray = Enumerable.ToArray<Type>(Enumerable.Select<ParameterInfo, Type>((IEnumerable<ParameterInfo>) method.GetParameters(), (Func<ParameterInfo, Type>) (parameter => parameter.ParameterType)));
      if (typeArray.Length != parameterTypes.Length)
        return false;
      for (int index = 0; index < typeArray.Length; ++index)
      {
        if (typeArray[index].ToString() != parameterTypes[index].ToString())
          return false;
      }
      return true;
    }

    public static IEnumerable<Type> GetAllInterfaces(this Type target)
    {
      foreach (Type type1 in target.GetInterfaces())
      {
        yield return type1;
        foreach (Type type2 in type1.GetInterfaces())
          yield return type2;
      }
    }

    public static IEnumerable<MethodInfo> GetAllMethods(this Type target)
    {
      List<Type> list = Enumerable.ToList<Type>(TypeExtensions.GetAllInterfaces(target));
      list.Add(target);
      return Enumerable.SelectMany<Type, MethodInfo, MethodInfo>((IEnumerable<Type>) list, (Func<Type, IEnumerable<MethodInfo>>) (type => (IEnumerable<MethodInfo>) type.GetMethods()), (Func<Type, MethodInfo, MethodInfo>) ((type, method) => method));
    }
  }
}
