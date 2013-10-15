// Type: FezEngine.Structure.Input.GamepadState
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FezEngine.Structure.Input
{
  public class GamepadState
  {
    private static readonly TimeSpan ConnectedCheckFrequency = TimeSpan.FromSeconds(1.0);
    private TimeSpan sinceCheckedConnected = GamepadState.ConnectedCheckFrequency;
    private GamepadState.VibrationMotorState leftMotor;
    private GamepadState.VibrationMotorState rightMotor;
    public readonly PlayerIndex PlayerIndex;

    public DirectionalState DPad { get; private set; }

    public ThumbstickState LeftStick { get; private set; }

    public ThumbstickState RightStick { get; private set; }

    public FezButtonState ExactUp { get; private set; }

    public TimedButtonState A { get; private set; }

    public TimedButtonState B { get; private set; }

    public TimedButtonState X { get; private set; }

    public TimedButtonState Y { get; private set; }

    public TimedButtonState RightShoulder { get; private set; }

    public TimedButtonState LeftShoulder { get; private set; }

    public TimedAnalogButtonState RightTrigger { get; private set; }

    public TimedAnalogButtonState LeftTrigger { get; private set; }

    public FezButtonState Start { get; private set; }

    public FezButtonState Back { get; private set; }

    public static bool AnyXInputConnected { get; set; }

    public static bool AnyConnected { get; set; }

    public bool XInputConnected { get; private set; }

    private bool SdlConnected { get; set; }

    public bool Connected
    {
      get
      {
        if (!this.XInputConnected)
          return this.SdlConnected;
        else
          return true;
      }
    }

    public bool NewlyDisconnected { get; set; }

    static GamepadState()
    {
    }

    public GamepadState(PlayerIndex playerIndex)
    {
      this.PlayerIndex = playerIndex;
    }

    public void Update(TimeSpan elapsed)
    {
      this.XInputUpdate(elapsed);
      if (!GamepadState.AnyXInputConnected)
        this.SdlUpdate(elapsed);
      if (!(this.sinceCheckedConnected >= GamepadState.ConnectedCheckFrequency))
        return;
      this.sinceCheckedConnected = TimeSpan.Zero;
    }

    private void XInputUpdate(TimeSpan elapsed)
    {
      this.sinceCheckedConnected += elapsed;
      if (this.sinceCheckedConnected >= GamepadState.ConnectedCheckFrequency)
      {
        bool xinputConnected = this.XInputConnected;
        this.XInputConnected = GamePad.GetState(this.PlayerIndex, GamePadDeadZone.IndependentAxes).IsConnected;
        if (xinputConnected && !this.XInputConnected)
          this.NewlyDisconnected = true;
      }
      if (!this.XInputConnected)
        return;
      GamePadState state;
      try
      {
        state = GamePad.GetState(this.PlayerIndex, GamePadDeadZone.IndependentAxes);
      }
      catch
      {
        return;
      }
      this.XInputConnected = state.IsConnected;
      if (!this.XInputConnected)
        return;
      if (SettingsManager.Settings.Vibration)
      {
        if (this.leftMotor.Active)
          this.leftMotor = GamepadState.UpdateMotor(this.leftMotor, elapsed);
        if (this.rightMotor.Active)
          this.rightMotor = GamepadState.UpdateMotor(this.rightMotor, elapsed);
        if ((double) this.leftMotor.LastAmount != (double) this.leftMotor.CurrentAmount || (double) this.rightMotor.LastAmount != (double) this.rightMotor.CurrentAmount)
          GamePad.SetVibration(this.PlayerIndex, this.leftMotor.CurrentAmount, this.rightMotor.CurrentAmount);
      }
      this.UpdateFromState(state, elapsed);
    }

    private void SdlUpdate(TimeSpan elapsed)
    {
      this.sinceCheckedConnected += elapsed;
      if (this.sinceCheckedConnected >= GamepadState.ConnectedCheckFrequency)
      {
        bool sdlConnected = this.SdlConnected;
        this.SdlConnected = SdlGamePad.GetState(this.PlayerIndex).IsConnected;
        if (sdlConnected && !this.SdlConnected)
          this.NewlyDisconnected = true;
      }
      if (!this.SdlConnected)
        return;
      GamePadState state;
      try
      {
        state = SdlGamePad.GetState(this.PlayerIndex, GamePadDeadZone.IndependentAxes);
      }
      catch
      {
        return;
      }
      this.SdlConnected = state.IsConnected;
      if (!this.SdlConnected)
        return;
      this.UpdateFromState(state, elapsed);
    }

    private void UpdateFromState(GamePadState gamepadState, TimeSpan elapsed)
    {
      this.LeftShoulder = this.LeftShoulder.NextState(gamepadState.Buttons.LeftShoulder == ButtonState.Pressed, elapsed);
      this.RightShoulder = this.RightShoulder.NextState(gamepadState.Buttons.RightShoulder == ButtonState.Pressed, elapsed);
      this.LeftTrigger = this.LeftTrigger.NextState(gamepadState.Triggers.Left, elapsed);
      this.RightTrigger = this.RightTrigger.NextState(gamepadState.Triggers.Right, elapsed);
      this.Start = FezButtonStateExtensions.NextState(this.Start, gamepadState.Buttons.Start == ButtonState.Pressed);
      this.Back = FezButtonStateExtensions.NextState(this.Back, gamepadState.Buttons.Back == ButtonState.Pressed);
      this.A = this.A.NextState(gamepadState.Buttons.A == ButtonState.Pressed, elapsed);
      this.B = this.B.NextState(gamepadState.Buttons.B == ButtonState.Pressed, elapsed);
      this.X = this.X.NextState(gamepadState.Buttons.X == ButtonState.Pressed, elapsed);
      this.Y = this.Y.NextState(gamepadState.Buttons.Y == ButtonState.Pressed, elapsed);
      this.DPad = this.DPad.NextState(gamepadState.DPad.Up == ButtonState.Pressed, gamepadState.DPad.Down == ButtonState.Pressed, gamepadState.DPad.Left == ButtonState.Pressed, gamepadState.DPad.Right == ButtonState.Pressed, elapsed);
      this.LeftStick = this.LeftStick.NextState(gamepadState.ThumbSticks.Left, gamepadState.Buttons.LeftStick == ButtonState.Pressed, elapsed);
      this.RightStick = this.RightStick.NextState(gamepadState.ThumbSticks.Right, gamepadState.Buttons.RightStick == ButtonState.Pressed, elapsed);
      this.ExactUp = FezButtonStateExtensions.NextState(this.ExactUp, (double) this.LeftStick.Position.Y > 0.899999976158142 || FezButtonStateExtensions.IsDown(this.DPad.Up.State) && this.DPad.Left.State == FezButtonState.Up && this.DPad.Right.State == FezButtonState.Up);
    }

    private static GamepadState.VibrationMotorState UpdateMotor(GamepadState.VibrationMotorState motorState, TimeSpan elapsedTime)
    {
      if (motorState.ElapsedTime <= motorState.Duration)
      {
        float num = Easing.EaseIn(1.0 - motorState.ElapsedTime.TotalSeconds / motorState.Duration.TotalSeconds, motorState.EasingType);
        motorState.CurrentAmount = num * motorState.MaximumAmount;
      }
      else
      {
        motorState.CurrentAmount = 0.0f;
        motorState.Active = false;
      }
      motorState.ElapsedTime += elapsedTime;
      return motorState;
    }

    public void Vibrate(VibrationMotor motor, double amount, TimeSpan duration)
    {
      this.Vibrate(motor, amount, duration, EasingType.Linear);
    }

    public void Vibrate(VibrationMotor motor, double amount, TimeSpan duration, EasingType easingType)
    {
      GamepadState.VibrationMotorState vibrationMotorState = new GamepadState.VibrationMotorState(amount, duration, easingType);
      switch (motor)
      {
        case VibrationMotor.LeftLow:
          this.leftMotor = vibrationMotorState;
          break;
        case VibrationMotor.RightHigh:
          this.rightMotor = vibrationMotorState;
          break;
      }
    }

    private struct VibrationMotorState
    {
      public readonly float MaximumAmount;
      public readonly TimeSpan Duration;
      public readonly EasingType EasingType;
      public bool Active;
      public TimeSpan ElapsedTime;
      private float currentAmount;

      public float LastAmount { get; private set; }

      public float CurrentAmount
      {
        get
        {
          return this.currentAmount;
        }
        set
        {
          this.LastAmount = this.currentAmount;
          this.currentAmount = value;
        }
      }

      public VibrationMotorState(double maximumAmount, TimeSpan duration, EasingType easingType)
      {
        this = new GamepadState.VibrationMotorState();
        this.Active = true;
        this.LastAmount = this.CurrentAmount = 0.0f;
        this.ElapsedTime = TimeSpan.Zero;
        this.MaximumAmount = (float) FezMath.Saturate(maximumAmount);
        this.Duration = duration;
        this.EasingType = easingType;
      }
    }
  }
}
