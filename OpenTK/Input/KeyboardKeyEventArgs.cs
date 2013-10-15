// Type: OpenTK.Input.KeyboardKeyEventArgs
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Input
{
  public class KeyboardKeyEventArgs : EventArgs
  {
    private Key key;

    public Key Key
    {
      get
      {
        return this.key;
      }
      internal set
      {
        this.key = value;
      }
    }

    public KeyboardKeyEventArgs()
    {
    }

    public KeyboardKeyEventArgs(KeyboardKeyEventArgs args)
    {
      this.Key = args.Key;
    }
  }
}
