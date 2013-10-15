// Type: FezEngine.Structure.SurfaceTypeComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class SurfaceTypeComparer : IEqualityComparer<SurfaceType>
  {
    public static readonly SurfaceTypeComparer Default = new SurfaceTypeComparer();

    static SurfaceTypeComparer()
    {
    }

    public bool Equals(SurfaceType x, SurfaceType y)
    {
      return x == y;
    }

    public int GetHashCode(SurfaceType obj)
    {
      return (int) obj;
    }
  }
}
