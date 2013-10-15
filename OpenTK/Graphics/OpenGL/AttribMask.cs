// Type: OpenTK.Graphics.OpenGL.AttribMask
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Graphics.OpenGL
{
  [Flags]
  public enum AttribMask
  {
    CurrentBit = 1,
    PointBit = 2,
    LineBit = 4,
    PolygonBit = 8,
    PolygonStippleBit = 16,
    PixelModeBit = 32,
    LightingBit = 64,
    FogBit = 128,
    DepthBufferBit = 256,
    AccumBufferBit = 512,
    StencilBufferBit = 1024,
    ViewportBit = 2048,
    TransformBit = 4096,
    EnableBit = 8192,
    ColorBufferBit = 16384,
    HintBit = 32768,
    EvalBit = 65536,
    ListBit = 131072,
    TextureBit = 262144,
    ScissorBit = 524288,
    MultisampleBit = 536870912,
    AllAttribBits = -1,
  }
}
