// Type: Newtonsoft.Json.Bson.BsonArray
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
  internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
  {
    private readonly List<BsonToken> _children = new List<BsonToken>();

    public override BsonType Type
    {
      get
      {
        return BsonType.Array;
      }
    }

    public void Add(BsonToken token)
    {
      this._children.Add(token);
      token.Parent = (BsonToken) this;
    }

    public IEnumerator<BsonToken> GetEnumerator()
    {
      return (IEnumerator<BsonToken>) this._children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
