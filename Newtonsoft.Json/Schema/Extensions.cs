// Type: Newtonsoft.Json.Schema.Extensions
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
  public static class Extensions
  {
    public static bool IsValid(this JToken source, JsonSchema schema)
    {
      bool valid = true;
      Extensions.Validate(source, schema, (ValidationEventHandler) ((sender, args) => valid = false));
      return valid;
    }

    public static bool IsValid(this JToken source, JsonSchema schema, out IList<string> errorMessages)
    {
      IList<string> errors = (IList<string>) new List<string>();
      Extensions.Validate(source, schema, (ValidationEventHandler) ((sender, args) => errors.Add(args.Message)));
      errorMessages = errors;
      return errorMessages.Count == 0;
    }

    public static void Validate(this JToken source, JsonSchema schema)
    {
      Extensions.Validate(source, schema, (ValidationEventHandler) null);
    }

    public static void Validate(this JToken source, JsonSchema schema, ValidationEventHandler validationEventHandler)
    {
      ValidationUtils.ArgumentNotNull((object) source, "source");
      ValidationUtils.ArgumentNotNull((object) schema, "schema");
      using (JsonValidatingReader validatingReader = new JsonValidatingReader(source.CreateReader()))
      {
        validatingReader.Schema = schema;
        if (validationEventHandler != null)
          validatingReader.ValidationEventHandler += validationEventHandler;
        do
          ;
        while (validatingReader.Read());
      }
    }
  }
}
