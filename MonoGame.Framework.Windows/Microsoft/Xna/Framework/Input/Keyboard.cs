// Type: Microsoft.Xna.Framework.Input.Keyboard
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Input
{
  public static class Keyboard
  {
    internal static KeyboardState State { private get; set; }

    public static KeyboardState GetState()
    {
      return Keyboard.State;
    }

    public static KeyboardState GetState(PlayerIndex playerIndex)
    {
      return Keyboard.State;
    }
  }
}
