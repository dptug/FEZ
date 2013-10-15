// Type: Newtonsoft.Json.Utilities.StringReference
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Utilities
{
  internal struct StringReference
  {
    private readonly char[] _chars;
    private readonly int _startIndex;
    private readonly int _length;

    public char[] Chars
    {
      get
      {
        return this._chars;
      }
    }

    public int StartIndex
    {
      get
      {
        return this._startIndex;
      }
    }

    public int Length
    {
      get
      {
        return this._length;
      }
    }

    public StringReference(char[] chars, int startIndex, int length)
    {
      this._chars = chars;
      this._startIndex = startIndex;
      this._length = length;
    }

    public override string ToString()
    {
      return new string(this._chars, this._startIndex, this._length);
    }
  }
}
