// Type: OpenTK.Platform.Windows.WinKeyMap
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using System.Collections.Generic;

namespace OpenTK.Platform.Windows
{
  internal class WinKeyMap : Dictionary<VirtualKeys, Key>
  {
    public WinKeyMap()
    {
      this.Add(VirtualKeys.ESCAPE, Key.Escape);
      for (int index = 0; index < 24; ++index)
        this.Add((VirtualKeys) (112 + index), (Key) (10 + index));
      for (int index = 0; index <= 9; ++index)
        this.Add((VirtualKeys) (48 + index), (Key) (109 + index));
      for (int index = 0; index < 26; ++index)
        this.Add((VirtualKeys) (65 + index), (Key) (83 + index));
      this.Add(VirtualKeys.TAB, Key.Tab);
      this.Add(VirtualKeys.CAPITAL, Key.CapsLock);
      this.Add(VirtualKeys.LCONTROL, Key.ControlLeft);
      this.Add(VirtualKeys.LSHIFT, Key.ShiftLeft);
      this.Add(VirtualKeys.LWIN, Key.WinLeft);
      this.Add(VirtualKeys.LMENU, Key.AltLeft);
      this.Add(VirtualKeys.SPACE, Key.Space);
      this.Add(VirtualKeys.RMENU, Key.AltRight);
      this.Add(VirtualKeys.RWIN, Key.WinRight);
      this.Add(VirtualKeys.APPS, Key.Menu);
      this.Add(VirtualKeys.RCONTROL, Key.ControlRight);
      this.Add(VirtualKeys.RSHIFT, Key.ShiftRight);
      this.Add(VirtualKeys.RETURN, Key.Enter);
      this.Add(VirtualKeys.BACK, Key.BackSpace);
      this.Add(VirtualKeys.OEM_1, Key.Semicolon);
      this.Add(VirtualKeys.OEM_2, Key.Slash);
      this.Add(VirtualKeys.OEM_3, Key.Tilde);
      this.Add(VirtualKeys.OEM_4, Key.BracketLeft);
      this.Add(VirtualKeys.OEM_5, Key.BackSlash);
      this.Add(VirtualKeys.OEM_6, Key.BracketRight);
      this.Add(VirtualKeys.OEM_7, Key.Quote);
      this.Add(VirtualKeys.OEM_PLUS, Key.Plus);
      this.Add(VirtualKeys.OEM_COMMA, Key.Comma);
      this.Add(VirtualKeys.OEM_MINUS, Key.Minus);
      this.Add(VirtualKeys.OEM_PERIOD, Key.Period);
      this.Add(VirtualKeys.HOME, Key.Home);
      this.Add(VirtualKeys.END, Key.End);
      this.Add(VirtualKeys.DELETE, Key.Delete);
      this.Add(VirtualKeys.PRIOR, Key.PageUp);
      this.Add(VirtualKeys.NEXT, Key.PageDown);
      this.Add(VirtualKeys.PRINT, Key.PrintScreen);
      this.Add(VirtualKeys.PAUSE, Key.Pause);
      this.Add(VirtualKeys.NUMLOCK, Key.NumLock);
      this.Add(VirtualKeys.SCROLL, Key.ScrollLock);
      this.Add(VirtualKeys.SNAPSHOT, Key.PrintScreen);
      this.Add(VirtualKeys.CLEAR, Key.Clear);
      this.Add(VirtualKeys.INSERT, Key.Insert);
      this.Add(VirtualKeys.SLEEP, Key.Sleep);
      for (int index = 0; index <= 9; ++index)
        this.Add((VirtualKeys) (96 + index), (Key) (67 + index));
      this.Add(VirtualKeys.DECIMAL, Key.KeypadDecimal);
      this.Add(VirtualKeys.ADD, Key.KeypadAdd);
      this.Add(VirtualKeys.SUBTRACT, Key.KeypadSubtract);
      this.Add(VirtualKeys.DIVIDE, Key.KeypadDivide);
      this.Add(VirtualKeys.MULTIPLY, Key.KeypadMultiply);
      this.Add(VirtualKeys.UP, Key.Up);
      this.Add(VirtualKeys.DOWN, Key.Down);
      this.Add(VirtualKeys.LEFT, Key.Left);
      this.Add(VirtualKeys.RIGHT, Key.Right);
    }
  }
}
