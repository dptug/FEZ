// Type: OpenTK.Platform.X11.GCFunction
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  [Flags]
  internal enum GCFunction
  {
    GCFunction = 1,
    GCPlaneMask = 2,
    GCForeground = 4,
    GCBackground = 8,
    GCLineWidth = 16,
    GCLineStyle = 32,
    GCCapStyle = 64,
    GCJoinStyle = 128,
    GCFillStyle = 256,
    GCFillRule = 512,
    GCTile = 1024,
    GCStipple = 2048,
    GCTileStipXOrigin = 4096,
    GCTileStipYOrigin = 8192,
    GCFont = 16384,
    GCSubwindowMode = 32768,
    GCGraphicsExposures = 65536,
    GCClipXOrigin = 131072,
    GCClipYOrigin = 262144,
    GCClipMask = 524288,
    GCDashOffset = 1048576,
    GCDashList = 2097152,
    GCArcMode = 4194304,
  }
}
