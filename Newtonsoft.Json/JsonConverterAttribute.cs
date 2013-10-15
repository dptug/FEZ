// Type: Newtonsoft.Json.JsonConverterAttribute
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
  public sealed class JsonConverterAttribute : Attribute
  {
    private readonly Type _converterType;

    public Type ConverterType
    {
      get
      {
        return this._converterType;
      }
    }

    public JsonConverterAttribute(Type converterType)
    {
      if (converterType == null)
        throw new ArgumentNullException("converterType");
      this._converterType = converterType;
    }

    internal static JsonConverter CreateJsonConverterInstance(Type converterType)
    {
      try
      {
        return (JsonConverter) Activator.CreateInstance(converterType);
      }
      catch (Exception ex)
      {
        throw new JsonException(StringUtils.FormatWith("Error creating {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) converterType), ex);
      }
    }
  }
}
