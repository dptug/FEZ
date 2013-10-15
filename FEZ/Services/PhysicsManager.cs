// Type: FezGame.Services.PhysicsManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezGame.Services
{
  public class PhysicsManager : IPhysicsManager
  {
    private static readonly Vector3 GroundFrictionV = new Vector3(0.85f, 1f, 0.85f);
    private static readonly Vector3 AirFrictionV = new Vector3(0.9975f, 1f, 0.9975f);
    private static readonly Vector3 WaterFrictionV = new Vector3(0.925f, 1f, 0.925f);
    private static readonly Vector3 SlidingFrictionV = new Vector3(0.8f, 1f, 0.8f);
    public const float TrileSize = 0.15f;
    private const float GroundFriction = 0.85f;
    private const float WaterFriction = 0.925f;
    private const float SlidingFriction = 0.8f;
    private const float AirFriction = 0.9975f;
    private const float FallingSpeedLimit = 0.4f;
    private const float HuggingDistance = 0.002f;

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    static PhysicsManager()
    {
    }

    public void DetermineOverlaps(IComplexPhysicsEntity entity)
    {
      this.DetermineOverlapsInternal((IPhysicsEntity) entity, this.CameraManager.Viewpoint);
      Vector3 center = entity.Center;
      Vector3 vector3 = entity.Size / 2f - new Vector3(1.0 / 1000.0);
      if ((double) this.CollisionManager.GravityFactor < 0.0)
        vector3 *= new Vector3(1f, -1f, 1f);
      QueryOptions options = entity.Background ? QueryOptions.Background : QueryOptions.None;
      entity.AxisCollision[VerticalDirection.Up] = this.LevelManager.NearestTrile(center + vector3 * Vector3.Up, options);
      entity.AxisCollision[VerticalDirection.Down] = this.LevelManager.NearestTrile(center + vector3 * Vector3.Down, options);
    }

    public void DetermineOverlaps(ISimplePhysicsEntity entity)
    {
      this.DetermineOverlapsInternal((IPhysicsEntity) entity, this.CameraManager.Viewpoint);
    }

    private void DetermineOverlapsInternal(IPhysicsEntity entity, Viewpoint viewpoint)
    {
      QueryOptions options = QueryOptions.None;
      if (entity.Background)
        options |= QueryOptions.Background;
      Vector3 center = entity.Center;
      if (entity.CornerCollision.Length == 1)
      {
        entity.CornerCollision[0] = new PointCollision(center, this.LevelManager.NearestTrile(center, options, new Viewpoint?(viewpoint)));
      }
      else
      {
        Vector3 vector3_1 = FezMath.RightVector(viewpoint);
        Vector3 vector3_2 = entity.Size / 2f - new Vector3(1.0 / 1000.0);
        if ((double) this.CollisionManager.GravityFactor < 0.0)
          vector3_2 *= new Vector3(1f, -1f, 1f);
        Vector3 vector3_3 = center + (vector3_1 + Vector3.Up) * vector3_2;
        entity.CornerCollision[0] = new PointCollision(vector3_3, this.LevelManager.NearestTrile(vector3_3, options, new Viewpoint?(viewpoint)));
        if (entity.CornerCollision[0].Instances.Deep == null && this.PlayerManager.CarriedInstance != null)
        {
          this.PlayerManager.CarriedInstance.PhysicsState.UpdatingPhysics = true;
          entity.CornerCollision[0] = new PointCollision(center, this.LevelManager.NearestTrile(center, options, new Viewpoint?(viewpoint)));
          this.PlayerManager.CarriedInstance.PhysicsState.UpdatingPhysics = false;
        }
        Vector3 vector3_4 = center + (vector3_1 + Vector3.Down) * vector3_2;
        entity.CornerCollision[1] = new PointCollision(vector3_4, this.LevelManager.NearestTrile(vector3_4, options, new Viewpoint?(viewpoint)));
        Vector3 vector3_5 = center + (-vector3_1 + Vector3.Up) * vector3_2;
        entity.CornerCollision[2] = new PointCollision(vector3_5, this.LevelManager.NearestTrile(vector3_5, options, new Viewpoint?(viewpoint)));
        Vector3 vector3_6 = center + (-vector3_1 + Vector3.Down) * vector3_2;
        entity.CornerCollision[3] = new PointCollision(vector3_6, this.LevelManager.NearestTrile(vector3_6, options, new Viewpoint?(viewpoint)));
      }
    }

    public void DetermineInBackground(IPhysicsEntity entity, bool allowEnterInBackground, bool postRotation, bool keepInFront)
    {
      if (allowEnterInBackground)
      {
        if (entity is IComplexPhysicsEntity)
        {
          IComplexPhysicsEntity complexPhysicsEntity = entity as IComplexPhysicsEntity;
          Vector3 impulse = 1.0 / 32.0 * Vector3.Down;
          QueryOptions options = QueryOptions.None;
          if (complexPhysicsEntity.Background)
            options |= QueryOptions.Background;
          bool flag = BoxCollisionResultExtensions.AnyHit(this.CollisionManager.CollideEdge(entity.Center, impulse, entity.Size / 2f, Direction2D.Vertical, options));
          if (!flag)
            flag = flag | BoxCollisionResultExtensions.AnyHit(this.CollisionManager.CollideEdge(entity.Center, impulse, entity.Size / 2f, Direction2D.Vertical, options, 0.0f, FezMath.GetOpposite(this.CameraManager.Viewpoint)));
          if (complexPhysicsEntity.Grounded && !flag)
          {
            this.DebuggingBag.Add("zz. had to re-clamp to ground", (object) "POSITIF");
            MultipleHits<CollisionResult> result = this.CollisionManager.CollideEdge(entity.Center, impulse, entity.Size / 2f, Direction2D.Vertical, options, 0.0f, this.CameraManager.LastViewpoint);
            if (BoxCollisionResultExtensions.AnyCollided(result))
              this.ClampToGround(entity, new Vector3?(BoxCollisionResultExtensions.First(result).NearestDistance), this.CameraManager.LastViewpoint);
          }
        }
        entity.Background = false;
        PhysicsManager.WallHuggingResult wallHuggingResult;
        do
        {
          this.DetermineOverlapsInternal(entity, this.CameraManager.Viewpoint);
          wallHuggingResult = this.HugWalls(entity, true, postRotation, keepInFront);
        }
        while (wallHuggingResult.Hugged);
        entity.Background = wallHuggingResult.Behind;
        this.DetermineOverlapsInternal(entity, this.CameraManager.Viewpoint);
        this.HugWalls(entity, false, false, keepInFront);
      }
      else
      {
        if (!entity.Background)
          return;
        bool flag = true;
        foreach (PointCollision pointCollision in entity.CornerCollision)
          flag = flag & !this.IsHuggable(pointCollision.Instances.Deep, entity);
        if (!flag)
          return;
        entity.Background = false;
      }
    }

    public PhysicsManager.WallHuggingResult HugWalls(IPhysicsEntity entity, bool determineBackground, bool postRotation, bool keepInFront)
    {
      Vector3 vector3_1 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      if (!entity.Background)
        vector3_1 = -vector3_1;
      float num1 = 1.0 / 500.0;
      if (entity is ISimplePhysicsEntity)
        num1 = 1.0 / 16.0;
      PhysicsManager.WallHuggingResult wallHuggingResult = new PhysicsManager.WallHuggingResult();
      Vector3 vector3_2 = new Vector3();
      if (entity.Background && entity.Grounded)
        return wallHuggingResult;
      foreach (PointCollision pointCollision in entity.CornerCollision)
      {
        TrileInstance trileInstance1 = (TrileInstance) null;
        if (this.IsHuggable(pointCollision.Instances.Surface, entity))
        {
          FaceOrientation face = FaceOrientation.Down;
          TrileEmplacement traversal = pointCollision.Instances.Surface.Emplacement.GetTraversal(ref face);
          TrileInstance trileInstance2 = this.LevelManager.TrileInstanceAt(ref traversal);
          if (trileInstance2 != null && trileInstance2.Enabled && trileInstance2.GetRotatedFace(this.CameraManager.VisibleOrientation) != CollisionType.None)
            trileInstance1 = pointCollision.Instances.Surface;
        }
        if (trileInstance1 == null && this.IsHuggable(pointCollision.Instances.Deep, entity))
          trileInstance1 = pointCollision.Instances.Deep;
        if (trileInstance1 != null && (!(entity is ISimplePhysicsEntity) || trileInstance1.PhysicsState == null || !trileInstance1.PhysicsState.Puppet) && trileInstance1.PhysicsState != entity)
        {
          Vector3 vector3_3 = trileInstance1.Center + vector3_1 * trileInstance1.TransformedSize / 2f;
          Vector3 vector1 = entity.Center - vector3_1 * entity.Size / 2f - vector3_3 + num1 * -vector3_1;
          float x = Vector3.Dot(vector1, vector3_1);
          if ((double) FezMath.AlmostClamp(x) < 0.0)
          {
            if (determineBackground && (!trileInstance1.Trile.Thin || trileInstance1.Trile.ForceHugging))
            {
              float num2 = Math.Abs(FezMath.Dot(trileInstance1.TransformedSize / 2f + entity.Size / 2f, vector3_1));
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              PhysicsManager.WallHuggingResult& local = @wallHuggingResult;
              // ISSUE: explicit reference operation
              int num3 = (^local).Behind | (double) Math.Abs(x) > (double) num2 ? 1 : 0;
              // ISSUE: explicit reference operation
              (^local).Behind = num3 != 0;
            }
            else if (keepInFront)
            {
              Vector3 vector3_4 = vector1 * FezMath.Abs(vector3_1);
              vector3_2 -= vector3_4;
              entity.Center -= vector3_4;
              wallHuggingResult.Hugged = true;
            }
          }
        }
        if (postRotation)
        {
          Vector3 vector3_3 = FezMath.AsVector(FezMath.VisibleOrientation(this.CameraManager.LastViewpoint));
          TrileInstance instance = this.LevelManager.ActualInstanceAt(pointCollision.Point + vector3_2);
          if (this.IsHuggable(instance, entity))
          {
            Vector3 vector3_4 = instance.Center + vector3_3 * FezMath.ZYX(instance.TransformedSize) / 2f;
            Vector3 vector1 = entity.Center - vector3_3 * entity.Size / 2f - vector3_4 + 1.0 / 500.0 * vector3_3;
            float x = Vector3.Dot(vector1, vector3_3);
            if ((double) FezMath.AlmostClamp(x) < 0.0 && (double) x > -1.0 && keepInFront)
            {
              Vector3 vector3_5 = vector1 * FezMath.Abs(vector3_3);
              vector3_2 -= vector3_5;
              entity.Center -= vector3_5;
              wallHuggingResult.Hugged = true;
            }
          }
        }
      }
      return wallHuggingResult;
    }

    private static bool IsInstanceStateful(TrileInstance instance)
    {
      if (instance != null)
        return instance.PhysicsState != null;
      else
        return false;
    }

    private bool IsHuggable(TrileInstance instance, IPhysicsEntity entity)
    {
      if (instance != null && instance.Enabled && !instance.Trile.Immaterial && ((!instance.Trile.Thin || instance.Trile.ForceHugging) && (instance != this.PlayerManager.CarriedInstance && instance != this.PlayerManager.PushedInstance)) && (!ActorTypeExtensions.IsBomb(instance.Trile.ActorSettings.Type) && (instance.PhysicsState == null || instance.PhysicsState != entity)))
        return !FezMath.In<CollisionType>(instance.GetRotatedFace(entity.Background ? FezMath.GetOpposite(this.CameraManager.VisibleOrientation) : this.CameraManager.VisibleOrientation), CollisionType.Immaterial, CollisionType.TopNoStraightLedge, CollisionType.AllSides, (IEqualityComparer<CollisionType>) CollisionTypeComparer.Default);
      else
        return false;
    }

    public bool Update(IComplexPhysicsEntity entity)
    {
      QueryOptions queryOptions = QueryOptions.None;
      if (entity.Background)
        queryOptions |= QueryOptions.Background;
      this.MoveAlongWithGround((IPhysicsEntity) entity, queryOptions);
      MultipleHits<CollisionResult> horizontalResults;
      MultipleHits<CollisionResult> verticalResults;
      this.CollisionManager.CollideRectangle(entity.Center, entity.Velocity, entity.Size, queryOptions, entity.Elasticity, out horizontalResults, out verticalResults);
      bool grounded = entity.Grounded;
      MultipleHits<TrileInstance> ground1 = entity.Ground;
      Vector3? clampToGroundDistance = new Vector3?();
      FaceOrientation visibleOrientation = this.CameraManager.VisibleOrientation;
      bool flag1 = (double) this.CollisionManager.GravityFactor < 0.0;
      if (BoxCollisionResultExtensions.AnyCollided(verticalResults) && (flag1 ? ((double) entity.Velocity.Y > 0.0 ? 1 : 0) : ((double) entity.Velocity.Y < 0.0 ? 1 : 0)) != 0)
      {
        MultipleHits<TrileInstance> ground2 = entity.Ground;
        CollisionResult collisionResult1 = verticalResults.NearLow;
        CollisionResult collisionResult2 = verticalResults.FarHigh;
        if (collisionResult2.Destination != null && collisionResult2.Destination.GetRotatedFace(visibleOrientation) != CollisionType.None)
        {
          ground2.FarHigh = collisionResult2.Destination;
          if (collisionResult2.Collided && (collisionResult2.ShouldBeClamped || entity.MustBeClampedToGround))
            clampToGroundDistance = new Vector3?(collisionResult2.NearestDistance);
        }
        else
          ground2.FarHigh = (TrileInstance) null;
        if (collisionResult1.Destination != null && collisionResult1.Destination.GetRotatedFace(visibleOrientation) != CollisionType.None)
        {
          ground2.NearLow = collisionResult1.Destination;
          if (collisionResult1.Collided && (collisionResult1.ShouldBeClamped || entity.MustBeClampedToGround))
            clampToGroundDistance = new Vector3?(collisionResult1.NearestDistance);
        }
        else
          ground2.NearLow = (TrileInstance) null;
        entity.Ground = ground2;
      }
      else
        entity.Ground = new MultipleHits<TrileInstance>();
      entity.Ceiling = (double) entity.Velocity.Y <= 0.0 || !BoxCollisionResultExtensions.AnyCollided(verticalResults) ? new MultipleHits<CollisionResult>() : verticalResults;
      bool flag2 = (this.PlayerManager.Action == ActionType.Grabbing || this.PlayerManager.Action == ActionType.Pushing || (this.PlayerManager.Action == ActionType.GrabCornerLedge || this.PlayerManager.Action == ActionType.LowerToCornerLedge) || this.PlayerManager.Action == ActionType.SuckedIn || this.PlayerManager.Action == ActionType.Landing) | entity.MustBeClampedToGround;
      entity.MustBeClampedToGround = false;
      bool velocityIrrelevant = ((flag2 ? 1 : 0) | (!entity.Grounded ? 0 : (entity.Ground.First.ForceClampToGround ? 1 : 0))) != 0;
      if (grounded && !entity.Grounded)
        entity.GroundedVelocity = new Vector3?(entity.Velocity);
      else if (!grounded && entity.Grounded)
        entity.GroundedVelocity = new Vector3?();
      Vector3 vector2 = FezMath.RightVector(this.CameraManager.Viewpoint);
      entity.MovingDirection = FezMath.DirectionFromMovement(Vector3.Dot(entity.Velocity, vector2));
      bool flag3 = this.PlayerManager.Action == ActionType.FrontClimbingLadder || this.PlayerManager.Action == ActionType.FrontClimbingVine;
      if (entity.GroundMovement != Vector3.Zero || flag3)
        this.DetermineInBackground((IPhysicsEntity) entity, true, false, !this.PlayerManager.Climbing);
      return this.UpdateInternal((IPhysicsEntity) entity, horizontalResults, verticalResults, clampToGroundDistance, grounded, !entity.HandlesZClamping, velocityIrrelevant, false);
    }

    public bool Update(ISimplePhysicsEntity entity)
    {
      return this.Update(entity, false, true);
    }

    public bool Update(ISimplePhysicsEntity entity, bool simple, bool keepInFront)
    {
      QueryOptions queryOptions = QueryOptions.None;
      if (entity.Background)
        queryOptions |= QueryOptions.Background;
      if (simple)
        queryOptions |= QueryOptions.Simple;
      if (entity is InstancePhysicsState)
        (entity as InstancePhysicsState).UpdatingPhysics = true;
      if (!simple)
        this.MoveAlongWithGround((IPhysicsEntity) entity, queryOptions);
      Vector3? clampToGroundDistance = new Vector3?();
      bool grounded = entity.Grounded;
      MultipleHits<CollisionResult> horizontalResults;
      MultipleHits<CollisionResult> verticalResults;
      if (!entity.IgnoreCollision)
      {
        this.CollisionManager.CollideRectangle(entity.Center, entity.Velocity, entity.Size, queryOptions, entity.Elasticity, out horizontalResults, out verticalResults);
        bool flag = (double) this.CollisionManager.GravityFactor < 0.0;
        FaceOrientation faceOrientation = this.CameraManager.VisibleOrientation;
        if (entity.Background)
          faceOrientation = FezMath.GetOpposite(faceOrientation);
        if ((flag ? ((double) entity.Velocity.Y > 0.0 ? 1 : 0) : ((double) entity.Velocity.Y < 0.0 ? 1 : 0)) != 0 && BoxCollisionResultExtensions.AnyCollided(verticalResults))
        {
          MultipleHits<TrileInstance> ground = entity.Ground;
          CollisionResult collisionResult1 = verticalResults.NearLow;
          CollisionResult collisionResult2 = verticalResults.FarHigh;
          if (collisionResult2.Destination != null && collisionResult2.Destination.GetRotatedFace(faceOrientation) != CollisionType.None)
          {
            ground.FarHigh = collisionResult2.Destination;
            if (collisionResult2.Collided && collisionResult2.ShouldBeClamped)
              clampToGroundDistance = new Vector3?(collisionResult2.NearestDistance);
          }
          else
            ground.FarHigh = (TrileInstance) null;
          if (collisionResult1.Destination != null && collisionResult1.Destination.GetRotatedFace(faceOrientation) != CollisionType.None)
          {
            ground.NearLow = collisionResult1.Destination;
            if (collisionResult1.Collided && collisionResult1.ShouldBeClamped)
              clampToGroundDistance = new Vector3?(collisionResult1.NearestDistance);
          }
          else
            ground.NearLow = (TrileInstance) null;
          entity.Ground = ground;
        }
        else
          entity.Ground = new MultipleHits<TrileInstance>();
      }
      else
      {
        horizontalResults = new MultipleHits<CollisionResult>();
        verticalResults = new MultipleHits<CollisionResult>();
      }
      bool flag1 = this.UpdateInternal((IPhysicsEntity) entity, horizontalResults, verticalResults, clampToGroundDistance, grounded, keepInFront, false, simple);
      if (entity is InstancePhysicsState)
        (entity as InstancePhysicsState).UpdatingPhysics = false;
      return flag1;
    }

    private bool UpdateInternal(IPhysicsEntity entity, MultipleHits<CollisionResult> horizontalResults, MultipleHits<CollisionResult> verticalResults, Vector3? clampToGroundDistance, bool wasGrounded, bool hugWalls, bool velocityIrrelevant, bool simple)
    {
      Vector3 velocity = entity.Velocity;
      if (!simple)
      {
        MultipleHits<CollisionResult> multipleHits = new MultipleHits<CollisionResult>();
        if (BoxCollisionResultExtensions.AnyCollided(horizontalResults))
        {
          if (horizontalResults.NearLow.Collided)
            multipleHits.NearLow = horizontalResults.NearLow;
          if (horizontalResults.FarHigh.Collided)
            multipleHits.FarHigh = horizontalResults.FarHigh;
        }
        entity.WallCollision = multipleHits;
      }
      if (horizontalResults.NearLow.Collided)
        velocity += horizontalResults.NearLow.Response;
      else if (horizontalResults.FarHigh.Collided)
        velocity += horizontalResults.FarHigh.Response;
      if (verticalResults.NearLow.Collided)
        velocity += verticalResults.NearLow.Response;
      else if (verticalResults.FarHigh.Collided)
        velocity += verticalResults.FarHigh.Response;
      Vector3 vector3_1 = !(entity is IComplexPhysicsEntity) || !(entity as IComplexPhysicsEntity).Swimming ? (entity.Grounded || wasGrounded ? (entity.Sliding ? PhysicsManager.SlidingFrictionV : PhysicsManager.GroundFrictionV) : PhysicsManager.AirFrictionV) : PhysicsManager.WaterFrictionV;
      float amount = (float) ((1.20000004768372 + (double) Math.Abs(this.CollisionManager.GravityFactor) * 0.800000011920929) / 2.0);
      Vector3 vector3_2 = FezMath.AlmostClamp(velocity * Vector3.Lerp(Vector3.One, vector3_1, amount), 1E-06f);
      if (!entity.NoVelocityClamping)
      {
        float max = entity is IComplexPhysicsEntity ? 0.4f : 0.38f;
        vector3_2.Y = MathHelper.Clamp(vector3_2.Y, -max, max);
      }
      Vector3 center = entity.Center;
      Vector3 a = center + vector3_2;
      bool flag = !FezMath.AlmostEqual(a, center);
      entity.Velocity = vector3_2;
      if (flag)
        entity.Center = a;
      if (velocityIrrelevant || flag)
      {
        this.DetermineInBackground(entity, false, false, hugWalls);
        this.ClampToGround(entity, clampToGroundDistance, this.CameraManager.Viewpoint);
        if (hugWalls)
          this.HugWalls(entity, false, false, true);
      }
      if (!simple && (!(entity is ISimplePhysicsEntity) || !(entity as ISimplePhysicsEntity).IgnoreCollision))
      {
        if (this.LevelManager.IsInvalidatingScreen)
          this.ScheduleRedefineCorners(entity);
        else
          this.RedefineCorners(entity);
      }
      return flag;
    }

    private void ScheduleRedefineCorners(IPhysicsEntity entity)
    {
      this.LevelManager.ScreenInvalidated += (Action) (() => this.RedefineCorners(entity));
    }

    private void RedefineCorners(IPhysicsEntity entity)
    {
      if (entity is IComplexPhysicsEntity)
        this.DetermineOverlaps(entity as IComplexPhysicsEntity);
      else
        this.DetermineOverlaps(entity as ISimplePhysicsEntity);
    }

    private void MoveAlongWithGround(IPhysicsEntity entity, QueryOptions queryOptions)
    {
      TrileInstance trileInstance = (TrileInstance) null;
      bool flag = false;
      if (PhysicsManager.IsInstanceStateful(entity.Ground.NearLow))
        trileInstance = entity.Ground.NearLow;
      else if (PhysicsManager.IsInstanceStateful(entity.Ground.FarHigh))
        trileInstance = entity.Ground.FarHigh;
      Vector3 vector3 = entity.GroundMovement;
      if (trileInstance != null)
      {
        entity.GroundMovement = FezMath.AlmostClamp(trileInstance.PhysicsState.Velocity + trileInstance.PhysicsState.GroundMovement);
        if (trileInstance.PhysicsState.Sticky)
          flag = true;
      }
      else
      {
        vector3 = Vector3.Clamp(vector3, -FezMath.XZMask, Vector3.One);
        entity.GroundMovement = Vector3.Zero;
      }
      if (entity.GroundMovement != Vector3.Zero)
      {
        MultipleHits<CollisionResult> horizontalResults;
        MultipleHits<CollisionResult> verticalResults;
        this.CollisionManager.CollideRectangle(entity.Center, entity.GroundMovement, entity.Size, queryOptions, entity.Elasticity, out horizontalResults, out verticalResults);
        entity.GroundMovement += (BoxCollisionResultExtensions.AnyCollided(horizontalResults) ? BoxCollisionResultExtensions.First(horizontalResults).Response : Vector3.Zero) + (BoxCollisionResultExtensions.AnyCollided(verticalResults) ? BoxCollisionResultExtensions.First(verticalResults).Response : Vector3.Zero);
        Vector3 min = flag ? -Vector3.One : -FezMath.XZMask;
        entity.Center += Vector3.Clamp(entity.GroundMovement, min, Vector3.One);
        if (!(vector3 == Vector3.Zero) || (double) entity.Velocity.Y <= 0.0)
          return;
        entity.Velocity -= entity.GroundMovement * Vector3.UnitY;
      }
      else
      {
        if (flag || !(vector3 != Vector3.Zero))
          return;
        entity.Velocity += vector3 * 0.85f;
      }
    }

    public void ClampToGround(IPhysicsEntity entity, Vector3? distance, Viewpoint viewpoint)
    {
      if (!distance.HasValue)
        return;
      Vector3 mask = FezMath.GetMask(FezMath.VisibleAxis(viewpoint));
      entity.Center = distance.Value * mask + (Vector3.One - mask) * entity.Center;
    }

    public struct WallHuggingResult
    {
      public bool Hugged;
      public bool Behind;
    }
  }
}
