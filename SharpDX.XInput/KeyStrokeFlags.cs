// Type: SharpDX.XInput.KeyStrokeFlags
// Assembly: SharpDX.XInput, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 7810FD3A-F5EE-4EAB-B451-0D4E18D9FE4F
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.XInput.dll

using System;

namespace SharpDX.XInput
{
  [Flags]
  public enum KeyStrokeFlags : short
  {
    KeyDown = (short) 1,
    KeyUp = (short) 2,
    Repeat = (short) 4,
    None = (short) 0,
  }
}
