// Type: FezEngine.Structure.LiquidTypeComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class LiquidTypeComparer : IEqualityComparer<LiquidType>
  {
    public static readonly LiquidTypeComparer Default = new LiquidTypeComparer();

    static LiquidTypeComparer()
    {
    }

    public bool Equals(LiquidType x, LiquidType y)
    {
      return x == y;
    }

    public int GetHashCode(LiquidType obj)
    {
      return (int) obj;
    }
  }
}
