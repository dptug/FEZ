// Type: FezGame.Components.Actions.ToCornerTransition
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class ToCornerTransition : PlayerAction
  {
    private SoundEffect transitionSound;

    public ToCornerTransition(Game game)
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
      SoundEffectExtensions.EmitAt(this.transitionSound, this.PlayerManager.Position);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      this.PlayerManager.Position = this.PlayerManager.HeldInstance.Center + (-(FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection)) + Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor)) * this.PlayerManager.HeldInstance.TransformedSize / 2f + FezMath.ForwardVector(this.CameraManager.Viewpoint) * -(this.PlayerManager.HeldInstance.TransformedSize / 2f + this.PlayerManager.Size.X * FezMath.DepthMask(this.CameraManager.Viewpoint) / 4f) * (this.PlayerManager.Background ? -1f : 1f);
      if (!this.PlayerManager.Animation.Timing.Ended)
        return true;
      this.PlayerManager.Action = ActionType.GrabCornerLedge;
      this.PlayerManager.Background = false;
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.ToCornerFront)
        return type == ActionType.ToCornerBack;
      else
        return true;
    }
  }
}
