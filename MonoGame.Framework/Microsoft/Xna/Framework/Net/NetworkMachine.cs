// Type: Microsoft.Xna.Framework.Net.NetworkMachine
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.GamerServices;
using System;

namespace Microsoft.Xna.Framework.Net
{
  public sealed class NetworkMachine
  {
    private GamerCollection<NetworkGamer> gamers;

    public GamerCollection<NetworkGamer> Gamers
    {
      get
      {
        return this.gamers;
      }
    }

    public NetworkMachine()
    {
      this.gamers = new GamerCollection<NetworkGamer>();
    }

    public void RemoveFromSession()
    {
      throw new NotImplementedException();
    }
  }
}
