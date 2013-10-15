// Type: FezGame.Components.Actions.ToClimbTransition
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class ToClimbTransition : PlayerAction
  {
    private TimeSpan? sinceGrabbed;
    private SoundEffect grabLadderSound;
    private SoundEffect grabVineSound;

    public ToClimbTransition(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      this.grabLadderSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/GrabLadder");
      this.grabVineSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/GrabVine");
    }

    protected override void Begin()
    {
      this.PlayerManager.Velocity = new Vector3(0.0f, Math.Max(this.PlayerManager.Velocity.Y, 0.0f), 0.0f);
      this.sinceGrabbed = new TimeSpan?(TimeSpan.Zero);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.sinceGrabbed.HasValue)
      {
        ToClimbTransition toClimbTransition = this;
        TimeSpan? nullable1 = toClimbTransition.sinceGrabbed;
        TimeSpan timeSpan = elapsed;
        TimeSpan? nullable2 = nullable1.HasValue ? new TimeSpan?(nullable1.GetValueOrDefault() + timeSpan) : new TimeSpan?();
        toClimbTransition.sinceGrabbed = nullable2;
        if (this.sinceGrabbed.Value.TotalSeconds >= (this.PlayerManager.Action == ActionType.JumpToClimb || this.PlayerManager.Action == ActionType.JumpToSideClimb ? 0.16 : 0.32))
        {
          this.PlayerManager.Velocity = Vector3.Zero;
          if (ActionTypeExtensions.IsClimbingLadder(this.PlayerManager.NextAction))
            SoundEffectExtensions.EmitAt(this.grabLadderSound, this.PlayerManager.Position);
          else if (ActionTypeExtensions.IsClimbingVine(this.PlayerManager.NextAction))
            SoundEffectExtensions.EmitAt(this.grabVineSound, this.PlayerManager.Position);
          this.sinceGrabbed = new TimeSpan?();
        }
      }
      if (ActionTypeExtensions.IsClimbingLadder(this.PlayerManager.NextAction) || this.PlayerManager.NextAction == ActionType.SideClimbingVine)
        this.PlayerManager.Position = Vector3.Lerp(this.PlayerManager.Position, this.PlayerManager.Position * Vector3.UnitY + (this.PlayerManager.HeldInstance.Position + FezMath.HalfVector) * FezMath.XZMask, this.PlayerManager.Animation.Timing.NormalizedStep);
      if ((double) this.PlayerManager.Velocity.Y > 0.0)
      {
        Vector3 vector3_1 = (float) (3.15000009536743 * (double) Math.Sign(this.CollisionManager.GravityFactor) * 0.150000005960464) * (float) elapsed.TotalSeconds * -Vector3.UnitY;
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3_2 = playerManager.Velocity + vector3_1;
        playerManager.Velocity = vector3_2;
      }
      if (!this.PlayerManager.Animation.Timing.Ended)
        return true;
      if (this.PlayerManager.NextAction == ActionType.None)
        throw new InvalidOperationException();
      this.PlayerManager.Action = this.PlayerManager.NextAction;
      this.PlayerManager.NextAction = ActionType.None;
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.IdleToClimb && type != ActionType.JumpToClimb && (type != ActionType.IdleToFrontClimb && type != ActionType.IdleToSideClimb))
        return type == ActionType.JumpToSideClimb;
      else
        return true;
    }
  }
}
