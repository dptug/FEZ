// Type: FezGame.Structure.SaveData
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezGame.Structure
{
  public class SaveData
  {
    public TimeSpan TimeOfDay = TimeSpan.FromHours(12.0);
    public List<string> UnlockedWarpDestinations = new List<string>()
    {
      "NATURE_HUB"
    };
    public List<string> Maps = new List<string>();
    public List<ActorType> Artifacts = new List<ActorType>();
    public List<string> EarnedAchievements = new List<string>();
    public List<string> EarnedGamerPictures = new List<string>();
    public Dictionary<string, LevelSaveData> World = new Dictionary<string, LevelSaveData>();
    public bool IsNew;
    public long CreationTime;
    public long PlayTime;
    public long? SinceLastSaved;
    public bool CanNewGamePlus;
    public bool IsNewGamePlus;
    public bool Finished32;
    public bool Finished64;
    public bool HasFPView;
    public bool HasStereo3D;
    public bool HasDoneHeartReboot;
    public string Level;
    public Viewpoint View;
    public Vector3 Ground;
    public int Keys;
    public int CubeShards;
    public int SecretCubes;
    public int CollectedParts;
    public int CollectedOwls;
    public int PiecesOfHeart;
    public bool ScoreDirty;
    public string ScriptingState;
    public bool FezHidden;
    public float? GlobalWaterLevelModifier;
    public bool HasHadMapHelp;
    public bool CanOpenMap;
    public bool AchievementCheatCodeDone;
    public bool MapCheatCodeDone;
    public bool AnyCodeDeciphered;
    public Dictionary<string, bool> OneTimeTutorials;

    public bool HasNewGamePlus
    {
      get
      {
        if (!this.Finished32)
          return this.Finished64;
        else
          return true;
      }
    }

    public LevelSaveData ThisLevel
    {
      get
      {
        LevelSaveData levelSaveData;
        if (this.Level != null && this.World.TryGetValue(this.Level, out levelSaveData))
          return levelSaveData;
        else
          return LevelSaveData.Default;
      }
    }

    public SaveData()
    {
      this.CreationTime = DateTime.Now.ToFileTimeUtc();
      this.Clear();
    }

    public void Clear()
    {
      this.Level = (string) null;
      this.View = Viewpoint.None;
      this.CanNewGamePlus = false;
      this.IsNewGamePlus = false;
      this.Finished32 = this.Finished64 = false;
      this.HasFPView = false;
      this.HasDoneHeartReboot = false;
      this.Ground = Vector3.Zero;
      this.TimeOfDay = TimeSpan.FromHours(12.0);
      this.UnlockedWarpDestinations = new List<string>()
      {
        "NATURE_HUB"
      };
      this.SecretCubes = this.CubeShards = this.Keys = 0;
      this.CollectedParts = this.CollectedOwls = 0;
      this.PiecesOfHeart = 0;
      this.Maps = new List<string>();
      this.Artifacts = new List<ActorType>();
      this.ScoreDirty = false;
      this.ScriptingState = (string) null;
      this.FezHidden = false;
      this.GlobalWaterLevelModifier = new float?();
      this.HasHadMapHelp = this.CanOpenMap = false;
      this.MapCheatCodeDone = this.AchievementCheatCodeDone = false;
      this.ScoreDirty = false;
      this.World = new Dictionary<string, LevelSaveData>();
      this.OneTimeTutorials = new Dictionary<string, bool>()
      {
        {
          "DOT_LOCKED_DOOR_A",
          false
        },
        {
          "DOT_NUT_N_BOLT_A",
          false
        },
        {
          "DOT_PIVOT_A",
          false
        },
        {
          "DOT_TIME_SWITCH_A",
          false
        },
        {
          "DOT_TOMBSTONE_A",
          false
        },
        {
          "DOT_TREASURE",
          false
        },
        {
          "DOT_VALVE_A",
          false
        },
        {
          "DOT_WEIGHT_SWITCH_A",
          false
        },
        {
          "DOT_LESSER_A",
          false
        },
        {
          "DOT_WARP_A",
          false
        },
        {
          "DOT_BOMB_A",
          false
        },
        {
          "DOT_CLOCK_A",
          false
        },
        {
          "DOT_CRATE_A",
          false
        },
        {
          "DOT_TELESCOPE_A",
          false
        },
        {
          "DOT_WELL_A",
          false
        },
        {
          "DOT_WORKING",
          false
        }
      };
      this.IsNew = true;
    }

    public void CloneInto(SaveData d)
    {
      d.AchievementCheatCodeDone = this.AchievementCheatCodeDone;
      d.AnyCodeDeciphered = this.AnyCodeDeciphered;
      d.CanNewGamePlus = this.CanNewGamePlus;
      d.CanOpenMap = this.CanOpenMap;
      d.CollectedOwls = this.CollectedOwls;
      d.CollectedParts = this.CollectedParts;
      d.CreationTime = this.CreationTime;
      d.CubeShards = this.CubeShards;
      d.FezHidden = this.FezHidden;
      d.Finished32 = this.Finished32;
      d.Finished64 = this.Finished64;
      d.GlobalWaterLevelModifier = this.GlobalWaterLevelModifier;
      d.Ground = this.Ground;
      d.HasDoneHeartReboot = this.HasDoneHeartReboot;
      d.HasFPView = this.HasFPView;
      d.HasHadMapHelp = this.HasHadMapHelp;
      d.HasStereo3D = this.HasStereo3D;
      d.IsNew = this.IsNew;
      d.IsNewGamePlus = this.IsNewGamePlus;
      d.Keys = this.Keys;
      d.Level = this.Level;
      d.MapCheatCodeDone = this.MapCheatCodeDone;
      d.PiecesOfHeart = this.PiecesOfHeart;
      d.PlayTime = this.PlayTime;
      d.ScoreDirty = this.ScoreDirty;
      d.ScriptingState = this.ScriptingState;
      d.SecretCubes = this.SecretCubes;
      d.SinceLastSaved = this.SinceLastSaved;
      d.TimeOfDay = this.TimeOfDay;
      d.View = this.View;
      try
      {
        d.Artifacts.Clear();
        d.Artifacts.AddRange((IEnumerable<ActorType>) this.Artifacts);
        d.EarnedAchievements.Clear();
        d.EarnedAchievements.AddRange((IEnumerable<string>) this.EarnedAchievements);
        d.EarnedGamerPictures.Clear();
        d.EarnedGamerPictures.AddRange((IEnumerable<string>) this.EarnedGamerPictures);
        d.Maps.Clear();
        d.Maps.AddRange((IEnumerable<string>) this.Maps);
        d.UnlockedWarpDestinations.Clear();
        d.UnlockedWarpDestinations.AddRange((IEnumerable<string>) this.UnlockedWarpDestinations);
        d.OneTimeTutorials.Clear();
        foreach (string key in this.OneTimeTutorials.Keys)
          d.OneTimeTutorials.Add(key, this.OneTimeTutorials[key]);
        foreach (string key in this.World.Keys)
        {
          if (!d.World.ContainsKey(key))
            d.World.Add(key, new LevelSaveData());
          this.World[key].CloneInto(d.World[key]);
        }
        foreach (string key in d.World.Keys)
        {
          if (!this.World.ContainsKey(key))
            d.World.Remove(key);
        }
      }
      catch (InvalidOperationException ex)
      {
        this.CloneInto(d);
      }
    }
  }
}
