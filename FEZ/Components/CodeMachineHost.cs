// Type: FezGame.Components.CodeMachineHost
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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class CodeMachineHost : GameComponent
  {
    private static readonly TimeSpan FadeOutDuration = TimeSpan.FromSeconds(0.100000001490116);
    private static readonly TimeSpan FadeInDuration = TimeSpan.FromSeconds(0.200000002980232);
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(1.0 / 30.0);
    private static readonly TimeSpan TimeOut = TimeSpan.FromSeconds(2.0);
    private static readonly Dictionary<CodeInput, int[]> BitPatterns = new Dictionary<CodeInput, int[]>((IEqualityComparer<CodeInput>) CodeInputComparer.Default)
    {
      {
        CodeInput.Down,
        new int[36]
        {
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          0,
          0,
          0,
          0,
          1,
          1,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0
        }
      },
      {
        CodeInput.Up,
        new int[36]
        {
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          1,
          1,
          0,
          0,
          0,
          0,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1
        }
      },
      {
        CodeInput.Left,
        new int[36]
        {
          0,
          0,
          0,
          0,
          1,
          1,
          0,
          0,
          0,
          0,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          0,
          0,
          1,
          1,
          0,
          0,
          0,
          0,
          1,
          1
        }
      },
      {
        CodeInput.Right,
        new int[36]
        {
          1,
          1,
          0,
          0,
          0,
          0,
          1,
          1,
          0,
          0,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          0,
          0,
          0,
          0,
          1,
          1,
          0,
          0,
          0,
          0
        }
      },
      {
        CodeInput.SpinRight,
        new int[36]
        {
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0
        }
      },
      {
        CodeInput.SpinLeft,
        new int[36]
        {
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1
        }
      },
      {
        CodeInput.Jump,
        new int[36]
        {
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1,
          0,
          0,
          1,
          1,
          1,
          1
        }
      }
    };
    private readonly List<CodeInput> Input = new List<CodeInput>();
    private ArtObjectInstance CodeMachineAO;
    private BackgroundPlane[] BitPlanes;
    private CodeMachineHost.BitState[] BitStates;
    private TimeSpan SinceInput;
    private SoundEffect inputSound;
    private SoundEmitter inputEmitter;

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ICodePatternService CPService { private get; set; }

    static CodeMachineHost()
    {
    }

    public CodeMachineHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.inputSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Zu/CodeMachineInput");
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.CodeMachineAO = (ArtObjectInstance) null;
      this.BitPlanes = (BackgroundPlane[]) null;
      this.BitStates = (CodeMachineHost.BitState[]) null;
      this.Enabled = false;
      this.Input.Clear();
      CodeMachineHost codeMachineHost1 = this;
      CodeMachineHost codeMachineHost2 = this;
      ICollection<ArtObjectInstance> values = this.LevelManager.ArtObjects.Values;
      Func<ArtObjectInstance, bool> predicate = (Func<ArtObjectInstance, bool>) (x => x.ArtObject.ActorType == ActorType.CodeMachine);
      ArtObjectInstance artObjectInstance1;
      ArtObjectInstance artObjectInstance2 = artObjectInstance1 = Enumerable.FirstOrDefault<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) values, predicate);
      codeMachineHost2.CodeMachineAO = artObjectInstance1;
      int num1 = artObjectInstance2 != null ? 1 : 0;
      codeMachineHost1.Enabled = num1 != 0;
      if (!this.Enabled)
        return;
      this.BitPlanes = new BackgroundPlane[144];
      this.BitStates = new CodeMachineHost.BitState[144];
      Texture2D texture2D = this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/glow/code_machine_glowbit");
      for (int index1 = 0; index1 < 36; ++index1)
      {
        BackgroundPlane backgroundPlane1 = new BackgroundPlane(this.LevelMaterializer.StaticPlanesMesh, (Texture) texture2D)
        {
          Fullbright = true,
          Opacity = 0.0f,
          Rotation = Quaternion.Identity
        };
        this.BitPlanes[index1 * 4] = backgroundPlane1;
        BackgroundPlane backgroundPlane2 = backgroundPlane1.Clone();
        backgroundPlane2.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 1.570796f);
        this.BitPlanes[index1 * 4 + 1] = backgroundPlane2;
        BackgroundPlane backgroundPlane3 = backgroundPlane1.Clone();
        backgroundPlane3.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 3.141593f);
        this.BitPlanes[index1 * 4 + 2] = backgroundPlane3;
        BackgroundPlane backgroundPlane4 = backgroundPlane1.Clone();
        backgroundPlane4.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 4.712389f);
        this.BitPlanes[index1 * 4 + 3] = backgroundPlane4;
        int num2 = index1 % 6;
        int num3 = index1 / 6;
        for (int index2 = 0; index2 < 4; ++index2)
        {
          BackgroundPlane plane = this.BitPlanes[index1 * 4 + index2];
          this.BitStates[index1 * 4 + index2] = new CodeMachineHost.BitState();
          Vector3 vector3_1 = Vector3.Transform(Vector3.UnitZ, plane.Rotation);
          Vector3 vector3_2 = Vector3.Transform(Vector3.Right, plane.Rotation);
          plane.Position = this.CodeMachineAO.Position + vector3_1 * 1.5f + vector3_2 * (float) (num2 * 8 - 20) / 16f + Vector3.Up * (float) (35 - num3 * 8) / 16f;
          this.LevelManager.AddPlane(plane);
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMenuCube || this.GameState.InMap))
        return;
      if (this.CameraManager.ViewTransitionReached)
        this.CheckInput();
      this.UpdateBits(gameTime.ElapsedGameTime);
      this.SinceInput += gameTime.ElapsedGameTime;
      if (!(this.SinceInput > CodeMachineHost.TimeOut))
        return;
      this.Input.Clear();
    }

    private void UpdateBits(TimeSpan elapsed)
    {
      for (int index1 = 0; index1 < 36; ++index1)
      {
        int num1 = index1 % 6;
        int num2 = index1 / 6;
        for (int index2 = 0; index2 < 4; ++index2)
        {
          BackgroundPlane backgroundPlane = this.BitPlanes[index1 * 4 + index2];
          CodeMachineHost.BitState bitState = this.BitStates[index1 * 4 + index2];
          TimeSpan timeSpan = TimeSpan.FromTicks(CodeMachineHost.Delay.Ticks * (long) (num1 + num2));
          if (bitState.On)
          {
            if (bitState.SinceOn < CodeMachineHost.FadeInDuration)
            {
              bitState.SinceOn += elapsed;
              backgroundPlane.Opacity = FezMath.Saturate((float) bitState.SinceOn.Ticks / (float) CodeMachineHost.FadeInDuration.Ticks);
              bitState.SinceOff = TimeSpan.FromSeconds((1.0 - (double) backgroundPlane.Opacity) * CodeMachineHost.FadeOutDuration.TotalSeconds) - timeSpan;
            }
            else if (bitState.SinceIdle < CodeMachineHost.TimeOut)
              bitState.SinceIdle += elapsed;
            else
              bitState.On = false;
          }
          else if (bitState.SinceOff < CodeMachineHost.FadeOutDuration)
          {
            bitState.SinceOff += elapsed;
            backgroundPlane.Opacity = FezMath.Saturate((float) (1.0 - (double) bitState.SinceOff.Ticks / (double) CodeMachineHost.FadeOutDuration.Ticks));
            bitState.SinceOn = TimeSpan.FromSeconds((double) backgroundPlane.Opacity * CodeMachineHost.FadeInDuration.TotalSeconds) - timeSpan;
          }
        }
      }
    }

    private void CheckInput()
    {
      Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = FezMath.DepthMask(this.CameraManager.Viewpoint);
      Vector3 vector3_3 = this.CodeMachineAO.ArtObject.Size * vector3_1;
      Vector3 vector3_4 = this.CodeMachineAO.Position * vector3_1;
      if (new BoundingBox(vector3_4 - vector3_3 - Vector3.UnitY * 2f, vector3_4 + vector3_3 + vector3_2).Contains(this.PlayerManager.Position * vector3_1 + vector3_2 / 2f) == ContainmentType.Disjoint)
        return;
      if (this.InputManager.Jump == FezButtonState.Pressed)
        this.OnInput(CodeInput.Jump);
      else if (this.InputManager.RotateRight == FezButtonState.Pressed)
        this.OnInput(CodeInput.SpinRight);
      else if (this.InputManager.RotateLeft == FezButtonState.Pressed)
        this.OnInput(CodeInput.SpinLeft);
      else if (this.InputManager.Left == FezButtonState.Pressed)
        this.OnInput(CodeInput.Left);
      else if (this.InputManager.Right == FezButtonState.Pressed)
        this.OnInput(CodeInput.Right);
      else if (this.InputManager.Up == FezButtonState.Pressed)
      {
        this.OnInput(CodeInput.Up);
      }
      else
      {
        if (this.InputManager.Down != FezButtonState.Pressed)
          return;
        this.OnInput(CodeInput.Down);
      }
    }

    private void OnInput(CodeInput newInput)
    {
      int[] numArray = CodeMachineHost.BitPatterns[newInput];
      if (this.inputEmitter != null && !this.inputEmitter.Dead)
        this.inputEmitter.Cue.Stop(false);
      this.inputEmitter = SoundEffectExtensions.EmitAt(this.inputSound, this.CodeMachineAO.Position, RandomHelper.Between(-0.05, 0.05));
      for (int index1 = 0; index1 < 36; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          CodeMachineHost.BitState bitState = this.BitStates[index1 * 4 + index2];
          bitState.On = numArray[index1] == 1;
          if (bitState.On)
            bitState.SinceIdle = TimeSpan.Zero;
        }
      }
      this.SinceInput = TimeSpan.Zero;
    }

    private class BitState
    {
      public TimeSpan SinceOn;
      public TimeSpan SinceOff;
      public TimeSpan SinceIdle;
      public bool On;
    }
  }
}
