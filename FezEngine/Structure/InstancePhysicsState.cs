// Type: FezEngine.Structure.InstancePhysicsState
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class InstancePhysicsState : ISimplePhysicsEntity, IPhysicsEntity
  {
    private readonly TrileInstance instance;

    public bool Vanished { get; set; }

    public bool ShouldRespawn { get; set; }

    public bool Respawned { get; set; }

    public bool UpdatingPhysics { get; set; }

    public bool Sticky { get; set; }

    public bool Puppet { get; set; }

    public bool Paused { get; set; }

    public bool PushedUp { get; set; }

    public TrileInstance PushedDownBy { get; set; }

    public bool IgnoreCollision { get; set; }

    public bool ForceNonStatic { get; set; }

    public bool Background { get; set; }

    public MultipleHits<CollisionResult> WallCollision { get; set; }

    public MultipleHits<TrileInstance> Ground { get; set; }

    public Vector3 Velocity { get; set; }

    public Vector3 GroundMovement { get; set; }

    public bool NoVelocityClamping { get; set; }

    public bool IgnoreClampToWater { get; set; }

    public PointCollision[] CornerCollision { get; private set; }

    public bool Grounded
    {
      get
      {
        return this.Ground.First != null;
      }
    }

    public Vector3 Center { get; set; }

    public Vector3 Size
    {
      get
      {
        return this.instance.TransformedSize;
      }
    }

    public bool Static
    {
      get
      {
        if (this.StaticGrounds && this.Grounded && FezMath.AlmostEqual(this.Velocity, Vector3.Zero))
          return !this.ForceNonStatic;
        else
          return false;
      }
    }

    public bool StaticGrounds
    {
      get
      {
        if (InstancePhysicsState.IsGroundStatic(this.Ground.NearLow))
          return InstancePhysicsState.IsGroundStatic(this.Ground.FarHigh);
        else
          return false;
      }
    }

    public bool Sliding
    {
      get
      {
        return !FezMath.AlmostEqual(FezMath.XZ(this.Velocity), Vector2.Zero);
      }
    }

    public float Elasticity { get; private set; }

    public bool Floating { get; set; }

    public InstancePhysicsState(TrileInstance instance)
    {
      this.instance = instance;
      this.Center = instance.Center;
      this.CornerCollision = new PointCollision[4];
      Trile trile = instance.Trile;
      this.Elasticity = trile.ActorSettings.Type == ActorType.Vase || Enumerable.Any<CollisionType>((IEnumerable<CollisionType>) trile.Faces.Values, (Func<CollisionType, bool>) (x => x == CollisionType.AllSides)) ? 0.0f : 0.15f;
    }

    private static bool IsGroundStatic(TrileInstance ground)
    {
      if (ground == null || ground.PhysicsState == null)
        return true;
      if (ground.PhysicsState.Velocity == Vector3.Zero)
        return ground.PhysicsState.GroundMovement == Vector3.Zero;
      else
        return false;
    }

    public void UpdateInstance()
    {
      this.instance.Position = this.Center - this.instance.Center - this.instance.Position;
    }
  }
}
