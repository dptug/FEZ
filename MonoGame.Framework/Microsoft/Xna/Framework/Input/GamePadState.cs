// Type: Microsoft.Xna.Framework.Input.GamePadState
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Input
{
  public struct GamePadState
  {
    private static GamePadState initializedGamePadState = new GamePadState();

    public bool IsConnected { get; internal set; }

    public int PacketNumber { get; internal set; }

    public GamePadButtons Buttons { get; internal set; }

    public GamePadDPad DPad { get; internal set; }

    public GamePadThumbSticks ThumbSticks { get; internal set; }

    public GamePadTriggers Triggers { get; internal set; }

    internal static GamePadState InitializedState
    {
      get
      {
        return GamePadState.initializedGamePadState;
      }
    }

    static GamePadState()
    {
    }

    public GamePadState(GamePadThumbSticks thumbSticks, GamePadTriggers triggers, GamePadButtons buttons, GamePadDPad dPad)
    {
      this = new GamePadState();
      this.ThumbSticks = thumbSticks;
      this.Triggers = triggers;
      this.Buttons = buttons;
      this.DPad = dPad;
      this.IsConnected = true;
    }

    public GamePadState(Vector2 leftThumbStick, Vector2 rightThumbStick, float leftTrigger, float rightTrigger, params Buttons[] buttons)
    {
      this = new GamePadState(new GamePadThumbSticks(leftThumbStick, rightThumbStick), new GamePadTriggers(leftTrigger, rightTrigger), new GamePadButtons(buttons), new GamePadDPad());
    }

    public static bool operator !=(GamePadState left, GamePadState right)
    {
      return !left.Equals((object) right);
    }

    public static bool operator ==(GamePadState left, GamePadState right)
    {
      return left.Equals((object) right);
    }

    public bool IsButtonDown(Buttons button)
    {
      return (this.Buttons.buttons & button) == button;
    }

    public bool IsButtonUp(Buttons button)
    {
      return (this.Buttons.buttons & button) != button;
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override string ToString()
    {
      return base.ToString();
    }
  }
}
