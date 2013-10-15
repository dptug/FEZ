// Type: FezEngine.Components.FarawayPlaceHost
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Effects;
using FezEngine.Readers;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Components
{
  public class FarawayPlaceHost : DrawableGameComponent
  {
    private static readonly object FarawayWaterMutex = new object();
    private static readonly object FarawayPlaceMutex = new object();
    private const float Visibility = 0.125f;
    private Mesh ThisLevelMesh;
    private Mesh LastLevelMesh;
    private Mesh NextLevelMesh;
    private Mesh FarawayWaterMesh;
    private Mesh LastWaterMesh;
    private float OriginalFakeRadius;
    private float DestinationFakeRadius;
    private float FakeRadius;
    private bool IsFake;
    private FarawayPlaceHost.PlaceFader Fader;
    private Vector3 waterRightVector;
    private Texture2D HorizontalGradientTex;
    private bool hasntSnapped;

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IFogManager FogManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    static FarawayPlaceHost()
    {
    }

    public FarawayPlaceHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      Material material = new Material();
      this.LastLevelMesh = new Mesh()
      {
        Effect = (BaseEffect) new FarawayEffect(),
        DepthWrites = false,
        Material = material,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerState.PointClamp
      };
      this.ThisLevelMesh = new Mesh()
      {
        Effect = (BaseEffect) new FarawayEffect(),
        DepthWrites = false,
        Material = material,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerState.PointClamp
      };
      this.NextLevelMesh = new Mesh()
      {
        Effect = (BaseEffect) new FarawayEffect(),
        DepthWrites = false,
        Material = material,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerState.PointClamp
      };
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      ServiceHelper.AddComponent((IGameComponent) (this.Fader = new FarawayPlaceHost.PlaceFader(this.Game)));
      this.Fader.Visible = false;
    }

    private void TryInitialize()
    {
      this.ThisLevelMesh.ClearGroups();
      this.NextLevelMesh.ClearGroups();
      lock (FarawayPlaceHost.FarawayWaterMutex)
        this.FarawayWaterMesh = (Mesh) null;
      (this.NextLevelMesh.Effect as FarawayEffect).CleanUp();
      foreach (Script script1 in Enumerable.Where<Script>((IEnumerable<Script>) this.LevelManager.Scripts.Values, (Func<Script, bool>) (x => Enumerable.Any<ScriptAction>((IEnumerable<ScriptAction>) x.Actions, (Func<ScriptAction, bool>) (y => y.Operation == "ChangeToFarAwayLevel")))))
      {
        Volume volume1;
        if (this.LevelManager.Volumes.TryGetValue(Enumerable.FirstOrDefault<ScriptTrigger>((IEnumerable<ScriptTrigger>) script1.Triggers, (Func<ScriptTrigger, bool>) (x =>
        {
          if (x.Object.Type == "Volume")
            return x.Event == "Enter";
          else
            return false;
        })).Object.Identifier.Value, out volume1))
        {
          FaceOrientation faceOrientation = Enumerable.FirstOrDefault<FaceOrientation>((IEnumerable<FaceOrientation>) volume1.Orientations);
          ScriptAction scriptAction1 = Enumerable.FirstOrDefault<ScriptAction>((IEnumerable<ScriptAction>) script1.Actions, (Func<ScriptAction, bool>) (x => x.Operation == "ChangeToFarAwayLevel"));
          string str1 = scriptAction1.Arguments[0];
          int num1 = 0;
          string songName;
          FaceOrientation orientation;
          Vector2 vector2;
          bool flag;
          float num2;
          float num3;
          float num4;
          using (MemoryContentManager memoryContentManager = new MemoryContentManager((IServiceProvider) this.Game.Services, this.Game.Content.RootDirectory))
          {
            string str2 = str1;
            if (!MemoryContentManager.AssetExists("Levels" + (object) '\\' + str1.Replace('/', '\\')))
              str2 = this.LevelManager.FullPath.Substring(0, this.LevelManager.FullPath.LastIndexOf("/") + 1) + str1.Substring(str1.LastIndexOf("/") + 1);
            LevelReader.MinimalRead = true;
            Level level;
            try
            {
              level = memoryContentManager.Load<Level>("Levels/" + str2);
            }
            catch (Exception ex)
            {
              Logger.Log("FarawayPlaceHost", Common.LogSeverity.Warning, "Couldn't load faraway place destination level : " + str1);
              continue;
            }
            LevelReader.MinimalRead = false;
            songName = level.SongName;
            int key;
            try
            {
              key = int.Parse(scriptAction1.Arguments[1]);
            }
            catch (Exception ex)
            {
              key = -1;
            }
            Volume volume2 = key == -1 || !level.Volumes.ContainsKey(key) ? level.Volumes[Enumerable.First<ScriptTrigger>((IEnumerable<ScriptTrigger>) Enumerable.First<Script>((IEnumerable<Script>) level.Scripts.Values, (Func<Script, bool>) (s => Enumerable.Any<ScriptAction>((IEnumerable<ScriptAction>) s.Actions, (Func<ScriptAction, bool>) (a =>
            {
              if (a.Object.Type == "Level" && a.Operation.Contains("Level"))
                return a.Arguments[0] == this.LevelManager.Name;
              else
                return false;
            })))).Triggers, (Func<ScriptTrigger, bool>) (t =>
            {
              if (t.Object.Type == "Volume")
                return t.Event == "Enter";
              else
                return false;
            })).Object.Identifier.Value] : level.Volumes[key];
            orientation = Enumerable.FirstOrDefault<FaceOrientation>((IEnumerable<FaceOrientation>) volume2.Orientations);
            Vector3 vector3 = (level.Size / 2f - (volume2.From + volume2.To) / 2f) * (FezMath.RightVector(FezMath.AsViewpoint(orientation)) + Vector3.Up);
            vector2 = new Vector2(vector3.X + vector3.Z, vector3.Y);
            flag = level.WaterType != LiquidType.None;
            num2 = level.WaterHeight - (volume2.From + volume2.To).Y / 2f + this.EngineState.WaterLevelOffset;
            num3 = this.LevelManager.WaterHeight - (volume1.From + volume1.To).Y / 2f - num2 / 4f;
            num4 = level.Size.Y;
            Script script2 = Enumerable.FirstOrDefault<Script>((IEnumerable<Script>) level.Scripts.Values, (Func<Script, bool>) (s =>
            {
              if (Enumerable.Any<ScriptTrigger>((IEnumerable<ScriptTrigger>) s.Triggers, (Func<ScriptTrigger, bool>) (t =>
              {
                if (t.Event == "Start")
                  return t.Object.Type == "Level";
                else
                  return false;
              })))
                return Enumerable.Any<ScriptAction>((IEnumerable<ScriptAction>) s.Actions, (Func<ScriptAction, bool>) (a =>
                {
                  if (a.Object.Type == "Camera")
                    return a.Operation == "SetPixelsPerTrixel";
                  else
                    return false;
                }));
              else
                return false;
            }));
            if (script2 != null)
            {
              ScriptAction scriptAction2 = Enumerable.First<ScriptAction>((IEnumerable<ScriptAction>) script2.Actions, (Func<ScriptAction, bool>) (a =>
              {
                if (a.Object.Type == "Camera")
                  return a.Operation == "SetPixelsPerTrixel";
                else
                  return false;
              }));
              try
              {
                num1 = int.Parse(scriptAction2.Arguments[0]);
              }
              catch (Exception ex)
              {
              }
            }
            num2 = level.WaterHeight;
          }
          Texture2D texture;
          try
          {
            string assetName = "Other Textures/faraway_thumbs/" + (object) str1 + " (" + (string) (object) FezMath.AsViewpoint(orientation) + ")";
            texture = this.CMProvider.CurrentLevel.Load<Texture2D>(assetName);
            texture.Name = assetName;
          }
          catch (Exception ex)
          {
            Logger.Log("FarawayPlacesHost", Common.LogSeverity.Warning, "Couldn't load faraway thumbnail for " + (object) str1 + " (viewpoint = " + (string) (object) FezMath.AsViewpoint(orientation) + ")");
            continue;
          }
          if (!Enumerable.Any<Group>((IEnumerable<Group>) this.ThisLevelMesh.Groups, (Func<Group, bool>) (x => x.Texture == texture)))
          {
            if (num1 == 0)
              num1 = (int) this.CameraManager.PixelsPerTrixel;
            Group group1 = this.ThisLevelMesh.AddFace(new Vector3((float) texture.Width, (float) texture.Height, (float) texture.Width) / 16f / 2f, Vector3.Zero, faceOrientation, true);
            Group group2 = this.NextLevelMesh.AddFace(new Vector3((float) texture.Width, (float) texture.Height, (float) texture.Width) / 16f / 2f, Vector3.Zero, faceOrientation, true);
            FarawayPlaceData farawayPlaceData = new FarawayPlaceData()
            {
              OriginalCenter = (volume1.From + volume1.To) / 2f,
              Viewpoint = FezMath.AsViewpoint(faceOrientation),
              Volume = volume1,
              DestinationOffset = vector2.X * FezMath.RightVector(FezMath.AsViewpoint(faceOrientation)) + Vector3.Up * vector2.Y,
              WaterLevelOffset = new float?(num3),
              DestinationLevelName = str1,
              DestinationWaterLevel = num2,
              DestinationLevelSize = num4
            };
            if (this.LevelManager.WaterType == LiquidType.None && flag)
            {
              if (this.HorizontalGradientTex == null || this.HorizontalGradientTex.IsDisposed)
                this.HorizontalGradientTex = this.CMProvider.Global.Load<Texture2D>("Other Textures/WaterHorizGradient");
              lock (FarawayPlaceHost.FarawayWaterMutex)
              {
                FarawayPlaceHost temp_380 = this;
                // ISSUE: explicit reference operation
                // ISSUE: variable of a reference type
                FarawayPlaceData& temp_381 = @farawayPlaceData;
                Mesh local_28 = new Mesh();
                Mesh temp_383 = local_28;
                DefaultEffect.Textured local_29 = new DefaultEffect.Textured();
                local_29.AlphaIsEmissive = false;
                DefaultEffect.Textured temp_387 = local_29;
                temp_383.Effect = (BaseEffect) temp_387;
                Mesh temp_388;
                Mesh local_43 = temp_388 = local_28;
                // ISSUE: explicit reference operation
                (^temp_381).WaterBodyMesh = temp_388;
                Mesh temp_389 = local_43;
                temp_380.FarawayWaterMesh = temp_389;
                this.FarawayWaterMesh.AddFace(Vector3.One, new Vector3(-0.5f, -1f, -0.5f) + FezMath.Abs(FezMath.AsVector(faceOrientation)) * 0.5f, faceOrientation, false).Material = new Material();
                this.FarawayWaterMesh.AddFace(Vector3.One, new Vector3(-0.5f, -1f, -0.5f) + FezMath.Abs(FezMath.AsVector(faceOrientation)) * 0.5f, faceOrientation, false).Material = new Material();
              }
            }
            group2.CustomData = group1.CustomData = (object) farawayPlaceData;
            group2.Position = group1.Position = farawayPlaceData.OriginalCenter;
            group2.Texture = group1.Texture = (Texture) texture;
            group2.Material = new Material()
            {
              Opacity = 0.125f
            };
            group1.Material = new Material()
            {
              Opacity = 0.125f
            };
            if (volume1.ActorSettings == null)
              volume1.ActorSettings = new VolumeActorSettings();
            volume1.ActorSettings.DestinationSong = songName;
            switch (num1)
            {
              case 1:
                volume1.ActorSettings.DestinationRadius = 80f;
                break;
              case 2:
                volume1.ActorSettings.DestinationRadius = 40f;
                break;
              case 3:
                volume1.ActorSettings.DestinationRadius = 26.66667f;
                break;
              case 4:
                volume1.ActorSettings.DestinationRadius = 20f;
                break;
              case 5:
                volume1.ActorSettings.DestinationRadius = 16f;
                break;
            }
            volume1.ActorSettings.DestinationPixelsPerTrixel = (float) num1;
            volume1.ActorSettings.DestinationOffset = vector2;
          }
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Paused || this.EngineState.InMap || (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.CameraManager.ProjectionTransition))
        return;
      if (this.EngineState.FarawaySettings.InTransition && (double) this.EngineState.FarawaySettings.OriginFadeOutStep == 1.0 && !this.IsFake)
      {
        for (int i = this.NextLevelMesh.Groups.Count - 1; i >= 0; --i)
        {
          try
          {
            if (i < this.NextLevelMesh.Groups.Count)
            {
              Group group = this.NextLevelMesh.Groups[i];
              FarawayPlaceData farawayPlaceData = (FarawayPlaceData) group.CustomData;
              string levelName = this.LevelManager.Name.Substring(0, this.LevelManager.Name.LastIndexOf("/") + 1) + farawayPlaceData.DestinationLevelName;
              if (this.CameraManager.Viewpoint != farawayPlaceData.Viewpoint)
              {
                this.NextLevelMesh.RemoveGroupAt(i);
                (this.NextLevelMesh.Effect as FarawayEffect).CleanUp();
              }
              else
                this.CMProvider.GetForLevel(levelName).Load<Texture2D>(group.Texture.Name);
            }
          }
          catch (Exception ex)
          {
          }
        }
        this.hasntSnapped = true;
        Mesh mesh = this.NextLevelMesh;
        this.NextLevelMesh = this.LastLevelMesh;
        this.LastLevelMesh = mesh;
        this.LastWaterMesh = this.FarawayWaterMesh;
        this.ThisLevelMesh.ClearGroups();
        this.OriginalFakeRadius = (float) this.GraphicsDevice.Viewport.Width / (this.CameraManager.PixelsPerTrixel * 16f);
        this.DestinationFakeRadius = this.EngineState.FarawaySettings.DestinationRadius / 4f;
        this.EngineState.FarawaySettings.InterpolatedFakeRadius = this.CameraManager.Radius;
        this.LastLevelMesh.Effect.ForcedViewMatrix = new Matrix?(this.CameraManager.View);
        (this.LastLevelMesh.Effect as FarawayEffect).ActualOpacity = 1f;
        if (this.LastWaterMesh != null && this.LastWaterMesh.Groups.Count > 0)
        {
          this.LastWaterMesh.Effect.ForcedViewMatrix = new Matrix?(this.CameraManager.View);
          try
          {
            this.LastWaterMesh.Groups[0].Material.Opacity = this.LastWaterMesh.Groups[1].Material.Opacity = 1f;
          }
          catch (Exception ex)
          {
          }
          lock (FarawayPlaceHost.FarawayWaterMutex)
            this.FarawayWaterMesh = (Mesh) null;
        }
        this.IsFake = true;
        this.Fader.PlacesMesh = this.LastLevelMesh;
        this.Fader.FarawayWaterMesh = this.LastWaterMesh;
        this.LastLevelMesh.AlwaysOnTop = true;
        if (this.LastWaterMesh != null)
          this.LastWaterMesh.AlwaysOnTop = true;
        this.EngineState.FarawaySettings.LoadingAllowed = true;
      }
      if (!this.EngineState.FarawaySettings.InTransition && this.IsFake)
      {
        this.IsFake = false;
        this.Fader.Visible = false;
        lock (FarawayPlaceHost.FarawayPlaceMutex)
          this.Fader.PlacesMesh = (Mesh) null;
        lock (FarawayPlaceHost.FarawayWaterMutex)
          this.Fader.FarawayWaterMesh = (Mesh) null;
      }
      if (this.EngineState.FarawaySettings.InTransition)
      {
        float amount = this.EngineState.FarawaySettings.TransitionStep;
        this.Fader.Visible = true;
        float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
        if (this.IsFake)
        {
          this.FakeRadius = MathHelper.Lerp(this.OriginalFakeRadius, this.DestinationFakeRadius, amount);
          this.EngineState.FarawaySettings.InterpolatedFakeRadius = MathHelper.Lerp(this.EngineState.FarawaySettings.InterpolatedFakeRadius, this.FakeRadius, MathHelper.Clamp((float) gameTime.ElapsedGameTime.TotalSeconds * this.CameraManager.InterpolationSpeed, 0.0f, 1f));
          this.LastLevelMesh.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(this.EngineState.FarawaySettings.InterpolatedFakeRadius / viewScale, this.EngineState.FarawaySettings.InterpolatedFakeRadius / this.CameraManager.AspectRatio / viewScale, this.CameraManager.NearPlane, this.CameraManager.FarPlane));
          if (this.LastWaterMesh != null)
            this.LastWaterMesh.Effect.ForcedProjectionMatrix = this.LastLevelMesh.Effect.ForcedProjectionMatrix;
          this.EngineState.SkipRendering = true;
          this.CameraManager.Radius = this.FakeRadius * 4f;
          (this.ThisLevelMesh.Effect as FarawayEffect).ActualOpacity = (float) (((double) amount - 0.5) * 2.0);
          (this.LastLevelMesh.Effect as FarawayEffect).ActualOpacity = 1f - this.EngineState.FarawaySettings.DestinationCrossfadeStep;
          try
          {
            if (this.FarawayWaterMesh != null)
            {
              lock (FarawayPlaceHost.FarawayWaterMutex)
              {
                if ((double) amount > 0.5)
                {
                  this.FarawayWaterMesh.Groups[0].Material.Opacity = (float) (((double) amount - 0.5) * 2.0);
                  this.FarawayWaterMesh.Groups[1].Material.Opacity = (float) (((double) amount - 0.5) * 2.0);
                }
                else
                  this.FarawayWaterMesh.Groups[0].Material.Opacity = this.FarawayWaterMesh.Groups[1].Material.Opacity = 0.0f;
              }
            }
            else if (this.LastWaterMesh != null)
            {
              lock (FarawayPlaceHost.FarawayWaterMutex)
              {
                this.LastWaterMesh.Groups[0].Material.Opacity = 1f - this.EngineState.FarawaySettings.DestinationCrossfadeStep;
                this.LastWaterMesh.Groups[1].Material.Opacity = 1f - this.EngineState.FarawaySettings.DestinationCrossfadeStep;
                this.LastWaterMesh.Groups[0].Material.Diffuse = Vector3.Lerp(this.FogManager.Color.ToVector3(), this.EngineState.WaterBodyColor * this.LevelManager.ActualDiffuse.ToVector3(), (float) ((double) Easing.EaseIn((double) amount, EasingType.Sine) * 0.875 + 0.125));
                this.LastWaterMesh.Groups[1].Material.Diffuse = Vector3.Lerp(this.FogManager.Color.ToVector3(), this.EngineState.WaterFoamColor * this.LevelManager.ActualDiffuse.ToVector3(), (float) ((double) Easing.EaseIn((double) amount, EasingType.Sine) * 0.875 + 0.125));
              }
            }
          }
          catch (Exception ex)
          {
          }
          if ((double) this.EngineState.FarawaySettings.DestinationCrossfadeStep == 0.0 && !this.hasntSnapped)
          {
            this.hasntSnapped = false;
            this.CameraManager.SnapInterpolation();
          }
          this.EngineState.SkipRendering = false;
          foreach (Group group in this.LastLevelMesh.Groups)
            group.Material.Opacity = (float) ((double) Easing.EaseIn((double) amount, EasingType.Sine) * 0.875 + 0.125);
        }
        else
        {
          foreach (Group group in this.ThisLevelMesh.Groups)
            group.Material.Opacity = (float) ((double) Easing.EaseIn((double) amount, EasingType.Sine) * 0.875 + 0.125);
        }
      }
      if (this.EngineState.Loading)
        return;
      for (int index = 0; index < this.ThisLevelMesh.Groups.Count; ++index)
      {
        Group group1 = this.ThisLevelMesh.Groups[index];
        Group group2 = this.NextLevelMesh.Groups[index];
        FarawayPlaceData farawayPlaceData = (FarawayPlaceData) this.ThisLevelMesh.Groups[index].CustomData;
        Vector2 vector2 = farawayPlaceData.Volume.ActorSettings == null ? Vector2.Zero : farawayPlaceData.Volume.ActorSettings.FarawayPlaneOffset;
        bool flag = farawayPlaceData.Volume.ActorSettings != null && farawayPlaceData.Volume.ActorSettings.WaterLocked;
        float num1 = this.CameraManager.PixelsPerTrixel;
        if (this.EngineState.FarawaySettings.InTransition && FezMath.AlmostEqual(this.EngineState.FarawaySettings.DestinationCrossfadeStep, 1f))
          num1 = MathHelper.Lerp(this.CameraManager.PixelsPerTrixel, this.EngineState.FarawaySettings.DestinationPixelsPerTrixel, (float) (((double) this.EngineState.FarawaySettings.TransitionStep - 0.875) / 0.125));
        float num2 = (float) ((double) (-4 * (this.LevelManager.Descending ? -1 : 1)) / (double) num1 - 15.0 / 32.0 + 1.0);
        Vector3 vector3 = this.CameraManager.InterpolatedCenter - farawayPlaceData.OriginalCenter + num2 * Vector3.UnitY;
        float num3 = (float) (FarawayPlaceHost.GetCustomOffset((double) num1) * (this.LevelManager.Descending ? -1.0 : 1.0) + 15.0 / 32.0);
        float num4 = 0.0f;
        if (flag && farawayPlaceData.WaterLevelOffset.HasValue)
        {
          vector3 *= FezMath.XZMask;
          vector2 *= Vector2.UnitX;
          num4 = (float) ((double) farawayPlaceData.WaterLevelOffset.Value - (double) num3 / 4.0 - 0.5 + 0.125);
          farawayPlaceData.Volume.ActorSettings.WaterOffset = num4;
        }
        group1.Position = group2.Position = (farawayPlaceData.OriginalCenter + (farawayPlaceData.DestinationOffset + num3 * Vector3.UnitY) / 4f) * FezMath.ScreenSpaceMask(farawayPlaceData.Viewpoint) + FezMath.DepthMask(farawayPlaceData.Viewpoint) * this.CameraManager.InterpolatedCenter + FezMath.ForwardVector(farawayPlaceData.Viewpoint) * 30f + FezMath.RightVector(farawayPlaceData.Viewpoint) * vector2.X + Vector3.Up * vector2.Y + vector3 * FezMath.ScreenSpaceMask(farawayPlaceData.Viewpoint) / 2f + num4 * Vector3.UnitY;
        if (farawayPlaceData.WaterBodyMesh != null && farawayPlaceData.WaterBodyMesh.Groups.Count > 0)
        {
          this.waterRightVector = FezMath.RightVector(farawayPlaceData.Viewpoint);
          farawayPlaceData.WaterBodyMesh.Position = group1.Position * (FezMath.DepthMask(farawayPlaceData.Viewpoint) + Vector3.UnitY) + this.CameraManager.InterpolatedCenter * FezMath.SideMask(farawayPlaceData.Viewpoint) + ((float) ((double) farawayPlaceData.DestinationWaterLevel - (double) farawayPlaceData.DestinationLevelSize / 2.0 - 0.5) + this.EngineState.WaterLevelOffset) * Vector3.UnitY / 4f;
          farawayPlaceData.WaterBodyMesh.Groups[0].Scale = new Vector3(this.CameraManager.Radius);
          farawayPlaceData.WaterBodyMesh.Groups[0].Material.Diffuse = Vector3.Lerp(this.EngineState.WaterBodyColor * this.LevelManager.ActualDiffuse.ToVector3(), this.FogManager.Color.ToVector3(), 0.875f);
          farawayPlaceData.WaterBodyMesh.Groups[1].Scale = new Vector3(this.CameraManager.Radius, 1.0 / 16.0, this.CameraManager.Radius);
          farawayPlaceData.WaterBodyMesh.Groups[1].Material.Diffuse = Vector3.Lerp(this.EngineState.WaterFoamColor * this.LevelManager.ActualDiffuse.ToVector3(), this.FogManager.Color.ToVector3(), 0.875f);
        }
      }
      this.LastLevelMesh.Material.Diffuse = this.FogManager.Color.ToVector3();
    }

    private static double GetCustomOffset(double pixelsPerTrixel)
    {
      return 0.0 + 12.0 * Math.Pow(pixelsPerTrixel, -1.0) - 2.0;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.EngineState.Loading || this.EngineState.InMap)
        return;
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.FarawayPlaces));
      this.ThisLevelMesh.Draw();
      lock (FarawayPlaceHost.FarawayWaterMutex)
      {
        if (this.FarawayWaterMesh != null)
        {
          this.FarawayWaterMesh.Draw();
          Vector3 local_0 = this.FarawayWaterMesh.Position;
          Vector3 local_1 = this.FarawayWaterMesh.Scale;
          this.FarawayWaterMesh.Blending = new BlendingMode?(BlendingMode.Alphablending);
          this.FarawayWaterMesh.SamplerState = SamplerState.LinearClamp;
          this.FarawayWaterMesh.Texture = (Dirtyable<Texture>) ((Texture) this.HorizontalGradientTex);
          this.FarawayWaterMesh.Position -= Math.Abs(this.FarawayWaterMesh.Groups[0].Scale.X) * this.waterRightVector;
          this.FarawayWaterMesh.Draw();
          this.FarawayWaterMesh.Scale = FezMath.Abs(this.waterRightVector) * -1f + Vector3.One - FezMath.Abs(this.waterRightVector);
          this.FarawayWaterMesh.Culling = CullMode.CullClockwiseFace;
          this.FarawayWaterMesh.Position += Math.Abs(this.FarawayWaterMesh.Groups[0].Scale.X) * this.waterRightVector * 2f;
          this.FarawayWaterMesh.Draw();
          this.FarawayWaterMesh.Culling = CullMode.CullCounterClockwiseFace;
          this.FarawayWaterMesh.Position = local_0;
          this.FarawayWaterMesh.Scale = local_1;
          this.FarawayWaterMesh.Texture = (Dirtyable<Texture>) null;
        }
      }
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
    }

    private class PlaceFader : DrawableGameComponent
    {
      private CombineEffect CombineEffect;
      private RenderTargetHandle rth;

      public Mesh PlacesMesh { private get; set; }

      public Mesh FarawayWaterMesh { private get; set; }

      [ServiceDependency]
      public ITargetRenderingManager TargetRenderer { get; set; }

      [ServiceDependency]
      public IEngineStateManager GameState { get; set; }

      public PlaceFader(Game game)
        : base(game)
      {
        this.DrawOrder = 1001;
      }

      protected override void LoadContent()
      {
        this.CombineEffect = new CombineEffect()
        {
          RedGamma = 1f
        };
      }

      public override void Update(GameTime gameTime)
      {
        if (this.GameState.StereoMode != (this.DrawOrder == 1002))
        {
          this.DrawOrder = this.GameState.StereoMode ? 1002 : 1001;
          this.OnDrawOrderChanged((object) this, EventArgs.Empty);
        }
        if (this.Visible)
        {
          if (this.GameState.StereoMode && this.rth == null)
          {
            this.rth = this.TargetRenderer.TakeTarget();
            this.TargetRenderer.ScheduleHook(this.DrawOrder, this.rth.Target);
          }
          else if (this.rth != null && !this.GameState.StereoMode)
          {
            this.TargetRenderer.ReturnTarget(this.rth);
            this.rth = (RenderTargetHandle) null;
          }
        }
        if (this.Visible || this.rth == null)
          return;
        this.TargetRenderer.ReturnTarget(this.rth);
        this.rth = (RenderTargetHandle) null;
      }

      protected override void Dispose(bool disposing)
      {
        base.Dispose(disposing);
        if (this.rth == null)
          return;
        this.TargetRenderer.ReturnTarget(this.rth);
        this.rth = (RenderTargetHandle) null;
      }

      public override void Draw(GameTime gameTime)
      {
        GraphicsDevice graphicsDevice = this.GraphicsDevice;
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.NotEqual, StencilMask.Water);
        lock (FarawayPlaceHost.FarawayPlaceMutex)
        {
          if (this.PlacesMesh != null)
            this.PlacesMesh.Draw();
        }
        GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Always, StencilMask.None);
        lock (FarawayPlaceHost.FarawayWaterMutex)
        {
          if (this.FarawayWaterMesh != null)
            this.FarawayWaterMesh.Draw();
        }
        if (!this.GameState.StereoMode || this.rth == null)
          return;
        this.TargetRenderer.Resolve(this.rth.Target, true);
        this.GraphicsDevice.Clear(Color.Black);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.CombineEffect.RightTexture = this.CombineEffect.LeftTexture = (Texture2D) this.rth.Target;
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Opaque);
        this.TargetRenderer.DrawFullscreen((BaseEffect) this.CombineEffect);
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      }
    }
  }
}
