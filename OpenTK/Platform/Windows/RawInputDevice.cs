// Type: OpenTK.Platform.Windows.RawInputDevice
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  internal struct RawInputDevice
  {
    internal short UsagePage;
    internal short Usage;
    internal RawInputDeviceFlags Flags;
    internal IntPtr Target;

    public override string ToString()
    {
      return string.Format("{0}/{1}, flags: {2}, window: {3}", (object) this.UsagePage, (object) this.Usage, (object) this.Flags, (object) this.Target);
    }
  }
}
