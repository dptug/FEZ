// Type: OpenTK.Platform.Windows.WinGraphicsMode
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace OpenTK.Platform.Windows
{
  internal class WinGraphicsMode : IGraphicsMode
  {
    private static readonly object SyncRoot = new object();
    private readonly List<GraphicsMode> modes = new List<GraphicsMode>();

    static WinGraphicsMode()
    {
    }

    public WinGraphicsMode()
    {
      lock (WinGraphicsMode.SyncRoot)
      {
        using (INativeWindow resource_0 = (INativeWindow) new NativeWindow())
        {
          this.modes.AddRange(this.GetModesARB(resource_0));
          if (this.modes.Count == 0)
            this.modes.AddRange(this.GetModesPFD(resource_0));
          if (this.modes.Count == 0)
            throw new GraphicsModeException("No GraphicsMode available. This should never happen, please report a bug at http://www.opentk.com");
        }
        this.modes.Sort((IComparer<GraphicsMode>) new GraphicsModeComparer());
      }
    }

    public GraphicsMode SelectGraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
    {
      GraphicsMode graphicsMode;
      do
      {
        graphicsMode = this.modes.Find((Predicate<GraphicsMode>) (current => this.ModeSelector(current, color, depth, stencil, samples, accum, buffers, stereo)));
      }
      while (graphicsMode == null && this.RelaxParameters(ref color, ref depth, ref stencil, ref samples, ref accum, ref buffers, ref stereo));
      if (graphicsMode == null)
        graphicsMode = this.modes[0];
      return graphicsMode;
    }

    private bool RelaxParameters(ref ColorFormat color, ref int depth, ref int stencil, ref int samples, ref ColorFormat accum, ref int buffers, ref bool stereo)
    {
      if (stereo)
      {
        stereo = false;
        return true;
      }
      else if (buffers != 2)
      {
        buffers = 2;
        return true;
      }
      else if (accum != (ColorFormat) 0)
      {
        accum = (ColorFormat) 0;
        return true;
      }
      else if (samples != 0)
      {
        samples = 0;
        return true;
      }
      else if (depth < 16)
      {
        depth = 16;
        return true;
      }
      else if (depth != 24)
      {
        depth = 24;
        return true;
      }
      else if (stencil > 0 && stencil != 8)
      {
        stencil = 8;
        return true;
      }
      else if (stencil == 8)
      {
        stencil = 0;
        return true;
      }
      else if (color < (ColorFormat) 8)
      {
        color = (ColorFormat) 8;
        return true;
      }
      else if (color < (ColorFormat) 16)
      {
        color = (ColorFormat) 16;
        return true;
      }
      else if (color < (ColorFormat) 24)
      {
        color = (ColorFormat) 24;
        return true;
      }
      else
      {
        if (!(color < (ColorFormat) 32) && !(color > (ColorFormat) 32))
          return false;
        color = (ColorFormat) 32;
        return true;
      }
    }

    private static unsafe int DescribePixelFormat(IntPtr hdc, int ipfd, int cjpfd, ref PixelFormatDescriptor pfd)
    {
      fixed (PixelFormatDescriptor* ppfd = &pfd)
        return Wgl.Imports.DescribePixelFormat(hdc, ipfd, (uint) cjpfd, ppfd);
    }

    private IEnumerable<GraphicsMode> GetModesPFD(INativeWindow native)
    {
      WinWindowInfo window = native.WindowInfo as WinWindowInfo;
      IntPtr deviceContext = window.DeviceContext;
      PixelFormatDescriptor pfd = new PixelFormatDescriptor();
      pfd.Size = API.PixelFormatDescriptorSize;
      pfd.Version = API.PixelFormatDescriptorVersion;
      pfd.Flags = PixelFormatDescriptorFlags.DRAW_TO_WINDOW | PixelFormatDescriptorFlags.SUPPORT_OPENGL;
      if (Environment.OSVersion.Version.Major >= 6)
        pfd.Flags |= PixelFormatDescriptorFlags.SUPPORT_COMPOSITION;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E7__wrapc = new bool[2]
      {
        false,
        true
      };
      bool[] flagArray;
      for (int index = 0; index < flagArray.Length; ++index)
      {
        bool generic_allowed = flagArray[index];
        int pixel = 0;
        while (WinGraphicsMode.DescribePixelFormat(deviceContext, ++pixel, (int) API.PixelFormatDescriptorSize, ref pfd) != 0)
        {
          if (generic_allowed || (pfd.Flags & PixelFormatDescriptorFlags.GENERIC_FORMAT) == (PixelFormatDescriptorFlags) 0)
          {
            GraphicsMode fmt = new GraphicsMode(new IntPtr?((IntPtr) pixel), new ColorFormat((int) pfd.RedBits, (int) pfd.GreenBits, (int) pfd.BlueBits, (int) pfd.AlphaBits), (int) pfd.DepthBits, (int) pfd.StencilBits, 0, new ColorFormat((int) pfd.AccumBits), (pfd.Flags & PixelFormatDescriptorFlags.DOUBLEBUFFER) != (PixelFormatDescriptorFlags) 0 ? 2 : 1, (pfd.Flags & PixelFormatDescriptorFlags.STEREO) != (PixelFormatDescriptorFlags) 0);
            yield return fmt;
          }
        }
      }
    }

    private IEnumerable<GraphicsMode> GetModesARB(INativeWindow native)
    {
      using ((IGraphicsContext) new GraphicsContext(new GraphicsMode(new IntPtr?(new IntPtr(2)), new ColorFormat(), 0, 0, 0, new ColorFormat(), 2, false), native.WindowInfo, 1, 0, GraphicsContextFlags.Default))
      {
        WinWindowInfo window = (WinWindowInfo) native.WindowInfo;
        if (Wgl.Delegates.wglChoosePixelFormatARB == null || Wgl.Delegates.wglGetPixelFormatAttribivARB == null)
        {
          // ISSUE: reference to a compiler-generated method
          this.System\u002EIDisposable\u002EDispose();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.\u003Cattribs\u003E5__13 = new int[18]
          {
            8195,
            8213,
            8215,
            8217,
            8219,
            8212,
            8226,
            8227,
            8257,
            8258,
            8222,
            8223,
            8224,
            8225,
            8221,
            8209,
            8210,
            0
          };
          int[] attribs;
          int[] values = new int[attribs.Length];
          // ISSUE: reference to a compiler-generated field
          this.\u003Cattribs_values\u003E5__15 = new int[8]
          {
            8195,
            8231,
            8208,
            1,
            8193,
            1,
            0,
            0
          };
          int[] num_formats = new int[1];
          int[] attribs_values;
          if (Wgl.Arb.ChoosePixelFormat(window.DeviceContext, attribs_values, (float[]) null, 0, (int[]) null, num_formats))
          {
            int[] pixel = new int[num_formats[0]];
            if (Wgl.Arb.ChoosePixelFormat(window.DeviceContext, attribs_values, (float[]) null, pixel.Length, pixel, num_formats))
            {
              foreach (int iPixelFormat in pixel)
              {
                if (Wgl.Arb.GetPixelFormatAttrib(window.DeviceContext, iPixelFormat, 0, attribs.Length - 1, attribs, values))
                {
                  GraphicsMode mode = new GraphicsMode(new IntPtr?(new IntPtr(iPixelFormat)), new ColorFormat(values[1], values[2], values[3], values[4]), values[6], values[7], values[8] != 0 ? values[9] : 0, new ColorFormat(values[10], values[11], values[12], values[13]), values[15] == 1 ? 2 : 1, values[16] == 1);
                  yield return mode;
                }
              }
            }
          }
        }
      }
    }

    private bool ModeSelector(GraphicsMode current, ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
    {
      return (color != ColorFormat.Empty ? (current.ColorFormat >= color ? 1 : 0) : 1) != 0 && (depth != 0 ? (current.Depth >= depth ? 1 : 0) : 1) != 0 && ((stencil != 0 ? (current.Stencil >= stencil ? 1 : 0) : 1) != 0 && (samples != 0 ? (current.Samples >= samples ? 1 : 0) : 1) != 0) && ((accum != ColorFormat.Empty ? (current.AccumulatorFormat >= accum ? 1 : 0) : 1) != 0 && (buffers != 0 ? (current.Buffers >= buffers ? 1 : 0) : 1) != 0) && current.Stereo == stereo;
    }
  }
}
