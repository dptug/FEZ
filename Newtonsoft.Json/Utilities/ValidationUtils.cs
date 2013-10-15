// Type: Newtonsoft.Json.Utilities.ValidationUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
  internal static class ValidationUtils
  {
    public static void ArgumentNotNullOrEmpty(string value, string parameterName)
    {
      if (value == null)
        throw new ArgumentNullException(parameterName);
      if (value.Length == 0)
        throw new ArgumentException(StringUtils.FormatWith("'{0}' cannot be empty.", (IFormatProvider) CultureInfo.InvariantCulture, (object) parameterName), parameterName);
    }

    public static void ArgumentTypeIsEnum(Type enumType, string parameterName)
    {
      ValidationUtils.ArgumentNotNull((object) enumType, "enumType");
      if (!TypeExtensions.IsEnum(enumType))
        throw new ArgumentException(StringUtils.FormatWith("Type {0} is not an Enum.", (IFormatProvider) CultureInfo.InvariantCulture, (object) enumType), parameterName);
    }

    public static void ArgumentNotNullOrEmpty<T>(ICollection<T> collection, string parameterName)
    {
      ValidationUtils.ArgumentNotNullOrEmpty<T>(collection, parameterName, StringUtils.FormatWith("Collection '{0}' cannot be empty.", (IFormatProvider) CultureInfo.InvariantCulture, (object) parameterName));
    }

    public static void ArgumentNotNullOrEmpty<T>(ICollection<T> collection, string parameterName, string message)
    {
      if (collection == null)
        throw new ArgumentNullException(parameterName);
      if (collection.Count == 0)
        throw new ArgumentException(message, parameterName);
    }

    public static void ArgumentNotNull(object value, string parameterName)
    {
      if (value == null)
        throw new ArgumentNullException(parameterName);
    }

    public static void ArgumentConditionTrue(bool condition, string parameterName, string message)
    {
      if (!condition)
        throw new ArgumentException(message, parameterName);
    }
  }
}
