// Type: SharpDX.XInput.GamepadButtonFlags
// Assembly: SharpDX.XInput, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 7810FD3A-F5EE-4EAB-B451-0D4E18D9FE4F
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.XInput.dll

using System;

namespace SharpDX.XInput
{
  [Flags]
  public enum GamepadButtonFlags : short
  {
    DPadUp = (short) 1,
    DPadDown = (short) 2,
    DPadLeft = (short) 4,
    DPadRight = (short) 8,
    Start = (short) 16,
    Back = (short) 32,
    LeftThumb = (short) 64,
    RightThumb = (short) 128,
    LeftShoulder = (short) 256,
    RightShoulder = (short) 512,
    A = (short) 4096,
    B = (short) 8192,
    X = (short) 16384,
    Y = (short) -32768,
    None = (short) 0,
  }
}
