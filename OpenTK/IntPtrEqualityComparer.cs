// Type: OpenTK.IntPtrEqualityComparer
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Collections.Generic;

namespace OpenTK
{
  internal class IntPtrEqualityComparer : IEqualityComparer<IntPtr>
  {
    public bool Equals(IntPtr x, IntPtr y)
    {
      return x == y;
    }

    public int GetHashCode(IntPtr obj)
    {
      return obj.GetHashCode();
    }
  }
}
