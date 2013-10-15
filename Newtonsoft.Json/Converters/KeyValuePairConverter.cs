// Type: Newtonsoft.Json.Converters.KeyValuePairConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Converters
{
  public class KeyValuePairConverter : JsonConverter
  {
    private const string KeyName = "Key";
    private const string ValueName = "Value";

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      Type type = value.GetType();
      PropertyInfo property1 = type.GetProperty("Key");
      PropertyInfo property2 = type.GetProperty("Value");
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      writer.WriteStartObject();
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Key") : "Key");
      serializer.Serialize(writer, ReflectionUtils.GetMemberValue((MemberInfo) property1, value));
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Value") : "Value");
      serializer.Serialize(writer, ReflectionUtils.GetMemberValue((MemberInfo) property2, value));
      writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      bool flag = ReflectionUtils.IsNullableType(objectType);
      if (reader.TokenType == JsonToken.Null)
      {
        if (!flag)
          throw JsonSerializationException.Create(reader, "Cannot convert null value to KeyValuePair.");
        else
          return (object) null;
      }
      else
      {
        Type type = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
        IList<Type> list = (IList<Type>) type.GetGenericArguments();
        Type objectType1 = list[0];
        Type objectType2 = list[1];
        object obj1 = (object) null;
        object obj2 = (object) null;
        reader.Read();
        while (reader.TokenType == JsonToken.PropertyName)
        {
          string a = reader.Value.ToString();
          if (string.Equals(a, "Key", StringComparison.OrdinalIgnoreCase))
          {
            reader.Read();
            obj1 = serializer.Deserialize(reader, objectType1);
          }
          else if (string.Equals(a, "Value", StringComparison.OrdinalIgnoreCase))
          {
            reader.Read();
            obj2 = serializer.Deserialize(reader, objectType2);
          }
          else
            reader.Skip();
          reader.Read();
        }
        return ReflectionUtils.CreateInstance(type, obj1, obj2);
      }
    }

    public override bool CanConvert(Type objectType)
    {
      Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      if (TypeExtensions.IsValueType(type) && TypeExtensions.IsGenericType(type))
        return type.GetGenericTypeDefinition() == typeof (KeyValuePair<,>);
      else
        return false;
    }
  }
}
