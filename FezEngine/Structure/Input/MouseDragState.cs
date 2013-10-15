// Type: FezEngine.Structure.Input.MouseDragState
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Structure.Input
{
  public struct MouseDragState : IEquatable<MouseDragState>
  {
    private readonly Point start;
    private readonly Point movement;
    private readonly bool preDrag;

    public Point Start
    {
      get
      {
        return this.start;
      }
    }

    public Point Movement
    {
      get
      {
        return this.movement;
      }
    }

    internal bool PreDrag
    {
      get
      {
        return this.preDrag;
      }
    }

    internal MouseDragState(Point start, Point current)
    {
      this = new MouseDragState(start, current, false);
    }

    internal MouseDragState(Point start, Point current, bool preDrag)
    {
      this.start = start;
      this.preDrag = preDrag;
      this.movement = new Point(current.X - start.X, current.Y - start.Y);
    }

    public static bool operator ==(MouseDragState left, MouseDragState right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(MouseDragState left, MouseDragState right)
    {
      return !left.Equals(right);
    }

    public bool Equals(MouseDragState other)
    {
      if (other.start.Equals(this.start) && other.movement.Equals(this.movement))
        return other.preDrag.Equals(this.preDrag);
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || obj.GetType() != typeof (MouseDragState))
        return false;
      else
        return this.Equals((MouseDragState) obj);
    }

    public override int GetHashCode()
    {
      return (this.start.GetHashCode() * 397 ^ this.movement.GetHashCode()) * 397 ^ this.preDrag.GetHashCode();
    }
  }
}
