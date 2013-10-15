// Type: Microsoft.Xna.Framework.Net.NetworkMachine
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
