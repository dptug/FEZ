// Type: Microsoft.Xna.Framework.Input.GamePadDPad
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Input
{
  public struct GamePadDPad
  {
    public ButtonState Down { get; internal set; }

    public ButtonState Left { get; internal set; }

    public ButtonState Right { get; internal set; }

    public ButtonState Up { get; internal set; }

    public GamePadDPad(ButtonState upValue, ButtonState downValue, ButtonState leftValue, ButtonState rightValue)
    {
      this = new GamePadDPad();
      this.Up = upValue;
      this.Down = downValue;
      this.Left = leftValue;
      this.Right = rightValue;
    }

    internal GamePadDPad(Buttons b)
    {
      this = new GamePadDPad();
      if ((b & Buttons.DPadDown) == Buttons.DPadDown)
        this.Down = ButtonState.Pressed;
      if ((b & Buttons.DPadLeft) == Buttons.DPadLeft)
        this.Left = ButtonState.Pressed;
      if ((b & Buttons.DPadRight) == Buttons.DPadRight)
        this.Right = ButtonState.Pressed;
      if ((b & Buttons.DPadUp) != Buttons.DPadUp)
        return;
      this.Up = ButtonState.Pressed;
    }
  }
}
