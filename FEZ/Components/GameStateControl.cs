// Type: FezGame.Components.GameStateControl
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components
{
  public class GameStateControl : GameComponent
  {
    private const float SaveWaitTimeSeconds = 4f;
    private Worker<bool> worker;
    private IWaiter loadWaiter;

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    public GameStateControl(Game game)
      : base(game)
    {
      this.UpdateOrder = -3;
    }

    public override void Initialize()
    {
      this.Game.Deactivated += (EventHandler<EventArgs>) ((s, ea) =>
      {
        if (ea is ActiveEventArgs && (ea as ActiveEventArgs).ForBounds)
          return;
        this.DoPause(s, ea);
      });
      this.InputManager.ActiveControllerDisconnected += (Action<PlayerIndex>) (_ => this.DoPause((object) null, EventArgs.Empty));
      this.GameState.DynamicUpgrade += new Action(this.DynamicUpgrade);
    }

    private void DynamicUpgrade()
    {
      this.GameState.ForcedSignOut = true;
      this.GameState.Restart();
    }

    private void DoPause(object s, EventArgs ea)
    {
      bool checkActive = s == this.Game;
      if (this.loadWaiter != null)
        return;
      this.loadWaiter = Waiters.Wait((Func<bool>) (() =>
      {
        if (this.GameState.Loading)
          return false;
        if (checkActive)
          return Intro.Instance == null;
        else
          return true;
      }), (Action) (() =>
      {
        this.loadWaiter = (IWaiter) null;
        if (checkActive && this.Game.IsActive || MainMenu.Instance != null)
          return;
        this.GameState.Pause();
      }));
    }

    public override void Update(GameTime gameTime)
    {
      if ((double) this.GameState.SinceSaveRequest != -1.0)
      {
        this.GameState.SinceSaveRequest += (float) gameTime.ElapsedGameTime.TotalSeconds;
        if ((double) this.GameState.SinceSaveRequest > 4.0)
          this.GameState.DoSave();
      }
      if (this.GameState.Loading || this.CameraManager.Viewpoint == Viewpoint.Perspective || this.worker != null)
        return;
      if ((!this.GameState.InCutscene || this.GameState.InEndCutscene) && this.InputManager.Start == FezButtonState.Pressed)
        this.GameState.Pause();
      if (this.GameState.InCutscene || !this.PlayerManager.CanControl || (this.GameState.FarawaySettings.InTransition || this.PlayerManager.HideFez) || (this.PlayerManager.Action == ActionType.OpeningTreasure || this.PlayerManager.Action == ActionType.OpeningDoor || (this.PlayerManager.Action == ActionType.FindingTreasure || this.PlayerManager.Action == ActionType.ReadingSign)) || (this.PlayerManager.Action == ActionType.LesserWarp || this.PlayerManager.Action == ActionType.GateWarp || (ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action) || this.PlayerManager.Action == ActionType.ExitDoor) || (this.PlayerManager.Action == ActionType.TurnToBell || this.PlayerManager.Action == ActionType.TurnAwayFromBell || (this.PlayerManager.Action == ActionType.HitBell || this.PlayerManager.Action == ActionType.WalkingTo))) || (!this.CameraManager.ViewTransitionReached || this.GameState.Paused || (this.LevelManager.Name == "ELDERS" || EndCutscene32Host.Instance != null) || EndCutscene64Host.Instance != null))
        return;
      if (this.InputManager.OpenInventory == FezButtonState.Pressed && !this.GameState.IsTrialMode && (!this.GameState.InMenuCube && !this.LevelManager.Name.StartsWith("GOMEZ_HOUSE_END")) && this.PlayerManager.Action != ActionType.WalkingTo)
        this.GameState.ToggleInventory();
      if (this.GameState.InMap || this.InputManager.Back != FezButtonState.Pressed || !this.GameState.SaveData.CanOpenMap && !Fez.LevelChooser || (!(this.LevelManager.Name != "PYRAMID") || this.LevelManager.Name.StartsWith("GOMEZ_HOUSE_END")))
        return;
      this.GameState.ToggleMap();
    }
  }
}
