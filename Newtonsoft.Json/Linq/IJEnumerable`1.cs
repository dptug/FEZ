// Type: Newtonsoft.Json.Linq.IJEnumerable`1
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
  public interface IJEnumerable<T> : IEnumerable<T>, IEnumerable where T : JToken
  {
    IJEnumerable<JToken> this[object key] { get; }
  }
}
