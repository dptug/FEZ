// Type: FezEngine.Tools.MovingTrileInstanceComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public class MovingTrileInstanceComparer : IComparer<TrileInstance>
  {
    private Vector3 ordering;

    public MovingTrileInstanceComparer(Vector3 ordering)
    {
      this.ordering = FezMath.Sign(ordering);
    }

    public int Compare(TrileInstance lhs, TrileInstance rhs)
    {
      Vector3 position1 = rhs.Position;
      Vector3 position2 = lhs.Position;
      int num = position1.X.CompareTo(position2.X) * (int) this.ordering.X;
      if (num == 0)
      {
        num = position1.Y.CompareTo(position2.Y) * (int) this.ordering.Y;
        if (num == 0)
          num = position1.Z.CompareTo(position2.Z) * (int) this.ordering.Z;
      }
      return num;
    }
  }
}
