// Type: FezGame.Components.Actions.PlayingDrums
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  internal class PlayingDrums : PlayerAction
  {
    public PlayingDrums(Game game)
      : base(game)
    {
    }

    protected override bool Act(TimeSpan elapsed)
    {
      return this.PlayerManager.Action == ActionType.DrumsIdle;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return ActionTypeExtensions.IsPlayingDrums(type);
    }
  }
}
