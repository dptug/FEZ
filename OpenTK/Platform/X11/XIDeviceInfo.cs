// Type: OpenTK.Platform.X11.XIDeviceInfo
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XIDeviceInfo
  {
    public int deviceid;
    public IntPtr name;
    public int use;
    public int attachment;
    public bool enabled;
    public int num_classes;
    public IntPtr classes;
  }
}
