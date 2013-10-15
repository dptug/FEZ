// Type: SharpDX.Design.Vector2Converter
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
  public class Vector2Converter : BaseConverter
  {
    public Vector2Converter()
    {
      Type type = typeof (Vector2);
      this.Properties = new PropertyDescriptorCollection((PropertyDescriptor[]) new FieldPropertyDescriptor[2]
      {
        new FieldPropertyDescriptor(type.GetField("X")),
        new FieldPropertyDescriptor(type.GetField("Y"))
      });
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (value is Vector2)
      {
        Vector2 vector2 = (Vector2) value;
        if (destinationType == typeof (string))
          return (object) BaseConverter.ConvertFromValues<float>(context, culture, vector2.ToArray());
        if (destinationType == typeof (InstanceDescriptor))
        {
          ConstructorInfo constructor = typeof (Vector2).GetConstructor(MathUtil.Array<Type>(typeof (float), 2));
          if (constructor != (ConstructorInfo) null)
            return (object) new InstanceDescriptor((MemberInfo) constructor, (ICollection) vector2.ToArray());
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
        return (object) new Vector2(values);
    }

    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
      if (propertyValues == null)
        throw new ArgumentNullException("propertyValues");
      else
        return (object) new Vector2((float) propertyValues[(object) "X"], (float) propertyValues[(object) "Y"]);
    }
  }
}
