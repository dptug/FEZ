// Type: SharpDX.Design.Vector3Converter
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace SharpDX.Design
{
  public class Vector3Converter : BaseConverter
  {
    public Vector3Converter()
    {
      try
      {
        Type type = typeof (Vector3);
        this.Properties = new PropertyDescriptorCollection((PropertyDescriptor[]) new FieldPropertyDescriptor[3]
        {
          new FieldPropertyDescriptor(type.GetField("X")),
          new FieldPropertyDescriptor(type.GetField("Y")),
          new FieldPropertyDescriptor(type.GetField("Z"))
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
      }
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (value is Vector3)
      {
        Vector3 vector3 = (Vector3) value;
        if (destinationType == typeof (string))
          return (object) BaseConverter.ConvertFromValues<float>(context, culture, vector3.ToArray());
        if (destinationType == typeof (InstanceDescriptor))
        {
          ConstructorInfo constructor = typeof (Vector3).GetConstructor(MathUtil.Array<Type>(typeof (float), 3));
          if (constructor != (ConstructorInfo) null)
            return (object) new InstanceDescriptor((MemberInfo) constructor, (ICollection) vector3.ToArray());
        }
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      float[] values = BaseConverter.ConvertToValues<float>(context, culture, value);
      if (values == null)
        return base.ConvertFrom(context, culture, value);
      else
        return (object) new Vector3(values);
    }

    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
      if (propertyValues == null)
        throw new ArgumentNullException("propertyValues");
      else
        return (object) new Vector3((float) propertyValues[(object) "X"], (float) propertyValues[(object) "Y"], (float) propertyValues[(object) "Z"]);
    }
  }
}
