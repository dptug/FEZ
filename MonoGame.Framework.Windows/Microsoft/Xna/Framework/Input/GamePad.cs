// Type: Microsoft.Xna.Framework.Input.GamePad
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using SharpDX.XInput;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Input
{
  public static class GamePad
  {
    private static Controller playerOne = new Controller(UserIndex.One);
    private static Controller playerTwo = new Controller(UserIndex.Two);
    private static Controller playerThree = new Controller(UserIndex.Three);
    private static Controller playerFour = new Controller(UserIndex.Four);
    private static Controller playerAny = new Controller(UserIndex.Any);
    private static readonly List<Tuple<GamepadButtonFlags, Buttons>> buttonMap = new List<Tuple<GamepadButtonFlags, Buttons>>()
    {
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.A, Buttons.A),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.B, Buttons.B),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.Back, Buttons.Back),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.DPadDown, Buttons.DPadDown),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.DPadLeft, Buttons.DPadLeft),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.DPadRight, Buttons.DPadRight),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.DPadUp, Buttons.DPadUp),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.LeftShoulder, Buttons.LeftShoulder),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.RightShoulder, Buttons.RightShoulder),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.LeftThumb, Buttons.LeftStick),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.RightThumb, Buttons.RightStick),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.Start, Buttons.Start),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.X, Buttons.X),
      Tuple.Create<GamepadButtonFlags, Buttons>(GamepadButtonFlags.Y, Buttons.Y)
    };

    static GamePad()
    {
    }

    public static GamePadCapabilities GetCapabilities(PlayerIndex playerIndex)
    {
      Controller controller = GamePad.GetController(playerIndex);
      if (!controller.IsConnected)
        return new GamePadCapabilities();
      Capabilities capabilities = controller.GetCapabilities(DeviceQueryType.Any);
      GamePadCapabilities gamePadCapabilities = new GamePadCapabilities();
      switch (capabilities.SubType)
      {
        case DeviceSubType.Gamepad:
          gamePadCapabilities.GamePadType = GamePadType.GamePad;
          break;
        case DeviceSubType.Wheel:
          gamePadCapabilities.GamePadType = GamePadType.Wheel;
          break;
        case DeviceSubType.ArcadeStick:
          gamePadCapabilities.GamePadType = GamePadType.ArcadeStick;
          break;
        case DeviceSubType.DancePad:
          gamePadCapabilities.GamePadType = GamePadType.DancePad;
          break;
        case DeviceSubType.Guitar:
          gamePadCapabilities.GamePadType = GamePadType.Guitar;
          break;
        case DeviceSubType.DrumKit:
          gamePadCapabilities.GamePadType = GamePadType.DrumKit;
          break;
        default:
          gamePadCapabilities.GamePadType = GamePadType.Unknown;
          break;
      }
      Gamepad gamepad = capabilities.Gamepad;
      GamepadButtonFlags gamepadButtonFlags = gamepad.Buttons;
      gamePadCapabilities.HasAButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.A);
      gamePadCapabilities.HasBackButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.Back);
      gamePadCapabilities.HasBButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.B);
      gamePadCapabilities.HasBigButton = false;
      gamePadCapabilities.HasDPadDownButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.DPadDown);
      gamePadCapabilities.HasDPadLeftButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.DPadLeft);
      gamePadCapabilities.HasDPadRightButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.DPadLeft);
      gamePadCapabilities.HasDPadUpButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.DPadUp);
      gamePadCapabilities.HasLeftShoulderButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.LeftShoulder);
      gamePadCapabilities.HasLeftStickButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.LeftThumb);
      gamePadCapabilities.HasRightShoulderButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.RightShoulder);
      gamePadCapabilities.HasRightStickButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.RightThumb);
      gamePadCapabilities.HasStartButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.Start);
      gamePadCapabilities.HasXButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.X);
      gamePadCapabilities.HasYButton = gamepadButtonFlags.HasFlag((Enum) GamepadButtonFlags.Y);
      gamePadCapabilities.HasRightTrigger = (int) gamepad.LeftTrigger > 0;
      gamePadCapabilities.HasRightXThumbStick = (int) gamepad.RightThumbX != 0;
      gamePadCapabilities.HasRightYThumbStick = (int) gamepad.RightThumbY != 0;
      gamePadCapabilities.HasLeftTrigger = (int) gamepad.LeftTrigger > 0;
      gamePadCapabilities.HasLeftXThumbStick = (int) gamepad.LeftThumbX != 0;
      gamePadCapabilities.HasLeftYThumbStick = (int) gamepad.LeftThumbY != 0;
      gamePadCapabilities.HasLeftVibrationMotor = (int) capabilities.Vibration.LeftMotorSpeed > 0;
      gamePadCapabilities.HasRightVibrationMotor = (int) capabilities.Vibration.RightMotorSpeed > 0;
      gamePadCapabilities.IsConnected = controller.IsConnected;
      gamePadCapabilities.HasVoiceSupport = capabilities.Flags.HasFlag((Enum) CapabilityFlags.VoiceSupported);
      return gamePadCapabilities;
    }

    public static GamePadState GetState(PlayerIndex playerIndex)
    {
      return GamePad.GetState(playerIndex, GamePadDeadZone.IndependentAxes);
    }

    public static GamePadState GetState(PlayerIndex playerIndex, GamePadDeadZone deadZoneMode)
    {
      Controller controller = GamePad.GetController(playerIndex);
      if (!controller.IsConnected)
        return new GamePadState();
      Gamepad gamepad = controller.GetState().Gamepad;
      GamePadThumbSticks gamePadThumbSticks1;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      GamePadThumbSticks& local1 = @gamePadThumbSticks1;
      Vector2 vector2_1 = GamePad.ConvertThumbStick(gamepad.LeftThumbX, gamepad.LeftThumbY, (short) 7849, deadZoneMode);
      Vector2 vector2_2 = GamePad.ConvertThumbStick(gamepad.RightThumbX, gamepad.RightThumbY, (short) 8689, deadZoneMode);
      Vector2 leftPosition = vector2_1;
      Vector2 rightPosition = vector2_2;
      // ISSUE: explicit reference operation
      ^local1 = new GamePadThumbSticks(leftPosition, rightPosition);
      GamePadTriggers gamePadTriggers1;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      GamePadTriggers& local2 = @gamePadTriggers1;
      float num1 = (float) gamepad.LeftTrigger / (float) byte.MaxValue;
      float num2 = (float) gamepad.RightTrigger / (float) byte.MaxValue;
      double num3 = (double) num1;
      double num4 = (double) num2;
      // ISSUE: explicit reference operation
      ^local2 = new GamePadTriggers((float) num3, (float) num4);
      GamePadState gamePadState;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      GamePadState& local3 = @gamePadState;
      GamePadThumbSticks gamePadThumbSticks2 = gamePadThumbSticks1;
      GamePadTriggers gamePadTriggers2 = gamePadTriggers1;
      GamePadButtons gamePadButtons = GamePad.ConvertToButtons(gamepad.Buttons, gamepad.LeftThumbX, gamepad.LeftThumbY, gamepad.RightThumbX, gamepad.RightThumbY, gamepad.LeftTrigger, gamepad.RightTrigger);
      GamePadDPad gamePadDpad = GamePad.ConvertToGamePadDPad(gamepad.Buttons);
      GamePadThumbSticks thumbSticks = gamePadThumbSticks2;
      GamePadTriggers triggers = gamePadTriggers2;
      GamePadButtons buttons = gamePadButtons;
      GamePadDPad dPad = gamePadDpad;
      // ISSUE: explicit reference operation
      ^local3 = new GamePadState(thumbSticks, triggers, buttons, dPad);
      return gamePadState;
    }

    public static void SetVibration(PlayerIndex playerIndex, float leftMotorSpeed, float rightMotorSpeed)
    {
      Controller controller = GamePad.GetController(playerIndex);
      if (!controller.IsConnected)
        return;
      controller.SetVibration(new Vibration()
      {
        LeftMotorSpeed = (short) ((double) leftMotorSpeed * (double) ushort.MaxValue),
        RightMotorSpeed = (short) ((double) rightMotorSpeed * (double) ushort.MaxValue)
      });
    }

    private static Controller GetController(PlayerIndex playerIndex)
    {
      Controller controller;
      switch (playerIndex)
      {
        case PlayerIndex.One:
          controller = GamePad.playerOne;
          break;
        case PlayerIndex.Two:
          controller = GamePad.playerTwo;
          break;
        case PlayerIndex.Three:
          controller = GamePad.playerThree;
          break;
        case PlayerIndex.Four:
          controller = GamePad.playerFour;
          break;
        default:
          controller = GamePad.playerAny;
          break;
      }
      return controller;
    }

    private static Vector2 ConvertThumbStick(short x, short y, short deadZone, GamePadDeadZone deadZoneMode)
    {
      if (deadZoneMode == GamePadDeadZone.IndependentAxes)
      {
        int num1 = (int) x;
        int num2 = (int) y;
        int num3 = (int) deadZone;
        if (num1 * num1 < num3 * num3)
          x = (short) 0;
        if (num2 * num2 < num3 * num3)
          y = (short) 0;
      }
      else if (deadZoneMode == GamePadDeadZone.Circular)
      {
        int num1 = (int) x;
        int num2 = (int) y;
        int num3 = (int) deadZone;
        if (num1 * num1 + num2 * num2 < num3 * num3)
        {
          x = (short) 0;
          y = (short) 0;
        }
      }
      return new Vector2((int) x < 0 ? (float) -((double) x / (double) short.MinValue) : (float) x / (float) short.MaxValue, (int) y < 0 ? (float) -((double) y / (double) short.MinValue) : (float) y / (float) short.MaxValue);
    }

    private static ButtonState ConvertToButtonState(GamepadButtonFlags buttonFlags, GamepadButtonFlags desiredButton)
    {
      return buttonFlags.HasFlag((Enum) desiredButton) ? ButtonState.Pressed : ButtonState.Released;
    }

    private static GamePadDPad ConvertToGamePadDPad(GamepadButtonFlags buttonFlags)
    {
      return new GamePadDPad(GamePad.ConvertToButtonState(buttonFlags, GamepadButtonFlags.DPadUp), GamePad.ConvertToButtonState(buttonFlags, GamepadButtonFlags.DPadDown), GamePad.ConvertToButtonState(buttonFlags, GamepadButtonFlags.DPadLeft), GamePad.ConvertToButtonState(buttonFlags, GamepadButtonFlags.DPadRight));
    }

    private static Buttons AddButtonIfPressed(Buttons originalButtonState, GamepadButtonFlags buttonFlags, GamepadButtonFlags xInputButton, Buttons xnaButton)
    {
      return GamePad.ConvertToButtonState(buttonFlags, xInputButton) == ButtonState.Pressed ? originalButtonState | xnaButton : originalButtonState;
    }

    private static Buttons AddThumbstickButtons(short thumbX, short thumbY, short deadZone, Buttons bitFieldToAddTo, Buttons thumbstickLeft, Buttons thumbStickRight, Buttons thumbStickUp, Buttons thumbStickDown)
    {
      if ((int) thumbX < (int) -deadZone)
        bitFieldToAddTo |= thumbstickLeft;
      if ((int) thumbX > (int) deadZone)
        bitFieldToAddTo |= thumbStickRight;
      if ((int) thumbY < (int) -deadZone)
        bitFieldToAddTo |= thumbStickUp;
      else if ((int) thumbY > (int) deadZone)
        bitFieldToAddTo |= thumbStickDown;
      return bitFieldToAddTo;
    }

    private static GamePadButtons ConvertToButtons(GamepadButtonFlags buttonFlags, short leftThumbX, short leftThumbY, short rightThumbX, short rightThumbY, byte leftTrigger, byte rightTrigger)
    {
      Buttons buttons1 = (Buttons) 0;
      for (int index = 0; index < GamePad.buttonMap.Count; ++index)
      {
        Tuple<GamepadButtonFlags, Buttons> tuple = GamePad.buttonMap[index];
        buttons1 = GamePad.AddButtonIfPressed(buttons1, buttonFlags, tuple.Item1, tuple.Item2);
      }
      Buttons bitFieldToAddTo = GamePad.AddThumbstickButtons(leftThumbX, leftThumbY, (short) 7849, buttons1, Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickRight, Buttons.LeftThumbstickUp, Buttons.LeftThumbstickDown);
      Buttons buttons2 = GamePad.AddThumbstickButtons(rightThumbX, rightThumbY, (short) 8689, bitFieldToAddTo, Buttons.RightThumbstickLeft, Buttons.RightThumbstickRight, Buttons.RightThumbstickUp, Buttons.RightThumbstickDown);
      if ((int) leftTrigger >= 30)
        buttons2 |= Buttons.LeftTrigger;
      if ((int) rightTrigger >= 30)
        buttons2 |= Buttons.RightTrigger;
      return new GamePadButtons(buttons2);
    }
  }
}
