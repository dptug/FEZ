// Type: OpenTK.Platform.Egl.EglGraphicsMode
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using System;

namespace OpenTK.Platform.Egl
{
  internal class EglGraphicsMode : IGraphicsMode
  {
    public GraphicsMode SelectGraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
    {
      return this.SelectGraphicsMode(color, depth, stencil, samples, accum, buffers, stereo, RenderableFlags.ES);
    }

    public GraphicsMode SelectGraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo, RenderableFlags renderable_flags)
    {
      IntPtr[] configs = new IntPtr[1];
      int[] attrib_list = new int[17]
      {
        12352,
        (int) renderable_flags,
        12324,
        color.Red,
        12323,
        color.Green,
        12322,
        color.Blue,
        12321,
        color.Alpha,
        12325,
        depth > 0 ? depth : 0,
        12326,
        stencil > 0 ? stencil : 0,
        12337,
        samples > 0 ? samples : 0,
        12344
      };
      IntPtr display = Egl.GetDisplay(IntPtr.Zero);
      int major;
      int minor;
      if (!Egl.Initialize(display, out major, out minor))
        throw new GraphicsModeException(string.Format("Failed to initialize display connection, error {0}", (object) Egl.GetError()));
      int num_config;
      if (!Egl.ChooseConfig(display, attrib_list, configs, configs.Length, out num_config) || num_config == 0)
        throw new GraphicsModeException(string.Format("Failed to retrieve GraphicsMode, error {0}", (object) Egl.GetError()));
      IntPtr config = configs[0];
      int red;
      Egl.GetConfigAttrib(display, config, 12324, out red);
      int green;
      Egl.GetConfigAttrib(display, config, 12323, out green);
      int blue;
      Egl.GetConfigAttrib(display, config, 12322, out blue);
      int alpha;
      Egl.GetConfigAttrib(display, config, 12321, out alpha);
      int depth1;
      Egl.GetConfigAttrib(display, config, 12325, out depth1);
      int stencil1;
      Egl.GetConfigAttrib(display, config, 12326, out stencil1);
      int num;
      Egl.GetConfigAttrib(display, config, 12337, out num);
      Egl.GetConfigAttrib(display, config, 12337, out samples);
      return new GraphicsMode(new IntPtr?(config), new ColorFormat(red, green, blue, alpha), depth1, stencil1, num > 0 ? samples : 0, (ColorFormat) 0, 2, false);
    }
  }
}
