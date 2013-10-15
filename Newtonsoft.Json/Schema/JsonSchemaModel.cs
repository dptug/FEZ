// Type: Newtonsoft.Json.Schema.JsonSchemaModel
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
  internal class JsonSchemaModel
  {
    public bool Required { get; set; }

    public JsonSchemaType Type { get; set; }

    public int? MinimumLength { get; set; }

    public int? MaximumLength { get; set; }

    public double? DivisibleBy { get; set; }

    public double? Minimum { get; set; }

    public double? Maximum { get; set; }

    public bool ExclusiveMinimum { get; set; }

    public bool ExclusiveMaximum { get; set; }

    public int? MinimumItems { get; set; }

    public int? MaximumItems { get; set; }

    public IList<string> Patterns { get; set; }

    public IList<JsonSchemaModel> Items { get; set; }

    public IDictionary<string, JsonSchemaModel> Properties { get; set; }

    public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

    public JsonSchemaModel AdditionalProperties { get; set; }

    public bool AllowAdditionalProperties { get; set; }

    public IList<JToken> Enum { get; set; }

    public JsonSchemaType Disallow { get; set; }

    public JsonSchemaModel()
    {
      this.Type = JsonSchemaType.Any;
      this.AllowAdditionalProperties = true;
      this.Required = false;
    }

    public static JsonSchemaModel Create(IList<JsonSchema> schemata)
    {
      JsonSchemaModel model = new JsonSchemaModel();
      foreach (JsonSchema schema in (IEnumerable<JsonSchema>) schemata)
        JsonSchemaModel.Combine(model, schema);
      return model;
    }

    private static void Combine(JsonSchemaModel model, JsonSchema schema)
    {
      JsonSchemaModel jsonSchemaModel1 = model;
      int num1;
      if (!model.Required)
      {
        bool? required = schema.Required;
        num1 = required.HasValue ? (required.GetValueOrDefault() ? 1 : 0) : 0;
      }
      else
        num1 = 1;
      jsonSchemaModel1.Required = num1 != 0;
      JsonSchemaModel jsonSchemaModel2 = model;
      int num2 = (int) model.Type;
      JsonSchemaType? type = schema.Type;
      int num3 = type.HasValue ? (int) type.GetValueOrDefault() : (int) sbyte.MaxValue;
      int num4 = num2 & num3;
      jsonSchemaModel2.Type = (JsonSchemaType) num4;
      model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
      model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
      model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
      model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
      model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
      JsonSchemaModel jsonSchemaModel3 = model;
      int num5;
      if (!model.ExclusiveMinimum)
      {
        bool? exclusiveMinimum = schema.ExclusiveMinimum;
        num5 = exclusiveMinimum.HasValue ? (exclusiveMinimum.GetValueOrDefault() ? 1 : 0) : 0;
      }
      else
        num5 = 1;
      jsonSchemaModel3.ExclusiveMinimum = num5 != 0;
      JsonSchemaModel jsonSchemaModel4 = model;
      int num6;
      if (!model.ExclusiveMaximum)
      {
        bool? exclusiveMaximum = schema.ExclusiveMaximum;
        num6 = exclusiveMaximum.HasValue ? (exclusiveMaximum.GetValueOrDefault() ? 1 : 0) : 0;
      }
      else
        num6 = 1;
      jsonSchemaModel4.ExclusiveMaximum = num6 != 0;
      model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
      model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
      model.AllowAdditionalProperties = model.AllowAdditionalProperties && schema.AllowAdditionalProperties;
      if (schema.Enum != null)
      {
        if (model.Enum == null)
          model.Enum = (IList<JToken>) new List<JToken>();
        CollectionUtils.AddRangeDistinct<JToken>(model.Enum, (IEnumerable<JToken>) schema.Enum, (IEqualityComparer<JToken>) new JTokenEqualityComparer());
      }
      JsonSchemaModel jsonSchemaModel5 = model;
      int num7 = (int) model.Disallow;
      JsonSchemaType? disallow = schema.Disallow;
      int num8 = disallow.HasValue ? (int) disallow.GetValueOrDefault() : 0;
      int num9 = num7 | num8;
      jsonSchemaModel5.Disallow = (JsonSchemaType) num9;
      if (schema.Pattern == null)
        return;
      if (model.Patterns == null)
        model.Patterns = (IList<string>) new List<string>();
      CollectionUtils.AddDistinct<string>(model.Patterns, schema.Pattern);
    }
  }
}
