// Type: Microsoft.Xna.Framework.Input.GamePadThumbSticks
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Input
{
  public struct GamePadThumbSticks
  {
    public static GamePadThumbSticks.GateType Gate = GamePadThumbSticks.GateType.Round;
    private Vector2 left;
    private Vector2 right;

    public Vector2 Left
    {
      get
      {
        return this.left;
      }
      internal set
      {
        switch (GamePadThumbSticks.Gate)
        {
          case GamePadThumbSticks.GateType.None:
            this.left = value;
            break;
          case GamePadThumbSticks.GateType.Round:
            if ((double) value.LengthSquared() > 1.0)
            {
              this.left = Vector2.Normalize(value);
              break;
            }
            else
            {
              this.left = value;
              break;
            }
          case GamePadThumbSticks.GateType.Square:
            this.left = new Vector2(MathHelper.Clamp(value.X, -1f, 1f), MathHelper.Clamp(value.Y, -1f, 1f));
            break;
          default:
            this.left = Vector2.Zero;
            break;
        }
      }
    }

    public Vector2 Right
    {
      get
      {
        return this.right;
      }
      internal set
      {
        switch (GamePadThumbSticks.Gate)
        {
          case GamePadThumbSticks.GateType.None:
            this.right = value;
            break;
          case GamePadThumbSticks.GateType.Round:
            if ((double) value.LengthSquared() > 1.0)
            {
              this.right = Vector2.Normalize(value);
              break;
            }
            else
            {
              this.right = value;
              break;
            }
          case GamePadThumbSticks.GateType.Square:
            this.right = new Vector2(MathHelper.Clamp(value.X, -1f, 1f), MathHelper.Clamp(value.Y, -1f, 1f));
            break;
          default:
            this.right = Vector2.Zero;
            break;
        }
      }
    }

    static GamePadThumbSticks()
    {
    }

    public GamePadThumbSticks(Vector2 leftPosition, Vector2 rightPosition)
    {
      this = new GamePadThumbSticks();
      this.Left = leftPosition;
      this.Right = rightPosition;
    }

    internal void ApplyDeadZone(GamePadDeadZone dz, float size)
    {
      switch (dz)
      {
        case GamePadDeadZone.IndependentAxes:
          if ((double) Math.Abs(this.left.X) < (double) size)
            this.left.X = 0.0f;
          if ((double) Math.Abs(this.left.Y) < (double) size)
            this.left.Y = 0.0f;
          if ((double) Math.Abs(this.right.X) < (double) size)
            this.right.X = 0.0f;
          if ((double) Math.Abs(this.right.Y) >= (double) size)
            break;
          this.right.Y = 0.0f;
          break;
        case GamePadDeadZone.Circular:
          if ((double) this.left.LengthSquared() < (double) size * (double) size)
            this.left = Vector2.Zero;
          if ((double) this.right.LengthSquared() >= (double) size * (double) size)
            break;
          this.right = Vector2.Zero;
          break;
      }
    }

    public enum GateType
    {
      None,
      Round,
      Square,
    }
  }
}
