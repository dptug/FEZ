// Type: SharpDX.Win32Native
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SharpDX
{
  internal class Win32Native
  {
    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll")]
    public static int PeekMessage(out Win32Native.NativeMessage lpMsg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax, int wRemoveMsg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll")]
    public static int GetMessage(out Win32Native.NativeMessage lpMsg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll")]
    public static int TranslateMessage(ref Win32Native.NativeMessage lpMsg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll")]
    public static int DispatchMessage(ref Win32Native.NativeMessage lpMsg);

    public static IntPtr GetWindowLong(HandleRef hWnd, Win32Native.WindowLongType index)
    {
      if (IntPtr.Size == 4)
        return Win32Native.GetWindowLong32(hWnd, index);
      else
        return Win32Native.GetWindowLong64(hWnd, index);
    }

    [DllImport("user32.dll", CharSet = CharSet.Ansi)]
    public static IntPtr GetFocus();

    [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Ansi)]
    private static IntPtr GetWindowLong32(HandleRef hwnd, Win32Native.WindowLongType index);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Ansi)]
    private static IntPtr GetWindowLong64(HandleRef hwnd, Win32Native.WindowLongType index);

    public static IntPtr SetWindowLong(HandleRef hwnd, Win32Native.WindowLongType index, IntPtr wndProcPtr)
    {
      if (IntPtr.Size == 4)
        return Win32Native.SetWindowLong32(hwnd, index, wndProcPtr);
      else
        return Win32Native.SetWindowLongPtr64(hwnd, index, wndProcPtr);
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Ansi)]
    private static IntPtr SetWindowLong32(HandleRef hwnd, Win32Native.WindowLongType index, IntPtr wndProc);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Ansi)]
    private static IntPtr SetWindowLongPtr64(HandleRef hwnd, Win32Native.WindowLongType index, IntPtr wndProc);

    [DllImport("user32.dll", CharSet = CharSet.Ansi)]
    public static IntPtr CallWindowProc(IntPtr wndProc, IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    public static IntPtr GetModuleHandle(string lpModuleName);

    [StructLayout(LayoutKind.Sequential)]
    public class LogFont
    {
      public int lfHeight;
      public int lfWidth;
      public int lfEscapement;
      public int lfOrientation;
      public int lfWeight;
      public byte lfItalic;
      public byte lfUnderline;
      public byte lfStrikeOut;
      public byte lfCharSet;
      public byte lfOutPrecision;
      public byte lfClipPrecision;
      public byte lfQuality;
      public byte lfPitchAndFamily;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string lfFaceName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct TextMetric
    {
      public int tmHeight;
      public int tmAscent;
      public int tmDescent;
      public int tmInternalLeading;
      public int tmExternalLeading;
      public int tmAveCharWidth;
      public int tmMaxCharWidth;
      public int tmWeight;
      public int tmOverhang;
      public int tmDigitizedAspectX;
      public int tmDigitizedAspectY;
      public char tmFirstChar;
      public char tmLastChar;
      public char tmDefaultChar;
      public char tmBreakChar;
      public byte tmItalic;
      public byte tmUnderlined;
      public byte tmStruckOut;
      public byte tmPitchAndFamily;
      public byte tmCharSet;
    }

    public struct TextMetricA
    {
      public int tmHeight;
      public int tmAscent;
      public int tmDescent;
      public int tmInternalLeading;
      public int tmExternalLeading;
      public int tmAveCharWidth;
      public int tmMaxCharWidth;
      public int tmWeight;
      public int tmOverhang;
      public int tmDigitizedAspectX;
      public int tmDigitizedAspectY;
      public byte tmFirstChar;
      public byte tmLastChar;
      public byte tmDefaultChar;
      public byte tmBreakChar;
      public byte tmItalic;
      public byte tmUnderlined;
      public byte tmStruckOut;
      public byte tmPitchAndFamily;
      public byte tmCharSet;
    }

    public struct NativeMessage
    {
      public IntPtr handle;
      public uint msg;
      public IntPtr wParam;
      public IntPtr lParam;
      public uint time;
      public DrawingPoint p;
    }

    public enum WindowLongType
    {
      UserData = -21,
      ExtendedStyle = -20,
      Style = -16,
      Id = -12,
      HwndParent = -8,
      HInstance = -6,
      WndProc = -4,
    }

    public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
  }
}
