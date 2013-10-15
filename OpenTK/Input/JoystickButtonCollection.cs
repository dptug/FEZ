// Type: OpenTK.Input.JoystickButtonCollection
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Input
{
  public sealed class JoystickButtonCollection
  {
    private bool[] button_state;

    public bool this[int index]
    {
      get
      {
        return this.button_state[index];
      }
      internal set
      {
        this.button_state[index] = value;
      }
    }

    public bool this[JoystickButton button]
    {
      get
      {
        return this.button_state[(int) button];
      }
      internal set
      {
        this.button_state[(int) button] = value;
      }
    }

    public int Count
    {
      get
      {
        return this.button_state.Length;
      }
    }

    internal JoystickButtonCollection(int numButtons)
    {
      if (numButtons < 0)
        throw new ArgumentOutOfRangeException("numButtons");
      this.button_state = new bool[numButtons];
    }
  }
}
