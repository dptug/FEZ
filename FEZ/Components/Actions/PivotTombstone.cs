// Type: FezGame.Components.Actions.PivotTombstone
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  internal class PivotTombstone : PlayerAction
  {
    private SoundEffect sTurnAway;
    private SoundEffect sTurnBack;

    protected override bool ViewTransitionIndependent
    {
      get
      {
        return true;
      }
    }

    public PivotTombstone(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.sTurnAway = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/TurnAway");
      this.sTurnBack = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/TurnBack");
    }

    protected override void Begin()
    {
      base.Begin();
      SoundEffectExtensions.EmitAt(this.sTurnAway, this.PlayerManager.Position);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Action == ActionType.GrabTombstone && this.PlayerManager.Animation.Timing.Ended && !FezButtonStateExtensions.IsDown(this.InputManager.GrabThrow))
      {
        this.PlayerManager.Action = ActionType.LetGoOfTombstone;
        this.PlayerManager.Animation.Timing.Restart();
        SoundEffectExtensions.EmitAt(this.sTurnBack, this.PlayerManager.Position);
      }
      if (this.PlayerManager.Action == ActionType.LetGoOfTombstone && this.PlayerManager.Animation.Timing.Ended)
        this.PlayerManager.Action = ActionType.Idle;
      this.PlayerManager.Background = false;
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.LetGoOfTombstone && type != ActionType.PivotTombstone)
        return type == ActionType.GrabTombstone;
      else
        return true;
    }
  }
}
