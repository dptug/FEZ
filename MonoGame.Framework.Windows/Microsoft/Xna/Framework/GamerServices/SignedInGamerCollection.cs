// Type: Microsoft.Xna.Framework.GamerServices.SignedInGamerCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
