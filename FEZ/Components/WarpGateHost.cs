// Type: FezGame.Components.WarpGateHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class WarpGateHost : DrawableGameComponent
  {
    private readonly Dictionary<WarpDestinations, WarpPanel> panels = new Dictionary<WarpDestinations, WarpPanel>((IEqualityComparer<WarpDestinations>) WarpDestinationsComparer.Default);
    private ArtObjectInstance warpGateAo;
    private string CurrentLevelName;
    private Vector3 InterpolatedCenter;

    [ServiceDependency]
    public ISpeechBubbleManager SpeechBubble { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    public WarpGateHost(Game game)
      : base(game)
    {
      this.DrawOrder = 10;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.Visible = this.Enabled = false;
    }

    private void TryInitialize()
    {
      this.warpGateAo = Enumerable.FirstOrDefault<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObject.ActorType == ActorType.WarpGate));
      this.Visible = this.Enabled = this.warpGateAo != null;
      if (!this.Enabled)
      {
        foreach (KeyValuePair<WarpDestinations, WarpPanel> keyValuePair in this.panels)
        {
          keyValuePair.Value.PanelMask.Dispose();
          keyValuePair.Value.Layers.Dispose();
        }
        this.panels.Clear();
      }
      else
      {
        if (this.panels.Count == 0)
        {
          Mesh mesh1 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            DepthWrites = false,
            Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/fullwhite"))
          };
          mesh1.AddFace(new Vector3(3.875f), Vector3.Backward * (25.0 / 16.0), FaceOrientation.Front, true);
          Mesh mesh2 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            AlwaysOnTop = true,
            DepthWrites = false,
            SamplerState = SamplerState.PointClamp
          };
          Texture2D texture2D1 = this.CMProvider.Global.Load<Texture2D>("Other Textures/warp/nature/background");
          mesh2.AddFace(new Vector3(10f, 4f, 10f), Vector3.Forward * 5f, FaceOrientation.Front, true).Texture = (Texture) texture2D1;
          mesh2.AddFace(new Vector3(10f, 4f, 10f), Vector3.Right * 5f, FaceOrientation.Left, true).Texture = (Texture) texture2D1;
          mesh2.AddFace(new Vector3(10f, 4f, 10f), Vector3.Left * 5f, FaceOrientation.Right, true).Texture = (Texture) texture2D1;
          Group group1 = mesh2.AddFace(new Vector3(32f, 32f, 1f), new Vector3(0.0f, 2f, -8f), FaceOrientation.Front, true);
          group1.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Skies/WATERFRONT/WATERFRONT_C");
          group1.Material = new Material()
          {
            Opacity = 0.3f,
            Diffuse = Vector3.Lerp(Vector3.One, new Vector3(0.1215686f, 0.96f, 1f), 0.7f)
          };
          Group group2 = mesh2.AddFace(new Vector3(32f, 32f, 1f), new Vector3(0.0f, 2f, -8f), FaceOrientation.Front, true);
          group2.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Skies/WATERFRONT/WATERFRONT_B");
          group2.Material = new Material()
          {
            Opacity = 0.5f,
            Diffuse = Vector3.Lerp(Vector3.One, new Vector3(0.1215686f, 0.96f, 1f), 0.5f)
          };
          Group group3 = mesh2.AddFace(new Vector3(32f, 32f, 1f), new Vector3(0.0f, 2f, -8f), FaceOrientation.Front, true);
          group3.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Skies/WATERFRONT/WATERFRONT_A");
          group3.Material = new Material()
          {
            Opacity = 1f,
            Diffuse = Vector3.Lerp(Vector3.One, new Vector3(0.1215686f, 0.96f, 1f), 0.4f)
          };
          this.panels.Add(WarpDestinations.First, new WarpPanel()
          {
            PanelMask = mesh1,
            Layers = mesh2,
            Face = FaceOrientation.Front,
            Destination = "NATURE_HUB"
          });
          Mesh mesh3 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            DepthWrites = false,
            Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/fullwhite"))
          };
          mesh3.AddFace(new Vector3(3.875f), Vector3.Right * (25.0 / 16.0), FaceOrientation.Right, true);
          Mesh mesh4 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            AlwaysOnTop = true,
            DepthWrites = false,
            SamplerState = SamplerState.PointClamp
          };
          Texture2D texture2D2 = this.CMProvider.Global.Load<Texture2D>("Other Textures/warp/graveyard/back");
          Group group4 = mesh4.AddFace(Vector3.One * 16f, Vector3.Zero, FaceOrientation.Right, true);
          group4.Texture = (Texture) texture2D2;
          group4.SamplerState = SamplerState.PointWrap;
          mesh4.AddFace(new Vector3(1f, 16f, 32f), new Vector3(-8f, 4f, 0.0f), FaceOrientation.Right, true).Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Skies/GRAVE/GRAVE_CLOUD_C");
          mesh4.AddFace(new Vector3(1f, 16f, 32f), new Vector3(-8f, 4f, 0.0f), FaceOrientation.Right, true).Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Skies/GRAVE/GRAVE_CLOUD_B");
          mesh4.AddFace(new Vector3(1f, 16f, 32f), new Vector3(-8f, 4f, 0.0f), FaceOrientation.Right, true).Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Skies/GRAVE/GRAVE_CLOUD_A");
          Group group5 = mesh4.AddFace(Vector3.One * 16f, Vector3.Zero, FaceOrientation.Right, true);
          group5.SamplerState = SamplerState.PointWrap;
          group5.Blending = new BlendingMode?(BlendingMode.Additive);
          group5.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/warp/graveyard/rainoverlay");
          this.panels.Add(WarpDestinations.Graveyard, new WarpPanel()
          {
            PanelMask = mesh3,
            Layers = mesh4,
            Face = FaceOrientation.Right,
            Destination = "GRAVEYARD_GATE"
          });
          Mesh mesh5 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            DepthWrites = false,
            Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/fullwhite"))
          };
          mesh5.AddFace(new Vector3(3.875f), Vector3.Left * (25.0 / 16.0), FaceOrientation.Left, true);
          Mesh mesh6 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            AlwaysOnTop = true,
            DepthWrites = false,
            SamplerState = SamplerState.PointClamp
          };
          Texture2D texture2D3 = this.CMProvider.Global.Load<Texture2D>("Other Textures/warp/industrial/background");
          mesh6.AddFace(new Vector3(10f, 4f, 10f), Vector3.Right * 5f, FaceOrientation.Left, true).Texture = (Texture) texture2D3;
          mesh6.AddFace(new Vector3(10f, 4f, 10f), Vector3.Backward * 5f, FaceOrientation.Back, true).Texture = (Texture) texture2D3;
          mesh6.AddFace(new Vector3(10f, 4f, 10f), Vector3.Forward * 5f, FaceOrientation.Front, true).Texture = (Texture) texture2D3;
          Group group6 = mesh6.AddFace(new Vector3(1f, 8f, 8f), new Vector3(8f, 0.0f, 0.0f), FaceOrientation.Left, true);
          group6.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/warp/industrial/INDUST_CLOUD_B");
          group6.Material = new Material()
          {
            Opacity = 0.5f
          };
          Group group7 = mesh6.AddFace(new Vector3(1f, 8f, 8f), new Vector3(8f, 0.0f, 0.0f), FaceOrientation.Left, true);
          group7.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/warp/industrial/INDUST_CLOUD_F");
          group7.Material = new Material()
          {
            Opacity = 0.325f
          };
          this.panels.Add(WarpDestinations.Mechanical, new WarpPanel()
          {
            PanelMask = mesh5,
            Layers = mesh6,
            Face = FaceOrientation.Left,
            Destination = "INDUSTRIAL_HUB"
          });
          Mesh mesh7 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            DepthWrites = false,
            Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/fullwhite"))
          };
          mesh7.AddFace(new Vector3(3.875f), Vector3.Forward * (25.0 / 16.0), FaceOrientation.Back, true);
          Mesh mesh8 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            AlwaysOnTop = true,
            DepthWrites = false,
            SamplerState = SamplerState.PointClamp
          };
          Texture2D texture2D4 = this.CMProvider.Global.Load<Texture2D>("Skies/SEWER/BRICK_BACKGROUND");
          Group group8 = mesh8.AddFace(Vector3.One * 16f, Vector3.Backward * 8f, FaceOrientation.Back, true);
          group8.Texture = (Texture) texture2D4;
          group8.SamplerState = SamplerState.PointWrap;
          Group group9 = mesh8.AddFace(Vector3.One * 16f, Vector3.Right * 8f, FaceOrientation.Left, true);
          group9.Texture = (Texture) texture2D4;
          group9.SamplerState = SamplerState.PointWrap;
          Group group10 = mesh8.AddFace(Vector3.One * 16f, Vector3.Left * 8f, FaceOrientation.Right, true);
          group10.Texture = (Texture) texture2D4;
          group10.SamplerState = SamplerState.PointWrap;
          Group group11 = mesh8.AddFace(new Vector3(128f, 8f, 1f), new Vector3(0.0f, 4f, -8f), FaceOrientation.Back, true);
          group11.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/warp/sewer/sewage");
          group11.SamplerState = SamplerState.PointWrap;
          this.panels.Add(WarpDestinations.Sewers, new WarpPanel()
          {
            PanelMask = mesh7,
            Layers = mesh8,
            Face = FaceOrientation.Back,
            Destination = "SEWER_HUB"
          });
          Mesh mesh9 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            DepthWrites = false,
            Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/fullwhite"))
          };
          mesh9.AddFace(new Vector3(3.875f), Vector3.Backward * (25.0 / 16.0), FaceOrientation.Front, true);
          Mesh mesh10 = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.Textured(),
            AlwaysOnTop = true,
            DepthWrites = false,
            SamplerState = SamplerState.PointClamp
          };
          Texture2D texture2D5 = this.CMProvider.Global.Load<Texture2D>("Other Textures/warp/zu/back");
          Group group12 = mesh10.AddFace(new Vector3(16f, 32f, 16f), Vector3.Zero, FaceOrientation.Front, true);
          group12.Texture = (Texture) texture2D5;
          group12.SamplerState = SamplerState.PointWrap;
          Group group13 = mesh10.AddFace(new Vector3(32f, 32f, 1f), new Vector3(0.0f, 0.0f, -8f), FaceOrientation.Front, true);
          group13.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Skies/ABOVE/ABOVE_C");
          group13.Material = new Material()
          {
            Opacity = 0.4f,
            Diffuse = Vector3.Lerp(new Vector3(0.6862745f, 1f, 0.97647f), new Vector3(0.1294118f, 0.4f, 1f), 0.7f)
          };
          Group group14 = mesh10.AddFace(new Vector3(32f, 32f, 1f), new Vector3(0.0f, 0.0f, -8f), FaceOrientation.Front, true);
          group14.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Skies/ABOVE/ABOVE_B");
          group14.Material = new Material()
          {
            Opacity = 0.6f,
            Diffuse = Vector3.Lerp(new Vector3(0.6862745f, 1f, 0.97647f), new Vector3(0.1294118f, 0.4f, 1f), 0.6f)
          };
          Group group15 = mesh10.AddFace(new Vector3(32f, 32f, 1f), new Vector3(0.0f, 0.0f, -8f), FaceOrientation.Front, true);
          group15.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Skies/ABOVE/ABOVE_A");
          group15.Material = new Material()
          {
            Opacity = 1f,
            Diffuse = Vector3.Lerp(new Vector3(0.6862745f, 1f, 0.97647f), new Vector3(0.1294118f, 0.4f, 1f), 0.5f)
          };
          this.panels.Add(WarpDestinations.Zu, new WarpPanel()
          {
            PanelMask = mesh9,
            Layers = mesh10,
            Face = FaceOrientation.Front,
            Destination = "ZU_CITY_RUINS"
          });
        }
        if (Fez.LongScreenshot)
        {
          this.GameState.SaveData.UnlockedWarpDestinations.Add("SEWER_HUB");
          this.GameState.SaveData.UnlockedWarpDestinations.Add("GRAVEYARD_GATE");
          this.GameState.SaveData.UnlockedWarpDestinations.Add("INDUSTRIAL_HUB");
          this.GameState.SaveData.UnlockedWarpDestinations.Add("ZU_CITY_RUINS");
        }
        string str = this.LevelManager.Name.Replace('\\', '/');
        this.CurrentLevelName = str.Substring(str.LastIndexOf('/') + 1);
        if (!this.GameState.SaveData.UnlockedWarpDestinations.Contains(this.CurrentLevelName))
          this.GameState.SaveData.UnlockedWarpDestinations.Add(this.CurrentLevelName);
        else if (this.GameState.SaveData.UnlockedWarpDestinations.Count > 1)
        {
          ICollection<Volume> values = this.LevelManager.Volumes.Values;
          Func<Volume, bool> predicate = (Func<Volume, bool>) (x =>
          {
            if (x.ActorSettings != null && x.ActorSettings.IsPointOfInterest)
              return (double) Vector3.DistanceSquared(FezMath.GetCenter(x.BoundingBox), this.warpGateAo.Position) < 4.0;
            else
              return false;
          });
          Volume volume;
          if ((volume = Enumerable.FirstOrDefault<Volume>((IEnumerable<Volume>) values, predicate)) != null)
          {
            volume.ActorSettings.DotDialogue.Clear();
            volume.ActorSettings.DotDialogue.AddRange((IEnumerable<DotDialogueLine>) new DotDialogueLine[3]
            {
              new DotDialogueLine()
              {
                ResourceText = "DOT_WARP_A",
                Grouped = true
              },
              new DotDialogueLine()
              {
                ResourceText = "DOT_WARP_B",
                Grouped = true
              },
              new DotDialogueLine()
              {
                ResourceText = "DOT_WARP_UP",
                Grouped = true
              }
            });
            bool flag;
            if (this.GameState.SaveData.OneTimeTutorials.TryGetValue("DOT_WARP_A", out flag) && flag)
              volume.ActorSettings.PreventHey = true;
          }
        }
        Vector3 zero = Vector3.Zero;
        if (this.warpGateAo.ArtObject.Cubemap.Height == 128)
          zero -= Vector3.UnitY;
        foreach (WarpPanel warpPanel in this.panels.Values)
        {
          warpPanel.PanelMask.Position = this.warpGateAo.Position + zero;
          warpPanel.Layers.Position = this.warpGateAo.Position + zero;
          warpPanel.Enabled = warpPanel.Destination != this.CurrentLevelName && this.GameState.SaveData.UnlockedWarpDestinations.Contains(warpPanel.Destination);
          if (warpPanel.Destination == "ZU_CITY_RUINS")
          {
            switch (this.CurrentLevelName)
            {
              case "NATURE_HUB":
                warpPanel.Face = FaceOrientation.Front;
                warpPanel.PanelMask.Rotation = Quaternion.Identity;
                warpPanel.Layers.Rotation = Quaternion.Identity;
                continue;
              case "GRAVEYARD_GATE":
                warpPanel.Face = FaceOrientation.Right;
                warpPanel.PanelMask.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 1.570796f);
                warpPanel.Layers.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 1.570796f);
                continue;
              case "INDUSTRIAL_HUB":
                warpPanel.Face = FaceOrientation.Left;
                warpPanel.PanelMask.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 4.712389f);
                warpPanel.Layers.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 4.712389f);
                continue;
              case "SEWER_HUB":
                warpPanel.Face = FaceOrientation.Back;
                warpPanel.PanelMask.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f);
                warpPanel.Layers.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f);
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    protected override void LoadContent()
    {
      this.LightingPostProcess.DrawOnTopLights += new Action(this.DrawLights);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.warpGateAo.ActorSettings.Inactive || (this.GameState.InMap || this.GameState.Paused))
        return;
      this.UpdateParallax(gameTime.ElapsedGameTime);
      this.UpdateDoors();
    }

    private void UpdateDoors()
    {
      if (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.InputManager.Up != FezButtonState.Pressed || (!this.PlayerManager.Grounded || this.PlayerManager.Action == ActionType.GateWarp) || (this.PlayerManager.Action == ActionType.WakingUp || this.PlayerManager.Action == ActionType.LesserWarp || !this.SpeechBubble.Hidden))
        return;
      Vector3 vector3_1 = this.PlayerManager.Position * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = this.warpGateAo.Position * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      if ((double) this.warpGateAo.ArtObject.Size.Y == 8.0)
        vector3_2 -= new Vector3(0.0f, 1f, 0.0f);
      Vector3 a = vector3_2 - vector3_1;
      if ((double) Math.Abs(FezMath.Dot(a, FezMath.SideMask(this.CameraManager.Viewpoint))) > 1.5 || (double) Math.Abs(a.Y) > 2.0)
        return;
      foreach (WarpPanel warpPanel in this.panels.Values)
      {
        if (warpPanel.Enabled && warpPanel.Face == FezMath.VisibleOrientation(this.CameraManager.Viewpoint))
        {
          this.PlayerManager.Action = ActionType.GateWarp;
          this.PlayerManager.WarpPanel = warpPanel;
          this.PlayerManager.OriginWarpViewpoint = FezMath.AsViewpoint(Enumerable.First<WarpPanel>((IEnumerable<WarpPanel>) this.panels.Values, (Func<WarpPanel, bool>) (x => x.Destination == this.CurrentLevelName)).Face);
          break;
        }
      }
    }

    private void UpdateParallax(TimeSpan elapsed)
    {
      if (!FezMath.IsOrthographic(this.CameraManager.Viewpoint))
        return;
      this.InterpolatedCenter = Vector3.Lerp(this.InterpolatedCenter, this.CameraManager.Center, MathHelper.Clamp((float) elapsed.TotalSeconds * this.CameraManager.InterpolationSpeed, 0.0f, 1f));
      Vector3 forward = this.CameraManager.View.Forward;
      forward.Z *= -1f;
      Vector3 right = this.CameraManager.View.Right;
      Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = (this.InterpolatedCenter - this.warpGateAo.Position) / 2.5f;
      Vector3 a = (this.CameraManager.InterpolatedCenter - this.warpGateAo.Position) * vector3_1;
      foreach (WarpDestinations index in this.panels.Keys)
      {
        WarpPanel warpPanel = this.panels[index];
        if (warpPanel.Enabled && (double) FezMath.Dot(FezMath.AsVector(warpPanel.Face), forward) > 0.0)
        {
          warpPanel.Timer += elapsed;
          Vector3 vector3_3 = vector3_2 * vector3_1;
          switch (index)
          {
            case WarpDestinations.First:
              warpPanel.Layers.Groups[3].Position = vector3_3 - Vector3.UnitY * 1.5f + (float) (warpPanel.Timer.TotalSeconds * 0.25 % 16.0 - 8.0) * right;
              warpPanel.Layers.Groups[4].Position = vector3_3 - Vector3.UnitY * 1.5f + (float) (warpPanel.Timer.TotalSeconds * 0.5 % 16.0 - 8.0) * right;
              warpPanel.Layers.Groups[5].Position = vector3_3 - Vector3.UnitY * 1.5f + (float) (warpPanel.Timer.TotalSeconds % 16.0 - 8.0) * right;
              continue;
            case WarpDestinations.Mechanical:
              warpPanel.Layers.Groups[3].Position = vector3_3 + new Vector3(0.0f, 2f, 0.0f) + (float) (warpPanel.Timer.TotalSeconds % 16.0 - 8.0) * right;
              warpPanel.Layers.Groups[4].Position = vector3_3 - new Vector3(0.0f, 2f, 0.0f) + (float) ((warpPanel.Timer.TotalSeconds + 8.0) % 16.0 - 8.0) * right;
              continue;
            case WarpDestinations.Graveyard:
              float m31_1 = (float) (((double) a.X + (double) a.Z) / 16.0);
              float m32_1 = a.Y / 16f;
              warpPanel.Layers.Groups[0].TextureMatrix.Set(new Matrix?(new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, m31_1, m32_1, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f)));
              warpPanel.Layers.Groups[4].TextureMatrix.Set(new Matrix?(new Matrix(2f, 0.0f, 0.0f, 0.0f, 0.0f, 2f, 0.0f, 0.0f, m31_1 / 2f, (float) ((double) m32_1 / 2.0 - warpPanel.Timer.TotalSeconds * 5.0), 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f)));
              warpPanel.Layers.Groups[1].Position = vector3_3 - Vector3.UnitY * 1.5f + (float) (warpPanel.Timer.TotalSeconds * 0.5 % 16.0 - 8.0) * right;
              warpPanel.Layers.Groups[2].Position = vector3_3 - Vector3.UnitY * 1.5f + (float) (warpPanel.Timer.TotalSeconds % 16.0 - 8.0) * right;
              warpPanel.Layers.Groups[3].Position = vector3_3 - Vector3.UnitY * 1.5f + (float) (warpPanel.Timer.TotalSeconds * 2.0 % 16.0 - 8.0) * right;
              continue;
            case WarpDestinations.Sewers:
              Matrix matrix = new Matrix(4f, 0.0f, 0.0f, 0.0f, 0.0f, 4f, 0.0f, 0.0f, (float) (((double) a.X + (double) a.Z) / 8.0), a.Y / 8f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
              warpPanel.Layers.Groups[0].TextureMatrix.Set(new Matrix?(matrix));
              warpPanel.Layers.Groups[1].TextureMatrix.Set(new Matrix?(matrix));
              warpPanel.Layers.Groups[2].TextureMatrix.Set(new Matrix?(matrix));
              warpPanel.Layers.Groups[3].Position = vector3_3 + new Vector3(0.0f, -8f, 0.0f);
              continue;
            case WarpDestinations.Zu:
              float m31_2 = (float) (-(double) FezMath.Dot(a, right) / 16.0);
              float m32_2 = a.Y / 32f;
              warpPanel.Layers.Groups[0].TextureMatrix.Set(new Matrix?(new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, m31_2, m32_2, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f)));
              warpPanel.Layers.Groups[1].Position = vector3_3 - Vector3.UnitY * 1.5f + (float) (warpPanel.Timer.TotalSeconds * 0.25 % 16.0 - 8.0) * right;
              warpPanel.Layers.Groups[2].Position = vector3_3 - Vector3.UnitY * 1.5f + (float) (warpPanel.Timer.TotalSeconds * 0.5 % 16.0 - 8.0) * right;
              warpPanel.Layers.Groups[3].Position = vector3_3 - Vector3.UnitY * 1.5f + (float) (warpPanel.Timer.TotalSeconds % 16.0 - 8.0) * right;
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void DrawLights()
    {
      if (!this.Visible || this.GameState.Loading || (this.warpGateAo.ActorSettings.Inactive || Fez.LongScreenshot))
        return;
      foreach (WarpDestinations index in this.panels.Keys)
      {
        WarpPanel warpPanel = this.panels[index];
        if (warpPanel.Enabled)
        {
          (warpPanel.PanelMask.Effect as DefaultEffect).Pass = LightingEffectPass.Pre;
          warpPanel.PanelMask.Draw();
          (warpPanel.PanelMask.Effect as DefaultEffect).Pass = LightingEffectPass.Main;
        }
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.warpGateAo.ActorSettings.Inactive)
        return;
      foreach (WarpDestinations index in this.panels.Keys)
      {
        WarpPanel warpPanel = this.panels[index];
        if (warpPanel.Enabled)
        {
          GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
          GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.WarpGate));
          warpPanel.PanelMask.Draw();
          GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.WarpGate);
          warpPanel.Layers.Draw();
          GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
          GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.None));
          warpPanel.PanelMask.Draw();
        }
      }
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
    }
  }
}
