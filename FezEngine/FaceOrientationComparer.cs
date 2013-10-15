// Type: FezEngine.FaceOrientationComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine
{
  public class FaceOrientationComparer : IEqualityComparer<FaceOrientation>
  {
    public static readonly FaceOrientationComparer Default = new FaceOrientationComparer();

    static FaceOrientationComparer()
    {
    }

    public bool Equals(FaceOrientation x, FaceOrientation y)
    {
      return x == y;
    }

    public int GetHashCode(FaceOrientation obj)
    {
      return (int) obj;
    }
  }
}
