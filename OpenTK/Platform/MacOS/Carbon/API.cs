// Type: OpenTK.Platform.MacOS.Carbon.API
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform.MacOS;
using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS.Carbon
{
  internal class API
  {
    private const string carbon = "/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon";
    private const string gestaltlib = "/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon";

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static EventClass GetEventClass(IntPtr inEvent);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static uint GetEventKind(IntPtr inEvent);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon", EntryPoint = "CreateNewWindow")]
    private static OSStatus _CreateNewWindow(WindowClass @class, WindowAttributes attributes, ref Rect r, out IntPtr window);

    internal static IntPtr CreateNewWindow(WindowClass @class, WindowAttributes attributes, Rect r)
    {
      IntPtr window;
      OSStatus newWindow = API._CreateNewWindow(@class, attributes, ref r, out window);
      if (newWindow != OSStatus.NoError)
        throw new MacOSException(newWindow);
      else
        return window;
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void DisposeWindow(IntPtr window);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void ShowWindow(IntPtr window);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void HideWindow(IntPtr window);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static bool IsWindowVisible(IntPtr window);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void SelectWindow(IntPtr window);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static OSStatus RepositionWindow(IntPtr window, IntPtr parentWindow, WindowPositionMethod method);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void SizeWindow(IntPtr window, short w, short h, bool fUpdate);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void MoveWindow(IntPtr window, short x, short y, bool fUpdate);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static OSStatus GetWindowBounds(IntPtr window, WindowRegionCode regionCode, out Rect globalBounds);

    internal static Rect GetWindowBounds(IntPtr window, WindowRegionCode regionCode)
    {
      Rect globalBounds;
      OSStatus windowBounds = API.GetWindowBounds(window, regionCode, out globalBounds);
      if (windowBounds != OSStatus.NoError)
        throw new MacOSException(windowBounds);
      else
        return globalBounds;
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static IntPtr GetEventDispatcherTarget();

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static OSStatus ReceiveNextEvent(uint inNumTypes, IntPtr inList, double inTimeout, bool inPullEvent, out IntPtr outEvent);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static void SendEventToEventTarget(IntPtr theEvent, IntPtr theTarget);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static void ReleaseEvent(IntPtr theEvent);

    internal static void SendEvent(IntPtr theEvent)
    {
      IntPtr dispatcherTarget = API.GetEventDispatcherTarget();
      API.SendEventToEventTarget(theEvent, dispatcherTarget);
    }

    internal static void ProcessEvents()
    {
      IntPtr dispatcherTarget = API.GetEventDispatcherTarget();
      while (true)
      {
        IntPtr outEvent;
        switch (API.ReceiveNextEvent(0U, IntPtr.Zero, 0.0, true, out outEvent))
        {
          case OSStatus.EventLoopTimedOut:
            goto label_8;
          case OSStatus.NoError:
            if (!(outEvent == IntPtr.Zero))
            {
              try
              {
                API.SendEventToEventTarget(outEvent, dispatcherTarget);
              }
              catch (ExecutionEngineException ex)
              {
                Console.Error.WriteLine("ExecutionEngineException caught.");
                Console.Error.WriteLine("theEvent: " + new EventInfo(outEvent).ToString());
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
              }
              API.ReleaseEvent(outEvent);
              continue;
            }
            else
              goto label_3;
          default:
            goto label_2;
        }
      }
label_8:
      return;
label_2:
      return;
label_3:;
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static bool ConvertEventRefToEventRecord(IntPtr inEvent, out API.EventRecord outEvent);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static OSStatus AEProcessAppleEvent(ref API.EventRecord theEventRecord);

    internal static void ProcessAppleEvent(IntPtr inEvent)
    {
      API.EventRecord outEvent;
      API.ConvertEventRefToEventRecord(inEvent, out outEvent);
      int num = (int) API.AEProcessAppleEvent(ref outEvent);
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon", EntryPoint = "CreateEvent")]
    private static OSStatus _CreateEvent(IntPtr inAllocator, EventClass inClassID, uint kind, double when, EventAttributes flags, out IntPtr outEvent);

    internal static IntPtr CreateWindowEvent(WindowEventKind kind)
    {
      IntPtr outEvent;
      OSStatus @event = API._CreateEvent(IntPtr.Zero, EventClass.Window, (uint) kind, 0.0, EventAttributes.kEventAttributeNone, out outEvent);
      if (@event != OSStatus.NoError)
        throw new MacOSException(@event);
      else
        return outEvent;
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static OSStatus GetEventParameter(IntPtr inEvent, EventParamName inName, EventParamType inDesiredType, IntPtr outActualType, uint inBufferSize, IntPtr outActualSize, IntPtr outData);

    internal static unsafe MacOSKeyCode GetEventKeyboardKeyCode(IntPtr inEvent)
    {
      int num;
      int* numPtr = &num;
      OSStatus eventParameter = API.GetEventParameter(inEvent, EventParamName.KeyCode, EventParamType.typeUInt32, IntPtr.Zero, (uint) Marshal.SizeOf(typeof (uint)), IntPtr.Zero, (IntPtr) ((void*) numPtr));
      if (eventParameter != OSStatus.NoError)
        throw new MacOSException(eventParameter);
      else
        return (MacOSKeyCode) num;
    }

    internal static unsafe char GetEventKeyboardChar(IntPtr inEvent)
    {
      char ch;
      char* chPtr = &ch;
      OSStatus eventParameter = API.GetEventParameter(inEvent, EventParamName.KeyMacCharCode, EventParamType.typeChar, IntPtr.Zero, (uint) Marshal.SizeOf(typeof (char)), IntPtr.Zero, (IntPtr) ((void*) chPtr));
      if (eventParameter != OSStatus.NoError)
        throw new MacOSException(eventParameter);
      else
        return ch;
    }

    internal static unsafe MouseButton GetEventMouseButton(IntPtr inEvent)
    {
      int num;
      int* numPtr = &num;
      OSStatus eventParameter = API.GetEventParameter(inEvent, EventParamName.MouseButton, EventParamType.typeMouseButton, IntPtr.Zero, (uint) Marshal.SizeOf(typeof (short)), IntPtr.Zero, (IntPtr) ((void*) numPtr));
      if (eventParameter != OSStatus.NoError)
        throw new MacOSException(eventParameter);
      else
        return (MouseButton) num;
    }

    internal static unsafe int GetEventMouseWheelDelta(IntPtr inEvent)
    {
      int num;
      int* numPtr = &num;
      OSStatus eventParameter = API.GetEventParameter(inEvent, EventParamName.MouseWheelDelta, EventParamType.typeSInt32, IntPtr.Zero, 4U, IntPtr.Zero, (IntPtr) ((void*) numPtr));
      if (eventParameter != OSStatus.NoError)
        throw new MacOSException(eventParameter);
      else
        return num;
    }

    internal static unsafe OSStatus GetEventWindowMouseLocation(IntPtr inEvent, out HIPoint pt)
    {
      HIPoint hiPoint;
      HIPoint* hiPointPtr = &hiPoint;
      OSStatus eventParameter = API.GetEventParameter(inEvent, EventParamName.WindowMouseLocation, EventParamType.typeHIPoint, IntPtr.Zero, (uint) Marshal.SizeOf(typeof (HIPoint)), IntPtr.Zero, (IntPtr) ((void*) hiPointPtr));
      pt = hiPoint;
      return eventParameter;
    }

    internal static unsafe OSStatus GetEventMouseDelta(IntPtr inEvent, out HIPoint pt)
    {
      HIPoint hiPoint;
      HIPoint* hiPointPtr = &hiPoint;
      OSStatus eventParameter = API.GetEventParameter(inEvent, EventParamName.MouseDelta, EventParamType.typeHIPoint, IntPtr.Zero, (uint) Marshal.SizeOf(typeof (HIPoint)), IntPtr.Zero, (IntPtr) ((void*) hiPointPtr));
      pt = hiPoint;
      return eventParameter;
    }

    internal static unsafe OSStatus GetEventWindowRef(IntPtr inEvent, out IntPtr windowRef)
    {
      IntPtr num;
      IntPtr* numPtr = &num;
      OSStatus eventParameter = API.GetEventParameter(inEvent, EventParamName.WindowRef, EventParamType.typeWindowRef, IntPtr.Zero, (uint) sizeof (IntPtr), IntPtr.Zero, (IntPtr) ((void*) numPtr));
      windowRef = num;
      return eventParameter;
    }

    internal static unsafe OSStatus GetEventMouseLocation(IntPtr inEvent, out HIPoint pt)
    {
      HIPoint hiPoint;
      HIPoint* hiPointPtr = &hiPoint;
      OSStatus eventParameter = API.GetEventParameter(inEvent, EventParamName.MouseLocation, EventParamType.typeHIPoint, IntPtr.Zero, (uint) Marshal.SizeOf(typeof (HIPoint)), IntPtr.Zero, (IntPtr) ((void*) hiPointPtr));
      pt = hiPoint;
      return eventParameter;
    }

    internal static unsafe MacOSKeyModifiers GetEventKeyModifiers(IntPtr inEvent)
    {
      uint num;
      uint* numPtr = &num;
      OSStatus eventParameter = API.GetEventParameter(inEvent, EventParamName.KeyModifiers, EventParamType.typeUInt32, IntPtr.Zero, (uint) Marshal.SizeOf(typeof (uint)), IntPtr.Zero, (IntPtr) ((void*) numPtr));
      if (eventParameter != OSStatus.NoError)
        throw new MacOSException(eventParameter);
      else
        return (MacOSKeyModifiers) num;
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon", EntryPoint = "InstallEventHandler")]
    private static OSStatus _InstallEventHandler(IntPtr eventTargetRef, IntPtr handlerProc, int numtypes, EventTypeSpec[] typeList, IntPtr userData, IntPtr handlerRef);

    internal static void InstallWindowEventHandler(IntPtr windowRef, IntPtr uppHandlerProc, EventTypeSpec[] eventTypes, IntPtr userData, IntPtr handlerRef)
    {
      OSStatus errorCode = API._InstallEventHandler(API.GetWindowEventTarget(windowRef), uppHandlerProc, eventTypes.Length, eventTypes, userData, handlerRef);
      if (errorCode != OSStatus.NoError)
        throw new MacOSException(errorCode);
    }

    internal static void InstallApplicationEventHandler(IntPtr uppHandlerProc, EventTypeSpec[] eventTypes, IntPtr userData, IntPtr handlerRef)
    {
      OSStatus errorCode = API._InstallEventHandler(API.GetApplicationEventTarget(), uppHandlerProc, eventTypes.Length, eventTypes, userData, handlerRef);
      if (errorCode != OSStatus.NoError)
        throw new MacOSException(errorCode);
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static OSStatus RemoveEventHandler(IntPtr inHandlerRef);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr GetWindowEventTarget(IntPtr window);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr GetApplicationEventTarget();

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr NewEventHandlerUPP(MacOSEventHandler handler);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void DisposeEventHandlerUPP(IntPtr userUPP);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static int TransformProcessType(ref ProcessSerialNumber psn, ProcessApplicationTransformState type);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static int GetCurrentProcess(ref ProcessSerialNumber psn);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static int SetFrontProcess(ref ProcessSerialNumber psn);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr CGColorSpaceCreateDeviceRGB();

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr CGDataProviderCreateWithData(IntPtr info, IntPtr[] data, int size, IntPtr releasefunc);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr CGImageCreate(int width, int height, int bitsPerComponent, int bitsPerPixel, int bytesPerRow, IntPtr colorspace, uint bitmapInfo, IntPtr provider, IntPtr decode, int shouldInterpolate, int intent);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void SetApplicationDockTileImage(IntPtr imageRef);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void RestoreApplicationDockTileImage();

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static IntPtr GetControlBounds(IntPtr control, out Rect bounds);

    internal static Rect GetControlBounds(IntPtr control)
    {
      Rect bounds;
      API.GetControlBounds(control, out bounds);
      return bounds;
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static OSStatus ActivateWindow(IntPtr inWindow, bool inActivate);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void RunApplicationEventLoop();

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static void QuitApplicationEventLoop();

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr GetControlOwner(IntPtr control);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr HIViewGetWindow(IntPtr inView);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static OSStatus HIViewGetFrame(IntPtr inView, out HIRect outRect);

    internal static HIRect HIViewGetFrame(IntPtr inView)
    {
      HIRect outRect;
      OSStatus frame = API.HIViewGetFrame(inView, out outRect);
      if (frame != OSStatus.NoError)
        throw new MacOSException(frame);
      else
        return outRect;
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static void SetWindowTitleWithCFString(IntPtr windowRef, IntPtr title);

    internal static void SetWindowTitle(IntPtr windowRef, string title)
    {
      IntPtr title1 = API.__CFStringMakeConstantString(title);
      API.SetWindowTitleWithCFString(windowRef, title1);
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon", EntryPoint = "ChangeWindowAttributes")]
    private static OSStatus _ChangeWindowAttributes(IntPtr windowRef, WindowAttributes setTheseAttributes, WindowAttributes clearTheseAttributes);

    internal static void ChangeWindowAttributes(IntPtr windowRef, WindowAttributes setTheseAttributes, WindowAttributes clearTheseAttributes)
    {
      OSStatus errorCode = API._ChangeWindowAttributes(windowRef, setTheseAttributes, clearTheseAttributes);
      if (errorCode != OSStatus.NoError)
        throw new MacOSException(errorCode);
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static IntPtr __CFStringMakeConstantString(string cStr);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    private static void CFRelease(IntPtr cfStr);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static OSStatus CallNextEventHandler(IntPtr nextHandler, IntPtr theEvent);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr GetWindowPort(IntPtr windowRef);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static IntPtr AcquireRootMenu();

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static bool IsWindowCollapsed(IntPtr windowRef);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon", EntryPoint = "CollapseWindow")]
    private static OSStatus _CollapseWindow(IntPtr windowRef, bool collapse);

    internal static void CollapseWindow(IntPtr windowRef, bool collapse)
    {
      OSStatus errorCode = API._CollapseWindow(windowRef, collapse);
      if (errorCode != OSStatus.NoError)
        throw new MacOSException(errorCode);
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon", EntryPoint = "IsWindowInStandardState")]
    private static bool _IsWindowInStandardState(IntPtr windowRef, IntPtr inIdealSize, IntPtr outIdealStandardState);

    internal static bool IsWindowInStandardState(IntPtr windowRef)
    {
      return API._IsWindowInStandardState(windowRef, IntPtr.Zero, IntPtr.Zero);
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon", EntryPoint = "ZoomWindowIdeal")]
    private static OSStatus _ZoomWindowIdeal(IntPtr windowRef, short inPartCode, IntPtr toIdealSize);

    internal static void ZoomWindowIdeal(IntPtr windowRef, WindowPartCode inPartCode, ref CarbonPoint toIdealSize)
    {
      IntPtr num = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (CarbonPoint)));
      Marshal.StructureToPtr((object) toIdealSize, num, false);
      OSStatus errorCode = API._ZoomWindowIdeal(windowRef, (short) inPartCode, num);
      toIdealSize = (CarbonPoint) Marshal.PtrToStructure(num, typeof (CarbonPoint));
      Marshal.FreeHGlobal(num);
      if (errorCode != OSStatus.NoError)
        throw new MacOSException(errorCode);
    }

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static OSStatus DMGetGDeviceByDisplayID(IntPtr displayID, out IntPtr displayDevice, bool failToMain);

    [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
    internal static OSStatus Gestalt(GestaltSelector selector, out int response);

    private struct EventRecord
    {
      public ushort what;
      public uint message;
      public uint when;
      public CarbonPoint where;
      public uint modifiers;
    }
  }
}
