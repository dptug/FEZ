// Type: Newtonsoft.Json.Serialization.DynamicValueProvider
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  public class DynamicValueProvider : IValueProvider
  {
    private readonly MemberInfo _memberInfo;
    private Func<object, object> _getter;
    private Action<object, object> _setter;

    public DynamicValueProvider(MemberInfo memberInfo)
    {
      ValidationUtils.ArgumentNotNull((object) memberInfo, "memberInfo");
      this._memberInfo = memberInfo;
    }

    public void SetValue(object target, object value)
    {
      try
      {
        if (this._setter == null)
          this._setter = ((ReflectionDelegateFactory) DynamicReflectionDelegateFactory.Instance).CreateSet<object>(this._memberInfo);
        this._setter(target, value);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException(StringUtils.FormatWith("Error setting value to '{0}' on '{1}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }

    public object GetValue(object target)
    {
      try
      {
        if (this._getter == null)
          this._getter = ((ReflectionDelegateFactory) DynamicReflectionDelegateFactory.Instance).CreateGet<object>(this._memberInfo);
        return this._getter(target);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException(StringUtils.FormatWith("Error getting value from '{0}' on '{1}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }
  }
}
