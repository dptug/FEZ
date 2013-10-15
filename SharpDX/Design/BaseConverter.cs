// Type: SharpDX.Design.BaseConverter
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace SharpDX.Design
{
  public abstract class BaseConverter : ExpandableObjectConverter
  {
    protected PropertyDescriptorCollection Properties { get; set; }

    protected static string ConvertFromValues<T>(ITypeDescriptorContext context, CultureInfo culture, T[] values)
    {
      if (culture == null)
        culture = CultureInfo.CurrentCulture;
      TypeConverter converter = TypeDescriptor.GetConverter(typeof (T));
      string[] strArray = Array.ConvertAll<T, string>(values, (Converter<T, string>) (t => converter.ConvertToString(context, culture, (object) t)));
      return string.Join(culture.TextInfo.ListSeparator + " ", strArray);
    }

    protected static T[] ConvertToValues<T>(ITypeDescriptorContext context, CultureInfo culture, object strValue)
    {
      string str = strValue as string;
      if (string.IsNullOrEmpty(str))
        return (T[]) null;
      if (culture == null)
        culture = CultureInfo.CurrentCulture;
      TypeConverter converter = TypeDescriptor.GetConverter(typeof (T));
      return Array.ConvertAll<string, T>(str.Trim().Split(new string[1]
      {
        culture.TextInfo.ListSeparator
      }, StringSplitOptions.RemoveEmptyEntries), (Converter<string, T>) (s => (T) converter.ConvertFromString(context, culture, s)));
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
      if (!(destinationType == typeof (string)) && !(destinationType == typeof (InstanceDescriptor)))
        return base.CanConvertTo(context, destinationType);
      else
        return true;
    }

    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
      return this.Properties;
    }
  }
}
