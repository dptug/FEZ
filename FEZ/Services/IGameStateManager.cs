// Type: FezGame.Services.IGameStateManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using CommunityExpressNS;
using EasyStorage;
using FezEngine.Services;
using FezGame.Components;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Services
{
  public interface IGameStateManager : IEngineStateManager
  {
    int SaveSlot { get; set; }

    SaveData SaveData { get; set; }

    ISaveDevice ActiveSaveDevice { get; set; }

    bool Saving { get; }

    bool LoadingVisible { get; set; }

    bool ForceLoadIcon { get; set; }

    float SinceSaveRequest { get; set; }

    bool ShowDebuggingBag { get; set; }

    bool JetpackMode { get; set; }

    bool DebugMode { get; set; }

    bool SkipFadeOut { get; set; }

    bool InCutscene { get; set; }

    bool ForcedSignOut { get; set; }

    bool InEndCutscene { get; set; }

    bool EndGame { get; set; }

    PlayerIndex ActivePlayer { get; }

    User ActiveGamer { get; }

    bool HasActivePlayer { get; }

    string LoggedOutPlayerTag { get; set; }

    bool IsTrialMode { get; }

    bool InMap { get; set; }

    bool InFpsMode { get; set; }

    bool InMenuCube { get; set; }

    bool MenuCubeIsZoomed { get; set; }

    bool SkipLoadBackground { get; set; }

    bool SkipLoadScreen { get; set; }

    bool HideHUD { get; set; }

    TextScroll ActiveScroll { get; set; }

    bool ForceTimePaused { get; set; }

    bool DisallowRotation { get; set; }

    bool ScheduleLoadEnd { get; set; }

    bool IsAchievementSave { get; set; }

    event Action LiveConnectionChanged;

    event Action DynamicUpgrade;

    event Action HudElementChanged;

    void OnHudElementChanged();

    void AwardAchievement(Achievement achievement);

    void UnPause();

    void Pause();

    void Pause(bool toCredits);

    void ToggleInventory();

    void ToggleMap();

    void Restart();

    void ClearSaveFile();

    void LoadSaveFile(Action onFinish);

    void LoadLevel();

    void LoadLevelAsync(Action onFinish);

    void SignInAndChooseStorage(Action onFinish);

    void Save();

    void SaveToCloud(bool force = false);

    void DoSave();

    void StartNewGame(Action onFinish);

    void Reset();

    void ReturnToArcade();

    void ShowScroll(string actualString, float forSeconds, bool onTop);
  }
}
