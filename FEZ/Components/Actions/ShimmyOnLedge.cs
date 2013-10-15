// Type: FezGame.Components.Actions.ShimmyOnLedge
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class ShimmyOnLedge : PlayerAction
  {
    private const float ShimmyingSpeed = 0.15f;
    private SoundEffect shimmySound;
    private int lastFrame;

    public ShimmyOnLedge(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.shimmySound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LedgeShimmy");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.GrabLedgeFront:
        case ActionType.GrabLedgeBack:
          if ((double) this.InputManager.Movement.X == 0.0 || !this.PlayerManager.Animation.Timing.Ended)
            break;
          this.PlayerManager.Action = ActionTypeExtensions.FacesBack(this.PlayerManager.Action) ? ActionType.ShimmyBack : ActionType.ShimmyFront;
          break;
      }
    }

    protected override bool Act(TimeSpan elapsed)
    {
      float num1 = FezMath.Saturate(Math.Abs(3f / (float) this.PlayerManager.Animation.Timing.EndFrame - this.PlayerManager.Animation.Timing.Step)) * 2f;
      int frame = this.PlayerManager.Animation.Timing.Frame;
      if (this.lastFrame != frame)
      {
        if (frame == 2)
          SoundEffectExtensions.EmitAt(this.shimmySound, this.PlayerManager.Position);
        this.lastFrame = frame;
      }
      TrileInstance heldInstance = this.PlayerManager.HeldInstance;
      this.PlayerManager.HeldInstance = this.PlayerManager.AxisCollision[VerticalDirection.Down].Deep;
      bool flag1 = this.PlayerManager.HeldInstance == null;
      Vector3 b = FezMath.ForwardVector(this.CameraManager.Viewpoint) * (this.PlayerManager.Background ? -1f : 1f);
      bool flag2 = this.PlayerManager.HeldInstance != null && this.PlayerManager.HeldInstance.GetRotatedFace(FezMath.VisibleOrientation(this.CameraManager.Viewpoint)) == CollisionType.None;
      if (flag1 || flag2 && (double) FezMath.Dot(this.PlayerManager.HeldInstance.Position, b) > (double) FezMath.Dot(heldInstance.Position, b))
      {
        this.PlayerManager.Action = ActionTypeExtensions.FacesBack(this.PlayerManager.Action) ? ActionType.ToCornerBack : ActionType.ToCornerFront;
        this.PlayerManager.HeldInstance = heldInstance;
        this.PlayerManager.LookingDirection = FezMath.GetOpposite(this.PlayerManager.LookingDirection);
        return false;
      }
      else if (flag2)
      {
        this.PlayerManager.Action = ActionType.Dropping;
        this.PlayerManager.HeldInstance = (TrileInstance) null;
        return false;
      }
      else
      {
        float num2 = (float) ((double) this.InputManager.Movement.X * 4.69999980926514 * 0.150000005960464) * (float) elapsed.TotalSeconds;
        if (this.PlayerManager.Action != ActionType.ShimmyBack && this.PlayerManager.Action != ActionType.ShimmyFront)
          num2 *= 0.6f;
        this.PlayerManager.Velocity = num2 * FezMath.RightVector(this.CameraManager.Viewpoint) * (1f + num1);
        if ((double) this.InputManager.Movement.X == 0.0)
          this.PlayerManager.Action = ActionTypeExtensions.FacesBack(this.PlayerManager.Action) ? ActionType.GrabLedgeBack : ActionType.GrabLedgeFront;
        else
          this.PlayerManager.GroundedVelocity = new Vector3?(this.PlayerManager.Velocity);
        if (this.InputManager.RotateLeft == FezButtonState.Pressed || this.InputManager.RotateRight == FezButtonState.Pressed)
          this.PlayerManager.Action = ActionTypeExtensions.FacesBack(this.PlayerManager.Action) ? ActionType.GrabLedgeBack : ActionType.GrabLedgeFront;
        if (this.PlayerManager.Action == ActionType.ShimmyBack || this.PlayerManager.Action == ActionType.ShimmyFront)
        {
          this.PlayerManager.Animation.Timing.Update(elapsed, Math.Abs(this.InputManager.Movement.X));
          if (this.PlayerManager.HeldInstance.PhysicsState != null)
            this.PlayerManager.Position += this.PlayerManager.HeldInstance.PhysicsState.Velocity;
        }
        Vector3 vector3 = FezMath.DepthMask(this.CameraManager.Viewpoint);
        this.PlayerManager.Position = this.PlayerManager.Position * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint) + this.PlayerManager.HeldInstance.Center * vector3 + b * -(this.PlayerManager.HeldInstance.TransformedSize / 2f + this.PlayerManager.Size.X * vector3 / 4f);
        this.PhysicsManager.HugWalls((IPhysicsEntity) this.PlayerManager, false, false, true);
        return false;
      }
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type == ActionType.ShimmyFront || type == ActionType.ShimmyBack)
        return true;
      if (type == ActionType.GrabLedgeBack || type == ActionType.GrabLedgeFront)
        return (double) this.InputManager.Movement.X != 0.0;
      else
        return false;
    }
  }
}
