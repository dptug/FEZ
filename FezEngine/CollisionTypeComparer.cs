// Type: FezEngine.CollisionTypeComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine
{
  public class CollisionTypeComparer : IEqualityComparer<CollisionType>
  {
    public static readonly CollisionTypeComparer Default = new CollisionTypeComparer();

    static CollisionTypeComparer()
    {
    }

    public bool Equals(CollisionType x, CollisionType y)
    {
      return x == y;
    }

    public int GetHashCode(CollisionType obj)
    {
      return (int) obj;
    }
  }
}
