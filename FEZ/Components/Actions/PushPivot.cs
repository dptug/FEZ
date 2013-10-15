// Type: FezGame.Components.Actions.PushPivot
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
  internal class PushPivot : PlayerAction
  {
    private SoundEffect sTurnAway;
    private SoundEffect sTurnBack;
    private SoundEffect sFallOnFace;
    private SoundEmitter eTurnAway;
    private SoundEmitter eTurnBack;
    private TimeSpan sinceStarted;
    private bool reverse;
    private int lastFrame;

    public PushPivot(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.sTurnAway = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/TurnAway");
      this.sTurnBack = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/TurnBack");
      this.sFallOnFace = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Land");
    }

    protected override void Begin()
    {
      this.sinceStarted = TimeSpan.Zero;
      this.eTurnAway = SoundEffectExtensions.EmitAt(this.sTurnAway, this.PlayerManager.Position);
      this.reverse = false;
      this.lastFrame = -1;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      int frame = this.PlayerManager.Animation.Timing.Frame;
      this.sinceStarted += elapsed;
      if (this.sinceStarted.TotalSeconds < 0.25 && !FezButtonStateExtensions.IsDown(this.InputManager.GrabThrow))
      {
        this.eTurnAway.FadeOutAndDie(0.1f);
        this.reverse = true;
      }
      if (this.reverse)
      {
        this.PlayerManager.Animation.Timing.Update(elapsed, -1f);
        if ((double) this.PlayerManager.Animation.Timing.Step <= 0.0)
          this.PlayerManager.Action = ActionType.Idle;
        return false;
      }
      else
      {
        if (this.PlayerManager.Animation.Timing.Frame == 32 && (this.eTurnBack == null || this.eTurnBack.Dead))
          this.eTurnBack = SoundEffectExtensions.EmitAt(this.sTurnBack, this.PlayerManager.Position);
        if (this.PlayerManager.Animation.Timing.Ended)
        {
          this.PlayerManager.Action = ActionType.Idle;
          return false;
        }
        else
        {
          if (frame != this.lastFrame && frame == 18)
            SoundEffectExtensions.EmitAt(this.sFallOnFace, this.PlayerManager.Position);
          this.lastFrame = frame;
          return true;
        }
      }
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.PushingPivot;
    }
  }
}
