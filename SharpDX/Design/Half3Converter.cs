// Type: SharpDX.Design.Half3Converter
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
  public class Half3Converter : ExpandableObjectConverter
  {
    private readonly PropertyDescriptorCollection m_Properties;

    public Half3Converter()
    {
      Type type = typeof (Half3);
      this.m_Properties = new PropertyDescriptorCollection(new PropertyDescriptor[3]
      {
        (PropertyDescriptor) new FieldPropertyDescriptor(type.GetField("X")),
        (PropertyDescriptor) new FieldPropertyDescriptor(type.GetField("Y")),
        (PropertyDescriptor) new FieldPropertyDescriptor(type.GetField("Z"))
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
      if (strArray.Length != 3)
        throw new ArgumentException("Invalid half format.");
      else
        return (object) new Half3((Half) converter.ConvertFromString(context, culture, strArray[0]), (Half) converter.ConvertFromString(context, culture, strArray[1]), (Half) converter.ConvertFromString(context, culture, strArray[2]));
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (culture == null)
        culture = CultureInfo.CurrentCulture;
      if (destinationType == typeof (string) && value is Half3)
      {
        TypeConverter converter = TypeDescriptor.GetConverter(typeof (Half));
        return (object) string.Join(culture.TextInfo.ListSeparator + " ", new string[3]
        {
          converter.ConvertToString(context, culture, (object) ((Half3) value).X),
          converter.ConvertToString(context, culture, (object) ((Half3) value).Y),
          converter.ConvertToString(context, culture, (object) ((Half3) value).Z)
        });
      }
      else
      {
        if (destinationType == typeof (InstanceDescriptor) && value is Half3)
        {
          ConstructorInfo constructor = typeof (Half3).GetConstructor(new Type[3]
          {
            typeof (Half),
            typeof (Half),
            typeof (Half)
          });
          if (constructor != (ConstructorInfo) null)
            return (object) new InstanceDescriptor((MemberInfo) constructor, (ICollection) new object[3]
            {
              (object) ((Half3) value).X,
              (object) ((Half3) value).Y,
              (object) ((Half3) value).Z
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
        return (object) new Half3((Half) propertyValues[(object) "X"], (Half) propertyValues[(object) "Y"], (Half) propertyValues[(object) "Z"]);
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
