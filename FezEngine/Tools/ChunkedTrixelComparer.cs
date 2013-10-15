// Type: FezEngine.Tools.ChunkedTrixelComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  internal class ChunkedTrixelComparer : IComparer<TrixelEmplacement>
  {
    public int Compare(TrixelEmplacement x, TrixelEmplacement y)
    {
      int num = x.X.CompareTo(y.X);
      if (num == 0)
      {
        num = x.Y.CompareTo(y.Y);
        if (num == 0)
          num = x.Z.CompareTo(y.Z);
      }
      return num;
    }
  }
}
