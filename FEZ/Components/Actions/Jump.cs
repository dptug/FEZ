// Type: FezGame.Components.Actions.Jump
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
using System.Collections.Generic;

namespace FezGame.Components.Actions
{
  public class Jump : PlayerAction
  {
    public const float SideJumpStrength = 0.25f;
    public const float UpJumpStrength = 1.025f;
    private SoundEffect jumpSound;
    private TimeSpan sinceJumped;
    private bool scheduleJump;

    public Jump(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.jumpSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Jump");
    }

    protected override void TestConditions()
    {
      if (!FezMath.In<ActionType>(this.PlayerManager.Action, ActionType.Sliding, ActionType.GrabCornerLedge, ActionType.Running, ActionType.RunTurnAround, ActionType.Walking, ActionType.Landing, ActionType.WalkingTo, ActionType.GrabTombstone, (IEqualityComparer<ActionType>) ActionTypeComparer.Default) && !this.PlayerManager.Climbing && (!this.PlayerManager.Swimming && !ActionTypeExtensions.IsIdle(this.PlayerManager.Action)) && (this.PlayerManager.Action != ActionType.Falling || !this.PlayerManager.CanDoubleJump) && (this.PlayerManager.Action != ActionType.Grabbing && this.PlayerManager.Action != ActionType.Pushing && !ActionTypeExtensions.IsLookingAround(this.PlayerManager.Action)) || this.InputManager.Jump != FezButtonState.Pressed && (!this.PlayerManager.Grounded && !ActionTypeExtensions.IsOnLedge(this.PlayerManager.Action) || (double) this.PlayerManager.Velocity.Y * (double) Math.Sign(this.CollisionManager.GravityFactor) <= 0.1))
        return;
      this.PlayerManager.PushedInstance = (TrileInstance) null;
      if (this.PlayerManager.CanDoubleJump)
        this.PlayerManager.CanDoubleJump = false;
      if (FezButtonStateExtensions.IsDown(this.InputManager.Down) && (this.PlayerManager.Grounded && this.PlayerManager.Ground.First.GetRotatedFace(FezMath.VisibleOrientation(this.CameraManager.Viewpoint)) == CollisionType.TopOnly || this.PlayerManager.Climbing))
        return;
      if (this.PlayerManager.Action == ActionType.GrabCornerLedge)
      {
        HorizontalDirection horizontalDirection = FezMath.DirectionFromMovement(this.InputManager.Movement.X);
        if (horizontalDirection == HorizontalDirection.None || horizontalDirection == this.PlayerManager.LookingDirection)
          return;
        Vector3 position = this.PlayerManager.Position;
        this.PlayerManager.Position += FezMath.RightVector(this.CameraManager.Viewpoint) * (float) -FezMath.Sign(this.PlayerManager.LookingDirection);
        this.PhysicsManager.DetermineInBackground((IPhysicsEntity) this.PlayerManager, true, false, false);
        this.PlayerManager.Position = position;
      }
      if (this.InputManager.Jump == FezButtonState.Pressed)
      {
        this.sinceJumped = TimeSpan.Zero;
        this.scheduleJump = true;
      }
      else
        this.DoJump();
      this.PlayerManager.Action = ActionType.Jumping;
    }

    private void DoJump()
    {
      bool flag = ActionTypeExtensions.IsClimbingLadder(this.PlayerManager.LastAction) || ActionTypeExtensions.IsClimbingVine(this.PlayerManager.LastAction) || this.PlayerManager.HeldInstance != null;
      if (flag)
      {
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3 = playerManager.Velocity + FezMath.RightVector(this.CameraManager.Viewpoint) * this.InputManager.Movement.X * 0.25f;
        playerManager.Velocity = vector3;
      }
      this.PlayerManager.HeldInstance = (TrileInstance) null;
      if (this.scheduleJump || this.InputManager.Jump == FezButtonState.Pressed)
        SoundEffectExtensions.EmitAt(this.jumpSound, this.PlayerManager.Position);
      float gravityFactor = this.CollisionManager.GravityFactor;
      float num = (float) ((1.32500004768372 + (double) Math.Abs(gravityFactor) * 0.675000011920929) / 2.0) * (float) Math.Sign(gravityFactor);
      IPlayerManager playerManager1 = this.PlayerManager;
      Vector3 vector3_1 = playerManager1.Velocity * FezMath.XZMask;
      playerManager1.Velocity = vector3_1;
      Vector3 vector3_2 = (float) (0.150000005960464 * (double) num * 1.02499997615814 * (flag || this.PlayerManager.Swimming ? 0.774999976158142 : 1.0)) * Vector3.UnitY;
      IPlayerManager playerManager2 = this.PlayerManager;
      Vector3 vector3_3 = playerManager2.Velocity + vector3_2;
      playerManager2.Velocity = vector3_3;
      this.sinceJumped = TimeSpan.Zero;
      this.GomezService.OnJump();
    }

    protected override bool Act(TimeSpan elapsed)
    {
      this.sinceJumped += elapsed;
      if (this.scheduleJump && this.sinceJumped.TotalMilliseconds >= 60.0)
      {
        this.DoJump();
        this.scheduleJump = false;
      }
      if (this.PlayerManager.Grounded)
        WalkRun.MovementHelper.Update((float) elapsed.TotalSeconds);
      else if (this.InputManager.Jump == FezButtonState.Down)
      {
        float num1 = this.sinceJumped.TotalSeconds < 0.25 ? 0.6f : 0.0f;
        float gravityFactor = this.CollisionManager.GravityFactor;
        float num2 = (float) ((1.20000004768372 + (double) Math.Abs(gravityFactor) * 0.800000011920929) / 2.0) * (float) Math.Sign(gravityFactor);
        Vector3 vector3_1 = (float) (elapsed.TotalSeconds * (double) num1 * (double) num2 * 1.02499997615814 / 2.0) * Vector3.UnitY;
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3_2 = playerManager.Velocity + vector3_1;
        playerManager.Velocity = vector3_2;
      }
      if (((double) this.CollisionManager.GravityFactor < 0.0 ? ((double) this.PlayerManager.Velocity.Y >= 0.0 ? 1 : 0) : ((double) this.PlayerManager.Velocity.Y <= 0.0 ? 1 : 0)) != 0 && !this.PlayerManager.Grounded && !this.scheduleJump)
        this.PlayerManager.Action = ActionType.Falling;
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Jumping;
    }
  }
}
