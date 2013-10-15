// Type: Newtonsoft.Json.Schema.JsonSchemaModelBuilder
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities.LinqBridge;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
  internal class JsonSchemaModelBuilder
  {
    private JsonSchemaNodeCollection _nodes = new JsonSchemaNodeCollection();
    private Dictionary<JsonSchemaNode, JsonSchemaModel> _nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
    private JsonSchemaNode _node;

    public JsonSchemaModel Build(JsonSchema schema)
    {
      this._nodes = new JsonSchemaNodeCollection();
      this._node = this.AddSchema((JsonSchemaNode) null, schema);
      this._nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
      return this.BuildNodeModel(this._node);
    }

    public JsonSchemaNode AddSchema(JsonSchemaNode existingNode, JsonSchema schema)
    {
      string id;
      if (existingNode != null)
      {
        if (existingNode.Schemas.Contains(schema))
          return existingNode;
        id = JsonSchemaNode.GetId(Enumerable.Union<JsonSchema>((IEnumerable<JsonSchema>) existingNode.Schemas, (IEnumerable<JsonSchema>) new JsonSchema[1]
        {
          schema
        }));
      }
      else
        id = JsonSchemaNode.GetId((IEnumerable<JsonSchema>) new JsonSchema[1]
        {
          schema
        });
      if (this._nodes.Contains(id))
        return this._nodes[id];
      JsonSchemaNode jsonSchemaNode = existingNode != null ? existingNode.Combine(schema) : new JsonSchemaNode(schema);
      this._nodes.Add(jsonSchemaNode);
      this.AddProperties(schema.Properties, (IDictionary<string, JsonSchemaNode>) jsonSchemaNode.Properties);
      this.AddProperties(schema.PatternProperties, (IDictionary<string, JsonSchemaNode>) jsonSchemaNode.PatternProperties);
      if (schema.Items != null)
      {
        for (int index = 0; index < schema.Items.Count; ++index)
          this.AddItem(jsonSchemaNode, index, schema.Items[index]);
      }
      if (schema.AdditionalProperties != null)
        this.AddAdditionalProperties(jsonSchemaNode, schema.AdditionalProperties);
      if (schema.Extends != null)
        jsonSchemaNode = this.AddSchema(jsonSchemaNode, schema.Extends);
      return jsonSchemaNode;
    }

    public void AddProperties(IDictionary<string, JsonSchema> source, IDictionary<string, JsonSchemaNode> target)
    {
      if (source == null)
        return;
      foreach (KeyValuePair<string, JsonSchema> keyValuePair in (IEnumerable<KeyValuePair<string, JsonSchema>>) source)
        this.AddProperty(target, keyValuePair.Key, keyValuePair.Value);
    }

    public void AddProperty(IDictionary<string, JsonSchemaNode> target, string propertyName, JsonSchema schema)
    {
      JsonSchemaNode existingNode;
      target.TryGetValue(propertyName, out existingNode);
      target[propertyName] = this.AddSchema(existingNode, schema);
    }

    public void AddItem(JsonSchemaNode parentNode, int index, JsonSchema schema)
    {
      JsonSchemaNode jsonSchemaNode = this.AddSchema(parentNode.Items.Count > index ? parentNode.Items[index] : (JsonSchemaNode) null, schema);
      if (parentNode.Items.Count <= index)
        parentNode.Items.Add(jsonSchemaNode);
      else
        parentNode.Items[index] = jsonSchemaNode;
    }

    public void AddAdditionalProperties(JsonSchemaNode parentNode, JsonSchema schema)
    {
      parentNode.AdditionalProperties = this.AddSchema(parentNode.AdditionalProperties, schema);
    }

    private JsonSchemaModel BuildNodeModel(JsonSchemaNode node)
    {
      JsonSchemaModel jsonSchemaModel1;
      if (this._nodeModels.TryGetValue(node, out jsonSchemaModel1))
        return jsonSchemaModel1;
      JsonSchemaModel jsonSchemaModel2 = JsonSchemaModel.Create((IList<JsonSchema>) node.Schemas);
      this._nodeModels[node] = jsonSchemaModel2;
      foreach (KeyValuePair<string, JsonSchemaNode> keyValuePair in node.Properties)
      {
        if (jsonSchemaModel2.Properties == null)
          jsonSchemaModel2.Properties = (IDictionary<string, JsonSchemaModel>) new Dictionary<string, JsonSchemaModel>();
        jsonSchemaModel2.Properties[keyValuePair.Key] = this.BuildNodeModel(keyValuePair.Value);
      }
      foreach (KeyValuePair<string, JsonSchemaNode> keyValuePair in node.PatternProperties)
      {
        if (jsonSchemaModel2.PatternProperties == null)
          jsonSchemaModel2.PatternProperties = (IDictionary<string, JsonSchemaModel>) new Dictionary<string, JsonSchemaModel>();
        jsonSchemaModel2.PatternProperties[keyValuePair.Key] = this.BuildNodeModel(keyValuePair.Value);
      }
      foreach (JsonSchemaNode node1 in node.Items)
      {
        if (jsonSchemaModel2.Items == null)
          jsonSchemaModel2.Items = (IList<JsonSchemaModel>) new List<JsonSchemaModel>();
        jsonSchemaModel2.Items.Add(this.BuildNodeModel(node1));
      }
      if (node.AdditionalProperties != null)
        jsonSchemaModel2.AdditionalProperties = this.BuildNodeModel(node.AdditionalProperties);
      return jsonSchemaModel2;
    }
  }
}
