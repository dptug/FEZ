// Type: FezGame.Components.PickupsHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FezGame.Components
{
  public class PickupsHost : DrawableGameComponent
  {
    private readonly ManualResetEvent initLock = new ManualResetEvent(false);
    private const float SubmergedPortion = 0.8125f;
    private SoundEffect vaseBreakSound;
    private SoundEffect thudSound;
    private AnimatedTexture largeDust;
    private AnimatedTexture smallDust;
    private List<PickupState> PickupStates;
    private float sinceLevelChanged;

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPhysicsManager PhysicsManager { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public ITrixelParticleSystems ParticleSystemManager { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    [ServiceDependency]
    public IPlaneParticleSystems PlaneParticleSystems { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency(Optional = true)]
    public IDebuggingBag DebuggingBag { private get; set; }

    public PickupsHost(Game game)
      : base(game)
    {
      this.UpdateOrder = -1;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.InitializePickups);
      this.InitializePickups();
      this.CameraManager.ViewpointChanged += (Action) (() =>
      {
        if (this.GameState.Loading || !FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.CameraManager.LastViewpoint == this.CameraManager.Viewpoint)
          return;
        this.PauseGroupOverlaps(false);
        this.LevelManager.ScreenInvalidated += new Action(this.DetectBackground);
      });
      this.CollisionManager.GravityChanged += (Action) (() =>
      {
        foreach (PickupState item_0 in this.PickupStates)
          item_0.Instance.PhysicsState.Ground = new MultipleHits<TrileInstance>();
      });
    }

    private void DetectBackground()
    {
      if (this.LevelManager.Name != "LAVA")
      {
        foreach (TrileGroup trileGroup in (IEnumerable<TrileGroup>) this.LevelManager.PickupGroups.Values)
        {
          foreach (TrileInstance trileInstance in trileGroup.Triles)
            trileInstance.PhysicsState.UpdatingPhysics = true;
          foreach (TrileInstance trileInstance in trileGroup.Triles)
          {
            if (!trileInstance.PhysicsState.IgnoreCollision)
              this.PhysicsManager.DetermineInBackground((IPhysicsEntity) trileInstance.PhysicsState, true, true, false);
          }
          foreach (TrileInstance trileInstance in trileGroup.Triles)
            trileInstance.PhysicsState.UpdatingPhysics = false;
        }
      }
      if (this.PickupStates == null)
        return;
      foreach (PickupState pickupState in this.PickupStates)
      {
        if (pickupState.Group == null && pickupState.Instance.PhysicsState != null)
          this.PhysicsManager.DetermineInBackground((IPhysicsEntity) pickupState.Instance.PhysicsState, true, true, false);
      }
    }

    private void PauseGroupOverlaps(bool force)
    {
      if (!force && this.GameState.Loading || (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.LevelManager.PickupGroups.Count == 0))
        return;
      Vector3 b1 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      Vector3 b2 = FezMath.SideMask(this.CameraManager.Viewpoint);
      Vector3 vector3 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      foreach (TrileGroup trileGroup in Enumerable.Distinct<TrileGroup>((IEnumerable<TrileGroup>) this.LevelManager.PickupGroups.Values))
      {
        float num = float.MaxValue;
        float? nullable = new float?();
        foreach (TrileInstance trileInstance in trileGroup.Triles)
        {
          num = Math.Min(num, FezMath.Dot(trileInstance.Center, b1));
          if (!trileInstance.PhysicsState.Puppet)
            nullable = new float?(FezMath.Dot(trileInstance.Center, b2));
        }
        foreach (PickupState pickupState1 in this.PickupStates)
        {
          if (pickupState1.Group == trileGroup)
          {
            TrileInstance trileInstance = pickupState1.Instance;
            bool flag = !FezMath.AlmostEqual(FezMath.Dot(trileInstance.Center, b1), num);
            trileInstance.PhysicsState.Paused = flag;
            if (flag)
            {
              trileInstance.PhysicsState.Puppet = true;
              pickupState1.LastMovement = Vector3.Zero;
            }
            else
            {
              pickupState1.VisibleOverlapper = (PickupState) null;
              foreach (PickupState pickupState2 in this.PickupStates)
              {
                if (FezMath.AlmostEqual(pickupState2.Instance.Center * vector3, pickupState1.Instance.Center * vector3))
                  pickupState2.VisibleOverlapper = pickupState1;
              }
              if (nullable.HasValue && FezMath.AlmostEqual(FezMath.Dot(trileInstance.Center, b2), nullable.Value))
                trileInstance.PhysicsState.Puppet = false;
            }
          }
        }
      }
    }

    protected override void LoadContent()
    {
      this.largeDust = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/dust_large");
      this.smallDust = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/dust_small");
      this.vaseBreakSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/VaseBreak");
      this.thudSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/HitFloor");
    }

    private void InitializePickups()
    {
      this.sinceLevelChanged = 0.0f;
      if (this.LevelManager.TrileSet == null)
      {
        this.initLock.Reset();
        this.PickupStates = (List<PickupState>) null;
        this.initLock.Set();
      }
      else
      {
        this.initLock.Reset();
        if (this.PickupStates != null)
        {
          foreach (PickupState pickupState in this.PickupStates)
            pickupState.Instance.PhysicsState.ShouldRespawn = false;
        }
        this.PickupStates = new List<PickupState>(Enumerable.Select<TrileInstance, PickupState>(Enumerable.SelectMany<Trile, TrileInstance>(Enumerable.Where<Trile>((IEnumerable<Trile>) this.LevelManager.TrileSet.Triles.Values, (Func<Trile, bool>) (t => ActorTypeExtensions.IsPickable(t.ActorSettings.Type))), (Func<Trile, IEnumerable<TrileInstance>>) (t => (IEnumerable<TrileInstance>) t.Instances)), (Func<TrileInstance, PickupState>) (t => new PickupState(t, this.LevelManager.PickupGroups.ContainsKey(t) ? this.LevelManager.PickupGroups[t] : (TrileGroup) null))));
        foreach (PickupState pickupState in Enumerable.Where<PickupState>((IEnumerable<PickupState>) this.PickupStates, (Func<PickupState, bool>) (x => x.Group != null)))
        {
          int groupId = pickupState.Group.Id;
          pickupState.AttachedAOs = Enumerable.ToArray<ArtObjectInstance>(Enumerable.Where<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelMaterializer.LevelArtObjects, (Func<ArtObjectInstance, bool>) (x =>
          {
            int? local_0 = x.ActorSettings.AttachedGroup;
            int local_1 = groupId;
            if (local_0.GetValueOrDefault() == local_1)
              return local_0.HasValue;
            else
              return false;
          })));
          if (pickupState.Group.Triles.Count == 1)
            pickupState.Group = (TrileGroup) null;
        }
        foreach (TrileInstance instance in Enumerable.Select<PickupState, TrileInstance>(Enumerable.Where<PickupState>((IEnumerable<PickupState>) this.PickupStates, (Func<PickupState, bool>) (x => x.Instance.PhysicsState == null)), (Func<PickupState, TrileInstance>) (x => x.Instance)))
          instance.PhysicsState = new InstancePhysicsState(instance)
          {
            Ground = new MultipleHits<TrileInstance>()
            {
              NearLow = this.LevelManager.ActualInstanceAt(instance.Center - instance.Trile.Size.Y * Vector3.UnitY)
            }
          };
        bool flag = this.LevelManager.WaterType == LiquidType.Sewer && !FezMath.In<string>(this.LevelManager.Name, "SEWER_PIVOT", "SEWER_TREASURE_TWO");
        foreach (TrileGroup trileGroup in Enumerable.Distinct<TrileGroup>(Enumerable.Where<TrileGroup>((IEnumerable<TrileGroup>) this.LevelManager.PickupGroups.Values, (Func<TrileGroup, bool>) (g => Enumerable.All<TrileInstance>((IEnumerable<TrileInstance>) g.Triles, (Func<TrileInstance, bool>) (t => !t.PhysicsState.Puppet))))))
        {
          foreach (TrileInstance instance in trileGroup.Triles)
          {
            instance.PhysicsState.IgnoreCollision = flag;
            instance.PhysicsState.Center += 1.0 / 500.0 * FezMath.XZMask;
            instance.PhysicsState.UpdateInstance();
            this.LevelManager.UpdateInstance(instance);
            instance.PhysicsState.Puppet = true;
          }
          trileGroup.Triles[trileGroup.Triles.Count / 2].PhysicsState.Puppet = false;
          trileGroup.InMidAir = true;
        }
        this.PauseGroupOverlaps(true);
        this.DetectBackground();
        this.initLock.Set();
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.CameraManager.Viewpoint == Viewpoint.Perspective || !this.CameraManager.ActionRunning || (this.GameState.Paused || this.GameState.InMap) || (this.GameState.Loading || this.PickupStates == null || this.PickupStates.Count == 0))
        return;
      this.sinceLevelChanged += (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.initLock.WaitOne();
      for (int index = this.PickupStates.Count - 1; index >= 0; --index)
      {
        if (this.PickupStates[index].Instance.PhysicsState == null)
          this.PickupStates.RemoveAt(index);
      }
      foreach (PickupState pickupState in this.PickupStates)
      {
        if (pickupState.Instance.PhysicsState.StaticGrounds)
          pickupState.Instance.PhysicsState.GroundMovement = Vector3.Zero;
      }
      this.PickupStates.Sort((IComparer<PickupState>) MovingGroundsPickupComparer.Default);
      this.UpdatePickups((float) gameTime.ElapsedGameTime.TotalSeconds);
      foreach (TrileGroup trileGroup1 in (IEnumerable<TrileGroup>) this.LevelManager.PickupGroups.Values)
      {
        if (trileGroup1.InMidAir)
        {
          foreach (TrileInstance trileInstance in trileGroup1.Triles)
          {
            if (!trileInstance.PhysicsState.Paused && trileInstance.PhysicsState.Grounded)
            {
              trileGroup1.InMidAir = false;
              if (trileInstance.PhysicsState.Puppet)
              {
                trileInstance.PhysicsState.Puppet = false;
                using (List<TrileInstance>.Enumerator enumerator = trileGroup1.Triles.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    TrileInstance current = enumerator.Current;
                    if (current != trileInstance)
                      current.PhysicsState.Puppet = true;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
        }
        else
        {
          trileGroup1.InMidAir = true;
          foreach (TrileInstance trileInstance in trileGroup1.Triles)
          {
            TrileGroup trileGroup2 = trileGroup1;
            int num = trileGroup2.InMidAir & !trileInstance.PhysicsState.Grounded ? 1 : 0;
            trileGroup2.InMidAir = num != 0;
          }
        }
      }
      foreach (PickupState pickupState1 in this.PickupStates)
      {
        if (pickupState1.Group != null && !pickupState1.Instance.PhysicsState.Puppet)
        {
          PickupState pickupState2 = pickupState1;
          foreach (PickupState pickupState3 in this.PickupStates)
          {
            if (pickupState3.Group == pickupState2.Group && pickupState3 != pickupState2)
            {
              pickupState3.Instance.PhysicsState.Center += pickupState2.LastMovement - pickupState3.LastMovement;
              pickupState3.Instance.PhysicsState.Background = pickupState2.Instance.PhysicsState.Background;
              pickupState3.Instance.PhysicsState.Velocity = pickupState2.Instance.PhysicsState.Velocity;
              pickupState3.Instance.PhysicsState.UpdateInstance();
              this.LevelManager.UpdateInstance(pickupState3.Instance);
              pickupState3.LastMovement = Vector3.Zero;
              pickupState3.FloatMalus = pickupState2.FloatMalus;
              pickupState3.FloatSeed = pickupState2.FloatSeed;
            }
          }
        }
        if (pickupState1.VisibleOverlapper != null)
        {
          PickupState pickupState2 = pickupState1.VisibleOverlapper;
          InstancePhysicsState physicsState = pickupState1.Instance.PhysicsState;
          physicsState.Background = pickupState2.Instance.PhysicsState.Background;
          physicsState.Ground = pickupState2.Instance.PhysicsState.Ground;
          physicsState.Floating = pickupState2.Instance.PhysicsState.Floating;
          Array.Copy((Array) physicsState.CornerCollision, (Array) pickupState1.Instance.PhysicsState.CornerCollision, 4);
          physicsState.GroundMovement = pickupState2.Instance.PhysicsState.GroundMovement;
          physicsState.Sticky = pickupState2.Instance.PhysicsState.Sticky;
          physicsState.WallCollision = pickupState2.Instance.PhysicsState.WallCollision;
          physicsState.PushedDownBy = pickupState2.Instance.PhysicsState.PushedDownBy;
          pickupState1.LastGroundedCenter = pickupState2.LastGroundedCenter;
          pickupState1.LastVelocity = pickupState2.LastVelocity;
          pickupState1.TouchesWater = pickupState2.TouchesWater;
        }
      }
      foreach (PickupState pickupState in this.PickupStates)
      {
        if (pickupState.Instance.PhysicsState != null && (pickupState.Instance.PhysicsState.Grounded || this.PlayerManager.CarriedInstance == pickupState.Instance || pickupState.Instance.PhysicsState.Floating))
        {
          pickupState.FlightApex = pickupState.Instance.Center.Y;
          pickupState.LastGroundedCenter = pickupState.Instance.Center;
        }
      }
      this.initLock.Set();
    }

    private void UpdatePickups(float elapsedSeconds)
    {
      Vector3 vector3 = Vector3.UnitY * this.CameraManager.Radius / this.CameraManager.AspectRatio;
      foreach (PickupState pickupState in this.PickupStates)
      {
        TrileInstance trileInstance = pickupState.Instance;
        InstancePhysicsState physicsState = trileInstance.PhysicsState;
        ActorType type = trileInstance.Trile.ActorSettings.Type;
        if (!physicsState.Paused && (physicsState.ShouldRespawn || trileInstance.Enabled && trileInstance != this.PlayerManager.CarriedInstance && (!physicsState.Static || pickupState.TouchesWater)))
        {
          this.TryFloat(pickupState, elapsedSeconds);
          if (!physicsState.Vanished && (!pickupState.TouchesWater || !ActorTypeExtensions.IsBuoyant(type)))
            physicsState.Velocity += (float) (3.15000009536743 * (double) this.CollisionManager.GravityFactor * 0.150000005960464) * elapsedSeconds * Vector3.Down;
          bool grounded = trileInstance.PhysicsState.Grounded;
          Vector3 center = physicsState.Center;
          this.PhysicsManager.Update((ISimplePhysicsEntity) physicsState, false, pickupState.Group == null && (!physicsState.Floating || !FezMath.AlmostEqual(physicsState.Velocity.X, 0.0f) || !FezMath.AlmostEqual(physicsState.Velocity.Z, 0.0f)));
          pickupState.LastMovement = physicsState.Center - center;
          if (physicsState.NoVelocityClamping)
          {
            physicsState.NoVelocityClamping = false;
            physicsState.Velocity = Vector3.Zero;
          }
          if (pickupState.AttachedAOs != null)
          {
            foreach (ArtObjectInstance artObjectInstance in pickupState.AttachedAOs)
              artObjectInstance.Position += pickupState.LastMovement;
          }
          if (((double) pickupState.LastGroundedCenter.Y - (double) trileInstance.Position.Y) * (double) Math.Sign(this.CollisionManager.GravityFactor) > (double) vector3.Y)
            physicsState.Vanished = true;
          else if (this.LevelManager.Loops)
          {
            while ((double) trileInstance.Position.Y < 0.0)
              trileInstance.Position += this.LevelManager.Size * Vector3.UnitY;
            while ((double) trileInstance.Position.Y > (double) this.LevelManager.Size.Y)
              trileInstance.Position -= this.LevelManager.Size * Vector3.UnitY;
          }
          if (physicsState.Floating && physicsState.Grounded && !physicsState.PushedUp)
            physicsState.Floating = pickupState.TouchesWater = (double) trileInstance.Position.Y <= (double) this.LevelManager.WaterHeight - 13.0 / 16.0 + (double) pickupState.FloatMalus;
          physicsState.ForceNonStatic = false;
          if (ActorTypeExtensions.IsFragile(type))
          {
            if (!trileInstance.PhysicsState.Grounded)
              pickupState.FlightApex = Math.Max(pickupState.FlightApex, trileInstance.Center.Y);
            else if (!trileInstance.PhysicsState.Respawned && (double) pickupState.FlightApex - (double) trileInstance.Center.Y > (double) PickupsHost.BreakHeight(type))
            {
              this.PlayBreakSound(type, trileInstance.Position);
              trileInstance.PhysicsState.Vanished = true;
              this.ParticleSystemManager.Add(new TrixelParticleSystem(this.Game, new TrixelParticleSystem.Settings()
              {
                ExplodingInstance = trileInstance,
                EnergySource = new Vector3?(trileInstance.Center - Vector3.Normalize(pickupState.LastVelocity) * trileInstance.TransformedSize / 2f),
                ParticleCount = 30,
                MinimumSize = 1,
                MaximumSize = 8,
                GravityModifier = 1f,
                Energy = 0.25f,
                BaseVelocity = pickupState.LastVelocity * 0.5f
              }));
              this.LevelMaterializer.CullInstanceOut(trileInstance);
              if (type == ActorType.Vase)
              {
                trileInstance.PhysicsState = (InstancePhysicsState) null;
                this.LevelManager.ClearTrile(trileInstance);
                break;
              }
              else
              {
                trileInstance.Enabled = false;
                trileInstance.PhysicsState.ShouldRespawn = true;
              }
            }
          }
          this.TryPushHorizontalStack(pickupState, elapsedSeconds);
          if (physicsState.Static)
          {
            pickupState.LastMovement = pickupState.LastVelocity = physicsState.Velocity = Vector3.Zero;
            physicsState.Respawned = false;
          }
          if (physicsState.Vanished)
            physicsState.ShouldRespawn = true;
          if (physicsState.ShouldRespawn && this.PlayerManager.Action != ActionType.FreeFalling)
          {
            physicsState.Center = pickupState.OriginalCenter + new Vector3(1.0 / 1000.0);
            physicsState.UpdateInstance();
            physicsState.Velocity = Vector3.Zero;
            physicsState.ShouldRespawn = false;
            pickupState.LastVelocity = Vector3.Zero;
            pickupState.TouchesWater = false;
            physicsState.Floating = false;
            physicsState.PushedDownBy = (TrileInstance) null;
            trileInstance.Enabled = false;
            trileInstance.Hidden = true;
            physicsState.Ground = new MultipleHits<TrileInstance>()
            {
              NearLow = this.LevelManager.ActualInstanceAt(physicsState.Center - trileInstance.Trile.Size.Y * Vector3.UnitY)
            };
            ServiceHelper.AddComponent((IGameComponent) new GlitchyRespawner(ServiceHelper.Game, trileInstance));
          }
          physicsState.UpdateInstance();
          this.LevelManager.UpdateInstance(trileInstance);
          if (!grounded && trileInstance.PhysicsState.Grounded && (double) Math.Abs(pickupState.LastVelocity.Y) > 0.0500000007450581)
          {
            float num1 = FezMath.Dot(pickupState.LastVelocity, FezMath.RightVector(this.CameraManager.Viewpoint));
            float val1 = FezMath.Saturate(pickupState.LastVelocity.Y / (-0.2f * (float) Math.Sign(this.CollisionManager.GravityFactor)));
            AnimatedTexture animation;
            if (ActorTypeExtensions.IsHeavy(trileInstance.Trile.ActorSettings.Type))
            {
              if ((double) val1 > 0.5)
              {
                animation = this.largeDust;
              }
              else
              {
                animation = this.smallDust;
                val1 *= 2f;
              }
            }
            else
              animation = this.smallDust;
            float num2 = Math.Max(val1, 0.4f);
            this.SpawnDust(trileInstance, num2, animation, (double) num1 >= 0.0, (double) num1 <= 0.0);
            if (animation == this.largeDust && (double) num1 != 0.0)
              this.SpawnDust(trileInstance, num2, this.smallDust, (double) num1 < 0.0, (double) num1 > 0.0);
            SoundEffectExtensions.EmitAt(this.thudSound, trileInstance.Position, (float) ((double) num2 * -0.600000023841858 + 0.300000011920929), num2);
          }
          if (physicsState.Grounded && physicsState.Ground.First.PhysicsState != null)
            physicsState.Ground.First.PhysicsState.PushedDownBy = trileInstance;
          pickupState.LastVelocity = trileInstance.PhysicsState.Velocity;
        }
      }
    }

    private void TryFloat(PickupState pickup, float elapsedSeconds)
    {
      // ISSUE: unable to decompile the method.
    }

    private void TryPushHorizontalStack(PickupState state, float elapsedSeconds)
    {
      TrileInstance trileInstance1 = state.Instance;
      TrileGroup trileGroup = state.Group;
      if (!BoxCollisionResultExtensions.AnyCollided(trileInstance1.PhysicsState.WallCollision))
        return;
      Vector3 vector3_1 = -FezMath.Sign(trileInstance1.PhysicsState.WallCollision.First.Response);
      trileInstance1.PhysicsState.Velocity = Vector3.Zero;
      TrileInstance instance = trileInstance1;
      while (instance != null && BoxCollisionResultExtensions.AnyCollided(instance.PhysicsState.WallCollision))
      {
        MultipleHits<CollisionResult> wallCollision = instance.PhysicsState.WallCollision;
        TrileInstance trileInstance2 = wallCollision.First.Destination;
        if (trileInstance2.PhysicsState != null && ActorTypeExtensions.IsPickable(trileInstance2.Trile.ActorSettings.Type) && (trileGroup == null || !trileGroup.Triles.Contains(trileInstance2)))
        {
          Vector3 vector = -wallCollision.First.Response;
          if (FezMath.Sign(vector) != vector3_1 || vector == Vector3.Zero)
          {
            instance = (TrileInstance) null;
          }
          else
          {
            instance = trileInstance2;
            Vector3 velocity = instance.PhysicsState.Velocity;
            instance.PhysicsState.Velocity = vector;
            Vector3 center = instance.PhysicsState.Center;
            if (instance.PhysicsState.Grounded)
              instance.PhysicsState.Velocity += (float) (3.15000009536743 * (double) Math.Sign(this.CollisionManager.GravityFactor) * 0.150000005960464) * elapsedSeconds * Vector3.Down;
            this.PhysicsManager.Update((ISimplePhysicsEntity) instance.PhysicsState, false, false);
            if (trileInstance1.PhysicsState.Grounded)
              instance.PhysicsState.Velocity = velocity;
            instance.PhysicsState.UpdateInstance();
            this.LevelManager.UpdateInstance(instance);
            foreach (PickupState pickupState in this.PickupStates)
            {
              if (pickupState.Instance.PhysicsState.Ground.NearLow == instance || pickupState.Instance.PhysicsState.Ground.FarHigh == instance)
              {
                Vector3 vector3_2 = (instance.PhysicsState.Center - center) / 0.85f;
                pickupState.Instance.PhysicsState.Velocity = vector3_2;
              }
            }
          }
        }
        else
          instance = (TrileInstance) null;
      }
    }

    private void SpawnDust(TrileInstance instance, float opacity, AnimatedTexture animation, bool onRight, bool onLeft)
    {
      float num1 = (float) ((double) instance.Center.Y - (double) instance.TransformedSize.Y / 2.0 * (double) Math.Sign(this.CollisionManager.GravityFactor) + (double) animation.FrameHeight / 32.0 * (double) Math.Sign(this.CollisionManager.GravityFactor));
      float num2 = (float) ((double) FezMath.Dot(instance.TransformedSize, FezMath.SideMask(this.CameraManager.Viewpoint)) / 2.0 + (double) animation.FrameWidth / 32.0 * 2.0 / 3.0);
      if (ActorTypeExtensions.IsBomb(instance.Trile.ActorSettings.Type))
        num2 -= 0.25f;
      opacity = 1f;
      Vector3 vector3_1 = FezMath.RightVector(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      bool b = (double) this.CollisionManager.GravityFactor < 0.0;
      if (onRight)
      {
        BackgroundPlane backgroundPlane;
        this.LevelManager.AddPlane(backgroundPlane = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, animation)
        {
          OriginalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float) FezMath.AsNumeric(b) * 3.141593f),
          Doublesided = true,
          Loop = false,
          Opacity = opacity,
          Timing = {
            Step = 0.0f
          }
        });
        backgroundPlane.Position = instance.Center * FezMath.XZMask + vector3_1 * num2 + num1 * Vector3.UnitY - vector3_2;
        backgroundPlane.Billboard = true;
      }
      if (!onLeft)
        return;
      BackgroundPlane backgroundPlane1;
      this.LevelManager.AddPlane(backgroundPlane1 = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, animation)
      {
        OriginalRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f) * Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float) FezMath.AsNumeric(b) * 3.141593f),
        Doublesided = true,
        Loop = false,
        Opacity = opacity,
        Timing = {
          Step = 0.0f
        }
      });
      backgroundPlane1.Position = instance.Center * FezMath.XZMask - vector3_1 * num2 + num1 * Vector3.UnitY - vector3_2;
      backgroundPlane1.Billboard = true;
    }

    private void DetermineFloatMalus(PickupState pickup)
    {
      TrileInstance trileInstance1 = pickup.Instance;
      int num = 0;
      Vector3 vector3 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      TrileInstance trileInstance2 = trileInstance1;
      Vector3 b = trileInstance1.Center * vector3;
      do
      {
        TrileInstance trileInstance3 = this.PlayerManager.Ground.NearLow;
        TrileInstance trileInstance4 = this.PlayerManager.Ground.FarHigh;
        TrileInstance heldInstance = this.PlayerManager.HeldInstance;
        if (trileInstance3 == trileInstance2 || trileInstance3 != null && FezMath.AlmostEqual(trileInstance3.Center * vector3, b) || (trileInstance4 == trileInstance2 || trileInstance4 != null && FezMath.AlmostEqual(trileInstance4.Center * vector3, b)) || (heldInstance == trileInstance2 || heldInstance != null && FezMath.AlmostEqual(heldInstance.Center * vector3, b)))
          ++num;
        if (trileInstance1.PhysicsState.PushedDownBy != null)
        {
          ++num;
          trileInstance2 = trileInstance2.PhysicsState.PushedDownBy;
        }
        else
          trileInstance2 = (TrileInstance) null;
      }
      while (trileInstance2 != null);
      pickup.FloatMalus = MathHelper.Lerp(pickup.FloatMalus, -0.25f * (float) num, 0.1f);
      if (num == 0 || pickup.Group == null)
        return;
      foreach (TrileInstance trileInstance3 in pickup.Group.Triles)
        trileInstance3.PhysicsState.Puppet = true;
      pickup.Instance.PhysicsState.Puppet = false;
    }

    private static float BreakHeight(ActorType type)
    {
      switch (type)
      {
        case ActorType.TntPickup:
        case ActorType.SinkPickup:
        case ActorType.PickUp:
          return 7f;
        case ActorType.Vase:
          return 1f;
        default:
          throw new InvalidOperationException();
      }
    }

    private void PlayBreakSound(ActorType type, Vector3 position)
    {
      switch (type)
      {
        case ActorType.Vase:
          SoundEffectExtensions.EmitAt(this.vaseBreakSound, position);
          break;
      }
    }
  }
}
