// Type: Microsoft.Xna.Framework.Input.GamePadButtons
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Input
{
  public struct GamePadButtons
  {
    internal Buttons buttons;

    public ButtonState A
    {
      get
      {
        return (this.buttons & Buttons.A) == Buttons.A ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState B
    {
      get
      {
        return (this.buttons & Buttons.B) == Buttons.B ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState Back
    {
      get
      {
        return (this.buttons & Buttons.Back) == Buttons.Back ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState X
    {
      get
      {
        return (this.buttons & Buttons.X) == Buttons.X ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState Y
    {
      get
      {
        return (this.buttons & Buttons.Y) == Buttons.Y ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState Start
    {
      get
      {
        return (this.buttons & Buttons.Start) == Buttons.Start ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState LeftShoulder
    {
      get
      {
        return (this.buttons & Buttons.LeftShoulder) == Buttons.LeftShoulder ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState LeftStick
    {
      get
      {
        return (this.buttons & Buttons.LeftStick) == Buttons.LeftStick ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState RightShoulder
    {
      get
      {
        return (this.buttons & Buttons.RightShoulder) == Buttons.RightShoulder ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState RightStick
    {
      get
      {
        return (this.buttons & Buttons.RightStick) == Buttons.RightStick ? ButtonState.Pressed : ButtonState.Released;
      }
    }

    public ButtonState BigButton
    {
      get
      {
        return (this.buttons & Buttons.BigButton) == Buttons.BigButton ? ButtonState.Pressed : ButtonState.Released;
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
