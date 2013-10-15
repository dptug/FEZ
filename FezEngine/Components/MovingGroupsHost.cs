// Type: FezEngine.Components.MovingGroupsHost
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Components
{
  public class MovingGroupsHost : DrawableGameComponent
  {
    protected readonly List<MovingGroupsHost.MovingGroupState> trackedGroups = new List<MovingGroupsHost.MovingGroupState>();
    private readonly Dictionary<int, MovingGroupsHost.MovingGroupState> enablingVolumes = new Dictionary<int, MovingGroupsHost.MovingGroupState>();
    private readonly List<ArtObjectInstance> connectedAos = new List<ArtObjectInstance>();
    protected bool DontTrack;
    private int firstAvailableVolumeId;

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IVolumeService VolumeService { private get; set; }

    public MovingGroupsHost(Game game)
      : base(game)
    {
      this.UpdateOrder = -2;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.Enabled = false;
      this.LevelManager.LevelChanging += new Action(this.TrackNewGroups);
      this.LevelManager.LevelChanged += new Action(this.PostTrackNewGroups);
      Waiters.Wait(0.100000001490116, new Action(this.DelayedHook));
    }

    private void DelayedHook()
    {
      this.CameraManager.PreViewpointChanged += new Action(this.TrackConnectivePaths);
    }

    private void TrackConnectivePaths()
    {
      if (this.CameraManager.ViewTransitionCancelled)
        return;
      this.TrackConnectivePaths(true);
    }

    private void TrackConnectivePaths(bool update)
    {
      if (this.DontTrack || update && this.EngineState.Loading || (this.CameraManager.Viewpoint == Viewpoint.Perspective || this.CameraManager.Viewpoint == this.CameraManager.LastViewpoint && update))
        return;
      List<MovingGroupsHost.MovingGroupState> list = new List<MovingGroupsHost.MovingGroupState>();
      for (int index = this.trackedGroups.Count - 1; index >= 0; --index)
      {
        if (this.trackedGroups[index].IsConnective)
        {
          if (!this.trackedGroups[index].Group.MoveToEnd)
            list.Add(this.trackedGroups[index]);
          this.trackedGroups[index].StopSound();
          this.trackedGroups.RemoveAt(index);
        }
      }
      foreach (ArtObjectInstance artObjectInstance in this.LevelMaterializer.LevelArtObjects)
      {
        if (artObjectInstance.ArtObject.ActorType == ActorType.ConnectiveRail && artObjectInstance.ActorSettings.AttachedGroup.HasValue)
        {
          bool flag = false;
          foreach (MovingGroupsHost.MovingGroupState movingGroupState in this.trackedGroups)
          {
            int num1 = flag ? 1 : 0;
            int id = movingGroupState.Group.Id;
            int? attachedGroup = artObjectInstance.ActorSettings.AttachedGroup;
            int num2 = id != attachedGroup.GetValueOrDefault() ? 0 : (attachedGroup.HasValue ? 1 : 0);
            flag = (num1 | num2) != 0;
            if (flag)
              break;
          }
          if (!flag)
            this.TrackGroupConnectivePath(artObjectInstance.ActorSettings.AttachedGroup.Value);
        }
      }
      if (list.Count > 0)
      {
        foreach (MovingGroupsHost.MovingGroupState movingGroupState1 in this.trackedGroups)
        {
          if (movingGroupState1.IsConnective)
          {
            MovingGroupsHost.MovingGroupState oldTrack = (MovingGroupsHost.MovingGroupState) null;
            foreach (MovingGroupsHost.MovingGroupState movingGroupState2 in list)
            {
              if (movingGroupState2.Group == movingGroupState1.Group)
              {
                oldTrack = movingGroupState2;
                break;
              }
            }
            if (oldTrack != null)
            {
              movingGroupState1.Enabled = oldTrack.Enabled;
              movingGroupState1.MoveTo(oldTrack);
              movingGroupState1.PrepareOldTrack(oldTrack.Enabled && !oldTrack.Group.Path.NeedsTrigger);
            }
          }
        }
      }
      foreach (int key in this.enablingVolumes.Keys)
        this.LevelManager.Volumes.Remove(key);
      this.enablingVolumes.Clear();
      int num = this.firstAvailableVolumeId;
      foreach (MovingGroupsHost.MovingGroupState movingGroupState in this.trackedGroups)
      {
        if (movingGroupState.IsConnective && !movingGroupState.Enabled)
        {
          Vector3 vector3_1 = new Vector3(float.MaxValue);
          Vector3 vector3_2 = new Vector3(float.MinValue);
          foreach (TrileInstance trileInstance in movingGroupState.Group.Triles)
          {
            vector3_1 = Vector3.Min(vector3_1, trileInstance.Position);
            vector3_2 = Vector3.Max(vector3_2, trileInstance.Position);
          }
          Volume volume = new Volume()
          {
            Id = num++,
            From = new Vector3(vector3_1.X, vector3_1.Y + 1f, vector3_1.Z),
            To = new Vector3(vector3_2.X + 1f, vector3_2.Y + 2f, vector3_2.Z + 1f),
            Orientations = {
              FaceOrientation.Left,
              FaceOrientation.Right,
              FaceOrientation.Back,
              FaceOrientation.Front
            },
            Enabled = true
          };
          this.LevelManager.Volumes.Add(volume.Id, volume);
          this.enablingVolumes.Add(volume.Id, movingGroupState);
        }
      }
      if (this.VolumeService == null)
        return;
      this.VolumeService.RegisterNeeded = true;
    }

    private void TryEnableConnectivePath(int volumeId)
    {
      MovingGroupsHost.MovingGroupState movingGroupState;
      if (!this.enablingVolumes.TryGetValue(volumeId, out movingGroupState))
        return;
      movingGroupState.Enabled = true;
    }

    private void TrackGroupConnectivePath(int groupId)
    {
      Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      Vector3 b = FezMath.Abs(FezMath.ForwardVector(this.CameraManager.Viewpoint));
      this.connectedAos.Clear();
      foreach (ArtObjectInstance artObjectInstance in this.LevelMaterializer.LevelArtObjects)
      {
        int? attachedGroup = artObjectInstance.ActorSettings.AttachedGroup;
        int num = groupId;
        if ((attachedGroup.GetValueOrDefault() != num ? 0 : (attachedGroup.HasValue ? 1 : 0)) != 0)
          this.connectedAos.Add(artObjectInstance);
      }
      foreach (ArtObjectInstance artObjectInstance1 in this.connectedAos)
      {
        if (artObjectInstance1.ActorSettings.NextNode.HasValue)
          artObjectInstance1.ActorSettings.NextNodeAo = this.LevelManager.ArtObjects[artObjectInstance1.ActorSettings.NextNode.Value];
        foreach (ArtObjectInstance artObjectInstance2 in this.connectedAos)
        {
          if (artObjectInstance2.ActorSettings.NextNode.HasValue && artObjectInstance2.ActorSettings.NextNode.Value == artObjectInstance1.Id)
            artObjectInstance1.ActorSettings.PrecedingNodeAo = artObjectInstance2;
        }
      }
      TrileGroup group;
      if (!this.LevelManager.Groups.TryGetValue(groupId, out group))
      {
        Logger.Log("MovingGroupsHost::TrackGroupConnectivePath", LogSeverity.Warning, "Node is connected to a group that doesn't exist!");
      }
      else
      {
        if (group.MoveToEnd)
        {
          ArtObjectInstance artObjectInstance1 = (ArtObjectInstance) null;
          foreach (ArtObjectInstance artObjectInstance2 in this.connectedAos)
          {
            if (!artObjectInstance2.ActorSettings.NextNode.HasValue && artObjectInstance2.ArtObject.ActorType == ActorType.ConnectiveRail)
            {
              artObjectInstance1 = artObjectInstance2;
              break;
            }
          }
          if (artObjectInstance1 == null)
            throw new InvalidOperationException("No end-node! Can't move to end.");
          Vector3 zero = Vector3.Zero;
          foreach (TrileInstance trileInstance in group.Triles)
            zero += trileInstance.Center;
          Vector3 vector3_2 = zero / (float) group.Triles.Count;
          Vector3 ordering = artObjectInstance1.Position - vector3_2;
          group.Triles.Sort((IComparer<TrileInstance>) new MovingTrileInstanceComparer(ordering));
          foreach (TrileInstance instance in group.Triles)
          {
            instance.Position += ordering;
            this.LevelManager.UpdateInstance(instance);
          }
        }
        BoundingBox boundingBox1 = new BoundingBox(new Vector3(float.MaxValue), new Vector3(float.MinValue));
        Vector3 zero1 = Vector3.Zero;
        foreach (TrileInstance trileInstance in group.Triles)
        {
          Vector3 vector3_2 = trileInstance.TransformedSize / 2f;
          boundingBox1.Min = Vector3.Min(boundingBox1.Min, (trileInstance.Center - vector3_2) * vector3_1);
          boundingBox1.Max = Vector3.Max(boundingBox1.Max, (trileInstance.Center + vector3_2) * vector3_1 + b);
          zero1 += trileInstance.Center;
        }
        Vector3 vector3_3 = zero1 / (float) group.Triles.Count;
        BoundingBox boundingBox2 = new BoundingBox();
        foreach (ArtObjectInstance artObjectInstance in this.connectedAos)
        {
          if (artObjectInstance.ArtObject.ActorType == ActorType.None)
          {
            Vector3 vector3_2 = artObjectInstance.Scale * artObjectInstance.ArtObject.Size / 2f;
            boundingBox2 = new BoundingBox(artObjectInstance.Position - vector3_2, artObjectInstance.Position + vector3_2);
            Quaternion rotation = artObjectInstance.Rotation;
            FezMath.RotateOnCenter(ref boundingBox2, ref rotation);
            boundingBox2.Min *= vector3_1;
            boundingBox2.Max = boundingBox2.Max * vector3_1 + b;
            if (boundingBox1.Intersects(boundingBox2))
              break;
          }
        }
        ArtObjectInstance artObjectInstance3 = (ArtObjectInstance) null;
        foreach (ArtObjectInstance artObjectInstance1 in this.connectedAos)
        {
          if (artObjectInstance1.ArtObject.ActorType == ActorType.ConnectiveRail)
          {
            Vector3 vector3_2 = artObjectInstance1.Scale * artObjectInstance1.ArtObject.Size / 2f;
            BoundingBox box = new BoundingBox((artObjectInstance1.Position - vector3_2) * vector3_1, (artObjectInstance1.Position + vector3_2) * vector3_1 + b);
            if (boundingBox2.Intersects(box))
            {
              artObjectInstance3 = artObjectInstance1;
              break;
            }
          }
        }
        if (artObjectInstance3 == null)
        {
          InvalidOperationException operationException = new InvalidOperationException("Nodeless branch!");
          Logger.Log("Connective Groups", LogSeverity.Warning, operationException.Message);
          throw operationException;
        }
        else
        {
          ArtObjectInstance artObjectInstance1;
          for (; artObjectInstance3.ActorSettings.PrecedingNodeAo != null; artObjectInstance3 = artObjectInstance3.ActorSettings.PrecedingNodeAo)
          {
            Vector3 a = artObjectInstance3.ActorSettings.PrecedingNodeAo.Position - artObjectInstance3.Position;
            if ((double) Math.Abs(FezMath.Dot(a, b)) <= (double) Math.Abs(FezMath.Dot(a, FezMath.XZMask - b)))
            {
              bool flag = false;
              Vector3 point = a / 2f + artObjectInstance3.Position;
              foreach (ArtObjectInstance artObjectInstance2 in this.connectedAos)
              {
                if (artObjectInstance2.ArtObject.ActorType == ActorType.None)
                {
                  Vector3 vector3_2 = artObjectInstance2.Scale * artObjectInstance2.ArtObject.Size / 2f;
                  BoundingBox boundingBox3 = new BoundingBox(artObjectInstance2.Position - vector3_2, artObjectInstance2.Position + vector3_2);
                  Quaternion rotation = artObjectInstance2.Rotation;
                  FezMath.RotateOnCenter(ref boundingBox3, ref rotation);
                  if (boundingBox3.Contains(point) != ContainmentType.Disjoint)
                  {
                    flag = true;
                    break;
                  }
                }
              }
              if (!flag)
              {
                artObjectInstance1 = artObjectInstance3;
                goto label_72;
              }
            }
          }
          artObjectInstance1 = artObjectInstance3;
label_72:
          Vector3 ordering = artObjectInstance1.Position - vector3_3;
          group.Triles.Sort((IComparer<TrileInstance>) new MovingTrileInstanceComparer(ordering));
          foreach (TrileInstance instance in group.Triles)
          {
            instance.Position += ordering;
            this.LevelManager.UpdateInstance(instance);
          }
          MovementPath movementPath = new MovementPath()
          {
            EndBehavior = PathEndBehavior.Bounce
          };
          ArtObjectInstance artObjectInstance4 = artObjectInstance1;
          PathSegment pathSegment1 = (PathSegment) null;
          while (true)
          {
            ArtObjectInstance nextNodeAo = artObjectInstance4.ActorSettings.NextNodeAo;
            if (nextNodeAo != null)
            {
              Vector3 a = nextNodeAo.Position - artObjectInstance4.Position;
              bool flag1 = (double) Math.Abs(FezMath.Dot(a, b)) > (double) Math.Abs(FezMath.Dot(a, FezMath.XZMask - b));
              bool flag2 = false;
              Vector3 point = a / 2f + artObjectInstance4.Position;
              foreach (ArtObjectInstance artObjectInstance2 in this.connectedAos)
              {
                if (artObjectInstance2.ArtObject.ActorType == ActorType.None)
                {
                  Vector3 vector3_2 = artObjectInstance2.Scale * artObjectInstance2.ArtObject.Size / 2f;
                  BoundingBox boundingBox3 = new BoundingBox(artObjectInstance2.Position - vector3_2, artObjectInstance2.Position + vector3_2);
                  Quaternion rotation = artObjectInstance2.Rotation;
                  FezMath.RotateOnCenter(ref boundingBox3, ref rotation);
                  if (boundingBox3.Contains(point) != ContainmentType.Disjoint)
                  {
                    flag2 = true;
                    break;
                  }
                }
              }
              if (flag2 || flag1)
              {
                PathSegment pathSegment2 = nextNodeAo.ActorSettings.Segment.Clone();
                pathSegment2.Destination = (pathSegment1 == null ? Vector3.Zero : pathSegment1.Destination) + nextNodeAo.Position - artObjectInstance4.Position;
                if (!flag2)
                  pathSegment2.Duration = TimeSpan.Zero;
                movementPath.Segments.Add(pathSegment2);
                pathSegment1 = pathSegment2;
                artObjectInstance4 = nextNodeAo;
              }
              else
                break;
            }
            else
              break;
          }
          group.Path = movementPath;
          MovingGroupsHost.MovingGroupState movingGroupState = new MovingGroupsHost.MovingGroupState(group, true)
          {
            Enabled = false
          };
          this.trackedGroups.Add(movingGroupState);
          if (group.MoveToEnd)
          {
            group.MoveToEnd = false;
            movingGroupState.MoveToEnd();
          }
          foreach (ArtObjectInstance artObjectInstance2 in this.connectedAos)
            artObjectInstance2.Enabled = true;
        }
      }
    }

    protected virtual void TrackNewGroups()
    {
      this.firstAvailableVolumeId = IdentifierPool.FirstAvailable<Volume>(this.LevelManager.Volumes);
      this.enablingVolumes.Clear();
      this.trackedGroups.Clear();
      foreach (TrileGroup group in (IEnumerable<TrileGroup>) this.LevelManager.Groups.Values)
      {
        if (group.Path != null)
        {
          MovingGroupsHost.MovingGroupState movingGroupState = new MovingGroupsHost.MovingGroupState(group, false);
          this.trackedGroups.Add(movingGroupState);
          if (this.LevelManager.IsPathRecorded(group.Id) || this.LevelManager.WasPathSupposedToBeRecorded(group.Id))
            movingGroupState.MoveToEnd();
        }
      }
    }

    private void PostTrackNewGroups()
    {
      this.TrackConnectivePaths(false);
      this.Enabled = this.trackedGroups.Count > 0;
      if (!this.Enabled || this.VolumeService == null)
        return;
      this.VolumeService.Enter += new Action<int>(this.TryEnableConnectivePath);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Paused || this.EngineState.InMenuCube || (this.EngineState.InMap || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)) || (!this.CameraManager.ActionRunning || this.EngineState.Loading || this.EngineState.FarawaySettings.InTransition && (double) this.EngineState.FarawaySettings.DestinationCrossfadeStep == 0.0))
        return;
      this.DoUpdate(gameTime);
    }

    protected void DoUpdate(GameTime gameTime)
    {
      bool flag = false;
      foreach (MovingGroupsHost.MovingGroupState movingGroupState in this.trackedGroups)
      {
        if (movingGroupState.Group.MoveToEnd)
        {
          flag = flag | movingGroupState.IsConnective;
          if (!movingGroupState.IsConnective)
          {
            movingGroupState.MoveToEnd();
            movingGroupState.Group.MoveToEnd = false;
          }
        }
      }
      if (flag)
        this.TrackConnectivePaths(false);
      foreach (MovingGroupsHost.MovingGroupState movingGroupState in this.trackedGroups)
        movingGroupState.Update(gameTime.ElapsedGameTime);
    }

    protected class MovingGroupState
    {
      private float SinceKlonk = 1f;
      public readonly TrileGroup Group;
      public readonly bool IsConnective;
      private readonly Vector3[] referenceOrigins;
      private readonly bool neededTrigger;
      private SoundEmitter eAssociatedSound;
      private SoundEmitter eConnectiveIdle;
      private readonly SoundEffect sConnectiveKlonk;
      private readonly SoundEffect sConnectiveStartUp;
      public int CurrentSegmentIndex;
      private Vector3 segmentOrigin;
      private Vector3 lastSegmentOrigin;
      private TimeSpan sinceSegmentStarted;
      private bool paused;
      private readonly ArtObjectInstance[] attachedAOs;
      private readonly BackgroundPlane[] attachedPlanes;
      private readonly Vector3[] aoOrigins;
      private readonly Vector3[] planeOrigins;
      private readonly Vector3 MarkedPosition;
      private readonly List<PathSegment> Segments;
      private bool Silent;
      private bool SkipStartUp;
      private Vector3 lastVelocity;

      public bool Enabled { get; set; }

      private MovementPath Path
      {
        get
        {
          return this.Group.Path;
        }
      }

      public PathSegment CurrentSegment
      {
        get
        {
          return this.Segments[this.CurrentSegmentIndex];
        }
      }

      [ServiceDependency]
      public ILevelManager LevelManager { private get; set; }

      [ServiceDependency]
      public IContentManagerProvider CMProvider { private get; set; }

      public MovingGroupState(TrileGroup group, bool connective)
      {
        this.LevelManager = ServiceHelper.Get<ILevelManager>();
        this.CMProvider = ServiceHelper.Get<IContentManagerProvider>();
        this.Group = group;
        this.IsConnective = connective;
        this.neededTrigger = this.Path.NeedsTrigger;
        this.paused = this.neededTrigger;
        this.referenceOrigins = new Vector3[group.Triles.Count];
        this.MarkedPosition = Vector3.Zero;
        int num = 0;
        foreach (TrileInstance trileInstance in group.Triles)
        {
          this.MarkedPosition += trileInstance.Center;
          this.referenceOrigins[num++] = trileInstance.Position;
        }
        this.MarkedPosition /= (float) group.Triles.Count;
        if (!group.PhysicsInitialized)
        {
          foreach (TrileInstance instance in group.Triles)
            instance.PhysicsState = new InstancePhysicsState(instance);
          group.PhysicsInitialized = true;
        }
        this.Segments = new List<PathSegment>((IEnumerable<PathSegment>) this.Path.Segments);
        foreach (TrileInstance trileInstance in group.Triles)
          trileInstance.ForceSeeThrough = true;
        if (this.Path.EndBehavior == PathEndBehavior.Bounce)
        {
          for (int index = this.Segments.Count - 1; index >= 0; --index)
          {
            PathSegment pathSegment = this.Segments[index];
            Vector3 vector3 = index == 0 ? Vector3.Zero : this.Segments[index - 1].Destination;
            this.Segments.Add(new PathSegment()
            {
              Acceleration = pathSegment.Deceleration,
              Deceleration = pathSegment.Acceleration,
              Destination = vector3,
              Duration = pathSegment.Duration,
              JitterFactor = pathSegment.JitterFactor,
              WaitTimeOnFinish = pathSegment.WaitTimeOnStart,
              WaitTimeOnStart = pathSegment.WaitTimeOnFinish,
              Bounced = true
            });
          }
        }
        if (this.Path.SoundName != null)
        {
          try
          {
            this.eAssociatedSound = SoundEffectExtensions.EmitAt(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/" + this.Path.SoundName), Vector3.Zero, true, true);
          }
          catch (Exception ex)
          {
            Logger.Log("Moving groups", LogSeverity.Warning, "Could not find sound " + this.Path.SoundName);
            this.Path.SoundName = (string) null;
          }
        }
        if (this.IsConnective)
        {
          this.eConnectiveIdle = SoundEffectExtensions.EmitAt(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Industrial/ConnectiveIdle"), Vector3.Zero, true, true);
          this.sConnectiveKlonk = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Industrial/ConnectiveKlonk");
          this.sConnectiveStartUp = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Industrial/ConnectiveStartUp");
        }
        if (!this.IsConnective)
        {
          this.attachedAOs = Enumerable.ToArray<ArtObjectInstance>(Enumerable.Where<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x =>
          {
            int? local_0 = x.ActorSettings.AttachedGroup;
            int local_1 = group.Id;
            if (local_0.GetValueOrDefault() == local_1)
              return local_0.HasValue;
            else
              return false;
          })));
          if (this.attachedAOs.Length > 0)
            this.aoOrigins = Enumerable.ToArray<Vector3>(Enumerable.Select<ArtObjectInstance, Vector3>((IEnumerable<ArtObjectInstance>) this.attachedAOs, (Func<ArtObjectInstance, Vector3>) (x => x.Position)));
          this.attachedPlanes = Enumerable.ToArray<BackgroundPlane>(Enumerable.Where<BackgroundPlane>((IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values, (Func<BackgroundPlane, bool>) (x =>
          {
            int? local_0 = x.AttachedGroup;
            int local_1 = group.Id;
            if (local_0.GetValueOrDefault() == local_1)
              return local_0.HasValue;
            else
              return false;
          })));
          if (this.attachedPlanes.Length > 0)
            this.planeOrigins = Enumerable.ToArray<Vector3>(Enumerable.Select<BackgroundPlane, Vector3>((IEnumerable<BackgroundPlane>) this.attachedPlanes, (Func<BackgroundPlane, Vector3>) (x => x.Position)));
        }
        else
        {
          foreach (TrileInstance trileInstance in this.Group.Triles)
            trileInstance.ForceClampToGround = true;
        }
        this.Enabled = true;
        this.Silent = true;
        this.Reset();
        this.StartNewSegment();
        this.Silent = false;
        this.sinceSegmentStarted -= TimeSpan.FromSeconds((double) this.Path.OffsetSeconds);
      }

      public void MoveTo(MovingGroupsHost.MovingGroupState oldTrack)
      {
        this.Silent = true;
        bool enabled = this.Enabled;
        this.Enabled = true;
        for (int index = 0; index < this.Segments.Count && (!FezMath.AlmostEqual(this.MarkedPosition + this.segmentOrigin, oldTrack.MarkedPosition + oldTrack.segmentOrigin) || this.CurrentSegment.Bounced != oldTrack.CurrentSegment.Bounced); ++index)
          this.ChangeSegment();
        if (this.CurrentSegment.Duration != TimeSpan.Zero)
          this.sinceSegmentStarted = oldTrack.sinceSegmentStarted;
        this.Update(TimeSpan.Zero);
        this.Enabled = enabled;
        this.Silent = false;
      }

      public void MoveToEnd()
      {
        this.Silent = true;
        bool enabled = this.Enabled;
        PathEndBehavior endBehavior = this.Path.EndBehavior;
        int count = this.Segments.Count;
        if (endBehavior != PathEndBehavior.Bounce)
        {
          for (int index = count - 1; index >= 0; --index)
          {
            PathSegment pathSegment = this.Segments[index];
            Vector3 vector3 = index == 0 ? Vector3.Zero : this.Segments[index - 1].Destination;
            this.Segments.Add(new PathSegment()
            {
              Acceleration = pathSegment.Deceleration,
              Deceleration = pathSegment.Acceleration,
              Destination = vector3,
              Duration = pathSegment.Duration,
              JitterFactor = pathSegment.JitterFactor,
              WaitTimeOnFinish = pathSegment.WaitTimeOnStart,
              WaitTimeOnStart = pathSegment.WaitTimeOnFinish,
              Bounced = true
            });
          }
        }
        this.Enabled = true;
        this.Path.NeedsTrigger = false;
        this.Path.EndBehavior = PathEndBehavior.Bounce;
        while (!this.CurrentSegment.Bounced)
          this.ChangeSegment();
        this.sinceSegmentStarted = TimeSpan.Zero;
        this.Update(TimeSpan.Zero);
        this.Path.EndBehavior = endBehavior;
        this.Enabled = enabled;
        this.Path.NeedsTrigger = this.neededTrigger;
        this.Silent = false;
        if (endBehavior == PathEndBehavior.Bounce)
          return;
        this.Segments.RemoveRange(count, this.Segments.Count - count);
      }

      public void Reset()
      {
        if (this.CurrentSegmentIndex >= 0 && this.CurrentSegmentIndex < this.Segments.Count)
        {
          this.lastSegmentOrigin = this.segmentOrigin;
          this.segmentOrigin = this.CurrentSegment.Destination;
        }
        if (this.lastSegmentOrigin != Vector3.Zero)
        {
          this.Group.Triles.Sort((IComparer<TrileInstance>) new MovingTrileInstanceComparer(-this.lastSegmentOrigin));
          Array.Sort<Vector3>(this.referenceOrigins, (IComparer<Vector3>) new MovingPositionComparer(-this.lastSegmentOrigin));
          for (int index = 0; index < this.Group.Triles.Count; ++index)
          {
            this.Group.Triles[index].PhysicsState.Velocity = Vector3.Zero;
            this.Group.Triles[index].Position = this.referenceOrigins[index] -= this.lastSegmentOrigin;
            this.LevelManager.UpdateInstance(this.Group.Triles[index]);
          }
        }
        if (this.aoOrigins != null)
        {
          for (int index = 0; index < this.aoOrigins.Length; ++index)
            this.attachedAOs[index].Position = this.aoOrigins[index];
        }
        if (this.planeOrigins != null)
        {
          for (int index = 0; index < this.planeOrigins.Length; ++index)
            this.attachedPlanes[index].Position = this.planeOrigins[index];
        }
        this.CurrentSegmentIndex = 0;
        this.lastSegmentOrigin = this.segmentOrigin = Vector3.Zero;
        this.sinceSegmentStarted = TimeSpan.Zero;
        if (!this.Path.Backwards)
          return;
        this.Path.Backwards = false;
        while (this.CurrentSegmentIndex != this.Segments.Count - 1)
          this.ChangeSegment();
        this.Path.Backwards = true;
        this.sinceSegmentStarted = this.CurrentSegment.Duration + this.CurrentSegment.WaitTimeOnFinish;
      }

      public void Update(TimeSpan elapsed)
      {
        if (!this.Enabled)
          return;
        if (this.Path.NeedsTrigger)
        {
          if (!(this.lastVelocity != Vector3.Zero))
            return;
          for (int index = 0; index < this.Group.Triles.Count; ++index)
            this.Group.Triles[index].PhysicsState.Velocity = Vector3.Zero;
          this.lastVelocity = Vector3.Zero;
          this.PauseSound();
        }
        else if (!this.LevelManager.Groups.ContainsKey(this.Group.Id))
        {
          this.Enabled = false;
        }
        else
        {
          if (this.eAssociatedSound != null)
          {
            if (this.lastVelocity != Vector3.Zero && this.eAssociatedSound.Cue.State == SoundState.Paused)
              this.eAssociatedSound.Cue.Resume();
            this.eAssociatedSound.Position = this.Group.Triles[this.Group.Triles.Count / 2].Center;
          }
          if (this.IsConnective)
          {
            if (this.lastVelocity != Vector3.Zero && this.eConnectiveIdle.Cue.State == SoundState.Paused)
            {
              if (this.IsConnective && !this.Silent && !this.SkipStartUp)
                SoundEffectExtensions.EmitAt(this.sConnectiveStartUp, this.eConnectiveIdle.Position);
              this.eConnectiveIdle.Cue.Resume();
              this.SkipStartUp = false;
            }
            this.eConnectiveIdle.Position = this.Group.Triles[this.Group.Triles.Count / 2].Center;
          }
          this.SinceKlonk += (float) elapsed.TotalSeconds;
          if (this.neededTrigger && (this.CurrentSegmentIndex == this.Segments.Count || this.CurrentSegmentIndex == -1))
          {
            this.Reset();
            if (!this.Path.Backwards)
              this.StartNewSegment();
          }
          if (this.neededTrigger && this.Path.RunSingleSegment && this.paused)
          {
            for (int index = 0; index < this.Group.Triles.Count; ++index)
              this.referenceOrigins[index] = this.Group.Triles[index].Position - this.segmentOrigin;
            this.paused = false;
          }
          TimeSpan timeSpan = this.sinceSegmentStarted;
          if (this.Path.Backwards)
            this.sinceSegmentStarted -= elapsed;
          else
            this.sinceSegmentStarted += elapsed;
          if (timeSpan.TotalSeconds < 0.0 && this.sinceSegmentStarted.TotalSeconds >= 0.0 && this.Group.ActorType == ActorType.Piston)
          {
            if ((double) this.CurrentSegment.Destination.Y >= 2.0)
              SoundEffectExtensions.EmitAt(this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/Piston"), this.Group.Triles[this.Group.Triles.Count / 2].Center, RandomHelper.Centered(0.100000001490116));
            else
              SoundEffectExtensions.EmitAt(this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/PistonSetBack"), this.Group.Triles[this.Group.Triles.Count / 2].Center, RandomHelper.Centered(0.100000001490116));
          }
          float f = (float) FezMath.Saturate(this.sinceSegmentStarted.TotalSeconds / this.CurrentSegment.Duration.TotalSeconds);
          if (float.IsNaN(f))
            f = 1f;
          float num = (double) this.CurrentSegment.Deceleration != 0.0 || (double) this.CurrentSegment.Acceleration != 0.0 ? ((double) this.CurrentSegment.Acceleration != 0.0 ? ((double) this.CurrentSegment.Deceleration != 0.0 ? Easing.EaseInOut((double) f, EasingType.Sine, this.CurrentSegment.Acceleration, EasingType.Sine, this.CurrentSegment.Deceleration) : Easing.Ease((double) f, this.CurrentSegment.Acceleration, EasingType.Quadratic)) : Easing.Ease((double) f, -this.CurrentSegment.Deceleration, EasingType.Quadratic)) : f;
          if (this.Path.Segments.Count == 1 && this.Path.EndBehavior == PathEndBehavior.Stop && (this.eAssociatedSound != null && !this.eAssociatedSound.Dead))
            this.eAssociatedSound.VolumeFactor = (float) (1.0 - (double) Easing.EaseIn((double) num, EasingType.Quartic) * 0.824999988079071);
          Vector3 vector3_1 = (this.CurrentSegment.Destination - this.segmentOrigin) * num;
          if ((!this.Path.Backwards && this.sinceSegmentStarted < this.CurrentSegment.Duration || this.Path.Backwards && this.sinceSegmentStarted.Ticks > 0L) && (double) this.CurrentSegment.JitterFactor > 0.0)
            vector3_1 += new Vector3(RandomHelper.Centered((double) this.CurrentSegment.JitterFactor) * 0.5f, RandomHelper.Centered((double) this.CurrentSegment.JitterFactor) * 0.5f, RandomHelper.Centered((double) this.CurrentSegment.JitterFactor) * 0.5f);
          Vector3 vector3_2 = this.referenceOrigins[0] + vector3_1 - this.Group.Triles[0].Position;
          this.lastVelocity = vector3_2;
          if (vector3_2 == Vector3.Zero)
            this.PauseSound();
          foreach (TrileInstance instance in this.Group.Triles)
          {
            instance.PhysicsState.Sticky = (double) this.CurrentSegment.JitterFactor != 0.0 && ((double) f < 0.1 || (double) f > 0.9);
            instance.PhysicsState.Velocity = vector3_2;
            instance.Position += vector3_2;
            this.LevelManager.UpdateInstance(instance);
          }
          if (elapsed.Ticks != 0L || this.Silent)
          {
            if (this.aoOrigins != null)
            {
              for (int index = 0; index < this.aoOrigins.Length; ++index)
                this.attachedAOs[index].Position += vector3_2;
            }
            if (this.planeOrigins != null)
            {
              for (int index = 0; index < this.planeOrigins.Length; ++index)
                this.attachedPlanes[index].Position = this.planeOrigins[index];
            }
          }
          if (!this.Path.Backwards && this.sinceSegmentStarted >= this.CurrentSegment.Duration + this.CurrentSegment.WaitTimeOnFinish || this.Path.Backwards && this.sinceSegmentStarted <= -this.CurrentSegment.WaitTimeOnStart)
            this.ChangeSegment();
          if (this.Enabled && !this.Path.NeedsTrigger)
            return;
          this.PauseSound();
        }
      }

      public void PauseSound()
      {
        if (this.eAssociatedSound != null && this.eAssociatedSound.Cue.State == SoundState.Playing)
          this.eAssociatedSound.Cue.Pause();
        if (!this.IsConnective || this.eConnectiveIdle.Cue.State != SoundState.Playing)
          return;
        this.eConnectiveIdle.Cue.Pause();
      }

      public void StopSound()
      {
        if (this.eAssociatedSound != null && !this.eAssociatedSound.Dead)
        {
          this.eAssociatedSound.FadeOutAndDie(0.25f);
          this.eAssociatedSound = (SoundEmitter) null;
        }
        if (this.eConnectiveIdle == null || this.eConnectiveIdle.Dead)
          return;
        this.eConnectiveIdle.FadeOutAndDie(0.1f);
        this.eConnectiveIdle = (SoundEmitter) null;
      }

      private void ChangeSegment()
      {
        if (this.Path.Backwards)
        {
          if (this.CurrentSegmentIndex == 0)
          {
            --this.CurrentSegmentIndex;
            this.EndPath();
            if (this.Enabled && !this.Path.NeedsTrigger)
              this.StartNewSegment();
          }
          else
          {
            this.Path.Backwards = false;
            int num = this.CurrentSegmentIndex - 1;
            this.Reset();
            while (this.CurrentSegmentIndex != num)
              this.ChangeSegment();
            this.Path.Backwards = true;
            this.sinceSegmentStarted = this.CurrentSegment.Duration + this.CurrentSegment.WaitTimeOnFinish;
            this.Update(TimeSpan.Zero);
          }
          if (!this.Path.RunSingleSegment)
            return;
          this.Path.NeedsTrigger = true;
          this.Path.RunSingleSegment = false;
        }
        else
        {
          this.lastSegmentOrigin = this.segmentOrigin;
          this.segmentOrigin = this.CurrentSegment.Destination;
          this.sinceSegmentStarted -= this.CurrentSegment.Duration + this.CurrentSegment.WaitTimeOnFinish;
          ++this.CurrentSegmentIndex;
          if (this.CurrentSegmentIndex == this.Segments.Count || this.CurrentSegmentIndex == -1)
            this.EndPath();
          if (this.Enabled && !this.Path.NeedsTrigger)
            this.StartNewSegment();
          if (this.Path.RunSingleSegment)
          {
            this.Path.NeedsTrigger = true;
            this.Path.RunSingleSegment = false;
          }
          if (this.Group.ActorType != ActorType.Piston)
            return;
          if ((double) this.CurrentSegment.Destination.Y >= 2.0 && this.LevelManager.Name != "LAVA_FORK")
            SoundEffectExtensions.EmitAt(this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/Piston"), this.Group.Triles[this.Group.Triles.Count / 2].Center, RandomHelper.Centered(0.100000001490116));
          else
            SoundEffectExtensions.EmitAt(this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/PistonSetBack"), this.Group.Triles[this.Group.Triles.Count / 2].Center, RandomHelper.Centered(0.100000001490116));
        }
      }

      private void StartNewSegment()
      {
        if (this.Path.Backwards)
        {
          this.Group.Triles.Sort((IComparer<TrileInstance>) new MovingTrileInstanceComparer(this.CurrentSegment.Destination - this.segmentOrigin));
          Array.Sort<Vector3>(this.referenceOrigins, (IComparer<Vector3>) new MovingPositionComparer(this.CurrentSegment.Destination - this.segmentOrigin));
          Vector3 vector3 = this.lastSegmentOrigin - this.segmentOrigin;
          for (int index = 0; index < this.Group.Triles.Count; ++index)
            this.referenceOrigins[index] += vector3;
          this.sinceSegmentStarted += this.CurrentSegment.Duration + this.CurrentSegment.WaitTimeOnFinish;
        }
        else
        {
          Vector3 vector3 = this.segmentOrigin - this.lastSegmentOrigin;
          Vector3 ordering = this.CurrentSegmentIndex == this.Segments.Count ? vector3 : this.CurrentSegment.Destination - this.segmentOrigin;
          this.Group.Triles.Sort((IComparer<TrileInstance>) new MovingTrileInstanceComparer(ordering));
          Array.Sort<Vector3>(this.referenceOrigins, (IComparer<Vector3>) new MovingPositionComparer(ordering));
          for (int index = 0; index < this.Group.Triles.Count; ++index)
            this.referenceOrigins[index] += vector3;
          if (this.CurrentSegmentIndex < this.Segments.Count)
            this.sinceSegmentStarted -= this.CurrentSegment.WaitTimeOnStart;
        }
        if (!this.IsConnective || this.Silent)
          return;
        if ((double) this.SinceKlonk < 0.100000001490116)
          Waiters.Wait(0.100000001490116, (Action) (() =>
          {
            if (this.eConnectiveIdle == null)
              return;
            SoundEffectExtensions.EmitAt(this.sConnectiveKlonk, this.eConnectiveIdle.Position, 0.0f, 0.1f);
          }));
        else
          SoundEffectExtensions.EmitAt(this.sConnectiveKlonk, this.eConnectiveIdle.Position);
        this.SinceKlonk = 0.0f;
      }

      private void EndPath()
      {
        foreach (TrileInstance trileInstance in this.Group.Triles)
          trileInstance.PhysicsState.Velocity = Vector3.Zero;
        if (this.Path.EndBehavior == PathEndBehavior.Stop)
        {
          if (this.neededTrigger && this.Path.RunOnce)
          {
            this.Path.NeedsTrigger = true;
            this.Path.RunOnce = false;
          }
          else
            this.Enabled = false;
        }
        else if (this.neededTrigger && this.Path.RunOnce)
        {
          this.Path.NeedsTrigger = true;
          this.Path.RunOnce = false;
        }
        else
          this.Reset();
      }

      public void PrepareOldTrack(bool wasRunning)
      {
        this.SkipStartUp = true;
        if (!wasRunning)
          return;
        this.eConnectiveIdle.Cue.Resume();
      }
    }
  }
}
