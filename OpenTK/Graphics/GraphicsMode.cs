// Type: OpenTK.Graphics.GraphicsMode
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Platform;
using System;

namespace OpenTK.Graphics
{
  public class GraphicsMode : IEquatable<GraphicsMode>
  {
    private IntPtr? index = new IntPtr?();
    private static readonly object SyncRoot = new object();
    private ColorFormat color_format;
    private ColorFormat accumulator_format;
    private int depth;
    private int stencil;
    private int buffers;
    private int samples;
    private bool stereo;
    private static GraphicsMode defaultMode;
    private static IGraphicsMode implementation;

    public IntPtr? Index
    {
      get
      {
        this.LazySelectGraphicsMode();
        return this.index;
      }
      set
      {
        this.index = value;
      }
    }

    public ColorFormat ColorFormat
    {
      get
      {
        this.LazySelectGraphicsMode();
        return this.color_format;
      }
      private set
      {
        this.color_format = value;
      }
    }

    public ColorFormat AccumulatorFormat
    {
      get
      {
        this.LazySelectGraphicsMode();
        return this.accumulator_format;
      }
      private set
      {
        this.accumulator_format = value;
      }
    }

    public int Depth
    {
      get
      {
        this.LazySelectGraphicsMode();
        return this.depth;
      }
      private set
      {
        this.depth = value;
      }
    }

    public int Stencil
    {
      get
      {
        this.LazySelectGraphicsMode();
        return this.stencil;
      }
      private set
      {
        this.stencil = value;
      }
    }

    public int Samples
    {
      get
      {
        this.LazySelectGraphicsMode();
        return this.samples;
      }
      private set
      {
        this.samples = value;
      }
    }

    public bool Stereo
    {
      get
      {
        this.LazySelectGraphicsMode();
        return this.stereo;
      }
      private set
      {
        this.stereo = value;
      }
    }

    public int Buffers
    {
      get
      {
        this.LazySelectGraphicsMode();
        return this.buffers;
      }
      private set
      {
        this.buffers = value;
      }
    }

    public static GraphicsMode Default
    {
      get
      {
        lock (GraphicsMode.SyncRoot)
        {
          if (GraphicsMode.defaultMode == null)
            GraphicsMode.defaultMode = new GraphicsMode((ColorFormat) DisplayDevice.Default.BitsPerPixel, 16, 0, 0, (ColorFormat) 0, 2, false);
          return GraphicsMode.defaultMode;
        }
      }
    }

    static GraphicsMode()
    {
      lock (GraphicsMode.SyncRoot)
        GraphicsMode.implementation = Factory.Default.CreateGraphicsMode();
    }

    internal GraphicsMode(GraphicsMode mode)
      : this(mode.ColorFormat, mode.Depth, mode.Stencil, mode.Samples, mode.AccumulatorFormat, mode.Buffers, mode.Stereo)
    {
    }

    internal GraphicsMode(IntPtr? index, ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
    {
      if (depth < 0)
        throw new ArgumentOutOfRangeException("depth", "Must be greater than, or equal to zero.");
      if (stencil < 0)
        throw new ArgumentOutOfRangeException("stencil", "Must be greater than, or equal to zero.");
      if (buffers <= 0)
        throw new ArgumentOutOfRangeException("buffers", "Must be greater than zero.");
      if (samples < 0)
        throw new ArgumentOutOfRangeException("samples", "Must be greater than, or equal to zero.");
      this.Index = index;
      this.ColorFormat = color;
      this.Depth = depth;
      this.Stencil = stencil;
      this.Samples = samples;
      this.AccumulatorFormat = accum;
      this.Buffers = buffers;
      this.Stereo = stereo;
    }

    public GraphicsMode()
      : this(GraphicsMode.Default)
    {
    }

    public GraphicsMode(ColorFormat color)
      : this(color, GraphicsMode.Default.Depth, GraphicsMode.Default.Stencil, GraphicsMode.Default.Samples, GraphicsMode.Default.AccumulatorFormat, GraphicsMode.Default.Buffers, GraphicsMode.Default.Stereo)
    {
    }

    public GraphicsMode(ColorFormat color, int depth)
      : this(color, depth, GraphicsMode.Default.Stencil, GraphicsMode.Default.Samples, GraphicsMode.Default.AccumulatorFormat, GraphicsMode.Default.Buffers, GraphicsMode.Default.Stereo)
    {
    }

    public GraphicsMode(ColorFormat color, int depth, int stencil)
      : this(color, depth, stencil, GraphicsMode.Default.Samples, GraphicsMode.Default.AccumulatorFormat, GraphicsMode.Default.Buffers, GraphicsMode.Default.Stereo)
    {
    }

    public GraphicsMode(ColorFormat color, int depth, int stencil, int samples)
      : this(color, depth, stencil, samples, GraphicsMode.Default.AccumulatorFormat, GraphicsMode.Default.Buffers, GraphicsMode.Default.Stereo)
    {
    }

    public GraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum)
      : this(color, depth, stencil, samples, accum, GraphicsMode.Default.Buffers, GraphicsMode.Default.Stereo)
    {
    }

    public GraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers)
      : this(color, depth, stencil, samples, accum, buffers, GraphicsMode.Default.Stereo)
    {
    }

    public GraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
      : this(new IntPtr?(), color, depth, stencil, samples, accum, buffers, stereo)
    {
    }

    private void LazySelectGraphicsMode()
    {
      if (this.index.HasValue)
        return;
      GraphicsMode graphicsMode = GraphicsMode.implementation.SelectGraphicsMode(this.color_format, this.depth, this.stencil, this.samples, this.accumulator_format, this.buffers, this.stereo);
      this.Index = graphicsMode.Index;
      this.ColorFormat = graphicsMode.ColorFormat;
      this.Depth = graphicsMode.Depth;
      this.Stencil = graphicsMode.Stencil;
      this.Samples = graphicsMode.Samples;
      this.AccumulatorFormat = graphicsMode.AccumulatorFormat;
      this.Buffers = graphicsMode.Buffers;
      this.Stereo = graphicsMode.Stereo;
    }

    public override string ToString()
    {
      return string.Format("Index: {0}, Color: {1}, Depth: {2}, Stencil: {3}, Samples: {4}, Accum: {5}, Buffers: {6}, Stereo: {7}", (object) this.Index, (object) this.ColorFormat, (object) this.Depth, (object) this.Stencil, (object) this.Samples, (object) this.AccumulatorFormat, (object) this.Buffers, (object) (bool) (this.Stereo ? 1 : 0));
    }

    public override int GetHashCode()
    {
      return this.Index.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is GraphicsMode)
        return this.Equals((GraphicsMode) obj);
      else
        return false;
    }

    public bool Equals(GraphicsMode other)
    {
      if (!this.Index.HasValue)
        return false;
      IntPtr? index1 = this.Index;
      IntPtr? index2 = other.Index;
      if (index1.HasValue != index2.HasValue)
        return false;
      if (index1.HasValue)
        return index1.GetValueOrDefault() == index2.GetValueOrDefault();
      else
        return true;
    }
  }
}
