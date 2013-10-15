// Type: Newtonsoft.Json.Serialization.JsonContainerContract
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Serialization
{
  public class JsonContainerContract : JsonContract
  {
    private JsonContract _itemContract;
    private JsonContract _finalItemContract;

    internal JsonContract ItemContract
    {
      get
      {
        return this._itemContract;
      }
      set
      {
        this._itemContract = value;
        if (this._itemContract != null)
          this._finalItemContract = TypeExtensions.IsSealed(this._itemContract.UnderlyingType) ? this._itemContract : (JsonContract) null;
        else
          this._finalItemContract = (JsonContract) null;
      }
    }

    internal JsonContract FinalItemContract
    {
      get
      {
        return this._finalItemContract;
      }
    }

    public JsonConverter ItemConverter { get; set; }

    public bool? ItemIsReference { get; set; }

    public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

    public TypeNameHandling? ItemTypeNameHandling { get; set; }

    internal JsonContainerContract(Type underlyingType)
      : base(underlyingType)
    {
      JsonContainerAttribute containerAttribute = JsonTypeReflector.GetJsonContainerAttribute(underlyingType);
      if (containerAttribute == null)
        return;
      if (containerAttribute.ItemConverterType != null)
        this.ItemConverter = JsonConverterAttribute.CreateJsonConverterInstance(containerAttribute.ItemConverterType);
      this.ItemIsReference = containerAttribute._itemIsReference;
      this.ItemReferenceLoopHandling = containerAttribute._itemReferenceLoopHandling;
      this.ItemTypeNameHandling = containerAttribute._itemTypeNameHandling;
    }
  }
}
