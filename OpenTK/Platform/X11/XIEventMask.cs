// Type: OpenTK.Platform.X11.XIEventMask
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  internal struct XIEventMask : IDisposable
  {
    public int deviceid;
    private int mask_len;
    private unsafe XIEventMasks* mask;

    public unsafe XIEventMask(int id, XIEventMasks m)
    {
      this.deviceid = id;
      this.mask_len = 4;
      this.mask = (XIEventMasks*) (void*) Marshal.AllocHGlobal(this.mask_len);
      *this.mask = m;
    }

    public unsafe void Dispose()
    {
      Marshal.FreeHGlobal(new IntPtr((void*) this.mask));
    }
  }
}
