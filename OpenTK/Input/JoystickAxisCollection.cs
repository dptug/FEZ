// Type: OpenTK.Input.JoystickAxisCollection
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Input
{
  public sealed class JoystickAxisCollection
  {
    private float[] axis_state;

    public float this[int index]
    {
      get
      {
        return this.axis_state[index];
      }
      internal set
      {
        this.axis_state[index] = value;
      }
    }

    public float this[JoystickAxis axis]
    {
      get
      {
        return this.axis_state[(int) axis];
      }
      internal set
      {
        this.axis_state[(int) axis] = value;
      }
    }

    public int Count
    {
      get
      {
        return this.axis_state.Length;
      }
    }

    internal JoystickAxisCollection(int numAxes)
    {
      if (numAxes < 0)
        throw new ArgumentOutOfRangeException("numAxes");
      this.axis_state = new float[numAxes];
    }
  }
}
