// Type: FezEngine.Structure.PlayerIndexComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class PlayerIndexComparer : IEqualityComparer<PlayerIndex>
  {
    public static readonly PlayerIndexComparer Default = new PlayerIndexComparer();

    static PlayerIndexComparer()
    {
    }

    public bool Equals(PlayerIndex x, PlayerIndex y)
    {
      return x == y;
    }

    public int GetHashCode(PlayerIndex obj)
    {
      return (int) obj;
    }
  }
}
