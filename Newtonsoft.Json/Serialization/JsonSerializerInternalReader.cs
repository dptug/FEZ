// Type: Newtonsoft.Json.Serialization.JsonSerializerInternalReader
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  internal class JsonSerializerInternalReader : JsonSerializerInternalBase
  {
    private JsonSerializerProxy _internalSerializer;
    private JsonFormatterConverter _formatterConverter;

    public JsonSerializerInternalReader(JsonSerializer serializer)
      : base(serializer)
    {
    }

    public void Populate(JsonReader reader, object target)
    {
      ValidationUtils.ArgumentNotNull(target, "target");
      Type type = target.GetType();
      JsonContract jsonContract = this.Serializer.ContractResolver.ResolveContract(type);
      if (reader.TokenType == JsonToken.None)
        reader.Read();
      if (reader.TokenType == JsonToken.StartArray)
      {
        if (jsonContract.ContractType != JsonContractType.Array)
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot populate JSON array onto type '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type));
        this.PopulateList(CollectionUtils.CreateCollectionWrapper(target), reader, (JsonArrayContract) jsonContract, (JsonProperty) null, (string) null);
      }
      else
      {
        if (reader.TokenType != JsonToken.StartObject)
          throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected initial token '{0}' when populating object. Expected JSON object or array.", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        this.CheckedRead(reader);
        string id = (string) null;
        if (reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$id", StringComparison.Ordinal))
        {
          this.CheckedRead(reader);
          id = reader.Value != null ? reader.Value.ToString() : (string) null;
          this.CheckedRead(reader);
        }
        if (jsonContract.ContractType == JsonContractType.Dictionary)
        {
          this.PopulateDictionary(CollectionUtils.CreateDictionaryWrapper(target), reader, (JsonDictionaryContract) jsonContract, (JsonProperty) null, id);
        }
        else
        {
          if (jsonContract.ContractType != JsonContractType.Object)
            throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot populate JSON object onto type '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type));
          this.PopulateObject(target, reader, (JsonObjectContract) jsonContract, (JsonProperty) null, id);
        }
      }
    }

    private JsonContract GetContractSafe(Type type)
    {
      if (type == null)
        return (JsonContract) null;
      else
        return this.Serializer.ContractResolver.ResolveContract(type);
    }

    public object Deserialize(JsonReader reader, Type objectType, bool checkAdditionalContent)
    {
      if (reader == null)
        throw new ArgumentNullException("reader");
      JsonContract contractSafe = this.GetContractSafe(objectType);
      try
      {
        JsonConverter converter = this.GetConverter(contractSafe, (JsonConverter) null, (JsonContainerContract) null, (JsonProperty) null);
        if (reader.TokenType == JsonToken.None && !this.ReadForType(reader, contractSafe, converter != null))
        {
          if (contractSafe != null && !contractSafe.IsNullable)
            throw JsonSerializationException.Create(reader, StringUtils.FormatWith("No JSON content found and type '{0}' is not nullable.", (IFormatProvider) CultureInfo.InvariantCulture, (object) contractSafe.UnderlyingType));
          else
            return (object) null;
        }
        else
        {
          object obj = converter == null || !converter.CanRead ? this.CreateValueInternal(reader, objectType, contractSafe, (JsonProperty) null, (JsonContainerContract) null, (JsonProperty) null, (object) null) : converter.ReadJson(reader, objectType, (object) null, (JsonSerializer) this.GetInternalSerializer());
          if (checkAdditionalContent && reader.Read() && reader.TokenType != JsonToken.Comment)
            throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
          else
            return obj;
        }
      }
      catch (Exception ex)
      {
        if (this.IsErrorHandled((object) null, contractSafe, (object) null, reader.Path, ex))
        {
          this.HandleError(reader, false, 0);
          return (object) null;
        }
        else
          throw;
      }
    }

    private JsonSerializerProxy GetInternalSerializer()
    {
      if (this._internalSerializer == null)
        this._internalSerializer = new JsonSerializerProxy(this);
      return this._internalSerializer;
    }

    private JsonFormatterConverter GetFormatterConverter()
    {
      if (this._formatterConverter == null)
        this._formatterConverter = new JsonFormatterConverter((JsonSerializer) this.GetInternalSerializer());
      return this._formatterConverter;
    }

    private JToken CreateJToken(JsonReader reader, JsonContract contract)
    {
      ValidationUtils.ArgumentNotNull((object) reader, "reader");
      if (contract != null && contract.UnderlyingType == typeof (JRaw))
        return (JToken) JRaw.Create(reader);
      using (JTokenWriter jtokenWriter = new JTokenWriter())
      {
        jtokenWriter.WriteToken(reader);
        return jtokenWriter.Token;
      }
    }

    private JToken CreateJObject(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, "reader");
      using (JTokenWriter jtokenWriter = new JTokenWriter())
      {
        jtokenWriter.WriteStartObject();
        if (reader.TokenType == JsonToken.PropertyName)
          jtokenWriter.WriteToken(reader, reader.Depth - 1);
        else
          jtokenWriter.WriteEndObject();
        return jtokenWriter.Token;
      }
    }

    private object CreateValueInternal(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue)
    {
      if (contract != null && contract.ContractType == JsonContractType.Linq)
        return (object) this.CreateJToken(reader, contract);
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.StartObject:
            return this.CreateObject(reader, objectType, contract, member, containerContract, containerMember, existingValue);
          case JsonToken.StartArray:
            return this.CreateList(reader, objectType, contract, member, existingValue, (string) null);
          case JsonToken.StartConstructor:
            string str = reader.Value.ToString();
            return this.EnsureType(reader, (object) str, CultureInfo.InvariantCulture, contract, objectType);
          case JsonToken.Comment:
            continue;
          case JsonToken.Raw:
            return (object) new JRaw((object) (string) reader.Value);
          case JsonToken.Integer:
          case JsonToken.Float:
          case JsonToken.Boolean:
          case JsonToken.Date:
          case JsonToken.Bytes:
            return this.EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, contract, objectType);
          case JsonToken.String:
            if (string.IsNullOrEmpty((string) reader.Value) && objectType != typeof (string) && (objectType != typeof (object) && contract != null) && contract.IsNullable)
              return (object) null;
            if (objectType == typeof (byte[]))
              return (object) Convert.FromBase64String((string) reader.Value);
            else
              return this.EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, contract, objectType);
          case JsonToken.Null:
          case JsonToken.Undefined:
            if (objectType == typeof (DBNull))
              return (object) DBNull.Value;
            else
              return this.EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, contract, objectType);
          default:
            throw JsonSerializationException.Create(reader, "Unexpected token while deserializing object: " + (object) reader.TokenType);
        }
      }
      while (reader.Read());
      throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
    }

    internal string GetExpectedDescription(JsonContract contract)
    {
      switch (contract.ContractType)
      {
        case JsonContractType.Object:
        case JsonContractType.Dictionary:
        case JsonContractType.Serializable:
          return "JSON object (e.g. {\"name\":\"value\"})";
        case JsonContractType.Array:
          return "JSON array (e.g. [1,2,3])";
        case JsonContractType.Primitive:
          return "JSON primitive value (e.g. string, number, boolean, null)";
        case JsonContractType.String:
          return "JSON string value";
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private JsonConverter GetConverter(JsonContract contract, JsonConverter memberConverter, JsonContainerContract containerContract, JsonProperty containerProperty)
    {
      JsonConverter jsonConverter = (JsonConverter) null;
      if (memberConverter != null)
        jsonConverter = memberConverter;
      else if (containerProperty != null && containerProperty.ItemConverter != null)
        jsonConverter = containerProperty.ItemConverter;
      else if (containerContract != null && containerContract.ItemConverter != null)
        jsonConverter = containerContract.ItemConverter;
      else if (contract != null)
      {
        if (contract.Converter != null)
        {
          jsonConverter = contract.Converter;
        }
        else
        {
          JsonConverter matchingConverter;
          if ((matchingConverter = this.Serializer.GetMatchingConverter(contract.UnderlyingType)) != null)
            jsonConverter = matchingConverter;
          else if (contract.InternalConverter != null)
            jsonConverter = contract.InternalConverter;
        }
      }
      return jsonConverter;
    }

    private object CreateObject(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue)
    {
      this.CheckedRead(reader);
      object newValue;
      string id;
      if (this.ReadSpecialProperties(reader, ref objectType, ref contract, member, containerContract, containerMember, existingValue, out newValue, out id))
        return newValue;
      if (!this.HasDefinedType(objectType))
        return (object) this.CreateJObject(reader);
      if (contract == null)
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Could not resolve type '{0}' to a JsonContract.", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
      switch (contract.ContractType)
      {
        case JsonContractType.Object:
          bool createdFromNonDefaultConstructor = false;
          JsonObjectContract jsonObjectContract = (JsonObjectContract) contract;
          object newObject = existingValue == null ? this.CreateNewObject(reader, jsonObjectContract, member, containerMember, id, out createdFromNonDefaultConstructor) : existingValue;
          if (createdFromNonDefaultConstructor)
            return newObject;
          else
            return this.PopulateObject(newObject, reader, jsonObjectContract, member, id);
        case JsonContractType.Primitive:
          JsonPrimitiveContract primitiveContract = (JsonPrimitiveContract) contract;
          if (reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$value", StringComparison.Ordinal))
          {
            this.CheckedRead(reader);
            object valueInternal = this.CreateValueInternal(reader, objectType, (JsonContract) primitiveContract, member, (JsonContainerContract) null, (JsonProperty) null, existingValue);
            this.CheckedRead(reader);
            return valueInternal;
          }
          else
            break;
        case JsonContractType.Dictionary:
          JsonDictionaryContract contract1 = (JsonDictionaryContract) contract;
          object dictionary = existingValue == null ? this.CreateNewDictionary(reader, contract1) : existingValue;
          return this.PopulateDictionary(contract1.CreateWrapper(dictionary), reader, contract1, member, id);
        case JsonContractType.Serializable:
          JsonISerializableContract contract2 = (JsonISerializableContract) contract;
          return this.CreateISerializable(reader, contract2, id);
      }
      throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot deserialize the current JSON object (e.g. {{\"name\":\"value\"}}) into type '{0}' because the type requires a {1} to deserialize correctly.\r\nTo fix this error either change the JSON to a {1} or change the deserialized type so that it is a normal .NET type (e.g. not a primitive type like integer, not a collection type like an array or List<T>) that can be deserialized from a JSON object. JsonObjectAttribute can also be added to the type to force it to deserialize from a JSON object.\r\n", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectType, (object) this.GetExpectedDescription(contract)));
    }

    private bool ReadSpecialProperties(JsonReader reader, ref Type objectType, ref JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue, out object newValue, out string id)
    {
      id = (string) null;
      newValue = (object) null;
      if (reader.TokenType == JsonToken.PropertyName)
      {
        string str = reader.Value.ToString();
        if (str.Length > 0 && (int) str[0] == 36)
        {
          bool flag;
          do
          {
            string a = reader.Value.ToString();
            if (string.Equals(a, "$ref", StringComparison.Ordinal))
            {
              this.CheckedRead(reader);
              if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null)
                throw JsonSerializationException.Create(reader, StringUtils.FormatWith("JSON reference {0} property must have a string or null value.", (IFormatProvider) CultureInfo.InvariantCulture, (object) "$ref"));
              string reference = reader.Value != null ? reader.Value.ToString() : (string) null;
              this.CheckedRead(reader);
              if (reference != null)
              {
                if (reader.TokenType == JsonToken.PropertyName)
                  throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Additional content found in JSON reference object. A JSON reference object should only have a {0} property.", (IFormatProvider) CultureInfo.InvariantCulture, (object) "$ref"));
                newValue = this.Serializer.ReferenceResolver.ResolveReference((object) this, reference);
                return true;
              }
              else
                flag = true;
            }
            else if (string.Equals(a, "$type", StringComparison.Ordinal))
            {
              this.CheckedRead(reader);
              string fullyQualifiedTypeName = reader.Value.ToString();
              TypeNameHandling? nullable1 = member != null ? member.TypeNameHandling : new TypeNameHandling?();
              int num;
              if (!nullable1.HasValue)
              {
                TypeNameHandling? nullable2 = containerContract != null ? containerContract.ItemTypeNameHandling : new TypeNameHandling?();
                if (!nullable2.HasValue)
                {
                  TypeNameHandling? nullable3 = containerMember != null ? containerMember.ItemTypeNameHandling : new TypeNameHandling?();
                  num = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : (int) this.Serializer.TypeNameHandling;
                }
                else
                  num = (int) nullable2.GetValueOrDefault();
              }
              else
                num = (int) nullable1.GetValueOrDefault();
              if (num != 0)
              {
                string typeName;
                string assemblyName;
                ReflectionUtils.SplitFullyQualifiedTypeName(fullyQualifiedTypeName, out typeName, out assemblyName);
                Type type;
                try
                {
                  type = this.Serializer.Binder.BindToType(assemblyName, typeName);
                }
                catch (Exception ex)
                {
                  throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Error resolving type specified in JSON '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) fullyQualifiedTypeName), ex);
                }
                if (type == null)
                  throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Type specified in JSON '{0}' was not resolved.", (IFormatProvider) CultureInfo.InvariantCulture, (object) fullyQualifiedTypeName));
                if (objectType != null && !objectType.IsAssignableFrom(type))
                  throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Type specified in JSON '{0}' is not compatible with '{1}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type.AssemblyQualifiedName, (object) objectType.AssemblyQualifiedName));
                objectType = type;
                contract = this.GetContractSafe(type);
              }
              this.CheckedRead(reader);
              flag = true;
            }
            else if (string.Equals(a, "$id", StringComparison.Ordinal))
            {
              this.CheckedRead(reader);
              id = reader.Value != null ? reader.Value.ToString() : (string) null;
              this.CheckedRead(reader);
              flag = true;
            }
            else if (string.Equals(a, "$values", StringComparison.Ordinal))
            {
              this.CheckedRead(reader);
              object list = this.CreateList(reader, objectType, contract, member, existingValue, id);
              this.CheckedRead(reader);
              newValue = list;
              return true;
            }
            else
              flag = false;
          }
          while (flag && reader.TokenType == JsonToken.PropertyName);
        }
      }
      return false;
    }

    private JsonArrayContract EnsureArrayContract(JsonReader reader, Type objectType, JsonContract contract)
    {
      if (contract == null)
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Could not resolve type '{0}' to a JsonContract.", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
      JsonArrayContract jsonArrayContract = contract as JsonArrayContract;
      if (jsonArrayContract == null)
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot deserialize the current JSON array (e.g. [1,2,3]) into type '{0}' because the type requires a {1} to deserialize correctly.\r\nTo fix this error either change the JSON to a {1} or change the deserialized type to an array or a type that implements a collection interface (e.g. ICollection, IList) like List<T> that can be deserialized from a JSON array. JsonArrayAttribute can also be added to the type to force it to deserialize from a JSON array.\r\n", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectType, (object) this.GetExpectedDescription(contract)));
      else
        return jsonArrayContract;
    }

    private void CheckedRead(JsonReader reader)
    {
      if (!reader.Read())
        throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
    }

    private object CreateList(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue, string id)
    {
      object obj;
      if (this.HasDefinedType(objectType))
      {
        JsonArrayContract contract1 = this.EnsureArrayContract(reader, objectType, contract);
        if (existingValue == null)
        {
          bool isReadOnlyOrFixedSize;
          IList list = CollectionUtils.CreateList(contract.CreatedType, out isReadOnlyOrFixedSize);
          if (id != null && isReadOnlyOrFixedSize)
            throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot preserve reference to array or readonly list: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
          if (contract.OnSerializing != null && isReadOnlyOrFixedSize)
            throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot call OnSerializing on an array or readonly list: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
          if (contract.OnError != null && isReadOnlyOrFixedSize)
            throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot call OnError on an array or readonly list: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
          if (!contract1.IsMultidimensionalArray)
            this.PopulateList(contract1.CreateWrapper((object) list), reader, contract1, member, id);
          else
            this.PopulateMultidimensionalArray(list, reader, contract1, member, id);
          if (isReadOnlyOrFixedSize)
          {
            if (contract1.IsMultidimensionalArray)
              list = (IList) CollectionUtils.ToMultidimensionalArray(list, ReflectionUtils.GetCollectionItemType(contract.CreatedType), contract.CreatedType.GetArrayRank());
            else if (contract.CreatedType.IsArray)
              list = (IList) CollectionUtils.ToArray((Array) ((List<object>) list).ToArray(), ReflectionUtils.GetCollectionItemType(contract.CreatedType));
            else if (ReflectionUtils.InheritsGenericDefinition(contract.CreatedType, typeof (ReadOnlyCollection<>)))
              list = (IList) ReflectionUtils.CreateInstance(contract.CreatedType, new object[1]
              {
                (object) list
              });
          }
          else if (list is IWrappedCollection)
            return ((IWrappedCollection) list).UnderlyingCollection;
          obj = (object) list;
        }
        else
          obj = this.PopulateList(contract1.CreateWrapper(existingValue), reader, contract1, member, id);
      }
      else
        obj = (object) this.CreateJToken(reader, contract);
      return obj;
    }

    private bool HasDefinedType(Type type)
    {
      if (type != null && type != typeof (object))
        return !typeof (JToken).IsSubclassOf(type);
      else
        return false;
    }

    private object EnsureType(JsonReader reader, object value, CultureInfo culture, JsonContract contract, Type targetType)
    {
      if (targetType == null)
        return value;
      if (ReflectionUtils.GetObjectType(value) == targetType)
        return value;
      try
      {
        if (value == null && contract.IsNullable)
          return (object) null;
        if (!contract.IsConvertable)
          return ConvertUtils.ConvertOrCast(value, culture, contract.NonNullableUnderlyingType);
        if (TypeExtensions.IsEnum(contract.NonNullableUnderlyingType))
        {
          if (value is string)
            return Enum.Parse(contract.NonNullableUnderlyingType, value.ToString(), true);
          if (ConvertUtils.IsInteger(value))
            return Enum.ToObject(contract.NonNullableUnderlyingType, value);
        }
        return Convert.ChangeType(value, contract.NonNullableUnderlyingType, (IFormatProvider) culture);
      }
      catch (Exception ex)
      {
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Error converting value {0} to type '{1}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.FormatValueForPrint(value), (object) targetType), ex);
      }
    }

    private string FormatValueForPrint(object value)
    {
      if (value == null)
        return "{null}";
      if (value is string)
        return "\"" + value + "\"";
      else
        return value.ToString();
    }

    private void SetPropertyValue(JsonProperty property, JsonConverter propertyConverter, JsonContainerContract containerContract, JsonProperty containerProperty, JsonReader reader, object target)
    {
      bool useExistingValue;
      object currentValue;
      JsonContract propertyContract;
      bool gottenCurrentValue;
      if (this.CalculatePropertyDetails(property, ref propertyConverter, containerContract, containerProperty, reader, target, out useExistingValue, out currentValue, out propertyContract, out gottenCurrentValue))
        return;
      object obj;
      if (propertyConverter != null && propertyConverter.CanRead)
      {
        if (!gottenCurrentValue && target != null && property.Readable)
          currentValue = property.ValueProvider.GetValue(target);
        obj = propertyConverter.ReadJson(reader, property.PropertyType, currentValue, (JsonSerializer) this.GetInternalSerializer());
      }
      else
        obj = this.CreateValueInternal(reader, property.PropertyType, propertyContract, property, containerContract, containerProperty, useExistingValue ? currentValue : (object) null);
      if (useExistingValue && obj == currentValue || !this.ShouldSetPropertyValue(property, obj))
        return;
      property.ValueProvider.SetValue(target, obj);
      if (property.SetIsSpecified == null)
        return;
      property.SetIsSpecified(target, (object) true);
    }

    private bool CalculatePropertyDetails(JsonProperty property, ref JsonConverter propertyConverter, JsonContainerContract containerContract, JsonProperty containerProperty, JsonReader reader, object target, out bool useExistingValue, out object currentValue, out JsonContract propertyContract, out bool gottenCurrentValue)
    {
      currentValue = (object) null;
      useExistingValue = false;
      propertyContract = (JsonContract) null;
      gottenCurrentValue = false;
      if (property.Ignored)
      {
        reader.Skip();
        return true;
      }
      else
      {
        switch (property.ObjectCreationHandling.GetValueOrDefault(this.Serializer.ObjectCreationHandling))
        {
          case ObjectCreationHandling.Auto:
          case ObjectCreationHandling.Reuse:
            if ((reader.TokenType == JsonToken.StartArray || reader.TokenType == JsonToken.StartObject) && property.Readable)
            {
              currentValue = property.ValueProvider.GetValue(target);
              gottenCurrentValue = true;
              useExistingValue = currentValue != null && !property.PropertyType.IsArray && !ReflectionUtils.InheritsGenericDefinition(property.PropertyType, typeof (ReadOnlyCollection<>)) && !TypeExtensions.IsValueType(property.PropertyType);
              break;
            }
            else
              break;
        }
        if (!property.Writable && !useExistingValue)
        {
          reader.Skip();
          return true;
        }
        else if (property.NullValueHandling.GetValueOrDefault(this.Serializer.NullValueHandling) == NullValueHandling.Ignore && reader.TokenType == JsonToken.Null)
        {
          reader.Skip();
          return true;
        }
        else if (this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) && JsonReader.IsPrimitiveToken(reader.TokenType) && MiscellaneousUtils.ValueEquals(reader.Value, property.GetResolvedDefaultValue()))
        {
          reader.Skip();
          return true;
        }
        else
        {
          if (property.PropertyContract == null)
            property.PropertyContract = this.GetContractSafe(property.PropertyType);
          if (currentValue == null)
          {
            propertyContract = property.PropertyContract;
          }
          else
          {
            propertyContract = this.GetContractSafe(currentValue.GetType());
            if (propertyContract != property.PropertyContract)
              propertyConverter = this.GetConverter(propertyContract, property.MemberConverter, containerContract, containerProperty);
          }
          return false;
        }
      }
    }

    private void AddReference(JsonReader reader, string id, object value)
    {
      try
      {
        this.Serializer.ReferenceResolver.AddReference((object) this, id, value);
      }
      catch (Exception ex)
      {
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Error reading object reference '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) id), ex);
      }
    }

    private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
    {
      return (value & flag) == flag;
    }

    private bool ShouldSetPropertyValue(JsonProperty property, object value)
    {
      return (property.NullValueHandling.GetValueOrDefault(this.Serializer.NullValueHandling) != NullValueHandling.Ignore || value != null) && ((!this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) || !MiscellaneousUtils.ValueEquals(value, property.GetResolvedDefaultValue())) && property.Writable);
    }

    public object CreateNewDictionary(JsonReader reader, JsonDictionaryContract contract)
    {
      if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || this.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
        return contract.DefaultCreator();
      else
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unable to find a default constructor to use for type {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType));
    }

    private object PopulateDictionary(IWrappedDictionary wrappedDictionary, JsonReader reader, JsonDictionaryContract contract, JsonProperty containerProperty, string id)
    {
      object underlyingDictionary = wrappedDictionary.UnderlyingDictionary;
      if (id != null)
        this.AddReference(reader, id, underlyingDictionary);
      contract.InvokeOnDeserializing(underlyingDictionary, this.Serializer.Context);
      int depth = reader.Depth;
      if (contract.KeyContract == null)
        contract.KeyContract = this.GetContractSafe(contract.DictionaryKeyType);
      if (contract.ItemContract == null)
        contract.ItemContract = this.GetContractSafe(contract.DictionaryValueType);
      JsonConverter jsonConverter = contract.ItemConverter ?? this.GetConverter(contract.ItemContract, (JsonConverter) null, (JsonContainerContract) contract, containerProperty);
      bool flag = false;
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            object keyValue = reader.Value;
            try
            {
              try
              {
                keyValue = this.EnsureType(reader, keyValue, CultureInfo.InvariantCulture, contract.KeyContract, contract.DictionaryKeyType);
              }
              catch (Exception ex)
              {
                throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Could not convert string '{0}' to dictionary key type '{1}'. Create a TypeConverter to convert from the string to the key type object.", (IFormatProvider) CultureInfo.InvariantCulture, reader.Value, (object) contract.DictionaryKeyType), ex);
              }
              if (!this.ReadForType(reader, contract.ItemContract, jsonConverter != null))
                throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
              object obj = jsonConverter == null || !jsonConverter.CanRead ? this.CreateValueInternal(reader, contract.DictionaryValueType, contract.ItemContract, (JsonProperty) null, (JsonContainerContract) contract, containerProperty, (object) null) : jsonConverter.ReadJson(reader, contract.DictionaryValueType, (object) null, (JsonSerializer) this.GetInternalSerializer());
              wrappedDictionary[keyValue] = obj;
              goto case JsonToken.Comment;
            }
            catch (Exception ex)
            {
              if (this.IsErrorHandled(underlyingDictionary, (JsonContract) contract, keyValue, reader.Path, ex))
              {
                this.HandleError(reader, true, depth);
                goto case JsonToken.Comment;
              }
              else
                throw;
            }
          case JsonToken.Comment:
            continue;
          case JsonToken.EndObject:
            flag = true;
            goto case JsonToken.Comment;
          default:
            throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + (object) reader.TokenType);
        }
      }
      while (!flag && reader.Read());
      if (!flag)
        this.ThrowUnexpectedEndException(reader, (JsonContract) contract, underlyingDictionary, "Unexpected end when deserializing object.");
      contract.InvokeOnDeserialized(underlyingDictionary, this.Serializer.Context);
      return underlyingDictionary;
    }

    private object PopulateMultidimensionalArray(IList list, JsonReader reader, JsonArrayContract contract, JsonProperty containerProperty, string id)
    {
      int arrayRank = contract.UnderlyingType.GetArrayRank();
      if (id != null)
        this.AddReference(reader, id, (object) list);
      contract.InvokeOnDeserializing((object) list, this.Serializer.Context);
      JsonContract contractSafe = this.GetContractSafe(contract.CollectionItemType);
      JsonConverter converter = this.GetConverter(contractSafe, (JsonConverter) null, (JsonContainerContract) contract, containerProperty);
      int? nullable1 = new int?();
      Stack<IList> stack = new Stack<IList>();
      stack.Push(list);
      IList list1 = list;
      bool flag = false;
      do
      {
        int depth = reader.Depth;
        if (stack.Count == arrayRank)
        {
          try
          {
            if (this.ReadForType(reader, contractSafe, converter != null))
            {
              switch (reader.TokenType)
              {
                case JsonToken.Comment:
                  break;
                case JsonToken.EndArray:
                  stack.Pop();
                  list1 = stack.Peek();
                  nullable1 = new int?();
                  break;
                default:
                  object obj = converter == null || !converter.CanRead ? this.CreateValueInternal(reader, contract.CollectionItemType, contractSafe, (JsonProperty) null, (JsonContainerContract) contract, containerProperty, (object) null) : converter.ReadJson(reader, contract.CollectionItemType, (object) null, (JsonSerializer) this.GetInternalSerializer());
                  list1.Add(obj);
                  break;
              }
            }
            else
              break;
          }
          catch (Exception ex)
          {
            JsonPosition position = reader.GetPosition(depth);
            if (this.IsErrorHandled((object) list, (JsonContract) contract, (object) position.Position, reader.Path, ex))
            {
              this.HandleError(reader, true, depth);
              if (nullable1.HasValue)
              {
                int? nullable2 = nullable1;
                int? nullable3 = position.Position;
                if ((nullable2.GetValueOrDefault() != nullable3.GetValueOrDefault() ? 0 : (nullable2.HasValue == nullable3.HasValue ? 1 : 0)) != 0)
                  throw JsonSerializationException.Create(reader, "Infinite loop detected from error handling.", ex);
              }
              nullable1 = position.Position;
            }
            else
              throw;
          }
        }
        else if (reader.Read())
        {
          switch (reader.TokenType)
          {
            case JsonToken.StartArray:
              IList list2 = (IList) new List<object>();
              list1.Add((object) list2);
              stack.Push(list2);
              list1 = list2;
              break;
            case JsonToken.Comment:
              break;
            case JsonToken.EndArray:
              stack.Pop();
              if (stack.Count > 0)
              {
                list1 = stack.Peek();
                break;
              }
              else
              {
                flag = true;
                break;
              }
            default:
              throw JsonSerializationException.Create(reader, "Unexpected token when deserializing multidimensional array: " + (object) reader.TokenType);
          }
        }
        else
          break;
      }
      while (!flag);
      if (!flag)
        this.ThrowUnexpectedEndException(reader, (JsonContract) contract, (object) list, "Unexpected end when deserializing array.");
      contract.InvokeOnDeserialized((object) list, this.Serializer.Context);
      return (object) list;
    }

    private void ThrowUnexpectedEndException(JsonReader reader, JsonContract contract, object currentObject, string message)
    {
      try
      {
        throw JsonSerializationException.Create(reader, message);
      }
      catch (Exception ex)
      {
        if (this.IsErrorHandled(currentObject, contract, (object) null, reader.Path, ex))
          this.HandleError(reader, false, 0);
        else
          throw;
      }
    }

    private object PopulateList(IWrappedCollection wrappedList, JsonReader reader, JsonArrayContract contract, JsonProperty containerProperty, string id)
    {
      object underlyingCollection = wrappedList.UnderlyingCollection;
      if (id != null)
        this.AddReference(reader, id, underlyingCollection);
      if (wrappedList.IsFixedSize)
      {
        reader.Skip();
        return underlyingCollection;
      }
      else
      {
        contract.InvokeOnDeserializing(underlyingCollection, this.Serializer.Context);
        int depth = reader.Depth;
        JsonContract contractSafe = this.GetContractSafe(contract.CollectionItemType);
        JsonConverter converter = this.GetConverter(contractSafe, (JsonConverter) null, (JsonContainerContract) contract, containerProperty);
        int? nullable1 = new int?();
        bool flag = false;
        do
        {
          try
          {
            if (this.ReadForType(reader, contractSafe, converter != null))
            {
              switch (reader.TokenType)
              {
                case JsonToken.Comment:
                  break;
                case JsonToken.EndArray:
                  flag = true;
                  break;
                default:
                  object obj = converter == null || !converter.CanRead ? this.CreateValueInternal(reader, contract.CollectionItemType, contractSafe, (JsonProperty) null, (JsonContainerContract) contract, containerProperty, (object) null) : converter.ReadJson(reader, contract.CollectionItemType, (object) null, (JsonSerializer) this.GetInternalSerializer());
                  wrappedList.Add(obj);
                  break;
              }
            }
            else
              break;
          }
          catch (Exception ex)
          {
            JsonPosition position = reader.GetPosition(depth);
            if (this.IsErrorHandled(underlyingCollection, (JsonContract) contract, (object) position.Position, reader.Path, ex))
            {
              this.HandleError(reader, true, depth);
              if (nullable1.HasValue)
              {
                int? nullable2 = nullable1;
                int? nullable3 = position.Position;
                if ((nullable2.GetValueOrDefault() != nullable3.GetValueOrDefault() ? 0 : (nullable2.HasValue == nullable3.HasValue ? 1 : 0)) != 0)
                  throw JsonSerializationException.Create(reader, "Infinite loop detected from error handling.", ex);
              }
              nullable1 = position.Position;
            }
            else
              throw;
          }
        }
        while (!flag);
        if (!flag)
          this.ThrowUnexpectedEndException(reader, (JsonContract) contract, underlyingCollection, "Unexpected end when deserializing array.");
        contract.InvokeOnDeserialized(underlyingCollection, this.Serializer.Context);
        return underlyingCollection;
      }
    }

    private object CreateISerializable(JsonReader reader, JsonISerializableContract contract, string id)
    {
      Type underlyingType = contract.UnderlyingType;
      if (!JsonTypeReflector.FullyTrusted)
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Type '{0}' implements ISerializable but cannot be deserialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data.\r\nTo fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true.\r\n", (IFormatProvider) CultureInfo.InvariantCulture, (object) underlyingType));
      SerializationInfo serializationInfo = new SerializationInfo(contract.UnderlyingType, (IFormatterConverter) this.GetFormatterConverter());
      bool flag = false;
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            string name = reader.Value.ToString();
            if (!reader.Read())
              throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected end when setting {0}'s value.", (IFormatProvider) CultureInfo.InvariantCulture, (object) name));
            serializationInfo.AddValue(name, (object) JToken.ReadFrom(reader));
            goto case JsonToken.Comment;
          case JsonToken.Comment:
            continue;
          case JsonToken.EndObject:
            flag = true;
            goto case JsonToken.Comment;
          default:
            throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + (object) reader.TokenType);
        }
      }
      while (!flag && reader.Read());
      if (!flag)
        this.ThrowUnexpectedEndException(reader, (JsonContract) contract, (object) serializationInfo, "Unexpected end when deserializing object.");
      if (contract.ISerializableCreator == null)
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("ISerializable type '{0}' does not have a valid constructor. To correctly implement ISerializable a constructor that takes SerializationInfo and StreamingContext parameters should be present.", (IFormatProvider) CultureInfo.InvariantCulture, (object) underlyingType));
      object o = contract.ISerializableCreator(new object[2]
      {
        (object) serializationInfo,
        (object) this.Serializer.Context
      });
      if (id != null)
        this.AddReference(reader, id, o);
      contract.InvokeOnDeserializing(o, this.Serializer.Context);
      contract.InvokeOnDeserialized(o, this.Serializer.Context);
      return o;
    }

    private object CreateObjectFromNonDefaultConstructor(JsonReader reader, JsonObjectContract contract, JsonProperty containerProperty, ConstructorInfo constructorInfo, string id)
    {
      ValidationUtils.ArgumentNotNull((object) constructorInfo, "constructorInfo");
      Type underlyingType = contract.UnderlyingType;
      IDictionary<JsonProperty, object> dictionary1 = this.ResolvePropertyAndConstructorValues(contract, containerProperty, reader, underlyingType);
      IDictionary<ParameterInfo, object> dictionary2 = (IDictionary<ParameterInfo, object>) Enumerable.ToDictionary<ParameterInfo, ParameterInfo, object>((IEnumerable<ParameterInfo>) constructorInfo.GetParameters(), (Func<ParameterInfo, ParameterInfo>) (p => p), (Func<ParameterInfo, object>) (p => (object) null));
      IDictionary<JsonProperty, object> dictionary3 = (IDictionary<JsonProperty, object>) new Dictionary<JsonProperty, object>();
      foreach (KeyValuePair<JsonProperty, object> keyValuePair in (IEnumerable<KeyValuePair<JsonProperty, object>>) dictionary1)
      {
        ParameterInfo key = StringUtils.ForgivingCaseSensitiveFind<KeyValuePair<ParameterInfo, object>>((IEnumerable<KeyValuePair<ParameterInfo, object>>) dictionary2, (Func<KeyValuePair<ParameterInfo, object>, string>) (kv => kv.Key.Name), keyValuePair.Key.UnderlyingName).Key;
        if (key != null)
          dictionary2[key] = keyValuePair.Value;
        else
          dictionary3.Add(keyValuePair);
      }
      object obj1 = constructorInfo.Invoke(Enumerable.ToArray<object>((IEnumerable<object>) dictionary2.Values));
      if (id != null)
        this.AddReference(reader, id, obj1);
      contract.InvokeOnDeserializing(obj1, this.Serializer.Context);
      foreach (KeyValuePair<JsonProperty, object> keyValuePair in (IEnumerable<KeyValuePair<JsonProperty, object>>) dictionary3)
      {
        JsonProperty key = keyValuePair.Key;
        object obj2 = keyValuePair.Value;
        if (this.ShouldSetPropertyValue(keyValuePair.Key, keyValuePair.Value))
          key.ValueProvider.SetValue(obj1, obj2);
        else if (!key.Writable && obj2 != null)
        {
          JsonContract jsonContract = this.Serializer.ContractResolver.ResolveContract(key.PropertyType);
          if (jsonContract.ContractType == JsonContractType.Array)
          {
            JsonArrayContract jsonArrayContract = (JsonArrayContract) jsonContract;
            object list = key.ValueProvider.GetValue(obj1);
            if (list != null)
            {
              IWrappedCollection wrapper = jsonArrayContract.CreateWrapper(list);
              foreach (object obj3 in (IEnumerable) jsonArrayContract.CreateWrapper(obj2))
                wrapper.Add(obj3);
            }
          }
          else if (jsonContract.ContractType == JsonContractType.Dictionary)
          {
            JsonDictionaryContract dictionaryContract = (JsonDictionaryContract) jsonContract;
            object dictionary4 = key.ValueProvider.GetValue(obj1);
            if (dictionary4 != null)
            {
              IWrappedDictionary wrapper = dictionaryContract.CreateWrapper(dictionary4);
              foreach (DictionaryEntry dictionaryEntry in (IDictionary) dictionaryContract.CreateWrapper(obj2))
                wrapper.Add(dictionaryEntry.Key, dictionaryEntry.Value);
            }
          }
        }
      }
      contract.InvokeOnDeserialized(obj1, this.Serializer.Context);
      return obj1;
    }

    private IDictionary<JsonProperty, object> ResolvePropertyAndConstructorValues(JsonObjectContract contract, JsonProperty containerProperty, JsonReader reader, Type objectType)
    {
      IDictionary<JsonProperty, object> dictionary = (IDictionary<JsonProperty, object>) new Dictionary<JsonProperty, object>();
      bool flag = false;
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            string propertyName = reader.Value.ToString();
            JsonProperty member = contract.ConstructorParameters.GetClosestMatchProperty(propertyName) ?? contract.Properties.GetClosestMatchProperty(propertyName);
            if (member != null)
            {
              if (member.PropertyContract == null)
                member.PropertyContract = this.GetContractSafe(member.PropertyType);
              JsonConverter converter = this.GetConverter(member.PropertyContract, member.MemberConverter, (JsonContainerContract) contract, containerProperty);
              if (!this.ReadForType(reader, member.PropertyContract, converter != null))
                throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected end when setting {0}'s value.", (IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName));
              if (!member.Ignored)
              {
                if (member.PropertyContract == null)
                  member.PropertyContract = this.GetContractSafe(member.PropertyType);
                object obj = converter == null || !converter.CanRead ? this.CreateValueInternal(reader, member.PropertyType, member.PropertyContract, member, (JsonContainerContract) contract, containerProperty, (object) null) : converter.ReadJson(reader, member.PropertyType, (object) null, (JsonSerializer) this.GetInternalSerializer());
                dictionary[member] = obj;
                goto case JsonToken.Comment;
              }
              else
              {
                reader.Skip();
                goto case JsonToken.Comment;
              }
            }
            else
            {
              if (!reader.Read())
                throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected end when setting {0}'s value.", (IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName));
              if (this.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
                throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Could not find member '{0}' on object of type '{1}'", (IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName, (object) objectType.Name));
              reader.Skip();
              goto case JsonToken.Comment;
            }
          case JsonToken.Comment:
            continue;
          case JsonToken.EndObject:
            flag = true;
            goto case JsonToken.Comment;
          default:
            throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + (object) reader.TokenType);
        }
      }
      while (!flag && reader.Read());
      return dictionary;
    }

    private bool ReadForType(JsonReader reader, JsonContract contract, bool hasConverter)
    {
      if (hasConverter)
        return reader.Read();
      switch (contract != null ? (int) contract.InternalReadType : 0)
      {
        case 0:
          while (reader.Read())
          {
            if (reader.TokenType != JsonToken.Comment)
              return true;
          }
          return false;
        case 1:
          reader.ReadAsInt32();
          break;
        case 2:
          reader.ReadAsBytes();
          break;
        case 3:
          reader.ReadAsString();
          break;
        case 4:
          reader.ReadAsDecimal();
          break;
        case 5:
          reader.ReadAsDateTime();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return reader.TokenType != JsonToken.None;
    }

    public object CreateNewObject(JsonReader reader, JsonObjectContract objectContract, JsonProperty containerMember, JsonProperty containerProperty, string id, out bool createdFromNonDefaultConstructor)
    {
      object obj = (object) null;
      if (TypeExtensions.IsInterface(objectContract.UnderlyingType) || TypeExtensions.IsAbstract(objectContract.UnderlyingType))
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectContract.UnderlyingType));
      if (objectContract.OverrideConstructor != null)
      {
        if (objectContract.OverrideConstructor.GetParameters().Length > 0)
        {
          createdFromNonDefaultConstructor = true;
          return this.CreateObjectFromNonDefaultConstructor(reader, objectContract, containerMember, objectContract.OverrideConstructor, id);
        }
        else
          obj = objectContract.OverrideConstructor.Invoke((object[]) null);
      }
      else if (objectContract.DefaultCreator != null && (!objectContract.DefaultCreatorNonPublic || this.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor || objectContract.ParametrizedConstructor == null))
        obj = objectContract.DefaultCreator();
      else if (objectContract.ParametrizedConstructor != null)
      {
        createdFromNonDefaultConstructor = true;
        return this.CreateObjectFromNonDefaultConstructor(reader, objectContract, containerMember, objectContract.ParametrizedConstructor, id);
      }
      if (obj == null)
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unable to find a constructor to use for type {0}. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute.", (IFormatProvider) CultureInfo.InvariantCulture, (object) objectContract.UnderlyingType));
      createdFromNonDefaultConstructor = false;
      return obj;
    }

    private object PopulateObject(object newObject, JsonReader reader, JsonObjectContract contract, JsonProperty member, string id)
    {
      contract.InvokeOnDeserializing(newObject, this.Serializer.Context);
      Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> dictionary = contract.HasRequiredOrDefaultValueProperties || this.HasFlag(this.Serializer.DefaultValueHandling, DefaultValueHandling.Populate) ? Enumerable.ToDictionary<JsonProperty, JsonProperty, JsonSerializerInternalReader.PropertyPresence>((IEnumerable<JsonProperty>) contract.Properties, (Func<JsonProperty, JsonProperty>) (m => m), (Func<JsonProperty, JsonSerializerInternalReader.PropertyPresence>) (m => JsonSerializerInternalReader.PropertyPresence.None)) : (Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence>) null;
      if (id != null)
        this.AddReference(reader, id, newObject);
      int depth = reader.Depth;
      bool flag = false;
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            string propertyName = reader.Value.ToString();
            try
            {
              JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(propertyName);
              if (closestMatchProperty == null)
              {
                if (this.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
                  throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Could not find member '{0}' on object of type '{1}'", (IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName, (object) contract.UnderlyingType.Name));
                reader.Skip();
                goto case JsonToken.Comment;
              }
              else
              {
                if (closestMatchProperty.PropertyContract == null)
                  closestMatchProperty.PropertyContract = this.GetContractSafe(closestMatchProperty.PropertyType);
                JsonConverter converter = this.GetConverter(closestMatchProperty.PropertyContract, closestMatchProperty.MemberConverter, (JsonContainerContract) contract, member);
                if (!this.ReadForType(reader, closestMatchProperty.PropertyContract, converter != null))
                  throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Unexpected end when setting {0}'s value.", (IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName));
                this.SetPropertyPresence(reader, closestMatchProperty, dictionary);
                this.SetPropertyValue(closestMatchProperty, converter, (JsonContainerContract) contract, member, reader, newObject);
                goto case JsonToken.Comment;
              }
            }
            catch (Exception ex)
            {
              if (this.IsErrorHandled(newObject, (JsonContract) contract, (object) propertyName, reader.Path, ex))
              {
                this.HandleError(reader, true, depth);
                goto case JsonToken.Comment;
              }
              else
                throw;
            }
          case JsonToken.Comment:
            continue;
          case JsonToken.EndObject:
            flag = true;
            goto case JsonToken.Comment;
          default:
            throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + (object) reader.TokenType);
        }
      }
      while (!flag && reader.Read());
      if (!flag)
        this.ThrowUnexpectedEndException(reader, (JsonContract) contract, newObject, "Unexpected end when deserializing object.");
      this.EndObject(newObject, reader, contract, depth, dictionary);
      contract.InvokeOnDeserialized(newObject, this.Serializer.Context);
      return newObject;
    }

    private void EndObject(object newObject, JsonReader reader, JsonObjectContract contract, int initialDepth, Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> propertiesPresence)
    {
      if (propertiesPresence == null)
        return;
      foreach (KeyValuePair<JsonProperty, JsonSerializerInternalReader.PropertyPresence> keyValuePair in propertiesPresence)
      {
        JsonProperty key = keyValuePair.Key;
        JsonSerializerInternalReader.PropertyPresence propertyPresence = keyValuePair.Value;
        switch (propertyPresence)
        {
          case JsonSerializerInternalReader.PropertyPresence.None:
          case JsonSerializerInternalReader.PropertyPresence.Null:
            try
            {
              Required? nullable = key._required;
              int num;
              if (!nullable.HasValue)
              {
                Required? itemRequired = contract.ItemRequired;
                num = itemRequired.HasValue ? (int) itemRequired.GetValueOrDefault() : 0;
              }
              else
                num = (int) nullable.GetValueOrDefault();
              Required required = (Required) num;
              switch (propertyPresence)
              {
                case JsonSerializerInternalReader.PropertyPresence.None:
                  if (required == Required.AllowNull || required == Required.Always)
                    throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Required property '{0}' not found in JSON.", (IFormatProvider) CultureInfo.InvariantCulture, (object) key.PropertyName));
                  if (key.PropertyContract == null)
                    key.PropertyContract = this.GetContractSafe(key.PropertyType);
                  if (this.HasFlag(key.DefaultValueHandling.GetValueOrDefault(this.Serializer.DefaultValueHandling), DefaultValueHandling.Populate))
                  {
                    if (key.Writable)
                    {
                      key.ValueProvider.SetValue(newObject, this.EnsureType(reader, key.GetResolvedDefaultValue(), CultureInfo.InvariantCulture, key.PropertyContract, key.PropertyType));
                      continue;
                    }
                    else
                      continue;
                  }
                  else
                    continue;
                case JsonSerializerInternalReader.PropertyPresence.Null:
                  if (required == Required.Always)
                    throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Required property '{0}' expects a value but got null.", (IFormatProvider) CultureInfo.InvariantCulture, (object) key.PropertyName));
                  else
                    continue;
                default:
                  continue;
              }
            }
            catch (Exception ex)
            {
              if (this.IsErrorHandled(newObject, (JsonContract) contract, (object) key.PropertyName, reader.Path, ex))
              {
                this.HandleError(reader, true, initialDepth);
                continue;
              }
              else
                throw;
            }
          default:
            continue;
        }
      }
    }

    private void SetPropertyPresence(JsonReader reader, JsonProperty property, Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> requiredProperties)
    {
      if (property == null || requiredProperties == null)
        return;
      requiredProperties[property] = reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.Undefined ? JsonSerializerInternalReader.PropertyPresence.Null : JsonSerializerInternalReader.PropertyPresence.Value;
    }

    private void HandleError(JsonReader reader, bool readPastError, int initialDepth)
    {
      this.ClearErrorContext();
      if (!readPastError)
        return;
      reader.Skip();
      do
        ;
      while (reader.Depth > initialDepth + 1 && reader.Read());
    }

    internal enum PropertyPresence
    {
      None,
      Null,
      Value,
    }
  }
}
