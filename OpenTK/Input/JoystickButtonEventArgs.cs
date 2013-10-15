// Type: OpenTK.Input.JoystickButtonEventArgs
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Input
{
  public class JoystickButtonEventArgs : EventArgs
  {
    private JoystickButton button;
    private bool pressed;

    public JoystickButton Button
    {
      get
      {
        return this.button;
      }
      internal set
      {
        this.button = value;
      }
    }

    public bool Pressed
    {
      get
      {
        return this.pressed;
      }
      internal set
      {
        this.pressed = value;
      }
    }

    internal JoystickButtonEventArgs(JoystickButton button, bool pressed)
    {
      this.button = button;
      this.pressed = pressed;
    }
  }
}
