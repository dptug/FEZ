// Type: Newtonsoft.Json.JsonWriterException
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
  [Serializable]
  public class JsonWriterException : JsonException
  {
    public string Path { get; private set; }

    public JsonWriterException()
    {
    }

    public JsonWriterException(string message)
      : base(message)
    {
    }

    public JsonWriterException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonWriterException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal JsonWriterException(string message, Exception innerException, string path)
      : base(message, innerException)
    {
      this.Path = path;
    }

    internal static JsonWriterException Create(JsonWriter writer, string message, Exception ex)
    {
      return JsonWriterException.Create(writer.ContainerPath, message, ex);
    }

    internal static JsonWriterException Create(string path, string message, Exception ex)
    {
      message = JsonException.FormatExceptionMessage((IJsonLineInfo) null, path, message);
      return new JsonWriterException(message, ex, path);
    }
  }
}
