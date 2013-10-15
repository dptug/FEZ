// Type: FezGame.Components.Actions.Bounce
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

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
  public class Bounce : PlayerAction
  {
    private static readonly TimeSpan BounceVibrateTime = TimeSpan.FromSeconds(0.300000011920929);
    private const float BouncerResponse = 0.32f;
    private SoundEffect bounceHigh;
    private SoundEffect bounceLow;

    static Bounce()
    {
    }

    public Bounce(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      this.bounceHigh = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/BounceHigh");
      this.bounceLow = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/BounceLow");
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
        case ActionType.Jumping:
        case ActionType.Falling:
        case ActionType.Dropping:
        case ActionType.Sliding:
        case ActionType.Landing:
          if (!this.PlayerManager.Grounded || this.PlayerManager.Ground.First.Trile.ActorSettings.Type != ActorType.Bouncer)
            break;
          this.PlayerManager.Action = ActionType.Bouncing;
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      this.InputManager.ActiveGamepad.Vibrate(VibrationMotor.LeftLow, 0.5, Bounce.BounceVibrateTime, EasingType.Quadratic);
      this.InputManager.ActiveGamepad.Vibrate(VibrationMotor.RightHigh, 0.600000023841858, Bounce.BounceVibrateTime, EasingType.Quadratic);
      if (RandomHelper.Probability(0.5))
        SoundEffectExtensions.EmitAt(this.bounceHigh, this.PlayerManager.Position);
      else
        SoundEffectExtensions.EmitAt(this.bounceLow, this.PlayerManager.Position);
      IPlayerManager playerManager1 = this.PlayerManager;
      Vector3 vector3_1 = playerManager1.Velocity * new Vector3(1f, 0.0f, 1f);
      playerManager1.Velocity = vector3_1;
      IPlayerManager playerManager2 = this.PlayerManager;
      Vector3 vector3_2 = playerManager2.Velocity + Vector3.UnitY * 0.32f;
      playerManager2.Velocity = vector3_2;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Bouncing;
    }
  }
}
