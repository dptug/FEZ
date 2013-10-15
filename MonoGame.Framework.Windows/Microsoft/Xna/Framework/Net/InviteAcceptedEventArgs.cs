// Type: Microsoft.Xna.Framework.Net.InviteAcceptedEventArgs
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
