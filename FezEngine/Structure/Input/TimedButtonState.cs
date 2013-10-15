// Type: FezEngine.Structure.Input.TimedButtonState
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using System;

namespace FezEngine.Structure.Input
{
  public struct TimedButtonState : IEquatable<TimedButtonState>
  {
    public readonly FezButtonState State;
    public readonly TimeSpan TimePressed;

    private TimedButtonState(FezButtonState state, TimeSpan timePressed)
    {
      this.State = state;
      this.TimePressed = timePressed;
    }

    internal TimedButtonState NextState(bool down, TimeSpan elapsed)
    {
      return new TimedButtonState(FezButtonStateExtensions.NextState(this.State, down), down ? this.TimePressed + elapsed : TimeSpan.Zero);
    }

    public bool Equals(TimedButtonState other)
    {
      if (other.State == this.State)
        return other.TimePressed == this.TimePressed;
      else
        return false;
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }
  }
}
