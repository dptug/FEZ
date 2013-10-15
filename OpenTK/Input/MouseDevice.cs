// Type: OpenTK.Input.MouseDevice
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Drawing;

namespace OpenTK.Input
{
  public sealed class MouseDevice : IInputDevice
  {
    private readonly bool[] button_state = new bool[Enum.GetValues(typeof (MouseButton)).Length];
    private Point pos = new Point();
    private Point last_pos = new Point();
    private MouseMoveEventArgs move_args = new MouseMoveEventArgs();
    private MouseButtonEventArgs button_args = new MouseButtonEventArgs();
    private MouseWheelEventArgs wheel_args = new MouseWheelEventArgs();
    private Point pos_last_accessed = new Point();
    private string description;
    private IntPtr id;
    private int numButtons;
    private int numWheels;
    private float wheel;
    private float last_wheel;
    private int wheel_last_accessed;

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
        return InputDeviceType.Mouse;
      }
    }

    public int NumberOfButtons
    {
      get
      {
        return this.numButtons;
      }
      internal set
      {
        this.numButtons = value;
      }
    }

    public int NumberOfWheels
    {
      get
      {
        return this.numWheels;
      }
      internal set
      {
        this.numWheels = value;
      }
    }

    public IntPtr DeviceID
    {
      get
      {
        return this.id;
      }
      internal set
      {
        this.id = value;
      }
    }

    public int Wheel
    {
      get
      {
        return (int) Math.Round((double) this.wheel, MidpointRounding.AwayFromZero);
      }
      internal set
      {
        this.WheelPrecise = (float) value;
      }
    }

    public float WheelPrecise
    {
      get
      {
        return this.wheel;
      }
      internal set
      {
        this.wheel = value;
        this.wheel_args.X = this.pos.X;
        this.wheel_args.Y = this.pos.Y;
        this.wheel_args.ValuePrecise = this.wheel;
        this.wheel_args.DeltaPrecise = this.wheel - this.last_wheel;
        this.WheelChanged((object) this, this.wheel_args);
        this.last_wheel = this.wheel;
      }
    }

    public int X
    {
      get
      {
        return this.pos.X;
      }
    }

    public int Y
    {
      get
      {
        return this.pos.Y;
      }
    }

    public bool this[MouseButton button]
    {
      get
      {
        return this.button_state[(int) button];
      }
      internal set
      {
        bool flag = this.button_state[(int) button];
        this.button_state[(int) button] = value;
        this.button_args.X = this.pos.X;
        this.button_args.Y = this.pos.Y;
        this.button_args.Button = button;
        this.button_args.IsPressed = value;
        if (value && !flag)
        {
          this.ButtonDown((object) this, this.button_args);
        }
        else
        {
          if (value || !flag)
            return;
          this.ButtonUp((object) this, this.button_args);
        }
      }
    }

    internal Point Position
    {
      set
      {
        this.pos = value;
        this.move_args.X = this.pos.X;
        this.move_args.Y = this.pos.Y;
        this.move_args.XDelta = this.pos.X - this.last_pos.X;
        this.move_args.YDelta = this.pos.Y - this.last_pos.Y;
        this.Move((object) this, this.move_args);
        this.last_pos = this.pos;
      }
    }

    [Obsolete("WheelDelta is only defined for a single WheelChanged event.  Use the OpenTK.Input.MouseWheelEventArgs::Delta property with the OpenTK.Input.MouseDevice::WheelChanged event.", false)]
    public int WheelDelta
    {
      get
      {
        int num = (int) Math.Round((double) this.wheel - (double) this.wheel_last_accessed, MidpointRounding.AwayFromZero);
        this.wheel_last_accessed = (int) this.wheel;
        return num;
      }
    }

    [Obsolete("XDelta is only defined for a single Move event.  Use the OpenTK.Input.MouseMoveEventArgs::Delta property with the OpenTK.Input.MouseDevice::Move event.", false)]
    public int XDelta
    {
      get
      {
        int num = this.pos.X - this.pos_last_accessed.X;
        this.pos_last_accessed.X = this.pos.X;
        return num;
      }
    }

    [Obsolete("YDelta is only defined for a single Move event.  Use the OpenTK.Input.MouseMoveEventArgs::Delta property with the OpenTK.Input.MouseDevice::Move event.", false)]
    public int YDelta
    {
      get
      {
        int num = this.pos.Y - this.pos_last_accessed.Y;
        this.pos_last_accessed.Y = this.pos.Y;
        return num;
      }
    }

    public event EventHandler<MouseMoveEventArgs> Move = delegate {};

    public event EventHandler<MouseButtonEventArgs> ButtonDown = delegate {};

    public event EventHandler<MouseButtonEventArgs> ButtonUp = delegate {};

    public event EventHandler<MouseWheelEventArgs> WheelChanged = delegate {};

    public override int GetHashCode()
    {
      return this.numButtons ^ this.numWheels ^ this.id.GetHashCode() ^ this.description.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("ID: {0} ({1}). Buttons: {2}, Wheels: {3}", (object) this.DeviceID, (object) this.Description, (object) this.NumberOfButtons, (object) this.NumberOfWheels);
    }
  }
}
