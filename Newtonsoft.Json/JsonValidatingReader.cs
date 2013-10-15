// Type: Newtonsoft.Json.JsonValidatingReader
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json
{
  public class JsonValidatingReader : JsonReader, IJsonLineInfo
  {
    private readonly JsonReader _reader;
    private readonly Stack<JsonValidatingReader.SchemaScope> _stack;
    private JsonSchema _schema;
    private JsonSchemaModel _model;
    private JsonValidatingReader.SchemaScope _currentScope;

    public override object Value
    {
      get
      {
        return this._reader.Value;
      }
    }

    public override int Depth
    {
      get
      {
        return this._reader.Depth;
      }
    }

    public override string Path
    {
      get
      {
        return this._reader.Path;
      }
    }

    public override char QuoteChar
    {
      get
      {
        return this._reader.QuoteChar;
      }
      protected internal set
      {
      }
    }

    public override JsonToken TokenType
    {
      get
      {
        return this._reader.TokenType;
      }
    }

    public override Type ValueType
    {
      get
      {
        return this._reader.ValueType;
      }
    }

    private IEnumerable<JsonSchemaModel> CurrentSchemas
    {
      get
      {
        return (IEnumerable<JsonSchemaModel>) this._currentScope.Schemas;
      }
    }

    private IEnumerable<JsonSchemaModel> CurrentMemberSchemas
    {
      get
      {
        if (this._currentScope == null)
        {
          return (IEnumerable<JsonSchemaModel>) new List<JsonSchemaModel>((IEnumerable<JsonSchemaModel>) new JsonSchemaModel[1]
          {
            this._model
          });
        }
        else
        {
          if (this._currentScope.Schemas == null || this._currentScope.Schemas.Count == 0)
            return Enumerable.Empty<JsonSchemaModel>();
          switch (this._currentScope.TokenType)
          {
            case JTokenType.None:
              return (IEnumerable<JsonSchemaModel>) this._currentScope.Schemas;
            case JTokenType.Object:
              if (this._currentScope.CurrentPropertyName == null)
                throw new JsonReaderException("CurrentPropertyName has not been set on scope.");
              IList<JsonSchemaModel> list1 = (IList<JsonSchemaModel>) new List<JsonSchemaModel>();
              foreach (JsonSchemaModel jsonSchemaModel1 in this.CurrentSchemas)
              {
                JsonSchemaModel jsonSchemaModel2;
                if (jsonSchemaModel1.Properties != null && jsonSchemaModel1.Properties.TryGetValue(this._currentScope.CurrentPropertyName, out jsonSchemaModel2))
                  list1.Add(jsonSchemaModel2);
                if (jsonSchemaModel1.PatternProperties != null)
                {
                  foreach (KeyValuePair<string, JsonSchemaModel> keyValuePair in (IEnumerable<KeyValuePair<string, JsonSchemaModel>>) jsonSchemaModel1.PatternProperties)
                  {
                    if (Regex.IsMatch(this._currentScope.CurrentPropertyName, keyValuePair.Key))
                      list1.Add(keyValuePair.Value);
                  }
                }
                if (list1.Count == 0 && jsonSchemaModel1.AllowAdditionalProperties && jsonSchemaModel1.AdditionalProperties != null)
                  list1.Add(jsonSchemaModel1.AdditionalProperties);
              }
              return (IEnumerable<JsonSchemaModel>) list1;
            case JTokenType.Array:
              IList<JsonSchemaModel> list2 = (IList<JsonSchemaModel>) new List<JsonSchemaModel>();
              foreach (JsonSchemaModel jsonSchemaModel in this.CurrentSchemas)
              {
                if (!CollectionUtils.IsNullOrEmpty<JsonSchemaModel>((ICollection<JsonSchemaModel>) jsonSchemaModel.Items))
                {
                  if (jsonSchemaModel.Items.Count == 1)
                    list2.Add(jsonSchemaModel.Items[0]);
                  else if (jsonSchemaModel.Items.Count > this._currentScope.ArrayItemCount - 1)
                    list2.Add(jsonSchemaModel.Items[this._currentScope.ArrayItemCount - 1]);
                }
                if (jsonSchemaModel.AllowAdditionalProperties && jsonSchemaModel.AdditionalProperties != null)
                  list2.Add(jsonSchemaModel.AdditionalProperties);
              }
              return (IEnumerable<JsonSchemaModel>) list2;
            case JTokenType.Constructor:
              return Enumerable.Empty<JsonSchemaModel>();
            default:
              throw new ArgumentOutOfRangeException("TokenType", StringUtils.FormatWith("Unexpected token type: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._currentScope.TokenType));
          }
        }
      }
    }

    public JsonSchema Schema
    {
      get
      {
        return this._schema;
      }
      set
      {
        if (this.TokenType != JsonToken.None)
          throw new InvalidOperationException("Cannot change schema while validating JSON.");
        this._schema = value;
        this._model = (JsonSchemaModel) null;
      }
    }

    public JsonReader Reader
    {
      get
      {
        return this._reader;
      }
    }

    int IJsonLineInfo.LineNumber
    {
      get
      {
        IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
        if (jsonLineInfo == null)
          return 0;
        else
          return jsonLineInfo.LineNumber;
      }
    }

    int IJsonLineInfo.LinePosition
    {
      get
      {
        IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
        if (jsonLineInfo == null)
          return 0;
        else
          return jsonLineInfo.LinePosition;
      }
    }

    public event ValidationEventHandler ValidationEventHandler;

    public JsonValidatingReader(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, "reader");
      this._reader = reader;
      this._stack = new Stack<JsonValidatingReader.SchemaScope>();
    }

    private void Push(JsonValidatingReader.SchemaScope scope)
    {
      this._stack.Push(scope);
      this._currentScope = scope;
    }

    private JsonValidatingReader.SchemaScope Pop()
    {
      JsonValidatingReader.SchemaScope schemaScope = this._stack.Pop();
      this._currentScope = this._stack.Count != 0 ? this._stack.Peek() : (JsonValidatingReader.SchemaScope) null;
      return schemaScope;
    }

    private void RaiseError(string message, JsonSchemaModel schema)
    {
      IJsonLineInfo jsonLineInfo = (IJsonLineInfo) this;
      this.OnValidationEvent(new JsonSchemaException(jsonLineInfo.HasLineInfo() ? message + StringUtils.FormatWith(" Line {0}, position {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) jsonLineInfo.LineNumber, (object) jsonLineInfo.LinePosition) : message, (Exception) null, this.Path, jsonLineInfo.LineNumber, jsonLineInfo.LinePosition));
    }

    private void OnValidationEvent(JsonSchemaException exception)
    {
      ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
      if (validationEventHandler == null)
        throw exception;
      validationEventHandler((object) this, new ValidationEventArgs(exception));
    }

    private void ValidateInEnumAndNotDisallowed(JsonSchemaModel schema)
    {
      if (schema == null)
        return;
      JToken jtoken = (JToken) new JValue(this._reader.Value);
      if (schema.Enum != null)
      {
        StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
        jtoken.WriteTo((JsonWriter) new JsonTextWriter((TextWriter) stringWriter), new JsonConverter[0]);
        if (!CollectionUtils.ContainsValue<JToken>((IEnumerable<JToken>) schema.Enum, jtoken, (IEqualityComparer<JToken>) new JTokenEqualityComparer()))
          this.RaiseError(StringUtils.FormatWith("Value {0} is not defined in enum.", (IFormatProvider) CultureInfo.InvariantCulture, (object) stringWriter.ToString()), schema);
      }
      JsonSchemaType? currentNodeSchemaType = this.GetCurrentNodeSchemaType();
      if (!currentNodeSchemaType.HasValue || !JsonSchemaGenerator.HasFlag(new JsonSchemaType?(schema.Disallow), currentNodeSchemaType.Value))
        return;
      this.RaiseError(StringUtils.FormatWith("Type {0} is disallowed.", (IFormatProvider) CultureInfo.InvariantCulture, (object) currentNodeSchemaType), schema);
    }

    private JsonSchemaType? GetCurrentNodeSchemaType()
    {
      switch (this._reader.TokenType)
      {
        case JsonToken.StartObject:
          return new JsonSchemaType?(JsonSchemaType.Object);
        case JsonToken.StartArray:
          return new JsonSchemaType?(JsonSchemaType.Array);
        case JsonToken.Integer:
          return new JsonSchemaType?(JsonSchemaType.Integer);
        case JsonToken.Float:
          return new JsonSchemaType?(JsonSchemaType.Float);
        case JsonToken.String:
          return new JsonSchemaType?(JsonSchemaType.String);
        case JsonToken.Boolean:
          return new JsonSchemaType?(JsonSchemaType.Boolean);
        case JsonToken.Null:
          return new JsonSchemaType?(JsonSchemaType.Null);
        default:
          return new JsonSchemaType?();
      }
    }

    public override int? ReadAsInt32()
    {
      int? nullable = this._reader.ReadAsInt32();
      this.ValidateCurrentToken();
      return nullable;
    }

    public override byte[] ReadAsBytes()
    {
      byte[] numArray = this._reader.ReadAsBytes();
      this.ValidateCurrentToken();
      return numArray;
    }

    public override Decimal? ReadAsDecimal()
    {
      Decimal? nullable = this._reader.ReadAsDecimal();
      this.ValidateCurrentToken();
      return nullable;
    }

    public override string ReadAsString()
    {
      string str = this._reader.ReadAsString();
      this.ValidateCurrentToken();
      return str;
    }

    public override DateTime? ReadAsDateTime()
    {
      DateTime? nullable = this._reader.ReadAsDateTime();
      this.ValidateCurrentToken();
      return nullable;
    }

    public override bool Read()
    {
      if (!this._reader.Read())
        return false;
      if (this._reader.TokenType == JsonToken.Comment)
        return true;
      this.ValidateCurrentToken();
      return true;
    }

    private void ValidateCurrentToken()
    {
      if (this._model == null)
        this._model = new JsonSchemaModelBuilder().Build(this._schema);
      switch (this._reader.TokenType)
      {
        case JsonToken.None:
          break;
        case JsonToken.StartObject:
          this.ProcessValue();
          this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Object, (IList<JsonSchemaModel>) Enumerable.ToList<JsonSchemaModel>(Enumerable.Where<JsonSchemaModel>(this.CurrentMemberSchemas, new Func<JsonSchemaModel, bool>(this.ValidateObject)))));
          break;
        case JsonToken.StartArray:
          this.ProcessValue();
          this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Array, (IList<JsonSchemaModel>) Enumerable.ToList<JsonSchemaModel>(Enumerable.Where<JsonSchemaModel>(this.CurrentMemberSchemas, new Func<JsonSchemaModel, bool>(this.ValidateArray)))));
          break;
        case JsonToken.StartConstructor:
          this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Constructor, (IList<JsonSchemaModel>) null));
          break;
        case JsonToken.PropertyName:
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidatePropertyName(enumerator.Current);
            break;
          }
        case JsonToken.Raw:
          break;
        case JsonToken.Integer:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateInteger(enumerator.Current);
            break;
          }
        case JsonToken.Float:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateFloat(enumerator.Current);
            break;
          }
        case JsonToken.String:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateString(enumerator.Current);
            break;
          }
        case JsonToken.Boolean:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateBoolean(enumerator.Current);
            break;
          }
        case JsonToken.Null:
          this.ProcessValue();
          using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ValidateNull(enumerator.Current);
            break;
          }
        case JsonToken.Undefined:
          break;
        case JsonToken.EndObject:
          foreach (JsonSchemaModel schema in this.CurrentSchemas)
            this.ValidateEndObject(schema);
          this.Pop();
          break;
        case JsonToken.EndArray:
          foreach (JsonSchemaModel schema in this.CurrentSchemas)
            this.ValidateEndArray(schema);
          this.Pop();
          break;
        case JsonToken.EndConstructor:
          this.Pop();
          break;
        case JsonToken.Date:
          break;
        case JsonToken.Bytes:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void ValidateEndObject(JsonSchemaModel schema)
    {
      if (schema == null)
        return;
      Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
      if (requiredProperties == null)
        return;
      List<string> list = Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<string, bool>, string>(Enumerable.Where<KeyValuePair<string, bool>>((IEnumerable<KeyValuePair<string, bool>>) requiredProperties, (Func<KeyValuePair<string, bool>, bool>) (kv => !kv.Value)), (Func<KeyValuePair<string, bool>, string>) (kv => kv.Key)));
      if (list.Count <= 0)
        return;
      this.RaiseError(StringUtils.FormatWith("Required properties are missing from object: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) string.Join(", ", list.ToArray())), schema);
    }

    private void ValidateEndArray(JsonSchemaModel schema)
    {
      if (schema == null)
        return;
      int arrayItemCount = this._currentScope.ArrayItemCount;
      if (schema.MaximumItems.HasValue)
      {
        int num = arrayItemCount;
        int? maximumItems = schema.MaximumItems;
        if ((num <= maximumItems.GetValueOrDefault() ? 0 : (maximumItems.HasValue ? 1 : 0)) != 0)
          this.RaiseError(StringUtils.FormatWith("Array item count {0} exceeds maximum count of {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) arrayItemCount, (object) schema.MaximumItems), schema);
      }
      if (!schema.MinimumItems.HasValue)
        return;
      int num1 = arrayItemCount;
      int? minimumItems = schema.MinimumItems;
      if ((num1 >= minimumItems.GetValueOrDefault() ? 0 : (minimumItems.HasValue ? 1 : 0)) == 0)
        return;
      this.RaiseError(StringUtils.FormatWith("Array item count {0} is less than minimum count of {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) arrayItemCount, (object) schema.MinimumItems), schema);
    }

    private void ValidateNull(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.Null))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
    }

    private void ValidateBoolean(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.Boolean))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
    }

    private void ValidateString(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.String))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
      string input = this._reader.Value.ToString();
      if (schema.MaximumLength.HasValue)
      {
        int length = input.Length;
        int? maximumLength = schema.MaximumLength;
        if ((length <= maximumLength.GetValueOrDefault() ? 0 : (maximumLength.HasValue ? 1 : 0)) != 0)
          this.RaiseError(StringUtils.FormatWith("String '{0}' exceeds maximum length of {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) input, (object) schema.MaximumLength), schema);
      }
      if (schema.MinimumLength.HasValue)
      {
        int length = input.Length;
        int? minimumLength = schema.MinimumLength;
        if ((length >= minimumLength.GetValueOrDefault() ? 0 : (minimumLength.HasValue ? 1 : 0)) != 0)
          this.RaiseError(StringUtils.FormatWith("String '{0}' is less than minimum length of {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) input, (object) schema.MinimumLength), schema);
      }
      if (schema.Patterns == null)
        return;
      foreach (string pattern in (IEnumerable<string>) schema.Patterns)
      {
        if (!Regex.IsMatch(input, pattern))
          this.RaiseError(StringUtils.FormatWith("String '{0}' does not match regex pattern '{1}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) input, (object) pattern), schema);
      }
    }

    private void ValidateInteger(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.Integer))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
      long num1 = Convert.ToInt64(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (schema.Maximum.HasValue)
      {
        double num2 = (double) num1;
        double? maximum1 = schema.Maximum;
        if ((num2 <= maximum1.GetValueOrDefault() ? 0 : (maximum1.HasValue ? 1 : 0)) != 0)
          this.RaiseError(StringUtils.FormatWith("Integer {0} exceeds maximum value of {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) num1, (object) schema.Maximum), schema);
        if (schema.ExclusiveMaximum)
        {
          double num3 = (double) num1;
          double? maximum2 = schema.Maximum;
          if ((num3 != maximum2.GetValueOrDefault() ? 0 : (maximum2.HasValue ? 1 : 0)) != 0)
            this.RaiseError(StringUtils.FormatWith("Integer {0} equals maximum value of {1} and exclusive maximum is true.", (IFormatProvider) CultureInfo.InvariantCulture, (object) num1, (object) schema.Maximum), schema);
        }
      }
      if (schema.Minimum.HasValue)
      {
        double num2 = (double) num1;
        double? minimum1 = schema.Minimum;
        if ((num2 >= minimum1.GetValueOrDefault() ? 0 : (minimum1.HasValue ? 1 : 0)) != 0)
          this.RaiseError(StringUtils.FormatWith("Integer {0} is less than minimum value of {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) num1, (object) schema.Minimum), schema);
        if (schema.ExclusiveMinimum)
        {
          double num3 = (double) num1;
          double? minimum2 = schema.Minimum;
          if ((num3 != minimum2.GetValueOrDefault() ? 0 : (minimum2.HasValue ? 1 : 0)) != 0)
            this.RaiseError(StringUtils.FormatWith("Integer {0} equals minimum value of {1} and exclusive minimum is true.", (IFormatProvider) CultureInfo.InvariantCulture, (object) num1, (object) schema.Minimum), schema);
        }
      }
      if (!schema.DivisibleBy.HasValue || JsonValidatingReader.IsZero((double) num1 % schema.DivisibleBy.Value))
        return;
      this.RaiseError(StringUtils.FormatWith("Integer {0} is not evenly divisible by {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.DivisibleBy), schema);
    }

    private void ProcessValue()
    {
      if (this._currentScope == null || this._currentScope.TokenType != JTokenType.Array)
        return;
      ++this._currentScope.ArrayItemCount;
      foreach (JsonSchemaModel schema in this.CurrentSchemas)
      {
        if (schema != null && schema.Items != null && (schema.Items.Count > 1 && this._currentScope.ArrayItemCount >= schema.Items.Count))
          this.RaiseError(StringUtils.FormatWith("Index {0} has not been defined and the schema does not allow additional items.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._currentScope.ArrayItemCount), schema);
      }
    }

    private void ValidateFloat(JsonSchemaModel schema)
    {
      if (schema == null || !this.TestType(schema, JsonSchemaType.Float))
        return;
      this.ValidateInEnumAndNotDisallowed(schema);
      double num1 = Convert.ToDouble(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (schema.Maximum.HasValue)
      {
        double num2 = num1;
        double? maximum1 = schema.Maximum;
        if ((num2 <= maximum1.GetValueOrDefault() ? 0 : (maximum1.HasValue ? 1 : 0)) != 0)
          this.RaiseError(StringUtils.FormatWith("Float {0} exceeds maximum value of {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.Maximum), schema);
        if (schema.ExclusiveMaximum)
        {
          double num3 = num1;
          double? maximum2 = schema.Maximum;
          if ((num3 != maximum2.GetValueOrDefault() ? 0 : (maximum2.HasValue ? 1 : 0)) != 0)
            this.RaiseError(StringUtils.FormatWith("Float {0} equals maximum value of {1} and exclusive maximum is true.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.Maximum), schema);
        }
      }
      if (schema.Minimum.HasValue)
      {
        double num2 = num1;
        double? minimum1 = schema.Minimum;
        if ((num2 >= minimum1.GetValueOrDefault() ? 0 : (minimum1.HasValue ? 1 : 0)) != 0)
          this.RaiseError(StringUtils.FormatWith("Float {0} is less than minimum value of {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.Minimum), schema);
        if (schema.ExclusiveMinimum)
        {
          double num3 = num1;
          double? minimum2 = schema.Minimum;
          if ((num3 != minimum2.GetValueOrDefault() ? 0 : (minimum2.HasValue ? 1 : 0)) != 0)
            this.RaiseError(StringUtils.FormatWith("Float {0} equals minimum value of {1} and exclusive minimum is true.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.Minimum), schema);
        }
      }
      if (!schema.DivisibleBy.HasValue || JsonValidatingReader.IsZero(num1 % schema.DivisibleBy.Value))
        return;
      this.RaiseError(StringUtils.FormatWith("Float {0} is not evenly divisible by {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JsonConvert.ToString(num1), (object) schema.DivisibleBy), schema);
    }

    private static bool IsZero(double value)
    {
      return Math.Abs(value) < 2.22044604925031E-15;
    }

    private void ValidatePropertyName(JsonSchemaModel schema)
    {
      if (schema == null)
        return;
      string index = Convert.ToString(this._reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (this._currentScope.RequiredProperties.ContainsKey(index))
        this._currentScope.RequiredProperties[index] = true;
      if (!schema.AllowAdditionalProperties && !this.IsPropertyDefinied(schema, index))
        this.RaiseError(StringUtils.FormatWith("Property '{0}' has not been defined and the schema does not allow additional properties.", (IFormatProvider) CultureInfo.InvariantCulture, (object) index), schema);
      this._currentScope.CurrentPropertyName = index;
    }

    private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
    {
      if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
        return true;
      if (schema.PatternProperties != null)
      {
        foreach (string pattern in (IEnumerable<string>) schema.PatternProperties.Keys)
        {
          if (Regex.IsMatch(propertyName, pattern))
            return true;
        }
      }
      return false;
    }

    private bool ValidateArray(JsonSchemaModel schema)
    {
      if (schema == null)
        return true;
      else
        return this.TestType(schema, JsonSchemaType.Array);
    }

    private bool ValidateObject(JsonSchemaModel schema)
    {
      if (schema == null)
        return true;
      else
        return this.TestType(schema, JsonSchemaType.Object);
    }

    private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
    {
      if (JsonSchemaGenerator.HasFlag(new JsonSchemaType?(currentSchema.Type), currentType))
        return true;
      this.RaiseError(StringUtils.FormatWith("Invalid type. Expected {0} but got {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) currentSchema.Type, (object) currentType), currentSchema);
      return false;
    }

    bool IJsonLineInfo.HasLineInfo()
    {
      IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
      if (jsonLineInfo != null)
        return jsonLineInfo.HasLineInfo();
      else
        return false;
    }

    private class SchemaScope
    {
      private readonly JTokenType _tokenType;
      private readonly IList<JsonSchemaModel> _schemas;
      private readonly Dictionary<string, bool> _requiredProperties;

      public string CurrentPropertyName { get; set; }

      public int ArrayItemCount { get; set; }

      public IList<JsonSchemaModel> Schemas
      {
        get
        {
          return this._schemas;
        }
      }

      public Dictionary<string, bool> RequiredProperties
      {
        get
        {
          return this._requiredProperties;
        }
      }

      public JTokenType TokenType
      {
        get
        {
          return this._tokenType;
        }
      }

      public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
      {
        this._tokenType = tokenType;
        this._schemas = schemas;
        this._requiredProperties = Enumerable.ToDictionary<string, string, bool>(Enumerable.Distinct<string>(Enumerable.SelectMany<JsonSchemaModel, string>((IEnumerable<JsonSchemaModel>) schemas, new Func<JsonSchemaModel, IEnumerable<string>>(this.GetRequiredProperties))), (Func<string, string>) (p => p), (Func<string, bool>) (p => false));
      }

      private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
      {
        if (schema == null || schema.Properties == null)
          return Enumerable.Empty<string>();
        else
          return Enumerable.Select<KeyValuePair<string, JsonSchemaModel>, string>(Enumerable.Where<KeyValuePair<string, JsonSchemaModel>>((IEnumerable<KeyValuePair<string, JsonSchemaModel>>) schema.Properties, (Func<KeyValuePair<string, JsonSchemaModel>, bool>) (p => p.Value.Required)), (Func<KeyValuePair<string, JsonSchemaModel>, string>) (p => p.Key));
      }
    }
  }
}
