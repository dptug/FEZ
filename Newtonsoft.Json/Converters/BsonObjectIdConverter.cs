// Type: Newtonsoft.Json.Converters.BsonObjectIdConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class BsonObjectIdConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      BsonObjectId bsonObjectId = (BsonObjectId) value;
      BsonWriter bsonWriter = writer as BsonWriter;
      if (bsonWriter != null)
        bsonWriter.WriteObjectId(bsonObjectId.Value);
      else
        writer.WriteValue(bsonObjectId.Value);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType != JsonToken.Bytes)
        throw new JsonSerializationException(StringUtils.FormatWith("Expected Bytes but got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      else
        return (object) new BsonObjectId((byte[]) reader.Value);
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (BsonObjectId);
    }
  }
}
