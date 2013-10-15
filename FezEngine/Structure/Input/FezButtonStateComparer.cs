// Type: FezEngine.Structure.Input.FezButtonStateComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Structure.Input
{
  public class FezButtonStateComparer : IEqualityComparer<FezButtonState>
  {
    public static readonly FezButtonStateComparer Default = new FezButtonStateComparer();

    static FezButtonStateComparer()
    {
    }

    public bool Equals(FezButtonState x, FezButtonState y)
    {
      return x == y;
    }

    public int GetHashCode(FezButtonState obj)
    {
      return (int) obj;
    }
  }
}
