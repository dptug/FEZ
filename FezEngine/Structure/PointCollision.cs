// Type: FezEngine.Structure.PointCollision
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
  public struct PointCollision
  {
    public readonly Vector3 Point;
    public readonly NearestTriles Instances;

    public PointCollision(Vector3 point, NearestTriles instances)
    {
      this.Point = point;
      this.Instances = instances;
    }
  }
}
