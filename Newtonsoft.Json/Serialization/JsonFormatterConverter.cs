// Type: Newtonsoft.Json.Serialization.JsonFormatterConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  internal class JsonFormatterConverter : IFormatterConverter
  {
    private readonly JsonSerializer _serializer;

    public JsonFormatterConverter(JsonSerializer serializer)
    {
      ValidationUtils.ArgumentNotNull((object) serializer, "serializer");
      this._serializer = serializer;
    }

    private T GetTokenValue<T>(object value)
    {
      ValidationUtils.ArgumentNotNull(value, "value");
      return (T) Convert.ChangeType(((JValue) value).Value, typeof (T), (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public object Convert(object value, Type type)
    {
      ValidationUtils.ArgumentNotNull(value, "value");
      JToken jtoken = value as JToken;
      if (jtoken == null)
        throw new ArgumentException("Value is not a JToken.", "value");
      else
        return this._serializer.Deserialize(jtoken.CreateReader(), type);
    }

    public object Convert(object value, TypeCode typeCode)
    {
      ValidationUtils.ArgumentNotNull(value, "value");
      if (value is JValue)
        value = ((JValue) value).Value;
      return Convert.ChangeType(value, typeCode, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public bool ToBoolean(object value)
    {
      return this.GetTokenValue<bool>(value);
    }

    public byte ToByte(object value)
    {
      return this.GetTokenValue<byte>(value);
    }

    public char ToChar(object value)
    {
      return this.GetTokenValue<char>(value);
    }

    public DateTime ToDateTime(object value)
    {
      return this.GetTokenValue<DateTime>(value);
    }

    public Decimal ToDecimal(object value)
    {
      return this.GetTokenValue<Decimal>(value);
    }

    public double ToDouble(object value)
    {
      return this.GetTokenValue<double>(value);
    }

    public short ToInt16(object value)
    {
      return this.GetTokenValue<short>(value);
    }

    public int ToInt32(object value)
    {
      return this.GetTokenValue<int>(value);
    }

    public long ToInt64(object value)
    {
      return this.GetTokenValue<long>(value);
    }

    public sbyte ToSByte(object value)
    {
      return this.GetTokenValue<sbyte>(value);
    }

    public float ToSingle(object value)
    {
      return this.GetTokenValue<float>(value);
    }

    public string ToString(object value)
    {
      return this.GetTokenValue<string>(value);
    }

    public ushort ToUInt16(object value)
    {
      return this.GetTokenValue<ushort>(value);
    }

    public uint ToUInt32(object value)
    {
      return this.GetTokenValue<uint>(value);
    }

    public ulong ToUInt64(object value)
    {
      return this.GetTokenValue<ulong>(value);
    }
  }
}
