// Type: SharpDX.Design.HalfConverter
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
  public class HalfConverter : ExpandableObjectConverter
  {
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
      string str = value as string;
      if (str == null)
        return base.ConvertFrom(context, culture, value);
      string[] strArray = str.Trim().Split(new char[1]
      {
        culture.TextInfo.ListSeparator[0]
      });
      if (strArray.Length != 1)
        throw new ArgumentException("Invalid half format.");
      else
        return (object) new Half((float) TypeDescriptor.GetConverter(typeof (float)).ConvertFromString(context, culture, strArray[0]));
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (culture == null)
        culture = CultureInfo.CurrentCulture;
      if (destinationType == typeof (string) && value is Half)
      {
        TypeConverter converter = TypeDescriptor.GetConverter(typeof (float));
        return (object) string.Join(culture.TextInfo.ListSeparator + " ", new string[1]
        {
          converter.ConvertToString(context, culture, (object) (float) ((Half) value))
        });
      }
      else
      {
        if (destinationType == typeof (InstanceDescriptor) && value is Half)
        {
          ConstructorInfo constructor = typeof (Half).GetConstructor(new Type[1]
          {
            typeof (float)
          });
          if (constructor != (ConstructorInfo) null)
            return (object) new InstanceDescriptor((MemberInfo) constructor, (ICollection) new object[1]
            {
              (object) (float) ((Half) value)
            });
        }
        return base.ConvertTo(context, culture, value, destinationType);
      }
    }
  }
}
