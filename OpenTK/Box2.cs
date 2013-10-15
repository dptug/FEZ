// Type: OpenTK.Box2
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  public struct Box2
  {
    public float Left;
    public float Right;
    public float Top;
    public float Bottom;

    public float Width
    {
      get
      {
        return Math.Abs(this.Right - this.Left);
      }
    }

    public float Height
    {
      get
      {
        return Math.Abs(this.Bottom - this.Top);
      }
    }

    public Box2(Vector2 topLeft, Vector2 bottomRight)
    {
      this.Left = topLeft.X;
      this.Top = topLeft.Y;
      this.Right = bottomRight.X;
      this.Bottom = bottomRight.Y;
    }

    public Box2(float left, float top, float right, float bottom)
    {
      this.Left = left;
      this.Top = top;
      this.Right = right;
      this.Bottom = bottom;
    }

    public static Box2 FromTLRB(float top, float left, float right, float bottom)
    {
      return new Box2(left, top, right, bottom);
    }

    public override string ToString()
    {
      return string.Format("({0},{1})-({2},{3})", (object) this.Left, (object) this.Top, (object) this.Right, (object) this.Bottom);
    }
  }
}
