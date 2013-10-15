// Type: FezGame.Components.Actions.GrabCornerLedge
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
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
  public class GrabCornerLedge : PlayerAction
  {
    private const float VelocityThreshold = 0.025f;
    private const float MovementThreshold = 0.1f;
    private const float DistanceThreshold = 0.35f;
    private Viewpoint? rotatedFrom;
    private Vector3 huggedCorner;
    private SoundEffect sound;

    protected override bool ViewTransitionIndependent
    {
      get
      {
        return true;
      }
    }

    public GrabCornerLedge(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      this.sound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LedgeGrab");
      this.CameraManager.ViewpointChanged += (Action) (() =>
      {
        if (!this.IsActionAllowed(this.PlayerManager.Action) || !FezMath.IsOrthographic(this.CameraManager.Viewpoint) || (this.CameraManager.Viewpoint == this.CameraManager.LastViewpoint || this.PlayerManager.IsOnRotato))
          return;
        if (this.rotatedFrom.HasValue && this.rotatedFrom.Value == this.CameraManager.Viewpoint)
        {
          this.rotatedFrom = new Viewpoint?();
        }
        else
        {
          if (this.rotatedFrom.HasValue)
            return;
          this.rotatedFrom = new Viewpoint?(this.CameraManager.LastViewpoint);
        }
      });
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.Jumping:
        case ActionType.Falling:
          HorizontalDirection lookingDirection = this.PlayerManager.LookingDirection;
          if (lookingDirection == HorizontalDirection.None)
            break;
          Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
          Vector3 b = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(lookingDirection);
          if ((double) FezMath.Dot(this.PlayerManager.Velocity, b) <= 0.025000000372529 && (double) (this.InputManager.Movement.X * (float) FezMath.Sign(lookingDirection)) <= 0.100000001490116)
            break;
          MultipleHits<CollisionResult> wallCollision = this.PlayerManager.WallCollision;
          FaceOrientation visibleOrientation = this.CameraManager.VisibleOrientation;
          TrileInstance trileInstance1 = wallCollision.NearLow.Destination ?? this.PlayerManager.CornerCollision[1 + (lookingDirection == HorizontalDirection.Left ? 2 : 0)].Instances.Deep;
          TrileInstance trileInstance2 = wallCollision.FarHigh.Destination ?? this.PlayerManager.CornerCollision[lookingDirection == HorizontalDirection.Left ? 2 : 0].Instances.Deep;
          Trile trile = trileInstance1 == null ? (Trile) null : trileInstance1.Trile;
          if (trileInstance1 == null || trileInstance1.GetRotatedFace(visibleOrientation) == CollisionType.None || (trile.ActorSettings.Type == ActorType.Ladder || trileInstance1 == trileInstance2) || (trileInstance2 != null && trileInstance2.GetRotatedFace(visibleOrientation) == CollisionType.AllSides || !trileInstance1.Enabled))
            break;
          TrileInstance trileInstance3 = this.LevelManager.ActualInstanceAt(trileInstance1.Center - b);
          TrileInstance trileInstance4 = this.LevelManager.NearestTrile(trileInstance1.Center - b).Deep;
          if (trileInstance4 != null && trileInstance4.Enabled && trileInstance4.GetRotatedFace(this.CameraManager.VisibleOrientation) != CollisionType.None || trileInstance3 != null && trileInstance3.Enabled && !trileInstance3.Trile.Immaterial || this.PlayerManager.Action == ActionType.Jumping && (double) ((trileInstance1.Center - this.PlayerManager.LeaveGroundPosition) * vector3_1).Length() < 1.25 || trileInstance1.GetRotatedFace(visibleOrientation) != CollisionType.AllSides && BoxCollisionResultExtensions.AnyHit(this.CollisionManager.CollideEdge(trileInstance1.Center, Vector3.Down * (float) Math.Sign(this.CollisionManager.GravityFactor), this.PlayerManager.Size * FezMath.XZMask / 2f, Direction2D.Vertical)))
            break;
          Vector3 vector3_2 = (-b + Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor)) * trileInstance1.TransformedSize / 2f;
          if ((double) (this.PlayerManager.Center * vector3_1 - trileInstance1.Center * vector3_1 + vector3_2).Length() >= 0.349999994039536)
            break;
          this.PlayerManager.HeldInstance = trileInstance1;
          this.PlayerManager.Action = ActionType.GrabCornerLedge;
          Waiters.Wait(0.1, (Action) (() =>
          {
            if (this.PlayerManager.HeldInstance == null)
              return;
            SoundEffectExtensions.EmitAt(this.sound, this.PlayerManager.Position);
            this.InputManager.ActiveGamepad.Vibrate(VibrationMotor.LeftLow, 0.100000001490116, TimeSpan.FromSeconds(0.200000002980232));
            this.InputManager.ActiveGamepad.Vibrate(VibrationMotor.RightHigh, 0.400000005960464, TimeSpan.FromSeconds(0.200000002980232));
          }));
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      this.PlayerManager.Velocity = Vector3.Zero;
      this.PlayerManager.GroundedVelocity = new Vector3?(FezMath.RightVector(this.CameraManager.Viewpoint) * 0.085f + Vector3.UnitY * 0.15f * (float) Math.Sign(this.CollisionManager.GravityFactor));
      this.InputManager.PressedToDown();
      this.GomezService.OnGrabLedge();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.rotatedFrom.HasValue && (double) this.CameraManager.ViewTransitionStep >= 0.6)
      {
        int distance = FezMath.GetDistance(this.CameraManager.Viewpoint, this.rotatedFrom.Value);
        if (Math.Abs(distance) % 2 == 0)
        {
          this.PlayerManager.LookingDirection = FezMath.GetOpposite(this.PlayerManager.LookingDirection);
        }
        else
        {
          if (this.PlayerManager.LookingDirection == HorizontalDirection.Right)
            this.PlayerManager.Action = Math.Sign(distance) > 0 ? ActionType.GrabLedgeBack : ActionType.GrabLedgeFront;
          else
            this.PlayerManager.Action = Math.Sign(distance) > 0 ? ActionType.GrabLedgeFront : ActionType.GrabLedgeBack;
          if (this.PlayerManager.Action == ActionType.GrabLedgeBack)
          {
            this.PlayerManager.Position -= this.PlayerManager.Size.Z / 4f * FezMath.ForwardVector(this.CameraManager.Viewpoint);
            this.CorrectWallOverlap(true);
            this.PlayerManager.Background = false;
          }
          else
          {
            this.PlayerManager.Position += this.PlayerManager.Size.Z / 4f * FezMath.ForwardVector(this.CameraManager.Viewpoint);
            this.PlayerManager.Background = true;
          }
        }
        this.SyncAnimation(this.IsActionAllowed(this.PlayerManager.Action));
        this.rotatedFrom = new Viewpoint?();
      }
      if (ActionTypeExtensions.IsOnLedge(this.PlayerManager.Action))
      {
        if (this.PlayerManager.HeldInstance == null)
          this.PlayerManager.Action = ActionType.Idle;
        else if (this.PlayerManager.HeldInstance.PhysicsState != null && (double) Math.Abs(FezMath.Dot(this.PlayerManager.HeldInstance.PhysicsState.Velocity, Vector3.One)) > 0.5)
        {
          this.PlayerManager.Velocity = this.PlayerManager.HeldInstance.PhysicsState.Velocity;
          this.PlayerManager.HeldInstance = (TrileInstance) null;
          this.PlayerManager.Action = ActionType.Jumping;
        }
      }
      base.Update(gameTime);
    }

    private void CorrectWallOverlap(bool overcompensate)
    {
      foreach (PointCollision pointCollision in this.PlayerManager.CornerCollision)
      {
        TrileInstance trileInstance = pointCollision.Instances.Deep;
        if (trileInstance != null && trileInstance != this.PlayerManager.CarriedInstance && trileInstance.GetRotatedFace(this.CameraManager.VisibleOrientation) == CollisionType.AllSides)
        {
          Vector3 vector = (pointCollision.Point - trileInstance.Center + FezMath.Sign(this.PlayerManager.Position - pointCollision.Point) * trileInstance.TransformedSize / 2f) * FezMath.SideMask(this.CameraManager.Viewpoint);
          this.PlayerManager.Position -= vector;
          if (overcompensate)
            this.PlayerManager.Position -= FezMath.Sign(vector) * (1.0 / 1000.0) * 2f;
          if (!(FezMath.Sign(this.PlayerManager.Velocity) == FezMath.Sign(vector)))
            break;
          Vector3 vector3_1 = FezMath.Abs(FezMath.Sign(vector));
          this.PlayerManager.Position -= this.PlayerManager.Velocity * vector3_1;
          IPlayerManager playerManager = this.PlayerManager;
          Vector3 vector3_2 = playerManager.Velocity * (Vector3.One - vector3_1);
          playerManager.Velocity = vector3_2;
          break;
        }
      }
    }

    protected override bool Act(TimeSpan elapsed)
    {
      NearestTriles nearestTriles = this.LevelManager.NearestTrile(this.PlayerManager.HeldInstance.Center);
      CollisionType collisionType = CollisionType.None;
      bool flag = false;
      if (nearestTriles.Deep != null)
      {
        collisionType = nearestTriles.Deep.GetRotatedFace(FezMath.VisibleOrientation(this.CameraManager.Viewpoint));
        flag = flag | collisionType == CollisionType.AllSides;
      }
      if (flag && (this.InputManager.RotateLeft == FezButtonState.Pressed || this.InputManager.RotateRight == FezButtonState.Pressed))
        this.InputManager.PressedToDown();
      if (nearestTriles.Deep == null)
        flag = true;
      if (nearestTriles.Deep != null)
        flag = flag | collisionType == CollisionType.TopNoStraightLedge;
      FezButtonState fezButtonState = this.PlayerManager.Animation.Timing.Ended ? FezButtonState.Down : FezButtonState.Pressed;
      if (this.CameraManager.ActionRunning && !flag && (this.PlayerManager.LookingDirection == HorizontalDirection.Right && this.InputManager.Right == fezButtonState || this.PlayerManager.LookingDirection == HorizontalDirection.Left && this.InputManager.Left == fezButtonState))
      {
        bool background1 = this.PlayerManager.Background;
        Vector3 position = this.PlayerManager.Position;
        this.PlayerManager.Position += FezMath.RightVector(this.CameraManager.Viewpoint) * (float) -FezMath.Sign(this.PlayerManager.LookingDirection) * 0.5f;
        this.PhysicsManager.DetermineInBackground((IPhysicsEntity) this.PlayerManager, true, false, false);
        bool background2 = this.PlayerManager.Background;
        this.PlayerManager.Background = background1;
        this.PlayerManager.Position = position;
        if (!background2)
        {
          this.PlayerManager.Action = ActionType.FromCornerBack;
          return false;
        }
      }
      this.huggedCorner = (-(!this.rotatedFrom.HasValue ? FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection) : FezMath.RightVector(this.rotatedFrom.Value) * (float) FezMath.Sign(this.PlayerManager.LookingDirection)) + Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor)) * this.PlayerManager.HeldInstance.TransformedSize / 2f;
      this.PlayerManager.Position = this.PlayerManager.HeldInstance.Center + this.huggedCorner;
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.GrabCornerLedge;
    }
  }
}
