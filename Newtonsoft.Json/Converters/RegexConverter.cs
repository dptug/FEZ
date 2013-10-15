// Type: Newtonsoft.Json.Converters.RegexConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Converters
{
  public class RegexConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      Regex regex = (Regex) value;
      BsonWriter writer1 = writer as BsonWriter;
      if (writer1 != null)
        this.WriteBson(writer1, regex);
      else
        this.WriteJson(writer, regex);
    }

    private bool HasFlag(RegexOptions options, RegexOptions flag)
    {
      return (options & flag) == flag;
    }

    private void WriteBson(BsonWriter writer, Regex regex)
    {
      string str = (string) null;
      if (this.HasFlag(regex.Options, RegexOptions.IgnoreCase))
        str = str + "i";
      if (this.HasFlag(regex.Options, RegexOptions.Multiline))
        str = str + "m";
      if (this.HasFlag(regex.Options, RegexOptions.Singleline))
        str = str + "s";
      string options = str + "u";
      if (this.HasFlag(regex.Options, RegexOptions.ExplicitCapture))
        options = options + "x";
      writer.WriteRegex(regex.ToString(), options);
    }

    private void WriteJson(JsonWriter writer, Regex regex)
    {
      writer.WriteStartObject();
      writer.WritePropertyName("Pattern");
      writer.WriteValue(regex.ToString());
      writer.WritePropertyName("Options");
      writer.WriteValue((object) regex.Options);
      writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      BsonReader reader1 = reader as BsonReader;
      if (reader1 != null)
        return this.ReadBson(reader1);
      else
        return (object) this.ReadJson(reader);
    }

    private object ReadBson(BsonReader reader)
    {
      string str1 = (string) reader.Value;
      int num = str1.LastIndexOf('/');
      string pattern = str1.Substring(1, num - 1);
      string str2 = str1.Substring(num + 1);
      RegexOptions options = RegexOptions.None;
      foreach (char ch in str2)
      {
        switch (ch)
        {
          case 's':
            options |= RegexOptions.Singleline;
            break;
          case 'x':
            options |= RegexOptions.ExplicitCapture;
            break;
          case 'i':
            options |= RegexOptions.IgnoreCase;
            break;
          case 'm':
            options |= RegexOptions.Multiline;
            break;
        }
      }
      return (object) new Regex(pattern, options);
    }

    private Regex ReadJson(JsonReader reader)
    {
      reader.Read();
      reader.Read();
      string pattern = (string) reader.Value;
      reader.Read();
      reader.Read();
      int num = Convert.ToInt32(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      reader.Read();
      return new Regex(pattern, (RegexOptions) num);
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (Regex);
    }
  }
}
