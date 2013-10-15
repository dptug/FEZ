// Type: FezGame.Components.Actions.EnterTunnel
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  internal class EnterTunnel : PlayerAction
  {
    private SoundEffect SwooshRight;
    private Vector3 originalForward;
    private Vector3 originalPosition;
    private float distanceToCover;

    protected override bool ViewTransitionIndependent
    {
      get
      {
        return true;
      }
    }

    public EnterTunnel(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.SwooshRight = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/RotateCenter");
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
        case ActionType.CarryIdle:
        case ActionType.CarryWalk:
        case ActionType.CarrySlide:
        case ActionType.CarryHeavyIdle:
        case ActionType.CarryHeavyWalk:
        case ActionType.CarryHeavySlide:
        case ActionType.Sliding:
          if (!this.PlayerManager.TunnelVolume.HasValue || this.InputManager.ExactUp != FezButtonState.Pressed)
            break;
          if (this.PlayerManager.CarriedInstance == null)
          {
            this.WalkTo.Destination = new Func<Vector3>(this.GetDestination);
            this.PlayerManager.Action = ActionType.WalkingTo;
            this.WalkTo.NextAction = ActionType.EnteringTunnel;
            break;
          }
          else
          {
            Vector3 position = this.PlayerManager.Position;
            this.PlayerManager.Position = this.GetDestination();
            this.PlayerManager.CarriedInstance.Position += this.PlayerManager.Position - position;
            this.PlayerManager.Action = ActorTypeExtensions.IsHeavy(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type) ? ActionType.EnterTunnelCarryHeavy : ActionType.EnterTunnelCarry;
            break;
          }
      }
    }

    private Vector3 GetDestination()
    {
      if (!this.PlayerManager.TunnelVolume.HasValue)
        return this.PlayerManager.Position;
      Volume volume = this.LevelManager.Volumes[this.PlayerManager.TunnelVolume.Value];
      return this.PlayerManager.Position * (Vector3.UnitY + FezMath.DepthMask(this.CameraManager.Viewpoint)) + (volume.From + volume.To) / 2f * FezMath.SideMask(this.CameraManager.Viewpoint);
    }

    protected override void Begin()
    {
      SoundEffectExtensions.Emit(this.SwooshRight);
      this.originalForward = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, 2));
      this.PlayerManager.LookingDirection = HorizontalDirection.Right;
      this.PlayerManager.Velocity = Vector3.Zero;
      this.originalPosition = this.PlayerManager.Position;
      this.distanceToCover = FezMath.Dot(this.LevelManager.NearestTrile(this.PlayerManager.Ground.First.Center, QueryOptions.None, new Viewpoint?(this.CameraManager.Viewpoint)).Deep.Center - this.originalPosition, this.originalForward);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if ((double) this.CameraManager.ViewTransitionStep != 0.0)
      {
        Vector3 position = this.PlayerManager.Position;
        this.PlayerManager.Position = this.originalPosition + Vector3.Lerp(Vector3.Zero, this.originalForward * this.distanceToCover, this.CameraManager.ViewTransitionStep);
        if (this.PlayerManager.CarriedInstance != null)
          this.PlayerManager.CarriedInstance.Position += this.PlayerManager.Position - position;
      }
      if (!this.CameraManager.ActionRunning)
        return true;
      this.PlayerManager.Action = this.PlayerManager.Action == ActionType.EnteringTunnel ? ActionType.Idle : (this.PlayerManager.Action == ActionType.EnterTunnelCarry ? ActionType.CarryIdle : ActionType.CarryHeavyIdle);
      this.PlayerManager.Background = false;
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (this.PlayerManager.Action != ActionType.EnteringTunnel && this.PlayerManager.Action != ActionType.EnterTunnelCarry)
        return this.PlayerManager.Action == ActionType.EnterTunnelCarryHeavy;
      else
        return true;
    }
  }
}
