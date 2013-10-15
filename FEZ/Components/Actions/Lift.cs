// Type: FezGame.Components.Actions.Lift
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class Lift : PlayerAction
  {
    public static readonly Vector2[] LightTrilePositioning = new Vector2[8]
    {
      Vector2.Zero,
      Vector2.Zero,
      new Vector2(1f, 2f),
      new Vector2(4f, 9f),
      new Vector2(8f, 14f),
      new Vector2(9f, 14f),
      new Vector2(10f, 10f),
      new Vector2(10f, 11f)
    };
    public static readonly Vector2[] HeavyTrilePositioning = new Vector2[10]
    {
      Vector2.Zero,
      Vector2.Zero,
      Vector2.Zero,
      new Vector2(1f, 1f),
      new Vector2(2f, 3f),
      new Vector2(4f, 7f),
      new Vector2(7f, 12f),
      new Vector2(8f, 13f),
      new Vector2(10f, 9f),
      new Vector2(11f, 10f)
    };
    private SoundEffect liftHeavySound;
    private SoundEffect liftLightSound;

    static Lift()
    {
    }

    public Lift(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.liftHeavySound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LiftHeavyPickup");
      this.liftLightSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LiftLightPickup");
    }

    protected override void TestConditions()
    {
      if ((double) this.CollisionManager.GravityFactor < 0.0)
        return;
      switch (this.PlayerManager.Action)
      {
        case ActionType.IdlePlay:
        case ActionType.IdleSleep:
        case ActionType.IdleLookAround:
        case ActionType.IdleYawn:
        case ActionType.Grabbing:
        case ActionType.Pushing:
        case ActionType.Teetering:
        case ActionType.Idle:
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Sliding:
        case ActionType.Landing:
          if (this.PlayerManager.Background || !this.PlayerManager.Grounded || this.InputManager.GrabThrow != FezButtonState.Pressed)
            break;
          TrileInstance key = this.PlayerManager.PushedInstance ?? this.PlayerManager.AxisCollision[VerticalDirection.Up].Deep ?? this.PlayerManager.AxisCollision[VerticalDirection.Down].Deep;
          if (key == null || !ActorTypeExtensions.IsPickable(key.Trile.ActorSettings.Type) || key.Trile.ActorSettings.Type == ActorType.Couch || !key.PhysicsState.Grounded)
            break;
          Vector3 halfSize = key.TransformedSize / 2f - new Vector3(0.004f);
          if (BoxCollisionResultExtensions.AnyCollided(this.CollisionManager.CollideEdge(key.Center, key.Trile.Size.Y * Vector3.Up * (float) Math.Sign(this.CollisionManager.GravityFactor), halfSize, Direction2D.Vertical)))
            break;
          TrileInstance trileInstance = this.LevelManager.ActualInstanceAt(key.Center + Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor));
          TrileGroup trileGroup;
          if (trileInstance != null && trileInstance.PhysicsState != null && trileInstance.PhysicsState.Ground.First == key || this.LevelManager.PickupGroups.TryGetValue(key, out trileGroup) && trileGroup.Triles.Count > 1)
            break;
          ActionType actionType = ActorTypeExtensions.IsLight(key.Trile.ActorSettings.Type) ? ActionType.Lifting : ActionType.LiftingHeavy;
          if (this.PlayerManager.Action == ActionType.Grabbing)
          {
            this.PlayerManager.CarriedInstance = key;
            this.PlayerManager.Action = actionType;
            break;
          }
          else
          {
            this.PlayerManager.PushedInstance = key;
            this.WalkTo.Destination = new Func<Vector3>(this.GetDestination);
            this.WalkTo.NextAction = actionType;
            this.PlayerManager.Action = ActionType.WalkingTo;
            break;
          }
      }
    }

    private Vector3 GetDestination()
    {
      TrileInstance pushedInstance = this.PlayerManager.PushedInstance;
      Vector3 vector = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
      Vector3 vector3_1 = FezMath.Abs(vector);
      Vector3 vector3_2 = FezMath.DepthMask(this.CameraManager.Viewpoint);
      return (pushedInstance.Center * vector3_1 + this.PlayerManager.Position * vector3_2) * FezMath.XZMask + -pushedInstance.TransformedSize / 2f * vector + -7.0 / 16.0 * vector + this.PlayerManager.Position * Vector3.UnitY;
    }

    protected override void Begin()
    {
      if (this.PlayerManager.CarriedInstance == null && this.PlayerManager.PushedInstance == null)
      {
        this.PlayerManager.Action = ActionType.Idle;
      }
      else
      {
        Vector3 vector = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
        Vector3 mask = FezMath.GetMask(FezMath.VisibleAxis(this.CameraManager.Viewpoint));
        Vector3 vector3_1 = FezMath.Abs(vector);
        if (this.PlayerManager.PushedInstance != null)
        {
          this.PlayerManager.CarriedInstance = this.PlayerManager.PushedInstance;
          this.PlayerManager.PushedInstance = (TrileInstance) null;
        }
        TrileInstance carriedInstance = this.PlayerManager.CarriedInstance;
        TrileInstance first = this.PlayerManager.Ground.First;
        Vector3 vector3_2 = carriedInstance.Center * vector3_1 + this.PlayerManager.Position * mask + (first.Center.Y + (float) ((double) first.Trile.Size.Y / 2.0 + (double) carriedInstance.Trile.Size.Y / 2.0) * (float) Math.Sign(this.CollisionManager.GravityFactor)) * Vector3.UnitY;
        this.PlayerManager.CarriedInstance.PhysicsState.Center = vector3_2;
        this.PlayerManager.CarriedInstance.PhysicsState.UpdateInstance();
        this.PlayerManager.Position = this.PlayerManager.Position * (Vector3.One - vector3_1) + vector3_2 * vector3_1 + -carriedInstance.TransformedSize / 2f * vector + -3.0 / 16.0 * vector;
        if (ActorTypeExtensions.IsHeavy(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type))
          SoundEffectExtensions.EmitAt(this.liftHeavySound, this.PlayerManager.Position);
        else
          SoundEffectExtensions.EmitAt(this.liftLightSound, this.PlayerManager.Position);
        this.GomezService.OnLiftObject();
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3_3 = playerManager.Velocity * Vector3.UnitY;
        playerManager.Velocity = vector3_3;
      }
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.CarriedInstance == null)
        return false;
      Vector3 vector3_1 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
      Vector3 vector3_2 = this.PlayerManager.Center + this.PlayerManager.Size / 2f * (Vector3.Down * (float) Math.Sign(this.CollisionManager.GravityFactor) + vector3_1) - this.PlayerManager.CarriedInstance.TransformedSize / 2f * vector3_1 + this.PlayerManager.CarriedInstance.Trile.Size / 2f * (Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor) + vector3_1);
      bool flag = ActorTypeExtensions.IsLight(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type);
      this.PlayerManager.Animation.Timing.Update(elapsed);
      int frame = this.PlayerManager.Animation.Timing.Frame;
      Vector2[] vector2Array = flag ? Lift.LightTrilePositioning : Lift.HeavyTrilePositioning;
      this.PlayerManager.CarriedInstance.PhysicsState.Center = vector3_2 + (vector2Array[frame].X * -vector3_1 + vector2Array[frame].Y * Vector3.Up * (float) Math.Sign(this.CollisionManager.GravityFactor)) * (1.0 / 16.0) + 3.0 / 16.0 * vector3_1;
      this.PlayerManager.CarriedInstance.PhysicsState.UpdateInstance();
      if (this.PlayerManager.Animation.Timing.Ended)
        this.PlayerManager.Action = flag ? ActionType.CarryIdle : ActionType.CarryHeavyIdle;
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.Lifting)
        return type == ActionType.LiftingHeavy;
      else
        return true;
    }
  }
}
