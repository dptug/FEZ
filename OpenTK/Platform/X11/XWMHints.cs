// Type: OpenTK.Platform.X11.XWMHints
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XWMHints
  {
    public IntPtr flags;
    public bool input;
    public XInitialState initial_state;
    public IntPtr icon_pixmap;
    public IntPtr icon_window;
    public int icon_x;
    public int icon_y;
    public IntPtr icon_mask;
    public IntPtr window_group;
  }
}
