// Type: SharpDX.Design.Half2Converter
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
  public class Half2Converter : ExpandableObjectConverter
  {
    private readonly PropertyDescriptorCollection properties;

    public Half2Converter()
    {
      Type type = typeof (Half2);
      this.properties = new PropertyDescriptorCollection(new PropertyDescriptor[2]
      {
        (PropertyDescriptor) new FieldPropertyDescriptor(type.GetField("X")),
        (PropertyDescriptor) new FieldPropertyDescriptor(type.GetField("Y"))
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
      if (strArray.Length != 2)
        throw new ArgumentException("Invalid half format.");
      else
        return (object) new Half2((Half) converter.ConvertFromString(context, culture, strArray[0]), (Half) converter.ConvertFromString(context, culture, strArray[1]));
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (culture == null)
        culture = CultureInfo.CurrentCulture;
      if (destinationType == typeof (string) && value is Half2)
      {
        TypeConverter converter = TypeDescriptor.GetConverter(typeof (Half));
        return (object) string.Join(culture.TextInfo.ListSeparator + " ", converter.ConvertToString(context, culture, (object) ((Half2) value).X), converter.ConvertToString(context, culture, (object) ((Half2) value).Y));
      }
      else
      {
        if (destinationType == typeof (InstanceDescriptor) && value is Half2)
        {
          ConstructorInfo constructor = typeof (Half2).GetConstructor(new Type[2]
          {
            typeof (Half),
            typeof (Half)
          });
          if (constructor != (ConstructorInfo) null)
            return (object) new InstanceDescriptor((MemberInfo) constructor, (ICollection) new object[2]
            {
              (object) ((Half2) value).X,
              (object) ((Half2) value).Y
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
        return (object) new Half2((Half) propertyValues[(object) "X"], (Half) propertyValues[(object) "Y"]);
    }

    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
      return this.properties;
    }

    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
      return true;
    }
  }
}
