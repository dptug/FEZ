// Type: FezEngine.Components.InputManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Services;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FezEngine.Components
{
  public class InputManager : GameComponent, IInputManager
  {
    private readonly Stack<InputManager.State> savedStates = new Stack<InputManager.State>();
    private GamepadState MockGamepad = new GamepadState(PlayerIndex.One);
    private readonly bool mouse;
    private readonly bool keyboard;
    private readonly bool gamepad;
    private Vector2 lastMouseCenter;

    public ControllerIndex ActiveControllers { get; private set; }

    public FezButtonState GrabThrow { get; private set; }

    public Vector2 Movement { get; private set; }

    public Vector2 FreeLook { get; private set; }

    public FezButtonState Jump { get; private set; }

    public FezButtonState Back { get; private set; }

    public FezButtonState OpenInventory { get; private set; }

    public FezButtonState Start { get; private set; }

    public FezButtonState RotateLeft { get; private set; }

    public FezButtonState RotateRight { get; private set; }

    public FezButtonState CancelTalk { get; private set; }

    public FezButtonState Up { get; private set; }

    public FezButtonState Down { get; private set; }

    public FezButtonState Left { get; private set; }

    public FezButtonState Right { get; private set; }

    public FezButtonState ClampLook { get; private set; }

    public FezButtonState FpsToggle { get; private set; }

    public FezButtonState ExactUp { get; private set; }

    public FezButtonState MapZoomIn { get; private set; }

    public FezButtonState MapZoomOut { get; private set; }

    public bool StrictRotation { get; set; }

    public GamepadState ActiveGamepad
    {
      get
      {
        if (!this.gamepad)
          return this.MockGamepad;
        if (this.ActiveControllers == ControllerIndex.None)
          return this.GamepadsManager[PlayerIndex.One];
        else
          return this.GamepadsManager[ControllerIndexExtensions.GetPlayer(this.ActiveControllers)];
      }
    }

    [ServiceDependency(Optional = true)]
    public IMouseStateManager MouseState { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    [ServiceDependency(Optional = true)]
    public IGamepadsManager GamepadsManager { private get; set; }

    public event Action<PlayerIndex> ActiveControllerDisconnected;

    public InputManager(Game game, bool mouse, bool keyboard, bool gamepad)
      : base(game)
    {
      this.ActiveControllers = ControllerIndex.Any;
      this.mouse = mouse;
      this.keyboard = keyboard;
      this.gamepad = gamepad;
    }

    public override void Update(GameTime gameTime)
    {
      if (!ServiceHelper.Game.IsActive)
        return;
      this.Reset();
      if (this.keyboard)
      {
        this.KeyboardState.Update(Keyboard.GetState(), gameTime);
        this.Movement = new Vector2(FezButtonStateExtensions.IsDown(this.KeyboardState.Right) ? 1f : (FezButtonStateExtensions.IsDown(this.KeyboardState.Left) ? -1f : 0.0f), FezButtonStateExtensions.IsDown(this.KeyboardState.Up) ? 1f : (FezButtonStateExtensions.IsDown(this.KeyboardState.Down) ? -1f : 0.0f));
        this.FreeLook = new Vector2(FezButtonStateExtensions.IsDown(this.KeyboardState.LookRight) ? 1f : (FezButtonStateExtensions.IsDown(this.KeyboardState.LookLeft) ? -1f : 0.0f), FezButtonStateExtensions.IsDown(this.KeyboardState.LookUp) ? 1f : (FezButtonStateExtensions.IsDown(this.KeyboardState.LookDown) ? -1f : 0.0f));
        this.Back = this.KeyboardState.OpenMap;
        this.Start = this.KeyboardState.Pause;
        this.Jump = this.KeyboardState.Jump;
        this.GrabThrow = this.KeyboardState.GrabThrow;
        this.CancelTalk = this.KeyboardState.CancelTalk;
        this.Down = this.KeyboardState.Down;
        this.ExactUp = this.Up = this.KeyboardState.Up;
        this.Left = this.KeyboardState.Left;
        this.Right = this.KeyboardState.Right;
        this.OpenInventory = this.KeyboardState.OpenInventory;
        this.RotateLeft = this.KeyboardState.RotateLeft;
        this.RotateRight = this.KeyboardState.RotateRight;
        this.MapZoomIn = this.KeyboardState.MapZoomIn;
        this.MapZoomOut = this.KeyboardState.MapZoomOut;
        this.FpsToggle = this.KeyboardState.FpViewToggle;
        this.ClampLook = this.KeyboardState.ClampLook;
      }
      if (this.gamepad)
      {
        PlayerIndex[] players = ControllerIndexExtensions.GetPlayers(ControllerIndex.Any);
        for (int index = 0; index < players.Length; ++index)
        {
          GamepadState gamepadState = this.GamepadsManager[players[index]];
          if (!gamepadState.Connected)
          {
            if (gamepadState.NewlyDisconnected && this.ActiveControllerDisconnected != null)
              this.ActiveControllerDisconnected(players[index]);
          }
          else
          {
            this.ClampLook = FezMath.Coalesce<FezButtonState>(this.ClampLook, gamepadState.RightStick.Clicked.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.FpsToggle = FezMath.Coalesce<FezButtonState>(this.FpsToggle, gamepadState.LeftStick.Clicked.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            Vector2 second1 = Vector2.Clamp(ThumbstickState.CircleToSquare(gamepadState.LeftStick.Position), -Vector2.One, Vector2.One);
            Vector2 second2 = Vector2.Clamp(ThumbstickState.CircleToSquare(gamepadState.RightStick.Position), -Vector2.One, Vector2.One);
            this.Movement = FezMath.Coalesce<Vector2>(this.Movement, second1, gamepadState.DPad.Direction);
            this.FreeLook = FezMath.Coalesce<Vector2>(this.FreeLook, second2);
            this.Back = FezMath.Coalesce<FezButtonState>(this.Back, gamepadState.Back, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.Start = FezMath.Coalesce<FezButtonState>(this.Start, gamepadState.Start, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.Jump = FezMath.Coalesce<FezButtonState>(this.Jump, gamepadState.A.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.GrabThrow = FezMath.Coalesce<FezButtonState>(this.GrabThrow, gamepadState.X.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.CancelTalk = FezMath.Coalesce<FezButtonState>(this.CancelTalk, gamepadState.B.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.OpenInventory = FezMath.Coalesce<FezButtonState>(this.OpenInventory, gamepadState.Y.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.Up = FezMath.Coalesce<FezButtonState>(this.Up, gamepadState.DPad.Up.State, gamepadState.LeftStick.Up.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.Down = FezMath.Coalesce<FezButtonState>(this.Down, gamepadState.DPad.Down.State, gamepadState.LeftStick.Down.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.Left = FezMath.Coalesce<FezButtonState>(this.Left, gamepadState.DPad.Left.State, gamepadState.LeftStick.Left.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.Right = FezMath.Coalesce<FezButtonState>(this.Right, gamepadState.DPad.Right.State, gamepadState.LeftStick.Right.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.ExactUp = FezMath.Coalesce<FezButtonState>(this.ExactUp, gamepadState.ExactUp, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.MapZoomIn = FezMath.Coalesce<FezButtonState>(this.MapZoomIn, gamepadState.RightShoulder.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            this.MapZoomOut = FezMath.Coalesce<FezButtonState>(this.MapZoomOut, gamepadState.LeftShoulder.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            if (this.StrictRotation)
            {
              this.RotateLeft = FezMath.Coalesce<FezButtonState>(this.RotateLeft, gamepadState.LeftTrigger.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
              this.RotateRight = FezMath.Coalesce<FezButtonState>(this.RotateRight, gamepadState.RightTrigger.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            }
            else
            {
              this.RotateLeft = FezMath.Coalesce<FezButtonState>(this.RotateLeft, gamepadState.LeftShoulder.State, gamepadState.LeftTrigger.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
              this.RotateRight = FezMath.Coalesce<FezButtonState>(this.RotateRight, gamepadState.RightShoulder.State, gamepadState.RightTrigger.State, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
            }
          }
        }
      }
      if (!this.mouse)
        return;
      this.MouseState.Update(gameTime);
      Vector2 second = Vector2.Zero;
      switch (this.MouseState.LeftButton.State)
      {
        case MouseButtonStates.DragStarted:
          this.lastMouseCenter = new Vector2((float) this.MouseState.LeftButton.DragState.Movement.X, (float) -this.MouseState.LeftButton.DragState.Movement.Y);
          break;
        case MouseButtonStates.Dragging:
          Vector2 vector2 = new Vector2((float) this.MouseState.LeftButton.DragState.Movement.X, (float) -this.MouseState.LeftButton.DragState.Movement.Y);
          second = (this.lastMouseCenter - vector2) / 32f;
          this.lastMouseCenter = vector2;
          break;
      }
      this.FreeLook = FezMath.Coalesce<Vector2>(this.FreeLook, second);
      this.MapZoomIn = FezMath.Coalesce<FezButtonState>(this.MapZoomIn, this.MouseState.WheelTurnedUp, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
      this.MapZoomOut = FezMath.Coalesce<FezButtonState>(this.MapZoomOut, this.MouseState.WheelTurnedDown, (IEqualityComparer<FezButtonState>) FezButtonStateComparer.Default);
    }

    public void SaveState()
    {
      this.savedStates.Push(new InputManager.State()
      {
        Up = this.Up,
        Down = this.Down,
        Left = this.Left,
        Right = this.Right,
        ExactUp = this.ExactUp,
        Cancel = this.CancelTalk,
        GrabThrow = this.GrabThrow,
        Jump = this.Jump,
        RotateLeft = this.RotateLeft,
        RotateRight = this.RotateRight,
        Start = this.Start,
        Back = this.Back,
        FreeLook = this.FreeLook,
        Movement = this.Movement,
        OpenInventory = this.OpenInventory,
        ClampLook = this.ClampLook,
        FpsToggle = this.FpsToggle,
        MapZoomIn = this.MapZoomIn,
        MapZoomOut = this.MapZoomOut
      });
    }

    public void RecoverState()
    {
      if (this.savedStates.Count == 0)
        return;
      InputManager.State state = this.savedStates.Pop();
      this.Up = state.Up;
      this.Down = state.Down;
      this.Left = state.Left;
      this.Right = state.Right;
      this.ExactUp = state.ExactUp;
      this.CancelTalk = state.Cancel;
      this.GrabThrow = state.GrabThrow;
      this.Jump = state.Jump;
      this.RotateLeft = state.RotateLeft;
      this.RotateRight = state.RotateRight;
      this.Start = state.Start;
      this.Back = state.Back;
      this.FreeLook = state.FreeLook;
      this.Movement = state.Movement;
      this.OpenInventory = state.OpenInventory;
      this.ClampLook = state.ClampLook;
      this.FpsToggle = state.FpsToggle;
      this.MapZoomIn = state.MapZoomIn;
      this.MapZoomOut = state.MapZoomOut;
    }

    public void Reset()
    {
      this.Up = this.Down = this.Left = this.Right = this.ExactUp = FezButtonState.Up;
      InputManager inputManager1 = this;
      InputManager inputManager2 = this;
      Vector2 vector2_1 = new Vector2();
      Vector2 vector2_2;
      Vector2 vector2_3 = vector2_2 = vector2_1;
      inputManager2.FreeLook = vector2_2;
      Vector2 vector2_4 = vector2_3;
      inputManager1.Movement = vector2_4;
      this.CancelTalk = this.GrabThrow = this.Jump = FezButtonState.Up;
      this.Start = this.Back = FezButtonState.Up;
      this.RotateLeft = this.RotateRight = FezButtonState.Up;
      this.OpenInventory = FezButtonState.Up;
      this.ClampLook = this.FpsToggle = FezButtonState.Up;
      this.MapZoomIn = this.MapZoomOut = FezButtonState.Up;
    }

    public void PressedToDown()
    {
      if (this.ExactUp == FezButtonState.Pressed)
        this.ExactUp = FezButtonState.Down;
      if (this.Up == FezButtonState.Pressed)
        this.Up = FezButtonState.Down;
      if (this.Down == FezButtonState.Pressed)
        this.Down = FezButtonState.Down;
      if (this.Left == FezButtonState.Pressed)
        this.Left = FezButtonState.Down;
      if (this.Right == FezButtonState.Pressed)
        this.Right = FezButtonState.Down;
      if (this.CancelTalk == FezButtonState.Pressed)
        this.CancelTalk = FezButtonState.Down;
      if (this.GrabThrow == FezButtonState.Pressed)
        this.GrabThrow = FezButtonState.Down;
      if (this.Jump == FezButtonState.Pressed)
        this.Jump = FezButtonState.Down;
      if (this.Start == FezButtonState.Pressed)
        this.Start = FezButtonState.Down;
      if (this.Back == FezButtonState.Pressed)
        this.Back = FezButtonState.Down;
      if (this.RotateLeft == FezButtonState.Pressed)
        this.RotateLeft = FezButtonState.Down;
      if (this.OpenInventory == FezButtonState.Pressed)
        this.OpenInventory = FezButtonState.Down;
      if (this.RotateRight == FezButtonState.Pressed)
        this.RotateRight = FezButtonState.Down;
      if (this.ClampLook == FezButtonState.Pressed)
        this.ClampLook = FezButtonState.Down;
      if (this.FpsToggle == FezButtonState.Pressed)
        this.FpsToggle = FezButtonState.Down;
      if (this.MapZoomIn == FezButtonState.Pressed)
        this.MapZoomIn = FezButtonState.Down;
      if (this.MapZoomOut != FezButtonState.Pressed)
        return;
      this.MapZoomOut = FezButtonState.Down;
    }

    public void ForceActiveController(ControllerIndex ci)
    {
      this.ActiveControllers = ci;
    }

    public void DetermineActiveController()
    {
      if (!this.gamepad)
        return;
      foreach (PlayerIndex index in ControllerIndexExtensions.GetPlayers(this.ActiveControllers))
      {
        GamepadState gamepadState = this.GamepadsManager[index];
        if (FezButtonStateExtensions.IsDown(gamepadState.Start) || FezButtonStateExtensions.IsDown(gamepadState.A.State) || (FezButtonStateExtensions.IsDown(gamepadState.Back) || FezButtonStateExtensions.IsDown(gamepadState.B.State)))
        {
          this.ActiveControllers = ControllerIndexExtensions.ToControllerIndex(index);
          return;
        }
      }
      this.ActiveControllers = ControllerIndex.None;
    }

    public void ClearActiveController()
    {
      this.ActiveControllers = ControllerIndex.Any;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      foreach (PlayerIndex playerIndex in Util.GetValues<PlayerIndex>())
        GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
    }

    public bool AnyButtonPressed()
    {
      if (this.GrabThrow != FezButtonState.Pressed && this.Jump != FezButtonState.Pressed && (this.OpenInventory != FezButtonState.Pressed && this.Start != FezButtonState.Pressed))
        return this.CancelTalk == FezButtonState.Pressed;
      else
        return true;
    }

    private struct State
    {
      public FezButtonState Up;
      public FezButtonState Down;
      public FezButtonState Left;
      public FezButtonState Right;
      public FezButtonState Cancel;
      public FezButtonState GrabThrow;
      public FezButtonState RotateLeft;
      public FezButtonState RotateRight;
      public FezButtonState Start;
      public FezButtonState Back;
      public FezButtonState Jump;
      public FezButtonState OpenInventory;
      public FezButtonState ExactUp;
      public FezButtonState ClampLook;
      public FezButtonState FpsToggle;
      public FezButtonState MapZoomIn;
      public FezButtonState MapZoomOut;
      public Vector2 Movement;
      public Vector2 FreeLook;
    }
  }
}
