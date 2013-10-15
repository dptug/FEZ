// Type: SharpDX.Design.FieldPropertyDescriptor
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.ComponentModel;
using System.Reflection;

namespace SharpDX.Design
{
  public class FieldPropertyDescriptor : PropertyDescriptor
  {
    private readonly FieldInfo fieldInfo;

    public override Type ComponentType
    {
      get
      {
        return this.fieldInfo.DeclaringType;
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
        return this.fieldInfo.FieldType;
      }
    }

    public FieldPropertyDescriptor(FieldInfo fieldInfo)
      : base(fieldInfo.Name, new Attribute[0])
    {
      this.fieldInfo = fieldInfo;
      object[] customAttributes = fieldInfo.GetCustomAttributes(true);
      Attribute[] attributeArray = new Attribute[customAttributes.Length];
      for (int index = 0; index < attributeArray.Length; ++index)
        attributeArray[index] = (Attribute) customAttributes[index];
      this.AttributeArray = attributeArray;
    }

    public override bool CanResetValue(object component)
    {
      return false;
    }

    public override object GetValue(object component)
    {
      return this.fieldInfo.GetValue(component);
    }

    public override void ResetValue(object component)
    {
    }

    public override void SetValue(object component, object value)
    {
      this.fieldInfo.SetValue(component, value);
      this.OnValueChanged(component, EventArgs.Empty);
    }

    public override bool ShouldSerializeValue(object component)
    {
      return true;
    }

    public override int GetHashCode()
    {
      return this.fieldInfo.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !(this.GetType() == obj.GetType()))
        return false;
      else
        return ((FieldPropertyDescriptor) obj).fieldInfo.Equals((object) this.fieldInfo);
    }
  }
}
