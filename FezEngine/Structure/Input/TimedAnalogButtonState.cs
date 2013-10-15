// Type: FezEngine.Structure.Input.TimedAnalogButtonState
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using System;

namespace FezEngine.Structure.Input
{
  public struct TimedAnalogButtonState
  {
    private const double TriggerThreshold = 0.5;
    public readonly float Value;
    public readonly FezButtonState State;
    public readonly TimeSpan TimePressed;

    private TimedAnalogButtonState(float value, FezButtonState state, TimeSpan timePressed)
    {
      this.Value = value;
      this.State = state;
      this.TimePressed = timePressed;
    }

    internal TimedAnalogButtonState NextState(float value, TimeSpan elapsed)
    {
      bool pressed = (double) value > 0.5;
      return new TimedAnalogButtonState(value, FezButtonStateExtensions.NextState(this.State, pressed), pressed ? this.TimePressed + elapsed : TimeSpan.Zero);
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }
  }
}
