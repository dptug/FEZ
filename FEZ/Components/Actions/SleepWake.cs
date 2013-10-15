// Type: FezGame.Components.Actions.SleepWake
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  internal class SleepWake : PlayerAction
  {
    public SleepWake(Game game)
      : base(game)
    {
    }

    protected override void Begin()
    {
      this.PlayerManager.Animation.Timing.Frame = 8;
      base.Begin();
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Animation.Timing.Ended)
        this.PlayerManager.Action = ActionType.Idle;
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.SleepWake;
    }
  }
}
