// Type: FezEngine.Services.LevelManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FezEngine.Services
{
  public abstract class LevelManager : GameComponent, ILevelManager
  {
    private static readonly object Mutex = new object();
    private Dictionary<Point, Limit> screenSpaceLimits = new Dictionary<Point, Limit>((IEqualityComparer<Point>) LevelManager.FastPointComparer.Default);
    private const bool AccurateCollision = false;
    private readonly Trile fallbackTrile;
    protected Level levelData;
    private Color actualAmbient;
    private Color actualDiffuse;
    private Worker<Dictionary<Point, Limit>> screenInvalidationWorker;

    public TrileFace StartingPosition
    {
      get
      {
        return this.levelData.StartingPosition;
      }
      set
      {
        this.levelData.StartingPosition = value;
      }
    }

    public string Name
    {
      get
      {
        return this.levelData.Name;
      }
      set
      {
        this.levelData.Name = value;
      }
    }

    public string FullPath { get; set; }

    public bool SkipPostProcess
    {
      get
      {
        return this.levelData.SkipPostProcess;
      }
      set
      {
        this.levelData.SkipPostProcess = value;
      }
    }

    public virtual float BaseAmbient
    {
      get
      {
        return this.levelData.BaseAmbient;
      }
      set
      {
        this.levelData.BaseAmbient = value;
      }
    }

    public virtual float BaseDiffuse
    {
      get
      {
        return this.levelData.BaseDiffuse;
      }
      set
      {
        this.levelData.BaseDiffuse = value;
      }
    }

    public Color ActualAmbient
    {
      get
      {
        return this.actualAmbient;
      }
      set
      {
        bool flag = this.actualAmbient != value;
        this.actualAmbient = value;
        if (!flag)
          return;
        this.OnLightingChanged();
      }
    }

    public Color ActualDiffuse
    {
      get
      {
        return this.actualDiffuse;
      }
      set
      {
        bool flag = this.actualDiffuse != value;
        this.actualDiffuse = value;
        if (!flag)
          return;
        this.OnLightingChanged();
      }
    }

    public bool Flat
    {
      get
      {
        return this.levelData.Flat;
      }
      set
      {
        this.levelData.Flat = value;
      }
    }

    public Vector3 Size
    {
      get
      {
        return this.levelData.Size;
      }
      set
      {
        this.levelData.Size = value;
      }
    }

    public string SequenceSamplesPath
    {
      get
      {
        return this.levelData.SequenceSamplesPath;
      }
      set
      {
        this.levelData.SequenceSamplesPath = value;
      }
    }

    public bool HaloFiltering
    {
      get
      {
        return this.levelData.HaloFiltering;
      }
      set
      {
        this.levelData.HaloFiltering = value;
      }
    }

    public string GomezHaloName
    {
      get
      {
        return this.levelData.GomezHaloName;
      }
      set
      {
        this.levelData.GomezHaloName = value;
      }
    }

    public bool BlinkingAlpha
    {
      get
      {
        return this.levelData.BlinkingAlpha;
      }
      set
      {
        this.levelData.BlinkingAlpha = value;
      }
    }

    public bool Loops
    {
      get
      {
        return this.levelData.Loops;
      }
      set
      {
        this.levelData.Loops = value;
      }
    }

    public float WaterHeight
    {
      get
      {
        return this.levelData.WaterHeight;
      }
      set
      {
        this.levelData.WaterHeight = value;
      }
    }

    public float OriginalWaterHeight { get; set; }

    public float WaterSpeed { get; set; }

    public LiquidType WaterType
    {
      get
      {
        return this.levelData.WaterType;
      }
      set
      {
        this.levelData.WaterType = value;
      }
    }

    public bool Descending
    {
      get
      {
        return this.levelData.Descending;
      }
      set
      {
        this.levelData.Descending = value;
      }
    }

    public bool Rainy
    {
      get
      {
        return this.levelData.Rainy;
      }
      set
      {
        this.levelData.Rainy = value;
      }
    }

    public string SongName
    {
      get
      {
        return this.levelData.SongName;
      }
      set
      {
        this.levelData.SongName = value;
      }
    }

    public bool LowPass
    {
      get
      {
        return this.levelData.LowPass;
      }
      set
      {
        this.levelData.LowPass = value;
      }
    }

    public LevelNodeType NodeType
    {
      get
      {
        return this.levelData.NodeType;
      }
      set
      {
        this.levelData.NodeType = value;
      }
    }

    public int FAPFadeOutStart
    {
      get
      {
        return this.levelData.FAPFadeOutStart;
      }
      set
      {
        this.levelData.FAPFadeOutStart = value;
      }
    }

    public int FAPFadeOutLength
    {
      get
      {
        return this.levelData.FAPFadeOutLength;
      }
      set
      {
        this.levelData.FAPFadeOutLength = value;
      }
    }

    public bool Quantum
    {
      get
      {
        return this.levelData.Quantum;
      }
      set
      {
        this.levelData.Quantum = value;
      }
    }

    public IDictionary<TrileEmplacement, TrileInstance> Triles
    {
      get
      {
        return (IDictionary<TrileEmplacement, TrileInstance>) this.levelData.Triles;
      }
    }

    public Sky Sky
    {
      get
      {
        return this.levelData.Sky;
      }
    }

    public TrileSet TrileSet
    {
      get
      {
        return this.levelData.TrileSet;
      }
    }

    public TrackedSong Song
    {
      get
      {
        return this.levelData.Song;
      }
    }

    public IDictionary<int, Volume> Volumes
    {
      get
      {
        return (IDictionary<int, Volume>) this.levelData.Volumes;
      }
    }

    public IDictionary<int, ArtObjectInstance> ArtObjects
    {
      get
      {
        return (IDictionary<int, ArtObjectInstance>) this.levelData.ArtObjects;
      }
    }

    public IDictionary<int, BackgroundPlane> BackgroundPlanes
    {
      get
      {
        return (IDictionary<int, BackgroundPlane>) this.levelData.BackgroundPlanes;
      }
    }

    public IDictionary<int, TrileGroup> Groups
    {
      get
      {
        return (IDictionary<int, TrileGroup>) this.levelData.Groups;
      }
    }

    public IDictionary<int, NpcInstance> NonPlayerCharacters
    {
      get
      {
        return (IDictionary<int, NpcInstance>) this.levelData.NonPlayerCharacters;
      }
    }

    public IDictionary<int, Script> Scripts
    {
      get
      {
        return (IDictionary<int, Script>) this.levelData.Scripts;
      }
    }

    public IDictionary<int, MovementPath> Paths
    {
      get
      {
        return (IDictionary<int, MovementPath>) this.levelData.Paths;
      }
    }

    public IList<string> MutedLoops
    {
      get
      {
        return (IList<string>) this.levelData.MutedLoops;
      }
    }

    public IList<AmbienceTrack> AmbienceTracks
    {
      get
      {
        return (IList<AmbienceTrack>) this.levelData.AmbienceTracks;
      }
    }

    public Dictionary<Point, Limit> ScreenSpaceLimits
    {
      get
      {
        return this.screenSpaceLimits;
      }
    }

    public bool SkipInvalidation { get; set; }

    public bool IsInvalidatingScreen
    {
      get
      {
        return this.screenInvalidationWorker != null;
      }
    }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { protected get; set; }

    [ServiceDependency]
    public IFogManager FogManager { protected get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { protected get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { protected get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { protected get; set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { protected get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { protected get; set; }

    public event Action LevelChanged = new Action(Util.NullAction);

    public event Action LevelChanging = new Action(Util.NullAction);

    public event Action LightingChanged = new Action(Util.NullAction);

    public event Action SkyChanged = new Action(Util.NullAction);

    public event Action ScreenInvalidated;

    public event Action<TrileInstance> TrileRestored = new Action<TrileInstance>(Util.NullAction<TrileInstance>);

    static LevelManager()
    {
    }

    protected LevelManager(Game game)
      : base(game)
    {
      this.levelData = new Level()
      {
        SkyName = "Blue"
      };
      this.fallbackTrile = new Trile(CollisionType.TopOnly)
      {
        Id = -1
      };
      this.UpdateOrder = -2;
    }

    public override void Initialize()
    {
      this.CameraManager.ViewpointChanged += new Action(this.InvalidateScreen);
      this.InvalidateScreen();
    }

    public abstract void Load(string levelName);

    public abstract void Rebuild();

    public void ClearArtSatellites()
    {
      foreach (ArtObjectInstance artObjectInstance in this.LevelMaterializer.LevelArtObjects)
        artObjectInstance.Dispose(true);
      foreach (BackgroundPlane backgroundPlane in (IEnumerable<BackgroundPlane>) this.BackgroundPlanes.Values)
        backgroundPlane.Dispose();
    }

    public Trile SafeGetTrile(int trileId)
    {
      if (trileId == -1 || this.TrileSet == null)
        return this.fallbackTrile;
      else
        return this.TrileSet[trileId];
    }

    public TrileInstance TrileInstanceAt(ref TrileEmplacement id)
    {
      TrileInstance trileInstance;
      if (!this.Triles.TryGetValue(id, out trileInstance))
        return (TrileInstance) null;
      else
        return trileInstance;
    }

    public bool TrileExists(TrileEmplacement emplacement)
    {
      return this.levelData.Triles.ContainsKey(emplacement);
    }

    protected void AddInstance(TrileEmplacement emplacement, TrileInstance instance)
    {
      this.levelData.Triles.Add(emplacement, instance);
      instance.Removed = false;
    }

    public virtual void RecordMoveToEnd(int groupId)
    {
    }

    public virtual bool IsPathRecorded(int groupId)
    {
      return false;
    }

    public bool IsCornerTrile(ref TrileEmplacement id, ref FaceOrientation face1, ref FaceOrientation face2)
    {
      TrileInstance trileInstance;
      if (!this.levelData.Triles.TryGetValue(id.GetTraversal(ref face1), out trileInstance) || trileInstance.Trile.SeeThrough || trileInstance.ForceSeeThrough)
        return true;
      TrileEmplacement traversal = id.GetTraversal(ref face2);
      if (!this.levelData.Triles.TryGetValue(traversal, out trileInstance) || trileInstance.Trile.SeeThrough || trileInstance.ForceSeeThrough)
        return true;
      traversal = traversal.GetTraversal(ref face1);
      return !this.levelData.Triles.TryGetValue(traversal, out trileInstance) || trileInstance.Trile.SeeThrough || trileInstance.ForceSeeThrough;
    }

    public bool IsBorderTrileFace(ref TrileEmplacement id, ref FaceOrientation face)
    {
      TrileInstance trileInstance;
      if (this.levelData.Triles.TryGetValue(id.GetTraversal(ref face), out trileInstance) && !trileInstance.Trile.SeeThrough)
        return trileInstance.ForceSeeThrough;
      else
        return true;
    }

    public bool IsBorderTrile(ref TrileEmplacement id)
    {
      bool flag = false;
      for (int index = 0; index < 6 && !flag; ++index)
      {
        FaceOrientation face = (FaceOrientation) index;
        flag = flag | this.IsBorderTrileFace(ref id, ref face);
      }
      return flag;
    }

    public bool IsInRange(ref TrileEmplacement id)
    {
      Vector3 size = this.levelData.Size;
      if (id.X >= 0 && (double) id.X < (double) size.X && (id.Y >= 0 && (double) id.Y < (double) size.Y) && id.Z >= 0)
        return (double) id.Z < (double) size.Z;
      else
        return false;
    }

    public bool IsInRange(Vector3 position)
    {
      Vector3 size = this.levelData.Size;
      if ((double) position.X >= 0.0 && (double) position.X < (double) size.X && ((double) position.Y >= 0.0 && (double) position.Y < (double) size.Y) && (double) position.Z >= 0.0)
        return (double) position.Z < (double) size.Z;
      else
        return false;
    }

    public bool VolumeExists(int id)
    {
      return this.levelData.Volumes.ContainsKey(id);
    }

    public void SwapTrile(TrileInstance instance, Trile newTrile)
    {
      this.LevelMaterializer.CullInstanceOut(instance);
      this.LevelMaterializer.RemoveInstance(instance);
      instance.TrileId = newTrile.Id;
      instance.RefreshTrile();
      this.LevelMaterializer.AddInstance(instance);
      this.LevelMaterializer.CullInstanceIn(instance);
    }

    public void RestoreTrile(TrileInstance instance)
    {
      if (this.TrileExists(instance.Emplacement))
        return;
      this.LevelMaterializer.AddInstance(instance);
      this.AddInstance(instance.Emplacement, instance);
      this.InvalidateScreenSpaceTile(instance.Emplacement);
      this.TrileRestored(instance);
    }

    public bool ClearTrile(TrileInstance instance)
    {
      return this.ClearTrile(instance, false);
    }

    public bool ClearTrile(TrileInstance instance, bool skipRecull)
    {
      this.LevelMaterializer.RemoveInstance(instance);
      TrileInstance trileInstance1;
      bool flag1;
      if (this.Triles.TryGetValue(instance.Emplacement, out trileInstance1) && instance != trileInstance1 && trileInstance1.OverlappedTriles != null)
      {
        flag1 = trileInstance1.OverlappedTriles.Remove(instance);
      }
      else
      {
        flag1 = this.Triles.Remove(instance.Emplacement);
        if (flag1 && instance.Overlaps)
          this.RestoreTrile(instance.PopOverlap());
      }
      if (!flag1)
      {
        foreach (TrileInstance trileInstance2 in (IEnumerable<TrileInstance>) this.Triles.Values)
        {
          if (trileInstance2.Overlaps && trileInstance2.OverlappedTriles.Contains(instance))
          {
            flag1 = trileInstance2.OverlappedTriles.Remove(instance);
            if (flag1)
              break;
          }
        }
      }
      if (!flag1)
      {
        foreach (KeyValuePair<TrileEmplacement, TrileInstance> keyValuePair in (IEnumerable<KeyValuePair<TrileEmplacement, TrileInstance>>) this.Triles)
        {
          if (keyValuePair.Value == instance)
          {
            flag1 = this.Triles.Remove(keyValuePair.Key);
            if (flag1)
              break;
          }
        }
      }
      bool flag2 = false;
      foreach (TrileGroup trileGroup in (IEnumerable<TrileGroup>) this.Groups.Values)
        flag2 = flag2 | trileGroup.Triles.Remove(instance);
      if (flag2)
      {
        foreach (int key in Enumerable.ToArray<int>((IEnumerable<int>) this.Groups.Keys))
        {
          if (this.Groups[key].Triles.Count == 0)
            this.Groups.Remove(key);
        }
      }
      if (!skipRecull)
      {
        this.LevelMaterializer.CullInstanceOut(instance, true);
        this.RecullAt(instance);
      }
      instance.Removed = true;
      return flag1;
    }

    public bool ClearTrile(TrileEmplacement emplacement)
    {
      TrileInstance trileInstance;
      if (!this.Triles.TryGetValue(emplacement, out trileInstance))
        return false;
      this.LevelMaterializer.RemoveInstance(trileInstance);
      bool flag1 = this.Triles.Remove(emplacement);
      trileInstance.Removed = true;
      this.LevelMaterializer.CullInstanceOut(trileInstance, true);
      bool flag2 = false;
      foreach (TrileGroup trileGroup in (IEnumerable<TrileGroup>) this.Groups.Values)
        flag2 = flag2 | trileGroup.Triles.Remove(trileInstance);
      if (flag2)
      {
        foreach (int key in Enumerable.ToArray<int>((IEnumerable<int>) this.Groups.Keys))
        {
          if (this.Groups[key].Triles.Count == 0)
            this.Groups.Remove(key);
        }
      }
      if (flag1 && trileInstance.Overlaps)
        this.RestoreTrile(trileInstance.PopOverlap());
      return true;
    }

    public void RecullAt(TrileInstance instance)
    {
      this.RecullAt(instance.Emplacement);
    }

    public void RecullAt(TrileEmplacement emplacement)
    {
      Viewpoint viewpoint = this.CameraManager.Viewpoint;
      if (!FezMath.IsOrthographic(viewpoint))
        return;
      this.RecullAt(new Point(FezMath.SideMask(viewpoint) == Vector3.Right ? emplacement.X : emplacement.Z, emplacement.Y));
    }

    public void RecullAt(Point ssPos)
    {
      this.WaitForScreenInvalidation();
      this.InvalidateScreenSpaceTile(ssPos);
      this.LevelMaterializer.FreeScreenSpace(ssPos.X, ssPos.Y);
      this.LevelMaterializer.FillScreenSpace(ssPos.X, ssPos.Y);
      this.LevelMaterializer.CommitBatchesIfNeeded();
    }

    protected void OnLevelChanged()
    {
      this.LevelChanged();
    }

    protected void OnLevelChanging()
    {
      this.LevelChanging();
      this.OnLightingChanged();
    }

    protected void OnSkyChanged()
    {
      this.SkyChanged();
    }

    protected virtual void OnLightingChanged()
    {
      this.LightingChanged();
    }

    public void AddPlane(BackgroundPlane plane)
    {
      int key = IdentifierPool.FirstAvailable<BackgroundPlane>(this.BackgroundPlanes);
      plane.Id = key;
      this.BackgroundPlanes.Add(key, plane);
    }

    public void RemovePlane(BackgroundPlane plane)
    {
      this.BackgroundPlanes.Remove(plane.Id);
      plane.Dispose();
    }

    public TrileInstance ActualInstanceAt(Vector3 position)
    {
      Vector3 vector3 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      bool depthIsZ = (double) vector3.Z != 0.0;
      bool flag = depthIsZ;
      int forwardSign = depthIsZ ? (int) vector3.Z : (int) vector3.X;
      Vector3 screenSpacePosition = new Vector3(flag ? position.X : position.Z, position.Y, depthIsZ ? position.Z : position.X);
      TrileEmplacement emplacement = new TrileEmplacement((int) Math.Floor((double) position.X), (int) Math.Floor((double) position.Y), (int) Math.Floor((double) position.Z));
      float num = FezMath.Frac(screenSpacePosition.Z);
      LevelManager.QueryResult queryResult;
      TrileInstance trileInstance = this.OffsetInstanceAt(emplacement, screenSpacePosition, depthIsZ, forwardSign, false, false, QueryOptions.None, out queryResult);
      if (trileInstance != null)
        return trileInstance;
      if ((double) num >= 0.5)
        return this.OffsetInstanceAt(emplacement.GetOffset(depthIsZ ? 0 : 1, 0, depthIsZ ? 1 : 0), screenSpacePosition, depthIsZ, forwardSign, false, false, QueryOptions.None, out queryResult);
      else
        return this.OffsetInstanceAt(emplacement.GetOffset(depthIsZ ? 0 : -1, 0, depthIsZ ? -1 : 0), screenSpacePosition, depthIsZ, forwardSign, false, false, QueryOptions.None, out queryResult);
    }

    public NearestTriles NearestTrile(Vector3 position)
    {
      return this.NearestTrile(position, QueryOptions.None);
    }

    public NearestTriles NearestTrile(Vector3 position, QueryOptions options)
    {
      return this.NearestTrile(position, options, new Viewpoint?());
    }

    public NearestTriles NearestTrile(Vector3 position, QueryOptions options, Viewpoint? vp)
    {
      NearestTriles nearestTriles = new NearestTriles();
      bool hasValue = vp.HasValue;
      Viewpoint view = hasValue ? vp.Value : this.CameraManager.Viewpoint;
      if (!hasValue)
        this.WaitForScreenInvalidation();
      bool flag1 = view == Viewpoint.Front || view == Viewpoint.Back;
      bool flag2 = (options & QueryOptions.Background) == QueryOptions.Background;
      bool simpleTest = (options & QueryOptions.Simple) == QueryOptions.Simple;
      TrileEmplacement emplacement = new TrileEmplacement((int) position.X, (int) position.Y, (int) position.Z);
      Vector3 vector3 = FezMath.ForwardVector(view);
      int forwardSign = flag1 ? (int) vector3.Z : (int) vector3.X;
      Vector3 screenSpacePosition = new Vector3(flag1 ? position.X : position.Z, position.Y, -1f);
      Point key = !flag1 ? new Point(emplacement.Z, emplacement.Y) : new Point(emplacement.X, emplacement.Y);
      int num1;
      if (hasValue)
      {
        forwardSign = flag1 ? (int) vector3.Z : (int) vector3.X;
        if (flag2)
          forwardSign *= -1;
        float num2 = (float) (((flag1 ? (double) this.Size.Z : (double) this.Size.X) - 1.0) / 2.0);
        if (flag1)
          emplacement.Z = (int) ((double) num2 - (double) forwardSign * (double) num2);
        else
          emplacement.X = (int) ((double) num2 - (double) forwardSign * (double) num2);
        num1 = (int) ((double) num2 + (double) forwardSign * (double) num2);
      }
      else if (simpleTest)
      {
        Limit limit;
        if (!this.screenSpaceLimits.TryGetValue(key, out limit))
          return nearestTriles;
        int num2 = flag2 ? limit.End : limit.Start;
        if (flag1)
          emplacement.Z = num2;
        else
          emplacement.X = num2;
        num1 = flag2 ? limit.Start : limit.End;
        if (flag2)
          forwardSign *= -1;
      }
      else
      {
        Limit limit1;
        bool flag3 = this.screenSpaceLimits.TryGetValue(key, out limit1);
        int num2 = (double) FezMath.Frac(screenSpacePosition.X) > 0.5 ? 1 : -1;
        int num3 = (double) FezMath.Frac(screenSpacePosition.Y) > 0.5 ? 1 : -1;
        key.X += num2;
        Limit limit2;
        bool flag4 = this.screenSpaceLimits.TryGetValue(key, out limit2);
        key.X -= num2;
        key.Y += num3;
        Limit limit3;
        bool flag5 = this.screenSpaceLimits.TryGetValue(key, out limit3);
        if (!flag3 && !flag5 && !flag4)
          return nearestTriles;
        Limit limit4;
        if (flag3)
        {
          limit4 = limit1;
          if (!flag4 && !flag5)
            simpleTest = true;
        }
        else
        {
          limit4.Start = forwardSign == 1 ? int.MaxValue : int.MinValue;
          limit4.End = forwardSign == 1 ? int.MinValue : int.MaxValue;
          limit4.NoOffset = true;
        }
        if (flag4)
        {
          limit4.Start = forwardSign == 1 ? Math.Min(limit4.Start, limit2.Start) : Math.Max(limit4.Start, limit2.Start);
          limit4.End = forwardSign == 1 ? Math.Max(limit4.End, limit2.End) : Math.Min(limit4.End, limit2.End);
        }
        if (flag5)
        {
          limit4.Start = forwardSign == 1 ? Math.Min(limit4.Start, limit3.Start) : Math.Max(limit4.Start, limit3.Start);
          limit4.End = forwardSign == 1 ? Math.Max(limit4.End, limit3.End) : Math.Min(limit4.End, limit3.End);
        }
        int num4 = flag2 ? limit4.End : limit4.Start;
        if (flag1)
          emplacement.Z = num4;
        else
          emplacement.X = num4;
        num1 = flag2 ? limit4.Start : limit4.End;
        if (flag2)
          forwardSign *= -1;
      }
      int num5 = num1 + forwardSign;
      bool flag6 = flag1 ? emplacement.Z != num5 : emplacement.X != num5;
      if (flag1)
      {
        for (; flag6; flag6 = emplacement.Z != num5)
        {
          LevelManager.QueryResult nearestQueryResult;
          TrileInstance trileInstance = this.OffsetInstanceAt(ref emplacement, ref screenSpacePosition, true, forwardSign, true, false, simpleTest, options, out nearestQueryResult);
          if (trileInstance != null)
          {
            if (nearestQueryResult == LevelManager.QueryResult.Full)
            {
              nearestTriles.Deep = trileInstance;
              break;
            }
            else if (nearestTriles.Surface == null)
              nearestTriles.Surface = trileInstance;
          }
          emplacement.Z += forwardSign;
        }
      }
      else
      {
        for (; flag6; flag6 = emplacement.X != num5)
        {
          LevelManager.QueryResult nearestQueryResult;
          TrileInstance trileInstance = this.OffsetInstanceAt(ref emplacement, ref screenSpacePosition, false, forwardSign, true, false, simpleTest, options, out nearestQueryResult);
          if (trileInstance != null)
          {
            if (nearestQueryResult == LevelManager.QueryResult.Full)
            {
              nearestTriles.Deep = trileInstance;
              break;
            }
            else if (nearestTriles.Surface == null)
              nearestTriles.Surface = trileInstance;
          }
          emplacement.X += forwardSign;
        }
      }
      return nearestTriles;
    }

    private TrileInstance OffsetInstanceAt(TrileEmplacement emplacement, Vector3 screenSpacePosition, bool depthIsZ, int forwardSign, bool useSelector, bool keepNearest, QueryOptions context, out LevelManager.QueryResult queryResult)
    {
      return this.OffsetInstanceAt(ref emplacement, ref screenSpacePosition, depthIsZ, forwardSign, useSelector, keepNearest, false, context, out queryResult);
    }

    private TrileInstance OffsetInstanceAt(ref TrileEmplacement emplacement, ref Vector3 screenSpacePosition, bool depthIsZ, int forwardSign, bool useSelector, bool keepNearest, bool simpleTest, QueryOptions context, out LevelManager.QueryResult nearestQueryResult)
    {
      LevelManager.QueryResult queryResult = LevelManager.QueryResult.Nothing;
      TrileInstance instance1;
      if (this.Triles.TryGetValue(emplacement, out instance1))
        instance1 = this.OffsetInstanceOrOverlapsContain(instance1, screenSpacePosition, depthIsZ, forwardSign, useSelector, context, out queryResult);
      if (simpleTest || instance1 != null && !keepNearest)
      {
        nearestQueryResult = queryResult;
        return instance1;
      }
      else
      {
        TrileInstance nearest = instance1;
        nearestQueryResult = queryResult;
        bool flag = depthIsZ;
        int num = (double) FezMath.Frac(screenSpacePosition.X) > 0.5 ? 1 : -1;
        TrileEmplacement id = flag ? emplacement.GetOffset(num, 0, 0) : emplacement.GetOffset(0, 0, num);
        TrileInstance instance2 = this.TrileInstanceAt(ref id);
        if (instance2 != null)
        {
          TrileInstance contender = this.OffsetInstanceOrOverlapsContain(instance2, screenSpacePosition, depthIsZ, forwardSign, useSelector, context, out queryResult);
          if (contender != null)
          {
            nearestQueryResult = queryResult;
            if (!keepNearest)
              return contender;
            nearest = LevelManager.KeepNearestInstance(nearest, contender, depthIsZ, forwardSign);
          }
        }
        int offsetY = (double) FezMath.Frac(screenSpacePosition.Y) > 0.5 ? 1 : -1;
        TrileEmplacement offset = emplacement.GetOffset(0, offsetY, 0);
        TrileInstance instance3 = this.TrileInstanceAt(ref offset);
        if (instance3 != null)
        {
          TrileInstance contender = this.OffsetInstanceOrOverlapsContain(instance3, screenSpacePosition, depthIsZ, forwardSign, useSelector, context, out queryResult);
          if (contender != null)
          {
            nearestQueryResult = queryResult;
            if (!keepNearest)
              return contender;
            nearest = LevelManager.KeepNearestInstance(nearest, contender, depthIsZ, forwardSign);
          }
        }
        offset = id.GetOffset(0, offsetY, 0);
        TrileInstance instance4 = this.TrileInstanceAt(ref offset);
        if (instance4 != null)
        {
          TrileInstance contender = this.OffsetInstanceOrOverlapsContain(instance4, screenSpacePosition, depthIsZ, forwardSign, useSelector, context, out queryResult);
          if (contender != null)
          {
            nearestQueryResult = queryResult;
            if (!keepNearest)
              return contender;
            nearest = LevelManager.KeepNearestInstance(nearest, contender, depthIsZ, forwardSign);
          }
        }
        return nearest;
      }
    }

    private TrileInstance OffsetInstanceOrOverlapsContain(TrileInstance instance, Vector3 screenSpacePosition, bool depthIsZ, int forwardSign, bool useSelector, QueryOptions context, out LevelManager.QueryResult queryResult)
    {
      TrileInstance nearest = (TrileInstance) null;
      queryResult = LevelManager.QueryResult.Full;
      LevelManager.QueryResult queryResult1 = LevelManager.QueryResult.Nothing;
      if (LevelManager.OffsetInstanceContains(screenSpacePosition, instance, depthIsZ) && (!useSelector || this.InstanceMaterialForQuery(instance, context, out queryResult)))
      {
        nearest = instance;
        queryResult1 = queryResult;
      }
      if (instance.Overlaps)
      {
        foreach (TrileInstance trileInstance in instance.OverlappedTriles)
        {
          if (LevelManager.OffsetInstanceContains(screenSpacePosition, trileInstance, depthIsZ) && (!useSelector || this.InstanceMaterialForQuery(trileInstance, context, out queryResult)))
          {
            nearest = LevelManager.KeepNearestInstance(nearest, trileInstance, depthIsZ, forwardSign);
            queryResult1 = queryResult;
          }
        }
      }
      queryResult = queryResult1;
      return nearest;
    }

    private static bool OffsetInstanceContains(Vector3 screenSpacePosition, TrileInstance instance, bool depthIsZ)
    {
      Vector3 center = instance.Center;
      Vector3 transformedSize = instance.TransformedSize;
      Vector3 vector3_1 = new Vector3(depthIsZ ? center.X : center.Z, center.Y, depthIsZ ? center.Z : center.X);
      Vector3 vector3_2 = new Vector3(depthIsZ ? transformedSize.X / 2f : transformedSize.Z / 2f, transformedSize.Y / 2f, depthIsZ ? transformedSize.Z / 2f : transformedSize.X / 2f);
      if ((double) screenSpacePosition.X <= (double) vector3_1.X - (double) vector3_2.X || (double) screenSpacePosition.X >= (double) vector3_1.X + (double) vector3_2.X || ((double) screenSpacePosition.Y < (double) vector3_1.Y - (double) vector3_2.Y || (double) screenSpacePosition.Y >= (double) vector3_1.Y + (double) vector3_2.Y))
        return false;
      if ((double) screenSpacePosition.Z == -1.0)
        return true;
      if ((double) screenSpacePosition.Z > (double) vector3_1.Z - (double) vector3_2.Z)
        return (double) screenSpacePosition.Z < (double) vector3_1.Z + (double) vector3_2.Z;
      else
        return false;
    }

    private bool InstanceMaterialForQuery(TrileInstance instance, QueryOptions options, out LevelManager.QueryResult queryResult)
    {
      Trile trile = instance.Trile;
      CollisionType rotatedFace = instance.GetRotatedFace((options & QueryOptions.Background) == QueryOptions.Background ? FezMath.GetOpposite(this.CameraManager.VisibleOrientation) : this.CameraManager.VisibleOrientation);
      queryResult = trile.Immaterial || instance.PhysicsState != null && instance.PhysicsState.UpdatingPhysics || rotatedFace == CollisionType.Immaterial ? LevelManager.QueryResult.Nothing : (trile.Thin ? LevelManager.QueryResult.Thin : LevelManager.QueryResult.Full);
      return queryResult != LevelManager.QueryResult.Nothing;
    }

    private static TrileInstance KeepNearestInstance(TrileInstance nearest, TrileInstance contender, bool depthIsZ, int forwardSign)
    {
      Vector3 b = (depthIsZ ? Vector3.UnitZ : Vector3.UnitX) * (float) -forwardSign;
      if ((nearest == null ? -3.40282346638529E+38 : (double) FezMath.Dot(nearest.Center + nearest.TransformedSize * b / 2f, b)) <= (double) FezMath.Dot(contender.Center + contender.TransformedSize * b / 2f, b))
        return contender;
      else
        return nearest;
    }

    public IEnumerable<Trile> ActorTriles(ActorType type)
    {
      if (this.TrileSet != null)
        return Enumerable.Where<Trile>((IEnumerable<Trile>) this.TrileSet.Triles.Values, (Func<Trile, bool>) (x => x.ActorSettings.Type == type));
      else
        return Enumerable.Repeat<Trile>((Trile) null, 1);
    }

    public IEnumerable<string> LinkedLevels()
    {
      return Enumerable.SelectMany<IEnumerable<string>, string>(Enumerable.Select<Script, IEnumerable<string>>((IEnumerable<Script>) this.levelData.Scripts.Values, (Func<Script, IEnumerable<string>>) (script => Enumerable.Select<ScriptAction, string>(Enumerable.Where<ScriptAction>((IEnumerable<ScriptAction>) script.Actions, (Func<ScriptAction, bool>) (action => action.Object.Type == "Level")), (Func<ScriptAction, string>) (action => Enumerable.FirstOrDefault<string>((IEnumerable<string>) action.Arguments))))), (Func<IEnumerable<string>, IEnumerable<string>>) (x => x));
    }

    public void UpdateInstance(TrileInstance instance)
    {
      if (FezMath.Round(instance.LastUpdatePosition) != FezMath.Round(instance.Position))
      {
        TrileEmplacement trileEmplacement = new TrileEmplacement(instance.LastUpdatePosition);
        TrileInstance trileInstance;
        TrileInstance instance1;
        if (this.Triles.TryGetValue(trileEmplacement, out trileInstance))
        {
          if (trileInstance == instance)
          {
            this.Triles.Remove(trileEmplacement);
            if (instance.Overlaps)
            {
              instance1 = instance.PopOverlap();
              this.AddInstance(trileEmplacement, instance1);
            }
          }
          else if (trileInstance.Overlaps && trileInstance.OverlappedTriles.Contains(instance))
            trileInstance.OverlappedTriles.Remove(instance);
        }
        if (this.Triles.TryGetValue(instance.Emplacement, out instance1))
        {
          instance.PushOverlap(instance1);
          this.Triles.Remove(instance.Emplacement);
        }
        this.LevelMaterializer.UpdateInstance(instance);
        this.Triles.Add(instance.Emplacement, instance);
        instance.Update();
        this.LevelMaterializer.UpdateRow(trileEmplacement, instance);
        if (!this.IsInvalidatingScreen)
        {
          this.InvalidateScreenSpaceTile(trileEmplacement);
          this.InvalidateScreenSpaceTile(instance.Emplacement);
        }
      }
      if (instance.InstanceId == -1)
        return;
      this.LevelMaterializer.GetTrileMaterializer(instance.VisualTrile).UpdateInstance(instance);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.screenInvalidationWorker != null || this.ScreenInvalidated == null || this.EngineState.Loading)
        return;
      if (this.SkipInvalidation)
      {
        this.ScreenInvalidated = (Action) null;
      }
      else
      {
        this.ScreenInvalidated();
        this.ScreenInvalidated = (Action) null;
      }
    }

    public void WaitForScreenInvalidation()
    {
      while (this.screenInvalidationWorker != null)
        Thread.Sleep(0);
    }

    public void AbortInvalidation()
    {
      if (this.screenInvalidationWorker == null)
        return;
      this.screenInvalidationWorker.Abort();
    }

    private void InvalidateScreenSpaceTile(TrileEmplacement emplacement)
    {
      Vector3 b = FezMath.SideMask(this.CameraManager.Viewpoint);
      this.InvalidateScreenSpaceTile(new Point((int) FezMath.Dot(emplacement.AsVector, b), emplacement.Y));
    }

    private void InvalidateScreenSpaceTile(Point ssPos)
    {
      this.WaitForScreenInvalidation();
      this.screenSpaceLimits.Remove(ssPos);
      this.FillScreenSpaceTile(ssPos, (IDictionary<Point, Limit>) this.screenSpaceLimits);
    }

    private void InvalidateScreen()
    {
      if (this.SkipInvalidation)
        return;
      lock (LevelManager.Mutex)
      {
        if (this.screenInvalidationWorker != null)
          this.screenInvalidationWorker.Abort();
      }
      this.WaitForScreenInvalidation();
      Dictionary<Point, Limit> newLimits = new Dictionary<Point, Limit>(this.screenSpaceLimits.Count, (IEqualityComparer<Point>) LevelManager.FastPointComparer.Default);
      this.screenInvalidationWorker = this.ThreadPool.Take<Dictionary<Point, Limit>>(new Action<Dictionary<Point, Limit>>(this.DoInvalidateScreen));
      this.screenInvalidationWorker.Priority = ThreadPriority.Normal;
      this.screenInvalidationWorker.Finished += (Action) (() =>
      {
        lock (LevelManager.Mutex)
        {
          if (this.screenInvalidationWorker == null)
            return;
          if (!this.screenInvalidationWorker.Aborted)
            this.screenSpaceLimits = newLimits;
          this.ThreadPool.Return<Dictionary<Point, Limit>>(this.screenInvalidationWorker);
          this.screenInvalidationWorker = (Worker<Dictionary<Point, Limit>>) null;
        }
      });
      this.screenInvalidationWorker.Start(newLimits);
    }

    private void DoInvalidateScreen(Dictionary<Point, Limit> newLimits)
    {
      float num = FezMath.Dot(this.Size, FezMath.SideMask(this.CameraManager.Viewpoint));
      for (int x = 0; (double) x < (double) num; ++x)
      {
        for (int y = 0; (double) y < (double) this.Size.Y; ++y)
        {
          if (this.screenInvalidationWorker.Aborted)
            return;
          this.FillScreenSpaceTile(new Point(x, y), (IDictionary<Point, Limit>) newLimits);
        }
      }
    }

    private void FillScreenSpaceTile(Point p, IDictionary<Point, Limit> newLimits)
    {
      Vector3 vector3 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      bool flag1 = (double) vector3.Z != 0.0;
      bool flag2 = flag1;
      int num1 = flag1 ? (int) vector3.Z : (int) vector3.X;
      float num2 = (float) (((flag1 ? (double) this.Size.Z : (double) this.Size.X) - 1.0) / 2.0);
      int num3 = (int) ((double) num2 + (double) num1 * (double) num2) + num1;
      Limit limit = new Limit()
      {
        Start = (int) ((double) num2 - (double) num1 * (double) num2),
        End = num3,
        NoOffset = true
      };
      TrileEmplacement key = new TrileEmplacement(flag2 ? p.X : limit.Start, p.Y, flag1 ? limit.Start : p.X);
      bool flag3 = true;
      bool flag4 = false;
      while (flag3)
      {
        TrileInstance trileInstance;
        if (this.Triles.TryGetValue(key, out trileInstance))
        {
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          Limit& local = @limit;
          // ISSUE: explicit reference operation
          int num4 = (^local).NoOffset & trileInstance.Position == trileInstance.Emplacement.AsVector ? 1 : 0;
          // ISSUE: explicit reference operation
          (^local).NoOffset = num4 != 0;
          int num5 = flag1 ? key.Z : key.X;
          if (!flag4)
          {
            limit.Start = num5;
            flag4 = true;
          }
          limit.End = num5;
        }
        if (flag1)
        {
          key.Z += num1;
          flag3 = key.Z != num3;
        }
        else
        {
          key.X += num1;
          flag3 = key.X != num3;
        }
      }
      if (limit.End == num3)
        return;
      if (newLimits.ContainsKey(p))
        newLimits.Remove(p);
      newLimits.Add(p, limit);
    }

    public abstract bool WasPathSupposedToBeRecorded(int id);

    private enum QueryResult
    {
      Nothing,
      Thin,
      Full,
    }

    private class FastPointComparer : IEqualityComparer<Point>
    {
      public static readonly LevelManager.FastPointComparer Default = new LevelManager.FastPointComparer();

      static FastPointComparer()
      {
      }

      public bool Equals(Point x, Point y)
      {
        if (x.X == y.X)
          return x.Y == y.Y;
        else
          return false;
      }

      public int GetHashCode(Point obj)
      {
        return obj.X | obj.Y << 16;
      }
    }
  }
}
