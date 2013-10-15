// Type: OpenTK.Platform.X11.XVisualInfo
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XVisualInfo
  {
    public IntPtr Visual;
    public IntPtr VisualID;
    public int Screen;
    public int Depth;
    public XVisualClass Class;
    public long RedMask;
    public long GreenMask;
    public long blueMask;
    public int ColormapSize;
    public int BitsPerRgb;

    public override string ToString()
    {
      return string.Format("id ({0}), screen ({1}), depth ({2}), class ({3})", (object) this.VisualID, (object) this.Screen, (object) this.Depth, (object) this.Class);
    }
  }
}
