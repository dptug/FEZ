// Type: OpenTK.Platform.X11.XSizeHintsFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  [Flags]
  internal enum XSizeHintsFlags
  {
    USPosition = 1,
    USSize = 2,
    PPosition = 4,
    PSize = 8,
    PMinSize = 16,
    PMaxSize = 32,
    PResizeInc = 64,
    PAspect = 128,
    PAllHints = PAspect | PResizeInc | PMaxSize | PMinSize | PSize | PPosition,
    PBaseSize = 256,
    PWinGravity = 512,
  }
}
