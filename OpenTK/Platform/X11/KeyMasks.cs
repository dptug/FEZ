// Type: OpenTK.Platform.X11.KeyMasks
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  [Flags]
  internal enum KeyMasks
  {
    ShiftMask = 1,
    LockMask = 2,
    ControlMask = 4,
    Mod1Mask = 8,
    Mod2Mask = 16,
    Mod3Mask = 32,
    Mod4Mask = 64,
    Mod5Mask = 128,
    ModMasks = Mod5Mask | Mod4Mask | Mod3Mask | Mod2Mask | Mod1Mask,
  }
}
