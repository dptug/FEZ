// Type: Common.ReflectionHelper
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9992B00D-7E50-4755-8BAA-4E3BBC8F3470
// Assembly location: F:\Program Files (x86)\FEZ\Common.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Common
{
  public static class ReflectionHelper
  {
    private static readonly Type[] EmptyTypes = new Type[0];
    private static readonly Dictionary<HandlePair<Type, Type>, Attribute> typeAttributeCache = new Dictionary<HandlePair<Type, Type>, Attribute>();
    private static readonly Dictionary<HandlePair<PropertyInfo, Type>, Attribute> propertyAttributeCache = new Dictionary<HandlePair<PropertyInfo, Type>, Attribute>();
    private static readonly Dictionary<HandlePair<FieldInfo, Type>, Attribute> fieldAttributeCache = new Dictionary<HandlePair<FieldInfo, Type>, Attribute>();
    private static readonly Dictionary<Type, MemberInfo[]> propertyCache = new Dictionary<Type, MemberInfo[]>();
    private static readonly Dictionary<Type, MemberInfo[]> serviceCache = new Dictionary<Type, MemberInfo[]>();
    private static readonly Dictionary<MethodInfo, DynamicMethodDelegate> methodCache = new Dictionary<MethodInfo, DynamicMethodDelegate>();
    private static readonly Dictionary<Type, DynamicMethodDelegate> constructorCache = new Dictionary<Type, DynamicMethodDelegate>();
    public const BindingFlags PublicInstanceMembers = BindingFlags.Instance | BindingFlags.Public;

    static ReflectionHelper()
    {
    }

    public static T GetFirstAttribute<T>(Type type) where T : Attribute, new()
    {
      Type second = typeof (T);
      HandlePair<Type, Type> key = new HandlePair<Type, Type>(type, second);
      Attribute attribute;
      lock (ReflectionHelper.typeAttributeCache)
      {
        if (!ReflectionHelper.typeAttributeCache.TryGetValue(key, out attribute))
          ReflectionHelper.typeAttributeCache.Add(key, attribute = (Attribute) Enumerable.FirstOrDefault<object>((IEnumerable<object>) type.GetCustomAttributes(typeof (T), false)));
      }
      return attribute as T;
    }

    public static T GetFirstAttribute<T>(PropertyInfo propInfo) where T : Attribute, new()
    {
      Type second = typeof (T);
      HandlePair<PropertyInfo, Type> key = new HandlePair<PropertyInfo, Type>(propInfo, second);
      Attribute attribute;
      lock (ReflectionHelper.propertyAttributeCache)
      {
        if (!ReflectionHelper.propertyAttributeCache.TryGetValue(key, out attribute))
          ReflectionHelper.propertyAttributeCache.Add(key, attribute = (Attribute) Enumerable.FirstOrDefault<object>((IEnumerable<object>) propInfo.GetCustomAttributes(typeof (T), false)));
      }
      return attribute as T;
    }

    public static T GetFirstAttribute<T>(FieldInfo fieldInfo) where T : Attribute, new()
    {
      Type second = typeof (T);
      HandlePair<FieldInfo, Type> key = new HandlePair<FieldInfo, Type>(fieldInfo, second);
      Attribute attribute;
      lock (ReflectionHelper.fieldAttributeCache)
      {
        if (!ReflectionHelper.fieldAttributeCache.TryGetValue(key, out attribute))
          ReflectionHelper.fieldAttributeCache.Add(key, attribute = (Attribute) Enumerable.FirstOrDefault<object>((IEnumerable<object>) fieldInfo.GetCustomAttributes(typeof (T), false)));
      }
      return attribute as T;
    }

    public static T GetFirstAttribute<T>(MemberInfo memberInfo) where T : Attribute, new()
    {
      if (!(memberInfo is PropertyInfo))
        return ReflectionHelper.GetFirstAttribute<T>(memberInfo as FieldInfo);
      else
        return ReflectionHelper.GetFirstAttribute<T>(memberInfo as PropertyInfo);
    }

    public static MemberInfo[] GetSerializableMembers(Type type)
    {
      MemberInfo[] memberInfoArray;
      lock (ReflectionHelper.propertyCache)
      {
        if (!ReflectionHelper.propertyCache.TryGetValue(type, out memberInfoArray))
        {
          memberInfoArray = Enumerable.ToArray<MemberInfo>(Enumerable.Union<MemberInfo>(Enumerable.Cast<MemberInfo>((IEnumerable) Enumerable.Where<PropertyInfo>((IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy), (Func<PropertyInfo, bool>) (p =>
          {
            if (p.GetGetMethod() != (MethodInfo) null && p.GetSetMethod() != (MethodInfo) null)
              return p.GetGetMethod().GetParameters().Length == 0;
            else
              return false;
          }))), Enumerable.Cast<MemberInfo>((IEnumerable) type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))));
          ReflectionHelper.propertyCache.Add(type, memberInfoArray);
        }
      }
      return memberInfoArray;
    }

    public static MemberInfo[] GetSettableProperties(Type type)
    {
      MemberInfo[] memberInfoArray;
      lock (ReflectionHelper.serviceCache)
      {
        if (!ReflectionHelper.serviceCache.TryGetValue(type, out memberInfoArray))
        {
          memberInfoArray = (MemberInfo[]) Enumerable.ToArray<PropertyInfo>(Enumerable.Where<PropertyInfo>((IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (Func<PropertyInfo, bool>) (p => p.GetSetMethod(true) != (MethodInfo) null)));
          ReflectionHelper.serviceCache.Add(type, memberInfoArray);
        }
      }
      return memberInfoArray;
    }

    public static Type GetMemberType(MemberInfo member)
    {
      if (member is PropertyInfo)
        return (member as PropertyInfo).PropertyType;
      if (member is FieldInfo)
        return (member as FieldInfo).FieldType;
      else
        throw new NotImplementedException();
    }

    public static bool IsGenericSet(Type type)
    {
      return Enumerable.Any<Type>((IEnumerable<Type>) type.GetInterfaces(), (Func<Type, bool>) (i =>
      {
        if (i.IsGenericType)
          return i.GetGenericTypeDefinition() == typeof (ISet<>);
        else
          return false;
      }));
    }

    public static bool IsGenericList(Type type)
    {
      return Enumerable.Any<Type>((IEnumerable<Type>) type.GetInterfaces(), (Func<Type, bool>) (i =>
      {
        if (i.IsGenericType)
          return i.GetGenericTypeDefinition() == typeof (IList<>);
        else
          return false;
      }));
    }

    public static bool IsGenericCollection(Type type)
    {
      return Enumerable.Any<Type>((IEnumerable<Type>) type.GetInterfaces(), (Func<Type, bool>) (i =>
      {
        if (i.IsGenericType)
          return i.GetGenericTypeDefinition() == typeof (ICollection<>);
        else
          return false;
      }));
    }

    public static bool IsGenericDictionary(Type type)
    {
      return Enumerable.Any<Type>((IEnumerable<Type>) type.GetInterfaces(), (Func<Type, bool>) (i =>
      {
        if (i.IsGenericType)
          return i.GetGenericTypeDefinition() == typeof (IDictionary<,>);
        else
          return false;
      }));
    }

    public static bool IsNullable(Type type)
    {
      if (type.IsGenericType)
        return type.GetGenericTypeDefinition() == typeof (Nullable<>);
      else
        return false;
    }

    public static DynamicMethodDelegate CreateDelegate(MethodBase method)
    {
      ParameterInfo[] parameters = method.GetParameters();
      int length = parameters.Length;
      DynamicMethod dynamicMethod = new DynamicMethod("", typeof (object), new Type[2]
      {
        typeof (object),
        typeof (object[])
      }, typeof (ReflectionHelper).Module, true);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      Label label = ilGenerator.DefineLabel();
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Ldlen);
      ilGenerator.Emit(OpCodes.Ldc_I4, length);
      ilGenerator.Emit(OpCodes.Beq, label);
      ilGenerator.Emit(OpCodes.Newobj, typeof (TargetParameterCountException).GetConstructor(Type.EmptyTypes));
      ilGenerator.Emit(OpCodes.Throw);
      ilGenerator.MarkLabel(label);
      if (!method.IsStatic && !method.IsConstructor)
      {
        ilGenerator.Emit(OpCodes.Ldarg_0);
        if (method.DeclaringType.IsValueType)
          ilGenerator.Emit(OpCodes.Unbox, method.DeclaringType);
      }
      for (int index = 0; index < length; ++index)
      {
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldc_I4, index);
        ilGenerator.Emit(OpCodes.Ldelem_Ref);
        Type parameterType = parameters[index].ParameterType;
        if (parameterType.IsValueType)
          ilGenerator.Emit(OpCodes.Unbox_Any, parameterType);
      }
      if (method.IsConstructor)
        ilGenerator.Emit(OpCodes.Newobj, method as ConstructorInfo);
      else if (method.IsFinal || !method.IsVirtual)
        ilGenerator.Emit(OpCodes.Call, method as MethodInfo);
      else
        ilGenerator.Emit(OpCodes.Callvirt, method as MethodInfo);
      Type cls = method.IsConstructor ? method.DeclaringType : (method as MethodInfo).ReturnType;
      if (cls != typeof (void))
      {
        if (cls.IsValueType)
          ilGenerator.Emit(OpCodes.Box, cls);
      }
      else
        ilGenerator.Emit(OpCodes.Ldnull);
      ilGenerator.Emit(OpCodes.Ret);
      return (DynamicMethodDelegate) dynamicMethod.CreateDelegate(typeof (DynamicMethodDelegate));
    }

    public static object Instantiate(Type type)
    {
      if (type.IsValueType)
        return Activator.CreateInstance(type);
      if (type.IsArray)
        return (object) Array.CreateInstance(type.GetElementType(), 0);
      DynamicMethodDelegate @delegate;
      lock (ReflectionHelper.constructorCache)
      {
        if (!ReflectionHelper.constructorCache.TryGetValue(type, out @delegate))
        {
          @delegate = ReflectionHelper.CreateDelegate((MethodBase) type.GetConstructor(ReflectionHelper.EmptyTypes));
          ReflectionHelper.constructorCache.Add(type, @delegate);
        }
      }
      return @delegate((object) null, new object[0]);
    }

    public static DynamicMethodDelegate GetDelegate(MethodInfo info)
    {
      DynamicMethodDelegate @delegate;
      lock (ReflectionHelper.methodCache)
      {
        if (!ReflectionHelper.methodCache.TryGetValue(info, out @delegate))
        {
          @delegate = ReflectionHelper.CreateDelegate((MethodBase) info);
          ReflectionHelper.methodCache.Add(info, @delegate);
        }
      }
      return @delegate;
    }

    public static object InvokeMethod(MethodInfo info, object targetInstance, params object[] arguments)
    {
      return ReflectionHelper.GetDelegate(info)(targetInstance, arguments);
    }

    public static object GetValue(PropertyInfo member, object instance)
    {
      return ReflectionHelper.InvokeMethod(member.GetGetMethod(true), instance, new object[0]);
    }

    public static object GetValue(MemberInfo member, object instance)
    {
      if (member is PropertyInfo)
        return ReflectionHelper.GetValue(member as PropertyInfo, instance);
      if (member is FieldInfo)
        return (member as FieldInfo).GetValue(instance);
      else
        throw new NotImplementedException();
    }

    public static void SetValue(PropertyInfo member, object instance, object value)
    {
      ReflectionHelper.InvokeMethod(member.GetSetMethod(true), instance, new object[1]
      {
        value
      });
    }

    public static void SetValue(MemberInfo member, object instance, object value)
    {
      if (member is PropertyInfo)
      {
        ReflectionHelper.SetValue(member as PropertyInfo, instance, value);
      }
      else
      {
        if (!(member is FieldInfo))
          throw new NotImplementedException();
        (member as FieldInfo).SetValue(instance, value);
      }
    }

    public static string GetShortAssemblyQualifiedName<T>()
    {
      return ReflectionHelper.GetShortAssemblyQualifiedName(typeof (T));
    }

    public static string GetShortAssemblyQualifiedName(Type type)
    {
      return ReflectionHelper.GetShortAssemblyQualifiedName(type.AssemblyQualifiedName);
    }

    public static string GetShortAssemblyQualifiedName(string assemblyQName)
    {
      for (; assemblyQName.Contains(", Version"); {
        int num;
        int startIndex;
        assemblyQName = assemblyQName.Substring(0, num) + assemblyQName.Substring(startIndex);
      }
      )
      {
        num = assemblyQName.IndexOf(", Version");
        startIndex = assemblyQName.IndexOf("],", num);
        if (startIndex == -1)
          startIndex = assemblyQName.Length;
        if ((int) assemblyQName[startIndex - 1] == 93)
          --startIndex;
      }
      return assemblyQName;
    }
  }
}
