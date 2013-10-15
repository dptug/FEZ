// Type: SharpDX.IdentityEqualityComparer`1
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SharpDX
{
  public class IdentityEqualityComparer<T> : IEqualityComparer<T> where T : class
  {
    public int GetHashCode(T value)
    {
      return RuntimeHelpers.GetHashCode((object) value);
    }

    public bool Equals(T left, T right)
    {
      return (object) left == (object) right;
    }
  }
}
