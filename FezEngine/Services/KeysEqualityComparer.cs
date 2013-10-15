// Type: FezEngine.Services.KeysEqualityComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FezEngine.Services
{
  public class KeysEqualityComparer : IEqualityComparer<Keys>
  {
    public static readonly KeysEqualityComparer Default = new KeysEqualityComparer();

    static KeysEqualityComparer()
    {
    }

    public bool Equals(Keys x, Keys y)
    {
      return x == y;
    }

    public int GetHashCode(Keys obj)
    {
      return (int) obj;
    }
  }
}
