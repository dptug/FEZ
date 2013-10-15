// Type: OpenTK.KeyPressEventArgs
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  public class KeyPressEventArgs : EventArgs
  {
    private char key_char;

    public char KeyChar
    {
      get
      {
        return this.key_char;
      }
      internal set
      {
        this.key_char = value;
      }
    }

    public KeyPressEventArgs(char keyChar)
    {
      this.KeyChar = keyChar;
    }
  }
}
