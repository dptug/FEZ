// Type: FezGame.Components.Actions.GateWarp
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components.Actions
{
  internal class GateWarp : PlayerAction
  {
    private const float FadeSeconds = 2.25f;
    private ArtObjectInstance GateAo;
    private GateWarp.Phases Phase;
    private TimeSpan SinceStarted;
    private TimeSpan SinceRisen;
    private SoundEffect WarpSound;
    private PlaneParticleSystem particles;
    private Mesh rgbPlanes;
    private float sinceInitialized;
    private Vector3 originalCenter;
    private float rotateAccum;

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { private get; set; }

    [ServiceDependency]
    public IPlaneParticleSystems PlaneParticleSystems { private get; set; }

    public GateWarp(Game game)
      : base(game)
    {
      this.DrawOrder = 901;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LightingPostProcess.DrawOnTopLights += new Action(this.DrawLights);
      this.WarpSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Zu/WarpGateActivate");
    }

    private void InitializeRgbGate()
    {
      IPlaneParticleSystems planeParticleSystems = this.PlaneParticleSystems;
      GateWarp gateWarp = this;
      Game game = this.Game;
      int maximumCount = 400;
      PlaneParticleSystemSettings particleSystemSettings = new PlaneParticleSystemSettings();
      particleSystemSettings.SpawnVolume = new BoundingBox()
      {
        Min = this.GateAo.Position + new Vector3(-3.25f, -6f, -3.25f),
        Max = this.GateAo.Position + new Vector3(3.25f, -2f, 3.25f)
      };
      particleSystemSettings.Velocity.Base = new Vector3(0.0f, 0.6f, 0.0f);
      particleSystemSettings.Velocity.Variation = new Vector3(0.0f, 0.1f, 0.1f);
      particleSystemSettings.SpawningSpeed = 7f;
      particleSystemSettings.ParticleLifetime = 6f;
      particleSystemSettings.Acceleration = 0.375f;
      particleSystemSettings.SizeBirth = (VaryingVector3) new Vector3(0.25f, 0.25f, 0.25f);
      particleSystemSettings.ColorBirth = (VaryingColor) Color.Black;
      particleSystemSettings.ColorLife.Base = new Color(0.5f, 0.5f, 0.5f, 1f);
      particleSystemSettings.ColorLife.Variation = new Color(0.5f, 0.5f, 0.5f, 1f);
      particleSystemSettings.ColorDeath = (VaryingColor) Color.Black;
      particleSystemSettings.FullBright = true;
      particleSystemSettings.RandomizeSpawnTime = true;
      particleSystemSettings.Billboarding = true;
      particleSystemSettings.Texture = this.CMProvider.Global.Load<Texture2D>("Background Planes/dust_particle");
      particleSystemSettings.BlendingMode = BlendingMode.Additive;
      PlaneParticleSystemSettings settings = particleSystemSettings;
      PlaneParticleSystem planeParticleSystem1;
      PlaneParticleSystem planeParticleSystem2 = planeParticleSystem1 = new PlaneParticleSystem(game, maximumCount, settings);
      gateWarp.particles = planeParticleSystem1;
      PlaneParticleSystem system = planeParticleSystem2;
      planeParticleSystems.Add(system);
      this.rgbPlanes = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        DepthWrites = false,
        AlwaysOnTop = true,
        SamplerState = SamplerState.LinearClamp,
        Blending = new BlendingMode?(BlendingMode.Additive),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/rgb_gradient"))
      };
      this.rgbPlanes.AddFace(new Vector3(1f, 4.5f, 1f), new Vector3(0.0f, 3f, 0.0f), FaceOrientation.Front, true).Material = new Material()
      {
        Diffuse = Vector3.Zero,
        Opacity = 0.0f
      };
      this.rgbPlanes.AddFace(new Vector3(1f, 4.5f, 1f), new Vector3(0.0f, 3f, 0.0f), FaceOrientation.Front, true).Material = new Material()
      {
        Diffuse = Vector3.Zero,
        Opacity = 0.0f
      };
      this.rgbPlanes.AddFace(new Vector3(1f, 4.5f, 1f), new Vector3(0.0f, 3f, 0.0f), FaceOrientation.Front, true).Material = new Material()
      {
        Diffuse = Vector3.Zero,
        Opacity = 0.0f
      };
    }

    protected override void Begin()
    {
      base.Begin();
      this.SinceStarted = TimeSpan.Zero;
      this.PlayerManager.LookingDirection = HorizontalDirection.Left;
      this.SinceRisen = TimeSpan.Zero;
      this.Phase = GateWarp.Phases.Rise;
      foreach (SoundEmitter soundEmitter in this.SoundManager.Emitters)
        soundEmitter.FadeOutAndDie(2f);
      this.SoundManager.FadeFrequencies(true, 2f);
      this.SoundManager.FadeVolume(this.SoundManager.MusicVolumeFactor, 0.0f, 3f);
      SoundEffectExtensions.EmitAt(this.WarpSound, this.PlayerManager.Position);
      this.rgbPlanes = (Mesh) null;
      this.particles = (PlaneParticleSystem) null;
      this.sinceInitialized = 0.0f;
      this.originalCenter = this.CameraManager.Center;
      this.CameraManager.Constrained = true;
      this.GateAo = Enumerable.FirstOrDefault<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObject.ActorType == ActorType.WarpGate));
      if (this.GateAo == null || this.GameState.SaveData.UnlockedWarpDestinations.Count <= 1)
        return;
      this.InitializeRgbGate();
    }

    protected override bool Act(TimeSpan elapsed)
    {
      // ISSUE: unable to decompile the method.
    }

    private void DoLoad(bool dummy)
    {
      this.LevelManager.ChangeLevel(this.PlayerManager.WarpPanel.Destination);
      this.Phase = GateWarp.Phases.FadeIn;
      this.GameState.SaveData.View = this.PlayerManager.OriginWarpViewpoint;
      this.GameState.SaveData.Ground = Enumerable.First<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObject.ActorType == ActorType.WarpGate)).Position - Vector3.UnitY + FezMath.AsVector(FezMath.VisibleOrientation(this.GameState.SaveData.View)) * 2f;
      this.PlayerManager.CheckpointGround = (TrileInstance) null;
      this.PlayerManager.RespawnAtCheckpoint();
      this.CameraManager.Center = this.PlayerManager.Position + Vector3.Up * this.PlayerManager.Size.Y / 2f + Vector3.UnitY;
      this.CameraManager.SnapInterpolation();
      this.LevelMaterializer.CullInstances();
      this.PlayerManager.Hidden = false;
      this.GameState.ScheduleLoadEnd = true;
      this.SinceStarted = TimeSpan.Zero;
      this.PlayerManager.WarpPanel = (WarpPanel) null;
      this.particles = (PlaneParticleSystem) null;
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
      if (!this.IsActionAllowed(this.PlayerManager.Action))
        return;
      if (this.rgbPlanes != null && this.Phase <= GateWarp.Phases.Decelerate)
      {
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
        int num = 1;
        float amount = 0.01f;
        for (int index = 0; index < 3; ++index)
        {
          this.rgbPlanes.Groups[index].Material.Diffuse = Vector3.Lerp(this.rgbPlanes.Groups[index].Material.Diffuse, new Vector3(index == 0 ? (float) num : 0.0f, index == 1 ? (float) num : 0.0f, index == 2 ? (float) num : 0.0f), amount);
          this.rgbPlanes.Groups[index].Material.Opacity = MathHelper.Lerp(this.rgbPlanes.Groups[index].Material.Opacity, (float) num, amount);
        }
        this.rgbPlanes.Draw();
      }
      if (this.Phase != GateWarp.Phases.FadeOut && this.Phase != GateWarp.Phases.FadeIn && this.Phase != GateWarp.Phases.LevelChange)
        return;
      double linearStep = this.SinceStarted.TotalSeconds / 2.25;
      if (this.Phase == GateWarp.Phases.FadeIn)
        linearStep = 1.0 - linearStep;
      float alpha = FezMath.Saturate(Easing.EaseIn(linearStep, EasingType.Cubic));
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      this.TargetRenderer.DrawFullscreen(new Color(0.0f, 0.0f, 0.0f, alpha));
    }

    private void DrawLights()
    {
      if (!this.IsActionAllowed(this.PlayerManager.Action) || this.LevelManager.WaterType == LiquidType.Sewer || (this.rgbPlanes == null || this.Phase > GateWarp.Phases.Decelerate))
        return;
      (this.rgbPlanes.Effect as DefaultEffect).Pass = LightingEffectPass.Pre;
      this.rgbPlanes.Draw();
      (this.rgbPlanes.Effect as DefaultEffect).Pass = LightingEffectPass.Main;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.GateWarp)
        return this.Phase != GateWarp.Phases.None;
      else
        return true;
    }

    private enum Phases
    {
      None,
      Rise,
      Accelerate,
      Warping,
      Decelerate,
      FadeOut,
      LevelChange,
      FadeIn,
    }
  }
}
