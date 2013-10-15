// Type: OpenTK.Platform.Windows.WinRawMouse
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using Microsoft.Win32;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  internal sealed class WinRawMouse : IMouseDriver2
  {
    private readonly List<MouseState> mice = new List<MouseState>();
    private readonly List<string> names = new List<string>();
    private readonly Dictionary<ContextHandle, int> rawids = new Dictionary<ContextHandle, int>();
    private readonly object UpdateLock = new object();
    private readonly IntPtr Window;

    public WinRawMouse(IntPtr window)
    {
      if (window == IntPtr.Zero)
        throw new ArgumentNullException("window");
      this.Window = window;
      this.RefreshDevices();
    }

    public void RefreshDevices()
    {
      lock (this.UpdateLock)
      {
        for (int local_0 = 0; local_0 < this.mice.Count; ++local_0)
        {
          MouseState local_1 = this.mice[local_0];
          local_1.IsConnected = false;
          this.mice[local_0] = local_1;
        }
        int local_2 = WinRawInput.DeviceCount;
        RawInputDeviceList[] local_3 = new RawInputDeviceList[local_2];
        for (int local_4 = 0; local_4 < local_2; ++local_4)
          local_3[local_4] = new RawInputDeviceList();
        Functions.GetRawInputDeviceList(local_3, out local_2, API.RawInputDeviceListSize);
        foreach (RawInputDeviceList item_0 in local_3)
        {
          ContextHandle local_6 = new ContextHandle(item_0.Device);
          if (this.rawids.ContainsKey(local_6))
          {
            MouseState local_7 = this.mice[this.rawids[local_6]];
            local_7.IsConnected = true;
            this.mice[this.rawids[local_6]] = local_7;
          }
          else
          {
            string local_8 = WinRawMouse.GetDeviceName(item_0);
            if (!local_8.ToLower().Contains("root") && (item_0.Type == RawInputDeviceType.MOUSE || item_0.Type == RawInputDeviceType.HID))
            {
              RegistryKey local_9 = WinRawMouse.FindRegistryKey(local_8);
              string local_10 = (string) local_9.GetValue("DeviceDesc");
              string local_11 = (string) local_9.GetValue("Class");
              string local_10_1 = local_10.Substring(local_10.LastIndexOf(';') + 1);
              if (!string.IsNullOrEmpty(local_11) && local_11.ToLower().Equals("mouse") && !this.rawids.ContainsKey(new ContextHandle(item_0.Device)))
              {
                RawInputDeviceInfo local_12 = new RawInputDeviceInfo();
                int local_13 = API.RawInputDeviceInfoSize;
                Functions.GetRawInputDeviceInfo(item_0.Device, RawInputDeviceInfoEnum.DEVICEINFO, local_12, out local_13);
                WinRawMouse.RegisterRawDevice(this.Window, local_10_1);
                this.mice.Add(new MouseState()
                {
                  IsConnected = true
                });
                this.names.Add(local_10_1);
                this.rawids.Add(new ContextHandle(item_0.Device), this.mice.Count - 1);
              }
            }
          }
        }
      }
    }

    public bool ProcessMouseEvent(RawInput rin)
    {
      RawMouse rawMouse = rin.Data.Mouse;
      ContextHandle key = new ContextHandle(rin.Header.Device);
      if (!this.rawids.ContainsKey(key))
        this.RefreshDevices();
      if (this.mice.Count == 0)
        return false;
      int index = this.rawids.ContainsKey(key) ? this.rawids[key] : 0;
      MouseState mouseState = this.mice[index];
      if ((rawMouse.ButtonFlags & RawInputMouseState.LEFT_BUTTON_DOWN) != (RawInputMouseState) 0)
        mouseState.EnableBit(0);
      if ((rawMouse.ButtonFlags & RawInputMouseState.LEFT_BUTTON_UP) != (RawInputMouseState) 0)
        mouseState.DisableBit(0);
      if ((rawMouse.ButtonFlags & RawInputMouseState.RIGHT_BUTTON_DOWN) != (RawInputMouseState) 0)
        mouseState.EnableBit(2);
      if ((rawMouse.ButtonFlags & RawInputMouseState.RIGHT_BUTTON_UP) != (RawInputMouseState) 0)
        mouseState.DisableBit(2);
      if ((rawMouse.ButtonFlags & RawInputMouseState.MIDDLE_BUTTON_DOWN) != (RawInputMouseState) 0)
        mouseState.EnableBit(1);
      if ((rawMouse.ButtonFlags & RawInputMouseState.MIDDLE_BUTTON_UP) != (RawInputMouseState) 0)
        mouseState.DisableBit(1);
      if ((rawMouse.ButtonFlags & RawInputMouseState.BUTTON_4_DOWN) != (RawInputMouseState) 0)
        mouseState.EnableBit(3);
      if ((rawMouse.ButtonFlags & RawInputMouseState.BUTTON_4_UP) != (RawInputMouseState) 0)
        mouseState.DisableBit(3);
      if ((rawMouse.ButtonFlags & RawInputMouseState.BUTTON_5_DOWN) != (RawInputMouseState) 0)
        mouseState.EnableBit(4);
      if ((rawMouse.ButtonFlags & RawInputMouseState.BUTTON_5_UP) != (RawInputMouseState) 0)
        mouseState.DisableBit(4);
      if ((rawMouse.ButtonFlags & RawInputMouseState.WHEEL) != (RawInputMouseState) 0)
        mouseState.WheelPrecise += (float) (short) rawMouse.ButtonData / 120f;
      if ((rawMouse.Flags & RawMouseFlags.MOUSE_MOVE_ABSOLUTE) != RawMouseFlags.MOUSE_MOVE_RELATIVE)
      {
        mouseState.X = rawMouse.LastX;
        mouseState.Y = rawMouse.LastY;
      }
      else
      {
        mouseState.X += rawMouse.LastX;
        mouseState.Y += rawMouse.LastY;
      }
      lock (this.UpdateLock)
      {
        this.mice[index] = mouseState;
        return true;
      }
    }

    private static string GetDeviceName(RawInputDeviceList dev)
    {
      uint Size = 0U;
      int num1 = (int) Functions.GetRawInputDeviceInfo(dev.Device, RawInputDeviceInfoEnum.DEVICENAME, IntPtr.Zero, out Size);
      IntPtr num2 = Marshal.AllocHGlobal((IntPtr) ((long) Size));
      int num3 = (int) Functions.GetRawInputDeviceInfo(dev.Device, RawInputDeviceInfoEnum.DEVICENAME, num2, out Size);
      string str = Marshal.PtrToStringAnsi(num2);
      Marshal.FreeHGlobal(num2);
      return str;
    }

    private static RegistryKey FindRegistryKey(string name)
    {
      name = name.Substring(4);
      string[] strArray = name.Split(new char[1]
      {
        '#'
      });
      string name1 = string.Format("System\\CurrentControlSet\\Enum\\{0}\\{1}\\{2}", (object) strArray[0], (object) strArray[1], (object) strArray[2]);
      return Registry.LocalMachine.OpenSubKey(name1);
    }

    private static void RegisterRawDevice(IntPtr window, string device)
    {
      RawInputDevice[] RawInputDevices = new RawInputDevice[1]
      {
        new RawInputDevice()
      };
      RawInputDevices[0].UsagePage = (short) 1;
      RawInputDevices[0].Usage = (short) 2;
      RawInputDevices[0].Flags = RawInputDeviceFlags.INPUTSINK;
      RawInputDevices[0].Target = window;
      Functions.RegisterRawInputDevices(RawInputDevices, 1, API.RawInputDeviceSize);
    }

    public MouseState GetState()
    {
      lock (this.UpdateLock)
      {
        MouseState local_0 = new MouseState();
        foreach (MouseState item_0 in this.mice)
          local_0.MergeBits(item_0);
        return local_0;
      }
    }

    public MouseState GetState(int index)
    {
      lock (this.UpdateLock)
      {
        if (this.mice.Count > index)
          return this.mice[index];
        else
          return new MouseState();
      }
    }

    public void SetPosition(double x, double y)
    {
      Functions.SetCursorPos((int) x, (int) y);
    }
  }
}
