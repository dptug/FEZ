// Type: FezGame.Components.Actions.ExitDoor
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class ExitDoor : PlayerAction
  {
    private SoundEffect sound;

    public ExitDoor(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      this.sound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/ExitDoor");
    }

    protected override void Begin()
    {
      base.Begin();
      SoundEffectExtensions.EmitAt(this.sound, this.PlayerManager.Position);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (!this.PlayerManager.Animation.Timing.Ended)
        return true;
      this.PlayerManager.Action = ActionType.Idle;
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.ExitDoor && type != ActionType.ExitDoorCarry)
        return type == ActionType.ExitDoorCarryHeavy;
      else
        return true;
    }
  }
}
