// Type: OpenTK.Input.JoystickDevice
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Input
{
  public abstract class JoystickDevice : IInputDevice
  {
    private JoystickMoveEventArgs move_args = new JoystickMoveEventArgs(JoystickAxis.Axis0, 0.0f, 0.0f);
    private JoystickButtonEventArgs button_args = new JoystickButtonEventArgs(JoystickButton.Button0, false);
    public EventHandler<JoystickMoveEventArgs> Move = (EventHandler<JoystickMoveEventArgs>) ((sender, e) => {});
    public EventHandler<JoystickButtonEventArgs> ButtonDown = (EventHandler<JoystickButtonEventArgs>) ((sender, e) => {});
    public EventHandler<JoystickButtonEventArgs> ButtonUp = (EventHandler<JoystickButtonEventArgs>) ((sender, e) => {});
    private int id;
    private string description;
    private JoystickAxisCollection axis_collection;
    private JoystickButtonCollection button_collection;

    public JoystickAxisCollection Axis
    {
      get
      {
        return this.axis_collection;
      }
    }

    public JoystickButtonCollection Button
    {
      get
      {
        return this.button_collection;
      }
    }

    public string Description
    {
      get
      {
        return this.description;
      }
      internal set
      {
        this.description = value;
      }
    }

    public InputDeviceType DeviceType
    {
      get
      {
        return InputDeviceType.Hid;
      }
    }

    internal int Id
    {
      get
      {
        return this.id;
      }
      set
      {
        this.id = value;
      }
    }

    internal JoystickDevice(int id, int axes, int buttons)
    {
      if (axes < 0)
        throw new ArgumentOutOfRangeException("axes");
      if (buttons < 0)
        throw new ArgumentOutOfRangeException("buttons");
      this.Id = id;
      this.axis_collection = new JoystickAxisCollection(axes);
      this.button_collection = new JoystickButtonCollection(buttons);
    }

    internal void SetAxis(JoystickAxis axis, float value)
    {
      this.move_args.Axis = axis;
      this.move_args.Delta = this.move_args.Value - value;
      this.axis_collection[axis] = this.move_args.Value = value;
      this.Move((object) this, this.move_args);
    }

    internal void SetButton(JoystickButton button, bool value)
    {
      if (this.button_collection[button] == value)
        return;
      this.button_args.Button = button;
      this.button_collection[button] = this.button_args.Pressed = value;
      if (value)
        this.ButtonDown((object) this, this.button_args);
      else
        this.ButtonUp((object) this, this.button_args);
    }
  }
}
