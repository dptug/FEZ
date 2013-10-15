// Type: FezGame.Components.DebugControls
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  public class DebugControls : GameComponent
  {
    private PolytronLogo pl;

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ITimeService TimeService { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency(Optional = true)]
    public IMouseStateManager MouseState { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameService GameService { private get; set; }

    [ServiceDependency]
    public ISoundManager SM { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    [ServiceDependency]
    public IBlackHoleManager BlackHoles { private get; set; }

    public DebugControls(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.KeyboardState.RegisterKey(Keys.F1);
      this.KeyboardState.RegisterKey(Keys.F2);
      this.KeyboardState.RegisterKey(Keys.F3);
      this.KeyboardState.RegisterKey(Keys.F4);
      this.KeyboardState.RegisterKey(Keys.F5);
      this.KeyboardState.RegisterKey(Keys.F6);
      this.KeyboardState.RegisterKey(Keys.F8);
      this.KeyboardState.RegisterKey(Keys.F9);
      this.KeyboardState.RegisterKey(Keys.F10);
      this.KeyboardState.RegisterKey(Keys.F11);
      this.KeyboardState.RegisterKey(Keys.F12);
      this.KeyboardState.RegisterKey(Keys.NumPad0);
      this.KeyboardState.RegisterKey(Keys.NumPad1);
      this.KeyboardState.RegisterKey(Keys.NumPad2);
      this.KeyboardState.RegisterKey(Keys.NumPad3);
      this.KeyboardState.RegisterKey(Keys.NumPad4);
      this.KeyboardState.RegisterKey(Keys.NumPad5);
      this.KeyboardState.RegisterKey(Keys.NumPad6);
      this.KeyboardState.RegisterKey(Keys.NumPad7);
      this.KeyboardState.RegisterKey(Keys.NumPad8);
      this.KeyboardState.RegisterKey(Keys.NumPad9);
      this.KeyboardState.RegisterKey(Keys.D0);
      this.KeyboardState.RegisterKey(Keys.D1);
      this.KeyboardState.RegisterKey(Keys.D2);
      this.KeyboardState.RegisterKey(Keys.D3);
      this.KeyboardState.RegisterKey(Keys.D4);
      this.KeyboardState.RegisterKey(Keys.D5);
      this.KeyboardState.RegisterKey(Keys.D6);
      this.KeyboardState.RegisterKey(Keys.D7);
      this.KeyboardState.RegisterKey(Keys.D8);
      this.KeyboardState.RegisterKey(Keys.D9);
      this.KeyboardState.RegisterKey(Keys.L);
      this.KeyboardState.RegisterKey(Keys.H);
      this.KeyboardState.RegisterKey(Keys.J);
      this.KeyboardState.RegisterKey(Keys.K);
      this.KeyboardState.RegisterKey(Keys.R);
      this.KeyboardState.RegisterKey(Keys.T);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.KeyboardState.GetKeyState(Keys.F1) == FezButtonState.Pressed)
        this.GameState.DebugMode = true;
      if (this.KeyboardState.GetKeyState(Keys.F2) == FezButtonState.Pressed)
        this.GameState.DebugMode = false;
      if (this.KeyboardState.GetKeyState(Keys.F3) == FezButtonState.Pressed)
        this.SM.GlobalVolumeFactor = 0.0f;
      if (this.KeyboardState.GetKeyState(Keys.F4) == FezButtonState.Pressed)
        this.SM.GlobalVolumeFactor = 1f;
      if (this.KeyboardState.GetKeyState(Keys.F5) == FezButtonState.Pressed)
        this.GameState.ShowDebuggingBag = true;
      if (this.KeyboardState.GetKeyState(Keys.F6) == FezButtonState.Pressed)
        this.GameState.ShowDebuggingBag = false;
      if (this.KeyboardState.GetKeyState(Keys.F9) == FezButtonState.Pressed)
        this.TimeService.SetHour(4, true);
      if (this.KeyboardState.GetKeyState(Keys.F10) == FezButtonState.Pressed)
        this.TimeService.SetHour(12, true);
      if (this.KeyboardState.GetKeyState(Keys.F11) == FezButtonState.Pressed)
        this.TimeService.SetHour(20, true);
      if (this.KeyboardState.GetKeyState(Keys.F12) == FezButtonState.Pressed)
        this.TimeService.SetHour(0, true);
      if (this.KeyboardState.GetKeyState(Keys.NumPad0) == FezButtonState.Pressed || this.KeyboardState.GetKeyState(Keys.D0) == FezButtonState.Pressed)
        this.CameraManager.PixelsPerTrixel = 1f;
      if (this.KeyboardState.GetKeyState(Keys.NumPad1) == FezButtonState.Pressed || this.KeyboardState.GetKeyState(Keys.D1) == FezButtonState.Pressed)
        this.CameraManager.PixelsPerTrixel = 2f;
      if (this.KeyboardState.GetKeyState(Keys.NumPad2) == FezButtonState.Pressed || this.KeyboardState.GetKeyState(Keys.D2) == FezButtonState.Pressed)
        this.CameraManager.PixelsPerTrixel = 3f;
      if (this.KeyboardState.GetKeyState(Keys.NumPad3) == FezButtonState.Pressed || this.KeyboardState.GetKeyState(Keys.D3) == FezButtonState.Pressed)
        this.CameraManager.PixelsPerTrixel = 4f;
      if (this.KeyboardState.GetKeyState(Keys.NumPad5) == FezButtonState.Pressed || this.KeyboardState.GetKeyState(Keys.D5) == FezButtonState.Pressed)
      {
        ++this.GameState.SaveData.CubeShards;
        this.GameState.SaveData.ScoreDirty = true;
        this.GameState.OnHudElementChanged();
      }
      if ((this.KeyboardState.GetKeyState(Keys.NumPad6) == FezButtonState.Pressed || this.KeyboardState.GetKeyState(Keys.D6) == FezButtonState.Pressed) && this.GameState.SaveData.CubeShards > 0)
      {
        --this.GameState.SaveData.CubeShards;
        this.GameState.SaveData.ScoreDirty = true;
        this.GameState.OnHudElementChanged();
      }
      if (this.KeyboardState.GetKeyState(Keys.NumPad7) == FezButtonState.Pressed || this.KeyboardState.GetKeyState(Keys.D7) == FezButtonState.Pressed)
      {
        ++this.GameState.SaveData.Keys;
        this.GameState.OnHudElementChanged();
      }
      if ((this.KeyboardState.GetKeyState(Keys.NumPad8) == FezButtonState.Pressed || this.KeyboardState.GetKeyState(Keys.D8) == FezButtonState.Pressed) && this.GameState.SaveData.Keys > 0)
      {
        --this.GameState.SaveData.Keys;
        this.GameState.OnHudElementChanged();
      }
      if (this.KeyboardState.GetKeyState(Keys.NumPad9) == FezButtonState.Pressed || this.KeyboardState.GetKeyState(Keys.D9) == FezButtonState.Pressed)
      {
        ++this.GameState.SaveData.SecretCubes;
        this.GameState.SaveData.ScoreDirty = true;
        this.GameState.OnHudElementChanged();
      }
      if (this.KeyboardState.GetKeyState(Keys.L) == FezButtonState.Pressed)
        this.GameState.SaveData.HasDoneHeartReboot = true;
      if (FezButtonStateExtensions.IsDown(this.KeyboardState.GetKeyState(Keys.LeftControl)) && this.KeyboardState.GetKeyState(Keys.S) == FezButtonState.Pressed)
      {
        this.GameState.SaveData.IsNew = false;
        this.GameState.Save();
      }
      if (this.KeyboardState.GetKeyState(Keys.H) == FezButtonState.Pressed)
        this.BlackHoles.EnableAll();
      if (this.KeyboardState.GetKeyState(Keys.J) == FezButtonState.Pressed)
        this.BlackHoles.DisableAll();
      if (this.KeyboardState.GetKeyState(Keys.K) == FezButtonState.Pressed)
        this.BlackHoles.Randomize();
      if (!Fez.LongScreenshot)
        return;
      if (this.KeyboardState.GetKeyState(Keys.R) == FezButtonState.Pressed)
      {
        this.SM.PlayNewSong((string) null);
        this.GameState.HideHUD = true;
        this.PlayerManager.Action = ActionType.StandWinking;
        this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, Fez.DoubleRotations ? 2 : 1));
      }
      if (this.KeyboardState.GetKeyState(Keys.T) != FezButtonState.Pressed)
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DebugControls.\u003C\u003Ec__DisplayClass5 cDisplayClass5_1 = new DebugControls.\u003C\u003Ec__DisplayClass5();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_1.\u003C\u003E4__this = this;
      this.SM.KillSounds(0.1f);
      this.SM.PlayNewSong((string) null, 0.1f);
      foreach (AmbienceTrack ambienceTrack in (IEnumerable<AmbienceTrack>) this.LevelManager.AmbienceTracks)
        this.SM.MuteAmbience(ambienceTrack.Name, 0.1f);
      if (this.pl != null)
        ServiceHelper.RemoveComponent<PolytronLogo>(this.pl);
      DebugControls debugControls = this;
      PolytronLogo polytronLogo1 = new PolytronLogo(this.Game);
      polytronLogo1.DrawOrder = 10000;
      polytronLogo1.Opacity = 1f;
      PolytronLogo polytronLogo2 = polytronLogo1;
      debugControls.pl = polytronLogo2;
      ServiceHelper.AddComponent((IGameComponent) this.pl);
      // ISSUE: variable of a compiler-generated type
      DebugControls.\u003C\u003Ec__DisplayClass5 cDisplayClass5_2 = cDisplayClass5_1;
      LogoRenderer logoRenderer1 = new LogoRenderer(this.Game);
      logoRenderer1.DrawOrder = 9999;
      logoRenderer1.Visible = false;
      logoRenderer1.Enabled = false;
      LogoRenderer logoRenderer2 = logoRenderer1;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_2.tl = logoRenderer2;
      // ISSUE: reference to a compiler-generated field
      ServiceHelper.AddComponent((IGameComponent) cDisplayClass5_1.tl);
      // ISSUE: reference to a compiler-generated field
      ServiceHelper.AddComponent((IGameComponent) (cDisplayClass5_1.FezLogo = new FezLogo(this.Game)));
      SoundEffect soundEffect = this.CMProvider.Global.Load<SoundEffect>("Sounds/Intro/LogoZoom");
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_1.FezLogo.Visible = true;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_1.FezLogo.Enabled = true;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_1.FezLogo.TransitionStarted = true;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_1.FezLogo.Opacity = 1f;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_1.FezLogo.Inverted = true;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_1.FezLogo.SinceStarted = 4.5f;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_1.FezLogo.HalfSpeed = true;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass5_1.FezLogo.Update(new GameTime());
      SoundEffectExtensions.Emit(soundEffect);
      SoundManager.NoMoreSounds = true;
      this.GameState.SkipRendering = true;
      // ISSUE: reference to a compiler-generated method
      Waiters.Wait(7.0, new Action(cDisplayClass5_1.\u003CUpdate\u003Eb__2));
    }
  }
}
