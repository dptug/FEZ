// Type: SharpDX.XInput.Capabilities
// Assembly: SharpDX.XInput, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 7810FD3A-F5EE-4EAB-B451-0D4E18D9FE4F
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.XInput.dll

using System.Runtime.InteropServices;

namespace SharpDX.XInput
{
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public struct Capabilities
  {
    public DeviceType Type;
    public DeviceSubType SubType;
    public CapabilityFlags Flags;
    public Gamepad Gamepad;
    public Vibration Vibration;
  }
}
