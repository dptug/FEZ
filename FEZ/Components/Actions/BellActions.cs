// Type: FezGame.Components.Actions.BellActions
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  internal class BellActions : PlayerAction
  {
    protected override bool ViewTransitionIndependent
    {
      get
      {
        return true;
      }
    }

    public BellActions(Game game)
      : base(game)
    {
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Action == ActionType.TurnToBell && this.PlayerManager.Animation.Timing.Ended)
      {
        this.PlayerManager.Action = ActionType.HitBell;
        this.PlayerManager.Animation.Timing.Restart();
      }
      if (this.PlayerManager.Action == ActionType.HitBell && this.PlayerManager.Animation.Timing.Ended)
      {
        this.PlayerManager.Action = ActionType.TurnAwayFromBell;
        this.PlayerManager.Animation.Timing.Restart();
      }
      if (this.PlayerManager.Action == ActionType.TurnAwayFromBell && this.PlayerManager.Animation.Timing.Ended)
      {
        this.PlayerManager.Action = ActionType.Idle;
        this.PlayerManager.Animation.Timing.Restart();
      }
      this.PlayerManager.Background = false;
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.TurnAwayFromBell && type != ActionType.HitBell)
        return type == ActionType.TurnToBell;
      else
        return true;
    }
  }
}
