// Type: Microsoft.Xna.Framework.ActiveEventArgs
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework
{
  public class ActiveEventArgs : EventArgs
  {
    public bool ForBounds { get; private set; }

    public ActiveEventArgs(bool forBounds)
    {
      this.ForBounds = forBounds;
    }
  }
}
