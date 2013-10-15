// Type: OpenTK.Platform.Windows.ChangeDisplaySettingsEnum
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum ChangeDisplaySettingsEnum
  {
    UpdateRegistry = 1,
    Test = 2,
    Fullscreen = 4,
  }
}
