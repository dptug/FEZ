// Type: OpenTK.Platform.X11.API
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenTK.Platform.X11
{
  internal static class API
  {
    internal static object Lock = new object();
    private const string _dll_name = "libX11";
    private const string _dll_name_vid = "libXxf86vm";
    private static IntPtr defaultDisplay;
    private static int defaultScreen;
    private static IntPtr rootWindow;
    private static int screenCount;

    internal static IntPtr DefaultDisplay
    {
      get
      {
        return API.defaultDisplay;
      }
    }

    private static int DefaultScreen
    {
      get
      {
        return API.defaultScreen;
      }
    }

    internal static int ScreenCount
    {
      get
      {
        return API.screenCount;
      }
    }

    static API()
    {
      Functions.XInitThreads();
      API.defaultDisplay = Functions.XOpenDisplay(IntPtr.Zero);
      if (API.defaultDisplay == IntPtr.Zero)
        throw new PlatformException("Could not establish connection to the X-Server.");
      using (new XLock(API.defaultDisplay))
        API.screenCount = Functions.XScreenCount(API.DefaultDisplay);
    }

    private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
    {
      if (!(API.defaultDisplay != IntPtr.Zero))
        return;
      Functions.XCloseDisplay(API.defaultDisplay);
      API.defaultDisplay = IntPtr.Zero;
      API.defaultScreen = 0;
      API.rootWindow = IntPtr.Zero;
    }

    [Obsolete("Use XCreateWindow instead")]
    [DllImport("libX11", EntryPoint = "XCreateWindow")]
    public static IntPtr CreateWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, int depth, int @class, IntPtr visual, [MarshalAs(UnmanagedType.SysUInt)] CreateWindowMask valuemask, SetWindowAttributes attributes);

    [DllImport("libX11", EntryPoint = "XCreateSimpleWindow")]
    public static IntPtr CreateSimpleWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, long border, long background);

    [DllImport("libX11")]
    public static int XResizeWindow(IntPtr display, IntPtr window, int width, int height);

    [DllImport("libX11", EntryPoint = "XDestroyWindow")]
    public static void DestroyWindow(IntPtr display, IntPtr window);

    [DllImport("libX11", EntryPoint = "XMapWindow")]
    public static void MapWindow(IntPtr display, IntPtr window);

    [DllImport("libX11", EntryPoint = "XMapRaised")]
    public static void MapRaised(IntPtr display, IntPtr window);

    [DllImport("libX11", EntryPoint = "XDefaultVisual")]
    public static IntPtr DefaultVisual(IntPtr display, int screen_number);

    [DllImport("libX11", EntryPoint = "XFree")]
    public static void Free(IntPtr buffer);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("libX11", EntryPoint = "XEventsQueued")]
    public static int EventsQueued(IntPtr display, int mode);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("libX11", EntryPoint = "XPending")]
    public static int Pending(IntPtr display);

    [DllImport("libX11", EntryPoint = "XNextEvent")]
    public static void NextEvent(IntPtr display, [MarshalAs(UnmanagedType.AsAny), In, Out] object e);

    [DllImport("libX11", EntryPoint = "XNextEvent")]
    public static void NextEvent(IntPtr display, [In, Out] IntPtr e);

    [DllImport("libX11", EntryPoint = "XPeekEvent")]
    public static void PeekEvent(IntPtr display, [MarshalAs(UnmanagedType.AsAny), In, Out] object event_return);

    [DllImport("libX11", EntryPoint = "XPeekEvent")]
    public static void PeekEvent(IntPtr display, [In, Out] XEvent event_return);

    [DllImport("libX11", EntryPoint = "XSendEvent")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static bool SendEvent(IntPtr display, IntPtr window, bool propagate, [MarshalAs(UnmanagedType.SysInt)] EventMask event_mask, ref XEvent event_send);

    [DllImport("libX11", EntryPoint = "XSelectInput")]
    public static void SelectInput(IntPtr display, IntPtr w, EventMask event_mask);

    [DllImport("libX11", EntryPoint = "XCheckIfEvent")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static bool CheckIfEvent(IntPtr display, ref XEvent event_return, API.CheckEventPredicate predicate, IntPtr arg);

    [DllImport("libX11", EntryPoint = "XIfEvent")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static bool IfEvent(IntPtr display, ref XEvent event_return, API.CheckEventPredicate predicate, IntPtr arg);

    [DllImport("libX11", EntryPoint = "XCheckMaskEvent")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static bool CheckMaskEvent(IntPtr display, EventMask event_mask, ref XEvent event_return);

    [DllImport("libX11", EntryPoint = "XGrabPointer")]
    public static ErrorCodes GrabPointer(IntPtr display, IntPtr grab_window, bool owner_events, int event_mask, GrabMode pointer_mode, GrabMode keyboard_mode, IntPtr confine_to, IntPtr cursor, int time);

    [DllImport("libX11", EntryPoint = "XUngrabPointer")]
    public static ErrorCodes UngrabPointer(IntPtr display, int time);

    [DllImport("libX11", EntryPoint = "XGrabKeyboard")]
    public static ErrorCodes GrabKeyboard(IntPtr display, IntPtr grab_window, bool owner_events, GrabMode pointer_mode, GrabMode keyboard_mode, int time);

    [DllImport("libX11", EntryPoint = "XUngrabKeyboard")]
    public static void UngrabKeyboard(IntPtr display, int time);

    [DllImport("libX11", EntryPoint = "XGetKeyboardMapping")]
    public static IntPtr GetKeyboardMapping(IntPtr display, byte first_keycode, int keycode_count, ref int keysyms_per_keycode_return);

    [DllImport("libX11", EntryPoint = "XDisplayKeycodes")]
    public static void DisplayKeycodes(IntPtr display, ref int min_keycodes_return, ref int max_keycodes_return);

    [DllImport("libXxf86vm")]
    public static bool XF86VidModeQueryExtension(IntPtr display, out int event_base_return, out int error_base_return);

    [DllImport("libXxf86vm")]
    public static bool XF86VidModeSwitchToMode(IntPtr display, int screen, IntPtr modeline);

    [DllImport("libXxf86vm")]
    public static bool XF86VidModeQueryVersion(IntPtr display, out int major_version_return, out int minor_version_return);

    [DllImport("libXxf86vm")]
    public static bool XF86VidModeGetModeLine(IntPtr display, int screen, out int dotclock_return, out API.XF86VidModeModeLine modeline);

    [DllImport("libXxf86vm")]
    public static bool XF86VidModeGetAllModeLines(IntPtr display, int screen, out int modecount_return, out IntPtr modesinfo);

    [DllImport("libXxf86vm")]
    public static bool XF86VidModeGetViewPort(IntPtr display, int screen, out int x_return, out int y_return);

    [DllImport("libXxf86vm")]
    public static bool XF86VidModeSetViewPort(IntPtr display, int screen, int x, int y);

    [DllImport("libX11", EntryPoint = "XLookupKeysym")]
    public static IntPtr LookupKeysym(ref XKeyEvent key_event, int index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool CheckEventPredicate(IntPtr display, ref XEvent @event, IntPtr arg);

    internal struct XF86VidModeModeLine
    {
      public short hdisplay;
      public short hsyncstart;
      public short hsyncend;
      public short htotal;
      public short vdisplay;
      public short vsyncstart;
      public short vsyncend;
      public short vtotal;
      public int flags;
      public int privsize;
      public IntPtr _private;
    }

    internal struct XF86VidModeModeInfo
    {
      public int dotclock;
      public short hdisplay;
      public short hsyncstart;
      public short hsyncend;
      public short htotal;
      public short hskew;
      public short vdisplay;
      public short vsyncstart;
      public short vsyncend;
      public short vtotal;
      public short vskew;
      public int flags;
      private int privsize;
      private IntPtr _private;
    }

    internal struct XF86VidModeMonitor
    {
      [MarshalAs(UnmanagedType.LPStr)]
      private string vendor;
      [MarshalAs(UnmanagedType.LPStr)]
      private string model;
      private float EMPTY;
      private byte nhsync;
      private IntPtr hsync;
      private byte nvsync;
      private IntPtr vsync;
    }

    internal struct XF86VidModeSyncRange
    {
      private float hi;
      private float lo;
    }

    internal struct XF86VidModeNotifyEvent
    {
      private int type;
      private ulong serial;
      private bool send_event;
      private IntPtr display;
      private IntPtr root;
      private int state;
      private int kind;
      private bool forced;
      private IntPtr time;
    }

    internal struct XF86VidModeGamma
    {
      private float red;
      private float green;
      private float blue;
    }
  }
}
