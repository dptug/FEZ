// Type: Microsoft.Xna.Framework.PlaneHelper
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
