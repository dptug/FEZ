// Type: FezGame.Components.Actions.WalkRun
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class WalkRun : PlayerAction
  {
    public static readonly MovementHelper MovementHelper = new MovementHelper(4.7f, 5.875f, 0.2f);
    public const float SecondsBeforeRun = 0.2f;
    public const float RunAcceleration = 1.25f;
    private int initialMovement;
    private SoundEffect turnAroundSound;

    static WalkRun()
    {
    }

    public WalkRun(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      WalkRun.MovementHelper.Entity = (IPhysicsEntity) this.PlayerManager;
      this.turnAroundSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/TurnAround");
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
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
        case ActionType.Sliding:
        case ActionType.Grabbing:
        case ActionType.Pushing:
          if (this.PlayerManager.Action == ActionType.Sliding && this.PlayerManager.LastAction == ActionType.Running && this.TestForTurn())
            break;
          if (this.PlayerManager.Grounded && (double) this.InputManager.Movement.X != 0.0 && this.PlayerManager.PushedInstance == null)
          {
            this.PlayerManager.Action = ActionType.Walking;
            break;
          }
          else
          {
            WalkRun.MovementHelper.Reset();
            break;
          }
        case ActionType.Walking:
        case ActionType.Running:
          this.TestForTurn();
          break;
      }
    }

    private bool TestForTurn()
    {
      int num = Math.Sign(this.InputManager.Movement.X);
      if (num == 0 || num == FezMath.Sign(this.PlayerManager.LookingDirection))
        return false;
      this.initialMovement = num;
      this.PlayerManager.Action = ActionType.RunTurnAround;
      SoundEffectExtensions.EmitAt(this.turnAroundSound, this.PlayerManager.Position);
      return true;
    }

    protected override void Begin()
    {
      WalkRun.MovementHelper.Reset();
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Action == ActionType.RunTurnAround)
      {
        if (Math.Sign(this.InputManager.Movement.X) != this.initialMovement)
        {
          this.PlayerManager.LookingDirection = FezMath.GetOpposite(this.PlayerManager.LookingDirection);
          this.PlayerManager.Action = ActionType.Idle;
          return false;
        }
        else if (this.PlayerManager.Animation.Timing.Ended)
        {
          this.PlayerManager.LookingDirection = FezMath.GetOpposite(this.PlayerManager.LookingDirection);
          this.PlayerManager.Action = ActionType.Running;
          return false;
        }
        else
          this.PlayerManager.Animation.Timing.Update(elapsed, (float) ((1.0 + (double) Math.Abs(this.CollisionManager.GravityFactor)) / 2.0));
      }
      else if (this.PlayerManager.Action != ActionType.Landing)
      {
        float num;
        if (WalkRun.MovementHelper.Running)
        {
          bool flag = this.PlayerManager.Action == ActionType.Walking;
          this.PlayerManager.Action = ActionType.Running;
          this.SyncAnimation(true);
          if (flag)
            this.PlayerManager.Animation.Timing.Frame = 1;
          num = 1.25f;
        }
        else
        {
          this.PlayerManager.Action = ActionType.Walking;
          num = Easing.EaseOut((double) Math.Min(1f, Math.Abs(this.InputManager.Movement.X) * 2f), EasingType.Cubic);
        }
        this.PlayerManager.Animation.Timing.Update(elapsed, (float) ((double) num * (1.0 + (double) Math.Abs(this.CollisionManager.GravityFactor)) / 2.0));
      }
      WalkRun.MovementHelper.Update((float) elapsed.TotalSeconds);
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.Running && type != ActionType.Landing && type != ActionType.Walking)
        return type == ActionType.RunTurnAround;
      else
        return true;
    }
  }
}
