// Type: SharpDX.CallbackBase
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public abstract class CallbackBase : DisposeBase, ICallbackable, IDisposable
  {
    IDisposable ICallbackable.Shadow { get; set; }

    protected override void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      ICallbackable callbackable = (ICallbackable) this;
      if (callbackable.Shadow == null)
        return;
      callbackable.Shadow.Dispose();
      callbackable.Shadow = (IDisposable) null;
    }
  }
}
