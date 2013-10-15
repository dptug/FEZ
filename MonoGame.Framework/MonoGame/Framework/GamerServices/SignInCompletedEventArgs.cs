// Type: MonoGame.Framework.GamerServices.SignInCompletedEventArgs
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
