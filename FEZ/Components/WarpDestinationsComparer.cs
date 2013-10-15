// Type: FezGame.Components.WarpDestinationsComparer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using System.Collections.Generic;

namespace FezGame.Components
{
  internal class WarpDestinationsComparer : IEqualityComparer<WarpDestinations>
  {
    public static readonly WarpDestinationsComparer Default = new WarpDestinationsComparer();

    static WarpDestinationsComparer()
    {
    }

    public bool Equals(WarpDestinations x, WarpDestinations y)
    {
      return x == y;
    }

    public int GetHashCode(WarpDestinations obj)
    {
      return (int) obj;
    }
  }
}
