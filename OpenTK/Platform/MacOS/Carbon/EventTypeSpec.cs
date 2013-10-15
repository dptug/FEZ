// Type: OpenTK.Platform.MacOS.Carbon.EventTypeSpec
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Platform.MacOS.Carbon
{
  internal struct EventTypeSpec
  {
    internal EventClass EventClass;
    internal uint EventKind;

    internal EventTypeSpec(EventClass evtClass, AppEventKind evtKind)
    {
      this.EventClass = evtClass;
      this.EventKind = (uint) evtKind;
    }

    internal EventTypeSpec(EventClass evtClass, AppleEventKind appleKind)
    {
      this.EventClass = evtClass;
      this.EventKind = (uint) appleKind;
    }

    internal EventTypeSpec(EventClass evtClass, MouseEventKind evtKind)
    {
      this.EventClass = evtClass;
      this.EventKind = (uint) evtKind;
    }

    internal EventTypeSpec(EventClass evtClass, KeyboardEventKind evtKind)
    {
      this.EventClass = evtClass;
      this.EventKind = (uint) evtKind;
    }

    internal EventTypeSpec(EventClass evtClass, WindowEventKind evtKind)
    {
      this.EventClass = evtClass;
      this.EventKind = (uint) evtKind;
    }
  }
}
