// Type: OpenTK.Graphics.ES10.ClearBufferMask
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Graphics.ES10
{
  [Flags]
  public enum ClearBufferMask
  {
    DepthBufferBit = 256,
    StencilBufferBit = 1024,
    ColorBufferBit = 16384,
  }
}
