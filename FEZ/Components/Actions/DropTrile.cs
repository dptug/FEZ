// Type: FezGame.Components.Actions.DropTrile
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
  internal class DropTrile : PlayerAction
  {
    private SoundEffect dropHeavySound;
    private SoundEffect dropLightSound;

    public DropTrile(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.dropHeavySound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/DropHeavyPickup");
      this.dropLightSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/DropLightPickup");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.CarryIdle:
        case ActionType.CarryWalk:
        case ActionType.CarryJump:
        case ActionType.CarrySlide:
        case ActionType.CarryHeavyIdle:
        case ActionType.CarryHeavyWalk:
        case ActionType.CarryHeavyJump:
        case ActionType.CarryHeavySlide:
          if (this.PlayerManager.Background || this.InputManager.GrabThrow != FezButtonState.Pressed || (double) this.InputManager.Movement.Y >= -0.5 && (double) Math.Abs(this.InputManager.Movement.X) >= 0.25)
            break;
          TrileInstance carriedInstance = this.PlayerManager.CarriedInstance;
          this.PlayerManager.Action = ActorTypeExtensions.IsLight(carriedInstance.Trile.ActorSettings.Type) ? ActionType.DropTrile : ActionType.DropHeavyTrile;
          Vector3 vector3_1 = FezMath.SideMask(this.CameraManager.Viewpoint);
          Vector3 vector3_2 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
          Vector3 vector3_3 = this.PlayerManager.Center + this.PlayerManager.Size / 2f * (Vector3.Down + vector3_2) - carriedInstance.TransformedSize / 2f * vector3_2 + carriedInstance.Trile.Size / 2f * (Vector3.UnitY + vector3_2) + 0.125f * vector3_2;
          carriedInstance.Enabled = false;
          MultipleHits<CollisionResult> result = this.CollisionManager.CollideEdge(carriedInstance.Center, vector3_3 - carriedInstance.Center, carriedInstance.TransformedSize / 2f, Direction2D.Horizontal);
          if (BoxCollisionResultExtensions.AnyCollided(result))
          {
            CollisionResult collisionResult = result.NearLow;
            if (!collisionResult.Collided || collisionResult.Destination.GetRotatedFace(this.CameraManager.VisibleOrientation) != CollisionType.AllSides || (double) Math.Abs(collisionResult.Destination.Center.Y - vector3_3.Y) >= 1.0)
              collisionResult = result.FarHigh;
            if (collisionResult.Collided && collisionResult.Destination.GetRotatedFace(this.CameraManager.VisibleOrientation) == CollisionType.AllSides && (double) Math.Abs(collisionResult.Destination.Center.Y - vector3_3.Y) < 1.0)
            {
              TrileInstance trileInstance = collisionResult.Destination;
              Vector3 vector3_4 = trileInstance.Center - vector3_2 * trileInstance.TransformedSize / 2f;
              Vector3 vector3_5 = vector3_3 + vector3_2 * carriedInstance.TransformedSize / 2f;
              this.PlayerManager.Position -= vector3_1 * (vector3_5 - vector3_4);
            }
          }
          carriedInstance.Enabled = true;
          IPlayerManager playerManager = this.PlayerManager;
          Vector3 vector3_6 = playerManager.Velocity * Vector3.UnitY;
          playerManager.Velocity = vector3_6;
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      if (ActorTypeExtensions.IsHeavy(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type))
        SoundEffectExtensions.EmitAt(this.dropHeavySound, this.PlayerManager.Position);
      else
        SoundEffectExtensions.EmitAt(this.dropLightSound, this.PlayerManager.Position);
      this.GomezService.OnDropObject();
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.CarriedInstance == null)
      {
        this.PlayerManager.Action = ActionType.Idle;
        return false;
      }
      else
      {
        Vector3 vector3_1 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
        TrileInstance carriedInstance = this.PlayerManager.CarriedInstance;
        Vector3 vector3_2 = this.PlayerManager.Center + this.PlayerManager.Size / 2f * (Vector3.Down + vector3_1) - carriedInstance.TransformedSize / 2f * vector3_1 + carriedInstance.Trile.Size / 2f * (Vector3.UnitY + vector3_1) + 0.125f * vector3_1;
        bool flag = ActorTypeExtensions.IsLight(carriedInstance.Trile.ActorSettings.Type);
        Vector2[] vector2Array = flag ? Lift.LightTrilePositioning : Lift.HeavyTrilePositioning;
        int index = (flag ? 4 : 7) - this.PlayerManager.Animation.Timing.Frame;
        Vector3 vector3_3 = vector3_2 + (vector2Array[index].X * -vector3_1 + vector2Array[index].Y * Vector3.Up) * 1f / 16f;
        carriedInstance.PhysicsState.Center = vector3_3;
        carriedInstance.PhysicsState.UpdateInstance();
        this.PlayerManager.Position -= vector3_3 - carriedInstance.Center;
        this.PlayerManager.CarriedInstance.PhysicsState.UpdateInstance();
        if (this.PlayerManager.Animation.Timing.Ended)
        {
          Vector3 impulse = (float) (3.15000009536743 * (double) Math.Sign(this.CollisionManager.GravityFactor) * 0.150000005960464) * (float) elapsed.TotalSeconds * Vector3.Down;
          if ((double) this.PlayerManager.GroundMovement.Y < 0.0)
            impulse += this.PlayerManager.GroundMovement;
          MultipleHits<CollisionResult> result = this.CollisionManager.CollideEdge(carriedInstance.PhysicsState.Center, impulse, carriedInstance.TransformedSize / 2f, Direction2D.Vertical);
          if (BoxCollisionResultExtensions.AnyCollided(result))
          {
            carriedInstance.PhysicsState.Ground = new MultipleHits<TrileInstance>()
            {
              NearLow = result.NearLow.Collided ? result.NearLow.Destination : (TrileInstance) null,
              FarHigh = result.FarHigh.Collided ? result.FarHigh.Destination : (TrileInstance) null
            };
            if (carriedInstance.PhysicsState.Ground.First.PhysicsState != null)
            {
              carriedInstance.PhysicsState.GroundMovement = carriedInstance.PhysicsState.Ground.First.PhysicsState.Velocity;
              carriedInstance.PhysicsState.Center += carriedInstance.PhysicsState.GroundMovement;
            }
          }
          carriedInstance.PhysicsState.Velocity = impulse;
          carriedInstance.PhysicsState.UpdateInstance();
          if (flag)
          {
            this.PlayerManager.Action = ActionType.Idle;
          }
          else
          {
            this.PlayerManager.PushedInstance = this.PlayerManager.CarriedInstance;
            this.PlayerManager.Action = ActionType.Grabbing;
          }
          this.PlayerManager.CarriedInstance = (TrileInstance) null;
          this.PhysicsManager.HugWalls((IPhysicsEntity) this.PlayerManager, false, false, true);
        }
        return true;
      }
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.DropTrile)
        return type == ActionType.DropHeavyTrile;
      else
        return true;
    }
  }
}
