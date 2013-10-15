// Type: Newtonsoft.Json.Serialization.JsonProperty
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Serialization
{
  public class JsonProperty
  {
    internal Required? _required;
    internal bool _hasExplicitDefaultValue;
    internal object _defaultValue;

    internal JsonContract PropertyContract { get; set; }

    public string PropertyName { get; set; }

    public Type DeclaringType { get; set; }

    public int? Order { get; set; }

    public string UnderlyingName { get; set; }

    public IValueProvider ValueProvider { get; set; }

    public Type PropertyType { get; set; }

    public JsonConverter Converter { get; set; }

    public JsonConverter MemberConverter { get; set; }

    public bool Ignored { get; set; }

    public bool Readable { get; set; }

    public bool Writable { get; set; }

    public bool HasMemberAttribute { get; set; }

    public object DefaultValue
    {
      get
      {
        return this._defaultValue;
      }
      set
      {
        this._hasExplicitDefaultValue = true;
        this._defaultValue = value;
      }
    }

    public Required Required
    {
      get
      {
        return this._required ?? Required.Default;
      }
      set
      {
        this._required = new Required?(value);
      }
    }

    public bool? IsReference { get; set; }

    public NullValueHandling? NullValueHandling { get; set; }

    public DefaultValueHandling? DefaultValueHandling { get; set; }

    public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

    public ObjectCreationHandling? ObjectCreationHandling { get; set; }

    public TypeNameHandling? TypeNameHandling { get; set; }

    public Predicate<object> ShouldSerialize { get; set; }

    public Predicate<object> GetIsSpecified { get; set; }

    public Action<object, object> SetIsSpecified { get; set; }

    public JsonConverter ItemConverter { get; set; }

    public bool? ItemIsReference { get; set; }

    public TypeNameHandling? ItemTypeNameHandling { get; set; }

    public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

    internal object GetResolvedDefaultValue()
    {
      if (!this._hasExplicitDefaultValue && this.PropertyType != null)
        return ReflectionUtils.GetDefaultValue(this.PropertyType);
      else
        return this._defaultValue;
    }

    public override string ToString()
    {
      return this.PropertyName;
    }
  }
}
