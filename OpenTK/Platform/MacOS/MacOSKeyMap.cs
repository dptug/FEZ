// Type: OpenTK.Platform.MacOS.MacOSKeyMap
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Input;
using OpenTK.Platform.MacOS.Carbon;
using System.Collections.Generic;

namespace OpenTK.Platform.MacOS
{
  internal class MacOSKeyMap : Dictionary<MacOSKeyCode, Key>
  {
    public MacOSKeyMap()
    {
      this.Add(MacOSKeyCode.A, Key.A);
      this.Add(MacOSKeyCode.B, Key.B);
      this.Add(MacOSKeyCode.Backslash, Key.BackSlash);
      this.Add(MacOSKeyCode.Backspace, Key.BackSpace);
      this.Add(MacOSKeyCode.BracketLeft, Key.BracketLeft);
      this.Add(MacOSKeyCode.BracketRight, Key.BracketRight);
      this.Add(MacOSKeyCode.C, Key.C);
      this.Add(MacOSKeyCode.Comma, Key.Comma);
      this.Add(MacOSKeyCode.D, Key.D);
      this.Add(MacOSKeyCode.Del, Key.Delete);
      this.Add(MacOSKeyCode.Down, Key.Down);
      this.Add(MacOSKeyCode.E, Key.E);
      this.Add(MacOSKeyCode.End, Key.End);
      this.Add(MacOSKeyCode.Enter, Key.Enter);
      this.Add(MacOSKeyCode.Return, Key.Enter);
      this.Add(MacOSKeyCode.Esc, Key.Escape);
      this.Add(MacOSKeyCode.F, Key.F);
      this.Add(MacOSKeyCode.F1, Key.F1);
      this.Add(MacOSKeyCode.F2, Key.F2);
      this.Add(MacOSKeyCode.F3, Key.F3);
      this.Add(MacOSKeyCode.F4, Key.F4);
      this.Add(MacOSKeyCode.F5, Key.F5);
      this.Add(MacOSKeyCode.F6, Key.F6);
      this.Add(MacOSKeyCode.F7, Key.F7);
      this.Add(MacOSKeyCode.F8, Key.F8);
      this.Add(MacOSKeyCode.F9, Key.F9);
      this.Add(MacOSKeyCode.F10, Key.F10);
      this.Add(MacOSKeyCode.F11, Key.F11);
      this.Add(MacOSKeyCode.F12, Key.F12);
      this.Add(MacOSKeyCode.F13, Key.F13);
      this.Add(MacOSKeyCode.F14, Key.F14);
      this.Add(MacOSKeyCode.F15, Key.F15);
      this.Add(MacOSKeyCode.G, Key.G);
      this.Add(MacOSKeyCode.H, Key.H);
      this.Add(MacOSKeyCode.Home, Key.Home);
      this.Add(MacOSKeyCode.I, Key.I);
      this.Add(MacOSKeyCode.Insert, Key.Insert);
      this.Add(MacOSKeyCode.J, Key.J);
      this.Add(MacOSKeyCode.K, Key.K);
      this.Add(MacOSKeyCode.KeyPad_0, Key.Keypad0);
      this.Add(MacOSKeyCode.KeyPad_1, Key.Keypad1);
      this.Add(MacOSKeyCode.KeyPad_2, Key.Keypad2);
      this.Add(MacOSKeyCode.KeyPad_3, Key.Keypad3);
      this.Add(MacOSKeyCode.KeyPad_4, Key.Keypad4);
      this.Add(MacOSKeyCode.KeyPad_5, Key.Keypad5);
      this.Add(MacOSKeyCode.KeyPad_6, Key.Keypad6);
      this.Add(MacOSKeyCode.KeyPad_7, Key.Keypad7);
      this.Add(MacOSKeyCode.KeyPad_8, Key.Keypad8);
      this.Add(MacOSKeyCode.KeyPad_9, Key.Keypad9);
      this.Add(MacOSKeyCode.KeyPad_Add, Key.KeypadAdd);
      this.Add(MacOSKeyCode.KeyPad_Decimal, Key.KeypadDecimal);
      this.Add(MacOSKeyCode.KeyPad_Divide, Key.KeypadDivide);
      this.Add(MacOSKeyCode.KeyPad_Enter, Key.KeypadEnter);
      this.Add(MacOSKeyCode.KeyPad_Multiply, Key.KeypadMultiply);
      this.Add(MacOSKeyCode.KeyPad_Subtract, Key.KeypadSubtract);
      this.Add(MacOSKeyCode.L, Key.L);
      this.Add(MacOSKeyCode.Left, Key.Left);
      this.Add(MacOSKeyCode.M, Key.M);
      this.Add(MacOSKeyCode.Menu, Key.Menu);
      this.Add(MacOSKeyCode.Minus, Key.Minus);
      this.Add(MacOSKeyCode.N, Key.N);
      this.Add(MacOSKeyCode.Key_0, Key.Number0);
      this.Add(MacOSKeyCode.Key_1, Key.Number1);
      this.Add(MacOSKeyCode.Key_2, Key.Number2);
      this.Add(MacOSKeyCode.Key_3, Key.Number3);
      this.Add(MacOSKeyCode.Key_4, Key.Number4);
      this.Add(MacOSKeyCode.Key_5, Key.Number4);
      this.Add(MacOSKeyCode.Key_6, Key.Number5);
      this.Add(MacOSKeyCode.Key_7, Key.Number6);
      this.Add(MacOSKeyCode.Key_8, Key.Number7);
      this.Add(MacOSKeyCode.Key_9, Key.Number9);
      this.Add(MacOSKeyCode.O, Key.O);
      this.Add(MacOSKeyCode.P, Key.P);
      this.Add(MacOSKeyCode.Pagedown, Key.PageDown);
      this.Add(MacOSKeyCode.Pageup, Key.PageUp);
      this.Add(MacOSKeyCode.Period, Key.Period);
      this.Add(MacOSKeyCode.Equals, Key.Plus);
      this.Add(MacOSKeyCode.Q, Key.Q);
      this.Add(MacOSKeyCode.Quote, Key.Quote);
      this.Add(MacOSKeyCode.R, Key.R);
      this.Add(MacOSKeyCode.Right, Key.Right);
      this.Add(MacOSKeyCode.S, Key.S);
      this.Add(MacOSKeyCode.Semicolon, Key.Semicolon);
      this.Add(MacOSKeyCode.Slash, Key.Slash);
      this.Add(MacOSKeyCode.Space, Key.Space);
      this.Add(MacOSKeyCode.T, Key.T);
      this.Add(MacOSKeyCode.Tab, Key.Tab);
      this.Add(MacOSKeyCode.Tilde, Key.Tilde);
      this.Add(MacOSKeyCode.U, Key.U);
      this.Add(MacOSKeyCode.Up, Key.Up);
      this.Add(MacOSKeyCode.V, Key.V);
      this.Add(MacOSKeyCode.W, Key.W);
      this.Add(MacOSKeyCode.X, Key.X);
      this.Add(MacOSKeyCode.Y, Key.Y);
      this.Add(MacOSKeyCode.Z, Key.Z);
    }
  }
}
