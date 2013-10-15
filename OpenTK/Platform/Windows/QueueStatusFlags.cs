// Type: OpenTK.Platform.Windows.QueueStatusFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum QueueStatusFlags
  {
    KEY = 1,
    MOUSEMOVE = 2,
    MOUSEBUTTON = 4,
    POSTMESSAGE = 8,
    TIMER = 16,
    PAINT = 32,
    SENDMESSAGE = 64,
    HOTKEY = 128,
    ALLPOSTMESSAGE = 256,
    RAWINPUT = 1024,
    MOUSE = MOUSEBUTTON | MOUSEMOVE,
    INPUT = MOUSE | RAWINPUT | KEY,
    INPUT_LEGACY = MOUSE | KEY,
    ALLEVENTS = INPUT_LEGACY | RAWINPUT | HOTKEY | PAINT | TIMER | POSTMESSAGE,
    ALLINPUT = ALLEVENTS | SENDMESSAGE,
  }
}
