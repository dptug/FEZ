// Type: OpenTK.Platform.Windows.RawMouseFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum RawMouseFlags : ushort
  {
    MOUSE_MOVE_RELATIVE = (ushort) 0,
    MOUSE_MOVE_ABSOLUTE = (ushort) 1,
    MOUSE_VIRTUAL_DESKTOP = (ushort) 2,
    MOUSE_ATTRIBUTES_CHANGED = (ushort) 4,
  }
}
