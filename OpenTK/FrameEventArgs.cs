// Type: OpenTK.FrameEventArgs
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  public class FrameEventArgs : EventArgs
  {
    private double elapsed;

    public double Time
    {
      get
      {
        return this.elapsed;
      }
      internal set
      {
        if (value <= 0.0)
          throw new ArgumentOutOfRangeException();
        this.elapsed = value;
      }
    }

    public FrameEventArgs()
    {
    }

    public FrameEventArgs(double elapsed)
    {
      this.Time = elapsed;
    }
  }
}
