// Type: OpenTK.Platform.Windows.WinRawKeyboard
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
  internal sealed class WinRawKeyboard : IKeyboardDriver2
  {
    private static readonly WinKeyMap KeyMap = new WinKeyMap();
    private readonly List<KeyboardState> keyboards = new List<KeyboardState>();
    private readonly List<string> names = new List<string>();
    private readonly Dictionary<ContextHandle, int> rawids = new Dictionary<ContextHandle, int>();
    private readonly object UpdateLock = new object();
    private readonly IntPtr window;

    static WinRawKeyboard()
    {
    }

    public WinRawKeyboard(IntPtr windowHandle)
    {
      this.window = windowHandle;
      this.RefreshDevices();
    }

    public void RefreshDevices()
    {
      lock (this.UpdateLock)
      {
        for (int local_0 = 0; local_0 < this.keyboards.Count; ++local_0)
        {
          KeyboardState local_1 = this.keyboards[local_0];
          local_1.IsConnected = false;
          this.keyboards[local_0] = local_1;
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
            KeyboardState local_7 = this.keyboards[this.rawids[local_6]];
            local_7.IsConnected = true;
            this.keyboards[this.rawids[local_6]] = local_7;
          }
          else
          {
            string local_8 = WinRawKeyboard.GetDeviceName(item_0);
            if (!local_8.ToLower().Contains("root") && (item_0.Type == RawInputDeviceType.KEYBOARD || item_0.Type == RawInputDeviceType.HID))
            {
              RegistryKey local_9 = WinRawKeyboard.GetRegistryKey(local_8);
              string local_10 = (string) local_9.GetValue("DeviceDesc");
              string local_11 = (string) local_9.GetValue("Class");
              if (!string.IsNullOrEmpty(local_10))
              {
                string local_10_1 = local_10.Substring(local_10.LastIndexOf(';') + 1);
                if (!string.IsNullOrEmpty(local_11) && local_11.ToLower().Equals("keyboard"))
                {
                  RawInputDeviceInfo local_12 = new RawInputDeviceInfo();
                  int local_13 = API.RawInputDeviceInfoSize;
                  Functions.GetRawInputDeviceInfo(item_0.Device, RawInputDeviceInfoEnum.DEVICEINFO, local_12, out local_13);
                  WinRawKeyboard.RegisterKeyboardDevice(this.window, local_10_1);
                  this.keyboards.Add(new KeyboardState()
                  {
                    IsConnected = true
                  });
                  this.names.Add(local_10_1);
                  this.rawids.Add(new ContextHandle(item_0.Device), this.keyboards.Count - 1);
                }
              }
            }
          }
        }
      }
    }

    public bool ProcessKeyboardEvent(RawInput rin)
    {
      bool flag1 = false;
      bool flag2 = rin.Data.Keyboard.Message == 256 || rin.Data.Keyboard.Message == 260;
      ContextHandle key = new ContextHandle(rin.Header.Device);
      if (!this.rawids.ContainsKey(key))
        this.RefreshDevices();
      if (this.keyboards.Count == 0)
        return false;
      int index = this.rawids.ContainsKey(key) ? this.rawids[key] : 0;
      KeyboardState keyboardState = this.keyboards[index];
      switch (rin.Data.Keyboard.VKey)
      {
        case VirtualKeys.SHIFT:
          keyboardState[Key.ShiftLeft] = keyboardState[Key.ShiftRight] = flag2;
          flag1 = true;
          break;
        case VirtualKeys.CONTROL:
          keyboardState[Key.ControlLeft] = keyboardState[Key.ControlRight] = flag2;
          flag1 = true;
          break;
        case VirtualKeys.MENU:
          keyboardState[Key.AltLeft] = keyboardState[Key.AltRight] = flag2;
          flag1 = true;
          break;
        default:
          if (WinRawKeyboard.KeyMap.ContainsKey(rin.Data.Keyboard.VKey))
          {
            keyboardState[WinRawKeyboard.KeyMap[rin.Data.Keyboard.VKey]] = flag2;
            flag1 = true;
            break;
          }
          else
            break;
      }
      lock (this.UpdateLock)
      {
        this.keyboards[index] = keyboardState;
        return flag1;
      }
    }

    private static RegistryKey GetRegistryKey(string name)
    {
      name = name.Substring(4);
      string[] strArray = name.Split(new char[1]
      {
        '#'
      });
      string name1 = string.Format("System\\CurrentControlSet\\Enum\\{0}\\{1}\\{2}", (object) strArray[0], (object) strArray[1], (object) strArray[2]);
      return Registry.LocalMachine.OpenSubKey(name1);
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

    private static void RegisterKeyboardDevice(IntPtr window, string name)
    {
      RawInputDevice[] RawInputDevices = new RawInputDevice[1]
      {
        new RawInputDevice()
      };
      RawInputDevices[0].UsagePage = (short) 1;
      RawInputDevices[0].Usage = (short) 6;
      RawInputDevices[0].Flags = RawInputDeviceFlags.INPUTSINK;
      RawInputDevices[0].Target = window;
      Functions.RegisterRawInputDevices(RawInputDevices, 1, API.RawInputDeviceSize);
    }

    public KeyboardState GetState()
    {
      lock (this.UpdateLock)
      {
        KeyboardState local_0 = new KeyboardState();
        foreach (KeyboardState item_0 in this.keyboards)
          local_0.MergeBits(item_0);
        return local_0;
      }
    }

    public KeyboardState GetState(int index)
    {
      lock (this.UpdateLock)
      {
        if (this.keyboards.Count > index)
          return this.keyboards[index];
        else
          return new KeyboardState();
      }
    }

    public string GetDeviceName(int index)
    {
      lock (this.UpdateLock)
      {
        if (this.names.Count > index)
          return this.names[index];
        else
          return string.Empty;
      }
    }
  }
}
