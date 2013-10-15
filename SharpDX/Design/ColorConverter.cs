// Type: SharpDX.Design.ColorConverter
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
  public class ColorConverter : BaseConverter
  {
    public ColorConverter()
    {
      Type type = typeof (Color);
      this.Properties = new PropertyDescriptorCollection((PropertyDescriptor[]) new FieldPropertyDescriptor[4]
      {
        new FieldPropertyDescriptor(type.GetField("R")),
        new FieldPropertyDescriptor(type.GetField("G")),
        new FieldPropertyDescriptor(type.GetField("B")),
        new FieldPropertyDescriptor(type.GetField("A"))
      });
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (value is Color)
      {
        Color color = (Color) value;
        if (destinationType == typeof (string))
          return (object) BaseConverter.ConvertFromValues<byte>(context, culture, color.ToArray());
        if (destinationType == typeof (InstanceDescriptor))
        {
          ConstructorInfo constructor = typeof (Color).GetConstructor(MathUtil.Array<Type>(typeof (byte), 4));
          if (constructor != (ConstructorInfo) null)
            return (object) new InstanceDescriptor((MemberInfo) constructor, (ICollection) color.ToArray());
        }
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      byte[] values = BaseConverter.ConvertToValues<byte>(context, culture, value);
      if (values == null)
        return base.ConvertFrom(context, culture, value);
      else
        return (object) new Color(values);
    }

    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
      if (propertyValues == null)
        throw new ArgumentNullException("propertyValues");
      else
        return (object) new Color((byte) propertyValues[(object) "R"], (byte) propertyValues[(object) "G"], (byte) propertyValues[(object) "B"], (byte) propertyValues[(object) "A"]);
    }
  }
}
