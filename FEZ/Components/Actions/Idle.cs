// Type: FezGame.Components.Actions.Idle
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
  public class Idle : PlayerAction
  {
    private int lastFrame = -1;
    private TimeSpan changeAnimationIn;
    private ActionType lastSpecialIdle;
    private SoundEmitter lastSpecialIdleSound;
    private SoundEffect sBlink;
    private SoundEffect sYawn;
    private SoundEffect sHatGrab;
    private SoundEffect sHatThrow;
    private SoundEffect sHatCatch;
    private SoundEffect sHatFinalThrow;
    private SoundEffect sHatFallOnHead;
    private SoundEffect sLayDown;
    private SoundEffect sSnore;
    private SoundEffect sWakeUp;
    private SoundEffect sIdleTurnLeft;
    private SoundEffect sIdleTurnRight;
    private SoundEffect sIdleTurnUp;
    private SoundEffect sIdleFaceFront;

    public Idle(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.CameraManager.ViewpointChanged += (Action) (() =>
      {
        if (this.PlayerManager.Action != ActionType.Teetering)
          return;
        this.PlayerManager.Action = ActionType.Idle;
      });
      this.sBlink = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Blink");
      this.sYawn = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Yawn");
      this.sHatGrab = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/HatGrab");
      this.sHatThrow = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/HatThrow");
      this.sHatCatch = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/HatCatch");
      this.sHatFinalThrow = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/HatFinalThrow");
      this.sHatFallOnHead = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/HatFallOnHead");
      this.sLayDown = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/LayDown");
      this.sSnore = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Snore");
      this.sWakeUp = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/WakeUp");
      this.sIdleTurnLeft = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/IdleTurnLeft");
      this.sIdleTurnRight = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/IdleTurnRight");
      this.sIdleTurnUp = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/IdleTurnUp");
      this.sIdleFaceFront = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/IdleFaceFront");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Dropping:
        case ActionType.Sliding:
        case ActionType.Grabbing:
        case ActionType.Pushing:
          if (!FezMath.AlmostEqual(FezMath.XZ(this.PlayerManager.Velocity), Vector2.Zero) || (double) this.InputManager.Movement.X != 0.0 || this.PlayerManager.PushedInstance != null || ((double) this.CollisionManager.GravityFactor < 0.0 ? ((double) this.PlayerManager.Velocity.Y >= 0.0 ? 1 : 0) : ((double) this.PlayerManager.Velocity.Y <= 0.0 ? 1 : 0)) == 0)
            break;
          this.PlayerManager.Action = ActionType.Idle;
          break;
      }
    }

    protected override void Begin()
    {
      this.lastFrame = -1;
      this.ScheduleSpecialIdle();
    }

    private void ScheduleSpecialIdle()
    {
      this.changeAnimationIn = TimeSpan.FromSeconds((double) RandomHelper.Between(7.0, 9.0));
    }

    protected override void End()
    {
      base.End();
      if (this.lastSpecialIdleSound == null || this.lastSpecialIdleSound.Dead)
        return;
      this.lastSpecialIdleSound.FadeOutAndDie(0.1f);
      this.lastSpecialIdleSound = (SoundEmitter) null;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      int num = this.PlayerManager.Animation.Timing.Frame;
      switch (this.PlayerManager.Action)
      {
        case ActionType.IdlePlay:
          if (this.lastFrame != num)
          {
            if (num == 2)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sHatGrab, this.PlayerManager.Position);
            if (num == 6 || num == 13 || num == 20)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sHatThrow, this.PlayerManager.Position);
            if (num == 10 || num == 17 || num == 24)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sHatCatch, this.PlayerManager.Position);
            if (num == 27)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sHatFinalThrow, this.PlayerManager.Position);
            if (num == 31)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sHatFallOnHead, this.PlayerManager.Position);
          }
          if (this.CheckNextIdle())
          {
            num = -1;
            break;
          }
          else
            break;
        case ActionType.IdleSleep:
          if (this.lastFrame != num)
          {
            if (num == 1)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sYawn, this.PlayerManager.Position);
            if (num == 3)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sLayDown, this.PlayerManager.Position);
            if (num == 11 || num == 21 || (num == 31 || num == 41))
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sSnore, this.PlayerManager.Position);
            if (num == 50)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sWakeUp, this.PlayerManager.Position);
            if (num == 51)
              SoundEffectExtensions.EmitAt(this.sBlink, this.PlayerManager.Position);
          }
          if (this.CheckNextIdle())
          {
            num = -1;
            break;
          }
          else
            break;
        case ActionType.IdleLookAround:
          if (this.lastFrame != num)
          {
            if (num == 1)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sIdleTurnLeft, this.PlayerManager.Position);
            if (num == 7)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sIdleTurnRight, this.PlayerManager.Position);
            if (num == 13)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sIdleTurnUp, this.PlayerManager.Position);
            if (num == 19)
              this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sIdleFaceFront, this.PlayerManager.Position);
          }
          if (this.CheckNextIdle())
          {
            num = -1;
            break;
          }
          else
            break;
        case ActionType.IdleYawn:
          if (this.lastFrame != num && num == 0)
            this.lastSpecialIdleSound = SoundEffectExtensions.EmitAt(this.sYawn, this.PlayerManager.Position);
          if (this.CheckNextIdle())
          {
            num = -1;
            break;
          }
          else
            break;
        default:
          if (this.PlayerManager.CanControl)
            this.changeAnimationIn -= elapsed;
          if (!this.GameState.TimePaused && !this.PlayerManager.Hidden && (!this.GameState.FarawaySettings.InTransition && !this.PlayerManager.InDoorTransition) && (this.lastFrame != num && (num == 1 || num == 13)))
            SoundEffectExtensions.EmitAt(this.sBlink, this.PlayerManager.Position);
          if (this.changeAnimationIn.Ticks <= 0L)
          {
            switch (this.lastSpecialIdle)
            {
              case ActionType.None:
              case ActionType.IdlePlay:
                this.PlayerManager.Action = ActionType.IdleYawn;
                break;
              case ActionType.IdleSleep:
                this.PlayerManager.Action = ActionType.IdleLookAround;
                break;
              case ActionType.IdleLookAround:
                if (this.PlayerManager.HideFez)
                {
                  this.PlayerManager.Action = ActionType.IdleYawn;
                  break;
                }
                else
                {
                  this.PlayerManager.Action = ActionType.IdlePlay;
                  break;
                }
              case ActionType.IdleYawn:
                this.PlayerManager.Action = ActionType.IdleSleep;
                break;
            }
            this.lastSpecialIdle = this.PlayerManager.Action;
            num = -1;
            break;
          }
          else
            break;
      }
      this.lastFrame = num;
      return true;
    }

    private bool CheckNextIdle()
    {
      if (!this.PlayerManager.Animation.Timing.Ended)
        return false;
      this.ScheduleSpecialIdle();
      this.PlayerManager.Action = ActionType.Idle;
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.Idle && type != ActionType.IdleSleep && (type != ActionType.IdlePlay && type != ActionType.IdleLookAround))
        return type == ActionType.IdleYawn;
      else
        return true;
    }
  }
}
