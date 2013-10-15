// Type: OpenTK.Platform.X11.XGCValues
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XGCValues
  {
    public GXFunction function;
    public IntPtr plane_mask;
    public IntPtr foreground;
    public IntPtr background;
    public int line_width;
    public GCLineStyle line_style;
    public GCCapStyle cap_style;
    public GCJoinStyle join_style;
    public GCFillStyle fill_style;
    public GCFillRule fill_rule;
    public GCArcMode arc_mode;
    public IntPtr tile;
    public IntPtr stipple;
    public int ts_x_origin;
    public int ts_y_origin;
    public IntPtr font;
    public GCSubwindowMode subwindow_mode;
    public bool graphics_exposures;
    public int clip_x_origin;
    public int clib_y_origin;
    public IntPtr clip_mask;
    public int dash_offset;
    public byte dashes;
  }
}
