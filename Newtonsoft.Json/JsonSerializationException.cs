// Type: Newtonsoft.Json.JsonSerializationException
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
  [Serializable]
  public class JsonSerializationException : JsonException
  {
    public JsonSerializationException()
    {
    }

    public JsonSerializationException(string message)
      : base(message)
    {
    }

    public JsonSerializationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonSerializationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal static JsonSerializationException Create(JsonReader reader, string message)
    {
      return JsonSerializationException.Create(reader, message, (Exception) null);
    }

    internal static JsonSerializationException Create(JsonReader reader, string message, Exception ex)
    {
      return JsonSerializationException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
    }

    internal static JsonSerializationException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
    {
      message = JsonException.FormatExceptionMessage(lineInfo, path, message);
      return new JsonSerializationException(message, ex);
    }
  }
}
