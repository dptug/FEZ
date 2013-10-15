// Type: OpenTK.Platform.X11.XIDeviceEvent
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XIDeviceEvent
  {
    public int type;
    public IntPtr serial;
    public bool send_event;
    public IntPtr display;
    public int extension;
    public XIEventType evtype;
    public IntPtr time;
    public int deviceid;
    public int sourceid;
    public int detail;
    public IntPtr root;
    public IntPtr @event;
    public IntPtr child;
    public double root_x;
    public double root_y;
    public double event_x;
    public double event_y;
    public int flags;
    public XIButtonState buttons;
    public XIValuatorState valuators;
    public XIModifierState mods;
    public XIGroupState group;
  }
}
