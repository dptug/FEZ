// Type: Newtonsoft.Json.Linq.JEnumerable`1
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
  public struct JEnumerable<T> : IJEnumerable<T>, IEnumerable<T>, IEnumerable where T : JToken
  {
    public static readonly JEnumerable<T> Empty = new JEnumerable<T>(Enumerable.Empty<T>());
    private readonly IEnumerable<T> _enumerable;

    public IJEnumerable<JToken> this[object key]
    {
      get
      {
        return (IJEnumerable<JToken>) new JEnumerable<JToken>(Extensions.Values<T, JToken>(this._enumerable, key));
      }
    }

    static JEnumerable()
    {
    }

    public JEnumerable(IEnumerable<T> enumerable)
    {
      ValidationUtils.ArgumentNotNull((object) enumerable, "enumerable");
      this._enumerable = enumerable;
    }

    public IEnumerator<T> GetEnumerator()
    {
      return this._enumerable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public override bool Equals(object obj)
    {
      if (obj is JEnumerable<T>)
        return this._enumerable.Equals((object) ((JEnumerable<T>) obj)._enumerable);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this._enumerable.GetHashCode();
    }
  }
}
