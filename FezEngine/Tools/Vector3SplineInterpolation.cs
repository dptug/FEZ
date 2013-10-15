// Type: FezEngine.Tools.Vector3SplineInterpolation
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Tools
{
  public class Vector3SplineInterpolation : SplineInterpolation<Vector3>
  {
    public Vector3SplineInterpolation(TimeSpan duration, params Vector3[] points)
      : base(duration, points)
    {
    }

    protected override void Interpolate(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
      if (SplineInterpolation<Vector3>.LongScreenshot)
        this.Current = FezMath.Slerp(p1, p2, t);
      else
        this.Current = Vector3.CatmullRom(p0, p1, p2, p3, t);
    }
  }
}
