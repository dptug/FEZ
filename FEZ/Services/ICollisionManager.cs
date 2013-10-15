// Type: FezGame.Services.ICollisionManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Services
{
  public interface ICollisionManager
  {
    float DistanceEpsilon { get; }

    float GravityFactor { get; set; }

    event Action GravityChanged;

    void CollideRectangle(Vector3 position, Vector3 impulse, Vector3 size, out MultipleHits<CollisionResult> horizontalResults, out MultipleHits<CollisionResult> verticalResults);

    void CollideRectangle(Vector3 position, Vector3 impulse, Vector3 size, QueryOptions options, out MultipleHits<CollisionResult> horizontalResults, out MultipleHits<CollisionResult> verticalResults);

    void CollideRectangle(Vector3 position, Vector3 impulse, Vector3 size, QueryOptions options, float elasticity, out MultipleHits<CollisionResult> horizontalResults, out MultipleHits<CollisionResult> verticalResults);

    void CollideRectangle(Vector3 position, Vector3 impulse, Vector3 size, QueryOptions options, float elasticity, Viewpoint viewpoint, out MultipleHits<CollisionResult> horizontalResults, out MultipleHits<CollisionResult> verticalResults);

    MultipleHits<CollisionResult> CollideEdge(Vector3 position, Vector3 impulse, Vector3 halfSize, Direction2D direction);

    MultipleHits<CollisionResult> CollideEdge(Vector3 position, Vector3 impulse, Vector3 halfSize, Direction2D direction, QueryOptions options);

    MultipleHits<CollisionResult> CollideEdge(Vector3 position, Vector3 impulse, Vector3 halfSize, Direction2D direction, QueryOptions options, float elasticity);

    MultipleHits<CollisionResult> CollideEdge(Vector3 position, Vector3 impulse, Vector3 halfSize, Direction2D direction, QueryOptions options, float elasticity, Viewpoint viewpoint);

    CollisionResult CollidePoint(Vector3 position, Vector3 impulse);

    CollisionResult CollidePoint(Vector3 position, Vector3 impulse, QueryOptions options);

    CollisionResult CollidePoint(Vector3 position, Vector3 impulse, QueryOptions options, float elasticity);

    CollisionResult CollidePoint(Vector3 position, Vector3 impulse, QueryOptions options, float elasticity, Viewpoint viewpoint);
  }
}
