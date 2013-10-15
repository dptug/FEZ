// Type: Newtonsoft.Json.Bson.BsonValue
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonValue : BsonToken
  {
    private readonly object _value;
    private readonly BsonType _type;

    public object Value
    {
      get
      {
        return this._value;
      }
    }

    public override BsonType Type
    {
      get
      {
        return this._type;
      }
    }

    public BsonValue(object value, BsonType type)
    {
      this._value = value;
      this._type = type;
    }
  }
}
