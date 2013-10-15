// Type: Newtonsoft.Json.Utilities.StringUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
  internal static class StringUtils
  {
    public const string CarriageReturnLineFeed = "\r\n";
    public const string Empty = "";
    public const char CarriageReturn = '\r';
    public const char LineFeed = '\n';
    public const char Tab = '\t';

    public static string FormatWith(this string format, IFormatProvider provider, object arg0)
    {
      return StringUtils.FormatWith(format, provider, new object[1]
      {
        arg0
      });
    }

    public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1)
    {
      return StringUtils.FormatWith(format, provider, new object[2]
      {
        arg0,
        arg1
      });
    }

    public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2)
    {
      return StringUtils.FormatWith(format, provider, new object[3]
      {
        arg0,
        arg1,
        arg2
      });
    }

    public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
    {
      ValidationUtils.ArgumentNotNull((object) format, "format");
      return string.Format(provider, format, args);
    }

    public static bool IsWhiteSpace(string s)
    {
      if (s == null)
        throw new ArgumentNullException("s");
      if (s.Length == 0)
        return false;
      for (int index = 0; index < s.Length; ++index)
      {
        if (!char.IsWhiteSpace(s[index]))
          return false;
      }
      return true;
    }

    public static string NullEmptyString(string s)
    {
      if (!string.IsNullOrEmpty(s))
        return s;
      else
        return (string) null;
    }

    public static StringWriter CreateStringWriter(int capacity)
    {
      return new StringWriter(new StringBuilder(capacity), (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static int? GetLength(string value)
    {
      if (value == null)
        return new int?();
      else
        return new int?(value.Length);
    }

    public static void ToCharAsUnicode(char c, char[] buffer)
    {
      buffer[0] = '\\';
      buffer[1] = 'u';
      buffer[2] = MathUtils.IntToHex((int) c >> 12 & 15);
      buffer[3] = MathUtils.IntToHex((int) c >> 8 & 15);
      buffer[4] = MathUtils.IntToHex((int) c >> 4 & 15);
      buffer[5] = MathUtils.IntToHex((int) c & 15);
    }

    public static TSource ForgivingCaseSensitiveFind<TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (valueSelector == null)
        throw new ArgumentNullException("valueSelector");
      IEnumerable<TSource> source1 = Enumerable.Where<TSource>(source, (Func<TSource, bool>) (s => string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase)));
      if (Enumerable.Count<TSource>(source1) <= 1)
        return Enumerable.SingleOrDefault<TSource>(source1);
      else
        return Enumerable.SingleOrDefault<TSource>(Enumerable.Where<TSource>(source, (Func<TSource, bool>) (s => string.Equals(valueSelector(s), testValue, StringComparison.Ordinal))));
    }

    public static string ToCamelCase(string s)
    {
      if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
        return s;
      string str = char.ToLower(s[0], CultureInfo.InvariantCulture).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      if (s.Length > 1)
        str = str + s.Substring(1);
      return str;
    }

    public static bool IsHighSurrogate(char c)
    {
      return char.IsHighSurrogate(c);
    }

    public static bool IsLowSurrogate(char c)
    {
      return char.IsLowSurrogate(c);
    }
  }
}
