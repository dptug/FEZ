// Type: Newtonsoft.Json.Serialization.JsonPropertyCollection
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
  public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
  {
    private readonly Type _type;

    public JsonPropertyCollection(Type type)
      : base((IEqualityComparer<string>) StringComparer.Ordinal)
    {
      ValidationUtils.ArgumentNotNull((object) type, "type");
      this._type = type;
    }

    protected override string GetKeyForItem(JsonProperty item)
    {
      return item.PropertyName;
    }

    public void AddProperty(JsonProperty property)
    {
      if (this.Contains(property.PropertyName))
      {
        if (property.Ignored)
          return;
        JsonProperty jsonProperty = this[property.PropertyName];
        bool flag = true;
        if (jsonProperty.Ignored)
        {
          base.Remove(jsonProperty);
          flag = false;
        }
        if (property.DeclaringType != null && jsonProperty.DeclaringType != null)
        {
          if (property.DeclaringType.IsSubclassOf(jsonProperty.DeclaringType))
          {
            base.Remove(jsonProperty);
            flag = false;
          }
          if (jsonProperty.DeclaringType.IsSubclassOf(property.DeclaringType))
            return;
        }
        if (flag)
          throw new JsonSerializationException(StringUtils.FormatWith("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.", (IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName, (object) this._type));
      }
      this.Add(property);
    }

    public JsonProperty GetClosestMatchProperty(string propertyName)
    {
      return this.GetProperty(propertyName, StringComparison.Ordinal) ?? this.GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);
    }

    private bool TryGetValue(string key, out JsonProperty item)
    {
      if (this.Dictionary != null)
        return this.Dictionary.TryGetValue(key, out item);
      item = (JsonProperty) null;
      return false;
    }

    public JsonProperty GetProperty(string propertyName, StringComparison comparisonType)
    {
      if (comparisonType == StringComparison.Ordinal)
      {
        JsonProperty jsonProperty;
        if (this.TryGetValue(propertyName, out jsonProperty))
          return jsonProperty;
        else
          return (JsonProperty) null;
      }
      else
      {
        foreach (JsonProperty jsonProperty in (Collection<JsonProperty>) this)
        {
          if (string.Equals(propertyName, jsonProperty.PropertyName, comparisonType))
            return jsonProperty;
        }
        return (JsonProperty) null;
      }
    }
  }
}
