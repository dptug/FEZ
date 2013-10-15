// Type: SharpDX.Win32.ErrorCodeHelper
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;

namespace SharpDX.Win32
{
  public class ErrorCodeHelper
  {
    public static Result ToResult(ErrorCode errorCode)
    {
      return ErrorCodeHelper.ToResult((int) errorCode);
    }

    public static Result ToResult(int errorCode)
    {
      return new Result(errorCode <= 0 ? (uint) errorCode : (uint) (errorCode & (int) ushort.MaxValue | -2147024896));
    }
  }
}
