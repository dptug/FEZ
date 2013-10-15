// Type: Newtonsoft.Json.Schema.JsonSchemaType
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Schema
{
  [Flags]
  public enum JsonSchemaType
  {
    None = 0,
    String = 1,
    Float = 2,
    Integer = 4,
    Boolean = 8,
    Object = 16,
    Array = 32,
    Null = 64,
    Any = Null | Array | Object | Boolean | Integer | Float | String,
  }
}
