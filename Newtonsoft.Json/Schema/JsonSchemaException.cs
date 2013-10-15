// Type: Newtonsoft.Json.Schema.JsonSchemaException
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
  [Serializable]
  public class JsonSchemaException : JsonException
  {
    public int LineNumber { get; private set; }

    public int LinePosition { get; private set; }

    public string Path { get; private set; }

    public JsonSchemaException()
    {
    }

    public JsonSchemaException(string message)
      : base(message)
    {
    }

    public JsonSchemaException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonSchemaException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition)
      : base(message, innerException)
    {
      this.Path = path;
      this.LineNumber = lineNumber;
      this.LinePosition = linePosition;
    }
  }
}
