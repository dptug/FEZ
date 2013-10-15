// Type: FezGame.Structure.IComplexPhysicsEntity
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FezGame.Structure
{
  public interface IComplexPhysicsEntity : IPhysicsEntity
  {
    bool MustBeClampedToGround { get; set; }

    Vector3? GroundedVelocity { get; set; }

    HorizontalDirection MovingDirection { get; set; }

    bool Climbing { get; }

    bool Swimming { get; }

    Dictionary<VerticalDirection, NearestTriles> AxisCollision { get; }

    MultipleHits<CollisionResult> Ceiling { get; set; }

    bool HandlesZClamping { get; }
  }
}
