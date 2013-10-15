// Type: FezGame.Components.Actions.Push
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class Push : PlayerAction
  {
    private readonly MovementHelper movementHelper = new MovementHelper(1.88f, 0.0f, float.MaxValue);
    private TrileGroup pickupGroup;
    private SoundEffect sCratePush;
    private SoundEffect sGomezPush;
    private SoundEmitter eCratePush;
    private SoundEmitter eGomezPush;

    public Push(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.sCratePush = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/PushPickup");
      this.sGomezPush = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/GomezPush");
    }

    protected override void Begin()
    {
      base.Begin();
      if (this.eCratePush == null || this.eCratePush.Dead)
        this.eCratePush = SoundEffectExtensions.EmitAt(this.sCratePush, this.PlayerManager.PushedInstance.Center, true);
      else
        this.eCratePush.Cue.Resume();
      if (this.eGomezPush == null || this.eGomezPush.Dead)
        this.eGomezPush = SoundEffectExtensions.EmitAt(this.sGomezPush, this.PlayerManager.Position, true);
      else
        this.eGomezPush.Cue.Resume();
      if (this.LevelManager.PickupGroups.TryGetValue(this.PlayerManager.PushedInstance, out this.pickupGroup))
        return;
      this.pickupGroup = (TrileGroup) null;
    }

    protected override void TestConditions()
    {
      base.TestConditions();
      if (this.PlayerManager.Action != ActionType.Pushing && this.eCratePush != null && (!this.eCratePush.Dead && this.eCratePush.Cue.State != SoundState.Paused))
        this.eCratePush.Cue.Pause();
      if (this.PlayerManager.Action == ActionType.Pushing || this.eGomezPush == null || (this.eGomezPush.Dead || this.eGomezPush.Cue.State == SoundState.Paused))
        return;
      this.eGomezPush.Cue.Pause();
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.PushedInstance == null || this.PlayerManager.PushedInstance.Hidden || this.PlayerManager.PushedInstance.PhysicsState == null)
      {
        this.PlayerManager.Action = ActionType.Idle;
        this.PlayerManager.PushedInstance = (TrileInstance) null;
        return false;
      }
      else
      {
        Vector3 b = FezMath.SideMask(this.CameraManager.Viewpoint);
        Vector3 vector3 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
        TrileInstance pushedInstance = this.PlayerManager.PushedInstance;
        InstancePhysicsState physicsState = pushedInstance.PhysicsState;
        this.eCratePush.Position = pushedInstance.Center;
        this.eGomezPush.Position = this.PlayerManager.Center;
        if (!physicsState.Grounded)
        {
          this.PlayerManager.PushedInstance = (TrileInstance) null;
          this.PlayerManager.Action = ActionType.Idle;
          return false;
        }
        else
        {
          int stackSize = this.FindStackSize(pushedInstance, 0);
          if (stackSize <= 2)
          {
            this.movementHelper.Entity = (IPhysicsEntity) physicsState;
            float num = this.InputManager.Movement.X;
            if (BoxCollisionResultExtensions.AnyCollided(physicsState.WallCollision) && ActorTypeExtensions.IsPickable(physicsState.WallCollision.First.Destination.Trile.ActorSettings.Type))
              num *= 5f;
            if (pushedInstance.Trile.ActorSettings.Type == ActorType.Couch)
              num *= 2f;
            this.movementHelper.Update((float) elapsed.TotalSeconds, num / (float) (stackSize + 1));
            if (this.pickupGroup != null)
            {
              pushedInstance.PhysicsState.Puppet = false;
              foreach (TrileInstance trileInstance in this.pickupGroup.Triles)
              {
                if (trileInstance != pushedInstance)
                {
                  trileInstance.PhysicsState.Velocity = pushedInstance.PhysicsState.Velocity;
                  trileInstance.PhysicsState.Puppet = true;
                }
              }
            }
            this.PlayerManager.Center = Vector3.Up * this.PlayerManager.Center + (FezMath.DepthMask(this.CameraManager.Viewpoint) + b) * physicsState.Center + -vector3 * (pushedInstance.TransformedSize / 2f + this.PlayerManager.Size / 2f);
            this.eCratePush.VolumeFactor = FezMath.Saturate(Math.Abs(FezMath.Dot(physicsState.Velocity, b)) / 0.024f);
            if (FezMath.AlmostEqual(FezMath.Dot(physicsState.Velocity, b), 0.0f))
            {
              this.PlayerManager.Action = ActionType.Grabbing;
              return false;
            }
          }
          else
          {
            this.PlayerManager.Action = ActionType.Grabbing;
            this.eCratePush.Cue.Pause();
            this.eGomezPush.Cue.Pause();
          }
          return this.PlayerManager.Action == ActionType.Pushing;
        }
      }
    }

    private int FindStackSize(TrileInstance instance, int stackSize)
    {
      Vector3 halfSize = instance.TransformedSize / 2f - new Vector3(0.004f);
      MultipleHits<CollisionResult> result = this.CollisionManager.CollideEdge(instance.Center, instance.Trile.Size.Y * Vector3.Up, halfSize, Direction2D.Vertical);
      if (BoxCollisionResultExtensions.AnyCollided(result))
      {
        TrileInstance instance1 = result.First.Destination;
        if (instance1.PhysicsState != null && instance1.PhysicsState.Grounded)
          return this.FindStackSize(instance1, stackSize + 1);
      }
      return stackSize;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Pushing;
    }
  }
}
