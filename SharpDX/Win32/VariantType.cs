// Type: SharpDX.Win32.VariantType
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX.Win32
{
  [Flags]
  public enum VariantType : ushort
  {
    Default = (ushort) 0,
    Vector = (ushort) 4096,
    Array = (ushort) 8192,
    ByRef = (ushort) 16384,
    Reserved = (ushort) 32768,
  }
}
