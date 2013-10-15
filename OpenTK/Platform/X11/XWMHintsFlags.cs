// Type: OpenTK.Platform.X11.XWMHintsFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  [Flags]
  internal enum XWMHintsFlags
  {
    InputHint = 1,
    StateHint = 2,
    IconPixmapHint = 4,
    IconWindowHint = 8,
    IconPositionHint = 16,
    IconMaskHint = 32,
    WindowGroupHint = 64,
    AllHints = WindowGroupHint | IconMaskHint | IconPositionHint | IconWindowHint | IconPixmapHint | StateHint | InputHint,
  }
}
