// Type: Newtonsoft.Json.JsonContainerAttribute
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
  public abstract class JsonContainerAttribute : Attribute
  {
    internal bool? _isReference;
    internal bool? _itemIsReference;
    internal ReferenceLoopHandling? _itemReferenceLoopHandling;
    internal TypeNameHandling? _itemTypeNameHandling;

    public string Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public Type ItemConverterType { get; set; }

    public bool IsReference
    {
      get
      {
        return this._isReference ?? false;
      }
      set
      {
        this._isReference = new bool?(value);
      }
    }

    public bool ItemIsReference
    {
      get
      {
        return this._itemIsReference ?? false;
      }
      set
      {
        this._itemIsReference = new bool?(value);
      }
    }

    public ReferenceLoopHandling ItemReferenceLoopHandling
    {
      get
      {
        return this._itemReferenceLoopHandling ?? ReferenceLoopHandling.Error;
      }
      set
      {
        this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
      }
    }

    public TypeNameHandling ItemTypeNameHandling
    {
      get
      {
        return this._itemTypeNameHandling ?? TypeNameHandling.None;
      }
      set
      {
        this._itemTypeNameHandling = new TypeNameHandling?(value);
      }
    }

    protected JsonContainerAttribute()
    {
    }

    protected JsonContainerAttribute(string id)
    {
      this.Id = id;
    }
  }
}
