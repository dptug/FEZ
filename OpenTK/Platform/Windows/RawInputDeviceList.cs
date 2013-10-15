// Type: OpenTK.Platform.Windows.RawInputDeviceList
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  internal struct RawInputDeviceList
  {
    internal IntPtr Device;
    internal RawInputDeviceType Type;

    public override string ToString()
    {
      return string.Format("{0}, Handle: {1}", (object) this.Type, (object) this.Device);
    }
  }
}
