// Type: FezGame.Fez
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using CommunityExpressNS;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Components.Scripting;
using FezGame.Services;
using FezGame.Services.Scripting;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace FezGame
{
  public class Fez : Game
  {
    public static string ForcedLevelName = "GOMEZ_HOUSE_2D";
    public static string Version = "1.06a";
    public const string ForcedTrialLevelName = "trial/BIG_TOWER";
    public static bool LevelChooser;
    public static bool SkipIntro;
    public static bool TrialMode;
    public static bool SkipLogos;
    public static int ForceGoldenCubes;
    public static int ForceAntiCubes;
    public static bool LongScreenshot;
    public static bool PublicDemo;
    public static bool DoubleRotations;
    public static bool VariableTimeStep;
    public static bool NoGamePad;
    public static bool Force60Hz;
    private readonly GraphicsDeviceManager deviceManager;
    private InputManager InputManager;
    private float sinceLoading;

    public bool IsDisposed { get; private set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderingManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    static Fez()
    {
    }

    public Fez()
    {
      ThreadExecutionState.SetUp();
      DateTime dateTime = Fez.RetrieveLinkerTimestamp();
      Logger.Log("Version", Fez.Version + ", Build Date : " + dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString());
      if (!CommunityExpress.Instance.Initialize())
      {
        int num = (int) MessageBox.Show("This version of FEZ needs Steam to be running.\r\nPlease make sure an instance of Steam is active before starting the game.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
        this.Exit();
        this.IsDisposed = true;
      }
      else
      {
        this.deviceManager = new GraphicsDeviceManager((Game) this);
        SettingsManager.DeviceManager = this.deviceManager;
        this.Content.RootDirectory = "Content";
        Guide.SimulateTrialMode = Fez.TrialMode;
        ServiceHelper.Game = (Game) this;
        ServiceHelper.IsFull = true;
      }
    }

    protected override void Initialize()
    {
      if (Logger.ErrorEncountered)
      {
        this.Exit();
        this.IsDisposed = true;
        int num = (int) MessageBox.Show("FEZ has encountered one or more error(s) and could not continue operation.\r\nPlease check the 'Debug Log.txt' file in the application folder for more information about the error.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
      }
      DriveInfo[] drives = DriveInfo.GetDrives();
      string str1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Substring(0, 3);
      foreach (DriveInfo driveInfo in drives)
      {
        if (!(driveInfo.Name != str1))
        {
          if (driveInfo.IsReady && driveInfo.AvailableFreeSpace < 202928128L)
          {
            this.Exit();
            this.IsDisposed = true;
            int num = (int) MessageBox.Show("You need at least 200Mb of free space on your main hard drive for FEZ to run.\r\nThis is required for music decompression. Please free some space and try again!", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
            break;
          }
          else
            break;
        }
      }
      this.deviceManager.SynchronizeWithVerticalRetrace = true;
      this.IsFixedTimeStep = !Fez.VariableTimeStep;
      this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
      if (Fez.Force60Hz)
        this.AssumeTargetElapsedTime = true;
      SettingsManager.Apply();
      ServiceHelper.AddService((object) new KeyboardStateManager());
      ServiceHelper.AddService((object) new MouseStateManager());
      ServiceHelper.AddService((object) new PlayerManager());
      ServiceHelper.AddService((object) new CollisionManager());
      ServiceHelper.AddService((object) new DebuggingBag());
      ServiceHelper.AddService((object) new PhysicsManager());
      ServiceHelper.AddService((object) new TimeManager());
      ServiceHelper.AddService((object) new GameStateManager());
      ServiceHelper.AddComponent((IGameComponent) new GamepadsManager((Game) this, !Fez.NoGamePad), true);
      ServiceHelper.AddComponent((IGameComponent) (this.InputManager = new InputManager((Game) this, true, true, !Fez.NoGamePad)), true);
      ServiceHelper.AddComponent((IGameComponent) new ContentManagerProvider((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new GameCameraManager((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new GameLevelMaterializer((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new GameLevelManager((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new FogManager((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new TargetRenderingManager((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new SoundManager((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new PersistentThreadPool((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new TrixelParticleSystems((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new TrialAndAwards((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new SpeechBubble((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new PlaneParticleSystems((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new FontManager((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new ScriptingHost((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new DotHost((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new RotatingGroupsHost((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new BigWaterfallHost((Game) this), true);
      ServiceHelper.AddComponent((IGameComponent) new BlackHolesHost((Game) this), true);
      ServiceHelper.AddService((object) new CameraService());
      ServiceHelper.AddService((object) new GomezService());
      ServiceHelper.AddService((object) new LevelService());
      ServiceHelper.AddService((object) new SoundService());
      ServiceHelper.AddService((object) new TimeService());
      ServiceHelper.AddService((object) new VolumeService());
      ServiceHelper.AddService((object) new ArtObjectService());
      ServiceHelper.AddService((object) new GroupService());
      ServiceHelper.AddService((object) new PlaneService());
      ServiceHelper.AddService((object) new NpcService());
      ServiceHelper.AddService((object) new ScriptService());
      ServiceHelper.AddService((object) new SwitchService());
      ServiceHelper.AddService((object) new BitDoorService());
      ServiceHelper.AddService((object) new SuckBlockService());
      ServiceHelper.AddService((object) new PathService());
      ServiceHelper.AddService((object) new SpinBlockService());
      ServiceHelper.AddService((object) new WarpGateService());
      ServiceHelper.AddService((object) new TombstoneService());
      ServiceHelper.AddService((object) new PivotService());
      ServiceHelper.AddService((object) new ValveService());
      ServiceHelper.AddService((object) new CodePatternService());
      ServiceHelper.AddService((object) new LaserEmitterService());
      ServiceHelper.AddService((object) new LaserReceiverService());
      ServiceHelper.AddService((object) new TimeswitchService());
      ServiceHelper.AddService((object) new DotService());
      ServiceHelper.AddService((object) new OwlService());
      ServiceHelper.InitializeServices();
      ServiceHelper.InjectServices((object) this);
      this.GameState.SaveData = new SaveData();
      this.Window.Title = "FEZ";
      if (Fez.LevelChooser)
      {
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Title = "Choose a level!";
        openFileDialog1.InitialDirectory = Path.Combine(Path.Combine(Application.StartupPath, "Content"), "Levels");
        openFileDialog1.Filter = "Fez Compiled Level (*.xnb)|*.xnb";
        openFileDialog1.Multiselect = false;
        using (OpenFileDialog openFileDialog2 = openFileDialog1)
        {
          if (openFileDialog2.ShowDialog((IWin32Window) Control.FromHandle(this.Window.Handle)) == DialogResult.OK)
          {
            Fez.LoadComponents(this);
            base.Initialize();
            string fileName = openFileDialog2.FileName;
            string str2 = fileName.Substring(fileName.LastIndexOf("\\Levels\\") + "\\Levels\\".Length);
            Fez.ForcedLevelName = str2.Substring(0, str2.LastIndexOf(".xnb"));
            this.GameState.SignInAndChooseStorage(new Action(Common.Util.NullAction));
            this.GameState.StartNewGame(new Action(Common.Util.NullAction));
            this.GameState.SaveData.CanOpenMap = true;
            this.GameState.SaveData.IsNew = false;
          }
          else
            this.Exit();
        }
      }
      else if (Fez.SkipIntro)
      {
        Fez.LoadComponents(this);
        base.Initialize();
        this.GameState.SaveSlot = 0;
        this.GameState.SignInAndChooseStorage(new Action(Common.Util.NullAction));
        this.GameState.LoadSaveFile((Action) (() => this.GameState.LoadLevelAsync(new Action(Common.Util.NullAction))));
        this.GameState.SaveData.CanOpenMap = true;
        this.GameState.SaveData.IsNew = false;
      }
      else
      {
        ServiceHelper.AddComponent((IGameComponent) new Intro((Game) this));
        base.Initialize();
      }
    }

    internal static void LoadComponents(Fez game)
    {
      if (ServiceHelper.FirstLoadDone)
        return;
      ServiceHelper.AddComponent((IGameComponent) new StaticPreloader((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new LoadingScreen((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new TimeHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new GameLevelHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new PlayerCameraControl((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new GameLightingPostProcess((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new PlayerActions((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new HeadsUpDisplay((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new GomezHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new VolumesHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new GameStateControl((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new SkyHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new PickupsHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new BombsHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new GameSequencer((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new BurnInPostProcess((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new WatchersHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new MovingGroupsHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new AnimatedPlanesHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new GameNpcHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new PushSwitchesHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new BitDoorsHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new SuckBlocksHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new LevelLooper((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new CameraPathsHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new WarpGateHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new AttachedPlanesHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new SpinBlocksHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new PivotsHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new SpinningTreasuresHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new LiquidHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new TombstonesHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new SplitUpCubeHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new ValvesBoltsTimeswitchesHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new RumblerHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new RainHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new TempleOfLoveHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new WaterfallsHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new GeysersHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new SewerLightHacks((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new FarawayPlaceHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new CodeMachineHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new CrumblersHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new MailboxesHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new PointsOfInterestHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new OrreryHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new OwlStatueHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new CryptHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new BellHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new TelescopeHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new TetrisPuzzleHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new SaveIndicator((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new UnfoldPuzzleHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new QrCodesHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new GameWideCodes((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new FirstPersonView((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new Quantumizer((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new NameOfGodPuzzleHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new ClockTowerHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new PyramidHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new FinalRebuildHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new SecretPassagesHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new OwlHeadHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new StargateHost((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new FlickeringNeon((Game) game));
      ServiceHelper.AddComponent((IGameComponent) new GodRays((Game) game));
      if (Fez.PublicDemo)
        ServiceHelper.AddComponent((IGameComponent) new IdleRestarter((Game) game));
      ServiceHelper.FirstLoadDone = true;
    }

    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      CommunityExpress.Instance.RunCallbacks();
    }

    protected override void Draw(GameTime gameTime)
    {
      if (this.GameState.ScheduleLoadEnd && (!this.GameState.DotLoading || (double) this.sinceLoading > 5.0))
      {
        this.GameState.ScheduleLoadEnd = this.GameState.Loading = this.GameState.DotLoading = false;
        ServiceHelper.FirstLoadDone = true;
        this.sinceLoading = 0.0f;
      }
      if (this.GameState.DotLoading)
        this.sinceLoading += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (this.InputManager.StrictRotation && !this.GameState.InMap)
        this.InputManager.StrictRotation = false;
      lock (this)
      {
        if (!this.GameState.Loading && !this.GameState.SkipRendering)
          this.TargetRenderingManager.OnPreDraw(gameTime);
        this.TargetRenderingManager.OnRtPrepare();
      }
      this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, Color.Black, 1f, 0);
      SettingsManager.SetupViewport(this.GraphicsDevice, false);
      GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.None));
      lock (this)
        base.Draw(gameTime);
    }

    private static DateTime RetrieveLinkerTimestamp()
    {
      string location = Assembly.GetCallingAssembly().Location;
      byte[] buffer = new byte[2048];
      Stream stream = (Stream) null;
      try
      {
        stream = (Stream) new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        stream.Read(buffer, 0, 2048);
      }
      finally
      {
        if (stream != null)
          stream.Close();
      }
      int num = BitConverter.ToInt32(buffer, 60);
      DateTime time = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((double) BitConverter.ToInt32(buffer, num + 8));
      return time.AddHours((double) TimeZone.CurrentTimeZone.GetUtcOffset(time).Hours);
    }
  }
}
