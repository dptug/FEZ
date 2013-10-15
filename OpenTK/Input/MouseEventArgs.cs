// Type: OpenTK.Input.MouseEventArgs
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Drawing;

namespace OpenTK.Input
{
  public class MouseEventArgs : EventArgs
  {
    private int x;
    private int y;

    public int X
    {
      get
      {
        return this.x;
      }
      internal set
      {
        this.x = value;
      }
    }

    public int Y
    {
      get
      {
        return this.y;
      }
      internal set
      {
        this.y = value;
      }
    }

    public Point Position
    {
      get
      {
        return new Point(this.x, this.y);
      }
    }

    public MouseEventArgs()
    {
    }

    public MouseEventArgs(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    public MouseEventArgs(MouseEventArgs args)
      : this(args.x, args.y)
    {
    }
  }
}
