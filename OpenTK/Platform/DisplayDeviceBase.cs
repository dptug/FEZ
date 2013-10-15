// Type: OpenTK.Platform.DisplayDeviceBase
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using System.Collections.Generic;

namespace OpenTK.Platform
{
  internal abstract class DisplayDeviceBase : IDisplayDeviceDriver
  {
    protected readonly List<DisplayDevice> AvailableDevices = new List<DisplayDevice>();
    protected DisplayDevice Primary;

    public abstract bool TryChangeResolution(DisplayDevice device, DisplayResolution resolution);

    public abstract bool TryRestoreResolution(DisplayDevice device);

    public virtual DisplayDevice GetDisplay(DisplayIndex index)
    {
      if (index == DisplayIndex.Primary)
        return this.Primary;
      if (index >= DisplayIndex.First && index < (DisplayIndex) this.AvailableDevices.Count)
        return this.AvailableDevices[(int) index];
      else
        return (DisplayDevice) null;
    }
  }
}
