// Type: FezGame.Components.DotHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame;
using FezGame.Components.Scripting;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  public class DotHost : DrawableGameComponent, IDotManager
  {
    private List<Vector4> Vertices = new List<Vector4>();
    private Quaternion CamRotationFollow = Quaternion.Identity;
    private int[] FaceVertexIndices;
    private Mesh DotMesh;
    private Mesh RaysMesh;
    private Mesh FlareMesh;
    private IndexedUserPrimitives<FezVertexPositionColor> DotWireGeometry;
    private IndexedUserPrimitives<FezVertexPositionColor> DotFacesGeometry;
    private float Theta;
    private Vector3 InterpolatedPosition;
    private Vector3 InterpolatedScale;
    private float EightShapeStep;
    private Vector3 ToBackFollow;
    private float SinceStartedTransition;
    private float SinceStartedCameraPan;
    private Vector3 PanOrigin;
    private BackgroundPlane HaloPlane;
    private bool BurrowAfterPan;
    private Vector3 SpiralingCenter;
    private Vector3 lastRelativePosition;
    private GlyphTextRenderer GTR;
    private SpriteBatch spriteBatch;
    private Mesh BPromptMesh;
    private Mesh VignetteMesh;
    private SoundEffect sHide;
    private SoundEffect sComeOut;
    private SoundEffect sIdle;
    private SoundEffect sMove;
    private SoundEffect sHeyListen;
    private SoundEmitter eHide;
    private SoundEmitter eIdle;
    private SoundEmitter eMove;
    private SoundEmitter eComeOut;
    private SoundEmitter eHey;
    private DotHost.BehaviourType _behaviour;
    private DotHost.BehaviourType lastBehaviour;
    private RenderTarget2D bTexture;

    public bool Burrowing { get; set; }

    public bool ComingOut { get; set; }

    public bool DrawRays { get; set; }

    public object Owner { get; set; }

    public DotFaceButton FaceButton { get; set; }

    public Texture2D DestinationVignette { get; set; }

    public bool Hidden
    {
      get
      {
        if (!this.Visible)
          return !this.Enabled;
        else
          return false;
      }
      set
      {
        this.Visible = this.Enabled = !value;
        if (!value)
          this.Burrowing = false;
        if (this.HaloPlane == null)
          return;
        this.HaloPlane.Hidden = value;
      }
    }

    public Vector3 Position
    {
      get
      {
        return this.DotMesh.Position;
      }
    }

    public float RotationSpeed { get; set; }

    public float Opacity { get; set; }

    public DotHost.BehaviourType Behaviour
    {
      get
      {
        return this._behaviour;
      }
      set
      {
        if (this._behaviour != value && this.lastBehaviour != value && value == DotHost.BehaviourType.ThoughtBubble)
          this.UpdateBTexture();
        this.lastBehaviour = this._behaviour;
        this._behaviour = value;
      }
    }

    public Vector3 Target { get; set; }

    public string[] Dialog { get; set; }

    public float TimeToWait { get; set; }

    public Volume RoamingVolume { get; set; }

    public float ScaleFactor { get; set; }

    public float ScalePulsing { get; set; }

    public bool AlwaysShowLines { get; set; }

    public float InnerScale { get; set; }

    public bool PreventPoI { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    [ServiceDependency]
    public IFontManager Fonts { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    internal IScriptingManager Scripting { get; set; }

    public DotHost(Game game)
      : base(game)
    {
      this.DrawOrder = 900;
      this.Reset();
    }

    public void Reset()
    {
      this.RotationSpeed = 1f;
      this.Opacity = 1f;
      this.Behaviour = DotHost.BehaviourType.FollowGomez;
      this.Target = new Vector3();
      this.Dialog = (string[]) null;
      this.TimeToWait = 0.0f;
      this.ScaleFactor = 1f;
      this.ScalePulsing = 1f;
      this.AlwaysShowLines = false;
      this.InnerScale = 1f;
      this.EightShapeStep = 0.0f;
      this.Hidden = true;
      this.Burrowing = this.ComingOut = false;
      this.RoamingVolume = (Volume) null;
      this.PreventPoI = false;
      this.SinceStartedCameraPan = 0.0f;
      this.DrawRays = true;
      this.Owner = (object) null;
      if (this.HaloPlane != null)
        this.HaloPlane.Hidden = true;
      this.KillSounds();
    }

    private void KillSounds()
    {
      if (this.eIdle != null && !this.eIdle.Dead)
        this.eIdle.FadeOutAndDie(0.1f, false);
      if (this.eMove != null && !this.eMove.Dead)
        this.eMove.FadeOutAndDie(0.1f, false);
      if (this.eHide != null && !this.eHide.Dead)
        this.eHide.FadeOutAndDie(0.1f, false);
      if (this.eComeOut != null && !this.eComeOut.Dead)
        this.eComeOut.FadeOutAndDie(0.1f, false);
      if (this.eHey == null || this.eHey.Dead)
        return;
      this.eHey.FadeOutAndDie(0.1f, false);
    }

    public void Burrow()
    {
      if (this.Burrowing || this.Hidden)
        return;
      if (this.eHide != null && !this.eHide.Dead)
        this.eHide.FadeOutAndDie(0.1f, false);
      this.eHide = SoundEffectExtensions.EmitAt(this.sHide, this.Position);
      if (this.eIdle != null && !this.eIdle.Dead)
      {
        this.eIdle.FadeOutAndDie(1f, false);
        this.eIdle = (SoundEmitter) null;
      }
      if (this.eMove != null && !this.eMove.Dead)
        this.eMove.FadeOutAndDie(1f, false);
      if (this.eComeOut != null && !this.eComeOut.Dead)
        this.eComeOut.FadeOutAndDie(0.1f, false);
      if (this.eHey != null && !this.eHey.Dead)
        this.eHey.FadeOutAndDie(0.1f, false);
      this.SinceStartedTransition = !this.ComingOut ? 0.0f : 1f - FezMath.Saturate(this.SinceStartedTransition);
      this.ComingOut = false;
      this.Burrowing = true;
    }

    public void Hey()
    {
      if (this.eHey != null && !this.eHey.Dead)
        this.eHey.FadeOutAndDie(0.1f, false);
      this.eHey = SoundEffectExtensions.EmitAt(this.sHeyListen, this.Position, RandomHelper.Centered(13.0 / 400.0));
    }

    public void ComeOut()
    {
      this.ComeOut(false);
    }

    private void ComeOut(bool mute)
    {
      if (this.ComingOut || !this.Burrowing && !this.Hidden)
        return;
      if (this.Burrowing)
      {
        this.SinceStartedTransition = 1f - FezMath.Saturate(this.SinceStartedTransition);
      }
      else
      {
        this.Reset();
        this.InterpolatedPosition = this.PlayerManager.Position;
        this.SinceStartedTransition = 0.0f;
      }
      if (!mute)
      {
        if (this.eHide != null && !this.eHide.Dead)
          this.eHide.FadeOutAndDie(0.1f, false);
        if (this.eComeOut != null && !this.eComeOut.Dead)
          this.eComeOut.FadeOutAndDie(0.1f, false);
        if (!this.Burrowing)
          this.eComeOut = SoundEffectExtensions.EmitAt(this.sComeOut, this.Position);
        this.eIdle = SoundEffectExtensions.EmitAt(this.sIdle, this.Position, true);
      }
      this.EightShapeStep = 0.0f;
      this.ComingOut = true;
      this.Burrowing = false;
      this.Hidden = false;
    }

    public void MoveWithCamera(Vector3 target, bool burrowAfter)
    {
      this.PanOrigin = this.Hidden ? this.PlayerManager.Position : this.DotMesh.Position;
      this.ComeOut();
      this.SinceStartedCameraPan = 0.0f;
      this.Behaviour = DotHost.BehaviourType.MoveToTargetWithCamera;
      this.CameraManager.Constrained = true;
      this.Target = target;
      this.BurrowAfterPan = burrowAfter;
      this.eMove = SoundEffectExtensions.EmitAt(this.sMove, this.Position, true, 0.0f, 0.0f);
    }

    public void SpiralAround(Volume volume, Vector3 center, bool hideDot)
    {
      this.PlayerManager.CanControl = false;
      this.ComeOut(hideDot);
      if (hideDot)
      {
        this.HaloPlane.Hidden = true;
        this.Visible = false;
      }
      volume.From = new Vector3(volume.From.X, Math.Max(volume.From.Y, this.PlayerManager.Position.Y + 4f / this.CameraManager.PixelsPerTrixel), volume.From.Z);
      this.PreventPoI = true;
      this.SinceStartedCameraPan = 0.0f;
      this.Behaviour = DotHost.BehaviourType.SpiralAroundWithCamera;
      this.CameraManager.Constrained = true;
      this.RoamingVolume = volume;
      this.SpiralingCenter = center;
      this.InterpolatedScale = new Vector3(50f);
      Vector3 vector3 = FezMath.Abs(this.RoamingVolume.BoundingBox.Max - this.RoamingVolume.BoundingBox.Min);
      this.InterpolatedPosition = new Vector3(vector3.X / 2f, vector3.Y, vector3.Z / 2f) + this.RoamingVolume.BoundingBox.Min;
      if (!hideDot)
        this.eMove = SoundEffectExtensions.EmitAt(this.sMove, this.Position, true, 0.0f, 0.0f);
      this.Update(new GameTime(), true);
      this.CameraManager.SnapInterpolation();
      this.LevelMaterializer.Rowify();
    }

    public void ForceDrawOrder(int drawOrder)
    {
      this.DrawOrder = drawOrder;
      this.OnDrawOrderChanged((object) this, EventArgs.Empty);
    }

    public void RevertDrawOrder()
    {
      this.DrawOrder = 900;
      this.OnDrawOrderChanged((object) this, EventArgs.Empty);
    }

    public override void Initialize()
    {
      base.Initialize();
      this.GTR = new GlyphTextRenderer(this.Game);
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.Scripting.CutsceneSkipped += new Action(this.OnCutsceneSkipped);
      this.Vertices = new List<Vector4>()
      {
        new Vector4(-1f, -1f, -1f, -1f),
        new Vector4(1f, -1f, -1f, -1f),
        new Vector4(-1f, 1f, -1f, -1f),
        new Vector4(1f, 1f, -1f, -1f),
        new Vector4(-1f, -1f, 1f, -1f),
        new Vector4(1f, -1f, 1f, -1f),
        new Vector4(-1f, 1f, 1f, -1f),
        new Vector4(1f, 1f, 1f, -1f),
        new Vector4(-1f, -1f, -1f, 1f),
        new Vector4(1f, -1f, -1f, 1f),
        new Vector4(-1f, 1f, -1f, 1f),
        new Vector4(1f, 1f, -1f, 1f),
        new Vector4(-1f, -1f, 1f, 1f),
        new Vector4(1f, -1f, 1f, 1f),
        new Vector4(-1f, 1f, 1f, 1f),
        new Vector4(1f, 1f, 1f, 1f)
      };
      this.DotMesh = new Mesh()
      {
        Effect = (BaseEffect) new DotEffect(),
        Blending = new BlendingMode?(BlendingMode.Additive),
        DepthWrites = false,
        Culling = CullMode.None,
        AlwaysOnTop = true,
        Material = {
          Opacity = 0.3333333f
        }
      };
      this.RaysMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/smooth_ray")),
        Blending = new BlendingMode?(BlendingMode.Additive),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      this.FlareMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/rainbow_flare")),
        Blending = new BlendingMode?(BlendingMode.Additive),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      DotHost dotHost = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.Textured textured1 = new DefaultEffect.Textured();
      textured1.IgnoreCache = true;
      DefaultEffect.Textured textured2 = textured1;
      mesh2.Effect = (BaseEffect) textured2;
      mesh1.Blending = new BlendingMode?(BlendingMode.Alphablending);
      mesh1.SamplerState = SamplerStates.PointMipClamp;
      mesh1.DepthWrites = false;
      mesh1.AlwaysOnTop = true;
      Mesh mesh3 = mesh1;
      dotHost.VignetteMesh = mesh3;
      this.VignetteMesh.AddFace(new Vector3(1f), Vector3.Zero, FaceOrientation.Front, true);
      this.BPromptMesh = new Mesh()
      {
        AlwaysOnTop = true,
        SamplerState = SamplerState.PointClamp,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        Effect = (BaseEffect) new DefaultEffect.Textured()
      };
      this.BPromptMesh.AddFace(new Vector3(1f, 1f, 0.0f), Vector3.Zero, FaceOrientation.Front, false);
      this.FlareMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, true);
      this.DotMesh.AddGroup().Geometry = (IIndexedPrimitiveCollection) (this.DotWireGeometry = new IndexedUserPrimitives<FezVertexPositionColor>(PrimitiveType.LineList));
      this.DotMesh.AddGroup().Geometry = (IIndexedPrimitiveCollection) (this.DotFacesGeometry = new IndexedUserPrimitives<FezVertexPositionColor>(PrimitiveType.TriangleList));
      this.DotWireGeometry.Vertices = new FezVertexPositionColor[16];
      for (int index = 0; index < 16; ++index)
        this.DotWireGeometry.Vertices[index].Color = new Color(1f, 1f, 1f, 1f);
      this.DotWireGeometry.Indices = new int[64]
      {
        0,
        1,
        0,
        2,
        2,
        3,
        3,
        1,
        4,
        5,
        6,
        7,
        4,
        6,
        5,
        7,
        4,
        0,
        6,
        2,
        3,
        7,
        1,
        5,
        10,
        11,
        8,
        9,
        8,
        10,
        9,
        11,
        12,
        14,
        14,
        15,
        15,
        13,
        12,
        13,
        12,
        8,
        14,
        10,
        15,
        11,
        13,
        9,
        2,
        10,
        3,
        11,
        0,
        8,
        1,
        9,
        6,
        14,
        7,
        15,
        4,
        12,
        5,
        13
      };
      this.DotFacesGeometry.Vertices = new FezVertexPositionColor[96];
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 6; ++index2)
        {
          Vector3 vector3 = Vector3.Zero;
          switch ((index2 + index1 * 6) % 6)
          {
            case 0:
              vector3 = new Vector3(0.0f, 1f, 0.75f);
              break;
            case 1:
              vector3 = new Vector3(0.1666667f, 1f, 0.75f);
              break;
            case 2:
              vector3 = new Vector3(0.3333333f, 1f, 0.75f);
              break;
            case 3:
              vector3 = new Vector3(0.5f, 1f, 0.75f);
              break;
            case 4:
              vector3 = new Vector3(0.6666667f, 1f, 0.75f);
              break;
            case 5:
              vector3 = new Vector3(0.8333333f, 1f, 0.75f);
              break;
          }
          for (int index3 = 0; index3 < 4; ++index3)
            this.DotFacesGeometry.Vertices[index3 + index2 * 4 + index1 * 24].Color = new Color(vector3.X, vector3.Y, vector3.Z);
        }
      }
      this.FaceVertexIndices = new int[96]
      {
        0,
        2,
        3,
        1,
        1,
        3,
        7,
        5,
        5,
        7,
        6,
        4,
        4,
        6,
        2,
        0,
        0,
        4,
        5,
        1,
        2,
        6,
        7,
        3,
        8,
        10,
        11,
        9,
        9,
        11,
        15,
        13,
        13,
        15,
        14,
        12,
        12,
        14,
        10,
        8,
        8,
        12,
        13,
        9,
        10,
        14,
        15,
        11,
        0,
        1,
        9,
        8,
        0,
        2,
        10,
        8,
        2,
        3,
        11,
        10,
        3,
        1,
        9,
        11,
        4,
        5,
        13,
        12,
        6,
        7,
        15,
        14,
        4,
        6,
        14,
        12,
        5,
        7,
        15,
        13,
        4,
        0,
        8,
        12,
        6,
        2,
        10,
        14,
        3,
        7,
        15,
        11,
        1,
        5,
        13,
        9
      };
      this.DotFacesGeometry.Indices = new int[144]
      {
        0,
        2,
        1,
        0,
        3,
        2,
        4,
        6,
        5,
        4,
        7,
        6,
        8,
        10,
        9,
        8,
        11,
        10,
        12,
        14,
        13,
        12,
        15,
        14,
        16,
        17,
        18,
        16,
        18,
        19,
        20,
        22,
        21,
        20,
        23,
        22,
        24,
        26,
        25,
        24,
        27,
        26,
        28,
        30,
        29,
        28,
        31,
        30,
        32,
        34,
        33,
        32,
        35,
        34,
        36,
        38,
        37,
        36,
        39,
        38,
        40,
        41,
        42,
        40,
        42,
        43,
        44,
        46,
        45,
        44,
        47,
        46,
        48,
        50,
        49,
        48,
        51,
        50,
        52,
        54,
        53,
        52,
        55,
        54,
        56,
        58,
        57,
        56,
        59,
        58,
        60,
        62,
        61,
        60,
        63,
        62,
        64,
        65,
        66,
        64,
        66,
        67,
        68,
        70,
        69,
        68,
        71,
        70,
        72,
        74,
        73,
        72,
        75,
        74,
        76,
        78,
        77,
        76,
        79,
        78,
        80,
        82,
        81,
        80,
        83,
        82,
        84,
        86,
        85,
        84,
        87,
        86,
        88,
        89,
        90,
        88,
        90,
        91,
        92,
        94,
        93,
        92,
        95,
        94
      };
      this.sHide = this.CMProvider.Global.Load<SoundEffect>("Sounds/Dot/Hide");
      this.sComeOut = this.CMProvider.Global.Load<SoundEffect>("Sounds/Dot/ComeOut");
      this.sMove = this.CMProvider.Global.Load<SoundEffect>("Sounds/Dot/Move");
      this.sIdle = this.CMProvider.Global.Load<SoundEffect>("Sounds/Dot/Idle");
      this.sHeyListen = this.CMProvider.Global.Load<SoundEffect>("Sounds/Dot/HeyListen");
      this.LevelManager.LevelChanged += new Action(this.RebuildFlare);
    }

    private void UpdateBTexture()
    {
      SpriteFont small = this.Fonts.Small;
      Vector2 vector2 = small.MeasureString(this.GTR.FillInGlyphs(" {B} ")) * FezMath.Saturate(this.Fonts.SmallFactor);
      if (this.bTexture != null)
        this.bTexture.Dispose();
      this.bTexture = new RenderTarget2D(this.GraphicsDevice, (int) vector2.X, (int) vector2.Y, false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, this.GraphicsDevice.PresentationParameters.DepthStencilFormat, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
      this.GraphicsDevice.SetRenderTarget(this.bTexture);
      GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
      this.GraphicsDevice.Clear(ClearOptions.Target, ColorEx.TransparentWhite, 1f, 0);
      GraphicsDeviceExtensions.BeginPoint(this.spriteBatch);
      this.GTR.DrawString(this.spriteBatch, small, " {B} ", new Vector2(0.0f, 0.0f), Color.White, FezMath.Saturate(this.Fonts.SmallFactor));
      this.spriteBatch.End();
      this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      this.BPromptMesh.Texture = (Dirtyable<Texture>) ((Texture) this.bTexture);
      this.BPromptMesh.Scale = new Vector3(vector2.X / 32f, vector2.Y / 32f, 1f);
      if (!Culture.IsCJK)
        return;
      this.BPromptMesh.Scale *= 0.75f;
    }

    private void OnCutsceneSkipped()
    {
      if (this.Behaviour != DotHost.BehaviourType.SpiralAroundWithCamera)
        return;
      this.EndSpiral();
    }

    private void RebuildFlare()
    {
      if (this.LevelManager.BackgroundPlanes.ContainsKey(-2))
        return;
      this.LevelManager.BackgroundPlanes.Add(-2, this.HaloPlane = new BackgroundPlane(this.LevelMaterializer.StaticPlanesMesh, this.LevelManager.GomezHaloName ?? "flare", false)
      {
        Id = -2,
        LightMap = true,
        AlwaysOnTop = true,
        Billboard = true,
        Hidden = false,
        Filter = this.LevelManager.HaloFiltering ? new Color(0.425f, 0.425f, 0.425f, 1f) : new Color(0.5f, 0.5f, 0.5f, 1f),
        PixelatedLightmap = !this.LevelManager.HaloFiltering
      });
    }

    public override void Update(GameTime gameTime)
    {
      this.Update(gameTime, false);
    }

    public void Update(GameTime gameTime, bool force)
    {
      if (!force && (this.GameState.Paused || this.GameState.Loading || (this.GameState.InMenuCube || this.GameState.InFpsMode) || this.GameState.TimePaused && !this.GameState.InMap))
        return;
      if (Fez.LongScreenshot)
        this.HaloPlane.Hidden = true;
      if (this.Visible)
      {
        if ((double) this.RotationSpeed == 0.0)
          this.Theta = 0.0f;
        this.Theta += (float) gameTime.ElapsedGameTime.TotalSeconds * this.RotationSpeed;
        float num1 = (float) Math.Cos((double) this.Theta);
        float m14 = (float) Math.Sin((double) this.Theta);
        Matrix matrix = new Matrix(num1, 0.0f, 0.0f, m14, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, -m14, 0.0f, 0.0f, num1);
        for (int index = 0; index < this.Vertices.Count; ++index)
        {
          Vector4 vector4 = Vector4.Transform(this.Vertices[index], matrix);
          float num2 = (float) ((((double) vector4.W + 1.0) / 3.0 * (double) this.InnerScale + 0.5) * 0.333333343267441);
          this.DotWireGeometry.Vertices[index].Position = new Vector3(vector4.X, vector4.Y, vector4.Z) * num2;
        }
        for (int index = 0; index < this.FaceVertexIndices.Length; ++index)
          this.DotFacesGeometry.Vertices[index].Position = this.DotWireGeometry.Vertices[this.FaceVertexIndices[index]].Position;
        this.CamRotationFollow = Quaternion.Slerp(this.CamRotationFollow, this.CameraManager.Rotation, 0.05f);
        float num3 = (float) (Math.Sin(gameTime.TotalGameTime.TotalSeconds / 3.0) * 0.5 + 1.0);
        this.EightShapeStep += (float) gameTime.ElapsedGameTime.TotalSeconds * num3;
        this.ToBackFollow = Vector3.Lerp(this.ToBackFollow, (float) ((this.PlayerManager.Action == ActionType.RunTurnAround ? -1 : 1) * FezMath.Sign(this.PlayerManager.LookingDirection)) * FezMath.RightVector(this.CameraManager.Viewpoint) * 1.5f, 0.05f);
      }
      Vector3 vector2 = Vector3.Zero;
      switch (this.Behaviour)
      {
        case DotHost.BehaviourType.FollowGomez:
          vector2 = this.PlayerManager.Position + this.PlayerManager.Size * Vector3.UnitY * 0.5f + Vector3.UnitY * 1.5f + (float) Math.Sin((double) this.EightShapeStep * 2.0) * Vector3.UnitY * 0.5f + (float) Math.Cos((double) this.EightShapeStep) * this.CameraManager.View.Right - this.ToBackFollow;
          break;
        case DotHost.BehaviourType.ReadyToTalk:
          vector2 = this.PlayerManager.Position + this.PlayerManager.Size * Vector3.UnitY * 0.75f + Vector3.UnitY * 0.5f + (float) Math.Sin((double) this.EightShapeStep * 2.0) * Vector3.UnitY * 0.1f + (float) Math.Cos((double) this.EightShapeStep) * this.CameraManager.View.Right * 0.1f + this.ToBackFollow;
          break;
        case DotHost.BehaviourType.ClampToTarget:
          this.InterpolatedPosition = vector2 = this.DotMesh.Position = this.Target;
          break;
        case DotHost.BehaviourType.RoamInVolume:
          Vector3 vector3_1 = FezMath.Abs(this.RoamingVolume.BoundingBox.Max - this.RoamingVolume.BoundingBox.Min) / 2f + Vector3.One;
          vector2 = this.RoamingVolume.From + vector3_1 + vector3_1 * ((float) Math.Sin((double) this.EightShapeStep * 3.0 / (double) vector3_1.Y) * Vector3.UnitY + (float) Math.Cos((double) this.EightShapeStep * 1.5 / (((double) vector3_1.X + (double) vector3_1.Z) / 3.14285707473755)) * this.CameraManager.View.Right);
          break;
        case DotHost.BehaviourType.MoveToTargetWithCamera:
          float num4 = Vector3.Distance(this.PanOrigin, this.Target);
          if ((double) num4 == 0.0)
            num4 = 1f;
          int num5 = this.BurrowAfterPan ? 2 : 1;
          bool flag1 = (double) this.SinceStartedCameraPan == 0.0;
          this.SinceStartedCameraPan += (float) (gameTime.ElapsedGameTime.TotalSeconds / ((double) num4 / 5.0)) * (float) num5;
          this.SinceStartedCameraPan = FezMath.Saturate(this.SinceStartedCameraPan);
          Vector3 vector3_2 = Vector3.Lerp(this.PanOrigin, this.Target, Easing.EaseInOut((double) this.SinceStartedCameraPan, EasingType.Sine));
          if (this.BurrowAfterPan)
            this.CameraManager.Center = vector3_2;
          else
            this.CameraManager.Center = Vector3.Lerp(this.CameraManager.Center, vector3_2, 0.05f);
          if ((double) this.SinceStartedCameraPan >= 1.0)
            this.EndMoveTo();
          vector2 = vector3_2 + (float) Math.Sin((double) this.EightShapeStep * 2.0) * Vector3.UnitY / 2f + (float) Math.Cos((double) this.EightShapeStep) * this.CameraManager.View.Right / 2f;
          if (this.eMove != null && !this.eMove.Dead)
          {
            Vector3 vector3_3 = vector2;
            float num1 = ((vector3_3 - this.lastRelativePosition) * (this.CameraManager.InverseView.Right + Vector3.UnitY)).Length();
            if (!flag1)
              this.eMove.VolumeFactor = MathHelper.Lerp(this.eMove.VolumeFactor, (float) ((double) FezMath.Saturate(num1 * 10f) * 0.75 + 0.25), 0.1f);
            this.lastRelativePosition = vector3_3;
            break;
          }
          else
            break;
        case DotHost.BehaviourType.WaitAtTarget:
          vector2 = this.Target + (float) Math.Sin((double) this.EightShapeStep * 2.0) * Vector3.UnitY / 3f + (float) Math.Cos((double) this.EightShapeStep) * this.CameraManager.View.Right / 3f;
          this.CameraManager.Center = Vector3.Lerp(this.CameraManager.Center, this.Target, 0.075f);
          break;
        case DotHost.BehaviourType.SpiralAroundWithCamera:
          Vector3 vector3_4 = FezMath.Abs(this.RoamingVolume.BoundingBox.Max - this.RoamingVolume.BoundingBox.Min);
          float num6 = (float) (((double) this.RoamingVolume.BoundingBox.Max.Y - (double) this.PlayerManager.Position.Y) * 0.899999976158142);
          if ((double) num6 == 0.0)
            num6 = 1f;
          bool flag2 = (double) this.SinceStartedCameraPan == 0.0;
          this.SinceStartedCameraPan += (float) (gameTime.ElapsedGameTime.TotalSeconds / (double) num6 * 2.0);
          float num7 = Easing.EaseOut((double) FezMath.Saturate(this.SinceStartedCameraPan), EasingType.Sine);
          double num8 = Math.Round((double) num6 / 20.0) * 6.28318548202515;
          int distance = FezMath.GetDistance(this.CameraManager.Viewpoint, Viewpoint.Front);
          vector2 = new Vector3((float) (Math.Sin((double) Easing.EaseIn((double) num7, EasingType.Sine) * num8 - (double) distance * 1.57079637050629) * (double) vector3_4.X / 2.0 + (double) vector3_4.X / 2.0), vector3_4.Y * (1f - num7), (float) (Math.Cos((double) Easing.EaseIn((double) num7, EasingType.Sine) * num8 - (double) distance * 1.57079637050629) * (double) vector3_4.Z / 2.0 + (double) vector3_4.Z / 2.0)) + this.RoamingVolume.BoundingBox.Min;
          this.Target = new Vector3(this.SpiralingCenter.X, vector2.Y, this.SpiralingCenter.Z);
          Vector3 vector3_5 = Vector3.Normalize(new Vector3(vector2.X, 0.0f, vector2.Z) - (this.RoamingVolume.BoundingBox.Min + vector3_4 / 2f) * FezMath.XZMask);
          if ((double) num7 > 0.75)
          {
            float amount = Easing.EaseInOut(((double) num7 - 0.75) / 0.25, EasingType.Sine);
            this.Target = Vector3.Lerp(this.Target, this.PlayerManager.Position + Vector3.Up * 4f / this.CameraManager.PixelsPerTrixel, amount);
            vector2 = this.Target + FezMath.RightVector(this.CameraManager.Viewpoint) * amount;
          }
          this.CameraManager.Center = this.Target;
          this.CameraManager.Direction = vector3_5;
          if ((double) num7 < 0.1)
          {
            this.Target = new Vector3(vector3_4.X / 2f, vector3_4.Y * (1f - num7), vector3_4.Z / 2f) + this.RoamingVolume.BoundingBox.Min;
            vector2 = this.Target;
          }
          if ((double) num7 < 0.75)
            vector2 += this.CameraManager.InverseView.Right * 5f;
          if (this.eMove != null && !this.eMove.Dead)
          {
            Vector3 vector3_3 = vector2 - this.CameraManager.InterpolatedCenter;
            float num1 = ((vector3_3 - this.lastRelativePosition) * (this.CameraManager.InverseView.Right + Vector3.UnitY)).Length();
            if (!flag2)
              this.eMove.VolumeFactor = MathHelper.Lerp(this.eMove.VolumeFactor, (float) ((double) FezMath.Saturate(num1 * 3f) * 0.75 + 0.25), 0.1f);
            this.lastRelativePosition = vector3_3;
          }
          if ((double) num7 >= 1.0)
          {
            this.EndSpiral();
            break;
          }
          else
            break;
        case DotHost.BehaviourType.ThoughtBubble:
          vector2 = this.PlayerManager.Position + this.PlayerManager.Size * Vector3.UnitY * 0.75f + Vector3.UnitY * 0.5f + (float) Math.Sin((double) this.EightShapeStep * 2.0) * Vector3.UnitY * 0.1f + (float) Math.Cos((double) this.EightShapeStep) * this.CameraManager.View.Right * 0.1f + this.ToBackFollow * 1.25f;
          break;
      }
      if (this.Burrowing || this.ComingOut)
      {
        float num1 = Vector3.Distance(this.PlayerManager.Position, vector2);
        if ((double) num1 == 0.0)
          num1 = 1f;
        this.SinceStartedTransition += (float) (gameTime.ElapsedGameTime.TotalSeconds / ((double) num1 / 20.0));
      }
      Vector3 vector3_6 = this.Behaviour != DotHost.BehaviourType.ThoughtBubble ? new Vector3(MathHelper.Lerp(this.ScaleFactor * 0.75f, (float) ((double) this.ScaleFactor * 0.75 + Math.Sin((double) this.EightShapeStep * 4.0 / 3.0) * 0.200000002980232 * ((double) this.ScaleFactor + 1.0) / 2.0), this.ScalePulsing)) : (this.DestinationVignette == null ? (this.FaceButton != DotFaceButton.Up ? new Vector3(0.825f * this.ScaleFactor) : new Vector3(1f * this.ScaleFactor)) : new Vector3(2f));
      if (this.Visible)
        this.DotMesh.Rotation = this.CamRotationFollow * Quaternion.CreateFromAxisAngle(Vector3.Right, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0))) * Quaternion.CreateFromAxisAngle(Vector3.Up, 0.7853982f);
      float amount1 = Easing.EaseInOut((double) FezMath.Saturate(this.SinceStartedTransition), EasingType.Linear);
      if (this.Burrowing)
      {
        vector2 = Vector3.Lerp(vector2, this.PlayerManager.Position, amount1);
        vector3_6 = Vector3.Lerp(vector3_6, Vector3.Zero, amount1);
        if (this.eIdle != null)
          this.eIdle.VolumeFactor = amount1;
        if ((double) this.InterpolatedScale.X <= 0.01)
        {
          if (this.eIdle != null && this.eIdle.Cue != null && !this.eIdle.Cue.IsDisposed)
            this.eIdle.Cue.Stop(false);
          this.eIdle = (SoundEmitter) null;
          this.SinceStartedTransition = 0.0f;
          this.Reset();
          this.Burrowing = false;
        }
      }
      else if (this.ComingOut && this.Behaviour != DotHost.BehaviourType.SpiralAroundWithCamera)
      {
        vector2 = Vector3.Lerp(vector2, this.PlayerManager.Position, 1f - amount1);
        vector3_6 = Vector3.Lerp(vector3_6, Vector3.Zero, 1f - amount1);
        if (this.eIdle != null)
          this.eIdle.VolumeFactor = amount1;
        if ((double) amount1 >= 1.0)
        {
          this.ComingOut = false;
          this.SinceStartedTransition = 0.0f;
        }
      }
      float amount2 = this.Behaviour == DotHost.BehaviourType.ThoughtBubble ? 0.2f : 0.05f;
      this.InterpolatedPosition = Vector3.Lerp(this.InterpolatedPosition, vector2, amount2);
      float amount3 = this.Behaviour == DotHost.BehaviourType.ThoughtBubble ? 0.1f : (this.lastBehaviour == DotHost.BehaviourType.ThoughtBubble ? 0.075f : 0.05f);
      this.InterpolatedScale = Vector3.Lerp(this.InterpolatedScale, vector3_6, amount3);
      if (this.Visible)
      {
        this.DotMesh.Position = this.InterpolatedPosition;
        this.DotMesh.Scale = this.InterpolatedScale;
      }
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      if (this.GameState.InMap)
      {
        this.DotMesh.Scale *= this.CameraManager.Radius / 16f / viewScale;
        float y = this.CameraManager.Radius / 6f / viewScale;
        this.DotMesh.Position = this.CameraManager.Center + this.CameraManager.InverseView.Left * y * this.CameraManager.AspectRatio - new Vector3(0.0f, y, 0.0f);
      }
      if (this.Behaviour == DotHost.BehaviourType.ThoughtBubble)
      {
        this.VignetteMesh.Position = this.DotMesh.Position;
        this.VignetteMesh.Rotation = this.CameraManager.Rotation;
        if (this.DestinationVignette != null)
        {
          this.VignetteMesh.Scale = new Vector3(2.65f);
          this.VignetteMesh.SamplerState = SamplerState.PointClamp;
          this.VignetteMesh.Texture = (Dirtyable<Texture>) ((Texture) this.DestinationVignette);
          this.VignetteMesh.TextureMatrix.Set(Matrix.Identity);
        }
        else if (this.FaceButton == DotFaceButton.B)
        {
          this.BPromptMesh.Position = this.DotMesh.Position + FezMath.RightVector(this.CameraManager.Viewpoint) * 0.25f + new Vector3(0.0f, -0.925f, 0.0f);
          this.BPromptMesh.Rotation = this.CameraManager.Rotation;
        }
      }
      if (this.eMove != null && !this.eMove.Dead)
        this.eMove.Position = this.Position;
      if (this.eIdle != null && !this.eIdle.Dead)
        this.eIdle.Position = this.Position;
      if (this.eComeOut != null && !this.eComeOut.Dead)
        this.eComeOut.Position = this.Position;
      if (this.eHide != null && !this.eHide.Dead)
        this.eHide.Position = this.Position;
      if (this.eHey != null && !this.eHey.Dead)
        this.eHey.Position = this.Position;
      if (!this.DrawRays || !this.Visible)
        return;
      this.UpdateRays((float) gameTime.ElapsedGameTime.TotalSeconds);
    }

    private void EndSpiral()
    {
      Vector3 vector3 = this.PlayerManager.Position + Vector3.Up * 4f / this.CameraManager.PixelsPerTrixel + FezMath.RightVector(this.CameraManager.Viewpoint);
      this.PlayerManager.CanControl = true;
      if (this.eMove != null && !this.eMove.Dead)
      {
        this.eMove.Cue.Stop(false);
        this.eMove = (SoundEmitter) null;
      }
      this.RoamingVolume = (Volume) null;
      if (!this.Visible)
        this.Hidden = true;
      this.Target = vector3;
      this.Behaviour = DotHost.BehaviourType.ReadyToTalk;
      this.CameraManager.Direction = -FezMath.MaxClampXZ(FezMath.ForwardVector(this.CameraManager.Viewpoint));
      this.LevelMaterializer.CullInstances();
      this.CameraManager.Constrained = false;
      this.LevelMaterializer.UnRowify();
    }

    private void EndMoveTo()
    {
      this.eMove.FadeOutAndDie(2f, false);
      this.eMove = (SoundEmitter) null;
      if (this.BurrowAfterPan)
        this.Burrow();
      this.Behaviour = DotHost.BehaviourType.WaitAtTarget;
    }

    private void UpdateRays(float elapsedSeconds)
    {
      if (RandomHelper.Probability(0.03))
      {
        float x = 6f + RandomHelper.Centered(4.0);
        float num = RandomHelper.Between(0.5, (double) x / 2.5);
        Group group = this.RaysMesh.AddGroup();
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
      bool flag = (double) this.CameraManager.ViewTransitionStep != 0.0 && FezMath.IsOrthographic(this.CameraManager.Viewpoint) && FezMath.IsOrthographic(this.CameraManager.LastViewpoint);
      float num1 = Easing.EaseOut(1.0 - (double) this.CameraManager.ViewTransitionStep, EasingType.Quadratic) * (float) FezMath.GetDistance(this.CameraManager.Viewpoint, this.CameraManager.LastViewpoint);
      for (int i = this.RaysMesh.Groups.Count - 1; i >= 0; --i)
      {
        Group group = this.RaysMesh.Groups[i];
        DotHost.RayState rayState = group.CustomData as DotHost.RayState;
        rayState.Age += elapsedSeconds * 0.15f;
        float num2 = Easing.EaseOut(Math.Sin((double) rayState.Age * 6.28318548202515 - 1.57079637050629) * 0.5 + 0.5, EasingType.Quadratic);
        group.Material.Diffuse = new Vector3(num2 * 0.0375f) + rayState.Tint.ToVector3() * 0.075f * num2;
        float num3 = rayState.Speed;
        if (flag)
          num3 *= (float) (1.0 + 10.0 * (double) num1);
        group.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Forward, (float) ((double) elapsedSeconds * (double) num3 * 0.300000011920929));
        group.Scale = new Vector3((float) ((double) num2 * 0.75 + 0.25), (float) ((double) num2 * 0.5 + 0.5), 1f);
        if ((double) rayState.Age > 1.0)
          this.RaysMesh.RemoveGroupAt(i);
      }
      this.FlareMesh.Position = this.RaysMesh.Position = this.DotMesh.Position;
      this.FlareMesh.Rotation = this.RaysMesh.Rotation = this.CameraManager.Rotation;
      this.RaysMesh.Scale = this.DotMesh.Scale * 0.5f;
      float num4 = MathHelper.Lerp(this.DotMesh.Scale.X, 1f, 0.325f);
      this.FlareMesh.Scale = new Vector3(MathHelper.Lerp(num4, (float) Math.Pow((double) num4 * 2.0, 1.5), this.Opacity));
      this.FlareMesh.Material.Diffuse = new Vector3(0.25f * MathHelper.Lerp(this.DotMesh.Scale.X, 0.75f, 0.75f) * FezMath.Saturate(this.Opacity * 2f));
      this.HaloPlane.Position = this.DotMesh.Position;
      this.HaloPlane.Scale = this.Visible ? this.DotMesh.Scale * 2f : new Vector3(0.0f);
      if (this.LevelManager.HaloFiltering)
        return;
      this.HaloPlane.Position = FezMath.Round(this.HaloPlane.Position * 16f) / 16f;
      this.HaloPlane.Scale = Vector3.Clamp(this.HaloPlane.Scale, Vector3.Zero, Vector3.One);
    }

    public override void Draw(GameTime gameTime)
    {
      if (Fez.LongScreenshot)
        this.eIdle.VolumeFactor = 0.0f;
      if (this.GameState.Loading || this.GameState.InMenuCube || Fez.LongScreenshot)
        return;
      bool flag = this.Behaviour == DotHost.BehaviourType.ThoughtBubble;
      this.FlareMesh.Draw();
      if ((double) this.Opacity == 1.0 && this.DrawRays)
        this.RaysMesh.Draw();
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.Dot));
      this.DotMesh.Blending = new BlendingMode?(BlendingMode.Alphablending);
      this.DotMesh.Material.Diffuse = new Vector3(0.0f);
      if (flag && (this.FaceButton == DotFaceButton.Up || this.DestinationVignette != null))
      {
        this.DotMesh.Material.Opacity = 1f;
        this.DotMesh.Draw();
      }
      else
      {
        this.DotMesh.Material.Opacity = (double) this.Opacity > 0.5 ? this.Opacity * 0.25f : 0.0f;
        this.DotMesh.Draw();
      }
      if (flag && !this.GameState.InMap)
      {
        if (this.DestinationVignette != null)
        {
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.Dot);
          this.VignetteMesh.Draw();
        }
        else if (this.FaceButton == DotFaceButton.B)
        {
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
          this.BPromptMesh.Draw();
          this.BPromptMesh.Material.Opacity += (float) (gameTime.ElapsedGameTime.TotalSeconds * 3.0);
          if ((double) this.BPromptMesh.Material.Opacity > 1.0)
            this.BPromptMesh.Material.Opacity = 1f;
        }
        else if (this.FaceButton == DotFaceButton.Up)
        {
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.Dot);
          this.VignetteMesh.Texture = (Dirtyable<Texture>) ((Texture) this.GTR.GetReplacedGlyphTexture("{UP}"));
          this.VignetteMesh.Scale = new Vector3(0.75f);
          this.VignetteMesh.Draw();
        }
      }
      else
        this.BPromptMesh.Material.Opacity = -1.5f;
      if (!flag || this.FaceButton != DotFaceButton.Up && this.DestinationVignette == null)
      {
        this.DotMesh.Groups[0].Enabled = true;
        this.DotMesh.Groups[1].Enabled = false;
        this.DotMesh.Blending = new BlendingMode?(BlendingMode.Additive);
        float num = (float) Math.Pow(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2.0) * 0.5 + 0.5, 3.0);
        this.DotMesh.Material.Opacity = 1f;
        this.DotMesh.Material.Diffuse = new Vector3(this.AlwaysShowLines ? this.Opacity : num * 0.5f * this.Opacity);
        this.DotMesh.Draw();
        this.DotMesh.Groups[0].Enabled = false;
        this.DotMesh.Groups[1].Enabled = true;
        this.DotMesh.Material.Diffuse = new Vector3(this.Opacity);
        this.DotMesh.Draw();
      }
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
    }

    public enum BehaviourType
    {
      FollowGomez,
      ReadyToTalk,
      ClampToTarget,
      RoamInVolume,
      MoveToTargetWithCamera,
      WaitAtTarget,
      SpiralAroundWithCamera,
      ThoughtBubble,
    }

    internal class RayState
    {
      public readonly float Speed = RandomHelper.Between(0.100000001490116, 1.5);
      public readonly Color Tint = Util.ColorFromHSV((double) RandomHelper.Between(0.0, 360.0), 1.0, 1.0);
      public float Age;
    }
  }
}
