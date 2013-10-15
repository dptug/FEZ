// Type: OpenTK.Input.MouseButtonEventArgs
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Input
{
  public class MouseButtonEventArgs : MouseEventArgs
  {
    private MouseButton button;
    private bool pressed;

    public MouseButton Button
    {
      get
      {
        return this.button;
      }
      internal set
      {
        this.button = value;
      }
    }

    public bool IsPressed
    {
      get
      {
        return this.pressed;
      }
      internal set
      {
        this.pressed = value;
      }
    }

    public MouseButtonEventArgs()
    {
    }

    public MouseButtonEventArgs(int x, int y, MouseButton button, bool pressed)
      : base(x, y)
    {
      this.button = button;
      this.pressed = pressed;
    }

    public MouseButtonEventArgs(MouseButtonEventArgs args)
      : this(args.X, args.Y, args.Button, args.IsPressed)
    {
    }
  }
}
