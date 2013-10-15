// Type: FezEngine.Components.LayerComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Components
{
  internal class LayerComparer : IEqualityComparer<Layer>
  {
    public static readonly LayerComparer Default = new LayerComparer();

    static LayerComparer()
    {
    }

    public bool Equals(Layer x, Layer y)
    {
      return x == y;
    }

    public int GetHashCode(Layer obj)
    {
      return (int) obj;
    }
  }
}
