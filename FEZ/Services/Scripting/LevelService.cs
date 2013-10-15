// Type: FezGame.Services.Scripting.LevelService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Components.Scripting;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Services.Scripting
{
  public class LevelService : ILevelService, IScriptingBase
  {
    private readonly Stack<object> waterStopStack = new Stack<object>();
    private SoundEffect sewageLevelSound;
    private SoundEffect sSolvedSecret;
    private SoundEmitter sewageLevelEmitter;

    public bool FirstVisit
    {
      get
      {
        return this.GameState.SaveData.ThisLevel.FirstVisit;
      }
    }

    [ServiceDependency]
    public IGameService GameService { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    internal IScriptingManager ScriptingManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    public event Action Start = new Action(Util.NullAction);

    public void OnStart()
    {
      if (this.sSolvedSecret == null)
        this.sSolvedSecret = this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/SecretSolved");
      this.Start();
    }

    public LongRunningAction ExploChangeLevel(string levelName)
    {
      return (LongRunningAction) null;
    }

    public LongRunningAction SetWaterHeight(float height)
    {
      float sign = (float) Math.Sign(height - this.LevelManager.WaterHeight);
      TrileInstance[] buoyantTriles = Enumerable.ToArray<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => ActorTypeExtensions.IsBuoyant(x.Trile.ActorSettings.Type))));
      return new LongRunningAction((Func<float, float, bool>) ((elapsed, __) =>
      {
        if (this.GameState.Paused || this.GameState.InMap || (!this.CameraManager.ActionRunning || this.CameraManager.Viewpoint == Viewpoint.Perspective))
          return false;
        if ((double) Math.Sign(height - this.LevelManager.WaterHeight) != (double) sign)
          return true;
        this.LevelManager.WaterSpeed = 1.2f * sign;
        IGameLevelManager temp_76 = this.LevelManager;
        double temp_84 = (double) temp_76.WaterHeight + (double) this.LevelManager.WaterSpeed * (double) elapsed;
        temp_76.WaterHeight = (float) temp_84;
        if (this.LevelManager.WaterType != LiquidType.Lava)
        {
          foreach (TrileInstance item_0 in buoyantTriles)
          {
            if ((double) item_0.Center.Y < (double) this.LevelManager.WaterHeight)
              item_0.PhysicsState.Velocity = new Vector3(0.0f, 0.01f, 0.0f);
          }
        }
        return false;
      }), (Action) (() =>
      {
        this.LevelManager.WaterSpeed = 0.0f;
        if (this.LevelManager.WaterType == LiquidType.Water)
          this.GameState.SaveData.GlobalWaterLevelModifier = new float?(this.LevelManager.WaterHeight - this.LevelManager.OriginalWaterHeight);
        else
          this.GameState.SaveData.ThisLevel.LastStableLiquidHeight = new float?(this.LevelManager.WaterHeight);
      }));
    }

    public LongRunningAction RaiseWater(float unitsPerSecond, float toHeight)
    {
      float sign = (float) Math.Sign(toHeight - this.LevelManager.WaterHeight);
      TrileInstance[] buoyantTriles = Enumerable.ToArray<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => ActorTypeExtensions.IsBuoyant(x.Trile.ActorSettings.Type))));
      if ((double) this.LevelManager.WaterSpeed != 0.0)
        this.waterStopStack.Push(new object());
      bool flag = false;
      if (this.LevelManager.WaterType == LiquidType.Sewer || this.LevelManager.Name == "WATER_WHEEL")
      {
        if (this.sewageLevelEmitter != null && !this.sewageLevelEmitter.Dead)
        {
          this.sewageLevelEmitter.Cue.Stop(false);
          flag = true;
        }
        if (this.sewageLevelSound == null)
          this.sewageLevelSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Sewer/SewageLevelChange");
        this.sewageLevelEmitter = SoundEffectExtensions.Emit(this.sewageLevelSound, true, !flag);
        if (!flag)
        {
          this.sewageLevelEmitter.VolumeFactor = 0.0f;
          this.sewageLevelEmitter.Cue.Play();
        }
      }
      return new LongRunningAction((Func<float, float, bool>) ((elapsed, __) =>
      {
        if (this.GameState.Paused || this.GameState.InMap || (this.GameState.ForceTimePaused || !this.CameraManager.ActionRunning) || this.CameraManager.Viewpoint == Viewpoint.Perspective)
          return false;
        if ((double) Math.Sign(toHeight - this.LevelManager.WaterHeight) != (double) sign || this.waterStopStack.Count > 0)
          return true;
        if (this.sewageLevelEmitter != null && !this.sewageLevelEmitter.Dead)
          this.sewageLevelEmitter.VolumeFactor = FezMath.Saturate(this.sewageLevelEmitter.VolumeFactor + elapsed * 1.25f);
        this.LevelManager.WaterSpeed = unitsPerSecond * sign;
        IGameLevelManager temp_142 = this.LevelManager;
        double temp_150 = (double) temp_142.WaterHeight + (double) this.LevelManager.WaterSpeed * (double) elapsed;
        temp_142.WaterHeight = (float) temp_150;
        foreach (TrileInstance item_0 in buoyantTriles)
        {
          if (!item_0.PhysicsState.Floating && item_0.PhysicsState.Static && (double) item_0.Center.Y < (double) this.LevelManager.WaterHeight - 0.5)
          {
            item_0.PhysicsState.ForceNonStatic = true;
            item_0.PhysicsState.Ground = new MultipleHits<TrileInstance>();
          }
        }
        return false;
      }), (Action) (() =>
      {
        this.LevelManager.WaterSpeed = 0.0f;
        if (this.LevelManager.WaterType == LiquidType.Water)
          this.GameState.SaveData.GlobalWaterLevelModifier = new float?(this.LevelManager.WaterHeight - this.LevelManager.OriginalWaterHeight);
        else
          this.GameState.SaveData.ThisLevel.LastStableLiquidHeight = new float?(this.LevelManager.WaterHeight);
        if (this.waterStopStack.Count == 0)
        {
          if (this.sewageLevelEmitter != null && !this.sewageLevelEmitter.Dead)
            this.sewageLevelEmitter.FadeOutAndDie(0.75f);
          this.sewageLevelEmitter = (SoundEmitter) null;
        }
        if (this.waterStopStack.Count <= 0)
          return;
        this.waterStopStack.Pop();
      }));
    }

    public void StopWater()
    {
      this.waterStopStack.Push(new object());
    }

    public LongRunningAction AllowPipeChangeLevel(string levelName)
    {
      this.PlayerManager.NextLevel = levelName;
      this.PlayerManager.PipeVolume = this.ScriptingManager.EvaluatedScript.InitiatingTrigger.Object.Identifier;
      return new LongRunningAction((Action) (() =>
      {
        this.PlayerManager.NextLevel = (string) null;
        this.PlayerManager.PipeVolume = new int?();
      }));
    }

    public LongRunningAction ChangeLevel(string levelName, bool asDoor, bool spin, bool trialEnding)
    {
      if (asDoor)
      {
        this.PlayerManager.SpinThroughDoor = spin;
        this.PlayerManager.NextLevel = levelName;
        this.PlayerManager.DoorVolume = this.ScriptingManager.EvaluatedScript.InitiatingTrigger.Object.Identifier;
        this.PlayerManager.DoorEndsTrial = trialEnding && this.GameState.IsTrialMode;
        IGameLevelManager levelManager = this.LevelManager;
        int num = (levelManager.WentThroughSecretPassage ? 1 : 0) | (!this.PlayerManager.DoorVolume.HasValue || this.LevelManager.Volumes[this.PlayerManager.DoorVolume.Value].ActorSettings == null ? 0 : (this.LevelManager.Volumes[this.PlayerManager.DoorVolume.Value].ActorSettings.IsSecretPassage ? 1 : 0));
        levelManager.WentThroughSecretPassage = num != 0;
        return new LongRunningAction((Action) (() =>
        {
          this.PlayerManager.NextLevel = (string) null;
          this.PlayerManager.DoorVolume = new int?();
          this.PlayerManager.DoorEndsTrial = false;
          if (ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action))
            return;
          this.LevelManager.WentThroughSecretPassage = false;
        }));
      }
      else
      {
        ServiceHelper.AddComponent((IGameComponent) new LevelTransition(ServiceHelper.Game, levelName));
        return new LongRunningAction();
      }
    }

    public LongRunningAction ChangeLevelToVolume(string levelName, int toVolume, bool asDoor, bool spin, bool trialEnding)
    {
      this.LevelManager.DestinationVolumeId = new int?(toVolume);
      LongRunningAction lra = this.ChangeLevel(levelName, asDoor, spin, trialEnding);
      return new LongRunningAction((Action) (() =>
      {
        if (lra.OnDispose != null)
          lra.OnDispose();
        if (ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action))
          return;
        this.LevelManager.DestinationVolumeId = new int?();
      }));
    }

    public LongRunningAction ReturnToLastLevel(bool asDoor, bool spin)
    {
      return this.ChangeLevel(this.LevelManager.LastLevelName, asDoor, spin, false);
    }

    public LongRunningAction ChangeToFarAwayLevel(string levelName, int toVolume, bool trialEnding)
    {
      this.LevelManager.DestinationIsFarAway = true;
      this.LevelManager.DestinationVolumeId = new int?(toVolume);
      LongRunningAction lra = this.ChangeLevel(levelName, true, false, trialEnding);
      return new LongRunningAction((Action) (() =>
      {
        if (lra.OnDispose != null)
          lra.OnDispose();
        this.LevelManager.DestinationIsFarAway = false;
        if (ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action))
          return;
        this.LevelManager.DestinationVolumeId = new int?();
      }));
    }

    public void ResolvePuzzle()
    {
      ++this.GameState.SaveData.ThisLevel.FilledConditions.SecretCount;
      List<Volume> currentVolumes = this.PlayerManager.CurrentVolumes;
      Func<Volume, bool> predicate = (Func<Volume, bool>) (x =>
      {
        if (x.ActorSettings != null)
          return x.ActorSettings.IsPointOfInterest;
        else
          return false;
      });
      Volume volume;
      if ((volume = Enumerable.FirstOrDefault<Volume>((IEnumerable<Volume>) currentVolumes, predicate)) != null && volume.Enabled)
      {
        volume.Enabled = false;
        this.GameState.SaveData.ThisLevel.InactiveVolumes.Add(volume.Id);
      }
      this.GameState.Save();
      SoundEffectExtensions.Emit(this.sSolvedSecret);
      this.SoundManager.MusicVolumeFactor = 0.125f;
      Waiters.Wait(2.75, (Action) (() => this.SoundManager.FadeVolume(0.125f, 1f, 3f))).AutoPause = true;
    }

    public void ResolvePuzzleSilent()
    {
      ++this.GameState.SaveData.ThisLevel.FilledConditions.SecretCount;
      this.GameState.Save();
    }

    public void ResolvePuzzleSoundOnly()
    {
      SoundEffectExtensions.Emit(this.sSolvedSecret);
      this.SoundManager.MusicVolumeFactor = 0.125f;
      Waiters.Wait(2.75, (Action) (() => this.SoundManager.FadeVolume(0.125f, 1f, 3f))).AutoPause = true;
    }

    public void ResetEvents()
    {
      this.Start = new Action(Util.NullAction);
    }
  }
}
