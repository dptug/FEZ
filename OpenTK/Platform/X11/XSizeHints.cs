// Type: OpenTK.Platform.X11.XSizeHints
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XSizeHints
  {
    public IntPtr flags;
    public int x;
    public int y;
    public int width;
    public int height;
    public int min_width;
    public int min_height;
    public int max_width;
    public int max_height;
    public int width_inc;
    public int height_inc;
    public int min_aspect_x;
    public int min_aspect_y;
    public int max_aspect_x;
    public int max_aspect_y;
    public int base_width;
    public int base_height;
    public int win_gravity;
  }
}
