// Type: Newtonsoft.Json.Schema.JsonSchemaBuilder
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
using System.Globalization;

namespace Newtonsoft.Json.Schema
{
  internal class JsonSchemaBuilder
  {
    private JsonReader _reader;
    private readonly IList<JsonSchema> _stack;
    private readonly JsonSchemaResolver _resolver;
    private JsonSchema _currentSchema;

    private JsonSchema CurrentSchema
    {
      get
      {
        return this._currentSchema;
      }
    }

    public JsonSchemaBuilder(JsonSchemaResolver resolver)
    {
      this._stack = (IList<JsonSchema>) new List<JsonSchema>();
      this._resolver = resolver;
    }

    private void Push(JsonSchema value)
    {
      this._currentSchema = value;
      this._stack.Add(value);
      this._resolver.LoadedSchemas.Add(value);
    }

    private JsonSchema Pop()
    {
      JsonSchema jsonSchema = this._currentSchema;
      this._stack.RemoveAt(this._stack.Count - 1);
      this._currentSchema = Enumerable.LastOrDefault<JsonSchema>((IEnumerable<JsonSchema>) this._stack);
      return jsonSchema;
    }

    internal JsonSchema Parse(JsonReader reader)
    {
      this._reader = reader;
      if (reader.TokenType == JsonToken.None)
        this._reader.Read();
      return this.BuildSchema();
    }

    private JsonSchema BuildSchema()
    {
      if (this._reader.TokenType != JsonToken.StartObject)
        throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Expected StartObject while parsing schema object, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      this._reader.Read();
      if (this._reader.TokenType == JsonToken.EndObject)
      {
        this.Push(new JsonSchema());
        return this.Pop();
      }
      else
      {
        string propertyName1 = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        this._reader.Read();
        if (propertyName1 == "$ref")
        {
          string id = (string) this._reader.Value;
          while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
          {
            if (this._reader.TokenType == JsonToken.StartObject)
              throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Found StartObject within the schema reference with the Id '{0}'", (IFormatProvider) CultureInfo.InvariantCulture, (object) id));
          }
          JsonSchema schema = this._resolver.GetSchema(id);
          if (schema == null)
            throw new JsonException(StringUtils.FormatWith("Could not resolve schema reference for Id '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) id));
          else
            return schema;
        }
        else
        {
          this.Push(new JsonSchema());
          this.ProcessSchemaProperty(propertyName1);
          while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
          {
            string propertyName2 = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
            this._reader.Read();
            this.ProcessSchemaProperty(propertyName2);
          }
          return this.Pop();
        }
      }
    }

    private void ProcessSchemaProperty(string propertyName)
    {
      switch (propertyName)
      {
        case "type":
          this.CurrentSchema.Type = this.ProcessType();
          break;
        case "id":
          this.CurrentSchema.Id = (string) this._reader.Value;
          break;
        case "title":
          this.CurrentSchema.Title = (string) this._reader.Value;
          break;
        case "description":
          this.CurrentSchema.Description = (string) this._reader.Value;
          break;
        case "properties":
          this.ProcessProperties();
          break;
        case "items":
          this.ProcessItems();
          break;
        case "additionalProperties":
          this.ProcessAdditionalProperties();
          break;
        case "patternProperties":
          this.ProcessPatternProperties();
          break;
        case "required":
          this.CurrentSchema.Required = new bool?((bool) this._reader.Value);
          break;
        case "requires":
          this.CurrentSchema.Requires = (string) this._reader.Value;
          break;
        case "identity":
          this.ProcessIdentity();
          break;
        case "minimum":
          this.CurrentSchema.Minimum = new double?(Convert.ToDouble(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "maximum":
          this.CurrentSchema.Maximum = new double?(Convert.ToDouble(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "exclusiveMinimum":
          this.CurrentSchema.ExclusiveMinimum = new bool?((bool) this._reader.Value);
          break;
        case "exclusiveMaximum":
          this.CurrentSchema.ExclusiveMaximum = new bool?((bool) this._reader.Value);
          break;
        case "maxLength":
          this.CurrentSchema.MaximumLength = new int?(Convert.ToInt32(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "minLength":
          this.CurrentSchema.MinimumLength = new int?(Convert.ToInt32(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "maxItems":
          this.CurrentSchema.MaximumItems = new int?(Convert.ToInt32(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "minItems":
          this.CurrentSchema.MinimumItems = new int?(Convert.ToInt32(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "divisibleBy":
          this.CurrentSchema.DivisibleBy = new double?(Convert.ToDouble(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "disallow":
          this.CurrentSchema.Disallow = this.ProcessType();
          break;
        case "default":
          this.ProcessDefault();
          break;
        case "hidden":
          this.CurrentSchema.Hidden = new bool?((bool) this._reader.Value);
          break;
        case "readonly":
          this.CurrentSchema.ReadOnly = new bool?((bool) this._reader.Value);
          break;
        case "format":
          this.CurrentSchema.Format = (string) this._reader.Value;
          break;
        case "pattern":
          this.CurrentSchema.Pattern = (string) this._reader.Value;
          break;
        case "options":
          this.ProcessOptions();
          break;
        case "enum":
          this.ProcessEnum();
          break;
        case "extends":
          this.ProcessExtends();
          break;
        default:
          this._reader.Skip();
          break;
      }
    }

    private void ProcessExtends()
    {
      this.CurrentSchema.Extends = this.BuildSchema();
    }

    private void ProcessEnum()
    {
      if (this._reader.TokenType != JsonToken.StartArray)
        throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Expected StartArray token while parsing enum values, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      this.CurrentSchema.Enum = (IList<JToken>) new List<JToken>();
      while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
        this.CurrentSchema.Enum.Add(JToken.ReadFrom(this._reader));
    }

    private void ProcessOptions()
    {
      this.CurrentSchema.Options = (IDictionary<JToken, string>) new Dictionary<JToken, string>((IEqualityComparer<JToken>) new JTokenEqualityComparer());
      if (this._reader.TokenType != JsonToken.StartArray)
        throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Expected array token, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
      {
        if (this._reader.TokenType != JsonToken.StartObject)
          throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Expect object token, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
        string str1 = (string) null;
        JToken key = (JToken) null;
        while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
        {
          string str2 = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
          this._reader.Read();
          switch (str2)
          {
            case "value":
              key = JToken.ReadFrom(this._reader);
              continue;
            case "label":
              str1 = (string) this._reader.Value;
              continue;
            default:
              throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Unexpected property in JSON schema option: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) str2));
          }
        }
        if (key == null)
          throw new JsonException("No value specified for JSON schema option.");
        if (this.CurrentSchema.Options.ContainsKey(key))
          throw new JsonException(StringUtils.FormatWith("Duplicate value in JSON schema option collection: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) key));
        this.CurrentSchema.Options.Add(key, str1);
      }
    }

    private void ProcessDefault()
    {
      this.CurrentSchema.Default = JToken.ReadFrom(this._reader);
    }

    private void ProcessIdentity()
    {
      this.CurrentSchema.Identity = (IList<string>) new List<string>();
      switch (this._reader.TokenType)
      {
        case JsonToken.StartArray:
          while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
          {
            if (this._reader.TokenType != JsonToken.String)
              throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Exception JSON property name string token, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
            this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
          }
          break;
        case JsonToken.String:
          this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
          break;
        default:
          throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Expected array or JSON property name string token, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      }
    }

    private void ProcessAdditionalProperties()
    {
      if (this._reader.TokenType == JsonToken.Boolean)
        this.CurrentSchema.AllowAdditionalProperties = (bool) this._reader.Value;
      else
        this.CurrentSchema.AdditionalProperties = this.BuildSchema();
    }

    private void ProcessPatternProperties()
    {
      Dictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
      if (this._reader.TokenType != JsonToken.StartObject)
        throw JsonReaderException.Create(this._reader, "Expected StartObject token.");
      while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
      {
        string key = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        this._reader.Read();
        if (dictionary.ContainsKey(key))
          throw new JsonException(StringUtils.FormatWith("Property {0} has already been defined in schema.", (IFormatProvider) CultureInfo.InvariantCulture, (object) key));
        dictionary.Add(key, this.BuildSchema());
      }
      this.CurrentSchema.PatternProperties = (IDictionary<string, JsonSchema>) dictionary;
    }

    private void ProcessItems()
    {
      this.CurrentSchema.Items = (IList<JsonSchema>) new List<JsonSchema>();
      switch (this._reader.TokenType)
      {
        case JsonToken.StartObject:
          this.CurrentSchema.Items.Add(this.BuildSchema());
          break;
        case JsonToken.StartArray:
          while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
            this.CurrentSchema.Items.Add(this.BuildSchema());
          break;
        default:
          throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Expected array or JSON schema object token, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      }
    }

    private void ProcessProperties()
    {
      IDictionary<string, JsonSchema> dictionary = (IDictionary<string, JsonSchema>) new Dictionary<string, JsonSchema>();
      if (this._reader.TokenType != JsonToken.StartObject)
        throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Expected StartObject token while parsing schema properties, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
      {
        string key = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        this._reader.Read();
        if (dictionary.ContainsKey(key))
          throw new JsonException(StringUtils.FormatWith("Property {0} has already been defined in schema.", (IFormatProvider) CultureInfo.InvariantCulture, (object) key));
        dictionary.Add(key, this.BuildSchema());
      }
      this.CurrentSchema.Properties = dictionary;
    }

    private JsonSchemaType? ProcessType()
    {
      switch (this._reader.TokenType)
      {
        case JsonToken.StartArray:
          JsonSchemaType? nullable1 = new JsonSchemaType?(JsonSchemaType.None);
          while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
          {
            if (this._reader.TokenType != JsonToken.String)
              throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Exception JSON schema type string token, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
            JsonSchemaType? nullable2 = nullable1;
            JsonSchemaType jsonSchemaType = JsonSchemaBuilder.MapType(this._reader.Value.ToString());
            nullable1 = nullable2.HasValue ? new JsonSchemaType?(nullable2.GetValueOrDefault() | jsonSchemaType) : new JsonSchemaType?();
          }
          return nullable1;
        case JsonToken.String:
          return new JsonSchemaType?(JsonSchemaBuilder.MapType(this._reader.Value.ToString()));
        default:
          throw JsonReaderException.Create(this._reader, StringUtils.FormatWith("Expected array or JSON schema type string token, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._reader.TokenType));
      }
    }

    internal static JsonSchemaType MapType(string type)
    {
      JsonSchemaType jsonSchemaType;
      if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, out jsonSchemaType))
        throw new JsonException(StringUtils.FormatWith("Invalid JSON schema type: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      else
        return jsonSchemaType;
    }

    internal static string MapType(JsonSchemaType type)
    {
      return Enumerable.Single<KeyValuePair<string, JsonSchemaType>>((IEnumerable<KeyValuePair<string, JsonSchemaType>>) JsonSchemaConstants.JsonSchemaTypeMapping, (Func<KeyValuePair<string, JsonSchemaType>, bool>) (kv => kv.Value == type)).Key;
    }
  }
}
