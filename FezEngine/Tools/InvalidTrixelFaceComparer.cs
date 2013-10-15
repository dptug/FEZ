// Type: FezEngine.Tools.InvalidTrixelFaceComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public class InvalidTrixelFaceComparer : IComparer<TrixelFace>
  {
    public int Compare(TrixelFace x, TrixelFace y)
    {
      if (x.Face != y.Face)
        return x.Face.CompareTo((object) y.Face);
      switch (x.Face)
      {
        case FaceOrientation.Down:
        case FaceOrientation.Top:
          int num1 = x.Id.X.CompareTo(y.Id.X);
          if (num1 != 0)
            return num1;
          else
            return x.Id.Z.CompareTo(y.Id.Z);
        case FaceOrientation.Back:
        case FaceOrientation.Front:
          int num2 = x.Id.Y.CompareTo(y.Id.Y);
          if (num2 != 0)
            return num2;
          else
            return x.Id.X.CompareTo(y.Id.X);
        default:
          int num3 = x.Id.Z.CompareTo(y.Id.Z);
          if (num3 != 0)
            return num3;
          else
            return x.Id.Y.CompareTo(y.Id.Y);
      }
    }
  }
}
