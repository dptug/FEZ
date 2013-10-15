// Type: OpenTK.Platform.Windows.API
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  internal static class API
  {
    internal static readonly int RawInputHeaderSize = Marshal.SizeOf(typeof (RawInputHeader));
    internal static readonly int RawInputSize = Marshal.SizeOf(typeof (RawInput));
    internal static readonly int RawMouseSize = Marshal.SizeOf(typeof (RawMouse));
    internal static readonly int RawInputDeviceSize = Marshal.SizeOf(typeof (RawInputDevice));
    internal static readonly int RawInputDeviceListSize = Marshal.SizeOf(typeof (RawInputDeviceList));
    internal static readonly int RawInputDeviceInfoSize = Marshal.SizeOf(typeof (RawInputDeviceInfo));
    internal static readonly short PixelFormatDescriptorVersion = (short) 1;
    internal static readonly short PixelFormatDescriptorSize = (short) Marshal.SizeOf(typeof (PixelFormatDescriptor));
    internal static readonly int WindowInfoSize = Marshal.SizeOf(typeof (WindowInfo));

    static API()
    {
    }
  }
}
