// Type: OpenTK.Platform.X11.XScreen
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XScreen
  {
    public IntPtr ext_data;
    public IntPtr display;
    public IntPtr root;
    public int width;
    public int height;
    public int mwidth;
    public int mheight;
    public int ndepths;
    public IntPtr depths;
    public int root_depth;
    public IntPtr root_visual;
    public IntPtr default_gc;
    public IntPtr cmap;
    public IntPtr white_pixel;
    public IntPtr black_pixel;
    public int max_maps;
    public int min_maps;
    public int backing_store;
    public bool save_unders;
    public IntPtr root_input_mask;
  }
}
