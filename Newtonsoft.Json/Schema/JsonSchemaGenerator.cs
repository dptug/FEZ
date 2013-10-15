// Type: Newtonsoft.Json.Schema.JsonSchemaGenerator
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Schema
{
  public class JsonSchemaGenerator
  {
    private readonly IList<JsonSchemaGenerator.TypeSchema> _stack = (IList<JsonSchemaGenerator.TypeSchema>) new List<JsonSchemaGenerator.TypeSchema>();
    private IContractResolver _contractResolver;
    private JsonSchemaResolver _resolver;
    private JsonSchema _currentSchema;

    public UndefinedSchemaIdHandling UndefinedSchemaIdHandling { get; set; }

    public IContractResolver ContractResolver
    {
      get
      {
        if (this._contractResolver == null)
          return DefaultContractResolver.Instance;
        else
          return this._contractResolver;
      }
      set
      {
        this._contractResolver = value;
      }
    }

    private JsonSchema CurrentSchema
    {
      get
      {
        return this._currentSchema;
      }
    }

    private void Push(JsonSchemaGenerator.TypeSchema typeSchema)
    {
      this._currentSchema = typeSchema.Schema;
      this._stack.Add(typeSchema);
      this._resolver.LoadedSchemas.Add(typeSchema.Schema);
    }

    private JsonSchemaGenerator.TypeSchema Pop()
    {
      JsonSchemaGenerator.TypeSchema typeSchema1 = this._stack[this._stack.Count - 1];
      this._stack.RemoveAt(this._stack.Count - 1);
      JsonSchemaGenerator.TypeSchema typeSchema2 = Enumerable.LastOrDefault<JsonSchemaGenerator.TypeSchema>((IEnumerable<JsonSchemaGenerator.TypeSchema>) this._stack);
      this._currentSchema = typeSchema2 == null ? (JsonSchema) null : typeSchema2.Schema;
      return typeSchema1;
    }

    public JsonSchema Generate(Type type)
    {
      return this.Generate(type, new JsonSchemaResolver(), false);
    }

    public JsonSchema Generate(Type type, JsonSchemaResolver resolver)
    {
      return this.Generate(type, resolver, false);
    }

    public JsonSchema Generate(Type type, bool rootSchemaNullable)
    {
      return this.Generate(type, new JsonSchemaResolver(), rootSchemaNullable);
    }

    public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
    {
      ValidationUtils.ArgumentNotNull((object) type, "type");
      ValidationUtils.ArgumentNotNull((object) resolver, "resolver");
      this._resolver = resolver;
      return this.GenerateInternal(type, !rootSchemaNullable ? Required.Always : Required.Default, false);
    }

    private string GetTitle(Type type)
    {
      JsonContainerAttribute containerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
      if (containerAttribute != null && !string.IsNullOrEmpty(containerAttribute.Title))
        return containerAttribute.Title;
      else
        return (string) null;
    }

    private string GetDescription(Type type)
    {
      JsonContainerAttribute containerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
      if (containerAttribute != null && !string.IsNullOrEmpty(containerAttribute.Description))
        return containerAttribute.Description;
      DescriptionAttribute attribute = ReflectionUtils.GetAttribute<DescriptionAttribute>((ICustomAttributeProvider) type);
      if (attribute != null)
        return attribute.Description;
      else
        return (string) null;
    }

    private string GetTypeId(Type type, bool explicitOnly)
    {
      JsonContainerAttribute containerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
      if (containerAttribute != null && !string.IsNullOrEmpty(containerAttribute.Id))
        return containerAttribute.Id;
      if (explicitOnly)
        return (string) null;
      switch (this.UndefinedSchemaIdHandling)
      {
        case UndefinedSchemaIdHandling.UseTypeName:
          return type.FullName;
        case UndefinedSchemaIdHandling.UseAssemblyQualifiedName:
          return type.AssemblyQualifiedName;
        default:
          return (string) null;
      }
    }

    private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
    {
      ValidationUtils.ArgumentNotNull((object) type, "type");
      string typeId1 = this.GetTypeId(type, false);
      string typeId2 = this.GetTypeId(type, true);
      if (!string.IsNullOrEmpty(typeId1))
      {
        JsonSchema schema = this._resolver.GetSchema(typeId1);
        if (schema != null)
        {
          if (valueRequired != Required.Always && !JsonSchemaGenerator.HasFlag(schema.Type, JsonSchemaType.Null))
          {
            JsonSchema jsonSchema = schema;
            JsonSchemaType? type1 = jsonSchema.Type;
            JsonSchemaType? nullable = type1.HasValue ? new JsonSchemaType?(type1.GetValueOrDefault() | JsonSchemaType.Null) : new JsonSchemaType?();
            jsonSchema.Type = nullable;
          }
          if (required)
          {
            bool? required1 = schema.Required;
            if ((!required1.GetValueOrDefault() ? 1 : (!required1.HasValue ? 1 : 0)) != 0)
              schema.Required = new bool?(true);
          }
          return schema;
        }
      }
      if (Enumerable.Any<JsonSchemaGenerator.TypeSchema>((IEnumerable<JsonSchemaGenerator.TypeSchema>) this._stack, (Func<JsonSchemaGenerator.TypeSchema, bool>) (tc => tc.Type == type)))
        throw new JsonException(StringUtils.FormatWith("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      JsonContract jsonContract = this.ContractResolver.ResolveContract(type);
      JsonConverter jsonConverter;
      if ((jsonConverter = jsonContract.Converter) != null || (jsonConverter = jsonContract.InternalConverter) != null)
      {
        JsonSchema schema = jsonConverter.GetSchema();
        if (schema != null)
          return schema;
      }
      this.Push(new JsonSchemaGenerator.TypeSchema(type, new JsonSchema()));
      if (typeId2 != null)
        this.CurrentSchema.Id = typeId2;
      if (required)
        this.CurrentSchema.Required = new bool?(true);
      this.CurrentSchema.Title = this.GetTitle(type);
      this.CurrentSchema.Description = this.GetDescription(type);
      if (jsonConverter != null)
      {
        this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
      }
      else
      {
        switch (jsonContract.ContractType)
        {
          case JsonContractType.Object:
            this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
            this.CurrentSchema.Id = this.GetTypeId(type, false);
            this.GenerateObjectSchema(type, (JsonObjectContract) jsonContract);
            break;
          case JsonContractType.Array:
            this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Array, valueRequired));
            this.CurrentSchema.Id = this.GetTypeId(type, false);
            JsonArrayAttribute jsonArrayAttribute = JsonTypeReflector.GetJsonContainerAttribute(type) as JsonArrayAttribute;
            bool flag = jsonArrayAttribute == null || jsonArrayAttribute.AllowNullItems;
            Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
            if (collectionItemType != null)
            {
              this.CurrentSchema.Items = (IList<JsonSchema>) new List<JsonSchema>();
              this.CurrentSchema.Items.Add(this.GenerateInternal(collectionItemType, !flag ? Required.Always : Required.Default, false));
              break;
            }
            else
              break;
          case JsonContractType.Primitive:
            this.CurrentSchema.Type = new JsonSchemaType?(this.GetJsonSchemaType(type, valueRequired));
            JsonSchemaType? type2 = this.CurrentSchema.Type;
            if ((type2.GetValueOrDefault() != JsonSchemaType.Integer ? 0 : (type2.HasValue ? 1 : 0)) != 0 && TypeExtensions.IsEnum(type) && !type.IsDefined(typeof (FlagsAttribute), true))
            {
              this.CurrentSchema.Enum = (IList<JToken>) new List<JToken>();
              this.CurrentSchema.Options = (IDictionary<JToken, string>) new Dictionary<JToken, string>();
              using (IEnumerator<EnumValue<long>> enumerator = EnumUtils.GetNamesAndValues<long>(type).GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  EnumValue<long> current = enumerator.Current;
                  JToken key = JToken.FromObject((object) current.Value);
                  this.CurrentSchema.Enum.Add(key);
                  this.CurrentSchema.Options.Add(key, current.Name);
                }
                break;
              }
            }
            else
              break;
          case JsonContractType.String:
            this.CurrentSchema.Type = new JsonSchemaType?(!ReflectionUtils.IsNullable(jsonContract.UnderlyingType) ? JsonSchemaType.String : this.AddNullType(JsonSchemaType.String, valueRequired));
            break;
          case JsonContractType.Dictionary:
            this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
            Type keyType;
            Type valueType;
            ReflectionUtils.GetDictionaryKeyValueTypes(type, out keyType, out valueType);
            if (keyType != null && ConvertUtils.IsConvertible(keyType))
            {
              this.CurrentSchema.AdditionalProperties = this.GenerateInternal(valueType, Required.Default, false);
              break;
            }
            else
              break;
          case JsonContractType.Serializable:
            this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
            this.CurrentSchema.Id = this.GetTypeId(type, false);
            this.GenerateISerializableContract(type, (JsonISerializableContract) jsonContract);
            break;
          case JsonContractType.Linq:
            this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
            break;
          default:
            throw new JsonException(StringUtils.FormatWith("Unexpected contract type: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) jsonContract));
        }
      }
      return this.Pop().Schema;
    }

    private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired)
    {
      if (valueRequired != Required.Always)
        return type | JsonSchemaType.Null;
      else
        return type;
    }

    private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
    {
      return (value & flag) == flag;
    }

    private void GenerateObjectSchema(Type type, JsonObjectContract contract)
    {
      this.CurrentSchema.Properties = (IDictionary<string, JsonSchema>) new Dictionary<string, JsonSchema>();
      foreach (JsonProperty jsonProperty in (Collection<JsonProperty>) contract.Properties)
      {
        if (!jsonProperty.Ignored)
        {
          NullValueHandling? nullValueHandling = jsonProperty.NullValueHandling;
          bool flag = (nullValueHandling.GetValueOrDefault() != NullValueHandling.Ignore ? 0 : (nullValueHandling.HasValue ? 1 : 0)) != 0 || (this.HasFlag(jsonProperty.DefaultValueHandling.GetValueOrDefault(), DefaultValueHandling.Ignore) || jsonProperty.ShouldSerialize != null) || jsonProperty.GetIsSpecified != null;
          JsonSchema jsonSchema = this.GenerateInternal(jsonProperty.PropertyType, jsonProperty.Required, !flag);
          if (jsonProperty.DefaultValue != null)
            jsonSchema.Default = JToken.FromObject(jsonProperty.DefaultValue);
          this.CurrentSchema.Properties.Add(jsonProperty.PropertyName, jsonSchema);
        }
      }
      if (!TypeExtensions.IsSealed(type))
        return;
      this.CurrentSchema.AllowAdditionalProperties = false;
    }

    private void GenerateISerializableContract(Type type, JsonISerializableContract contract)
    {
      this.CurrentSchema.AllowAdditionalProperties = true;
    }

    internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
    {
      if (!value.HasValue)
        return true;
      JsonSchemaType? nullable1 = value;
      JsonSchemaType jsonSchemaType1 = flag;
      JsonSchemaType? nullable2 = nullable1.HasValue ? new JsonSchemaType?(nullable1.GetValueOrDefault() & jsonSchemaType1) : new JsonSchemaType?();
      JsonSchemaType jsonSchemaType2 = flag;
      if (nullable2.GetValueOrDefault() == jsonSchemaType2 && nullable2.HasValue)
        return true;
      JsonSchemaType? nullable3 = value;
      return (nullable3.GetValueOrDefault() != JsonSchemaType.Float ? 0 : (nullable3.HasValue ? 1 : 0)) != 0 && flag == JsonSchemaType.Integer;
    }

    private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
    {
      JsonSchemaType jsonSchemaType = JsonSchemaType.None;
      if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
      {
        jsonSchemaType = JsonSchemaType.Null;
        if (ReflectionUtils.IsNullableType(type))
          type = Nullable.GetUnderlyingType(type);
      }
      TypeCode typeCode = ConvertUtils.GetTypeCode(type);
      switch (typeCode)
      {
        case TypeCode.Empty:
        case TypeCode.Object:
          return jsonSchemaType | JsonSchemaType.String;
        case TypeCode.DBNull:
          return jsonSchemaType | JsonSchemaType.Null;
        case TypeCode.Boolean:
          return jsonSchemaType | JsonSchemaType.Boolean;
        case TypeCode.Char:
          return jsonSchemaType | JsonSchemaType.String;
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return jsonSchemaType | JsonSchemaType.Integer;
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return jsonSchemaType | JsonSchemaType.Float;
        case TypeCode.DateTime:
          return jsonSchemaType | JsonSchemaType.String;
        case TypeCode.String:
          return jsonSchemaType | JsonSchemaType.String;
        default:
          throw new JsonException(StringUtils.FormatWith("Unexpected type code '{0}' for type '{1}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) typeCode, (object) type));
      }
    }

    private class TypeSchema
    {
      public Type Type { get; private set; }

      public JsonSchema Schema { get; private set; }

      public TypeSchema(Type type, JsonSchema schema)
      {
        ValidationUtils.ArgumentNotNull((object) type, "type");
        ValidationUtils.ArgumentNotNull((object) schema, "schema");
        this.Type = type;
        this.Schema = schema;
      }
    }
  }
}
