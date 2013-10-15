// Type: FezGame.Components.MenuBase
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FezGame.Components
{
  internal class MenuBase : DrawableGameComponent
  {
    public static readonly Action SliderAction = (Action) (() => {});
    protected float SinceMouseMoved = 3f;
    protected SpriteBatch SpriteBatch;
    protected MenuLevel CurrentMenuLevel;
    protected MenuLevel MenuRoot;
    protected MenuLevel UnlockNeedsLIVEMenu;
    protected MenuLevel HelpOptionsMenu;
    protected MenuLevel StartNewGameMenu;
    protected MenuLevel ExitToArcadeMenu;
    protected MenuLevel GameSettingsMenu;
    protected MenuLevel AudioSettingsMenu;
    protected MenuLevel VideoSettingsMenu;
    protected LeaderboardsMenuLevel LeaderboardsMenu;
    protected ControlsMenuLevel ControlsMenu;
    public CreditsMenuLevel CreditsMenu;
    protected List<MenuLevel> MenuLevels;
    protected MenuItem StereoMenuItem;
    protected MenuItem VibrationMenuItem;
    protected SaveManagementLevel SaveManagementMenu;
    protected TimeSpan sliderDownLeft;
    public MenuLevel nextMenuLevel;
    protected MenuLevel lastMenuLevel;
    protected GlyphTextRenderer tr;
    protected Mesh Selector;
    protected Mesh Frame;
    protected Mesh Mask;
    protected SoundEffect sAdvanceLevel;
    protected SoundEffect sCancel;
    protected SoundEffect sConfirm;
    protected SoundEffect sCursorUp;
    protected SoundEffect sCursorDown;
    protected SoundEffect sExitGame;
    protected SoundEffect sReturnLevel;
    protected SoundEffect sScreenNarrowen;
    protected SoundEffect sScreenWiden;
    protected SoundEffect sSliderValueDecrease;
    protected SoundEffect sSliderValueIncrease;
    protected SoundEffect sStartGame;
    protected SoundEffect sAppear;
    protected SoundEffect sDisappear;
    protected SelectorPhase selectorPhase;
    protected float sinceSelectorPhaseStarted;
    protected RenderTarget2D CurrentMenuLevelTexture;
    protected RenderTarget2D NextMenuLevelTexture;
    protected Mesh MenuLevelOverlay;
    protected int currentResolution;
    protected bool isFullscreen;
    public bool EndGameMenu;
    protected bool StartedNewGame;
    public bool CursorSelectable;
    public bool CursorClicking;
    protected Rectangle? AButtonRect;
    protected Rectangle? BButtonRect;
    protected Rectangle? XButtonRect;
    private Texture2D CanClickCursor;
    private Texture2D PointerCursor;
    private Texture2D ClickedCursor;
    protected bool isDisposed;

    [ServiceDependency]
    public IMouseStateManager MouseState { protected get; set; }

    [ServiceDependency]
    public IKeyboardStateManager KeyboardState { protected get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { protected get; set; }

    [ServiceDependency]
    public IInputManager InputManager { protected get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { protected get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { protected get; set; }

    [ServiceDependency]
    public IFontManager Fonts { protected get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { protected get; set; }

    static MenuBase()
    {
    }

    protected MenuBase(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.KeyboardState.IgnoreMapping = true;
      MenuBase menuBase1 = this;
      CreditsMenuLevel creditsMenuLevel1 = new CreditsMenuLevel();
      creditsMenuLevel1.Title = "Credits";
      creditsMenuLevel1.Oversized = true;
      creditsMenuLevel1.IsDynamic = true;
      CreditsMenuLevel creditsMenuLevel2 = creditsMenuLevel1;
      menuBase1.CreditsMenu = creditsMenuLevel2;
      this.StartNewGameMenu = new MenuLevel()
      {
        Title = "StartNewGameTitle",
        AButtonStarts = true,
        AButtonString = "StartNewGameWithGlyph",
        AButtonAction = new Action(this.StartNewGame)
      };
      this.StartNewGameMenu.AddItem("StartNewGameTextLine", new Action(Util.NullAction), -1);
      this.ExitToArcadeMenu = new MenuLevel()
      {
        Title = "ExitConfirmationTitle",
        AButtonString = "ExitChoiceYes",
        AButtonAction = new Action(this.ReturnToArcade)
      };
      this.ExitToArcadeMenu.AddItem("ReturnToArcadeTextLine", new Action(Util.NullAction), -1);
      MenuBase menuBase2 = this;
      LeaderboardsMenuLevel leaderboardsMenuLevel1 = new LeaderboardsMenuLevel(this);
      leaderboardsMenuLevel1.Title = "LeaderboardsTitle";
      leaderboardsMenuLevel1.Oversized = true;
      LeaderboardsMenuLevel leaderboardsMenuLevel2 = leaderboardsMenuLevel1;
      menuBase2.LeaderboardsMenu = leaderboardsMenuLevel2;
      MenuBase menuBase3 = this;
      ControlsMenuLevel controlsMenuLevel1 = new ControlsMenuLevel(this);
      controlsMenuLevel1.Title = "Controls";
      controlsMenuLevel1.Oversized = true;
      ControlsMenuLevel controlsMenuLevel2 = controlsMenuLevel1;
      menuBase3.ControlsMenu = controlsMenuLevel2;
      this.GameSettingsMenu = new MenuLevel()
      {
        Title = "GameSettings",
        BButtonString = "MenuSaveWithGlyph",
        IsDynamic = true,
        Oversized = true
      };
      this.AudioSettingsMenu = new MenuLevel()
      {
        Title = "AudioSettings",
        BButtonString = "MenuSaveWithGlyph",
        IsDynamic = true,
        Oversized = true
      };
      this.VideoSettingsMenu = new MenuLevel()
      {
        Title = "VideoSettings",
        AButtonString = "MenuApplyWithGlyph",
        IsDynamic = true,
        Oversized = true
      };
      this.VideoSettingsMenu.AddItem<string>("Resolution", new Action(this.ApplyVideo), false, (Func<string>) (() =>
      {
        DisplayMode local_0 = SettingsManager.Resolutions[this.currentResolution];
        float wD = (float) (local_0.Width / 1280);
        float hD = (float) (local_0.Height / 720);
        bool local_1 = local_0.Width % 1280 == 0 && (double) local_0.Height >= (double) wD * 720.0 && ((double) local_0.Height == (double) wD * 720.0 || !Enumerable.Any<DisplayMode>((IEnumerable<DisplayMode>) SettingsManager.Resolutions, (Func<DisplayMode, bool>) (x =>
        {
          if ((double) x.Width == (double) wD * 1280.0)
            return (double) x.Height == (double) wD * 720.0;
          else
            return false;
        }))) || local_0.Height % 720 == 0 && (double) local_0.Width >= (double) hD * 1280.0 && ((double) local_0.Width == (double) hD * 1280.0 || !Enumerable.Any<DisplayMode>((IEnumerable<DisplayMode>) SettingsManager.Resolutions, (Func<DisplayMode, bool>) (x =>
        {
          if ((double) x.Width == (double) hD * 1280.0)
            return (double) x.Height == (double) hD * 720.0;
          else
            return false;
        }))) || local_0.Width == 1920 && local_0.Height == 1080 && !Enumerable.Any<DisplayMode>((IEnumerable<DisplayMode>) SettingsManager.Resolutions, (Func<DisplayMode, bool>) (x =>
        {
          if (x.Width >= 2560)
            return x.Height >= 1440;
          else
            return false;
        })) || local_0 == SettingsManager.NativeResolution;
        return string.Concat(new object[4]
        {
          (object) local_0.Width,
          (object) "x",
          (object) local_0.Height,
          local_1 ? (object) " *" : (object) ""
        });
      }), (Action<string, int>) ((lastValue, change) =>
      {
        this.currentResolution += change;
        if (this.currentResolution == SettingsManager.Resolutions.Count)
          this.currentResolution = 0;
        if (this.currentResolution != -1)
          return;
        this.currentResolution = SettingsManager.Resolutions.Count - 1;
      }), -1).UpperCase = true;
      this.VideoSettingsMenu.AddItem<string>("ScreenMode", new Action(this.ApplyVideo), false, (Func<string>) (() =>
      {
        if (!this.isFullscreen)
          return StaticText.GetString("Windowed");
        else
          return StaticText.GetString("Fullscreen");
      }), (Action<string, int>) ((_, __) => this.isFullscreen = !this.isFullscreen), -1).UpperCase = true;
      this.VideoSettingsMenu.AddItem("ResetToDefault", new Action(this.ReturnToVideoDefault), -1);
      this.VideoSettingsMenu.OnPostDraw += (Action<SpriteBatch, SpriteFont, GlyphTextRenderer, float>) ((batch, font, tr, alpha) =>
      {
        float local_0 = this.Fonts.SmallFactor * SettingsManager.GetViewScale(batch.GraphicsDevice);
        float local_1 = (float) batch.GraphicsDevice.Viewport.Height / 2f;
        if (this.VideoSettingsMenu.SelectedIndex != 0)
          return;
        tr.DrawCenteredString(batch, this.Fonts.Small, StaticText.GetString("RecommendedResolution"), new Color(1f, 1f, 1f, alpha), new Vector2(0.0f, local_1 * 1.5f), local_0);
      });
      this.AudioSettingsMenu.AddItem<float>("SoundVolume", MenuBase.SliderAction, false, (Func<float>) (() => SettingsManager.Settings.SoundVolume), (Action<float, int>) ((lastValue, change) =>
      {
        float local_0 = (double) lastValue > 0.0500000007450581 || change >= 0 ? ((double) lastValue < 0.949999988079071 || change <= 0 ? lastValue + (float) change * 0.05f : 1f) : 0.0f;
        this.SoundManager.SoundEffectVolume = SettingsManager.Settings.SoundVolume = local_0;
      }), -1).UpperCase = true;
      this.AudioSettingsMenu.AddItem<float>("MusicVolume", MenuBase.SliderAction, false, (Func<float>) (() => SettingsManager.Settings.MusicVolume), (Action<float, int>) ((lastValue, change) =>
      {
        float local_0 = (double) lastValue > 0.0500000007450581 || change >= 0 ? ((double) lastValue < 0.949999988079071 || change <= 0 ? lastValue + (float) change * 0.05f : 1f) : 0.0f;
        this.SoundManager.MusicVolume = SettingsManager.Settings.MusicVolume = local_0;
      }), -1).UpperCase = true;
      this.AudioSettingsMenu.AddItem("ResetToDefault", new Action(this.ReturnToAudioDefault), -1);
      Language toSet = Culture.Language;
      MenuItem<Language> menuItem1 = this.GameSettingsMenu.AddItem<Language>("Language", MenuBase.SliderAction, false, (Func<Language>) (() => toSet), (Action<Language, int>) ((lastValue, change) =>
      {
        if (change < 0 && toSet == Language.English)
          toSet = Language.Korean;
        else if (change > 0 && toSet == Language.Korean)
          toSet = Language.English;
        else
          toSet += (Language) change;
      }), -1);
      this.GameSettingsMenu.AButtonString = "MenuApplyWithGlyph";
      menuItem1.Selected = (Action) (() => Culture.Language = SettingsManager.Settings.Language = toSet);
      this.GameSettingsMenu.OnReset = (Action) (() => toSet = Culture.Language);
      menuItem1.UpperCase = true;
      menuItem1.LocalizeSliderValue = true;
      menuItem1.LocalizationTagFormat = "Language{0}";
      if (this.GameState.SaveData.HasStereo3D)
        this.StereoMenuItem = this.GameSettingsMenu.AddItem(this.GameState.StereoMode ? "Stereo3DOn" : "Stereo3DOff", new Action(this.ToggleStereo), -1);
      this.VibrationMenuItem = this.GameSettingsMenu.AddItem(SettingsManager.Settings.Vibration ? "VibrationOn" : "VibrationOff", new Action(this.ToggleVibration), -1);
      this.GameSettingsMenu.AddItem("ResetToDefault", (Action) (() =>
      {
        this.ReturnToGameDefault();
        toSet = Culture.Language;
      }), -1);
      this.SaveManagementMenu = new SaveManagementLevel(this);
      this.HelpOptionsMenu = new MenuLevel()
      {
        Title = "HelpOptions"
      };
      this.HelpOptionsMenu.AddItem("Controls", (Action) (() => this.ChangeMenuLevel((MenuLevel) this.ControlsMenu, false)), -1);
      this.HelpOptionsMenu.AddItem("GameSettings", (Action) (() => this.ChangeMenuLevel(this.GameSettingsMenu, false)), -1);
      this.HelpOptionsMenu.AddItem("VideoSettings", (Action) (() =>
      {
        FezEngine.Tools.Settings s = SettingsManager.Settings;
        DisplayMode local_0 = Enumerable.FirstOrDefault<DisplayMode>((IEnumerable<DisplayMode>) SettingsManager.Resolutions, (Func<DisplayMode, bool>) (x =>
        {
          if (x.Width == s.Width)
            return x.Height == s.Height;
          else
            return false;
        })) ?? GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        this.currentResolution = SettingsManager.Resolutions.IndexOf(local_0);
        if (this.currentResolution == -1 || this.currentResolution >= SettingsManager.Resolutions.Count)
          this.currentResolution = 0;
        this.isFullscreen = SettingsManager.Settings.ScreenMode == ScreenMode.Fullscreen;
        this.ChangeMenuLevel(this.VideoSettingsMenu, false);
      }), -1).UpperCase = true;
      this.HelpOptionsMenu.AddItem("AudioSettings", (Action) (() => this.ChangeMenuLevel(this.AudioSettingsMenu, false)), -1);
      if (!Fez.PublicDemo)
        this.HelpOptionsMenu.AddItem("SaveManagementTitle", (Action) (() => this.ChangeMenuLevel((MenuLevel) this.SaveManagementMenu, false)), -1);
      this.SaveManagementMenu.Parent = this.HelpOptionsMenu;
      this.GameSettingsMenu.Parent = this.HelpOptionsMenu;
      this.AudioSettingsMenu.Parent = this.HelpOptionsMenu;
      this.VideoSettingsMenu.Parent = this.HelpOptionsMenu;
      this.ControlsMenu.Parent = this.HelpOptionsMenu;
      this.UnlockNeedsLIVEMenu = new MenuLevel();
      this.UnlockNeedsLIVEMenu.AddItem("UnlockNeedsLIVE", MenuBase.SliderAction, -1).Selectable = false;
      this.MenuRoot = new MenuLevel();
      this.MenuRoot.AddItem("HelpOptions", (Action) (() => this.ChangeMenuLevel(this.HelpOptionsMenu, false)), -1);
      MenuItem menuItem2 = this.MenuRoot.AddItem("Leaderboards", (Action) (() => this.ChangeMenuLevel((MenuLevel) this.LeaderboardsMenu, false)), -1);
      this.MenuRoot.AddItem("Credits", (Action) (() => this.ChangeMenuLevel((MenuLevel) this.CreditsMenu, false)), -1);
      this.CreditsMenu.Parent = this.MenuRoot;
      MenuItem menuItem3 = (MenuItem) null;
      if (this.GameState.IsTrialMode)
        menuItem3 = this.MenuRoot.AddItem("UnlockFullGame", new Action(this.UnlockFullGame), -1);
      MenuItem menuItem4 = this.MenuRoot.AddItem("ReturnToArcade", (Action) (() => this.ChangeMenuLevel(this.ExitToArcadeMenu, false)), -1);
      if (Fez.PublicDemo)
      {
        menuItem4.Disabled = true;
        menuItem2.Disabled = true;
        if (menuItem3 != null)
          menuItem3.Disabled = true;
        menuItem4.Selectable = false;
        menuItem2.Selectable = false;
        if (menuItem3 != null)
          menuItem3.Selectable = false;
      }
      this.MenuLevels = new List<MenuLevel>()
      {
        this.MenuRoot,
        this.UnlockNeedsLIVEMenu,
        this.StartNewGameMenu,
        this.HelpOptionsMenu,
        this.AudioSettingsMenu,
        this.VideoSettingsMenu,
        this.GameSettingsMenu,
        this.ExitToArcadeMenu,
        (MenuLevel) this.LeaderboardsMenu,
        (MenuLevel) this.ControlsMenu,
        (MenuLevel) this.CreditsMenu,
        (MenuLevel) this.SaveManagementMenu
      };
      foreach (MenuLevel menuLevel in this.MenuLevels)
      {
        if (menuLevel != this.MenuRoot && menuLevel.Parent == null)
          menuLevel.Parent = this.MenuRoot;
      }
      this.nextMenuLevel = this.EndGameMenu ? (MenuLevel) this.CreditsMenu : this.MenuRoot;
      this.GameState.DynamicUpgrade += new Action(this.DynamicUpgrade);
      this.PostInitialize();
      base.Initialize();
    }

    protected virtual void PostInitialize()
    {
    }

    protected override void LoadContent()
    {
      this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.tr = new GlyphTextRenderer(this.Game);
      ContentManager contentManager = this.CMProvider.Get(CM.Menu);
      this.PointerCursor = contentManager.Load<Texture2D>("Other Textures/cursor/CURSOR_POINTER");
      this.CanClickCursor = contentManager.Load<Texture2D>("Other Textures/cursor/CURSOR_CLICKER_A");
      this.ClickedCursor = contentManager.Load<Texture2D>("Other Textures/cursor/CURSOR_CLICKER_B");
      this.sAdvanceLevel = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/AdvanceLevel");
      this.sCancel = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/Cancel");
      this.sConfirm = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/Confirm");
      this.sCursorUp = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/CursorUp");
      this.sCursorDown = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/CursorDown");
      this.sExitGame = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/Menu/ExitGame");
      this.sReturnLevel = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/ReturnLevel");
      this.sScreenNarrowen = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/ScreenNarrowen");
      this.sScreenWiden = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/ScreenWiden");
      this.sSliderValueDecrease = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/SliderValueDecrease");
      this.sSliderValueIncrease = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/SliderValueIncrease");
      this.sStartGame = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/Menu/StartGame");
      this.sAppear = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/Appear");
      this.sDisappear = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/Menu/Disappear");
      this.LeaderboardsMenu.InputManager = this.InputManager;
      this.LeaderboardsMenu.GameState = this.GameState;
      this.LeaderboardsMenu.Font = this.Fonts.Big;
      this.LeaderboardsMenu.MouseState = this.MouseState;
      this.ControlsMenu.FontManager = this.Fonts;
      this.ControlsMenu.CMProvider = this.CMProvider;
      this.CreditsMenu.FontManager = this.Fonts;
      foreach (MenuLevel menuLevel in this.MenuLevels)
      {
        menuLevel.CMProvider = this.CMProvider;
        menuLevel.Initialize();
      }
      MenuBase menuBase1 = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
      vertexColored1.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up));
      vertexColored1.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height, 0.1f, 100f));
      DefaultEffect.VertexColored vertexColored2 = vertexColored1;
      mesh2.Effect = (BaseEffect) vertexColored2;
      mesh1.DepthWrites = false;
      mesh1.AlwaysOnTop = true;
      mesh1.Culling = CullMode.None;
      Mesh mesh3 = mesh1;
      menuBase1.Selector = mesh3;
      this.Selector.AddLines(new Color[4]
      {
        Color.White,
        Color.White,
        Color.White,
        Color.White
      }, new Vector3(-1f, -1f, 10f), new Vector3(-1f, 1f, 10f), new Vector3(1f, 1f, 10f), new Vector3(1f, -1f, 10f));
      this.Selector.AddLines(new Color[4]
      {
        Color.White,
        Color.White,
        Color.White,
        Color.White
      }, new Vector3(-1f, 1f, 10f), new Vector3(0.0f, 1f, 10f), new Vector3(-1f, -1f, 10f), new Vector3(0.0f, -1f, 10f));
      this.Selector.AddLines(new Color[4]
      {
        Color.White,
        Color.White,
        Color.White,
        Color.White
      }, new Vector3(0.0f, 1f, 10f), new Vector3(1f, 1f, 10f), new Vector3(0.0f, -1f, 10f), new Vector3(1f, -1f, 10f));
      MenuBase menuBase2 = this;
      Mesh mesh4 = new Mesh();
      Mesh mesh5 = mesh4;
      DefaultEffect.VertexColored vertexColored3 = new DefaultEffect.VertexColored();
      vertexColored3.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up));
      vertexColored3.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height, 0.1f, 100f));
      DefaultEffect.VertexColored vertexColored4 = vertexColored3;
      mesh5.Effect = (BaseEffect) vertexColored4;
      mesh4.DepthWrites = false;
      mesh4.AlwaysOnTop = true;
      mesh4.Culling = CullMode.None;
      mesh4.Enabled = false;
      Mesh mesh6 = mesh4;
      menuBase2.Frame = mesh6;
      this.Frame.AddLines(new Color[8]
      {
        Color.White,
        Color.White,
        Color.White,
        Color.White,
        Color.White,
        Color.White,
        Color.White,
        Color.White
      }, new Vector3(-1f, -1f, 10f), new Vector3(-1f, 1f, 10f), new Vector3(1f, 1f, 10f), new Vector3(1f, -1f, 10f), new Vector3(-1f, 1f, 10f), new Vector3(1f, 1f, 10f), new Vector3(-1f, -1f, 10f), new Vector3(1f, -1f, 10f));
      MenuBase menuBase3 = this;
      Mesh mesh7 = new Mesh();
      Mesh mesh8 = mesh7;
      DefaultEffect.Textured textured1 = new DefaultEffect.Textured();
      textured1.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up));
      textured1.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height, 0.1f, 100f));
      DefaultEffect.Textured textured2 = textured1;
      mesh8.Effect = (BaseEffect) textured2;
      mesh7.DepthWrites = false;
      mesh7.AlwaysOnTop = true;
      mesh7.SamplerState = SamplerState.PointClamp;
      Mesh mesh9 = mesh7;
      menuBase3.MenuLevelOverlay = mesh9;
      this.MenuLevelOverlay.AddFace(new Vector3(2f, 2f, 1f), new Vector3(0.0f, 0.0f, 10f), FaceOrientation.Back, true);
      MenuBase menuBase4 = this;
      Mesh mesh10 = new Mesh();
      Mesh mesh11 = mesh10;
      DefaultEffect.Textured textured3 = new DefaultEffect.Textured();
      textured3.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up));
      textured3.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height, 0.1f, 100f));
      DefaultEffect.Textured textured4 = textured3;
      mesh11.Effect = (BaseEffect) textured4;
      mesh10.DepthWrites = false;
      mesh10.AlwaysOnTop = true;
      Mesh mesh12 = mesh10;
      menuBase4.Mask = mesh12;
      this.Mask.AddFace(new Vector3(2f, 2f, 1f), new Vector3(0.0f, 0.0f, 10f), FaceOrientation.Back, true);
      Waiters.Wait(0.0, new Action(this.Rescale));
      this.RenderToTexture();
    }

    private void ToggleStereo()
    {
      this.GameState.StereoMode = !this.GameState.StereoMode;
      this.StereoMenuItem.Text = this.GameState.StereoMode ? "Stereo3DOn" : "Stereo3DOff";
    }

    private void ToggleVibration()
    {
      SettingsManager.Settings.Vibration = !SettingsManager.Settings.Vibration;
      this.VibrationMenuItem.Text = SettingsManager.Settings.Vibration ? "VibrationOn" : "VibrationOff";
    }

    protected virtual void ReturnToArcade()
    {
    }

    protected virtual void ContinueGame()
    {
    }

    protected virtual void ResumeGame()
    {
    }

    private void ReturnToVideoDefault()
    {
      DisplayMode displayMode = Enumerable.FirstOrDefault<DisplayMode>((IEnumerable<DisplayMode>) SettingsManager.Resolutions, (Func<DisplayMode, bool>) (x =>
      {
        if (x.Width == 1280)
          return x.Height == 720;
        else
          return false;
      })) ?? GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
      this.currentResolution = SettingsManager.Resolutions.IndexOf(displayMode);
      if (this.currentResolution == -1 || this.currentResolution >= SettingsManager.Resolutions.Count)
        this.currentResolution = 0;
      this.isFullscreen = true;
      FezEngine.Tools.Settings settings = SettingsManager.Settings;
      settings.UseCurrentMode = false;
      settings.ScreenMode = ScreenMode.Fullscreen;
      settings.Width = displayMode.Width;
      settings.Height = displayMode.Height;
      SettingsManager.Apply();
      this.Rescale();
    }

    private void ApplyVideo()
    {
      DisplayMode displayMode = SettingsManager.Resolutions[this.currentResolution];
      SettingsManager.Settings.Width = displayMode.Width;
      SettingsManager.Settings.Height = displayMode.Height;
      SettingsManager.Settings.ScreenMode = this.isFullscreen ? ScreenMode.Fullscreen : ScreenMode.Windowed;
      SettingsManager.Apply();
      this.Rescale();
    }

    private void Rescale()
    {
      this.MenuLevelOverlay.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height, 0.1f, 100f));
      this.Mask.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height, 0.1f, 100f));
      this.Selector.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height, 0.1f, 100f));
      this.Frame.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height, 0.1f, 100f));
      this.Frame.Scale = new Vector3(this.CurrentMenuLevel == null ? 512f : (this.CurrentMenuLevel.Oversized ? 512f : 352f), 256f, 1f) * SettingsManager.GetViewScale(this.GraphicsDevice);
    }

    private void ReturnToAudioDefault()
    {
      this.SoundManager.SoundEffectVolume = SettingsManager.Settings.SoundVolume = this.SoundManager.MusicVolume = SettingsManager.Settings.MusicVolume = 1f;
    }

    private void ReturnToGameDefault()
    {
      if (this.GameState.SaveData.HasStereo3D)
      {
        this.GameState.StereoMode = true;
        this.ToggleStereo();
      }
      SettingsManager.Settings.Vibration = false;
      this.ToggleVibration();
      SettingsManager.Settings.Language = Culture.Language = Culture.LanguageFromCurrentCulture();
    }

    protected virtual void StartNewGame()
    {
      this.sinceSelectorPhaseStarted = 0.0f;
      this.selectorPhase = SelectorPhase.Disappear;
      SoundEffectExtensions.Emit(this.sDisappear).Persistent = true;
    }

    private void ShowAchievements()
    {
    }

    private void UnlockFullGame()
    {
    }

    private void DynamicUpgrade()
    {
      ServiceHelper.RemoveComponent<MenuBase>(this);
      Console.WriteLine("Removed main menu component");
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      foreach (MenuLevel menuLevel in this.MenuLevels)
        menuLevel.Dispose();
      if (this.CurrentMenuLevelTexture != null)
      {
        this.CurrentMenuLevelTexture.Dispose();
        this.CurrentMenuLevelTexture = (RenderTarget2D) null;
      }
      if (this.NextMenuLevelTexture != null)
      {
        this.NextMenuLevelTexture.Dispose();
        this.NextMenuLevelTexture = (RenderTarget2D) null;
      }
      this.Selector.Dispose();
      this.Frame.Dispose();
      this.Mask.Dispose();
      this.MenuLevelOverlay.Dispose();
      this.GameState.UnPause();
      this.CMProvider.Dispose(CM.Menu);
      this.KeyboardState.IgnoreMapping = false;
      this.GameState.DynamicUpgrade -= new Action(this.DynamicUpgrade);
      this.isDisposed = true;
    }

    protected virtual bool UpdateEarlyOut()
    {
      return false;
    }

    protected virtual bool AllowDismiss()
    {
      return false;
    }

    public override void Update(GameTime gameTime)
    {
      this.UpdateSelector((float) gameTime.ElapsedGameTime.TotalSeconds);
      if (this.isDisposed || this.UpdateEarlyOut())
        return;
      MenuLevel activeLevel = this.nextMenuLevel ?? this.CurrentMenuLevel;
      if (activeLevel == null)
      {
        this.DestroyMenu();
      }
      else
      {
        Point position = this.MouseState.Position;
        this.SinceMouseMoved += (float) gameTime.ElapsedGameTime.TotalSeconds;
        if (this.MouseState.Movement.X != 0 || this.MouseState.Movement.Y != 0)
          this.SinceMouseMoved = 0.0f;
        if (this.MouseState.LeftButton.State != MouseButtonStates.Idle)
          this.SinceMouseMoved = 0.0f;
        bool flag = false;
        foreach (MenuItem menuItem in activeLevel.Items)
        {
          if (!menuItem.Hidden && menuItem.Selectable)
          {
            if (menuItem.HoverArea.Contains(position.X, position.Y))
            {
              flag = menuItem.Selected != new Action(Util.NullAction) && menuItem.Selected != MenuBase.SliderAction;
              if (this.MouseState.Movement != Point.Zero)
              {
                int selectedIndex = activeLevel.SelectedIndex;
                activeLevel.SelectedIndex = activeLevel.Items.IndexOf(menuItem);
                if (activeLevel.SelectedIndex > selectedIndex)
                  SoundEffectExtensions.Emit(this.sCursorUp);
                else if (activeLevel.SelectedIndex < selectedIndex)
                  SoundEffectExtensions.Emit(this.sCursorDown);
              }
              if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
                this.Select(activeLevel);
            }
            if (menuItem.IsSlider)
            {
              Rectangle hoverArea1 = menuItem.HoverArea;
              hoverArea1.X -= (int) ((double) menuItem.HoverArea.Height * 1.5);
              hoverArea1.Width = menuItem.HoverArea.Height;
              Rectangle hoverArea2 = menuItem.HoverArea;
              hoverArea2.X += menuItem.HoverArea.Width + menuItem.HoverArea.Height / 2;
              hoverArea2.Width = menuItem.HoverArea.Height;
              if (hoverArea1.Contains(position.X, position.Y))
              {
                flag = true;
                if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
                {
                  SoundEffectExtensions.Emit(this.sSliderValueDecrease);
                  this.CurrentMenuLevel.SelectedItem.Slide(-1);
                }
              }
              if (hoverArea2.Contains(position.X, position.Y))
              {
                flag = true;
                if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
                {
                  SoundEffectExtensions.Emit(this.sSliderValueIncrease);
                  this.CurrentMenuLevel.SelectedItem.Slide(1);
                }
              }
            }
          }
        }
        Point point = SettingsManager.PositionInViewport(this.MouseState);
        if (this.AButtonRect.HasValue && this.AButtonRect.Value.Contains(point.X, point.Y))
        {
          flag = true;
          if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
            this.Select(activeLevel);
        }
        if (this.BButtonRect.HasValue && this.BButtonRect.Value.Contains(point.X, point.Y))
        {
          flag = true;
          if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
            this.UpOneLevel(activeLevel);
        }
        if (activeLevel.XButtonAction != null && this.XButtonRect.HasValue && this.XButtonRect.Value.Contains(point.X, point.Y))
        {
          flag = true;
          if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
            activeLevel.XButtonAction();
        }
        this.CursorSelectable = flag;
        this.CursorClicking = this.CursorSelectable && this.MouseState.LeftButton.State == MouseButtonStates.Down;
        if (!activeLevel.TrapInput)
        {
          if (this.InputManager.Up == FezButtonState.Pressed && activeLevel.MoveUp())
            SoundEffectExtensions.Emit(this.sCursorUp);
          if (this.InputManager.Down == FezButtonState.Pressed && activeLevel.MoveDown())
            SoundEffectExtensions.Emit(this.sCursorDown);
          if ((!this.EndGameMenu && this.InputManager.CancelTalk == FezButtonState.Pressed || this.EndGameMenu && this.InputManager.Start == FezButtonState.Pressed || (this.InputManager.Back == FezButtonState.Pressed || activeLevel.ForceCancel)) && (this.AllowDismiss() || this.CurrentMenuLevel != this.MenuRoot))
            this.UpOneLevel(activeLevel);
          if (this.InputManager.Jump == FezButtonState.Pressed || this.InputManager.Start == FezButtonState.Pressed)
            this.Select(activeLevel);
          if (!Fez.PublicDemo && activeLevel.XButtonAction != null && this.InputManager.GrabThrow == FezButtonState.Pressed)
          {
            SoundEffectExtensions.Emit(this.sConfirm);
            activeLevel.XButtonAction();
          }
          TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
          if (this.CurrentMenuLevel != null && this.CurrentMenuLevel.SelectedItem != null && this.CurrentMenuLevel.SelectedItem.IsSlider)
          {
            if (this.InputManager.Left == FezButtonState.Down || this.InputManager.Right == FezButtonState.Down)
              this.sliderDownLeft -= elapsedGameTime;
            else
              this.sliderDownLeft = TimeSpan.FromSeconds(0.300000011920929);
            if (this.InputManager.Left == FezButtonState.Pressed || this.InputManager.Left == FezButtonState.Down && this.sliderDownLeft.Ticks <= 0L)
            {
              if (this.sliderDownLeft.Ticks <= 0L)
                this.sliderDownLeft = TimeSpan.FromSeconds(0.100000001490116);
              SoundEffectExtensions.Emit(this.sSliderValueDecrease);
              this.CurrentMenuLevel.SelectedItem.Slide(-1);
            }
            if (this.InputManager.Right == FezButtonState.Pressed || this.InputManager.Right == FezButtonState.Down && this.sliderDownLeft.Ticks <= 0L)
            {
              if (this.sliderDownLeft.Ticks <= 0L)
                this.sliderDownLeft = TimeSpan.FromSeconds(0.100000001490116);
              SoundEffectExtensions.Emit(this.sSliderValueIncrease);
              this.CurrentMenuLevel.SelectedItem.Slide(1);
            }
          }
        }
        if (this.selectorPhase == SelectorPhase.Appear)
          return;
        activeLevel.Update(gameTime.ElapsedGameTime);
      }
    }

    private void UpOneLevel(MenuLevel activeLevel)
    {
      SoundEffectExtensions.Emit(this.sCancel);
      activeLevel.ForceCancel = false;
      if (this.EndGameMenu)
      {
        this.GameState.EndGame = true;
        this.GameState.Restart();
        this.Enabled = false;
        Waiters.Wait(0.400000005960464, (Action) (() => ServiceHelper.RemoveComponent<MenuBase>(this)));
      }
      else if (activeLevel is SaveSlotSelectionLevel)
      {
        this.sinceSelectorPhaseStarted = 0.0f;
        this.selectorPhase = SelectorPhase.Disappear;
        this.GameState.ReturnToArcade();
      }
      else
      {
        if (activeLevel.Parent == this.HelpOptionsMenu)
          SettingsManager.Save();
        this.ChangeMenuLevel(activeLevel.Parent, false);
      }
    }

    private void Select(MenuLevel activeLevel)
    {
      if (activeLevel.AButtonAction == new Action(this.StartNewGame) || activeLevel.SelectedItem != null && (activeLevel.SelectedItem.Selected == new Action(this.ContinueGame) || activeLevel.SelectedItem.Selected == new Action(this.StartNewGame)))
        SoundEffectExtensions.Emit(this.sStartGame).Persistent = true;
      else if (activeLevel.AButtonAction == new Action(this.ReturnToArcade) && !this.GameState.IsTrialMode)
      {
        this.SoundManager.KillSounds();
        SoundEffectExtensions.Emit(this.sExitGame).Persistent = true;
      }
      else if ((activeLevel.AButtonAction != null || activeLevel.SelectedItem != null) && activeLevel.SelectedItem.Selected != MenuBase.SliderAction)
        SoundEffectExtensions.Emit(this.sConfirm);
      if (activeLevel.AButtonAction != null)
        activeLevel.AButtonAction();
      else
        activeLevel.Select();
    }

    private void UpdateSelector(float elapsedSeconds)
    {
      Vector3 vector3_1 = Vector3.Zero;
      Vector3 vector3_2 = Vector3.Zero;
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      if (this.CurrentMenuLevel != null && this.CurrentMenuLevel.SelectedItem != null)
      {
        float num1 = (this.CurrentMenuLevel.Oversized ? 512f : 256f) * viewScale;
        int num2 = Enumerable.Count<MenuItem>((IEnumerable<MenuItem>) this.CurrentMenuLevel.Items, (Func<MenuItem, bool>) (x => !x.Hidden));
        float num3 = this.CurrentMenuLevel.Items.Count == 0 ? 0.0f : (this.CurrentMenuLevel.SelectedItem.Size.Y + this.Fonts.TopSpacing) * this.Fonts.BigFactor;
        int selectedIndex = this.CurrentMenuLevel.SelectedIndex;
        MenuItem menuItem = this.CurrentMenuLevel.Items[selectedIndex];
        vector3_1 = new Vector3((menuItem.Size + new Vector2(this.Fonts.SideSpacing * 2f, this.Fonts.TopSpacing)) * this.Fonts.BigFactor / 2f, 1f);
        if (num2 > 10)
        {
          bool flag = false;
          switch (Culture.Language)
          {
            case Language.English:
            case Language.Chinese:
            case Language.Japanese:
            case Language.Korean:
              for (int index = 0; index <= this.CurrentMenuLevel.SelectedIndex; ++index)
              {
                if (this.CurrentMenuLevel.Items[index].Hidden)
                  --selectedIndex;
              }
              float num4 = 5f;
              if (selectedIndex == num2 - 1)
                vector3_2 = new Vector3(0.0f, (float) (((double) num4 - 9.0) * (double) num3 - (double) num3 / 2.0), 0.0f);
              else if (selectedIndex < 8)
              {
                vector3_2 = new Vector3(num1 / 2f, (float) (((double) num4 - (double) selectedIndex) * (double) num3 - (double) num3 / 2.0), 0.0f);
              }
              else
              {
                selectedIndex -= 8;
                vector3_2 = new Vector3((float) (-(double) num1 / 2.0), (float) (((double) num4 - (double) selectedIndex) * (double) num3 - (double) num3 / 2.0), 0.0f);
              }
              if (flag && selectedIndex != num2 - 1)
                vector3_1 = vector3_1 * this.Fonts.SmallFactor / this.Fonts.BigFactor;
              string str = WordWrap.Split(menuItem.ToString(), this.Fonts.Small, (float) (((double) ((float) this.Game.GraphicsDevice.Viewport.Width * 0.45f) + (double) vector3_2.X / 2.0) / ((double) this.Fonts.SmallFactor * (double) viewScale)));
              int num5 = 0;
              foreach (int num6 in str)
              {
                if (num6 == 10)
                  ++num5;
              }
              if (num5 > 0)
              {
                vector3_1.Y *= (float) (1 + num5);
                break;
              }
              else
                break;
            default:
              flag = true;
              goto case Language.English;
          }
        }
        else
        {
          float num4 = (float) num2 / 2f;
          for (int index = 0; index <= this.CurrentMenuLevel.SelectedIndex; ++index)
          {
            if (this.CurrentMenuLevel.Items[index].Hidden)
              --selectedIndex;
          }
          vector3_2 = new Vector3(0.0f, (float) (((double) num4 - (double) selectedIndex) * (double) num3 - (double) num3 / 2.0), 0.0f);
        }
      }
      this.sinceSelectorPhaseStarted += elapsedSeconds;
      switch (this.selectorPhase)
      {
        case SelectorPhase.Appear:
        case SelectorPhase.Disappear:
          Group group1 = this.Selector.Groups[0];
          Group group2 = this.Selector.Groups[1];
          Group group3 = this.Selector.Groups[2];
          this.Frame.Enabled = false;
          this.Selector.Material.Opacity = 1f;
          this.Selector.Enabled = true;
          this.Selector.Position = Vector3.Zero;
          this.Selector.Scale = Vector3.One;
          float num7 = Easing.EaseInOut((double) FezMath.Saturate(this.sinceSelectorPhaseStarted / 0.75f), EasingType.Sine, EasingType.Cubic);
          if (this.selectorPhase == SelectorPhase.Disappear)
            num7 = 1f - num7;
          group2.Enabled = group3.Enabled = (double) num7 > 0.5;
          float x1 = (this.nextMenuLevel.Oversized ? 512f : 352f) * viewScale;
          float num8 = FezMath.Saturate((float) (((double) num7 - 0.5) * 2.0));
          float num9 = FezMath.Saturate(num7 * 2f);
          group1.Scale = new Vector3(x1, 256f * num9 * viewScale, 1f);
          group2.Scale = new Vector3(x1 * num8, 256f * viewScale, 1f);
          group2.Position = new Vector3((float) (-(double) x1 * (1.0 - (double) num8)), 0.0f, 1f);
          group3.Scale = new Vector3(x1 * num8, 256f * viewScale, 1f);
          group3.Position = new Vector3(x1 * (1f - num8), 0.0f, 1f);
          if ((double) num7 <= 0.0 && this.selectorPhase == SelectorPhase.Disappear && !this.StartedNewGame)
            this.DestroyMenu();
          if ((double) num7 < 1.0 || this.selectorPhase != SelectorPhase.Appear)
            break;
          this.selectorPhase = SelectorPhase.Shrink;
          group1.Scale = group2.Scale = group3.Scale = Vector3.One;
          group2.Position = group3.Position = Vector3.Zero;
          this.Frame.Scale = this.Selector.Scale = new Vector3(x1, 256f * viewScale, 1f);
          this.Frame.Enabled = true;
          this.sinceSelectorPhaseStarted = 0.0f;
          this.CurrentMenuLevel = this.nextMenuLevel;
          this.CurrentMenuLevelTexture = this.NextMenuLevelTexture;
          break;
        case SelectorPhase.Shrink:
          float amount1 = Easing.EaseInOut((double) FezMath.Saturate(this.sinceSelectorPhaseStarted * 2.5f), EasingType.Sine, EasingType.Cubic);
          if (this.CurrentMenuLevel.SelectedItem == null || !this.CurrentMenuLevel.SelectedItem.Selectable)
          {
            this.Selector.Material.Opacity = 0.0f;
          }
          else
          {
            this.Selector.Material.Opacity = 1f;
            this.Selector.Scale = Vector3.Lerp(new Vector3((this.lastMenuLevel ?? this.CurrentMenuLevel).Oversized ? 512f : 352f, 256f, 1f) * viewScale, vector3_1, amount1);
            this.Selector.Position = Vector3.Lerp(Vector3.Zero, vector3_2, amount1);
          }
          this.Frame.Scale = Vector3.Lerp(new Vector3((this.lastMenuLevel ?? this.CurrentMenuLevel).Oversized ? 512f : 352f, 256f, 1f) * viewScale, new Vector3(this.CurrentMenuLevel.Oversized ? 512f : 352f, 256f, 1f) * viewScale, amount1);
          if ((double) amount1 < 1.0)
            break;
          this.selectorPhase = SelectorPhase.Select;
          break;
        case SelectorPhase.Grow:
          float amount2 = 1f - Easing.EaseInOut((double) FezMath.Saturate(this.sinceSelectorPhaseStarted / 0.3f), EasingType.Sine, EasingType.Quadratic);
          if (this.CurrentMenuLevel.SelectedItem == null || !this.CurrentMenuLevel.SelectedItem.Selectable)
          {
            this.Selector.Material.Opacity = 0.0f;
          }
          else
          {
            this.Selector.Material.Opacity = 1f;
            this.Selector.Scale = Vector3.Lerp(new Vector3(this.nextMenuLevel.Oversized ? 512f : 352f, 256f, 1f) * viewScale, vector3_1, amount2);
            this.Selector.Position = Vector3.Lerp(Vector3.Zero, vector3_2, amount2);
          }
          this.Frame.Scale = Vector3.Lerp(new Vector3(this.CurrentMenuLevel.Oversized ? 512f : 352f, 256f, 1f) * viewScale, new Vector3(this.nextMenuLevel.Oversized ? 512f : 352f, 256f, 1f) * viewScale, 1f - amount2);
          if ((double) amount2 > 0.0)
            break;
          this.lastMenuLevel = this.CurrentMenuLevel;
          this.CurrentMenuLevel = this.nextMenuLevel;
          this.CurrentMenuLevelTexture = this.NextMenuLevelTexture;
          if (this.CurrentMenuLevel.SelectedItem == null || !this.CurrentMenuLevel.SelectedItem.Selectable)
          {
            this.CurrentMenuLevel.Reset();
            this.selectorPhase = SelectorPhase.Select;
          }
          else
          {
            this.CurrentMenuLevel.Reset();
            this.selectorPhase = SelectorPhase.FadeIn;
          }
          this.sinceSelectorPhaseStarted = 0.0f;
          break;
        case SelectorPhase.Select:
          if (this.CurrentMenuLevel.SelectedItem == null || !this.CurrentMenuLevel.SelectedItem.Selectable)
          {
            this.Selector.Material.Opacity = 0.0f;
            break;
          }
          else
          {
            this.Selector.Material.Opacity = 1f;
            this.Selector.Scale = Vector3.Lerp(this.Selector.Scale, vector3_1, 0.3f);
            this.Selector.Position = Vector3.Lerp(this.Selector.Position, vector3_2, 0.3f);
            break;
          }
        case SelectorPhase.FadeIn:
          float amount3 = Easing.EaseInOut((double) FezMath.Saturate(this.sinceSelectorPhaseStarted / 0.25f), EasingType.Sine, EasingType.Cubic);
          this.Selector.Material.Opacity = amount3;
          this.Selector.Scale = Vector3.Lerp(this.Selector.Scale, vector3_1, 0.3f);
          this.Selector.Position = Vector3.Lerp(this.Selector.Position, vector3_2, 0.3f);
          float x2 = (this.CurrentMenuLevel.Oversized ? 512f : 352f) * viewScale;
          if ((double) this.Frame.Scale.X != (double) x2)
            this.Frame.Scale = Vector3.Lerp(new Vector3((this.lastMenuLevel ?? this.CurrentMenuLevel).Oversized ? 512f : 352f, 256f, 1f) * viewScale, new Vector3(x2, 256f * viewScale, 1f), amount3);
          if ((double) amount3 < 1.0)
            break;
          this.selectorPhase = SelectorPhase.Select;
          this.sinceSelectorPhaseStarted = 0.0f;
          break;
      }
    }

    private void DestroyMenu()
    {
      ServiceHelper.RemoveComponent<MenuBase>(this);
      this.nextMenuLevel = this.CurrentMenuLevel = (MenuLevel) null;
    }

    public bool ChangeMenuLevel(MenuLevel next, bool silent = false)
    {
      if (this.CurrentMenuLevel == null)
        return false;
      bool flag1 = this.CurrentMenuLevel.SelectedItem == null || !this.CurrentMenuLevel.SelectedItem.Selectable;
      this.selectorPhase = flag1 ? SelectorPhase.FadeIn : SelectorPhase.Grow;
      bool flag2 = next == this.CurrentMenuLevel.Parent;
      if (this.CurrentMenuLevel.OnClose != null)
        this.CurrentMenuLevel.OnClose();
      if (next == null)
      {
        this.ResumeGame();
        return true;
      }
      else
      {
        this.nextMenuLevel = next;
        this.nextMenuLevel.Reset();
        this.RenderToTexture();
        this.sinceSelectorPhaseStarted = 0.0f;
        this.lastMenuLevel = this.CurrentMenuLevel;
        if (flag1)
        {
          this.CurrentMenuLevel = this.nextMenuLevel;
          this.CurrentMenuLevelTexture = this.NextMenuLevelTexture;
          if (this.CurrentMenuLevel == null)
            this.DestroyMenu();
        }
        else if (!silent)
        {
          if (flag2)
            SoundEffectExtensions.Emit(this.sReturnLevel);
          else
            SoundEffectExtensions.Emit(this.sAdvanceLevel);
          if (this.lastMenuLevel.Oversized && !this.CurrentMenuLevel.Oversized)
            SoundEffectExtensions.Emit(this.sScreenNarrowen);
        }
        if (!this.lastMenuLevel.Oversized && this.CurrentMenuLevel.Oversized)
          SoundEffectExtensions.Emit(this.sScreenWiden);
        return true;
      }
    }

    private void RenderToTexture()
    {
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      if (this.CurrentMenuLevel != null)
      {
        if (this.CurrentMenuLevelTexture != null)
        {
          this.CurrentMenuLevelTexture.Tag = (object) "DISPOSED";
          this.CurrentMenuLevelTexture.Dispose();
        }
        this.CurrentMenuLevelTexture = new RenderTarget2D(this.GraphicsDevice, FezMath.Round((double) (2 * (this.CurrentMenuLevel.Oversized ? 512 : 352)) * (double) viewScale), (int) (512.0 * (double) viewScale), false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PlatformContents);
        this.CurrentMenuLevelTexture.Tag = (object) ("Current | " + this.CurrentMenuLevel.Title);
        this.GraphicsDevice.SetRenderTarget(this.CurrentMenuLevelTexture);
        this.GraphicsDevice.Clear(ClearOptions.Target, ColorEx.TransparentWhite, 1f, 0);
        GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
        this.DrawLevel(this.CurrentMenuLevel, true);
        this.SpriteBatch.End();
        this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      }
      if (this.nextMenuLevel == null)
        return;
      if (this.NextMenuLevelTexture != null)
      {
        this.NextMenuLevelTexture.Tag = (object) "DISPOSED";
        this.NextMenuLevelTexture.Dispose();
      }
      this.NextMenuLevelTexture = new RenderTarget2D(this.GraphicsDevice, FezMath.Round((double) (2 * (this.nextMenuLevel.Oversized ? 512 : 352)) * (double) viewScale), (int) (512.0 * (double) viewScale), false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PlatformContents);
      this.NextMenuLevelTexture.Tag = (object) ("Next | " + this.nextMenuLevel.Title);
      this.GraphicsDevice.SetRenderTarget(this.NextMenuLevelTexture);
      this.GraphicsDevice.Clear(ClearOptions.Target, ColorEx.TransparentWhite, 1f, 0);
      GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
      this.DrawLevel(this.nextMenuLevel, true);
      this.SpriteBatch.End();
      this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
    }

    public override void Draw(GameTime gameTime)
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      this.Mask.Position = this.Selector.Position;
      this.Mask.Scale = this.Selector.Scale;
      float num1 = Culture.IsCJK ? this.Fonts.BigFactor + 0.25f : this.Fonts.BigFactor + 1f;
      bool isCjk = Culture.IsCJK;
      float scale1 = num1 * viewScale;
      int num2 = Culture.IsCJK ? -1 : 1;
      if (this.selectorPhase != SelectorPhase.Select && this.selectorPhase != SelectorPhase.Disappear)
      {
        GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.None);
        GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.MenuWipe));
        this.Mask.Draw();
        GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.All);
      }
      if (this.selectorPhase == SelectorPhase.Grow)
      {
        this.MenuLevelOverlay.Scale = new Vector3((this.nextMenuLevel.Oversized ? 512f : 352f) * viewScale, 256f * viewScale, 1f);
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Equal, StencilMask.MenuWipe);
        this.MenuLevelOverlay.Texture = (Dirtyable<Texture>) ((Texture) this.NextMenuLevelTexture);
        this.MenuLevelOverlay.Draw();
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Always, StencilMask.None);
        if (!isCjk)
          GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
        else
          GraphicsDeviceExtensions.BeginLinear(this.SpriteBatch);
        if (this.nextMenuLevel.Title != null)
          this.tr.DrawCenteredString(this.SpriteBatch, this.Fonts.Big, this.nextMenuLevel.Title, new Color(1f, 1f, 1f, this.sinceSelectorPhaseStarted / 0.3f), new Vector2(0.0f, (float) (30.0 + (double) this.Fonts.TopSpacing * (double) this.Fonts.BigFactor * (double) num2)) * viewScale, scale1);
        this.SpriteBatch.End();
      }
      else
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Always, StencilMask.None);
      if (this.selectorPhase == SelectorPhase.Shrink)
      {
        this.MenuLevelOverlay.Scale = new Vector3((this.CurrentMenuLevel.Oversized ? 512f : 352f) * viewScale, 256f * viewScale, 1f);
        this.MenuLevelOverlay.Texture = (Dirtyable<Texture>) ((Texture) this.CurrentMenuLevelTexture);
        this.MenuLevelOverlay.Draw();
        if (!isCjk)
          GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
        else
          GraphicsDeviceExtensions.BeginLinear(this.SpriteBatch);
        if (this.nextMenuLevel.Title != null)
          this.tr.DrawCenteredString(this.SpriteBatch, this.Fonts.Big, this.nextMenuLevel.Title, Color.White, new Vector2(0.0f, (float) (30.0 + (double) this.Fonts.TopSpacing * (double) this.Fonts.BigFactor * (double) num2)) * viewScale, scale1);
        this.SpriteBatch.End();
      }
      if ((this.selectorPhase == SelectorPhase.Select || this.selectorPhase == SelectorPhase.FadeIn) && this.CurrentMenuLevel != null)
      {
        if (!this.CurrentMenuLevel.IsDynamic)
        {
          this.MenuLevelOverlay.Scale = new Vector3((this.CurrentMenuLevel.Oversized ? 512f : 352f) * viewScale, 256f * viewScale, 1f);
          this.MenuLevelOverlay.Texture = (Dirtyable<Texture>) ((Texture) this.CurrentMenuLevelTexture);
          this.MenuLevelOverlay.Draw();
        }
        if (!isCjk)
          GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
        else
          GraphicsDeviceExtensions.BeginLinear(this.SpriteBatch);
        if (this.CurrentMenuLevel.IsDynamic)
          this.DrawLevel(this.CurrentMenuLevel, false);
        if (this.CurrentMenuLevel.Title != null)
          this.tr.DrawCenteredString(this.SpriteBatch, this.Fonts.Big, this.CurrentMenuLevel.Title, Color.White, new Vector2(0.0f, (float) (30.0 + (double) this.Fonts.TopSpacing * (double) this.Fonts.BigFactor * (double) num2)) * viewScale, scale1);
        this.SpriteBatch.End();
      }
      this.Selector.Draw();
      this.Frame.Draw();
      if (this.CurrentMenuLevel != null && this.selectorPhase != SelectorPhase.Disappear)
        this.DrawButtons();
      GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
      float scale2 = viewScale * 2f;
      Point point = SettingsManager.PositionInViewport(this.MouseState);
      this.SpriteBatch.Draw(this.CursorClicking ? this.ClickedCursor : (this.CursorSelectable ? this.CanClickCursor : this.PointerCursor), new Vector2((float) point.X - scale2 * 11.5f, (float) point.Y - scale2 * 8.5f), new Rectangle?(), new Color(1f, 1f, 1f, FezMath.Saturate((float) (1.0 - ((double) this.SinceMouseMoved - 2.0)))), 0.0f, Vector2.Zero, scale2, SpriteEffects.None, 0.0f);
      this.SpriteBatch.End();
    }

    protected virtual bool AlwaysShowBackButton()
    {
      return false;
    }

    private void DrawButtons()
    {
      Viewport viewport = this.GraphicsDevice.Viewport;
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      float num = this.Frame.Scale.X;
      Vector2 vector2_1 = new Vector2((float) ((double) viewport.Width / 2.0 + (double) num - 5.0), (float) ((double) viewport.Height / 2.0 + 512.0 * (double) viewScale / 2.0 + 5.0 + (double) this.Fonts.TopSpacing * (double) this.Fonts.BigFactor));
      MenuLevel menuLevel = this.selectorPhase == SelectorPhase.Grow ? this.nextMenuLevel : this.CurrentMenuLevel;
      bool flag1 = (this.AlwaysShowBackButton() || menuLevel != this.MenuRoot) && (!this.EndGameMenu || menuLevel != this.CreditsMenu);
      if (menuLevel is SaveSlotSelectionLevel)
        flag1 = true;
      bool flag2 = menuLevel.XButtonString != null;
      bool flag3 = menuLevel.AButtonString != null;
      if (menuLevel == this.VideoSettingsMenu)
        flag3 = true;
      if (flag3 && flag1 && flag2)
      {
        switch (Culture.TwoLetterISOLanguageName)
        {
          case "en":
            vector2_1.X += 60f;
            break;
          case "fr":
            vector2_1.X += 230f;
            break;
          case "de":
            vector2_1.X += 210f;
            break;
          case "es":
            vector2_1.X += 230f;
            break;
          case "it":
            vector2_1.X += 125f;
            break;
          case "pt":
            vector2_1.X += 185f;
            break;
        }
      }
      if (menuLevel == this.LeaderboardsMenu)
        vector2_1.X += 45f;
      GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
      SpriteFont small = this.Fonts.Small;
      float scale = this.Fonts.SmallFactor * viewScale;
      if (flag1)
      {
        string str = menuLevel.BButtonString ?? StaticText.GetString("MenuBackWithGlyph");
        if (!GamepadState.AnyConnected)
          str = str.Replace("{B}", "{BACK}");
        Vector2 vector2_2 = small.MeasureString(this.tr.FillInGlyphs(str.ToUpper(CultureInfo.InvariantCulture))) * scale;
        Vector2 position = vector2_1 - vector2_2 * Vector2.UnitX;
        this.tr.DrawShadowedText(this.SpriteBatch, small, str.ToUpper(CultureInfo.InvariantCulture), position, new Color(1f, 0.5f, 0.5f, 1f), scale);
        this.BButtonRect = new Rectangle?(new Rectangle((int) position.X, (int) position.Y, (int) vector2_2.X, (int) vector2_2.Y));
        vector2_1 = position - this.tr.Margin * Vector2.UnitX / 4f;
      }
      else
        this.BButtonRect = new Rectangle?();
      if (flag2)
      {
        Vector2 vector2_2 = small.MeasureString(this.tr.FillInGlyphs(menuLevel.XButtonString.ToUpper(CultureInfo.InvariantCulture))) * scale;
        Vector2 position = vector2_1 - vector2_2 * Vector2.UnitX;
        this.tr.DrawShadowedText(this.SpriteBatch, small, menuLevel.XButtonString.ToUpper(CultureInfo.InvariantCulture), position, new Color(0.5f, 0.5f, 1f, 1f), scale);
        this.XButtonRect = new Rectangle?(new Rectangle((int) position.X, (int) position.Y, (int) vector2_2.X, (int) vector2_2.Y));
        vector2_1 = position - this.tr.Margin * Vector2.UnitX / 4f;
      }
      else
        this.XButtonRect = new Rectangle?();
      if (flag3)
      {
        string str = menuLevel.AButtonString;
        if (!GamepadState.AnyConnected)
          str = str.Replace("{A}", "{START}");
        Vector2 vector2_2 = small.MeasureString(this.tr.FillInGlyphs(str.ToUpper(CultureInfo.InvariantCulture))) * scale;
        Vector2 position = vector2_1 - vector2_2 * Vector2.UnitX;
        this.tr.DrawShadowedText(this.SpriteBatch, small, str.ToUpper(CultureInfo.InvariantCulture), position, new Color(0.5f, 1f, 0.5f, 1f), scale);
        this.AButtonRect = new Rectangle?(new Rectangle((int) position.X, (int) position.Y, (int) vector2_2.X, (int) vector2_2.Y));
        Vector2 vector2_3 = position - this.tr.Margin * Vector2.UnitX / 4f;
      }
      else
        this.AButtonRect = new Rectangle?();
      this.SpriteBatch.End();
    }

    private void DrawLevel(MenuLevel level, bool toTexture)
    {
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      float num = toTexture ? 512f * viewScale : (float) this.GraphicsDevice.Viewport.Height;
      bool flag = false;
      switch (Culture.Language)
      {
        case Language.English:
        case Language.Chinese:
        case Language.Japanese:
        case Language.Korean:
          lock (level)
          {
            SpriteFont local_3 = !Culture.IsCJK || (double) viewScale <= 1.5 ? this.Fonts.Small : this.Fonts.Big;
            int local_4 = 0;
            for (int local_5 = 0; local_5 < level.Items.Count; ++local_5)
            {
              if (!level.Items[local_5].Hidden)
                ++local_4;
            }
            float local_6 = (level.Oversized ? 512f : 256f) * viewScale;
            for (int local_7 = 0; local_7 < level.Items.Count; ++local_7)
            {
              MenuItem local_8 = level.Items[local_7];
              if (!local_8.Hidden)
              {
                bool local_9 = false;
                string local_10 = local_8.ToString();
                Vector2 local_11 = this.Fonts.Big.MeasureString(local_10) * viewScale;
                if (string.IsNullOrEmpty(local_8.Text))
                  local_11 = this.Fonts.Big.MeasureString("A");
                local_8.Size = local_11;
                float local_12 = level.Items.Count == 0 ? 0.0f : (local_8.Size.Y + this.Fonts.TopSpacing) * this.Fonts.BigFactor;
                float local_13 = this.Fonts.BigFactor * viewScale;
                if (Culture.IsCJK && (double) viewScale <= 1.5)
                  local_13 *= 2f;
                int local_14 = local_7;
                Vector3 local_15;
                if (local_4 > 10)
                {
                  for (int local_16 = 0; local_16 <= local_7; ++local_16)
                  {
                    if (level.Items[local_16].Hidden)
                      --local_14;
                  }
                  local_9 = local_4 > 10 && local_14 != local_4 - 1;
                  if (flag)
                    local_13 = (local_8.IsGamerCard || local_9 ? this.Fonts.SmallFactor : this.Fonts.BigFactor) * viewScale;
                  float local_17 = 5f;
                  if (local_14 == local_4 - 1)
                    local_15 = new Vector3(0.0f, (float) (((double) local_17 - 9.0) * (double) local_12 - (double) local_12 / 2.0), 0.0f);
                  else if (local_14 < 8)
                  {
                    local_15 = new Vector3((float) (-(double) local_6 / 2.0), (float) (((double) local_17 - (double) local_14) * (double) local_12 - (double) local_12 / 2.0), 0.0f);
                  }
                  else
                  {
                    int local_14_1 = local_14 - 8;
                    local_15 = new Vector3(local_6 / 2f, (float) (((double) local_17 - (double) local_14_1) * (double) local_12 - (double) local_12 / 2.0), 0.0f);
                  }
                  if (local_9)
                  {
                    float local_18 = (float) this.Game.GraphicsDevice.Viewport.Width * 0.45f;
                    local_10 = WordWrap.Split(local_10, local_3, (local_18 - local_15.X / 2f) / local_13);
                    int local_19 = 0;
                    foreach (int item_0 in local_10)
                    {
                      if (item_0 == 10)
                        ++local_19;
                    }
                    if (local_19 > 0)
                    {
                      local_11 = this.Fonts.Small.MeasureString(local_10) * viewScale;
                      local_12 = (local_11.Y + this.Fonts.TopSpacing) * this.Fonts.SmallFactor;
                      local_8.Size = new Vector2(local_11.X, local_8.Size.Y);
                    }
                    else if (flag)
                    {
                      local_12 = level.Items.Count == 0 ? 0.0f : (local_8.Size.Y + this.Fonts.TopSpacing) * this.Fonts.SmallFactor;
                      local_11.X *= this.Fonts.SmallFactor / this.Fonts.BigFactor;
                    }
                  }
                }
                else
                {
                  float local_21 = (float) local_4 / 2f;
                  for (int local_22 = 0; local_22 <= local_7; ++local_22)
                  {
                    if (level.Items[local_22].Hidden)
                      --local_14;
                  }
                  local_15 = new Vector3(0.0f, (float) (((double) local_21 - (double) local_14) * (double) local_12 - (double) local_12 / 2.0), 0.0f);
                }
                local_15.Y *= -1f;
                local_15.Y += num / 2f;
                local_15.Y -= local_12 / 2f;
                if (Culture.IsCJK)
                  local_15.Y += viewScale * 4f;
                SpriteFont local_23 = !local_8.IsGamerCard && !local_9 || Culture.IsCJK ? local_3 : this.Fonts.Small;
                Color local_24 = local_8.Disabled ? new Color(0.2f, 0.2f, 0.2f, 1f) : new Color(1f, 1f, 1f, 1f);
                if (local_8.IsGamerCard)
                  local_24 = new Color(0.5f, 1f, 0.5f, 1f);
                this.tr.DrawCenteredString(this.SpriteBatch, local_23, local_10, local_24, FezMath.XY(local_15), local_13);
                Vector2 local_25 = local_23.MeasureString(local_10) * local_13;
                Point local_26;
                local_26.X = (int) ((double) this.GraphicsDevice.PresentationParameters.BackBufferWidth / 2.0 + (double) local_15.X - (double) local_25.X / 2.0);
                local_26.Y = (int) ((double) local_15.Y + (double) this.GraphicsDevice.PresentationParameters.BackBufferHeight / 2.0 - (double) num / 2.0);
                local_8.HoverArea = new Rectangle(local_26.X, local_26.Y, (int) local_25.X, (int) local_25.Y);
                if (local_8.IsSlider && level.SelectedItem == local_8)
                {
                  local_15.Y += 7f * local_13;
                  if (local_9 && flag && (double) local_13 / (double) viewScale < 2.0)
                    local_15.Y -= 4f * viewScale;
                  float local_27 = (float) ((double) viewScale * 30.0 * (double) local_13 / ((double) this.Fonts.BigFactor * (double) viewScale));
                  if (Culture.IsCJK)
                  {
                    local_27 *= 0.475f * viewScale;
                    local_15.Y += viewScale * 5f;
                  }
                  this.tr.DrawCenteredString(this.SpriteBatch, this.Fonts.Big, "{LA}", new Color(1f, 1f, 1f, 1f), new Vector2(local_15.X - local_11.X / 2f * this.Fonts.BigFactor - local_27, local_15.Y), (Culture.IsCJK ? 0.2f : 1f) * viewScale);
                  this.tr.DrawCenteredString(this.SpriteBatch, this.Fonts.Big, "{RA}", new Color(1f, 1f, 1f, 1f), new Vector2((float) ((double) local_15.X + (double) local_11.X / 2.0 * (double) this.Fonts.BigFactor + (double) local_27 * 2.0), local_15.Y), (Culture.IsCJK ? 0.2f : 1f) * viewScale);
                }
              }
            }
            level.PostDraw(this.SpriteBatch, local_3, this.tr, 1f);
            break;
          }
        default:
          flag = true;
          goto case Language.English;
      }
    }
  }
}
