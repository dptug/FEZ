// Type: OpenTK.Platform.MacOS.Carbon.EventInfo
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.MacOS.Carbon
{
  internal struct EventInfo
  {
    private uint _eventKind;
    private EventClass _eventClass;

    public EventClass EventClass
    {
      get
      {
        return this._eventClass;
      }
    }

    public WindowEventKind WindowEventKind
    {
      get
      {
        if (this.EventClass == EventClass.Window)
          return (WindowEventKind) this._eventKind;
        else
          throw new InvalidCastException("Event is not a Window event.");
      }
    }

    public KeyboardEventKind KeyboardEventKind
    {
      get
      {
        if (this.EventClass == EventClass.Keyboard)
          return (KeyboardEventKind) this._eventKind;
        else
          throw new InvalidCastException("Event is not a Keyboard event.");
      }
    }

    public MouseEventKind MouseEventKind
    {
      get
      {
        if (this.EventClass == EventClass.Mouse)
          return (MouseEventKind) this._eventKind;
        else
          throw new InvalidCastException("Event is not an Mouse event.");
      }
    }

    public AppEventKind AppEventKind
    {
      get
      {
        if (this.EventClass == EventClass.Application)
          return (AppEventKind) this._eventKind;
        else
          throw new InvalidCastException("Event is not an Application event.");
      }
    }

    internal EventInfo(IntPtr eventRef)
    {
      this._eventClass = API.GetEventClass(eventRef);
      this._eventKind = API.GetEventKind(eventRef);
    }

    public override string ToString()
    {
      switch (this.EventClass)
      {
        case EventClass.Mouse:
          return "Event: Mouse " + ((object) this.MouseEventKind).ToString();
        case EventClass.Window:
          return "Event: Window " + ((object) this.WindowEventKind).ToString();
        case EventClass.Application:
          return "Event: App " + ((object) this.AppEventKind).ToString();
        case EventClass.Keyboard:
          return "Event: Keyboard " + ((object) this.KeyboardEventKind).ToString();
        default:
          return "Event: Unknown Class " + ((object) this.EventClass).ToString() + "   kind: " + this._eventKind.ToString();
      }
    }
  }
}
