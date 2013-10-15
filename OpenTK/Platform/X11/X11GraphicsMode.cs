// Type: OpenTK.Platform.X11.X11GraphicsMode
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  internal class X11GraphicsMode : IGraphicsMode
  {
    public GraphicsMode SelectGraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
    {
      IntPtr num1 = IntPtr.Zero;
      IntPtr defaultDisplay = API.DefaultDisplay;
      IntPtr num2 = this.SelectVisualUsingFBConfig(color, depth, stencil, samples, accum, buffers, stereo);
      if (num2 == IntPtr.Zero)
        num2 = this.SelectVisualUsingChooseVisual(color, depth, stencil, samples, accum, buffers, stereo);
      if (num2 == IntPtr.Zero)
        throw new GraphicsModeException("Requested GraphicsMode not available.");
      XVisualInfo vis = (XVisualInfo) Marshal.PtrToStructure(num2, typeof (XVisualInfo));
      int alpha1;
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.ALPHA_SIZE, out alpha1);
      int red1;
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.RED_SIZE, out red1);
      int green1;
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.GREEN_SIZE, out green1);
      int blue1;
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.BLUE_SIZE, out blue1);
      int alpha2;
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.ACCUM_ALPHA_SIZE, out alpha2);
      int red2;
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.ACCUM_RED_SIZE, out red2);
      int green2;
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.ACCUM_GREEN_SIZE, out green2);
      int blue2;
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.ACCUM_BLUE_SIZE, out blue2);
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.DEPTH_SIZE, out depth);
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.STENCIL_SIZE, out stencil);
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.SAMPLES, out samples);
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.DOUBLEBUFFER, out buffers);
      ++buffers;
      int num3;
      Glx.GetConfig(defaultDisplay, ref vis, GLXAttribute.STEREO, out num3);
      stereo = num3 != 0;
      GraphicsMode graphicsMode = new GraphicsMode(new IntPtr?(vis.VisualID), new ColorFormat(red1, green1, blue1, alpha1), depth, stencil, samples, new ColorFormat(red2, green2, blue2, alpha2), buffers, stereo);
      using (new XLock(defaultDisplay))
        Functions.XFree(num2);
      return graphicsMode;
    }

    private unsafe IntPtr SelectVisualUsingFBConfig(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
    {
      List<int> list = new List<int>();
      IntPtr num1 = IntPtr.Zero;
      if (color.BitsPerPixel > 0)
      {
        if (!color.IsIndexed)
        {
          list.Add(4);
          list.Add(1);
        }
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
      {
        list.Add(5);
        list.Add(1);
      }
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
        list.Add(100000);
        list.Add(1);
        list.Add(100001);
        list.Add(samples);
      }
      if (stereo)
      {
        list.Add(6);
        list.Add(1);
      }
      list.Add(0);
      IntPtr defaultDisplay = API.DefaultDisplay;
      using (new XLock(defaultDisplay))
      {
        try
        {
          int num2 = Functions.XDefaultScreen(defaultDisplay);
          Functions.XRootWindow(defaultDisplay, num2);
          int fbount;
          IntPtr* numPtr = Glx.ChooseFBConfig(defaultDisplay, num2, list.ToArray(), out fbount);
          if (fbount > 0)
          {
            if ((IntPtr) numPtr != IntPtr.Zero)
            {
              num1 = Glx.GetVisualFromFBConfig(defaultDisplay, *numPtr);
              Functions.XFree((IntPtr) ((void*) numPtr));
            }
          }
        }
        catch (EntryPointNotFoundException ex)
        {
          return IntPtr.Zero;
        }
      }
      return num1;
    }

    private IntPtr SelectVisualUsingChooseVisual(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
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
        list.Add(100000);
        list.Add(1);
        list.Add(100001);
        list.Add(samples);
      }
      if (stereo)
        list.Add(6);
      list.Add(0);
      IntPtr defaultDisplay = API.DefaultDisplay;
      using (new XLock(defaultDisplay))
        return Glx.ChooseVisual(defaultDisplay, Functions.XDefaultScreen(defaultDisplay), list.ToArray());
    }
  }
}
