// Type: SharpDX.Design.Half4Converter
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
  public class Half4Converter : ExpandableObjectConverter
  {
    private readonly PropertyDescriptorCollection m_Properties;

    public Half4Converter()
    {
      Type type = typeof (Half4);
      this.m_Properties = new PropertyDescriptorCollection(new PropertyDescriptor[4]
      {
        (PropertyDescriptor) new FieldPropertyDescriptor(type.GetField("X")),
        (PropertyDescriptor) new FieldPropertyDescriptor(type.GetField("Y")),
        (PropertyDescriptor) new FieldPropertyDescriptor(type.GetField("Z")),
        (PropertyDescriptor) new FieldPropertyDescriptor(type.GetField("W"))
      });
    }

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      if (!(sourceType == typeof (string)))
        return base.CanConvertFrom(context, sourceType);
      else
        return true;
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      if (destinationType != typeof (string) && destinationType != typeof (InstanceDescriptor))
        return base.CanConvertTo(context, destinationType);
      else
        return true;
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      if (culture == null)
        culture = CultureInfo.CurrentCulture;
      string str1 = value as string;
      if (str1 == null)
        return base.ConvertFrom(context, culture, value);
      string str2 = str1.Trim();
      TypeConverter converter = TypeDescriptor.GetConverter(typeof (Half));
      char[] chArray = new char[1]
      {
        culture.TextInfo.ListSeparator[0]
      };
      string[] strArray = str2.Split(chArray);
      if (strArray.Length != 4)
        throw new ArgumentException("Invalid half format.");
      else
        return (object) new Half4((Half) converter.ConvertFromString(context, culture, strArray[0]), (Half) converter.ConvertFromString(context, culture, strArray[1]), (Half) converter.ConvertFromString(context, culture, strArray[2]), (Half) converter.ConvertFromString(context, culture, strArray[3]));
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (culture == null)
        culture = CultureInfo.CurrentCulture;
      if (destinationType == typeof (string) && value is Half4)
      {
        TypeConverter converter = TypeDescriptor.GetConverter(typeof (Half));
        return (object) string.Join(culture.TextInfo.ListSeparator + " ", converter.ConvertToString(context, culture, (object) ((Half4) value).X), converter.ConvertToString(context, culture, (object) ((Half4) value).Y), converter.ConvertToString(context, culture, (object) ((Half4) value).Z), converter.ConvertToString(context, culture, (object) ((Half4) value).W));
      }
      else
      {
        if (destinationType == typeof (InstanceDescriptor) && value is Half4)
        {
          ConstructorInfo constructor = typeof (Half4).GetConstructor(new Type[4]
          {
            typeof (Half),
            typeof (Half),
            typeof (Half),
            typeof (Half)
          });
          if (constructor != (ConstructorInfo) null)
            return (object) new InstanceDescriptor((MemberInfo) constructor, (ICollection) new object[4]
            {
              (object) ((Half4) value).X,
              (object) ((Half4) value).Y,
              (object) ((Half4) value).Z,
              (object) ((Half4) value).W
            });
        }
        return base.ConvertTo(context, culture, value, destinationType);
      }
    }

    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
      if (propertyValues == null)
        throw new ArgumentNullException("propertyValues");
      else
        return (object) new Half4((Half) propertyValues[(object) "X"], (Half) propertyValues[(object) "Y"], (Half) propertyValues[(object) "Z"], (Half) propertyValues[(object) "W"]);
    }

    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
      return this.m_Properties;
    }

    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
      return true;
    }
  }
}
