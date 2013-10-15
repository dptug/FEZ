// Type: OpenTK.Platform.X11.XLock
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct XLock : IDisposable
  {
    private IntPtr _display;

    public IntPtr Display
    {
      get
      {
        if (this._display == IntPtr.Zero)
          throw new InvalidOperationException("Internal error (XLockDisplay with IntPtr.Zero). Please report this at http://www.opentk.com/node/add/project-issue/opentk");
        else
          return this._display;
      }
      set
      {
        if (value == IntPtr.Zero)
          throw new ArgumentException();
        this._display = value;
      }
    }

    public XLock(IntPtr display)
    {
      this = new XLock();
      this.Display = display;
      Functions.XLockDisplay(this.Display);
    }

    public void Dispose()
    {
      Functions.XUnlockDisplay(this.Display);
    }
  }
}
