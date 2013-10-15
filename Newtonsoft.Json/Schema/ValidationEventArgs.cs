// Type: Newtonsoft.Json.Schema.ValidationEventArgs
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Schema
{
  public class ValidationEventArgs : EventArgs
  {
    private readonly JsonSchemaException _ex;

    public JsonSchemaException Exception
    {
      get
      {
        return this._ex;
      }
    }

    public string Path
    {
      get
      {
        return this._ex.Path;
      }
    }

    public string Message
    {
      get
      {
        return this._ex.Message;
      }
    }

    internal ValidationEventArgs(JsonSchemaException ex)
    {
      ValidationUtils.ArgumentNotNull((object) ex, "ex");
      this._ex = ex;
    }
  }
}
