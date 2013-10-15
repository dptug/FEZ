// Type: Newtonsoft.Json.Utilities.LinqBridge.Tuple`2
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities.LinqBridge
{
  [Serializable]
  internal struct Tuple<TFirst, TSecond> : IEquatable<Tuple<TFirst, TSecond>>
  {
    public TFirst First { get; private set; }

    public TSecond Second { get; private set; }

    public Tuple(TFirst first, TSecond second)
    {
      this = new Tuple<TFirst, TSecond>();
      this.First = first;
      this.Second = second;
    }

    public override bool Equals(object obj)
    {
      if (obj != null && obj is Tuple<TFirst, TSecond>)
        return base.Equals((object) (Tuple<TFirst, TSecond>) obj);
      else
        return false;
    }

    public bool Equals(Tuple<TFirst, TSecond> other)
    {
      if (EqualityComparer<TFirst>.Default.Equals(other.First, this.First))
        return EqualityComparer<TSecond>.Default.Equals(other.Second, this.Second);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return -1521134295 * (-1521134295 * 2049903426 + EqualityComparer<TFirst>.Default.GetHashCode(this.First)) + EqualityComparer<TSecond>.Default.GetHashCode(this.Second);
    }

    public override string ToString()
    {
      return string.Format("{{ First = {0}, Second = {1} }}", (object) this.First, (object) this.Second);
    }
  }
}
