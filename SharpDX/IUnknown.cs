// Type: SharpDX.IUnknown
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [Guid("00000000-0000-0000-C000-000000000046")]
  public interface IUnknown
  {
    Result QueryInterface(ref Guid guid, out IntPtr comObject);

    int AddReference();

    int Release();
  }
}
