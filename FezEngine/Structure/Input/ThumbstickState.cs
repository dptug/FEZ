// Type: FezEngine.Structure.Input.ThumbstickState
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Structure.Input
{
  public struct ThumbstickState
  {
    private const double PressThreshold = 0.5;
    public readonly Vector2 Position;
    public readonly Vector2 Movement;
    public readonly TimedButtonState Clicked;
    public readonly TimedButtonState Up;
    public readonly TimedButtonState Down;
    public readonly TimedButtonState Left;
    public readonly TimedButtonState Right;

    private ThumbstickState(Vector2 position, Vector2 movement, TimedButtonState clicked, TimedButtonState up, TimedButtonState down, TimedButtonState left, TimedButtonState right)
    {
      this.Position = position;
      this.Movement = movement;
      this.Clicked = clicked;
      this.Up = up;
      this.Down = down;
      this.Left = left;
      this.Right = right;
    }

    internal ThumbstickState NextState(Vector2 position, bool clicked, TimeSpan elapsed)
    {
      return new ThumbstickState(position, position - this.Position, this.Clicked.NextState(clicked, elapsed), this.Up.NextState((double) FezMath.Saturate(position.Y) > 0.5, elapsed), this.Down.NextState((double) FezMath.Saturate(-position.Y) > 0.5, elapsed), this.Left.NextState((double) FezMath.Saturate(-position.X) > 0.5, elapsed), this.Right.NextState((double) FezMath.Saturate(position.X) > 0.5, elapsed));
    }

    public override string ToString()
    {
      return Util.ReflectToString((object) this);
    }

    public static Vector2 CircleToSquare(Vector2 point)
    {
      double num = Math.Atan2((double) point.Y, (double) point.X) + 3.14159274101257;
      if (num <= 0.785398185253143 || num > 5.497787296772)
        return point * (float) (1.0 / Math.Cos(num));
      if (num > 0.785398185253143 && num <= 2.35619455575943)
        return point * (float) (1.0 / Math.Sin(num));
      if (num > 2.35619455575943 && num <= 3.92699092626572)
        return point * (float) (-1.0 / Math.Cos(num));
      if (num > 3.92699092626572 && num <= 5.497787296772)
        return point * (float) (-1.0 / Math.Sin(num));
      else
        throw new InvalidOperationException("Invalid angle...?");
    }
  }
}
