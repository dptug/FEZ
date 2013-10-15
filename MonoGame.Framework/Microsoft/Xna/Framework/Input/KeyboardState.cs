// Type: Microsoft.Xna.Framework.Input.KeyboardState
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Input
{
  public struct KeyboardState
  {
    private static Keys[] empty = new Keys[0];
    private uint keys0;
    private uint keys1;
    private uint keys2;
    private uint keys3;
    private uint keys4;
    private uint keys5;
    private uint keys6;
    private uint keys7;

    public KeyState this[Keys key]
    {
      get
      {
        return !this.InternalGetKey(key) ? KeyState.Up : KeyState.Down;
      }
    }

    static KeyboardState()
    {
    }

    internal KeyboardState(List<Keys> keys)
    {
      this.keys0 = 0U;
      this.keys1 = 0U;
      this.keys2 = 0U;
      this.keys3 = 0U;
      this.keys4 = 0U;
      this.keys5 = 0U;
      this.keys6 = 0U;
      this.keys7 = 0U;
      if (keys == null)
        return;
      foreach (Keys key in keys)
        this.InternalSetKey(key);
    }

    public KeyboardState(params Keys[] keys)
    {
      this.keys0 = 0U;
      this.keys1 = 0U;
      this.keys2 = 0U;
      this.keys3 = 0U;
      this.keys4 = 0U;
      this.keys5 = 0U;
      this.keys6 = 0U;
      this.keys7 = 0U;
      if (keys == null)
        return;
      foreach (Keys key in keys)
        this.InternalSetKey(key);
    }

    public static bool operator ==(KeyboardState a, KeyboardState b)
    {
      if ((int) a.keys0 == (int) b.keys0 && (int) a.keys1 == (int) b.keys1 && ((int) a.keys2 == (int) b.keys2 && (int) a.keys3 == (int) b.keys3) && ((int) a.keys4 == (int) b.keys4 && (int) a.keys5 == (int) b.keys5 && (int) a.keys6 == (int) b.keys6))
        return (int) a.keys7 == (int) b.keys7;
      else
        return false;
    }

    public static bool operator !=(KeyboardState a, KeyboardState b)
    {
      return !(a == b);
    }

    private bool InternalGetKey(Keys key)
    {
      uint num1 = 1U << (int) (key & (Keys) 31 & (Keys) 31);
      uint num2;
      switch ((int) key >> 5)
      {
        case 0:
          num2 = this.keys0;
          break;
        case 1:
          num2 = this.keys1;
          break;
        case 2:
          num2 = this.keys2;
          break;
        case 3:
          num2 = this.keys3;
          break;
        case 4:
          num2 = this.keys4;
          break;
        case 5:
          num2 = this.keys5;
          break;
        case 6:
          num2 = this.keys6;
          break;
        case 7:
          num2 = this.keys7;
          break;
        default:
          num2 = 0U;
          break;
      }
      return ((int) num2 & (int) num1) != 0;
    }

    private void InternalSetKey(Keys key)
    {
      uint num = 1U << (int) (key & (Keys) 31 & (Keys) 31);
      switch ((int) key >> 5)
      {
        case 0:
          this.keys0 |= num;
          break;
        case 1:
          this.keys1 |= num;
          break;
        case 2:
          this.keys2 |= num;
          break;
        case 3:
          this.keys3 |= num;
          break;
        case 4:
          this.keys4 |= num;
          break;
        case 5:
          this.keys5 |= num;
          break;
        case 6:
          this.keys6 |= num;
          break;
        case 7:
          this.keys7 |= num;
          break;
      }
    }

    private void InternalClearKey(Keys key)
    {
      uint num = 1U << (int) (key & (Keys) 31 & (Keys) 31);
      switch ((int) key >> 5)
      {
        case 0:
          this.keys0 &= ~num;
          break;
        case 1:
          this.keys1 &= ~num;
          break;
        case 2:
          this.keys2 &= ~num;
          break;
        case 3:
          this.keys3 &= ~num;
          break;
        case 4:
          this.keys4 &= ~num;
          break;
        case 5:
          this.keys5 &= ~num;
          break;
        case 6:
          this.keys6 &= ~num;
          break;
        case 7:
          this.keys7 &= ~num;
          break;
      }
    }

    private void InternalClearAllKeys()
    {
      this.keys0 = 0U;
      this.keys1 = 0U;
      this.keys2 = 0U;
      this.keys3 = 0U;
      this.keys4 = 0U;
      this.keys5 = 0U;
      this.keys6 = 0U;
      this.keys7 = 0U;
    }

    public bool IsKeyDown(Keys key)
    {
      return this.InternalGetKey(key);
    }

    public bool IsKeyUp(Keys key)
    {
      return !this.InternalGetKey(key);
    }

    private static uint CountBits(uint v)
    {
      v -= v >> 1 & 1431655765U;
      v = (uint) (((int) v & 858993459) + ((int) (v >> 2) & 858993459));
      return (uint) (((int) v + (int) (v >> 4) & 252645135) * 16843009) >> 24;
    }

    private static int AddKeysToArray(uint keys, int offset, Keys[] pressedKeys, int index)
    {
      for (int index1 = 0; index1 < 32; ++index1)
      {
        if (((long) keys & (long) (1 << index1)) != 0L)
          pressedKeys[index++] = (Keys) (offset + index1);
      }
      return index;
    }

    public Keys[] GetPressedKeys()
    {
      uint num = KeyboardState.CountBits(this.keys0) + KeyboardState.CountBits(this.keys1) + KeyboardState.CountBits(this.keys2) + KeyboardState.CountBits(this.keys3) + KeyboardState.CountBits(this.keys4) + KeyboardState.CountBits(this.keys5) + KeyboardState.CountBits(this.keys6) + KeyboardState.CountBits(this.keys7);
      if ((int) num == 0)
        return KeyboardState.empty;
      Keys[] pressedKeys = new Keys[(IntPtr) num];
      int index = 0;
      if ((int) this.keys0 != 0)
        index = KeyboardState.AddKeysToArray(this.keys0, 0, pressedKeys, index);
      if ((int) this.keys1 != 0)
        index = KeyboardState.AddKeysToArray(this.keys1, 32, pressedKeys, index);
      if ((int) this.keys2 != 0)
        index = KeyboardState.AddKeysToArray(this.keys2, 64, pressedKeys, index);
      if ((int) this.keys3 != 0)
        index = KeyboardState.AddKeysToArray(this.keys3, 96, pressedKeys, index);
      if ((int) this.keys4 != 0)
        index = KeyboardState.AddKeysToArray(this.keys4, 128, pressedKeys, index);
      if ((int) this.keys5 != 0)
        index = KeyboardState.AddKeysToArray(this.keys5, 160, pressedKeys, index);
      if ((int) this.keys6 != 0)
        index = KeyboardState.AddKeysToArray(this.keys6, 192, pressedKeys, index);
      if ((int) this.keys7 != 0)
        KeyboardState.AddKeysToArray(this.keys7, 224, pressedKeys, index);
      return pressedKeys;
    }

    public override int GetHashCode()
    {
      return (int) this.keys0 ^ (int) this.keys1 ^ (int) this.keys2 ^ (int) this.keys3 ^ (int) this.keys4 ^ (int) this.keys5 ^ (int) this.keys6 ^ (int) this.keys7;
    }

    public override bool Equals(object obj)
    {
      if (obj is KeyboardState)
        return this == (KeyboardState) obj;
      else
        return false;
    }
  }
}
