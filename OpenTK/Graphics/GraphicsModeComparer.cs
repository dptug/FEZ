// Type: OpenTK.Graphics.GraphicsModeComparer
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Collections.Generic;

namespace OpenTK.Graphics
{
  internal sealed class GraphicsModeComparer : IComparer<GraphicsMode>
  {
    public int Compare(GraphicsMode x, GraphicsMode y)
    {
      int num1 = x.ColorFormat.CompareTo(y.ColorFormat);
      if (num1 != 0)
        return num1;
      int num2 = x.Depth.CompareTo(y.Depth);
      if (num2 != 0)
        return num2;
      int num3 = x.Stencil.CompareTo(y.Stencil);
      if (num3 != 0)
        return num3;
      int num4 = x.Samples.CompareTo(y.Samples);
      if (num4 != 0)
        return num4;
      int num5 = x.Stereo.CompareTo(y.Stereo);
      if (num5 != 0)
        return num5;
      int num6 = x.Buffers.CompareTo(y.Buffers);
      if (num6 != 0)
        return num6;
      else
        return x.AccumulatorFormat.CompareTo(y.AccumulatorFormat);
    }
  }
}
