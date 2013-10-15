// Type: OpenTK.Platform.Windows.ExtendedWindowStyle
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum ExtendedWindowStyle : uint
  {
    DialogModalFrame = 1U,
    NoParentNotify = 4U,
    Topmost = 8U,
    AcceptFiles = 16U,
    Transparent = 32U,
    MdiChild = 64U,
    ToolWindow = 128U,
    WindowEdge = 256U,
    ClientEdge = 512U,
    ContextHelp = 1024U,
    Right = 4096U,
    Left = 0U,
    RightToLeftReading = 8192U,
    LeftToRightReading = 0U,
    LeftScrollbar = 16384U,
    RightScrollbar = 0U,
    ControlParent = 65536U,
    StaticEdge = 131072U,
    ApplicationWindow = 262144U,
    OverlappedWindow = ClientEdge | WindowEdge,
    PaletteWindow = WindowEdge | ToolWindow | Topmost,
    Layered = 524288U,
    NoInheritLayout = 1048576U,
    RightToLeftLayout = 4194304U,
    Composited = 33554432U,
    NoActivate = 134217728U,
  }
}
