// Type: FezEngine.Structure.CollisionResult
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
  public struct CollisionResult
  {
    public bool Collided;
    public bool ShouldBeClamped;
    public Vector3 Response;
    public Vector3 NearestDistance;
    public TrileInstance Destination;

    public override string ToString()
    {
      return string.Format("{{{0} @ {1}}}", (object) (bool) (this.Collided ? 1 : 0), this.Destination == null ? (object) "none" : (object) this.Destination.ToString());
    }
  }
}
