// Type: OpenTK.Input.JoystickMoveEventArgs
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Input
{
  public class JoystickMoveEventArgs : JoystickEventArgs
  {
    private JoystickAxis axis;
    private float value;
    private float delta;

    public JoystickAxis Axis
    {
      get
      {
        return this.axis;
      }
      internal set
      {
        this.axis = value;
      }
    }

    public float Value
    {
      get
      {
        return this.value;
      }
      internal set
      {
        this.value = value;
      }
    }

    public float Delta
    {
      get
      {
        return this.delta;
      }
      internal set
      {
        this.delta = value;
      }
    }

    public JoystickMoveEventArgs(JoystickAxis axis, float value, float delta)
    {
      this.axis = axis;
      this.value = value;
      this.delta = delta;
    }
  }
}
