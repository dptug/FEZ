// Type: FezGame.Components.BombsHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  internal class BombsHost : DrawableGameComponent
  {
    private static readonly Point[] SmallBombOffsets = new Point[9]
    {
      new Point(0, 0),
      new Point(1, 0),
      new Point(-1, 0),
      new Point(0, 1),
      new Point(0, -1),
      new Point(1, 1),
      new Point(1, -1),
      new Point(-1, 1),
      new Point(-1, -1)
    };
    private static readonly Point[] BigBombOffsets = new Point[25]
    {
      new Point(0, 0),
      new Point(1, 0),
      new Point(2, 0),
      new Point(-1, 0),
      new Point(-2, 0),
      new Point(0, 1),
      new Point(0, -1),
      new Point(0, 2),
      new Point(0, -2),
      new Point(-2, -2),
      new Point(-1, -2),
      new Point(-2, -1),
      new Point(-1, -1),
      new Point(2, -2),
      new Point(1, -2),
      new Point(2, -1),
      new Point(1, -1),
      new Point(-2, 2),
      new Point(-1, 2),
      new Point(-2, 1),
      new Point(-1, 1),
      new Point(2, 2),
      new Point(1, 2),
      new Point(2, 1),
      new Point(1, 1)
    };
    private static readonly Vector3[] CornerNeighbors = new Vector3[5]
    {
      new Vector3(0.499f, 1f, 0.499f),
      new Vector3(0.499f, 1f, -0.499f),
      new Vector3(-0.499f, 1f, 0.499f),
      new Vector3(-0.499f, 1f, -0.499f),
      new Vector3(0.0f, 1f, 0.0f)
    };
    private readonly Color FlashColor = new Color((int) byte.MaxValue, 0, 0, 128);
    private readonly TimeSpan FlashTime = TimeSpan.FromSeconds(4.0);
    private readonly TimeSpan ExplodeStart = TimeSpan.FromSeconds(6.0);
    private readonly TimeSpan ChainsplodeDelay = TimeSpan.FromSeconds(0.25);
    private readonly Dictionary<TrileInstance, BombsHost.BombState> bombStates = new Dictionary<TrileInstance, BombsHost.BombState>();
    private readonly List<BombsHost.DestructibleGroup> destructibleGroups = new List<BombsHost.DestructibleGroup>();
    private readonly Dictionary<TrileInstance, BombsHost.DestructibleGroup> indexedDg = new Dictionary<TrileInstance, BombsHost.DestructibleGroup>();
    private readonly List<TrileInstance> bsToRemove = new List<TrileInstance>();
    private readonly List<KeyValuePair<TrileInstance, BombsHost.BombState>> bsToAdd = new List<KeyValuePair<TrileInstance, BombsHost.BombState>>();
    private const float H = 0.499f;
    private AnimatedTexture bombAnimation;
    private AnimatedTexture bigBombAnimation;
    private AnimatedTexture tntAnimation;
    private Texture2D flare;
    private Mesh flashesMesh;
    private SoundEffect explodeSound;
    private SoundEffect crystalsplodeSound;
    private SoundEffect countdownSound;

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public ITrixelParticleSystems TrixelParticleSystems { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ITrixelParticleSystems ParticleSystemManager { private get; set; }

    static BombsHost()
    {
    }

    public BombsHost(Game game)
      : base(game)
    {
      this.DrawOrder = 10;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
    }

    private void TryInitialize()
    {
      this.flashesMesh.ClearGroups();
      this.bombStates.Clear();
      this.indexedDg.Clear();
      this.destructibleGroups.Clear();
      foreach (TrileGroup trileGroup in (IEnumerable<TrileGroup>) this.LevelManager.Groups.Values)
      {
        if (trileGroup.Triles.Count != 0 && ActorTypeExtensions.IsDestructible(trileGroup.Triles[0].Trile.ActorSettings.Type) && ActorTypeExtensions.IsDestructible(trileGroup.Triles[trileGroup.Triles.Count - 1].Trile.ActorSettings.Type))
        {
          BombsHost.DestructibleGroup destructibleGroup = new BombsHost.DestructibleGroup()
          {
            AllTriles = new List<TrileInstance>((IEnumerable<TrileInstance>) trileGroup.Triles),
            Group = trileGroup
          };
          this.destructibleGroups.Add(destructibleGroup);
          FaceOrientation face = FaceOrientation.Down;
          foreach (TrileInstance key in trileGroup.Triles)
          {
            this.indexedDg.Add(key, destructibleGroup);
            TrileEmplacement traversal = key.Emplacement.GetTraversal(ref face);
            TrileInstance instance = this.LevelManager.TrileInstanceAt(ref traversal);
            if (instance != null && !ActorTypeExtensions.IsDestructible(instance.Trile.ActorSettings.Type) && instance.PhysicsState == null)
              instance.PhysicsState = new InstancePhysicsState(instance);
          }
        }
      }
    }

    protected override void LoadContent()
    {
      this.explodeSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/BombExplode");
      this.crystalsplodeSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/TntExplode");
      this.countdownSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/BombCountdown");
      this.bombAnimation = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/BombExplosion");
      this.bigBombAnimation = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/BigBombExplosion");
      this.tntAnimation = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/TntExplosion");
      this.flare = this.CMProvider.Global.Load<Texture2D>("Background Planes/Flare");
      this.flashesMesh = new Mesh()
      {
        AlwaysOnTop = true,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        Effect = (BaseEffect) new DefaultEffect.VertexColored()
      };
    }

    public override void Update(GameTime gameTime)
    {
      if (this.CameraManager.Viewpoint == Viewpoint.Perspective || !this.CameraManager.ActionRunning || (this.GameState.Paused || this.GameState.InMap) || (this.CameraManager.RequestedViewpoint != Viewpoint.None || this.GameState.Loading))
        return;
      foreach (BombsHost.DestructibleGroup destructibleGroup1 in this.destructibleGroups)
      {
        if (destructibleGroup1.RespawnIn.HasValue)
        {
          BombsHost.DestructibleGroup destructibleGroup2 = destructibleGroup1;
          float? nullable1 = destructibleGroup2.RespawnIn;
          float num = (float) gameTime.ElapsedGameTime.TotalSeconds;
          float? nullable2 = nullable1.HasValue ? new float?(nullable1.GetValueOrDefault() - num) : new float?();
          destructibleGroup2.RespawnIn = nullable2;
          if ((double) destructibleGroup1.RespawnIn.Value <= 0.0)
          {
            bool flag = true;
            foreach (TrileInstance instance in destructibleGroup1.AllTriles)
            {
              if (!instance.Enabled || instance.Hidden || instance.Removed)
              {
                instance.Enabled = false;
                instance.Hidden = true;
                ServiceHelper.AddComponent((IGameComponent) new GlitchyRespawner(ServiceHelper.Game, instance, flag || RandomHelper.Probability(0.25)));
                flag = false;
              }
            }
            destructibleGroup1.RespawnIn = new float?();
          }
        }
      }
      TrileInstance carriedInstance = this.PlayerManager.CarriedInstance;
      if (carriedInstance != null && ActorTypeExtensions.IsBomb(carriedInstance.Trile.ActorSettings.Type) && !this.bombStates.ContainsKey(carriedInstance))
      {
        carriedInstance.Foreign = carriedInstance.PhysicsState.Respawned = false;
        this.bombStates.Add(carriedInstance, new BombsHost.BombState());
      }
      bool flag1 = false;
      bool flag2 = false;
      foreach (TrileInstance instance in this.bombStates.Keys)
      {
        BombsHost.BombState state = this.bombStates[instance];
        if (!ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action))
          state.SincePickup += gameTime.ElapsedGameTime;
        bool flag3 = instance.Trile.ActorSettings.Type == ActorType.BigBomb;
        bool flag4 = instance.Trile.ActorSettings.Type == ActorType.TntBlock || instance.Trile.ActorSettings.Type == ActorType.TntPickup;
        if (ActorTypeExtensions.IsBomb(instance.Trile.ActorSettings.Type) && instance.Hidden)
        {
          this.bsToRemove.Add(instance);
          if (state.Flash != null)
          {
            this.flashesMesh.RemoveGroup(state.Flash);
            state.Flash = (Group) null;
          }
          if (state.Emitter != null && state.Emitter.Cue != null)
            state.Emitter.Cue.Stop(false);
        }
        else
        {
          if (state.SincePickup > this.FlashTime && state.Explosion == null)
          {
            if (state.Flash == null)
            {
              state.Flash = this.flashesMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, this.FlashColor, true);
              if (ActorTypeExtensions.IsBomb(instance.Trile.ActorSettings.Type) && !state.IsChainsploding)
              {
                state.Emitter = SoundEffectExtensions.EmitAt(this.countdownSound, instance.Center);
                state.Emitter.PauseViewTransitions = true;
              }
            }
            double totalSeconds = state.SincePickup.TotalSeconds;
            if (totalSeconds > this.ExplodeStart.TotalSeconds - 1.0)
              totalSeconds *= 2.0;
            state.Flash.Enabled = FezMath.Frac(totalSeconds) < 0.5;
            if (state.Flash.Enabled)
            {
              state.Flash.Position = instance.Center;
              state.Flash.Rotation = this.CameraManager.Rotation;
            }
          }
          if (state.SincePickup > this.ExplodeStart && state.Explosion == null)
          {
            if (flag4 && !flag1 || !flag4 && !flag2)
            {
              SoundEffectExtensions.EmitAt(flag4 ? this.crystalsplodeSound : this.explodeSound, instance.Center, RandomHelper.Centered(0.025));
              if (flag4)
                flag1 = true;
              else
                flag2 = true;
            }
            if (state.ChainsplodedBy != null && state.ChainsplodedBy.Emitter != null)
              state.ChainsplodedBy.Emitter.FadeOutAndDie(0.0f);
            float num1 = (flag3 ? 0.6f : 0.3f) * FezMath.Saturate((float) (1.0 - (double) (instance.Center - this.PlayerManager.Center).Length() / 15.0));
            if (CamShake.CurrentCamShake == null)
              ServiceHelper.AddComponent((IGameComponent) new CamShake(this.Game)
              {
                Duration = TimeSpan.FromSeconds(0.75),
                Distance = num1
              });
            else
              CamShake.CurrentCamShake.Reset();
            this.ParticleSystemManager.PropagateEnergy(instance.Center, flag3 ? 6f : 3f);
            this.flashesMesh.RemoveGroup(state.Flash);
            state.Flash = (Group) null;
            switch (instance.Trile.ActorSettings.Type)
            {
              case ActorType.BigBomb:
                state.Explosion = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.bigBombAnimation)
                {
                  ActorType = ActorType.Bomb
                };
                break;
              case ActorType.TntBlock:
              case ActorType.TntPickup:
                state.Explosion = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.tntAnimation)
                {
                  ActorType = ActorType.Bomb
                };
                break;
              default:
                state.Explosion = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.bombAnimation)
                {
                  ActorType = ActorType.Bomb
                };
                break;
            }
            state.Explosion.Timing.Loop = false;
            state.Explosion.Billboard = true;
            state.Explosion.Fullbright = true;
            state.Explosion.OriginalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float) RandomHelper.Random.Next(0, 4) * 1.570796f);
            state.Explosion.Timing.Restart();
            this.LevelManager.AddPlane(state.Explosion);
            state.Flare = new BackgroundPlane(this.LevelMaterializer.StaticPlanesMesh, (Texture) this.flare)
            {
              AlwaysOnTop = true,
              LightMap = true,
              AllowOverbrightness = true,
              Billboard = true
            };
            this.LevelManager.AddPlane(state.Flare);
            state.Flare.Scale = Vector3.One * (flag3 ? 3f : 1.5f);
            state.Explosion.Position = state.Flare.Position = instance.Center + (RandomHelper.Centered(1.0 / 1000.0) - 0.5f) * FezMath.ForwardVector(this.CameraManager.Viewpoint);
            float num2 = flag3 ? 3f : 1.5f;
            float num3 = ((this.PlayerManager.Position - instance.Center) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint)).Length();
            if ((this.PlayerManager.CarriedInstance == instance || (double) num3 < (double) num2) && this.PlayerManager.Action != ActionType.Dying)
              this.PlayerManager.Action = ActionType.Suffering;
            if ((instance.Trile.ActorSettings.Type == ActorType.TntBlock || state.IsChainsploding) && instance.InstanceId != -1)
              this.ParticleSystemManager.Add(new TrixelParticleSystem(this.Game, new TrixelParticleSystem.Settings()
              {
                ExplodingInstance = instance,
                EnergySource = new Vector3?(instance.Center),
                MaximumSize = 7,
                Energy = flag4 ? 3f : 1.5f,
                Darken = true,
                ParticleCount = 4 + 12 / Math.Max(1, this.TrixelParticleSystems.Count - 3)
              }));
            if (ActorTypeExtensions.IsPickable(instance.Trile.ActorSettings.Type))
            {
              instance.Enabled = false;
              this.LevelMaterializer.GetTrileMaterializer(instance.Trile).UpdateInstance(instance);
            }
            else
              this.ClearDestructible(instance, false);
            this.DropSupportedTriles(instance);
            this.DestroyNeighborhood(instance, state);
          }
          if (state.Explosion != null)
          {
            state.Flare.Filter = Color.Lerp(flag4 ? new Color(0.5f, 1f, 0.25f) : new Color(1f, 0.5f, 0.25f), Color.Black, state.Explosion.Timing.NormalizedStep);
            if (state.Explosion.Timing.Ended)
            {
              this.bsToRemove.Add(instance);
              if (instance.PhysicsState != null)
                instance.PhysicsState.ShouldRespawn = ActorTypeExtensions.IsPickable(instance.Trile.ActorSettings.Type);
              this.LevelManager.RemovePlane(state.Explosion);
              this.LevelManager.RemovePlane(state.Flare);
            }
          }
        }
      }
      foreach (TrileInstance key in this.bsToRemove)
        this.bombStates.Remove(key);
      this.bsToRemove.Clear();
      foreach (KeyValuePair<TrileInstance, BombsHost.BombState> keyValuePair in this.bsToAdd)
      {
        if (!this.bombStates.ContainsKey(keyValuePair.Key))
          this.bombStates.Add(keyValuePair.Key, keyValuePair.Value);
      }
      this.bsToAdd.Clear();
    }

    private void ClearDestructible(TrileInstance instance, bool skipRecull)
    {
      BombsHost.DestructibleGroup destructibleGroup;
      if (this.indexedDg.TryGetValue(instance, out destructibleGroup))
      {
        int count = this.LevelManager.Groups.Count;
        this.LevelManager.ClearTrile(instance, skipRecull);
        if (count != this.LevelManager.Groups.Count)
        {
          foreach (TrileInstance key in destructibleGroup.AllTriles)
          {
            this.GameState.SaveData.ThisLevel.DestroyedTriles.Add(key.OriginalEmplacement);
            this.indexedDg.Remove(key);
          }
          this.destructibleGroups.Remove(destructibleGroup);
          destructibleGroup.RespawnIn = new float?();
        }
        else
          destructibleGroup.RespawnIn = new float?(1.5f);
      }
      else
        this.LevelManager.ClearTrile(instance, skipRecull);
    }

    private void DestroyNeighborhood(TrileInstance instance, BombsHost.BombState state)
    {
      Vector3 vector3_1 = FezMath.SideMask(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      bool flag1 = (double) vector3_1.X != 0.0;
      bool flag2 = flag1;
      int num = flag2 ? (int) vector3_2.Z : (int) vector3_2.X;
      Point point1 = new Point(flag1 ? instance.Emplacement.X : instance.Emplacement.Z, instance.Emplacement.Y);
      Point[] pointArray = instance.Trile.ActorSettings.Type == ActorType.BigBomb ? BombsHost.BigBombOffsets : BombsHost.SmallBombOffsets;
      this.LevelManager.WaitForScreenInvalidation();
      foreach (Point point2 in pointArray)
      {
        bool chainsploded = false;
        bool needsRecull = false;
        Point key = new Point(point1.X + point2.X, point1.Y + point2.Y);
        Limit limit;
        if (this.LevelManager.ScreenSpaceLimits.TryGetValue(key, out limit))
        {
          limit.End += num;
          TrileEmplacement id = new TrileEmplacement(flag1 ? key.X : limit.Start, key.Y, flag2 ? limit.Start : key.X);
          while ((flag2 ? id.Z : id.X) != limit.End)
          {
            TrileInstance nearestNeighbor = this.LevelManager.TrileInstanceAt(ref id);
            if (!this.TryExplodeAt(state, nearestNeighbor, ref chainsploded, ref needsRecull))
            {
              if (flag2)
                id.Z += num;
              else
                id.X += num;
            }
            else
              break;
          }
          if (needsRecull)
          {
            this.LevelManager.RecullAt(id);
            this.TrixelParticleSystems.UnGroundAll();
          }
        }
      }
    }

    private bool TryExplodeAt(BombsHost.BombState state, TrileInstance nearestNeighbor, ref bool chainsploded, ref bool needsRecull)
    {
      if (nearestNeighbor != null && nearestNeighbor.Enabled && !nearestNeighbor.Trile.Immaterial)
      {
        if (!ActorTypeExtensions.IsChainsploding(nearestNeighbor.Trile.ActorSettings.Type) && !ActorTypeExtensions.IsDestructible(nearestNeighbor.Trile.ActorSettings.Type))
          return true;
        if (!this.bombStates.ContainsKey(nearestNeighbor))
        {
          if (ActorTypeExtensions.IsBomb(nearestNeighbor.Trile.ActorSettings.Type))
            nearestNeighbor.PhysicsState.Respawned = false;
          if (!chainsploded)
          {
            this.bsToAdd.Add(new KeyValuePair<TrileInstance, BombsHost.BombState>(nearestNeighbor, new BombsHost.BombState()
            {
              SincePickup = state.SincePickup - this.ChainsplodeDelay,
              IsChainsploding = true,
              ChainsplodedBy = state
            }));
            chainsploded = true;
          }
          else
          {
            this.ClearDestructible(nearestNeighbor, true);
            this.LevelMaterializer.CullInstanceOut(nearestNeighbor);
            this.DropSupportedTriles(nearestNeighbor);
            needsRecull = true;
          }
          return true;
        }
      }
      return false;
    }

    private void DropSupportedTriles(TrileInstance instance)
    {
      foreach (Vector3 vector3 in BombsHost.CornerNeighbors)
      {
        TrileInstance trileInstance = this.LevelManager.ActualInstanceAt(instance.Center + instance.TransformedSize * vector3);
        if (trileInstance != null && trileInstance.PhysicsState != null)
        {
          MultipleHits<TrileInstance> ground = trileInstance.PhysicsState.Ground;
          if (ground.NearLow == instance)
            trileInstance.PhysicsState.Ground = new MultipleHits<TrileInstance>()
            {
              FarHigh = ground.FarHigh
            };
          if (ground.FarHigh == instance)
            trileInstance.PhysicsState.Ground = new MultipleHits<TrileInstance>()
            {
              NearLow = ground.NearLow
            };
        }
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.CameraManager.Viewpoint == Viewpoint.Perspective || this.bombStates.Count == 0)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Equal, StencilMask.Bomb);
      this.flashesMesh.Draw();
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }

    private class BombState
    {
      public TimeSpan SincePickup;
      public BackgroundPlane Explosion;
      public BackgroundPlane Flare;
      public Group Flash;
      public bool IsChainsploding;
      public SoundEmitter Emitter;
      public BombsHost.BombState ChainsplodedBy;
    }

    private class DestructibleGroup
    {
      public List<TrileInstance> AllTriles;
      public TrileGroup Group;
      public float? RespawnIn;
    }
  }
}
