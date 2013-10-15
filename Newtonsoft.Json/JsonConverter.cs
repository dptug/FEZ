// Type: Newtonsoft.Json.JsonConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Schema;
using System;

namespace Newtonsoft.Json
{
  public abstract class JsonConverter
  {
    public virtual bool CanRead
    {
      get
      {
        return true;
      }
    }

    public virtual bool CanWrite
    {
      get
      {
        return true;
      }
    }

    public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

    public abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

    public abstract bool CanConvert(Type objectType);

    public virtual JsonSchema GetSchema()
    {
      return (JsonSchema) null;
    }
  }
}
