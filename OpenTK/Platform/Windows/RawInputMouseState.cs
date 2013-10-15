// Type: OpenTK.Platform.Windows.RawInputMouseState
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum RawInputMouseState : ushort
  {
    LEFT_BUTTON_DOWN = (ushort) 1,
    LEFT_BUTTON_UP = (ushort) 2,
    RIGHT_BUTTON_DOWN = (ushort) 4,
    RIGHT_BUTTON_UP = (ushort) 8,
    MIDDLE_BUTTON_DOWN = (ushort) 16,
    MIDDLE_BUTTON_UP = (ushort) 32,
    BUTTON_1_DOWN = LEFT_BUTTON_DOWN,
    BUTTON_1_UP = LEFT_BUTTON_UP,
    BUTTON_2_DOWN = RIGHT_BUTTON_DOWN,
    BUTTON_2_UP = RIGHT_BUTTON_UP,
    BUTTON_3_DOWN = MIDDLE_BUTTON_DOWN,
    BUTTON_3_UP = MIDDLE_BUTTON_UP,
    BUTTON_4_DOWN = (ushort) 64,
    BUTTON_4_UP = (ushort) 128,
    BUTTON_5_DOWN = (ushort) 256,
    BUTTON_5_UP = (ushort) 512,
    WHEEL = (ushort) 1024,
  }
}
