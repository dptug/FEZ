// Type: Newtonsoft.Json.Linq.JPropertyDescriptor
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.ComponentModel;

namespace Newtonsoft.Json.Linq
{
  public class JPropertyDescriptor : PropertyDescriptor
  {
    private readonly Type _propertyType;

    public override Type ComponentType
    {
      get
      {
        return typeof (JObject);
      }
    }

    public override bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public override Type PropertyType
    {
      get
      {
        return this._propertyType;
      }
    }

    protected override int NameHashCode
    {
      get
      {
        return base.NameHashCode;
      }
    }

    public JPropertyDescriptor(string name, Type propertyType)
      : base(name, (Attribute[]) null)
    {
      ValidationUtils.ArgumentNotNull((object) name, "name");
      ValidationUtils.ArgumentNotNull((object) propertyType, "propertyType");
      this._propertyType = propertyType;
    }

    private static JObject CastInstance(object instance)
    {
      return (JObject) instance;
    }

    public override bool CanResetValue(object component)
    {
      return false;
    }

    public override object GetValue(object component)
    {
      return (object) JPropertyDescriptor.CastInstance(component)[this.Name];
    }

    public override void ResetValue(object component)
    {
    }

    public override void SetValue(object component, object value)
    {
      JToken jtoken = value is JToken ? (JToken) value : (JToken) new JValue(value);
      JPropertyDescriptor.CastInstance(component)[this.Name] = jtoken;
    }

    public override bool ShouldSerializeValue(object component)
    {
      return false;
    }
  }
}
