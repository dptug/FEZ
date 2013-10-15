// Type: FezEngine.ViewpointComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine
{
  public class ViewpointComparer : IEqualityComparer<Viewpoint>
  {
    public static readonly ViewpointComparer Default = new ViewpointComparer();

    static ViewpointComparer()
    {
    }

    public bool Equals(Viewpoint x, Viewpoint y)
    {
      return x == y;
    }

    public int GetHashCode(Viewpoint obj)
    {
      return (int) obj;
    }
  }
}
