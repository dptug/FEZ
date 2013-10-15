// Type: OpenTK.Platform.MacOS.Carbon.CG
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS.Carbon
{
  internal static class CG
  {
    private const string appServices = "/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices";

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGGetActiveDisplayList")]
    internal static CGDisplayErr GetActiveDisplayList(int maxDisplays, IntPtr* activeDspys, out int dspyCnt);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGMainDisplayID")]
    internal static IntPtr MainDisplayID();

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplayBounds")]
    internal static HIRect DisplayBounds(IntPtr display);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplayPixelsWide")]
    internal static int DisplayPixelsWide(IntPtr display);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplayPixelsHigh")]
    internal static int DisplayPixelsHigh(IntPtr display);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplayCurrentMode")]
    internal static IntPtr DisplayCurrentMode(IntPtr display);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplayCapture")]
    internal static CGDisplayErr DisplayCapture(IntPtr display);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplayRelease")]
    internal static CGDisplayErr DisplayRelease(IntPtr display);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplayAvailableModes")]
    internal static IntPtr DisplayAvailableModes(IntPtr display);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplaySwitchToMode")]
    internal static IntPtr DisplaySwitchToMode(IntPtr display, IntPtr displayMode);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGWarpMouseCursorPosition")]
    internal static CGError WarpMouseCursorPosition(HIPoint newCursorPosition);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGCursorIsVisible")]
    internal static bool CursorIsVisible();

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplayShowCursor")]
    internal static CGError DisplayShowCursor(IntPtr display);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGDisplayHideCursor")]
    internal static CGError DisplayHideCursor(IntPtr display);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGAssociateMouseAndMouseCursorPosition")]
    internal static CGError AssociateMouseAndMouseCursorPosition(bool connected);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices", EntryPoint = "CGSetLocalEventsSuppressionInterval")]
    internal static CGError SetLocalEventsSuppressionInterval(double seconds);
  }
}
