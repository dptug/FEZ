// Type: FezGame.Components.Actions.Grab
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class Grab : PlayerAction
  {
    private int pushingDirectionSign;
    private SoundEffect grabSound;
    private int lastFrame;

    public Grab(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.grabSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/GrabPickup");
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
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Sliding:
          if (!this.PlayerManager.Grounded || (double) this.InputManager.Movement.X == 0.0 || this.PlayerManager.LookingDirection == HorizontalDirection.None)
            break;
          TrileInstance trileInstance = this.PlayerManager.WallCollision.NearLow.Destination;
          if (trileInstance == null || !ActorTypeExtensions.IsPickable(trileInstance.Trile.ActorSettings.Type) || (trileInstance.GetRotatedFace(this.CameraManager.VisibleOrientation) != CollisionType.AllSides || trileInstance.Hidden) || (trileInstance.PhysicsState == null || !trileInstance.PhysicsState.Grounded))
            break;
          NearestTriles nearestTriles = this.LevelManager.NearestTrile(trileInstance.Position);
          if (nearestTriles.Surface != null && nearestTriles.Surface.Trile.ForceHugging || (double) Math.Abs(trileInstance.Center.Y - this.PlayerManager.Position.Y) > 0.5 || trileInstance.Trile.ActorSettings.Type == ActorType.Couch && FezMath.OrientationFromPhi(FezMath.ToPhi(trileInstance.Trile.ActorSettings.Face) + trileInstance.Phi) != FezMath.VisibleOrientation(FezMath.GetRotatedView(this.CameraManager.Viewpoint, this.PlayerManager.LookingDirection == HorizontalDirection.Right ? -1 : 1)))
            break;
          this.PlayerManager.Action = ActionType.Grabbing;
          this.PlayerManager.PushedInstance = trileInstance;
          break;
      }
    }

    protected override void Begin()
    {
      this.pushingDirectionSign = FezMath.Sign(this.PlayerManager.LookingDirection);
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity * Vector3.UnitY;
      playerManager.Velocity = vector3;
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
        int frame = this.PlayerManager.Animation.Timing.Frame;
        if (this.lastFrame != frame && this.PlayerManager.LastAction != ActionType.Pushing && this.PlayerManager.Action == ActionType.Grabbing)
        {
          if (frame == 3)
            SoundEffectExtensions.EmitAt(this.grabSound, this.PlayerManager.Position);
          this.lastFrame = frame;
        }
        Vector3 vector3_1 = FezMath.SideMask(this.CameraManager.Viewpoint);
        Vector3 vector3_2 = FezMath.DepthMask(this.CameraManager.Viewpoint);
        Vector3 vector3_3 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) this.pushingDirectionSign;
        this.PlayerManager.Center = Vector3.Up * this.PlayerManager.Center + (vector3_2 + vector3_1) * this.PlayerManager.PushedInstance.Center + -vector3_3 * (this.PlayerManager.PushedInstance.TransformedSize / 2f + this.PlayerManager.Size / 2f);
        if ((this.PlayerManager.Action == ActionType.Pushing || this.PlayerManager.Action == ActionType.Grabbing) && (this.pushingDirectionSign == -Math.Sign(this.InputManager.Movement.X) || !this.PlayerManager.Grounded))
        {
          this.PlayerManager.PushedInstance = (TrileInstance) null;
          this.PlayerManager.Action = ActionType.Idle;
          return false;
        }
        else
        {
          if (this.PlayerManager.Action != ActionType.Grabbing || !this.PlayerManager.Animation.Timing.Ended || (double) this.InputManager.Movement.X == 0.0)
            return this.PlayerManager.Action == ActionType.Grabbing;
          this.PlayerManager.Action = ActionType.Pushing;
          return false;
        }
      }
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.Grabbing)
        return type == ActionType.Pushing;
      else
        return true;
    }
  }
}
