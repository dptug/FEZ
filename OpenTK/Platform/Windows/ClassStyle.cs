// Type: OpenTK.Platform.Windows.ClassStyle
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum ClassStyle
  {
    VRedraw = 1,
    HRedraw = 2,
    DoubleClicks = 8,
    OwnDC = 32,
    ClassDC = 64,
    ParentDC = 128,
    NoClose = 512,
    SaveBits = 2048,
    ByteAlignClient = 4096,
    ByteAlignWindow = 8192,
    GlobalClass = 16384,
    Ime = 65536,
    DropShadow = 131072,
  }
}
