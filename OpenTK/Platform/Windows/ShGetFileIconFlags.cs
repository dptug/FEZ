// Type: OpenTK.Platform.Windows.ShGetFileIconFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum ShGetFileIconFlags
  {
    Icon = 256,
    DisplayName = 512,
    TypeName = 1024,
    Attributes = 2048,
    IconLocation = 4096,
    ExeType = 8192,
    SysIconIndex = 16384,
    LinkOverlay = 32768,
    Selected = 65536,
    Attr_Specified = 131072,
    LargeIcon = 0,
    SmallIcon = 1,
    OpenIcon = 2,
    ShellIconSize = 4,
    PIDL = 8,
    UseFileAttributes = 16,
    AddOverlays = 32,
    OverlayIndex = 64,
  }
}
