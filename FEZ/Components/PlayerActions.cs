// Type: FezGame.Components.PlayerActions
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components.Actions;
using FezGame.Components.Scripting;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  public class PlayerActions : GameComponent
  {
    private readonly Dictionary<SurfaceType, SoundEffect[]> SurfaceHits = new Dictionary<SurfaceType, SoundEffect[]>((IEqualityComparer<SurfaceType>) SurfaceTypeComparer.Default);
    private readonly List<PlayerAction> LightActions = new List<PlayerAction>();
    public const float PlayerSpeed = 4.7f;
    private HorizontalDirection oldLookDir;
    private int lastFrame;
    private SoundEffect LeftStep;
    private SoundEffect RightStep;
    private bool isLeft;

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IPhysicsManager PhysicsManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    internal IScriptingManager ScriptingManager { get; set; }

    public PlayerActions(Game game)
      : base(game)
    {
      this.UpdateOrder = 1;
      WalkTo walkTo = new WalkTo(game);
      ServiceHelper.InjectServices((object) walkTo);
      ServiceHelper.AddComponent((IGameComponent) walkTo, true);
      if (walkTo.UpdateOrder == 0)
      {
        this.LightActions.Add((PlayerAction) walkTo);
        walkTo.Enabled = false;
      }
      this.AddAction((PlayerAction) new Fall(game));
      this.AddAction((PlayerAction) new Idle(game));
      this.AddAction((PlayerAction) new DropDown(game));
      this.AddAction((PlayerAction) new LowerToStraightLedge(game));
      this.AddAction((PlayerAction) new Land(game));
      this.AddAction((PlayerAction) new Slide(game));
      this.AddAction((PlayerAction) new Lift(game));
      this.AddAction((PlayerAction) new Jump(game));
      this.AddAction((PlayerAction) new WalkRun(game));
      this.AddAction((PlayerAction) new Bounce(game));
      this.AddAction((PlayerAction) new ClimbLadder(game));
      this.AddAction((PlayerAction) new ReadSign(game));
      this.AddAction((PlayerAction) new FreeFall(game));
      this.AddAction((PlayerAction) new Die(game));
      this.AddAction((PlayerAction) new Victory(game));
      this.AddAction((PlayerAction) new GrabCornerLedge(game));
      this.AddAction((PlayerAction) new PullUpFromCornerLedge(game));
      this.AddAction((PlayerAction) new LowerToCornerLedge(game));
      this.AddAction((PlayerAction) new Carry(game));
      this.AddAction((PlayerAction) new Throw(game));
      this.AddAction((PlayerAction) new DropTrile(game));
      this.AddAction((PlayerAction) new Suffer(game));
      this.AddAction((PlayerAction) new EnterDoor(game));
      this.AddAction((PlayerAction) new Grab(game));
      this.AddAction((PlayerAction) new Push(game));
      this.AddAction((PlayerAction) new SuckedIn(game));
      this.AddAction((PlayerAction) new ClimbVine(game));
      this.AddAction((PlayerAction) new WakingUp(game));
      this.AddAction((PlayerAction) new Jetpack(game));
      this.AddAction((PlayerAction) new OpenTreasure(game));
      this.AddAction((PlayerAction) new OpenDoor(game));
      this.AddAction((PlayerAction) new Swim(game));
      this.AddAction((PlayerAction) new Sink(game));
      this.AddAction((PlayerAction) new LookAround(game));
      this.AddAction((PlayerAction) new Teeter(game));
      this.AddAction((PlayerAction) new EnterTunnel(game));
      this.AddAction((PlayerAction) new PushPivot(game));
      this.AddAction((PlayerAction) new PullUpFromStraightLedge(game));
      this.AddAction((PlayerAction) new GrabStraightLedge(game));
      this.AddAction((PlayerAction) new ShimmyOnLedge(game));
      this.AddAction((PlayerAction) new ToCornerTransition(game));
      this.AddAction((PlayerAction) new FromCornerTransition(game));
      this.AddAction((PlayerAction) new ToClimbTransition(game));
      this.AddAction((PlayerAction) new ClimbOverLadder(game));
      this.AddAction((PlayerAction) new PivotTombstone(game));
      this.AddAction((PlayerAction) new EnterPipe(game));
      this.AddAction((PlayerAction) new ExitDoor(game));
      this.AddAction((PlayerAction) new LesserWarp(game));
      this.AddAction((PlayerAction) new GateWarp(game));
      this.AddAction((PlayerAction) new SleepWake(game));
      this.AddAction((PlayerAction) new ReadTurnAround(game));
      this.AddAction((PlayerAction) new BellActions(game));
      this.AddAction((PlayerAction) new Crush(game));
      this.AddAction((PlayerAction) new PlayingDrums(game));
      this.AddAction((PlayerAction) new Floating(game));
      this.AddAction((PlayerAction) new Standing(game));
      ServiceHelper.AddComponent((IGameComponent) new PlayerActions.ActionsManager(game, this));
    }

    private void AddAction(PlayerAction action)
    {
      ServiceHelper.AddComponent((IGameComponent) action);
      if (action.UpdateOrder != 0 || action.IsUpdateOverridden || !(action.GetType().Name != "Jump"))
        return;
      this.LightActions.Add(action);
      action.Enabled = false;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.CameraManager.ViewpointChanged += (Action) (() => this.LevelManager.ScreenInvalidated += (Action) (() =>
      {
        this.PhysicsManager.DetermineOverlaps((IComplexPhysicsEntity) this.PlayerManager);
        if (FezMath.IsOrthographic(this.CameraManager.Viewpoint) && this.CameraManager.LastViewpoint != this.CameraManager.Viewpoint && !this.PlayerManager.HandlesZClamping)
        {
          this.CorrectWallOverlap(true);
          this.PhysicsManager.DetermineInBackground((IPhysicsEntity) this.PlayerManager, !this.PlayerManager.IsOnRotato, true, !this.PlayerManager.Climbing && !this.LevelManager.LowPass);
        }
        this.PhysicsManager.DetermineOverlaps((IComplexPhysicsEntity) this.PlayerManager);
      }));
      this.LevelManager.LevelChanged += (Action) (() => this.LevelManager.ScreenInvalidated += (Action) (() => this.PhysicsManager.HugWalls((IPhysicsEntity) this.PlayerManager, false, false, !this.PlayerManager.Climbing)));
      foreach (SurfaceType key in Util.GetValues<SurfaceType>())
        this.SurfaceHits.Add(key, Enumerable.ToArray<SoundEffect>(Enumerable.Select<string, SoundEffect>(this.CMProvider.GetAllIn("Sounds/Gomez\\Footsteps\\" + (object) key), (Func<string, SoundEffect>) (f => this.CMProvider.Global.Load<SoundEffect>(f)))));
      this.LeftStep = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez\\Footsteps\\Left");
      this.RightStep = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez\\Footsteps\\Right");
      this.ScriptingManager.CutsceneSkipped += (Action) (() =>
      {
        while (!this.PlayerManager.CanControl)
          this.PlayerManager.CanControl = true;
      });
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.PlayerManager.Hidden || this.GameState.InCutscene)
        return;
      Vector3 position = this.PlayerManager.Position;
      if (!this.PlayerManager.CanControl)
      {
        this.InputManager.SaveState();
        this.InputManager.Reset();
      }
      if (this.CameraManager.Viewpoint != Viewpoint.Perspective && this.CameraManager.ActionRunning && (!this.GameState.InMenuCube && !this.GameState.Paused) && (this.CameraManager.RequestedViewpoint == Viewpoint.None && !this.GameState.InMap && !this.LevelManager.IsInvalidatingScreen))
      {
        if (ActionTypeExtensions.AllowsLookingDirectionChange(this.PlayerManager.Action) && !FezMath.AlmostEqual(this.InputManager.Movement.X, 0.0f))
        {
          this.oldLookDir = this.PlayerManager.LookingDirection;
          this.PlayerManager.LookingDirection = FezMath.DirectionFromMovement(this.InputManager.Movement.X);
        }
        Vector3 velocity = this.PlayerManager.Velocity;
        this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
        if (this.PlayerManager.Grounded && this.PlayerManager.Ground.NearLow == null)
        {
          TrileInstance trileInstance = this.PlayerManager.Ground.FarHigh;
          Vector3 b = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
          float num = FezMath.Dot(trileInstance.Center - trileInstance.TransformedSize / 2f * b - this.PlayerManager.Center + this.PlayerManager.Size / 2f * b, b);
          if ((double) num > -0.25)
          {
            this.PlayerManager.Position -= Vector3.UnitY * 0.01f * (float) Math.Sign(this.CollisionManager.GravityFactor);
            if (trileInstance.GetRotatedFace(FezMath.VisibleOrientation(this.CameraManager.Viewpoint)) == CollisionType.AllSides)
            {
              this.PlayerManager.Position += num * b;
              this.PlayerManager.Velocity = velocity * Vector3.UnitY;
            }
            else
              this.PlayerManager.Velocity = velocity;
            this.PlayerManager.Ground = new MultipleHits<TrileInstance>();
          }
        }
        this.PlayerManager.RecordRespawnInformation();
        if (!ActionTypeExtensions.HandlesZClamping(this.PlayerManager.Action) && (this.oldLookDir != this.PlayerManager.LookingDirection || this.PlayerManager.LastAction == ActionType.RunTurnAround) && (this.PlayerManager.Action != ActionType.Dropping && this.PlayerManager.Action != ActionType.GrabCornerLedge && (this.PlayerManager.Action != ActionType.SuckedIn && this.PlayerManager.Action != ActionType.CrushVertical)) && this.PlayerManager.Action != ActionType.CrushHorizontal)
          this.CorrectWallOverlap(false);
      }
      if (this.PlayerManager.CarriedInstance != null && this.PlayerManager.Action != ActionType.Suffering)
      {
        this.PlayerManager.CarriedInstance.Position += this.PlayerManager.Position - position;
        this.LevelManager.UpdateInstance(this.PlayerManager.CarriedInstance);
      }
      if (this.PlayerManager.Grounded)
        this.PlayerManager.IgnoreFreefall = false;
      if (this.PlayerManager.Animation != null && this.lastFrame != this.PlayerManager.Animation.Timing.Frame)
      {
        if (this.PlayerManager.Grounded)
        {
          SurfaceType surfaceType = this.PlayerManager.Ground.First.Trile.SurfaceType;
          if (this.PlayerManager.Action == ActionType.Landing && this.PlayerManager.Animation.Timing.Frame == 0)
            this.PlaySurfaceHit(surfaceType, false);
          else if ((this.PlayerManager.Action == ActionType.PullUpBack || this.PlayerManager.Action == ActionType.PullUpFront || this.PlayerManager.Action == ActionType.PullUpCornerLedge) && this.PlayerManager.Animation.Timing.Frame == 5)
            this.PlaySurfaceHit(surfaceType, false);
          else if (ActionTypeExtensions.GetAnimationPath(this.PlayerManager.Action) == "Walk")
          {
            if (this.PlayerManager.Animation.Timing.Frame == 1 || this.PlayerManager.Animation.Timing.Frame == 4)
            {
              if (this.PlayerManager.Action != ActionType.Sliding)
              {
                SoundEffectExtensions.EmitAt(this.isLeft ? this.LeftStep : this.RightStep, this.PlayerManager.Position, RandomHelper.Between(-0.100000001490116, 0.100000001490116), RandomHelper.Between(0.899999976158142, 1.0));
                this.isLeft = !this.isLeft;
              }
              this.PlaySurfaceHit(surfaceType, false);
            }
          }
          else if (this.PlayerManager.Action == ActionType.Running)
          {
            if (this.PlayerManager.Animation.Timing.Frame == 0 || this.PlayerManager.Animation.Timing.Frame == 3)
              this.PlaySurfaceHit(surfaceType, true);
          }
          else if (this.PlayerManager.CarriedInstance != null)
          {
            if (ActionTypeExtensions.GetAnimationPath(this.PlayerManager.Action) == "CarryHeavyWalk")
            {
              if (this.PlayerManager.Animation.Timing.Frame == 0 || this.PlayerManager.Animation.Timing.Frame == 4)
                this.PlaySurfaceHit(surfaceType, true);
            }
            else if (ActionTypeExtensions.GetAnimationPath(this.PlayerManager.Action) == "CarryWalk" && (this.PlayerManager.Animation.Timing.Frame == 3 || this.PlayerManager.Animation.Timing.Frame == 7))
              this.PlaySurfaceHit(surfaceType, true);
          }
          else
            this.isLeft = false;
        }
        else
          this.isLeft = false;
        this.lastFrame = this.PlayerManager.Animation.Timing.Frame;
      }
      if (this.PlayerManager.CanControl)
        return;
      this.InputManager.RecoverState();
    }

    private void PlaySurfaceHit(SurfaceType surfaceType, bool withStep)
    {
      if (withStep)
      {
        SoundEffectExtensions.EmitAt(this.isLeft ? this.LeftStep : this.RightStep, this.PlayerManager.Position, RandomHelper.Between(-0.100000001490116, 0.100000001490116), RandomHelper.Between(0.899999976158142, 1.0));
        this.isLeft = !this.isLeft;
      }
      SoundEffectExtensions.EmitAt(RandomHelper.InList<SoundEffect>(this.SurfaceHits[surfaceType]), this.PlayerManager.Position, RandomHelper.Between(-0.100000001490116, 0.100000001490116), RandomHelper.Between(0.899999976158142, 1.0));
    }

    private void CorrectWallOverlap(bool overcompensate)
    {
      foreach (PointCollision pointCollision in this.PlayerManager.CornerCollision)
      {
        TrileInstance trileInstance = pointCollision.Instances.Deep;
        if (trileInstance != null && trileInstance != this.PlayerManager.CarriedInstance && trileInstance.GetRotatedFace(this.CameraManager.VisibleOrientation) == CollisionType.AllSides)
        {
          Vector3 vector = (pointCollision.Point - trileInstance.Center + FezMath.Sign(this.PlayerManager.Position - pointCollision.Point) * trileInstance.TransformedSize / 2f) * FezMath.SideMask(this.CameraManager.Viewpoint);
          this.PlayerManager.Position -= vector;
          this.PlayerManager.Position -= FezMath.Sign(vector) * (1.0 / 1000.0) * 2f;
          if (!(FezMath.Sign(this.PlayerManager.Velocity) == FezMath.Sign(vector)))
            break;
          Vector3 vector3_1 = FezMath.Abs(FezMath.Sign(vector));
          this.PlayerManager.Position -= this.PlayerManager.Velocity * vector3_1;
          IPlayerManager playerManager = this.PlayerManager;
          Vector3 vector3_2 = playerManager.Velocity * (Vector3.One - vector3_1);
          playerManager.Velocity = vector3_2;
          break;
        }
      }
    }

    private class ActionsManager : GameComponent
    {
      private readonly PlayerActions Host;

      [ServiceDependency]
      public IGameCameraManager CameraManager { private get; set; }

      [ServiceDependency]
      public IGameStateManager GameState { private get; set; }

      [ServiceDependency]
      public IPlayerManager PlayerManager { private get; set; }

      [ServiceDependency]
      public IInputManager InputManager { private get; set; }

      public ActionsManager(Game game, PlayerActions host)
        : base(game)
      {
        this.Host = host;
      }

      public override void Update(GameTime gameTime)
      {
        if (this.GameState.Paused || this.GameState.Loading || (this.GameState.InCutscene || this.GameState.InMap) || (this.GameState.InFpsMode || this.GameState.InMenuCube))
          return;
        if (!this.PlayerManager.CanControl)
        {
          this.InputManager.SaveState();
          this.InputManager.Reset();
        }
        bool actionNotRunning = !this.CameraManager.ActionRunning;
        foreach (PlayerAction playerAction in this.Host.LightActions)
          playerAction.LightUpdate(gameTime, actionNotRunning);
        if (this.PlayerManager.CanControl)
          return;
        this.InputManager.RecoverState();
      }
    }
  }
}
