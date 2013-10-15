// Type: FezGame.Components.WaterfallsHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
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
using System.Linq;

namespace FezGame.Components
{
  internal class WaterfallsHost : GameComponent
  {
    private readonly List<WaterfallsHost.WaterfallState> Waterfalls = new List<WaterfallsHost.WaterfallState>();
    private SoundEffect SewageFallSound;

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IPlaneParticleSystems PlaneParticleSystems { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    public WaterfallsHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.SewageFallSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Sewer/SewageFall");
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      foreach (WaterfallsHost.WaterfallState waterfallState in this.Waterfalls)
        waterfallState.Dispose();
      this.Waterfalls.Clear();
      foreach (BackgroundPlane plane in Enumerable.ToArray<BackgroundPlane>((IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values))
      {
        if (plane.ActorType == ActorType.Waterfall || plane.ActorType == ActorType.Trickle)
        {
          Vector3 vector3_1 = Vector3.Transform(plane.Size * plane.Scale * Vector3.UnitX / 2f, plane.Rotation);
          Vector3 vector = Vector3.Transform(Vector3.UnitZ, plane.Rotation);
          Vector3 vector3_2 = FezMath.XZMask - FezMath.Abs(vector);
          Vector3 vector3_3 = plane.Position + plane.Size * plane.Scale * Vector3.UnitY / 2f - new Vector3(0.0f, 1.0 / 32.0, 0.0f) - vector * 2f / 16f;
          Game game = this.Game;
          int maximumCount = 25;
          PlaneParticleSystemSettings particleSystemSettings1 = new PlaneParticleSystemSettings();
          particleSystemSettings1.SpawnVolume = new BoundingBox()
          {
            Min = vector3_3 - vector3_1,
            Max = vector3_3 + vector3_1
          };
          PlaneParticleSystemSettings particleSystemSettings2 = particleSystemSettings1;
          VaryingVector3 varyingVector3_1 = new VaryingVector3();
          varyingVector3_1.Base = Vector3.Up * 1.6f + vector / 4f;
          varyingVector3_1.Variation = Vector3.Up * 0.8f + vector / 4f + vector3_2 / 2f;
          VaryingVector3 varyingVector3_2 = varyingVector3_1;
          particleSystemSettings2.Velocity = varyingVector3_2;
          particleSystemSettings1.Gravity = new Vector3(0.0f, -0.15f, 0.0f);
          particleSystemSettings1.SpawningSpeed = 5f;
          particleSystemSettings1.RandomizeSpawnTime = true;
          particleSystemSettings1.ParticleLifetime = 2f;
          particleSystemSettings1.FadeInDuration = 0.0f;
          particleSystemSettings1.FadeOutDuration = 0.1f;
          particleSystemSettings1.SizeBirth = (VaryingVector3) new Vector3(1.0 / 16.0);
          particleSystemSettings1.ColorLife = (VaryingColor) (this.LevelManager.WaterType == LiquidType.Sewer ? new Color(215, 232, 148) : new Color(1f, 1f, 1f, 0.75f));
          particleSystemSettings1.Texture = this.CMProvider.Global.Load<Texture2D>("Background Planes/white_square");
          particleSystemSettings1.BlendingMode = BlendingMode.Alphablending;
          particleSystemSettings1.ClampToTrixels = true;
          particleSystemSettings1.Billboarding = true;
          particleSystemSettings1.FullBright = this.LevelManager.WaterType == LiquidType.Sewer;
          particleSystemSettings1.UseCallback = true;
          PlaneParticleSystemSettings settings = particleSystemSettings1;
          PlaneParticleSystem planeParticleSystem = new PlaneParticleSystem(game, maximumCount, settings);
          if (this.LevelManager.WaterType == LiquidType.Sewer)
          {
            planeParticleSystem.DrawOrder = 20;
            planeParticleSystem.Settings.StencilMask = new StencilMask?(StencilMask.Level);
          }
          this.PlaneParticleSystems.Add(planeParticleSystem);
          this.Waterfalls.Add(new WaterfallsHost.WaterfallState(plane, planeParticleSystem, this));
        }
        else if (plane.ActorType == ActorType.Drips)
        {
          Vector3 vector3_1 = new Vector3(plane.Size.X, 0.0f, plane.Size.X) / 2f;
          Vector3 vector3_2 = Vector3.Transform(Vector3.UnitZ, plane.Rotation);
          Vector3 vector3_3 = FezMath.XZMask - FezMath.Abs(vector3_2);
          Vector3 vector3_4 = plane.Position - new Vector3(0.0f, 0.125f, 0.0f);
          bool flag = plane.Crosshatch || plane.Billboard;
          Game game = this.Game;
          int maximumCount = 25;
          PlaneParticleSystemSettings particleSystemSettings1 = new PlaneParticleSystemSettings();
          particleSystemSettings1.SpawnVolume = new BoundingBox()
          {
            Min = vector3_4 - vector3_1,
            Max = vector3_4 + vector3_1
          };
          PlaneParticleSystemSettings particleSystemSettings2 = particleSystemSettings1;
          VaryingVector3 varyingVector3_1 = new VaryingVector3();
          varyingVector3_1.Base = Vector3.Zero;
          varyingVector3_1.Variation = Vector3.Zero;
          VaryingVector3 varyingVector3_2 = varyingVector3_1;
          particleSystemSettings2.Velocity = varyingVector3_2;
          particleSystemSettings1.Gravity = new Vector3(0.0f, -0.15f, 0.0f);
          particleSystemSettings1.SpawningSpeed = 2f;
          particleSystemSettings1.RandomizeSpawnTime = true;
          particleSystemSettings1.ParticleLifetime = 2f;
          particleSystemSettings1.FadeInDuration = 0.0f;
          particleSystemSettings1.FadeOutDuration = 0.0f;
          particleSystemSettings1.SizeBirth = (VaryingVector3) new Vector3(1.0 / 16.0);
          particleSystemSettings1.ColorLife = (VaryingColor) (this.LevelManager.WaterType == LiquidType.Sewer ? new Color(215, 232, 148) : Color.White);
          particleSystemSettings1.Texture = this.CMProvider.Global.Load<Texture2D>("Background Planes/white_square");
          particleSystemSettings1.BlendingMode = BlendingMode.Alphablending;
          particleSystemSettings1.ClampToTrixels = true;
          particleSystemSettings1.FullBright = true;
          PlaneParticleSystemSettings settings = particleSystemSettings1;
          PlaneParticleSystem system = new PlaneParticleSystem(game, maximumCount, settings);
          if (this.LevelManager.WaterType == LiquidType.Sewer)
          {
            system.DrawOrder = 20;
            system.Settings.StencilMask = new StencilMask?(StencilMask.Level);
          }
          if (flag)
          {
            system.Settings.Billboarding = true;
            system.Settings.SpawnVolume = new BoundingBox()
            {
              Min = vector3_4 - vector3_1,
              Max = vector3_4 + vector3_1
            };
          }
          else
          {
            system.Settings.Doublesided = plane.Doublesided;
            system.Settings.SpawnVolume = new BoundingBox()
            {
              Min = vector3_4 - vector3_1 * vector3_3,
              Max = vector3_4 + vector3_1 * vector3_3
            };
            system.Settings.Orientation = new FaceOrientation?(FezMath.OrientationFromDirection(vector3_2));
          }
          this.PlaneParticleSystems.Add(system);
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      foreach (WaterfallsHost.WaterfallState waterfallState in this.Waterfalls)
        waterfallState.Update(gameTime.ElapsedGameTime);
    }

    private class WaterfallState
    {
      private readonly List<BackgroundPlane> AttachedPlanes = new List<BackgroundPlane>();
      private readonly BackgroundPlane Plane;
      private readonly BackgroundPlane Splash;
      private readonly PlaneParticleSystem ParticleSystem;
      private readonly float Top;
      private readonly Vector3 TerminalPosition;
      private readonly WaterfallsHost Host;
      private float lastDistToTop;
      private SoundEmitter BubblingEmitter;
      private float sinceAlive;

      [ServiceDependency]
      public IPlayerManager PlayerManager { private get; set; }

      [ServiceDependency]
      public ILevelMaterializer LevelMaterializer { private get; set; }

      [ServiceDependency]
      public IGameLevelManager LevelManager { private get; set; }

      [ServiceDependency]
      public IContentManagerProvider CMProvider { private get; set; }

      [ServiceDependency]
      public IGameStateManager GameState { private get; set; }

      public WaterfallState(BackgroundPlane plane, PlaneParticleSystem ps, WaterfallsHost host)
      {
        WaterfallsHost.WaterfallState waterfallState = this;
        ServiceHelper.InjectServices((object) this);
        this.Host = host;
        this.Plane = plane;
        this.ParticleSystem = ps;
        bool flag = plane.ActorType == ActorType.Trickle;
        this.Splash = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.LevelManager.WaterType != LiquidType.Sewer ? (this.LevelManager.WaterType != LiquidType.Purple ? this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/water/" + (flag ? "water_small_splash" : "water_large_splash")) : this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/waterPink/" + (flag ? "water_small_splash" : "water_large_splash"))) : this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/sewer/" + (flag ? "sewer_small_splash" : "sewer_large_splash")))
        {
          Doublesided = true,
          Crosshatch = true
        };
        this.LevelManager.AddPlane(this.Splash);
        this.Top = FezMath.Dot(this.Plane.Position + this.Plane.Scale * this.Plane.Size / 2f, Vector3.UnitY);
        this.TerminalPosition = this.Plane.Position - this.Plane.Scale * this.Plane.Size / 2f * Vector3.UnitY + Vector3.Transform(Vector3.UnitZ, plane.Rotation) / 16f;
        foreach (BackgroundPlane backgroundPlane in Enumerable.Where<BackgroundPlane>((IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values, (Func<BackgroundPlane, bool>) (x =>
        {
          int? local_0 = x.AttachedPlane;
          int local_1 = plane.Id;
          if ((local_0.GetValueOrDefault() != local_1 ? 0 : (local_0.HasValue ? 1 : 0)) != 0)
            return FezMath.AlmostEqual(Vector3.Transform(Vector3.UnitZ, plane.Rotation).Y, 0.0f);
          else
            return false;
        })))
          this.AttachedPlanes.Add(backgroundPlane);
        Vector3 position = this.LevelManager.WaterType == LiquidType.None ? this.Top * Vector3.UnitY + this.Plane.Position * FezMath.XZMask : this.TerminalPosition * FezMath.XZMask + this.LevelManager.WaterHeight * Vector3.UnitY;
        Waiters.Wait((double) RandomHelper.Between(0.0, 1.0), (Action) (() => waterfallState.BubblingEmitter = SoundEffectExtensions.EmitAt(waterfallState.Host.SewageFallSound, position, true, RandomHelper.Centered(0.025), 0.0f)));
      }

      public void Update(TimeSpan elapsed)
      {
        float num = this.LevelManager.WaterHeight - 0.5f;
        if (this.BubblingEmitter != null)
        {
          bool flag = !this.GameState.FarawaySettings.InTransition && !ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action);
          this.sinceAlive = FezMath.Saturate(this.sinceAlive + (float) (elapsed.TotalSeconds / 2.0 * (flag ? 1.0 : -1.0)));
        }
        if ((double) this.TerminalPosition.Y <= (double) num)
        {
          float b = this.Top - num;
          if ((double) b <= 0.0)
          {
            if (!this.Splash.Hidden)
            {
              this.ParticleSystem.Enabled = this.ParticleSystem.Visible = false;
              this.Splash.Hidden = true;
              this.Plane.Hidden = true;
              foreach (BackgroundPlane backgroundPlane in this.AttachedPlanes)
                backgroundPlane.Hidden = true;
            }
            if (this.BubblingEmitter == null)
              return;
            this.BubblingEmitter.VolumeFactor = 0.0f;
          }
          else
          {
            if (this.Splash.Hidden)
            {
              this.ParticleSystem.Enabled = this.ParticleSystem.Visible = true;
              this.Splash.Hidden = false;
              this.Plane.Hidden = false;
              foreach (BackgroundPlane backgroundPlane in this.AttachedPlanes)
                backgroundPlane.Hidden = false;
            }
            if (this.BubblingEmitter != null)
            {
              this.BubblingEmitter.VolumeFactor = FezMath.Saturate(b / 2f) * this.sinceAlive;
              if (this.LevelManager.WaterType != LiquidType.None)
                this.BubblingEmitter.Position = FezMath.XZMask * this.TerminalPosition + num * Vector3.UnitY;
            }
            this.Splash.Position = new Vector3(this.TerminalPosition.X, num + this.Splash.Size.Y / 2f, this.TerminalPosition.Z);
            if (FezMath.AlmostEqual(this.lastDistToTop, b, 1.0 / 16.0))
              return;
            foreach (BackgroundPlane backgroundPlane in this.AttachedPlanes)
            {
              backgroundPlane.Scale = new Vector3(backgroundPlane.Scale.X, b / backgroundPlane.Size.Y, backgroundPlane.Scale.Z);
              backgroundPlane.Position = new Vector3(backgroundPlane.Position.X, num + b / 2f, backgroundPlane.Position.Z);
            }
            this.Plane.Scale = new Vector3(this.Plane.Scale.X, b / this.Plane.Size.Y, this.Plane.Scale.Z);
            this.Plane.Position = new Vector3(this.Plane.Position.X, num + b / 2f, this.Plane.Position.Z);
            this.lastDistToTop = b;
          }
        }
        else
        {
          if (this.Splash.Hidden)
            return;
          this.Splash.Hidden = true;
        }
      }

      public void Dispose()
      {
        if (this.BubblingEmitter == null || this.BubblingEmitter.Dead)
          return;
        this.BubblingEmitter.Cue.Stop(false);
      }
    }
  }
}
