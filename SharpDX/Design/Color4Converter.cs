// Type: SharpDX.Design.Color4Converter
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
  public class Color4Converter : BaseConverter
  {
    public Color4Converter()
    {
      Type type = typeof (Color4);
      this.Properties = new PropertyDescriptorCollection((PropertyDescriptor[]) new FieldPropertyDescriptor[4]
      {
        new FieldPropertyDescriptor(type.GetField("Red")),
        new FieldPropertyDescriptor(type.GetField("Green")),
        new FieldPropertyDescriptor(type.GetField("Blue")),
        new FieldPropertyDescriptor(type.GetField("Alpha"))
      });
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (value is Color4)
      {
        Color4 color4 = (Color4) value;
        if (destinationType == typeof (string))
          return (object) BaseConverter.ConvertFromValues<float>(context, culture, color4.ToArray());
        if (destinationType == typeof (InstanceDescriptor))
        {
          ConstructorInfo constructor = typeof (Color4).GetConstructor(MathUtil.Array<Type>(typeof (float), 4));
          if (constructor != (ConstructorInfo) null)
            return (object) new InstanceDescriptor((MemberInfo) constructor, (ICollection) color4.ToArray());
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
        return (object) new Color4(values);
    }

    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
      if (propertyValues == null)
        throw new ArgumentNullException("propertyValues");
      else
        return (object) new Color4((float) propertyValues[(object) "Red"], (float) propertyValues[(object) "Green"], (float) propertyValues[(object) "Blue"], (float) propertyValues[(object) "Alpha"]);
    }
  }
}
