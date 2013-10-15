// Type: SharpDX.ComObjectCallback
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public class ComObjectCallback : ComObject, ICallbackable, IDisposable
  {
    public IDisposable Shadow
    {
      get
      {
        throw new InvalidOperationException("Invalid access to Callback. This is used internally.");
      }
      set
      {
        throw new InvalidOperationException("Invalid access to Callback. This is used internally.");
      }
    }

    protected ComObjectCallback(IntPtr pointer)
      : base(pointer)
    {
    }

    protected ComObjectCallback()
    {
    }
  }
}
