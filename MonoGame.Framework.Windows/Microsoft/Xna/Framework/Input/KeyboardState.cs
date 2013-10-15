// Type: Microsoft.Xna.Framework.Input.KeyboardState
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Xna.Framework.Input
{
  public struct KeyboardState
  {
    private Keys[] _keys;

    public KeyState this[Keys key]
    {
      get
      {
        return this.IsKeyDown(key) ? KeyState.Down : KeyState.Up;
      }
    }

    public KeyboardState(Keys[] keys)
    {
      this._keys = keys;
    }

    public static bool operator ==(KeyboardState first, KeyboardState second)
    {
      return first.GetHashCode() == second.GetHashCode();
    }

    public static bool operator !=(KeyboardState first, KeyboardState second)
    {
      return first.GetHashCode() != second.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return this.GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
      if (this._keys != null)
        return this._keys.GetHashCode();
      else
        return -1;
    }

    public Keys[] GetPressedKeys()
    {
      if (this._keys == null)
        this._keys = new Keys[0];
      return this._keys;
    }

    public bool IsKeyDown(Keys key)
    {
      if (this._keys != null)
        return Enumerable.Contains<Keys>((IEnumerable<Keys>) this._keys, key);
      else
        return false;
    }

    public bool IsKeyUp(Keys key)
    {
      if (this._keys != null)
        return !Enumerable.Contains<Keys>((IEnumerable<Keys>) this._keys, key);
      else
        return true;
    }
  }
}
