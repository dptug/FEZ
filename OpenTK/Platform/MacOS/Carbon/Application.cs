// Type: OpenTK.Platform.MacOS.Carbon.Application
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform.MacOS;
using System;

namespace OpenTK.Platform.MacOS.Carbon
{
  internal static class Application
  {
    private static bool mInitialized = false;
    private static IntPtr uppHandler;
    private static CarbonGLNative eventHandler;
    private static int osMajor;
    private static int osMinor;
    private static int osBugfix;

    internal static CarbonGLNative WindowEventHandler
    {
      get
      {
        return Application.eventHandler;
      }
      set
      {
        Application.eventHandler = value;
      }
    }

    static Application()
    {
      Application.Initialize();
    }

    internal static void Initialize()
    {
      if (Application.mInitialized)
        return;
      API.AcquireRootMenu();
      Application.ConnectEvents();
      int num1 = (int) API.Gestalt(GestaltSelector.SystemVersionMajor, out Application.osMajor);
      int num2 = (int) API.Gestalt(GestaltSelector.SystemVersionMinor, out Application.osMinor);
      int num3 = (int) API.Gestalt(GestaltSelector.SystemVersionBugFix, out Application.osBugfix);
      Application.TransformProcessToForeground();
    }

    private static void TransformProcessToForeground()
    {
      ProcessSerialNumber psn = new ProcessSerialNumber();
      API.GetCurrentProcess(ref psn);
      API.TransformProcessType(ref psn, ProcessApplicationTransformState.kProcessTransformToForegroundApplication);
      API.SetFrontProcess(ref psn);
    }

    private static void ConnectEvents()
    {
      EventTypeSpec[] eventTypes = new EventTypeSpec[15]
      {
        new EventTypeSpec(EventClass.Application, AppEventKind.AppActivated),
        new EventTypeSpec(EventClass.Application, AppEventKind.AppDeactivated),
        new EventTypeSpec(EventClass.Application, AppEventKind.AppQuit),
        new EventTypeSpec(EventClass.Mouse, MouseEventKind.MouseDown),
        new EventTypeSpec(EventClass.Mouse, MouseEventKind.MouseUp),
        new EventTypeSpec(EventClass.Mouse, MouseEventKind.MouseMoved),
        new EventTypeSpec(EventClass.Mouse, MouseEventKind.MouseDragged),
        new EventTypeSpec(EventClass.Mouse, MouseEventKind.MouseEntered),
        new EventTypeSpec(EventClass.Mouse, MouseEventKind.MouseExited),
        new EventTypeSpec(EventClass.Mouse, MouseEventKind.WheelMoved),
        new EventTypeSpec(EventClass.Keyboard, KeyboardEventKind.RawKeyDown),
        new EventTypeSpec(EventClass.Keyboard, KeyboardEventKind.RawKeyRepeat),
        new EventTypeSpec(EventClass.Keyboard, KeyboardEventKind.RawKeyUp),
        new EventTypeSpec(EventClass.Keyboard, KeyboardEventKind.RawKeyModifiersChanged),
        new EventTypeSpec(EventClass.AppleEvent, AppleEventKind.AppleEvent)
      };
      Application.uppHandler = API.NewEventHandlerUPP(new MacOSEventHandler(Application.EventHandler));
      API.InstallApplicationEventHandler(Application.uppHandler, eventTypes, IntPtr.Zero, IntPtr.Zero);
      Application.mInitialized = true;
    }

    private static OSStatus EventHandler(IntPtr inCaller, IntPtr inEvent, IntPtr userData)
    {
      EventInfo evt = new EventInfo(inEvent);
      switch (evt.EventClass)
      {
        case EventClass.Keyboard:
        case EventClass.Mouse:
          if (Application.WindowEventHandler != null)
            return Application.WindowEventHandler.DispatchEvent(inCaller, inEvent, evt, userData);
          else
            break;
        case EventClass.Application:
          int num = (int) evt.AppEventKind;
          return OSStatus.EventNotHandled;
        case EventClass.AppleEvent:
          API.ProcessAppleEvent(inEvent);
          break;
      }
      return OSStatus.EventNotHandled;
    }

    public static void Run(CarbonGLNative window)
    {
      window.Closed += new EventHandler<EventArgs>(Application.MainWindowClosed);
      window.Visible = true;
      API.RunApplicationEventLoop();
      window.Closed -= new EventHandler<EventArgs>(Application.MainWindowClosed);
    }

    private static void MainWindowClosed(object sender, EventArgs e)
    {
      API.QuitApplicationEventLoop();
    }

    internal static void ProcessEvents()
    {
      API.ProcessEvents();
    }
  }
}
