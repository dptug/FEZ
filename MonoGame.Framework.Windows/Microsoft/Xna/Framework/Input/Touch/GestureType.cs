// Type: Microsoft.Xna.Framework.Input.Touch.GestureType
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Input.Touch
{
  [Flags]
  public enum GestureType
  {
    None = 0,
    Tap = 1,
    DragComplete = 2,
    Flick = 4,
    FreeDrag = 8,
    Hold = 16,
    HorizontalDrag = 32,
    Pinch = 64,
    PinchComplete = 128,
    DoubleTap = 256,
    VerticalDrag = 512,
  }
}
