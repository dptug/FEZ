// Type: FezGame.Components.Actions.LesserWarp
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Structure.Input;
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
  internal class LesserWarp : PlayerAction
  {
    private const float FadeSeconds = 2.5f;
    private ArtObjectInstance GateAo;
    private Vector3 OriginalPosition;
    private Mesh MaskMesh;
    private Texture2D WhiteTexture;
    private Texture2D StarTexture;
    private LesserWarp.Phases Phase;
    private float GateAngle;
    private float GateTurnSpeed;
    private Vector3 RiseAxis;
    private float RisePhi;
    private float RiseStep;
    private TimeSpan SinceRisen;
    private TimeSpan SinceStarted;
    private bool HasCubeShard;
    private Vector3 OriginalCenter;
    private PlaneParticleSystem particles;
    private Mesh rgbPlanes;
    private float sinceInitialized;
    private SoundEffect sRise;
    private SoundEffect sLower;
    private SoundEffect sActivate;
    private SoundEmitter eIdleSpin;
    private IWaiter fader;

    [ServiceDependency]
    public IDotManager DotManager { private get; set; }

    [ServiceDependency]
    public ISpeechBubbleManager SpeechBubble { private get; set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { private get; set; }

    [ServiceDependency]
    public IPlaneParticleSystems PlaneParticleSystems { private get; set; }

    public LesserWarp(Game game)
      : base(game)
    {
      this.DrawOrder = 901;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      if (this.eIdleSpin != null && this.eIdleSpin.Cue != null && (!this.eIdleSpin.Cue.IsDisposed && this.eIdleSpin.Cue.State != SoundState.Stopped))
        this.eIdleSpin.Cue.Stop(false);
      this.eIdleSpin = (SoundEmitter) null;
      this.rgbPlanes = (Mesh) null;
      this.particles = (PlaneParticleSystem) null;
      this.Phase = LesserWarp.Phases.None;
      this.sinceInitialized = 0.0f;
      this.GateAo = Enumerable.FirstOrDefault<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObject.ActorType == ActorType.LesserGate));
      if (this.GateAo == null)
        return;
      this.InitializeRgbGate();
      LesserWarp lesserWarp = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.Textured textured1 = new DefaultEffect.Textured();
      textured1.Fullbright = true;
      DefaultEffect.Textured textured2 = textured1;
      mesh2.Effect = (BaseEffect) textured2;
      mesh1.DepthWrites = false;
      mesh1.Texture = (Dirtyable<Texture>) ((Texture) this.WhiteTexture);
      mesh1.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, -1.570796f);
      Mesh mesh3 = mesh1;
      lesserWarp.MaskMesh = mesh3;
      this.MaskMesh.AddFace(new Vector3(2f), Vector3.Zero, FaceOrientation.Front, true, true);
      this.MaskMesh.BakeTransformWithNormal<FezVertexPositionNormalTexture>();
      this.MaskMesh.Position = this.GateAo.Position - Vector3.UnitY * 1.25f;
      this.HasCubeShard = Enumerable.Any<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x =>
      {
        if (ActorTypeExtensions.IsCubeShard(x.Trile.ActorSettings.Type))
          return (double) Vector3.Distance(x.Center, this.GateAo.Position) < 3.0;
        else
          return false;
      }));
      this.OriginalPosition = this.GateAo.Position;
      this.sRise = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Zu/LesserWarpRise");
      this.sLower = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Zu/LesserWarpLower");
      this.sActivate = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Zu/WarpGateActivate");
      this.eIdleSpin = SoundEffectExtensions.EmitAt(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Zu/LesserWarpIdleSpin"), this.GateAo.Position, true, true);
    }

    private void InitializeRgbGate()
    {
      IPlaneParticleSystems planeParticleSystems = this.PlaneParticleSystems;
      LesserWarp lesserWarp = this;
      Game game = this.Game;
      int maximumCount = 200;
      PlaneParticleSystemSettings particleSystemSettings = new PlaneParticleSystemSettings();
      particleSystemSettings.SpawnVolume = new BoundingBox()
      {
        Min = this.GateAo.Position + new Vector3(-2f, -3.5f, -2f),
        Max = this.GateAo.Position + new Vector3(2f, 2f, 2f)
      };
      particleSystemSettings.Velocity.Base = new Vector3(0.0f, 0.6f, 0.0f);
      particleSystemSettings.Velocity.Variation = new Vector3(0.0f, 0.1f, 0.1f);
      particleSystemSettings.SpawningSpeed = 5f;
      particleSystemSettings.ParticleLifetime = 6f;
      particleSystemSettings.Acceleration = 0.375f;
      particleSystemSettings.SizeBirth = (VaryingVector3) new Vector3(0.25f, 0.25f, 0.25f);
      particleSystemSettings.ColorBirth = (VaryingColor) Color.Black;
      particleSystemSettings.ColorLife = (VaryingColor) Color.Black;
      particleSystemSettings.ColorDeath = (VaryingColor) Color.Black;
      particleSystemSettings.FullBright = true;
      particleSystemSettings.RandomizeSpawnTime = true;
      particleSystemSettings.Billboarding = true;
      particleSystemSettings.Texture = this.CMProvider.Global.Load<Texture2D>("Background Planes/dust_particle");
      particleSystemSettings.BlendingMode = BlendingMode.Additive;
      PlaneParticleSystemSettings settings = particleSystemSettings;
      PlaneParticleSystem planeParticleSystem1;
      PlaneParticleSystem planeParticleSystem2 = planeParticleSystem1 = new PlaneParticleSystem(game, maximumCount, settings);
      lesserWarp.particles = planeParticleSystem1;
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
      this.rgbPlanes.Position = this.GateAo.Position + new Vector3(0.0f, -2f, 0.0f);
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.WhiteTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/FullWhite");
      this.StarTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/black_hole/Stars");
      this.LightingPostProcess.DrawOnTopLights += new Action(this.DrawLights);
    }

    protected override void TestConditions()
    {
      if (this.PlayerManager.Action == ActionType.LesserWarp || this.GateAo == null || this.Phase == LesserWarp.Phases.FadeIn)
        return;
      Vector3 b = FezMath.SideMask(this.CameraManager.Viewpoint);
      Vector3 a = FezMath.Abs(this.OriginalPosition - this.PlayerManager.Position);
      if (this.LevelManager.Name == "ZU_CODE_LOOP" && this.GameState.SaveData.ThisLevel.ScriptingState == "NOT_COLLECTED")
        this.HasCubeShard = true;
      bool flag = (double) FezMath.Dot(a, b) < 3.0 && (double) a.Y < 4.0 && !this.HasCubeShard;
      if (flag && (this.Phase == LesserWarp.Phases.None || this.Phase == LesserWarp.Phases.Lower))
      {
        if (this.Phase != LesserWarp.Phases.Lower)
        {
          this.RiseAxis = FezMath.RightVector(this.CameraManager.Viewpoint);
          this.RisePhi = FezMath.ToPhi(this.CameraManager.Viewpoint);
          this.SinceRisen = TimeSpan.Zero;
          this.SinceStarted = TimeSpan.Zero;
        }
        this.Phase = LesserWarp.Phases.Rise;
        SoundEffectExtensions.EmitAt(this.sRise, this.GateAo.Position);
      }
      else if (!flag && this.Phase != LesserWarp.Phases.Lower && this.Phase != LesserWarp.Phases.None)
      {
        if (this.Phase != LesserWarp.Phases.Rise)
        {
          this.RiseAxis = FezMath.RightVector(this.CameraManager.Viewpoint);
          this.RisePhi = FezMath.ToPhi(this.CameraManager.Viewpoint);
        }
        this.Phase = this.Phase == LesserWarp.Phases.Rise ? LesserWarp.Phases.Lower : LesserWarp.Phases.Decelerate;
        this.SinceStarted = TimeSpan.Zero;
        if (this.Phase == LesserWarp.Phases.Lower)
          SoundEffectExtensions.EmitAt(this.sLower, this.GateAo.Position);
      }
      else if (!flag && this.Phase == LesserWarp.Phases.None && (this.eIdleSpin != null && !this.eIdleSpin.Dead) && this.eIdleSpin.Cue.State == SoundState.Playing)
        this.eIdleSpin.Cue.Pause();
      if (this.HasCubeShard && (double) FezMath.Dot(a, b) < 3.0 && ((double) a.Y < 4.0 && this.PlayerManager.LastAction == ActionType.FindingTreasure))
        this.HasCubeShard = false;
      if (!this.PlayerManager.Grounded || !flag || (this.InputManager.Up != FezButtonState.Pressed || !this.SpeechBubble.Hidden))
        return;
      this.PlayerManager.Action = ActionType.LesserWarp;
      if (this.Phase == LesserWarp.Phases.None)
      {
        this.RiseAxis = FezMath.RightVector(this.CameraManager.Viewpoint);
        this.RisePhi = FezMath.ToPhi(this.CameraManager.Viewpoint);
        this.SinceRisen = TimeSpan.Zero;
        this.SinceStarted = TimeSpan.Zero;
        this.Phase = LesserWarp.Phases.Rise;
        SoundEffectExtensions.EmitAt(this.sRise, this.GateAo.Position);
      }
      else
      {
        if (this.Phase != LesserWarp.Phases.SpinWait)
          return;
        if (this.eIdleSpin.Cue.State == SoundState.Playing)
        {
          if (this.fader != null)
            this.fader.Cancel();
          this.eIdleSpin.FadeOutAndPause(1f);
        }
        this.DotManager.PreventPoI = true;
        this.DotManager.Burrow();
        foreach (SoundEmitter soundEmitter in this.SoundManager.Emitters)
          soundEmitter.FadeOutAndDie(2f);
        this.SoundManager.FadeFrequencies(true, 2f);
        this.SoundManager.FadeVolume(this.SoundManager.MusicVolumeFactor, 0.0f, 3f);
        SoundEffectExtensions.EmitAt(this.sActivate, this.GateAo.Position);
        this.Phase = LesserWarp.Phases.Accelerate;
        this.CameraManager.Constrained = true;
        this.OriginalCenter = this.CameraManager.Center;
        this.PlayerManager.LookingDirection = HorizontalDirection.Left;
        this.PlayerManager.Velocity = Vector3.Zero;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      this.SinceStarted = TimeSpan.Zero;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      base.Update(gameTime);
      if (this.rgbPlanes == null)
        return;
      this.rgbPlanes.Rotation = this.CameraManager.Rotation;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      // ISSUE: unable to decompile the method.
    }

    private void DoLoad(bool dummy)
    {
      this.LevelManager.ChangeLevel(this.GateAo.ActorSettings.DestinationLevel);
      this.Phase = LesserWarp.Phases.FadeIn;
      this.GameState.SaveData.Ground = Enumerable.First<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObject.ActorType == ActorType.WarpGate)).Position + Vector3.Down + FezMath.AsVector(FezMath.VisibleOrientation(this.GameState.SaveData.View)) * 2f;
      this.DotManager.PreventPoI = false;
      this.PlayerManager.Hidden = false;
      this.PlayerManager.CheckpointGround = (TrileInstance) null;
      this.PlayerManager.RespawnAtCheckpoint();
      this.CameraManager.Center = this.PlayerManager.Position + Vector3.Up * this.PlayerManager.Size.Y / 2f + Vector3.UnitY;
      this.CameraManager.SnapInterpolation();
      this.LevelMaterializer.CullInstances();
      this.GameState.ScheduleLoadEnd = true;
      this.SinceStarted = TimeSpan.Zero;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || !this.IsActionAllowed(this.PlayerManager.Action))
        return;
      if (this.Phase != LesserWarp.Phases.LevelChange && this.Phase != LesserWarp.Phases.FadeIn)
      {
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.WarpGate));
        this.MaskMesh.Draw();
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.WarpGate);
        float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
        float m11 = this.CameraManager.Radius / ((float) this.StarTexture.Width / 16f) / viewScale;
        float m22 = (float) ((double) this.CameraManager.Radius / (double) this.CameraManager.AspectRatio / ((double) this.StarTexture.Height / 16.0)) / viewScale;
        Matrix textureMatrix = new Matrix(m11, 0.0f, 0.0f, 0.0f, 0.0f, m22, 0.0f, 0.0f, (float) (-(double) m11 / 2.0), (float) (-(double) m22 / 2.0), 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
        this.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        this.TargetRenderer.DrawFullscreen((Texture) this.StarTexture, textureMatrix);
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.None));
      }
      if (this.rgbPlanes != null && this.Phase <= LesserWarp.Phases.Decelerate)
      {
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
        float num = (float) ((this.Phase == LesserWarp.Phases.Lower || this.Phase == LesserWarp.Phases.Rise ? (double) this.RiseStep : 1.0) * (1.0 - (double) this.LevelManager.ActualDiffuse.R / 512.0) * 0.800000011920929);
        float amount = this.Phase == LesserWarp.Phases.Decelerate || this.Phase == LesserWarp.Phases.Lower || this.Phase == LesserWarp.Phases.FadeOut ? 1f : 0.01f;
        if (this.Phase == LesserWarp.Phases.Accelerate)
          num = 1f;
        for (int index = 0; index < 3; ++index)
        {
          this.rgbPlanes.Groups[index].Material.Diffuse = Vector3.Lerp(this.rgbPlanes.Groups[index].Material.Diffuse, new Vector3(index == 0 ? num : 0.0f, index == 1 ? num : 0.0f, index == 2 ? num : 0.0f), amount);
          this.rgbPlanes.Groups[index].Material.Opacity = MathHelper.Lerp(this.rgbPlanes.Groups[index].Material.Opacity, num, amount);
        }
        this.rgbPlanes.Draw();
      }
      if (this.Phase != LesserWarp.Phases.FadeOut && this.Phase != LesserWarp.Phases.FadeIn && this.Phase != LesserWarp.Phases.LevelChange)
        return;
      double linearStep = this.SinceStarted.TotalSeconds / 2.5;
      if (this.Phase == LesserWarp.Phases.FadeIn)
        linearStep = 1.0 - linearStep;
      float alpha = FezMath.Saturate(Easing.EaseIn(linearStep, EasingType.Cubic));
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      this.TargetRenderer.DrawFullscreen(new Color(0.0f, 0.0f, 0.0f, alpha));
    }

    private void DrawLights()
    {
      if (this.GameState.Loading || !this.IsActionAllowed(this.PlayerManager.Action) || this.LevelManager.WaterType == LiquidType.Sewer)
        return;
      float num = this.Phase == LesserWarp.Phases.Lower || this.Phase == LesserWarp.Phases.Rise || this.Phase == LesserWarp.Phases.FadeOut ? this.RiseStep : 1f;
      if (this.rgbPlanes != null && this.Phase <= LesserWarp.Phases.Decelerate)
      {
        this.particles.Settings.ColorLife.Base = new Color(num / 2f, num / 2f, num / 2f, 1f);
        this.particles.Settings.ColorLife.Variation = new Color(num / 2f, num / 2f, num / 2f, 1f);
        (this.rgbPlanes.Effect as DefaultEffect).Pass = LightingEffectPass.Pre;
        this.rgbPlanes.Draw();
        (this.rgbPlanes.Effect as DefaultEffect).Pass = LightingEffectPass.Main;
      }
      (this.MaskMesh.Effect as DefaultEffect).Pass = LightingEffectPass.Pre;
      this.MaskMesh.Material.Opacity = num;
      this.MaskMesh.Draw();
      this.MaskMesh.Material.Opacity = 1f;
      (this.MaskMesh.Effect as DefaultEffect).Pass = LightingEffectPass.Main;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.LesserWarp)
        return this.Phase != LesserWarp.Phases.None;
      else
        return true;
    }

    private enum Phases
    {
      None,
      Rise,
      SpinWait,
      Lower,
      Accelerate,
      Warping,
      Decelerate,
      FadeOut,
      LevelChange,
      FadeIn,
    }
  }
}
