// Type: FezGame.Components.Actions.Slide
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  public class Slide : PlayerAction
  {
    public Slide(Game game)
      : base(game)
    {
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
        case ActionType.Walking:
        case ActionType.Running:
          if (FezMath.AlmostEqual(FezMath.XZ(this.PlayerManager.Velocity), Vector2.Zero) || !FezMath.AlmostEqual(this.InputManager.Movement, Vector2.Zero))
            break;
          this.PlayerManager.Action = ActionType.Sliding;
          break;
      }
    }

    protected override bool Act(TimeSpan elapsed)
    {
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Sliding;
    }
  }
}
