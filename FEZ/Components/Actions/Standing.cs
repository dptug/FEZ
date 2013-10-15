// Type: FezGame.Components.Actions.Standing
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
  internal class Standing : PlayerAction
  {
    private SoundEffect sBlink;
    private int lastFrame;

    public Standing(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.sBlink = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Blink");
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Action == ActionType.StandWinking)
      {
        int frame = this.PlayerManager.Animation.Timing.Frame;
        if (this.lastFrame != frame && (frame == 1 || frame == 13))
          SoundEffectExtensions.EmitAt(this.sBlink, this.PlayerManager.Position);
        this.lastFrame = frame;
      }
      return this.PlayerManager.Action == ActionType.StandWinking;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.Standing)
        return type == ActionType.StandWinking;
      else
        return true;
    }
  }
}
