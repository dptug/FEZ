// Type: Newtonsoft.Json.JsonObjectAttribute
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonObjectAttribute : JsonContainerAttribute
  {
    private MemberSerialization _memberSerialization;
    internal Required? _itemRequired;

    public MemberSerialization MemberSerialization
    {
      get
      {
        return this._memberSerialization;
      }
      set
      {
        this._memberSerialization = value;
      }
    }

    public Required ItemRequired
    {
      get
      {
        return this._itemRequired ?? Required.Default;
      }
      set
      {
        this._itemRequired = new Required?(value);
      }
    }

    public JsonObjectAttribute()
    {
    }

    public JsonObjectAttribute(MemberSerialization memberSerialization)
    {
      this.MemberSerialization = memberSerialization;
    }

    public JsonObjectAttribute(string id)
      : base(id)
    {
    }
  }
}
