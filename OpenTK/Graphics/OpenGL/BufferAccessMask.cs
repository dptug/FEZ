// Type: OpenTK.Graphics.OpenGL.BufferAccessMask
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Graphics.OpenGL
{
  [Flags]
  public enum BufferAccessMask
  {
    MapReadBit = 1,
    MapWriteBit = 2,
    MapInvalidateRangeBit = 4,
    MapInvalidateBufferBit = 8,
    MapFlushExplicitBit = 16,
    MapUnsynchronizedBit = 32,
  }
}
