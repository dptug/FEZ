// Type: Microsoft.Xna.Framework.GamerServices.SignedInGamerCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.GamerServices
{
  public class SignedInGamerCollection : List<SignedInGamer>
  {
    public SignedInGamer this[PlayerIndex index]
    {
      get
      {
        if (this.Count == 0 || index > (PlayerIndex) (this.Count - 1))
          return (SignedInGamer) null;
        else
          return base[(int) index];
      }
    }
  }
}
