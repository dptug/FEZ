// Type: FezGame.Components.Actions.Carry
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class Carry : PlayerAction
  {
    private static readonly Vector2[] LightWalkOffset = new Vector2[8]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(0.0f, 2f),
      new Vector2(0.0f, 3f),
      new Vector2(0.0f, 2f),
      new Vector2(0.0f, -1f),
      new Vector2(0.0f, 2f),
      new Vector2(0.0f, 3f),
      new Vector2(0.0f, 2f)
    };
    private static readonly Vector2[] HeavyWalkOffset = new Vector2[8]
    {
      new Vector2(0.0f, -1f),
      new Vector2(0.0f, -3f),
      new Vector2(0.0f, -2f),
      new Vector2(0.0f, 0.0f),
      new Vector2(0.0f, -1f),
      new Vector2(0.0f, -3f),
      new Vector2(0.0f, -2f),
      new Vector2(0.0f, 0.0f)
    };
    private static readonly Vector2[] LightJumpOffset = new Vector2[8]
    {
      new Vector2(0.0f, -3f),
      new Vector2(-1f, 3f),
      new Vector2(0.0f, 2f),
      new Vector2(0.0f, -2f),
      new Vector2(0.0f, 0.0f),
      new Vector2(0.0f, 2f),
      new Vector2(0.0f, -3f),
      new Vector2(0.0f, -2f)
    };
    private static readonly Vector2[] HeavyJumpOffset = new Vector2[8]
    {
      new Vector2(-1f, -3f),
      new Vector2(0.0f, 3f),
      new Vector2(0.0f, 2f),
      new Vector2(0.0f, 0.0f),
      new Vector2(0.0f, 0.0f),
      new Vector2(0.0f, 3f),
      new Vector2(-1f, 3f),
      new Vector2(-1f, -3f)
    };
    private readonly MovementHelper movementHelper = new MovementHelper(4.086957f, 0.0f, float.MaxValue);
    private const float CarryJumpStrength = 0.885f;
    private const float CarryWalkSpeed = 4.086957f;
    private const float CarryHeavyWalkSpeed = 2.35f;
    private SoundEffect jumpSound;
    private SoundEffect landSound;
    private bool jumpIsFall;
    private bool wasNotGrounded;

    static Carry()
    {
    }

    public Carry(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.movementHelper.Entity = (IPhysicsEntity) this.PlayerManager;
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.jumpSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Jump");
      this.landSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Land");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.CarryIdle:
        case ActionType.CarryWalk:
        case ActionType.CarryJump:
        case ActionType.CarrySlide:
        case ActionType.CarryHeavyIdle:
        case ActionType.CarryHeavyWalk:
        case ActionType.CarryHeavyJump:
        case ActionType.CarryHeavySlide:
          if (this.PlayerManager.CarriedInstance == null)
            break;
          bool isLight = ActorTypeExtensions.IsLight(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type);
          bool flag1 = this.PlayerManager.Action == ActionType.CarryHeavyJump || this.PlayerManager.Action == ActionType.CarryJump;
          bool flag2 = flag1 && !this.PlayerManager.Animation.Timing.Ended;
          if (this.PlayerManager.Grounded && this.InputManager.Jump == FezButtonState.Pressed && FezButtonStateExtensions.IsDown(this.InputManager.Down) && this.PlayerManager.Ground.First.GetRotatedFace(this.CameraManager.VisibleOrientation) == CollisionType.TopOnly)
          {
            this.PlayerManager.Position -= Vector3.UnitY * this.CollisionManager.DistanceEpsilon * 2f;
            IPlayerManager playerManager = this.PlayerManager;
            Vector3 vector3 = playerManager.Velocity - 0.0075f * Vector3.UnitY;
            playerManager.Velocity = vector3;
            this.PlayerManager.Action = isLight ? ActionType.CarryJump : ActionType.CarryHeavyJump;
            this.PlayerManager.CanDoubleJump = false;
            break;
          }
          else if ((this.PlayerManager.Grounded || this.PlayerManager.CanDoubleJump) && (!flag1 || this.PlayerManager.Animation.Timing.Frame != 0) && this.InputManager.Jump == FezButtonState.Pressed)
          {
            this.jumpIsFall = false;
            this.Jump(isLight);
            break;
          }
          else if (this.PlayerManager.Grounded && (double) this.InputManager.Movement.X != 0.0 && !flag2)
          {
            this.PlayerManager.Action = isLight ? ActionType.CarryWalk : ActionType.CarryHeavyWalk;
            break;
          }
          else
          {
            if (this.PlayerManager.Action != ActionType.CarryHeavyJump && this.PlayerManager.Action != ActionType.CarryJump && !this.PlayerManager.Grounded)
            {
              this.jumpIsFall = true;
              this.PlayerManager.Action = isLight ? ActionType.CarryJump : ActionType.CarryHeavyJump;
            }
            if (this.wasNotGrounded && this.PlayerManager.Grounded)
              SoundEffectExtensions.EmitAt(this.landSound, this.PlayerManager.Position);
            this.wasNotGrounded = !this.PlayerManager.Grounded;
            if (this.PlayerManager.Action != ActionType.CarryIdle && this.PlayerManager.Action != ActionType.CarryHeavyIdle && (this.PlayerManager.Grounded && FezMath.AlmostEqual(FezMath.XZ(this.PlayerManager.Velocity), Vector2.Zero)) && !flag2)
            {
              this.PlayerManager.Action = isLight ? ActionType.CarryIdle : ActionType.CarryHeavyIdle;
              break;
            }
            else
            {
              if (this.PlayerManager.Action == ActionType.CarrySlide || !this.PlayerManager.Grounded || (FezMath.AlmostEqual(FezMath.XZ(this.PlayerManager.Velocity), Vector2.Zero) || !FezMath.AlmostEqual(this.InputManager.Movement, Vector2.Zero)) || flag2)
                break;
              this.PlayerManager.Action = isLight ? ActionType.CarrySlide : ActionType.CarryHeavySlide;
              this.PlayerManager.Animation.Timing.Paused = false;
              break;
            }
          }
      }
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.CarriedInstance == null)
      {
        this.PlayerManager.Action = ActionType.Idle;
        return false;
      }
      else
      {
        bool flag = ActorTypeExtensions.IsLight(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type);
        if (this.PlayerManager.Action == ActionType.CarryWalk || this.PlayerManager.Action == ActionType.CarryHeavyWalk || (this.PlayerManager.Action == ActionType.CarryHeavyJump || this.PlayerManager.Action == ActionType.CarryJump) && this.PlayerManager.Grounded)
        {
          this.movementHelper.WalkAcceleration = flag ? 4.086957f : 2.35f;
          this.movementHelper.Update((float) elapsed.TotalSeconds);
        }
        float timeFactor = 1.2f;
        if (this.PlayerManager.Action == ActionType.CarryJump || this.PlayerManager.Action == ActionType.CarryHeavyJump)
          timeFactor = 1f;
        this.PlayerManager.Animation.Timing.Update(elapsed, timeFactor);
        if (this.PlayerManager.Action == ActionType.CarryJump || this.PlayerManager.Action == ActionType.CarryHeavyJump)
        {
          if (this.PlayerManager.Animation.Timing.Frame == 1 && this.PlayerManager.Grounded && !this.jumpIsFall)
          {
            SoundEffectExtensions.EmitAt(this.jumpSound, this.PlayerManager.Position);
            IPlayerManager playerManager1 = this.PlayerManager;
            Vector3 vector3_1 = playerManager1.Velocity * FezMath.XZMask;
            playerManager1.Velocity = vector3_1;
            IPlayerManager playerManager2 = this.PlayerManager;
            Vector3 vector3_2 = playerManager2.Velocity + 0.13275f * Vector3.UnitY * (flag ? 1f : 0.75f);
            playerManager2.Velocity = vector3_2;
          }
          else
            this.JumpAftertouch((float) elapsed.TotalSeconds);
        }
        this.MoveCarriedInstance();
        return false;
      }
    }

    private void Jump(bool isLight)
    {
      this.PlayerManager.Action = isLight ? ActionType.CarryJump : ActionType.CarryHeavyJump;
      this.PlayerManager.Animation.Timing.Restart();
      this.PlayerManager.CanDoubleJump = false;
    }

    private void JumpAftertouch(float secondsElapsed)
    {
      int frame = this.PlayerManager.Animation.Timing.Frame;
      int num = ActorTypeExtensions.IsHeavy(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type) ? 7 : 6;
      if (!this.PlayerManager.Grounded && (double) this.PlayerManager.Velocity.Y < 0.0)
        this.PlayerManager.Animation.Timing.Step = Math.Max(this.PlayerManager.Animation.Timing.Step, 0.5f);
      if (frame != 0 && frame < num && this.PlayerManager.Grounded)
        this.PlayerManager.Animation.Timing.Step = (float) num / 8f;
      else if (!this.PlayerManager.Grounded && (double) this.PlayerManager.Velocity.Y < 0.0)
        this.PlayerManager.Animation.Timing.Step = Math.Min(this.PlayerManager.Animation.Timing.Step, (float) ((double) num / 8.0 - 1.0 / 1000.0));
      else if (frame < num)
        this.PlayerManager.Animation.Timing.Step = Math.Min(this.PlayerManager.Animation.Timing.Step, 0.499f);
      if (frame == 0 || frame >= num || this.InputManager.Jump != FezButtonState.Down)
        return;
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity + (float) ((double) secondsElapsed * 0.884999990463257 / 4.0) * Vector3.UnitY;
      playerManager.Velocity = vector3;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.CarryWalk && type != ActionType.CarryIdle && (type != ActionType.CarryJump && type != ActionType.CarrySlide) && (type != ActionType.CarryHeavyWalk && type != ActionType.CarryHeavyIdle && type != ActionType.CarryHeavyJump))
        return type == ActionType.CarryHeavySlide;
      else
        return true;
    }

    private void MoveCarriedInstance()
    {
      // ISSUE: unable to decompile the method.
    }
  }
}
