// Type: FezGame.Components.MainMenu
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezGame;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components
{
  internal class MainMenu : MenuBase
  {
    public static MainMenu Instance;
    private SaveSlotSelectionLevel SaveSlotMenuLevel;
    private MenuLevel RealMenuRoot;

    public bool StartedGame { get; private set; }

    public bool ContinuedGame { get; private set; }

    public bool SellingTime { get; private set; }

    public bool HasBought { get; private set; }

    public bool ReturnedToArcade { get; private set; }

    public MainMenu(Game game)
      : base(game)
    {
      this.UpdateOrder = -10;
      this.DrawOrder = 2010;
      MainMenu.Instance = this;
    }

    protected override void PostInitialize()
    {
      if (!Fez.PublicDemo && this.GameState.SaveSlot == -1)
      {
        this.SaveSlotMenuLevel = new SaveSlotSelectionLevel();
        this.MenuLevels.Add((MenuLevel) this.SaveSlotMenuLevel);
        this.SaveSlotMenuLevel.Parent = this.MenuRoot;
        this.nextMenuLevel = (MenuLevel) this.SaveSlotMenuLevel;
        this.SaveSlotMenuLevel.RecoverMainMenu = new Func<bool>(this.RecoverMenuRoot);
        this.RealMenuRoot = this.MenuRoot;
        this.MenuRoot = (MenuLevel) this.SaveSlotMenuLevel;
      }
      else
        this.AddTopElements();
    }

    private bool RecoverMenuRoot()
    {
      if (this.CurrentMenuLevel == null)
        return false;
      this.MenuRoot = this.RealMenuRoot;
      this.AddTopElements();
      this.ChangeMenuLevel(this.MenuRoot, false);
      return true;
    }

    private void AddTopElements()
    {
      MenuLevel menuLevel1 = this.MenuRoot;
      int num1 = 0;
      string text1 = "ContinueGame";
      Action onSelect1 = new Action(((MenuBase) this).ContinueGame);
      int at1 = num1;
      MenuItem menuItem = menuLevel1.AddItem(text1, onSelect1, at1);
      menuItem.Disabled = this.GameState.SaveData.IsNew || this.GameState.SaveData.Level == null || this.GameState.SaveData.CanNewGamePlus;
      menuItem.Selectable = !menuItem.Disabled;
      if (this.GameState.IsTrialMode || this.GameState.SaveData.IsNew)
      {
        MenuLevel menuLevel2 = this.MenuRoot;
        int num2 = 1;
        string text2 = "StartNewGame";
        Action onSelect2 = new Action(((MenuBase) this).StartNewGame);
        int num3 = menuItem.Disabled ? 1 : 0;
        int at2 = num2;
        menuLevel2.AddItem(text2, onSelect2, num3 != 0, at2);
      }
      else
      {
        MenuLevel menuLevel2 = this.MenuRoot;
        int num2 = 1;
        string text2 = "StartNewGame";
        Action onSelect2 = (Action) (() => this.ChangeMenuLevel(this.StartNewGameMenu, false));
        int num3 = menuItem.Disabled ? 1 : 0;
        int at2 = num2;
        menuLevel2.AddItem(text2, onSelect2, num3 != 0, at2);
      }
      if (this.GameState.SaveData.CanNewGamePlus)
      {
        MenuLevel menuLevel2 = this.MenuRoot;
        int num2 = 2;
        string text2 = "StartNewGamePlus";
        Action onSelect2 = new Action(this.NewGamePlus);
        int at2 = num2;
        menuLevel2.AddItem(text2, onSelect2, at2);
      }
      this.MenuRoot.SelectedIndex = this.GameState.SaveData.CanNewGamePlus ? 2 : (this.MenuRoot.Items[0].Selectable ? 0 : 1);
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      SoundEffectExtensions.Emit(this.sAppear);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      MainMenu.Instance = (MainMenu) null;
    }

    protected override void StartNewGame()
    {
      base.StartNewGame();
      this.GameState.ClearSaveFile();
      this.GameState.SaveData.IsNew = false;
      if (this.GameState.SaveData.HasNewGamePlus)
        this.GameState.SaveData.Level = "GOMEZ_HOUSE_2D";
      this.StartedGame = true;
    }

    protected override void ContinueGame()
    {
      this.sinceSelectorPhaseStarted = 0.0f;
      this.selectorPhase = SelectorPhase.Disappear;
      SoundEffectExtensions.Emit(this.sDisappear);
      this.ContinuedGame = true;
    }

    protected override void ReturnToArcade()
    {
      if (this.GameState.IsTrialMode)
      {
        this.SellingTime = true;
      }
      else
      {
        this.sinceSelectorPhaseStarted = 0.0f;
        this.selectorPhase = SelectorPhase.Disappear;
        this.GameState.ReturnToArcade();
        this.ReturnedToArcade = true;
      }
    }

    private void NewGamePlus()
    {
      this.GameState.SaveData.Level = "GOMEZ_HOUSE_2D";
      this.GameState.SaveData.IsNewGamePlus = true;
      this.sinceSelectorPhaseStarted = 0.0f;
      this.selectorPhase = SelectorPhase.Disappear;
      SoundEffectExtensions.Emit(this.sDisappear).Persistent = true;
      this.StartedGame = true;
    }

    protected override bool UpdateEarlyOut()
    {
      if (!this.ContinuedGame && !this.StartedGame && (!this.HasBought && !this.SellingTime))
        return this.ReturnedToArcade;
      else
        return true;
    }

    protected override bool AllowDismiss()
    {
      return this.CurrentMenuLevel == this.SaveSlotMenuLevel;
    }
  }
}
