// Type: SharpDX.XInput.ResultCode
// Assembly: SharpDX.XInput, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 7810FD3A-F5EE-4EAB-B451-0D4E18D9FE4F
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.XInput.dll

using SharpDX;
using SharpDX.Win32;

namespace SharpDX.XInput
{
  public sealed class ResultCode
  {
    public static readonly Result NotConnected = ErrorCodeHelper.ToResult(ErrorCode.DeviceNotConnected);

    static ResultCode()
    {
    }
  }
}
