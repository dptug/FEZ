// Type: FezEngine.Services.KeysComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FezEngine.Services
{
  public class KeysComparer : IComparer<Keys>
  {
    public static readonly KeysComparer Default = new KeysComparer();

    static KeysComparer()
    {
    }

    public int Compare(Keys x, Keys y)
    {
      if (x < y)
        return -1;
      return x > y ? 1 : 0;
    }
  }
}
