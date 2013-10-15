// Type: FezGame.Components.Actions.FromCornerTransition
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class FromCornerTransition : PlayerAction
  {
    private SoundEffect transitionSound;

    public FromCornerTransition(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.transitionSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LedgeToCorner");
    }

    protected override void Begin()
    {
      this.PlayerManager.Velocity = Vector3.Zero;
      this.PlayerManager.Position = this.PlayerManager.HeldInstance.Center + (-(FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection)) + Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor)) * this.PlayerManager.HeldInstance.TransformedSize / 2f + FezMath.ForwardVector(this.CameraManager.Viewpoint) * -(this.PlayerManager.HeldInstance.TransformedSize / 2f + this.PlayerManager.Size.X * FezMath.DepthMask(this.CameraManager.Viewpoint) / 4f);
      this.PlayerManager.ForceOverlapsDetermination();
      this.PhysicsManager.HugWalls((IPhysicsEntity) this.PlayerManager, false, false, true);
      SoundEffectExtensions.EmitAt(this.transitionSound, this.PlayerManager.Position);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.HeldInstance.PhysicsState != null)
        this.PlayerManager.Position += this.PlayerManager.HeldInstance.PhysicsState.Velocity;
      if (!this.PlayerManager.Animation.Timing.Ended)
        return true;
      this.PlayerManager.Action = ActionType.GrabLedgeBack;
      Vector3 vector3_1 = FezMath.SideMask(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = FezMath.DepthMask(this.CameraManager.Viewpoint);
      Vector3 vector3_3 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      TrileInstance heldInstance = this.PlayerManager.HeldInstance;
      this.PlayerManager.Position = this.PlayerManager.Position * vector3_1 + heldInstance.Center * (Vector3.UnitY + vector3_2) + vector3_3 * (float) -(0.5 + (double) this.PlayerManager.Size.X / 4.0) + Vector3.UnitY * 8f / 16f * (float) Math.Sign(this.CollisionManager.GravityFactor);
      this.PlayerManager.ForceOverlapsDetermination();
      this.PhysicsManager.HugWalls((IPhysicsEntity) this.PlayerManager, false, false, true);
      this.PlayerManager.HeldInstance = this.PlayerManager.AxisCollision[VerticalDirection.Down].Deep;
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.FromCornerBack;
    }
  }
}
