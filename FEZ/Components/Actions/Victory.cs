// Type: FezGame.Components.Actions.Victory
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  public class Victory : PlayerAction
  {
    private static readonly TimeSpan HappyTime = TimeSpan.FromSeconds(2.0);
    private TimeSpan sinceActive;

    static Victory()
    {
    }

    public Victory(Game game)
      : base(game)
    {
    }

    protected override void Begin()
    {
      this.sinceActive = TimeSpan.Zero;
      this.PlayerManager.Velocity = new Vector3(0.0f, 0.05f, 0.0f);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Action != ActionType.VictoryForever)
      {
        this.sinceActive += elapsed;
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3 = playerManager.Velocity * 0.95f;
        playerManager.Velocity = vector3;
        if (this.sinceActive.Ticks >= Victory.HappyTime.Ticks)
          this.PlayerManager.Action = ActionType.Idle;
      }
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.Victory)
        return type == ActionType.VictoryForever;
      else
        return true;
    }
  }
}
