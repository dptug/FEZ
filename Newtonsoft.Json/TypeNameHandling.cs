// Type: Newtonsoft.Json.TypeNameHandling
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json
{
  [Flags]
  public enum TypeNameHandling
  {
    None = 0,
    Objects = 1,
    Arrays = 2,
    All = Arrays | Objects,
    Auto = 4,
  }
}
