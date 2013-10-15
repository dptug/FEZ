// Type: FezGame.Components.PauseMenu
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components
{
  internal class PauseMenu : MenuBase
  {
    public static StarField Starfield;
    public bool Ready;
    private IntroZoomIn IntroZoomIn;
    public static PauseMenu Instance;
    private bool wasStrict;
    private SoundEffect sStarZoom;

    [ServiceDependency]
    public IGameService GameService { get; private set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { get; private set; }

    public PauseMenu(Game game)
      : base(game)
    {
      this.UpdateOrder = -10;
      this.DrawOrder = 2009;
      PauseMenu.Instance = this;
    }

    public static void PreInitialize()
    {
      ServiceHelper.AddComponent((IGameComponent) (PauseMenu.Starfield = new StarField(ServiceHelper.Game)));
    }

    protected override void PostInitialize()
    {
      if (PauseMenu.Starfield == null)
        ServiceHelper.AddComponent((IGameComponent) (PauseMenu.Starfield = new StarField(this.Game)));
      MenuLevel menuLevel1 = this.MenuRoot;
      int num1 = 0;
      string text1 = "ResumeGame";
      Action onSelect1 = new Action(((MenuBase) this).ResumeGame);
      int at1 = num1;
      menuLevel1.AddItem(text1, onSelect1, at1);
      MenuLevel menuLevel2 = this.MenuRoot;
      int num2 = 1;
      string text2 = "StartNewGame";
      Action onSelect2 = (Action) (() => this.ChangeMenuLevel(this.StartNewGameMenu, false));
      int at2 = num2;
      menuLevel2.AddItem(text2, onSelect2, at2);
      this.wasStrict = this.InputManager.StrictRotation;
      this.InputManager.StrictRotation = false;
      this.GameState.SaveToCloud(false);
    }

    protected override void ResumeGame()
    {
      ServiceHelper.AddComponent((IGameComponent) new TileTransition(ServiceHelper.Game)
      {
        ScreenCaptured = (Action) (() => ServiceHelper.RemoveComponent<PauseMenu>(this))
      });
      this.Enabled = false;
      SoundEffectExtensions.Emit(this.sDisappear).Persistent = true;
    }

    protected override void StartNewGame()
    {
      base.StartNewGame();
      this.GameState.ClearSaveFile();
      if (this.GameState.SaveData.HasNewGamePlus)
      {
        this.GameState.SaveData.HasFPView = false;
        this.GameState.SaveData.Level = "GOMEZ_HOUSE_2D";
      }
      SoundEffectExtensions.Emit(this.sStarZoom).Persistent = true;
      this.StartedNewGame = true;
      this.StartLoading();
      PauseMenu.Starfield.Enabled = true;
      this.GameState.InCutscene = true;
    }

    protected override void ReturnToArcade()
    {
      if (this.GameState.IsTrialMode)
      {
        this.GameService.EndTrial(true);
        Waiters.Wait(0.100000001490116, (Action) (() => ServiceHelper.RemoveComponent<PauseMenu>(this)));
      }
      else
        this.GameState.ReturnToArcade();
      this.Enabled = false;
    }

    private void StartLoading()
    {
      this.GameState.Loading = true;
      Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoLoad));
      worker.Finished += (Action) (() => this.ThreadPool.Return<bool>(worker));
      worker.Start(false);
    }

    private void DoLoad(bool dummy)
    {
      Logger.Try(new Action(this.DoLoad));
    }

    private void DoLoad()
    {
      this.GameState.Loading = true;
      this.Game.IsFixedTimeStep = false;
      this.GameState.SkipLoadBackground = true;
      this.GameState.Reset();
      this.GameState.UnPause();
      this.GameState.LoadLevel();
      Logger.Log("Pause Menu", "Game restarted.");
      this.GameState.ScheduleLoadEnd = true;
      this.Game.IsFixedTimeStep = true;
      this.GameState.SkipLoadBackground = false;
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.sStarZoom = this.CMProvider.Global.Load<SoundEffect>("Sounds/Intro/StarZoom");
      if (this.EndGameMenu)
        return;
      SoundEffectExtensions.Emit(this.sAppear);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      PauseMenu.Instance = (PauseMenu) null;
      if (Intro.Instance == null && EndCutscene32Host.Instance == null && EndCutscene64Host.Instance == null)
        this.GameState.InCutscene = false;
      this.InputManager.StrictRotation = this.wasStrict;
    }

    protected override bool UpdateEarlyOut()
    {
      if (this.GameState.IsTrialMode)
      {
        if (this.StartedNewGame && this.selectorPhase != SelectorPhase.Disappear)
        {
          this.sinceSelectorPhaseStarted = 0.0f;
          this.selectorPhase = SelectorPhase.Disappear;
        }
        if (this.StartedNewGame && !this.GameState.Loading)
        {
          this.DestroyMenu();
          PauseMenu.Starfield = (StarField) null;
          this.CMProvider.Dispose(CM.Intro);
          return true;
        }
      }
      else
      {
        if (this.StartedNewGame && this.IntroZoomIn == null && (PauseMenu.Starfield != null && PauseMenu.Starfield.IsDisposed))
        {
          PauseMenu.Starfield = (StarField) null;
          ServiceHelper.AddComponent((IGameComponent) (this.IntroZoomIn = new IntroZoomIn(this.Game)));
        }
        if (this.StartedNewGame && this.IntroZoomIn != null && this.IntroZoomIn.IsDisposed)
        {
          this.IntroZoomIn = (IntroZoomIn) null;
          this.CMProvider.Dispose(CM.Intro);
          ServiceHelper.RemoveComponent<PauseMenu>(this);
          return true;
        }
      }
      if ((this.nextMenuLevel ?? this.CurrentMenuLevel) == null)
      {
        this.DestroyMenu();
        return true;
      }
      else
        return this.StartedNewGame;
    }

    protected override bool AllowDismiss()
    {
      return true;
    }

    private void DestroyMenu()
    {
      this.DestroyMenu(true);
    }

    private void DestroyMenu(bool viaSignOut)
    {
      if (viaSignOut)
      {
        ServiceHelper.RemoveComponent<PauseMenu>(this);
      }
      else
      {
        if (!this.Enabled)
          return;
        ServiceHelper.AddComponent((IGameComponent) new TileTransition(ServiceHelper.Game)
        {
          ScreenCaptured = (Action) (() => ServiceHelper.RemoveComponent<PauseMenu>(this))
        });
        this.Enabled = false;
        this.nextMenuLevel = this.CurrentMenuLevel = (MenuLevel) null;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      this.Ready = true;
      if (this.IntroZoomIn == null)
        this.TargetRenderer.DrawFullscreen(Color.Black);
      if (PauseMenu.Starfield != null && !PauseMenu.Starfield.IsDisposed)
        PauseMenu.Starfield.Draw();
      base.Draw(gameTime);
    }

    protected override bool AlwaysShowBackButton()
    {
      return true;
    }
  }
}
