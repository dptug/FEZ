// Type: OpenTK.Platform.X11.CreateWindowMask
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  [Flags]
  internal enum CreateWindowMask : long
  {
    CWBackPixmap = 1L,
    CWBackPixel = 2L,
    CWSaveUnder = 1024L,
    CWEventMask = 2048L,
    CWDontPropagate = 4096L,
    CWColormap = 8192L,
    CWCursor = 16384L,
    CWBorderPixmap = 4L,
    CWBorderPixel = 8L,
    CWBitGravity = 16L,
    CWWinGravity = 32L,
    CWBackingStore = 64L,
    CWBackingPlanes = 128L,
    CWBackingPixel = 256L,
    CWOverrideRedirect = 512L,
  }
}
