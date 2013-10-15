// Type: FezEngine.Structure.IdentifierPool
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class IdentifierPool
  {
    private readonly List<int> available = new List<int>();
    private int maximum;

    public int Take()
    {
      if (this.available.Count == 0)
        this.available.Add(this.maximum++);
      return Enumerable.First<int>((IEnumerable<int>) this.available);
    }

    public void Return(int id)
    {
      this.available.Add(id);
    }

    public static int FirstAvailable<T>(IDictionary<int, T> values)
    {
      int val1 = -1;
      foreach (int val2 in (IEnumerable<int>) values.Keys)
        val1 = Math.Max(val1, val2);
      return val1 + 1;
    }
  }
}
