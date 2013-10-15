// Type: OpenTK.Platform.MacOS.CarbonInput
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace OpenTK.Platform.MacOS
{
  internal class CarbonInput : IInputDriver, IKeyboardDriver, IMouseDriver, IJoystickDriver, IDisposable, IInputDriver2
  {
    private List<KeyboardDevice> dummy_keyboard_list = new List<KeyboardDevice>(1);
    private List<MouseDevice> dummy_mice_list = new List<MouseDevice>(1);
    private List<JoystickDevice> dummy_joystick_list = new List<JoystickDevice>(1);

    public IList<KeyboardDevice> Keyboard
    {
      get
      {
        return (IList<KeyboardDevice>) this.dummy_keyboard_list;
      }
    }

    public IList<MouseDevice> Mouse
    {
      get
      {
        return (IList<MouseDevice>) this.dummy_mice_list;
      }
    }

    public IList<JoystickDevice> Joysticks
    {
      get
      {
        return (IList<JoystickDevice>) this.dummy_joystick_list;
      }
    }

    public IMouseDriver2 MouseDriver
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public IKeyboardDriver2 KeyboardDriver
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public IGamePadDriver GamePadDriver
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    internal CarbonInput()
    {
      this.dummy_mice_list.Add(new MouseDevice());
      this.dummy_keyboard_list.Add(new KeyboardDevice());
      this.dummy_joystick_list.Add((JoystickDevice) new JoystickDevice<object>(0, 0, 0));
    }

    public void Poll()
    {
    }

    public void Dispose()
    {
    }
  }
}
