// Type: FezGame.Components.Actions.GrabStraightLedge
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
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
  public class GrabStraightLedge : PlayerAction
  {
    private Viewpoint? rotatedFrom;
    private SoundEffect sound;

    protected override bool ViewTransitionIndependent
    {
      get
      {
        return true;
      }
    }

    public GrabStraightLedge(Game game)
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
          if (!FezButtonStateExtensions.IsDown(this.InputManager.Up))
            break;
          FaceOrientation face = FezMath.VisibleOrientation(this.CameraManager.Viewpoint);
          TrileInstance trileInstance1 = this.PlayerManager.AxisCollision[VerticalDirection.Up].Deep;
          TrileInstance trileInstance2 = this.PlayerManager.AxisCollision[VerticalDirection.Down].Deep;
          if (trileInstance1 != null && trileInstance1.GetRotatedFace(face) == CollisionType.AllSides || (trileInstance2 == null || trileInstance2.GetRotatedFace(face) != CollisionType.TopOnly) || BoxCollisionResultExtensions.AnyHit(this.CollisionManager.CollideEdge(trileInstance2.Center, Vector3.Down * (float) Math.Sign(this.CollisionManager.GravityFactor), this.PlayerManager.Size * FezMath.XZMask / 2f, Direction2D.Vertical)))
            break;
          TrileInstance trileInstance3 = this.PlayerManager.AxisCollision[VerticalDirection.Down].Surface;
          if (trileInstance3 != null && ActorTypeExtensions.IsClimbable(trileInstance3.Trile.ActorSettings.Type) || !trileInstance2.Enabled || this.PlayerManager.Action == ActionType.Jumping && (double) ((trileInstance2.Center - this.PlayerManager.LeaveGroundPosition) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint)).Length() < 1.25)
            break;
          this.PlayerManager.Action = ActionType.GrabLedgeBack;
          Vector3 vector3_1 = FezMath.DepthMask(this.CameraManager.Viewpoint);
          Vector3 vector3_2 = FezMath.SideMask(this.CameraManager.Viewpoint);
          Vector3 vector3_3 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
          this.PlayerManager.HeldInstance = trileInstance2;
          IPlayerManager playerManager = this.PlayerManager;
          Vector3 vector3_4 = playerManager.Velocity * vector3_2 * 0.5f;
          playerManager.Velocity = vector3_4;
          this.PlayerManager.Position = this.PlayerManager.Position * vector3_2 + trileInstance2.Center * (Vector3.UnitY + vector3_1) + vector3_3 * -(this.PlayerManager.HeldInstance.TransformedSize / 2f + this.PlayerManager.Size.X * vector3_1 / 4f) + this.PlayerManager.HeldInstance.Trile.Size.Y / 2f * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor);
          Waiters.Wait(0.1, (Action) (() =>
          {
            SoundEffectExtensions.EmitAt(this.sound, this.PlayerManager.Position);
            this.PlayerManager.Velocity = Vector3.Zero;
          }));
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      this.GomezService.OnGrabLedge();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.rotatedFrom.HasValue && (double) this.CameraManager.ViewTransitionStep >= 0.6)
      {
        int distance = FezMath.GetDistance(this.CameraManager.Viewpoint, this.rotatedFrom.Value);
        if (Math.Abs(distance) % 2 == 0)
        {
          this.PlayerManager.Background = !this.PlayerManager.Background;
          this.PlayerManager.Action = ActionTypeExtensions.FacesBack(this.PlayerManager.Action) ? ActionType.GrabLedgeFront : ActionType.GrabLedgeBack;
        }
        else
        {
          if (ActionTypeExtensions.FacesBack(this.PlayerManager.Action))
            this.PlayerManager.LookingDirection = Math.Sign(distance) > 0 ? HorizontalDirection.Left : HorizontalDirection.Right;
          else
            this.PlayerManager.LookingDirection = Math.Sign(distance) > 0 ? HorizontalDirection.Right : HorizontalDirection.Left;
          this.PlayerManager.Action = ActionType.GrabCornerLedge;
          this.PlayerManager.Position += this.PlayerManager.Size.Z / 4f * FezMath.ForwardVector(this.rotatedFrom.Value);
          this.PlayerManager.Background = false;
        }
        this.SyncAnimation(true);
        this.rotatedFrom = new Viewpoint?();
      }
      base.Update(gameTime);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity * 0.85f;
      playerManager.Velocity = vector3;
      if (this.PlayerManager.HeldInstance.PhysicsState != null && this.CameraManager.ActionRunning)
        this.PlayerManager.Position += this.PlayerManager.HeldInstance.PhysicsState.Velocity;
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.GrabLedgeFront)
        return type == ActionType.GrabLedgeBack;
      else
        return true;
    }
  }
}
