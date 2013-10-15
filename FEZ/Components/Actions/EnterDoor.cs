// Type: FezGame.Components.Actions.EnterDoor
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components.Actions
{
  public class EnterDoor : PlayerAction
  {
    private float step = -1f;
    private Mesh fadeOutQuad;
    private Mesh trileFadeQuad;
    private bool hasFlipped;
    private bool hasChangedLevel;
    private string newLevel;
    private Vector3 spinOrigin;
    private Vector3 spinDestination;
    private SoundEffect sound;
    private bool skipFade;
    private TimeSpan transitionTime;
    private bool skipPreview;

    protected override bool ViewTransitionIndependent
    {
      get
      {
        return true;
      }
    }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    [ServiceDependency]
    public IDotManager DotManager { private get; set; }

    [ServiceDependency]
    public IGameService GameService { private get; set; }

    [ServiceDependency]
    public IVolumeService VolumeService { private get; set; }

    public EnterDoor(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      this.fadeOutQuad = new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false
      };
      this.fadeOutQuad.AddFace(Vector3.One * 2f, Vector3.Zero, FaceOrientation.Front, Color.Black, true);
      Mesh mesh = this.fadeOutQuad;
      DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
      vertexColored1.ForcedViewMatrix = new Matrix?(Matrix.Identity);
      vertexColored1.ForcedProjectionMatrix = new Matrix?(Matrix.Identity);
      DefaultEffect.VertexColored vertexColored2 = vertexColored1;
      mesh.Effect = (BaseEffect) vertexColored2;
      this.trileFadeQuad = new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false
      };
      this.trileFadeQuad.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, Color.Black, true);
      this.trileFadeQuad.Effect = (BaseEffect) new DefaultEffect.VertexColored();
      this.sound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/EnterDoor");
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        this.skipPreview = true;
        this.VolumeService.Exit += new Action<int>(this.RevertSkipPreview);
      });
      this.DrawOrder = 901;
    }

    private void RevertSkipPreview(int id)
    {
      if (!this.PlayerManager.CanControl)
        return;
      this.skipPreview = false;
      this.VolumeService.Exit -= new Action<int>(this.RevertSkipPreview);
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.Teetering:
        case ActionType.IdlePlay:
        case ActionType.IdleSleep:
        case ActionType.IdleLookAround:
        case ActionType.IdleYawn:
        case ActionType.Idle:
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Jumping:
        case ActionType.Lifting:
        case ActionType.Falling:
        case ActionType.Bouncing:
        case ActionType.Flying:
        case ActionType.Dropping:
        case ActionType.Sliding:
        case ActionType.Landing:
          string key = this.PlayerManager.NextLevel;
          if (this.PlayerManager.NextLevel == "CABIN_INTERIOR_A")
            key = "CABIN_INTERIOR_B";
          if (this.PlayerManager.DoorVolume.HasValue && this.PlayerManager.Grounded && (!this.PlayerManager.HideFez && this.PlayerManager.CanControl) && (!this.PlayerManager.Background && !this.DotManager.PreventPoI && (this.GameState.SaveData.World.ContainsKey(key) && !this.skipPreview)) && (key != this.LevelManager.Name && this.LevelManager.Name != "CRYPT" && this.LevelManager.Name != "PYRAMID"))
          {
            if (MemoryContentManager.AssetExists("Other Textures\\map_screens\\" + key.Replace('/', '\\')))
            {
              Texture2D texture2D = this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/map_screens/" + key);
              this.DotManager.Behaviour = DotHost.BehaviourType.ThoughtBubble;
              this.DotManager.DestinationVignette = texture2D;
              this.DotManager.ComeOut();
              if (this.DotManager.Owner != this)
                this.DotManager.Hey();
              this.DotManager.Owner = (object) this;
            }
            else
              this.UnDotize();
          }
          else
            this.UnDotize();
          if ((double) this.step != -1.0 || this.InputManager.ExactUp != FezButtonState.Pressed && this.PlayerManager.LastAction != ActionType.OpeningDoor || !this.PlayerManager.Grounded || (!this.PlayerManager.DoorVolume.HasValue || this.PlayerManager.Background))
            break;
          this.UnDotize();
          this.GameState.SkipLoadScreen = this.skipFade = this.LevelManager.DestinationVolumeId.HasValue && this.PlayerManager.NextLevel == this.LevelManager.Name;
          bool spinThroughDoor = this.PlayerManager.SpinThroughDoor;
          if (spinThroughDoor)
          {
            Vector3 b = FezMath.ForwardVector(this.CameraManager.Viewpoint);
            Vector3 vector3 = FezMath.DepthMask(this.CameraManager.Viewpoint);
            Volume volume = this.LevelManager.Volumes[this.PlayerManager.DoorVolume.Value];
            Vector3 a = (volume.From + volume.To) / 2f - (volume.To - volume.From) / 2f * b - b;
            if ((double) FezMath.Dot(this.PlayerManager.Position, b) < (double) FezMath.Dot(a, b))
              this.PlayerManager.Position = this.PlayerManager.Position * (Vector3.One - vector3) + a * vector3;
            this.spinOrigin = this.GetDestination();
            this.spinDestination = this.GetDestination() + b * 1.5f;
          }
          if (this.PlayerManager.CarriedInstance != null)
          {
            bool flag = ActorTypeExtensions.IsLight(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type);
            this.PlayerManager.Position = this.GetDestination();
            this.PlayerManager.Action = flag ? (spinThroughDoor ? ActionType.EnterDoorSpinCarry : ActionType.CarryEnter) : (spinThroughDoor ? ActionType.EnterDoorSpinCarryHeavy : ActionType.CarryHeavyEnter);
            break;
          }
          else
          {
            this.WalkTo.Destination = new Func<Vector3>(this.GetDestination);
            this.PlayerManager.Action = ActionType.WalkingTo;
            this.WalkTo.NextAction = spinThroughDoor ? ActionType.EnterDoorSpin : ActionType.EnteringDoor;
            break;
          }
      }
    }

    private void UnDotize()
    {
      if (this.DotManager.Owner != this)
        return;
      this.DotManager.Behaviour = DotHost.BehaviourType.FollowGomez;
      this.DotManager.Owner = (object) null;
      this.DotManager.DestinationVignette = (Texture2D) null;
      this.DotManager.Burrow();
    }

    private Vector3 GetDestination()
    {
      if (!this.PlayerManager.DoorVolume.HasValue || !this.LevelManager.Volumes.ContainsKey(this.PlayerManager.DoorVolume.Value))
        return this.PlayerManager.Position;
      Volume volume = this.LevelManager.Volumes[this.PlayerManager.DoorVolume.Value];
      return this.PlayerManager.Position * (Vector3.UnitY + FezMath.DepthMask(this.CameraManager.Viewpoint)) + (volume.From + volume.To) / 2f * FezMath.SideMask(this.CameraManager.Viewpoint);
    }

    protected override void Begin()
    {
      base.Begin();
      if (this.GameState.IsTrialMode && this.PlayerManager.DoorEndsTrial)
      {
        this.GameService.EndTrial(false);
        this.PlayerManager.Action = ActionType.ExitDoor;
        this.PlayerManager.Action = ActionType.Idle;
      }
      else if (!this.PlayerManager.DoorVolume.HasValue || !this.LevelManager.Volumes.ContainsKey(this.PlayerManager.DoorVolume.Value))
      {
        this.PlayerManager.Action = ActionType.Idle;
      }
      else
      {
        if (!this.PlayerManager.SpinThroughDoor)
          SoundEffectExtensions.EmitAt(this.sound, this.PlayerManager.Position);
        this.hasFlipped = this.hasChangedLevel = false;
        this.newLevel = this.PlayerManager.NextLevel;
        this.step = 0.0f;
        this.PlayerManager.InDoorTransition = true;
        this.transitionTime = new TimeSpan();
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3_1 = playerManager.Velocity * Vector3.UnitY;
        playerManager.Velocity = vector3_1;
        if (this.PlayerManager.SpinThroughDoor)
        {
          this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, 1), 2f);
          this.PlayerManager.LookingDirection = HorizontalDirection.Right;
        }
        if (this.LevelManager.DestinationIsFarAway)
        {
          Vector3 center = this.CameraManager.Center;
          float num = (float) (4.0 * (this.LevelManager.Descending ? -1.0 : 1.0)) / this.CameraManager.PixelsPerTrixel;
          this.CameraManager.StickyCam = false;
          this.CameraManager.Constrained = true;
          Volume volume = this.LevelManager.Volumes[this.PlayerManager.DoorVolume.Value];
          Vector2 vector2 = volume.ActorSettings == null ? Vector2.Zero : volume.ActorSettings.FarawayPlaneOffset;
          if (volume.ActorSettings != null && volume.ActorSettings.WaterLocked)
            vector2.Y = volume.ActorSettings.WaterOffset / 2f;
          if (volume.ActorSettings != null)
            this.GameState.FarawaySettings.DestinationOffset = volume.ActorSettings.DestinationOffset;
          Vector3 vector3_2 = FezMath.RightVector(this.CameraManager.Viewpoint) * vector2.X + Vector3.Up * vector2.Y;
          Vector3 destinationCenter = new Vector3(this.PlayerManager.Position.X, this.PlayerManager.Position.Y + num, this.PlayerManager.Position.Z) + vector3_2 * 2f;
          this.StartTransition(center, destinationCenter);
        }
        this.GomezService.OnEnterDoor();
      }
    }

    private void StartTransition(Vector3 originalCenter, Vector3 destinationCenter)
    {
      Waiters.Interpolate(1.75, (Action<float>) (s => this.CameraManager.Center = Vector3.Lerp(originalCenter, destinationCenter, Easing.EaseInOut((double) s, EasingType.Quadratic)))).AutoPause = true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (!ActionTypeExtensions.IsEnteringDoor(type))
        return (double) this.step != -1.0;
      else
        return true;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.GameState.Loading)
        return false;
      if (ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action))
        this.PlayerManager.Animation.Timing.Update(elapsed);
      if ((double) this.step < 1.0 && !this.hasFlipped && this.PlayerManager.SpinThroughDoor)
      {
        Vector3 position = this.PlayerManager.Position;
        this.PlayerManager.Position = Vector3.Lerp(this.spinOrigin, this.spinDestination, this.step);
        if (this.PlayerManager.CarriedInstance != null)
          this.PlayerManager.CarriedInstance.Position += this.PlayerManager.Position - position;
      }
      if (string.IsNullOrEmpty(this.newLevel))
      {
        this.PlayerManager.Action = ActionType.Idle;
        this.step = -1f;
        this.PlayerManager.InDoorTransition = false;
        return false;
      }
      else
      {
        this.transitionTime += elapsed;
        this.step = (float) (this.transitionTime.TotalSeconds / (this.PlayerManager.SpinThroughDoor ? 0.75 : 1.25));
        if ((double) this.step >= 1.0 && !this.hasFlipped)
        {
          if (this.LevelManager.DestinationIsFarAway)
          {
            ServiceHelper.AddComponent((IGameComponent) new FarawayTransition(this.Game));
            this.PlayerManager.Action = ActionType.Idle;
            this.step = -1f;
            this.PlayerManager.InDoorTransition = false;
            return false;
          }
          else
          {
            if (this.skipFade)
            {
              this.DoLoad(false);
            }
            else
            {
              this.GameState.Loading = true;
              Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoLoad));
              worker.Finished += (Action) (() => this.ThreadPool.Return<bool>(worker));
              worker.Start(false);
            }
            this.transitionTime = new TimeSpan();
            this.step = 0.0f;
            this.hasFlipped = true;
          }
        }
        else if ((double) this.step >= 1.0 && this.hasFlipped)
        {
          this.step = -1f;
          this.PlayerManager.SpinThroughDoor = false;
          this.PlayerManager.InDoorTransition = false;
          if (ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action))
            this.PlayerManager.Action = ActionType.Idle;
        }
        if (this.PlayerManager.SpinThroughDoor && this.hasFlipped && (this.CameraManager.ActionRunning && !this.hasChangedLevel))
        {
          this.hasChangedLevel = true;
          this.GameState.SkipRendering = true;
          this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, -1), 0.0f);
          this.CameraManager.SnapInterpolation();
          this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, 1));
          this.GameState.SkipRendering = false;
        }
        return false;
      }
    }

    private void DoLoad(bool dummy)
    {
      this.LevelManager.ChangeLevel(this.newLevel);
      this.PlayerManager.ForceOverlapsDetermination();
      TrileInstance instance1 = this.PlayerManager.AxisCollision[VerticalDirection.Up].Surface;
      if (instance1 != null && instance1.Trile.ActorSettings.Type == ActorType.UnlockedDoor && FezMath.OrientationFromPhi(FezMath.ToPhi(instance1.Trile.ActorSettings.Face) + instance1.Phi) == FezMath.VisibleOrientation(this.CameraManager.Viewpoint))
      {
        ++this.GameState.SaveData.ThisLevel.FilledConditions.UnlockedDoorCount;
        TrileEmplacement id = instance1.Emplacement + Vector3.UnitY;
        TrileInstance instance2 = this.LevelManager.TrileInstanceAt(ref id);
        if (instance2.Trile.ActorSettings.Type == ActorType.UnlockedDoor)
          ++this.GameState.SaveData.ThisLevel.FilledConditions.UnlockedDoorCount;
        this.LevelManager.ClearTrile(instance1);
        this.LevelManager.ClearTrile(instance2);
        this.GameState.SaveData.ThisLevel.InactiveTriles.Add(instance1.Emplacement);
        instance1.ActorSettings.Inactive = true;
      }
      if (!this.PlayerManager.SpinThroughDoor)
      {
        if (this.PlayerManager.CarriedInstance != null)
          this.PlayerManager.Action = ActorTypeExtensions.IsHeavy(this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type) ? ActionType.ExitDoorCarryHeavy : ActionType.ExitDoorCarry;
        else
          this.PlayerManager.Action = ActionType.ExitDoor;
      }
      if (!this.skipFade)
        this.GameState.ScheduleLoadEnd = true;
      this.GameState.SkipLoadScreen = false;
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.IsActionAllowed(this.PlayerManager.Action) || this.LevelManager.DestinationIsFarAway || this.skipFade)
        return;
      float num = (float) Math.Pow((double) FezMath.Saturate(this.step), this.PlayerManager.SpinThroughDoor ? 2.0 : 3.0);
      if (this.PlayerManager.CarriedInstance != null && !this.PlayerManager.SpinThroughDoor)
      {
        this.trileFadeQuad.Rotation = this.CameraManager.Rotation;
        this.trileFadeQuad.Position = this.PlayerManager.CarriedInstance.Center;
        switch (this.PlayerManager.CarriedInstance.Trile.ActorSettings.Type)
        {
          case ActorType.Bomb:
          case ActorType.BigBomb:
            this.trileFadeQuad.Scale = new Vector3(0.75f, 1f, 0.75f);
            break;
          case ActorType.Vase:
            this.trileFadeQuad.Scale = new Vector3(0.875f, 1f, 0.875f);
            break;
          default:
            this.trileFadeQuad.Scale = Vector3.One;
            break;
        }
        this.trileFadeQuad.Material.Opacity = this.hasFlipped ? 1f - this.step : num;
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.NoSilhouette);
        this.trileFadeQuad.Draw();
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.None));
      }
      this.fadeOutQuad.Material.Opacity = this.hasFlipped ? 1f - num : num;
      this.fadeOutQuad.Draw();
    }
  }
}
