// Type: OpenTK.Platform.X11.XSetWindowAttributes
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XSetWindowAttributes
  {
    public IntPtr background_pixmap;
    public IntPtr background_pixel;
    public IntPtr border_pixmap;
    public IntPtr border_pixel;
    public Gravity bit_gravity;
    public Gravity win_gravity;
    public int backing_store;
    public IntPtr backing_planes;
    public IntPtr backing_pixel;
    public bool save_under;
    public IntPtr event_mask;
    public IntPtr do_not_propagate_mask;
    public bool override_redirect;
    public IntPtr colormap;
    public IntPtr cursor;
  }
}
