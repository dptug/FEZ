// Type: FezGame.Components.MenuCubeFaceComparer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using System.Collections.Generic;

namespace FezGame.Components
{
  internal class MenuCubeFaceComparer : IEqualityComparer<MenuCubeFace>
  {
    public static readonly MenuCubeFaceComparer Default = new MenuCubeFaceComparer();

    static MenuCubeFaceComparer()
    {
    }

    public bool Equals(MenuCubeFace x, MenuCubeFace y)
    {
      return x == y;
    }

    public int GetHashCode(MenuCubeFace obj)
    {
      return (int) obj;
    }
  }
}
