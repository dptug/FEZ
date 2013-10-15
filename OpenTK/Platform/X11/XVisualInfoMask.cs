// Type: OpenTK.Platform.X11.XVisualInfoMask
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  [Flags]
  internal enum XVisualInfoMask
  {
    No = 0,
    ID = 1,
    Screen = 2,
    Depth = 4,
    Class = 8,
    Red = 16,
    Green = 32,
    Blue = 64,
    ColormapSize = 128,
    BitsPerRGB = 256,
    All = BitsPerRGB | ColormapSize | Blue | Green | Red | Class | Depth | Screen | ID,
  }
}
