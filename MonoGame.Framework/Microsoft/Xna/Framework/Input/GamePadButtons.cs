// Type: Microsoft.Xna.Framework.Input.GamePadButtons
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Input
{
  public struct GamePadButtons
  {
    internal Buttons buttons;

    public ButtonState A
    {
      get
      {
        return (this.buttons & Buttons.A) != Buttons.A ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState B
    {
      get
      {
        return (this.buttons & Buttons.B) != Buttons.B ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState Back
    {
      get
      {
        return (this.buttons & Buttons.Back) != Buttons.Back ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState X
    {
      get
      {
        return (this.buttons & Buttons.X) != Buttons.X ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState Y
    {
      get
      {
        return (this.buttons & Buttons.Y) != Buttons.Y ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState Start
    {
      get
      {
        return (this.buttons & Buttons.Start) != Buttons.Start ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState LeftShoulder
    {
      get
      {
        return (this.buttons & Buttons.LeftShoulder) != Buttons.LeftShoulder ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState LeftStick
    {
      get
      {
        return (this.buttons & Buttons.LeftStick) != Buttons.LeftStick ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState RightShoulder
    {
      get
      {
        return (this.buttons & Buttons.RightShoulder) != Buttons.RightShoulder ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState RightStick
    {
      get
      {
        return (this.buttons & Buttons.RightStick) != Buttons.RightStick ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState BigButton
    {
      get
      {
        return (this.buttons & Buttons.BigButton) != Buttons.BigButton ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public GamePadButtons(Buttons buttons)
    {
      this.buttons = buttons;
    }

    internal GamePadButtons(params Buttons[] buttons)
    {
      this = new GamePadButtons();
      foreach (Buttons buttons1 in buttons)
        this.buttons |= buttons1;
    }
  }
}
