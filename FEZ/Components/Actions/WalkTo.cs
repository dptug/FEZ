// Type: FezGame.Components.Actions.WalkTo
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  internal class WalkTo : PlayerAction, IWalkToService
  {
    private readonly MovementHelper movementHelper = new MovementHelper(4.7f, 5.875f, 0.2f);
    private HorizontalDirection originalLookingDirection;

    public Func<Vector3> Destination { get; set; }

    public ActionType NextAction { get; set; }

    public WalkTo(Game game)
      : base(game)
    {
    }

    protected override void Begin()
    {
      this.originalLookingDirection = this.PlayerManager.LookingDirection;
      this.movementHelper.Entity = (IPhysicsEntity) this.PlayerManager;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      float timeFactor = this.movementHelper.Running ? 1.25f : 1f;
      this.PlayerManager.Animation.Timing.Update(elapsed, timeFactor);
      this.PlayerManager.LookingDirection = this.originalLookingDirection;
      float num1 = FezMath.Dot(this.Destination() - this.PlayerManager.Position, FezMath.RightVector(this.CameraManager.Viewpoint));
      int num2 = (double) num1 < 0.0 ? -1 : 1;
      this.PlayerManager.LookingDirection = (double) num1 < 0.0 ? HorizontalDirection.Left : HorizontalDirection.Right;
      if (FezMath.AlmostEqual((double) num1, 0.0, 0.01))
      {
        this.ChangeAction();
      }
      else
      {
        this.movementHelper.Update((float) elapsed.TotalSeconds, (float) num2 * 0.75f);
        this.PlayerManager.Velocity = this.PlayerManager.Velocity * (Vector3.UnitY + FezMath.DepthMask(this.CameraManager.Viewpoint)) + FezMath.RightVector(this.CameraManager.Viewpoint) * Math.Min(Math.Abs(FezMath.Dot(this.PlayerManager.Velocity, FezMath.SideMask(this.CameraManager.Viewpoint))), Math.Abs(num1)) * (float) num2;
      }
      return false;
    }

    private void ChangeAction()
    {
      this.PlayerManager.LookingDirection = this.originalLookingDirection;
      this.PlayerManager.Action = this.NextAction;
      this.PlayerManager.Position = this.Destination();
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity * 0.5f;
      playerManager.Velocity = vector3;
      this.Destination = (Func<Vector3>) null;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.WalkingTo;
    }
  }
}
