// Type: Newtonsoft.Json.JsonException
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
  [Serializable]
  public class JsonException : Exception
  {
    public JsonException()
    {
    }

    public JsonException(string message)
      : base(message)
    {
    }

    public JsonException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal static string FormatExceptionMessage(IJsonLineInfo lineInfo, string path, string message)
    {
      if (!message.EndsWith(Environment.NewLine))
      {
        message = message.Trim();
        if (!message.EndsWith("."))
          message = message + ".";
        message = message + " ";
      }
      message = message + StringUtils.FormatWith("Path '{0}'", (IFormatProvider) CultureInfo.InvariantCulture, (object) path);
      if (lineInfo != null && lineInfo.HasLineInfo())
        message = message + StringUtils.FormatWith(", line {0}, position {1}", (IFormatProvider) CultureInfo.InvariantCulture, (object) lineInfo.LineNumber, (object) lineInfo.LinePosition);
      message = message + ".";
      return message;
    }
  }
}
