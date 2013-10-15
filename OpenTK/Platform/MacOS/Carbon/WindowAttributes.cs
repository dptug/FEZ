// Type: OpenTK.Platform.MacOS.Carbon.WindowAttributes
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.MacOS.Carbon
{
  [Flags]
  internal enum WindowAttributes : uint
  {
    NoAttributes = 0U,
    CloseBox = 1U,
    HorizontalZoom = 2U,
    VerticalZoom = 4U,
    FullZoom = VerticalZoom | HorizontalZoom,
    CollapseBox = 8U,
    Resizable = 16U,
    SideTitlebar = 32U,
    NoUpdates = 65536U,
    NoActivates = 131072U,
    NoBuffering = 1048576U,
    StandardHandler = 33554432U,
    InWindowMenu = 134217728U,
    LiveResize = 268435456U,
    StandardDocument = Resizable | CollapseBox | FullZoom | CloseBox,
    StandardFloating = CollapseBox | CloseBox,
  }
}
