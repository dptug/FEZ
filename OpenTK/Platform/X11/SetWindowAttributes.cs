// Type: OpenTK.Platform.X11.SetWindowAttributes
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  [Obsolete("Use XSetWindowAttributes instead")]
  [StructLayout(LayoutKind.Sequential)]
  internal class SetWindowAttributes
  {
    public IntPtr background_pixmap;
    public long background_pixel;
    public IntPtr border_pixmap;
    public long border_pixel;
    public int bit_gravity;
    public int win_gravity;
    public int backing_store;
    public long backing_planes;
    public long backing_pixel;
    public bool save_under;
    public EventMask event_mask;
    public long do_not_propagate_mask;
    public bool override_redirect;
    public IntPtr colormap;
    public IntPtr cursor;
  }
}
