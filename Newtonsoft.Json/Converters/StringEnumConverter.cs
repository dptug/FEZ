// Type: Newtonsoft.Json.Converters.StringEnumConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Converters
{
  public class StringEnumConverter : JsonConverter
  {
    private readonly Dictionary<Type, BidirectionalDictionary<string, string>> _enumMemberNamesPerType = new Dictionary<Type, BidirectionalDictionary<string, string>>();

    public bool CamelCaseText { get; set; }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        Enum @enum = (Enum) value;
        string first = @enum.ToString("G");
        if (char.IsNumber(first[0]) || (int) first[0] == 45)
        {
          writer.WriteValue(value);
        }
        else
        {
          string second;
          this.GetEnumNameMap(@enum.GetType()).TryGetByFirst(first, out second);
          second = second ?? first;
          if (this.CamelCaseText)
            second = string.Join(", ", Enumerable.ToArray<string>(Enumerable.Select<string, string>((IEnumerable<string>) second.Split(new char[1]
            {
              ','
            }), (Func<string, string>) (item => StringUtils.ToCamelCase(item.Trim())))));
          writer.WriteValue(second);
        }
      }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullableType(objectType))
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot convert null value to {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        else
          return (object) null;
      }
      else if (reader.TokenType == JsonToken.String)
      {
        string first;
        this.GetEnumNameMap(type).TryGetBySecond(reader.Value.ToString(), out first);
        string str = first ?? reader.Value.ToString();
        return Enum.Parse(type, str, true);
      }
      else if (reader.TokenType == JsonToken.Integer)
        return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
      else
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected token when parsing enum. Expected String or Integer, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    private BidirectionalDictionary<string, string> GetEnumNameMap(Type t)
    {
      BidirectionalDictionary<string, string> bidirectionalDictionary;
      if (!this._enumMemberNamesPerType.TryGetValue(t, out bidirectionalDictionary))
      {
        lock (this._enumMemberNamesPerType)
        {
          if (this._enumMemberNamesPerType.TryGetValue(t, out bidirectionalDictionary))
            return bidirectionalDictionary;
          bidirectionalDictionary = new BidirectionalDictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
          foreach (FieldInfo item_0 in t.GetFields())
          {
            string local_2 = item_0.Name;
            string local_3 = item_0.Name;
            string local_4;
            if (bidirectionalDictionary.TryGetBySecond(local_3, out local_4))
              throw new InvalidOperationException(StringUtils.FormatWith("Enum name '{0}' already exists on enum '{1}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) local_3, (object) t.Name));
            bidirectionalDictionary.Set(local_2, local_3);
          }
          this._enumMemberNamesPerType[t] = bidirectionalDictionary;
        }
      }
      return bidirectionalDictionary;
    }

    public override bool CanConvert(Type objectType)
    {
      return TypeExtensions.IsEnum(ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType);
    }
  }
}
