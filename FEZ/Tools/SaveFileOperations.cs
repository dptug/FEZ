// Type: FezGame.Tools.SaveFileOperations
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using System;
using System.Collections.Generic;
using System.IO;

namespace FezGame.Tools
{
  public static class SaveFileOperations
  {
    private const long Version = 6L;

    public static void Write(CrcWriter w, SaveData sd)
    {
      w.Write(6L);
      w.Write(sd.CreationTime);
      w.Write(sd.Finished32);
      w.Write(sd.Finished64);
      w.Write(sd.HasFPView);
      w.Write(sd.HasStereo3D);
      w.Write(sd.CanNewGamePlus);
      w.Write(sd.IsNewGamePlus);
      w.Write(sd.OneTimeTutorials.Count);
      foreach (KeyValuePair<string, bool> keyValuePair in sd.OneTimeTutorials)
      {
        BinaryWritingTools.WriteObject(w, keyValuePair.Key);
        w.Write(keyValuePair.Value);
      }
      BinaryWritingTools.WriteObject(w, sd.Level);
      w.Write((int) sd.View);
      BinaryWritingTools.Write(w, sd.Ground);
      BinaryWritingTools.Write(w, sd.TimeOfDay);
      w.Write(sd.UnlockedWarpDestinations.Count);
      foreach (string s in sd.UnlockedWarpDestinations)
        BinaryWritingTools.WriteObject(w, s);
      w.Write(sd.Keys);
      w.Write(sd.CubeShards);
      w.Write(sd.SecretCubes);
      w.Write(sd.CollectedParts);
      w.Write(sd.CollectedOwls);
      w.Write(sd.PiecesOfHeart);
      w.Write(sd.Maps.Count);
      foreach (string s in sd.Maps)
        BinaryWritingTools.WriteObject(w, s);
      w.Write(sd.Artifacts.Count);
      foreach (ActorType actorType in sd.Artifacts)
        w.Write((int) actorType);
      w.Write(sd.EarnedAchievements.Count);
      foreach (string s in sd.EarnedAchievements)
        BinaryWritingTools.WriteObject(w, s);
      w.Write(sd.EarnedGamerPictures.Count);
      foreach (string s in sd.EarnedGamerPictures)
        BinaryWritingTools.WriteObject(w, s);
      BinaryWritingTools.WriteObject(w, sd.ScriptingState);
      w.Write(sd.FezHidden);
      BinaryWritingTools.WriteObject(w, sd.GlobalWaterLevelModifier);
      w.Write(sd.HasHadMapHelp);
      w.Write(sd.CanOpenMap);
      w.Write(sd.AchievementCheatCodeDone);
      w.Write(sd.AnyCodeDeciphered);
      w.Write(sd.MapCheatCodeDone);
      w.Write(sd.World.Count);
      foreach (KeyValuePair<string, LevelSaveData> keyValuePair in sd.World)
      {
        BinaryWritingTools.WriteObject(w, keyValuePair.Key);
        SaveFileOperations.Write(w, keyValuePair.Value);
      }
      w.Write(sd.ScoreDirty);
      w.Write(sd.HasDoneHeartReboot);
      w.Write(sd.PlayTime);
      w.Write(sd.IsNew);
    }

    public static void Write(CrcWriter w, LevelSaveData lsd)
    {
      w.Write(lsd.DestroyedTriles.Count);
      foreach (TrileEmplacement s in lsd.DestroyedTriles)
        BinaryWritingTools.Write(w, s);
      w.Write(lsd.InactiveTriles.Count);
      foreach (TrileEmplacement s in lsd.InactiveTriles)
        BinaryWritingTools.Write(w, s);
      w.Write(lsd.InactiveArtObjects.Count);
      foreach (int num in lsd.InactiveArtObjects)
        w.Write(num);
      w.Write(lsd.InactiveEvents.Count);
      foreach (int num in lsd.InactiveEvents)
        w.Write(num);
      w.Write(lsd.InactiveGroups.Count);
      foreach (int num in lsd.InactiveGroups)
        w.Write(num);
      w.Write(lsd.InactiveVolumes.Count);
      foreach (int num in lsd.InactiveVolumes)
        w.Write(num);
      w.Write(lsd.InactiveNPCs.Count);
      foreach (int num in lsd.InactiveNPCs)
        w.Write(num);
      w.Write(lsd.PivotRotations.Count);
      foreach (KeyValuePair<int, int> keyValuePair in lsd.PivotRotations)
      {
        w.Write(keyValuePair.Key);
        w.Write(keyValuePair.Value);
      }
      BinaryWritingTools.WriteObject(w, lsd.LastStableLiquidHeight);
      BinaryWritingTools.WriteObject(w, lsd.ScriptingState);
      w.Write(lsd.FirstVisit);
      SaveFileOperations.Write(w, lsd.FilledConditions);
    }

    public static void Write(CrcWriter w, WinConditions wc)
    {
      w.Write(wc.LockedDoorCount);
      w.Write(wc.UnlockedDoorCount);
      w.Write(wc.ChestCount);
      w.Write(wc.CubeShardCount);
      w.Write(wc.OtherCollectibleCount);
      w.Write(wc.SplitUpCount);
      w.Write(wc.ScriptIds.Count);
      foreach (int num in wc.ScriptIds)
        w.Write(num);
      w.Write(wc.SecretCount);
    }

    public static SaveData Read(CrcReader r)
    {
      SaveData saveData = new SaveData();
      long num1 = r.ReadInt64();
      if (num1 != 6L)
      {
        throw new IOException("Invalid version : " + (object) num1 + " (expected " + (string) (object) 6 + ")");
      }
      else
      {
        saveData.CreationTime = r.ReadInt64();
        saveData.Finished32 = r.ReadBoolean();
        saveData.Finished64 = r.ReadBoolean();
        saveData.HasFPView = r.ReadBoolean();
        saveData.HasStereo3D = r.ReadBoolean();
        saveData.CanNewGamePlus = r.ReadBoolean();
        saveData.IsNewGamePlus = r.ReadBoolean();
        saveData.OneTimeTutorials.Clear();
        int num2;
        saveData.OneTimeTutorials = new Dictionary<string, bool>(num2 = r.ReadInt32());
        for (int index = 0; index < num2; ++index)
          saveData.OneTimeTutorials.Add(BinaryWritingTools.ReadNullableString(r), r.ReadBoolean());
        saveData.Level = BinaryWritingTools.ReadNullableString(r);
        saveData.View = (Viewpoint) r.ReadInt32();
        saveData.Ground = BinaryWritingTools.ReadVector3(r);
        saveData.TimeOfDay = BinaryWritingTools.ReadTimeSpan(r);
        int num3;
        saveData.UnlockedWarpDestinations = new List<string>(num3 = r.ReadInt32());
        for (int index = 0; index < num3; ++index)
          saveData.UnlockedWarpDestinations.Add(BinaryWritingTools.ReadNullableString(r));
        saveData.Keys = r.ReadInt32();
        saveData.CubeShards = r.ReadInt32();
        saveData.SecretCubes = r.ReadInt32();
        saveData.CollectedParts = r.ReadInt32();
        saveData.CollectedOwls = r.ReadInt32();
        saveData.PiecesOfHeart = r.ReadInt32();
        if (saveData.SecretCubes > 32 || saveData.CubeShards > 32 || saveData.PiecesOfHeart > 3)
          saveData.ScoreDirty = true;
        saveData.SecretCubes = Math.Min(saveData.SecretCubes, 32);
        saveData.CubeShards = Math.Min(saveData.CubeShards, 32);
        saveData.PiecesOfHeart = Math.Min(saveData.PiecesOfHeart, 3);
        int num4;
        saveData.Maps = new List<string>(num4 = r.ReadInt32());
        for (int index = 0; index < num4; ++index)
          saveData.Maps.Add(BinaryWritingTools.ReadNullableString(r));
        int num5;
        saveData.Artifacts = new List<ActorType>(num5 = r.ReadInt32());
        for (int index = 0; index < num5; ++index)
          saveData.Artifacts.Add((ActorType) r.ReadInt32());
        int num6;
        saveData.EarnedAchievements = new List<string>(num6 = r.ReadInt32());
        for (int index = 0; index < num6; ++index)
          saveData.EarnedAchievements.Add(BinaryWritingTools.ReadNullableString(r));
        int num7;
        saveData.EarnedGamerPictures = new List<string>(num7 = r.ReadInt32());
        for (int index = 0; index < num7; ++index)
          saveData.EarnedGamerPictures.Add(BinaryWritingTools.ReadNullableString(r));
        saveData.ScriptingState = BinaryWritingTools.ReadNullableString(r);
        saveData.FezHidden = r.ReadBoolean();
        saveData.GlobalWaterLevelModifier = BinaryWritingTools.ReadNullableSingle(r);
        saveData.HasHadMapHelp = r.ReadBoolean();
        saveData.CanOpenMap = r.ReadBoolean();
        saveData.AchievementCheatCodeDone = r.ReadBoolean();
        saveData.AnyCodeDeciphered = r.ReadBoolean();
        saveData.MapCheatCodeDone = r.ReadBoolean();
        int num8;
        saveData.World = new Dictionary<string, LevelSaveData>(num8 = r.ReadInt32());
        for (int index = 0; index < num8; ++index)
        {
          try
          {
            saveData.World.Add(BinaryWritingTools.ReadNullableString(r), SaveFileOperations.ReadLevel(r));
          }
          catch (Exception ex)
          {
            break;
          }
        }
        r.ReadBoolean();
        saveData.ScoreDirty = true;
        saveData.HasDoneHeartReboot = r.ReadBoolean();
        saveData.PlayTime = r.ReadInt64();
        saveData.IsNew = string.IsNullOrEmpty(saveData.Level) || saveData.Level == "GOMEZ_HOUSE_2D";
        return saveData;
      }
    }

    public static LevelSaveData ReadLevel(CrcReader r)
    {
      LevelSaveData levelSaveData = new LevelSaveData();
      int num1;
      levelSaveData.DestroyedTriles = new List<TrileEmplacement>(num1 = r.ReadInt32());
      for (int index = 0; index < num1; ++index)
        levelSaveData.DestroyedTriles.Add(BinaryWritingTools.ReadTrileEmplacement(r));
      int num2;
      levelSaveData.InactiveTriles = new List<TrileEmplacement>(num2 = r.ReadInt32());
      for (int index = 0; index < num2; ++index)
        levelSaveData.InactiveTriles.Add(BinaryWritingTools.ReadTrileEmplacement(r));
      int num3;
      levelSaveData.InactiveArtObjects = new List<int>(num3 = r.ReadInt32());
      for (int index = 0; index < num3; ++index)
        levelSaveData.InactiveArtObjects.Add(r.ReadInt32());
      int num4;
      levelSaveData.InactiveEvents = new List<int>(num4 = r.ReadInt32());
      for (int index = 0; index < num4; ++index)
        levelSaveData.InactiveEvents.Add(r.ReadInt32());
      int num5;
      levelSaveData.InactiveGroups = new List<int>(num5 = r.ReadInt32());
      for (int index = 0; index < num5; ++index)
        levelSaveData.InactiveGroups.Add(r.ReadInt32());
      int num6;
      levelSaveData.InactiveVolumes = new List<int>(num6 = r.ReadInt32());
      for (int index = 0; index < num6; ++index)
        levelSaveData.InactiveVolumes.Add(r.ReadInt32());
      int num7;
      levelSaveData.InactiveNPCs = new List<int>(num7 = r.ReadInt32());
      for (int index = 0; index < num7; ++index)
        levelSaveData.InactiveNPCs.Add(r.ReadInt32());
      int num8;
      levelSaveData.PivotRotations = new Dictionary<int, int>(num8 = r.ReadInt32());
      for (int index = 0; index < num8; ++index)
        levelSaveData.PivotRotations.Add(r.ReadInt32(), r.ReadInt32());
      levelSaveData.LastStableLiquidHeight = BinaryWritingTools.ReadNullableSingle(r);
      levelSaveData.ScriptingState = BinaryWritingTools.ReadNullableString(r);
      levelSaveData.FirstVisit = r.ReadBoolean();
      levelSaveData.FilledConditions = SaveFileOperations.ReadWonditions(r);
      return levelSaveData;
    }

    public static WinConditions ReadWonditions(CrcReader r)
    {
      WinConditions winConditions = new WinConditions();
      winConditions.LockedDoorCount = r.ReadInt32();
      winConditions.UnlockedDoorCount = r.ReadInt32();
      winConditions.ChestCount = r.ReadInt32();
      winConditions.CubeShardCount = r.ReadInt32();
      winConditions.OtherCollectibleCount = r.ReadInt32();
      winConditions.SplitUpCount = r.ReadInt32();
      int num;
      winConditions.ScriptIds = new List<int>(num = r.ReadInt32());
      for (int index = 0; index < num; ++index)
        winConditions.ScriptIds.Add(r.ReadInt32());
      winConditions.SecretCount = r.ReadInt32();
      return winConditions;
    }
  }
}
