// Type: FezGame.Components.Actions.ClimbLadder
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
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
  public class ClimbLadder : PlayerAction
  {
    private const float ClimbingSpeed = 0.425f;
    private ClimbingApproach currentApproach;
    private bool shouldSyncAnimationHalfway;
    private TimeSpan sinceGrabbed;
    private TrileInstance lastGrabbed;
    private SoundEffect climbSound;
    private int lastFrame;

    public ClimbLadder(Game game)
      : base(game)
    {
      this.UpdateOrder = 1;
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.climbSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/ClimbLadder");
    }

    public override void Initialize()
    {
      this.CameraManager.ViewpointChanged += new Action(this.ChangeApproach);
      base.Initialize();
    }

    private void ChangeApproach()
    {
      if (!this.IsActionAllowed(this.PlayerManager.Action) || !FezMath.IsOrthographic(this.CameraManager.Viewpoint) || (this.CameraManager.Viewpoint == this.CameraManager.LastViewpoint || this.PlayerManager.IsOnRotato))
        return;
      int num = (int) (this.currentApproach + FezMath.GetDistance(this.CameraManager.Viewpoint, this.CameraManager.LastViewpoint));
      if (num > 4)
        num -= 4;
      if (num < 1)
        num += 4;
      this.currentApproach = (ClimbingApproach) num;
      this.RefreshPlayerAction();
      this.RefreshPlayerDirection();
      this.shouldSyncAnimationHalfway = true;
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.Teetering:
        case ActionType.IdlePlay:
        case ActionType.IdleSleep:
        case ActionType.IdleLookAround:
        case ActionType.IdleYawn:
        case ActionType.Idle:
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Jumping:
        case ActionType.Lifting:
        case ActionType.Falling:
        case ActionType.Bouncing:
        case ActionType.Flying:
        case ActionType.Dropping:
        case ActionType.Sliding:
        case ActionType.Landing:
          TrileInstance trileInstance1 = this.IsOnLadder(out this.currentApproach);
          if (this.currentApproach == ClimbingApproach.None)
            break;
          bool flag = false;
          if (FezButtonStateExtensions.IsDown(this.InputManager.Down) && this.PlayerManager.Grounded)
          {
            TrileInstance trileInstance2 = this.LevelManager.NearestTrile(trileInstance1.Center - Vector3.UnitY).Surface;
            flag = trileInstance2 != null && trileInstance2.Trile.ActorSettings.Type == ActorType.Ladder;
          }
          FezButtonState fezButtonState = this.PlayerManager.Grounded ? FezButtonState.Pressed : FezButtonState.Down;
          if (!flag && this.InputManager.Up != fezButtonState && (this.PlayerManager.Grounded || this.currentApproach != ClimbingApproach.Left && this.currentApproach != ClimbingApproach.Right || Math.Sign(this.InputManager.Movement.X) != FezMath.Sign(ClimbingApproachExtensions.AsDirection(this.currentApproach))) || this.lastGrabbed != null && (trileInstance1 == this.lastGrabbed || trileInstance1.Position == this.lastGrabbed.Position + Vector3.UnitY || trileInstance1.Position == this.lastGrabbed.Position - Vector3.UnitY))
            break;
          this.PlayerManager.HeldInstance = trileInstance1;
          switch (this.currentApproach)
          {
            case ClimbingApproach.Right:
            case ClimbingApproach.Left:
              this.PlayerManager.NextAction = ActionType.SideClimbingLadder;
              break;
            case ClimbingApproach.Back:
              this.PlayerManager.NextAction = ActionType.BackClimbingLadder;
              break;
            case ClimbingApproach.Front:
              this.PlayerManager.NextAction = ActionType.FrontClimbingLadder;
              break;
          }
          if (this.PlayerManager.Grounded)
          {
            ActionType actionType = this.currentApproach == ClimbingApproach.Back ? ActionType.IdleToClimb : (this.currentApproach == ClimbingApproach.Front ? ActionType.IdleToFrontClimb : (this.currentApproach == ClimbingApproach.Back ? ActionType.IdleToClimb : ActionType.IdleToSideClimb));
            if (this.CollisionManager.CollidePoint(this.GetDestination(), Vector3.Down, QueryOptions.None, 0.0f, this.CameraManager.Viewpoint).Collided)
            {
              this.WalkTo.Destination = new Func<Vector3>(this.GetDestination);
              this.WalkTo.NextAction = actionType;
              this.PlayerManager.Action = ActionType.WalkingTo;
            }
            else
            {
              this.PlayerManager.Action = actionType;
              this.PlayerManager.Position -= 0.15f * Vector3.UnitY;
            }
          }
          else
            this.PlayerManager.Action = this.currentApproach == ClimbingApproach.Back ? ActionType.JumpToClimb : ActionType.JumpToSideClimb;
          if (this.currentApproach != ClimbingApproach.Left && this.currentApproach != ClimbingApproach.Right)
            break;
          this.PlayerManager.LookingDirection = ClimbingApproachExtensions.AsDirection(this.currentApproach);
          break;
      }
    }

    protected override void Begin()
    {
      this.PlayerManager.Position = this.PlayerManager.Position * Vector3.UnitY + (this.PlayerManager.HeldInstance.Position + FezMath.HalfVector) * FezMath.XZMask;
      if (FezButtonStateExtensions.IsDown(this.InputManager.Down))
        this.PlayerManager.Position -= 1.0 / 500.0 * Vector3.UnitY;
      this.GomezService.OnClimbLadder();
    }

    private TrileInstance IsOnLadder(out ClimbingApproach approach)
    {
      Vector3 b = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      Vector3 vector3 = FezMath.RightVector(this.CameraManager.Viewpoint);
      float num1 = float.MaxValue;
      bool flag1 = false;
      TrileInstance trileInstance1 = (TrileInstance) null;
      bool flag2 = true;
      if (this.currentApproach == ClimbingApproach.None)
      {
        NearestTriles nearestTriles = this.LevelManager.NearestTrile(this.PlayerManager.Center, this.PlayerManager.Background ? QueryOptions.Background : QueryOptions.None);
        flag2 = nearestTriles.Surface != null && nearestTriles.Surface.Trile.ActorSettings.Type == ActorType.Ladder;
      }
      foreach (NearestTriles nearestTriles in this.PlayerManager.AxisCollision.Values)
      {
        if (nearestTriles.Surface != null && this.TestLadderCollision(nearestTriles.Surface, true))
        {
          TrileInstance trileInstance2 = nearestTriles.Surface;
          float num2 = FezMath.Dot(trileInstance2.Position, b);
          if (flag2 && (double) num2 < (double) num1)
          {
            num1 = num2;
            trileInstance1 = trileInstance2;
          }
        }
      }
      if (trileInstance1 == null)
      {
        foreach (NearestTriles nearestTriles in this.PlayerManager.AxisCollision.Values)
        {
          if (nearestTriles.Surface != null && this.TestLadderCollision(nearestTriles.Surface, false))
          {
            TrileInstance trileInstance2 = nearestTriles.Surface;
            float num2 = FezMath.Dot(trileInstance2.Position, b);
            if (flag2 && (double) num2 < (double) num1)
            {
              flag1 = true;
              num1 = num2;
              trileInstance1 = trileInstance2;
            }
          }
        }
      }
      if (trileInstance1 != null)
      {
        float num2 = FezMath.Dot(FezMath.AsVector(FezMath.OrientationFromPhi(FezMath.WrapAngle(FezMath.ToPhi(trileInstance1.Trile.ActorSettings.Face) + trileInstance1.Phi))), flag1 ? vector3 : b);
        approach = !flag1 ? ((double) num2 > 0.0 ? ClimbingApproach.Front : ClimbingApproach.Back) : ((double) num2 > 0.0 ? ClimbingApproach.Left : ClimbingApproach.Right);
      }
      else
        approach = ClimbingApproach.None;
      return trileInstance1;
    }

    private bool TestLadderCollision(TrileInstance instance, bool onAxis)
    {
      TrileActorSettings actorSettings = instance.Trile.ActorSettings;
      Axis axis = FezMath.AxisFromPhi(FezMath.WrapAngle(FezMath.ToPhi(actorSettings.Face) + instance.Phi));
      if (actorSettings.Type == ActorType.Ladder)
        return axis == FezMath.VisibleAxis(this.CameraManager.Viewpoint) == onAxis;
      else
        return false;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.sinceGrabbed.TotalSeconds < 0.75)
        this.sinceGrabbed += gameTime.ElapsedGameTime;
      else
        this.lastGrabbed = (TrileInstance) null;
      if (this.shouldSyncAnimationHalfway && (double) this.CameraManager.ViewTransitionStep >= 0.5)
      {
        this.shouldSyncAnimationHalfway = false;
        this.SyncAnimation(true);
      }
      base.Update(gameTime);
    }

    private Vector3 GetDestination()
    {
      return this.PlayerManager.Position * Vector3.UnitY + (this.PlayerManager.HeldInstance.Position + FezMath.HalfVector) * FezMath.XZMask;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      ClimbingApproach approach;
      TrileInstance trileInstance1 = this.IsOnLadder(out approach);
      this.PlayerManager.HeldInstance = trileInstance1;
      if (trileInstance1 == null || this.currentApproach == ClimbingApproach.None)
      {
        this.PlayerManager.Action = ActionType.Idle;
        return false;
      }
      else
      {
        this.lastGrabbed = trileInstance1;
        this.sinceGrabbed = TimeSpan.Zero;
        this.currentApproach = approach;
        this.RefreshPlayerAction();
        this.RefreshPlayerDirection();
        this.PlayerManager.Position = this.PlayerManager.Position * Vector3.UnitY + (trileInstance1.Position + FezMath.HalfVector) * FezMath.XZMask;
        Vector3 vector3_1 = (float) ((double) this.InputManager.Movement.Y * 4.69999980926514 * 0.425000011920929) * (float) elapsed.TotalSeconds * Vector3.UnitY;
        Vector3 b = FezMath.ForwardVector(this.CameraManager.Viewpoint) * (this.PlayerManager.Background ? -1f : 1f);
        if ((double) this.PlayerManager.Center.Y < (double) this.LevelManager.Size.Y - 1.0 && (double) this.PlayerManager.Center.Y > 1.0)
        {
          QueryOptions options = this.PlayerManager.Background ? QueryOptions.Background : QueryOptions.None;
          NearestTriles nearestTriles1 = this.LevelManager.NearestTrile(this.PlayerManager.Center + Vector3.Down, options);
          NearestTriles nearestTriles2 = this.LevelManager.NearestTrile(this.PlayerManager.Center + vector3_1, options);
          NearestTriles nearestTriles3 = this.LevelManager.NearestTrile(this.PlayerManager.Center + vector3_1 + FezMath.Sign(vector3_1) * new Vector3(0.0f, 0.5f, 0.0f), options);
          bool flag = false;
          if ((nearestTriles2.Surface == null || (flag = nearestTriles3.Deep != null && (double) FezMath.Dot(this.PlayerManager.Position, b) > (double) FezMath.Dot(nearestTriles3.Deep.Center, b))) && (nearestTriles1.Deep == null || nearestTriles1.Deep.GetRotatedFace(this.PlayerManager.Background ? this.CameraManager.VisibleOrientation : FezMath.GetOpposite(this.CameraManager.VisibleOrientation)) == CollisionType.None))
          {
            vector3_1 = Vector3.Zero;
            if (!flag && (this.PlayerManager.LookingDirection == HorizontalDirection.Left && FezButtonStateExtensions.IsDown(this.InputManager.Left) || this.PlayerManager.LookingDirection == HorizontalDirection.Right && FezButtonStateExtensions.IsDown(this.InputManager.Right)))
            {
              if (nearestTriles2.Deep == null || nearestTriles1.Surface == null)
              {
                this.PlayerManager.Action = ActionType.ClimbOverLadder;
                return false;
              }
              else if ((double) FezMath.Dot(nearestTriles2.Deep.Center, b) > (double) FezMath.Dot(nearestTriles1.Surface.Center, b))
              {
                this.PlayerManager.Action = ActionType.ClimbOverLadder;
                return false;
              }
            }
          }
        }
        float num1 = (float) ((double) FezMath.Saturate(Math.Abs((float) ((double) this.PlayerManager.Animation.Timing.NormalizedStep * 2.0 % 1.0 - 0.5))) * 1.39999997615814 + 0.25);
        int frame = this.PlayerManager.Animation.Timing.Frame;
        if (this.lastFrame != frame)
        {
          if (frame == 1 || frame == 4)
            SoundEffectExtensions.EmitAt(this.climbSound, this.PlayerManager.Position);
          this.lastFrame = frame;
        }
        this.PlayerManager.Velocity = vector3_1 * num1;
        if (trileInstance1.PhysicsState != null)
        {
          IPlayerManager playerManager = this.PlayerManager;
          Vector3 vector3_2 = playerManager.Velocity + trileInstance1.PhysicsState.Velocity;
          playerManager.Velocity = vector3_2;
        }
        int num2 = Math.Sign(vector3_1.Y);
        this.PlayerManager.Animation.Timing.Update(elapsed, (float) num2);
        this.PlayerManager.GroundedVelocity = new Vector3?(this.PlayerManager.Velocity);
        MultipleHits<CollisionResult> multipleHits = this.CollisionManager.CollideEdge(this.PlayerManager.Center, vector3_1, this.PlayerManager.Size / 2f, Direction2D.Vertical);
        if ((double) vector3_1.Y < 0.0 && (multipleHits.NearLow.Collided || multipleHits.FarHigh.Collided))
        {
          TrileInstance trileInstance2 = this.LevelManager.NearestTrile(multipleHits.First.Destination.Center).Surface;
          if (trileInstance2 != null && trileInstance2.Trile.ActorSettings.Type == ActorType.Ladder && this.currentApproach == ClimbingApproach.Back)
          {
            IPlayerManager playerManager = this.PlayerManager;
            Vector3 vector3_2 = playerManager.Center + vector3_1;
            playerManager.Center = vector3_2;
          }
          else
          {
            this.lastGrabbed = (TrileInstance) null;
            this.PlayerManager.HeldInstance = (TrileInstance) null;
            this.PlayerManager.Action = ActionType.Falling;
          }
        }
        return false;
      }
    }

    private void RefreshPlayerAction()
    {
      switch (this.currentApproach)
      {
        case ClimbingApproach.Right:
        case ClimbingApproach.Left:
          this.PlayerManager.Action = ActionType.SideClimbingLadder;
          break;
        case ClimbingApproach.Back:
          this.PlayerManager.Action = ActionType.BackClimbingLadder;
          break;
        case ClimbingApproach.Front:
          this.PlayerManager.Action = ActionType.FrontClimbingLadder;
          break;
      }
    }

    private void RefreshPlayerDirection()
    {
      if (this.PlayerManager.Action != ActionType.SideClimbingLadder)
        return;
      this.PlayerManager.LookingDirection = ClimbingApproachExtensions.AsDirection(this.currentApproach);
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return ActionTypeExtensions.IsClimbingLadder(this.PlayerManager.Action);
    }
  }
}
