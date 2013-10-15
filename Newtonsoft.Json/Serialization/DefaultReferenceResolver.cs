// Type: Newtonsoft.Json.Serialization.DefaultReferenceResolver
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
  internal class DefaultReferenceResolver : IReferenceResolver
  {
    private int _referenceCount;

    private BidirectionalDictionary<string, object> GetMappings(object context)
    {
      JsonSerializerInternalBase serializerInternalBase;
      if (context is JsonSerializerInternalBase)
      {
        serializerInternalBase = (JsonSerializerInternalBase) context;
      }
      else
      {
        if (!(context is JsonSerializerProxy))
          throw new JsonException("The DefaultReferenceResolver can only be used internally.");
        serializerInternalBase = ((JsonSerializerProxy) context).GetInternalSerializer();
      }
      return serializerInternalBase.DefaultReferenceMappings;
    }

    public object ResolveReference(object context, string reference)
    {
      object second;
      this.GetMappings(context).TryGetByFirst(reference, out second);
      return second;
    }

    public string GetReference(object context, object value)
    {
      BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
      string first;
      if (!mappings.TryGetBySecond(value, out first))
      {
        ++this._referenceCount;
        first = this._referenceCount.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        mappings.Set(first, value);
      }
      return first;
    }

    public void AddReference(object context, string reference, object value)
    {
      this.GetMappings(context).Set(reference, value);
    }

    public bool IsReferenced(object context, object value)
    {
      string first;
      return this.GetMappings(context).TryGetBySecond(value, out first);
    }
  }
}
