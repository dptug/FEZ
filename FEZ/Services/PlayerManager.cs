// Type: FezGame.Services.PlayerManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Services
{
  public class PlayerManager : IPlayerManager, IComplexPhysicsEntity, IPhysicsEntity
  {
    private readonly Vector3 BaseSize = new Vector3(0.625f, 15.0 / 16.0, 1f);
    private readonly Stack<object> controlStack = new Stack<object>();
    private const float TrixelSize = 0.0625f;
    private Vector3 position;
    private Vector3 variableSize;
    private ActionType action;
    private TrileInstance lastHeldInstance;
    private MultipleHits<TrileInstance> lastGround;
    private Dictionary<ActionType, AnimatedTexture> HatAnimations;
    private Dictionary<ActionType, AnimatedTexture> NoHatAnimations;
    private TrileInstance carriedInstance;
    private TrileInstance lastCarriedInstance;

    public Vector3 Velocity { get; set; }

    public Vector3? GroundedVelocity { get; set; }

    public MultipleHits<TrileInstance> Ground { get; set; }

    public MultipleHits<CollisionResult> Ceiling { get; set; }

    public bool MustBeClampedToGround { get; set; }

    public bool CanRotate { get; set; }

    public List<Volume> CurrentVolumes { get; private set; }

    public TrileInstance HeldInstance { get; set; }

    public TrileInstance PushedInstance { get; set; }

    public TimeSpan AirTime { get; set; }

    public bool CanDoubleJump { get; set; }

    public float BlinkSpeed { get; set; }

    public bool NoVelocityClamping
    {
      get
      {
        return false;
      }
    }

    public HorizontalDirection MovingDirection { get; set; }

    public HorizontalDirection LookingDirection { get; set; }

    public Vector3 GroundMovement { get; set; }

    public ActionType NextAction { get; set; }

    public ActionType LastAction { get; set; }

    public ActionType LastGroundedAction { get; private set; }

    public Viewpoint LastGroundedView { get; private set; }

    public HorizontalDirection LastGroundedLookingDirection { get; private set; }

    public TrileInstance CheckpointGround { get; set; }

    public Vector3 RespawnPosition { get; set; }

    public Vector3 LeaveGroundPosition { get; set; }

    public bool InDoorTransition { get; set; }

    public PointCollision[] CornerCollision { get; private set; }

    public Dictionary<VerticalDirection, NearestTriles> AxisCollision { get; private set; }

    public bool CanControl
    {
      get
      {
        return this.controlStack.Count == 0;
      }
      set
      {
        if (value && this.controlStack.Count > 0)
        {
          this.controlStack.Pop();
        }
        else
        {
          if (value)
            return;
          this.controlStack.Push(new object());
        }
      }
    }

    public bool Background { get; set; }

    public float Elasticity
    {
      get
      {
        return 0.0f;
      }
    }

    public Mesh Mesh { get; set; }

    public AnimatedTexture Animation { get; set; }

    public TimeSpan InvincibilityLeft { get; set; }

    public string NextLevel { get; set; }

    public int? DoorVolume { get; set; }

    public int? TunnelVolume { get; set; }

    public bool SpinThroughDoor { get; set; }

    public int? PipeVolume { get; set; }

    public bool IgnoreFreefall { get; set; }

    public bool IsOnRotato { get; set; }

    public bool DoorEndsTrial { get; set; }

    public TrileInstance ForcedTreasure { get; set; }

    public Vector3 SplitUpCubeCollectorOffset { get; set; }

    public bool HideFez
    {
      get
      {
        return this.GameState.SaveData.FezHidden;
      }
      set
      {
        this.GameState.SaveData.FezHidden = value;
      }
    }

    public float GomezOpacity { get; set; }

    public WarpPanel WarpPanel { get; set; }

    public Viewpoint OriginWarpViewpoint { get; set; }

    public bool Swimming
    {
      get
      {
        return ActionTypeExtensions.IsSwimming(this.Action);
      }
    }

    public Vector3 Size
    {
      get
      {
        if (this.CameraManager.Viewpoint == Viewpoint.Left || this.CameraManager.Viewpoint == Viewpoint.Right)
          return FezMath.ZYX(this.variableSize);
        else
          return this.variableSize;
      }
    }

    public Vector3 Center
    {
      get
      {
        return this.position + FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.LookingDirection) * 0.125f + Vector3.UnitY * (this.variableSize.Y - this.BaseSize.Y);
      }
      set
      {
        this.Position = value - FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.LookingDirection) * 0.125f - Vector3.UnitY * (this.variableSize.Y - this.BaseSize.Y);
      }
    }

    public Vector3 Position
    {
      get
      {
        return this.position;
      }
      set
      {
        this.position = value;
      }
    }

    public bool Grounded
    {
      get
      {
        return this.Ground.First != null;
      }
    }

    public MultipleHits<CollisionResult> WallCollision { get; set; }

    public ActionType Action
    {
      get
      {
        return this.action;
      }
      set
      {
        if (this.action != value)
          this.LastAction = this.action;
        this.action = value;
      }
    }

    public TrileInstance CarriedInstance
    {
      get
      {
        return this.carriedInstance;
      }
      set
      {
        this.carriedInstance = value;
        if (this.Action != ActionType.ThrowingHeavy)
          this.SyncCollisionSize();
        if (this.carriedInstance != null)
        {
          this.CameraManager.RecordNewCarriedInstancePhi();
          this.carriedInstance.PhysicsState.Ground = new MultipleHits<TrileInstance>();
        }
        else
        {
          if (this.Action == ActionType.WakingUp)
            return;
          this.ForceOverlapsDetermination();
        }
      }
    }

    public bool Climbing
    {
      get
      {
        if (!ActionTypeExtensions.IsClimbingLadder(this.Action))
          return ActionTypeExtensions.IsClimbingVine(this.Action);
        else
          return true;
      }
    }

    public bool Sliding
    {
      get
      {
        return this.Action == ActionType.Sliding;
      }
    }

    public bool HandlesZClamping
    {
      get
      {
        return ActionTypeExtensions.HandlesZClamping(this.Action);
      }
    }

    public bool Hidden { get; set; }

    public bool FullBright { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public IVolumeService VolumeService { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPhysicsManager PhysicsManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public PlayerManager()
    {
      this.HatAnimations = new Dictionary<ActionType, AnimatedTexture>(Enumerable.Count<ActionType>(Util.GetValues<ActionType>()) - 1, (IEqualityComparer<ActionType>) ActionTypeComparer.Default);
      this.NoHatAnimations = new Dictionary<ActionType, AnimatedTexture>((IEqualityComparer<ActionType>) ActionTypeComparer.Default);
      this.Reset();
    }

    public void FillAnimations()
    {
      this.HatAnimations.Clear();
      this.NoHatAnimations.Clear();
      foreach (ActionType actionType in Util.GetValues<ActionType>())
      {
        if (actionType != ActionType.None)
        {
          string assetName = "Character Animations/Gomez/" + ActionTypeExtensions.GetAnimationPath(actionType);
          this.HatAnimations.Add(actionType, this.CMProvider.Global.Load<AnimatedTexture>(assetName));
          if (MemoryContentManager.AssetExists(assetName.Replace('/', '\\') + "_NoHat"))
          {
            AnimatedTexture animatedTexture = this.CMProvider.Global.Load<AnimatedTexture>(assetName + "_NoHat");
            animatedTexture.NoHat = true;
            this.NoHatAnimations.Add(actionType, animatedTexture);
          }
        }
      }
    }

    public AnimatedTexture GetAnimation(ActionType type)
    {
      if (this.HideFez && !this.GameState.SaveData.IsNewGamePlus && this.NoHatAnimations.ContainsKey(type))
        return this.NoHatAnimations[type];
      else
        return this.HatAnimations[type];
    }

    public void Reset()
    {
      this.Ground = new MultipleHits<TrileInstance>();
      this.CornerCollision = new PointCollision[4];
      this.AxisCollision = new Dictionary<VerticalDirection, NearestTriles>((IEqualityComparer<VerticalDirection>) VerticalDirectionComparer.Default)
      {
        {
          VerticalDirection.Up,
          new NearestTriles()
        },
        {
          VerticalDirection.Down,
          new NearestTriles()
        }
      };
      this.Background = false;
      this.variableSize = this.BaseSize;
      this.CanRotate = true;
      this.controlStack.Clear();
      this.CanControl = true;
      this.Action = ActionType.Idle;
      this.CurrentVolumes = new List<Volume>();
      this.GomezOpacity = 1f;
      this.InDoorTransition = false;
      this.FullBright = false;
    }

    public void SyncCollisionSize()
    {
      if (this.carriedInstance != null && this.lastCarriedInstance == null)
      {
        this.variableSize = !ActorTypeExtensions.IsLight(this.carriedInstance.Trile.ActorSettings.Type) ? new Vector3(0.75f, 1.75f, 0.75f) : new Vector3(0.75f, 31.0 / 16.0, 0.75f);
        this.Position -= (this.variableSize - this.BaseSize) / 2f * Vector3.UnitY;
      }
      else if (this.carriedInstance == null && this.lastCarriedInstance != null)
      {
        this.Position += (this.variableSize - this.BaseSize) / 2f * Vector3.UnitY;
        this.variableSize = this.BaseSize;
      }
      this.lastCarriedInstance = this.carriedInstance;
    }

    public void RespawnAtCheckpoint()
    {
      this.HeldInstance = (TrileInstance) null;
      this.CarriedInstance = (TrileInstance) null;
      this.IsOnRotato = false;
      this.GameState.SkipRendering = true;
      this.CameraManager.ChangeViewpoint(this.GameState.SaveData.View, 0.0f);
      this.GameState.SkipRendering = false;
      this.CameraManager.SnapInterpolation();
      TrileInstance trileInstance = this.CheckpointGround ?? this.LevelManager.ActualInstanceAt(this.GameState.SaveData.Ground) ?? this.LevelManager.NearestTrile(this.GameState.SaveData.Ground).Deep;
      this.RespawnPosition = trileInstance != null ? (this.Position = trileInstance.Center + (trileInstance.TransformedSize / 2f + this.Size / 2f) * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor)) : (this.Position = this.GameState.SaveData.Ground + this.Size / 2f * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor));
      if (this.GameState.FarawaySettings.InTransition)
        this.CameraManager.SnapInterpolation();
      this.Action = ActionType.WakingUp;
      this.LookingDirection = HorizontalDirection.Right;
      this.ForceOverlapsDetermination();
      this.PhysicsManager.HugWalls((IPhysicsEntity) this, false, false, true);
    }

    public void Respawn()
    {
      foreach (Volume volume in this.CurrentVolumes)
      {
        if (!volume.PlayerInside)
          this.VolumeService.OnExit(volume.Id);
        volume.PlayerInside = false;
      }
      this.CurrentVolumes.Clear();
      this.IsOnRotato = false;
      this.Position = this.RespawnPosition + Vector3.UnitY * 1f / 32f * (float) Math.Sign(this.CollisionManager.GravityFactor);
      if (this.LastGroundedAction == ActionType.None)
        this.LastGroundedAction = ActionType.Idle;
      this.Action = this.LastGroundedAction;
      this.LookingDirection = this.LastGroundedLookingDirection;
      this.Velocity = Vector3.Down * 1f / 16f * (float) Math.Sign(this.CollisionManager.GravityFactor);
      this.HeldInstance = this.lastHeldInstance;
      this.Ground = this.lastGround;
      if (this.LastGroundedView == Viewpoint.None)
        this.LastGroundedView = this.CameraManager.Viewpoint;
      this.CameraManager.ChangeViewpoint(this.LastGroundedView);
      this.GroundedVelocity = new Vector3?();
      if (!this.CameraManager.Constrained)
        this.CameraManager.Center = this.Position + Vector3.UnitY * ((float) (4.0 * (this.LevelManager.Descending ? -1.0 : 1.0)) / this.CameraManager.PixelsPerTrixel);
      this.LevelManager.ScreenInvalidated += new Action(this.ForceOverlapsDetermination);
    }

    public void RecordRespawnInformation()
    {
      this.RecordRespawnInformation(false);
    }

    public void RecordRespawnInformation(bool markCheckpoint)
    {
      if (!this.Grounded && !this.Climbing && (this.action != ActionType.GrabCornerLedge && !ActionTypeExtensions.IsSwimming(this.action)) && this.action != ActionType.EnteringPipe)
        return;
      TrileInstance first = this.Ground.First;
      if (this.Climbing)
      {
        Vector3 vector3 = FezMath.SideMask(this.CameraManager.Viewpoint);
        this.LeaveGroundPosition = vector3 * FezMath.Floor(this.Position) + vector3 * 0.5f + Vector3.UnitY * ((float) (int) Math.Ceiling((double) this.Position.Y) + 0.5f) + (Vector3.One - vector3 - Vector3.UnitY) * this.Position;
      }
      else
        this.LeaveGroundPosition = this.action == ActionType.GrabCornerLedge || ActionTypeExtensions.IsSwimming(this.action) || this.action == ActionType.EnteringPipe ? this.Position : first.Center + (float) ((double) first.TransformedSize.Y / 2.0 + (double) this.Size.Y / 2.0) * Vector3.UnitY * (float) Math.Sign(this.CollisionManager.GravityFactor);
      if (!ActionTypeExtensions.DisallowsRespawn(this.Action) && this.CarriedInstance == null && !this.Background && (!this.Grounded || first.PhysicsState == null && !first.Unsafe) && (this.HeldInstance == null || this.HeldInstance.PhysicsState == null) && (this.Grounded && ActorTypeExtensions.IsSafe(first.Trile.ActorSettings.Type) || this.Action == ActionType.GrabCornerLedge && this.HeldInstance != null && (!this.HeldInstance.Unsafe && ActorTypeExtensions.IsSafe(this.HeldInstance.Trile.ActorSettings.Type))))
      {
        this.LastGroundedAction = this.Action;
        this.LastGroundedView = this.CameraManager.Viewpoint;
        this.LastGroundedLookingDirection = this.LookingDirection;
        this.RespawnPosition = this.LeaveGroundPosition;
        this.lastGround = this.Ground;
        this.lastHeldInstance = this.HeldInstance;
      }
      if (!markCheckpoint && this.LastGroundedView != Viewpoint.None)
        return;
      this.GameState.SaveData.View = this.CameraManager.Viewpoint;
      this.GameState.SaveData.TimeOfDay = this.TimeManager.CurrentTime.TimeOfDay;
      this.CheckpointGround = first;
      this.GameState.SaveData.Ground = this.CheckpointGround.Center;
      this.GameState.SaveData.Level = this.LevelManager.Name;
    }

    public void ForceOverlapsDetermination()
    {
      this.PhysicsManager.DetermineOverlaps((IComplexPhysicsEntity) this);
    }
  }
}
