// Type: FezEngine.Tools.MovingPositionComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public class MovingPositionComparer : IComparer<Vector3>
  {
    private Vector3 ordering;

    public MovingPositionComparer(Vector3 ordering)
    {
      this.ordering = FezMath.Sign(ordering);
    }

    public int Compare(Vector3 lhs, Vector3 rhs)
    {
      int num = rhs.X.CompareTo(lhs.X) * (int) this.ordering.X;
      if (num == 0)
      {
        num = rhs.Y.CompareTo(lhs.Y) * (int) this.ordering.Y;
        if (num == 0)
          num = rhs.Z.CompareTo(lhs.Z) * (int) this.ordering.Z;
      }
      return num;
    }
  }
}
