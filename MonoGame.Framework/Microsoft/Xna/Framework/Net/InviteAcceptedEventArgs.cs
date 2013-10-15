// Type: Microsoft.Xna.Framework.Net.InviteAcceptedEventArgs
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.GamerServices;
using System;

namespace Microsoft.Xna.Framework.Net
{
  public class InviteAcceptedEventArgs : EventArgs
  {
    private SignedInGamer gamer;

    public SignedInGamer Gamer
    {
      get
      {
        return this.gamer;
      }
    }

    public bool IsCurrentSession
    {
      get
      {
        return false;
      }
    }

    public InviteAcceptedEventArgs(SignedInGamer aGamer)
    {
      this.gamer = aGamer;
    }
  }
}
