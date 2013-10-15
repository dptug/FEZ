// Type: OpenTK.Platform.Windows.PixelFormatDescriptorFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum PixelFormatDescriptorFlags
  {
    DOUBLEBUFFER = 1,
    STEREO = 2,
    DRAW_TO_WINDOW = 4,
    DRAW_TO_BITMAP = 8,
    SUPPORT_GDI = 16,
    SUPPORT_OPENGL = 32,
    GENERIC_FORMAT = 64,
    NEED_PALETTE = 128,
    NEED_SYSTEM_PALETTE = 256,
    SWAP_EXCHANGE = 512,
    SWAP_COPY = 1024,
    SWAP_LAYER_BUFFERS = 2048,
    GENERIC_ACCELERATED = 4096,
    SUPPORT_DIRECTDRAW = 8192,
    SUPPORT_COMPOSITION = 32768,
    DEPTH_DONTCARE = 536870912,
    DOUBLEBUFFER_DONTCARE = 1073741824,
    STEREO_DONTCARE = -2147483648,
  }
}
