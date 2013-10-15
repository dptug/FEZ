// Type: Newtonsoft.Json.Converters.JavaScriptDateTimeConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class JavaScriptDateTimeConverter : DateTimeConverterBase
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (!(value is DateTime))
        throw new JsonSerializationException("Expected date object value.");
      long num = JsonConvert.ConvertDateTimeToJavaScriptTicks(((DateTime) value).ToUniversalTime());
      writer.WriteStartConstructor("Date");
      writer.WriteValue(num);
      writer.WriteEndConstructor();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (ReflectionUtils.IsNullableType(objectType))
        Nullable.GetUnderlyingType(objectType);
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullable(objectType))
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot convert null value to {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        else
          return (object) null;
      }
      else
      {
        if (reader.TokenType != JsonToken.StartConstructor || !string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected token or value when parsing date. Token: {0}, Value: {1}", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType, reader.Value));
        reader.Read();
        if (reader.TokenType != JsonToken.Integer)
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected token parsing date. Expected Integer, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        DateTime dateTime = JsonConvert.ConvertJavaScriptTicksToDateTime((long) reader.Value);
        reader.Read();
        if (reader.TokenType != JsonToken.EndConstructor)
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected token parsing date. Expected EndConstructor, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        else
          return (object) dateTime;
      }
    }
  }
}
