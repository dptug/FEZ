// Type: FezEngine.Structure.NpcActionComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Structure
{
  internal class NpcActionComparer : IEqualityComparer<NpcAction>
  {
    public static readonly NpcActionComparer Default = new NpcActionComparer();

    static NpcActionComparer()
    {
    }

    public bool Equals(NpcAction x, NpcAction y)
    {
      return x == y;
    }

    public int GetHashCode(NpcAction obj)
    {
      return (int) obj;
    }
  }
}
