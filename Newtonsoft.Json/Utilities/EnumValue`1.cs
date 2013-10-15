// Type: Newtonsoft.Json.Utilities.EnumValue`1
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Utilities
{
  internal class EnumValue<T> where T : struct
  {
    private readonly string _name;
    private readonly T _value;

    public string Name
    {
      get
      {
        return this._name;
      }
    }

    public T Value
    {
      get
      {
        return this._value;
      }
    }

    public EnumValue(string name, T value)
    {
      this._name = name;
      this._value = value;
    }
  }
}
