// Type: OpenTK.Platform.X11.XWindowAttributes
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XWindowAttributes
  {
    public int x;
    public int y;
    public int width;
    public int height;
    public int border_width;
    public int depth;
    public IntPtr visual;
    public IntPtr root;
    public int c_class;
    public Gravity bit_gravity;
    public Gravity win_gravity;
    public int backing_store;
    public IntPtr backing_planes;
    public IntPtr backing_pixel;
    public bool save_under;
    public IntPtr colormap;
    public bool map_installed;
    public MapState map_state;
    public IntPtr all_event_masks;
    public IntPtr your_event_mask;
    public IntPtr do_not_propagate_mask;
    public bool override_direct;
    public IntPtr screen;

    public override string ToString()
    {
      return XEvent.ToString((object) this);
    }
  }
}
