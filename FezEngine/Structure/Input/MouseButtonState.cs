// Type: FezEngine.Structure.Input.MouseButtonState
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Structure.Input
{
  public struct MouseButtonState : IEquatable<MouseButtonState>
  {
    private readonly MouseDragState dragState;
    private readonly MouseButtonStates state;

    public MouseButtonStates State
    {
      get
      {
        return this.state;
      }
    }

    public MouseDragState DragState
    {
      get
      {
        return this.dragState;
      }
    }

    internal MouseButtonState(MouseButtonStates state)
    {
      this = new MouseButtonState(state, new MouseDragState());
    }

    internal MouseButtonState(MouseButtonStates state, MouseDragState dragState)
    {
      this.dragState = dragState;
      this.state = state;
    }

    public static bool operator ==(MouseButtonState left, MouseButtonState right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(MouseButtonState left, MouseButtonState right)
    {
      return !left.Equals(right);
    }

    public bool Equals(MouseButtonState other)
    {
      if (object.Equals((object) other.state, (object) this.state))
        return other.dragState.Equals(this.dragState);
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (MouseButtonState))
        return false;
      else
        return this.Equals((MouseButtonState) obj);
    }

    public override int GetHashCode()
    {
      return this.state.GetHashCode() * 397 ^ this.dragState.GetHashCode();
    }
  }
}
