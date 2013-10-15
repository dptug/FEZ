// Type: OpenTK.Platform.X11.XEvent
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  [StructLayout(LayoutKind.Explicit)]
  internal struct XEvent
  {
    [FieldOffset(0)]
    public XEventName type;
    [FieldOffset(0)]
    public XAnyEvent AnyEvent;
    [FieldOffset(0)]
    public XKeyEvent KeyEvent;
    [FieldOffset(0)]
    public XButtonEvent ButtonEvent;
    [FieldOffset(0)]
    public XMotionEvent MotionEvent;
    [FieldOffset(0)]
    public XCrossingEvent CrossingEvent;
    [FieldOffset(0)]
    public XFocusChangeEvent FocusChangeEvent;
    [FieldOffset(0)]
    public XExposeEvent ExposeEvent;
    [FieldOffset(0)]
    public XGraphicsExposeEvent GraphicsExposeEvent;
    [FieldOffset(0)]
    public XNoExposeEvent NoExposeEvent;
    [FieldOffset(0)]
    public XVisibilityEvent VisibilityEvent;
    [FieldOffset(0)]
    public XCreateWindowEvent CreateWindowEvent;
    [FieldOffset(0)]
    public XDestroyWindowEvent DestroyWindowEvent;
    [FieldOffset(0)]
    public XUnmapEvent UnmapEvent;
    [FieldOffset(0)]
    public XMapEvent MapEvent;
    [FieldOffset(0)]
    public XMapRequestEvent MapRequestEvent;
    [FieldOffset(0)]
    public XReparentEvent ReparentEvent;
    [FieldOffset(0)]
    public XConfigureEvent ConfigureEvent;
    [FieldOffset(0)]
    public XGravityEvent GravityEvent;
    [FieldOffset(0)]
    public XResizeRequestEvent ResizeRequestEvent;
    [FieldOffset(0)]
    public XConfigureRequestEvent ConfigureRequestEvent;
    [FieldOffset(0)]
    public XCirculateEvent CirculateEvent;
    [FieldOffset(0)]
    public XCirculateRequestEvent CirculateRequestEvent;
    [FieldOffset(0)]
    public XPropertyEvent PropertyEvent;
    [FieldOffset(0)]
    public XSelectionClearEvent SelectionClearEvent;
    [FieldOffset(0)]
    public XSelectionRequestEvent SelectionRequestEvent;
    [FieldOffset(0)]
    public XSelectionEvent SelectionEvent;
    [FieldOffset(0)]
    public XColormapEvent ColormapEvent;
    [FieldOffset(0)]
    public XClientMessageEvent ClientMessageEvent;
    [FieldOffset(0)]
    public XMappingEvent MappingEvent;
    [FieldOffset(0)]
    public XErrorEvent ErrorEvent;
    [FieldOffset(0)]
    public XKeymapEvent KeymapEvent;
    [FieldOffset(0)]
    public XGenericEvent GenericEvent;
    [FieldOffset(0)]
    public XGenericEventCookie GenericEventCookie;
    [FieldOffset(0)]
    public XEventPad Pad;

    public override string ToString()
    {
      switch (this.type)
      {
        case XEventName.ConfigureNotify:
          return XEvent.ToString((object) this.ConfigureEvent);
        case XEventName.ResizeRequest:
          return XEvent.ToString((object) this.ResizeRequestEvent);
        case XEventName.PropertyNotify:
          return XEvent.ToString((object) this.PropertyEvent);
        default:
          return ((object) this.type).ToString();
      }
    }

    public static string ToString(object ev)
    {
      string str = string.Empty;
      Type type = ev.GetType();
      FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      for (int index = 0; index < fields.Length; ++index)
      {
        if (str != string.Empty)
          str = str + ", ";
        object obj = fields[index].GetValue(ev);
        str = str + fields[index].Name + "=" + (obj == null ? "<null>" : obj.ToString());
      }
      return type.Name + " (" + str + ")";
    }
  }
}
