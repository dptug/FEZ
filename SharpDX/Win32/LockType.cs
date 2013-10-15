// Type: SharpDX.Win32.LockType
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX.Win32
{
  [Flags]
  public enum LockType
  {
    Write = 1,
    Exclusive = 2,
    OnlyOnce = 4,
  }
}
