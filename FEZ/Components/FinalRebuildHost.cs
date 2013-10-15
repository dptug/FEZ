// Type: FezGame.Components.FinalRebuildHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
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
  internal class FinalRebuildHost : DrawableGameComponent
  {
    private const float ZoomDuration = 10f;
    private const float FlickerDuration = 1.25f;
    private const float SpinFillDuration = 10f;
    private const float Start1Duration = 5f;
    private const float Start2Duration = 4f;
    private const float Start3Duration = 6f;
    private const float SmoothStartDuration = 10f;
    private readonly Vector3[] CubeOffsets;
    private RenderTargetHandle RtHandle;
    private InvertEffect InvertEffect;
    private Mesh SolidCubes;
    private Mesh WhiteCube;
    private Quaternion OriginalCubeRotation;
    private ArtObjectInstance HexahedronAo;
    private NesGlitches Glitches;
    private Mesh RaysMesh;
    private Mesh FlareMesh;
    private SoundEffect sHexAppear;
    private SoundEffect sCubeAppear;
    private SoundEffect sMotorSpin1;
    private SoundEffect sMotorSpin2;
    private SoundEffect sMotorSpinAOK;
    private SoundEffect sMotorSpinCrash;
    private SoundEffect sRayWhiteout;
    private SoundEffect sAku;
    private SoundEffect sAmbientDrone;
    private SoundEffect sZoomIn;
    private SoundEmitter eAku;
    private SoundEmitter eMotor;
    private SoundEmitter eAmbient;
    private FinalRebuildHost.Phases ActivePhase;
    private float PhaseTime;
    private bool FirstUpdate;
    private float lastStep;

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { get; set; }

    public FinalRebuildHost(Game game)
      : base(game)
    {
      this.CubeOffsets = new Vector3[64];
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          for (int index3 = 0; index3 < 4; ++index3)
            this.CubeOffsets[index1 * 16 + index2 * 4 + index3] = new Vector3((float) index2 - 1.5f, (float) index1 - 1.5f, (float) index3 - 1.5f);
        }
      }
      this.DrawOrder = 750;
      this.Visible = this.Enabled = false;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.Destroy();
      this.Visible = this.Enabled = this.LevelManager.Name == "HEX_REBUILD";
      if (!this.Enabled)
        return;
      this.GameState.HideHUD = true;
      this.CameraManager.ChangeViewpoint(Viewpoint.Right, 0.0f);
      this.PlayerManager.Background = false;
      ArtObject artObject = this.CMProvider.CurrentLevel.Load<ArtObject>("Art Objects/NEW_HEXAO");
      int key = IdentifierPool.FirstAvailable<ArtObjectInstance>(this.LevelManager.ArtObjects);
      this.HexahedronAo = new ArtObjectInstance(artObject)
      {
        Id = key
      };
      this.LevelManager.ArtObjects.Add(key, this.HexahedronAo);
      this.HexahedronAo.Initialize();
      this.HexahedronAo.Hidden = true;
      this.WhiteCube = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.VertexColored(),
        Blending = new BlendingMode?(BlendingMode.Additive),
        DepthWrites = false
      };
      this.WhiteCube.Rotation = this.CameraManager.Rotation * Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAt(Vector3.One, Vector3.Zero, Vector3.Up));
      this.WhiteCube.AddColoredBox(new Vector3(4f), Vector3.Zero, Color.White, true);
      FinalRebuildHost finalRebuildHost = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.LitTextured litTextured1 = new DefaultEffect.LitTextured();
      litTextured1.Specular = true;
      litTextured1.Emissive = 0.5f;
      litTextured1.AlphaIsEmissive = true;
      DefaultEffect.LitTextured litTextured2 = litTextured1;
      mesh2.Effect = (BaseEffect) litTextured2;
      mesh1.Blending = new BlendingMode?(BlendingMode.Opaque);
      Mesh mesh3 = mesh1;
      finalRebuildHost.SolidCubes = mesh3;
      this.OriginalCubeRotation = this.SolidCubes.Rotation = this.WhiteCube.Rotation;
      ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry1 = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.CubeShard)).Geometry;
      ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry2 = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.SecretCube)).Geometry;
      this.sHexAppear = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/HexAppear");
      this.sCubeAppear = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/CubeAppear");
      this.sMotorSpin1 = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/MotorStart1");
      this.sMotorSpin2 = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/MotorStart2");
      this.sMotorSpinAOK = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/MotorStartAOK");
      this.sMotorSpinCrash = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/MotorStartCrash");
      this.sRayWhiteout = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/RayWhiteout");
      this.sAku = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/Aku");
      this.sZoomIn = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/ZoomIn");
      this.sAmbientDrone = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/HexRebuild/AmbientDrone");
      for (int index = 0; index < Math.Min(this.GameState.SaveData.CubeShards + this.GameState.SaveData.SecretCubes, 64); ++index)
      {
        Vector3 vector3 = this.CubeOffsets[index];
        ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> indexedPrimitives = index < this.GameState.SaveData.CubeShards ? geometry1 : geometry2;
        Group group = this.SolidCubes.AddGroup();
        group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(Enumerable.ToArray<VertexPositionNormalTextureInstance>((IEnumerable<VertexPositionNormalTextureInstance>) indexedPrimitives.Vertices), indexedPrimitives.Indices, indexedPrimitives.PrimitiveType);
        group.Position = vector3;
        group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, (float) RandomHelper.Random.Next(0, 4) * 1.570796f);
        group.Enabled = false;
        group.Material = new Material();
      }
      this.SolidCubes.Texture = this.LevelMaterializer.TrilesMesh.Texture;
      this.InvertEffect = new InvertEffect();
      this.RaysMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.VertexColored(),
        Blending = new BlendingMode?(BlendingMode.Additive),
        DepthWrites = false
      };
      this.FlareMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/flare_alpha")),
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      this.FlareMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, true);
      this.RtHandle = this.TargetRenderer.TakeTarget();
      this.TargetRenderer.ScheduleHook(this.DrawOrder, this.RtHandle.Target);
      ServiceHelper.AddComponent((IGameComponent) (this.Glitches = new NesGlitches(this.Game)));
    }

    private void Destroy()
    {
      if (this.Glitches != null)
        ServiceHelper.RemoveComponent<NesGlitches>(this.Glitches);
      this.Glitches = (NesGlitches) null;
      if (this.RtHandle != null)
      {
        this.TargetRenderer.UnscheduleHook(this.RtHandle.Target);
        this.TargetRenderer.ReturnTarget(this.RtHandle);
      }
      this.RtHandle = (RenderTargetHandle) null;
      if (this.SolidCubes != null)
        this.SolidCubes.Dispose();
      this.SolidCubes = (Mesh) null;
      if (this.WhiteCube != null)
        this.WhiteCube.Dispose();
      this.WhiteCube = (Mesh) null;
      if (this.RaysMesh != null)
        this.RaysMesh.Dispose();
      if (this.FlareMesh != null)
        this.FlareMesh.Dispose();
      this.RaysMesh = this.FlareMesh = (Mesh) null;
      if (this.InvertEffect != null)
        this.InvertEffect.Dispose();
      this.InvertEffect = (InvertEffect) null;
      this.HexahedronAo = (ArtObjectInstance) null;
      this.FirstUpdate = true;
      this.sAmbientDrone = this.sAku = this.sZoomIn = this.sHexAppear = this.sCubeAppear = this.sMotorSpin1 = this.sMotorSpin2 = this.sMotorSpinAOK = this.sMotorSpinCrash = this.sRayWhiteout = (SoundEffect) null;
      this.eAku = this.eAmbient = this.eMotor = (SoundEmitter) null;
      this.ActivePhase = FinalRebuildHost.Phases.ZoomInNega;
      this.PhaseTime = 0.0f;
      this.GameState.SkipRendering = false;
      this.GameState.HideHUD = false;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      if (this.FirstUpdate)
      {
        gameTime = new GameTime();
        this.FirstUpdate = false;
      }
      this.PhaseTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
      switch (this.ActivePhase)
      {
        case FinalRebuildHost.Phases.ZoomInNega:
          this.GameState.SkipRendering = true;
          if (gameTime.ElapsedGameTime.Ticks == 0L)
          {
            this.Glitches.ActiveGlitches = 0;
            this.Glitches.FreezeProbability = 0.0f;
            this.CameraManager.PixelsPerTrixel = 0.5f;
            this.CameraManager.SnapInterpolation();
            this.PlayerManager.Position = Vector3.Zero;
            this.PlayerManager.LookingDirection = HorizontalDirection.Right;
            this.SetHexVisible(false);
            this.CollisionManager.GravityFactor = 1f;
            SoundEffectExtensions.Emit(this.sZoomIn);
            this.eAmbient = SoundEffectExtensions.Emit(this.sAmbientDrone, true, 0.0f, 0.0f);
          }
          float amount = Easing.EaseIn((double) FezMath.Saturate(this.PhaseTime / 10f), EasingType.Sine);
          if ((double) this.PhaseTime > 0.25)
          {
            IGameCameraManager cameraManager = this.CameraManager;
            double num = (double) cameraManager.Radius * (double) MathHelper.Lerp(0.99f, 1f, amount);
            cameraManager.Radius = (float) num;
          }
          this.PlayerManager.Action = (double) this.PhaseTime > 7.0 ? ActionType.StandWinking : ActionType.Standing;
          this.PlayerManager.Velocity = Vector3.Zero;
          float num1 = Easing.EaseIn((double) FezMath.Saturate(this.PhaseTime / 11f), EasingType.Sine);
          this.CameraManager.Center = this.PlayerManager.Position + new Vector3(0.0f, 0.125f, 0.0f) + new Vector3(0.0f, (float) (Math.Sin((double) this.PhaseTime) * 0.25 * (1.0 - (double) num1)), 0.0f);
          this.CameraManager.SnapInterpolation();
          this.GameState.SkipRendering = false;
          this.eAmbient.VolumeFactor = num1;
          if ((double) this.PhaseTime <= 11.0)
            break;
          Waiters.Wait(0.75, (Action) (() => SoundEffectExtensions.Emit(this.sHexAppear))).AutoPause = true;
          this.ChangePhase();
          break;
        case FinalRebuildHost.Phases.FlickerIn:
          this.GameState.SkipRendering = true;
          if (gameTime.ElapsedGameTime.Ticks == 0L)
          {
            this.WhiteCube.Material.Diffuse = Vector3.Zero;
            this.WhiteCube.Rotation = this.OriginalCubeRotation;
            this.CameraManager.PixelsPerTrixel = 3f;
            this.CameraManager.SnapInterpolation();
            this.PlayerManager.Position = Vector3.Zero;
            if (this.eAmbient != null)
              this.eAmbient.VolumeFactor = 0.625f;
            this.PhaseTime = -1f;
          }
          this.PlayerManager.Action = ActionType.Standing;
          this.PlayerManager.Velocity = Vector3.Zero;
          this.CameraManager.Center = this.PlayerManager.Position + new Vector3(0.0f, 4.5f, 0.0f);
          this.CameraManager.SnapInterpolation();
          this.GameState.SkipRendering = false;
          this.WhiteCube.Position = this.PlayerManager.Position + new Vector3(0.0f, 6f, 0.0f);
          if ((double) this.PhaseTime <= 2.25)
            break;
          this.ChangePhase();
          break;
        case FinalRebuildHost.Phases.SpinFill:
          this.GameState.SkipRendering = true;
          if (gameTime.ElapsedGameTime.Ticks == 0L)
          {
            this.CameraManager.PixelsPerTrixel = 3f;
            this.CameraManager.SnapInterpolation();
            this.PlayerManager.Position = Vector3.Zero;
            this.WhiteCube.Material.Diffuse = Vector3.One;
            for (int index = 0; index < this.SolidCubes.Groups.Count; ++index)
              this.SolidCubes.Groups[index].CustomData = (object) null;
          }
          this.PlayerManager.Action = ActionType.Standing;
          this.PlayerManager.Velocity = Vector3.Zero;
          this.CameraManager.Center = this.PlayerManager.Position + new Vector3(0.0f, 4.5f, 0.0f);
          this.CameraManager.SnapInterpolation();
          this.GameState.SkipRendering = false;
          float num2 = Easing.EaseInOut((double) FezMath.Saturate(this.PhaseTime / 11f), EasingType.Sine);
          this.SolidCubes.Position = this.WhiteCube.Position = this.PlayerManager.Position + new Vector3(0.0f, 6f, 0.0f);
          this.SolidCubes.Rotation = this.WhiteCube.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) ((double) num2 * 6.28318548202515 * 3.0)) * this.OriginalCubeRotation;
          float num3 = Easing.EaseInOut((double) FezMath.Saturate(this.PhaseTime / 10f), EasingType.Quadratic);
          float pitch = MathHelper.Clamp((float) (((double) num3 - (double) this.lastStep) * 200.0 - 0.200000002980232), -1f, 1f);
          float num4 = 1f / (float) this.SolidCubes.Groups.Count;
          for (int index = 0; index < this.SolidCubes.Groups.Count; ++index)
          {
            float num5 = (float) index / (float) this.SolidCubes.Groups.Count;
            float num6 = Easing.EaseIn((double) FezMath.Saturate((num3 - num5) / num4), EasingType.Sine);
            if ((double) num6 == 1.0)
            {
              this.SolidCubes.Groups[index].Material.Diffuse = Vector3.One;
              this.SolidCubes.Groups[index].Enabled = true;
            }
            else if ((double) num6 == 0.0)
            {
              this.SolidCubes.Groups[index].Enabled = false;
            }
            else
            {
              if ((double) num6 > 0.125 && this.SolidCubes.Groups[index].CustomData == null)
              {
                SoundEffectExtensions.Emit(this.sCubeAppear, pitch);
                this.SolidCubes.Groups[index].CustomData = (object) true;
              }
              this.SolidCubes.Groups[index].Material.Diffuse = new Vector3((float) FezMath.AsNumeric(RandomHelper.Probability((double) num6)), (float) FezMath.AsNumeric(RandomHelper.Probability((double) num6)), (float) FezMath.AsNumeric(RandomHelper.Probability((double) num6)));
              this.SolidCubes.Groups[index].Enabled = RandomHelper.Probability((double) num6);
            }
          }
          this.lastStep = num3;
          if ((double) this.PhaseTime <= 12.0)
            break;
          this.eMotor = SoundEffectExtensions.Emit(this.sMotorSpin1);
          this.eAku = SoundEffectExtensions.Emit(this.sAku, true, 0.0f, 0.0f);
          this.lastStep = 0.0f;
          this.ChangePhase();
          break;
        case FinalRebuildHost.Phases.MotorStart1:
          this.GameState.SkipRendering = true;
          if (gameTime.ElapsedGameTime.Ticks == 0L)
          {
            this.HexahedronAo.Position = this.SolidCubes.Position;
            this.lastStep = 0.0f;
            for (int index = 0; index < this.SolidCubes.Groups.Count; ++index)
            {
              this.SolidCubes.Groups[index].Enabled = true;
              this.SolidCubes.Groups[index].Material.Diffuse = Vector3.One;
            }
            this.SolidCubes.Rotation = Quaternion.Identity;
            this.SolidCubes.Position = Vector3.Zero;
            this.SolidCubes.CollapseToBufferWithNormal<VertexPositionNormalTextureInstance>();
            this.SolidCubes.Position = this.PlayerManager.Position + new Vector3(0.0f, 6f, 0.0f);
          }
          this.PlayerManager.Action = ActionType.Standing;
          this.PlayerManager.Velocity = Vector3.Zero;
          this.CameraManager.Center = this.PlayerManager.Position + new Vector3(0.0f, 4.5f, 0.0f);
          this.CameraManager.SnapInterpolation();
          this.GameState.SkipRendering = false;
          float num7 = Easing.EaseIn((double) Easing.EaseOut((double) FezMath.Saturate(this.PhaseTime / 5f), EasingType.Sine), EasingType.Sextic);
          this.SetHexVisible((double) num7 - (double) this.lastStep > 0.00825);
          this.lastStep = num7;
          this.SolidCubes.Rotation = this.WhiteCube.Rotation = this.HexahedronAo.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) ((double) num7 * 6.28318548202515 * 4.0)) * this.OriginalCubeRotation;
          if ((double) this.PhaseTime <= 5.0)
            break;
          this.eMotor = SoundEffectExtensions.Emit(this.sMotorSpin2);
          this.ChangePhase();
          break;
        case FinalRebuildHost.Phases.MotorStart2:
          this.GameState.SkipRendering = true;
          if (gameTime.ElapsedGameTime.Ticks == 0L)
          {
            this.HexahedronAo.Position = this.SolidCubes.Position;
            this.lastStep = 0.0f;
          }
          this.PlayerManager.Action = ActionType.Standing;
          this.PlayerManager.Velocity = Vector3.Zero;
          this.CameraManager.Center = this.PlayerManager.Position + new Vector3(0.0f, 4.5f, 0.0f);
          this.CameraManager.SnapInterpolation();
          this.GameState.SkipRendering = false;
          float num8 = Easing.EaseIn((double) Easing.EaseOut((double) FezMath.Saturate(this.PhaseTime / 4f), EasingType.Sine), EasingType.Sextic);
          float num9 = num8 - this.lastStep;
          this.SetHexVisible((double) num9 > 0.01);
          this.lastStep = num8;
          if (this.GameState.SaveData.SecretCubes + this.GameState.SaveData.CubeShards < 64)
          {
            this.Glitches.DisappearProbability = 0.05f;
            float num5 = Easing.EaseIn((double) num9 / 0.00999999977648258, EasingType.Quartic);
            this.Glitches.ActiveGlitches = FezMath.Round((double) num5 * 7.0 + (double) (int) RandomHelper.Between(0.0, (double) num5 * 10.0));
            this.Glitches.FreezeProbability = (float) ((double) num9 / 0.00999999977648258 * (1.0 / 400.0));
          }
          this.SolidCubes.Rotation = this.WhiteCube.Rotation = this.HexahedronAo.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) ((double) num8 * 6.28318548202515 * 5.0)) * this.OriginalCubeRotation;
          if ((double) this.PhaseTime <= 4.0 + (this.GameState.SaveData.SecretCubes + this.GameState.SaveData.CubeShards >= 64 ? 0.5 : 0.0))
            break;
          if (this.GameState.SaveData.SecretCubes + this.GameState.SaveData.CubeShards < 64)
          {
            this.eMotor = SoundEffectExtensions.Emit(this.sMotorSpinCrash);
            this.ChangePhase();
            break;
          }
          else
          {
            this.eMotor = SoundEffectExtensions.Emit(this.sMotorSpinAOK);
            this.ChangePhaseTo(FinalRebuildHost.Phases.SmoothStart);
            break;
          }
        case FinalRebuildHost.Phases.MotorStart3:
          this.GameState.SkipRendering = true;
          if (gameTime.ElapsedGameTime.Ticks == 0L)
            this.HexahedronAo.Position = this.SolidCubes.Position;
          this.PlayerManager.Action = ActionType.Standing;
          this.PlayerManager.Velocity = Vector3.Zero;
          this.CameraManager.Center = this.PlayerManager.Position + new Vector3(0.0f, 4.5f, 0.0f);
          this.CameraManager.SnapInterpolation();
          this.GameState.SkipRendering = false;
          float num10 = Easing.EaseIn((double) this.PhaseTime / 6.0, EasingType.Sextic);
          float num11 = Math.Min(num10 - this.lastStep, 0.05f);
          this.SetHexVisible((double) num11 > 1.0 / 80.0);
          this.lastStep = num10;
          this.Glitches.DisappearProbability = 0.0375f;
          float num12 = Easing.EaseIn((double) num11 / 0.0375000014901161, EasingType.Quartic);
          this.Glitches.ActiveGlitches = FezMath.Round((double) FezMath.Saturate(num12) * 500.0 + (double) (int) RandomHelper.Between(0.0, (double) FezMath.Saturate(num12) * 250.0));
          this.Glitches.FreezeProbability = Easing.EaseIn((double) num11 / 0.0500000007450581, EasingType.Quadratic) * 0.15f;
          this.SolidCubes.Rotation = this.WhiteCube.Rotation = this.HexahedronAo.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) ((double) num10 * 6.28318548202515 * 20.0)) * this.OriginalCubeRotation;
          if ((double) this.PhaseTime <= 8.0)
            break;
          this.ChangePhase();
          break;
        case FinalRebuildHost.Phases.Crash:
          this.Glitches.FreezeProbability = 1f;
          if ((double) this.PhaseTime <= 2.0)
            break;
          if (this.eAku != null)
            this.eAku.FadeOutAndDie(0.0f);
          if (this.eAmbient != null)
            this.eAmbient.FadeOutAndDie(0.0f, false);
          this.Glitches.ActiveGlitches = 0;
          this.Glitches.FreezeProbability = 0.0f;
          this.Glitches.DisappearProbability = 0.0f;
          this.GlitchReboot();
          break;
        case FinalRebuildHost.Phases.SmoothStart:
          this.GameState.SkipRendering = true;
          if (gameTime.ElapsedGameTime.Ticks == 0L)
          {
            this.HexahedronAo.Position = this.SolidCubes.Position;
            this.lastStep = 0.0f;
            for (int index = 0; index < this.SolidCubes.Groups.Count; ++index)
              this.SolidCubes.Groups[index].Enabled = true;
          }
          this.PlayerManager.Action = ActionType.Standing;
          this.PlayerManager.Velocity = Vector3.Zero;
          this.CameraManager.Center = this.PlayerManager.Position + new Vector3(0.0f, 4.5f, 0.0f);
          this.CameraManager.SnapInterpolation();
          this.GameState.SkipRendering = false;
          float num13 = Easing.EaseInOut((double) FezMath.Saturate(this.PhaseTime / 10f), EasingType.Quadratic);
          this.SetHexVisible((double) num13 > 0.425000011920929);
          this.lastStep = num13;
          this.SolidCubes.Rotation = this.WhiteCube.Rotation = this.HexahedronAo.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) ((double) num13 * 6.28318548202515 * 18.0)) * this.OriginalCubeRotation;
          if ((double) this.PhaseTime <= 10.0)
            break;
          this.eAku.FadeOutAndDie(2f);
          SoundEffectExtensions.Emit(this.sRayWhiteout);
          this.ChangePhase();
          break;
        case FinalRebuildHost.Phases.ShineReboot:
          this.GameState.SkipRendering = true;
          if (gameTime.ElapsedGameTime.Ticks == 0L)
          {
            this.HexahedronAo.Position = this.SolidCubes.Position;
            this.SetHexVisible(true);
            this.RaysMesh.ClearGroups();
            this.HexahedronAo.Rotation = this.OriginalCubeRotation;
            if (this.eAmbient != null)
              this.eAmbient.FadeOutAndDie(0.0f, false);
          }
          this.PlayerManager.Action = ActionType.Standing;
          this.PlayerManager.Velocity = Vector3.Zero;
          this.CameraManager.Center = this.PlayerManager.Position + new Vector3(0.0f, 4.5f, 0.0f);
          this.CameraManager.SnapInterpolation();
          this.GameState.SkipRendering = false;
          this.UpdateRays((float) gameTime.ElapsedGameTime.TotalSeconds);
          if ((double) this.PhaseTime <= 4.0)
            break;
          this.SmoothReboot();
          break;
      }
    }

    private void GlitchReboot()
    {
      ServiceHelper.AddComponent((IGameComponent) new Reboot(this.Game, "GOMEZ_HOUSE_END_32"));
      Waiters.Wait(0.100000001490116, (Action) (() =>
      {
        this.Destroy();
        this.Enabled = this.Visible = false;
      }));
      this.Enabled = false;
    }

    private void SmoothReboot()
    {
      ServiceHelper.AddComponent((IGameComponent) new Intro(this.Game)
      {
        Fake = true,
        FakeLevel = "GOMEZ_HOUSE_END_64",
        Glitch = false
      });
      Waiters.Wait(0.100000001490116, (Action) (() =>
      {
        this.Destroy();
        this.Enabled = this.Visible = false;
      }));
      this.Enabled = false;
    }

    private void SetHexVisible(bool visible)
    {
      if (this.eAku != null)
        this.eAku.VolumeFactor = visible ? 1f : 0.0f;
      if (this.eMotor != null)
        this.eMotor.VolumeFactor = visible ? 0.0f : 1f;
      if (this.eAmbient != null)
        this.eAmbient.VolumeFactor = visible ? 0.0f : 0.625f;
      this.HexahedronAo.Hidden = !visible;
      this.HexahedronAo.Visible = visible;
      this.HexahedronAo.ArtObject.Group.Enabled = visible;
    }

    private void ChangePhase()
    {
      this.PhaseTime = 0.0f;
      ++this.ActivePhase;
      this.Update(new GameTime());
    }

    private void ChangePhaseTo(FinalRebuildHost.Phases phase)
    {
      this.PhaseTime = 0.0f;
      this.ActivePhase = phase;
      this.Update(new GameTime());
    }

    private void UpdateRays(float elapsedSeconds)
    {
      bool flag = (double) this.PhaseTime > 1.5;
      this.MakeRay();
      if (flag)
        this.MakeRay();
      for (int i = this.RaysMesh.Groups.Count - 1; i >= 0; --i)
      {
        Group group = this.RaysMesh.Groups[i];
        DotHost.RayState rayState = group.CustomData as DotHost.RayState;
        rayState.Age += elapsedSeconds * 0.15f;
        group.Material.Diffuse = Vector3.One * FezMath.Saturate(rayState.Age * 8f);
        group.Scale *= new Vector3(1.5f, 1f, 1f);
        if ((double) rayState.Age > 1.0)
          this.RaysMesh.RemoveGroupAt(i);
      }
      this.RaysMesh.AlwaysOnTop = false;
      this.FlareMesh.Position = this.RaysMesh.Position = this.HexahedronAo.Position;
      this.FlareMesh.Rotation = this.RaysMesh.Rotation = this.CameraManager.Rotation;
      this.FlareMesh.Material.Opacity = Easing.EaseIn((double) FezMath.Saturate(this.PhaseTime / 2.5f), EasingType.Cubic);
      this.FlareMesh.Scale = Vector3.One + this.RaysMesh.Scale * Easing.EaseIn(((double) this.PhaseTime - 0.25) / 1.75, EasingType.Decic) * 4f;
    }

    private void MakeRay()
    {
      if (this.RaysMesh.Groups.Count >= 150 || !RandomHelper.Probability(0.1 + (double) Easing.EaseIn((double) FezMath.Saturate(this.PhaseTime / 1.75f), EasingType.Sine) * 0.9))
        return;
      float num = RandomHelper.Probability(0.75) ? 0.1f : 0.4f;
      Group group = this.RaysMesh.AddGroup();
      group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(new FezVertexPositionColor[6]
      {
        new FezVertexPositionColor(new Vector3(0.0f, (float) ((double) num / 2.0 * 0.5), 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(1f, num / 2f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(1f, (float) ((double) num / 2.0 * 0.5), 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(1f, (float) (-(double) num / 2.0 * 0.5), 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(1f, (float) (-(double) num / 2.0), 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(0.0f, (float) (-(double) num / 2.0 * 0.5), 0.0f), Color.White)
      }, new int[12]
      {
        0,
        1,
        2,
        0,
        2,
        5,
        5,
        2,
        3,
        5,
        3,
        4
      }, PrimitiveType.TriangleList);
      group.CustomData = (object) new DotHost.RayState();
      group.Material = new Material()
      {
        Diffuse = new Vector3(0.0f)
      };
      group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, RandomHelper.Between(0.0, -3.14159274101257)) * Quaternion.CreateFromAxisAngle(Vector3.Forward, RandomHelper.Between(0.0, 6.28318548202515));
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
      {
        if (this.RtHandle == null || !this.TargetRenderer.IsHooked(this.RtHandle.Target))
          return;
        this.TargetRenderer.Resolve(this.RtHandle.Target, true);
        this.TargetRenderer.DrawFullscreen(Color.Black);
      }
      else
      {
        switch (this.ActivePhase)
        {
          case FinalRebuildHost.Phases.ZoomInNega:
            this.TargetRenderer.DrawFullscreen(new Color(0.0f, 0.0f, 0.0f, 1f - Easing.EaseOut((double) FezMath.Saturate(this.PhaseTime / 10f), EasingType.Quadratic)));
            this.TargetRenderer.Resolve(this.RtHandle.Target, true);
            this.TargetRenderer.DrawFullscreen((BaseEffect) this.InvertEffect, (Texture) this.RtHandle.Target);
            break;
          case FinalRebuildHost.Phases.FlickerIn:
            float num = Easing.EaseIn((double) FezMath.Saturate(this.PhaseTime / 1.25f), EasingType.Quadratic);
            if (RandomHelper.Probability(0.5))
              this.WhiteCube.Material.Diffuse = new Vector3((float) FezMath.AsNumeric(RandomHelper.Probability((double) num)), (float) FezMath.AsNumeric(RandomHelper.Probability((double) num)), (float) FezMath.AsNumeric(RandomHelper.Probability((double) num)));
            this.WhiteCube.Enabled = true;
            this.WhiteCube.Draw();
            this.TargetRenderer.Resolve(this.RtHandle.Target, true);
            this.TargetRenderer.DrawFullscreen((BaseEffect) this.InvertEffect, (Texture) this.RtHandle.Target);
            break;
          case FinalRebuildHost.Phases.SpinFill:
            this.WhiteCube.Draw();
            this.SolidCubes.Draw();
            this.TargetRenderer.Resolve(this.RtHandle.Target, true);
            this.TargetRenderer.DrawFullscreen((BaseEffect) this.InvertEffect, (Texture) this.RtHandle.Target);
            break;
          case FinalRebuildHost.Phases.MotorStart1:
          case FinalRebuildHost.Phases.MotorStart2:
          case FinalRebuildHost.Phases.MotorStart3:
          case FinalRebuildHost.Phases.SmoothStart:
            if (!this.HexahedronAo.Visible)
            {
              this.WhiteCube.Draw();
              this.SolidCubes.Draw();
              if (this.TargetRenderer.IsHooked(this.RtHandle.Target))
              {
                this.TargetRenderer.Resolve(this.RtHandle.Target, true);
                this.TargetRenderer.DrawFullscreen((BaseEffect) this.InvertEffect, (Texture) this.RtHandle.Target);
                break;
              }
              else
              {
                this.TargetRenderer.ScheduleHook(this.DrawOrder, this.RtHandle.Target);
                break;
              }
            }
            else
            {
              if (!this.TargetRenderer.IsHooked(this.RtHandle.Target))
                break;
              this.TargetRenderer.Resolve(this.RtHandle.Target, false);
              this.TargetRenderer.DrawFullscreen((Texture) this.RtHandle.Target);
              break;
            }
          case FinalRebuildHost.Phases.ShineReboot:
            this.RaysMesh.Draw();
            this.FlareMesh.Draw();
            break;
        }
      }
    }

    private enum Phases
    {
      ZoomInNega,
      FlickerIn,
      SpinFill,
      MotorStart1,
      MotorStart2,
      MotorStart3,
      Crash,
      SmoothStart,
      ShineReboot,
    }
  }
}
