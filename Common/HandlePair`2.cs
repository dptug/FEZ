// Type: Common.HandlePair`2
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9992B00D-7E50-4755-8BAA-4E3BBC8F3470
// Assembly location: F:\Program Files (x86)\FEZ\Common.dll

using System;

namespace Common
{
  internal struct HandlePair<T, U> : IEquatable<HandlePair<T, U>>
  {
    private readonly T first;
    private readonly U second;
    private readonly int hash;

    public HandlePair(T first, U second)
    {
      this.first = first;
      this.second = second;
      this.hash = 27232 + first.GetHashCode();
    }

    public override int GetHashCode()
    {
      return this.hash;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is HandlePair<T, U>))
        return false;
      else
        return this.Equals((HandlePair<T, U>) obj);
    }

    public bool Equals(HandlePair<T, U> other)
    {
      if (other.first.Equals((object) this.first))
        return other.second.Equals((object) this.second);
      else
        return false;
    }
  }
}
