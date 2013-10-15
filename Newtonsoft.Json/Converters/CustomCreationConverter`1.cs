// Type: Newtonsoft.Json.Converters.CustomCreationConverter`1
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using System;

namespace Newtonsoft.Json.Converters
{
  public abstract class CustomCreationConverter<T> : JsonConverter
  {
    public override bool CanWrite
    {
      get
      {
        return false;
      }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      T obj = this.Create(objectType);
      if ((object) obj == null)
        throw new JsonSerializationException("No object created.");
      serializer.Populate(reader, (object) obj);
      return (object) obj;
    }

    public abstract T Create(Type objectType);

    public override bool CanConvert(Type objectType)
    {
      return typeof (T).IsAssignableFrom(objectType);
    }
  }
}
