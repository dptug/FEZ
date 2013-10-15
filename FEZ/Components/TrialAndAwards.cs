// Type: FezGame.Components.TrialAndAwards
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using CommunityExpressNS;
using FezEngine.Components;
using FezEngine.Components.Scripting;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  public class TrialAndAwards : GameComponent, IGameService, IScriptingBase
  {
    private static readonly TimeSpan CheckFrequency = TimeSpan.FromSeconds(0.25);
    private TimeSpan sinceChecked = TrialAndAwards.CheckFrequency;
    private Achievements Achievements;

    public bool IsMapQrResolved
    {
      get
      {
        return this.GameState.SaveData.MapCheatCodeDone;
      }
    }

    public bool IsScrollOpen
    {
      get
      {
        return this.GameState.ActiveScroll != null;
      }
    }

    public string GetGlobalState
    {
      get
      {
        return this.GameState.SaveData.ScriptingState ?? string.Empty;
      }
    }

    public string GetLevelState
    {
      get
      {
        return this.GameState.SaveData.ThisLevel.ScriptingState ?? string.Empty;
      }
    }

    public bool IsSewerQrResolved
    {
      get
      {
        bool flag1 = this.GameState.SaveData.World.ContainsKey("SEWER_QR") && this.GameState.SaveData.World["SEWER_QR"].InactiveArtObjects.Contains(0);
        bool flag2 = this.GameState.SaveData.World.ContainsKey("ZU_THRONE_RUINS") && this.GameState.SaveData.World["ZU_THRONE_RUINS"].InactiveVolumes.Contains(2);
        bool flag3 = this.GameState.SaveData.World.ContainsKey("ZU_HOUSE_EMPTY") && this.GameState.SaveData.World["ZU_HOUSE_EMPTY"].InactiveVolumes.Contains(2);
        if (!flag1 && !flag3)
          return flag2;
        else
          return true;
      }
    }

    public bool IsZuQrResolved
    {
      get
      {
        bool flag1 = this.GameState.SaveData.World.ContainsKey("PARLOR") && this.GameState.SaveData.World["PARLOR"].InactiveVolumes.Contains(4);
        bool flag2 = this.GameState.SaveData.World.ContainsKey("ZU_HOUSE_QR") && this.GameState.SaveData.World["ZU_HOUSE_QR"].InactiveVolumes.Contains(0);
        if (!flag1)
          return flag2;
        else
          return true;
      }
    }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    static TrialAndAwards()
    {
    }

    public TrialAndAwards(Game game)
      : base(game)
    {
    }

    public void ResetEvents()
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += (Action) (() => this.CollisionManager.GravityFactor = 1f);
    }

    public override void Update(GameTime gameTime)
    {
      this.sinceChecked += gameTime.ElapsedGameTime;
      if (this.sinceChecked < TrialAndAwards.CheckFrequency)
        return;
      this.sinceChecked = TimeSpan.Zero;
      bool flag = PauseMenu.Instance != null && PauseMenu.Instance.EndGameMenu;
      if (this.GameState.Paused && !flag || this.GameState.InCutscene && !flag || this.GameState.Loading)
        return;
      if (this.Achievements == null)
      {
        CommunityExpress.Instance.UserAchievements.InitializeAchievementList((IEnumerable<string>) new string[12]
        {
          "Achievement_01",
          "Achievement_02",
          "Achievement_03",
          "Achievement_04",
          "Achievement_05",
          "Achievement_06",
          "Achievement_07",
          "Achievement_08",
          "Achievement_09",
          "Achievement_10",
          "Achievement_11",
          "Achievement_12"
        });
        this.Achievements = CommunityExpress.Instance.UserAchievements;
      }
      foreach (Achievement achievement in this.Achievements)
      {
        if (!achievement.IsAchieved && this.CheckAchievement(achievement.AchievementName))
        {
          if (!this.GameState.SaveData.EarnedAchievements.Contains(achievement.AchievementName))
            this.GameState.SaveData.EarnedAchievements.Add(achievement.AchievementName);
          this.GameState.AwardAchievement(achievement);
        }
      }
    }

    public bool CheckAchievement(string key)
    {
      switch (key)
      {
        case "Achievement_01":
          if (this.GameState.SaveData.SecretCubes >= 32 && this.GameState.SaveData.CubeShards >= 32)
            return this.GameState.SaveData.Artifacts.Count >= 4;
          else
            return false;
        case "Achievement_02":
          if (this.GameState.SaveData.CubeShards >= 1)
            return this.PlayerManager.CanControl;
          else
            return false;
        case "Achievement_03":
          if (this.GameState.SaveData.HasNewGamePlus)
            return true;
          if (PauseMenu.Instance != null)
            return PauseMenu.Instance.EndGameMenu;
          else
            return false;
        case "Achievement_04":
          if (this.GameState.SaveData.SecretCubes >= 32)
            return this.GameState.SaveData.CubeShards >= 32;
          else
            return false;
        case "Achievement_05":
          return this.GameState.SaveData.Artifacts.Contains(ActorType.Tome);
        case "Achievement_06":
          return this.GameState.SaveData.Artifacts.Contains(ActorType.TriSkull);
        case "Achievement_07":
          return this.GameState.SaveData.Artifacts.Contains(ActorType.LetterCube);
        case "Achievement_08":
          return this.GameState.SaveData.Artifacts.Contains(ActorType.NumberCube);
        case "Achievement_09":
          return this.GameState.SaveData.SecretCubes > 0;
        case "Achievement_10":
          return this.GameState.SaveData.UnlockedWarpDestinations.Count >= 5;
        case "Achievement_11":
          return this.GameState.SaveData.AnyCodeDeciphered;
        case "Achievement_12":
          return this.GameState.SaveData.AchievementCheatCodeDone;
        default:
          return false;
      }
    }

    public void EndTrial(bool forceRestart)
    {
      if (!this.GameState.IsTrialMode)
        return;
      ScreenFade screenFade1 = new ScreenFade(ServiceHelper.Game);
      screenFade1.FromColor = ColorEx.TransparentBlack;
      screenFade1.ToColor = Color.Black;
      screenFade1.EaseOut = true;
      screenFade1.CaptureScreen = true;
      screenFade1.Duration = 1f;
      screenFade1.DrawOrder = 2050;
      screenFade1.WaitUntil = (Func<bool>) (() => !this.GameState.Loading);
      ScreenFade screenFade2 = screenFade1;
      screenFade2.ScreenCaptured += (Action) (() =>
      {
        this.GameState.SkipLoadBackground = true;
        this.GameState.Loading = true;
        Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoSellScreen));
        worker.Finished += (Action) (() =>
        {
          this.ThreadPool.Return<bool>(worker);
          this.GameState.ScheduleLoadEnd = true;
          this.GameState.SkipLoadBackground = false;
        });
        worker.Start(forceRestart);
      });
      ServiceHelper.AddComponent((IGameComponent) screenFade2);
    }

    private void DoSellScreen(bool forceRestart)
    {
      if (forceRestart)
        this.GameState.Reset();
      if (this.GameState.InCutscene && Intro.Instance != null)
        ServiceHelper.RemoveComponent<Intro>(Intro.Instance);
      Intro intro = new Intro(this.Game)
      {
        Sell = true,
        FadeBackToGame = !forceRestart
      };
      ServiceHelper.AddComponent((IGameComponent) intro);
      intro.LoadVideo();
    }

    public LongRunningAction Wait(float seconds)
    {
      return new LongRunningAction((Func<float, float, bool>) ((elapsed, sinceStarted) => (double) sinceStarted >= (double) seconds));
    }

    public LongRunningAction GlitchUp()
    {
      NesGlitches nesGlitches = new NesGlitches(this.Game);
      ServiceHelper.AddComponent((IGameComponent) nesGlitches);
      bool disposed = false;
      nesGlitches.Disposed += (EventHandler<EventArgs>) ((_, __) => disposed = true);
      return new LongRunningAction((Func<float, float, bool>) ((_, __) => disposed));
    }

    public LongRunningAction Reboot(string toLevel)
    {
      Reboot reboot = new Reboot(this.Game, toLevel);
      ServiceHelper.AddComponent((IGameComponent) reboot);
      bool disposed = false;
      reboot.Disposed += (EventHandler<EventArgs>) ((_, __) => disposed = true);
      return new LongRunningAction((Func<float, float, bool>) ((_, __) => disposed));
    }

    public void SetGravity(bool inverted, float factor)
    {
      if ((double) factor == 0.0)
        factor = 1f;
      factor = Math.Abs(factor);
      this.CollisionManager.GravityFactor = inverted ? -factor : factor;
    }

    public void AllowMapUsage()
    {
      this.GameState.SaveData.CanOpenMap = true;
    }

    public void ShowCapsuleLetter()
    {
      ServiceHelper.AddComponent((IGameComponent) new GeezerLetterSender(this.Game));
    }

    public LongRunningAction ShowScroll(string localizedString, float forSeconds, bool onTop, bool onVolume)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TrialAndAwards.\u003C\u003Ec__DisplayClass19 cDisplayClass19 = new TrialAndAwards.\u003C\u003Ec__DisplayClass19();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass19.localizedString = localizedString;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass19.\u003C\u003E4__this = this;
      if (this.GameState.ActiveScroll != null)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TrialAndAwards.\u003C\u003Ec__DisplayClass1b cDisplayClass1b = new TrialAndAwards.\u003C\u003Ec__DisplayClass1b();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass1b.CS\u0024\u003C\u003E8__locals1a = cDisplayClass19;
        TextScroll textScroll = this.GameState.ActiveScroll;
        while (textScroll.NextScroll != null)
          textScroll = textScroll.NextScroll;
        // ISSUE: reference to a compiler-generated field
        if (textScroll.Key == cDisplayClass19.localizedString && !textScroll.Closing)
          return (LongRunningAction) null;
        textScroll.Closing = true;
        textScroll.Timeout = new float?();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cDisplayClass1b.nextScroll = new TextScroll(this.Game, GameText.GetString(cDisplayClass19.localizedString), onTop)
        {
          Key = cDisplayClass19.localizedString
        };
        if ((double) forSeconds > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass1b.nextScroll.Timeout = new float?(forSeconds);
        }
        // ISSUE: reference to a compiler-generated field
        this.GameState.ActiveScroll.NextScroll = cDisplayClass1b.nextScroll;
        if (onVolume)
        {
          // ISSUE: reference to a compiler-generated method
          return new LongRunningAction(new Action(cDisplayClass1b.\u003CShowScroll\u003Eb__17));
        }
        else
          return (LongRunningAction) null;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ServiceHelper.AddComponent((IGameComponent) (this.GameState.ActiveScroll = new TextScroll(this.Game, GameText.GetString(cDisplayClass19.localizedString), onTop)
        {
          Key = cDisplayClass19.localizedString
        }));
        if ((double) forSeconds > 0.0)
          this.GameState.ActiveScroll.Timeout = new float?(forSeconds);
        if (onVolume)
        {
          // ISSUE: reference to a compiler-generated method
          return new LongRunningAction(new Action(cDisplayClass19.\u003CShowScroll\u003Eb__18));
        }
        else
          return (LongRunningAction) null;
      }
    }

    private void CloseScroll(TextScroll scroll)
    {
      if (this.GameState.ActiveScroll == null)
        return;
      if (this.GameState.ActiveScroll == scroll)
      {
        this.GameState.ActiveScroll.Close();
      }
      else
      {
        TextScroll textScroll = this.GameState.ActiveScroll;
        for (TextScroll nextScroll = textScroll.NextScroll; nextScroll != null; nextScroll = nextScroll.NextScroll)
        {
          if (nextScroll == scroll)
          {
            textScroll.NextScroll = nextScroll.NextScroll;
            break;
          }
          else
            textScroll = nextScroll;
        }
      }
    }

    public void CloseScroll(string key)
    {
      if (this.GameState.ActiveScroll == null)
        return;
      if (string.IsNullOrEmpty(key))
      {
        this.GameState.ActiveScroll.Close();
        this.GameState.ActiveScroll.NextScroll = (TextScroll) null;
      }
      else if (this.GameState.ActiveScroll.Key == key)
      {
        this.GameState.ActiveScroll.Close();
      }
      else
      {
        TextScroll textScroll = this.GameState.ActiveScroll;
        for (TextScroll nextScroll = textScroll.NextScroll; nextScroll != null; nextScroll = nextScroll.NextScroll)
        {
          if (nextScroll.Key == key)
          {
            textScroll.NextScroll = nextScroll.NextScroll;
            break;
          }
          else
            textScroll = nextScroll;
        }
      }
    }

    public void SetGlobalState(string state)
    {
      this.GameState.SaveData.ScriptingState = state;
      this.GameState.Save();
    }

    public void SetLevelState(string state)
    {
      this.GameState.SaveData.ThisLevel.ScriptingState = state;
      this.GameState.Save();
    }

    public void ResolveMapQR()
    {
      this.GameState.SaveData.MapCheatCodeDone = true;
      this.GameState.Save();
    }

    public void ResolveSewerQR()
    {
      if (this.GameState.SaveData.World.ContainsKey("SEWER_QR") && !this.GameState.SaveData.World["SEWER_QR"].InactiveArtObjects.Contains(0))
      {
        this.GameState.SaveData.World["SEWER_QR"].InactiveArtObjects.Add(0);
        ++this.GameState.SaveData.World["SEWER_QR"].FilledConditions.SecretCount;
      }
      if (this.GameState.SaveData.World.ContainsKey("ZU_THRONE_RUINS") && !this.GameState.SaveData.World["ZU_THRONE_RUINS"].InactiveVolumes.Contains(2))
      {
        this.GameState.SaveData.World["ZU_THRONE_RUINS"].InactiveVolumes.Add(2);
        ++this.GameState.SaveData.World["ZU_THRONE_RUINS"].FilledConditions.SecretCount;
      }
      if (this.GameState.SaveData.World.ContainsKey("ZU_HOUSE_EMPTY") && !this.GameState.SaveData.World["ZU_HOUSE_EMPTY"].InactiveVolumes.Contains(2))
      {
        this.GameState.SaveData.World["ZU_HOUSE_EMPTY"].InactiveVolumes.Add(2);
        ++this.GameState.SaveData.World["ZU_HOUSE_EMPTY"].FilledConditions.SecretCount;
      }
      this.GameState.Save();
    }

    public void ResolveZuQR()
    {
      if (this.GameState.SaveData.World.ContainsKey("PARLOR") && !this.GameState.SaveData.World["PARLOR"].InactiveVolumes.Contains(4))
      {
        this.GameState.SaveData.World["PARLOR"].InactiveVolumes.Add(4);
        ++this.GameState.SaveData.World["PARLOR"].FilledConditions.SecretCount;
      }
      if (this.GameState.SaveData.World.ContainsKey("ZU_HOUSE_QR") && !this.GameState.SaveData.World["ZU_HOUSE_QR"].InactiveVolumes.Contains(0))
      {
        this.GameState.SaveData.World["ZU_HOUSE_QR"].InactiveVolumes.Add(0);
        ++this.GameState.SaveData.World["ZU_HOUSE_QR"].FilledConditions.SecretCount;
      }
      this.GameState.Save();
    }

    public void Start32BitCutscene()
    {
      ServiceHelper.AddComponent((IGameComponent) new EndCutscene32Host(this.Game));
    }

    public void Start64BitCutscene()
    {
      ServiceHelper.AddComponent((IGameComponent) new EndCutscene64Host(this.Game));
    }

    public void Checkpoint()
    {
      Waiters.Wait((Func<bool>) (() =>
      {
        if (this.PlayerManager.Grounded)
          return ActorTypeExtensions.IsSafe(this.PlayerManager.Ground.First.Trile.ActorSettings.Type);
        else
          return false;
      }), (Action) (() =>
      {
        if (this.LevelManager.Name == "LAVA" && (double) this.LevelManager.WaterHeight < 50.0)
          this.LevelManager.WaterHeight = 132f;
        this.PlayerManager.RecordRespawnInformation(true);
      }));
    }
  }
}
