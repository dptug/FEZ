// Type: Microsoft.Xna.Framework.Input.SdlGamePad
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Tao.Sdl;

namespace Microsoft.Xna.Framework.Input
{
  public static class SdlGamePad
  {
    private static IntPtr[] devices = new IntPtr[4];
    private static bool running;
    private static bool sdl;
    private static Settings settings;

    private static Settings Settings
    {
      get
      {
        return SdlGamePad.PrepSettings();
      }
    }

    static SdlGamePad()
    {
    }

    private static void AutoConfig()
    {
      SdlGamePad.Init();
      if (!SdlGamePad.sdl)
        return;
      int num = Math.Min(4, Sdl.SDL_NumJoysticks());
      for (int index = 0; index < num; ++index)
      {
        PadConfig padConfig = new PadConfig(Sdl.SDL_JoystickName(index), index);
        SdlGamePad.devices[index] = Sdl.SDL_JoystickOpen(padConfig.Index);
        padConfig.Button_A.ID = 1;
        padConfig.Button_A.Type = InputType.Button;
        padConfig.Button_B.ID = 2;
        padConfig.Button_B.Type = InputType.Button;
        padConfig.Button_X.ID = 0;
        padConfig.Button_X.Type = InputType.Button;
        padConfig.Button_Y.ID = 3;
        padConfig.Button_Y.Type = InputType.Button;
        padConfig.Button_LB.ID = 4;
        padConfig.Button_LB.Type = InputType.Button;
        padConfig.Button_RB.ID = 5;
        padConfig.Button_RB.Type = InputType.Button;
        padConfig.Button_Back.ID = 8;
        padConfig.Button_Back.Type = InputType.Button;
        padConfig.Button_Start.ID = 9;
        padConfig.Button_Start.Type = InputType.Button;
        padConfig.LeftStick.Press.ID = 10;
        padConfig.LeftStick.Press.Type = InputType.Button;
        padConfig.RightStick.Press.ID = 11;
        padConfig.RightStick.Press.Type = InputType.Button;
        padConfig.LeftStick.X.Negative.ID = 0;
        padConfig.LeftStick.X.Negative.Type = InputType.Axis;
        padConfig.LeftStick.X.Negative.Negative = true;
        padConfig.LeftStick.X.Positive.ID = 0;
        padConfig.LeftStick.X.Positive.Type = InputType.Axis;
        padConfig.LeftStick.X.Positive.Negative = false;
        padConfig.LeftStick.Y.Negative.ID = 1;
        padConfig.LeftStick.Y.Negative.Type = InputType.Axis;
        padConfig.LeftStick.Y.Negative.Negative = true;
        padConfig.LeftStick.Y.Positive.ID = 1;
        padConfig.LeftStick.Y.Positive.Type = InputType.Axis;
        padConfig.LeftStick.Y.Positive.Negative = false;
        padConfig.RightStick.X.Negative.ID = 2;
        padConfig.RightStick.X.Negative.Type = InputType.Axis;
        padConfig.RightStick.X.Negative.Negative = true;
        padConfig.RightStick.X.Positive.ID = 2;
        padConfig.RightStick.X.Positive.Type = InputType.Axis;
        padConfig.RightStick.X.Positive.Negative = false;
        padConfig.RightStick.Y.Negative.ID = 3;
        padConfig.RightStick.Y.Negative.Type = InputType.Axis;
        padConfig.RightStick.Y.Negative.Negative = true;
        padConfig.RightStick.Y.Positive.ID = 3;
        padConfig.RightStick.Y.Positive.Type = InputType.Axis;
        padConfig.RightStick.Y.Positive.Negative = false;
        padConfig.Dpad.Up.ID = 0;
        padConfig.Dpad.Up.Type = InputType.PovUp;
        padConfig.Dpad.Down.ID = 0;
        padConfig.Dpad.Down.Type = InputType.PovDown;
        padConfig.Dpad.Left.ID = 0;
        padConfig.Dpad.Left.Type = InputType.PovLeft;
        padConfig.Dpad.Right.ID = 0;
        padConfig.Dpad.Right.Type = InputType.PovRight;
        padConfig.LeftTrigger.ID = 6;
        padConfig.LeftTrigger.Type = InputType.Button;
        padConfig.RightTrigger.ID = 7;
        padConfig.RightTrigger.Type = InputType.Button;
        SdlGamePad.settings[index] = padConfig;
      }
    }

    private static Settings PrepSettings()
    {
      if (SdlGamePad.settings == null)
      {
        SdlGamePad.settings = new Settings();
        SdlGamePad.AutoConfig();
      }
      else if (!SdlGamePad.running)
      {
        SdlGamePad.Init();
        return SdlGamePad.settings;
      }
      if (!SdlGamePad.running)
        SdlGamePad.Init();
      return SdlGamePad.settings;
    }

    private static void Init()
    {
      SdlGamePad.running = true;
      try
      {
        Joystick.Init();
        SdlGamePad.sdl = true;
      }
      catch (Exception ex)
      {
      }
      for (int index = 0; index < 4; ++index)
      {
        PadConfig padConfig = SdlGamePad.settings[index];
        if (padConfig != null)
          SdlGamePad.devices[index] = Sdl.SDL_JoystickOpen(padConfig.Index);
      }
    }

    public static void Cleanup()
    {
      if (SdlGamePad.running)
      {
        for (int index = 0; index < SdlGamePad.GetPadCount(); ++index)
        {
          IntPtr device = SdlGamePad.GetDevice((PlayerIndex) index);
          if (Sdl.SDL_JoystickOpened(Sdl.SDL_JoystickIndex(device)) == 1)
            Sdl.SDL_JoystickClose(device);
        }
      }
      SdlGamePad.running = false;
    }

    private static IntPtr GetDevice(PlayerIndex index)
    {
      return SdlGamePad.devices[(int) index];
    }

    public static PadConfig GetConfig(PlayerIndex index)
    {
      return SdlGamePad.Settings[(int) index];
    }

    public static int? GetPressedButtonId()
    {
      for (int index = 0; index < SdlGamePad.devices.Length; ++index)
      {
        IntPtr device = SdlGamePad.GetDevice((PlayerIndex) index);
        for (int button = 0; button < Sdl.SDL_JoystickNumButtons(device); ++button)
        {
          if ((int) Sdl.SDL_JoystickGetButton(device, button) > 0)
            return new int?(button);
        }
      }
      return new int?();
    }

    public static int GetPadCount()
    {
      return Enumerable.Count<IntPtr>((IEnumerable<IntPtr>) SdlGamePad.devices, (Func<IntPtr, bool>) (x => x != IntPtr.Zero));
    }

    private static Buttons ReadButtons(IntPtr device, PadConfig c, float deadZoneSize)
    {
      short DeadZone = (short) ((double) deadZoneSize * (double) short.MaxValue);
      Buttons buttons = (Buttons) 0;
      if (c.Button_A.ReadBool(device, DeadZone))
        buttons |= Buttons.A;
      if (c.Button_B.ReadBool(device, DeadZone))
        buttons |= Buttons.B;
      if (c.Button_X.ReadBool(device, DeadZone))
        buttons |= Buttons.X;
      if (c.Button_Y.ReadBool(device, DeadZone))
        buttons |= Buttons.Y;
      if (c.Button_LB.ReadBool(device, DeadZone))
        buttons |= Buttons.LeftShoulder;
      if (c.Button_RB.ReadBool(device, DeadZone))
        buttons |= Buttons.RightShoulder;
      if (c.Button_Back.ReadBool(device, DeadZone))
        buttons |= Buttons.Back;
      if (c.Button_Start.ReadBool(device, DeadZone))
        buttons |= Buttons.Start;
      if (c.LeftStick.Press.ReadBool(device, DeadZone))
        buttons |= Buttons.LeftStick;
      if (c.RightStick.Press.ReadBool(device, DeadZone))
        buttons |= Buttons.RightStick;
      if (c.Dpad.Up.ReadBool(device, DeadZone))
        buttons |= Buttons.DPadUp;
      if (c.Dpad.Down.ReadBool(device, DeadZone))
        buttons |= Buttons.DPadDown;
      if (c.Dpad.Left.ReadBool(device, DeadZone))
        buttons |= Buttons.DPadLeft;
      if (c.Dpad.Right.ReadBool(device, DeadZone))
        buttons |= Buttons.DPadRight;
      return buttons;
    }

    private static Buttons StickToButtons(Vector2 stick, Buttons left, Buttons right, Buttons up, Buttons down, float DeadZoneSize)
    {
      Buttons buttons = (Buttons) 0;
      if ((double) stick.X > (double) DeadZoneSize)
        buttons |= right;
      if ((double) stick.X < -(double) DeadZoneSize)
        buttons |= left;
      if ((double) stick.Y > (double) DeadZoneSize)
        buttons |= up;
      if ((double) stick.Y < -(double) DeadZoneSize)
        buttons |= down;
      return buttons;
    }

    private static Buttons TriggerToButton(float trigger, Buttons button, float DeadZoneSize)
    {
      Buttons buttons = (Buttons) 0;
      if ((double) trigger > (double) DeadZoneSize)
        buttons |= button;
      return buttons;
    }

    private static GamePadState ReadState(PlayerIndex index, GamePadDeadZone deadZone)
    {
      IntPtr device = SdlGamePad.GetDevice(index);
      PadConfig config = SdlGamePad.GetConfig(index);
      if (device == IntPtr.Zero || config == null)
        return GamePadState.InitializedState;
      Vector2 vector2_1 = config.LeftStick.ReadAxisPair(device);
      Vector2 vector2_2 = config.RightStick.ReadAxisPair(device);
      GamePadThumbSticks thumbSticks = new GamePadThumbSticks(new Vector2(vector2_1.X, vector2_1.Y), new Vector2(vector2_2.X, vector2_2.Y));
      thumbSticks.ApplyDeadZone(deadZone, 0.27f);
      GamePadTriggers triggers = new GamePadTriggers(config.LeftTrigger.ReadFloat(device), config.RightTrigger.ReadFloat(device));
      GamePadButtons buttons = new GamePadButtons(SdlGamePad.ReadButtons(device, config, 0.27f) | SdlGamePad.StickToButtons(thumbSticks.Left, Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickRight, Buttons.LeftThumbstickUp, Buttons.LeftThumbstickDown, 0.27f) | SdlGamePad.StickToButtons(thumbSticks.Right, Buttons.RightThumbstickLeft, Buttons.RightThumbstickRight, Buttons.RightThumbstickUp, Buttons.RightThumbstickDown, 0.27f) | SdlGamePad.TriggerToButton(triggers.Left, Buttons.LeftTrigger, 0.27f) | SdlGamePad.TriggerToButton(triggers.Right, Buttons.RightTrigger, 0.27f));
      GamePadDPad dPad = new GamePadDPad(buttons.buttons);
      return new GamePadState(thumbSticks, triggers, buttons, dPad);
    }

    public static GamePadCapabilities GetCapabilities(PlayerIndex playerIndex)
    {
      IntPtr device = SdlGamePad.GetDevice(playerIndex);
      PadConfig config = SdlGamePad.GetConfig(playerIndex);
      if (config == null || (config.JoystickName == null || config.JoystickName == string.Empty) && device == IntPtr.Zero)
        return new GamePadCapabilities();
      return new GamePadCapabilities()
      {
        IsConnected = device != IntPtr.Zero,
        HasAButton = config.Button_A.Type != InputType.None,
        HasBButton = config.Button_B.Type != InputType.None,
        HasXButton = config.Button_X.Type != InputType.None,
        HasYButton = config.Button_Y.Type != InputType.None,
        HasBackButton = config.Button_Back.Type != InputType.None,
        HasStartButton = config.Button_Start.Type != InputType.None,
        HasDPadDownButton = config.Dpad.Down.Type != InputType.None,
        HasDPadLeftButton = config.Dpad.Left.Type != InputType.None,
        HasDPadRightButton = config.Dpad.Right.Type != InputType.None,
        HasDPadUpButton = config.Dpad.Up.Type != InputType.None,
        HasLeftShoulderButton = config.Button_LB.Type != InputType.None,
        HasRightShoulderButton = config.Button_RB.Type != InputType.None,
        HasLeftStickButton = config.LeftStick.Press.Type != InputType.None,
        HasRightStickButton = config.RightStick.Press.Type != InputType.None,
        HasLeftTrigger = config.LeftTrigger.Type != InputType.None,
        HasRightTrigger = config.RightTrigger.Type != InputType.None,
        HasLeftXThumbStick = config.LeftStick.X.Type != InputType.None,
        HasLeftYThumbStick = config.LeftStick.Y.Type != InputType.None,
        HasRightXThumbStick = config.RightStick.X.Type != InputType.None,
        HasRightYThumbStick = config.RightStick.Y.Type != InputType.None,
        HasLeftVibrationMotor = false,
        HasRightVibrationMotor = false,
        HasVoiceSupport = false,
        HasBigButton = false
      };
    }

    public static GamePadState GetState(PlayerIndex playerIndex)
    {
      return SdlGamePad.GetState(playerIndex, GamePadDeadZone.IndependentAxes);
    }

    public static GamePadState GetState(PlayerIndex playerIndex, GamePadDeadZone deadZoneMode)
    {
      SdlGamePad.PrepSettings();
      if (SdlGamePad.sdl)
        Sdl.SDL_JoystickUpdate();
      return SdlGamePad.ReadState(playerIndex, deadZoneMode);
    }

    public static bool SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
    {
      return false;
    }
  }
}
