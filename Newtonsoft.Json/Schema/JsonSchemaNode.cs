// Type: Newtonsoft.Json.Schema.JsonSchemaNode
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Newtonsoft.Json.Schema
{
  internal class JsonSchemaNode
  {
    public string Id { get; private set; }

    public ReadOnlyCollection<JsonSchema> Schemas { get; private set; }

    public Dictionary<string, JsonSchemaNode> Properties { get; private set; }

    public Dictionary<string, JsonSchemaNode> PatternProperties { get; private set; }

    public List<JsonSchemaNode> Items { get; private set; }

    public JsonSchemaNode AdditionalProperties { get; set; }

    public JsonSchemaNode(JsonSchema schema)
    {
      this.Schemas = new ReadOnlyCollection<JsonSchema>((IList<JsonSchema>) new JsonSchema[1]
      {
        schema
      });
      this.Properties = new Dictionary<string, JsonSchemaNode>();
      this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
      this.Items = new List<JsonSchemaNode>();
      this.Id = JsonSchemaNode.GetId((IEnumerable<JsonSchema>) this.Schemas);
    }

    private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
    {
      this.Schemas = new ReadOnlyCollection<JsonSchema>((IList<JsonSchema>) Enumerable.ToList<JsonSchema>(Enumerable.Union<JsonSchema>((IEnumerable<JsonSchema>) source.Schemas, (IEnumerable<JsonSchema>) new JsonSchema[1]
      {
        schema
      })));
      this.Properties = new Dictionary<string, JsonSchemaNode>((IDictionary<string, JsonSchemaNode>) source.Properties);
      this.PatternProperties = new Dictionary<string, JsonSchemaNode>((IDictionary<string, JsonSchemaNode>) source.PatternProperties);
      this.Items = new List<JsonSchemaNode>((IEnumerable<JsonSchemaNode>) source.Items);
      this.AdditionalProperties = source.AdditionalProperties;
      this.Id = JsonSchemaNode.GetId((IEnumerable<JsonSchema>) this.Schemas);
    }

    public JsonSchemaNode Combine(JsonSchema schema)
    {
      return new JsonSchemaNode(this, schema);
    }

    public static string GetId(IEnumerable<JsonSchema> schemata)
    {
      return string.Join("-", Enumerable.ToArray<string>((IEnumerable<string>) Enumerable.OrderBy<string, string>(Enumerable.Select<JsonSchema, string>(schemata, (Func<JsonSchema, string>) (s => s.InternalId)), (Func<string, string>) (id => id), (IComparer<string>) StringComparer.Ordinal)));
    }
  }
}
