// Type: OpenTK.Platform.Windows.RawInputDeviceInfo
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  [StructLayout(LayoutKind.Sequential)]
  internal class RawInputDeviceInfo
  {
    private int Size = Marshal.SizeOf(typeof (RawInputDeviceInfo));
    internal RawInputDeviceType Type;
    internal RawInputDeviceInfo.DeviceStruct Device;

    [StructLayout(LayoutKind.Explicit)]
    internal struct DeviceStruct
    {
      [FieldOffset(0)]
      internal RawInputMouseDeviceInfo Mouse;
      [FieldOffset(0)]
      internal RawInputKeyboardDeviceInfo Keyboard;
      [FieldOffset(0)]
      internal RawInputHIDDeviceInfo HID;
    }
  }
}
