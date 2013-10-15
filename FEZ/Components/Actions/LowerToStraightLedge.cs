// Type: FezGame.Components.Actions.LowerToStraightLedge
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
  public class LowerToStraightLedge : PlayerAction
  {
    private Vector3 camOrigin;
    private SoundEffect sound;
    private SoundEffect sLowerToLedge;

    public LowerToStraightLedge(Game game)
      : base(game)
    {
      this.UpdateOrder = 3;
    }

    protected override void LoadContent()
    {
      this.sound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LedgeGrab");
      this.sLowerToLedge = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LowerToLedge");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.Idle:
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Sliding:
        case ActionType.Landing:
          if (this.PlayerManager.Background || (this.InputManager.Jump != FezButtonState.Pressed || !FezButtonStateExtensions.IsDown(this.InputManager.Down)) && (!ActionTypeExtensions.IsOnLedge(this.PlayerManager.Action) || this.InputManager.Down != FezButtonState.Pressed) || !this.PlayerManager.Grounded || (this.PlayerManager.Ground.First.GetRotatedFace(FezMath.VisibleOrientation(this.CameraManager.Viewpoint)) != CollisionType.TopOnly || !FezMath.AlmostEqual(this.InputManager.Movement.X, 0.0f)))
            break;
          TrileInstance trileInstance1 = this.PlayerManager.Ground.NearLow ?? this.PlayerManager.Ground.FarHigh;
          if (BoxCollisionResultExtensions.AnyHit(this.CollisionManager.CollideEdge(trileInstance1.Center + trileInstance1.TransformedSize * Vector3.UnitY * 0.498f, Vector3.Down * (float) Math.Sign(this.CollisionManager.GravityFactor), this.PlayerManager.Size * FezMath.XZMask / 2f, Direction2D.Vertical)))
          {
            this.PlayerManager.Position -= Vector3.UnitY * 0.01f * (float) Math.Sign(this.CollisionManager.GravityFactor);
            IPlayerManager playerManager = this.PlayerManager;
            Vector3 vector3 = playerManager.Velocity - 0.0075f * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor);
            playerManager.Velocity = vector3;
            break;
          }
          else
          {
            if (this.PlayerManager.Grounded)
            {
              TrileInstance trileInstance2 = this.LevelManager.NearestTrile(this.PlayerManager.Ground.First.Center, QueryOptions.None).Surface;
              if (trileInstance2 != null && trileInstance2.Trile.ActorSettings.Type == ActorType.Ladder)
                break;
            }
            this.PlayerManager.HeldInstance = this.PlayerManager.Ground.NearLow;
            Vector3 vector3_1 = FezMath.SideMask(this.CameraManager.Viewpoint);
            IPlayerManager playerManager = this.PlayerManager;
            Vector3 vector3_2 = playerManager.Velocity * vector3_1 * 0.5f;
            playerManager.Velocity = vector3_2;
            this.PlayerManager.Action = ActionType.LowerToLedge;
            Waiters.Wait(0.3, (Action) (() =>
            {
              if (this.PlayerManager.Action != ActionType.LowerToLedge)
                return;
              SoundEffectExtensions.EmitAt(this.sound, this.PlayerManager.Position);
              this.PlayerManager.Velocity = Vector3.Zero;
            }));
            this.camOrigin = this.CameraManager.Center;
            break;
          }
      }
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.HeldInstance == null)
      {
        this.PlayerManager.Action = ActionType.Idle;
        return false;
      }
      else
      {
        if (this.PlayerManager.HeldInstance.PhysicsState != null)
          this.camOrigin += this.PlayerManager.HeldInstance.PhysicsState.Velocity;
        Vector3 vector3_1 = FezMath.SideMask(this.CameraManager.Viewpoint);
        Vector3 vector3_2 = FezMath.DepthMask(this.CameraManager.Viewpoint);
        Vector3 vector3_3 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
        this.PlayerManager.Position = this.PlayerManager.Position * vector3_1 + this.PlayerManager.HeldInstance.Center * (Vector3.UnitY + vector3_2) + vector3_3 * (float) -(0.5 + (double) this.PlayerManager.Size.X / 2.0) + this.PlayerManager.HeldInstance.Trile.Size.Y / 2f * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor);
        this.PlayerManager.Position = this.PlayerManager.Position * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint) + this.PlayerManager.HeldInstance.Center * vector3_2 + vector3_3 * -(this.PlayerManager.HeldInstance.TransformedSize / 2f + this.PlayerManager.Size.X * vector3_2 / 4f);
        this.PhysicsManager.HugWalls((IPhysicsEntity) this.PlayerManager, false, false, true);
        Vector3 vector3_4 = this.PlayerManager.Size.Y / 2f * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor);
        if (!this.CameraManager.StickyCam && !this.CameraManager.Constrained)
          this.CameraManager.Center = Vector3.Lerp(this.camOrigin, this.camOrigin - vector3_4, this.PlayerManager.Animation.Timing.NormalizedStep);
        this.PlayerManager.SplitUpCubeCollectorOffset = vector3_4 * (1f - this.PlayerManager.Animation.Timing.NormalizedStep);
        if (!this.PlayerManager.Animation.Timing.Ended)
          return true;
        this.PlayerManager.SplitUpCubeCollectorOffset = Vector3.Zero;
        this.PlayerManager.Action = ActionType.GrabLedgeBack;
        return false;
      }
    }

    protected override void Begin()
    {
      SoundEffectExtensions.EmitAt(this.sLowerToLedge, this.PlayerManager.Position);
      base.Begin();
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.LowerToLedge;
    }
  }
}
