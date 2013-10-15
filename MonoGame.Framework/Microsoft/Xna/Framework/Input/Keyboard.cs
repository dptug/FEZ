// Type: Microsoft.Xna.Framework.Input.Keyboard
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Input
{
  public static class Keyboard
  {
    private static List<Keys> _keys;

    public static KeyboardState GetState()
    {
      return new KeyboardState(Keyboard._keys);
    }

    public static KeyboardState GetState(PlayerIndex playerIndex)
    {
      return new KeyboardState(Keyboard._keys);
    }

    internal static void SetKeys(List<Keys> keys)
    {
      Keyboard._keys = keys;
    }
  }
}
