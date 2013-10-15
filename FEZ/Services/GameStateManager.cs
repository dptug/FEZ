// Type: FezGame.Services.GameStateManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using CommunityExpressNS;
using EasyStorage;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame;
using FezGame.Components;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FezGame.Services
{
  public class GameStateManager : EngineStateManager, IGameStateManager, IEngineStateManager
  {
    private readonly SaveData tempSaveData = new SaveData();
    public const string StorageContainerName = "FEZ";
    public const string SaveFileName = "SaveSlot";
    private Action OnLoadingComplete;
    private bool shouldSaveToCloud;

    public Action OnStorageSelected { get; set; }

    public bool SkipLoadScreen { get; set; }

    public bool LoadingVisible { get; set; }

    public bool ForceLoadIcon { get; set; }

    public float SinceSaveRequest { get; set; }

    public bool InFpsMode { get; set; }

    public bool MenuCubeIsZoomed { get; set; }

    public bool InEndCutscene { get; set; }

    public bool DisallowRotation { get; set; }

    public bool ForceTimePaused { get; set; }

    public bool ForcedSignOut { get; set; }

    public string LoggedOutPlayerTag { get; set; }

    public bool EndGame { get; set; }

    public bool ScheduleLoadEnd { get; set; }

    public bool IsAchievementSave { get; set; }

    public int SaveSlot { get; set; }

    public override bool TimePaused
    {
      get
      {
        if (!this.Paused && !this.InMenuCube && (!this.InMap && !this.ForceTimePaused) && !this.InCutscene)
          return this.InFpsMode;
        else
          return true;
      }
    }

    public override float WaterLevelOffset
    {
      get
      {
        return this.SaveData.GlobalWaterLevelModifier ?? 0.0f;
      }
    }

    public new bool InMap
    {
      get
      {
        return this.inMap;
      }
      set
      {
        this.inMap = value;
      }
    }

    public new bool InMenuCube
    {
      get
      {
        return this.inMenuCube;
      }
      set
      {
        this.inMenuCube = value;
      }
    }

    public ISaveDevice ActiveSaveDevice { get; set; }

    public TextScroll ActiveScroll { get; set; }

    public bool HasActivePlayer
    {
      get
      {
        if (this.InputManager.ActiveControllers != ControllerIndex.Any)
          return this.InputManager.ActiveControllers != ControllerIndex.None;
        else
          return false;
      }
    }

    public PlayerIndex ActivePlayer
    {
      get
      {
        return ControllerIndexExtensions.GetPlayer(this.InputManager.ActiveControllers);
      }
    }

    public User ActiveGamer
    {
      get
      {
        return CommunityExpress.Instance.User;
      }
    }

    public SaveData SaveData { get; set; }

    public bool Saving { get; set; }

    public bool ShowDebuggingBag { get; set; }

    public bool JetpackMode { get; set; }

    public bool DebugMode { get; set; }

    public bool SkipFadeOut { get; set; }

    public bool InCutscene { get; set; }

    public bool SkipLoadBackground { get; set; }

    public bool HideHUD { get; set; }

    public bool IsTrialMode
    {
      get
      {
        return false;
      }
    }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    [ServiceDependency]
    public IGameService TrialAndAchievements { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    public event Action ActiveGamerSignedOut = new Action(Util.NullAction);

    public event Action LiveConnectionChanged = new Action(Util.NullAction);

    public event Action DynamicUpgrade = new Action(Util.NullAction);

    public event Action HudElementChanged = new Action(Util.NullAction);

    public GameStateManager()
    {
      this.SinceSaveRequest = -1f;
      this.SaveSlot = -1;
    }

    public void ShowScroll(string actualString, float forSeconds, bool onTop)
    {
      if (this.ActiveScroll != null)
      {
        TextScroll textScroll1 = this.ActiveScroll;
        while (textScroll1.NextScroll != null)
          textScroll1 = textScroll1.NextScroll;
        if (textScroll1.Key == actualString && !textScroll1.Closing)
          return;
        textScroll1.Closing = true;
        textScroll1.Timeout = new float?();
        TextScroll textScroll2 = new TextScroll(ServiceHelper.Game, actualString, onTop)
        {
          Key = actualString
        };
        if ((double) forSeconds > 0.0)
          textScroll2.Timeout = new float?(forSeconds);
        this.ActiveScroll.NextScroll = textScroll2;
      }
      else
      {
        ServiceHelper.AddComponent((IGameComponent) (this.ActiveScroll = new TextScroll(ServiceHelper.Game, actualString, onTop)
        {
          Key = actualString
        }));
        if ((double) forSeconds <= 0.0)
          return;
        this.ActiveScroll.Timeout = new float?(forSeconds);
      }
    }

    public void OnHudElementChanged()
    {
      this.HudElementChanged();
    }

    public void AwardAchievement(Achievement achievement)
    {
      this.IsAchievementSave = true;
      CommunityExpress.Instance.UserAchievements.UnlockAchievement(achievement, true);
    }

    private void WriteLeaderboardEntry()
    {
      if (!this.SaveData.ScoreDirty)
        return;
      CommunityExpress.Instance.Leaderboards.FindLeaderboard((OnLeaderboardRetrieved) (leaderboard =>
      {
        if (leaderboard == null)
        {
          Logger.Log("Leaderboard", LogSeverity.Error, "Could not find leaderboard!");
        }
        else
        {
          int local_0 = Math.Min(this.SaveData.CubeShards, 32) + Math.Min(this.SaveData.SecretCubes, 32) + Math.Min(this.SaveData.PiecesOfHeart, 3);
          leaderboard.UploadLeaderboardScore(ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, local_0, (List<int>) null);
          this.SaveData.ScoreDirty = false;
        }
      }), "CompletionPercentage");
    }

    public void SignInAndChooseStorage(Action onFinish)
    {
      this.OnStorageSelected = onFinish;
      this.ActiveSaveDevice = (ISaveDevice) new PCSaveDevice("FEZ");
      this.OnStorageSelected();
      this.OnStorageSelected = (Action) null;
    }

    public void LoadSaveFile(Action onFinish)
    {
      if (this.IsTrialMode || Fez.PublicDemo || !this.ActiveSaveDevice.FileExists("SaveSlot" + (object) this.SaveSlot))
        this.SaveData = new SaveData()
        {
          Level = this.IsTrialMode ? "trial/BIG_TOWER" : Fez.ForcedLevelName,
          IsNew = true
        };
      else if (!this.ActiveSaveDevice.Load("SaveSlot" + (object) this.SaveSlot, new LoadAction(this.LoadSaveFile)))
        this.SaveData = new SaveData()
        {
          Level = this.IsTrialMode ? "trial/BIG_TOWER" : Fez.ForcedLevelName,
          IsNew = true
        };
      onFinish();
    }

    private void LoadSaveFile(BinaryReader reader)
    {
      this.SaveData = SaveFileOperations.Read(new CrcReader(reader));
    }

    public void LoadLevelAsync(Action onFinish)
    {
      if (this.IsTrialMode)
        throw new InvalidOperationException("Save files should not be used in trial mode");
      if (this.Saving)
        throw new InvalidOperationException("Can't save and load at the same time...?");
      this.OnLoadingComplete = onFinish;
      this.Loading = true;
      Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoLoadLevel));
      worker.Finished += (Action) (() => this.ThreadPool.Return<bool>(worker));
      worker.Start(false);
    }

    public void LoadLevel()
    {
      this.DoLoadLevel(false);
    }

    private void DoLoadLevel(bool dummy)
    {
      this.TimeManager.CurrentTime = DateTime.Today.Add(this.SaveData.TimeOfDay);
      this.LevelManager.ChangeLevel(this.SaveData.Level ?? (this.IsTrialMode ? "trial/BIG_TOWER" : Fez.ForcedLevelName));
      this.TimeManager.OnTick();
      if (this.OnLoadingComplete != null)
        this.OnLoadingComplete();
      this.OnLoadingComplete = (Action) null;
      this.Loading = false;
    }

    public void ReturnToArcade()
    {
      ScreenFade screenFade1 = new ScreenFade(ServiceHelper.Game);
      screenFade1.FromColor = ColorEx.TransparentBlack;
      screenFade1.ToColor = Color.Black;
      screenFade1.Duration = 0.75f;
      screenFade1.EaseOut = true;
      screenFade1.DrawOrder = 3000;
      ScreenFade screenFade2;
      ServiceHelper.AddComponent((IGameComponent) (screenFade2 = screenFade1));
      screenFade2.Faded += new Action(ServiceHelper.Game.Exit);
    }

    public void SaveToCloud(bool force = false)
    {
      if (this.IsTrialMode || Fez.PublicDemo || this.ActiveSaveDevice == null || !force && !this.shouldSaveToCloud)
        return;
      Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoSaveToCloud));
      worker.Finished += (Action) (() => this.ThreadPool.Return<bool>(worker));
      worker.Start(false);
    }

    private void DoSaveToCloud(bool dummy)
    {
      string path = Path.Combine((this.ActiveSaveDevice as PCSaveDevice).RootDirectory, "SaveSlot" + (object) this.SaveSlot);
      try
      {
        CommunityExpress.Instance.RemoteStorage.WriteFile("SaveSlot" + (object) this.SaveSlot, System.IO.File.ReadAllBytes(path));
        this.shouldSaveToCloud = false;
      }
      catch (IOException ex)
      {
        Logger.Log("GameState", LogSeverity.Information, "Failed to save to Steam Cloud, will try again later");
        this.shouldSaveToCloud = true;
      }
    }

    public void Save()
    {
      if (this.IsTrialMode || this.ActiveSaveDevice == null)
        return;
      this.SinceSaveRequest = 0.0f;
      this.SaveData.CloneInto(this.tempSaveData);
    }

    public void DoSave()
    {
      if (this.IsTrialMode || Fez.PublicDemo || this.ActiveSaveDevice == null)
        return;
      this.shouldSaveToCloud = true;
      this.SinceSaveRequest = -1f;
      this.Saving = true;
      Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoSave));
      worker.Finished += (Action) (() =>
      {
        this.ThreadPool.Return<bool>(worker);
        this.Saving = false;
      });
      worker.Start(false);
    }

    private void DoSave(bool dummy)
    {
      this.ActiveSaveDevice.Save("SaveSlot" + (object) this.SaveSlot, new SaveAction(this.DoSave));
      Thread.Sleep(33);
      this.WriteLeaderboardEntry();
    }

    private void DoSave(BinaryWriter writer)
    {
      try
      {
        if (this.SaveData.SinceLastSaved.HasValue)
        {
          this.tempSaveData.PlayTime += DateTime.Now.Ticks - this.SaveData.SinceLastSaved.Value;
          this.SaveData.PlayTime = this.tempSaveData.PlayTime;
        }
        this.SaveData.SinceLastSaved = new long?(DateTime.Now.Ticks);
        SaveFileOperations.Write(new CrcWriter(writer), this.tempSaveData);
      }
      catch (Exception ex)
      {
        Logger.Log("Saving", LogSeverity.Error, ((object) ex).ToString());
      }
    }

    public void Reset()
    {
      this.InMap = this.InMenuCube = this.InFpsMode = false;
      this.LevelManager.Reset();
      this.PlayerManager.Reset();
      this.SoundManager.Stop();
      if (this.ActiveScroll == null)
        return;
      this.ActiveScroll.Close();
      this.ActiveScroll.NextScroll = (TextScroll) null;
    }

    public void Restart()
    {
      if (this.ForcedSignOut)
      {
        this.SinceSaveRequest = -1f;
        this.ActiveGamerSignedOut();
      }
      this.ForceTimePaused = true;
      ScreenFade screenFade1 = new ScreenFade(ServiceHelper.Game);
      screenFade1.FromColor = ColorEx.TransparentBlack;
      screenFade1.ToColor = Color.Black;
      screenFade1.EaseOut = true;
      screenFade1.CaptureScreen = true;
      screenFade1.Duration = 0.5f;
      screenFade1.WaitUntil = (Func<bool>) (() => !this.Loading);
      screenFade1.DrawOrder = 2050;
      ScreenFade screenFade2 = screenFade1;
      screenFade2.ScreenCaptured += (Action) (() =>
      {
        this.Loading = true;
        Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoRestart));
        worker.Finished += (Action) (() =>
        {
          this.ThreadPool.Return<bool>(worker);
          this.SkipLoadBackground = false;
          this.Loading = false;
        });
        worker.Start(false);
      });
      ServiceHelper.AddComponent((IGameComponent) screenFade2);
    }

    private void DoRestart(bool dummy)
    {
      this.SkipLoadBackground = true;
      if (MenuCube.Instance != null)
        ServiceHelper.RemoveComponent<MenuCube>(MenuCube.Instance);
      if (WorldMap.Instance != null)
        ServiceHelper.RemoveComponent<WorldMap>(WorldMap.Instance);
      if (!this.EndGame)
      {
        this.SaveData = new SaveData();
      }
      else
      {
        if (this.SaveData.CubeShards + this.SaveData.SecretCubes < 64)
          this.SaveData.Finished32 = true;
        else
          this.SaveData.Finished64 = true;
        this.SaveData.CanNewGamePlus = true;
        if (this.SaveData.World.ContainsKey("GOMEZ_HOUSE"))
          this.SaveData.World["GOMEZ_HOUSE"].FirstVisit = true;
        this.SaveData.CloneInto(this.tempSaveData);
        this.DoSave();
      }
      this.Reset();
      if (this.InCutscene && Intro.Instance != null)
        ServiceHelper.RemoveComponent<Intro>(Intro.Instance);
      if (MainMenu.Instance != null)
        ServiceHelper.RemoveComponent<MainMenu>(MainMenu.Instance);
      if (PauseMenu.Instance != null)
        ServiceHelper.RemoveComponent<PauseMenu>(PauseMenu.Instance);
      ServiceHelper.AddComponent((IGameComponent) new Intro(ServiceHelper.Game)
      {
        Restarted = true,
        FullLogos = (Fez.PublicDemo || this.EndGame)
      });
      this.EndGame = false;
    }

    public void ClearSaveFile()
    {
      this.SaveData.Clear();
      this.SaveData.IsNew = true;
    }

    public void StartNewGame(Action onFinish)
    {
      this.OnLoadingComplete = onFinish;
      this.ClearSaveFile();
      this.Loading = true;
      Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoStartNewGame));
      worker.Finished += (Action) (() => this.ThreadPool.Return<bool>(worker));
      worker.Start(false);
    }

    private void DoStartNewGame(bool dummy)
    {
      this.SaveData = new SaveData();
      this.PlayerManager.Reset();
      this.LevelManager.Reset();
      this.SoundManager.SoundEffectVolume = SettingsManager.Settings.SoundVolume;
      this.SoundManager.MusicVolume = SettingsManager.Settings.MusicVolume;
      this.TimeManager.CurrentTime = DateTime.Today.Add(this.SaveData.TimeOfDay);
      this.LevelManager.ChangeLevel(this.SaveData.Level ?? (this.IsTrialMode ? "trial/BIG_TOWER" : Fez.ForcedLevelName));
      this.TimeManager.OnTick();
      this.OnLoadingComplete();
      this.Loading = false;
    }

    public void Pause()
    {
      this.Pause(false);
    }

    public void Pause(bool toCredits)
    {
      if (this.Paused)
        return;
      this.paused = true;
      this.OnPauseStateChanged();
      PauseMenu pauseMenu = new PauseMenu(ServiceHelper.Game);
      if (toCredits)
      {
        pauseMenu.EndGameMenu = true;
        ServiceHelper.AddComponent((IGameComponent) pauseMenu);
        pauseMenu.nextMenuLevel = (MenuLevel) pauseMenu.CreditsMenu;
        pauseMenu.nextMenuLevel.Reset();
      }
      else
        ServiceHelper.AddComponent((IGameComponent) new TileTransition(ServiceHelper.Game)
        {
          ScreenCaptured = (Action) (() => ServiceHelper.AddComponent((IGameComponent) pauseMenu)),
          WaitFor = (Func<bool>) (() => pauseMenu.Ready)
        });
    }

    public void UnPause()
    {
      if (!this.Paused)
        return;
      this.paused = false;
      this.OnPauseStateChanged();
    }

    public void ToggleInventory()
    {
      this.InMenuCube = true;
      ServiceHelper.AddComponent((IGameComponent) new MenuCube(ServiceHelper.Game));
    }

    public void ToggleMap()
    {
      ServiceHelper.AddComponent((IGameComponent) new WorldMap(ServiceHelper.Game));
    }
  }
}
