// Type: SharpDX.IO.NativeFileAccess
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX.IO
{
  [Flags]
  public enum NativeFileAccess : uint
  {
    Read = 2147483648U,
    Write = 1073741824U,
    ReadWrite = Write | Read,
    Execute = 536870912U,
    All = 268435456U,
  }
}
