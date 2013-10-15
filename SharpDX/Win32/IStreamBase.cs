// Type: SharpDX.Win32.IStreamBase
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
  [Guid("0c733a30-2a1c-11ce-ade5-00aa0044773d")]
  [Shadow(typeof (ComStreamBaseShadow))]
  public interface IStreamBase : ICallbackable, IDisposable
  {
    int Read(IntPtr buffer, int numberOfBytesToRead);

    int Write(IntPtr buffer, int numberOfBytesToRead);
  }
}
