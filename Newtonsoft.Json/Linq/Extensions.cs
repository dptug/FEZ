// Type: Newtonsoft.Json.Linq.Extensions
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  public static class Extensions
  {
    public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, "source");
      return Extensions.AsJEnumerable(Enumerable.SelectMany<T, JToken>(source, (Func<T, IEnumerable<JToken>>) (j => j.Ancestors())));
    }

    public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
    {
      ValidationUtils.ArgumentNotNull((object) source, "source");
      return Extensions.AsJEnumerable(Enumerable.SelectMany<T, JToken>(source, (Func<T, IEnumerable<JToken>>) (j => j.Descendants())));
    }

    public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
    {
      ValidationUtils.ArgumentNotNull((object) source, "source");
      return Extensions.AsJEnumerable<JProperty>(Enumerable.SelectMany<JObject, JProperty>(source, (Func<JObject, IEnumerable<JProperty>>) (d => d.Properties())));
    }

    public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object key)
    {
      return Extensions.AsJEnumerable(Extensions.Values<JToken, JToken>(source, key));
    }

    public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
    {
      return Extensions.Values(source, (object) null);
    }

    public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key)
    {
      return Extensions.Values<JToken, U>(source, key);
    }

    public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source)
    {
      return Extensions.Values<JToken, U>(source, (object) null);
    }

    public static U Value<U>(this IEnumerable<JToken> value)
    {
      return Extensions.Value<JToken, U>(value);
    }

    public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) value, "source");
      JToken token = value as JToken;
      if (token == null)
        throw new ArgumentException("Source value must be a JToken.");
      else
        return Extensions.Convert<JToken, U>(token);
    }

    internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object key) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, "source");
      foreach (T obj in source)
      {
        JToken token = (JToken) obj;
        if (key == null)
        {
          if (token is JValue)
          {
            yield return Extensions.Convert<JValue, U>((JValue) token);
          }
          else
          {
            foreach (JToken token1 in token.Children())
              yield return Extensions.Convert<JToken, U>(token1);
          }
        }
        else
        {
          JToken value = token[key];
          if (value != null)
            yield return Extensions.Convert<JToken, U>(value);
        }
      }
    }

    public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken
    {
      return Extensions.AsJEnumerable(Extensions.Children<T, JToken>(source));
    }

    public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, "source");
      return Extensions.Convert<JToken, U>(Enumerable.SelectMany<T, JToken>(source, (Func<T, IEnumerable<JToken>>) (c => (IEnumerable<JToken>) c.Children())));
    }

    internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, "source");
      foreach (T obj in source)
        yield return Extensions.Convert<JToken, U>((JToken) obj);
    }

    internal static U Convert<T, U>(this T token) where T : JToken
    {
      if ((object) token == null)
        return default (U);
      if ((object) token is U && typeof (U) != typeof (IComparable) && typeof (U) != typeof (IFormattable))
        return (U) (object) token;
      JValue jvalue = (object) token as JValue;
      if (jvalue == null)
        throw new InvalidCastException(StringUtils.FormatWith("Cannot cast {0} to {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) token.GetType(), (object) typeof (T)));
      if (jvalue.Value is U)
        return (U) jvalue.Value;
      Type type = typeof (U);
      if (ReflectionUtils.IsNullableType(type))
      {
        if (jvalue.Value == null)
          return default (U);
        type = Nullable.GetUnderlyingType(type);
      }
      return (U) Convert.ChangeType(jvalue.Value, type, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
    {
      return Extensions.AsJEnumerable<JToken>(source);
    }

    public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
    {
      if (source == null)
        return (IJEnumerable<T>) null;
      if (source is IJEnumerable<T>)
        return (IJEnumerable<T>) source;
      else
        return (IJEnumerable<T>) new JEnumerable<T>(source);
    }
  }
}
