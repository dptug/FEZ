// Type: FezEngine.Components.SkyHost
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Components
{
  public class SkyHost : DrawableGameComponent
  {
    private static readonly float[] starSideOffsets = new float[4];
    private readonly List<Mesh> cloudMeshes = new List<Mesh>();
    private readonly Dictionary<Layer, List<SkyHost.CloudState>> cloudStates = new Dictionary<Layer, List<SkyHost.CloudState>>((IEqualityComparer<Layer>) LayerComparer.Default);
    private Matrix backgroundMatrix = Matrix.Identity;
    private const int Clouds = 64;
    private const int BaseDistance = 32;
    private const int ParallaxDistance = 48;
    private const int HeightSpread = 96;
    private const float MovementSpeed = 0.025f;
    private const float PerspectiveScaling = 4f;
    private readonly Mesh stars;
    private Texture2D skyBackground;
    private AnimatedTexture shootingStar;
    public Mesh BgLayers;
    private SoundEffect sShootingStar;
    public static SkyHost Instance;
    private Color[] fogColors;
    private Color[] cloudColors;
    private float flickerIn;
    private float flickerCount;
    private string lastSkyName;
    private bool flickering;
    private Vector3 lastCamPos;
    private TimeSpan sinceReached;
    private IWaiter waiter;
    private int sideToSwap;
    private float lastCamSide;
    private float sideOffset;
    private float startStep;
    private float startStep2;
    private RenderTargetHandle RtHandle;
    private float RadiusAtFirstDraw;

    private Color CurrentFogColor
    {
      get
      {
        float number = this.TimeManager.DayFraction * (float) this.fogColors.Length;
        if ((double) number == (double) this.fogColors.Length)
          number = 0.0f;
        this.TimeManager.CurrentFogColor = Color.Lerp(this.fogColors[Math.Max((int) Math.Floor((double) number), 0)], this.fogColors[Math.Min((int) Math.Ceiling((double) number), this.fogColors.Length - 1)], FezMath.Frac(number));
        this.TimeManager.CurrentAmbientFactor = Math.Max(FezMath.Dot(this.TimeManager.CurrentFogColor.ToVector3(), new Vector3(0.3333333f)), 0.1f);
        return this.TimeManager.CurrentFogColor;
      }
    }

    private Color CurrentCloudTint
    {
      get
      {
        float number = this.TimeManager.DayFraction * (float) this.cloudColors.Length;
        if ((double) number == (double) this.cloudColors.Length)
          number = 0.0f;
        return Color.Lerp(this.cloudColors[Math.Max((int) Math.Floor((double) number), 0)], this.cloudColors[Math.Min((int) Math.Ceiling((double) number), this.cloudColors.Length - 1)], FezMath.Frac(number));
      }
    }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    [ServiceDependency]
    public IFogManager FogManager { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    static SkyHost()
    {
    }

    public SkyHost(Game game)
      : base(game)
    {
      this.DrawOrder = 0;
      this.UpdateOrder = 11;
      SkyHost.Instance = this;
      this.stars = new Mesh()
      {
        Culling = CullMode.CullClockwiseFace,
        AlwaysOnTop = true,
        DepthWrites = false,
        SamplerState = SamplerState.PointWrap
      };
      ServiceHelper.AddComponent((IGameComponent) new CloudShadowsHost(game, this));
    }

    public override void Initialize()
    {
      base.Initialize();
      int index = 0;
      foreach (FaceOrientation faceOrientation in Util.GetValues<FaceOrientation>())
      {
        if (FezMath.IsSide(faceOrientation))
        {
          this.stars.AddFace(Vector3.One, FezMath.AsVector(faceOrientation) / 2f, faceOrientation, true);
          SkyHost.starSideOffsets[index] = (float) index++ / 4f;
        }
      }
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        this.ResizeStars();
        this.ResizeLayers();
        this.OnViewpointChanged();
        this.OnViewChanged(true);
      });
      this.CameraManager.ViewChanged += new Action(this.OnViewChanged);
      this.CameraManager.ViewpointChanged += new Action(this.OnViewpointChanged);
      this.TimeManager.Tick += new Action(this.UpdateTimeOfDay);
    }

    protected override void LoadContent()
    {
      this.stars.Effect = (BaseEffect) new StarsEffect();
      this.shootingStar = this.CMProvider.Global.Load<AnimatedTexture>("Background Planes/shootingstar");
      this.sShootingStar = this.CMProvider.Global.Load<SoundEffect>("Sounds/Nature/ShootingStar");
      this.LevelManager.SkyChanged += new Action(this.InitializeSky);
    }

    private void InitializeSky()
    {
      if (this.LevelManager.Sky == null)
        return;
      ContentManager contentManager = this.LevelManager.Name == null ? this.CMProvider.Global : this.CMProvider.GetForLevel(this.LevelManager.Name);
      string str1 = "Skies/" + this.LevelManager.Sky.Name + "/";
      if (this.LevelManager.Sky.Name == this.lastSkyName && !this.EngineState.InEditor)
      {
        foreach (string str2 in this.LevelManager.Sky.Clouds)
          contentManager.Load<Texture2D>(str1 + str2);
        foreach (SkyLayer skyLayer in this.LevelManager.Sky.Layers)
          contentManager.Load<Texture2D>(str1 + skyLayer.Name);
        try
        {
          this.skyBackground = contentManager.Load<Texture2D>(str1 + this.LevelManager.Sky.Background);
        }
        catch (Exception ex)
        {
          this.skyBackground = contentManager.Load<Texture2D>("Skies/Default/SkyBack");
        }
        if (this.LevelManager.Sky.Stars == null)
          return;
        contentManager.Load<Texture2D>(str1 + this.LevelManager.Sky.Stars);
      }
      else
      {
        this.lastSkyName = this.LevelManager.Sky.Name;
        if (this.LevelManager.Sky.Stars != null)
          this.stars.Texture = (Dirtyable<Texture>) ((Texture) contentManager.Load<Texture2D>(str1 + this.LevelManager.Sky.Stars));
        else
          this.stars.Texture.Set((Texture) null);
        string assetName1 = str1 + this.LevelManager.Sky.Background;
        try
        {
          this.skyBackground = contentManager.Load<Texture2D>(assetName1);
        }
        catch (Exception ex)
        {
          this.skyBackground = contentManager.Load<Texture2D>("Skies/Default/SkyBack");
        }
        this.fogColors = new Color[this.skyBackground.Width];
        Color[] data1 = new Color[this.skyBackground.Width * this.skyBackground.Height];
        this.skyBackground.GetData<Color>(data1);
        Array.Copy((Array) data1, this.skyBackground.Width * this.skyBackground.Height / 2, (Array) this.fogColors, 0, this.skyBackground.Width);
        using (MemoryContentManager memoryContentManager = new MemoryContentManager(contentManager.ServiceProvider, contentManager.RootDirectory))
        {
          Texture2D texture2D = (Texture2D) null;
          if (this.LevelManager.Sky.CloudTint != null)
          {
            string assetName2 = str1 + this.LevelManager.Sky.CloudTint;
            try
            {
              texture2D = memoryContentManager.Load<Texture2D>(assetName2);
            }
            catch (Exception ex)
            {
              Logger.Log("Sky Init", Common.LogSeverity.Warning, "Cloud tinting texture could not be found");
            }
          }
          if (texture2D != null)
          {
            this.cloudColors = new Color[texture2D.Width];
            Color[] data2 = new Color[texture2D.Width * texture2D.Height];
            texture2D.GetData<Color>(data2);
            Array.Copy((Array) data2, texture2D.Width * (texture2D.Height / 2), (Array) this.cloudColors, 0, texture2D.Width);
          }
          else
            this.cloudColors = new Color[1]
            {
              Color.White
            };
        }
        this.cloudStates.Clear();
        foreach (Mesh mesh in this.cloudMeshes)
          mesh.Dispose();
        this.cloudMeshes.Clear();
        if (this.BgLayers != null)
          this.BgLayers.ClearGroups();
        else
          this.BgLayers = new Mesh()
          {
            AlwaysOnTop = true,
            DepthWrites = false,
            Effect = (BaseEffect) new DefaultEffect.Textured()
          };
        int num1 = 0;
        foreach (SkyLayer skyLayer in this.LevelManager.Sky.Layers)
        {
          Texture2D texture2D1 = contentManager.Load<Texture2D>(str1 + skyLayer.Name);
          Texture2D texture2D2 = (Texture2D) null;
          if (skyLayer.Name == "OBS_SKY_A")
            texture2D2 = contentManager.Load<Texture2D>(str1 + "OBS_SKY_C");
          int num2 = 0;
          foreach (FaceOrientation faceOrientation in Util.GetValues<FaceOrientation>())
          {
            if (FezMath.IsSide(faceOrientation))
            {
              Group group = this.BgLayers.AddFace(Vector3.One, -FezMath.AsVector(faceOrientation) / 2f, faceOrientation, true);
              group.Texture = texture2D2 == null || faceOrientation == FaceOrientation.Left ? (Texture) texture2D1 : (Texture) texture2D2;
              group.AlwaysOnTop = new bool?(skyLayer.InFront);
              group.Material = new Material()
              {
                Opacity = skyLayer.Opacity
              };
              group.CustomData = (object) new SkyHost.BgLayerState()
              {
                Layer = num1,
                Side = num2++,
                OriginalOpacity = skyLayer.Opacity
              };
            }
          }
          ++num1;
        }
        foreach (Layer key in Util.GetValues<Layer>())
          this.cloudStates.Add(key, new List<SkyHost.CloudState>());
        foreach (string str2 in this.LevelManager.Sky.Clouds)
          this.cloudMeshes.Add(new Mesh()
          {
            AlwaysOnTop = true,
            DepthWrites = false,
            Effect = (BaseEffect) new CloudsEffect(),
            Texture = (Dirtyable<Texture>) ((Texture) contentManager.Load<Texture2D>(str1 + str2)),
            Culling = CullMode.None,
            SamplerState = SamplerState.PointClamp
          });
        float num3 = 64f * this.LevelManager.Sky.Density;
        int num4 = (int) Math.Sqrt((double) num3);
        float num5 = num3 / (float) num4;
        float num6 = RandomHelper.Between(0.0, 6.28318548202515);
        float num7 = RandomHelper.Between(0.0, 192.0);
        if (this.cloudMeshes.Count > 0)
        {
          for (int index1 = 0; index1 < num4; ++index1)
          {
            for (int index2 = 0; (double) index2 < (double) num5; ++index2)
            {
              Group group = RandomHelper.InList<Mesh>(this.cloudMeshes).AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, true);
              float num2 = RandomHelper.Between(0.0, 1.0 / (double) num4 * 6.28318548202515);
              float num8 = RandomHelper.Between(0.0, 1.0 / (double) num5 * 192.0);
              this.cloudStates[RandomHelper.EnumField<Layer>()].Add(new SkyHost.CloudState()
              {
                Group = group,
                Phi = (float) (((double) index1 / (double) num4 * 6.28318548202515 + (double) num6 + (double) num2) % 6.28318548202515),
                LocalHeightOffset = (float) (((double) index2 / (double) num5 * 96.0 * 2.0 + (double) num7 + (double) num8) % 192.0 - 96.0)
              });
              group.Material = new Material();
            }
          }
        }
        this.flickerIn = RandomHelper.Between(2.0, 10.0);
        this.ResizeLayers();
        this.ResizeStars();
        this.OnViewpointChanged();
        this.OnViewChanged();
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Paused || this.EngineState.InMap || this.EngineState.Loading && !this.EngineState.FarawaySettings.InTransition || (this.LevelManager.Sky == null || this.LevelManager.Name == null))
        return;
      this.ForceUpdate(gameTime);
    }

    private void ForceUpdate(GameTime gameTime)
    {
      float elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds * (this.TimeManager.TimeFactor / 360f);
      Vector3 a = this.LevelManager.Size == Vector3.Zero ? new Vector3(16f) : this.LevelManager.Size;
      Vector3 vector3_1 = a / 2f;
      Vector3 forward = this.CameraManager.View.Forward;
      if ((double) forward.Z != 0.0)
        forward.Z *= -1f;
      float currentAmbientFactor = this.TimeManager.CurrentAmbientFactor;
      float num1 = Math.Abs(FezMath.Dot(a, FezMath.RightVector(this.CameraManager.Viewpoint))) / 32f;
      if (this.CameraManager.ActionRunning)
        this.sinceReached += gameTime.ElapsedGameTime;
      bool flag1 = FezMath.IsOrthographic(this.CameraManager.Viewpoint);
      if (flag1)
      {
        if (this.LevelManager.Sky.HorizontalScrolling)
        {
          foreach (Group group in this.BgLayers.Groups)
            (group.CustomData as SkyHost.BgLayerState).WindOffset += (float) ((double) elapsedTime * (double) this.LevelManager.Sky.WindSpeed * 0.025000000372529);
        }
        this.ResizeLayers();
      }
      foreach (Layer layer in this.cloudStates.Keys)
      {
        float num2 = (float) ((double) elapsedTime * (double) this.LevelManager.Sky.WindSpeed * 0.025000000372529) * CloudLayerExtensions.SpeedFactor(layer) / num1;
        Vector3 vector3_2 = !flag1 ? vector3_1 + Vector3.One * (float) (32.0 + 48.0 * (double) CloudLayerExtensions.DistanceFactor(layer)) : vector3_1 + Vector3.One * 32f / 2.5f;
        foreach (SkyHost.CloudState cloudState in this.cloudStates[layer])
        {
          if (flag1)
            cloudState.Phi -= num2;
          else
            cloudState.GlobalHeightOffset = this.CameraManager.Center.Y;
          float spreadFactor = !flag1 ? 1f : (this.CameraManager.ProjectionTransition ? MathHelper.Lerp(1f, 0.2f, this.CameraManager.ViewTransitionStep) : 0.2f);
          Vector3 vector3_3 = new Vector3((float) Math.Sin((double) cloudState.Phi) * vector3_2.X + vector3_1.X, cloudState.GetHeight(spreadFactor), (float) Math.Cos((double) cloudState.Phi) * vector3_2.Z + vector3_1.Z);
          Quaternion fromYawPitchRoll = Quaternion.CreateFromYawPitchRoll(cloudState.Phi, 3.141593f, 3.141593f);
          if (this.CameraManager.ProjectionTransition)
          {
            if (flag1)
            {
              int width = cloudState.Group.Mesh.TextureMap.Width;
              int height = cloudState.Group.Mesh.TextureMap.Height;
              cloudState.Group.Scale = new Vector3((float) width, (float) height, (float) width) / 16f * (2f - this.CameraManager.ViewTransitionStep);
              cloudState.Group.Rotation = Quaternion.Slerp(fromYawPitchRoll, this.CameraManager.Rotation, this.CameraManager.ViewTransitionStep);
              cloudState.Group.Enabled = (double) FezMath.Dot(vector3_3 - vector3_1, forward) <= 0.0;
            }
            else
              cloudState.Group.Rotation = Quaternion.Slerp(this.CameraManager.Rotation, fromYawPitchRoll, this.CameraManager.ViewTransitionStep);
          }
          else if (flag1)
          {
            bool flag2 = (double) FezMath.Dot(vector3_3 - vector3_1, forward) <= 0.0;
            if (!cloudState.Group.Enabled && flag2)
              cloudState.Group.Rotation = this.CameraManager.Rotation;
            cloudState.Group.Enabled = flag2;
          }
          else
            cloudState.Group.Rotation = fromYawPitchRoll;
          if (!flag1 || cloudState.Group.Enabled)
            cloudState.Group.Position = vector3_3;
          cloudState.Group.Material.Opacity = CloudLayerExtensions.Opacity(layer) * currentAmbientFactor;
        }
      }
      this.ShootStars();
      this.ResizeStars();
      this.DoFlicker(elapsedTime);
    }

    private void ShootStars()
    {
      if (this.LevelManager.Sky == null || !FezMath.IsOrthographic(this.CameraManager.Viewpoint) || (this.LevelManager.Rainy || (double) this.TimeManager.TimeFactor <= 0.0) || (this.LevelManager.Sky.Stars == null || (double) this.TimeManager.NightContribution != 1.0 || (!RandomHelper.Probability(5E-05) || this.LevelManager.Name == "TELESCOPE")) || this.LevelManager.Sky.Name == "TREE")
        return;
      Vector3 position = this.CameraManager.Center + this.LevelManager.Size / 2f * FezMath.ForwardVector(this.CameraManager.Viewpoint) + new Vector3(RandomHelper.Centered((double) this.CameraManager.Radius / 2.0 - (double) this.shootingStar.FrameWidth / 32.0)) * FezMath.SideMask(this.CameraManager.Viewpoint) + RandomHelper.Between(-(double) this.CameraManager.Radius / (double) this.CameraManager.AspectRatio / 6.0, (double) this.CameraManager.Radius / (double) this.CameraManager.AspectRatio / 2.0 - (double) this.shootingStar.FrameHeight / 32.0) * Vector3.UnitY;
      BackgroundPlane plane = new BackgroundPlane(this.LevelMaterializer.AnimatedPlanesMesh, this.shootingStar)
      {
        Position = position,
        Rotation = this.CameraManager.Rotation,
        Doublesided = true,
        Loop = false,
        Fullbright = true,
        Opacity = this.TimeManager.NightContribution,
        Timing = {
          Step = 0.0f
        }
      };
      SoundEffectExtensions.EmitAt(this.sShootingStar, position);
      this.LevelManager.AddPlane(plane);
    }

    private void OnViewpointChanged()
    {
      foreach (Layer layer in this.cloudStates.Keys)
      {
        foreach (Group group in Enumerable.Select<SkyHost.CloudState, Group>((IEnumerable<SkyHost.CloudState>) this.cloudStates[layer], (Func<SkyHost.CloudState, Group>) (x => x.Group)))
        {
          group.Scale = new Vector3((float) group.Mesh.TextureMap.Width / 16f, (float) group.Mesh.TextureMap.Height / 16f, 1f) * (FezMath.IsOrthographic(this.CameraManager.Viewpoint) ? Vector3.One : new Vector3((float) (4.0 + (double) CloudLayerExtensions.DistanceFactor(layer) * 2.0)));
          if (!FezMath.IsOrthographic(this.CameraManager.Viewpoint))
            group.Enabled = true;
        }
      }
      if (this.CameraManager.Viewpoint != Viewpoint.Perspective)
        return;
      float num = this.LevelManager.Sky.Name == "OBS_SKY" ? 1f : 1.5f;
      foreach (Group group in this.BgLayers.Groups)
      {
        Matrix matrix = group.TextureMatrix.Value ?? Matrix.Identity;
        matrix.M31 += (float) (-(double) matrix.M11 / 2.0 + (double) matrix.M11 / (2.0 * (double) num));
        matrix.M32 += (float) ((double) matrix.M22 / 2.0 - (double) matrix.M22 / (2.0 * (double) num));
        matrix.M11 /= num;
        matrix.M22 /= num;
        group.TextureMatrix.Set(new Matrix?(matrix));
      }
    }

    public void RotateLayer(int layerId, Quaternion rotation)
    {
      this.BgLayers.Groups[layerId].Rotation = rotation;
    }

    private void DoFlicker(float elapsedTime)
    {
      if (this.LevelManager.Sky == null || this.LevelManager.Sky.Name != "INDUS_CITY")
        return;
      this.flickerIn -= elapsedTime;
      if ((double) this.flickerIn > 0.0)
        return;
      if ((double) this.flickerCount == -1.0)
      {
        this.flickerCount = (float) RandomHelper.Random.Next(2, 6);
        this.flickering = false;
      }
      this.flickerIn = RandomHelper.Between(0.0500000007450581, 0.25);
      for (int index = 0; index < 16; ++index)
        this.BgLayers.Groups[index].Material.Opacity = !this.flickering ? 0.0f : (this.BgLayers.Groups[index].CustomData as SkyHost.BgLayerState).OriginalOpacity;
      if (!this.flickering)
        this.SoundManager.MuteAmbience("Ambience ^ rain", 0.0f);
      else
        this.SoundManager.UnmuteAmbience("Ambience ^ rain", 0.0f);
      this.flickering = !this.flickering;
      --this.flickerCount;
      if ((double) this.flickerCount != 0.0)
        return;
      this.SoundManager.UnmuteAmbience("Ambience ^ rain", 0.0f);
      this.flickerCount = -1f;
      this.flickerIn = RandomHelper.Between(2.0, 4.0);
      for (int index = 0; index < 16; ++index)
        this.BgLayers.Groups[index].Material.Opacity = (this.BgLayers.Groups[index].CustomData as SkyHost.BgLayerState).OriginalOpacity;
    }

    private void OnViewChanged()
    {
      if (this.EngineState.LoopRender || this.EngineState.SkyRender)
        return;
      this.OnViewChanged(false);
    }

    private void OnViewChanged(bool force)
    {
      if (this.LevelManager.Sky == null)
        return;
      Vector3 vector3_1 = this.CameraManager.Position - this.CameraManager.ViewOffset;
      if (this.BgLayers != null && this.BgLayers.Groups.Count != 0)
        this.BgLayers.Position = vector3_1;
      Vector3 b = this.CameraManager.InterpolatedCenter - this.CameraManager.ViewOffset;
      if (!this.CameraManager.ActionRunning)
        this.sinceReached = TimeSpan.Zero;
      if (FezMath.IsOrthographic(this.CameraManager.Viewpoint))
      {
        Vector3 vector3_2 = FezMath.RightVector(this.CameraManager.Viewpoint);
        float num1 = FezMath.Dot(vector3_2, b) - FezMath.Dot(vector3_2, this.lastCamPos);
        Quaternion rotation = this.CameraManager.Rotation;
        float num2 = Math.Abs(FezMath.Dot(this.LevelManager.Size + Vector3.One * 32f, vector3_2));
        bool flag = this.CameraManager.ActionRunning && this.sinceReached.TotalSeconds > 1.0 || force;
        foreach (Layer layer in this.cloudStates.Keys)
        {
          float num3 = MathHelper.Lerp(1f, CloudLayerExtensions.ParallaxFactor(layer), this.LevelManager.Sky.CloudsParallax);
          foreach (SkyHost.CloudState cloudState in this.cloudStates[layer])
          {
            while ((double) cloudState.GetHeight(0.2f) - (double) b.Y > 19.2000007629395)
              cloudState.GlobalHeightOffset -= 38.4f;
            while ((double) cloudState.GetHeight(0.2f) - (double) b.Y < -19.2000007629395)
              cloudState.GlobalHeightOffset += 38.4f;
            if (flag)
            {
              cloudState.GlobalHeightOffset += num3 * (b.Y - this.lastCamPos.Y);
              cloudState.Phi -= num3 * 2.25f * num1 / num2;
            }
            if (cloudState.Group.Enabled)
              cloudState.Group.Rotation = rotation;
          }
        }
      }
      if (!this.CameraManager.ActionRunning)
        return;
      this.lastCamPos = b;
    }

    private void UpdateTimeOfDay()
    {
      if (this.LevelManager.Sky == null)
        return;
      this.backgroundMatrix.M11 = 0.0001f;
      this.backgroundMatrix.M31 = this.TimeManager.DayFraction;
      if (this.fogColors != null)
      {
        this.FogManager.Color = this.CurrentFogColor;
        foreach (Group group in this.BgLayers.Groups)
          group.Material.Diffuse = Vector3.Lerp(this.CurrentCloudTint.ToVector3(), this.FogManager.Color.ToVector3(), this.LevelManager.Sky.Layers[((SkyHost.BgLayerState) group.CustomData).Layer].FogTint);
      }
      this.stars.Material.Opacity = this.LevelManager.Rainy || this.LevelManager.Sky.Name == "PYRAMID_SKY" || this.LevelManager.Sky.Name == "ABOVE" ? 1f : (this.LevelManager.Sky.Name == "OBS_SKY" ? MathHelper.Lerp(this.TimeManager.NightContribution, 1f, 0.25f) : this.TimeManager.NightContribution);
    }

    private void ResizeStars()
    {
      if (this.LevelManager.Sky == null || this.stars.TextureMap == null || !this.LevelManager.Rainy && !(this.LevelManager.Sky.Name == "ABOVE") && (!(this.LevelManager.Sky.Name == "PYRAMID_SKY") && !(this.LevelManager.Sky.Name == "OBS_SKY")) && (double) this.TimeManager.NightContribution == 0.0)
        return;
      if ((double) this.stars.Material.Opacity > 0.0)
        this.stars.Position = this.CameraManager.Position - this.CameraManager.ViewOffset;
      if (this.EngineState.FarawaySettings.InTransition)
      {
        float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
        if (!this.stars.Effect.ForcedProjectionMatrix.HasValue)
        {
          this.sideToSwap = (int) FezMath.GetOpposite(FezMath.VisibleOrientation(this.CameraManager.Viewpoint));
          if (this.sideToSwap > 1)
            --this.sideToSwap;
          if (this.sideToSwap == 4)
            --this.sideToSwap;
        }
        float num1 = (float) this.GraphicsDevice.Viewport.Width / (this.CameraManager.PixelsPerTrixel * 16f);
        float num2;
        if ((double) this.EngineState.FarawaySettings.OriginFadeOutStep == 1.0)
        {
          float amount = Easing.EaseInOut(((double) this.EngineState.FarawaySettings.TransitionStep - (double) this.startStep) / (1.0 - (double) this.startStep), EasingType.Sine);
          num2 = num1 = MathHelper.Lerp(num1, this.EngineState.FarawaySettings.DestinationRadius, amount);
        }
        else
          num2 = num1;
        this.stars.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(num1 / viewScale, num1 / this.CameraManager.AspectRatio / viewScale, this.CameraManager.NearPlane, this.CameraManager.FarPlane));
        int index = (int) FezMath.GetOpposite(FezMath.VisibleOrientation(this.CameraManager.Viewpoint));
        if (index > 1)
          --index;
        if (index == 4)
          --index;
        if (index != this.sideToSwap)
        {
          float num3 = SkyHost.starSideOffsets[index];
          SkyHost.starSideOffsets[index] = SkyHost.starSideOffsets[this.sideToSwap];
          SkyHost.starSideOffsets[this.sideToSwap] = num3;
          this.sideToSwap = index;
        }
        this.stars.Scale = new Vector3(1f, 5f, 1f) * num2 * 2f;
      }
      else
      {
        if (this.waiter == null && this.stars.Effect.ForcedProjectionMatrix.HasValue)
          this.waiter = Waiters.Wait(1.0, (Action) (() =>
          {
            this.stars.Effect.ForcedProjectionMatrix = new Matrix?();
            this.waiter = (IWaiter) null;
          }));
        this.stars.Scale = new Vector3(1f, 5f, 1f) * (float) ((double) this.CameraManager.Radius * 2.0 + (double) Easing.EaseOut(this.CameraManager.ProjectionTransition ? (FezMath.IsOrthographic(this.CameraManager.Viewpoint) ? (double) (1f - this.CameraManager.ViewTransitionStep) : (double) this.CameraManager.ViewTransitionStep) : 1.0, EasingType.Quintic) * 40.0);
      }
      int num4 = 0;
      foreach (Group group in this.stars.Groups)
      {
        float m11 = this.stars.Scale.X / ((float) this.stars.TextureMap.Width / 16f);
        float m22 = this.stars.Scale.Y / ((float) this.stars.TextureMap.Height / 16f);
        float num1 = SkyHost.starSideOffsets[num4++];
        group.TextureMatrix.Set(new Matrix?(new Matrix(m11, 0.0f, 0.0f, 0.0f, 0.0f, m22, 0.0f, 0.0f, num1 - m11 / 2f, num1 - m22 / 2f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f)));
      }
    }

    private void ResizeLayers()
    {
      if (this.BgLayers == null || this.BgLayers.Groups.Count == 0 || (this.EngineState.SkyRender || this.LevelManager.Sky == null))
        return;
      float num1 = 0.0f;
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      if (this.EngineState.FarawaySettings.InTransition)
      {
        float num2 = (float) this.GraphicsDevice.Viewport.Width / (this.CameraManager.PixelsPerTrixel * 16f);
        if ((double) this.EngineState.FarawaySettings.OriginFadeOutStep == 1.0)
        {
          float num3 = this.startStep;
          if ((double) num3 == 0.0)
            num3 = 0.1275f;
          float amount = Easing.EaseInOut(((double) this.EngineState.FarawaySettings.TransitionStep - (double) num3) / (1.0 - (double) num3), EasingType.Sine);
          num1 = num2 = MathHelper.Lerp(num2, this.EngineState.FarawaySettings.DestinationRadius, amount);
        }
        this.BgLayers.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(num2 / viewScale, num2 / this.CameraManager.AspectRatio / viewScale, this.CameraManager.NearPlane, this.CameraManager.FarPlane));
      }
      else if (this.BgLayers.Effect.ForcedProjectionMatrix.HasValue)
        this.BgLayers.Effect.ForcedProjectionMatrix = new Matrix?();
      Vector3 vector3 = new Vector3(this.CameraManager.InterpolatedCenter.X, this.CameraManager.Position.Y, this.CameraManager.InterpolatedCenter.Z);
      if (this.EngineState.FarawaySettings.InTransition)
        vector3 = this.BgLayers.Position = this.CameraManager.Position;
      float num4;
      float num5;
      if (this.EngineState.FarawaySettings.InTransition && (double) this.EngineState.FarawaySettings.OriginFadeOutStep == 1.0)
      {
        float num2 = this.CameraManager.PixelsPerTrixel;
        if (this.EngineState.FarawaySettings.InTransition && FezMath.AlmostEqual(this.EngineState.FarawaySettings.DestinationCrossfadeStep, 1f))
          num2 = MathHelper.Lerp(this.CameraManager.PixelsPerTrixel, this.EngineState.FarawaySettings.DestinationPixelsPerTrixel, (float) (((double) this.EngineState.FarawaySettings.TransitionStep - 0.875) / 0.125));
        float num3 = (float) ((double) (-4 * (this.LevelManager.Descending ? -1 : 1)) / (double) num2 - 15.0 / 32.0 + 1.0);
        num4 = -this.EngineState.FarawaySettings.DestinationOffset.X;
        num5 = -this.EngineState.FarawaySettings.DestinationOffset.Y + num3;
        if (!this.EngineState.Loading)
        {
          if ((double) this.startStep2 == 0.0)
            this.startStep2 = this.EngineState.FarawaySettings.TransitionStep;
          num4 = MathHelper.Lerp(num4, FezMath.Dot(vector3 - this.LevelManager.Size / 2f, this.CameraManager.InverseView.Right), Easing.EaseInOut(((double) this.EngineState.FarawaySettings.TransitionStep - (double) this.startStep2) / (1.0 - (double) this.startStep2), EasingType.Sine));
          num5 = MathHelper.Lerp(num5, vector3.Y - this.LevelManager.Size.Y / 2f - this.CameraManager.ViewOffset.Y, Easing.EaseInOut(((double) this.EngineState.FarawaySettings.TransitionStep - (double) this.startStep2) / (1.0 - (double) this.startStep2), EasingType.Sine));
        }
      }
      else
      {
        num4 = FezMath.Dot(vector3 - this.LevelManager.Size / 2f, this.CameraManager.InverseView.Right);
        num5 = vector3.Y - this.LevelManager.Size.Y / 2f - this.CameraManager.ViewOffset.Y;
      }
      if (this.LevelManager.Sky.NoPerFaceLayerXOffset)
        this.sideOffset = num4;
      else if (this.CameraManager.ActionRunning && FezMath.IsOrthographic(this.CameraManager.Viewpoint))
      {
        if (this.sinceReached.TotalSeconds > 0.45)
          this.sideOffset -= this.lastCamSide - num4;
        this.lastCamSide = num4;
      }
      float num6 = FezMath.IsOrthographic(this.CameraManager.Viewpoint) ? 1f - this.CameraManager.ViewTransitionStep : this.CameraManager.ViewTransitionStep;
      this.BgLayers.Scale = !FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.CameraManager.ProjectionTransition ? new Vector3(1f, 5f, 1f) * (float) ((double) this.CameraManager.Radius * 2.0 + (double) Easing.EaseOut(this.CameraManager.ProjectionTransition ? (double) num6 : 1.0, EasingType.Quintic) * 40.0) : (!this.EngineState.FarawaySettings.InTransition || (double) this.EngineState.FarawaySettings.OriginFadeOutStep != 1.0 ? new Vector3(1f, 5f, 1f) * this.CameraManager.Radius * 2f / viewScale : new Vector3(1f, 5f, 1f) * num1 * 2f);
      foreach (Group group in this.BgLayers.Groups)
      {
        group.Enabled = false;
        SkyHost.BgLayerState bgLayerState = (SkyHost.BgLayerState) group.CustomData;
        float num2 = (float) bgLayerState.Layer / (this.LevelManager.Sky.Layers.Count == 1 ? 1f : (float) (this.LevelManager.Sky.Layers.Count - 1));
        int num3 = 1;
        float num7 = this.BgLayers.Scale.X / ((float) group.TextureMap.Width / 16f) / (float) num3;
        float m22 = this.BgLayers.Scale.Y / ((float) group.TextureMap.Height / 16f) / (float) num3;
        if (this.CameraManager.ProjectionTransition)
          group.Scale = Vector3.One + FezMath.XZMask * num2 * 0.125f * num6;
        Vector2 vector2_1 = new Vector2(this.sideOffset / ((float) group.TextureMap.Width / 16f), num5 / ((float) group.TextureMap.Height / 16f));
        if (this.EngineState.FarawaySettings.InTransition && (double) this.EngineState.FarawaySettings.OriginFadeOutStep != 1.0)
        {
          bgLayerState.OriginalTC = vector2_1;
          this.startStep = 0.0f;
          this.startStep2 = 0.0f;
        }
        else if (this.EngineState.FarawaySettings.InTransition && (double) this.EngineState.FarawaySettings.OriginFadeOutStep == 1.0)
        {
          if (vector2_1 != bgLayerState.OriginalTC && (double) this.startStep == 0.0)
            this.startStep = this.EngineState.FarawaySettings.TransitionStep;
          if ((double) this.startStep != 0.0)
            vector2_1 = Vector2.Lerp(bgLayerState.OriginalTC, vector2_1, Easing.EaseInOut(((double) this.EngineState.FarawaySettings.TransitionStep - (double) this.startStep) / (1.0 - (double) this.startStep), EasingType.Sine));
        }
        Vector2 vector2_2;
        vector2_2.X = (float) ((this.LevelManager.Sky.NoPerFaceLayerXOffset ? 0.0 : (double) bgLayerState.Side / 4.0) + (double) this.LevelManager.Sky.LayerBaseXOffset + (double) vector2_1.X * (double) this.LevelManager.Sky.HorizontalDistance + (double) vector2_1.X * (double) this.LevelManager.Sky.InterLayerHorizontalDistance * (double) num2 + -(double) bgLayerState.WindOffset * (double) this.LevelManager.Sky.WindDistance + -(double) bgLayerState.WindOffset * (double) this.LevelManager.Sky.WindParallax * (double) num2);
        if (!this.LevelManager.Sky.VerticalTiling)
          num2 -= 0.5f;
        vector2_2.Y = (float) ((double) this.LevelManager.Sky.LayerBaseHeight + (double) num2 * (double) this.LevelManager.Sky.LayerBaseSpacing + -(double) vector2_1.Y * (double) this.LevelManager.Sky.VerticalDistance + -(double) num2 * (double) this.LevelManager.Sky.InterLayerVerticalDistance * (double) vector2_1.Y);
        group.TextureMatrix.Set(new Matrix?(new Matrix(-num7, 0.0f, 0.0f, 0.0f, 0.0f, m22, 0.0f, 0.0f, (float) (-(double) vector2_2.X + (double) num7 / 2.0), vector2_2.Y - m22 / 2f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f)));
      }
    }

    public void DrawBackground()
    {
      this.GraphicsDevice.SamplerStates[0] = SamplerStates.LinearUWrapVClamp;
      this.TargetRenderer.DrawFullscreen((Texture) this.skyBackground, this.backgroundMatrix, new Color(this.EngineState.SkyOpacity, this.EngineState.SkyOpacity, this.EngineState.SkyOpacity, 1f));
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.EngineState.Loading && !this.EngineState.FarawaySettings.InTransition || (this.BgLayers == null || (double) this.EngineState.SkyOpacity == 0.0) || (this.LevelManager.Name == null || this.LevelManager.Sky == null || this.EngineState.InMap))
        return;
      this.ForceDraw();
    }

    public void ForceDraw()
    {
      RenderTarget2D renderTarget = (RenderTarget2D) null;
      bool flag1 = false;
      if ((double) this.EngineState.FarawaySettings.OriginFadeOutStep == 1.0 && this.RtHandle == null)
      {
        this.RtHandle = this.TargetRenderer.TakeTarget();
        renderTarget = this.GraphicsDevice.GetRenderTargets()[0].RenderTarget as RenderTarget2D;
        this.GraphicsDevice.SetRenderTarget(this.RtHandle.Target);
        this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, Color.Black, 1f, 0);
        flag1 = true;
        this.RadiusAtFirstDraw = this.CameraManager.Radius;
      }
      else if (this.RtHandle != null && !this.EngineState.FarawaySettings.InTransition)
      {
        this.TargetRenderer.ReturnTarget(this.RtHandle);
        this.RtHandle = (RenderTargetHandle) null;
      }
      this.EngineState.SkyRender = true;
      Vector3 viewOffset = this.CameraManager.ViewOffset;
      this.CameraManager.ViewOffset -= viewOffset;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Sky));
      GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Maximum);
      bool flag2 = this.LevelManager.Name == "INDUSTRIAL_CITY";
      if ((double) this.EngineState.FarawaySettings.OriginFadeOutStep < 1.0 || flag1 || (double) this.EngineState.FarawaySettings.DestinationCrossfadeStep > 0.0)
      {
        bool flag3 = (double) this.EngineState.FarawaySettings.DestinationCrossfadeStep > 0.0;
        if (!flag2 || this.flickering)
        {
          foreach (Mesh mesh in this.cloudMeshes)
          {
            if (flag3)
              mesh.Material.Opacity = this.EngineState.FarawaySettings.DestinationCrossfadeStep;
            float num = mesh.Material.Opacity;
            if ((double) this.EngineState.SkyOpacity != 1.0)
              mesh.Material.Opacity = num * this.EngineState.SkyOpacity;
            mesh.Draw();
            mesh.Material.Opacity = num;
          }
        }
      }
      if (this.RtHandle != null)
      {
        if (flag1)
        {
          this.GraphicsDevice.SetRenderTarget(renderTarget);
          this.GraphicsDevice.Clear(Color.Black);
        }
        float num1 = ((double) this.EngineState.FarawaySettings.InterpolatedFakeRadius == 0.0 ? this.RadiusAtFirstDraw : this.EngineState.FarawaySettings.InterpolatedFakeRadius) / this.RadiusAtFirstDraw;
        Matrix matrix1 = new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, -0.5f, -0.5f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
        Matrix matrix2 = new Matrix(num1, 0.0f, 0.0f, 0.0f, 0.0f, num1, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
        Matrix matrix3 = new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.5f, 0.5f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
        float num2 = num1 * (1f - this.EngineState.FarawaySettings.DestinationCrossfadeStep);
        this.TargetRenderer.DrawFullscreen((Texture) this.RtHandle.Target, matrix1 * matrix2 * matrix3, new Color(num2, num2, num2));
      }
      if (!flag2 || this.flickering)
      {
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Screen);
        this.DrawBackground();
      }
      if (this.stars.TextureMap != null && (double) this.stars.Material.Opacity > 0.0)
      {
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.StarsOverClouds);
        float num = this.stars.Material.Opacity;
        this.stars.Material.Opacity = num * this.EngineState.SkyOpacity;
        this.stars.Draw();
        this.stars.Material.Opacity = num;
      }
      this.GraphicsDevice.SamplerStates[0] = this.LevelManager.Sky.VerticalTiling ? SamplerState.PointWrap : SamplerStates.PointUWrapVClamp;
      GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Alphablending);
      if (this.LevelManager.Rainy || this.LevelManager.Sky.Name == "WATERFRONT")
      {
        int count = this.BgLayers.Groups.Count;
        int num1 = count / 3;
        for (int index1 = 0; index1 < 3; ++index1)
        {
          for (int index2 = index1 * num1; index2 < count && index2 < (index1 + 1) * num1; ++index2)
          {
            Group group = this.BgLayers.Groups[index2];
            bool? alwaysOnTop = this.BgLayers.Groups[index2].AlwaysOnTop;
            int num2 = (alwaysOnTop.HasValue ? (alwaysOnTop.GetValueOrDefault() ? 1 : 0) : 0) == 0 ? 1 : 0;
            group.Enabled = num2 != 0;
          }
          GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?((StencilMask) (5 + index1)));
          this.BgLayers.Draw();
          for (int index2 = index1 * num1; index2 < count && index2 < (index1 + 1) * num1; ++index2)
            this.BgLayers.Groups[index2].Enabled = false;
        }
      }
      else
      {
        if (this.LevelManager.Name != null && (this.LevelManager.BlinkingAlpha || this.LevelManager.WaterType == LiquidType.Sewer && this.EngineState.StereoMode))
        {
          GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.SkyLayer1));
          GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.None);
          this.LevelMaterializer.StaticPlanesMesh.AlwaysOnTop = true;
          this.LevelMaterializer.StaticPlanesMesh.DepthWrites = false;
          foreach (BackgroundPlane backgroundPlane in this.LevelMaterializer.LevelPlanes)
            backgroundPlane.Group.Enabled = backgroundPlane.Id < 0;
          this.LevelMaterializer.StaticPlanesMesh.Draw();
          this.LevelMaterializer.StaticPlanesMesh.AlwaysOnTop = false;
          this.LevelMaterializer.StaticPlanesMesh.DepthWrites = true;
          foreach (BackgroundPlane backgroundPlane in this.LevelMaterializer.LevelPlanes)
            backgroundPlane.Group.Enabled = true;
          GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.All);
          GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Equal, StencilMask.SkyLayer1);
          GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Alphablending);
          this.GraphicsDevice.SamplerStates[0] = this.LevelManager.Sky.VerticalTiling ? SamplerState.PointWrap : SamplerStates.PointUWrapVClamp;
        }
        foreach (Group group1 in this.BgLayers.Groups)
        {
          Group group2 = group1;
          bool? alwaysOnTop = group1.AlwaysOnTop;
          int num = (alwaysOnTop.HasValue ? (alwaysOnTop.GetValueOrDefault() ? 1 : 0) : 0) == 0 ? 1 : 0;
          group2.Enabled = num != 0;
        }
        this.BgLayers.Draw();
        GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
      }
      this.CameraManager.ViewOffset += viewOffset;
      this.EngineState.SkyRender = false;
    }

    private class CloudState
    {
      public float Phi;
      public float LocalHeightOffset;
      public float GlobalHeightOffset;
      public Group Group;

      public float GetHeight(float spreadFactor)
      {
        return this.LocalHeightOffset * spreadFactor + this.GlobalHeightOffset;
      }
    }

    private class BgLayerState
    {
      public int Layer;
      public int Side;
      public float WindOffset;
      public Vector2 OriginalTC;
      public float OriginalOpacity;
    }
  }
}
