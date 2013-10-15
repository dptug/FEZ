// Type: Newtonsoft.Json.Schema.JsonSchemaResolver
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
  public class JsonSchemaResolver
  {
    public IList<JsonSchema> LoadedSchemas { get; protected set; }

    public JsonSchemaResolver()
    {
      this.LoadedSchemas = (IList<JsonSchema>) new List<JsonSchema>();
    }

    public virtual JsonSchema GetSchema(string id)
    {
      return Enumerable.SingleOrDefault<JsonSchema>((IEnumerable<JsonSchema>) this.LoadedSchemas, (Func<JsonSchema, bool>) (s => s.Id == id));
    }
  }
}
