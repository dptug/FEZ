// Type: OpenTK.Platform.X11.XClientMessageEvent
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XClientMessageEvent
  {
    public XEventName type;
    public IntPtr serial;
    public bool send_event;
    public IntPtr display;
    public IntPtr window;
    public IntPtr message_type;
    public int format;
    public IntPtr ptr1;
    public IntPtr ptr2;
    public IntPtr ptr3;
    public IntPtr ptr4;
    public IntPtr ptr5;
  }
}
