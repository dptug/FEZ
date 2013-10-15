// Type: Newtonsoft.Json.Bson.BsonRegex
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonRegex : BsonToken
  {
    public BsonString Pattern { get; set; }

    public BsonString Options { get; set; }

    public override BsonType Type
    {
      get
      {
        return BsonType.Regex;
      }
    }

    public BsonRegex(string pattern, string options)
    {
      this.Pattern = new BsonString((object) pattern, false);
      this.Options = new BsonString((object) options, false);
    }
  }
}
