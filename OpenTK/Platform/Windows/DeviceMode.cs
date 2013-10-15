// Type: OpenTK.Platform.Windows.DeviceMode
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal class DeviceMode
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    internal string DeviceName;
    internal short SpecVersion;
    internal short DriverVersion;
    private short Size;
    internal short DriverExtra;
    internal int Fields;
    internal POINT Position;
    internal int DisplayOrientation;
    internal int DisplayFixedOutput;
    internal short Color;
    internal short Duplex;
    internal short YResolution;
    internal short TTOption;
    internal short Collate;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    internal string FormName;
    internal short LogPixels;
    internal int BitsPerPel;
    internal int PelsWidth;
    internal int PelsHeight;
    internal int DisplayFlags;
    internal int DisplayFrequency;
    internal int ICMMethod;
    internal int ICMIntent;
    internal int MediaType;
    internal int DitherType;
    internal int Reserved1;
    internal int Reserved2;
    internal int PanningWidth;
    internal int PanningHeight;

    internal DeviceMode()
    {
      this.Size = (short) Marshal.SizeOf((object) this);
    }
  }
}
