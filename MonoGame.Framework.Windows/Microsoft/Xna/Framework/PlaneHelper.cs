// Type: Microsoft.Xna.Framework.PlaneHelper
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework
{
  internal class PlaneHelper
  {
    public static float ClassifyPoint(ref Vector3 point, ref Plane plane)
    {
      return (float) ((double) point.X * (double) plane.Normal.X + (double) point.Y * (double) plane.Normal.Y + (double) point.Z * (double) plane.Normal.Z) + plane.D;
    }

    public static float PerpendicularDistance(ref Vector3 point, ref Plane plane)
    {
      return (float) Math.Abs(((double) plane.Normal.X * (double) point.X + (double) plane.Normal.Y * (double) point.Y + (double) plane.Normal.Z * (double) point.Z) / Math.Sqrt((double) plane.Normal.X * (double) plane.Normal.X + (double) plane.Normal.Y * (double) plane.Normal.Y + (double) plane.Normal.Z * (double) plane.Normal.Z));
    }
  }
}
