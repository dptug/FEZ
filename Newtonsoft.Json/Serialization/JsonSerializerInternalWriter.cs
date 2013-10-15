// Type: Newtonsoft.Json.Serialization.JsonSerializerInternalWriter
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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  internal class JsonSerializerInternalWriter : JsonSerializerInternalBase
  {
    private readonly List<object> _serializeStack = new List<object>();
    private JsonSerializerProxy _internalSerializer;

    public JsonSerializerInternalWriter(JsonSerializer serializer)
      : base(serializer)
    {
    }

    public void Serialize(JsonWriter jsonWriter, object value)
    {
      if (jsonWriter == null)
        throw new ArgumentNullException("jsonWriter");
      this.SerializeValue(jsonWriter, value, this.GetContractSafe(value), (JsonProperty) null, (JsonContainerContract) null, (JsonProperty) null);
    }

    private JsonSerializerProxy GetInternalSerializer()
    {
      if (this._internalSerializer == null)
        this._internalSerializer = new JsonSerializerProxy(this);
      return this._internalSerializer;
    }

    private JsonContract GetContractSafe(object value)
    {
      if (value == null)
        return (JsonContract) null;
      else
        return this.Serializer.ContractResolver.ResolveContract(value.GetType());
    }

    private void SerializePrimitive(JsonWriter writer, object value, JsonPrimitiveContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
    {
      if (contract.UnderlyingType == typeof (byte[]) && this.ShouldWriteType(TypeNameHandling.Objects, (JsonContract) contract, member, containerContract, containerProperty))
      {
        writer.WriteStartObject();
        this.WriteTypeProperty(writer, contract.CreatedType);
        writer.WritePropertyName("$value");
        writer.WriteValue(value);
        writer.WriteEndObject();
      }
      else
        writer.WriteValue(value);
    }

    private void SerializeValue(JsonWriter writer, object value, JsonContract valueContract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        JsonConverter converter;
        if (((converter = member != null ? member.Converter : (JsonConverter) null) != null || (converter = containerProperty != null ? containerProperty.ItemConverter : (JsonConverter) null) != null || ((converter = containerContract != null ? containerContract.ItemConverter : (JsonConverter) null) != null || (converter = valueContract.Converter) != null || ((converter = this.Serializer.GetMatchingConverter(valueContract.UnderlyingType)) != null || (converter = valueContract.InternalConverter) != null))) && converter.CanWrite)
        {
          this.SerializeConvertable(writer, converter, value, valueContract, containerContract, containerProperty);
        }
        else
        {
          switch (valueContract.ContractType)
          {
            case JsonContractType.Object:
              this.SerializeObject(writer, value, (JsonObjectContract) valueContract, member, containerContract, containerProperty);
              break;
            case JsonContractType.Array:
              JsonArrayContract contract1 = (JsonArrayContract) valueContract;
              if (!contract1.IsMultidimensionalArray)
              {
                this.SerializeList(writer, contract1.CreateWrapper(value), contract1, member, containerContract, containerProperty);
                break;
              }
              else
              {
                this.SerializeMultidimensionalArray(writer, (Array) value, contract1, member, containerContract, containerProperty);
                break;
              }
            case JsonContractType.Primitive:
              this.SerializePrimitive(writer, value, (JsonPrimitiveContract) valueContract, member, containerContract, containerProperty);
              break;
            case JsonContractType.String:
              this.SerializeString(writer, value, (JsonStringContract) valueContract);
              break;
            case JsonContractType.Dictionary:
              JsonDictionaryContract contract2 = (JsonDictionaryContract) valueContract;
              this.SerializeDictionary(writer, contract2.CreateWrapper(value), contract2, member, containerContract, containerProperty);
              break;
            case JsonContractType.Serializable:
              this.SerializeISerializable(writer, (ISerializable) value, (JsonISerializableContract) valueContract, member, containerContract, containerProperty);
              break;
            case JsonContractType.Linq:
              ((JToken) value).WriteTo(writer, this.Serializer.Converters != null ? Enumerable.ToArray<JsonConverter>((IEnumerable<JsonConverter>) this.Serializer.Converters) : (JsonConverter[]) null);
              break;
          }
        }
      }
    }

    private bool? ResolveIsReference(JsonContract contract, JsonProperty property, JsonContainerContract collectionContract, JsonProperty containerProperty)
    {
      bool? nullable = new bool?();
      if (property != null)
        nullable = property.IsReference;
      if (!nullable.HasValue && containerProperty != null)
        nullable = containerProperty.ItemIsReference;
      if (!nullable.HasValue && collectionContract != null)
        nullable = collectionContract.ItemIsReference;
      if (!nullable.HasValue)
        nullable = contract.IsReference;
      return nullable;
    }

    private bool ShouldWriteReference(object value, JsonProperty property, JsonContract valueContract, JsonContainerContract collectionContract, JsonProperty containerProperty)
    {
      if (value == null || valueContract.ContractType == JsonContractType.Primitive || valueContract.ContractType == JsonContractType.String)
        return false;
      bool? nullable = this.ResolveIsReference(valueContract, property, collectionContract, containerProperty);
      if (!nullable.HasValue)
        nullable = valueContract.ContractType != JsonContractType.Array ? new bool?(this.HasFlag(this.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects)) : new bool?(this.HasFlag(this.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays));
      if (!nullable.Value)
        return false;
      else
        return this.Serializer.ReferenceResolver.IsReferenced((object) this, value);
    }

    private bool ShouldWriteProperty(object memberValue, JsonProperty property)
    {
      return (property.NullValueHandling.GetValueOrDefault(this.Serializer.NullValueHandling) != NullValueHandling.Ignore || memberValue != null) && (!this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) || !MiscellaneousUtils.ValueEquals(memberValue, property.GetResolvedDefaultValue()));
    }

    private bool CheckForCircularReference(JsonWriter writer, object value, JsonProperty property, JsonContract contract, JsonContainerContract containerContract, JsonProperty containerProperty)
    {
      if (value == null || contract.ContractType == JsonContractType.Primitive || contract.ContractType == JsonContractType.String)
        return true;
      ReferenceLoopHandling? nullable = new ReferenceLoopHandling?();
      if (property != null)
        nullable = property.ReferenceLoopHandling;
      if (!nullable.HasValue && containerProperty != null)
        nullable = containerProperty.ItemReferenceLoopHandling;
      if (!nullable.HasValue && containerContract != null)
        nullable = containerContract.ItemReferenceLoopHandling;
      if (this._serializeStack.IndexOf(value) == -1)
        return true;
      switch (nullable.GetValueOrDefault(this.Serializer.ReferenceLoopHandling))
      {
        case ReferenceLoopHandling.Error:
          string str = "Self referencing loop detected";
          if (property != null)
            str = str + StringUtils.FormatWith(" for property '{0}'", (IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName);
          string message = str + StringUtils.FormatWith(" with type '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType());
          throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, message, (Exception) null);
        case ReferenceLoopHandling.Ignore:
          return false;
        case ReferenceLoopHandling.Serialize:
          return true;
        default:
          throw new InvalidOperationException(StringUtils.FormatWith("Unexpected ReferenceLoopHandling value: '{0}'", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.Serializer.ReferenceLoopHandling));
      }
    }

    private void WriteReference(JsonWriter writer, object value)
    {
      writer.WriteStartObject();
      writer.WritePropertyName("$ref");
      writer.WriteValue(this.GetReference(writer, value));
      writer.WriteEndObject();
    }

    private string GetReference(JsonWriter writer, object value)
    {
      try
      {
        return this.Serializer.ReferenceResolver.GetReference((object) this, value);
      }
      catch (Exception ex)
      {
        throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, StringUtils.FormatWith("Error writing object reference for '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()), ex);
      }
    }

    internal static bool TryConvertToString(object value, Type type, out string s)
    {
      TypeConverter converter = ConvertUtils.GetConverter(type);
      if (converter != null && !(converter is ComponentConverter) && (converter.GetType() != typeof (TypeConverter) && converter.CanConvertTo(typeof (string))))
      {
        s = converter.ConvertToInvariantString(value);
        return true;
      }
      else if (value is Type)
      {
        s = ((Type) value).AssemblyQualifiedName;
        return true;
      }
      else
      {
        s = (string) null;
        return false;
      }
    }

    private void SerializeString(JsonWriter writer, object value, JsonStringContract contract)
    {
      contract.InvokeOnSerializing(value, this.Serializer.Context);
      string s;
      JsonSerializerInternalWriter.TryConvertToString(value, contract.UnderlyingType, out s);
      writer.WriteValue(s);
      contract.InvokeOnSerialized(value, this.Serializer.Context);
    }

    private void SerializeObject(JsonWriter writer, object value, JsonObjectContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
    {
      contract.InvokeOnSerializing(value, this.Serializer.Context);
      this._serializeStack.Add(value);
      this.WriteObjectStart(writer, value, (JsonContract) contract, member, collectionContract, containerProperty);
      int top = writer.Top;
      foreach (JsonProperty jsonProperty in (Collection<JsonProperty>) contract.Properties)
      {
        try
        {
          JsonContract memberContract;
          object memberValue;
          if (this.CalculatePropertyValues(writer, value, (JsonContainerContract) contract, member, jsonProperty, out memberContract, out memberValue))
          {
            writer.WritePropertyName(jsonProperty.PropertyName);
            this.SerializeValue(writer, memberValue, memberContract, jsonProperty, (JsonContainerContract) contract, member);
          }
        }
        catch (Exception ex)
        {
          if (this.IsErrorHandled(value, (JsonContract) contract, (object) jsonProperty.PropertyName, writer.ContainerPath, ex))
            this.HandleError(writer, top);
          else
            throw;
        }
      }
      writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      contract.InvokeOnSerialized(value, this.Serializer.Context);
    }

    private bool CalculatePropertyValues(JsonWriter writer, object value, JsonContainerContract contract, JsonProperty member, JsonProperty property, out JsonContract memberContract, out object memberValue)
    {
      if (!property.Ignored && property.Readable && (this.ShouldSerialize(property, value) && this.IsSpecified(property, value)))
      {
        if (property.PropertyContract == null)
          property.PropertyContract = this.Serializer.ContractResolver.ResolveContract(property.PropertyType);
        memberValue = property.ValueProvider.GetValue(value);
        memberContract = TypeExtensions.IsSealed(property.PropertyContract.UnderlyingType) ? property.PropertyContract : this.GetContractSafe(memberValue);
        if (this.ShouldWriteProperty(memberValue, property))
        {
          if (this.ShouldWriteReference(memberValue, property, memberContract, contract, member))
          {
            writer.WritePropertyName(property.PropertyName);
            this.WriteReference(writer, memberValue);
            return false;
          }
          else
          {
            if (!this.CheckForCircularReference(writer, memberValue, property, memberContract, contract, member))
              return false;
            if (memberValue == null)
            {
              JsonObjectContract jsonObjectContract = contract as JsonObjectContract;
              Required? nullable1 = property._required;
              int num;
              if (!nullable1.HasValue)
              {
                Required? nullable2 = jsonObjectContract != null ? jsonObjectContract.ItemRequired : new Required?();
                num = nullable2.HasValue ? (int) nullable2.GetValueOrDefault() : 0;
              }
              else
                num = (int) nullable1.GetValueOrDefault();
              if (num == 2)
                throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, StringUtils.FormatWith("Cannot write a null value for property '{0}'. Property requires a value.", (IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName), (Exception) null);
            }
            return true;
          }
        }
      }
      memberContract = (JsonContract) null;
      memberValue = (object) null;
      return false;
    }

    private void WriteObjectStart(JsonWriter writer, object value, JsonContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
    {
      writer.WriteStartObject();
      bool? nullable = this.ResolveIsReference(contract, member, collectionContract, containerProperty);
      if (nullable.HasValue ? nullable.GetValueOrDefault() : this.HasFlag(this.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects))
      {
        writer.WritePropertyName("$id");
        writer.WriteValue(this.GetReference(writer, value));
      }
      if (!this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionContract, containerProperty))
        return;
      this.WriteTypeProperty(writer, contract.UnderlyingType);
    }

    private void WriteTypeProperty(JsonWriter writer, Type type)
    {
      writer.WritePropertyName("$type");
      writer.WriteValue(ReflectionUtils.GetTypeName(type, this.Serializer.TypeNameAssemblyFormat, this.Serializer.Binder));
    }

    private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
    {
      return (value & flag) == flag;
    }

    private bool HasFlag(PreserveReferencesHandling value, PreserveReferencesHandling flag)
    {
      return (value & flag) == flag;
    }

    private bool HasFlag(TypeNameHandling value, TypeNameHandling flag)
    {
      return (value & flag) == flag;
    }

    private void SerializeConvertable(JsonWriter writer, JsonConverter converter, object value, JsonContract contract, JsonContainerContract collectionContract, JsonProperty containerProperty)
    {
      if (this.ShouldWriteReference(value, (JsonProperty) null, contract, collectionContract, containerProperty))
      {
        this.WriteReference(writer, value);
      }
      else
      {
        if (!this.CheckForCircularReference(writer, value, (JsonProperty) null, contract, collectionContract, containerProperty))
          return;
        this._serializeStack.Add(value);
        converter.WriteJson(writer, value, (JsonSerializer) this.GetInternalSerializer());
        this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      }
    }

    private void SerializeList(JsonWriter writer, IWrappedCollection values, JsonArrayContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
    {
      contract.InvokeOnSerializing(values.UnderlyingCollection, this.Serializer.Context);
      this._serializeStack.Add(values.UnderlyingCollection);
      bool flag = this.WriteStartArray(writer, values.UnderlyingCollection, contract, member, collectionContract, containerProperty);
      writer.WriteStartArray();
      int top = writer.Top;
      int num = 0;
      foreach (object obj in (IEnumerable) values)
      {
        try
        {
          JsonContract jsonContract = contract.FinalItemContract ?? this.GetContractSafe(obj);
          if (this.ShouldWriteReference(obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
            this.WriteReference(writer, obj);
          else if (this.CheckForCircularReference(writer, obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
            this.SerializeValue(writer, obj, jsonContract, (JsonProperty) null, (JsonContainerContract) contract, member);
        }
        catch (Exception ex)
        {
          if (this.IsErrorHandled(values.UnderlyingCollection, (JsonContract) contract, (object) num, writer.ContainerPath, ex))
            this.HandleError(writer, top);
          else
            throw;
        }
        finally
        {
          ++num;
        }
      }
      writer.WriteEndArray();
      if (flag)
        writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      contract.InvokeOnSerialized(values.UnderlyingCollection, this.Serializer.Context);
    }

    private void SerializeMultidimensionalArray(JsonWriter writer, Array values, JsonArrayContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
    {
      contract.InvokeOnSerializing((object) values, this.Serializer.Context);
      this._serializeStack.Add((object) values);
      bool flag = this.WriteStartArray(writer, (object) values, contract, member, collectionContract, containerProperty);
      this.SerializeMultidimensionalArray(writer, values, contract, member, writer.Top, new int[0]);
      if (flag)
        writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      contract.InvokeOnSerialized((object) values, this.Serializer.Context);
    }

    private void SerializeMultidimensionalArray(JsonWriter writer, Array values, JsonArrayContract contract, JsonProperty member, int initialDepth, int[] indices)
    {
      int length = indices.Length;
      int[] indices1 = new int[length + 1];
      for (int index = 0; index < length; ++index)
        indices1[index] = indices[index];
      writer.WriteStartArray();
      for (int index = 0; index < values.GetLength(length); ++index)
      {
        indices1[length] = index;
        if (indices1.Length == values.Rank)
        {
          object obj = values.GetValue(indices1);
          try
          {
            JsonContract jsonContract = contract.FinalItemContract ?? this.GetContractSafe(obj);
            if (this.ShouldWriteReference(obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
              this.WriteReference(writer, obj);
            else if (this.CheckForCircularReference(writer, obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
              this.SerializeValue(writer, obj, jsonContract, (JsonProperty) null, (JsonContainerContract) contract, member);
          }
          catch (Exception ex)
          {
            if (this.IsErrorHandled((object) values, (JsonContract) contract, (object) index, writer.ContainerPath, ex))
              this.HandleError(writer, initialDepth + 1);
            else
              throw;
          }
        }
        else
          this.SerializeMultidimensionalArray(writer, values, contract, member, initialDepth + 1, indices1);
      }
      writer.WriteEndArray();
    }

    private bool WriteStartArray(JsonWriter writer, object values, JsonArrayContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
    {
      bool? nullable = this.ResolveIsReference((JsonContract) contract, member, containerContract, containerProperty);
      bool flag1 = nullable.HasValue ? nullable.GetValueOrDefault() : this.HasFlag(this.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays);
      bool flag2 = this.ShouldWriteType(TypeNameHandling.Arrays, (JsonContract) contract, member, containerContract, containerProperty);
      bool flag3 = flag1 || flag2;
      if (flag3)
      {
        writer.WriteStartObject();
        if (flag1)
        {
          writer.WritePropertyName("$id");
          writer.WriteValue(this.GetReference(writer, values));
        }
        if (flag2)
          this.WriteTypeProperty(writer, values.GetType());
        writer.WritePropertyName("$values");
      }
      if (contract.ItemContract == null)
        contract.ItemContract = this.Serializer.ContractResolver.ResolveContract(contract.CollectionItemType ?? typeof (object));
      return flag3;
    }

    private void SerializeISerializable(JsonWriter writer, ISerializable value, JsonISerializableContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
    {
      if (!JsonTypeReflector.FullyTrusted)
        throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, StringUtils.FormatWith("Type '{0}' implements ISerializable but cannot be serialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data.\r\nTo fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true.", (IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()), (Exception) null);
      contract.InvokeOnSerializing((object) value, this.Serializer.Context);
      this._serializeStack.Add((object) value);
      this.WriteObjectStart(writer, (object) value, (JsonContract) contract, member, collectionContract, containerProperty);
      SerializationInfo info = new SerializationInfo(contract.UnderlyingType, (IFormatterConverter) new FormatterConverter());
      value.GetObjectData(info, this.Serializer.Context);
      foreach (SerializationEntry serializationEntry in info)
      {
        writer.WritePropertyName(serializationEntry.Name);
        this.SerializeValue(writer, serializationEntry.Value, this.GetContractSafe(serializationEntry.Value), (JsonProperty) null, (JsonContainerContract) null, member);
      }
      writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      contract.InvokeOnSerialized((object) value, this.Serializer.Context);
    }

    private bool ShouldWriteType(TypeNameHandling typeNameHandlingFlag, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
    {
      TypeNameHandling? nullable1 = member != null ? member.TypeNameHandling : new TypeNameHandling?();
      int num;
      if (!nullable1.HasValue)
      {
        TypeNameHandling? nullable2 = containerProperty != null ? containerProperty.ItemTypeNameHandling : new TypeNameHandling?();
        if (!nullable2.HasValue)
        {
          TypeNameHandling? nullable3 = containerContract != null ? containerContract.ItemTypeNameHandling : new TypeNameHandling?();
          num = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : (int) this.Serializer.TypeNameHandling;
        }
        else
          num = (int) nullable2.GetValueOrDefault();
      }
      else
        num = (int) nullable1.GetValueOrDefault();
      TypeNameHandling typeNameHandling = (TypeNameHandling) num;
      if (this.HasFlag(typeNameHandling, typeNameHandlingFlag))
        return true;
      if (this.HasFlag(typeNameHandling, TypeNameHandling.Auto))
      {
        if (member != null)
        {
          if (contract.UnderlyingType != member.PropertyContract.CreatedType)
            return true;
        }
        else if (containerContract != null && containerContract.ItemContract != null && contract.UnderlyingType != containerContract.ItemContract.CreatedType)
          return true;
      }
      return false;
    }

    private void SerializeDictionary(JsonWriter writer, IWrappedDictionary values, JsonDictionaryContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
    {
      contract.InvokeOnSerializing(values.UnderlyingDictionary, this.Serializer.Context);
      this._serializeStack.Add(values.UnderlyingDictionary);
      this.WriteObjectStart(writer, values.UnderlyingDictionary, (JsonContract) contract, member, collectionContract, containerProperty);
      if (contract.ItemContract == null)
        contract.ItemContract = this.Serializer.ContractResolver.ResolveContract(contract.DictionaryValueType ?? typeof (object));
      int top = writer.Top;
      foreach (DictionaryEntry entry in (IDictionary) values)
      {
        string propertyName = this.GetPropertyName(entry);
        string name = contract.PropertyNameResolver != null ? contract.PropertyNameResolver(propertyName) : propertyName;
        try
        {
          object obj = entry.Value;
          JsonContract jsonContract = contract.FinalItemContract ?? this.GetContractSafe(obj);
          if (this.ShouldWriteReference(obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
          {
            writer.WritePropertyName(name);
            this.WriteReference(writer, obj);
          }
          else if (this.CheckForCircularReference(writer, obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
          {
            writer.WritePropertyName(name);
            this.SerializeValue(writer, obj, jsonContract, (JsonProperty) null, (JsonContainerContract) contract, member);
          }
        }
        catch (Exception ex)
        {
          if (this.IsErrorHandled(values.UnderlyingDictionary, (JsonContract) contract, (object) name, writer.ContainerPath, ex))
            this.HandleError(writer, top);
          else
            throw;
        }
      }
      writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      contract.InvokeOnSerialized(values.UnderlyingDictionary, this.Serializer.Context);
    }

    private string GetPropertyName(DictionaryEntry entry)
    {
      if (ConvertUtils.IsConvertible(entry.Key))
        return Convert.ToString(entry.Key, (IFormatProvider) CultureInfo.InvariantCulture);
      string s;
      if (JsonSerializerInternalWriter.TryConvertToString(entry.Key, entry.Key.GetType(), out s))
        return s;
      else
        return entry.Key.ToString();
    }

    private void HandleError(JsonWriter writer, int initialDepth)
    {
      this.ClearErrorContext();
      if (writer.WriteState == WriteState.Property)
        writer.WriteNull();
      while (writer.Top > initialDepth)
        writer.WriteEnd();
    }

    private bool ShouldSerialize(JsonProperty property, object target)
    {
      if (property.ShouldSerialize == null)
        return true;
      else
        return property.ShouldSerialize(target);
    }

    private bool IsSpecified(JsonProperty property, object target)
    {
      if (property.GetIsSpecified == null)
        return true;
      else
        return property.GetIsSpecified(target);
    }
  }
}
