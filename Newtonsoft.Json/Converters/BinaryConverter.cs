// Type: Newtonsoft.Json.Converters.BinaryConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class BinaryConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        byte[] byteArray = this.GetByteArray(value);
        writer.WriteValue(byteArray);
      }
    }

    private byte[] GetByteArray(object value)
    {
      if (value is SqlBinary)
        return ((SqlBinary) value).Value;
      else
        throw new JsonSerializationException(StringUtils.FormatWith("Unexpected value type when writing binary: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullable(objectType))
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot convert null value to {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        else
          return (object) null;
      }
      else
      {
        byte[] numArray;
        if (reader.TokenType == JsonToken.StartArray)
        {
          numArray = this.ReadByteArray(reader);
        }
        else
        {
          if (reader.TokenType != JsonToken.String)
            throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected token parsing binary. Expected String or StartArray, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
          numArray = Convert.FromBase64String(reader.Value.ToString());
        }
        if (type == typeof (SqlBinary))
          return (object) new SqlBinary(numArray);
        else
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected object type when writing binary: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
      }
    }

    private byte[] ReadByteArray(JsonReader reader)
    {
      List<byte> list = new List<byte>();
      while (reader.Read())
      {
        switch (reader.TokenType)
        {
          case JsonToken.Comment:
            continue;
          case JsonToken.Integer:
            list.Add(Convert.ToByte(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            continue;
          case JsonToken.EndArray:
            return list.ToArray();
          default:
            throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected token when reading bytes: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        }
      }
      throw JsonSerializationException.Create(reader, "Unexpected end when reading bytes.");
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (SqlBinary) || objectType == typeof (SqlBinary?);
    }
  }
}
