// Type: OpenTK.Platform.MacOS.QuartzDisplayDeviceDriver
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Platform;
using OpenTK.Platform.MacOS.Carbon;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenTK.Platform.MacOS
{
  internal sealed class QuartzDisplayDeviceDriver : DisplayDeviceBase
  {
    private static object display_lock = new object();
    private Dictionary<IntPtr, IntPtr> storedModes = new Dictionary<IntPtr, IntPtr>();
    private List<IntPtr> displaysCaptured = new List<IntPtr>();

    static QuartzDisplayDeviceDriver()
    {
    }

    public unsafe QuartzDisplayDeviceDriver()
    {
      lock (QuartzDisplayDeviceDriver.display_lock)
      {
        IntPtr[] local_0 = new IntPtr[20];
        int local_1;
        fixed (IntPtr* fixed_0 = local_0)
        {
          int temp_15 = (int) CG.GetActiveDisplayList(20, fixed_0, out local_1);
        }
        for (int local_3 = 0; local_3 < local_1; ++local_3)
        {
          IntPtr local_4 = local_0[local_3];
          bool local_5 = local_3 == 0;
          CG.DisplayPixelsWide(local_4);
          CG.DisplayPixelsHigh(local_4);
          CFArray local_7 = new CFArray(CG.DisplayAvailableModes(local_4));
          DisplayResolution local_8 = (DisplayResolution) null;
          List<DisplayResolution> local_9 = new List<DisplayResolution>();
          CFDictionary local_11 = new CFDictionary(CG.DisplayCurrentMode(local_4));
          for (int local_12 = 0; local_12 < local_7.Count; ++local_12)
          {
            CFDictionary local_13 = new CFDictionary(local_7[local_12]);
            int local_14 = (int) local_13.GetNumberValue("Width");
            int local_15 = (int) local_13.GetNumberValue("Height");
            int local_16 = (int) local_13.GetNumberValue("BitsPerPixel");
            double local_17 = local_13.GetNumberValue("RefreshRate");
            bool local_18 = local_11.Ref == local_13.Ref;
            DisplayResolution local_19 = new DisplayResolution(0, 0, local_14, local_15, local_16, (float) local_17);
            local_9.Add(local_19);
            if (local_18)
              local_8 = local_19;
          }
          HIRect local_20 = CG.DisplayBounds(local_4);
          Rectangle local_21 = new Rectangle((int) local_20.Origin.X, (int) local_20.Origin.Y, (int) local_20.Size.Width, (int) local_20.Size.Height);
          DisplayDevice local_22 = new DisplayDevice(local_8, local_5, (IEnumerable<DisplayResolution>) local_9, local_21, (object) local_4);
          this.AvailableDevices.Add(local_22);
          if (local_5)
            this.Primary = local_22;
        }
      }
    }

    internal static IntPtr HandleTo(DisplayDevice displayDevice)
    {
      return (IntPtr) displayDevice.Id;
    }

    public override sealed bool TryChangeResolution(DisplayDevice device, DisplayResolution resolution)
    {
      IntPtr num1 = QuartzDisplayDeviceDriver.HandleTo(device);
      IntPtr num2 = CG.DisplayCurrentMode(num1);
      if (!this.storedModes.ContainsKey(num1))
        this.storedModes.Add(num1, num2);
      CFArray cfArray = new CFArray(CG.DisplayAvailableModes(num1));
      for (int index = 0; index < cfArray.Count; ++index)
      {
        CFDictionary cfDictionary = new CFDictionary(cfArray[index]);
        int num3 = (int) cfDictionary.GetNumberValue("Width");
        int num4 = (int) cfDictionary.GetNumberValue("Height");
        int num5 = (int) cfDictionary.GetNumberValue("BitsPerPixel");
        double numberValue = cfDictionary.GetNumberValue("RefreshRate");
        if (num3 == resolution.Width && num4 == resolution.Height && (num5 == resolution.BitsPerPixel && Math.Abs(numberValue - (double) resolution.RefreshRate) < 1E-06))
        {
          if (!this.displaysCaptured.Contains(num1))
          {
            int num6 = (int) CG.DisplayCapture(num1);
          }
          CG.DisplaySwitchToMode(num1, cfArray[index]);
          return true;
        }
      }
      return false;
    }

    public override sealed bool TryRestoreResolution(DisplayDevice device)
    {
      IntPtr index = QuartzDisplayDeviceDriver.HandleTo(device);
      if (!this.storedModes.ContainsKey(index))
        return false;
      CG.DisplaySwitchToMode(index, this.storedModes[index]);
      int num = (int) CG.DisplayRelease(index);
      this.displaysCaptured.Remove(index);
      return true;
    }
  }
}
