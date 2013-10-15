// Type: FezEngine.Structure.Input.CodeInput
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Structure.Input
{
  [Flags]
  public enum CodeInput
  {
    None = 0,
    Up = 1,
    Down = 2,
    Left = 4,
    Right = 8,
    SpinLeft = 16,
    SpinRight = 32,
    Jump = 64,
  }
}
