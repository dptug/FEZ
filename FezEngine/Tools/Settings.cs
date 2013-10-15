// Type: FezEngine.Tools.Settings
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public class Settings
  {
    public bool UseCurrentMode { get; set; }

    public ScreenMode ScreenMode { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public Language Language { get; set; }

    public float SoundVolume { get; set; }

    public float MusicVolume { get; set; }

    [Serialization(Optional = true)]
    public bool Vibration { get; set; }

    public Dictionary<MappedAction, Keys> KeyboardMapping { get; set; }

    public Dictionary<MappedAction, int> GamepadMapping { get; set; }

    public Settings()
    {
      this.KeyboardMapping = new Dictionary<MappedAction, Keys>();
      this.GamepadMapping = new Dictionary<MappedAction, int>();
      this.RevertToDefaults();
    }

    public void RevertToDefaults()
    {
      this.ScreenMode = ScreenMode.Fullscreen;
      this.Width = 1280;
      this.Height = 720;
      this.Language = Culture.Language;
      this.SoundVolume = 1f;
      this.MusicVolume = 1f;
      this.Vibration = true;
      this.ResetMapping(true, true);
    }

    public void ResetMapping(bool forKeyboard = true, bool forGamepad = true)
    {
      if (forKeyboard)
      {
        this.KeyboardMapping[MappedAction.Jump] = Keys.Space;
        this.KeyboardMapping[MappedAction.GrabThrow] = Keys.LeftControl;
        this.KeyboardMapping[MappedAction.CancelTalk] = Keys.LeftShift;
        this.KeyboardMapping[MappedAction.Up] = Keys.Up;
        this.KeyboardMapping[MappedAction.Down] = Keys.Down;
        this.KeyboardMapping[MappedAction.Left] = Keys.Left;
        this.KeyboardMapping[MappedAction.Right] = Keys.Right;
        this.KeyboardMapping[MappedAction.LookUp] = Keys.I;
        this.KeyboardMapping[MappedAction.LookDown] = Keys.K;
        this.KeyboardMapping[MappedAction.LookRight] = Keys.L;
        this.KeyboardMapping[MappedAction.LookLeft] = Keys.J;
        this.KeyboardMapping[MappedAction.OpenMap] = Keys.Escape;
        this.KeyboardMapping[MappedAction.OpenInventory] = Keys.Tab;
        this.KeyboardMapping[MappedAction.MapZoomIn] = Keys.W;
        this.KeyboardMapping[MappedAction.MapZoomOut] = Keys.S;
        this.KeyboardMapping[MappedAction.Pause] = Keys.Enter;
        this.KeyboardMapping[MappedAction.RotateLeft] = Keys.A;
        this.KeyboardMapping[MappedAction.RotateRight] = Keys.D;
        this.KeyboardMapping[MappedAction.FpViewToggle] = Keys.RightAlt;
        this.KeyboardMapping[MappedAction.ClampLook] = Keys.RightShift;
      }
      if (!forGamepad)
        return;
      this.GamepadMapping[MappedAction.Jump] = 1;
      this.GamepadMapping[MappedAction.GrabThrow] = 0;
      this.GamepadMapping[MappedAction.CancelTalk] = 2;
      this.GamepadMapping[MappedAction.OpenMap] = 8;
      this.GamepadMapping[MappedAction.OpenInventory] = 3;
      this.GamepadMapping[MappedAction.MapZoomIn] = 5;
      this.GamepadMapping[MappedAction.MapZoomOut] = 4;
      this.GamepadMapping[MappedAction.Pause] = 9;
      this.GamepadMapping[MappedAction.RotateLeft] = 6;
      this.GamepadMapping[MappedAction.RotateRight] = 7;
      this.GamepadMapping[MappedAction.FpViewToggle] = 10;
      this.GamepadMapping[MappedAction.ClampLook] = 11;
    }

    public void ApplyGamepadMapping()
    {
      for (PlayerIndex index = PlayerIndex.One; index < (PlayerIndex) SdlGamePad.GetPadCount(); ++index)
      {
        PadConfig config = SdlGamePad.GetConfig(index);
        if (config != null)
        {
          config.Button_A.ID = this.GamepadMapping[MappedAction.Jump];
          config.Button_B.ID = this.GamepadMapping[MappedAction.CancelTalk];
          config.Button_X.ID = this.GamepadMapping[MappedAction.GrabThrow];
          config.Button_Y.ID = this.GamepadMapping[MappedAction.OpenInventory];
          config.Button_LB.ID = this.GamepadMapping[MappedAction.MapZoomOut];
          config.Button_RB.ID = this.GamepadMapping[MappedAction.MapZoomIn];
          config.LeftTrigger.ID = this.GamepadMapping[MappedAction.RotateLeft];
          config.RightTrigger.ID = this.GamepadMapping[MappedAction.RotateRight];
          config.Button_Back.ID = this.GamepadMapping[MappedAction.OpenMap];
          config.Button_Start.ID = this.GamepadMapping[MappedAction.Pause];
          config.LeftStick.Press.ID = this.GamepadMapping[MappedAction.FpViewToggle];
          config.RightStick.Press.ID = this.GamepadMapping[MappedAction.ClampLook];
        }
      }
    }
  }
}
