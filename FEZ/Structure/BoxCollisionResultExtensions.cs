// Type: FezGame.Structure.BoxCollisionResultExtensions
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;

namespace FezGame.Structure
{
  public static class BoxCollisionResultExtensions
  {
    public static CollisionResult First(this MultipleHits<CollisionResult> result)
    {
      if (result.NearLow.Collided)
        return result.NearLow;
      if (!result.FarHigh.Collided)
        return new CollisionResult();
      else
        return result.FarHigh;
    }

    public static bool AnyCollided(this MultipleHits<CollisionResult> result)
    {
      if (!result.NearLow.Collided)
        return result.FarHigh.Collided;
      else
        return true;
    }

    public static bool AnyHit(this MultipleHits<CollisionResult> result)
    {
      if (result.NearLow.Destination == null)
        return result.FarHigh.Destination != null;
      else
        return true;
    }
  }
}
