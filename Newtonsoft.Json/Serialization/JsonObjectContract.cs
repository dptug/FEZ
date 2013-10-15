// Type: Newtonsoft.Json.Serialization.JsonObjectContract
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  public class JsonObjectContract : JsonContainerContract
  {
    private bool? _hasRequiredOrDefaultValueProperties;

    public MemberSerialization MemberSerialization { get; set; }

    public Required? ItemRequired { get; set; }

    public JsonPropertyCollection Properties { get; private set; }

    public JsonPropertyCollection ConstructorParameters { get; private set; }

    public ConstructorInfo OverrideConstructor { get; set; }

    public ConstructorInfo ParametrizedConstructor { get; set; }

    internal bool HasRequiredOrDefaultValueProperties
    {
      get
      {
        if (!this._hasRequiredOrDefaultValueProperties.HasValue)
        {
          this._hasRequiredOrDefaultValueProperties = new bool?(false);
          if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
          {
            this._hasRequiredOrDefaultValueProperties = new bool?(true);
          }
          else
          {
            foreach (JsonProperty jsonProperty in (Collection<JsonProperty>) this.Properties)
            {
              if (jsonProperty.Required == Required.Default)
              {
                DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling;
                DefaultValueHandling? nullable = defaultValueHandling.HasValue ? new DefaultValueHandling?(defaultValueHandling.GetValueOrDefault() & DefaultValueHandling.Populate) : new DefaultValueHandling?();
                if ((nullable.GetValueOrDefault() != DefaultValueHandling.Populate ? 0 : (nullable.HasValue ? 1 : 0)) == 0 || !jsonProperty.Writable)
                  continue;
              }
              this._hasRequiredOrDefaultValueProperties = new bool?(true);
              break;
            }
          }
        }
        return this._hasRequiredOrDefaultValueProperties.Value;
      }
    }

    public JsonObjectContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Object;
      this.Properties = new JsonPropertyCollection(this.UnderlyingType);
      this.ConstructorParameters = new JsonPropertyCollection(this.UnderlyingType);
    }

    internal object GetUninitializedObject()
    {
      if (!JsonTypeReflector.FullyTrusted)
        throw new JsonException(StringUtils.FormatWith("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.NonNullableUnderlyingType));
      else
        return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
    }
  }
}
