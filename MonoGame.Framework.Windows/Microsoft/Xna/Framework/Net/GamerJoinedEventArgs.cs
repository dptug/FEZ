// Type: Microsoft.Xna.Framework.Net.GamerJoinedEventArgs
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Net
{
  public class GamerJoinedEventArgs : EventArgs
  {
    private NetworkGamer gamer;

    public NetworkGamer Gamer
    {
      get
      {
        return this.gamer;
      }
    }

    public GamerJoinedEventArgs(NetworkGamer aGamer)
    {
      this.gamer = aGamer;
    }
  }
}
