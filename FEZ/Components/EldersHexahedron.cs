// Type: FezGame.Components.EldersHexahedron
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Components.Actions;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class EldersHexahedron : DrawableGameComponent
  {
    private static readonly string[] HexStrings = new string[8]
    {
      "HEX_A",
      "HEX_B",
      "HEX_C",
      "HEX_D",
      "HEX_E",
      "HEX_F",
      "HEX_G",
      "HEX_H"
    };
    private readonly Texture2D[] MatrixWords = new Texture2D[8];
    private readonly ArtObjectInstance AoInstance;
    private Vector3 Origin;
    private Vector3 CameraOrigin;
    private Vector3 GomezOrigin;
    private Quaternion AoRotationOrigin;
    private EldersHexahedron.StarfieldRenderer SfRenderer;
    private StarField Starfield;
    private PlaneParticleSystem Particles;
    private Mesh BeamMesh;
    private Mesh BeamMask;
    private Mesh SolidCubes;
    private Mesh SmallCubes;
    private Mesh MatrixMesh;
    private Mesh RaysMesh;
    private Mesh FlareMesh;
    private ArtObjectInstance TinyChapeau;
    private BackgroundPlane[] StatuePlanes;
    private NesGlitches Glitches;
    private Mesh DealGlassesPlane;
    private Mesh TrialRaysMesh;
    private Mesh TrialFlareMesh;
    private float TrialTimeAccumulator;
    private SoundEffect sCollectFez;
    private SoundEffect sHexaTalk;
    private SoundEffect sAmbientHex;
    private SoundEffect sBeamGrow;
    private SoundEffect sExplode;
    private SoundEffect sGomezBeamUp;
    private SoundEffect sTinyBeam;
    private SoundEffect sMatrixRampUp;
    private SoundEffect sHexSlowDown;
    private SoundEffect sRayExplosion;
    private SoundEffect sHexRise;
    private SoundEffect sGomezBeamAppear;
    private SoundEffect sNightTransition;
    private SoundEffect sHexAlign;
    private SoundEffect sStarTrails;
    private SoundEffect sWhiteOut;
    private SoundEffect sHexDisappear;
    private SoundEffect sSparklyParticles;
    private SoundEffect sTrialWhiteOut;
    private SoundEffect[] sHexDrones;
    private SoundEmitter eAmbientHex;
    private SoundEmitter eSparklyParticles;
    private SoundEmitter eHexaTalk;
    private SoundEmitter eHexDrone;
    private int currentDroneIndex;
    private float SpinSpeed;
    private float DestinationSpinSpeed;
    private float SincePhaseStarted;
    private float LastPhaseRadians;
    private float OriginalSpin;
    private float CameraSpinSpeed;
    private float CameraSpins;
    private float DestinationSpins;
    private float ExplodeSpeed;
    private EldersHexahedron.Phase CurrentPhase;
    private Quaternion RotationFrom;
    private float WhiteOutFactor;
    private bool playedRise1;

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IPlaneParticleSystems PlaneParticleSystems { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IArtObjectService ArtObjectService { get; set; }

    [ServiceDependency]
    public IGameService GameService { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IInputManager InputManager { get; set; }

    [ServiceDependency]
    public ISpeechBubbleManager SpeechBubble { get; set; }

    [ServiceDependency(Optional = true)]
    public IWalkToService WalkTo { protected get; set; }

    static EldersHexahedron()
    {
    }

    public EldersHexahedron(Game game, ArtObjectInstance aoInstance)
      : base(game)
    {
      this.UpdateOrder = 20;
      this.DrawOrder = 101;
      this.AoInstance = aoInstance;
    }

    public override void Initialize()
    {
      base.Initialize();
      ServiceHelper.AddComponent((IGameComponent) (this.Starfield = new StarField(this.Game)
      {
        Opacity = 0.0f,
        HasHorizontalTrails = true,
        FollowCamera = true
      }));
      ServiceHelper.AddComponent((IGameComponent) (this.SfRenderer = new EldersHexahedron.StarfieldRenderer(this.Game, this)));
      this.StatuePlanes = Enumerable.ToArray<BackgroundPlane>(Enumerable.Where<BackgroundPlane>((IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values, (Func<BackgroundPlane, bool>) (x => x.Id >= 0)));
      this.DealGlassesPlane = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        DepthWrites = false,
        AlwaysOnTop = false,
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures" + (this.GameState.SaveData.Finished64 ? "/deal_with_3d" : "/deal_with_it"))),
        SamplerState = SamplerState.PointClamp
      };
      this.DealGlassesPlane.AddFace(new Vector3(1f, 0.25f, 1f), Vector3.Zero, FaceOrientation.Right, true, true);
      ArtObject artObject = this.CMProvider.CurrentLevel.Load<ArtObject>("Art Objects/TINY_CHAPEAUAO");
      int key = IdentifierPool.FirstAvailable<ArtObjectInstance>(this.LevelManager.ArtObjects);
      this.TinyChapeau = new ArtObjectInstance(artObject)
      {
        Id = key
      };
      this.LevelManager.ArtObjects.Add(key, this.TinyChapeau);
      this.TinyChapeau.Initialize();
      this.TinyChapeau.Hidden = true;
      this.TinyChapeau.ArtObject.Group.Position = new Vector3(-0.125f, 0.375f, -0.125f);
      this.TinyChapeau.ArtObject.Group.BakeTransformInstanced<VertexPositionNormalTextureInstance, Matrix>();
      this.BeamMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        DepthWrites = false,
        AlwaysOnTop = false,
        Material = {
          Diffuse = new Vector3(221f, 178f, (float) byte.MaxValue) / (float) byte.MaxValue
        }
      };
      this.BeamMesh.AddFace(new Vector3(1f, 1f, 1f), Vector3.Zero, FaceOrientation.Right, true, true).Texture = (Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/VerticalGradient");
      Group group = this.BeamMesh.AddFace(new Vector3(2f, 1f, 2f), Vector3.Zero, FaceOrientation.Right, true, true);
      group.Texture = (Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/HorizontalGradient");
      group.Material = new Material()
      {
        Opacity = 0.4f,
        Diffuse = new Vector3(221f, 178f, (float) byte.MaxValue) / (float) byte.MaxValue
      };
      group.Enabled = false;
      this.BeamMask = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        DepthWrites = false,
        AlwaysOnTop = false
      };
      this.BeamMask.AddFace(new Vector3(1f, 1f, 1f), Vector3.Zero, FaceOrientation.Right, true, true);
      this.MatrixMesh = new Mesh()
      {
        Effect = (BaseEffect) new MatrixEffect(),
        DepthWrites = false,
        AlwaysOnTop = false,
        Blending = new BlendingMode?(BlendingMode.Multiply2X)
      };
      this.RaysMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.VertexColored(),
        DepthWrites = false,
        AlwaysOnTop = false,
        Blending = new BlendingMode?(BlendingMode.Alphablending)
      };
      this.FlareMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        DepthWrites = false,
        AlwaysOnTop = false,
        SamplerState = SamplerState.LinearClamp,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/flare_alpha"))
      };
      this.FlareMesh.AddFace(new Vector3(1f, 1f, 1f), Vector3.Zero, FaceOrientation.Right, true, true);
      this.TrialRaysMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.VertexColored(),
        Blending = new BlendingMode?(BlendingMode.Additive),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      if (this.GameState.IsTrialMode)
        this.TrialRaysMesh.Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/smooth_ray"));
      this.TrialFlareMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/flare_alpha")),
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      this.TrialFlareMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Right, true);
      this.LoadSounds();
      this.AoInstance.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 1.570796f) * Quaternion.CreateFromAxisAngle(Vector3.Right, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0))) * Quaternion.CreateFromAxisAngle(Vector3.Up, 0.7853982f);
      this.Origin = this.AoInstance.Position;
      this.AoRotationOrigin = Quaternion.CreateFromAxisAngle(Vector3.Forward, 0.7853982f) * Quaternion.CreateFromAxisAngle(Vector3.Up, 0.7853982f);
      this.AoInstance.Material = new Material();
      this.PlayerManager.Position = new Vector3(18.5f, 879.0 / 32.0, 34.5f);
      this.PlayerManager.Position = this.PlayerManager.Position * Vector3.UnitY + FezMath.XZMask * this.AoInstance.Position;
      this.GomezOrigin = this.PlayerManager.Position;
      this.CameraManager.Center = new Vector3(18.5f, 879.0 / 32.0, 34.5f);
      this.CameraManager.Center = this.CameraManager.Center * Vector3.UnitY + FezMath.XZMask * this.AoInstance.Position + new Vector3(0.0f, 4f, 0.0f);
      this.CameraManager.SnapInterpolation();
      this.CameraOrigin = this.CameraManager.Center;
      while (!this.PlayerManager.CanControl)
        this.PlayerManager.CanControl = true;
      this.PlayerManager.CanControl = false;
      this.CameraManager.Constrained = true;
      this.PlayerManager.HideFez = true;
      this.GenerateCubes();
      for (int index = 1; index < 9; ++index)
        this.MatrixWords[index - 1] = this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/zuish_matrix/" + (object) index);
      this.SpinSpeed = 225f;
      SoundEffectExtensions.Emit(this.sHexSlowDown);
      this.eAmbientHex = SoundEffectExtensions.Emit(this.sAmbientHex, true, 0.0f, 0.5f);
      Vector3 vector3 = -FezMath.ForwardVector(this.CameraManager.Viewpoint) * 4f;
      IPlaneParticleSystems planeParticleSystems = this.PlaneParticleSystems;
      EldersHexahedron eldersHexahedron = this;
      Game game = this.Game;
      int maximumCount = 200;
      PlaneParticleSystemSettings particleSystemSettings = new PlaneParticleSystemSettings();
      particleSystemSettings.SpawnVolume = new BoundingBox()
      {
        Min = this.PlayerManager.Position + new Vector3(-1f, 5f, -1f) + vector3,
        Max = this.PlayerManager.Position + new Vector3(1f, 20f, 1f) + vector3
      };
      particleSystemSettings.Velocity.Base = new Vector3(0.0f, -0.5f, 0.0f);
      particleSystemSettings.Velocity.Variation = new Vector3(0.0f, -0.25f, 0.1f);
      particleSystemSettings.SpawningSpeed = 12f;
      particleSystemSettings.ParticleLifetime = 15f;
      particleSystemSettings.Acceleration = -0.1f;
      particleSystemSettings.SizeBirth = (VaryingVector3) new Vector3(0.125f, 0.125f, 0.125f);
      particleSystemSettings.ColorBirth = (VaryingColor) Color.Black;
      particleSystemSettings.ColorLife.Base = new Color(0.6f, 0.6f, 0.6f, 1f);
      particleSystemSettings.ColorLife.Variation = new Color(0.1f, 0.1f, 0.1f, 0.0f);
      particleSystemSettings.ColorDeath = (VaryingColor) Color.Black;
      particleSystemSettings.FullBright = true;
      particleSystemSettings.RandomizeSpawnTime = true;
      particleSystemSettings.FadeInDuration = 0.25f;
      particleSystemSettings.FadeOutDuration = 0.5f;
      particleSystemSettings.Texture = this.CMProvider.Global.Load<Texture2D>("Background Planes/dust_particle");
      particleSystemSettings.BlendingMode = BlendingMode.Additive;
      PlaneParticleSystemSettings settings = particleSystemSettings;
      PlaneParticleSystem planeParticleSystem1;
      PlaneParticleSystem planeParticleSystem2 = planeParticleSystem1 = new PlaneParticleSystem(game, maximumCount, settings);
      eldersHexahedron.Particles = planeParticleSystem1;
      PlaneParticleSystem system = planeParticleSystem2;
      planeParticleSystems.Add(system);
      this.Particles.Enabled = false;
      this.LevelManager.LevelChanged += new Action(this.Kill);
    }

    private void LoadSounds()
    {
      this.sCollectFez = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Collects/CollectFez");
      this.sHexaTalk = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Npc/HexahedronTalk");
      this.sAmbientHex = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/AmbientHex");
      this.sBeamGrow = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/BeamGrow");
      this.sExplode = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/Explode");
      this.sGomezBeamUp = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/GomezBeamUp");
      this.sHexSlowDown = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/HexSlowDown");
      this.sMatrixRampUp = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/MatrixRampUp");
      this.sRayExplosion = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/RayExplosion");
      this.sTinyBeam = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/TinyBeam");
      this.sHexRise = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/HexRise");
      this.sGomezBeamAppear = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/GomezBeamAppear");
      this.sNightTransition = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/NightTransition");
      this.sHexAlign = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/HexAlign");
      this.sStarTrails = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/StarTrails");
      this.sWhiteOut = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/WhiteOut");
      this.sHexDisappear = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/HexDisappear");
      this.sSparklyParticles = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/SparklyParticles");
      this.sTrialWhiteOut = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ending/Pyramid/WhiteOut");
      this.sHexDrones = new SoundEffect[5]
      {
        this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/HexDrones/HexDrone1"),
        this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/HexDrones/HexDrone2"),
        this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/HexDrones/HexDrone3"),
        this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/HexDrones/HexDrone4"),
        this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/HexDrones/HexDrone5")
      };
    }

    private void GenerateCubes()
    {
      Vector3[] vector3Array = new Vector3[64];
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          for (int index3 = 0; index3 < 4; ++index3)
            vector3Array[index1 * 16 + index2 * 4 + index3] = new Vector3((float) index1 - 1.5f, (float) index2 - 1.5f, (float) index3 - 1.5f);
        }
      }
      EldersHexahedron eldersHexahedron = this;
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
      eldersHexahedron.SolidCubes = mesh3;
      this.SmallCubes = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.LitTextured()
        {
          Specular = true
        },
        Blending = new BlendingMode?(BlendingMode.Opaque)
      };
      this.SmallCubes.Rotation = this.SolidCubes.Rotation = this.AoRotationOrigin;
      Trile trile1 = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.CubeShard));
      Trile trile2 = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.GoldenCube));
      foreach (Vector3 vector3 in vector3Array)
      {
        Group group1 = this.SolidCubes.AddGroup();
        group1.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(Enumerable.ToArray<VertexPositionNormalTextureInstance>((IEnumerable<VertexPositionNormalTextureInstance>) trile1.Geometry.Vertices), trile1.Geometry.Indices, trile1.Geometry.PrimitiveType);
        group1.Position = vector3;
        group1.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, (float) RandomHelper.Random.Next(0, 4) * 1.570796f);
        group1.BakeTransformWithNormal<VertexPositionNormalTextureInstance>();
        group1.CustomData = (object) new EldersHexahedron.ShardProjectionData()
        {
          Direction = (vector3 * RandomHelper.Between(0.5, 5.0)),
          Spin = Quaternion.CreateFromAxisAngle(RandomHelper.NormalizedVector(), RandomHelper.Between(0.0, Math.PI / 1000.0))
        };
        Group group2 = this.SmallCubes.AddGroup();
        group2.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(Enumerable.ToArray<VertexPositionNormalTextureInstance>((IEnumerable<VertexPositionNormalTextureInstance>) trile2.Geometry.Vertices), trile2.Geometry.Indices, trile2.Geometry.PrimitiveType);
        group2.Position = vector3 * RandomHelper.Between(0.5, 1.0);
        group2.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, (float) RandomHelper.Random.Next(0, 4) * 1.570796f);
        group2.BakeTransformWithNormal<VertexPositionNormalTextureInstance>();
        group2.CustomData = (object) new EldersHexahedron.ShardProjectionData()
        {
          Direction = (vector3 * RandomHelper.Between(0.5, 5.0)),
          Spin = Quaternion.CreateFromAxisAngle(RandomHelper.NormalizedVector(), RandomHelper.Between(0.0, Math.PI / 1000.0))
        };
      }
      this.SolidCubes.Texture = this.SmallCubes.Texture = (Dirtyable<Texture>) ((Texture) this.LevelManager.TrileSet.TextureAtlas);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      ServiceHelper.RemoveComponent<StarField>(this.Starfield);
      ServiceHelper.RemoveComponent<EldersHexahedron.StarfieldRenderer>(this.SfRenderer);
      ServiceHelper.RemoveComponent<NesGlitches>(this.Glitches);
      this.SolidCubes.Dispose();
      this.SmallCubes.Dispose();
      this.MatrixMesh.Dispose();
      this.BeamMask.Dispose();
      this.BeamMesh.Dispose();
      this.TrialRaysMesh.Dispose();
      this.TrialFlareMesh.Dispose();
      this.FlareMesh.Dispose();
      this.RaysMesh.Dispose();
      this.DealGlassesPlane.Dispose();
      this.GameState.SkyOpacity = 1f;
      this.Visible = this.Enabled = false;
      this.LevelManager.LevelChanged -= new Action(this.Kill);
    }

    public override void Update(GameTime gameTime)
    {
      // ISSUE: unable to decompile the method.
    }

    private void WaitSpin(float elapsedTime)
    {
      if ((double) this.SincePhaseStarted < 3.0 && (double) this.SincePhaseStarted > 1.0)
      {
        this.CameraManager.PixelsPerTrixel = MathHelper.Lerp(10f, 2f, Easing.EaseInOut((double) FezMath.Saturate((float) (((double) this.SincePhaseStarted - 1.0) / 2.0)), EasingType.Sine));
        this.CameraManager.Center = Vector3.Lerp(this.CameraOrigin - new Vector3(0.0f, 3f, 0.0f), this.CameraOrigin + new Vector3(0.0f, 4f, 0.0f), Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.SincePhaseStarted - 1.0) / 2.0)), EasingType.Sine));
        this.AoInstance.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 1.570796f) * Quaternion.CreateFromAxisAngle(Vector3.Right, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0))) * Quaternion.CreateFromAxisAngle(Vector3.Up, 0.7853982f);
        this.CameraManager.SnapInterpolation();
        this.AoInstance.Visible = false;
        this.PlayerManager.CanRotate = true;
      }
      if ((double) this.SincePhaseStarted < 2.0)
        return;
      float num1 = Easing.EaseInOut((double) FezMath.Saturate(this.SincePhaseStarted - 3f), EasingType.Sine);
      this.AoInstance.Material.Opacity = num1;
      if (this.eHexDrone != null)
        this.eHexDrone.VolumeFactor = num1;
      this.AoInstance.Hidden = false;
      this.AoInstance.Visible = true;
      this.AoInstance.MarkDirty();
      if ((double) Math.Abs(this.SpinSpeed) < (double) Math.Abs(this.DestinationSpinSpeed))
      {
        this.SpinSpeed *= 1f + elapsedTime;
        if ((double) Math.Abs(this.SpinSpeed) > (double) Math.Abs(this.DestinationSpinSpeed))
          this.SpinSpeed = this.DestinationSpinSpeed;
      }
      if ((double) Math.Abs(this.SpinSpeed) > 32.0)
      {
        this.DestinationSpinSpeed *= 1f + elapsedTime;
        if (!this.GameState.IsTrialMode)
        {
          this.UpdateRays(elapsedTime * 2f);
          if ((double) Math.Abs(this.SpinSpeed) > 50.0)
          {
            this.UpdateRays(elapsedTime);
            this.UpdateRays(elapsedTime);
          }
        }
      }
      float num2 = Easing.EaseIn((double) this.SpinSpeed / 100.0, EasingType.Quadratic);
      IGameCameraManager cameraManager = this.CameraManager;
      Vector3 vector3 = cameraManager.InterpolatedCenter + new Vector3(RandomHelper.Between(-(double) num2, (double) num2), RandomHelper.Between(-(double) num2, (double) num2), RandomHelper.Between(-(double) num2, (double) num2));
      cameraManager.InterpolatedCenter = vector3;
      if (!this.GameState.IsTrialMode && (double) Math.Abs(this.SpinSpeed) > 30.0)
      {
        this.Glitches.DisappearProbability = FezMath.Saturate((float) (1.0 - ((double) Math.Abs(this.SpinSpeed) - 30.0) / 98.0)) * 0.1f;
        this.Glitches.ActiveGlitches = FezMath.Round((double) FezMath.Saturate((float) (((double) Math.Abs(this.SpinSpeed) - 30.0) / 98.0)) * 75.0);
        this.Glitches.FreezeProbability = Easing.EaseIn((double) FezMath.Saturate((float) (((double) Math.Abs(this.SpinSpeed) - 30.0) / 98.0)), EasingType.Cubic) * 0.01f;
      }
      if (this.CameraManager.Viewpoint != Viewpoint.Perspective && !this.CameraManager.ProjectionTransition)
      {
        this.AoInstance.Position = this.CameraManager.Center + new Vector3(0.0f, (float) Math.Sin((double) this.SincePhaseStarted / 2.0 + (double) this.LastPhaseRadians) * 0.25f, 0.0f);
        this.AoInstance.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, this.SpinSpeed * 0.01f) * this.AoInstance.Rotation;
      }
      if ((double) Math.Abs(this.SpinSpeed) < 105.0)
        return;
      if (this.PlayerManager.CanControl)
        ServiceHelper.AddComponent((IGameComponent) new ScreenFade(this.Game)
        {
          FromColor = Color.White,
          ToColor = ColorEx.TransparentWhite,
          Duration = 0.75f
        });
      this.PlayerManager.CanControl = false;
      this.CameraManager.ViewpointChanged -= new Action(this.AddSpin);
      this.ExplodeSpeed = 1.0 / 16.0;
      this.CameraSpinSpeed = 1E-05f;
      this.CameraSpins = 0.0f;
      Vector3 position = this.CameraManager.Position;
      Vector3 center = this.CameraManager.Center;
      this.OriginalSpin = (float) Math.Atan2((double) position.X - (double) center.X, (double) position.Z - (double) center.Z);
      this.ScheduleExplode();
    }

    private void HexaExplode(float elapsedTime)
    {
      this.AoInstance.Hidden = true;
      this.AoInstance.Visible = false;
      this.SmallCubes.Position = this.SolidCubes.Position = this.AoInstance.Position;
      if ((double) this.SincePhaseStarted > 0.25)
      {
        double num = (double) this.SincePhaseStarted / 13.0;
        if (this.sExplode != null)
        {
          SoundEffectExtensions.Emit(this.sExplode);
          this.sExplode = (SoundEffect) null;
        }
        this.ExplodeSpeed *= 0.95f;
        foreach (Group group in this.SolidCubes.Groups)
        {
          group.Position += ((EldersHexahedron.ShardProjectionData) group.CustomData).Direction * (float) (0.00499999988824129 + (double) this.ExplodeSpeed + (double) this.CameraSpinSpeed / 3.0);
          group.Rotation *= ((EldersHexahedron.ShardProjectionData) group.CustomData).Spin;
        }
        foreach (Group group in this.SmallCubes.Groups)
        {
          group.Position += ((EldersHexahedron.ShardProjectionData) group.CustomData).Direction * (float) (0.00499999988824129 + (double) this.ExplodeSpeed + (double) this.CameraSpinSpeed / 3.0);
          group.Rotation *= ((EldersHexahedron.ShardProjectionData) group.CustomData).Spin;
        }
      }
      this.CameraSpinSpeed *= 1.01f;
      if ((double) this.SincePhaseStarted < 10.0)
        this.CameraSpinSpeed = MathHelper.Min(this.CameraSpinSpeed, 0.004f);
      this.CameraSpins += this.CameraSpinSpeed;
      this.CameraSpins += this.ExplodeSpeed / 5f;
      this.CameraManager.Direction = Vector3.Normalize(new Vector3((float) Math.Sin((double) this.OriginalSpin + (double) this.CameraSpins * 6.28318548202515), 0.0f, (float) Math.Cos((double) this.OriginalSpin + (double) this.CameraSpins * 6.28318548202515)));
      this.CameraManager.SnapInterpolation();
      if (!this.GameState.IsTrialMode)
      {
        this.Glitches.ActiveGlitches = FezMath.Round((double) Easing.EaseIn((double) FezMath.Saturate(this.SincePhaseStarted / 13f), EasingType.Decic) * 400.0 + 2.0);
        this.Glitches.FreezeProbability = (double) this.SincePhaseStarted < 8.0 ? 0.0f : ((double) this.SincePhaseStarted < 10.0 ? 1.0 / 1000.0 : ((double) this.SincePhaseStarted < 11.0 ? 0.1f : 0.01f));
        if ((double) this.SincePhaseStarted > 13.0)
          this.Glitches.FreezeProbability = 1f;
      }
      if (this.GameState.IsTrialMode)
      {
        this.UpdateRays(elapsedTime * this.CameraSpinSpeed);
        this.UpdateRays(elapsedTime * this.CameraSpinSpeed);
      }
      else
      {
        for (int i = this.TrialRaysMesh.Groups.Count - 1; i >= 0; --i)
        {
          Group group = this.TrialRaysMesh.Groups[i];
          group.Material.Diffuse = new Vector3(FezMath.Saturate(1f - this.SincePhaseStarted));
          group.Scale *= new Vector3(1.5f, 1f, 1f);
          if (FezMath.AlmostEqual(group.Material.Diffuse.X, 0.0f))
            this.TrialRaysMesh.RemoveGroupAt(i);
        }
      }
      if (!this.GameState.IsTrialMode && (double) this.SincePhaseStarted > 15.0)
      {
        this.CurrentPhase = EldersHexahedron.Phase.ThatsIt;
        ServiceHelper.AddComponent((IGameComponent) new Reboot(this.Game, "GOMEZ_HOUSE"));
      }
      else
      {
        if (!this.GameState.IsTrialMode || (double) this.TrialTimeAccumulator <= 7.5)
          return;
        this.CurrentPhase = EldersHexahedron.Phase.ThatsIt;
        this.GameState.SkipLoadScreen = true;
        ServiceHelper.AddComponent((IGameComponent) new ScreenFade(this.Game)
        {
          FromColor = Color.White,
          ToColor = ColorEx.TransparentWhite,
          Duration = 2f
        });
        this.LevelManager.ChangeLevel("ARCH");
        Waiters.Wait((Func<bool>) (() => !this.GameState.Loading), (Action) (() =>
        {
          this.GameState.SkipLoadScreen = false;
          this.Visible = false;
          while (!this.PlayerManager.CanControl)
            this.PlayerManager.CanControl = true;
          this.SoundManager.MusicVolumeFactor = 1f;
        }));
      }
    }

    private void UpdateRays(float elapsedSeconds)
    {
      if (this.GameState.IsTrialMode)
      {
        if (this.TrialRaysMesh.Groups.Count < 50 && RandomHelper.Probability(0.25))
        {
          float x = 6f + RandomHelper.Centered(4.0);
          float num = RandomHelper.Between(0.5, (double) x / 2.5);
          Group group = this.TrialRaysMesh.AddGroup();
          group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionTexture>(new FezVertexPositionTexture[6]
          {
            new FezVertexPositionTexture(new Vector3(0.0f, (float) ((double) num / 2.0 * 0.100000001490116), 0.0f), new Vector2(0.0f, 0.0f)),
            new FezVertexPositionTexture(new Vector3(x, num / 2f, 0.0f), new Vector2(1f, 0.0f)),
            new FezVertexPositionTexture(new Vector3(x, (float) ((double) num / 2.0 * 0.100000001490116), 0.0f), new Vector2(1f, 0.45f)),
            new FezVertexPositionTexture(new Vector3(x, (float) (-(double) num / 2.0 * 0.100000001490116), 0.0f), new Vector2(1f, 0.55f)),
            new FezVertexPositionTexture(new Vector3(x, (float) (-(double) num / 2.0), 0.0f), new Vector2(1f, 1f)),
            new FezVertexPositionTexture(new Vector3(0.0f, (float) (-(double) num / 2.0 * 0.100000001490116), 0.0f), new Vector2(0.0f, 1f))
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
          group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Forward, RandomHelper.Between(0.0, 6.28318548202515));
        }
        for (int i = this.TrialRaysMesh.Groups.Count - 1; i >= 0; --i)
        {
          Group group = this.TrialRaysMesh.Groups[i];
          DotHost.RayState rayState = group.CustomData as DotHost.RayState;
          rayState.Age += elapsedSeconds * 0.15f;
          float num1 = Easing.EaseOut((double) Easing.EaseOut(Math.Sin((double) rayState.Age * 6.28318548202515 - 1.57079637050629) * 0.5 + 0.5, EasingType.Quintic), EasingType.Quintic);
          group.Material.Diffuse = Vector3.Lerp(Vector3.One, rayState.Tint.ToVector3(), 0.05f) * 0.15f * num1;
          float num2 = rayState.Speed;
          group.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Forward, (float) ((double) elapsedSeconds * (double) num2 * (0.100000001490116 + (double) Easing.EaseIn((double) this.TrialTimeAccumulator / 3.0, EasingType.Quadratic) * 0.200000002980232)));
          group.Scale = new Vector3((float) ((double) num1 * 0.75 + 0.25), (float) ((double) num1 * 0.5 + 0.5), 1f);
          if ((double) rayState.Age > 1.0)
            this.TrialRaysMesh.RemoveGroupAt(i);
        }
        this.TrialFlareMesh.Position = this.TrialRaysMesh.Position = this.AoInstance.Position;
        this.TrialFlareMesh.Rotation = this.TrialRaysMesh.Rotation = this.CameraManager.Rotation;
        this.TrialRaysMesh.Scale = new Vector3(Easing.EaseIn((double) this.TrialTimeAccumulator / 2.0, EasingType.Quadratic) + 1f);
        this.TrialFlareMesh.Material.Opacity = (float) (0.125 + (double) Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.TrialTimeAccumulator - 2.0) / 3.0)), EasingType.Cubic) * 0.875);
        this.TrialFlareMesh.Scale = Vector3.One + this.TrialRaysMesh.Scale * Easing.EaseIn((double) Math.Max(this.TrialTimeAccumulator - 2.5f, 0.0f) / 1.5, EasingType.Cubic) * 4f;
      }
      else
      {
        this.MakeRay();
        for (int index = this.TrialRaysMesh.Groups.Count - 1; index >= 0; --index)
        {
          Group group = this.TrialRaysMesh.Groups[index];
          DotHost.RayState rayState = group.CustomData as DotHost.RayState;
          rayState.Age += elapsedSeconds * 0.15f;
          group.Material.Diffuse = Vector3.One * FezMath.Saturate(rayState.Age * 8f);
          group.Scale *= new Vector3(2f, 1f, 1f);
        }
        this.TrialRaysMesh.AlwaysOnTop = false;
        this.TrialRaysMesh.Position = this.AoInstance.Position;
        this.TrialRaysMesh.Rotation = this.CameraManager.Rotation;
        this.TrialRaysMesh.Scale = new Vector3(Easing.EaseIn((double) this.TrialTimeAccumulator / 2.0, EasingType.Quadratic) + 1f);
      }
    }

    private void MakeRay()
    {
      if (this.TrialRaysMesh.Groups.Count >= 150 || !RandomHelper.Probability(0.25))
        return;
      float num = RandomHelper.Probability(0.75) ? 0.1f : 0.5f;
      Group group = this.TrialRaysMesh.AddGroup();
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

    private void ScheduleFades()
    {
      ServiceHelper.AddComponent((IGameComponent) new ScreenFade(this.Game)
      {
        FromColor = ColorEx.TransparentWhite,
        ToColor = Color.White,
        Duration = 0.5f,
        EasingType = EasingType.Quintic,
        Faded = (Action) (() => ServiceHelper.AddComponent((IGameComponent) new ScreenFade(this.Game)
        {
          FromColor = Color.White,
          ToColor = ColorEx.TransparentWhite,
          Duration = 4f,
          EaseOut = true
        }))
      });
    }

    private void AddSpin()
    {
      if (this.CameraManager.Viewpoint == Viewpoint.Perspective || this.CameraManager.ProjectionTransition)
        return;
      this.DestinationSpinSpeed = Math.Abs(this.DestinationSpinSpeed);
      this.SpinSpeed = Math.Abs(this.SpinSpeed);
      if ((double) this.SpinSpeed < 3.0)
      {
        this.SpinSpeed += 3f;
        this.DestinationSpinSpeed += 4f;
      }
      else
        this.DestinationSpinSpeed *= 3f;
      ++this.currentDroneIndex;
      if (this.currentDroneIndex <= 4)
      {
        this.eHexDrone.FadeOutAndDie(0.5f);
        this.eHexDrone = SoundEffectExtensions.Emit(this.sHexDrones[this.currentDroneIndex], true, 0.0f, 0.0f);
        Waiters.Interpolate(0.5, (Action<float>) (s =>
        {
          if (this.eHexDrone == null || this.eHexDrone.Dead)
            return;
          this.eHexDrone.VolumeFactor = s;
        }));
      }
      else
        this.currentDroneIndex = 4;
      this.DestinationSpinSpeed *= (float) FezMath.GetDistance(this.CameraManager.LastViewpoint, this.CameraManager.Viewpoint);
      this.SpinSpeed *= (float) FezMath.GetDistance(this.CameraManager.LastViewpoint, this.CameraManager.Viewpoint);
    }

    private void AddSplodeBeam(bool fullForce)
    {
      this.AddSplodeBeamInternal(fullForce);
      if (!fullForce)
        return;
      this.AddSplodeBeamInternal(true);
    }

    private void AddSplodeBeamInternal(bool fullForce)
    {
      float num1 = RandomHelper.Between(0.0, 3.14159274101257);
      float num2 = RandomHelper.Between(0.0, 3.0 / 32.0);
      Group group = this.RaysMesh.AddColoredTriangle(Vector3.Zero, new Vector3(0.0f, (float) Math.Sin((double) num1 - (double) num2) * 55f, (float) Math.Cos((double) num1 - (double) num2) * 55f), new Vector3(0.0f, (float) Math.Sin((double) num1 + (double) num2) * 55f, (float) Math.Cos((double) num1 + (double) num2) * 55f), new Color(241, 23, 101), new Color(37, 22, 53), new Color(37, 22, 53));
      group.Material = new Material()
      {
        Opacity = 1f
      };
      group.CustomData = (object) new EldersHexahedron.RayData()
      {
        Sign = RandomHelper.Sign(),
        Speed = (RandomHelper.Between(0.5, 1.5) * (fullForce ? 6f : 3f))
      };
    }

    private void ScheduleText()
    {
      IWaiter waiter = Waiters.Wait(3.0, (Action) (() =>
      {
        this.GameService.ShowScroll(this.GameState.SaveData.IsNewGamePlus ? (this.GameState.SaveData.HasStereo3D ? "STEREO_INSTRUCTIONS" : "FPVIEW_INSTRUCTIONS") : "ROTATE_INSTRUCTIONS", 0.0f, true, false);
        this.PlayerManager.Action = ActionType.Idle;
      }));
      waiter.CustomPause = (Func<bool>) (() => this.GameState.InCutscene);
      waiter.AutoPause = true;
    }

    private void ScheduleExplode()
    {
      Waiters.Wait((Func<bool>) (() => this.PlayerManager.Grounded), (Action) (() =>
      {
        this.WalkTo.Destination = (Func<Vector3>) (() => this.PlayerManager.Position * Vector3.UnitY + this.Origin * FezMath.XZMask);
        this.WalkTo.NextAction = ActionType.LookingUp;
        this.PlayerManager.Action = ActionType.WalkingTo;
        this.CurrentPhase = EldersHexahedron.Phase.HexaExplode;
        this.SincePhaseStarted = 0.0f;
        if (this.GameState.IsTrialMode)
          Waiters.Wait(1.0, (Action) (() => SoundEffectExtensions.Emit(this.sTrialWhiteOut)));
        Waiters.Interpolate(0.5, (Action<float>) (s => this.eHexDrone.Pitch = FezMath.Saturate(s)), (Action) (() => this.eHexDrone.FadeOutAndDie(0.1f)));
      })).AutoPause = true;
    }

    private void Talk1()
    {
      Waiters.Interpolate(0.5, (Action<float>) (s => this.eAmbientHex.VolumeFactor = FezMath.Saturate((float) (0.25 * (1.0 - (double) s) + 0.25)) * 0.85f));
      this.Say(0, 4, (Action) (() =>
      {
        Waiters.Interpolate(0.5, (Action<float>) (s => this.eAmbientHex.VolumeFactor = FezMath.Saturate((float) (0.25 * (double) s + 0.25)) * 0.85f));
        SoundEffectExtensions.Emit(this.sNightTransition);
        this.SincePhaseStarted = 0.0f;
        this.CurrentPhase = EldersHexahedron.Phase.Beam;
        this.eHexaTalk.FadeOutAndDie(0.1f);
        this.eHexaTalk = (SoundEmitter) null;
        this.PlayerManager.CanControl = false;
      }));
    }

    private void Talk2()
    {
      Waiters.Interpolate(0.5, (Action<float>) (s => this.eAmbientHex.VolumeFactor = FezMath.Saturate((float) (0.25 * (1.0 - (double) s) + 0.25)) * 0.85f));
      this.Say(5, 7, (Action) (() =>
      {
        Waiters.Interpolate(0.5, (Action<float>) (s => this.eAmbientHex.VolumeFactor = FezMath.Saturate((float) (0.25 * (1.0 - (double) s))) * 0.85f));
        SoundEffectExtensions.Emit(this.sHexDisappear);
        this.SincePhaseStarted = 0.0f;
        this.CurrentPhase = EldersHexahedron.Phase.Disappear;
        this.eHexaTalk.FadeOutAndDie(0.1f);
        this.eHexaTalk = (SoundEmitter) null;
        this.PlayerManager.CanControl = false;
      }));
    }

    private void Say(int current, int stopAt, Action onEnded)
    {
      if (this.eHexaTalk == null || this.eHexaTalk.Dead)
        return;
      IWaiter w = Waiters.Wait(0.25, new Action(this.eHexaTalk.Cue.Resume));
      w.AutoPause = true;
      string stringRaw = GameText.GetStringRaw(EldersHexahedron.HexStrings[current]);
      IWaiter w2 = Waiters.Wait(0.25 + 0.100000001490116 * (double) stringRaw.Length, new Action(this.eHexaTalk.Cue.Pause));
      w2.AutoPause = true;
      this.ArtObjectService.Say(this.AoInstance.Id, stringRaw, true).Ended += (Action) (() =>
      {
        if (w.Alive)
          w.Cancel();
        if (w2.Alive)
        {
          if (this.eHexaTalk != null && !this.eHexaTalk.Dead && this.eHexaTalk.Cue.State != SoundState.Paused)
            this.eHexaTalk.Cue.Pause();
          w2.Cancel();
        }
        if (current == stopAt)
          onEnded();
        else
          this.Say(current + 1, stopAt, onEnded);
      });
    }

    private void Kill()
    {
      ServiceHelper.RemoveComponent<EldersHexahedron>(this);
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      if (this.CurrentPhase > EldersHexahedron.Phase.Talk1)
      {
        this.GameState.SkyOpacity = 1f - this.Starfield.Opacity;
        foreach (BackgroundPlane backgroundPlane in this.StatuePlanes)
          backgroundPlane.Opacity = Easing.EaseIn(1.0 - (double) this.Starfield.Opacity, EasingType.Quadratic);
        this.SfRenderer.Visible = true;
      }
      switch (this.CurrentPhase)
      {
        case EldersHexahedron.Phase.Beam:
          this.DrawBeamWithMask();
          break;
        case EldersHexahedron.Phase.MatrixSpin:
          this.DrawBeamWithMask();
          GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Additive);
          this.TargetRenderer.DrawFullscreen(new Color(this.WhiteOutFactor * 0.6f, this.WhiteOutFactor * 0.6f, this.WhiteOutFactor * 0.6f));
          GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
          break;
        case EldersHexahedron.Phase.Talk2:
          this.DrawBeamWithMask();
          break;
        case EldersHexahedron.Phase.Disappear:
          this.DrawBeamWithMask();
          break;
        case EldersHexahedron.Phase.FezBeamGrow:
        case EldersHexahedron.Phase.FezComeDown:
          this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
          this.BeamMesh.Draw();
          if (this.GameState.SaveData.IsNewGamePlus)
            this.DealGlassesPlane.Draw();
          this.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
          break;
        case EldersHexahedron.Phase.Beamsplode:
          if (this.GameState.SaveData.IsNewGamePlus)
            this.DealGlassesPlane.Draw();
          this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
          this.BeamMesh.Draw();
          this.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
          if ((double) this.SincePhaseStarted <= 0.25)
            break;
          this.RaysMesh.Draw();
          break;
        case EldersHexahedron.Phase.Yay:
          if (!this.GameState.SaveData.IsNewGamePlus)
            break;
          this.DealGlassesPlane.Draw();
          break;
        case EldersHexahedron.Phase.WaitSpin:
          this.TrialRaysMesh.Draw();
          break;
        case EldersHexahedron.Phase.HexaExplode:
          this.SolidCubes.Draw();
          this.SmallCubes.Draw();
          if (this.GameState.IsTrialMode)
          {
            this.TargetRenderer.DrawFullscreen(new Color(1f, 1f, 1f, FezMath.Saturate(Easing.EaseIn(((double) this.TrialTimeAccumulator - 6.0) / 1.0, EasingType.Quintic))));
            this.TrialFlareMesh.Draw();
          }
          this.TrialRaysMesh.Draw();
          break;
        case EldersHexahedron.Phase.ThatsIt:
          if (!this.GameState.IsTrialMode)
            break;
          this.TargetRenderer.DrawFullscreen(Color.White);
          break;
      }
    }

    private void DrawBeamWithMask()
    {
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.LightShaft));
      this.BeamMask.Draw();
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.NotEqual, StencilMask.LightShaft);
      this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
      this.BeamMesh.Draw();
      this.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
      this.MatrixMesh.Draw();
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
    }

    private enum Phase
    {
      ZoomOut,
      Talk1,
      Beam,
      MatrixSpin,
      Talk2,
      Disappear,
      FezBeamGrow,
      FezComeDown,
      Beamsplode,
      Yay,
      WaitSpin,
      HexaExplode,
      ThatsIt,
    }

    private struct RayData
    {
      public int Sign;
      public float Speed;
      public float SinceAlive;
    }

    private struct ShardProjectionData
    {
      public Vector3 Direction;
      public Quaternion Spin;
    }

    private class StarfieldRenderer : DrawableGameComponent
    {
      private readonly EldersHexahedron Host;

      public StarfieldRenderer(Game game, EldersHexahedron host)
        : base(game)
      {
        this.Host = host;
        this.DrawOrder = 0;
        this.Visible = false;
      }

      public override void Draw(GameTime gameTime)
      {
        this.Host.TargetRenderer.DrawFullscreen(new Color(0.1411765f, 0.0882353f, 0.2058824f, this.Host.Starfield.Opacity));
        this.Host.Starfield.Draw();
      }
    }
  }
}
