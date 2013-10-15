// Type: OpenTK.Platform.Windows.WindowStyle
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum WindowStyle : uint
  {
    Overlapped = 0U,
    Popup = 2147483648U,
    Child = 1073741824U,
    Minimize = 536870912U,
    Visible = 268435456U,
    Disabled = 134217728U,
    ClipSiblings = 67108864U,
    ClipChildren = 33554432U,
    Maximize = 16777216U,
    Caption = 12582912U,
    Border = 8388608U,
    DialogFrame = 4194304U,
    VScroll = 2097152U,
    HScreen = 1048576U,
    SystemMenu = 524288U,
    ThickFrame = 262144U,
    Group = 131072U,
    TabStop = 65536U,
    MinimizeBox = Group,
    MaximizeBox = TabStop,
    Tiled = 0U,
    Iconic = Minimize,
    SizeBox = ThickFrame,
    TiledWindow = SizeBox | MaximizeBox | MinimizeBox | SystemMenu | DialogFrame | Border,
    OverlappedWindow = TiledWindow,
    PopupWindow = SystemMenu | Border | Popup,
    ChildWindow = Child,
  }
}
