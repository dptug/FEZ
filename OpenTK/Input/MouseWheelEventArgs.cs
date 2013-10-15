// Type: OpenTK.Input.MouseWheelEventArgs
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Input
{
  public class MouseWheelEventArgs : MouseEventArgs
  {
    private float value;
    private float delta;

    public int Value
    {
      get
      {
        return (int) Math.Round((double) this.value, MidpointRounding.AwayFromZero);
      }
    }

    public int Delta
    {
      get
      {
        return (int) Math.Round((double) this.delta, MidpointRounding.AwayFromZero);
      }
    }

    public float ValuePrecise
    {
      get
      {
        return this.value;
      }
      internal set
      {
        this.value = value;
      }
    }

    public float DeltaPrecise
    {
      get
      {
        return this.delta;
      }
      internal set
      {
        this.delta = value;
      }
    }

    public MouseWheelEventArgs()
    {
    }

    public MouseWheelEventArgs(int x, int y, int value, int delta)
      : base(x, y)
    {
      this.value = (float) value;
      this.delta = (float) delta;
    }

    public MouseWheelEventArgs(MouseWheelEventArgs args)
      : this(args.X, args.Y, args.Value, args.Delta)
    {
    }
  }
}
