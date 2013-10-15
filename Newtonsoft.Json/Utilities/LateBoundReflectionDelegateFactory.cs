// Type: Newtonsoft.Json.Utilities.LateBoundReflectionDelegateFactory
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal class LateBoundReflectionDelegateFactory : ReflectionDelegateFactory
  {
    private static readonly LateBoundReflectionDelegateFactory _instance = new LateBoundReflectionDelegateFactory();

    internal static ReflectionDelegateFactory Instance
    {
      get
      {
        return (ReflectionDelegateFactory) LateBoundReflectionDelegateFactory._instance;
      }
    }

    static LateBoundReflectionDelegateFactory()
    {
    }

    public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
    {
      ValidationUtils.ArgumentNotNull((object) method, "method");
      ConstructorInfo c = method as ConstructorInfo;
      if (c != null)
        return (MethodCall<T, object>) ((o, a) => c.Invoke(a));
      else
        return (MethodCall<T, object>) ((o, a) => method.Invoke((object) o, a));
    }

    public override Func<T> CreateDefaultConstructor<T>(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, "type");
      if (TypeExtensions.IsValueType(type))
        return (Func<T>) (() => (T) ReflectionUtils.CreateInstance(type, new object[0]));
      ConstructorInfo constructorInfo = ReflectionUtils.GetDefaultConstructor(type, true);
      return (Func<T>) (() => (T) constructorInfo.Invoke((object[]) null));
    }

    public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
    {
      ValidationUtils.ArgumentNotNull((object) propertyInfo, "propertyInfo");
      return (Func<T, object>) (o => propertyInfo.GetValue((object) o, (object[]) null));
    }

    public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
    {
      ValidationUtils.ArgumentNotNull((object) fieldInfo, "fieldInfo");
      return (Func<T, object>) (o => fieldInfo.GetValue((object) o));
    }

    public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
    {
      ValidationUtils.ArgumentNotNull((object) fieldInfo, "fieldInfo");
      return (Action<T, object>) ((o, v) => fieldInfo.SetValue((object) o, v));
    }

    public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
    {
      ValidationUtils.ArgumentNotNull((object) propertyInfo, "propertyInfo");
      return (Action<T, object>) ((o, v) => propertyInfo.SetValue((object) o, v, (object[]) null));
    }
  }
}
