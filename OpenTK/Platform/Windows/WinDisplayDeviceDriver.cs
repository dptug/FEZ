// Type: OpenTK.Platform.Windows.WinDisplayDeviceDriver
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using Microsoft.Win32;
using OpenTK;
using OpenTK.Platform;
using System;
using System.Collections.Generic;

namespace OpenTK.Platform.Windows
{
  internal sealed class WinDisplayDeviceDriver : DisplayDeviceBase
  {
    private readonly object display_lock = new object();

    public WinDisplayDeviceDriver()
    {
      this.RefreshDisplayDevices();
      SystemEvents.DisplaySettingsChanged += new EventHandler(this.HandleDisplaySettingsChanged);
    }

    ~WinDisplayDeviceDriver()
    {
      SystemEvents.DisplaySettingsChanged -= new EventHandler(this.HandleDisplaySettingsChanged);
    }

    public override sealed bool TryChangeResolution(DisplayDevice device, DisplayResolution resolution)
    {
      DeviceMode lpDevMode = (DeviceMode) null;
      if (resolution != (DisplayResolution) null)
      {
        lpDevMode = new DeviceMode();
        lpDevMode.PelsWidth = resolution.Width;
        lpDevMode.PelsHeight = resolution.Height;
        lpDevMode.BitsPerPel = resolution.BitsPerPixel;
        lpDevMode.DisplayFrequency = (int) resolution.RefreshRate;
        lpDevMode.Fields = 6029312;
      }
      return 0 == Functions.ChangeDisplaySettingsEx((string) device.Id, lpDevMode, IntPtr.Zero, ChangeDisplaySettingsEnum.Fullscreen, IntPtr.Zero);
    }

    public override sealed bool TryRestoreResolution(DisplayDevice device)
    {
      return this.TryChangeResolution(device, (DisplayResolution) null);
    }

    public void RefreshDisplayDevices()
    {
      lock (this.display_lock)
      {
        this.AvailableDevices.Clear();
        DisplayResolution local_1 = (DisplayResolution) null;
        List<DisplayResolution> local_2 = new List<DisplayResolution>();
        bool local_3 = false;
        int local_4 = 0;
        WindowsDisplayDevice local_6 = new WindowsDisplayDevice();
        WindowsDisplayDevice temp_10 = new WindowsDisplayDevice();
        while (Functions.EnumDisplayDevices((string) null, local_4++, local_6, 0))
        {
          if ((local_6.StateFlags & DisplayDeviceStateFlags.AttachedToDesktop) != DisplayDeviceStateFlags.None)
          {
            DeviceMode local_7 = new DeviceMode();
            if (Functions.EnumDisplaySettingsEx(((object) local_6.DeviceName).ToString(), DisplayModeSettingsEnum.CurrentSettings, local_7, 0) || Functions.EnumDisplaySettingsEx(((object) local_6.DeviceName).ToString(), DisplayModeSettingsEnum.RegistrySettings, local_7, 0))
            {
              local_1 = new DisplayResolution(local_7.Position.X, local_7.Position.Y, local_7.PelsWidth, local_7.PelsHeight, local_7.BitsPerPel, (float) local_7.DisplayFrequency);
              local_3 = (local_6.StateFlags & DisplayDeviceStateFlags.PrimaryDevice) != DisplayDeviceStateFlags.None;
            }
            local_2.Clear();
            int local_5_1 = 0;
            while (Functions.EnumDisplaySettings(((object) local_6.DeviceName).ToString(), local_5_1++, local_7))
            {
              DisplayResolution local_8 = new DisplayResolution(local_7.Position.X, local_7.Position.Y, local_7.PelsWidth, local_7.PelsHeight, local_7.BitsPerPel, (float) local_7.DisplayFrequency);
              local_2.Add(local_8);
            }
            DisplayDevice local_0 = new DisplayDevice(local_1, local_3, (IEnumerable<DisplayResolution>) local_2, local_1.Bounds, (object) local_6.DeviceName);
            this.AvailableDevices.Add(local_0);
            if (local_3)
              this.Primary = local_0;
          }
        }
      }
    }

    private void HandleDisplaySettingsChanged(object sender, EventArgs e)
    {
      this.RefreshDisplayDevices();
    }
  }
}
