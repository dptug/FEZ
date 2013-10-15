// Type: OpenTK.Platform.MacOS.MacOSGraphicsMode
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace OpenTK.Platform.MacOS
{
  internal class MacOSGraphicsMode : IGraphicsMode
  {
    public GraphicsMode SelectGraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
    {
      return this.GetGraphicsModeFromPixelFormat(this.SelectPixelFormat(color, depth, stencil, samples, accum, buffers, stereo));
    }

    private GraphicsMode GetGraphicsModeFromPixelFormat(IntPtr pixelformat)
    {
      int red1;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_RED_SIZE, out red1);
      int green1;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_GREEN_SIZE, out green1);
      int blue1;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_BLUE_SIZE, out blue1);
      int alpha1;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ALPHA_SIZE, out alpha1);
      int alpha2;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ACCUM_ALPHA_SIZE, out alpha2);
      int red2;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ACCUM_RED_SIZE, out red2);
      int green2;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ACCUM_GREEN_SIZE, out green2);
      int blue2;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ACCUM_BLUE_SIZE, out blue2);
      int depth;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_DEPTH_SIZE, out depth);
      int stencil;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_STENCIL_SIZE, out stencil);
      int samples;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_SAMPLES_ARB, out samples);
      int num1;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_DOUBLEBUFFER, out num1);
      int num2;
      Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_STEREO, out num2);
      return new GraphicsMode(new IntPtr?(pixelformat), new ColorFormat(red1, green1, blue1, alpha1), depth, stencil, samples, new ColorFormat(red2, green2, blue2, alpha2), num1 + 1, num2 != 0);
    }

    private IntPtr SelectPixelFormat(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
    {
      List<int> list = new List<int>();
      if (color.BitsPerPixel > 0)
      {
        if (!color.IsIndexed)
          list.Add(4);
        list.Add(8);
        list.Add(color.Red);
        list.Add(9);
        list.Add(color.Green);
        list.Add(10);
        list.Add(color.Blue);
        list.Add(11);
        list.Add(color.Alpha);
      }
      if (depth > 0)
      {
        list.Add(12);
        list.Add(depth);
      }
      if (buffers > 1)
        list.Add(5);
      if (stencil > 1)
      {
        list.Add(13);
        list.Add(stencil);
      }
      if (accum.BitsPerPixel > 0)
      {
        list.Add(17);
        list.Add(accum.Alpha);
        list.Add(16);
        list.Add(accum.Blue);
        list.Add(15);
        list.Add(accum.Green);
        list.Add(14);
        list.Add(accum.Red);
      }
      if (samples > 0)
      {
        list.Add(55);
        list.Add(1);
        list.Add(56);
        list.Add(samples);
      }
      if (stereo)
        list.Add(6);
      list.Add(0);
      list.Add(0);
      IntPtr num = Agl.aglChoosePixelFormat(IntPtr.Zero, 0, list.ToArray());
      if (num == IntPtr.Zero)
        throw new GraphicsModeException(string.Format("[Error] Failed to select GraphicsMode, error {0}.", (object) Agl.GetError()));
      else
        return num;
    }
  }
}
