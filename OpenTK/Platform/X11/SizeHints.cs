// Type: OpenTK.Platform.X11.SizeHints
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Platform.X11
{
  internal struct SizeHints
  {
    public long flags;
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
    public SizeHints.Rectangle min_aspect;
    public SizeHints.Rectangle max_aspect;
    public int base_width;
    public int base_height;
    public int win_gravity;

    internal struct Rectangle
    {
      public int x;
      public int y;

      private void stop_the_compiler_warnings()
      {
        this.x = this.y = 0;
      }
    }
  }
}
