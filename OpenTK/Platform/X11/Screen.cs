// Type: OpenTK.Platform.X11.Screen
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct Screen
  {
    private XExtData ext_data;
    private IntPtr display;
    private IntPtr root;
    private int width;
    private int height;
    private int mwidth;
    private int mheight;
    private int ndepths;
    private int root_depth;
    private IntPtr default_gc;
    private IntPtr cmap;
    private UIntPtr white_pixel;
    private UIntPtr black_pixel;
    private int max_maps;
    private int min_maps;
    private int backing_store;
    private bool save_unders;
    private long root_input_mask;
  }
}
