// Type: FezGame.Components.Actions.Throw
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
  public class Throw : PlayerAction
  {
    private static readonly Vector2[] LightOffsets = new Vector2[2]
    {
      new Vector2(-1f, -1f),
      new Vector2(1f, 2f)
    };
    private static readonly Vector2[] HeavyOffsets = new Vector2[4]
    {
      new Vector2(1f, 1f),
      new Vector2(1f, 0.0f),
      new Vector2(2f, 0.0f),
      new Vector2(7f, 4f)
    };
    private const float ThrowStrength = 0.08f;
    private bool thrown;
    private SoundEffect throwHeavySound;
    private SoundEffect throwLightSound;

    static Throw()
    {
    }

    public Throw(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.throwHeavySound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/ThrowHeavyPickup");
      this.throwLightSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/ThrowLightPickup");
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
          if (this.PlayerManager.Background || this.InputManager.GrabThrow != FezButtonState.Pressed || this.InputManager.Down == FezButtonState.Down && !FezMath.AlmostEqual(this.InputManager.Movement, Vector2.Zero, 0.5f))
            break;
          this.PlayerManager.Action = ActorTypeExtensions.IsLight(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type) ? ActionType.Throwing : ActionType.ThrowingHeavy;
          this.thrown = false;
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      if (this.PlayerManager.CarriedInstance == null)
      {
        this.PlayerManager.Action = ActionType.Idle;
      }
      else
      {
        if (ActorTypeExtensions.IsHeavy(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type))
          SoundEffectExtensions.EmitAt(this.throwHeavySound, this.PlayerManager.Position);
        else
          SoundEffectExtensions.EmitAt(this.throwLightSound, this.PlayerManager.Position);
        this.GomezService.OnThrowObject();
      }
    }

    protected override bool Act(TimeSpan elapsed)
    {
      Vector3 vector3_1 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
      this.PlayerManager.Animation.Timing.Update(elapsed);
      if (this.PlayerManager.CarriedInstance != null)
      {
        if (this.PlayerManager.CarriedInstance.PhysicsState == null)
          this.PlayerManager.CarriedInstance = (TrileInstance) null;
        bool flag = ActorTypeExtensions.IsLight(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type);
        Vector2[] vector2Array = flag ? Throw.LightOffsets : Throw.HeavyOffsets;
        if (!flag)
        {
          IPlayerManager playerManager = this.PlayerManager;
          Vector3 vector3_2 = playerManager.Velocity * Vector3.UnitY;
          playerManager.Velocity = vector3_2;
        }
        if (this.PlayerManager.Animation.Timing.Frame < vector2Array.Length)
        {
          int frame = this.PlayerManager.Animation.Timing.Frame;
          TrileInstance carriedInstance = this.PlayerManager.CarriedInstance;
          Vector2 vector2 = vector2Array[frame];
          Vector3 vector3_2 = this.PlayerManager.Center + this.PlayerManager.Size / 2f * (Vector3.Down + vector3_1) - carriedInstance.TransformedSize / 2f * vector3_1 + carriedInstance.Trile.Size / 2f * (Vector3.UnitY + vector3_1) - vector3_1 * 8f / 16f + Vector3.UnitY * 9f / 16f;
          if (flag)
            vector3_2 += vector3_1 * 1f / 16f + Vector3.UnitY * 2f / 16f;
          Vector3 vector3_3 = vector3_2 + (vector2.X * vector3_1 + vector2.Y * Vector3.Up) * (1.0 / 16.0);
          Vector3 vector3_4 = vector3_3 - carriedInstance.Center;
          carriedInstance.PhysicsState.Velocity = vector3_4;
          carriedInstance.PhysicsState.UpdatingPhysics = true;
          this.PhysicsManager.Update((ISimplePhysicsEntity) carriedInstance.PhysicsState, false, false);
          carriedInstance.PhysicsState.UpdatingPhysics = false;
          carriedInstance.PhysicsState.UpdateInstance();
          carriedInstance.PhysicsState.Velocity = Vector3.Zero;
          IPlayerManager playerManager = this.PlayerManager;
          Vector3 vector3_5 = playerManager.Velocity - vector3_3 - carriedInstance.Center;
          playerManager.Velocity = vector3_5;
        }
        else if (!this.thrown)
        {
          this.thrown = true;
          this.PlayerManager.CarriedInstance.Phi = FezMath.SnapPhi(this.PlayerManager.CarriedInstance.Phi);
          this.PlayerManager.CarriedInstance.PhysicsState.Background = false;
          this.PlayerManager.CarriedInstance.PhysicsState.Velocity = this.PlayerManager.Velocity * 0.5f + ((float) FezMath.Sign(this.PlayerManager.LookingDirection) * FezMath.RightVector(this.CameraManager.Viewpoint) + Vector3.Up) * 0.08f;
          this.PlayerManager.CarriedInstance = (TrileInstance) null;
        }
      }
      if (this.PlayerManager.Animation.Timing.Ended)
      {
        this.thrown = false;
        this.PlayerManager.SyncCollisionSize();
        this.PlayerManager.Action = ActionType.Idle;
      }
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.Throwing)
        return type == ActionType.ThrowingHeavy;
      else
        return true;
    }
  }
}
