// Type: OpenTK.Platform.Windows.DisplayDeviceStateFlags
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [Flags]
  internal enum DisplayDeviceStateFlags
  {
    None = 0,
    AttachedToDesktop = 1,
    MultiDriver = 2,
    PrimaryDevice = 4,
    MirroringDriver = 8,
    VgaCompatible = 16,
    Removable = 32,
    ModesPruned = 134217728,
    Remote = 67108864,
    Disconnect = 33554432,
    Active = AttachedToDesktop,
    Attached = MultiDriver,
  }
}
