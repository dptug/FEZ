// Type: MonoGame.Framework.GamerServices.SignInCompletedEventArgs
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.GamerServices;
using System;

namespace MonoGame.Framework.GamerServices
{
  public class SignInCompletedEventArgs : EventArgs
  {
    private Gamer gamer;

    public Gamer Gamer
    {
      get
      {
        return this.gamer;
      }
    }

    internal SignInCompletedEventArgs(Gamer gamer)
    {
      this.gamer = gamer;
    }
  }
}
