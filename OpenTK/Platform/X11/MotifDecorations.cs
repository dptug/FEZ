// Type: OpenTK.Platform.X11.MotifDecorations
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  [Flags]
  internal enum MotifDecorations
  {
    All = 1,
    Border = 2,
    ResizeH = 4,
    Title = 8,
    Menu = 16,
    Minimize = 32,
    Maximize = 64,
  }
}
