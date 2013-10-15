// Type: FezEngine.Structure.Input.MouseButtonStatesComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Structure.Input
{
  public class MouseButtonStatesComparer : IEqualityComparer<MouseButtonStates>
  {
    public static readonly MouseButtonStatesComparer Default = new MouseButtonStatesComparer();

    static MouseButtonStatesComparer()
    {
    }

    public bool Equals(MouseButtonStates x, MouseButtonStates y)
    {
      return x == y;
    }

    public int GetHashCode(MouseButtonStates obj)
    {
      return (int) obj;
    }
  }
}
