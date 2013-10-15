// Type: FezGame.Components.Actions.Jetpack
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  public class Jetpack : PlayerAction
  {
    private const float JetpackSpeed = 0.075f;

    public Jetpack(Game game)
      : base(game)
    {
    }

    protected override void TestConditions()
    {
      if (!this.GameState.JetpackMode && !this.GameState.DebugMode || (this.InputManager.Jump != FezButtonState.Down || this.PlayerManager.Action == ActionType.FindingTreasure) || (this.PlayerManager.Action == ActionType.OpeningTreasure || this.PlayerManager.Action == ActionType.Suffering || (!(this.LevelManager.Name != "VILLAGEVILLE_2D") || !(this.LevelManager.Name != "ELDERS"))))
        return;
      this.PlayerManager.CarriedInstance = (TrileInstance) null;
      this.PlayerManager.Action = ActionType.Flying;
    }

    protected override void Begin()
    {
      base.Begin();
      this.PlayerManager.CarriedInstance = this.PlayerManager.PushedInstance = (TrileInstance) null;
      this.CameraManager.Constrained = false;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.InputManager.Jump == FezButtonState.Down)
      {
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3 = playerManager.Velocity + (float) (0.150000005960464 * (double) Math.Sign(this.CollisionManager.GravityFactor) * 1.02499997615814) * Vector3.UnitY * 0.075f;
        playerManager.Velocity = vector3;
      }
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Flying;
    }
  }
}
