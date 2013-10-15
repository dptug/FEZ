// Type: Newtonsoft.Json.JsonArrayAttribute
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonArrayAttribute : JsonContainerAttribute
  {
    private bool _allowNullItems;

    public bool AllowNullItems
    {
      get
      {
        return this._allowNullItems;
      }
      set
      {
        this._allowNullItems = value;
      }
    }

    public JsonArrayAttribute()
    {
    }

    public JsonArrayAttribute(bool allowNullItems)
    {
      this._allowNullItems = allowNullItems;
    }

    public JsonArrayAttribute(string id)
      : base(id)
    {
    }
  }
}
