// Type: OpenTK.Platform.Windows.RawInputHeader
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  internal struct RawInputHeader
  {
    internal RawInputDeviceType Type;
    internal int Size;
    internal IntPtr Device;
    internal IntPtr Param;
  }
}
