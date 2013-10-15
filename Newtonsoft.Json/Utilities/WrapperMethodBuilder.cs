// Type: Newtonsoft.Json.Utilities.WrapperMethodBuilder
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Newtonsoft.Json.Utilities
{
  internal class WrapperMethodBuilder
  {
    private readonly Type _realObjectType;
    private readonly TypeBuilder _wrapperBuilder;

    public WrapperMethodBuilder(Type realObjectType, TypeBuilder proxyBuilder)
    {
      this._realObjectType = realObjectType;
      this._wrapperBuilder = proxyBuilder;
    }

    public void Generate(MethodInfo newMethod)
    {
      if (newMethod.IsGenericMethod)
        newMethod = newMethod.GetGenericMethodDefinition();
      FieldInfo field = typeof (DynamicWrapperBase).GetField("UnderlyingObject", BindingFlags.Instance | BindingFlags.NonPublic);
      ParameterInfo[] parameters = newMethod.GetParameters();
      Type[] parameterTypes = Enumerable.ToArray<Type>(Enumerable.Select<ParameterInfo, Type>((IEnumerable<ParameterInfo>) parameters, (Func<ParameterInfo, Type>) (parameter => parameter.ParameterType)));
      MethodBuilder methodBuilder = this._wrapperBuilder.DefineMethod(newMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual, newMethod.ReturnType, parameterTypes);
      if (newMethod.IsGenericMethod)
        methodBuilder.DefineGenericParameters(Enumerable.ToArray<string>(Enumerable.Select<Type, string>((IEnumerable<Type>) newMethod.GetGenericArguments(), (Func<Type, string>) (arg => arg.Name))));
      ILGenerator ilGenerator = methodBuilder.GetILGenerator();
      WrapperMethodBuilder.LoadUnderlyingObject(ilGenerator, field);
      WrapperMethodBuilder.PushParameters((ICollection<ParameterInfo>) parameters, ilGenerator);
      this.ExecuteMethod((MethodBase) newMethod, parameterTypes, ilGenerator);
      WrapperMethodBuilder.Return(ilGenerator);
    }

    private static void Return(ILGenerator ilGenerator)
    {
      ilGenerator.Emit(OpCodes.Ret);
    }

    private void ExecuteMethod(MethodBase newMethod, Type[] parameterTypes, ILGenerator ilGenerator)
    {
      MethodInfo method = this.GetMethod(newMethod, parameterTypes);
      if (method == null)
        throw new MissingMethodException("Unable to find method " + newMethod.Name + " on " + this._realObjectType.FullName);
      ilGenerator.Emit(OpCodes.Call, method);
    }

    private MethodInfo GetMethod(MethodBase realMethod, Type[] parameterTypes)
    {
      if (realMethod.IsGenericMethod)
        return TypeExtensions.GetGenericMethod(this._realObjectType, realMethod.Name, parameterTypes);
      else
        return this._realObjectType.GetMethod(realMethod.Name, parameterTypes);
    }

    private static void PushParameters(ICollection<ParameterInfo> parameters, ILGenerator ilGenerator)
    {
      for (int index = 1; index < parameters.Count + 1; ++index)
        ilGenerator.Emit(OpCodes.Ldarg, index);
    }

    private static void LoadUnderlyingObject(ILGenerator ilGenerator, FieldInfo srcField)
    {
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldfld, srcField);
    }
  }
}
