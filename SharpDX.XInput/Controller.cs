// Type: SharpDX.XInput.Controller
// Assembly: SharpDX.XInput, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 7810FD3A-F5EE-4EAB-B451-0D4E18D9FE4F
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.XInput.dll

using SharpDX;
using SharpDX.Win32;
using System;

namespace SharpDX.XInput
{
  public class Controller
  {
    private readonly UserIndex userIndex;

    public UserIndex UserIndex
    {
      get
      {
        return this.userIndex;
      }
    }

    public bool IsConnected
    {
      get
      {
        State stateRef;
        return XInput.XInputGetState((int) this.userIndex, out stateRef) == 0;
      }
    }

    public Guid SoundRenderGuid
    {
      get
      {
        Guid dSoundRenderGuidRef;
        Guid dSoundCaptureGuidRef;
        XInput.XInputGetDSoundAudioDeviceGuids((int) this.userIndex, out dSoundRenderGuidRef, out dSoundCaptureGuidRef);
        return dSoundRenderGuidRef;
      }
    }

    public Guid SoundCaptureGuid
    {
      get
      {
        Guid dSoundRenderGuidRef;
        Guid dSoundCaptureGuidRef;
        XInput.XInputGetDSoundAudioDeviceGuids((int) this.userIndex, out dSoundRenderGuidRef, out dSoundCaptureGuidRef);
        return dSoundCaptureGuidRef;
      }
    }

    public Controller(UserIndex userIndex = UserIndex.Any)
    {
      this.userIndex = userIndex;
    }

    public BatteryInformation GetBatteryInformation(BatteryDeviceType batteryDeviceType)
    {
      BatteryInformation batteryInformationRef;
      ErrorCodeHelper.ToResult(XInput.XInputGetBatteryInformation((int) this.userIndex, batteryDeviceType, out batteryInformationRef)).CheckError();
      return batteryInformationRef;
    }

    public Capabilities GetCapabilities(DeviceQueryType deviceQueryType)
    {
      Capabilities capabilitiesRef;
      ErrorCodeHelper.ToResult(XInput.XInputGetCapabilities((int) this.userIndex, deviceQueryType, out capabilitiesRef)).CheckError();
      return capabilitiesRef;
    }

    public Result GetKeystroke(DeviceQueryType deviceQueryType, out Keystroke keystroke)
    {
      Result result = ErrorCodeHelper.ToResult(XInput.XInputGetKeystroke((int) this.userIndex, (int) deviceQueryType, out keystroke));
      result.CheckError();
      return result;
    }

    public State GetState()
    {
      State stateRef;
      ErrorCodeHelper.ToResult(XInput.XInputGetState((int) this.userIndex, out stateRef)).CheckError();
      return stateRef;
    }

    public bool GetState(out State state)
    {
      return XInput.XInputGetState((int) this.userIndex, out state) == 0;
    }

    public static void SetReporting(bool enableReporting)
    {
      XInput.XInputEnable((Bool) enableReporting);
    }

    public Result SetVibration(Vibration vibration)
    {
      Result result = ErrorCodeHelper.ToResult(XInput.XInputSetState((int) this.userIndex, vibration));
      result.CheckError();
      return result;
    }
  }
}
