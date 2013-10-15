// Type: FezEngine.AxisComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine
{
  public class AxisComparer : IEqualityComparer<Axis>
  {
    public static readonly AxisComparer Default = new AxisComparer();

    static AxisComparer()
    {
    }

    public bool Equals(Axis x, Axis y)
    {
      return x == y;
    }

    public int GetHashCode(Axis obj)
    {
      return (int) obj;
    }
  }
}
