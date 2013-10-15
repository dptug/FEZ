// Type: OpenTK.Platform.Windows.RawInputData
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  [StructLayout(LayoutKind.Explicit)]
  internal struct RawInputData
  {
    [FieldOffset(0)]
    internal RawMouse Mouse;
    [FieldOffset(0)]
    internal RawKeyboard Keyboard;
    [FieldOffset(0)]
    internal RawHID HID;
  }
}
