// Type: OpenTK.Input.MouseMoveEventArgs
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Input
{
  public class MouseMoveEventArgs : MouseEventArgs
  {
    private int x_delta;
    private int y_delta;

    public int XDelta
    {
      get
      {
        return this.x_delta;
      }
      internal set
      {
        this.x_delta = value;
      }
    }

    public int YDelta
    {
      get
      {
        return this.y_delta;
      }
      internal set
      {
        this.y_delta = value;
      }
    }

    public MouseMoveEventArgs()
    {
    }

    public MouseMoveEventArgs(int x, int y, int xDelta, int yDelta)
      : base(x, y)
    {
      this.XDelta = xDelta;
      this.YDelta = yDelta;
    }

    public MouseMoveEventArgs(MouseMoveEventArgs args)
      : this(args.X, args.Y, args.XDelta, args.YDelta)
    {
    }
  }
}
