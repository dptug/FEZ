// Type: FezGame.Services.IPhysicsManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezGame.Structure;
using Microsoft.Xna.Framework;

namespace FezGame.Services
{
  public interface IPhysicsManager
  {
    void DetermineOverlaps(IComplexPhysicsEntity entity);

    void DetermineOverlaps(ISimplePhysicsEntity entity);

    void DetermineInBackground(IPhysicsEntity entity, bool allowEnterInBackground, bool viewpointChanged, bool keepInFront);

    bool Update(ISimplePhysicsEntity entity);

    bool Update(ISimplePhysicsEntity entity, bool simple, bool keepInFront);

    bool Update(IComplexPhysicsEntity entity);

    void ClampToGround(IPhysicsEntity entity, Vector3? distance, Viewpoint viewpoint);

    PhysicsManager.WallHuggingResult HugWalls(IPhysicsEntity entity, bool determineBackground, bool postRotation, bool keepInFront);
  }
}
