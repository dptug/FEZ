// Type: OpenTK.Platform.MacOS.Carbon.CF
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS.Carbon
{
  internal class CF
  {
    public static readonly IntPtr RunLoopModeDefault = CF.CFSTR("kCFRunLoopDefaultMode");
    private const string appServices = "/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices";

    static CF()
    {
    }

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    internal static int CFArrayGetCount(IntPtr theArray);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    internal static IntPtr CFArrayGetValueAtIndex(IntPtr theArray, int idx);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    internal static int CFDictionaryGetCount(IntPtr theDictionary);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    internal static IntPtr CFDictionaryGetValue(IntPtr theDictionary, IntPtr theKey);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    private static IntPtr __CFStringMakeConstantString(string cStr);

    internal static IntPtr CFSTR(string cStr)
    {
      return CF.__CFStringMakeConstantString(cStr);
    }

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    internal static bool CFNumberGetValue(IntPtr number, CF.CFNumberType theType, int* valuePtr);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    internal static bool CFNumberGetValue(IntPtr number, CF.CFNumberType theType, double* valuePtr);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    internal static IntPtr CFRunLoopGetCurrent();

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    internal static IntPtr CFRunLoopGetMain();

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    internal static CF.CFRunLoopExitReason CFRunLoopRunInMode(IntPtr cfstrMode, double interval, bool returnAfterSourceHandled);

    internal enum CFNumberType
    {
      kCFNumberSInt8Type = 1,
      kCFNumberSInt16Type = 2,
      kCFNumberSInt32Type = 3,
      kCFNumberSInt64Type = 4,
      kCFNumberFloat32Type = 5,
      kCFNumberFloat64Type = 6,
      kCFNumberCharType = 7,
      kCFNumberShortType = 8,
      kCFNumberIntType = 9,
      kCFNumberLongType = 10,
      kCFNumberLongLongType = 11,
      kCFNumberFloatType = 12,
      kCFNumberDoubleType = 13,
      kCFNumberCFIndexType = 14,
      kCFNumberNSIntegerType = 15,
      kCFNumberCGFloatType = 16,
      kCFNumberMaxType = 16,
    }

    public enum CFRunLoopExitReason
    {
      Finished = 1,
      Stopped = 2,
      TimedOut = 3,
      HandledSource = 4,
    }
  }
}
