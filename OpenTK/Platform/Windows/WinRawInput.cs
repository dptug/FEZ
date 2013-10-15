// Type: OpenTK.Platform.Windows.WinRawInput
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Input;
using System;

namespace OpenTK.Platform.Windows
{
  internal sealed class WinRawInput : WinInputBase
  {
    private static RawInput data = new RawInput();
    private static readonly int rawInputStructSize = API.RawInputSize;
    private static readonly Guid DeviceInterfaceHid = new Guid("4D1E55B2-F16F-11CF-88CB-001111000030");
    private WinRawKeyboard keyboard_driver;
    private WinRawMouse mouse_driver;
    private WinMMJoystick joystick_driver;
    private IntPtr DevNotifyHandle;

    public static int DeviceCount
    {
      get
      {
        int NumDevices = 0;
        Functions.GetRawInputDeviceList((RawInputDeviceList[]) null, out NumDevices, API.RawInputDeviceListSize);
        return NumDevices;
      }
    }

    public override IKeyboardDriver2 KeyboardDriver
    {
      get
      {
        return (IKeyboardDriver2) this.keyboard_driver;
      }
    }

    public override IMouseDriver2 MouseDriver
    {
      get
      {
        return (IMouseDriver2) this.mouse_driver;
      }
    }

    public override IGamePadDriver GamePadDriver
    {
      get
      {
        return (IGamePadDriver) this.joystick_driver;
      }
    }

    static WinRawInput()
    {
    }

    private static unsafe IntPtr RegisterForDeviceNotifications(WinWindowInfo parent)
    {
      BroadcastDeviceInterface type = new BroadcastDeviceInterface();
      type.Size = BlittableValueType.StrideOf<BroadcastDeviceInterface>(type);
      type.DeviceType = DeviceBroadcastType.INTERFACE;
      type.ClassGuid = WinRawInput.DeviceInterfaceHid;
      IntPtr num1 = Functions.RegisterDeviceNotification(parent.WindowHandle, new IntPtr((void*) &type), DeviceNotification.WINDOW_HANDLE);
      int num2 = num1 == IntPtr.Zero ? 1 : 0;
      return num1;
    }

    protected override IntPtr WindowProcedure(IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam)
    {
      switch (message)
      {
        case WindowMessage.INPUT:
          int Size = 0;
          Functions.GetRawInputData(lParam, GetRawInputDataEnum.INPUT, IntPtr.Zero, out Size, API.RawInputHeaderSize);
          if (Size == Functions.GetRawInputData(lParam, GetRawInputDataEnum.INPUT, out WinRawInput.data, out Size, API.RawInputHeaderSize))
          {
            switch (WinRawInput.data.Header.Type)
            {
              case RawInputDeviceType.MOUSE:
                if (((WinRawMouse) this.MouseDriver).ProcessMouseEvent(WinRawInput.data))
                  return IntPtr.Zero;
                else
                  break;
              case RawInputDeviceType.KEYBOARD:
                if (((WinRawKeyboard) this.KeyboardDriver).ProcessKeyboardEvent(WinRawInput.data))
                  return IntPtr.Zero;
                else
                  break;
            }
          }
          else
            break;
        case WindowMessage.DEVICECHANGE:
          ((WinRawKeyboard) this.KeyboardDriver).RefreshDevices();
          ((WinRawMouse) this.MouseDriver).RefreshDevices();
          break;
      }
      return base.WindowProcedure(handle, message, wParam, lParam);
    }

    protected override void CreateDrivers()
    {
      this.keyboard_driver = new WinRawKeyboard(this.Parent.WindowHandle);
      this.mouse_driver = new WinRawMouse(this.Parent.WindowHandle);
      this.joystick_driver = new WinMMJoystick();
      this.DevNotifyHandle = WinRawInput.RegisterForDeviceNotifications(this.Parent);
    }

    protected override void Dispose(bool manual)
    {
      if (this.Disposed)
        return;
      Functions.UnregisterDeviceNotification(this.DevNotifyHandle);
      base.Dispose(manual);
    }
  }
}
