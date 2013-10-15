// Type: OpenTK.Platform.X11.X11DisplayDevice
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  internal sealed class X11DisplayDevice : DisplayDeviceBase
  {
    private readonly List<Dictionary<DisplayResolution, int>> screenResolutionToIndex = new List<Dictionary<DisplayResolution, int>>();
    private readonly Dictionary<DisplayDevice, int> deviceToDefaultResolution = new Dictionary<DisplayDevice, int>();
    private readonly Dictionary<DisplayDevice, int> deviceToScreen = new Dictionary<DisplayDevice, int>();
    private readonly List<IntPtr> lastConfigUpdate = new List<IntPtr>();
    private bool xinerama_supported;
    private bool xrandr_supported;
    private bool xf86_supported;

    public X11DisplayDevice()
    {
      this.RefreshDisplayDevices();
    }

    private void RefreshDisplayDevices()
    {
      using (new XLock(API.DefaultDisplay))
      {
        List<DisplayDevice> devices = new List<DisplayDevice>();
        this.xinerama_supported = false;
        try
        {
          this.xinerama_supported = this.QueryXinerama(devices);
        }
        catch
        {
        }
        if (!this.xinerama_supported)
        {
          for (int index = 0; index < API.ScreenCount; ++index)
          {
            DisplayDevice key = new DisplayDevice();
            key.IsPrimary = index == Functions.XDefaultScreen(API.DefaultDisplay);
            devices.Add(key);
            this.deviceToScreen.Add(key, index);
          }
        }
        try
        {
          this.xrandr_supported = this.QueryXRandR(devices);
        }
        catch
        {
        }
        if (!this.xrandr_supported)
        {
          try
          {
            this.xf86_supported = this.QueryXF86(devices);
          }
          catch
          {
          }
          int num = this.xf86_supported ? 1 : 0;
        }
        this.AvailableDevices.Clear();
        this.AvailableDevices.AddRange((IEnumerable<DisplayDevice>) devices);
        this.Primary = X11DisplayDevice.FindDefaultDevice((IEnumerable<DisplayDevice>) devices);
      }
    }

    private static DisplayDevice FindDefaultDevice(IEnumerable<DisplayDevice> devices)
    {
      foreach (DisplayDevice displayDevice in devices)
      {
        if (displayDevice.IsPrimary)
          return displayDevice;
      }
      throw new InvalidOperationException("No primary display found. Please file a bug at http://www.opentk.com");
    }

    private bool QueryXinerama(List<DisplayDevice> devices)
    {
      int event_basep;
      int error_basep;
      if (X11DisplayDevice.NativeMethods.XineramaQueryExtension(API.DefaultDisplay, out event_basep, out error_basep) && X11DisplayDevice.NativeMethods.XineramaIsActive(API.DefaultDisplay))
      {
        IList<X11DisplayDevice.XineramaScreenInfo> list = X11DisplayDevice.NativeMethods.XineramaQueryScreens(API.DefaultDisplay);
        bool flag = true;
        foreach (X11DisplayDevice.XineramaScreenInfo xineramaScreenInfo in (IEnumerable<X11DisplayDevice.XineramaScreenInfo>) list)
        {
          DisplayDevice key = new DisplayDevice();
          key.Bounds = new System.Drawing.Rectangle((int) xineramaScreenInfo.X, (int) xineramaScreenInfo.Y, (int) xineramaScreenInfo.Width, (int) xineramaScreenInfo.Height);
          if (flag)
          {
            key.IsPrimary = true;
            flag = false;
          }
          devices.Add(key);
          this.deviceToScreen.Add(key, 0);
        }
      }
      return devices.Count > 0;
    }

    private bool QueryXRandR(List<DisplayDevice> devices)
    {
      foreach (DisplayDevice key1 in devices)
      {
        int index1 = this.deviceToScreen[key1];
        IntPtr config_timestamp;
        Functions.XRRTimes(API.DefaultDisplay, index1, out config_timestamp);
        this.lastConfigUpdate.Add(config_timestamp);
        List<DisplayResolution> list = new List<DisplayResolution>();
        this.screenResolutionToIndex.Add(new Dictionary<DisplayResolution, int>());
        int[] availableDepths = X11DisplayDevice.FindAvailableDepths(index1);
        int size_index = 0;
        foreach (XRRScreenSize xrrScreenSize in X11DisplayDevice.FindAvailableResolutions(index1))
        {
          if (xrrScreenSize.Width != 0 && xrrScreenSize.Height != 0)
          {
            short[] numArray = Functions.XRRRates(API.DefaultDisplay, index1, size_index);
            foreach (short num in numArray)
            {
              if ((int) num != 0 || numArray.Length == 1)
              {
                foreach (int bitsPerPixel in availableDepths)
                  list.Add(new DisplayResolution(0, 0, xrrScreenSize.Width, xrrScreenSize.Height, bitsPerPixel, (float) num));
              }
            }
            foreach (int bitsPerPixel in availableDepths)
            {
              DisplayResolution key2 = new DisplayResolution(0, 0, xrrScreenSize.Width, xrrScreenSize.Height, bitsPerPixel, 0.0f);
              if (!this.screenResolutionToIndex[index1].ContainsKey(key2))
                this.screenResolutionToIndex[index1].Add(key2, size_index);
            }
            ++size_index;
          }
        }
        float currentRefreshRate = X11DisplayDevice.FindCurrentRefreshRate(index1);
        int currentDepth = X11DisplayDevice.FindCurrentDepth(index1);
        ushort rotation;
        int index2 = (int) Functions.XRRConfigCurrentConfiguration(Functions.XRRGetScreenInfo(API.DefaultDisplay, Functions.XRootWindow(API.DefaultDisplay, index1)), out rotation);
        if (key1.Bounds == System.Drawing.Rectangle.Empty)
          key1.Bounds = new System.Drawing.Rectangle(0, 0, list[index2].Width, list[index2].Height);
        key1.BitsPerPixel = currentDepth;
        key1.RefreshRate = currentRefreshRate;
        key1.AvailableResolutions = (IList<DisplayResolution>) list;
        this.deviceToDefaultResolution.Add(key1, index2);
      }
      return true;
    }

    private bool QueryXF86(List<DisplayDevice> devices)
    {
      try
      {
        int major_version_return;
        int minor_version_return;
        if (!API.XF86VidModeQueryVersion(API.DefaultDisplay, out major_version_return, out minor_version_return))
          return false;
      }
      catch (DllNotFoundException ex)
      {
        return false;
      }
      int screen = 0;
      foreach (DisplayDevice displayDevice in devices)
      {
        int modecount_return;
        IntPtr modesinfo;
        API.XF86VidModeGetAllModeLines(API.DefaultDisplay, screen, out modecount_return, out modesinfo);
        IntPtr[] destination = new IntPtr[modecount_return];
        Marshal.Copy(modesinfo, destination, 0, modecount_return);
        API.XF86VidModeModeInfo xf86VidModeModeInfo = new API.XF86VidModeModeInfo();
        int x_return;
        int y_return;
        API.XF86VidModeGetViewPort(API.DefaultDisplay, screen, out x_return, out y_return);
        List<DisplayResolution> list = new List<DisplayResolution>();
        for (int index = 0; index < modecount_return; ++index)
        {
          xf86VidModeModeInfo = (API.XF86VidModeModeInfo) Marshal.PtrToStructure(destination[index], typeof (API.XF86VidModeModeInfo));
          list.Add(new DisplayResolution(x_return, y_return, (int) xf86VidModeModeInfo.hdisplay, (int) xf86VidModeModeInfo.vdisplay, 24, (float) xf86VidModeModeInfo.dotclock * 1000f / (float) ((int) xf86VidModeModeInfo.vtotal * (int) xf86VidModeModeInfo.htotal)));
        }
        displayDevice.AvailableResolutions = (IList<DisplayResolution>) list;
        int dotclock_return;
        API.XF86VidModeModeLine modeline;
        API.XF86VidModeGetModeLine(API.DefaultDisplay, screen, out dotclock_return, out modeline);
        displayDevice.Bounds = new System.Drawing.Rectangle(x_return, y_return, (int) modeline.hdisplay, (int) modeline.vdisplay == 0 ? (int) modeline.vsyncstart : (int) modeline.vdisplay);
        displayDevice.BitsPerPixel = X11DisplayDevice.FindCurrentDepth(screen);
        displayDevice.RefreshRate = (float) dotclock_return * 1000f / (float) ((int) modeline.vtotal * (int) modeline.htotal);
        ++screen;
      }
      return true;
    }

    private static int[] FindAvailableDepths(int screen)
    {
      return Functions.XListDepths(API.DefaultDisplay, screen);
    }

    private static XRRScreenSize[] FindAvailableResolutions(int screen)
    {
      XRRScreenSize[] xrrScreenSizeArray = Functions.XRRSizes(API.DefaultDisplay, screen);
      if (xrrScreenSizeArray == null)
        throw new NotSupportedException("XRandR extensions not available.");
      else
        return xrrScreenSizeArray;
    }

    private static float FindCurrentRefreshRate(int screen)
    {
      IntPtr screenInfo = Functions.XRRGetScreenInfo(API.DefaultDisplay, Functions.XRootWindow(API.DefaultDisplay, screen));
      ushort rotation = (ushort) 0;
      int num1 = (int) Functions.XRRConfigCurrentConfiguration(screenInfo, out rotation);
      short num2 = Functions.XRRConfigCurrentRate(screenInfo);
      Functions.XRRFreeScreenConfigInfo(screenInfo);
      return (float) num2;
    }

    private static int FindCurrentDepth(int screen)
    {
      return (int) Functions.XDefaultDepth(API.DefaultDisplay, screen);
    }

    private bool ChangeResolutionXRandR(DisplayDevice device, DisplayResolution resolution)
    {
      using (new XLock(API.DefaultDisplay))
      {
        int screen_number = this.deviceToScreen[device];
        IntPtr draw = Functions.XRootWindow(API.DefaultDisplay, screen_number);
        IntPtr screenInfo = Functions.XRRGetScreenInfo(API.DefaultDisplay, draw);
        ushort rotation;
        int num = (int) Functions.XRRConfigCurrentConfiguration(screenInfo, out rotation);
        int size_index = !(resolution != (DisplayResolution) null) ? this.deviceToDefaultResolution[device] : this.screenResolutionToIndex[screen_number][new DisplayResolution(0, 0, resolution.Width, resolution.Height, resolution.BitsPerPixel, 0.0f)];
        short rate = resolution != (DisplayResolution) null ? (short) resolution.RefreshRate : (short) 0;
        return ((int) rate <= 0 ? Functions.XRRSetScreenConfig(API.DefaultDisplay, screenInfo, draw, size_index, rotation, IntPtr.Zero) : Functions.XRRSetScreenConfigAndRate(API.DefaultDisplay, screenInfo, draw, size_index, rotation, rate, IntPtr.Zero)) == 0;
      }
    }

    private static bool ChangeResolutionXF86(DisplayDevice device, DisplayResolution resolution)
    {
      return false;
    }

    public override sealed bool TryChangeResolution(DisplayDevice device, DisplayResolution resolution)
    {
      if (this.xrandr_supported)
        return this.ChangeResolutionXRandR(device, resolution);
      if (this.xf86_supported)
        return X11DisplayDevice.ChangeResolutionXF86(device, resolution);
      else
        return false;
    }

    public override sealed bool TryRestoreResolution(DisplayDevice device)
    {
      return this.TryChangeResolution(device, (DisplayResolution) null);
    }

    private static class NativeMethods
    {
      private const string Xinerama = "libXinerama";

      [DllImport("libXinerama")]
      public static bool XineramaQueryExtension(IntPtr dpy, out int event_basep, out int error_basep);

      [DllImport("libXinerama")]
      public static int XineramaQueryVersion(IntPtr dpy, out int major_versionp, out int minor_versionp);

      [DllImport("libXinerama")]
      public static bool XineramaIsActive(IntPtr dpy);

      [DllImport("libXinerama")]
      private static IntPtr XineramaQueryScreens(IntPtr dpy, out int number);

      public static unsafe IList<X11DisplayDevice.XineramaScreenInfo> XineramaQueryScreens(IntPtr dpy)
      {
        int number;
        IntPtr num = X11DisplayDevice.NativeMethods.XineramaQueryScreens(dpy, out number);
        List<X11DisplayDevice.XineramaScreenInfo> list = new List<X11DisplayDevice.XineramaScreenInfo>(number);
        X11DisplayDevice.XineramaScreenInfo* xineramaScreenInfoPtr = (X11DisplayDevice.XineramaScreenInfo*) (void*) num;
        while (--number >= 0)
        {
          list.Add(*xineramaScreenInfoPtr);
          ++xineramaScreenInfoPtr;
        }
        return (IList<X11DisplayDevice.XineramaScreenInfo>) list;
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct XineramaScreenInfo
    {
      public int ScreenNumber;
      public short X;
      public short Y;
      public short Width;
      public short Height;
    }
  }
}
