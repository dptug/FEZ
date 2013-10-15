// Type: FezGame.Components.Actions.FreeFall
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
using System.Collections.Generic;

namespace FezGame.Components.Actions
{
  public class FreeFall : PlayerAction
  {
    private static readonly Dictionary<string, int> EndCaps = new Dictionary<string, int>()
    {
      {
        "INDUSTRIAL_SUPERSPIN",
        0
      },
      {
        "PIVOT_ONE",
        0
      },
      {
        "PIVOT_TWO",
        7
      },
      {
        "INDUSTRIAL_HUB",
        5
      },
      {
        "GRAVE_TREASURE_A",
        0
      },
      {
        "WELL_2",
        0
      },
      {
        "TREE_SKY",
        0
      },
      {
        "PIVOT_THREE_CAVE",
        40
      },
      {
        "ZU_BRIDGE",
        22
      },
      {
        "LIGHTHOUSE_SPIN",
        4
      },
      {
        "FRACTAL",
        0
      },
      {
        "MINE_A",
        0
      },
      {
        "MINE_WRAP",
        4
      },
      {
        "BIG_TOWER",
        0
      },
      {
        "ZU_CITY_RUINS",
        5
      },
      {
        "CODE_MACHINE",
        3
      },
      {
        "TELESCOPE",
        5
      },
      {
        "GLOBE",
        5
      },
      {
        "MEMORY_CORE",
        10
      },
      {
        "ZU_CITY",
        6
      }
    };
    private const float FreeFallStart = 8f;
    private const float FreeFallEnd = 36f;
    private const float CamPanUp = 5f;
    private const float CamFollowEnd = 27f;
    private SoundEffect thudSound;
    private SoundEffect panicSound;
    private SoundEmitter panicEmitter;
    private bool WasConstrained;
    private Vector3 OldConstrainedCenter;
    private int? CapEnd;

    static FreeFall()
    {
    }

    public FreeFall(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.thudSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/CrashLand");
      this.panicSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/AirPanic");
    }

    protected override void TestConditions()
    {
      if (this.PlayerManager.IgnoreFreefall || this.PlayerManager.Grounded || (ActionTypeExtensions.PreventsFall(this.PlayerManager.Action) || this.PlayerManager.Action == ActionType.SuckedIn) || (double) Math.Sign(this.CollisionManager.GravityFactor) * ((double) this.PlayerManager.LeaveGroundPosition.Y - (double) this.PlayerManager.Position.Y) <= 8.0)
        return;
      this.PlayerManager.Action = ActionType.FreeFalling;
    }

    protected override void Begin()
    {
      base.Begin();
      if (this.panicEmitter != null)
      {
        this.panicEmitter.FadeOutAndDie(0.0f);
        this.panicEmitter = (SoundEmitter) null;
      }
      (this.panicEmitter = SoundEffectExtensions.EmitAt(this.panicSound, this.PlayerManager.Position)).NoAttenuation = true;
      if (this.PlayerManager.CarriedInstance != null)
      {
        this.PlayerManager.CarriedInstance.PhysicsState.Velocity = this.PlayerManager.Velocity * 0.95f;
        this.PlayerManager.CarriedInstance = (TrileInstance) null;
      }
      this.WasConstrained = this.CameraManager.Constrained;
      if (this.WasConstrained)
        this.OldConstrainedCenter = this.CameraManager.Center;
      this.CameraManager.Constrained = true;
      int num;
      if (FreeFall.EndCaps.TryGetValue(this.LevelManager.Name, out num))
        this.CapEnd = new int?(num);
      else
        this.CapEnd = new int?();
    }

    protected override void End()
    {
      base.End();
      if (!this.WasConstrained)
        this.CameraManager.Constrained = false;
      else
        this.CameraManager.Center = this.OldConstrainedCenter;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      float num1 = (float) Math.Sign(this.CollisionManager.GravityFactor) * (this.PlayerManager.RespawnPosition.Y - this.PlayerManager.Position.Y);
      float num2 = this.CameraManager.Radius / this.CameraManager.AspectRatio;
      float num3 = 36f;
      if (this.CapEnd.HasValue)
        num3 = Math.Min(36f, this.PlayerManager.RespawnPosition.Y - (float) this.CapEnd.Value);
      if (!this.GameState.SkipFadeOut && (double) num1 < 27.0 && (!this.CapEnd.HasValue || (double) this.CameraManager.Center.Y - (double) num2 / 2.0 > (double) (this.CapEnd.Value + 1)))
        this.CameraManager.Center = this.CameraManager.Center * (FezMath.SideMask(this.CameraManager.Viewpoint) + FezMath.DepthMask(this.CameraManager.Viewpoint)) + (this.PlayerManager.Position.Y - (float) (((double) num1 - 8.0) / 27.0 * 5.0)) * Vector3.UnitY;
      if (this.PlayerManager.Grounded)
      {
        this.panicEmitter.FadeOutAndDie(0.0f);
        this.panicEmitter = (SoundEmitter) null;
        SoundEffectExtensions.EmitAt(this.thudSound, this.PlayerManager.Position).NoAttenuation = true;
        this.InputManager.ActiveGamepad.Vibrate(VibrationMotor.RightHigh, 1.0, TimeSpan.FromSeconds(0.5), EasingType.Quadratic);
        this.InputManager.ActiveGamepad.Vibrate(VibrationMotor.LeftLow, 1.0, TimeSpan.FromSeconds(0.349999994039536));
        this.PlayerManager.Action = ActionType.Dying;
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3 = playerManager.Velocity * Vector3.UnitY;
        playerManager.Velocity = vector3;
      }
      if (!this.GameState.SkipFadeOut && (double) num1 > (double) num3)
      {
        if (!this.WasConstrained)
          this.CameraManager.Constrained = false;
        else
          this.CameraManager.Center = this.OldConstrainedCenter;
        this.PlayerManager.Respawn();
      }
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.FreeFalling;
    }
  }
}
