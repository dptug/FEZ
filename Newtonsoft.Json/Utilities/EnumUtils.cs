// Type: Newtonsoft.Json.Utilities.EnumUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class EnumUtils
  {
    public static IList<T> GetFlagsValues<T>(T value) where T : struct
    {
      Type type = typeof (T);
      if (!type.IsDefined(typeof (FlagsAttribute), false))
        throw new ArgumentException(StringUtils.FormatWith("Enum type {0} is not a set of flags.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      Type underlyingType = Enum.GetUnderlyingType(value.GetType());
      ulong num = Convert.ToUInt64((object) value, (IFormatProvider) CultureInfo.InvariantCulture);
      EnumValues<ulong> namesAndValues = EnumUtils.GetNamesAndValues<T>();
      IList<T> list = (IList<T>) new List<T>();
      foreach (EnumValue<ulong> enumValue in (Collection<EnumValue<ulong>>) namesAndValues)
      {
        if (((long) num & (long) enumValue.Value) == (long) enumValue.Value && (long) enumValue.Value != 0L)
          list.Add((T) Convert.ChangeType((object) enumValue.Value, underlyingType, (IFormatProvider) CultureInfo.CurrentCulture));
      }
      if (list.Count == 0 && Enumerable.SingleOrDefault<EnumValue<ulong>>((IEnumerable<EnumValue<ulong>>) namesAndValues, (Func<EnumValue<ulong>, bool>) (v => (long) v.Value == 0L)) != null)
        list.Add(default (T));
      return list;
    }

    public static EnumValues<ulong> GetNamesAndValues<T>() where T : struct
    {
      return EnumUtils.GetNamesAndValues<ulong>(typeof (T));
    }

    public static EnumValues<TUnderlyingType> GetNamesAndValues<TUnderlyingType>(Type enumType) where TUnderlyingType : struct
    {
      if (enumType == null)
        throw new ArgumentNullException("enumType");
      ValidationUtils.ArgumentTypeIsEnum(enumType, "enumType");
      IList<object> values = EnumUtils.GetValues(enumType);
      IList<string> names = EnumUtils.GetNames(enumType);
      EnumValues<TUnderlyingType> enumValues = new EnumValues<TUnderlyingType>();
      for (int index = 0; index < values.Count; ++index)
      {
        try
        {
          enumValues.Add(new EnumValue<TUnderlyingType>(names[index], (TUnderlyingType) Convert.ChangeType(values[index], typeof (TUnderlyingType), (IFormatProvider) CultureInfo.CurrentCulture)));
        }
        catch (OverflowException ex)
        {
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}", (object) Enum.GetUnderlyingType(enumType), (object) typeof (TUnderlyingType), (object) Convert.ToUInt64(values[index], (IFormatProvider) CultureInfo.InvariantCulture)), (Exception) ex);
        }
      }
      return enumValues;
    }

    public static IList<object> GetValues(Type enumType)
    {
      if (!TypeExtensions.IsEnum(enumType))
        throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
      List<object> list = new List<object>();
      foreach (FieldInfo fieldInfo in Enumerable.Where<FieldInfo>((IEnumerable<FieldInfo>) enumType.GetFields(), (Func<FieldInfo, bool>) (field => field.IsLiteral)))
      {
        object obj = fieldInfo.GetValue((object) enumType);
        list.Add(obj);
      }
      return (IList<object>) list;
    }

    public static IList<string> GetNames(Type enumType)
    {
      if (!TypeExtensions.IsEnum(enumType))
        throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
      List<string> list = new List<string>();
      foreach (FieldInfo fieldInfo in Enumerable.Where<FieldInfo>((IEnumerable<FieldInfo>) enumType.GetFields(), (Func<FieldInfo, bool>) (field => field.IsLiteral)))
        list.Add(fieldInfo.Name);
      return (IList<string>) list;
    }
  }
}
