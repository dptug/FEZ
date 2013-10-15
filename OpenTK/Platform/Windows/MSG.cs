// Type: OpenTK.Platform.Windows.MSG
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  [CLSCompliant(false)]
  internal struct MSG
  {
    internal IntPtr HWnd;
    internal WindowMessage Message;
    internal IntPtr WParam;
    internal IntPtr LParam;
    internal uint Time;
    internal POINT Point;

    public override string ToString()
    {
      return string.Format("msg=0x{0:x} ({1}) hwnd=0x{2:x} wparam=0x{3:x} lparam=0x{4:x} pt=0x{5:x}", (object) (int) this.Message, (object) ((object) this.Message).ToString(), (object) this.HWnd.ToInt32(), (object) this.WParam.ToInt32(), (object) this.LParam.ToInt32(), (object) this.Point);
    }
  }
}
