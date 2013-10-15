// Type: FezEngine.Structure.Input.DirectionalState
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Structure.Input
{
  public struct DirectionalState
  {
    public readonly Vector2 Direction;
    public readonly Vector2 Movement;
    public readonly TimedButtonState Up;
    public readonly TimedButtonState Down;
    public readonly TimedButtonState Left;
    public readonly TimedButtonState Right;

    private DirectionalState(Vector2 direction, Vector2 movement, TimedButtonState up, TimedButtonState down, TimedButtonState left, TimedButtonState right)
    {
      this.Direction = direction;
      this.Movement = movement;
      this.Up = up;
      this.Down = down;
      this.Left = left;
      this.Right = right;
    }

    internal DirectionalState NextState(bool up, bool down, bool left, bool right, TimeSpan elapsed)
    {
      Vector2 direction = new Vector2(left ? -1f : (right ? 1f : 0.0f), up ? 1f : (down ? -1f : 0.0f));
      return new DirectionalState(direction, direction - this.Direction, this.Up.NextState(up, elapsed), this.Down.NextState(down, elapsed), this.Left.NextState(left, elapsed), this.Right.NextState(right, elapsed));
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }
  }
}
