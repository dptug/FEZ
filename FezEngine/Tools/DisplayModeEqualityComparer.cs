// Type: FezEngine.Tools.DisplayModeEqualityComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  internal class DisplayModeEqualityComparer : IEqualityComparer<DisplayMode>
  {
    public static readonly DisplayModeEqualityComparer Default = new DisplayModeEqualityComparer();

    static DisplayModeEqualityComparer()
    {
    }

    public bool Equals(DisplayMode x, DisplayMode y)
    {
      if (x.Width == y.Width)
        return x.Height == y.Height;
      else
        return false;
    }

    public int GetHashCode(DisplayMode obj)
    {
      return Util.CombineHashCodes(obj.Width.GetHashCode(), obj.Height.GetHashCode());
    }
  }
}
