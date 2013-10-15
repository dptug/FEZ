// Type: Newtonsoft.Json.Utilities.DynamicReflectionDelegateFactory
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace Newtonsoft.Json.Utilities
{
  internal class DynamicReflectionDelegateFactory : ReflectionDelegateFactory
  {
    public static DynamicReflectionDelegateFactory Instance = new DynamicReflectionDelegateFactory();

    static DynamicReflectionDelegateFactory()
    {
    }

    private static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
    {
      return !TypeExtensions.IsInterface(owner) ? new DynamicMethod(name, returnType, parameterTypes, owner, true) : new DynamicMethod(name, returnType, parameterTypes, owner.Module, true);
    }

    public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(method.ToString(), typeof (object), new Type[2]
      {
        typeof (object),
        typeof (object[])
      }, method.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      this.GenerateCreateMethodCallIL(method, ilGenerator);
      return (MethodCall<T, object>) dynamicMethod.CreateDelegate(typeof (MethodCall<T, object>));
    }

    private void GenerateCreateMethodCallIL(MethodBase method, ILGenerator generator)
    {
      ParameterInfo[] parameters = method.GetParameters();
      Label label = generator.DefineLabel();
      generator.Emit(OpCodes.Ldarg_1);
      generator.Emit(OpCodes.Ldlen);
      generator.Emit(OpCodes.Ldc_I4, parameters.Length);
      generator.Emit(OpCodes.Beq, label);
      generator.Emit(OpCodes.Newobj, typeof (TargetParameterCountException).GetConstructor(ReflectionUtils.EmptyTypes));
      generator.Emit(OpCodes.Throw);
      generator.MarkLabel(label);
      if (!method.IsConstructor && !method.IsStatic)
        ILGeneratorExtensions.PushInstance(generator, method.DeclaringType);
      for (int index = 0; index < parameters.Length; ++index)
      {
        generator.Emit(OpCodes.Ldarg_1);
        generator.Emit(OpCodes.Ldc_I4, index);
        generator.Emit(OpCodes.Ldelem_Ref);
        ILGeneratorExtensions.UnboxIfNeeded(generator, parameters[index].ParameterType);
      }
      if (method.IsConstructor)
        generator.Emit(OpCodes.Newobj, (ConstructorInfo) method);
      else if (method.IsFinal || !method.IsVirtual)
        ILGeneratorExtensions.CallMethod(generator, (MethodInfo) method);
      Type type = method.IsConstructor ? method.DeclaringType : ((MethodInfo) method).ReturnType;
      if (type != typeof (void))
        ILGeneratorExtensions.BoxIfNeeded(generator, type);
      else
        generator.Emit(OpCodes.Ldnull);
      ILGeneratorExtensions.Return(generator);
    }

    public override Func<T> CreateDefaultConstructor<T>(Type type)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Create" + type.FullName, typeof (T), ReflectionUtils.EmptyTypes, type);
      dynamicMethod.InitLocals = true;
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      this.GenerateCreateDefaultConstructorIL(type, ilGenerator);
      return (Func<T>) dynamicMethod.CreateDelegate(typeof (Func<T>));
    }

    private void GenerateCreateDefaultConstructorIL(Type type, ILGenerator generator)
    {
      if (TypeExtensions.IsValueType(type))
      {
        generator.DeclareLocal(type);
        generator.Emit(OpCodes.Ldloc_0);
        generator.Emit(OpCodes.Box, type);
      }
      else
      {
        ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, ReflectionUtils.EmptyTypes, (ParameterModifier[]) null);
        if (constructor == null)
          throw new ArgumentException(StringUtils.FormatWith("Could not get constructor for {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type));
        generator.Emit(OpCodes.Newobj, constructor);
      }
      ILGeneratorExtensions.Return(generator);
    }

    public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Get" + propertyInfo.Name, typeof (T), new Type[1]
      {
        typeof (object)
      }, propertyInfo.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      this.GenerateCreateGetPropertyIL(propertyInfo, ilGenerator);
      return (Func<T, object>) dynamicMethod.CreateDelegate(typeof (Func<T, object>));
    }

    private void GenerateCreateGetPropertyIL(PropertyInfo propertyInfo, ILGenerator generator)
    {
      MethodInfo getMethod = propertyInfo.GetGetMethod(true);
      if (getMethod == null)
        throw new ArgumentException(StringUtils.FormatWith("Property '{0}' does not have a getter.", (IFormatProvider) CultureInfo.InvariantCulture, (object) propertyInfo.Name));
      if (!getMethod.IsStatic)
        ILGeneratorExtensions.PushInstance(generator, propertyInfo.DeclaringType);
      ILGeneratorExtensions.CallMethod(generator, getMethod);
      ILGeneratorExtensions.BoxIfNeeded(generator, propertyInfo.PropertyType);
      ILGeneratorExtensions.Return(generator);
    }

    public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Get" + fieldInfo.Name, typeof (T), new Type[1]
      {
        typeof (object)
      }, fieldInfo.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      this.GenerateCreateGetFieldIL(fieldInfo, ilGenerator);
      return (Func<T, object>) dynamicMethod.CreateDelegate(typeof (Func<T, object>));
    }

    private void GenerateCreateGetFieldIL(FieldInfo fieldInfo, ILGenerator generator)
    {
      if (!fieldInfo.IsStatic)
        ILGeneratorExtensions.PushInstance(generator, fieldInfo.DeclaringType);
      generator.Emit(OpCodes.Ldfld, fieldInfo);
      ILGeneratorExtensions.BoxIfNeeded(generator, fieldInfo.FieldType);
      ILGeneratorExtensions.Return(generator);
    }

    public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Set" + fieldInfo.Name, (Type) null, new Type[2]
      {
        typeof (T),
        typeof (object)
      }, fieldInfo.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      DynamicReflectionDelegateFactory.GenerateCreateSetFieldIL(fieldInfo, ilGenerator);
      return (Action<T, object>) dynamicMethod.CreateDelegate(typeof (Action<T, object>));
    }

    internal static void GenerateCreateSetFieldIL(FieldInfo fieldInfo, ILGenerator generator)
    {
      if (!fieldInfo.IsStatic)
        ILGeneratorExtensions.PushInstance(generator, fieldInfo.DeclaringType);
      generator.Emit(OpCodes.Ldarg_1);
      ILGeneratorExtensions.UnboxIfNeeded(generator, fieldInfo.FieldType);
      generator.Emit(OpCodes.Stfld, fieldInfo);
      ILGeneratorExtensions.Return(generator);
    }

    public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Set" + propertyInfo.Name, (Type) null, new Type[2]
      {
        typeof (T),
        typeof (object)
      }, propertyInfo.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      DynamicReflectionDelegateFactory.GenerateCreateSetPropertyIL(propertyInfo, ilGenerator);
      return (Action<T, object>) dynamicMethod.CreateDelegate(typeof (Action<T, object>));
    }

    internal static void GenerateCreateSetPropertyIL(PropertyInfo propertyInfo, ILGenerator generator)
    {
      MethodInfo setMethod = propertyInfo.GetSetMethod(true);
      if (!setMethod.IsStatic)
        ILGeneratorExtensions.PushInstance(generator, propertyInfo.DeclaringType);
      generator.Emit(OpCodes.Ldarg_1);
      ILGeneratorExtensions.UnboxIfNeeded(generator, propertyInfo.PropertyType);
      ILGeneratorExtensions.CallMethod(generator, setMethod);
      ILGeneratorExtensions.Return(generator);
    }
  }
}
