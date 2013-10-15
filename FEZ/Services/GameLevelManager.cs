// Type: FezGame.Services.GameLevelManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Components.Scripting;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Services
{
  public class GameLevelManager : LevelManager, IGameLevelManager, ILevelManager
  {
    private readonly List<string> DotLoadLevels = new List<string>()
    {
      "MEMORY_CORE+NATURE_HUB",
      "NATURE_HUB+MEMORY_CORE",
      "MEMORY_CORE+ZU_CITY",
      "ZU_CITY+MEMORY_CORE",
      "MEMORY_CORE+WALL_VILLAGE",
      "WALL_VILLAGE+MEMORY_CORE",
      "MEMORY_CORE+INDUSTRIAL_CITY",
      "INDUSTRIAL_CITY+MEMORY_CORE",
      "PIVOT_WATERTOWER+INDUSTRIAL_HUB",
      "INDUSTRIAL_HUB+PIVOT_WATERTOWER",
      "WELL_2+SEWER_START",
      "SEWER_START+WELL_2",
      "GRAVE_CABIN+GRAVEYARD_GATE",
      "GRAVEYARD_GATE+GRAVE_CABIN",
      "TREE+TREE_SKY",
      "TREE_SKY+TREE",
      "WATERFALL+MINE_A",
      "MINE_A+WATERFALL",
      "SEWER_TO_LAVA+LAVA",
      "LAVA+SEWER_TO_LAVA"
    };
    private readonly Dictionary<TrileInstance, TrileGroup> pickupGroups = new Dictionary<TrileInstance, TrileGroup>();
    private Level oldLevel;

    public bool SongChanged { get; set; }

    public string LastLevelName { get; set; }

    public bool DestinationIsFarAway { get; set; }

    public int? DestinationVolumeId { get; set; }

    public bool WentThroughSecretPassage { get; set; }

    public IDictionary<TrileInstance, TrileGroup> PickupGroups
    {
      get
      {
        return (IDictionary<TrileInstance, TrileGroup>) this.pickupGroups;
      }
    }

    [ServiceDependency]
    public IDotManager DotManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IPhysicsManager PhysicsManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    [ServiceDependency]
    public IGameService GameService { private get; set; }

    [ServiceDependency]
    public ILevelService LevelService { private get; set; }

    public GameLevelManager(Game game)
      : base(game)
    {
    }

    public override void Load(string levelName)
    {
      levelName = levelName.Replace('\\', '/');
      string str = levelName;
      Level level;
      using (MemoryContentManager memoryContentManager = new MemoryContentManager((IServiceProvider) this.Game.Services, this.Game.Content.RootDirectory))
      {
        if (!string.IsNullOrEmpty(this.Name))
          levelName = this.Name.Substring(0, this.Name.LastIndexOf("/") + 1) + levelName.Substring(levelName.LastIndexOf("/") + 1);
        if (!MemoryContentManager.AssetExists("Levels" + (object) '\\' + levelName.Replace('/', '\\')))
          levelName = str;
        try
        {
          level = memoryContentManager.Load<Level>("Levels/" + levelName);
        }
        catch (Exception ex)
        {
          Logger.LogError(ex);
          this.oldLevel = new Level();
          return;
        }
      }
      level.Name = levelName;
      ContentManager forLevel = this.CMProvider.GetForLevel(levelName);
      foreach (ArtObjectInstance artObjectInstance in level.ArtObjects.Values)
        artObjectInstance.ArtObject = forLevel.Load<ArtObject>(string.Format("{0}/{1}", (object) "Art Objects", (object) artObjectInstance.ArtObjectName));
      if (level.Sky == null)
        level.Sky = forLevel.Load<Sky>("Skies/" + level.SkyName);
      if (level.TrileSetName != null)
        level.TrileSet = forLevel.Load<TrileSet>("Trile Sets/" + level.TrileSetName);
      if (level.SongName != null)
      {
        level.Song = forLevel.Load<TrackedSong>("Music/" + level.SongName);
        level.Song.Initialize();
      }
      if (this.levelData != null)
        this.GameState.SaveData.ThisLevel.FirstVisit = false;
      this.ClearArtSatellites();
      this.oldLevel = this.levelData ?? new Level();
      this.levelData = level;
    }

    public override void Rebuild()
    {
      this.OnSkyChanged();
      this.LevelMaterializer.ClearBatches();
      this.LevelMaterializer.RebuildTriles(this.levelData.TrileSet, this.levelData.TrileSet == this.oldLevel.TrileSet);
      this.LevelMaterializer.RebuildInstances();
      if (!this.Quantum)
        this.LevelMaterializer.CleanUp();
      this.LevelMaterializer.InitializeArtObjects();
      foreach (BackgroundPlane backgroundPlane in this.levelData.BackgroundPlanes.Values)
      {
        backgroundPlane.HostMesh = backgroundPlane.Animated ? this.LevelMaterializer.AnimatedPlanesMesh : this.LevelMaterializer.StaticPlanesMesh;
        backgroundPlane.Initialize();
      }
      if (!this.levelData.BackgroundPlanes.ContainsKey(-1) && this.GomezHaloName != null)
        this.levelData.BackgroundPlanes.Add(-1, new BackgroundPlane(this.LevelMaterializer.StaticPlanesMesh, this.GomezHaloName, false)
        {
          Id = -1,
          LightMap = true,
          AlwaysOnTop = true,
          Billboard = true,
          Filter = this.HaloFiltering ? new Color(0.425f, 0.425f, 0.425f, 1f) : new Color(0.5f, 0.5f, 0.5f, 1f),
          PixelatedLightmap = !this.HaloFiltering
        });
      this.pickupGroups.Clear();
      foreach (TrileGroup trileGroup in (IEnumerable<TrileGroup>) this.Groups.Values)
      {
        if (trileGroup.ActorType != ActorType.SuckBlock && Enumerable.All<TrileInstance>((IEnumerable<TrileInstance>) trileGroup.Triles, (Func<TrileInstance, bool>) (x =>
        {
          if (ActorTypeExtensions.IsPickable(x.Trile.ActorSettings.Type))
            return x.Trile.ActorSettings.Type != ActorType.Couch;
          else
            return false;
        })))
        {
          foreach (TrileInstance key in trileGroup.Triles)
            this.pickupGroups.Add(key, trileGroup);
        }
      }
      this.SongChanged = this.Song == null != (this.SoundManager.CurrentlyPlayingSong == null) || this.Song != null && this.SoundManager.CurrentlyPlayingSong != null && this.Song.Name != this.SoundManager.CurrentlyPlayingSong.Name;
      if (this.SongChanged)
      {
        this.SoundManager.ScriptChangedSong = false;
        if (!this.GameState.InCutscene || this.GameState.IsTrialMode)
          Waiters.Wait((Func<bool>) (() =>
          {
            if (!this.GameState.Loading)
              return !this.GameState.FarawaySettings.InTransition;
            else
              return false;
          }), (Action) (() =>
          {
            if (!this.SoundManager.ScriptChangedSong)
              this.SoundManager.PlayNewSong(8f);
            this.SoundManager.ScriptChangedSong = false;
          }));
      }
      else if (this.Song != null)
      {
        if (!this.GameState.DotLoading)
          this.SoundManager.UpdateSongActiveTracks();
        else
          Waiters.Wait((Func<bool>) (() => !this.GameState.Loading), (Action) (() => this.SoundManager.UpdateSongActiveTracks()));
      }
      this.SoundManager.FadeFrequencies(this.LowPass, 2f);
      this.SoundManager.UnmuteAmbienceTracks();
      if (!this.GameState.InCutscene || this.GameState.IsTrialMode)
        Waiters.Wait((Func<bool>) (() =>
        {
          if (!this.GameState.Loading)
            return !this.GameState.FarawaySettings.InTransition;
          else
            return false;
        }), (Action) (() => this.SoundManager.PlayNewAmbience()));
      this.oldLevel = (Level) null;
      this.FullPath = this.Name;
    }

    public void ChangeLevel(string levelName)
    {
      this.GameState.SaveToCloud(false);
      this.GameState.DotLoading = this.DotLoadLevels.Contains(this.Name + "+" + levelName) || this.PlayerManager.Action == ActionType.LesserWarp || this.PlayerManager.Action == ActionType.GateWarp;
      if (this.GameState.DotLoading)
      {
        this.SoundManager.PlayNewSong((string) null, 1f);
        List<AmbienceTrack> ambienceTracks = this.levelData.AmbienceTracks;
        this.levelData.AmbienceTracks = new List<AmbienceTrack>();
        this.SoundManager.PlayNewAmbience();
        this.levelData.AmbienceTracks = ambienceTracks;
      }
      this.GameService.CloseScroll((string) null);
      if (levelName == this.Name && this.DestinationVolumeId.HasValue && this.Volumes.ContainsKey(this.DestinationVolumeId.Value))
      {
        this.LastLevelName = this.Name;
        Volume volume = this.Volumes[this.DestinationVolumeId.Value];
        Viewpoint view = FezMath.AsViewpoint(Enumerable.FirstOrDefault<FaceOrientation>((IEnumerable<FaceOrientation>) volume.Orientations));
        this.CameraManager.ChangeViewpoint(view, 1.5f);
        Vector3 position = (volume.BoundingBox.Min + volume.BoundingBox.Max) / 2f + new Vector3(1.0 / 1000.0);
        position.Y = volume.BoundingBox.Min.Y - 0.25f;
        TrileInstance trileInstance = this.NearestTrile(position, QueryOptions.None, new Viewpoint?(view)).Deep;
        this.GameState.SaveData.Ground = trileInstance.Center;
        this.GameState.SaveData.View = view;
        float num = this.PlayerManager.Position.Y;
        this.PlayerManager.Position = trileInstance.Center + (trileInstance.TransformedSize / 2f + this.PlayerManager.Size / 2f) * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor);
        this.PlayerManager.WallCollision = new MultipleHits<CollisionResult>();
        this.PlayerManager.Ground = new MultipleHits<TrileInstance>();
        this.PlayerManager.Velocity = (float) (3.15000009536743 * (double) Math.Sign(this.CollisionManager.GravityFactor) * 0.150000005960464 * 0.0166666675359011) * -Vector3.UnitY;
        this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
        this.PlayerManager.Velocity = (float) (3.15000009536743 * (double) Math.Sign(this.CollisionManager.GravityFactor) * 0.150000005960464 * 0.0166666675359011) * -Vector3.UnitY;
        Vector3 originalCenter = this.CameraManager.Center;
        float diff = this.PlayerManager.Position.Y - num;
        Waiters.Interpolate(1.5, (Action<float>) (s => this.CameraManager.Center = new Vector3(originalCenter.X, originalCenter.Y + diff / 2f * Easing.EaseInOut((double) s, EasingType.Sine), originalCenter.Z)));
        this.OnLevelChanging();
        this.OnLevelChanged();
      }
      else
      {
        bool flag1 = this.GameState.SaveData.World.Count > 0;
        string str1 = this.GameState.SaveData.Level;
        this.LastLevelName = !flag1 ? (string) null : this.Name;
        this.Load(levelName);
        this.Rebuild();
        if (!this.GameState.SaveData.World.ContainsKey(this.Name))
          this.GameState.SaveData.World.Add(this.Name, new LevelSaveData()
          {
            FirstVisit = true
          });
        this.GameState.SaveData.Level = this.Name;
        this.OnLevelChanging();
        LevelSaveData thisLevel = this.GameState.SaveData.ThisLevel;
        foreach (TrileEmplacement emplacement in thisLevel.DestroyedTriles)
          this.ClearTrile(emplacement);
        foreach (int num in thisLevel.InactiveArtObjects)
        {
          if (num < 0)
            this.RemoveArtObject(this.ArtObjects[-(num + 1)]);
        }
        TrileInstance trileInstance = !flag1 || !(str1 == levelName) ? (TrileInstance) null : this.ActualInstanceAt(this.GameState.SaveData.Ground);
        float? nullable = new float?();
        Viewpoint spawnView = !flag1 || !(str1 == levelName) ? Viewpoint.Left : this.GameState.SaveData.View;
        bool flag2 = false;
        if (this.LastLevelName != null)
        {
          Volume volume = (Volume) null;
          if (this.DestinationVolumeId.HasValue && this.DestinationVolumeId.Value != -1 && this.Volumes.ContainsKey(this.DestinationVolumeId.Value))
          {
            volume = this.Volumes[this.DestinationVolumeId.Value];
            flag2 = true;
            this.DestinationVolumeId = new int?();
          }
          else
          {
            string str2 = this.LastLevelName.Replace('\\', '/');
            string trimmedLln = str2.Substring(str2.LastIndexOf('/') + 1);
            foreach (Script script in Enumerable.Where<Script>(Enumerable.Where<Script>((IEnumerable<Script>) this.Scripts.Values, (Func<Script, bool>) (s => Enumerable.Any<ScriptTrigger>((IEnumerable<ScriptTrigger>) s.Triggers, (Func<ScriptTrigger, bool>) (t => t.Object.Type == "Volume")))), (Func<Script, bool>) (s => Enumerable.Any<ScriptAction>((IEnumerable<ScriptAction>) s.Actions, (Func<ScriptAction, bool>) (a =>
            {
              if (!(a.Object.Type == "Level") || !a.Operation.Contains("ChangeLevel"))
                return false;
              if (!(a.Arguments[0] == this.LastLevelName))
                return a.Arguments[0] == trimmedLln;
              else
                return true;
            })))))
            {
              int key = Enumerable.First<ScriptTrigger>(Enumerable.Where<ScriptTrigger>((IEnumerable<ScriptTrigger>) script.Triggers, (Func<ScriptTrigger, bool>) (x => x.Object.Type == "Volume"))).Object.Identifier.Value;
              if (this.Volumes.ContainsKey(key))
              {
                volume = this.Volumes[key];
                flag2 = true;
              }
            }
          }
          if (flag2 && volume != null)
          {
            Vector3 vector3 = (volume.BoundingBox.Min + volume.BoundingBox.Max) / 2f + new Vector3(1.0 / 1000.0);
            vector3.Y = volume.BoundingBox.Min.Y - 0.25f;
            spawnView = FezMath.AsViewpoint(Enumerable.FirstOrDefault<FaceOrientation>((IEnumerable<FaceOrientation>) volume.Orientations));
            nullable = new float?(FezMath.Dot(vector3, FezMath.SideMask(spawnView)));
            float num = (float) ((double) FezMath.Dot(volume.BoundingBox.Max - volume.BoundingBox.Min, FezMath.DepthMask(spawnView)) / 2.0 + 0.5);
            Vector3 position = vector3 + num * -FezMath.ForwardVector(spawnView);
            foreach (TrileEmplacement trileEmplacement in Enumerable.Union<TrileEmplacement>((IEnumerable<TrileEmplacement>) thisLevel.InactiveTriles, (IEnumerable<TrileEmplacement>) thisLevel.DestroyedTriles))
            {
              if ((double) Vector3.DistanceSquared(trileEmplacement.AsVector, position) < 2.0)
              {
                position -= FezMath.ForwardVector(spawnView);
                break;
              }
            }
            trileInstance = this.ActualInstanceAt(position) ?? this.NearestTrile(vector3, QueryOptions.None, new Viewpoint?(spawnView)).Deep;
          }
        }
        InstanceFace instanceFace = new InstanceFace();
        if (!flag1 || trileInstance == null)
        {
          if (this.StartingPosition != (TrileFace) null)
          {
            instanceFace.Instance = this.TrileInstanceAt(ref this.StartingPosition.Id);
            instanceFace.Face = this.StartingPosition.Face;
          }
          else
            instanceFace.Face = FezMath.VisibleOrientation(spawnView);
          if (instanceFace.Instance == null)
            instanceFace.Instance = Enumerable.FirstOrDefault<TrileInstance>((IEnumerable<TrileInstance>) Enumerable.OrderBy<TrileInstance, float>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.Triles.Values, (Func<TrileInstance, bool>) (x => !FezMath.In<CollisionType>(x.GetRotatedFace(FezMath.VisibleOrientation(spawnView)), CollisionType.None, CollisionType.Immaterial, (IEqualityComparer<CollisionType>) CollisionTypeComparer.Default))), (Func<TrileInstance, float>) (x => Math.Abs(FezMath.Dot(x.Center - this.Size / 2f, FezMath.ScreenSpaceMask(spawnView))))));
          trileInstance = instanceFace.Instance;
          spawnView = FezMath.AsViewpoint(instanceFace.Face);
        }
        this.CameraManager.Constrained = false;
        this.CameraManager.PanningConstraints = new Vector2?();
        if (trileInstance != null)
          this.GameState.SaveData.Ground = trileInstance.Center;
        this.GameState.SaveData.View = spawnView;
        this.GameState.SaveData.TimeOfDay = this.TimeManager.CurrentTime.TimeOfDay;
        if (flag2)
          this.PlayerManager.CheckpointGround = (TrileInstance) null;
        this.PlayerManager.RespawnAtCheckpoint();
        if (nullable.HasValue)
          this.PlayerManager.Position = this.PlayerManager.Position * (Vector3.One - FezMath.SideMask(spawnView)) + nullable.Value * FezMath.SideMask(spawnView);
        this.PlayerManager.Action = ActionType.Idle;
        this.PlayerManager.WallCollision = new MultipleHits<CollisionResult>();
        this.PlayerManager.Ground = new MultipleHits<TrileInstance>();
        this.PlayerManager.Velocity = (float) (3.15000009536743 * (double) Math.Sign(this.CollisionManager.GravityFactor) * 0.150000005960464 * 0.0166666675359011) * -Vector3.UnitY;
        this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
        this.PlayerManager.Velocity = (float) (3.15000009536743 * (double) Math.Sign(this.CollisionManager.GravityFactor) * 0.150000005960464 * 0.0166666675359011) * -Vector3.UnitY;
        this.CameraManager.InterpolatedCenter = this.CameraManager.Center = this.PlayerManager.Position;
        this.OnLevelChanged();
        this.LevelService.OnStart();
        ScriptingHost.Instance.ForceUpdate(new GameTime());
        if (!this.PlayerManager.SpinThroughDoor)
        {
          if (!this.CameraManager.Constrained)
          {
            this.CameraManager.Center = this.PlayerManager.Position + (float) (4.0 * (this.Descending ? -1.0 : 1.0)) / this.CameraManager.PixelsPerTrixel * Vector3.UnitY;
            this.CameraManager.SnapInterpolation();
          }
          if (!this.GameState.FarawaySettings.InTransition)
            this.LevelMaterializer.ForceCull();
        }
        if (this.Name != "HEX_REBUILD" && this.Name != "DRUM" && (this.Name != "VILLAGEVILLE_3D_END_64" && this.Name != "VILLAGEVILLE_3D_END_32"))
          this.GameState.Save();
        GC.Collect(3);
      }
    }

    public void ChangeSky(Sky sky)
    {
      this.levelData.Sky = sky;
      this.OnSkyChanged();
    }

    public override void RecordMoveToEnd(int groupId)
    {
      this.GameState.SaveData.ThisLevel.InactiveGroups.Add(groupId);
      this.GameState.Save();
    }

    public override bool IsPathRecorded(int groupId)
    {
      return this.GameState.SaveData.ThisLevel.InactiveGroups.Contains(groupId);
    }

    public void Reset()
    {
      this.ClearArtSatellites();
      if (this.levelData.TrileSet != null)
        this.LevelMaterializer.DestroyMaterializers(this.levelData.TrileSet);
      this.levelData = new Level()
      {
        Name = string.Empty
      };
      this.levelData.Sky = this.CMProvider.Global.Load<Sky>("Skies/Default");
      this.OnSkyChanged();
      this.levelData.TrileSet = (TrileSet) null;
      this.LevelMaterializer.RebuildInstances();
      this.LevelMaterializer.CullInstances();
      this.LastLevelName = (string) null;
      this.OnLevelChanging();
      this.OnLevelChanged();
      this.LevelMaterializer.TrilesMesh.Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/FullWhite"));
    }

    public void RemoveArtObject(ArtObjectInstance aoInstance)
    {
      if (aoInstance.ActorSettings.AttachedGroup.HasValue)
      {
        int key = aoInstance.ActorSettings.AttachedGroup.Value;
        foreach (TrileInstance instance in this.Groups[aoInstance.ActorSettings.AttachedGroup.Value].Triles.ToArray())
          this.ClearTrile(instance);
        this.Groups.Remove(key);
      }
      this.ArtObjects.Remove(aoInstance.Id);
      aoInstance.Dispose();
      this.LevelMaterializer.RegisterSatellites();
    }

    public override bool WasPathSupposedToBeRecorded(int id)
    {
      switch (this.Name)
      {
        case "OWL":
          if (id == 0 && this.GameState.SaveData.ThisLevel.ScriptingState == "4")
          {
            this.RecordMoveToEnd(id);
            return true;
          }
          else
            break;
        case "ARCH":
          if (id == 3 && this.GameState.SaveData.ThisLevel.InactiveGroups.Contains(4))
          {
            this.RecordMoveToEnd(id);
            return true;
          }
          else
            break;
        case "WATERFALL":
          if (id == 1 && this.GameState.SaveData.ThisLevel.InactiveVolumes.Contains(19))
          {
            this.RecordMoveToEnd(id);
            return true;
          }
          else
            break;
      }
      return false;
    }
  }
}
