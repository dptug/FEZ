// Type: FezGame.Components.GameWideCodes
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class GameWideCodes : GameComponent
  {
    private static readonly CodeInput[] AchievementCode = new CodeInput[8]
    {
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinLeft,
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinLeft,
      CodeInput.SpinLeft,
      CodeInput.SpinLeft
    };
    private static readonly CodeInput[] JetpackCode = new CodeInput[5]
    {
      CodeInput.Up,
      CodeInput.Up,
      CodeInput.Up,
      CodeInput.Up,
      CodeInput.Jump
    };
    private static readonly CodeInput[] MapCode = new CodeInput[8]
    {
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinLeft,
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinRight,
      CodeInput.SpinLeft
    };
    private readonly List<CodeInput> Input = new List<CodeInput>();
    private TimeSpan SinceInput;
    private TrileInstance waitingForTrile;
    private bool isMapQr;
    private bool isAchievementCode;

    [ServiceDependency]
    public IGomezService GomezService { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public ILevelService LevelService { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    static GameWideCodes()
    {
    }

    public GameWideCodes(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        this.waitingForTrile = (TrileInstance) null;
        this.Input.Clear();
        this.isMapQr = this.isAchievementCode = false;
      });
    }

    public override void Update(GameTime gameTime)
    {
      if (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.GameState.InMap || (this.GameState.Paused || this.GameState.Loading) || (this.GameState.InCutscene || this.GameState.IsTrialMode))
        return;
      this.TestInput();
      this.SinceInput += gameTime.ElapsedGameTime;
    }

    private void TestInput()
    {
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
      this.Input.Add(codeInput);
      if (this.Input.Count > 16)
        this.Input.RemoveAt(0);
      if (!this.isAchievementCode && !this.GameState.SaveData.AchievementCheatCodeDone && (!this.GameState.SaveData.FezHidden && PatternTester.Test((IList<CodeInput>) this.Input, GameWideCodes.AchievementCode)) && this.LevelManager.Name != "ELDERS")
      {
        this.Input.Clear();
        this.isAchievementCode = true;
        this.LevelService.ResolvePuzzleSoundOnly();
        Waiters.Wait((Func<bool>) (() =>
        {
          if (this.CameraManager.ViewTransitionReached && this.PlayerManager.Grounded)
            return !this.PlayerManager.Background;
          else
            return false;
        }), (Action) (() =>
        {
          Vector3 local_0 = this.PlayerManager.Center + new Vector3(0.0f, 2f, 0.0f);
          Trile local_1 = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.SecretCube));
          if (local_1 == null)
            return;
          Vector3 local_2 = local_0 - Vector3.One / 2f;
          NearestTriles local_3 = this.LevelManager.NearestTrile(local_0);
          TrileInstance local_4 = local_3.Surface ?? local_3.Deep;
          if (local_4 != null)
            local_2 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint) * local_2 + local_4.Center * FezMath.DepthMask(this.CameraManager.Viewpoint) - FezMath.ForwardVector(this.CameraManager.Viewpoint) * 2f;
          ServiceHelper.AddComponent((IGameComponent) new GlitchyRespawner(this.Game, this.waitingForTrile = new TrileInstance(Vector3.Clamp(local_2, Vector3.Zero, this.LevelManager.Size - Vector3.One), local_1.Id)));
          this.GomezService.CollectedAnti += new Action(this.GotTrile);
        }));
      }
      if (!this.isMapQr && !this.GameState.SaveData.MapCheatCodeDone && (this.GameState.SaveData.Maps.Contains("MAP_MYSTERY") && this.LevelManager.Name != "WATERTOWER_SECRET") && PatternTester.Test((IList<CodeInput>) this.Input, GameWideCodes.MapCode))
      {
        this.Input.Clear();
        this.GameState.SaveData.AnyCodeDeciphered = true;
        this.isMapQr = true;
        if (this.GameState.SaveData.World.ContainsKey("WATERTOWER_SECRET"))
          this.GameState.SaveData.World["WATERTOWER_SECRET"].FilledConditions.SecretCount = 1;
        this.LevelService.ResolvePuzzleSoundOnly();
        Waiters.Wait((Func<bool>) (() =>
        {
          if (this.CameraManager.ViewTransitionReached && this.PlayerManager.Grounded)
            return !this.PlayerManager.Background;
          else
            return false;
        }), (Action) (() =>
        {
          Vector3 local_0 = this.PlayerManager.Center + new Vector3(0.0f, 2f, 0.0f);
          Trile local_1 = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.SecretCube));
          if (local_1 == null)
            return;
          Vector3 local_2 = local_0 - Vector3.One / 2f;
          NearestTriles local_3 = this.LevelManager.NearestTrile(local_0);
          TrileInstance local_4 = local_3.Surface ?? local_3.Deep;
          if (local_4 != null)
            local_2 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint) * local_2 + local_4.Center * FezMath.DepthMask(this.CameraManager.Viewpoint) - FezMath.ForwardVector(this.CameraManager.Viewpoint) * 2f;
          ServiceHelper.AddComponent((IGameComponent) new GlitchyRespawner(this.Game, this.waitingForTrile = new TrileInstance(Vector3.Clamp(local_2, Vector3.Zero, this.LevelManager.Size - Vector3.One), local_1.Id)));
          this.GomezService.CollectedAnti += new Action(this.GotTrile);
        }));
      }
      if (this.GameState.SaveData.HasNewGamePlus && PatternTester.Test((IList<CodeInput>) this.Input, GameWideCodes.JetpackCode))
      {
        this.Input.Clear();
        this.GameState.JetpackMode = true;
      }
      this.SinceInput = TimeSpan.Zero;
    }

    private void GotTrile()
    {
      if (this.waitingForTrile == null || !this.waitingForTrile.Collected)
        return;
      this.waitingForTrile = (TrileInstance) null;
      this.GomezService.CollectedAnti -= new Action(this.GotTrile);
      if (this.isMapQr)
        this.GameState.SaveData.MapCheatCodeDone = true;
      else if (this.isAchievementCode)
        this.GameState.SaveData.AchievementCheatCodeDone = true;
      this.isAchievementCode = this.isMapQr = false;
    }
  }
}
