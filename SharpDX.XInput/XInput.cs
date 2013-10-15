// Type: SharpDX.XInput.XInput
// Assembly: SharpDX.XInput, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 7810FD3A-F5EE-4EAB-B451-0D4E18D9FE4F
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.XInput.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SharpDX.XInput
{
  internal static class XInput
  {
    public static unsafe int XInputGetKeystroke(int dwUserIndex, int dwReserved, out Keystroke keystrokeRef)
    {
      keystrokeRef = new Keystroke();
      int keystroke;
      fixed (Keystroke* keystrokePtr = &keystrokeRef)
        keystroke = XInput.XInputGetKeystroke_(dwUserIndex, dwReserved, (void*) keystrokePtr);
      return keystroke;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("xinput1_3.dll", EntryPoint = "XInputGetKeystroke", CallingConvention = CallingConvention.StdCall)]
    private static int XInputGetKeystroke_(int arg0, int arg1, void* arg2);

    public static unsafe int XInputSetState(int dwUserIndex, Vibration vibrationRef)
    {
      return XInput.XInputSetState_(dwUserIndex, (void*) &vibrationRef);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("xinput1_3.dll", EntryPoint = "XInputSetState", CallingConvention = CallingConvention.StdCall)]
    private static int XInputSetState_(int arg0, void* arg1);

    public static unsafe int XInputGetDSoundAudioDeviceGuids(int dwUserIndex, out Guid dSoundRenderGuidRef, out Guid dSoundCaptureGuidRef)
    {
      dSoundRenderGuidRef = new Guid();
      dSoundCaptureGuidRef = new Guid();
      int audioDeviceGuids;
      fixed (Guid* guidPtr1 = &dSoundRenderGuidRef)
        fixed (Guid* guidPtr2 = &dSoundCaptureGuidRef)
          audioDeviceGuids = XInput.XInputGetDSoundAudioDeviceGuids_(dwUserIndex, (void*) guidPtr1, (void*) guidPtr2);
      return audioDeviceGuids;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("xinput1_3.dll", EntryPoint = "XInputGetDSoundAudioDeviceGuids", CallingConvention = CallingConvention.StdCall)]
    private static int XInputGetDSoundAudioDeviceGuids_(int arg0, void* arg1, void* arg2);

    public static unsafe int XInputGetState(int dwUserIndex, out State stateRef)
    {
      stateRef = new State();
      int state;
      fixed (State* statePtr = &stateRef)
        state = XInput.XInputGetState_(dwUserIndex, (void*) statePtr);
      return state;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("xinput1_3.dll", EntryPoint = "XInputGetState", CallingConvention = CallingConvention.StdCall)]
    private static int XInputGetState_(int arg0, void* arg1);

    public static unsafe int XInputGetCapabilities(int dwUserIndex, DeviceQueryType dwFlags, out Capabilities capabilitiesRef)
    {
      capabilitiesRef = new Capabilities();
      int capabilities;
      fixed (Capabilities* capabilitiesPtr = &capabilitiesRef)
        capabilities = XInput.XInputGetCapabilities_(dwUserIndex, (int) dwFlags, (void*) capabilitiesPtr);
      return capabilities;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("xinput1_3.dll", EntryPoint = "XInputGetCapabilities", CallingConvention = CallingConvention.StdCall)]
    private static int XInputGetCapabilities_(int arg0, int arg1, void* arg2);

    public static unsafe int XInputGetBatteryInformation(int dwUserIndex, BatteryDeviceType devType, out BatteryInformation batteryInformationRef)
    {
      batteryInformationRef = new BatteryInformation();
      int batteryInformation;
      fixed (BatteryInformation* batteryInformationPtr = &batteryInformationRef)
        batteryInformation = XInput.XInputGetBatteryInformation_(dwUserIndex, (int) devType, (void*) batteryInformationPtr);
      return batteryInformation;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("xinput1_3.dll", EntryPoint = "XInputGetBatteryInformation", CallingConvention = CallingConvention.StdCall)]
    private static int XInputGetBatteryInformation_(int arg0, int arg1, void* arg2);

    public static void XInputEnable(Bool enable)
    {
      XInput.XInputEnable_(enable);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("xinput1_3.dll", EntryPoint = "XInputEnable", CallingConvention = CallingConvention.StdCall)]
    private static void XInputEnable_(Bool arg0);
  }
}
