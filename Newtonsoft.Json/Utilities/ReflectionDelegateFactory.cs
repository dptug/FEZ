// Type: Newtonsoft.Json.Utilities.ReflectionDelegateFactory
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal abstract class ReflectionDelegateFactory
  {
    public Func<T, object> CreateGet<T>(MemberInfo memberInfo)
    {
      PropertyInfo propertyInfo = memberInfo as PropertyInfo;
      if (propertyInfo != null)
        return this.CreateGet<T>(propertyInfo);
      FieldInfo fieldInfo = memberInfo as FieldInfo;
      if (fieldInfo != null)
        return this.CreateGet<T>(fieldInfo);
      else
        throw new Exception(StringUtils.FormatWith("Could not create getter for {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo));
    }

    public Action<T, object> CreateSet<T>(MemberInfo memberInfo)
    {
      PropertyInfo propertyInfo = memberInfo as PropertyInfo;
      if (propertyInfo != null)
        return this.CreateSet<T>(propertyInfo);
      FieldInfo fieldInfo = memberInfo as FieldInfo;
      if (fieldInfo != null)
        return this.CreateSet<T>(fieldInfo);
      else
        throw new Exception(StringUtils.FormatWith("Could not create setter for {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo));
    }

    public abstract MethodCall<T, object> CreateMethodCall<T>(MethodBase method);

    public abstract Func<T> CreateDefaultConstructor<T>(Type type);

    public abstract Func<T, object> CreateGet<T>(PropertyInfo propertyInfo);

    public abstract Func<T, object> CreateGet<T>(FieldInfo fieldInfo);

    public abstract Action<T, object> CreateSet<T>(FieldInfo fieldInfo);

    public abstract Action<T, object> CreateSet<T>(PropertyInfo propertyInfo);
  }
}
