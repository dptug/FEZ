// Type: OpenTK.Platform.X11.XErrorEvent
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XErrorEvent
  {
    public XEventName type;
    public IntPtr display;
    public IntPtr resourceid;
    public IntPtr serial;
    public byte error_code;
    public XRequest request_code;
    public byte minor_code;
  }
}
