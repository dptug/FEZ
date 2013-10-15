// Type: OpenTK.Platform.Windows.RawInputDeviceFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum RawInputDeviceFlags
  {
    REMOVE = 1,
    EXCLUDE = 16,
    PAGEONLY = 32,
    NOLEGACY = PAGEONLY | EXCLUDE,
    INPUTSINK = 256,
    CAPTUREMOUSE = 512,
    NOHOTKEYS = CAPTUREMOUSE,
    APPKEYS = 1024,
    EXINPUTSINK = 4096,
    DEVNOTIFY = 8192,
  }
}
