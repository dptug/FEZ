// Type: OpenTK.Platform.X11.Functions
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenTK.Platform.X11
{
  internal static class Functions
  {
    public static readonly object Lock = API.Lock;
    private static readonly IntPtr CopyFromParent = IntPtr.Zero;
    internal const string X11Library = "libX11";
    private const string XrandrLibrary = "libXrandr.so.2";

    static Functions()
    {
    }

    [DllImport("libX11", EntryPoint = "XOpenDisplay")]
    private static IntPtr sys_XOpenDisplay(IntPtr display);

    public static IntPtr XOpenDisplay(IntPtr display)
    {
      lock (Functions.Lock)
        return Functions.sys_XOpenDisplay(display);
    }

    [DllImport("libX11")]
    public static int XCloseDisplay(IntPtr display);

    [DllImport("libX11")]
    public static IntPtr XSynchronize(IntPtr display, bool onoff);

    [DllImport("libX11")]
    public static IntPtr XCreateWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, int depth, int xclass, IntPtr visual, IntPtr valuemask, ref XSetWindowAttributes attributes);

    [DllImport("libX11")]
    public static IntPtr XCreateSimpleWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, UIntPtr border, UIntPtr background);

    [DllImport("libX11")]
    public static IntPtr XCreateSimpleWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, IntPtr border, IntPtr background);

    [DllImport("libX11")]
    public static int XMapWindow(IntPtr display, IntPtr window);

    [DllImport("libX11")]
    public static int XUnmapWindow(IntPtr display, IntPtr window);

    [DllImport("libX11", EntryPoint = "XMapSubwindows")]
    public static int XMapSubindows(IntPtr display, IntPtr window);

    [DllImport("libX11")]
    public static int XUnmapSubwindows(IntPtr display, IntPtr window);

    [DllImport("libX11")]
    public static IntPtr XRootWindow(IntPtr display, int screen_number);

    [DllImport("libX11")]
    public static IntPtr XNextEvent(IntPtr display, ref XEvent xevent);

    [DllImport("libX11")]
    public static bool XWindowEvent(IntPtr display, IntPtr w, EventMask event_mask, ref XEvent event_return);

    [DllImport("libX11")]
    public static bool XCheckWindowEvent(IntPtr display, IntPtr w, EventMask event_mask, ref XEvent event_return);

    [DllImport("libX11")]
    public static bool XCheckTypedWindowEvent(IntPtr display, IntPtr w, XEventName event_type, ref XEvent event_return);

    [DllImport("libX11")]
    public static bool XCheckTypedEvent(IntPtr display, XEventName event_type, out XEvent event_return);

    [DllImport("libX11")]
    public static bool XIfEvent(IntPtr display, ref XEvent e, IntPtr predicate, IntPtr arg);

    [DllImport("libX11")]
    public static bool XCheckIfEvent(IntPtr display, ref XEvent e, IntPtr predicate, IntPtr arg);

    [DllImport("libX11")]
    public static int XConnectionNumber(IntPtr diplay);

    [DllImport("libX11")]
    public static int XPending(IntPtr diplay);

    [DllImport("libX11")]
    public static int XSelectInput(IntPtr display, IntPtr window, IntPtr mask);

    [DllImport("libX11")]
    public static int XDestroyWindow(IntPtr display, IntPtr window);

    [DllImport("libX11")]
    public static int XReparentWindow(IntPtr display, IntPtr window, IntPtr parent, int x, int y);

    [DllImport("libX11")]
    public static int XMoveResizeWindow(IntPtr display, IntPtr window, int x, int y, int width, int height);

    [DllImport("libX11")]
    public static int XMoveWindow(IntPtr display, IntPtr w, int x, int y);

    [DllImport("libX11")]
    public static int XResizeWindow(IntPtr display, IntPtr window, int width, int height);

    [DllImport("libX11")]
    public static int XGetWindowAttributes(IntPtr display, IntPtr window, ref XWindowAttributes attributes);

    [DllImport("libX11")]
    public static int XFlush(IntPtr display);

    [DllImport("libX11")]
    public static int XSetWMName(IntPtr display, IntPtr window, ref XTextProperty text_prop);

    [DllImport("libX11")]
    public static int XStoreName(IntPtr display, IntPtr window, string window_name);

    [DllImport("libX11")]
    public static int XFetchName(IntPtr display, IntPtr window, ref IntPtr window_name);

    [DllImport("libX11")]
    public static int XSendEvent(IntPtr display, IntPtr window, bool propagate, IntPtr event_mask, ref XEvent send_event);

    public static int XSendEvent(IntPtr display, IntPtr window, bool propagate, EventMask event_mask, ref XEvent send_event)
    {
      return Functions.XSendEvent(display, window, propagate, new IntPtr((int) event_mask), ref send_event);
    }

    [DllImport("libX11")]
    public static int XQueryTree(IntPtr display, IntPtr window, out IntPtr root_return, out IntPtr parent_return, out IntPtr children_return, out int nchildren_return);

    [DllImport("libX11")]
    public static int XFree(IntPtr data);

    [DllImport("libX11")]
    public static int XRaiseWindow(IntPtr display, IntPtr window);

    [DllImport("libX11")]
    public static uint XLowerWindow(IntPtr display, IntPtr window);

    [DllImport("libX11")]
    public static uint XConfigureWindow(IntPtr display, IntPtr window, ChangeWindowAttributes value_mask, ref XWindowChanges values);

    [DllImport("libX11")]
    public static IntPtr XInternAtom(IntPtr display, string atom_name, bool only_if_exists);

    [DllImport("libX11")]
    public static int XInternAtoms(IntPtr display, string[] atom_names, int atom_count, bool only_if_exists, IntPtr[] atoms);

    [DllImport("libX11")]
    public static int XSetWMProtocols(IntPtr display, IntPtr window, IntPtr[] protocols, int count);

    [DllImport("libX11")]
    public static int XGrabPointer(IntPtr display, IntPtr window, bool owner_events, EventMask event_mask, GrabMode pointer_mode, GrabMode keyboard_mode, IntPtr confine_to, IntPtr cursor, IntPtr timestamp);

    [DllImport("libX11")]
    public static int XUngrabPointer(IntPtr display, IntPtr timestamp);

    [DllImport("libX11")]
    public static int XGrabButton(IntPtr display, int button, uint modifiers, IntPtr grab_window, bool owner_events, EventMask event_mask, GrabMode pointer_mode, GrabMode keyboard_mode, IntPtr confine_to, IntPtr cursor);

    [DllImport("libX11")]
    public static int XUngrabButton(IntPtr display, uint button, uint modifiers, IntPtr grab_window);

    [DllImport("libX11")]
    public static bool XQueryPointer(IntPtr display, IntPtr window, out IntPtr root, out IntPtr child, out int root_x, out int root_y, out int win_x, out int win_y, out int keys_buttons);

    [DllImport("libX11")]
    public static bool XTranslateCoordinates(IntPtr display, IntPtr src_w, IntPtr dest_w, int src_x, int src_y, out int intdest_x_return, out int dest_y_return, out IntPtr child_return);

    [DllImport("libX11")]
    public static int XGrabKey(IntPtr display, int keycode, uint modifiers, IntPtr grab_window, bool owner_events, GrabMode pointer_mode, GrabMode keyboard_mode);

    [DllImport("libX11")]
    public static int XUngrabKey(IntPtr display, int keycode, uint modifiers, IntPtr grab_window);

    [DllImport("libX11")]
    public static int XGrabKeyboard(IntPtr display, IntPtr window, bool owner_events, GrabMode pointer_mode, GrabMode keyboard_mode, IntPtr timestamp);

    [DllImport("libX11")]
    public static int XUngrabKeyboard(IntPtr display, IntPtr timestamp);

    [DllImport("libX11")]
    public static int XAllowEvents(IntPtr display, EventMode event_mode, IntPtr time);

    [DllImport("libX11")]
    public static bool XGetGeometry(IntPtr display, IntPtr window, out IntPtr root, out int x, out int y, out int width, out int height, out int border_width, out int depth);

    [DllImport("libX11")]
    public static bool XGetGeometry(IntPtr display, IntPtr window, IntPtr root, out int x, out int y, out int width, out int height, IntPtr border_width, IntPtr depth);

    [DllImport("libX11")]
    public static bool XGetGeometry(IntPtr display, IntPtr window, IntPtr root, out int x, out int y, IntPtr width, IntPtr height, IntPtr border_width, IntPtr depth);

    [DllImport("libX11")]
    public static bool XGetGeometry(IntPtr display, IntPtr window, IntPtr root, IntPtr x, IntPtr y, out int width, out int height, IntPtr border_width, IntPtr depth);

    [DllImport("libX11")]
    public static uint XWarpPointer(IntPtr display, IntPtr src_w, IntPtr dest_w, int src_x, int src_y, uint src_width, uint src_height, int dest_x, int dest_y);

    [DllImport("libX11")]
    public static int XClearWindow(IntPtr display, IntPtr window);

    [DllImport("libX11")]
    public static int XClearArea(IntPtr display, IntPtr window, int x, int y, int width, int height, bool exposures);

    [DllImport("libX11")]
    public static IntPtr XDefaultScreenOfDisplay(IntPtr display);

    [DllImport("libX11")]
    public static int XScreenNumberOfScreen(IntPtr display, IntPtr Screen);

    [DllImport("libX11")]
    public static IntPtr XDefaultVisual(IntPtr display, int screen_number);

    [DllImport("libX11")]
    public static uint XDefaultDepth(IntPtr display, int screen_number);

    [DllImport("libX11")]
    public static int XDefaultScreen(IntPtr display);

    [DllImport("libX11")]
    public static IntPtr XDefaultColormap(IntPtr display, int screen_number);

    [DllImport("libX11")]
    public static int XLookupColor(IntPtr display, IntPtr Colormap, string Coloranem, ref XColor exact_def_color, ref XColor screen_def_color);

    [DllImport("libX11")]
    public static int XAllocColor(IntPtr display, IntPtr Colormap, ref XColor colorcell_def);

    [DllImport("libX11")]
    public static int XSetTransientForHint(IntPtr display, IntPtr window, IntPtr prop_window);

    [DllImport("libX11")]
    public static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref MotifWmHints data, int nelements);

    [DllImport("libX11")]
    public static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref uint value, int nelements);

    [DllImport("libX11")]
    public static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref int value, int nelements);

    [DllImport("libX11")]
    public static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref IntPtr value, int nelements);

    [DllImport("libX11")]
    public static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, uint[] data, int nelements);

    [DllImport("libX11")]
    public static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, int[] data, int nelements);

    [DllImport("libX11")]
    public static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, IntPtr[] data, int nelements);

    [DllImport("libX11")]
    public static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, IntPtr atoms, int nelements);

    [DllImport("libX11", CharSet = CharSet.Ansi)]
    public static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, string text, int text_length);

    [DllImport("libX11")]
    public static int XDeleteProperty(IntPtr display, IntPtr window, IntPtr property);

    [DllImport("libX11")]
    public static IntPtr XCreateGC(IntPtr display, IntPtr window, IntPtr valuemask, XGCValues[] values);

    [DllImport("libX11")]
    public static int XFreeGC(IntPtr display, IntPtr gc);

    [DllImport("libX11")]
    public static int XSetFunction(IntPtr display, IntPtr gc, GXFunction function);

    [DllImport("libX11")]
    public static int XSetLineAttributes(IntPtr display, IntPtr gc, int line_width, GCLineStyle line_style, GCCapStyle cap_style, GCJoinStyle join_style);

    [DllImport("libX11")]
    public static int XDrawLine(IntPtr display, IntPtr drawable, IntPtr gc, int x1, int y1, int x2, int y2);

    [DllImport("libX11")]
    public static int XDrawRectangle(IntPtr display, IntPtr drawable, IntPtr gc, int x1, int y1, int width, int height);

    [DllImport("libX11")]
    public static int XFillRectangle(IntPtr display, IntPtr drawable, IntPtr gc, int x1, int y1, int width, int height);

    [DllImport("libX11")]
    public static int XSetWindowBackground(IntPtr display, IntPtr window, IntPtr background);

    [DllImport("libX11")]
    public static int XCopyArea(IntPtr display, IntPtr src, IntPtr dest, IntPtr gc, int src_x, int src_y, int width, int height, int dest_x, int dest_y);

    [DllImport("libX11")]
    public static int XGetWindowProperty(IntPtr display, IntPtr window, IntPtr atom, IntPtr long_offset, IntPtr long_length, bool delete, IntPtr req_type, out IntPtr actual_type, out int actual_format, out IntPtr nitems, out IntPtr bytes_after, ref IntPtr prop);

    [DllImport("libX11")]
    public static int XSetInputFocus(IntPtr display, IntPtr window, RevertTo revert_to, IntPtr time);

    [DllImport("libX11")]
    public static int XIconifyWindow(IntPtr display, IntPtr window, int screen_number);

    [DllImport("libX11")]
    public static int XDefineCursor(IntPtr display, IntPtr window, IntPtr cursor);

    [DllImport("libX11")]
    public static int XUndefineCursor(IntPtr display, IntPtr window);

    [DllImport("libX11")]
    public static int XFreeCursor(IntPtr display, IntPtr cursor);

    [DllImport("libX11")]
    public static IntPtr XCreateFontCursor(IntPtr display, CursorFontShape shape);

    [DllImport("libX11")]
    public static IntPtr XCreatePixmapCursor(IntPtr display, IntPtr source, IntPtr mask, ref XColor foreground_color, ref XColor background_color, int x_hot, int y_hot);

    [DllImport("libX11")]
    public static IntPtr XCreatePixmapFromBitmapData(IntPtr display, IntPtr drawable, byte[] data, int width, int height, IntPtr fg, IntPtr bg, int depth);

    [DllImport("libX11")]
    public static IntPtr XCreatePixmap(IntPtr display, IntPtr d, int width, int height, int depth);

    [DllImport("libX11")]
    public static IntPtr XFreePixmap(IntPtr display, IntPtr pixmap);

    [DllImport("libX11")]
    public static int XQueryBestCursor(IntPtr display, IntPtr drawable, int width, int height, out int best_width, out int best_height);

    [DllImport("libX11")]
    public static int XQueryExtension(IntPtr display, string extension_name, out int major, out int first_event, out int first_error);

    [DllImport("libX11")]
    public static IntPtr XWhitePixel(IntPtr display, int screen_no);

    [DllImport("libX11")]
    public static IntPtr XBlackPixel(IntPtr display, int screen_no);

    [DllImport("libX11")]
    public static void XGrabServer(IntPtr display);

    [DllImport("libX11")]
    public static void XUngrabServer(IntPtr display);

    [DllImport("libX11")]
    public static int XGetWMNormalHints(IntPtr display, IntPtr window, ref XSizeHints hints, out IntPtr supplied_return);

    [DllImport("libX11")]
    public static void XSetWMNormalHints(IntPtr display, IntPtr window, ref XSizeHints hints);

    [DllImport("libX11")]
    public static void XSetZoomHints(IntPtr display, IntPtr window, ref XSizeHints hints);

    [DllImport("libX11")]
    public static IntPtr XGetWMHints(IntPtr display, IntPtr w);

    [DllImport("libX11")]
    public static void XSetWMHints(IntPtr display, IntPtr w, ref XWMHints wmhints);

    [DllImport("libX11")]
    public static IntPtr XAllocWMHints();

    [DllImport("libX11")]
    public static int XGetIconSizes(IntPtr display, IntPtr window, out IntPtr size_list, out int count);

    [DllImport("libX11")]
    public static IntPtr XSetErrorHandler(XErrorHandler error_handler);

    [DllImport("libX11")]
    public static IntPtr XGetErrorText(IntPtr display, byte code, StringBuilder buffer, int length);

    [DllImport("libX11")]
    public static int XInitThreads();

    [DllImport("libX11")]
    public static int XConvertSelection(IntPtr display, IntPtr selection, IntPtr target, IntPtr property, IntPtr requestor, IntPtr time);

    [DllImport("libX11")]
    public static IntPtr XGetSelectionOwner(IntPtr display, IntPtr selection);

    [DllImport("libX11")]
    public static int XSetSelectionOwner(IntPtr display, IntPtr selection, IntPtr owner, IntPtr time);

    [DllImport("libX11")]
    public static int XSetPlaneMask(IntPtr display, IntPtr gc, IntPtr mask);

    [DllImport("libX11")]
    public static int XSetForeground(IntPtr display, IntPtr gc, UIntPtr foreground);

    [DllImport("libX11")]
    public static int XSetForeground(IntPtr display, IntPtr gc, IntPtr foreground);

    [DllImport("libX11")]
    public static int XSetBackground(IntPtr display, IntPtr gc, UIntPtr background);

    [DllImport("libX11")]
    public static int XSetBackground(IntPtr display, IntPtr gc, IntPtr background);

    [DllImport("libX11")]
    public static int XBell(IntPtr display, int percent);

    [DllImport("libX11")]
    public static int XChangeActivePointerGrab(IntPtr display, EventMask event_mask, IntPtr cursor, IntPtr time);

    [DllImport("libX11")]
    public static bool XFilterEvent(ref XEvent xevent, IntPtr window);

    [DllImport("libX11")]
    public static bool XkbSetDetectableAutoRepeat(IntPtr display, bool detectable, out bool supported);

    [DllImport("libX11")]
    public static void XPeekEvent(IntPtr display, ref XEvent xevent);

    [DllImport("libX11", EntryPoint = "XGetVisualInfo")]
    private static IntPtr XGetVisualInfoInternal(IntPtr display, IntPtr vinfo_mask, ref XVisualInfo template, out int nitems);

    public static IntPtr XGetVisualInfo(IntPtr display, XVisualInfoMask vinfo_mask, ref XVisualInfo template, out int nitems)
    {
      return Functions.XGetVisualInfoInternal(display, (IntPtr) ((int) vinfo_mask), ref template, out nitems);
    }

    [DllImport("libX11")]
    public static IntPtr XCreateColormap(IntPtr display, IntPtr window, IntPtr visual, int alloc);

    [DllImport("libX11")]
    public static void XLockDisplay(IntPtr display);

    [DllImport("libX11")]
    public static void XUnlockDisplay(IntPtr display);

    [DllImport("libX11")]
    public static int XGetTransientForHint(IntPtr display, IntPtr w, out IntPtr prop_window_return);

    [DllImport("libX11")]
    public static void XSync(IntPtr display, bool discard);

    [DllImport("libX11")]
    public static void XAutoRepeatOff(IntPtr display);

    [DllImport("libX11")]
    public static void XAutoRepeatOn(IntPtr display);

    [DllImport("libX11")]
    public static IntPtr XDefaultRootWindow(IntPtr display);

    [DllImport("libX11")]
    public static int XBitmapBitOrder(IntPtr display);

    [DllImport("libX11")]
    public static IntPtr XCreateImage(IntPtr display, IntPtr visual, uint depth, ImageFormat format, int offset, byte[] data, uint width, uint height, int bitmap_pad, int bytes_per_line);

    [DllImport("libX11")]
    public static IntPtr XCreateImage(IntPtr display, IntPtr visual, uint depth, ImageFormat format, int offset, IntPtr data, uint width, uint height, int bitmap_pad, int bytes_per_line);

    [DllImport("libX11")]
    public static void XPutImage(IntPtr display, IntPtr drawable, IntPtr gc, IntPtr image, int src_x, int src_y, int dest_x, int dest_y, uint width, uint height);

    [DllImport("libX11")]
    public static int XLookupString(ref XKeyEvent event_struct, [Out] byte[] buffer_return, int bytes_buffer, [Out] IntPtr[] keysym_return, IntPtr status_in_out);

    [DllImport("libX11")]
    public static byte XKeysymToKeycode(IntPtr display, IntPtr keysym);

    [DllImport("libX11")]
    public static IntPtr XKeycodeToKeysym(IntPtr display, byte keycode, int index);

    [DllImport("libX11")]
    public static int XRefreshKeyboardMapping(ref XMappingEvent event_map);

    [DllImport("libX11")]
    public static int XGetEventData(IntPtr display, ref XGenericEventCookie cookie);

    [DllImport("libX11")]
    public static void XFreeEventData(IntPtr display, ref XGenericEventCookie cookie);

    [DllImport("libXi")]
    private static int XISelectEvents(IntPtr dpy, IntPtr win, [In] XIEventMask[] masks, int num_masks);

    [DllImport("libXi")]
    private static int XISelectEvents(IntPtr dpy, IntPtr win, [In] ref XIEventMask masks, int num_masks);

    public static int XISelectEvents(IntPtr dpy, IntPtr win, XIEventMask[] masks)
    {
      return Functions.XISelectEvents(dpy, win, masks, masks.Length);
    }

    public static int XISelectEvents(IntPtr dpy, IntPtr win, XIEventMask mask)
    {
      return Functions.XISelectEvents(dpy, win, ref mask, 1);
    }

    [DllImport("libXi")]
    private static int XIGrabDevice(IntPtr display, int deviceid, IntPtr grab_window, IntPtr time, IntPtr cursor, int grab_mode, int paired_device_mode, bool owner_events, XIEventMask[] mask);

    [DllImport("libXi")]
    private static int XIUngrabDevice(IntPtr display, int deviceid, IntPtr time);

    [DllImport("libXi")]
    public static bool XIWarpPointer(IntPtr display, int deviceid, IntPtr src_w, IntPtr dest_w, double src_x, double src_y, int src_width, int src_height, double dest_x, double dest_y);

    public static void SendNetWMMessage(X11WindowInfo window, IntPtr message_type, IntPtr l0, IntPtr l1, IntPtr l2)
    {
      Functions.XSendEvent(window.Display, window.RootWindow, false, new IntPtr(1572864), ref new XEvent()
      {
        ClientMessageEvent = {
          type = XEventName.ClientMessage,
          send_event = true,
          window = window.WindowHandle,
          message_type = message_type,
          format = 32,
          ptr1 = l0,
          ptr2 = l1,
          ptr3 = l2
        }
      });
    }

    public static void SendNetClientMessage(X11WindowInfo window, IntPtr message_type, IntPtr l0, IntPtr l1, IntPtr l2)
    {
      Functions.XSendEvent(window.Display, window.WindowHandle, false, new IntPtr(0), ref new XEvent()
      {
        ClientMessageEvent = {
          type = XEventName.ClientMessage,
          send_event = true,
          window = window.WindowHandle,
          message_type = message_type,
          format = 32,
          ptr1 = l0,
          ptr2 = l1,
          ptr3 = l2
        }
      });
    }

    public static IntPtr CreatePixmapFromImage(IntPtr display, Bitmap image)
    {
      int width = image.Width;
      int height = image.Height;
      BitmapData bitmapdata = image.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
      IntPtr image1 = Functions.XCreateImage(display, Functions.CopyFromParent, 24U, ImageFormat.ZPixmap, 0, bitmapdata.Scan0, (uint) width, (uint) height, 32, 0);
      IntPtr num = Functions.XCreatePixmap(display, Functions.XDefaultRootWindow(display), width, height, 24);
      IntPtr gc = Functions.XCreateGC(display, num, IntPtr.Zero, (XGCValues[]) null);
      Functions.XPutImage(display, num, gc, image1, 0, 0, 0, 0, (uint) width, (uint) height);
      Functions.XFreeGC(display, gc);
      image.UnlockBits(bitmapdata);
      return num;
    }

    public static IntPtr CreateMaskFromImage(IntPtr display, Bitmap image)
    {
      int width = image.Width;
      int height = image.Height;
      int num1 = width + 7 >> 3;
      byte[] data = new byte[num1 * height];
      bool flag = Functions.XBitmapBitOrder(display) == 1;
      for (int y = 0; y < height; ++y)
      {
        for (int x = 0; x < width; ++x)
        {
          byte num2 = (byte) (1 << (flag ? 7 - (x & 7) : x & 7));
          int index = y * num1 + (x >> 3);
          if ((int) image.GetPixel(x, y).A >= 128)
            data[index] |= num2;
        }
      }
      return Functions.XCreatePixmapFromBitmapData(display, Functions.XDefaultRootWindow(display), data, width, height, new IntPtr(1), IntPtr.Zero, 1);
    }

    [DllImport("libX11")]
    public static IntPtr XCreateWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, int depth, int @class, IntPtr visual, UIntPtr valuemask, ref XSetWindowAttributes attributes);

    [DllImport("libX11")]
    internal static void XChangeWindowAttributes(IntPtr display, IntPtr w, UIntPtr valuemask, ref XSetWindowAttributes attributes);

    internal static void XChangeWindowAttributes(IntPtr display, IntPtr w, SetWindowValuemask valuemask, ref XSetWindowAttributes attributes)
    {
      Functions.XChangeWindowAttributes(display, w, (UIntPtr) ((ulong) valuemask), ref attributes);
    }

    [DllImport("libX11")]
    public static void XQueryKeymap(IntPtr display, byte[] keys);

    [DllImport("libX11")]
    public static void XMaskEvent(IntPtr display, EventMask event_mask, ref XEvent e);

    [DllImport("libX11")]
    public static void XPutBackEvent(IntPtr display, ref XEvent @event);

    [DllImport("libXrandr.so.2")]
    public static bool XRRQueryExtension(IntPtr dpy, ref int event_basep, ref int error_basep);

    [DllImport("libXrandr.so.2")]
    public static int XRRQueryVersion(IntPtr dpy, ref int major_versionp, ref int minor_versionp);

    [DllImport("libXrandr.so.2")]
    public static IntPtr XRRGetScreenInfo(IntPtr dpy, IntPtr draw);

    [DllImport("libXrandr.so.2")]
    public static void XRRFreeScreenConfigInfo(IntPtr config);

    [DllImport("libXrandr.so.2")]
    public static int XRRSetScreenConfig(IntPtr dpy, IntPtr config, IntPtr draw, int size_index, ushort rotation, IntPtr timestamp);

    [DllImport("libXrandr.so.2")]
    public static int XRRSetScreenConfigAndRate(IntPtr dpy, IntPtr config, IntPtr draw, int size_index, ushort rotation, short rate, IntPtr timestamp);

    [DllImport("libXrandr.so.2")]
    public static ushort XRRConfigRotations(IntPtr config, ref ushort current_rotation);

    [DllImport("libXrandr.so.2")]
    public static IntPtr XRRConfigTimes(IntPtr config, ref IntPtr config_timestamp);

    [DllImport("libXrandr.so.2")]
    [return: MarshalAs(UnmanagedType.LPStruct)]
    public static XRRScreenSize XRRConfigSizes(IntPtr config, int[] nsizes);

    [DllImport("libXrandr.so.2")]
    public static short* XRRConfigRates(IntPtr config, int size_index, int[] nrates);

    [DllImport("libXrandr.so.2")]
    public static ushort XRRConfigCurrentConfiguration(IntPtr config, out ushort rotation);

    [DllImport("libXrandr.so.2")]
    public static short XRRConfigCurrentRate(IntPtr config);

    [DllImport("libXrandr.so.2")]
    public static int XRRRootToScreen(IntPtr dpy, IntPtr root);

    [DllImport("libXrandr.so.2")]
    public static IntPtr XRRScreenConfig(IntPtr dpy, int screen);

    [DllImport("libXrandr.so.2")]
    public static IntPtr XRRConfig(ref Screen screen);

    [DllImport("libXrandr.so.2")]
    public static void XRRSelectInput(IntPtr dpy, IntPtr window, int mask);

    [DllImport("libXrandr.so.2")]
    public static int XRRUpdateConfiguration(ref XEvent @event);

    [DllImport("libXrandr.so.2")]
    public static ushort XRRRotations(IntPtr dpy, int screen, ref ushort current_rotation);

    [DllImport("libXrandr.so.2")]
    private static IntPtr XRRSizes(IntPtr dpy, int screen, int* nsizes);

    public static unsafe XRRScreenSize[] XRRSizes(IntPtr dpy, int screen)
    {
      int length;
      byte* numPtr = (byte*) (void*) Functions.XRRSizes(dpy, screen, &length);
      if (length == 0)
        return (XRRScreenSize[]) null;
      XRRScreenSize[] xrrScreenSizeArray = new XRRScreenSize[length];
      for (int index = 0; index < length; ++index)
      {
        xrrScreenSizeArray[index] = new XRRScreenSize();
        xrrScreenSizeArray[index] = (XRRScreenSize) Marshal.PtrToStructure((IntPtr) ((void*) numPtr), typeof (XRRScreenSize));
        numPtr += Marshal.SizeOf(typeof (XRRScreenSize));
      }
      return xrrScreenSizeArray;
    }

    [DllImport("libXrandr.so.2")]
    private static short* XRRRates(IntPtr dpy, int screen, int size_index, int* nrates);

    public static unsafe short[] XRRRates(IntPtr dpy, int screen, int size_index)
    {
      int length;
      short* numPtr = Functions.XRRRates(dpy, screen, size_index, &length);
      if (length == 0)
        return (short[]) null;
      short[] numArray = new short[length];
      for (int index = 0; index < length; ++index)
        numArray[index] = numPtr[index];
      return numArray;
    }

    [DllImport("libXrandr.so.2")]
    public static IntPtr XRRTimes(IntPtr dpy, int screen, out IntPtr config_timestamp);

    [DllImport("libX11")]
    public static int XScreenCount(IntPtr display);

    [DllImport("libX11")]
    private static int* XListDepths(IntPtr display, int screen_number, int* count_return);

    public static unsafe int[] XListDepths(IntPtr display, int screen_number)
    {
      int length;
      int* numPtr = Functions.XListDepths(display, screen_number, &length);
      if (length == 0)
        return (int[]) null;
      int[] numArray = new int[length];
      for (int index = 0; index < length; ++index)
        numArray[index] = numPtr[index];
      return numArray;
    }

    public static unsafe IntPtr XCreateBitmapFromData(IntPtr display, IntPtr d, byte[,] data)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      byte[,] numArray;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      fixed (byte* data1 = &^((numArray = data) == null || numArray.Length == 0 ? (byte&) IntPtr.Zero : numArray.Address(0, 0)))
        return Functions.XCreateBitmapFromData(display, d, data1, data.GetLength(0), data.GetLength(1));
    }

    [DllImport("libX11")]
    public static IntPtr XCreateBitmapFromData(IntPtr display, IntPtr d, byte* data, int width, int height);

    [DllImport("libX11", EntryPoint = "XAllocColor")]
    public static int XAllocNamedColor(IntPtr display, IntPtr colormap, string color_name, out XColor screen_def_return, out XColor exact_def_return);

    public delegate bool EventPredicate(IntPtr display, ref XEvent e, IntPtr arg);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct Pixel
    {
      public byte A;
      public byte R;
      public byte G;
      public byte B;

      public Pixel(byte a, byte r, byte g, byte b)
      {
        this.A = a;
        this.R = r;
        this.G = g;
        this.B = b;
      }

      public static implicit operator Functions.Pixel(int argb)
      {
        return new Functions.Pixel((byte) (argb >> 24 & (int) byte.MaxValue), (byte) (argb >> 16 & (int) byte.MaxValue), (byte) (argb >> 8 & (int) byte.MaxValue), (byte) (argb & (int) byte.MaxValue));
      }
    }
  }
}
