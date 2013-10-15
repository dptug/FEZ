// Type: OpenTK.Platform.Windows.RawMouse
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  [StructLayout(LayoutKind.Explicit)]
  internal struct RawMouse
  {
    [FieldOffset(0)]
    public RawMouseFlags Flags;
    [FieldOffset(4)]
    public RawInputMouseState ButtonFlags;
    [FieldOffset(6)]
    public ushort ButtonData;
    [FieldOffset(8)]
    public uint RawButtons;
    [FieldOffset(12)]
    public int LastX;
    [FieldOffset(16)]
    public int LastY;
    [FieldOffset(20)]
    public uint ExtraInformation;
  }
}
