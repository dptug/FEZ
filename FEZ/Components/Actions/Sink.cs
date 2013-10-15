// Type: FezGame.Components.Actions.Sink
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  internal class Sink : PlayerAction
  {
    private SoundEffect burnSound;
    private SoundEffect drownSound;
    private TimeSpan sinceStarted;
    private ScreenFade fade;
    private bool doneFor;

    public Sink(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.burnSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/BurnInLava");
      this.drownSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/DrownToxic");
    }

    protected override void Begin()
    {
      this.PlayerManager.CarriedInstance = (TrileInstance) null;
      this.PlayerManager.Velocity = new Vector3(0.0f, -0.005f, 0.0f);
      if (this.LevelManager.WaterType == LiquidType.Lava)
        SoundEffectExtensions.EmitAt(this.burnSound, this.PlayerManager.Position);
      else if (this.LevelManager.WaterType == LiquidType.Sewer)
        SoundEffectExtensions.EmitAt(this.drownSound, this.PlayerManager.Position);
      this.sinceStarted = TimeSpan.Zero;
      this.doneFor = (double) this.PlayerManager.RespawnPosition.Y < (double) this.LevelManager.WaterHeight - 0.25;
    }

    protected override void End()
    {
      this.fade = (ScreenFade) null;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      this.sinceStarted += elapsed;
      if (this.fade == null && this.sinceStarted.TotalSeconds > (this.doneFor ? 1.25 : 2.0))
      {
        if (this.doneFor)
        {
          this.fade = new ScreenFade(ServiceHelper.Game)
          {
            FromColor = ColorEx.TransparentBlack,
            ToColor = Color.Black,
            Duration = 1f
          };
          ServiceHelper.AddComponent((IGameComponent) this.fade);
          this.fade.Faded += new Action(this.Respawn);
        }
        else
          this.PlayerManager.Respawn();
      }
      else
        this.PlayerManager.BlinkSpeed = Easing.EaseIn(this.sinceStarted.TotalSeconds / (this.doneFor ? 1.25 : 2.0), EasingType.Cubic) * 1.5f;
      return true;
    }

    private void Respawn()
    {
      ServiceHelper.AddComponent((IGameComponent) new ScreenFade(ServiceHelper.Game)
      {
        FromColor = Color.Black,
        ToColor = ColorEx.TransparentBlack,
        Duration = 1.5f
      });
      this.GameState.LoadSaveFile((Action) (() =>
      {
        this.GameState.Loading = true;
        this.LevelManager.ChangeLevel(this.LevelManager.Name);
        this.GameState.ScheduleLoadEnd = true;
        this.PlayerManager.RespawnAtCheckpoint();
        this.LevelMaterializer.ForceCull();
      }));
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.Sinking;
    }
  }
}
