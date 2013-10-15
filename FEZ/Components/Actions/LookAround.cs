// Type: FezGame.Components.Actions.LookAround
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
  public class LookAround : PlayerAction
  {
    private ActionType nextAction;
    private SoundEffect rightSound;
    private SoundEffect leftSound;
    private SoundEffect upSound;
    private SoundEffect downSound;

    public LookAround(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.rightSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LookRight");
      this.leftSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LookLeft");
      this.upSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LookUp");
      this.downSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LookDown");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.IdlePlay:
        case ActionType.IdleSleep:
        case ActionType.IdleLookAround:
        case ActionType.IdleYawn:
        case ActionType.Idle:
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
        case ActionType.Teetering:
          if (this.PlayerManager.CanControl)
          {
            if ((double) this.InputManager.FreeLook.Y < -0.4)
              this.nextAction = ActionType.LookingDown;
            else if ((double) this.InputManager.FreeLook.Y > 0.4)
              this.nextAction = ActionType.LookingUp;
            else if ((double) this.InputManager.FreeLook.X < -0.4)
              this.nextAction = ActionType.LookingLeft;
            else if ((double) this.InputManager.FreeLook.X > 0.4)
              this.nextAction = ActionType.LookingRight;
            else if (FezMath.AlmostEqual(this.InputManager.FreeLook, Vector2.Zero))
              this.nextAction = ActionType.Idle;
          }
          else
            this.nextAction = ActionTypeExtensions.IsLookingAround(this.PlayerManager.Action) ? this.PlayerManager.Action : ActionType.Idle;
          if (this.PlayerManager.LookingDirection == HorizontalDirection.Left && (this.nextAction == ActionType.LookingLeft || this.nextAction == ActionType.LookingRight))
            this.nextAction = this.nextAction == ActionType.LookingRight ? ActionType.LookingLeft : ActionType.LookingRight;
          if (ActionTypeExtensions.IsIdle(this.PlayerManager.Action) && this.nextAction != ActionType.None && this.nextAction != ActionType.Idle)
          {
            this.PlaySound();
            this.PlayerManager.Action = this.nextAction;
            this.nextAction = ActionType.None;
          }
          if (this.nextAction != this.PlayerManager.Action)
            break;
          this.nextAction = ActionType.None;
          break;
        default:
          this.nextAction = ActionType.None;
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      if (!this.PlayerManager.CanControl)
        return;
      this.GomezService.OnLookAround();
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if ((double) this.PlayerManager.Animation.Timing.NormalizedStep <= 0.55)
        this.PlayerManager.Animation.Timing.Update(elapsed);
      else if (this.nextAction != ActionType.None)
        this.PlayerManager.Animation.Timing.Update(elapsed, 1.25f);
      if (this.PlayerManager.Animation.Timing.Ended && this.nextAction != ActionType.None)
      {
        this.PlaySound();
        this.PlayerManager.Action = this.nextAction;
        this.nextAction = ActionType.None;
      }
      return false;
    }

    private void PlaySound()
    {
      switch (this.nextAction)
      {
        case ActionType.LookingLeft:
          SoundEffectExtensions.EmitAt(this.leftSound, this.PlayerManager.Position);
          break;
        case ActionType.LookingRight:
          SoundEffectExtensions.EmitAt(this.rightSound, this.PlayerManager.Position);
          break;
        case ActionType.LookingUp:
          SoundEffectExtensions.EmitAt(this.upSound, this.PlayerManager.Position);
          break;
        case ActionType.LookingDown:
          SoundEffectExtensions.EmitAt(this.downSound, this.PlayerManager.Position);
          break;
      }
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.LookingDown && type != ActionType.LookingLeft && type != ActionType.LookingRight)
        return type == ActionType.LookingUp;
      else
        return true;
    }
  }
}
