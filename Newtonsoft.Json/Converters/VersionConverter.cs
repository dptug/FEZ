// Type: Newtonsoft.Json.Converters.VersionConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class VersionConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        if (!(value is Version))
          throw new JsonSerializationException("Expected Version object value");
        writer.WriteValue(value.ToString());
      }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      if (reader.TokenType != JsonToken.String)
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected token or value when parsing version. Token: {0}, Value: {1}", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType, reader.Value));
      try
      {
        return (object) new Version((string) reader.Value);
      }
      catch (Exception ex)
      {
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Error parsing version string: {0}", (IFormatProvider) CultureInfo.InvariantCulture, reader.Value), ex);
      }
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (Version);
    }
  }
}
