// Type: FezEngine.HorizontalDirectionComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine
{
  public class HorizontalDirectionComparer : IEqualityComparer<HorizontalDirection>
  {
    public static readonly HorizontalDirectionComparer Default = new HorizontalDirectionComparer();

    static HorizontalDirectionComparer()
    {
    }

    public bool Equals(HorizontalDirection x, HorizontalDirection y)
    {
      return x == y;
    }

    public int GetHashCode(HorizontalDirection obj)
    {
      return (int) obj;
    }
  }
}
