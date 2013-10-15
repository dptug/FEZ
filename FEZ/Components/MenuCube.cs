// Type: FezGame.Components.MenuCube
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class MenuCube : DrawableGameComponent
  {
    private static readonly CodeInput[] LetterCode = new CodeInput[8]
    {
      CodeInput.SpinLeft,
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinLeft,
      CodeInput.SpinRight,
      CodeInput.SpinLeft,
      CodeInput.SpinLeft,
      CodeInput.SpinLeft
    };
    private static readonly CodeInput[] NumberCode = new CodeInput[8]
    {
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinLeft,
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinLeft,
      CodeInput.SpinLeft
    };
    private readonly Dictionary<MenuCubeFace, Vector2> HighlightPosition = new Dictionary<MenuCubeFace, Vector2>((IEqualityComparer<MenuCubeFace>) MenuCubeFaceComparer.Default)
    {
      {
        MenuCubeFace.Maps,
        new Vector2(0.0f, 0.0f)
      },
      {
        MenuCubeFace.Artifacts,
        new Vector2(0.0f, 0.0f)
      }
    };
    private readonly List<ArtObjectInstance> ArtifactAOs = new List<ArtObjectInstance>();
    private readonly List<CodeInput> codeInputs = new List<CodeInput>();
    private readonly Vector3[] originalMapPositions = new Vector3[6];
    private Viewpoint OriginalViewpoint;
    private Vector3 OriginalCenter;
    private Quaternion OriginalRotation;
    private float OriginalPixPerTrix;
    private Vector3 OriginalDirection;
    private ArtObjectInstance AoInstance;
    private ArtObjectInstance ZoomedArtifact;
    private List<bool> AoVisibility;
    private Mesh GoldenCubes;
    private Mesh Maps;
    private Mesh AntiCubes;
    private Mesh HidingPlanes;
    private Mesh Highlights;
    private Mesh TomePages;
    private RenderTargetHandle InRtHandle;
    private RenderTargetHandle OutRtHandle;
    private FastBlurEffect BlurEffect;
    private bool Resolved;
    private bool ScheduleExit;
    private bool TomeZoom;
    private bool TomeOpen;
    private bool NumberZoom;
    private bool LetterZoom;
    private int TomePageIndex;
    private SoundEffect enterSound;
    private SoundEffect exitSound;
    private SoundEffect zoomInSound;
    private SoundEffect zoomOutSound;
    private SoundEffect rotateLeftSound;
    private SoundEffect rotateRightSound;
    private SoundEffect cursorSound;
    private SoundEffect sBackground;
    private SoundEmitter eBackground;
    private Texture2D oldTextureCache;
    private int Turns;
    private MenuCubeFace Face;
    private MenuCubeFace LastFace;
    private ArtObjectInstance TomeCoverAo;
    private ArtObjectInstance TomeBackAo;
    private bool wasLowPass;
    public static MenuCube Instance;
    private bool letterCodeDone;
    private bool numberCodeDone;
    private bool zoomed;
    private bool zooming;
    private MenuCubeFace zoomedFace;
    private Vector3 originalObjectPosition;
    private IWaiter tomeOpenWaiter;

    [ServiceDependency]
    public ILevelService LevelService { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public IGameService GameService { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IFontManager FontManager { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderingManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    static MenuCube()
    {
    }

    public MenuCube(Game game)
      : base(game)
    {
      this.UpdateOrder = -9;
      this.DrawOrder = 1000;
      MenuCube.Instance = this;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.GameState.MenuCubeIsZoomed = false;
      this.PlayerManager.CanControl = false;
      ArtObject artObject = this.CMProvider.Global.Load<ArtObject>("Art Objects/MENU_CUBEAO");
      bool flag = true;
      if (this.LevelManager.WaterType == LiquidType.Sewer)
      {
        this.oldTextureCache = artObject.Cubemap;
        artObject.Cubemap = this.CMProvider.Global.Load<Texture2D>("Art Objects/MENU_CUBE_GB");
      }
      else if (this.LevelManager.WaterType == LiquidType.Lava)
      {
        this.oldTextureCache = artObject.Cubemap;
        artObject.Cubemap = this.CMProvider.Global.Load<Texture2D>("Art Objects/MENU_CUBE_VIRTUAL");
      }
      else if (this.LevelManager.BlinkingAlpha)
      {
        this.oldTextureCache = artObject.Cubemap;
        artObject.Cubemap = this.CMProvider.Global.Load<Texture2D>("Art Objects/MENU_CUBE_CMY");
      }
      else
        flag = false;
      if (flag)
        new ArtObjectMaterializer(artObject).RecomputeTexCoords(false);
      int key = IdentifierPool.FirstAvailable<ArtObjectInstance>(this.LevelManager.ArtObjects);
      this.AoInstance = new ArtObjectInstance(artObject)
      {
        Id = key,
        Position = this.PlayerManager.Center
      };
      this.AoInstance.Initialize();
      this.AoInstance.Material = new Material();
      this.LevelManager.ArtObjects.Add(key, this.AoInstance);
      this.AoInstance.Scale = new Vector3(0.0f);
      this.OriginalViewpoint = this.CameraManager.Viewpoint;
      this.OriginalCenter = this.CameraManager.Center;
      this.OriginalPixPerTrix = this.CameraManager.PixelsPerTrixel;
      this.OriginalRotation = FezMath.QuaternionFromPhi(FezMath.ToPhi(this.CameraManager.Viewpoint));
      RenderTarget2D renderTarget = this.GraphicsDevice.GetRenderTargets().Length == 0 ? (RenderTarget2D) null : this.GraphicsDevice.GetRenderTargets()[0].RenderTarget as RenderTarget2D;
      this.FillInPlanes();
      this.CreateGoldenCubeFace();
      this.CreateMapsFace();
      this.CreateArtifactsFace();
      this.CreateAntiCubeFace();
      this.CreateHighlights();
      this.CreateTomePages();
      this.GraphicsDevice.SetRenderTarget(renderTarget);
      this.AntiCubes.Position = this.Maps.Position = this.HidingPlanes.Position = this.GoldenCubes.Position = this.AoInstance.Position;
      this.AntiCubes.Scale = this.Maps.Scale = this.HidingPlanes.Scale = this.GoldenCubes.Scale = this.AoInstance.Scale = new Vector3(0.0f);
      this.TransformArtifacts();
      this.BlurEffect = new FastBlurEffect();
      this.enterSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/EnterMenucubeOrMap");
      this.exitSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/ExitMenucubeOrMap");
      this.zoomInSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/ZoomIn");
      this.zoomOutSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/ZoomOut");
      this.rotateLeftSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/RotateLeft");
      this.rotateRightSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/RotateRight");
      this.cursorSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/MoveCursorMenucube");
      this.sBackground = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/MenuCubeBackground");
      this.eBackground = SoundEffectExtensions.Emit(this.sBackground, true);
      SoundEffectExtensions.Emit(this.enterSound);
      this.AoVisibility = new List<bool>();
      this.AoInstance.Hidden = true;
      this.AoInstance.Visible = false;
      this.GameService.CloseScroll((string) null);
      this.GameState.ShowScroll(MenuCubeFaceExtensions.GetTitle(this.Face), 0.0f, true);
      this.wasLowPass = this.SoundManager.IsLowPass;
      if (!this.wasLowPass)
        this.SoundManager.FadeFrequencies(true);
      this.InRtHandle = this.TargetRenderingManager.TakeTarget();
      this.OutRtHandle = this.TargetRenderingManager.TakeTarget();
      this.TargetRenderingManager.ScheduleHook(this.DrawOrder, this.InRtHandle.Target);
    }

    private void CreateTomePages()
    {
      this.TomePages = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.LitTextured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/PAGES/tome_pages")),
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerState.PointClamp
      };
      Vector3 origin = new Vector3(0.0f, -0.875f, 0.0f);
      Vector3 size = new Vector3(0.875f, 0.875f, 0.0f);
      this.TomePages.AddFace(size, origin, FaceOrientation.Front, false);
      this.TomePages.AddFace(size, origin, FaceOrientation.Back, false);
      this.TomePages.CollapseWithNormalTexture<FezVertexPositionNormalTexture>();
      this.TomePages.Groups[0].Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/PAGES/blank");
      for (int index1 = 0; index1 < 2; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          this.TomePages.AddFace(size, origin, FaceOrientation.Front, false).TextureMatrix = (Dirtyable<Matrix?>) new Matrix?(new Matrix(0.25f, 0.0f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.0f, (float) index1 / 2f, (float) index2 / 4f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
          this.TomePages.AddFace(size, origin, FaceOrientation.Back, false).TextureMatrix = (Dirtyable<Matrix?>) new Matrix?(new Matrix(0.25f, 0.0f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.0f, (float) (((double) index1 + 0.5) / 2.0), (float) index2 / 4f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
          this.TomePages.CollapseWithNormalTexture<FezVertexPositionNormalTexture>(this.TomePages.Groups.Count - 2, 2).Enabled = false;
        }
      }
    }

    private void FillInPlanes()
    {
      bool flag = true;
      Color color;
      if (this.LevelManager.WaterType == LiquidType.Sewer)
        color = new Color(32, 70, 49);
      else if (this.LevelManager.WaterType == LiquidType.Lava)
        color = Color.Black;
      else if (this.LevelManager.BlinkingAlpha)
      {
        color = Color.Black;
      }
      else
      {
        flag = false;
        color = new Color(56, 40, 95);
      }
      MenuCube menuCube = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.LitVertexColored litVertexColored1 = new DefaultEffect.LitVertexColored();
      litVertexColored1.Fullbright = flag;
      DefaultEffect.LitVertexColored litVertexColored2 = litVertexColored1;
      mesh2.Effect = (BaseEffect) litVertexColored2;
      mesh1.Material = this.AoInstance.Material;
      mesh1.Blending = new BlendingMode?(BlendingMode.Alphablending);
      Mesh mesh3 = mesh1;
      menuCube.HidingPlanes = mesh3;
      Vector3 size = this.AoInstance.ArtObject.Size;
      int fromGroup = 0;
      foreach (MenuCubeFace face in Util.GetValues<MenuCubeFace>())
      {
        if (face != MenuCubeFace.AntiCubes || this.GameState.SaveData.SecretCubes > 0)
        {
          Vector3 vector3_1 = FezMath.Abs(MenuCubeFaceExtensions.GetForward(face));
          for (int index = 0; index < MenuCubeFaceExtensions.GetCount(face) && index < 32; ++index)
          {
            int num1 = index;
            if (num1 >= 14)
              num1 += 2;
            if (num1 >= 20)
              num1 += 2;
            Vector3 vector3_2 = (float) MenuCubeFaceExtensions.GetDepth(face) * MenuCubeFaceExtensions.GetForward(face) / 16f;
            Vector3 vector3_3 = (float) MenuCubeFaceExtensions.GetSize(face) * (Vector3.One - vector3_1) / 16f;
            Vector3 vector3_4 = -vector3_2 / 2f + vector3_3 * MenuCubeFaceExtensions.GetRight(face) / 2f;
            Vector3 vector3_5 = -vector3_2 / 2f + vector3_3 * -MenuCubeFaceExtensions.GetRight(face) / 2f;
            Vector3 vector3_6 = -vector3_2 / 2f + vector3_3 * Vector3.Up / 2f;
            Vector3 vector3_7 = -vector3_2 / 2f + vector3_3 * Vector3.Down / 2f;
            Vector3 vector3_8 = -MenuCubeFaceExtensions.GetForward(face) * (float) MenuCubeFaceExtensions.GetDepth(face) / 32f;
            Vector3 vector3_9 = Vector3.Up * vector3_3 / 2f;
            Vector3 vector3_10 = -MenuCubeFaceExtensions.GetForward(face) * (float) MenuCubeFaceExtensions.GetDepth(face) / 32f;
            Vector3 vector3_11 = Vector3.Up * vector3_3 / 2f;
            Vector3 vector3_12 = -MenuCubeFaceExtensions.GetRight(face) * vector3_3 / 2f;
            Vector3 vector3_13 = MenuCubeFaceExtensions.GetForward(face) * (float) MenuCubeFaceExtensions.GetDepth(face) / 32f;
            Vector3 vector3_14 = -MenuCubeFaceExtensions.GetRight(face) * vector3_3 / 2f;
            Vector3 vector3_15 = MenuCubeFaceExtensions.GetForward(face) * (float) MenuCubeFaceExtensions.GetDepth(face) / 32f;
            Group group = this.HidingPlanes.AddGroup();
            group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalColor>(new VertexPositionNormalColor[16]
            {
              new VertexPositionNormalColor(vector3_4 - vector3_8 + vector3_9, -MenuCubeFaceExtensions.GetRight(face), color),
              new VertexPositionNormalColor(vector3_4 - vector3_8 - vector3_9, -MenuCubeFaceExtensions.GetRight(face), color),
              new VertexPositionNormalColor(vector3_4 + vector3_8 + vector3_9, -MenuCubeFaceExtensions.GetRight(face), color),
              new VertexPositionNormalColor(vector3_4 + vector3_8 - vector3_9, -MenuCubeFaceExtensions.GetRight(face), color),
              new VertexPositionNormalColor(vector3_5 - vector3_10 + vector3_11, MenuCubeFaceExtensions.GetRight(face), color),
              new VertexPositionNormalColor(vector3_5 - vector3_10 - vector3_11, MenuCubeFaceExtensions.GetRight(face), color),
              new VertexPositionNormalColor(vector3_5 + vector3_10 + vector3_11, MenuCubeFaceExtensions.GetRight(face), color),
              new VertexPositionNormalColor(vector3_5 + vector3_10 - vector3_11, MenuCubeFaceExtensions.GetRight(face), color),
              new VertexPositionNormalColor(vector3_6 - vector3_12 + vector3_13, Vector3.Down, color),
              new VertexPositionNormalColor(vector3_6 - vector3_12 - vector3_13, Vector3.Down, color),
              new VertexPositionNormalColor(vector3_6 + vector3_12 + vector3_13, Vector3.Down, color),
              new VertexPositionNormalColor(vector3_6 + vector3_12 - vector3_13, Vector3.Down, color),
              new VertexPositionNormalColor(vector3_7 - vector3_14 + vector3_15, Vector3.Up, color),
              new VertexPositionNormalColor(vector3_7 - vector3_14 - vector3_15, Vector3.Up, color),
              new VertexPositionNormalColor(vector3_7 + vector3_14 + vector3_15, Vector3.Up, color),
              new VertexPositionNormalColor(vector3_7 + vector3_14 - vector3_15, Vector3.Up, color)
            }, new int[24]
            {
              0,
              1,
              2,
              2,
              1,
              3,
              4,
              6,
              5,
              5,
              6,
              7,
              8,
              9,
              10,
              10,
              9,
              11,
              13,
              12,
              14,
              13,
              14,
              15
            }, PrimitiveType.TriangleList);
            int num2 = (int) Math.Sqrt((double) MenuCubeFaceExtensions.GetCount(face));
            int num3 = num1 % num2;
            int num4 = num1 / num2;
            Vector3 vector3_16 = (float) (num3 * MenuCubeFaceExtensions.GetSpacing(face)) / 16f * MenuCubeFaceExtensions.GetRight(face) + (float) (num4 * MenuCubeFaceExtensions.GetSpacing(face)) / 16f * -Vector3.UnitY + size / 2f * (MenuCubeFaceExtensions.GetForward(face) + Vector3.Up - MenuCubeFaceExtensions.GetRight(face)) + MenuCubeFaceExtensions.GetForward(face) * -8f / 16f + (Vector3.Down + MenuCubeFaceExtensions.GetRight(face)) * (float) MenuCubeFaceExtensions.GetOffset(face) / 16f;
            group.Position = vector3_16;
          }
          this.HidingPlanes.CollapseToBufferWithNormal<VertexPositionNormalColor>(fromGroup, this.HidingPlanes.Groups.Count - fromGroup).CustomData = (object) face;
          ++fromGroup;
        }
      }
    }

    private void CreateGoldenCubeFace()
    {
      Vector3 size1 = this.AoInstance.ArtObject.Size;
      Trile trile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.CubeShard));
      bool flag = this.LevelManager.WaterType == LiquidType.Sewer || this.LevelManager.WaterType == LiquidType.Lava || this.LevelManager.BlinkingAlpha;
      this.GoldenCubes = new Mesh()
      {
        Effect = flag ? (BaseEffect) new DefaultEffect.Textured() : (BaseEffect) new DefaultEffect.LitTextured(),
        Texture = this.LevelMaterializer.TrilesMesh.Texture,
        Blending = new BlendingMode?(BlendingMode.Opaque),
        Material = this.AoInstance.Material
      };
      if (trile == null)
        return;
      ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry = trile.Geometry;
      int offset = MenuCubeFaceExtensions.GetOffset(MenuCubeFace.CubeShards);
      int spacing = MenuCubeFaceExtensions.GetSpacing(MenuCubeFace.CubeShards);
      for (int index = 0; index < this.GameState.SaveData.CubeShards; ++index)
      {
        int num1 = index;
        if (num1 >= 14)
          num1 += 2;
        if (num1 >= 20)
          num1 += 2;
        Group group = this.GoldenCubes.AddGroup();
        group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(Enumerable.ToArray<VertexPositionNormalTextureInstance>((IEnumerable<VertexPositionNormalTextureInstance>) geometry.Vertices), geometry.Indices, geometry.PrimitiveType);
        int num2 = num1 % 6;
        int num3 = num1 / 6;
        group.Position = size1 / 2f * (Vector3.UnitZ + Vector3.UnitY - Vector3.UnitX) + (float) (offset + num2 * spacing) / 16f * Vector3.UnitX + (float) (offset + num3 * spacing) / 16f * -Vector3.UnitY + 0.5f * -Vector3.UnitZ;
        group.Scale = new Vector3(0.5f);
      }
      Group cubesGroup = this.GoldenCubes.CollapseToBufferWithNormal<VertexPositionNormalTextureInstance>();
      this.GoldenCubes.CustomRenderingHandler = (Mesh.RenderingHandler) ((m, e) =>
      {
        foreach (Group item_0 in m.Groups)
        {
          (e as DefaultEffect).AlphaIsEmissive = item_0 == cubesGroup;
          item_0.Draw(e);
        }
      });
      Group group1 = this.GoldenCubes.AddFace(new Vector3(0.5f, 0.5f, 1f), Vector3.Zero, FaceOrientation.Front, true);
      group1.Position = size1 / 2f * (Vector3.UnitZ + Vector3.UnitY) + 21.0 / 16.0 * -Vector3.UnitY + 3.0 / 16.0 * -Vector3.UnitX + 0.499f * -Vector3.UnitZ;
      group1.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/hud/tiny_key");
      group1.Blending = new BlendingMode?(BlendingMode.Alphablending);
      string text1 = this.GameState.SaveData.Keys.ToString();
      Vector2 vector2_1 = this.FontManager.Small.MeasureString(text1);
      Vector3 size2 = new Vector3(vector2_1 / 16f / 2f - 1.0 / 16.0 * Vector2.One, 1f);
      if (Culture.IsCJK)
        size2 /= 3f;
      Group group2 = this.GoldenCubes.AddFace(size2, Vector3.Zero, FaceOrientation.Front, true);
      group2.Position = size1 / 2f * (Vector3.UnitZ + Vector3.UnitY) + 1.25f * -Vector3.UnitY + 7.0 / 32.0 * Vector3.UnitX + 0.499f * -Vector3.UnitZ;
      group2.Blending = new BlendingMode?(BlendingMode.Alphablending);
      if (Culture.IsCJK)
        group2.SamplerState = SamplerState.AnisotropicClamp;
      RenderTarget2D renderTarget1 = new RenderTarget2D(this.GraphicsDevice, (int) vector2_1.X, (int) vector2_1.Y, false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, this.GraphicsDevice.PresentationParameters.DepthStencilFormat, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
      using (SpriteBatch spriteBatch = new SpriteBatch(this.GraphicsDevice))
      {
        this.GraphicsDevice.SetRenderTarget(renderTarget1);
        this.GraphicsDevice.Clear(ClearOptions.Target, ColorEx.TransparentWhite, 1f, 0);
        GraphicsDeviceExtensions.BeginPoint(spriteBatch);
        float num = Culture.IsCJK ? this.FontManager.TopSpacing * 2f : this.FontManager.TopSpacing;
        spriteBatch.DrawString(this.FontManager.Small, text1, Vector2.Zero, Color.White, 0.0f, new Vector2(0.0f, (float) (-(double) num * 4.0 / 5.0)), 1f, SpriteEffects.None, 0.0f);
        spriteBatch.End();
        this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
        group2.Texture = (Texture) renderTarget1;
      }
      string text2 = this.GameState.SaveData.CubeShards.ToString();
      int num4 = 2;
      Vector2 vector2_2 = this.FontManager.Small.MeasureString(text2) * (float) num4;
      size2 = new Vector3(vector2_2 / 16f / 2f - 1.0 / 16.0 * Vector2.One, 1f);
      if (Culture.IsCJK)
        size2 /= 3.25f;
      Group group3 = this.GoldenCubes.AddFace(size2, Vector3.Zero, FaceOrientation.Front, true);
      group3.Position = size1 / 2f * Vector3.UnitZ + 0.499f * -Vector3.UnitZ;
      group3.Blending = new BlendingMode?(BlendingMode.Alphablending);
      if (Culture.IsCJK)
        group3.SamplerState = SamplerState.AnisotropicClamp;
      RenderTarget2D renderTarget2 = new RenderTarget2D(this.GraphicsDevice, (int) Math.Ceiling((double) vector2_2.X), (int) Math.Ceiling((double) vector2_2.Y), false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, this.GraphicsDevice.PresentationParameters.DepthStencilFormat, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
      using (SpriteBatch spriteBatch = new SpriteBatch(this.GraphicsDevice))
      {
        this.GraphicsDevice.SetRenderTarget(renderTarget2);
        this.GraphicsDevice.Clear(ClearOptions.Target, ColorEx.TransparentWhite, 1f, 0);
        GraphicsDeviceExtensions.BeginPoint(spriteBatch);
        spriteBatch.DrawString(this.FontManager.Small, text2, Vector2.Zero, Color.White, 0.0f, new Vector2(0.0f, (float) (-(double) this.FontManager.TopSpacing * 4.0 / 5.0)), (float) num4, SpriteEffects.None, 0.0f);
        spriteBatch.End();
        this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
        group3.Texture = (Texture) renderTarget2;
      }
    }

    private void CreateMapsFace()
    {
      bool flag = this.LevelManager.WaterType == LiquidType.Sewer || this.LevelManager.WaterType == LiquidType.Lava || this.LevelManager.BlinkingAlpha;
      MenuCube menuCube = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.LitTextured litTextured1 = new DefaultEffect.LitTextured();
      litTextured1.Fullbright = flag;
      DefaultEffect.LitTextured litTextured2 = litTextured1;
      mesh2.Effect = (BaseEffect) litTextured2;
      mesh1.Blending = new BlendingMode?(BlendingMode.Alphablending);
      mesh1.Material = this.AoInstance.Material;
      mesh1.SamplerState = SamplerState.PointClamp;
      Mesh mesh3 = mesh1;
      menuCube.Maps = mesh3;
      Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle(Vector3.Up, -1.570796f);
      int num1 = 0;
      foreach (string str in this.GameState.SaveData.Maps)
      {
        Texture2D texture2D1 = this.CMProvider.Global.Load<Texture2D>("Other Textures/maps/" + str + "_1");
        Texture2D texture2D2 = this.CMProvider.Global.Load<Texture2D>("Other Textures/maps/" + str + "_2");
        int num2 = (int) Math.Sqrt((double) MenuCubeFaceExtensions.GetCount(MenuCubeFace.Maps));
        Vector2 vector2 = new Vector2((float) (num1 % num2), (float) (num1 / num2));
        Vector3 size = this.AoInstance.ArtObject.Size;
        Vector3 vector3 = (float) ((double) vector2.X * (double) MenuCubeFaceExtensions.GetSpacing(MenuCubeFace.Maps) / 16.0) * MenuCubeFaceExtensions.GetRight(MenuCubeFace.Maps) + (float) ((double) vector2.Y * (double) MenuCubeFaceExtensions.GetSpacing(MenuCubeFace.Maps) / 16.0) * -Vector3.UnitY + size / 2f * (MenuCubeFaceExtensions.GetForward(MenuCubeFace.Maps) + Vector3.Up - MenuCubeFaceExtensions.GetRight(MenuCubeFace.Maps)) + -MenuCubeFaceExtensions.GetForward(MenuCubeFace.Maps) * (float) MenuCubeFaceExtensions.GetDepth(MenuCubeFace.Maps) / 16f / 2f + (Vector3.Down + MenuCubeFaceExtensions.GetRight(MenuCubeFace.Maps)) * (float) MenuCubeFaceExtensions.GetOffset(MenuCubeFace.Maps) / 16f;
        Group group1 = this.Maps.AddGroup();
        group1.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionNormalTexture>(new FezVertexPositionNormalTexture[4]
        {
          new FezVertexPositionNormalTexture(new Vector3(-1f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(1f, 0.0f)),
          new FezVertexPositionNormalTexture(new Vector3(-1f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(1f, 1f)),
          new FezVertexPositionNormalTexture(new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.625f, 0.0f)),
          new FezVertexPositionNormalTexture(new Vector3(0.0f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.625f, 1f))
        }, new int[6]
        {
          0,
          1,
          2,
          2,
          1,
          3
        }, PrimitiveType.TriangleList);
        group1.Scale = new Vector3(0.375f, 1f, 1f) * 1.5f;
        group1.Texture = (Texture) texture2D1;
        group1.Position = vector3 + MenuCubeFaceExtensions.GetRight(MenuCubeFace.Maps) * 0.125f * 1.5f;
        group1.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 0.3926991f) * fromAxisAngle;
        Group group2 = this.Maps.CloneGroup(group1);
        group2.InvertNormals<FezVertexPositionNormalTexture>();
        group2.Texture = (Texture) texture2D2;
        group2.CullMode = new CullMode?(CullMode.CullClockwiseFace);
        group2.TextureMatrix = (Dirtyable<Matrix?>) new Matrix?(new Matrix(-1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 1f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
        Group group3 = this.Maps.AddGroup();
        group3.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionNormalTexture>(new FezVertexPositionNormalTexture[4]
        {
          new FezVertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.625f, 0.0f)),
          new FezVertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.625f, 1f)),
          new FezVertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.375f, 0.0f)),
          new FezVertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.375f, 1f))
        }, new int[6]
        {
          0,
          1,
          2,
          2,
          1,
          3
        }, PrimitiveType.TriangleList);
        group3.Scale = new Vector3(0.25f, 1f, 1f) * 1.5f;
        group3.Texture = (Texture) texture2D1;
        group3.Position = vector3;
        group3.Rotation = fromAxisAngle;
        Group group4 = this.Maps.CloneGroup(group3);
        group4.InvertNormals<FezVertexPositionNormalTexture>();
        group4.Texture = (Texture) texture2D2;
        group4.CullMode = new CullMode?(CullMode.CullClockwiseFace);
        group4.TextureMatrix = (Dirtyable<Matrix?>) new Matrix?(new Matrix(-1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 1f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
        Group group5 = this.Maps.AddGroup();
        group5.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionNormalTexture>(new FezVertexPositionNormalTexture[4]
        {
          new FezVertexPositionNormalTexture(new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.375f, 0.0f)),
          new FezVertexPositionNormalTexture(new Vector3(0.0f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.375f, 1f)),
          new FezVertexPositionNormalTexture(new Vector3(1f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.0f, 0.0f)),
          new FezVertexPositionNormalTexture(new Vector3(1f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.5f), new Vector2(0.0f, 1f))
        }, new int[6]
        {
          0,
          1,
          2,
          2,
          1,
          3
        }, PrimitiveType.TriangleList);
        group5.Scale = new Vector3(0.375f, 1f, 1f) * 1.5f;
        group5.Texture = (Texture) texture2D1;
        group5.Position = vector3 - MenuCubeFaceExtensions.GetRight(MenuCubeFace.Maps) * 0.125f * 1.5f;
        group5.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 0.3926991f) * fromAxisAngle;
        Group group6 = this.Maps.CloneGroup(group5);
        group6.InvertNormals<FezVertexPositionNormalTexture>();
        group6.Texture = (Texture) texture2D2;
        group6.CullMode = new CullMode?(CullMode.CullClockwiseFace);
        group6.TextureMatrix = (Dirtyable<Matrix?>) new Matrix?(new Matrix(-1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 1f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
        ++num1;
      }
    }

    private void CreateArtifactsFace()
    {
      foreach (ActorType type in this.GameState.SaveData.Artifacts)
      {
        if (type == ActorType.Tome)
        {
          ArtObject artObject1 = this.CMProvider.Global.Load<ArtObject>("Art Objects/TOME_BAO");
          int key1 = IdentifierPool.FirstAvailable<ArtObjectInstance>(this.LevelManager.ArtObjects);
          this.TomeBackAo = new ArtObjectInstance(artObject1)
          {
            Id = key1,
            Position = this.PlayerManager.Center
          };
          this.TomeBackAo.Initialize();
          this.TomeBackAo.Material = this.AoInstance.Material;
          this.TomeBackAo.Hidden = true;
          this.LevelManager.ArtObjects.Add(key1, this.TomeBackAo);
          this.ArtifactAOs.Add(this.TomeBackAo);
          ArtObject artObject2 = this.CMProvider.Global.Load<ArtObject>("Art Objects/TOME_COVERAO");
          int key2 = IdentifierPool.FirstAvailable<ArtObjectInstance>(this.LevelManager.ArtObjects);
          this.TomeCoverAo = new ArtObjectInstance(artObject2)
          {
            Id = key2,
            Position = this.PlayerManager.Center
          };
          this.TomeCoverAo.Initialize();
          this.TomeCoverAo.Material = this.AoInstance.Material;
          this.TomeCoverAo.Hidden = true;
          this.LevelManager.ArtObjects.Add(key2, this.TomeCoverAo);
          this.ArtifactAOs.Add(this.TomeCoverAo);
        }
        else
        {
          ArtObject artObject = this.CMProvider.Global.Load<ArtObject>("Art Objects/" + ActorTypeExtensions.GetArtObjectName(type));
          int key = IdentifierPool.FirstAvailable<ArtObjectInstance>(this.LevelManager.ArtObjects);
          ArtObjectInstance artObjectInstance = new ArtObjectInstance(artObject)
          {
            Id = key,
            Position = this.PlayerManager.Center
          };
          artObjectInstance.Initialize();
          artObjectInstance.Material = this.AoInstance.Material;
          artObjectInstance.Hidden = true;
          this.LevelManager.ArtObjects.Add(key, artObjectInstance);
          this.ArtifactAOs.Add(artObjectInstance);
        }
      }
    }

    private void CreateAntiCubeFace()
    {
      Vector3 size = this.AoInstance.ArtObject.Size;
      Vector3 forward = MenuCubeFaceExtensions.GetForward(MenuCubeFace.AntiCubes);
      Vector3 right = MenuCubeFaceExtensions.GetRight(MenuCubeFace.AntiCubes);
      int offset = MenuCubeFaceExtensions.GetOffset(MenuCubeFace.AntiCubes);
      int spacing = MenuCubeFaceExtensions.GetSpacing(MenuCubeFace.AntiCubes);
      bool flag = this.LevelManager.WaterType == LiquidType.Sewer || this.LevelManager.WaterType == LiquidType.Lava || this.LevelManager.BlinkingAlpha;
      if (this.GameState.SaveData.SecretCubes == 0)
      {
        this.AntiCubes = new Mesh()
        {
          Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/MENU_CUBE_COVER")),
          Blending = new BlendingMode?(BlendingMode.Alphablending),
          Material = this.AoInstance.Material,
          SamplerState = SamplerState.PointClamp
        };
        this.AntiCubes.Texture = this.LevelManager.WaterType != LiquidType.Sewer ? (this.LevelManager.WaterType != LiquidType.Lava ? (!this.LevelManager.BlinkingAlpha ? (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/MENU_CUBE_COVER")) : (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/MENU_CUBE_COVER_CMY"))) : (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/MENU_CUBE_COVER_VIRTUAL"))) : (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/MENU_CUBE_COVER_GB"));
        this.AntiCubes.Effect = flag ? (BaseEffect) new DefaultEffect.Textured() : (BaseEffect) new DefaultEffect.LitTextured();
        this.AntiCubes.AddFace(size, size * forward / 2f, FezMath.OrientationFromDirection(forward), true);
      }
      else
      {
        Trile trile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.SecretCube));
        if (trile == null)
        {
          this.AntiCubes = new Mesh()
          {
            Effect = (BaseEffect) new DefaultEffect.LitTextured(),
            Texture = this.LevelMaterializer.TrilesMesh.Texture,
            Blending = new BlendingMode?(BlendingMode.Alphablending),
            Material = this.AoInstance.Material
          };
          Logger.Log("MenuCube", "No anti-cube trile in " + this.LevelManager.TrileSet.Name);
        }
        else
        {
          ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry = trile.Geometry;
          this.AntiCubes = new Mesh()
          {
            Effect = flag ? (BaseEffect) new DefaultEffect.Textured() : (BaseEffect) new DefaultEffect.LitTextured(),
            Texture = this.LevelMaterializer.TrilesMesh.Texture,
            Blending = new BlendingMode?(BlendingMode.Opaque),
            Material = this.AoInstance.Material
          };
          for (int index = 0; index < this.GameState.SaveData.SecretCubes; ++index)
          {
            int num1 = index;
            if (num1 >= 14)
              num1 += 2;
            if (num1 >= 20)
              num1 += 2;
            Group group = this.AntiCubes.AddGroup();
            group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(Enumerable.ToArray<VertexPositionNormalTextureInstance>((IEnumerable<VertexPositionNormalTextureInstance>) geometry.Vertices), geometry.Indices, geometry.PrimitiveType);
            int num2 = num1 % 6;
            int num3 = num1 / 6;
            group.Position = size / 2f * (forward + Vector3.UnitY - right) + (float) (offset + num2 * spacing) / 16f * right + (float) (offset + num3 * spacing) / 16f * -Vector3.UnitY + 0.5f * -forward;
            group.Scale = new Vector3(0.5f);
            group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 1.570796f * (float) RandomHelper.Random.Next(0, 4));
          }
          Group cubesGroup = this.AntiCubes.CollapseToBufferWithNormal<VertexPositionNormalTextureInstance>();
          this.AntiCubes.CustomRenderingHandler = (Mesh.RenderingHandler) ((m, e) =>
          {
            foreach (Group item_0 in m.Groups)
            {
              (e as DefaultEffect).AlphaIsEmissive = item_0 == cubesGroup;
              item_0.Draw(e);
            }
          });
        }
        string text = this.GameState.SaveData.SecretCubes.ToString();
        int num = 2;
        Vector2 vector2_1 = this.FontManager.Small.MeasureString(text) * (float) num;
        Vector2 vector2_2 = vector2_1 / 16f / 2f - 1.0 / 16.0 * Vector2.One;
        if (Culture.IsCJK)
          vector2_2 /= 3.25f;
        Group group1 = this.GoldenCubes.AddFace(new Vector3(1f, vector2_2.Y, vector2_2.X), Vector3.Zero, FezMath.OrientationFromDirection(forward), true);
        group1.Position = size / 2f * forward + 0.499f * -forward;
        group1.Blending = new BlendingMode?(BlendingMode.Alphablending);
        if (Culture.IsCJK)
          group1.SamplerState = SamplerState.AnisotropicClamp;
        RenderTarget2D renderTarget = new RenderTarget2D(this.GraphicsDevice, (int) Math.Ceiling((double) vector2_1.X), (int) Math.Ceiling((double) vector2_1.Y), false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, this.GraphicsDevice.PresentationParameters.DepthStencilFormat, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
        using (SpriteBatch spriteBatch = new SpriteBatch(this.GraphicsDevice))
        {
          this.GraphicsDevice.SetRenderTarget(renderTarget);
          this.GraphicsDevice.Clear(ClearOptions.Target, ColorEx.TransparentWhite, 1f, 0);
          GraphicsDeviceExtensions.BeginPoint(spriteBatch);
          spriteBatch.DrawString(this.FontManager.Small, text, Vector2.Zero, Color.White, 0.0f, new Vector2(0.0f, (float) (-(double) this.FontManager.TopSpacing * 4.0 / 5.0)), (float) num, SpriteEffects.None, 0.0f);
          spriteBatch.End();
          this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
          group1.Texture = (Texture) renderTarget;
        }
      }
    }

    private void CreateHighlights()
    {
      this.Highlights = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.VertexColored(),
        Material = this.AoInstance.Material,
        Blending = new BlendingMode?(BlendingMode.Alphablending)
      };
      Color color = this.LevelManager.BlinkingAlpha ? Color.Yellow : Color.White;
      for (int index = 0; index < 4; ++index)
        this.Highlights.AddGroup();
      this.CreateFaceHighlights(MenuCubeFace.Maps, color);
      this.CreateFaceHighlights(MenuCubeFace.Artifacts, color);
    }

    private void CreateFaceHighlights(MenuCubeFace cf, Color color)
    {
      Vector3 vector3 = color.ToVector3();
      Vector3 size = this.AoInstance.ArtObject.Size;
      for (int index = 0; index < 4; ++index)
        this.Highlights.AddWireframeFace(new Vector3((float) ((double) MenuCubeFaceExtensions.GetSize(cf) * 1.25 / 16.0)) * (Vector3.UnitY + FezMath.Abs(MenuCubeFaceExtensions.GetRight(cf))) * (float) (0.949999988079071 - (double) index * 0.0500000007450581), Vector3.Zero, FezMath.OrientationFromDirection(MenuCubeFaceExtensions.GetForward(cf)), new Color(vector3.X, vector3.Y, vector3.Z, (float) (1.0 - (double) index / 4.0)), true).Position = size / 2f * (MenuCubeFaceExtensions.GetForward(cf) + Vector3.Up - MenuCubeFaceExtensions.GetRight(cf)) + MenuCubeFaceExtensions.GetForward(cf) * (-7.0 / 16.0) + (Vector3.Down + MenuCubeFaceExtensions.GetRight(cf)) * (float) MenuCubeFaceExtensions.GetOffset(cf) / 16f;
    }

    private void StartInTransition()
    {
      this.GameState.SkipRendering = true;
      this.LevelManager.SkipInvalidation = true;
      SettingsManager.SetupViewport(this.GraphicsDevice, false);
      this.CameraManager.Radius = 26.25f;
      this.CameraManager.ChangeViewpoint(Viewpoint.Perspective, 1.5f);
      this.GameState.SkyOpacity = 0.0f;
      Quaternion phi180 = this.OriginalRotation * Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f);
      Vector3 aoMaxPos = this.PlayerManager.Center + this.AoInstance.ArtObject.Size * Vector3.UnitY / 2f + Vector3.UnitY;
      Waiters.Interpolate(0.75, (Action<float>) (s =>
      {
        if (!this.Enabled)
          return;
        float local_0 = Easing.EaseOut((double) s, EasingType.Cubic);
        this.AoInstance.Material.Opacity = local_0;
        this.AoInstance.MarkDirty();
        this.AntiCubes.Scale = this.Maps.Scale = this.HidingPlanes.Scale = this.Highlights.Scale = this.GoldenCubes.Scale = this.AoInstance.Scale = new Vector3(local_0);
        this.AntiCubes.Position = this.Maps.Position = this.HidingPlanes.Position = this.Highlights.Position = this.AoInstance.Position = this.PlayerManager.Center + local_0 * (this.AoInstance.ArtObject.Size * Vector3.UnitY / 2f + Vector3.UnitY);
        this.AntiCubes.Rotation = this.Maps.Rotation = this.HidingPlanes.Rotation = this.Highlights.Rotation = this.GoldenCubes.Rotation = this.AoInstance.Rotation = Quaternion.Slerp(phi180, this.OriginalRotation, local_0);
        this.CameraManager.Center = Vector3.Lerp(this.OriginalCenter, aoMaxPos, local_0);
        this.BlurEffect.BlurWidth = local_0;
        this.TransformArtifacts();
      }), (Action) (() => this.BlurEffect.BlurWidth = 1f));
    }

    private void StartOutTransition()
    {
      this.CameraManager.PixelsPerTrixel = this.OriginalPixPerTrix;
      this.CameraManager.Center = this.OriginalCenter;
      this.CameraManager.ChangeViewpoint(this.OriginalViewpoint == Viewpoint.None ? this.CameraManager.LastViewpoint : this.OriginalViewpoint, 0.0f);
      this.CameraManager.SnapInterpolation();
      this.GameState.SkipRendering = false;
      this.LevelManager.SkipInvalidation = false;
      this.GameState.SkyOpacity = 1f;
      foreach (ArtObjectInstance artObjectInstance in this.ArtifactAOs)
      {
        artObjectInstance.SoftDispose();
        this.LevelManager.ArtObjects.Remove(artObjectInstance.Id);
      }
      SoundEffectExtensions.Emit(this.exitSound);
      this.GameService.CloseScroll((string) null);
      this.GameState.InMenuCube = false;
      this.GameState.DisallowRotation = false;
      Waiters.Interpolate(0.5, (Action<float>) (s =>
      {
        float local_0 = 1f - Easing.EaseOut((double) s, EasingType.Cubic);
        this.AoInstance.Material.Opacity = local_0;
        this.BlurEffect.BlurWidth = local_0;
      }), (Action) (() => ServiceHelper.RemoveComponent<MenuCube>(this)));
    }

    protected override void Dispose(bool disposing)
    {
      if (this.oldTextureCache != null)
      {
        this.AoInstance.ArtObject.Cubemap = this.oldTextureCache;
        new ArtObjectMaterializer(this.AoInstance.ArtObject).RecomputeTexCoords(true);
      }
      this.LevelManager.ArtObjects.Remove(this.AoInstance.Id);
      this.AoInstance.SoftDispose();
      this.GameState.SkyOpacity = 1f;
      this.PlayerManager.CanControl = true;
      if (this.InRtHandle != null)
      {
        this.TargetRenderingManager.UnscheduleHook(this.InRtHandle.Target);
        this.TargetRenderingManager.ReturnTarget(this.InRtHandle);
      }
      this.InRtHandle = (RenderTargetHandle) null;
      if (this.OutRtHandle != null)
      {
        this.TargetRenderingManager.UnscheduleHook(this.OutRtHandle.Target);
        this.TargetRenderingManager.ReturnTarget(this.OutRtHandle);
      }
      this.OutRtHandle = (RenderTargetHandle) null;
      this.HidingPlanes.Dispose();
      this.GoldenCubes.Dispose();
      this.AntiCubes.Dispose();
      this.TomePages.Dispose();
      this.Maps.Dispose();
      this.Highlights.Dispose();
      this.BlurEffect.Dispose();
      if (this.eBackground != null && !this.eBackground.Dead)
      {
        this.eBackground.FadeOutAndDie(0.25f, false);
        this.eBackground = (SoundEmitter) null;
      }
      this.GameService.CloseScroll((string) null);
      if (!this.wasLowPass)
        this.SoundManager.FadeFrequencies(false);
      MenuCube.Instance = (MenuCube) null;
      base.Dispose(disposing);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.Loading || this.GameState.InMap)
        return;
      if (!this.GameState.InMenuCube)
        ServiceHelper.RemoveComponent<MenuCube>(this);
      else if (!this.GameState.MenuCubeIsZoomed && !this.CameraManager.ProjectionTransition && (this.InputManager.Back == FezButtonState.Pressed || this.InputManager.CancelTalk == FezButtonState.Pressed || this.InputManager.OpenInventory == FezButtonState.Pressed))
      {
        this.ScheduleExit = true;
        this.Enabled = false;
        this.Resolved = false;
        this.TargetRenderingManager.ScheduleHook(this.DrawOrder, this.OutRtHandle.Target);
      }
      else
      {
        bool flag = FezMath.IsOrthographic(this.CameraManager.Viewpoint);
        bool menuCubeIsZoomed = this.GameState.MenuCubeIsZoomed;
        if (this.InputManager.RotateRight == FezButtonState.Pressed)
        {
          if (this.TomeOpen)
          {
            if (this.TomePageIndex > 0)
            {
              if (this.TomePageIndex != 0)
                --this.TomePageIndex;
              int tpi = this.TomePageIndex;
              Waiters.Interpolate(0.25, (Action<float>) (s =>
              {
                s = Easing.EaseOut((double) FezMath.Saturate(1f - s), EasingType.Quadratic);
                this.TomePages.Groups[tpi].Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) (-2.35619449615479 * (double) s * (1.0 - (double) tpi / 8.0 * 0.0500000007450581)));
              }), (Action) (() =>
              {
                this.TomePages.Groups[tpi].Rotation = Quaternion.Identity;
                if (tpi == 8)
                  return;
                this.TomePages.Groups[tpi + 1].Enabled = false;
              }));
            }
          }
          else
          {
            SoundEffectExtensions.Emit(this.rotateRightSound);
            if (!flag && !menuCubeIsZoomed)
            {
              this.LastFace = this.Face;
              ++this.Face;
              if (this.Face > MenuCubeFace.AntiCubes)
                this.Face = MenuCubeFace.CubeShards;
              ++this.Turns;
              this.GameService.CloseScroll((string) null);
              if (this.Face != MenuCubeFace.AntiCubes || this.GameState.SaveData.SecretCubes > 0)
                this.GameState.ShowScroll(MenuCubeFaceExtensions.GetTitle(this.Face), 0.0f, true);
              foreach (Group group in this.HidingPlanes.Groups)
              {
                MenuCubeFace menuCubeFace = (MenuCubeFace) group.CustomData;
                group.Enabled = menuCubeFace == this.Face || menuCubeFace == this.LastFace;
              }
            }
            if (FezMath.IsOrthographic(this.CameraManager.Viewpoint) && !menuCubeIsZoomed)
              this.OriginalRotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, -1.570796f);
          }
        }
        else if (this.InputManager.RotateLeft == FezButtonState.Pressed)
        {
          if (this.TomeOpen)
          {
            if (this.TomePageIndex <= 8)
            {
              int tpi = this.TomePageIndex;
              if (this.TomePageIndex <= 8)
                ++this.TomePageIndex;
              Waiters.Interpolate(0.25, (Action<float>) (s =>
              {
                s = Easing.EaseOut((double) FezMath.Saturate(s), EasingType.Quadratic);
                if (tpi != 8)
                  this.TomePages.Groups[tpi + 1].Enabled = true;
                this.TomePages.Groups[tpi].Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) (-2.35619449615479 * (double) s * (1.0 - (double) tpi / 8.0 * 0.0500000007450581)));
              }), (Action) (() => this.TomePages.Groups[tpi].Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) (-2.35619449615479 * (1.0 - (double) tpi / 8.0 * 0.0500000007450581)))));
            }
          }
          else
          {
            SoundEffectExtensions.Emit(this.rotateLeftSound);
            if (!flag && !menuCubeIsZoomed)
            {
              this.LastFace = this.Face;
              --this.Face;
              if (this.Face < MenuCubeFace.CubeShards)
                this.Face = MenuCubeFace.AntiCubes;
              --this.Turns;
              this.GameService.CloseScroll((string) null);
              if (this.Face != MenuCubeFace.AntiCubes || this.GameState.SaveData.SecretCubes > 0)
                this.GameState.ShowScroll(MenuCubeFaceExtensions.GetTitle(this.Face), 0.0f, true);
              foreach (Group group in this.HidingPlanes.Groups)
              {
                MenuCubeFace menuCubeFace = (MenuCubeFace) group.CustomData;
                group.Enabled = menuCubeFace == this.Face || menuCubeFace == this.LastFace;
              }
            }
            if (FezMath.IsOrthographic(this.CameraManager.Viewpoint) && !menuCubeIsZoomed)
              this.OriginalRotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, 1.570796f);
          }
        }
        this.UpdateHighlights((float) gameTime.TotalGameTime.TotalSeconds);
        this.AntiCubes.Rotation = this.Maps.Rotation = this.HidingPlanes.Rotation = this.Highlights.Rotation = this.AoInstance.Rotation = this.GoldenCubes.Rotation = Quaternion.Slerp(this.GoldenCubes.Rotation, this.OriginalRotation, 0.0875f);
        this.AntiCubes.Position = this.Maps.Position = this.HidingPlanes.Position = this.GoldenCubes.Position = this.AoInstance.Position;
        this.TransformArtifacts();
        this.HandleSelection();
        this.TestForTempleOfLove();
      }
    }

    private void TestForTempleOfLove()
    {
      if (this.LevelManager.Name != "TEMPLE_OF_LOVE" || this.GameState.SaveData.PiecesOfHeart < 3 || (this.GameState.SaveData.HasDoneHeartReboot || this.Face != MenuCubeFace.Artifacts) || (!this.GameState.MenuCubeIsZoomed || !this.LetterZoom && !this.NumberZoom) || this.letterCodeDone && this.numberCodeDone)
        return;
      CodeInput codeInput = CodeInput.None;
      if (this.InputManager.Jump == FezButtonState.Pressed)
        codeInput = CodeInput.Jump;
      else if (this.InputManager.RotateRight == FezButtonState.Pressed)
        codeInput = CodeInput.SpinRight;
      else if (this.InputManager.RotateLeft == FezButtonState.Pressed)
        codeInput = CodeInput.SpinLeft;
      else if (this.InputManager.Left == FezButtonState.Pressed)
        codeInput = CodeInput.Left;
      else if (this.InputManager.Right == FezButtonState.Pressed)
        codeInput = CodeInput.Right;
      else if (this.InputManager.Up == FezButtonState.Pressed)
        codeInput = CodeInput.Up;
      else if (this.InputManager.Down == FezButtonState.Pressed)
        codeInput = CodeInput.Down;
      if (codeInput == CodeInput.None)
        return;
      this.codeInputs.Add(codeInput);
      if (this.codeInputs.Count > 8)
        this.codeInputs.RemoveAt(0);
      if (!this.letterCodeDone && this.LetterZoom)
        this.letterCodeDone = PatternTester.Test((IList<CodeInput>) this.codeInputs, MenuCube.LetterCode);
      if (!this.numberCodeDone && this.NumberZoom)
        this.numberCodeDone = PatternTester.Test((IList<CodeInput>) this.codeInputs, MenuCube.NumberCode);
      if (!this.letterCodeDone || !this.numberCodeDone)
        return;
      this.GameState.SaveData.HasDoneHeartReboot = true;
      this.LevelService.ResolvePuzzleSoundOnly();
      this.zooming = true;
      this.zoomed = false;
      this.DoArtifactZoom(this.ZoomedArtifact);
      Waiters.Wait((Func<bool>) (() => !this.GameState.MenuCubeIsZoomed), (Action) (() =>
      {
        this.ScheduleExit = true;
        this.Enabled = false;
        this.Resolved = false;
        this.TargetRenderingManager.ScheduleHook(this.DrawOrder, this.OutRtHandle.Target);
      }));
    }

    private void UpdateHighlights(float elapsedSeconds)
    {
      if (this.GameState.MenuCubeIsZoomed || this.Face == MenuCubeFace.CubeShards || this.Face == MenuCubeFace.AntiCubes)
        return;
      MenuCubeFace menuCubeFace = this.Face;
      int num1 = (int) Math.Sqrt((double) MenuCubeFaceExtensions.GetCount(menuCubeFace));
      if (this.InputManager.Right == FezButtonState.Pressed && (double) this.HighlightPosition[this.Face].X + 1.0 < (double) num1)
        this.MoveAndRotate(menuCubeFace, Vector2.UnitX);
      if (this.InputManager.Left == FezButtonState.Pressed && (double) this.HighlightPosition[this.Face].X - 1.0 >= 0.0)
        this.MoveAndRotate(menuCubeFace, -Vector2.UnitX);
      if (this.InputManager.Up == FezButtonState.Pressed && (double) this.HighlightPosition[this.Face].Y - 1.0 >= 0.0)
        this.MoveAndRotate(menuCubeFace, -Vector2.UnitY);
      if (this.InputManager.Down == FezButtonState.Pressed && (double) this.HighlightPosition[this.Face].Y + 1.0 < (double) num1)
        this.MoveAndRotate(menuCubeFace, Vector2.UnitY);
      int num2 = (int) this.Face;
      for (int index = 0; index < 4; ++index)
        this.Highlights.Groups[num2 * 4 + index].Scale = (float) (Math.Sin((double) elapsedSeconds * 5.0) * 0.100000001490116 * (1.0 / (double) (index + 1)) + 1.0) * (Vector3.UnitY + FezMath.Abs(MenuCubeFaceExtensions.GetRight(this.Face))) + FezMath.Abs(MenuCubeFaceExtensions.GetForward(this.Face));
    }

    private void MoveAndRotate(MenuCubeFace cf, Vector2 diff)
    {
      Vector2 op = this.HighlightPosition[cf];
      this.HighlightPosition[cf] = FezMath.Round(this.HighlightPosition[cf] + diff);
      int sgn = Math.Sign(Vector2.Dot(diff, Vector2.One));
      Vector3 axis = (double) diff.X != 0.0 ? Vector3.Up : MenuCubeFaceExtensions.GetRight(cf);
      Vector3 scale = this.AoInstance.ArtObject.Size;
      SoundEffectExtensions.Emit(this.cursorSound);
      Waiters.Interpolate(0.15, (Action<float>) (s =>
      {
        for (int local_0 = 0; local_0 < 4; ++local_0)
        {
          Group local_1 = this.Highlights.Groups[(int) cf * 4 + local_0];
          Vector2 local_2 = this.HighlightPosition[cf] - diff;
          if (op != local_2)
            break;
          s = Easing.EaseOut((double) FezMath.Saturate(s), EasingType.Sine);
          local_1.Position = (float) (((double) local_2.X + (double) diff.X * (double) s) * (double) MenuCubeFaceExtensions.GetSpacing(cf) / 16.0) * MenuCubeFaceExtensions.GetRight(cf) + (float) (((double) local_2.Y + (double) diff.Y * (double) s) * (double) MenuCubeFaceExtensions.GetSpacing(cf) / 16.0) * -Vector3.UnitY + scale / 2f * (MenuCubeFaceExtensions.GetForward(cf) + Vector3.Up - MenuCubeFaceExtensions.GetRight(cf)) + MenuCubeFaceExtensions.GetForward(cf) * (float) ((double) MenuCubeFaceExtensions.GetSize(cf) / 2.0 / 16.0 * Math.Sin((double) s * 3.14159274101257) - 7.0 / 16.0) + (Vector3.Down + MenuCubeFaceExtensions.GetRight(cf)) * (float) MenuCubeFaceExtensions.GetOffset(cf) / 16f;
          local_1.Rotation = Quaternion.CreateFromAxisAngle(axis, s * 3.141593f * (float) sgn);
        }
      }), (Action) (() =>
      {
        for (int local_0 = 0; local_0 < 4; ++local_0)
          this.Highlights.Groups[(int) cf * 4 + local_0].Rotation = Quaternion.Identity;
      }));
    }

    private void TransformArtifacts()
    {
      if (this.GameState.MenuCubeIsZoomed)
        return;
      int num1 = 0;
      foreach (ArtObjectInstance artObjectInstance in this.ArtifactAOs)
      {
        int num2 = (int) Math.Sqrt((double) MenuCubeFaceExtensions.GetCount(MenuCubeFace.Artifacts));
        Vector2 vector2 = new Vector2((float) (num1 % num2), (float) (num1 / num2));
        Vector3 size = this.AoInstance.ArtObject.Size;
        Vector2 artifactOffset = ActorTypeExtensions.GetArtifactOffset(artObjectInstance.ArtObject.ActorType);
        Vector3 vec = (float) (((double) vector2.X * (double) MenuCubeFaceExtensions.GetSpacing(MenuCubeFace.Artifacts) - (double) artifactOffset.X) / 16.0) * MenuCubeFaceExtensions.GetRight(MenuCubeFace.Artifacts) + (float) (((double) vector2.Y * (double) MenuCubeFaceExtensions.GetSpacing(MenuCubeFace.Artifacts) - (double) artifactOffset.Y) / 16.0) * -Vector3.UnitY + size / 2f * (MenuCubeFaceExtensions.GetForward(MenuCubeFace.Artifacts) + Vector3.Up - MenuCubeFaceExtensions.GetRight(MenuCubeFace.Artifacts)) + -MenuCubeFaceExtensions.GetForward(MenuCubeFace.Artifacts) * (float) MenuCubeFaceExtensions.GetDepth(MenuCubeFace.Artifacts) / 16f * 1.25f + (Vector3.Down + MenuCubeFaceExtensions.GetRight(MenuCubeFace.Artifacts)) * (float) MenuCubeFaceExtensions.GetOffset(MenuCubeFace.Artifacts) / 16f;
        artObjectInstance.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f) * this.AoInstance.Rotation;
        artObjectInstance.Position = this.AoInstance.Position + Vector3.Transform(Vector3.Transform(vec, this.AoInstance.Rotation), Matrix.CreateScale(this.AoInstance.Scale));
        artObjectInstance.Scale = this.AoInstance.Scale;
        if (artObjectInstance.ArtObjectName != "TOME_BAO")
          ++num1;
      }
    }

    private void HandleSelection()
    {
      if (!this.zooming && this.Face == MenuCubeFace.CubeShards || this.Face == MenuCubeFace.AntiCubes)
        return;
      if (this.TomeZoom && this.InputManager.GrabThrow == FezButtonState.Pressed || this.TomeOpen && this.InputManager.CancelTalk == FezButtonState.Pressed)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MenuCube.\u003C\u003Ec__DisplayClass31 cDisplayClass31_1 = new MenuCube.\u003C\u003Ec__DisplayClass31();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass31_1.\u003C\u003E4__this = this;
        this.TomeOpen = !this.TomeOpen;
        if (this.TomeOpen)
        {
          this.CameraManager.OriginalDirection = this.OriginalDirection;
          this.GameState.DisallowRotation = true;
        }
        else
        {
          this.GameState.DisallowRotation = false;
          for (int index = this.TomePageIndex - 1; index >= 0; --index)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            MenuCube.\u003C\u003Ec__DisplayClass33 cDisplayClass33 = new MenuCube.\u003C\u003Ec__DisplayClass33();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass33.CS\u0024\u003C\u003E8__locals32 = cDisplayClass31_1;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass33.i1 = index;
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            Waiters.Interpolate(0.25 + (double) (8 - index) / 8.0 * 0.150000005960464, new Action<float>(cDisplayClass33.\u003CHandleSelection\u003Eb__2b), new Action(cDisplayClass33.\u003CHandleSelection\u003Eb__2c));
          }
          this.TomePageIndex = 0;
        }
        // ISSUE: reference to a compiler-generated field
        cDisplayClass31_1.thisWaiter = (IWaiter) null;
        // ISSUE: variable of a compiler-generated type
        MenuCube.\u003C\u003Ec__DisplayClass31 cDisplayClass31_2 = cDisplayClass31_1;
        MenuCube menuCube = this;
        double durationSeconds = this.TomeOpen ? 0.875 : 0.425000011920929;
        IWaiter waiter1;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        IWaiter waiter2 = waiter1 = Waiters.Interpolate(durationSeconds, new Action<float>(cDisplayClass31_1.\u003CHandleSelection\u003Eb__2d), new Action(cDisplayClass31_1.\u003CHandleSelection\u003Eb__2e));
        menuCube.tomeOpenWaiter = waiter1;
        IWaiter waiter3 = waiter2;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass31_2.thisWaiter = waiter3;
      }
      else
      {
        if (this.TomeOpen || this.zooming || (this.GameState.MenuCubeIsZoomed || this.InputManager.Jump != FezButtonState.Pressed) && (!this.GameState.MenuCubeIsZoomed || this.InputManager.Back != FezButtonState.Pressed && this.InputManager.CancelTalk != FezButtonState.Pressed))
          return;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MenuCube.\u003C\u003Ec__DisplayClass36 cDisplayClass36 = new MenuCube.\u003C\u003Ec__DisplayClass36();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass36.\u003C\u003E4__this = this;
        this.zoomed = !this.zoomed;
        if (this.zoomed)
        {
          this.GameService.CloseScroll((string) null);
          this.GameState.MenuCubeIsZoomed = true;
          this.zoomedFace = this.Face;
          this.OriginalDirection = this.CameraManager.OriginalDirection;
          this.AntiCubes.Blending = this.GoldenCubes.Blending = new BlendingMode?(BlendingMode.Alphablending);
        }
        else
        {
          this.AntiCubes.Blending = this.GoldenCubes.Blending = new BlendingMode?(BlendingMode.Opaque);
          this.CameraManager.OriginalDirection = this.OriginalDirection;
          this.GameState.ShowScroll(MenuCubeFaceExtensions.GetTitle(this.Face), 0.0f, true);
          this.TomeOpen = false;
        }
        // ISSUE: reference to a compiler-generated field
        cDisplayClass36.oid = (int) ((double) this.HighlightPosition[this.zoomedFace].X + (double) this.HighlightPosition[this.zoomedFace].Y * Math.Sqrt((double) MenuCubeFaceExtensions.GetCount(this.zoomedFace)));
        this.zooming = true;
        switch (this.zoomedFace)
        {
          case MenuCubeFace.Maps:
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            MenuCube.\u003C\u003Ec__DisplayClass38 cDisplayClass38 = new MenuCube.\u003C\u003Ec__DisplayClass38();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass38.CS\u0024\u003C\u003E8__locals37 = cDisplayClass36;
            // ISSUE: reference to a compiler-generated field
            if (this.GameState.SaveData.Maps.Count <= cDisplayClass36.oid)
            {
              this.zooming = false;
              this.zoomed = false;
              this.GameState.MenuCubeIsZoomed = false;
              return;
            }
            else
            {
              if (this.zoomed)
              {
                for (int index = 0; index < 6; ++index)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.Maps.Groups[cDisplayClass36.oid * 6 + index].Material = new Material();
                  // ISSUE: reference to a compiler-generated field
                  this.originalMapPositions[index] = this.Maps.Groups[cDisplayClass36.oid * 6 + index].Position;
                }
              }
              Vector2 vector2 = this.HighlightPosition[this.zoomedFace];
              // ISSUE: reference to a compiler-generated field
              cDisplayClass38.middleOffset = (float) ((double) vector2.X * (double) MenuCubeFaceExtensions.GetSpacing(this.zoomedFace) / 16.0) * MenuCubeFaceExtensions.GetRight(this.zoomedFace) + (float) ((double) vector2.Y * (double) MenuCubeFaceExtensions.GetSpacing(this.zoomedFace) / 16.0) * Vector3.Down + (Vector3.Down + MenuCubeFaceExtensions.GetRight(this.zoomedFace)) * (float) MenuCubeFaceExtensions.GetOffset(this.zoomedFace) / 16f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              cDisplayClass38.middleOffset = this.AoInstance.ArtObject.Size / 2f * (MenuCubeFaceExtensions.GetRight(this.zoomedFace) + Vector3.Down) - cDisplayClass38.middleOffset;
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              Waiters.Interpolate(0.25, new Action<float>(cDisplayClass38.\u003CHandleSelection\u003Eb__2f), new Action(cDisplayClass36.\u003CHandleSelection\u003Eb__30));
              break;
            }
          case MenuCubeFace.Artifacts:
            int count = this.ArtifactAOs.Count;
            if (this.GameState.SaveData.Artifacts.Contains(ActorType.Tome))
              --count;
            // ISSUE: reference to a compiler-generated field
            if (count <= cDisplayClass36.oid)
            {
              this.zooming = false;
              this.zoomed = false;
              this.GameState.MenuCubeIsZoomed = false;
              return;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              for (int index = 0; index <= cDisplayClass36.oid; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                if (index != cDisplayClass36.oid && this.ArtifactAOs[index].ArtObjectName == "TOME_BAO")
                {
                  // ISSUE: reference to a compiler-generated field
                  ++cDisplayClass36.oid;
                }
              }
              // ISSUE: reference to a compiler-generated field
              this.DoArtifactZoom(this.ArtifactAOs[cDisplayClass36.oid]);
              // ISSUE: reference to a compiler-generated field
              if (this.ArtifactAOs[cDisplayClass36.oid].ArtObjectName == "TOME_BAO")
              {
                // ISSUE: reference to a compiler-generated field
                this.DoArtifactZoom(this.ArtifactAOs[cDisplayClass36.oid + 1]);
                break;
              }
              else
                break;
            }
        }
        if (this.zoomed)
          SoundEffectExtensions.Emit(this.zoomInSound);
        else
          SoundEffectExtensions.Emit(this.zoomOutSound);
      }
    }

    private void DoTomeOpen(ArtObjectInstance ao, float s)
    {
      Vector3 vector3_1 = Vector3.Transform(MenuCubeFaceExtensions.GetForward(this.zoomedFace), this.AoInstance.Rotation);
      Vector3 vector3_2 = Vector3.Transform(MenuCubeFaceExtensions.GetRight(this.zoomedFace), this.AoInstance.Rotation);
      Vector3 position = -vector3_1 * 13f / 16f + vector3_2 * 2f / 16f;
      Vector3 scale;
      Quaternion rotation;
      Vector3 translation;
      (Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f) * this.AoInstance.Rotation) * Matrix.CreateTranslation(position) * Matrix.CreateFromAxisAngle(Vector3.UnitY, -2.356194f * s) * Matrix.CreateTranslation(this.AoInstance.Position + vector3_1 * 12f - position)).Decompose(out scale, out rotation, out translation);
      ao.Position = translation;
      ao.Rotation = rotation;
      this.TomePages.Position = translation;
    }

    private void DoArtifactZoom(ArtObjectInstance ao)
    {
      if (this.zoomed)
      {
        ao.Material = new Material();
        this.originalObjectPosition = ao.Position;
        MenuCube menuCube1 = this;
        int num1 = menuCube1.TomeZoom | ao.ArtObjectName == "TOME_BAO" ? 1 : 0;
        menuCube1.TomeZoom = num1 != 0;
        MenuCube menuCube2 = this;
        int num2 = menuCube2.NumberZoom | ao.ArtObjectName == "NUMBER_CUBEAO" ? 1 : 0;
        menuCube2.NumberZoom = num2 != 0;
        MenuCube menuCube3 = this;
        int num3 = menuCube3.LetterZoom | ao.ArtObjectName == "LETTER_CUBEAO" ? 1 : 0;
        menuCube3.LetterZoom = num3 != 0;
        this.ZoomedArtifact = ao;
      }
      else
      {
        this.NumberZoom = this.LetterZoom = this.TomeZoom = false;
        this.codeInputs.Clear();
        this.ZoomedArtifact = (ArtObjectInstance) null;
      }
      Waiters.Interpolate(0.25, (Action<float>) (s =>
      {
        s = Easing.EaseOut((double) s, EasingType.Quadratic);
        if (!this.zoomed)
          s = 1f - s;
        Vector2 local_0 = ActorTypeExtensions.GetArtifactOffset(ao.ArtObject.ActorType);
        this.AoInstance.Material.Opacity = FezMath.Saturate((float) (1.0 - (double) s * 1.5));
        this.AoInstance.MarkDirty();
        ao.Position = Vector3.Lerp(this.originalObjectPosition, this.AoInstance.Position + Vector3.Transform(MenuCubeFaceExtensions.GetForward(this.zoomedFace), this.AoInstance.Rotation) * 12f, s);
        this.CameraManager.Center = Vector3.Lerp(this.AoInstance.Position, ao.Position - Vector3.Transform(FezMath.XZMask * local_0.X / 16f, this.AoInstance.Rotation) - Vector3.UnitY * local_0.Y / 16f, s);
        this.CameraManager.Radius = MathHelper.Lerp(17.82f, 4.192941f, Easing.EaseIn((double) s, EasingType.Quadratic));
      }), (Action) (() =>
      {
        if (!this.zoomed)
        {
          ao.Material = this.AoInstance.Material;
          this.GameState.MenuCubeIsZoomed = false;
        }
        this.zooming = false;
      }));
    }

    public override void Draw(GameTime gameTime)
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue);
      if (this.TargetRenderingManager.IsHooked(this.InRtHandle.Target) && !this.Resolved)
      {
        this.TargetRenderingManager.Resolve(this.InRtHandle.Target, false);
        this.Resolved = true;
        this.StartInTransition();
      }
      if (this.ScheduleExit && this.Resolved)
      {
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Alphablending);
        this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        this.TargetRenderingManager.DrawFullscreen((Texture) this.OutRtHandle.Target, new Color(1f, 1f, 1f, this.AoInstance.Material.Opacity));
      }
      else
      {
        this.AoVisibility.Clear();
        foreach (ArtObjectInstance artObjectInstance in this.LevelMaterializer.LevelArtObjects)
        {
          this.AoVisibility.Add(artObjectInstance.Visible);
          artObjectInstance.Visible = false;
          artObjectInstance.ArtObject.Group.Enabled = false;
        }
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Opaque);
        RenderTargetHandle handle1 = (RenderTargetHandle) null;
        if (this.GameState.StereoMode)
          handle1 = this.TargetRenderingManager.TakeTarget();
        RenderTarget2D renderTarget = this.GraphicsDevice.GetRenderTargets().Length == 0 ? (this.GameState.StereoMode ? handle1.Target : (RenderTarget2D) null) : this.GraphicsDevice.GetRenderTargets()[0].RenderTarget as RenderTarget2D;
        this.BlurEffect.Pass = BlurPass.Horizontal;
        RenderTargetHandle handle2 = this.TargetRenderingManager.TakeTarget();
        this.GraphicsDevice.SetRenderTarget(handle2.Target);
        this.GraphicsDevice.Clear(Color.Black);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        this.TargetRenderingManager.DrawFullscreen((BaseEffect) this.BlurEffect, (Texture) this.InRtHandle.Target);
        this.BlurEffect.Pass = BlurPass.Vertical;
        this.GraphicsDevice.SetRenderTarget(renderTarget);
        this.GraphicsDevice.Clear(Color.Black);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        this.TargetRenderingManager.DrawFullscreen((BaseEffect) this.BlurEffect, (Texture) handle2.Target);
        this.TargetRenderingManager.ReturnTarget(handle2);
        if (this.GameState.StereoMode)
        {
          this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
          this.GraphicsDevice.Clear(Color.Black);
        }
        this.GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1f, 0);
        if (this.GameState.StereoMode)
          GameLevelHost.Instance.DoStereo(this.CameraManager.Center, this.AoInstance.ArtObject.Size * 1.5f, new Action(this.DrawMenuCube), (Texture) handle1.Target);
        else
          this.DrawMenuCube();
        if (this.GameState.StereoMode)
          this.TargetRenderingManager.ReturnTarget(handle1);
        int num = 0;
        foreach (ArtObjectInstance artObjectInstance in this.LevelMaterializer.LevelArtObjects)
        {
          artObjectInstance.Visible = this.AoVisibility[num++];
          if (artObjectInstance.Visible)
            artObjectInstance.ArtObject.Group.Enabled = true;
        }
        if (this.TargetRenderingManager.IsHooked(this.OutRtHandle.Target) && !this.Resolved)
        {
          this.TargetRenderingManager.Resolve(this.OutRtHandle.Target, false);
          this.GraphicsDevice.Clear(Color.Black);
          SettingsManager.SetupViewport(this.GraphicsDevice, false);
          this.TargetRenderingManager.DrawFullscreen((Texture) this.OutRtHandle.Target);
          this.Resolved = true;
          this.StartOutTransition();
        }
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
      }
    }

    private void DrawMenuCube()
    {
      if (this.GameState.StereoMode)
      {
        this.LevelManager.ActualDiffuse = new Color(new Vector3(0.8f));
        this.LevelManager.ActualAmbient = new Color(new Vector3(0.2f));
      }
      RasterizerCombiner rasterCombiner = GraphicsDeviceExtensions.GetRasterCombiner(this.GraphicsDevice);
      bool flag = (double) this.LevelManager.BaseDiffuse != 0.0;
      foreach (ArtObjectInstance artObjectInstance in this.ArtifactAOs)
      {
        artObjectInstance.Visible = true;
        artObjectInstance.ArtObject.Group.Enabled = true;
        if (flag)
          artObjectInstance.ForceShading = true;
      }
      this.AoInstance.Visible = true;
      this.AoInstance.ArtObject.Group.Enabled = true;
      if (flag)
        this.AoInstance.ForceShading = true;
      this.LevelMaterializer.ArtObjectsMesh.Draw();
      if (flag)
        this.AoInstance.ForceShading = false;
      foreach (ArtObjectInstance artObjectInstance in this.ArtifactAOs)
      {
        if (flag)
          artObjectInstance.ForceShading = false;
      }
      rasterCombiner.DepthBias = FezMath.IsOrthographic(this.CameraManager.Viewpoint) ? -1E-06f : (float) (-9.99999974737875E-05 / ((double) this.CameraManager.FarPlane - (double) this.CameraManager.NearPlane));
      this.HidingPlanes.Draw();
      rasterCombiner.DepthBias = 0.0f;
      this.GoldenCubes.Draw();
      this.Highlights.Draw();
      this.Maps.Draw();
      rasterCombiner.DepthBias = FezMath.IsOrthographic(this.CameraManager.Viewpoint) ? -1E-06f : (float) (-0.00999999977648258 / ((double) this.CameraManager.FarPlane - (double) this.CameraManager.NearPlane));
      if (this.TomeOpen || this.tomeOpenWaiter != null && this.TomeZoom)
      {
        this.TomePages.Rotation = this.TomeBackAo.Rotation;
        this.TomePages.Position = this.TomeBackAo.Position + Vector3.Transform(MenuCubeFaceExtensions.GetForward(this.zoomedFace), this.AoInstance.Rotation) * 13f / 16f;
        this.TomePages.Draw();
      }
      rasterCombiner.DepthBias = FezMath.IsOrthographic(this.CameraManager.Viewpoint) ? -1E-06f : (float) (-9.99999974737875E-05 / ((double) this.CameraManager.FarPlane - (double) this.CameraManager.NearPlane));
      this.AntiCubes.Draw();
      rasterCombiner.DepthBias = 0.0f;
    }
  }
}
