// Type: Newtonsoft.Json.JsonConvert
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Newtonsoft.Json
{
  public static class JsonConvert
  {
    public static readonly string True = "true";
    public static readonly string False = "false";
    public static readonly string Null = "null";
    public static readonly string Undefined = "undefined";
    public static readonly string PositiveInfinity = "Infinity";
    public static readonly string NegativeInfinity = "-Infinity";
    public static readonly string NaN = "NaN";
    internal static readonly long InitialJavaScriptDateTicks = 621355968000000000L;

    static JsonConvert()
    {
    }

    public static string ToString(DateTime value)
    {
      return JsonConvert.ToString(value, DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.RoundtripKind);
    }

    public static string ToString(DateTime value, DateFormatHandling format, DateTimeZoneHandling timeZoneHandling)
    {
      DateTime d = JsonConvert.EnsureDateTime(value, timeZoneHandling);
      using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
      {
        JsonConvert.WriteDateTimeString((TextWriter) stringWriter, d, DateTimeUtils.GetUtcOffset(d), d.Kind, format);
        return stringWriter.ToString();
      }
    }

    internal static DateTime EnsureDateTime(DateTime value, DateTimeZoneHandling timeZone)
    {
      switch (timeZone)
      {
        case DateTimeZoneHandling.Local:
          value = JsonConvert.SwitchToLocalTime(value);
          goto case 3;
        case DateTimeZoneHandling.Utc:
          value = JsonConvert.SwitchToUtcTime(value);
          goto case 3;
        case DateTimeZoneHandling.Unspecified:
          value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
          goto case 3;
        case DateTimeZoneHandling.RoundtripKind:
          return value;
        default:
          throw new ArgumentException("Invalid date time handling value.");
      }
    }

    internal static void WriteDateTimeString(TextWriter writer, DateTime value, DateFormatHandling format)
    {
      JsonConvert.WriteDateTimeString(writer, value, DateTimeUtils.GetUtcOffset(value), value.Kind, format);
    }

    internal static void WriteDateTimeString(TextWriter writer, DateTime value, TimeSpan offset, DateTimeKind kind, DateFormatHandling format)
    {
      if (format == DateFormatHandling.MicrosoftDateFormat)
      {
        long num = JsonConvert.ConvertDateTimeToJavaScriptTicks(value, offset);
        writer.Write("\"\\/Date(");
        writer.Write(num);
        switch (kind)
        {
          case DateTimeKind.Unspecified:
            if (value != DateTime.MaxValue && value != DateTime.MinValue)
            {
              JsonConvert.WriteDateTimeOffset(writer, offset, format);
              break;
            }
            else
              break;
          case DateTimeKind.Local:
            JsonConvert.WriteDateTimeOffset(writer, offset, format);
            break;
        }
        writer.Write(")\\/\"");
      }
      else
      {
        writer.Write("\"");
        writer.Write(value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFF", (IFormatProvider) CultureInfo.InvariantCulture));
        switch (kind)
        {
          case DateTimeKind.Utc:
            writer.Write("Z");
            break;
          case DateTimeKind.Local:
            JsonConvert.WriteDateTimeOffset(writer, offset, format);
            break;
        }
        writer.Write("\"");
      }
    }

    internal static void WriteDateTimeOffset(TextWriter writer, TimeSpan offset, DateFormatHandling format)
    {
      writer.Write(offset.Ticks >= 0L ? "+" : "-");
      int num1 = Math.Abs(offset.Hours);
      if (num1 < 10)
        writer.Write(0);
      writer.Write(num1);
      if (format == DateFormatHandling.IsoDateFormat)
        writer.Write(':');
      int num2 = Math.Abs(offset.Minutes);
      if (num2 < 10)
        writer.Write(0);
      writer.Write(num2);
    }

    private static long ToUniversalTicks(DateTime dateTime)
    {
      if (dateTime.Kind == DateTimeKind.Utc)
        return dateTime.Ticks;
      else
        return JsonConvert.ToUniversalTicks(dateTime, DateTimeUtils.GetUtcOffset(dateTime));
    }

    private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
    {
      if (dateTime.Kind == DateTimeKind.Utc || dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
        return dateTime.Ticks;
      long num = dateTime.Ticks - offset.Ticks;
      if (num > 3155378975999999999L)
        return 3155378975999999999L;
      if (num < 0L)
        return 0L;
      else
        return num;
    }

    internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
    {
      return JsonConvert.UniversialTicksToJavaScriptTicks(JsonConvert.ToUniversalTicks(dateTime, offset));
    }

    internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
    {
      return JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTime, true);
    }

    internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
    {
      return JsonConvert.UniversialTicksToJavaScriptTicks(convertToUtc ? JsonConvert.ToUniversalTicks(dateTime) : dateTime.Ticks);
    }

    private static long UniversialTicksToJavaScriptTicks(long universialTicks)
    {
      return (universialTicks - JsonConvert.InitialJavaScriptDateTicks) / 10000L;
    }

    internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
    {
      return new DateTime(javaScriptTicks * 10000L + JsonConvert.InitialJavaScriptDateTicks, DateTimeKind.Utc);
    }

    private static DateTime SwitchToLocalTime(DateTime value)
    {
      switch (value.Kind)
      {
        case DateTimeKind.Unspecified:
          return new DateTime(value.Ticks, DateTimeKind.Local);
        case DateTimeKind.Utc:
          return value.ToLocalTime();
        case DateTimeKind.Local:
          return value;
        default:
          return value;
      }
    }

    private static DateTime SwitchToUtcTime(DateTime value)
    {
      switch (value.Kind)
      {
        case DateTimeKind.Unspecified:
          return new DateTime(value.Ticks, DateTimeKind.Utc);
        case DateTimeKind.Utc:
          return value;
        case DateTimeKind.Local:
          return value.ToUniversalTime();
        default:
          return value;
      }
    }

    public static string ToString(bool value)
    {
      if (!value)
        return JsonConvert.False;
      else
        return JsonConvert.True;
    }

    public static string ToString(char value)
    {
      return JsonConvert.ToString(char.ToString(value));
    }

    public static string ToString(Enum value)
    {
      return value.ToString("D");
    }

    public static string ToString(int value)
    {
      return value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string ToString(short value)
    {
      return value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static string ToString(ushort value)
    {
      return value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static string ToString(uint value)
    {
      return value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string ToString(long value)
    {
      return value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static string ToString(ulong value)
    {
      return value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string ToString(float value)
    {
      return JsonConvert.EnsureDecimalPlace((double) value, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static string ToString(double value)
    {
      return JsonConvert.EnsureDecimalPlace(value, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));
    }

    private static string EnsureDecimalPlace(double value, string text)
    {
      if (double.IsNaN(value) || double.IsInfinity(value) || (text.IndexOf('.') != -1 || text.IndexOf('E') != -1) || text.IndexOf('e') != -1)
        return text;
      else
        return text + ".0";
    }

    private static string EnsureDecimalPlace(string text)
    {
      if (text.IndexOf('.') != -1)
        return text;
      else
        return text + ".0";
    }

    public static string ToString(byte value)
    {
      return value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static string ToString(sbyte value)
    {
      return value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string ToString(Decimal value)
    {
      return JsonConvert.EnsureDecimalPlace(value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static string ToString(Guid value)
    {
      return (string) (object) '"' + (object) value.ToString("D", (IFormatProvider) CultureInfo.InvariantCulture) + (string) (object) '"';
    }

    public static string ToString(TimeSpan value)
    {
      return (string) (object) '"' + (object) value.ToString() + (string) (object) '"';
    }

    public static string ToString(Uri value)
    {
      if (value == (Uri) null)
        return JsonConvert.Null;
      else
        return JsonConvert.ToString(value.ToString());
    }

    public static string ToString(string value)
    {
      return JsonConvert.ToString(value, '"');
    }

    public static string ToString(string value, char delimter)
    {
      return JavaScriptUtils.ToEscapedJavaScriptString(value, delimter, true);
    }

    public static string ToString(object value)
    {
      if (value == null)
        return JsonConvert.Null;
      IConvertible convertible = ConvertUtils.ToConvertible(value);
      if (convertible != null)
      {
        switch (convertible.GetTypeCode())
        {
          case TypeCode.DBNull:
            return JsonConvert.Null;
          case TypeCode.Boolean:
            return JsonConvert.ToString(convertible.ToBoolean((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.Char:
            return JsonConvert.ToString(convertible.ToChar((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.SByte:
            return JsonConvert.ToString(convertible.ToSByte((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.Byte:
            return JsonConvert.ToString(convertible.ToByte((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.Int16:
            return JsonConvert.ToString(convertible.ToInt16((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.UInt16:
            return JsonConvert.ToString(convertible.ToUInt16((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.Int32:
            return JsonConvert.ToString(convertible.ToInt32((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.UInt32:
            return JsonConvert.ToString(convertible.ToUInt32((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.Int64:
            return JsonConvert.ToString(convertible.ToInt64((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.UInt64:
            return JsonConvert.ToString(convertible.ToUInt64((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.Single:
            return JsonConvert.ToString(convertible.ToSingle((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.Double:
            return JsonConvert.ToString(convertible.ToDouble((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.Decimal:
            return JsonConvert.ToString(convertible.ToDecimal((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.DateTime:
            return JsonConvert.ToString(convertible.ToDateTime((IFormatProvider) CultureInfo.InvariantCulture));
          case TypeCode.String:
            return JsonConvert.ToString(convertible.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        }
      }
      else
      {
        if (value is Guid)
          return JsonConvert.ToString((Guid) value);
        if (value is Uri)
          return JsonConvert.ToString((Uri) value);
        if (value is TimeSpan)
          return JsonConvert.ToString((TimeSpan) value);
      }
      throw new ArgumentException(StringUtils.FormatWith("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.", (IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
    }

    private static bool IsJsonPrimitiveTypeCode(TypeCode typeCode)
    {
      switch (typeCode)
      {
        case TypeCode.DBNull:
        case TypeCode.Boolean:
        case TypeCode.Char:
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
        case TypeCode.DateTime:
        case TypeCode.String:
          return true;
        default:
          return false;
      }
    }

    internal static bool IsJsonPrimitiveType(Type type)
    {
      if (ReflectionUtils.IsNullableType(type))
        type = Nullable.GetUnderlyingType(type);
      if (type == typeof (byte[]) || type == typeof (Uri) || (type == typeof (TimeSpan) || type == typeof (Guid)))
        return true;
      else
        return JsonConvert.IsJsonPrimitiveTypeCode(ConvertUtils.GetTypeCode(type));
    }

    public static string SerializeObject(object value)
    {
      return JsonConvert.SerializeObject(value, Formatting.None, (JsonSerializerSettings) null);
    }

    public static string SerializeObject(object value, Formatting formatting)
    {
      return JsonConvert.SerializeObject(value, formatting, (JsonSerializerSettings) null);
    }

    public static string SerializeObject(object value, params JsonConverter[] converters)
    {
      return JsonConvert.SerializeObject(value, Formatting.None, converters);
    }

    public static string SerializeObject(object value, Formatting formatting, params JsonConverter[] converters)
    {
      JsonSerializerSettings serializerSettings;
      if (converters == null || converters.Length <= 0)
        serializerSettings = (JsonSerializerSettings) null;
      else
        serializerSettings = new JsonSerializerSettings()
        {
          Converters = (IList<JsonConverter>) converters
        };
      JsonSerializerSettings settings = serializerSettings;
      return JsonConvert.SerializeObject(value, formatting, settings);
    }

    public static string SerializeObject(object value, JsonSerializerSettings settings)
    {
      return JsonConvert.SerializeObject(value, Formatting.None, settings);
    }

    public static string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings)
    {
      JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
      StringWriter stringWriter = new StringWriter(new StringBuilder(256), (IFormatProvider) CultureInfo.InvariantCulture);
      using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
      {
        jsonTextWriter.Formatting = formatting;
        jsonSerializer.Serialize((JsonWriter) jsonTextWriter, value);
      }
      return stringWriter.ToString();
    }

    public static object DeserializeObject(string value)
    {
      return JsonConvert.DeserializeObject(value, (Type) null, (JsonSerializerSettings) null);
    }

    public static object DeserializeObject(string value, JsonSerializerSettings settings)
    {
      return JsonConvert.DeserializeObject(value, (Type) null, settings);
    }

    public static object DeserializeObject(string value, Type type)
    {
      return JsonConvert.DeserializeObject(value, type, (JsonSerializerSettings) null);
    }

    public static T DeserializeObject<T>(string value)
    {
      return JsonConvert.DeserializeObject<T>(value, (JsonSerializerSettings) null);
    }

    public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
    {
      return JsonConvert.DeserializeObject<T>(value);
    }

    public static T DeserializeObject<T>(string value, params JsonConverter[] converters)
    {
      return (T) JsonConvert.DeserializeObject(value, typeof (T), converters);
    }

    public static T DeserializeObject<T>(string value, JsonSerializerSettings settings)
    {
      return (T) JsonConvert.DeserializeObject(value, typeof (T), settings);
    }

    public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
    {
      JsonSerializerSettings serializerSettings;
      if (converters == null || converters.Length <= 0)
        serializerSettings = (JsonSerializerSettings) null;
      else
        serializerSettings = new JsonSerializerSettings()
        {
          Converters = (IList<JsonConverter>) converters
        };
      JsonSerializerSettings settings = serializerSettings;
      return JsonConvert.DeserializeObject(value, type, settings);
    }

    public static object DeserializeObject(string value, Type type, JsonSerializerSettings settings)
    {
      ValidationUtils.ArgumentNotNull((object) value, "value");
      StringReader stringReader = new StringReader(value);
      JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
      if (!jsonSerializer.IsCheckAdditionalContentSet())
        jsonSerializer.CheckAdditionalContent = true;
      return jsonSerializer.Deserialize((JsonReader) new JsonTextReader((TextReader) stringReader), type);
    }

    public static void PopulateObject(string value, object target)
    {
      JsonConvert.PopulateObject(value, target, (JsonSerializerSettings) null);
    }

    public static void PopulateObject(string value, object target, JsonSerializerSettings settings)
    {
      StringReader stringReader = new StringReader(value);
      using (JsonReader reader = (JsonReader) new JsonTextReader((TextReader) stringReader))
      {
        JsonSerializer.Create(settings).Populate(reader, target);
        if (reader.Read() && reader.TokenType != JsonToken.Comment)
          throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
      }
    }

    public static string SerializeXmlNode(XmlNode node)
    {
      return JsonConvert.SerializeXmlNode(node, Formatting.None);
    }

    public static string SerializeXmlNode(XmlNode node, Formatting formatting)
    {
      XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
      return JsonConvert.SerializeObject((object) node, formatting, new JsonConverter[1]
      {
        (JsonConverter) xmlNodeConverter
      });
    }

    public static string SerializeXmlNode(XmlNode node, Formatting formatting, bool omitRootObject)
    {
      XmlNodeConverter xmlNodeConverter = new XmlNodeConverter()
      {
        OmitRootObject = omitRootObject
      };
      return JsonConvert.SerializeObject((object) node, formatting, new JsonConverter[1]
      {
        (JsonConverter) xmlNodeConverter
      });
    }

    public static XmlDocument DeserializeXmlNode(string value)
    {
      return JsonConvert.DeserializeXmlNode(value, (string) null);
    }

    public static XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName)
    {
      return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, false);
    }

    public static XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
    {
      return (XmlDocument) JsonConvert.DeserializeObject(value, typeof (XmlDocument), new JsonConverter[1]
      {
        (JsonConverter) new XmlNodeConverter()
        {
          DeserializeRootElementName = deserializeRootElementName,
          WriteArrayAttribute = writeArrayAttribute
        }
      });
    }
  }
}
