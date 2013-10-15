// Type: OpenTK.Platform.MacOS.MacOSException
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.MacOS
{
  internal class MacOSException : Exception
  {
    private OSStatus errorCode;

    public OSStatus ErrorCode
    {
      get
      {
        return this.errorCode;
      }
    }

    public MacOSException()
    {
    }

    public MacOSException(OSStatus errorCode)
      : base("Error Code " + ((int) errorCode).ToString() + ": " + ((object) errorCode).ToString())
    {
      this.errorCode = errorCode;
    }

    public MacOSException(OSStatus errorCode, string message)
      : base(message)
    {
      this.errorCode = errorCode;
    }

    internal MacOSException(Agl.AglError errorCode, string message)
      : base(message)
    {
      this.errorCode = (OSStatus) errorCode;
    }
  }
}
