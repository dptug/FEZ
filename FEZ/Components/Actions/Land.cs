// Type: FezGame.Components.Actions.Land
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
  public class Land : PlayerAction
  {
    private SoundEffect landSound;

    public Land(Game game)
      : base(game)
    {
    }

    protected override void TestConditions()
    {
      if (this.PlayerManager.Action != ActionType.Falling || !this.PlayerManager.Grounded)
        return;
      this.PlayerManager.Action = ActionType.Landing;
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.landSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Land");
    }

    protected override void Begin()
    {
      base.Begin();
      this.InputManager.ActiveGamepad.Vibrate(VibrationMotor.RightHigh, 0.400000005960464, TimeSpan.FromSeconds(0.150000005960464));
      SoundEffectExtensions.EmitAt(this.landSound, this.PlayerManager.Position);
      this.GomezService.OnLand();
      this.GameState.JetpackMode = false;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Animation.Timing.Ended)
        this.PlayerManager.Action = ActionType.Idle;
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Landing;
    }
  }
}
