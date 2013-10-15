// Type: FezEngine.VerticalDirectionComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine
{
  public class VerticalDirectionComparer : IEqualityComparer<VerticalDirection>
  {
    public static readonly VerticalDirectionComparer Default = new VerticalDirectionComparer();

    static VerticalDirectionComparer()
    {
    }

    public bool Equals(VerticalDirection x, VerticalDirection y)
    {
      return x == y;
    }

    public int GetHashCode(VerticalDirection obj)
    {
      return (int) obj;
    }
  }
}
