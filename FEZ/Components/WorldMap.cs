// Type: FezGame.Components.WorldMap
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
using FezEngine.Structure.Input;
using FezEngine.Tools;
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
  public class WorldMap : DrawableGameComponent
  {
    private static readonly float[] ZoomCycle = new float[5]
    {
      80f,
      40f,
      20f,
      10f,
      5f
    };
    private static readonly string[] DotDialogue = new string[6]
    {
      "DOT_MAP_A",
      "DOT_MAP_B",
      "DOT_MAP_C",
      "DOT_MAP_D",
      "DOT_MAP_E",
      "DOT_MAP_F"
    };
    private static readonly Vector3 GoldColor = new Color((int) byte.MaxValue, 190, 36).ToVector3();
    private float SinceMouseMoved = 3f;
    private int ZoomLevel = WorldMap.ZoomCycle.Length / 2;
    private readonly List<WorldMap.QualifiedNode> closestNodes = new List<WorldMap.QualifiedNode>();
    private readonly List<MapNode> nextToCover = new List<MapNode>();
    private readonly List<MapNode> toCover = new List<MapNode>();
    private readonly HashSet<MapNode> hasCovered = new HashSet<MapNode>();
    private const float LinkThickness = 0.05375f;
    private bool ShowAll;
    private bool AllVisited;
    private Vector3 OriginalCenter;
    private Quaternion OriginalRotation;
    private float OriginalPixPerTrix;
    private Vector3 OriginalDirection;
    private Viewpoint OriginalViewpoint;
    private ProjectedNodeEffect NodeEffect;
    private MapTree MapTree;
    private Mesh NodesMesh;
    private Mesh LinksMesh;
    private Mesh ButtonsMesh;
    private Mesh WavesMesh;
    private Mesh IconsMesh;
    private Mesh LegendMesh;
    private ShaderInstancedIndexedPrimitives<VertexPositionColorTextureInstance, Matrix> GoldenHighlightsGeometry;
    private ShaderInstancedIndexedPrimitives<VertexPositionColorTextureInstance, Matrix> WhiteHighlightsGeometry;
    private ShaderInstancedIndexedPrimitives<VertexPositionColorTextureInstance, Matrix> LinksGeometry;
    private ShaderInstancedIndexedPrimitives<VertexPositionTextureInstance, Matrix> IconsGeometry;
    private List<float> IconsTrailingOffset;
    private Matrix[] IconsOriginalInstances;
    private MapNode CurrentNode;
    private MapNode LastFocusedNode;
    private MapNode FocusNode;
    private GlyphTextRenderer GTR;
    private TimeSpan SinceStarted;
    private RenderTargetHandle FadeInRtHandle;
    private RenderTargetHandle FadeOutRtHandle;
    private SpriteBatch SpriteBatch;
    private static StarField Starfield;
    private SoundEffect sTextNext;
    private SoundEffect sRotateLeft;
    private SoundEffect sRotateRight;
    private SoundEffect sBackground;
    private SoundEffect sZoomIn;
    private SoundEffect sZoomOut;
    private SoundEffect sEnter;
    private SoundEffect sExit;
    private SoundEffect sMagnet;
    private SoundEffect sBeacon;
    private SoundEmitter eBackground;
    private Texture2D ShineTex;
    private Texture2D GrabbedCursor;
    private Texture2D CanClickCursor;
    private Texture2D ClickedCursor;
    private Texture2D PointerCursor;
    private bool CursorSelectable;
    private bool Resolved;
    private int DotDialogueIndex;
    private bool FinishedInTransition;
    private bool ScheduleExit;
    private string CurrentLevelName;
    private bool wasLowPass;
    public static WorldMap Instance;
    private bool chosenByMouseClick;
    private bool blockViewPicking;

    [ServiceDependency]
    public IMouseStateManager MouseState { protected get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderingManager { get; set; }

    [ServiceDependency]
    public IInputManager InputManager { get; set; }

    [ServiceDependency]
    public IGameService GameService { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IFontManager FontManager { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IDotManager DotManager { get; set; }

    [ServiceDependency]
    public ISpeechBubbleManager SpeechBubble { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    static WorldMap()
    {
    }

    public WorldMap(Game game)
      : base(game)
    {
      this.UpdateOrder = -10;
      this.DrawOrder = 1000;
      WorldMap.Instance = this;
    }

    public static void PreInitialize()
    {
      ServiceHelper.AddComponent((IGameComponent) (WorldMap.Starfield = new StarField(ServiceHelper.Game)
      {
        FollowCamera = true
      }));
    }

    public override void Initialize()
    {
      base.Initialize();
      string str = this.LevelManager.Name.Replace('\\', '/');
      this.CurrentLevelName = str.Substring(str.LastIndexOf('/') + 1);
      if (this.CurrentLevelName == "CABIN_INTERIOR_A")
        this.CurrentLevelName = "CABIN_INTERIOR_B";
      this.MapTree = this.CMProvider.Global.Load<MapTree>("MapTree").Clone();
      if (WorldMap.Starfield == null)
        ServiceHelper.AddComponent((IGameComponent) (WorldMap.Starfield = new StarField(this.Game)
        {
          FollowCamera = true
        }));
      this.LastFocusedNode = this.FocusNode = this.CurrentNode = this.MapTree.Root;
      this.NodeEffect = new ProjectedNodeEffect();
      this.NodesMesh = new Mesh()
      {
        Effect = (BaseEffect) this.NodeEffect,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerStates.PointMipClamp
      };
      this.LinksMesh = new Mesh()
      {
        Effect = (BaseEffect) new InstancedMapEffect(),
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        Culling = CullMode.None
      };
      this.LinksGeometry = this.CreateLinksGroup(Color.White, false, CullMode.CullCounterClockwiseFace);
      List<Matrix> instances = new List<Matrix>();
      this.BuildNodes(this.MapTree.Root, (MapNode.Connection) null, (MapNode) null, Vector3.Zero, instances);
      this.GoldenHighlightsGeometry = this.CreateLinksGroup(new Color(WorldMap.GoldColor), true, CullMode.CullClockwiseFace);
      List<Matrix> list1 = new List<Matrix>();
      this.WhiteHighlightsGeometry = this.CreateLinksGroup(Color.White, false, CullMode.CullClockwiseFace);
      List<Matrix> list2 = new List<Matrix>();
      foreach (Group group in this.NodesMesh.Groups)
      {
        NodeGroupData nodeGroupData = group.CustomData as NodeGroupData;
        MapNode mapNode = nodeGroupData.Node;
        Vector3 vector3_1 = new Vector3(LevelNodeTypeExtensions.GetSizeFactor(mapNode.NodeType) + 0.125f);
        Vector3 position = nodeGroupData.Node.Group.Position;
        Vector3 vector3_2 = Vector3.One;
        LevelSaveData levelSaveData;
        if (this.GameState.SaveData.World.TryGetValue(this.GameState.IsTrialMode ? "trial/" + mapNode.LevelName : mapNode.LevelName, out levelSaveData) && levelSaveData.FilledConditions.Fullfills(mapNode.Conditions))
        {
          nodeGroupData.Complete = true;
          vector3_2 = WorldMap.GoldColor;
          nodeGroupData.HighlightInstance = list1.Count;
          list1.Add(new Matrix(position.X, position.Y, position.Z, 0.0f, vector3_2.X, vector3_2.Y, vector3_2.Z, 1f, vector3_1.X, vector3_1.Y, vector3_1.Z, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
        }
        else
        {
          nodeGroupData.HighlightInstance = list2.Count;
          list2.Add(new Matrix(position.X, position.Y, position.Z, 0.0f, vector3_2.X, vector3_2.Y, vector3_2.Z, 1f, vector3_1.X, vector3_1.Y, vector3_1.Z, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
        }
      }
      this.LinksGeometry.Instances = instances.ToArray();
      this.LinksGeometry.InstanceCount = instances.Count;
      this.LinksGeometry.UpdateBuffers();
      this.GoldenHighlightsGeometry.Instances = list1.ToArray();
      this.GoldenHighlightsGeometry.InstanceCount = list1.Count;
      this.GoldenHighlightsGeometry.UpdateBuffers();
      this.WhiteHighlightsGeometry.Instances = list2.ToArray();
      this.WhiteHighlightsGeometry.InstanceCount = list2.Count;
      this.WhiteHighlightsGeometry.UpdateBuffers();
      this.CreateIcons();
      this.PlayerManager.CanControl = false;
      this.OriginalCenter = this.CameraManager.Center;
      this.OriginalPixPerTrix = this.CameraManager.PixelsPerTrixel;
      this.OriginalViewpoint = this.CameraManager.Viewpoint;
      this.OriginalRotation = Quaternion.Identity;
      this.OriginalDirection = this.CameraManager.Direction;
      this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.GameService.CloseScroll((string) null);
      float width = 22.5f * this.GraphicsDevice.Viewport.AspectRatio;
      float height = 22.5f;
      WorldMap worldMap1 = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      AnimatedPlaneEffect animatedPlaneEffect1 = new AnimatedPlaneEffect();
      animatedPlaneEffect1.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(width, height, 0.1f, 100f));
      animatedPlaneEffect1.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10f), Vector3.Zero, Vector3.Up));
      AnimatedPlaneEffect animatedPlaneEffect2 = animatedPlaneEffect1;
      mesh2.Effect = (BaseEffect) animatedPlaneEffect2;
      mesh1.AlwaysOnTop = true;
      mesh1.DepthWrites = false;
      mesh1.SamplerState = SamplerState.PointClamp;
      mesh1.Position = new Vector3((float) ((double) width / 2.0 * 0.75), (float) ((double) height / 2.0 * 0.850000023841858 - 1.0), 0.0f);
      Mesh mesh3 = mesh1;
      worldMap1.ButtonsMesh = mesh3;
      Group group1 = this.ButtonsMesh.AddFace(new Vector3(1f, 7.375f, 1f), new Vector3(0.0f, -5.875f, 0.0f), FaceOrientation.Front, false);
      group1.Material = new Material()
      {
        Diffuse = new Vector3(1.0 / 16.0)
      };
      group1.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/fullwhite");
      WorldMap worldMap2 = this;
      Mesh mesh4 = new Mesh();
      Mesh mesh5 = mesh4;
      DefaultEffect.Textured textured1 = new DefaultEffect.Textured();
      textured1.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(width, height, 0.1f, 100f));
      textured1.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10f), Vector3.Zero, Vector3.Up));
      DefaultEffect.Textured textured2 = textured1;
      mesh5.Effect = (BaseEffect) textured2;
      mesh4.AlwaysOnTop = true;
      mesh4.DepthWrites = false;
      mesh4.SamplerState = SamplerState.PointClamp;
      mesh4.Position = new Vector3((float) (-(double) width / 2.0 * 0.899999976158142), (float) (-(double) height / 2.0 * 0.899999976158142), 0.0f);
      mesh4.Blending = new BlendingMode?(BlendingMode.Alphablending);
      Mesh mesh6 = mesh4;
      worldMap2.LegendMesh = mesh6;
      Group group2 = this.LegendMesh.AddFace(new Vector3(1f, 5.75f, 1f), new Vector3(0.0f, 0.0f, 0.0f), FaceOrientation.Front, false);
      group2.Material = new Material()
      {
        Diffuse = new Vector3(1.0 / 16.0)
      };
      group2.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/FullWhite");
      this.LegendMesh.AddFace(new Vector3(1f, 8f, 1f), new Vector3(0.25f, -2.5f, 0.0f), FaceOrientation.Front, false).Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/map_controls/icons_legend");
      this.WavesMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerState.LinearClamp,
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/map_controls/cube_outline"))
      };
      for (int index = 0; index < 4; ++index)
        this.WavesMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, true).Material = new Material();
      this.GTR = new GlyphTextRenderer(this.Game);
      this.ZoomLevel = 2;
      if (!this.GameState.SaveData.HasHadMapHelp)
      {
        this.DotManager.ForceDrawOrder(this.DrawOrder + 1);
        this.SpeechBubble.ForceDrawOrder(this.DrawOrder + 1);
        this.DotManager.Behaviour = DotHost.BehaviourType.ClampToTarget;
        this.DotManager.Hidden = false;
      }
      else
        this.DotManager.Hidden = true;
      this.sTextNext = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/TextNext");
      this.sRotateLeft = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/RotateLeft");
      this.sRotateRight = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/RotateRight");
      this.sEnter = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/EnterMenucubeOrMap");
      this.sExit = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/ExitMenucubeOrMap");
      this.sZoomIn = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/ZoomIn");
      this.sZoomOut = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/ZoomOut");
      this.sMagnet = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/WorldMapMagnet");
      this.sBeacon = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/MapBeacon");
      this.sBackground = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/MapBackground");
      this.eBackground = SoundEffectExtensions.Emit(this.sBackground, true);
      SoundEffectExtensions.Emit(this.sEnter);
      this.CameraManager.OriginalDirection = this.CameraManager.Direction;
      this.ShineTex = this.CMProvider.Global.Load<Texture2D>("Other Textures/map_screens/shine_rays");
      this.PointerCursor = this.CMProvider.Global.Load<Texture2D>("Other Textures/cursor/CURSOR_POINTER");
      this.CanClickCursor = this.CMProvider.Global.Load<Texture2D>("Other Textures/cursor/CURSOR_CLICKER_A");
      this.ClickedCursor = this.CMProvider.Global.Load<Texture2D>("Other Textures/cursor/CURSOR_CLICKER_B");
      this.GrabbedCursor = this.CMProvider.Global.Load<Texture2D>("Other Textures/cursor/CURSOR_GRABBER");
      this.wasLowPass = this.SoundManager.IsLowPass;
      if (!this.wasLowPass)
        this.SoundManager.FadeFrequencies(true);
      this.FadeOutRtHandle = this.TargetRenderingManager.TakeTarget();
      this.FadeInRtHandle = this.TargetRenderingManager.TakeTarget();
      this.TargetRenderingManager.ScheduleHook(this.DrawOrder, this.FadeInRtHandle.Target);
      WorldMap.Starfield.Opacity = this.LinksMesh.Material.Opacity = this.NodesMesh.Material.Opacity = this.LegendMesh.Material.Opacity = 0.0f;
    }

    private ShaderInstancedIndexedPrimitives<VertexPositionColorTextureInstance, Matrix> CreateLinksGroup(Color color, bool isComplete, CullMode cullMode)
    {
      Group group = this.LinksMesh.AddGroup();
      ShaderInstancedIndexedPrimitives<VertexPositionColorTextureInstance, Matrix> indexedPrimitives = new ShaderInstancedIndexedPrimitives<VertexPositionColorTextureInstance, Matrix>(PrimitiveType.TriangleList, 58);
      group.Geometry = (IIndexedPrimitiveCollection) indexedPrimitives;
      indexedPrimitives.Vertices = new VertexPositionColorTextureInstance[8]
      {
        new VertexPositionColorTextureInstance(new Vector3(-1f, -1f, -1f) / 2f, color, Vector2.Zero),
        new VertexPositionColorTextureInstance(new Vector3(1f, -1f, -1f) / 2f, color, Vector2.Zero),
        new VertexPositionColorTextureInstance(new Vector3(1f, 1f, -1f) / 2f, color, Vector2.Zero),
        new VertexPositionColorTextureInstance(new Vector3(-1f, 1f, -1f) / 2f, color, Vector2.Zero),
        new VertexPositionColorTextureInstance(new Vector3(-1f, -1f, 1f) / 2f, color, Vector2.Zero),
        new VertexPositionColorTextureInstance(new Vector3(1f, -1f, 1f) / 2f, color, Vector2.Zero),
        new VertexPositionColorTextureInstance(new Vector3(1f, 1f, 1f) / 2f, color, Vector2.Zero),
        new VertexPositionColorTextureInstance(new Vector3(-1f, 1f, 1f) / 2f, color, Vector2.Zero)
      };
      indexedPrimitives.Indices = new int[36]
      {
        0,
        1,
        2,
        0,
        2,
        3,
        1,
        5,
        6,
        1,
        6,
        2,
        0,
        7,
        4,
        0,
        3,
        7,
        3,
        2,
        6,
        3,
        6,
        7,
        4,
        6,
        5,
        4,
        7,
        6,
        0,
        5,
        1,
        0,
        4,
        5
      };
      group.CustomData = (object) (bool) (isComplete ? 1 : 0);
      group.CullMode = new CullMode?(cullMode);
      return indexedPrimitives;
    }

    private void CreateIcons()
    {
      this.IconsMesh = new Mesh()
      {
        Effect = (BaseEffect) new InstancedMapEffect()
        {
          Billboard = true
        },
        AlwaysOnTop = true,
        DepthWrites = false,
        SamplerState = SamplerState.PointClamp,
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/map_controls/icons"))
      };
      Group group1 = this.IconsMesh.AddGroup();
      this.IconsGeometry = new ShaderInstancedIndexedPrimitives<VertexPositionTextureInstance, Matrix>(PrimitiveType.TriangleList, 58);
      group1.Geometry = (IIndexedPrimitiveCollection) this.IconsGeometry;
      this.IconsGeometry.Vertices = new VertexPositionTextureInstance[4]
      {
        new VertexPositionTextureInstance(new Vector3(-0.5f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f)),
        new VertexPositionTextureInstance(new Vector3(0.5f, 0.0f, 0.0f), new Vector2(0.625f, 0.0f)),
        new VertexPositionTextureInstance(new Vector3(0.5f, -1f, 0.0f), new Vector2(0.625f, 1f)),
        new VertexPositionTextureInstance(new Vector3(-0.5f, -1f, 0.0f), new Vector2(0.0f, 1f))
      };
      this.IconsGeometry.Indices = new int[6]
      {
        0,
        1,
        3,
        3,
        1,
        2
      };
      List<Matrix> list = new List<Matrix>();
      this.IconsTrailingOffset = new List<float>();
      foreach (Group group2 in this.NodesMesh.Groups)
      {
        NodeGroupData nodeGroupData = (NodeGroupData) group2.CustomData;
        MapNode mapNode = nodeGroupData.Node;
        LevelSaveData levelSaveData;
        if (!this.GameState.SaveData.World.TryGetValue(this.GameState.IsTrialMode ? "trial/" + mapNode.LevelName : mapNode.LevelName, out levelSaveData))
        {
          if (this.ShowAll && this.AllVisited)
            levelSaveData = new LevelSaveData();
          else
            continue;
        }
        float num = 0.0f;
        Vector3 vector3 = group2.Position + LevelNodeTypeExtensions.GetSizeFactor(mapNode.NodeType) / 2f * new Vector3(1f, 1f, -1f) + 0.2f * new Vector3(1f, 0.0f, -1f);
        if (mapNode.HasWarpGate)
        {
          nodeGroupData.IconInstances.Add(list.Count);
          list.Add(new Matrix(vector3.X, vector3.Y, vector3.Z, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.25f, 0.225f, 0.25f, 0.0f, 0.0f, 0.0f, 1f, 9.0 / 64.0));
          this.IconsTrailingOffset.Add(num);
          num += 0.9f;
        }
        if (mapNode.HasLesserGate)
        {
          nodeGroupData.IconInstances.Add(list.Count);
          list.Add(new Matrix(vector3.X, vector3.Y, vector3.Z, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.25f, 0.175f, 0.25f, 0.0f, 0.0f, 9.0 / 64.0, 1f, 7.0 / 64.0));
          this.IconsTrailingOffset.Add(num);
          num += 0.7f;
        }
        if (nodeGroupData.Node.Conditions.ChestCount > levelSaveData.FilledConditions.ChestCount)
        {
          nodeGroupData.IconInstances.Add(list.Count);
          list.Add(new Matrix(vector3.X, vector3.Y, vector3.Z, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.25f, 0.15f, 0.25f, 0.0f, 0.0f, 0.25f, 1f, 3.0 / 32.0));
          this.IconsTrailingOffset.Add(num);
          num += 0.6f;
        }
        if (nodeGroupData.Node.Conditions.LockedDoorCount > levelSaveData.FilledConditions.LockedDoorCount)
        {
          nodeGroupData.IconInstances.Add(list.Count);
          list.Add(new Matrix(vector3.X, vector3.Y, vector3.Z, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.25f, 0.225f, 0.25f, 0.0f, 0.0f, 11.0 / 32.0, 1f, 9.0 / 64.0));
          this.IconsTrailingOffset.Add(num);
          num += 0.9f;
        }
        if (nodeGroupData.Node.Conditions.CubeShardCount > levelSaveData.FilledConditions.CubeShardCount)
        {
          nodeGroupData.IconInstances.Add(list.Count);
          list.Add(new Matrix(vector3.X, vector3.Y, vector3.Z, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.25f, 0.225f, 0.25f, 0.0f, 0.0f, 31.0 / 64.0, 1f, 9.0 / 64.0));
          this.IconsTrailingOffset.Add(num);
          num += 0.9f;
        }
        if (nodeGroupData.Node.Conditions.SplitUpCount > levelSaveData.FilledConditions.SplitUpCount)
        {
          nodeGroupData.IconInstances.Add(list.Count);
          list.Add(new Matrix(vector3.X, vector3.Y, vector3.Z, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.25f, 0.275f, 0.25f, 0.0f, 0.0f, 0.625f, 1f, 11.0 / 64.0));
          this.IconsTrailingOffset.Add(num);
          num += 1.1f;
        }
        if (nodeGroupData.Node.Conditions.SecretCount + nodeGroupData.Node.Conditions.ScriptIds.Count > levelSaveData.FilledConditions.SecretCount + levelSaveData.FilledConditions.ScriptIds.Count)
        {
          nodeGroupData.IconInstances.Add(list.Count);
          list.Add(new Matrix(vector3.X, vector3.Y, vector3.Z, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.25f, 0.275f, 0.25f, 0.0f, 0.0f, 51.0 / 64.0, 1f, 11.0 / 64.0));
          this.IconsTrailingOffset.Add(num);
        }
      }
      this.IconsGeometry.Instances = list.ToArray();
      this.IconsOriginalInstances = list.ToArray();
      this.IconsGeometry.InstanceCount = list.Count;
      this.IconsGeometry.UpdateBuffers();
    }

    protected override void Dispose(bool disposing)
    {
      this.GameState.InMap = false;
      this.GameState.SkipRendering = false;
      this.PlayerManager.CanControl = true;
      if (this.FadeOutRtHandle != null)
        this.TargetRenderingManager.ReturnTarget(this.FadeOutRtHandle);
      this.FadeOutRtHandle = (RenderTargetHandle) null;
      if (this.FadeInRtHandle != null)
        this.TargetRenderingManager.ReturnTarget(this.FadeInRtHandle);
      this.FadeInRtHandle = (RenderTargetHandle) null;
      if (this.eBackground != null && !this.eBackground.Dead)
      {
        this.eBackground.FadeOutAndDie(0.25f);
        this.eBackground = (SoundEmitter) null;
      }
      this.LinksMesh.Dispose();
      this.NodesMesh.Dispose();
      this.IconsMesh.Dispose();
      this.ButtonsMesh.Dispose();
      this.InputManager.StrictRotation = false;
      if (!this.wasLowPass)
        this.SoundManager.FadeFrequencies(false);
      WorldMap.Instance = (WorldMap) null;
      base.Dispose(disposing);
    }

    private void BuildNodes(MapNode node, MapNode.Connection parentConnection, MapNode parentNode, Vector3 offset, List<Matrix> instances)
    {
      Group group = (Group) null;
      bool flag1 = this.GameState.SaveData.World.ContainsKey(this.GameState.IsTrialMode ? "trial/" + node.LevelName : node.LevelName);
      if (parentNode != null && this.GameState.SaveData.World.ContainsKey(this.GameState.IsTrialMode ? "trial/" + parentNode.LevelName : parentNode.LevelName) || Enumerable.Any<MapNode.Connection>((IEnumerable<MapNode.Connection>) node.Connections, (Func<MapNode.Connection, bool>) (connection => this.GameState.SaveData.World.ContainsKey(this.GameState.IsTrialMode ? "trial/" + connection.Node.LevelName : connection.Node.LevelName))) || flag1 || (this.AllVisited || this.ShowAll))
      {
        group = this.NodesMesh.AddFlatShadedBox(new Vector3(LevelNodeTypeExtensions.GetSizeFactor(node.NodeType)), Vector3.Zero, Color.White, true);
        group.Position = offset;
        group.CustomData = (object) new NodeGroupData()
        {
          Node = node,
          LevelName = node.LevelName
        };
        group.Material = new Material();
        node.Group = group;
      }
      if (node.LevelName == this.CurrentLevelName)
        this.LastFocusedNode = this.FocusNode = this.CurrentNode = node;
      if ((flag1 || this.AllVisited || this.ShowAll) && (group != null && MemoryContentManager.AssetExists("Other Textures/map_screens/" + node.LevelName)))
        group.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/map_screens/" + node.LevelName);
      using (List<MapNode.Connection>.Enumerator enumerator = node.Connections.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          MapNode.Connection c = enumerator.Current;
          if (c.Node.NodeType == LevelNodeType.Lesser && Enumerable.Any<MapNode.Connection>((IEnumerable<MapNode.Connection>) node.Connections, (Func<MapNode.Connection, bool>) (x =>
          {
            if (x.Face == c.Face)
              return c.Node.NodeType != LevelNodeType.Lesser;
            else
              return false;
          })))
          {
            if (!Enumerable.Any<MapNode.Connection>((IEnumerable<MapNode.Connection>) node.Connections, (Func<MapNode.Connection, bool>) (x => x.Face == FaceOrientation.Top)))
              c.Face = FaceOrientation.Top;
            else if (!Enumerable.Any<MapNode.Connection>((IEnumerable<MapNode.Connection>) node.Connections, (Func<MapNode.Connection, bool>) (x => x.Face == FaceOrientation.Down)))
              c.Face = FaceOrientation.Down;
          }
        }
      }
      using (List<MapNode.Connection>.Enumerator enumerator = node.Connections.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          MapNode.Connection c = enumerator.Current;
          c.MultiBranchId = Enumerable.Max<MapNode.Connection>(Enumerable.Where<MapNode.Connection>((IEnumerable<MapNode.Connection>) node.Connections, (Func<MapNode.Connection, bool>) (x => x.Face == c.Face)), (Func<MapNode.Connection, int>) (x => x.MultiBranchId)) + 1;
          c.MultiBranchCount = Enumerable.Count<MapNode.Connection>((IEnumerable<MapNode.Connection>) node.Connections, (Func<MapNode.Connection, bool>) (x => x.Face == c.Face));
        }
      }
      float val1 = 0.0f;
      foreach (MapNode.Connection connection in (IEnumerable<MapNode.Connection>) Enumerable.OrderByDescending<MapNode.Connection, float>((IEnumerable<MapNode.Connection>) node.Connections, (Func<MapNode.Connection, float>) (x => LevelNodeTypeExtensions.GetSizeFactor(x.Node.NodeType))))
      {
        if (parentConnection != null && connection.Face == FezMath.GetOpposite(parentConnection.Face))
          connection.Face = FezMath.GetOpposite(connection.Face);
        bool flag2 = this.AllVisited || flag1 || this.GameState.SaveData.World.ContainsKey(this.GameState.IsTrialMode ? "trial/" + connection.Node.LevelName : connection.Node.LevelName);
        float num1 = (float) (3.0 + ((double) LevelNodeTypeExtensions.GetSizeFactor(node.NodeType) + (double) LevelNodeTypeExtensions.GetSizeFactor(connection.Node.NodeType)) / 2.0);
        if ((node.NodeType == LevelNodeType.Hub || connection.Node.NodeType == LevelNodeType.Hub) && (node.NodeType != LevelNodeType.Lesser && connection.Node.NodeType != LevelNodeType.Lesser))
          ++num1;
        if ((node.NodeType == LevelNodeType.Lesser || connection.Node.NodeType == LevelNodeType.Lesser) && connection.MultiBranchCount == 1)
          num1 -= FezMath.IsSide(connection.Face) ? 1f : 2f;
        float sizeFactor = num1 * (1.25f + connection.BranchOversize);
        float num2 = sizeFactor * 0.375f;
        if (connection.Node.NodeType == LevelNodeType.Node && node.NodeType == LevelNodeType.Node)
          num2 *= 1.5f;
        Vector3 faceVector = FezMath.AsVector(connection.Face);
        Vector3 vector3_1 = Vector3.Zero;
        if (connection.MultiBranchCount > 1)
          vector3_1 = ((float) (connection.MultiBranchId - 1) - (float) (connection.MultiBranchCount - 1) / 2f) * (FezMath.XZMask - FezMath.Abs(FezMath.AsVector(connection.Face))) * num2;
        this.BuildNodes(connection.Node, connection, node, offset + faceVector * sizeFactor + vector3_1, instances);
        if (flag2)
        {
          if (connection.LinkInstances == null)
            connection.LinkInstances = new List<int>();
          if (connection.MultiBranchCount > 1)
          {
            val1 = Math.Max(val1, sizeFactor / 2f);
            Vector3 vector3_2 = faceVector * val1 + new Vector3(0.05375f);
            Vector3 vector3_3 = faceVector * val1 / 2f + offset;
            connection.LinkInstances.Add(instances.Count);
            instances.Add(new Matrix(vector3_3.X, vector3_3.Y, vector3_3.Z, 0.0f, 1f, 1f, 1f, 1f, vector3_2.X, vector3_2.Y, vector3_2.Z, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
            vector3_2 = vector3_1 + new Vector3(0.05375f);
            Vector3 vector3_4 = vector3_1 / 2f + offset + faceVector * val1;
            connection.LinkInstances.Add(instances.Count);
            instances.Add(new Matrix(vector3_4.X, vector3_4.Y, vector3_4.Z, 0.0f, 1f, 1f, 1f, 1f, vector3_2.X, vector3_2.Y, vector3_2.Z, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
            float num3 = sizeFactor - val1;
            vector3_2 = faceVector * num3 + new Vector3(0.05375f);
            Vector3 vector3_5 = faceVector * num3 / 2f + offset + faceVector * val1 + vector3_1;
            connection.LinkInstances.Add(instances.Count);
            instances.Add(new Matrix(vector3_5.X, vector3_5.Y, vector3_5.Z, 0.0f, 1f, 1f, 1f, 1f, vector3_2.X, vector3_2.Y, vector3_2.Z, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
          }
          else
          {
            Vector3 vector3_2 = faceVector * sizeFactor + new Vector3(0.05375f);
            Vector3 vector3_3 = faceVector * sizeFactor / 2f + offset;
            connection.LinkInstances.Add(instances.Count);
            instances.Add(new Matrix(vector3_3.X, vector3_3.Y, vector3_3.Z, 0.0f, 1f, 1f, 1f, 1f, vector3_2.X, vector3_2.Y, vector3_2.Z, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
          }
          WorldMap.DoSpecial(connection, offset, faceVector, sizeFactor, instances);
        }
        connection.Node.Connections.Add(new MapNode.Connection()
        {
          Node = node,
          Face = FezMath.GetOpposite(connection.Face),
          BranchOversize = connection.BranchOversize,
          LinkInstances = connection.LinkInstances
        });
      }
    }

    private static void DoSpecial(MapNode.Connection c, Vector3 offset, Vector3 faceVector, float sizeFactor, List<Matrix> instances)
    {
      if (c.Node.LevelName == "LIGHTHOUSE_SPIN")
      {
        Vector3 backward = Vector3.Backward;
        float num = 3.425f;
        Vector3 vector3_1 = backward * num + new Vector3(0.05375f);
        Vector3 vector3_2 = backward * num / 2f + offset + faceVector * sizeFactor;
        c.LinkInstances.Add(instances.Count);
        instances.Add(new Matrix(vector3_2.X, vector3_2.Y, vector3_2.Z, 0.0f, 1f, 1f, 1f, 1f, vector3_1.X, vector3_1.Y, vector3_1.Z, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
      }
      if (!(c.Node.LevelName == "LIGHTHOUSE_HOUSE_A"))
        return;
      Vector3 right = Vector3.Right;
      float num1 = 5f;
      Vector3 vector3_3 = right * num1 + new Vector3(0.05375f);
      Vector3 vector3_4 = right * num1 / 2f + offset + faceVector * sizeFactor;
      c.LinkInstances.Add(instances.Count);
      instances.Add(new Matrix(vector3_4.X, vector3_4.Y, vector3_4.Z, 0.0f, 1f, 1f, 1f, 1f, vector3_3.X, vector3_3.Y, vector3_3.Z, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.Loading || !this.Resolved && !this.ScheduleExit)
        return;
      this.SinceStarted += gameTime.ElapsedGameTime;
      this.SinceMouseMoved += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (this.MouseState.Movement.X != 0 || this.MouseState.Movement.Y != 0)
        this.SinceMouseMoved = 0.0f;
      Vector3 right = this.CameraManager.InverseView.Right;
      Vector3 up = this.CameraManager.InverseView.Up;
      Vector3 forward = this.CameraManager.InverseView.Forward;
      if (!this.GameState.InMap)
        ServiceHelper.RemoveComponent<WorldMap>(this);
      else if (this.InputManager.Back == FezButtonState.Pressed || this.InputManager.CancelTalk == FezButtonState.Pressed && this.SpeechBubble.Hidden)
      {
        this.Exit();
      }
      else
      {
        float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
        this.CameraManager.Radius = WorldMap.ZoomCycle[this.ZoomLevel] * viewScale;
        float radius = this.CameraManager.Radius;
        bool flag = this.MouseState.RightButton.State == MouseButtonStates.Dragging;
        if (this.FinishedInTransition)
        {
          if (this.InputManager.RotateRight == FezButtonState.Pressed)
            SoundEffectExtensions.Emit(this.sRotateRight);
          if (this.InputManager.RotateLeft == FezButtonState.Pressed)
            SoundEffectExtensions.Emit(this.sRotateLeft);
          if (!FezMath.AlmostEqual(this.InputManager.Movement, Vector2.Zero))
          {
            IGameCameraManager cameraManager = this.CameraManager;
            Vector3 vector3 = cameraManager.Center + this.InputManager.Movement.X * right * 0.02f * radius / viewScale + this.InputManager.Movement.Y * up * 0.02f * radius / viewScale;
            cameraManager.Center = vector3;
          }
          if (flag)
          {
            IGameCameraManager cameraManager = this.CameraManager;
            Vector3 vector3 = cameraManager.Center + (float) -this.MouseState.Movement.X * right * 0.0008f * radius / (viewScale * viewScale) + (float) this.MouseState.Movement.Y * up * 0.0008f * radius / (viewScale * viewScale);
            cameraManager.Center = vector3;
          }
          else if (this.MouseState.RightButton.State == MouseButtonStates.DragEnded)
            this.blockViewPicking = false;
          WorldMap worldMap = this;
          int num = worldMap.blockViewPicking | flag ? 1 : 0;
          worldMap.blockViewPicking = num != 0;
          if (this.InputManager.MapZoomIn == FezButtonState.Pressed && this.ZoomLevel != WorldMap.ZoomCycle.Length - 1)
          {
            ++this.ZoomLevel;
            SoundEffectExtensions.Emit(this.sZoomIn);
            this.ShadeNeighbourNodes();
          }
          if (this.InputManager.MapZoomOut == FezButtonState.Pressed && this.ZoomLevel != 0)
          {
            --this.ZoomLevel;
            SoundEffectExtensions.Emit(this.sZoomOut);
            this.ShadeNeighbourNodes();
          }
          if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
            this.MousePicking();
          this.ViewPicking();
        }
        foreach (Group group in this.ButtonsMesh.Groups)
        {
          AnimatedTexture animatedTexture = group.CustomData as AnimatedTexture;
          if (animatedTexture != null)
          {
            animatedTexture.Timing.Update(gameTime.ElapsedGameTime);
            int width = animatedTexture.Texture.Width;
            int height = animatedTexture.Texture.Height;
            int frame = animatedTexture.Timing.Frame;
            Rectangle rectangle = animatedTexture.Offsets[frame];
            group.TextureMatrix.Set(new Matrix?(new Matrix((float) rectangle.Width / (float) width, 0.0f, 0.0f, 0.0f, 0.0f, (float) rectangle.Height / (float) height, 0.0f, 0.0f, (float) rectangle.X / (float) width, (float) rectangle.Y / (float) height, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f)));
          }
        }
        if (this.CurrentNode.Group != null)
        {
          this.WavesMesh.Position = Vector3.Transform(Vector3.Zero, (Matrix) this.CurrentNode.Group.WorldMatrix) - this.CameraManager.Direction;
          this.WavesMesh.Rotation = this.CameraManager.Rotation;
          double num1 = this.SinceStarted.TotalSeconds * 0.449999988079071;
          for (int index = 0; index < this.WavesMesh.Groups.Count; ++index)
          {
            float num2 = Easing.Ease((double) FezMath.Frac((float) num1 + (float) index / (float) (this.WavesMesh.Groups.Count * 2)), -0.5f, EasingType.Sine);
            float num3 = this.WavesMesh.Groups[index].Scale.X;
            this.WavesMesh.Groups[index].Scale = new Vector3(num2 * 5f * LevelNodeTypeExtensions.GetSizeFactor(this.CurrentNode.NodeType));
            if (index == 0 && (double) num3 > (double) this.WavesMesh.Groups[index].Scale.X)
              SoundEffectExtensions.EmitAt(this.sBeacon, this.WavesMesh.Position).OverrideMap = true;
            this.WavesMesh.Groups[index].Material.Opacity = (float) (1.0 - ((double) num2 - 0.400000005960464) / 0.600000023841858);
          }
        }
        this.ScaleIcons();
        foreach (Group group in this.NodesMesh.Groups)
        {
          NodeGroupData nodeGroupData = group.CustomData as NodeGroupData;
          ShaderInstancedIndexedPrimitives<VertexPositionColorTextureInstance, Matrix> indexedPrimitives = nodeGroupData.Complete ? this.GoldenHighlightsGeometry : this.WhiteHighlightsGeometry;
          float num = LevelNodeTypeExtensions.GetSizeFactor(nodeGroupData.Node.NodeType) + 0.125f;
          Matrix matrix = indexedPrimitives.Instances[nodeGroupData.HighlightInstance];
          Vector3 vector3_1 = new Vector3(matrix.M31, matrix.M32, matrix.M33);
          if (this.FocusNode == nodeGroupData.Node)
            vector3_1 = Vector3.Lerp(vector3_1, new Vector3(1.25f * num), 0.25f);
          else if (!FezMath.AlmostEqual(vector3_1.X, 1f))
            vector3_1 = Vector3.Lerp(vector3_1, new Vector3(num), 0.25f);
          Vector3 vector3_2 = group.Position + LevelNodeTypeExtensions.GetSizeFactor(nodeGroupData.Node.NodeType) / 2f * new Vector3(1f, 1f, -1f) * vector3_1 / num + 0.2f * new Vector3(1f, 0.0f, -1f);
          float iconScale = this.GetIconScale(viewScale, radius / viewScale);
          indexedPrimitives.Instances[nodeGroupData.HighlightInstance].M31 = vector3_1.X;
          indexedPrimitives.Instances[nodeGroupData.HighlightInstance].M32 = vector3_1.Y;
          indexedPrimitives.Instances[nodeGroupData.HighlightInstance].M33 = vector3_1.Z;
          foreach (int index in nodeGroupData.IconInstances)
          {
            Vector3 vector3_3 = this.IconsTrailingOffset[index] * (iconScale / 4f) * this.CameraManager.InverseView.Down;
            this.IconsGeometry.Instances[index].M11 = vector3_2.X + vector3_3.X;
            this.IconsGeometry.Instances[index].M12 = vector3_2.Y + vector3_3.Y;
            this.IconsGeometry.Instances[index].M13 = vector3_2.Z + vector3_3.Z;
          }
        }
        if (this.FocusNode != null && this.FocusNode.Group != null && !flag)
        {
          Vector3 vector3_1 = Vector3.Transform(Vector3.Zero, (Matrix) this.FocusNode.Group.WorldMatrix);
          Vector3 vector3_2 = this.CameraManager.Center - vector3_1;
          float a = vector3_2.Length();
          if (!FezMath.AlmostEqual(a, 0.0f))
          {
            Vector3 vector3_3 = vector3_2 / a;
            float num = MathHelper.Clamp(a * 2f, 0.0f, this.chosenByMouseClick ? 3f : 1f);
            IGameCameraManager cameraManager1 = this.CameraManager;
            Vector3 vector3_4 = cameraManager1.Center - vector3_3 * num * radius * 0.005f;
            cameraManager1.Center = vector3_4;
            IGameCameraManager cameraManager2 = this.CameraManager;
            Vector3 vector3_5 = cameraManager2.Center + FezMath.Dot(vector3_1 - this.CameraManager.Center, forward) * forward;
            cameraManager2.Center = vector3_5;
          }
          if (this.blockViewPicking && (double) (this.CameraManager.InterpolatedCenter - vector3_1).Length() < 0.5)
            this.blockViewPicking = false;
        }
        this.ShadeNeighbourNodes();
        if (this.DotDialogueIndex >= WorldMap.DotDialogue.Length || this.DotManager.Hidden || this.GameState.SaveData.HasHadMapHelp)
          return;
        if (this.SpeechBubble.Hidden)
        {
          this.SpeechBubble.ChangeText(GameText.GetString(WorldMap.DotDialogue[this.DotDialogueIndex]));
        }
        else
        {
          this.SpeechBubble.Origin = this.DotManager.Position - new Vector3(0.0f, (float) (1.0 / (double) radius * 2.0) * viewScale, 0.0f);
          if (this.InputManager.CancelTalk != FezButtonState.Pressed)
            return;
          SoundEffectExtensions.Emit(this.sTextNext);
          ++this.DotDialogueIndex;
          if (this.DotDialogueIndex == WorldMap.DotDialogue.Length)
          {
            this.DotManager.Burrow();
            this.GameState.SaveData.HasHadMapHelp = true;
          }
          this.SpeechBubble.Hide();
        }
      }
    }

    private void ViewPicking()
    {
      this.CursorSelectable = false;
      Vector3 right = this.CameraManager.InverseView.Right;
      Vector3 up = this.CameraManager.InverseView.Up;
      Vector3 forward = this.CameraManager.InverseView.Forward;
      this.closestNodes.Clear();
      float minDepth = float.MaxValue;
      float val2_1 = float.MinValue;
      float minDist = float.MaxValue;
      float val2_2 = float.MinValue;
      Ray ray1 = new Ray(this.GraphicsDevice.Viewport.Unproject(new Vector3((float) this.MouseState.Position.X, (float) this.MouseState.Position.Y, 0.0f), this.CameraManager.Projection, this.CameraManager.View, Matrix.Identity), forward);
      Ray ray2 = new Ray(this.CameraManager.Position - forward * this.CameraManager.Radius, forward);
      foreach (Group group in this.NodesMesh.Groups)
      {
        float num1 = group.Material.Opacity;
        if ((double) num1 >= 0.00999999977648258)
        {
          NodeGroupData nodeGroupData = group.CustomData as NodeGroupData;
          float sizeFactor = LevelNodeTypeExtensions.GetSizeFactor(nodeGroupData.Node.NodeType);
          float num2 = (float) (((double) sizeFactor - 0.5) / 2.0 + 1.0) * MathHelper.Lerp(num1, 1f, 0.5f);
          Vector3 vector3 = Vector3.Transform(Vector3.Zero, (Matrix) group.WorldMatrix);
          BoundingBox box = new BoundingBox(vector3 - new Vector3(num2), vector3 + new Vector3(num2));
          float val1_1 = FezMath.Dot(vector3 - this.CameraManager.Position, forward);
          nodeGroupData.Depth = val1_1;
          if (ray1.Intersects(box).HasValue)
            this.CursorSelectable = true;
          if (!this.blockViewPicking && ray2.Intersects(box).HasValue)
          {
            minDepth = Math.Min(val1_1, minDepth);
            val2_1 = Math.Max(val1_1, val2_1);
            Vector3 a = vector3 - this.CameraManager.Position;
            float val1_2 = new Vector2(FezMath.Dot(a, right), FezMath.Dot(a, up)).Length() * sizeFactor;
            minDist = Math.Min(val1_2, minDist);
            val2_2 = Math.Max(val1_2, val2_2);
            this.closestNodes.Add(new WorldMap.QualifiedNode()
            {
              Node = nodeGroupData.Node,
              Depth = val1_1,
              ScreenDistance = val1_2,
              Transparency = FezMath.AlmostClamp(1f - num1)
            });
          }
        }
      }
      if (this.blockViewPicking)
        return;
      if (this.closestNodes.Count > 0)
      {
        float depthRange = val2_1 - minDepth;
        float distRange = val2_2 - minDist;
        WorldMap.QualifiedNode qualifiedNode = Enumerable.FirstOrDefault<WorldMap.QualifiedNode>((IEnumerable<WorldMap.QualifiedNode>) Enumerable.OrderBy<WorldMap.QualifiedNode, float>((IEnumerable<WorldMap.QualifiedNode>) this.closestNodes, (Func<WorldMap.QualifiedNode, float>) (n => (float) ((double) n.Transparency * 2.0 + ((double) n.ScreenDistance - (double) minDist) / (double) distRange + ((double) n.Depth - (double) minDepth) / (double) depthRange / 2.0))));
        MapNode mapNode = this.FocusNode;
        this.LastFocusedNode = this.FocusNode = qualifiedNode.Node;
        if (this.FocusNode != null && this.FocusNode != mapNode)
          SoundEffectExtensions.Emit(this.sMagnet);
        this.chosenByMouseClick = false;
      }
      else
        this.FocusNode = (MapNode) null;
      this.NodesMesh.Groups.Sort((IComparer<Group>) WorldMap.NodeGroupDataComparer.Default);
    }

    private void MousePicking()
    {
      Vector3 right = this.CameraManager.InverseView.Right;
      Vector3 up = this.CameraManager.InverseView.Up;
      Vector3 forward = this.CameraManager.InverseView.Forward;
      this.closestNodes.Clear();
      float minDepth = float.MaxValue;
      float val2_1 = float.MinValue;
      float minDist = float.MaxValue;
      float val2_2 = float.MinValue;
      Ray ray = new Ray(this.GraphicsDevice.Viewport.Unproject(new Vector3((float) this.MouseState.Position.X, (float) this.MouseState.Position.Y, 0.0f), this.CameraManager.Projection, this.CameraManager.View, Matrix.Identity), forward);
      foreach (Group group in this.NodesMesh.Groups)
      {
        float num1 = group.Material.Opacity;
        if ((double) num1 >= 0.00999999977648258)
        {
          NodeGroupData nodeGroupData = group.CustomData as NodeGroupData;
          float sizeFactor = LevelNodeTypeExtensions.GetSizeFactor(nodeGroupData.Node.NodeType);
          float num2 = sizeFactor * 0.625f;
          Vector3 vector3 = Vector3.Transform(Vector3.Zero, (Matrix) group.WorldMatrix);
          BoundingBox box = new BoundingBox(vector3 - new Vector3(num2), vector3 + new Vector3(num2));
          float val1_1 = FezMath.Dot(vector3 - this.CameraManager.Position, forward);
          nodeGroupData.Depth = val1_1;
          if (ray.Intersects(box).HasValue)
          {
            minDepth = Math.Min(val1_1, minDepth);
            val2_1 = Math.Max(val1_1, val2_1);
            Vector3 a = vector3 - this.CameraManager.Position;
            float val1_2 = new Vector2(FezMath.Dot(a, right), FezMath.Dot(a, up)).Length() * sizeFactor;
            minDist = Math.Min(val1_2, minDist);
            val2_2 = Math.Max(val1_2, val2_2);
            this.closestNodes.Add(new WorldMap.QualifiedNode()
            {
              Node = nodeGroupData.Node,
              Depth = val1_1,
              ScreenDistance = val1_2,
              Transparency = FezMath.AlmostClamp(1f - num1)
            });
          }
        }
      }
      if (this.closestNodes.Count > 0)
      {
        float depthRange = val2_1 - minDepth;
        float distRange = val2_2 - minDist;
        WorldMap.QualifiedNode qualifiedNode = Enumerable.FirstOrDefault<WorldMap.QualifiedNode>((IEnumerable<WorldMap.QualifiedNode>) Enumerable.OrderBy<WorldMap.QualifiedNode, float>((IEnumerable<WorldMap.QualifiedNode>) this.closestNodes, (Func<WorldMap.QualifiedNode, float>) (n => (float) ((double) n.Transparency * 2.0 + ((double) n.ScreenDistance - (double) minDist) / (double) distRange + ((double) n.Depth - (double) minDepth) / (double) depthRange / 2.0))));
        MapNode mapNode = this.FocusNode;
        this.LastFocusedNode = this.FocusNode = qualifiedNode.Node;
        if (this.FocusNode != null && this.FocusNode != mapNode)
          SoundEffectExtensions.Emit(this.sMagnet);
        this.chosenByMouseClick = true;
        this.blockViewPicking = true;
      }
      this.NodesMesh.Groups.Sort((IComparer<Group>) WorldMap.NodeGroupDataComparer.Default);
    }

    private float GetIconScale(float viewScale, float radius)
    {
      return (double) viewScale <= 1.0 ? ((double) radius <= 16.0 || (double) radius > 40.0 ? ((double) radius <= 40.0 ? 1f : (float) (((double) radius - 40.0) / 40.0 * 2.5 + 2.5)) : (float) (((double) radius - 16.0) / 24.0 * 1.5 + 1.0)) : ((double) radius <= 16.0 || (double) radius > 40.0 ? ((double) radius <= 40.0 ? 1f : (float) (((double) radius - 40.0) / 40.0 * 1.25 + 1.25)) : (float) (((double) radius - 16.0) / 24.0 * 0.25 + 1.0));
    }

    private void ScaleIcons()
    {
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      float radius = this.CameraManager.Radius / viewScale;
      float iconScale = this.GetIconScale(viewScale, radius);
      for (int index = 0; index < this.IconsGeometry.Instances.Length; ++index)
      {
        this.IconsGeometry.Instances[index].M31 = this.IconsOriginalInstances[index].M31 * iconScale;
        this.IconsGeometry.Instances[index].M32 = this.IconsOriginalInstances[index].M32 * iconScale;
        this.IconsGeometry.Instances[index].M33 = this.IconsOriginalInstances[index].M33 * iconScale;
      }
    }

    private void ShadeNeighbourNodes()
    {
      int num1 = 0;
      MapNode mapNode1 = this.FocusNode ?? this.LastFocusedNode;
      this.nextToCover.Clear();
      this.nextToCover.Add(mapNode1);
      this.toCover.Clear();
      this.hasCovered.Clear();
      this.hasCovered.Add(mapNode1);
      float num2 = 5f - (float) this.ZoomLevel;
      float num3 = (float) (1.0 + Math.Round((double) num2 > 3.0 ? Math.Pow((double) num2 - 1.75, 2.5) : (double) num2));
      while (this.nextToCover.Count > 0)
      {
        this.toCover.Clear();
        this.toCover.AddRange((IEnumerable<MapNode>) this.nextToCover);
        this.nextToCover.Clear();
        foreach (MapNode mapNode2 in this.toCover)
        {
          Group group = mapNode2.Group;
          if (group != null)
          {
            float num4 = mapNode2 == this.CurrentNode ? 1f : FezMath.Saturate((num3 - (float) num1) / num3);
            group.Material.Opacity = MathHelper.Lerp(group.Material.Opacity, num4, 0.2f);
            group.Enabled = (double) group.Material.Opacity > 0.00999999977648258;
            NodeGroupData nodeGroupData = (NodeGroupData) group.CustomData;
            (nodeGroupData.Complete ? this.GoldenHighlightsGeometry : this.WhiteHighlightsGeometry).Instances[nodeGroupData.HighlightInstance].M24 = group.Material.Opacity;
            foreach (int index in nodeGroupData.IconInstances)
              this.IconsGeometry.Instances[index].M24 = MathHelper.Lerp(this.IconsGeometry.Instances[index].M24, group.Material.Opacity, 0.2f);
            float num5 = FezMath.Saturate((num3 - (num1 == 0 ? 0.0f : (float) (num1 + 1))) / num3);
            foreach (MapNode.Connection connection in mapNode2.Connections)
            {
              if (!this.hasCovered.Contains(connection.Node))
              {
                if (connection.LinkInstances != null)
                {
                  foreach (int index in connection.LinkInstances)
                    this.LinksGeometry.Instances[index].M24 = MathHelper.Lerp(this.LinksGeometry.Instances[index].M24, num5, 0.2f);
                }
                this.nextToCover.Add(connection.Node);
                this.hasCovered.Add(connection.Node);
              }
            }
          }
        }
        ++num1;
      }
    }

    private void Exit()
    {
      this.ScheduleExit = true;
      this.Resolved = false;
      this.TargetRenderingManager.ScheduleHook(this.DrawOrder, this.FadeOutRtHandle.Target);
      this.GameService.CloseScroll((string) null);
    }

    private void StartInTransition()
    {
      this.GameState.SkipRendering = true;
      this.CameraManager.PixelsPerTrixel = 3f;
      WorldMap.Starfield.Opacity = this.LinksMesh.Material.Opacity = this.NodesMesh.Material.Opacity = this.LegendMesh.Material.Opacity = 0.0f;
      this.CameraManager.ChangeViewpoint(Viewpoint.Front, 0.0f);
      Quaternion phi180 = this.OriginalRotation * Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f);
      Vector3 aoMaxPos = this.PlayerManager.Center + 6f * Vector3.UnitY;
      Waiters.Interpolate(0.75, (Action<float>) (s =>
      {
        if (!this.Enabled)
          return;
        float local_0 = Easing.EaseOut((double) s, EasingType.Cubic);
        WorldMap.Starfield.Opacity = this.LinksMesh.Material.Opacity = this.NodesMesh.Material.Opacity = this.LegendMesh.Material.Opacity = local_0;
        this.IconsMesh.Scale = this.LinksMesh.Scale = this.NodesMesh.Scale = new Vector3((float) (0.5 + (double) local_0 / 2.0));
        this.IconsMesh.Position = this.LinksMesh.Position = this.NodesMesh.Position = this.PlayerManager.Center + local_0 * 6f * Vector3.UnitY;
        this.IconsMesh.Rotation = this.LinksMesh.Rotation = this.NodesMesh.Rotation = Quaternion.Slerp(phi180, this.OriginalRotation, local_0);
        this.CameraManager.Center = Vector3.Lerp(this.OriginalCenter, aoMaxPos + Vector3.Transform(this.CurrentNode.Group == null ? Vector3.Zero : this.CurrentNode.Group.Position, this.NodesMesh.Rotation), local_0);
      }), (Action) (() => this.FinishedInTransition = true));
    }

    private void StartOutTransition()
    {
      this.GameState.SkipRendering = false;
      if (!this.GameState.SaveData.HasHadMapHelp)
      {
        this.DotManager.RevertDrawOrder();
        this.SpeechBubble.RevertDrawOrder();
        this.DotManager.Burrow();
        this.SpeechBubble.Hide();
      }
      SoundEffectExtensions.Emit(this.sExit);
      this.CameraManager.PixelsPerTrixel = this.OriginalPixPerTrix;
      this.GameState.InMap = false;
      this.CameraManager.ChangeViewpoint(this.OriginalViewpoint, 0.0f);
      this.CameraManager.Center = this.OriginalCenter + 6f * Vector3.UnitY;
      this.CameraManager.Direction = this.OriginalDirection;
      this.CameraManager.SnapInterpolation();
      Waiters.Interpolate(0.75, (Action<float>) (s =>
      {
        this.CameraManager.Center = Vector3.Lerp(this.OriginalCenter, this.OriginalCenter + 6f * Vector3.UnitY, 1f - Easing.EaseInOut((double) s, EasingType.Sine, EasingType.Quadratic));
        this.NodesMesh.Material.Opacity = 1f - Easing.EaseOut((double) s, EasingType.Quadratic);
      }), (Action) (() => ServiceHelper.RemoveComponent<WorldMap>(this)));
      this.Enabled = false;
    }

    public override void Draw(GameTime gameTime)
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      if (this.GameState.StereoMode)
        BaseEffect.EyeSign = Vector3.Zero;
      if (this.TargetRenderingManager.IsHooked(this.FadeInRtHandle.Target) && !this.Resolved && !this.ScheduleExit)
      {
        this.TargetRenderingManager.Resolve(this.FadeInRtHandle.Target, false);
        this.GraphicsDevice.Clear(Color.Black);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.Resolved = true;
        this.GameState.InMap = true;
        this.InputManager.StrictRotation = true;
        this.StartInTransition();
      }
      if (this.ScheduleExit && this.Resolved)
      {
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Alphablending);
        graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        this.TargetRenderingManager.DrawFullscreen((Texture) this.FadeOutRtHandle.Target, new Color(1f, 1f, 1f, this.NodesMesh.Material.Opacity));
      }
      else
      {
        if (this.ScheduleExit && !this.Resolved)
          GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue);
        float alpha = this.NodesMesh.Material.Opacity;
        foreach (Group group in this.ButtonsMesh.Groups)
          group.Material.Opacity = alpha;
        if (this.Enabled)
        {
          if ((double) alpha < 1.0)
          {
            this.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1f, 0);
            GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Opaque);
            SettingsManager.SetupViewport(this.GraphicsDevice, false);
            this.TargetRenderingManager.DrawFullscreen((Texture) this.FadeInRtHandle.Target);
          }
        }
        else
        {
          for (int index = 0; index < this.WavesMesh.Groups.Count; ++index)
            this.WavesMesh.Groups[index].Material.Opacity *= 0.9f;
        }
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Alphablending);
        if ((double) alpha < 1.0)
          this.TargetRenderingManager.DrawFullscreen(new Color(0.0f, 0.0f, 0.0f, alpha));
        else
          this.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1f, 0);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        WorldMap.Starfield.Draw();
        this.GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Stencil, Color.Black, 1f, 0);
        this.NodesMesh.Draw();
        this.LinksMesh.Draw();
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.Trails);
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Additive);
        this.TargetRenderingManager.DrawFullscreen((Texture) this.ShineTex, new Color(0.5f, 0.435f, 0.285f));
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
        this.WavesMesh.Draw();
        this.IconsMesh.Draw();
        this.ButtonsMesh.Draw();
        this.LegendMesh.Draw();
        if (Culture.IsCJK)
          GraphicsDeviceExtensions.BeginLinear(this.SpriteBatch);
        else
          GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
        bool flag = this.GraphicsDevice.DisplayMode.Width < 1280;
        float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
        SpriteFont font = Culture.IsCJK ? this.FontManager.Small : this.FontManager.Big;
        float scale1 = (Culture.IsCJK ? this.FontManager.SmallFactor * 0.8f : (flag ? 1.5f : 1f)) * viewScale;
        string[] strArray1 = new string[5]
        {
          StaticText.GetString("MapPan"),
          StaticText.GetString("MapZoom"),
          StaticText.GetString("MapSpin"),
          StaticText.GetString("MapLook"),
          StaticText.GetString("MapBack")
        };
        float[] numArray = new float[5]
        {
          48f,
          48f,
          41f,
          37f,
          45f
        };
        float num1 = Culture.IsCJK ? -15f : -25f;
        float val1_1 = 0.0f;
        int num2 = 0;
        foreach (string text in strArray1)
        {
          val1_1 = Math.Max(val1_1, font.MeasureString(text).X * scale1);
          this.GTR.DrawStringLFLeftAlign(this.SpriteBatch, font, text, new Color(1f, 1f, 1f, alpha), new Vector2((float) (1280 - (GamepadState.AnyConnected ? 150 : 180)), 130f + num1) * viewScale, scale1);
          num1 += numArray[num2++];
        }
        this.ButtonsMesh.Groups[0].Scale = new Vector3((float) ((double) val1_1 / 1280.0 * 40.0 / (double) viewScale + (GamepadState.AnyConnected ? 3.125 : 4.0)), 0.975f, 1f);
        this.ButtonsMesh.Groups[0].Position = new Vector3(2.7f - this.ButtonsMesh.Groups[0].Scale.X, 0.0f, 0.0f);
        string[] strArray2 = new string[7]
        {
          StaticText.GetString("MapLegendWarpGate"),
          StaticText.GetString("MapLegendSmallGate"),
          StaticText.GetString("MapLegendTreasure"),
          StaticText.GetString("MapLegendLockedDoor"),
          StaticText.GetString("MapLegendCube"),
          StaticText.GetString("MapLegendBits"),
          StaticText.GetString("MapLegendSecret")
        };
        if (flag)
        {
          float num3 = Culture.IsCJK ? 3f : -8f;
          float val1_2 = 0.0f;
          foreach (string text in strArray2)
          {
            val1_2 = Math.Max(val1_2, font.MeasureString(text).X * scale1);
            this.GTR.DrawString(this.SpriteBatch, font, text, new Vector2(104f, 507f + num3), new Color(1f, 1f, 1f, alpha), scale1);
            num3 += 24.5f;
          }
          this.LegendMesh.Groups[0].Scale = new Vector3((float) ((double) val1_2 / 1280.0 * 40.0 + 1.75), 1f, 1f);
        }
        else
        {
          float num3 = Culture.IsCJK ? 3f : 0.0f;
          float val1_2 = 0.0f;
          foreach (string text in strArray2)
          {
            val1_2 = Math.Max(val1_2, font.MeasureString(text).X * scale1);
            this.GTR.DrawString(this.SpriteBatch, font, text, new Vector2(104f, 507f + num3) * viewScale, new Color(1f, 1f, 1f, alpha), scale1);
            num3 += 24f;
          }
          this.LegendMesh.Groups[0].Scale = new Vector3((float) ((double) val1_2 / 1280.0 * 40.0 / (double) viewScale + 1.75), 1f, 1f);
        }
        float num4 = GamepadState.AnyConnected ? 2f : 1.5f;
        int num5 = GamepadState.AnyConnected ? 170 : 180;
        if (Culture.IsCJK)
        {
          this.SpriteBatch.End();
          GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
        }
        Texture2D replacedGlyphTexture1 = this.GTR.GetReplacedGlyphTexture("{LS}");
        this.SpriteBatch.Draw(replacedGlyphTexture1, new Vector2((float) (1280 - num5) + (float) ((double) (64 - replacedGlyphTexture1.Width) / 2.0 * 1.5), 57f) * viewScale, new Rectangle?(), Color.White, 0.0f, Vector2.Zero, num4 * viewScale, SpriteEffects.None, 0.0f);
        Texture2D replacedGlyphTexture2 = this.GTR.GetReplacedGlyphTexture("{RB}");
        this.SpriteBatch.Draw(replacedGlyphTexture2, new Vector2((float) (1280 - num5) + (float) ((double) (64 - replacedGlyphTexture2.Width) / 2.0 * 1.5), (float) (107.0 - 6.0 * (double) num4)) * viewScale, new Rectangle?(), Color.White, 0.0f, Vector2.Zero, num4 * viewScale, SpriteEffects.None, 0.0f);
        Texture2D replacedGlyphTexture3 = this.GTR.GetReplacedGlyphTexture("{LB}");
        this.SpriteBatch.Draw(replacedGlyphTexture3, new Vector2((float) (1280 - num5) + (float) ((double) (64 - replacedGlyphTexture3.Width) / 2.0 * 1.5), (float) (107.0 + 6.0 * (double) num4)) * viewScale, new Rectangle?(), Color.White, 0.0f, Vector2.Zero, num4 * viewScale, SpriteEffects.None, 0.0f);
        Texture2D replacedGlyphTexture4 = this.GTR.GetReplacedGlyphTexture("{LT}");
        this.SpriteBatch.Draw(replacedGlyphTexture4, new Vector2((float) ((double) (1280 - num5) + (double) (64 - replacedGlyphTexture4.Width) / 2.0 * 1.5 + (GamepadState.AnyConnected ? -5.0 * (double) num4 : 0.0)), (float) (155.0 + (GamepadState.AnyConnected ? 0.0 : -6.0 * (double) num4))) * viewScale, new Rectangle?(), Color.White, 0.0f, Vector2.Zero, num4 * viewScale, SpriteEffects.None, 0.0f);
        Texture2D replacedGlyphTexture5 = this.GTR.GetReplacedGlyphTexture("{RT}");
        this.SpriteBatch.Draw(replacedGlyphTexture5, new Vector2((float) ((double) (1280 - num5) + (double) (64 - replacedGlyphTexture5.Width) / 2.0 * 1.5 + (GamepadState.AnyConnected ? 5.0 * (double) num4 : 0.0)), (float) (155.0 + (GamepadState.AnyConnected ? 0.0 : 6.0 * (double) num4))) * viewScale, new Rectangle?(), Color.White, 0.0f, Vector2.Zero, num4 * viewScale, SpriteEffects.None, 0.0f);
        Texture2D replacedGlyphTexture6 = this.GTR.GetReplacedGlyphTexture("{RS}");
        this.SpriteBatch.Draw(replacedGlyphTexture6, new Vector2((float) (1280 - num5) + (float) ((double) (64 - replacedGlyphTexture6.Width) / 2.0 * 1.5), 195f) * viewScale, new Rectangle?(), Color.White, 0.0f, Vector2.Zero, num4 * viewScale, SpriteEffects.None, 0.0f);
        Texture2D replacedGlyphTexture7 = this.GTR.GetReplacedGlyphTexture("{B}");
        this.SpriteBatch.Draw(replacedGlyphTexture7, new Vector2((float) (1280 - num5) + (float) ((double) (64 - replacedGlyphTexture7.Width) / 2.0 * 1.5), (float) (233 + (GamepadState.AnyConnected ? -5 : 0))) * viewScale, new Rectangle?(), Color.White, 0.0f, Vector2.Zero, num4 * viewScale, SpriteEffects.None, 0.0f);
        this.SpriteBatch.End();
        GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
        float scale2 = viewScale * 2f;
        Point point = SettingsManager.PositionInViewport(this.MouseState);
        this.SpriteBatch.Draw(this.MouseState.LeftButton.State == MouseButtonStates.Dragging || this.MouseState.RightButton.State == MouseButtonStates.Dragging ? this.GrabbedCursor : (this.CursorSelectable ? (this.MouseState.LeftButton.State == MouseButtonStates.Down ? this.ClickedCursor : this.CanClickCursor) : this.PointerCursor), new Vector2((float) point.X - scale2 * 11.5f, (float) point.Y - scale2 * 8.5f), new Rectangle?(), new Color(1f, 1f, 1f, FezMath.Saturate((float) (1.0 - ((double) this.SinceMouseMoved - 2.0)))), 0.0f, Vector2.Zero, scale2, SpriteEffects.None, 0.0f);
        this.SpriteBatch.End();
        if (!this.TargetRenderingManager.IsHooked(this.FadeOutRtHandle.Target) || this.Resolved || !this.ScheduleExit)
          return;
        this.TargetRenderingManager.Resolve(this.FadeOutRtHandle.Target, false);
        this.GraphicsDevice.Clear(Color.Black);
        SettingsManager.SetupViewport(this.GraphicsDevice, false);
        this.TargetRenderingManager.DrawFullscreen((Texture) this.FadeOutRtHandle.Target);
        this.Resolved = true;
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
        this.StartOutTransition();
      }
    }

    private struct QualifiedNode
    {
      public MapNode Node;
      public float Depth;
      public float ScreenDistance;
      public float Transparency;
    }

    public class NodeGroupDataComparer : IComparer<Group>
    {
      public static readonly WorldMap.NodeGroupDataComparer Default = new WorldMap.NodeGroupDataComparer();

      static NodeGroupDataComparer()
      {
      }

      public int Compare(Group x, Group y)
      {
        return -((NodeGroupData) x.CustomData).Depth.CompareTo(((NodeGroupData) y.CustomData).Depth);
      }
    }
  }
}
