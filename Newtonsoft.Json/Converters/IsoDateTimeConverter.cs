// Type: Newtonsoft.Json.Converters.IsoDateTimeConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class IsoDateTimeConverter : DateTimeConverterBase
  {
    private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
    private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
    private string _dateTimeFormat;
    private CultureInfo _culture;

    public DateTimeStyles DateTimeStyles
    {
      get
      {
        return this._dateTimeStyles;
      }
      set
      {
        this._dateTimeStyles = value;
      }
    }

    public string DateTimeFormat
    {
      get
      {
        return this._dateTimeFormat ?? string.Empty;
      }
      set
      {
        this._dateTimeFormat = StringUtils.NullEmptyString(value);
      }
    }

    public CultureInfo Culture
    {
      get
      {
        return this._culture ?? CultureInfo.CurrentCulture;
      }
      set
      {
        this._culture = value;
      }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (!(value is DateTime))
        throw new JsonSerializationException(StringUtils.FormatWith("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) ReflectionUtils.GetObjectType(value)));
      DateTime dateTime = (DateTime) value;
      if ((this._dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (this._dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
        dateTime = dateTime.ToUniversalTime();
      string str = dateTime.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", (IFormatProvider) this.Culture);
      writer.WriteValue(str);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      bool flag = ReflectionUtils.IsNullableType(objectType);
      if (flag)
        Nullable.GetUnderlyingType(objectType);
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullableType(objectType))
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot convert null value to {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        else
          return (object) null;
      }
      else
      {
        if (reader.TokenType == JsonToken.Date)
          return reader.Value;
        if (reader.TokenType != JsonToken.String)
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected token parsing date. Expected String, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        string s = reader.Value.ToString();
        if (string.IsNullOrEmpty(s) && flag)
          return (object) null;
        if (!string.IsNullOrEmpty(this._dateTimeFormat))
          return (object) DateTime.ParseExact(s, this._dateTimeFormat, (IFormatProvider) this.Culture, this._dateTimeStyles);
        else
          return (object) DateTime.Parse(s, (IFormatProvider) this.Culture, this._dateTimeStyles);
      }
    }
  }
}
