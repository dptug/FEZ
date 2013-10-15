// Type: OpenTK.Platform.Windows.Functions
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace OpenTK.Platform.Windows
{
  internal static class Functions
  {
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool SetWindowPos(IntPtr handle, IntPtr insertAfter, int x, int y, int cx, int cy, SetWindowPosFlags flags);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static bool AdjustWindowRect([In, Out] ref Win32Rectangle lpRect, WindowStyle dwStyle, bool bMenu);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    internal static bool AdjustWindowRectEx(ref Win32Rectangle lpRect, WindowStyle dwStyle, bool bMenu, ExtendedWindowStyle dwExStyle);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static IntPtr CreateWindowEx(ExtendedWindowStyle ExStyle, [MarshalAs(UnmanagedType.LPTStr)] string className, [MarshalAs(UnmanagedType.LPTStr)] string windowName, WindowStyle Style, int X, int Y, int Width, int Height, IntPtr HandleToParentWindow, IntPtr Menu, IntPtr Instance, IntPtr Param);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static IntPtr CreateWindowEx(ExtendedWindowStyle ExStyle, IntPtr ClassAtom, IntPtr WindowName, WindowStyle Style, int X, int Y, int Width, int Height, IntPtr HandleToParentWindow, IntPtr Menu, IntPtr Instance, IntPtr Param);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool DestroyWindow(IntPtr windowHandle);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static ushort RegisterClass(ref WindowClass window_class);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static ushort RegisterClassEx(ref ExtendedWindowClass window_class);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static short UnregisterClass([MarshalAs(UnmanagedType.LPTStr)] string className, IntPtr instance);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static short UnregisterClass(IntPtr className, IntPtr instance);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static bool GetClassInfoEx(IntPtr hinst, [MarshalAs(UnmanagedType.LPTStr)] string lpszClass, ref ExtendedWindowClass lpwcx);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static bool GetClassInfoEx(IntPtr hinst, UIntPtr lpszClass, ref ExtendedWindowClass lpwcx);

    [DllImport("user32.dll", SetLastError = true)]
    internal static IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

    internal static IntPtr SetWindowLong(IntPtr handle, GetWindowLongOffsets item, IntPtr newValue)
    {
      IntPtr num = IntPtr.Zero;
      Functions.SetLastError(0);
      num = IntPtr.Size != 4 ? Functions.SetWindowLongPtr(handle, item, newValue) : new IntPtr(Functions.SetWindowLong(handle, item, newValue.ToInt32()));
      if (num == IntPtr.Zero)
      {
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (lastWin32Error != 0)
          throw new PlatformException(string.Format("Failed to modify window border. Error: {0}", (object) lastWin32Error));
      }
      return num;
    }

    internal static IntPtr SetWindowLong(IntPtr handle, WindowProcedure newValue)
    {
      return Functions.SetWindowLong(handle, GetWindowLongOffsets.WNDPROC, Marshal.GetFunctionPointerForDelegate((Delegate) newValue));
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static int SetWindowLong(IntPtr hWnd, GetWindowLongOffsets nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static IntPtr SetWindowLongPtr(IntPtr hWnd, GetWindowLongOffsets nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static int SetWindowLong(IntPtr hWnd, GetWindowLongOffsets nIndex, [MarshalAs(UnmanagedType.FunctionPtr)] WindowProcedure dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static IntPtr SetWindowLongPtr(IntPtr hWnd, GetWindowLongOffsets nIndex, [MarshalAs(UnmanagedType.FunctionPtr)] WindowProcedure dwNewLong);

    internal static UIntPtr GetWindowLong(IntPtr handle, GetWindowLongOffsets index)
    {
      if (IntPtr.Size == 4)
        return (UIntPtr) Functions.GetWindowLongInternal(handle, index);
      else
        return Functions.GetWindowLongPtrInternal(handle, index);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
    private static uint GetWindowLongInternal(IntPtr hWnd, GetWindowLongOffsets nIndex);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
    private static UIntPtr GetWindowLongPtrInternal(IntPtr hWnd, GetWindowLongOffsets nIndex);

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("User32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool PeekMessage(ref MSG msg, IntPtr hWnd, int messageFilterMin, int messageFilterMax, int flags);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("User32.dll")]
    internal static int GetMessage(ref MSG msg, IntPtr windowHandle, int messageFilterMin, int messageFilterMax);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static IntPtr SendMessage(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool PostMessage(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    internal static void PostQuitMessage(int exitCode);

    [CLSCompliant(false)]
    [DllImport("User32.dll")]
    internal static IntPtr DispatchMessage(ref MSG msg);

    [CLSCompliant(false)]
    [DllImport("User32.dll")]
    internal static bool TranslateMessage(ref MSG lpMsg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    internal static int GetQueueStatus([MarshalAs(UnmanagedType.U4)] QueueStatusFlags flags);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static IntPtr DefWindowProc(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("winmm.dll")]
    internal static IntPtr TimeBeginPeriod(int period);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool QueryPerformanceFrequency(ref long PerformanceFrequency);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool QueryPerformanceCounter(ref long PerformanceCount);

    [DllImport("user32.dll")]
    internal static IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    internal static IntPtr GetWindowDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool ReleaseDC(IntPtr hwnd, IntPtr DC);

    [DllImport("gdi32.dll")]
    internal static int ChoosePixelFormat(IntPtr dc, ref PixelFormatDescriptor pfd);

    [DllImport("gdi32.dll")]
    internal static int DescribePixelFormat(IntPtr deviceContext, int pixel, int pfdSize, ref PixelFormatDescriptor pixelFormat);

    [DllImport("gdi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool SetPixelFormat(IntPtr dc, int format, ref PixelFormatDescriptor pfd);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("gdi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool SwapBuffers(IntPtr dc);

    [DllImport("kernel32.dll")]
    internal static IntPtr GetProcAddress(IntPtr handle, string funcname);

    [DllImport("kernel32.dll")]
    internal static void SetLastError(int dwErrCode);

    [DllImport("kernel32.dll")]
    internal static IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPTStr)] string module_name);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static IntPtr LoadLibrary(string dllName);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool FreeLibrary(IntPtr handle);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static short GetAsyncKeyState(VirtualKeys vKey);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static short GetKeyState(VirtualKeys vKey);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static uint MapVirtualKey(uint uCode, MapVirtualKeyType uMapType);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static uint MapVirtualKey(VirtualKeys vkey, MapVirtualKeyType uMapType);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static bool ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static bool SetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] string lpString);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static int GetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr), In, Out] StringBuilder lpString, int nMaxCount);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static bool ScreenToClient(IntPtr hWnd, ref Point point);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static bool ClientToScreen(IntPtr hWnd, ref Point point);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static bool GetClientRect(IntPtr windowHandle, out Win32Rectangle clientRectangle);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static bool GetWindowRect(IntPtr windowHandle, out Win32Rectangle windowRectangle);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll")]
    internal static bool GetWindowInfo(IntPtr hwnd, ref WindowInfo wi);

    [DllImport("dwmapi.dll")]
    public static IntPtr DwmGetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, void* pvAttribute, int cbAttribute);

    [DllImport("user32.dll")]
    public static IntPtr GetFocus();

    [DllImport("user32.dll")]
    public static bool IsWindowVisible(IntPtr intPtr);

    [DllImport("user32.dll")]
    public static IntPtr LoadIcon(IntPtr hInstance, string lpIconName);

    [DllImport("user32.dll")]
    public static IntPtr LoadCursor(IntPtr hInstance, string lpCursorName);

    [DllImport("user32.dll")]
    public static IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);

    public static IntPtr LoadCursor(CursorName lpCursorName)
    {
      return Functions.LoadCursor(IntPtr.Zero, new IntPtr((int) lpCursorName));
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static bool BringWindowToTop(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static bool SetParent(IntPtr child, IntPtr newParent);

    [DllImport("user32.dll", SetLastError = true)]
    public static IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, DeviceNotification Flags);

    [DllImport("user32.dll", SetLastError = true)]
    public static bool UnregisterDeviceNotification(IntPtr Handle);

    [DllImport("user32.dll", SetLastError = true)]
    internal static int ChangeDisplaySettings(DeviceMode device_mode, ChangeDisplaySettingsEnum flags);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static int ChangeDisplaySettingsEx([MarshalAs(UnmanagedType.LPTStr)] string lpszDeviceName, DeviceMode lpDevMode, IntPtr hwnd, ChangeDisplaySettingsEnum dwflags, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static bool EnumDisplayDevices([MarshalAs(UnmanagedType.LPTStr)] string lpDevice, int iDevNum, [In, Out] WindowsDisplayDevice lpDisplayDevice, int dwFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EnumDisplaySettings([MarshalAs(UnmanagedType.LPTStr)] string device_name, int graphics_mode, [In, Out] DeviceMode device_mode);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EnumDisplaySettings([MarshalAs(UnmanagedType.LPTStr)] string device_name, DisplayModeSettingsEnum graphics_mode, [In, Out] DeviceMode device_mode);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static bool EnumDisplaySettingsEx([MarshalAs(UnmanagedType.LPTStr)] string lpszDeviceName, DisplayModeSettingsEnum iModeNum, [In, Out] DeviceMode lpDevMode, int dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

    [DllImport("user32.dll", SetLastError = true)]
    public static IntPtr MonitorFromPoint(POINT pt, MonitorFrom dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static IntPtr MonitorFromWindow(IntPtr hwnd, MonitorFrom dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static bool TrackMouseEvent(ref TrackMouseEventStructure lpEventTrack);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static bool ReleaseCapture();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static IntPtr SetCapture(IntPtr hwnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static IntPtr GetCapture();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static IntPtr SetFocus(IntPtr hwnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static int ShowCursor(bool show);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static bool ClipCursor(ref Win32Rectangle rcClip);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static bool ClipCursor(IntPtr rcClip);

    [DllImport("user32.dll")]
    public static bool SetCursorPos(int X, int Y);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static bool GetCursorPos(ref POINT point);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("user32.dll", SetLastError = true)]
    internal static IntPtr DefRawInputProc(RawInput[] RawInput, int Input, uint SizeHeader);

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static IntPtr DefRawInputProc(ref RawInput RawInput, int Input, uint SizeHeader);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("user32.dll", SetLastError = true)]
    internal static IntPtr DefRawInputProc(IntPtr RawInput, int Input, uint SizeHeader);

    [CLSCompliant(false)]
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool RegisterRawInputDevices(RawInputDevice[] RawInputDevices, uint NumDevices, uint Size);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool RegisterRawInputDevices(RawInputDevice[] RawInputDevices, int NumDevices, int Size);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("user32.dll", SetLastError = true)]
    internal static uint GetRawInputBuffer([Out] RawInput[] Data, [In, Out] ref uint Size, [In] uint SizeHeader);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRawInputBuffer([Out] RawInput[] Data, [In, Out] ref int Size, [In] int SizeHeader);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRawInputBuffer([Out] IntPtr Data, [In, Out] ref int Size, [In] int SizeHeader);

    [CLSCompliant(false)]
    [DllImport("user32.dll", SetLastError = true)]
    internal static uint GetRegisteredRawInputDevices([Out] RawInput[] RawInputDevices, [In, Out] ref uint NumDevices, uint cbSize);

    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRegisteredRawInputDevices([Out] RawInput[] RawInputDevices, [In, Out] ref int NumDevices, int cbSize);

    [CLSCompliant(false)]
    [DllImport("user32.dll", SetLastError = true)]
    internal static uint GetRawInputDeviceList([In, Out] RawInputDeviceList[] RawInputDeviceList, [In, Out] ref uint NumDevices, uint Size);

    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRawInputDeviceList([In, Out] RawInputDeviceList[] RawInputDeviceList, [In, Out] ref int NumDevices, int Size);

    [CLSCompliant(false)]
    [DllImport("user32.dll", SetLastError = true)]
    internal static uint GetRawInputDeviceList([In, Out] IntPtr RawInputDeviceList, [In, Out] ref uint NumDevices, uint Size);

    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRawInputDeviceList([In, Out] IntPtr RawInputDeviceList, [In, Out] ref int NumDevices, int Size);

    [CLSCompliant(false)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static uint GetRawInputDeviceInfo(IntPtr Device, [MarshalAs(UnmanagedType.U4)] RawInputDeviceInfoEnum Command, [In, Out] IntPtr Data, [In, Out] ref uint Size);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRawInputDeviceInfo(IntPtr Device, [MarshalAs(UnmanagedType.U4)] RawInputDeviceInfoEnum Command, [In, Out] IntPtr Data, [In, Out] ref int Size);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("user32.dll", SetLastError = true)]
    internal static uint GetRawInputDeviceInfo(IntPtr Device, [MarshalAs(UnmanagedType.U4)] RawInputDeviceInfoEnum Command, [In, Out] RawInputDeviceInfo Data, [In, Out] ref uint Size);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRawInputDeviceInfo(IntPtr Device, [MarshalAs(UnmanagedType.U4)] RawInputDeviceInfoEnum Command, [In, Out] RawInputDeviceInfo Data, [In, Out] ref int Size);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRawInputData(IntPtr RawInput, GetRawInputDataEnum Command, [Out] IntPtr Data, [In, Out] ref int Size, int SizeHeader);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRawInputData(IntPtr RawInput, GetRawInputDataEnum Command, out RawInput Data, [In, Out] ref int Size, int SizeHeader);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", SetLastError = true)]
    internal static int GetRawInputData(IntPtr RawInput, GetRawInputDataEnum Command, RawInput* Data, [In, Out] ref int Size, int SizeHeader);

    internal static unsafe IntPtr NextRawInputStructure(IntPtr data)
    {
      return Functions.RawInputAlign((IntPtr) ((void*) ((IntPtr) (void*) data + API.RawInputHeaderSize)));
    }

    private static unsafe IntPtr RawInputAlign(IntPtr data)
    {
      return (IntPtr) ((void*) ((IntPtr) (void*) data + (IntPtr.Size - 1 & ~(IntPtr.Size - 1))));
    }

    [DllImport("gdi32.dll", SetLastError = true)]
    internal static IntPtr GetStockObject(int index);

    [DllImport("user32.dll", SetLastError = true)]
    public static UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, Functions.TimerProc lpTimerFunc);

    [DllImport("user32.dll", SetLastError = true)]
    public static bool KillTimer(IntPtr hWnd, UIntPtr uIDEvent);

    [DllImport("shell32.dll")]
    public static IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, ShGetFileIconFlags uFlags);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    public delegate void TimerProc(IntPtr hwnd, WindowMessage uMsg, UIntPtr idEvent, int dwTime);
  }
}
