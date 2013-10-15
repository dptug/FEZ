// Type: Microsoft.Xna.Framework.Input.Touch.GestureType
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
