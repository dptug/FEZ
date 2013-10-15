// Type: FezGame.Components.LiquidHost
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  public class LiquidHost : DrawableGameComponent
  {
    public static readonly Dictionary<LiquidType, LiquidColorScheme> ColorSchemes = new Dictionary<LiquidType, LiquidColorScheme>((IEqualityComparer<LiquidType>) LiquidTypeComparer.Default)
    {
      {
        LiquidType.Water,
        new LiquidColorScheme()
        {
          LiquidBody = new Color(61, 117, 254),
          SolidOverlay = new Color(40, 76, 162),
          SubmergedFoam = new Color(91, 159, 254),
          EmergedFoam = new Color(175, 205, (int) byte.MaxValue)
        }
      },
      {
        LiquidType.Blood,
        new LiquidColorScheme()
        {
          LiquidBody = new Color(174, 26, 0),
          SolidOverlay = new Color(84, 0, 21),
          SubmergedFoam = new Color(230, 81, 55),
          EmergedFoam = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)
        }
      },
      {
        LiquidType.Sewer,
        new LiquidColorScheme()
        {
          LiquidBody = new Color(82, (int) sbyte.MaxValue, 57),
          SolidOverlay = new Color(32, 70, 49),
          SubmergedFoam = new Color(174, 196, 64),
          EmergedFoam = new Color(174, 196, 64)
        }
      },
      {
        LiquidType.Lava,
        new LiquidColorScheme()
        {
          LiquidBody = new Color(209, 0, 0),
          SolidOverlay = new Color(150, 0, 0),
          SubmergedFoam = new Color((int) byte.MaxValue, 0, 0),
          EmergedFoam = new Color((int) byte.MaxValue, 0, 0)
        }
      },
      {
        LiquidType.Purple,
        new LiquidColorScheme()
        {
          LiquidBody = new Color(194, 1, 171),
          SolidOverlay = new Color(76, 9, 103),
          SubmergedFoam = new Color(247, 52, 223),
          EmergedFoam = new Color(254, 254, 254)
        }
      },
      {
        LiquidType.Green,
        new LiquidColorScheme()
        {
          LiquidBody = new Color(47, (int) byte.MaxValue, 139),
          SolidOverlay = new Color(0, 167, 134),
          SubmergedFoam = new Color(0, 218, 175),
          EmergedFoam = new Color(184, 249, 207)
        }
      }
    };
    private float lastVisibleWaterHeight = -1f;
    private const int MinShoreSegmentWidth = 10;
    private const int MaxShoreSegmentWidth = 22;
    private const int ShoreThickness = 4;
    private const int ShoreActualTotalWidth = 48;
    private Mesh LiquidMesh;
    private Mesh FoamMesh;
    private Mesh RaysMesh;
    private Mesh CausticsMesh;
    private AnimatedTexture CausticsAnimation;
    private AnimationTiming BackgroundCausticsTiming;
    private float CausticsHeight;
    private PlaneParticleSystem BubbleSystem;
    private PlaneParticleSystem EmbersSystem;
    private SoundEmitter eSewageBubbling;
    private SoundEffect sSmallBubble;
    private SoundEffect sMidBubble;
    private SoundEffect sLargeBubble;
    private AnimatedTexture LargeBubbleAnim;
    private AnimatedTexture MediumBubbleAnim;
    private AnimatedTexture SmallBubbleAnim;
    private AnimatedTexture SmokeAnim;
    private TimeSpan TimeUntilBubble;
    private LiquidType? LastWaterType;
    private LiquidHost.WaterTransitionRenderer TransitionRenderer;
    private FoamEffect FoamEffect;
    private bool WaterVisible;
    private float WaterLevel;
    private Vector3 RightVector;
    private Quaternion CameraRotation;
    private Vector3 ScreenCenter;
    private float CameraRadius;
    private Vector3 CameraPosition;
    private Vector3 CameraInterpolatedCenter;
    private Vector3 ForwardVector;
    private float OriginalPixPerTrix;
    public static LiquidHost Instance;
    private float lastVariation;
    private TimeSpan accumulator;
    public bool ForcedUpdate;
    private float OriginalDistance;

    public static Func<Vector3, Vector3, Vector3> EmberScaling
    {
      get
      {
        return (Func<Vector3, Vector3, Vector3>) ((b, v) =>
        {
          if (RandomHelper.Probability(0.333))
            return new Vector3(0.125f, 1.0 / 16.0, 1f);
          if (RandomHelper.Probability(0.5))
            return new Vector3(1.0 / 16.0, 0.125f, 1f);
          else
            return new Vector3(1.0 / 16.0, 1.0 / 16.0, 1f);
        });
      }
    }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IPlaneParticleSystems PlaneParticleSystems { get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    [ServiceDependency]
    public IDotManager DotManager { get; set; }

    static LiquidHost()
    {
    }

    public LiquidHost(Game game)
      : base(game)
    {
      this.DrawOrder = 50;
      LiquidHost.Instance = this;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LightingPostProcess.DrawOnTopLights += new Action(this.DrawLights);
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.Visible = this.Enabled = false;
      this.lastVisibleWaterHeight = -1f;
    }

    private void TryInitialize()
    {
      this.GameState.WaterBodyColor = LiquidHost.ColorSchemes[LiquidType.Water].LiquidBody.ToVector3();
      this.GameState.WaterFoamColor = LiquidHost.ColorSchemes[LiquidType.Water].EmergedFoam.ToVector3();
      LiquidType waterType = this.LevelManager.WaterType;
      LiquidType? nullable = this.LastWaterType;
      if ((waterType != nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
      {
        this.CreateParticleSystems();
        this.ReestablishLiquidHeight();
        this.ReloadSounds();
        this.ForcedUpdate = true;
        this.Update(new GameTime());
        this.ForcedUpdate = false;
      }
      else
      {
        this.LastWaterType = new LiquidType?(this.LevelManager.WaterType);
        this.Visible = this.Enabled = this.LevelManager.WaterType != LiquidType.None;
        this.lastVisibleWaterHeight = -1f;
        this.ReestablishLiquidHeight();
        this.ReloadSounds();
        this.CreateFoam();
        this.CreateParticleSystems();
        if (this.Enabled)
        {
          LiquidColorScheme liquidColorScheme = LiquidHost.ColorSchemes[this.LevelManager.WaterType];
          this.GameState.WaterBodyColor = liquidColorScheme.LiquidBody.ToVector3();
          this.GameState.WaterFoamColor = liquidColorScheme.EmergedFoam.ToVector3();
          this.LiquidMesh.Groups[0].Material.Diffuse = liquidColorScheme.LiquidBody.ToVector3();
          this.LiquidMesh.Groups[1].Material.Diffuse = liquidColorScheme.SolidOverlay.ToVector3();
          this.FoamMesh.Groups[0].Material.Diffuse = liquidColorScheme.SubmergedFoam.ToVector3();
          if (this.FoamMesh.Groups.Count > 1)
            this.FoamMesh.Groups[1].Material.Diffuse = liquidColorScheme.EmergedFoam.ToVector3();
        }
        this.ForcedUpdate = true;
        this.Update(new GameTime());
        this.ForcedUpdate = false;
      }
    }

    private void ReloadSounds()
    {
      if (this.LevelManager.WaterType == LiquidType.Sewer)
        this.eSewageBubbling = SoundEffectExtensions.EmitAt(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Sewer/SewageBubbling"), this.CameraManager.InterpolatedCenter * FezMath.XZMask + this.LevelManager.WaterHeight * Vector3.UnitY, true);
      else if (this.eSewageBubbling != null && !this.eSewageBubbling.Dead)
        this.eSewageBubbling.Cue.Stop(false);
      if (this.LevelManager.WaterType == LiquidType.Lava)
      {
        this.sSmallBubble = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Lava/SmallBubble");
        this.sMidBubble = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Lava/MediumBubble");
        this.sLargeBubble = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Lava/LargeBubble");
      }
      else
        this.sSmallBubble = this.sMidBubble = this.sLargeBubble = (SoundEffect) null;
    }

    private void ReestablishLiquidHeight()
    {
      if (this.LevelManager.Name != null)
      {
        if (this.LevelManager.WaterType == LiquidType.Water)
        {
          this.LevelManager.OriginalWaterHeight = this.LevelManager.WaterHeight;
          if (this.GameState.SaveData.GlobalWaterLevelModifier.HasValue)
          {
            IGameLevelManager levelManager = this.LevelManager;
            double num = (double) levelManager.WaterHeight + (double) this.GameState.SaveData.GlobalWaterLevelModifier.Value;
            levelManager.WaterHeight = (float) num;
          }
        }
        else if (!this.GameState.SaveData.ThisLevel.LastStableLiquidHeight.HasValue)
          this.GameState.SaveData.ThisLevel.LastStableLiquidHeight = new float?(this.LevelManager.WaterHeight);
        else
          this.LevelManager.WaterHeight = this.GameState.SaveData.ThisLevel.LastStableLiquidHeight.Value;
      }
      if ((double) this.PlayerManager.Position.Y <= (double) this.LevelManager.WaterHeight)
        this.LevelManager.WaterHeight = this.PlayerManager.Position.Y - 1f;
      if (!(this.LevelManager.Name == "SEWER_START"))
        return;
      if ((double) this.LevelManager.WaterHeight > 21.75)
        this.LevelManager.WaterHeight = 24.5f;
      else
        this.LevelManager.WaterHeight = 19f;
    }

    public void StartTransition()
    {
      ServiceHelper.AddComponent((IGameComponent) (this.TransitionRenderer = new LiquidHost.WaterTransitionRenderer(this.Game, this)));
      this.WaterLevel = this.LevelManager.WaterHeight;
      this.RightVector = FezMath.RightVector(this.CameraManager.Viewpoint);
    }

    public void LockView()
    {
      this.TransitionRenderer.LockView();
    }

    public void EndTransition()
    {
      ServiceHelper.RemoveComponent<LiquidHost.WaterTransitionRenderer>(this.TransitionRenderer);
      this.TransitionRenderer = (LiquidHost.WaterTransitionRenderer) null;
      this.ForcedUpdate = true;
      this.Update(new GameTime());
      this.ForcedUpdate = false;
    }

    private void CreateFoam()
    {
      if (this.FoamMesh == null)
        this.FoamMesh = new Mesh()
        {
          AlwaysOnTop = true,
          DepthWrites = false,
          Blending = new BlendingMode?(BlendingMode.Alphablending),
          Culling = CullMode.CullClockwiseFace,
          Effect = (BaseEffect) (this.FoamEffect = new FoamEffect())
        };
      this.FoamMesh.ClearGroups();
      this.RaysMesh.ClearGroups();
      this.FoamMesh.Rotation = Quaternion.Identity;
      this.FoamMesh.Position = Vector3.Zero;
      if (this.LevelManager.WaterType == LiquidType.None)
        return;
      switch (this.LevelManager.WaterType)
      {
        case LiquidType.Water:
        case LiquidType.Blood:
        case LiquidType.Purple:
        case LiquidType.Green:
          this.FoamEffect.IsWobbling = true;
          float x1 = (float) RandomHelper.Random.Next(10, 22) / 16f;
          float x2 = -24f;
          while ((double) x2 < 24.0)
          {
            Group group = this.FoamMesh.AddFace(new Vector3(1f, 0.125f, 1f), Vector3.Zero, FaceOrientation.Back, Color.White, true);
            group.Position = new Vector3(0.5f, -1.0 / 16.0, 0.0f);
            group.BakeTransform<VertexPositionNormalColor>();
            IndexedUserPrimitives<VertexPositionNormalColor> indexedUserPrimitives = group.Geometry as IndexedUserPrimitives<VertexPositionNormalColor>;
            for (int index = 0; index < indexedUserPrimitives.Vertices.Length; ++index)
              indexedUserPrimitives.Vertices[index].Normal = new Vector3(x2, 0.0f, 0.0f);
            group.Scale = new Vector3(x1, 1f, 1f);
            x2 += x1;
          }
          Group group1 = this.FoamMesh.CollapseToBuffer<VertexPositionNormalColor>();
          group1.Material = new Material();
          group1.Position = new Vector3(0.0f, -1f, 0.0f);
          group1.CustomData = (object) true;
          float x3 = -24f;
          while ((double) x3 < 24.0)
          {
            Group group2 = this.FoamMesh.AddFace(new Vector3(1f, 0.125f, 1f), Vector3.Zero, FaceOrientation.Back, Color.White, true);
            group2.Position = new Vector3(0.5f, 1.0 / 16.0, 0.0f);
            group2.BakeTransform<VertexPositionNormalColor>();
            IndexedUserPrimitives<VertexPositionNormalColor> indexedUserPrimitives = group2.Geometry as IndexedUserPrimitives<VertexPositionNormalColor>;
            for (int index = 0; index < indexedUserPrimitives.Vertices.Length; ++index)
              indexedUserPrimitives.Vertices[index].Normal = new Vector3(x3, 0.0f, 0.0f);
            group2.Scale = new Vector3(x1, 1f, 1f);
            x3 += x1;
          }
          Group group3 = this.FoamMesh.CollapseToBuffer<VertexPositionNormalColor>(1, this.FoamMesh.Groups.Count - 1);
          group3.Material = new Material();
          group3.Position = new Vector3(0.0f, -1f, 0.0f);
          group3.CustomData = (object) false;
          break;
        case LiquidType.Lava:
        case LiquidType.Sewer:
          this.FoamEffect.IsWobbling = false;
          Group group4 = this.FoamMesh.AddFace(new Vector3(1f, 0.125f, 1f), Vector3.Zero, FaceOrientation.Back, Color.White, false);
          group4.Position = new Vector3(0.5f, -0.125f, 0.0f);
          group4.BakeTransform<VertexPositionNormalColor>();
          group4.Scale = new Vector3(100f, 0.5f, 1f);
          group4.Position = new Vector3(-100f, -1f, 0.0f);
          group4.Material = new Material();
          group4.CustomData = (object) true;
          break;
      }
    }

    private void CreateParticleSystems()
    {
      if (this.LevelManager.WaterType == LiquidType.None)
        return;
      LiquidColorScheme liquidColorScheme = LiquidHost.ColorSchemes[this.LevelManager.WaterType];
      switch (this.LevelManager.WaterType)
      {
        case LiquidType.Lava:
        case LiquidType.Sewer:
          Color color = new Color(liquidColorScheme.SubmergedFoam.ToVector3() * 0.5f);
          PlaneParticleSystemSettings particleSystemSettings1 = new PlaneParticleSystemSettings();
          particleSystemSettings1.Velocity = (VaryingVector3) new Vector3(0.0f, 0.15f, 0.0f);
          particleSystemSettings1.Gravity = new Vector3(0.0f, 0.0f, 0.0f);
          particleSystemSettings1.SpawningSpeed = 50f;
          particleSystemSettings1.ParticleLifetime = 2.2f;
          particleSystemSettings1.SpawnBatchSize = 1;
          PlaneParticleSystemSettings particleSystemSettings2 = particleSystemSettings1;
          VaryingVector3 varyingVector3_1 = new VaryingVector3();
          varyingVector3_1.Base = new Vector3(1.0 / 16.0);
          varyingVector3_1.Variation = new Vector3(1.0 / 16.0);
          varyingVector3_1.Function = VaryingVector3.Uniform;
          VaryingVector3 varyingVector3_2 = varyingVector3_1;
          particleSystemSettings2.SizeBirth = varyingVector3_2;
          PlaneParticleSystemSettings particleSystemSettings3 = particleSystemSettings1;
          VaryingVector3 varyingVector3_3 = new VaryingVector3();
          varyingVector3_3.Base = new Vector3(0.125f);
          varyingVector3_3.Variation = new Vector3(0.125f);
          varyingVector3_3.Function = VaryingVector3.Uniform;
          VaryingVector3 varyingVector3_4 = varyingVector3_3;
          particleSystemSettings3.SizeDeath = varyingVector3_4;
          particleSystemSettings1.FadeInDuration = 0.1f;
          particleSystemSettings1.FadeOutDuration = 0.1f;
          PlaneParticleSystemSettings particleSystemSettings4 = particleSystemSettings1;
          VaryingColor varyingColor1 = new VaryingColor();
          varyingColor1.Base = color;
          varyingColor1.Variation = color;
          varyingColor1.Function = VaryingColor.Uniform;
          VaryingColor varyingColor2 = varyingColor1;
          particleSystemSettings4.ColorLife = varyingColor2;
          particleSystemSettings1.Texture = this.CMProvider.Global.Load<Texture2D>("Background Planes/white_square");
          particleSystemSettings1.BlendingMode = BlendingMode.Alphablending;
          particleSystemSettings1.Billboarding = true;
          PlaneParticleSystemSettings settings1 = particleSystemSettings1;
          IPlaneParticleSystems planeParticleSystems1 = this.PlaneParticleSystems;
          LiquidHost liquidHost1 = this;
          PlaneParticleSystem planeParticleSystem1 = new PlaneParticleSystem(this.Game, 100, settings1);
          planeParticleSystem1.DrawOrder = this.DrawOrder + 1;
          PlaneParticleSystem planeParticleSystem2;
          PlaneParticleSystem planeParticleSystem3 = planeParticleSystem2 = planeParticleSystem1;
          liquidHost1.BubbleSystem = planeParticleSystem2;
          PlaneParticleSystem system1 = planeParticleSystem3;
          planeParticleSystems1.Add(system1);
          if (this.LevelManager.WaterType == LiquidType.Sewer)
            break;
          PlaneParticleSystemSettings particleSystemSettings5 = new PlaneParticleSystemSettings();
          PlaneParticleSystemSettings particleSystemSettings6 = particleSystemSettings5;
          VaryingVector3 varyingVector3_5 = new VaryingVector3();
          varyingVector3_5.Variation = new Vector3(1f);
          VaryingVector3 varyingVector3_6 = varyingVector3_5;
          particleSystemSettings6.Velocity = varyingVector3_6;
          particleSystemSettings5.Gravity = new Vector3(0.0f, 0.01f, 0.0f);
          particleSystemSettings5.SpawningSpeed = 40f;
          particleSystemSettings5.ParticleLifetime = 2f;
          particleSystemSettings5.SpawnBatchSize = 1;
          particleSystemSettings5.RandomizeSpawnTime = true;
          PlaneParticleSystemSettings particleSystemSettings7 = particleSystemSettings5;
          VaryingVector3 varyingVector3_7 = new VaryingVector3();
          varyingVector3_7.Function = LiquidHost.EmberScaling;
          VaryingVector3 varyingVector3_8 = varyingVector3_7;
          particleSystemSettings7.SizeBirth = varyingVector3_8;
          particleSystemSettings5.FadeInDuration = 0.15f;
          particleSystemSettings5.FadeOutDuration = 0.4f;
          particleSystemSettings5.ColorBirth = (VaryingColor) new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          particleSystemSettings5.ColorLife.Base = new Color((int) byte.MaxValue, 16, 16);
          particleSystemSettings5.ColorLife.Variation = new Color(0, 32, 32);
          particleSystemSettings5.ColorLife.Function = VaryingColor.Uniform;
          particleSystemSettings5.ColorDeath = (VaryingColor) new Color(0, 0, 0, 32);
          particleSystemSettings5.Texture = this.CMProvider.Global.Load<Texture2D>("Background Planes/white_square");
          particleSystemSettings5.BlendingMode = BlendingMode.Alphablending;
          particleSystemSettings5.Billboarding = true;
          PlaneParticleSystemSettings settings2 = particleSystemSettings5;
          IPlaneParticleSystems planeParticleSystems2 = this.PlaneParticleSystems;
          LiquidHost liquidHost2 = this;
          PlaneParticleSystem planeParticleSystem4 = new PlaneParticleSystem(this.Game, 50, settings2);
          planeParticleSystem4.DrawOrder = this.DrawOrder + 1;
          PlaneParticleSystem planeParticleSystem5;
          PlaneParticleSystem planeParticleSystem6 = planeParticleSystem5 = planeParticleSystem4;
          liquidHost2.EmbersSystem = planeParticleSystem5;
          PlaneParticleSystem system2 = planeParticleSystem6;
          planeParticleSystems2.Add(system2);
          break;
      }
    }

    protected override void LoadContent()
    {
      this.LiquidMesh = new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        Culling = CullMode.None,
        Effect = (BaseEffect) new DefaultEffect.VertexColored()
      };
      Group group1 = this.LiquidMesh.AddColoredBox(Vector3.One, Vector3.Zero, Color.White, true);
      group1.Position = new Vector3(0.0f, -0.5f, 0.0f);
      group1.BakeTransform<FezVertexPositionColor>();
      group1.Position = new Vector3(0.0f, -1f, 0.0f);
      group1.Scale = new Vector3(100f);
      group1.Material = new Material();
      Group group2 = this.LiquidMesh.AddColoredBox(Vector3.One, Vector3.Zero, Color.White, true);
      group2.Position = new Vector3(0.0f, -0.5f, 0.0f);
      group2.BakeTransform<FezVertexPositionColor>();
      group2.Position = new Vector3(0.0f, -1f, 0.0f);
      group2.Scale = new Vector3(100f);
      group2.Material = new Material();
      this.RaysMesh = new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false,
        Blending = new BlendingMode?(BlendingMode.Additive),
        Culling = CullMode.CullClockwiseFace,
        Effect = (BaseEffect) new DefaultEffect.VertexColored()
      };
      this.CausticsAnimation = this.CMProvider.Global.Load<AnimatedTexture>("Other Textures/FINAL_caustics");
      this.CausticsAnimation.Timing.Loop = true;
      this.BackgroundCausticsTiming = this.CausticsAnimation.Timing.Clone();
      this.BackgroundCausticsTiming.RandomizeStep();
      this.CausticsMesh = new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false,
        SamplerState = SamplerState.PointWrap,
        Effect = (BaseEffect) new CausticsEffect()
      };
      Group group3 = this.CausticsMesh.AddTexturedCylinder(Vector3.One, Vector3.Zero, 3, 4, false, false);
      group3.Material = new Material();
      group3.Texture = (Texture) this.CausticsAnimation.Texture;
      this.SmallBubbleAnim = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/lava/lava_a");
      this.MediumBubbleAnim = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/lava/lava_b");
      this.LargeBubbleAnim = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/lava/lava_c");
      this.SmokeAnim = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/lava/lava_smoke");
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading && !this.ForcedUpdate && this.TransitionRenderer == null || this.GameState.TimePaused && !this.ForcedUpdate)
        return;
      if (this.TransitionRenderer == null)
      {
        this.WaterLevel = this.LevelManager.WaterHeight;
        this.RightVector = this.CameraManager.InverseView.Right;
        this.CameraRotation = this.CameraManager.Rotation;
        this.ScreenCenter = this.CameraManager.Center;
        this.CameraRadius = this.CameraManager.Radius;
        this.CameraPosition = this.CameraManager.Position;
        this.CameraInterpolatedCenter = this.CameraManager.InterpolatedCenter;
        this.ForwardVector = this.CameraManager.InverseView.Forward;
        this.OriginalPixPerTrix = this.CameraManager.PixelsPerTrixel;
      }
      if (this.GameState.FarawaySettings.InTransition && (double) this.GameState.FarawaySettings.OriginFadeOutStep == 1.0 && (this.TransitionRenderer != null && !this.TransitionRenderer.ViewLocked))
      {
        this.LockView();
        this.CameraRadius = this.CameraManager.Radius;
        this.OriginalDistance = (float) ((double) this.WaterLevel - (double) this.CameraManager.InverseView.Translation.Y - 0.625);
      }
      float waterLevel1 = this.WaterLevel;
      if (this.TransitionRenderer != null && (double) this.GameState.FarawaySettings.OriginFadeOutStep == 1.0)
        waterLevel1 = (float) ((double) this.WaterLevel - (double) this.OriginalDistance + (double) this.OriginalDistance * ((double) this.CameraRadius / (double) MathHelper.Lerp(this.CameraRadius, this.GameState.FarawaySettings.DestinationRadius / 4f, this.GameState.FarawaySettings.TransitionStep)));
      this.FoamMesh.Rotation = this.CameraRotation;
      if (this.LevelManager.WaterType == LiquidType.Lava || this.LevelManager.WaterType == LiquidType.Sewer)
        this.BubbleSystem.Enabled = this.CameraManager.Viewpoint != Viewpoint.Perspective;
      if (this.LevelManager.WaterType == LiquidType.Lava)
        this.EmbersSystem.Enabled = this.CameraManager.Viewpoint != Viewpoint.Perspective;
      foreach (Group group in this.RaysMesh.Groups)
        group.Rotation = this.CameraRotation;
      float num1 = this.CameraPosition.Y - this.CameraRadius / 2f / this.CameraManager.AspectRatio;
      if (this.WaterVisible || (double) this.lastVisibleWaterHeight < (double) waterLevel1)
        this.lastVisibleWaterHeight = waterLevel1;
      if (this.LevelManager.WaterType == LiquidType.Sewer || this.LevelManager.WaterType == LiquidType.Lava)
      {
        this.BubbleSystem.Visible = (double) waterLevel1 > (double) num1;
        PlaneParticleSystem planeParticleSystem = this.BubbleSystem;
        int num2 = planeParticleSystem.Enabled & (double) waterLevel1 > (double) num1 ? 1 : 0;
        planeParticleSystem.Enabled = num2 != 0;
      }
      if (this.LevelManager.WaterType == LiquidType.Lava)
        this.EmbersSystem.Settings.SpawnVolume = new BoundingBox()
        {
          Min = this.ScreenCenter - new Vector3(this.CameraRadius / 2f),
          Max = this.ScreenCenter + new Vector3(this.CameraRadius / 2f / this.CameraManager.AspectRatio)
        };
      if (this.LevelManager.WaterType == LiquidType.Sewer)
        this.eSewageBubbling.Position = this.CameraInterpolatedCenter * FezMath.XZMask + waterLevel1 * Vector3.UnitY;
      this.WaterVisible = (double) this.lastVisibleWaterHeight > (double) num1 || this.TransitionRenderer != null;
      if (!this.WaterVisible && !this.ForcedUpdate)
      {
        if ((this.LevelManager.WaterType == LiquidType.Lava || this.LevelManager.WaterType == LiquidType.Sewer) && (double) this.lastVisibleWaterHeight != (double) waterLevel1)
          this.BubbleSystem.Clear();
        if (this.LevelManager.WaterType != LiquidType.Lava || this.GameState.Loading)
          return;
        this.SpawnBubbles(gameTime.ElapsedGameTime, waterLevel1, true);
      }
      else
      {
        this.accumulator += gameTime.ElapsedGameTime;
        if (this.accumulator.TotalSeconds > 6.28318548202515)
          this.accumulator -= TimeSpan.FromSeconds(6.28318548202515);
        float num2 = (float) (Math.Sin(this.accumulator.TotalSeconds / 2.0) * 2.0 / 16.0);
        float waterLevel2 = waterLevel1 - this.lastVariation + num2;
        this.lastVariation = num2;
        this.RaysMesh.Position = (waterLevel2 - 0.5f) * Vector3.UnitY;
        this.LiquidMesh.Position = this.ScreenCenter * FezMath.XZMask + (waterLevel2 + 0.5f) * Vector3.UnitY;
        if (this.LevelManager.Sky != null && this.LevelManager.Sky.Name != "Cave")
          this.CausticsMesh.Position = FezMath.XZMask * this.CausticsMesh.Position + (waterLevel2 - 0.5f) * Vector3.UnitY;
        if (LiquidTypeExtensions.IsWater(this.LevelManager.WaterType))
        {
          if (this.LevelManager.Sky != null && this.LevelManager.Sky.Name == "Cave")
            this.BackgroundCausticsTiming.Update(gameTime.ElapsedGameTime, 0.375f);
          this.CausticsAnimation.Timing.Update(gameTime.ElapsedGameTime, 0.875f);
        }
        if (this.LevelManager.WaterType == LiquidType.Lava || this.LevelManager.WaterType == LiquidType.Sewer)
        {
          this.BubbleSystem.Settings.Velocity = (VaryingVector3) new Vector3(0.0f, (float) (0.150000005960464 + (double) this.LevelManager.WaterSpeed * 0.75), 0.0f);
          this.BubbleSystem.Settings.SpawnVolume = new BoundingBox()
          {
            Min = (this.ScreenCenter - new Vector3(this.CameraRadius / 1.5f)) * FezMath.XZMask + (waterLevel2 - 1.8f) * Vector3.UnitY,
            Max = (this.ScreenCenter + new Vector3(this.CameraRadius / 1.5f)) * FezMath.XZMask + (waterLevel2 - 0.8f) * Vector3.UnitY
          };
        }
        switch (this.LevelManager.WaterType)
        {
          case LiquidType.Water:
          case LiquidType.Blood:
          case LiquidType.Purple:
          case LiquidType.Green:
            this.FoamMesh.Position = (waterLevel2 + 0.5f) * Vector3.UnitY + this.CameraInterpolatedCenter * this.ForwardVector;
            this.FoamEffect.TimeAccumulator = (float) this.accumulator.TotalSeconds;
            this.FoamEffect.ScreenCenterSide = FezMath.Dot(this.CameraInterpolatedCenter, this.RightVector);
            this.FoamEffect.ShoreTotalWidth = 48f;
            if (this.TransitionRenderer == null && RandomHelper.Probability(0.03))
            {
              Vector3 vector3_1 = this.ScreenCenter - this.CameraRadius / 2f * FezMath.XZMask;
              Vector3 vector3_2 = this.ScreenCenter + this.CameraRadius / 2f * FezMath.XZMask;
              Vector3 vector3_3 = new Vector3(RandomHelper.Between((double) vector3_1.X, (double) vector3_2.X), 0.0f, RandomHelper.Between((double) vector3_1.Z, (double) vector3_2.Z));
              float num3 = RandomHelper.Between(0.1, 1.25);
              float num4 = 3f + RandomHelper.Centered(1.0);
              Group group = this.RaysMesh.AddColoredQuad(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(-num4 - num3, -num4, 0.0f), new Vector3(-num4, -num4, 0.0f), new Vector3(-num3, 0.0f, 0.0f), Color.White, Color.Black, Color.Black, Color.White);
              group.CustomData = (object) new LiquidHost.RayCustomData()
              {
                RandomSpeed = RandomHelper.Between(0.5, 1.5)
              };
              group.Material = new Material();
              group.Position = vector3_3;
            }
            for (int i = 0; i < this.RaysMesh.Groups.Count; ++i)
            {
              Group group = this.RaysMesh.Groups[i];
              LiquidHost.RayCustomData rayCustomData = (LiquidHost.RayCustomData) group.CustomData;
              if (rayCustomData != null)
              {
                rayCustomData.AccumulatedTime += gameTime.ElapsedGameTime;
                group.Material.Diffuse = new Vector3(Easing.EaseOut(Math.Sin(rayCustomData.AccumulatedTime.TotalSeconds / 5.0 * 3.14159274101257), EasingType.Quadratic) * 0.2f);
                group.Position += (float) gameTime.ElapsedGameTime.TotalSeconds * this.RightVector * 0.4f * rayCustomData.RandomSpeed;
                if (rayCustomData.AccumulatedTime.TotalSeconds > 5.0)
                {
                  this.RaysMesh.RemoveGroupAt(i);
                  --i;
                }
              }
            }
            break;
          case LiquidType.Lava:
            if (!this.ForcedUpdate)
              this.SpawnBubbles(gameTime.ElapsedGameTime, waterLevel2, false);
            this.FoamMesh.Position = this.LiquidMesh.Position;
            this.FoamEffect.ScreenCenterSide = FezMath.Dot(this.CameraInterpolatedCenter, this.RightVector);
            this.FoamEffect.ShoreTotalWidth = 48f;
            break;
          case LiquidType.Sewer:
            this.FoamMesh.Position = this.LiquidMesh.Position;
            this.FoamEffect.ScreenCenterSide = FezMath.Dot(this.CameraInterpolatedCenter, this.RightVector);
            this.FoamEffect.ShoreTotalWidth = 48f;
            break;
        }
        if (this.LevelManager.WaterType != LiquidType.Lava || (double) this.LevelManager.WaterHeight < 135.5)
          return;
        foreach (TrileInstance trileInstance in (IEnumerable<TrileInstance>) this.LevelManager.PickupGroups.Keys)
          trileInstance.PhysicsState.IgnoreCollision = true;
      }
    }

    private void SpawnBubbles(TimeSpan elapsed, float waterLevel, bool invisible)
    {
      this.TimeUntilBubble -= elapsed;
      if (this.TimeUntilBubble.TotalSeconds > 0.0)
        return;
      AnimatedTexture animation = RandomHelper.Probability(0.7) ? this.SmallBubbleAnim : (RandomHelper.Probability(0.7) ? this.MediumBubbleAnim : this.LargeBubbleAnim);
      Vector3 position = new Vector3(RandomHelper.Between((double) this.ScreenCenter.X - (double) this.CameraRadius / 2.0, (double) this.ScreenCenter.X + (double) this.CameraRadius / 2.0), (float) ((double) waterLevel + (double) animation.FrameHeight / 32.0 - 0.5 - 1.0 / 16.0), RandomHelper.Between((double) this.ScreenCenter.Z - (double) this.CameraRadius / 2.0, (double) this.ScreenCenter.Z + (double) this.CameraRadius / 2.0));
      if (!invisible)
        this.LevelManager.AddPlane(new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, animation)
        {
          Position = position,
          Rotation = this.CameraRotation * (RandomHelper.Probability(0.5) ? Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f) : Quaternion.Identity),
          Doublesided = true,
          Loop = false,
          Timing = {
            Step = 0.0f
          }
        });
      if (animation == this.SmallBubbleAnim)
        SoundEffectExtensions.EmitAt(this.sSmallBubble, position).FadeDistance = 20f;
      else if (animation == this.MediumBubbleAnim)
        SoundEffectExtensions.EmitAt(this.sMidBubble, position).FadeDistance = 20f;
      else
        SoundEffectExtensions.EmitAt(this.sLargeBubble, position).FadeDistance = 20f;
      if (!invisible && animation != this.SmallBubbleAnim)
        Waiters.Wait(0.1, (Action) (() => this.LevelManager.AddPlane(new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.SmokeAnim)
        {
          Position = new Vector3(position.X, (float) ((double) waterLevel + (double) this.SmokeAnim.FrameHeight / 32.0 - 0.5 + 0.125), position.Z) + this.ForwardVector,
          Rotation = this.CameraRotation * (RandomHelper.Probability(0.5) ? Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f) : Quaternion.Identity),
          Doublesided = true,
          Opacity = 0.4f,
          Loop = false,
          Timing = {
            Step = 0.0f
          }
        })));
      this.TimeUntilBubble = TimeSpan.FromSeconds((double) RandomHelper.Between(0.1, 0.4));
    }

    public void DrawLights()
    {
      if (!this.Visible || this.LevelManager.WaterType == LiquidType.None || this.GameState.Loading)
        return;
      LiquidColorScheme liquidColorScheme = LiquidHost.ColorSchemes[this.LevelManager.WaterType];
      Vector3 vector3_1 = new Vector3(0.5f);
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      this.LiquidMesh.AlwaysOnTop = this.CameraManager.Viewpoint != Viewpoint.Perspective && !this.CameraManager.ProjectionTransition;
      this.LiquidMesh.Groups[0].Material.Diffuse = vector3_1;
      this.LiquidMesh.Groups[0].Enabled = true;
      this.LiquidMesh.Groups[1].Enabled = false;
      this.LiquidMesh.Draw();
      this.LiquidMesh.Groups[0].Material.Diffuse = liquidColorScheme.LiquidBody.ToVector3();
      if (LiquidTypeExtensions.IsWater(this.LevelManager.WaterType) && !this.GameState.InFpsMode)
      {
        if (this.LevelManager.WaterType != LiquidType.Water)
        {
          Vector3 vector3_2 = LiquidHost.ColorSchemes[this.LevelManager.WaterType].LiquidBody.ToVector3();
          this.CausticsMesh.FirstGroup.Material.Diffuse = vector3_2 / Math.Max(Math.Max(vector3_2.X, vector3_2.Y), vector3_2.Z);
        }
        else
          this.CausticsMesh.FirstGroup.Material.Diffuse = Vector3.One;
        if (this.LevelManager.Sky != null && this.LevelManager.Sky.Name == "Cave")
        {
          float num1 = this.CameraRadius * 3f;
          this.CausticsMesh.Position = new Vector3((float) ((double) this.LevelManager.Size.X / 2.0 - (double) num1 / 2.0), this.WaterLevel - 0.5f, (float) ((double) this.LevelManager.Size.Z / 2.0 - (double) num1 / 2.0));
          this.CausticsHeight = 12f;
          this.CausticsMesh.Scale = new Vector3(num1, this.CausticsHeight, num1);
          this.CausticsMesh.SamplerState = SamplerState.LinearWrap;
          this.CausticsMesh.Culling = CullMode.CullClockwiseFace;
          float num2 = num1 / (this.CausticsHeight / 2f);
          int width = this.CausticsAnimation.Texture.Width;
          int height = this.CausticsAnimation.Texture.Height;
          int frame1 = this.BackgroundCausticsTiming.Frame;
          Rectangle rectangle = this.CausticsAnimation.Offsets[frame1];
          this.CausticsMesh.TextureMatrix = (Dirtyable<Matrix>) new Matrix(num2 * (float) rectangle.Width / (float) width, 0.0f, 0.0f, 0.0f, 0.0f, (float) rectangle.Height / (float) height, 0.0f, 0.0f, (float) ((double) -rectangle.Width / (double) width / 4.0) * num2, (float) rectangle.Y / (float) height, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
          rectangle = this.CausticsAnimation.Offsets[(frame1 + 1) % this.BackgroundCausticsTiming.FrameTimings.Length];
          this.CausticsMesh.CustomData = (object) new Matrix(num2 * (float) rectangle.Width / (float) width, 0.0f, 0.0f, 0.0f, 0.0f, (float) rectangle.Height / (float) height, 0.0f, 0.0f, (float) ((double) -rectangle.Width / (double) width / 4.0) * num2, (float) rectangle.Y / (float) height, this.BackgroundCausticsTiming.NextFrameContribution, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
          this.CausticsMesh.Blending = new BlendingMode?(BlendingMode.Maximum);
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Greater, StencilMask.Level);
          this.CausticsMesh.Draw();
          Vector3 vector3_2 = this.LevelManager.Size * 1.5f;
          float num3 = Math.Max(vector3_2.X, vector3_2.Z);
          this.CausticsHeight = 3f;
          this.CausticsMesh.Position = new Vector3((float) ((double) vector3_2.X / 1.5 / 2.0 + (double) vector3_2.X * -0.5), this.WaterLevel - 0.5f, (float) ((double) vector3_2.Z / 1.5 / 2.0 + (double) vector3_2.Z * -0.5));
          this.CausticsMesh.Culling = CullMode.CullCounterClockwiseFace;
          this.CausticsMesh.Scale = new Vector3(num3, this.CausticsHeight, num3);
          this.CausticsMesh.Blending = new BlendingMode?();
          this.CausticsMesh.SamplerState = SamplerState.LinearWrap;
          float num4 = num3 / (this.CausticsHeight / 2f);
          int frame2 = this.CausticsAnimation.Timing.Frame;
          rectangle = this.CausticsAnimation.Offsets[frame2];
          this.CausticsMesh.TextureMatrix = (Dirtyable<Matrix>) new Matrix(num4 * (float) rectangle.Width / (float) width, 0.0f, 0.0f, 0.0f, 0.0f, (float) rectangle.Height / (float) height, 0.0f, 0.0f, (float) (-(double) num4 / 2.0), (float) rectangle.Y / (float) height, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
          rectangle = this.CausticsAnimation.Offsets[(frame2 + 1) % this.CausticsAnimation.Timing.FrameTimings.Length];
          this.CausticsMesh.CustomData = (object) new Matrix(num4 * (float) rectangle.Width / (float) width, 0.0f, 0.0f, 0.0f, 0.0f, (float) rectangle.Height / (float) height, 0.0f, 0.0f, (float) (-(double) num4 / 2.0), (float) rectangle.Y / (float) height, this.CausticsAnimation.Timing.NextFrameContribution, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
          GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
        }
        else
        {
          Vector3 vector3_2 = this.LevelManager.Size * 1.5f;
          float num1 = Math.Max(vector3_2.X, vector3_2.Z);
          this.CausticsMesh.SamplerState = SamplerState.LinearWrap;
          this.CausticsHeight = 3f;
          this.CausticsMesh.Scale = new Vector3(num1, this.CausticsHeight, num1);
          float num2 = num1 / (this.CausticsHeight / 2f);
          int width = this.CausticsAnimation.Texture.Width;
          int height = this.CausticsAnimation.Texture.Height;
          int frame = this.CausticsAnimation.Timing.Frame;
          Rectangle rectangle1 = this.CausticsAnimation.Offsets[frame];
          this.CausticsMesh.TextureMatrix = (Dirtyable<Matrix>) new Matrix(num2 * (float) rectangle1.Width / (float) width, 0.0f, 0.0f, 0.0f, 0.0f, (float) rectangle1.Height / (float) height, 0.0f, 0.0f, (float) (-(double) num2 / 2.0), (float) rectangle1.Y / (float) height, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
          Rectangle rectangle2 = this.CausticsAnimation.Offsets[(frame + 1) % this.CausticsAnimation.Timing.FrameTimings.Length];
          this.CausticsMesh.CustomData = (object) new Matrix(num2 * (float) rectangle2.Width / (float) width, 0.0f, 0.0f, 0.0f, 0.0f, (float) rectangle2.Height / (float) height, 0.0f, 0.0f, (float) (-(double) num2 / 2.0), (float) rectangle2.Y / (float) height, this.CausticsAnimation.Timing.NextFrameContribution, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
        }
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.LessEqual, StencilMask.Level);
        this.CausticsMesh.Draw();
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      }
      if (this.FoamMesh.Groups.Count <= 1)
        return;
      this.FoamMesh.Groups[0].Enabled = false;
      this.FoamMesh.Groups[1].Material.Diffuse = vector3_1;
      this.FoamMesh.Draw();
      this.FoamMesh.Groups[1].Material.Diffuse = liquidColorScheme.EmergedFoam.ToVector3();
      this.FoamMesh.Groups[0].Enabled = true;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.TransitionRenderer != null || this.GameState.StereoMode)
        return;
      this.DoDraw(false);
    }

    public void DoDraw(bool skipUnderwater = false)
    {
      bool flag = this.CameraManager.Viewpoint == Viewpoint.Perspective || this.CameraManager.ProjectionTransition;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      LiquidColorScheme liquidColorScheme = LiquidHost.ColorSchemes[this.LevelManager.WaterType];
      Vector3 vector3 = this.LevelManager.WaterType == LiquidType.Sewer || this.GameState.StereoMode ? Vector3.One : this.LevelManager.ActualDiffuse.ToVector3();
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).StencilEnable = true;
      this.LiquidMesh.AlwaysOnTop = !flag;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).StencilPass = StencilOperation.Keep;
      this.LiquidMesh.Groups[0].Enabled = true;
      this.LiquidMesh.Groups[1].Enabled = false;
      this.LiquidMesh.Groups[0].Material.Diffuse *= vector3;
      this.LiquidMesh.Draw();
      this.LiquidMesh.Groups[0].Material.Diffuse = liquidColorScheme.LiquidBody.ToVector3();
      if (!skipUnderwater)
      {
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.LessEqual, StencilMask.Level);
        this.LiquidMesh.Groups[0].Enabled = false;
        this.LiquidMesh.Groups[1].Enabled = true;
        if (this.GameState.FarawaySettings.InTransition)
        {
          if ((double) this.GameState.FarawaySettings.TransitionStep < 0.5)
            this.LiquidMesh.Groups[1].Material.Opacity = 1f - this.GameState.FarawaySettings.OriginFadeOutStep;
          else if ((double) this.GameState.FarawaySettings.DestinationCrossfadeStep > 0.0)
            this.LiquidMesh.Groups[1].Material.Opacity = this.GameState.FarawaySettings.DestinationCrossfadeStep;
        }
        else
          this.LiquidMesh.Groups[1].Material.Opacity = 1f;
        this.LiquidMesh.Groups[1].Material.Diffuse *= vector3;
        this.LiquidMesh.Draw();
        this.LiquidMesh.Groups[1].Material.Diffuse = liquidColorScheme.SolidOverlay.ToVector3();
      }
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Water));
      GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.None);
      this.LiquidMesh.Groups[0].Enabled = true;
      this.LiquidMesh.Draw();
      GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.All);
      float num = 1f;
      if (!this.GameState.FarawaySettings.InTransition)
      {
        if (this.CameraManager.Viewpoint == Viewpoint.Perspective)
          num = 0.0f;
        else if (this.CameraManager.ProjectionTransition && this.CameraManager.Viewpoint == Viewpoint.Perspective)
          num = 1f - this.CameraManager.ViewTransitionStep;
        else if (this.CameraManager.ProjectionTransition && FezMath.IsOrthographic(this.CameraManager.Viewpoint))
          num = this.CameraManager.ViewTransitionStep;
      }
      this.FoamMesh.Groups[0].Material.Opacity = num;
      if (this.FoamMesh.Groups.Count > 1)
        this.FoamMesh.Groups[1].Material.Opacity = num;
      this.FoamMesh.Groups[0].Material.Diffuse *= vector3;
      if (this.FoamMesh.Groups.Count > 1)
        this.FoamMesh.Groups[1].Material.Diffuse *= vector3;
      this.FoamMesh.Draw();
      this.FoamMesh.Groups[0].Material.Diffuse = liquidColorScheme.SubmergedFoam.ToVector3();
      if (this.FoamMesh.Groups.Count > 1)
        this.FoamMesh.Groups[1].Material.Diffuse = liquidColorScheme.EmergedFoam.ToVector3();
      this.RaysMesh.AlwaysOnTop = !flag;
      this.RaysMesh.Draw();
      GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Always, StencilMask.None);
    }

    private class RayCustomData
    {
      public TimeSpan AccumulatedTime;
      public float RandomSpeed;
    }

    private class WaterTransitionRenderer : DrawableGameComponent
    {
      private readonly LiquidHost Host;
      public bool ViewLocked;

      [ServiceDependency]
      public IEngineStateManager EngineState { get; set; }

      [ServiceDependency]
      public IGameCameraManager CameraManager { get; set; }

      public WaterTransitionRenderer(Game game, LiquidHost host)
        : base(game)
      {
        this.DrawOrder = 1001;
        this.Host = host;
      }

      public void LockView()
      {
        (this.Host.LiquidMesh.Effect as DefaultEffect).ForcedViewMatrix = new Matrix?(this.Host.CameraManager.View);
        this.Host.FoamEffect.ForcedViewMatrix = new Matrix?(this.Host.CameraManager.View);
        (this.Host.RaysMesh.Effect as DefaultEffect).ForcedViewMatrix = new Matrix?(this.Host.CameraManager.View);
        (this.Host.CausticsMesh.Effect as CausticsEffect).ForcedViewMatrix = new Matrix?(this.Host.CameraManager.View);
        this.ViewLocked = true;
      }

      public override void Update(GameTime gameTime)
      {
        if (this.EngineState.Loading || this.EngineState.Paused)
          return;
        float num1 = (float) this.GraphicsDevice.Viewport.Width / (this.CameraManager.PixelsPerTrixel * 16f);
        float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
        if ((double) this.EngineState.FarawaySettings.OriginFadeOutStep == 1.0)
        {
          float num2 = (float) (((double) this.EngineState.FarawaySettings.TransitionStep - 0.127499997615814) / 0.872500002384186);
          num1 = MathHelper.Lerp(num1, this.EngineState.FarawaySettings.DestinationRadius, Easing.EaseInOut((double) num2, EasingType.Sine));
        }
        Matrix orthographic = Matrix.CreateOrthographic(num1 / viewScale, num1 / this.CameraManager.AspectRatio / viewScale, this.CameraManager.NearPlane, this.CameraManager.FarPlane);
        (this.Host.LiquidMesh.Effect as DefaultEffect).ForcedProjectionMatrix = new Matrix?(orthographic);
        this.Host.FoamEffect.ForcedProjectionMatrix = new Matrix?(orthographic);
        (this.Host.RaysMesh.Effect as DefaultEffect).ForcedProjectionMatrix = new Matrix?(orthographic);
        (this.Host.CausticsMesh.Effect as CausticsEffect).ForcedProjectionMatrix = new Matrix?(orthographic);
      }

      protected override void Dispose(bool disposing)
      {
        base.Dispose(disposing);
        (this.Host.LiquidMesh.Effect as DefaultEffect).ForcedViewMatrix = new Matrix?();
        this.Host.FoamEffect.ForcedViewMatrix = new Matrix?();
        (this.Host.RaysMesh.Effect as DefaultEffect).ForcedViewMatrix = new Matrix?();
        (this.Host.CausticsMesh.Effect as CausticsEffect).ForcedViewMatrix = new Matrix?();
        (this.Host.LiquidMesh.Effect as DefaultEffect).ForcedProjectionMatrix = new Matrix?();
        this.Host.FoamEffect.ForcedProjectionMatrix = new Matrix?();
        (this.Host.RaysMesh.Effect as DefaultEffect).ForcedProjectionMatrix = new Matrix?();
        (this.Host.CausticsMesh.Effect as CausticsEffect).ForcedProjectionMatrix = new Matrix?();
        this.Host.ForcedUpdate = true;
        this.Host.Update(new GameTime());
        this.Host.ForcedUpdate = false;
      }

      public override void Draw(GameTime gameTime)
      {
        this.Host.DoDraw(this.EngineState.StereoMode);
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      }
    }
  }
}
