// Type: Newtonsoft.Json.Bson.BsonBinaryType
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Bson
{
  internal enum BsonBinaryType : byte
  {
    Binary = (byte) 0,
    Function = (byte) 1,
    [Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")] Data = (byte) 2,
    Uuid = (byte) 3,
    Md5 = (byte) 5,
    UserDefined = (byte) 128,
  }
}
